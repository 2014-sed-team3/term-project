

using System;
using System.Configuration;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterGetListNetworkDialog
//
/// <summary>
/// Gets a network that shows the connections between a set of Twitter users
/// specified by either a single Twitter List name or by a set of Twitter
/// screen names.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class TwitterGetListNetworkDialog
    : TwitterGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: TwitterGetListNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterGetListNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterGetListNetworkDialog()
    : base ( new TwitterListNetworkAnalyzer() )
    {
        InitializeComponent();
        InitializeTwitterAuthorizationManager(usrTwitterAuthorization);

        // A Twitter screen name can have 15 characters.  Allow for a LF-CR
        // between screen names in the TextBox.

        txbScreenNames.MaxLength = MaximumScreenNames * (15 + 2);

        // m_bUseListName
        // m_sListName
        // m_sScreenNames
        // m_bIncludeFollowedEdges
        // m_bIncludeRepliesToEdges
        // m_bIncludeMentionsEdges
        // m_bIncludeLatestStatuses
        // m_bExpandLatestStatusUrls
        // m_bIncludeStatistics

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: ToolStripStatusLabel
    //
    /// <summary>
    /// Gets the dialog's ToolStripStatusLabel control.
    /// </summary>
    ///
    /// <value>
    /// The dialog's ToolStripStatusLabel control, or null if the dialog
    /// doesn't have one.  The default is null.
    /// </value>
    ///
    /// <remarks>
    /// If the derived dialog overrides this property and returns a non-null
    /// ToolStripStatusLabel control, the control's text will automatically get
    /// updated when the HttpNetworkAnalyzer fires a ProgressChanged event.
    /// </remarks>
    //*************************************************************************

    protected override ToolStripStatusLabel
    ToolStripStatusLabel
    {
        get
        {
            AssertValid();

            return (this.slStatusLabel);
        }
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected override Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            m_bUseListName = radUseListName.Checked;

            // Validate the controls.

            if (m_bUseListName)
            {
                const String ListNameMessage =
                    "Enter a Twitter List name.  A List name looks like"
                    + " \"bob/bobs\", where \"bob\" is a Twitter username"
                    + " and \"bobs\" is a List that bob created."
                    ;

                if ( !ValidateRequiredTextBox(txbListName, ListNameMessage,
                    out m_sListName) )
                {
                    return (false);
                }

                String sSlug, sOwnerScreenName;

                if ( !TwitterListNetworkAnalyzer.TryParseListName(m_sListName,
                    out sSlug, out sOwnerScreenName) )
                {
                    return ( OnInvalidTextBox(txbListName, ListNameMessage) );
                }
            }
            else
            {
                const String ScreenNamesMessage =
                    "Enter a set of Twitter usernames separated with spaces,"
                    + " commas or returns."
                    ;

                if ( !ValidateRequiredTextBox(txbScreenNames,
                    ScreenNamesMessage, out m_sScreenNames) )
                {
                    return (false);
                }

                String [] asScreenNames =
                    StringUtil.SplitOnCommonDelimiters(m_sScreenNames);

                if (asScreenNames.Length > MaximumScreenNames)
                {
                    return (OnInvalidTextBox(txbScreenNames, String.Format(
                        "The maximum number of usernames is {0}."
                        ,
                        MaximumScreenNames
                        ) ) );
                }

                foreach (String sScreenName in asScreenNames)
                {
                    if (sScreenName.Length > 15)
                    {
                        return ( OnInvalidTextBox(txbScreenNames,
                            String.Format(
                                "The username \"{0}\" is too long.  Twitter"
                                + " usernames can't be longer than 15"
                                + " characters."
                                ,
                                sScreenName
                                ) ) );
                    }
                }
            }

            m_bIncludeFollowedEdges = chkIncludeFollowedEdges.Checked;
            m_bIncludeRepliesToEdges = chkIncludeRepliesToEdges.Checked;
            m_bIncludeMentionsEdges = chkIncludeMentionsEdges.Checked;
            m_bIncludeLatestStatuses = chkIncludeLatestStatuses.Checked;
            m_bExpandLatestStatusUrls = chkExpandLatestStatusUrls.Checked;
            m_bIncludeStatistics = chkIncludeStatistics.Checked;
        }
        else
        {
            radUseListName.Checked = m_bUseListName;
            radUseScreenNames.Checked = !m_bUseListName;

            txbListName.Text = m_sListName;
            txbScreenNames.Text = m_sScreenNames;

            chkIncludeFollowedEdges.Checked = m_bIncludeFollowedEdges;
            chkIncludeRepliesToEdges.Checked = m_bIncludeRepliesToEdges;
            chkIncludeMentionsEdges.Checked = m_bIncludeMentionsEdges;
            chkIncludeLatestStatuses.Checked = m_bIncludeLatestStatuses;
            chkExpandLatestStatusUrls.Checked = m_bExpandLatestStatusUrls;
            chkIncludeStatistics.Checked = m_bIncludeStatistics;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the Twitter analysis.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that DoDataExchange(true) was called and succeeded.
    /// </remarks>
    //*************************************************************************

    protected override void
    StartAnalysis()
    {
        AssertValid();

        m_oGraphMLXmlDocument = null;

        Debug.Assert(m_oHttpNetworkAnalyzer is TwitterListNetworkAnalyzer);

        TwitterListNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bIncludeLatestStatuses ?
                TwitterListNetworkAnalyzer.WhatToInclude.LatestStatuses : 0)
            |
            (m_bExpandLatestStatusUrls ?
                TwitterListNetworkAnalyzer.WhatToInclude
                    .ExpandedLatestStatusUrls : 0)
            |
            (m_bIncludeStatistics ?
                TwitterListNetworkAnalyzer.WhatToInclude.Statistics : 0)
            |
            (m_bIncludeFollowedEdges ?
                TwitterListNetworkAnalyzer.WhatToInclude.FollowedEdges : 0)
            |
            (m_bIncludeRepliesToEdges ?
                TwitterListNetworkAnalyzer.WhatToInclude.RepliesToEdges : 0)
            |
            (m_bIncludeMentionsEdges ?
                TwitterListNetworkAnalyzer.WhatToInclude.MentionsEdges : 0)
            ;

        ( (TwitterListNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_bUseListName, m_sListName,
                StringUtil.SplitOnCommonDelimiters(m_sScreenNames),
                eWhatToInclude);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected override void
    EnableControls()
    {
        AssertValid();

        Boolean bUseListName = radUseListName.Checked;
        Boolean bIsBusy = m_oHttpNetworkAnalyzer.IsBusy;

        txbListName.Enabled = bUseListName;
        EnableControls(!bUseListName, txbScreenNames, lblScreenNamesHelp);
        EnableControls(!bIsBusy, pnlUserInputs, btnOK);
        chkExpandLatestStatusUrls.Enabled = chkIncludeLatestStatuses.Checked;

        this.UseWaitCursor = bIsBusy;
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
    /// Handles any event that should changed the enabled state of the dialog's
    /// controls.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    OnEventThatRequiresControlEnabling
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
    }

    //*************************************************************************
    //  Method: OnEmptyGraph()
    //
    /// <summary>
    /// Handles the case where a graph was successfully obtained by is empty.
    /// </summary>
    //*************************************************************************

    protected override void
    OnEmptyGraph()
    {
        AssertValid();

        String sMessage;
        TextBox oTextBoxToFocus;

        if (m_bUseListName)
        {
            sMessage = "There is no such Twitter List.";
            oTextBoxToFocus = txbListName;
        }
        else
        {
            sMessage =
                "Either there are no such users, or they have all protected"
                + " their Twitter accounts.";

            oTextBoxToFocus = txbScreenNames;
        }

        this.ShowInformation(sMessage);
        oTextBoxToFocus.Focus();
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    btnOK_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnOKClick();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // m_bUseListName
        // m_sListName
        // m_sScreenNames
        // m_bIncludeFollowedEdges
        // m_bIncludeRepliesToEdges
        // m_bIncludeMentionsEdges
        // m_bIncludeLatestStatuses
        // m_bExpandLatestStatusUrls
        // m_bIncludeStatistics
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum number of screen names to allow.

    protected const Int32 MaximumScreenNames = 10000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// true if the users were specified as a Twitter List name, false if they
    /// were specified as a set of screen names.

    protected static Boolean m_bUseListName = true;

    /// Twitter List name if m_bUseListName is true; unused otherwise.

    protected static String m_sListName = "bob/bobs";

    /// One or more Twitter screen names delimited with spaces, commas or
    /// newlines if m_bUseListName is false; unused otherwise.

    protected static String m_sScreenNames = "bob nora dan";

    /// true to include an edge for each followed relationship.

    protected static Boolean m_bIncludeFollowedEdges = false;

    /// true to include an edge from person A to person B if person A's tweet
    /// is a reply to person B.

    protected static Boolean m_bIncludeRepliesToEdges = true;

    /// true to include an edge from person A to person B if person A's tweet
    /// is mentions person B.

    protected static Boolean m_bIncludeMentionsEdges = true;

    /// true to include each latest status.

    protected static Boolean m_bIncludeLatestStatuses = false;

    /// true to expand the URLs in the latest statuses.

    protected static Boolean m_bExpandLatestStatusUrls = false;

    /// true to include statistics for each user.

    protected static Boolean m_bIncludeStatistics = false;
}

}
