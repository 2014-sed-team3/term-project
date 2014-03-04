

using System;
using System.Configuration;
using System.Windows.Forms;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphSummaryDialog
//
/// <summary>
/// Shows a summary of the steps that were taken to create the graph.
/// </summary>
//*****************************************************************************

public partial class GraphSummaryDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GraphSummaryDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphSummaryDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public GraphSummaryDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        m_oWorkbook = workbook;
        InitializeComponent();

        PerWorkbookSettings oPerWorkbookSettings = this.PerWorkbookSettings;
        m_oGraphHistory = oPerWorkbookSettings.GraphHistory;

        m_oAutoFillWorkbookResults =
            oPerWorkbookSettings.AutoFillWorkbookResults;

        ( new OverallMetricsReader() ).TryReadMetrics(
            m_oWorkbook, out m_oOverallMetrics);

        ( new TopNByMetricsReader() ).TryReadMetrics(
            m_oWorkbook, out m_sTopNByMetrics);

        ( new TwitterSearchNetworkTopItemsReader() ).TryReadMetrics(
            m_oWorkbook, out m_sTwitterSearchNetworkTopItems);

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oGraphSummaryDialogUserSettings =
            new GraphSummaryDialogUserSettings(this);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: PerWorkbookSettings
    //
    /// <summary>
    /// Gets a new PerWorkbookSettings object.
    /// </summary>
    ///
    /// <value>
    /// A new PerWorkbookSettings object.
    /// </value>
    //*************************************************************************

    private PerWorkbookSettings
    PerWorkbookSettings
    {
        get
        {
            // AssertValid();
            Debug.Assert(m_oWorkbook != null);

            return ( new PerWorkbookSettings(m_oWorkbook) );
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

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        AssertValid();

        if (bFromControls)
        {
            String sComments = TrimTextBox(txbComments);

            if (sComments != EnterCommentsMessage)
            {
                m_oGraphHistory[GraphHistoryKeys.Comments] = sComments;
            }
        }
        else
        {
            SetGraphHistoryTextBoxText(this.txbImportDescription,
                GraphHistoryKeys.ImportDescription);

            SetGraphHistoryTextBoxText(this.txbGraphDirectedness,
                GraphHistoryKeys.GraphDirectedness);

            SetGraphHistoryTextBoxText(this.txbLayoutAlgorithm,
                GraphHistoryKeys.LayoutAlgorithm);

            SetGraphHistoryTextBoxText(this.txbGroupingDescription,
                GraphHistoryKeys.GroupingDescription);

            SetTextBoxText( this.txbAutoFillWorkbookResults,
                m_oAutoFillWorkbookResults.ConvertToSummaryString() );

            SetTextBoxText( this.txbOverallMetrics,
                (m_oOverallMetrics == null) ? null :
                m_oOverallMetrics.ConvertToSummaryString() );

            SetTextBoxText(this.txbTopNByMetrics, m_sTopNByMetrics);

            SetTextBoxText(this.txbTwitterSearchNetworkTopItems,
                m_sTwitterSearchNetworkTopItems);

            SetGraphHistoryTextBoxText(this.txbComments,
                GraphHistoryKeys.Comments, EnterCommentsMessage);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SetGraphHistoryTextBoxText()
    //
    /// <overloads>
    /// Sets the text in a TextBox based on a graph history value.
    /// </overloads>
    ///
    /// <summary>
    /// Sets the text in a TextBox based on a graph history value using a
    /// default message if the graph history value isn't available.
    /// </summary>
    ///
    /// <param name="oTextBox">
    /// The TextBox to set the text on.
    /// </param>
    ///
    /// <param name="sGraphHistoryKey">
    /// <see cref="GraphHistoryKeys" /> key corresponding to the TextBox.
    /// </param>
    //*************************************************************************

    protected void
    SetGraphHistoryTextBoxText
    (
        TextBox oTextBox,
        String sGraphHistoryKey
    )
    {
        Debug.Assert(oTextBox != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphHistoryKey) );
        AssertValid();

        SetGraphHistoryTextBoxText(oTextBox, sGraphHistoryKey,
            NotAvailableMessage);
    }

    //*************************************************************************
    //  Method: SetGraphHistoryTextBoxText()
    //
    /// <summary>
    /// Sets the text in a TextBox based on a graph history value using a
    /// specified message if the graph history value isn't available.
    /// </summary>
    ///
    /// <param name="oTextBox">
    /// The TextBox to set the text on.
    /// </param>
    ///
    /// <param name="sGraphHistoryKey">
    /// <see cref="GraphHistoryKeys" /> key corresponding to the TextBox.
    /// </param>
    ///
    /// <param name="sNotAvailableMessage">
    /// Message to show if the graph history value isn't available.  Can be
    /// empty but not null.
    /// </param>
    //*************************************************************************

    protected void
    SetGraphHistoryTextBoxText
    (
        TextBox oTextBox,
        String sGraphHistoryKey,
        String sNotAvailableMessage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sGraphHistoryKey) );
        Debug.Assert(oTextBox != null);
        Debug.Assert(sNotAvailableMessage != null);

        String sGraphHistoryValue;

        if ( !m_oGraphHistory.TryGetValue(sGraphHistoryKey,
            out sGraphHistoryValue) )
        {
            sGraphHistoryValue = sNotAvailableMessage;
        }

        oTextBox.Text = sGraphHistoryValue;
    }

    //*************************************************************************
    //  Method: SetTextBoxText()
    //
    /// <summary>
    /// Sets the text in a TextBox to either a specified string, or a "not
    /// available" message if the string is null or empty.
    /// </summary>
    ///
    /// <param name="oTextBox">
    /// The TextBox to set the text on.
    /// </param>
    ///
    /// <param name="sString">
    /// The string to set the TextBox text to, or null.
    /// </param>
    //*************************************************************************

    protected void
    SetTextBoxText
    (
        TextBox oTextBox,
        String sString
    )
    {
        Debug.Assert(oTextBox != null);

        oTextBox.Text = String.IsNullOrEmpty(sString) ?
            NotAvailableMessage : sString;
    }

    //*************************************************************************
    //  Method: btnCopyToClipboard_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCopyToClipboard button.
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

    private void
    btnCopyToClipboard_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        if ( DoDataExchange(true) )
        {
            String sGraphSummary;

            if ( GraphSummarizer.TrySummarizeGraph(m_oGraphHistory,
                m_oAutoFillWorkbookResults, m_oOverallMetrics,
                m_sTopNByMetrics, m_sTwitterSearchNetworkTopItems,
                out sGraphSummary) )
            {
                Clipboard.SetText(sGraphSummary);
            }
        }
    }

    //*************************************************************************
    //  Method: btnClear_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnClear button.
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

    private void
    btnClear_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        m_oGraphHistory = new GraphHistory();
        m_oAutoFillWorkbookResults = new AutoFillWorkbookResults();
        DoDataExchange(false);
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

    private void
    btnOK_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        if ( DoDataExchange(true) )
        {
            PerWorkbookSettings oPerWorkbookSettings =
                this.PerWorkbookSettings;

            oPerWorkbookSettings.GraphHistory = m_oGraphHistory;

            oPerWorkbookSettings.AutoFillWorkbookResults =
                m_oAutoFillWorkbookResults;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
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

        Debug.Assert(m_oGraphSummaryDialogUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oGraphHistory != null);
        Debug.Assert(m_oAutoFillWorkbookResults != null);
        // m_oOverallMetrics
        // m_sTopNByMetrics
        // m_sTwitterSearchNetworkTopItems
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Message to show in the comments TextBox when comments aren't available.

    protected const String EnterCommentsMessage =
        "[Enter your own comments here.]";

    /// Message to show in other TextBoxes when information isn't available.

    protected const String NotAvailableMessage =
        "[Not available.]";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected GraphSummaryDialogUserSettings m_oGraphSummaryDialogUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Stores attributes that describe how a graph was created.

    protected GraphHistory m_oGraphHistory;

    /// Stores the results of a call to WorkbookAutoFiller.AutoFillWorkbook.

    protected AutoFillWorkbookResults m_oAutoFillWorkbookResults;

    /// Overall metrics for the graph, or null if not available.

    protected OverallMetrics m_oOverallMetrics;

    /// Top-N-by metrics for the graph, or null if not available.

    protected String m_sTopNByMetrics;

    /// Twitter search network top items for the graph, or null if not
    /// available.

    protected String m_sTwitterSearchNetworkTopItems;
}


//*****************************************************************************
//  Class: GraphSummaryDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="GraphSummaryDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphSummaryDialog") ]

public class GraphSummaryDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GraphSummaryDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphSummaryDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GraphSummaryDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, false)
    {
        Debug.Assert(oForm != null);

        // (Do nothing.)

        AssertValid();
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
