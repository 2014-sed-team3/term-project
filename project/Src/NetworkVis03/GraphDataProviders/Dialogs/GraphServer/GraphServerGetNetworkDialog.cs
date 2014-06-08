
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.GraphServer
{
//*****************************************************************************
//  Class: GraphServerGetNetworkDialog
//
/// <summary>
/// Uses the NodeXL Graph Server to get a network of people who have tweeted
/// a specified search term.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to show the dialog.  If it returns
/// DialogResult.OK, get the network from the <see
/// cref="GraphDataProviderDialogBase.Results" /> property.
/// </remarks>
//*****************************************************************************

public partial class GraphServerGetNetworkDialog : GraphDataProviderDialogBase
{
    //*************************************************************************
    //  Constructor: GraphServerGetNetworkDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphServerGetNetworkDialog" /> class.
    /// </summary>
    //*************************************************************************

    public GraphServerGetNetworkDialog()
    :
    base( new GraphServerNetworkAnalyzer() )
    {
        InitializeComponent();

        // m_sSearchTerm
        // m_oMinimumStatusDateUtc
        // m_oMaximumStatusDateUtc
        // m_bExpandStatusUrls
        // m_sGraphServerUserName
        // m_sGraphServerPassword

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

            if (
                !ValidateRequiredTextBox(txbSearchTerm,
                    "Enter one or more words to search for.",
                    out m_sSearchTerm)
                )
            {
                return (false);
            }

            m_oMinimumStatusDateUtc = dtpMinimumStatusDateUtc.Value;
            m_oMaximumStatusDateUtc = dtpMaximumStatusDateUtc.Value;

            if (m_oMaximumStatusDateUtc < m_oMinimumStatusDateUtc)
            {
                OnInvalidDateTimePicker(dtpMaximumStatusDateUtc,
                    "The second date can't be earlier than the first date."
                    );

                return (false);
            }

            m_bExpandStatusUrls = chkExpandStatusUrls.Checked;

            if (
                !ValidateRequiredTextBox(txbGraphServerUserName,

                    "Enter the user name for your account on the NodeXL Graph"
                        + " Server.", 

                    out m_sGraphServerUserName)
                ||
                !ValidateRequiredTextBox(txbGraphServerPassword,

                    "Enter the password for your account on the NodeXL Graph"
                        + " Server.", 

                    out m_sGraphServerPassword)
                )
            {
                return (false);
            }
        }
        else
        {
            txbSearchTerm.Text = m_sSearchTerm;
            dtpMinimumStatusDateUtc.Value = m_oMinimumStatusDateUtc;
            dtpMaximumStatusDateUtc.Value = m_oMaximumStatusDateUtc;
            chkExpandStatusUrls.Checked = m_bExpandStatusUrls;
            txbGraphServerUserName.Text = m_sGraphServerUserName;
            txbGraphServerPassword.Text = m_sGraphServerPassword;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: StartAnalysis()
    //
    /// <summary>
    /// Starts the analysis.
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

        Debug.Assert(m_oHttpNetworkAnalyzer is GraphServerNetworkAnalyzer);

        ( (GraphServerNetworkAnalyzer)m_oHttpNetworkAnalyzer ).
            GetNetworkAsync(m_sSearchTerm, m_oMinimumStatusDateUtc,
                AddDayToMaximumStatusDateUtc(m_oMaximumStatusDateUtc),
                m_bExpandStatusUrls, m_sGraphServerUserName,
                m_sGraphServerPassword);
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
        btnOK.Enabled = !bIsBusy;
        this.UseWaitCursor = bIsBusy;
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

        this.ShowInformation(
            "That network is not available from the NodeXL Graph Server."
            );

        txbSearchTerm.Focus();
    }

    //*************************************************************************
    //  Method: AddDayToMaximumStatusDateUtc()
    //
    /// <summary>
    /// Adds one day minus one second to the maximum status date.
    /// </summary>
    ///
    /// <param name="oMaximumStatusDateUtc">
    /// Maximum status date, in UTC.  The time component must be zero.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="oMaximumStatusDateUtc" /> with one day minus
    /// one second added to it.
    /// </returns>
    ///
    /// <remarks>
    /// The dialog's DateTimePicker controls provide dates that have their time
    /// components set to zero.  This method sets the time component of the
    /// maximum date to 23:59:59.
    ///
    /// <para>
    /// If the user specifies a date range of 5/1/2013 through 5/2/2013, for
    /// example, the range to request from the Graph Server should be 5/1/2013
    /// at 00:00:00 through 5/2/2013 at 23:59:59.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected DateTime
    AddDayToMaximumStatusDateUtc
    (
        DateTime oMaximumStatusDateUtc
    )
    {
        Debug.Assert(oMaximumStatusDateUtc.TimeOfDay == TimeSpan.Zero);
        AssertValid();

        return ( oMaximumStatusDateUtc.AddDays(1).AddSeconds(-1) );
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
        // m_oMinimumStatusDateUtc
        // m_oMaximumStatusDateUtc
        // m_bExpandStatusUrls
        Debug.Assert(m_sGraphServerUserName != null);
        Debug.Assert(m_sGraphServerPassword != null);
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

    /// Minimum status date, in UTC.

    protected static DateTime m_oMinimumStatusDateUtc =
        DateTime.Now.Date.AddDays(-1);

    /// Maximum status date, in UTC.

    protected static DateTime m_oMaximumStatusDateUtc = DateTime.Now.Date;

    /// true to expand the URLs in each status.

    protected static Boolean m_bExpandStatusUrls = false;

    /// User name for the account to use on the NodeXL Graph Server.

    protected static String m_sGraphServerUserName = String.Empty;

    /// Password for the account to use on the NodeXL Graph Server.

    protected static String m_sGraphServerPassword = String.Empty;
}
}
