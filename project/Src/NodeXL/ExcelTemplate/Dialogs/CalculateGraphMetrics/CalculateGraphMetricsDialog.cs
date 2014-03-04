

using System;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CalculateGraphMetricsDialog
//
/// <summary>
/// Calculates one or more sets of graph metrics and stores the result in the
/// workbook.
/// </summary>
///
/// <remarks>
/// <see cref="GraphMetricCalculationManager" /> and <see
/// cref="GraphMetricWriter" /> objects do most of the work.  The calculations
/// are done asynchronously, so they don't hang the UI and can be cancelled by
/// the user.  Writing the results to the worksheet is done synchronously,
/// however, and can't be cancelled.
///
/// <para>
/// If graph metrics are successfully calculated, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  If graph metrics are
/// cancelled, <see cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class CalculateGraphMetricsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: CalculateGraphMetricsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="CalculateGraphMetricsDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CalculateGraphMetricsDialog" /> class with a specified list of
    /// graph metric calculators.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to calculate the graph metrics for, or null.  If null, the graph
    /// is read from <paramref name="workbook" />.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook where the graph metrics will get written.  If <paramref
    /// name="graph" /> is null, the graph is read from this workbook, too.
    /// </param>
    ///
    /// <param name="graphMetricCalculators">
    /// An array of <see cref="IGraphMetricCalculator2" /> implementations, one
    /// for each set of graph metrics that should be calculated, or null to use
    /// a default list of graph metric calculators.
    /// </param>
    ///
    /// <param name="dialogTitle">
    /// Title for the dialog, or null to use a default title.
    /// </param>
    ///
    /// <param name="activateRelevantWorksheetWhenDone">
    /// true to activate one of the worksheets to let the user know that graph
    /// metrics have been calculated.  The worksheet to activate is determined
    /// by which columns were calculated.
    /// </param>
    //*************************************************************************

    public CalculateGraphMetricsDialog
    (
        IGraph graph,
        Microsoft.Office.Interop.Excel.Workbook workbook,
        IGraphMetricCalculator2 [] graphMetricCalculators,
        String dialogTitle,
        Boolean activateRelevantWorksheetWhenDone
    )
    : this(graph, workbook)
    {
        m_oGraphMetricCalculators = graphMetricCalculators;

        m_bActivateRelevantWorksheetWhenDone =
            activateRelevantWorksheetWhenDone;

        if (dialogTitle != null)
        {
            this.Text = dialogTitle;
        }

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: CalculateGraphMetricsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CalculateGraphMetricsDialog" /> class with a default list of
    /// graph metric calculators.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to calculate the graph metrics for, or null.  If null, the graph
    /// is read from <paramref name="workbook" />.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook where the graph metrics will get written.  If <paramref
    /// name="graph" /> is null, the graph is read from this workbook, too.
    /// </param>
    //*************************************************************************

    public CalculateGraphMetricsDialog
    (
        IGraph graph,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oCalculateGraphMetricsDialogUserSettings =
            new CalculateGraphMetricsDialogUserSettings(this);

        m_oGraph = graph;
        m_oWorkbook = workbook;
        m_oGraphMetricCalculators = null;
        m_bActivateRelevantWorksheetWhenDone = true;

        m_oGraphMetricCalculationManager = new GraphMetricCalculationManager();

        m_oGraphMetricCalculationManager.GraphMetricCalculationProgressChanged
            += new ProgressChangedEventHandler(
        GraphMetricCalculationManager_GraphMetricCalculationProgressChanged);

        m_oGraphMetricCalculationManager.GraphMetricCalculationCompleted +=
            new RunWorkerCompletedEventHandler(
                GraphMetricCalculationManager_GraphMetricCalculationCompleted);

        DoDataExchange(false);

        // Assume that calculations will not succeed.

        this.DialogResult = DialogResult.Cancel;

        AssertValid();
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
        if (bFromControls)
        {
            // (Do nothing.)
        }
        else
        {
            // (Do nothing.)
        }

        return (true);
    }

    //*************************************************************************
    //  Method: WriteGraphMetricColumnsToWorkbook()
    //
    /// <summary>
    /// Writes an array of GraphMetricColumn objects to the workbook.
    /// </summary>
    ///
    /// <param name="aoGraphMetricColumns">
    /// An array of GraphMetricColumn objects, one for each column of metrics
    /// that were calculated.
    /// </param>
    //*************************************************************************

    protected void
    WriteGraphMetricColumnsToWorkbook
    (
        GraphMetricColumn [] aoGraphMetricColumns
    )
    {
        Debug.Assert(aoGraphMetricColumns != null);
        AssertValid();

        Microsoft.Office.Interop.Excel.Application oApplication =
            m_oWorkbook.Application;

        GraphMetricWriter oGraphMetricWriter = new GraphMetricWriter();

        oApplication.ScreenUpdating = false;

        try
        {
            oGraphMetricWriter.WriteGraphMetricColumnsToWorkbook(
                aoGraphMetricColumns, m_oWorkbook);

            if (m_bActivateRelevantWorksheetWhenDone)
            {
                // Let the user know that graph metrics have been calculated.

                oGraphMetricWriter.ActivateRelevantWorksheet(
                    aoGraphMetricColumns, m_oWorkbook);
            }
        }
        catch (Exception oException)
        {
            oApplication.ScreenUpdating = true;

            ErrorUtil.OnException(oException);

            this.Close();
            return;
        }

        oApplication.ScreenUpdating = true;
    }

    //*************************************************************************
    //  Method: OnLoad()
    //
    /// <summary>
    /// Handles the OnLoad event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected override void
    OnLoad
    (
        EventArgs e
    )
    {
        AssertValid();

        base.OnLoad(e);

        // Start the calculations.

        try
        {
            if (m_oGraphMetricCalculators != null)
            {
                // Use the specified graph metric calculators.

                m_oGraphMetricCalculationManager.CalculateGraphMetricsAsync(
                    m_oGraph, m_oWorkbook, m_oGraphMetricCalculators);
            }
            else
            {
                // Use a default list of graph metric calculators.

                m_oGraphMetricCalculationManager.CalculateGraphMetricsAsync(
                    m_oGraph, m_oWorkbook);
            }
        }
        catch (Exception oException)
        {
            // An exception was thrown from the synchronous code within
            // CalculateGraphMetricsAsync().  (Exceptions thrown from the
            // asynchronous code are handled by the
            // GraphMetricCalculationCompleted event handler.)

            ErrorUtil.OnException(oException);

            this.Close();
        }
    }

    //*************************************************************************
    //  Method: OnClosed()
    //
    /// <summary>
    /// Handles the Closed event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnClosed
    (
        EventArgs e
    )
    {
        AssertValid();

        if (m_oGraphMetricCalculationManager.IsBusy)
        {
            // Let the background thread cancel its task, but don't try to
            // notify this dialog.

            m_oGraphMetricCalculationManager.
                GraphMetricCalculationProgressChanged -=
                new ProgressChangedEventHandler(
        GraphMetricCalculationManager_GraphMetricCalculationProgressChanged);

            m_oGraphMetricCalculationManager.GraphMetricCalculationCompleted -=
            new RunWorkerCompletedEventHandler(
                GraphMetricCalculationManager_GraphMetricCalculationCompleted);

            m_oGraphMetricCalculationManager.CancelAsync();
        }
    }

    //*************************************************************************
    //  Method: GraphMetricCalculationManager_
    //          GraphMetricCalculationProgressChanged()
    //
    /// <summary>
    /// Handles the GraphMetricCalculationProgressChanged event on the
    /// GraphMetricCalculationManager object.
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
    GraphMetricCalculationManager_GraphMetricCalculationProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        Debug.Assert(e.UserState is GraphMetricProgress);
        AssertValid();

        GraphMetricProgress oGraphMetricProgress =
            (GraphMetricProgress)e.UserState;

        lblStatus.Text = oGraphMetricProgress.ProgressMessage;
        pbProgress.Value = e.ProgressPercentage;

        if (oGraphMetricProgress.AllGraphMetricsCalculated)
        {
            // Writing to the workbook will occur shortly within the
            // GraphMetricCalculationCompleted handler.  Writing can't be
            // cancelled, so disable the cancel button.

            btnCancel.Enabled = false;
        }
    }

    //*************************************************************************
    //  Method: GraphMetricCalculationManager_GraphMetricCalculationCompleted()
    //
    /// <summary>
    /// Handles the GraphMetricCalculationCompleted event on the
    /// GraphMetricCalculationManager object.
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
    GraphMetricCalculationManager_GraphMetricCalculationCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        Exception oException = e.Error;

        if (oException != null)
        {
            if (oException is GraphMetricException)
            {
                // This is a known exception.

                this.ShowWarning(oException.Message);
            }
            else
            {
                // The exception is unexpected.

                ErrorUtil.OnException(oException);
            }
        }
        else if (!e.Cancelled)
        {
            Debug.Assert( e.Result is GraphMetricColumn[] );

            WriteGraphMetricColumnsToWorkbook(
                ( GraphMetricColumn[] )e.Result );

            // Everything succeeded.

            this.DialogResult = DialogResult.OK;
        }

        this.Close();
    }

    //*************************************************************************
    //  Method: btnCancel_Click()
    //
    /// <summary>
    /// Handles the Clicke event on the btnCancel button.
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
    btnCancel_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // Close the dialog, which will cancel the calculations.

        this.Close();
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

        Debug.Assert(m_oCalculateGraphMetricsDialogUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        // m_oGraphMetricCalculators
        Debug.Assert(m_oGraphMetricCalculationManager != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected CalculateGraphMetricsDialogUserSettings
        m_oCalculateGraphMetricsDialogUserSettings;

    /// Graph to calculate the graph metrics for, or null.

    protected IGraph m_oGraph;

    /// Workbook where the graph metrics will get written.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// An array of IGraphMetricCalculator2 implementations, one
    /// for each set of graph metrics that should be calculated, or null to use
    /// a default list of implementations.

    protected IGraphMetricCalculator2 [] m_oGraphMetricCalculators;

    /// true to activate one of the worksheets to let the user know that graph
    /// metrics have been calculated.  The worksheet to activate is determined

    protected Boolean m_bActivateRelevantWorksheetWhenDone;

    /// Object that does most of the work.

    protected GraphMetricCalculationManager m_oGraphMetricCalculationManager;
}


//*****************************************************************************
//  Class: CalculateGraphMetricsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="CalculateGraphMetricsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("CalculateGraphMetricsDialog") ]

public class CalculateGraphMetricsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: CalculateGraphMetricsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CalculateGraphMetricsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public CalculateGraphMetricsDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
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
