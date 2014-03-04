

using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterGetSearchNetworkDialog
//
/// <summary>
/// Gets the the network of people who have tweeted a specified search term.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class TwitterGetSearchNetworkDialog :
    TwitterGraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: TwitterGetSearchNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterGetSearchNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterGetSearchNetworkDialog()
    :
    base( new TwitterSearchNetworkAnalyzer() )
    {
        InitializeComponent();
        InitializeTwitterAuthorizationManager(usrTwitterAuthorization);

        // m_sSearchTerm
        // m_bIncludeFollowedEdges
        // m_bIncludeRepliesToEdges
        // m_bIncludeMentionsEdges
        // m_bIncludeNonRepliesToNonMentionsEdges

        // m_iMaximumStatuses
        // m_bIncludeStatuses
        // m_bExpandStatusUrls
        // m_bIncludeStatistics

        clbWhatEdgesToInclude.Items.AddRange(
            WhatEdgeToIncludeInformation.GetAllWhatEdgeToIncludeInformation()
            );

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
            // Validate the controls.

            if ( !ValidateRequiredTextBox(txbSearchTerm,
                    "Enter one or more words to search for.",
                    out m_sSearchTerm) )
            {
                return (false);
            }

            String sSearchTermLower = m_sSearchTerm.ToLower();

            foreach (String sProhibitedTerm in new String [] {
                "near:",
                "within:"
                } )
            {
                if (sSearchTermLower.IndexOf(sProhibitedTerm) >= 0)
                {
                    OnInvalidTextBox(txbSearchTerm, String.Format(
                    
                        "Although you can use \"{0}\" on Twitter's own search"
                        + " page, Twitter doesn't allow it to be used when"
                        + " searching from another program, such as NodeXL."
                        + "  Remove the \"{0}\" and try again."
                        ,
                        sProhibitedTerm
                        ) );

                    return (false);
                }
            }

            m_bIncludeRepliesToEdges = clbWhatEdgesToInclude.GetItemChecked(0);
            m_bIncludeMentionsEdges = clbWhatEdgesToInclude.GetItemChecked(1);

            m_bIncludeNonRepliesToNonMentionsEdges =
                clbWhatEdgesToInclude.GetItemChecked(2);

            m_bIncludeFollowedEdges = clbWhatEdgesToInclude.GetItemChecked(3);

            m_iMaximumStatuses = (Int32)nudMaximumStatuses.Value;

            m_bIncludeStatuses = chkIncludeStatuses.Checked;
            m_bExpandStatusUrls = chkExpandStatusUrls.Checked;
            m_bIncludeStatistics = chkIncludeStatistics.Checked;
        }
        else
        {
            txbSearchTerm.Text = m_sSearchTerm;

            clbWhatEdgesToInclude.SetItemChecked(0, m_bIncludeRepliesToEdges);
            clbWhatEdgesToInclude.SetItemChecked(1, m_bIncludeMentionsEdges);

            clbWhatEdgesToInclude.SetItemChecked(2,
                m_bIncludeNonRepliesToNonMentionsEdges);

            clbWhatEdgesToInclude.SetItemChecked(3, m_bIncludeFollowedEdges);

            nudMaximumStatuses.Value = m_iMaximumStatuses;
            chkIncludeStatuses.Checked = m_bIncludeStatuses;
            chkExpandStatusUrls.Checked = m_bExpandStatusUrls;
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

        Debug.Assert(m_oHttpNetworkAnalyzer is TwitterSearchNetworkAnalyzer);

        TwitterSearchNetworkAnalyzer.WhatToInclude eWhatToInclude =

            (m_bIncludeStatuses ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.Statuses : 0)
            |
            (m_bExpandStatusUrls ?
                TwitterSearchNetworkAnalyzer
                    .WhatToInclude.ExpandedStatusUrls : 0)
            |
            (m_bIncludeStatistics ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.Statistics : 0)
            |
            (m_bIncludeFollowedEdges ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.FollowedEdges : 0)
            |
            (m_bIncludeRepliesToEdges ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.RepliesToEdges : 0)
            |
            (m_bIncludeMentionsEdges ?
                TwitterSearchNetworkAnalyzer.WhatToInclude.MentionsEdges : 0)
            |
            (m_bIncludeNonRepliesToNonMentionsEdges ?
                TwitterSearchNetworkAnalyzer.
                WhatToInclude.NonRepliesToNonMentionsEdges : 0)
            ;

        ( (TwitterSearchNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_sSearchTerm, eWhatToInclude, m_iMaximumStatuses);
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

        Boolean bIsBusy = m_oHttpNetworkAnalyzer.IsBusy;

        EnableControls(!bIsBusy, pnlUserInputs);
        chkExpandStatusUrls.Enabled = chkIncludeStatuses.Checked;
        btnOK.Enabled = !bIsBusy;
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

        this.ShowInformation("There are no people in that network.");
        txbSearchTerm.Focus();
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

        Debug.Assert(m_sSearchTerm != null);
        // m_bIncludeFollowedEdges
        // m_bIncludeRepliesToEdges
        // m_bIncludeMentionsEdges
        // m_bIncludeNonRepliesToNonMentionsEdges

        Debug.Assert(m_iMaximumStatuses > 0);
        Debug.Assert(m_iMaximumStatuses != Int32.MaxValue);
        // m_bIncludeStatuses
        // m_bExpandStatusUrls
        // m_bIncludeStatistics
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // These are static so that the dialog's controls will retain their values
    // between dialog invocations.  Most NodeXL dialogs persist control values
    // via ApplicationSettingsBase, but this plugin does not have access to
    // that and so it resorts to static fields.

    /// The search term to use.

    protected static String m_sSearchTerm = "NodeXL";

    /// true to include an edge for each followed relationship.

    protected static Boolean m_bIncludeFollowedEdges = false;

    /// true to include an edge from person A to person B if person A's tweet
    /// is a reply to person B.

    protected static Boolean m_bIncludeRepliesToEdges = true;

    /// true to include an edge from person A to person B if person A's tweet
    /// is mentions person B.

    protected static Boolean m_bIncludeMentionsEdges = true;

    /// Include an edge from person A to person A (a self-loop) if person A's
    /// tweet doesn't reply to or mention anyone.

    protected static Boolean m_bIncludeNonRepliesToNonMentionsEdges = true;

    /// Maximum number of tweets to request.

    protected static Int32 m_iMaximumStatuses = 100;

    /// true to include each status.

    protected static Boolean m_bIncludeStatuses = false;

    /// true to expand the URLs in each status.

    protected static Boolean m_bExpandStatusUrls = false;

    /// true to include statistics for each user.

    protected static Boolean m_bIncludeStatistics = false;


//*****************************************************************************
//  Embedded class: WhatEdgeToIncludeInformation
//
/// <summary>
/// Stores information about one edge-related flag in the <see
/// cref="TwitterSearchNetworkAnalyzer.WhatToInclude" /> enumeration.
/// </summary>
///
/// <remarks>
/// Call <see cref="GetAllWhatEdgeToIncludeInformation"/> to get information
/// about each edge-related flag in the <see
/// cref="TwitterSearchNetworkAnalyzer.WhatToInclude" /> enumeration.
/// </remarks>
//*****************************************************************************

private class
WhatEdgeToIncludeInformation
{
    //*************************************************************************
    //  Property: WhatEdgeToInclude
    //
    /// <summary>
    /// Gets the edge-related flag.
    /// </summary>
    ///
    /// <value>
    /// The flag in the <see
    /// cref="TwitterSearchNetworkAnalyzer.WhatToInclude" /> enumeration.
    /// </value>
    //*************************************************************************

    public TwitterSearchNetworkAnalyzer.WhatToInclude
    WhatEdgeToInclude
    {
        get
        {
            AssertValid();

            return (m_eWhatEdgeToInclude);
        }
    }

    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the flag's friendly name.
    /// </summary>
    ///
    /// <value>
    /// The flag's friendly name, suitable for display in a dialog box.
    /// </value>
    //*************************************************************************

    public String
    Name
    {
        get
        {
            AssertValid();

            return (m_sName);
        }
    }

    //*************************************************************************
    //  Method: GetAllWhatEdgeToIncludeInformation()
    //
    /// <summary>
    /// Gets an array of <see cref="WhatEdgeToIncludeInformation" /> objects.
    /// </summary>
    ///
    /// <returns>
    /// An array of <see cref="WhatEdgeToIncludeInformation" /> objects, one
    /// for each edge-related flag in the <see
    /// cref="TwitterSearchNetworkAnalyzer.WhatToInclude" /> enumeration.
    /// </returns>
    //*************************************************************************

    public static WhatEdgeToIncludeInformation []
    GetAllWhatEdgeToIncludeInformation()
    {
        // NOTE:
        //
        // If this list is modified, including the item order, DoDataExchange()
        // must also be modified.

        return ( new WhatEdgeToIncludeInformation[] {

            new WhatEdgeToIncludeInformation(
                TwitterSearchNetworkAnalyzer.WhatToInclude.RepliesToEdges,
                "\"Replies-to\" relationship in tweet"
                ),

            new WhatEdgeToIncludeInformation(
                TwitterSearchNetworkAnalyzer.WhatToInclude.MentionsEdges,
                "\"Mentions\" relationship in tweet"
                ),

            new WhatEdgeToIncludeInformation(
                TwitterSearchNetworkAnalyzer.WhatToInclude
                    .NonRepliesToNonMentionsEdges,
                "Tweet that is not a \"replies-to\" or \"mentions\""
                ),

            new WhatEdgeToIncludeInformation(
                TwitterSearchNetworkAnalyzer.WhatToInclude.FollowedEdges,
                "Follows relationship (very slow!)"
                ),
            } );
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Formats the value of the current instance using the default format. 
    /// </summary>
    ///
    /// <returns>
    /// The formatted string.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        return (m_sName);
    }

    //*************************************************************************
    //  Constructor: WhatEdgeToIncludeInformation()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="WhatEdgeToIncludeInformation" /> class.
    /// </summary>
    ///
    /// <param name="whatEdgeToInclude">
    /// The flag in the <see
    /// cref="TwitterSearchNetworkAnalyzer.WhatToInclude" /> enumeration.
    /// </param>
    ///
    /// <param name="name">
    /// The flag's friendly name, suitable for display in a dialog box.
    /// </param>
    //*************************************************************************

    private WhatEdgeToIncludeInformation
    (
        TwitterSearchNetworkAnalyzer.WhatToInclude whatEdgeToInclude,
        String name
    )
    {
        m_eWhatEdgeToInclude = whatEdgeToInclude;
        m_sName = name;

        AssertValid();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // m_eWhatEdgeToInclude
        Debug.Assert( !String.IsNullOrEmpty(m_sName) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The flag in the WhatToInclude enumeration.

    protected TwitterSearchNetworkAnalyzer.WhatToInclude m_eWhatEdgeToInclude;

    /// The flag's friendly name.

    protected String m_sName;
}

}

}
