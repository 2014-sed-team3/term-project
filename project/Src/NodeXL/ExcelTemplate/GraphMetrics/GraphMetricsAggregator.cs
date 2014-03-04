
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricsAggregator
//
/// <summary>
/// Aggregates the overall metrics from a folder full of NodeXL workbooks into
/// a new Excel workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="AggregateGraphMetricsAsync" /> to aggregate the overall
/// metrics.  Call <see cref="CancelAsync" /> to stop the aggregation.  Handle
/// the <see cref="AggregationProgressChanged" /> and <see
/// cref="AggregationCompleted" /> events to monitor the progress and
/// completion of the aggregation.
/// </remarks>
//*****************************************************************************

public class GraphMetricsAggregator : Object
{
    //*************************************************************************
    //  Constructor: GraphMetricsAggregator()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricsAggregator" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphMetricsAggregator()
    {
        m_oBackgroundWorker = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a flag indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            return (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: AggregateGraphMetricsAsync()
    //
    /// <summary>
    /// Asynchronously aggregates the overall metrics from a folder full of
    /// NodeXL workbooks into a new Excel workbook.
    /// </summary>
    ///
    /// <param name="sourceFolderPath">
    /// Folder containing NodeXL workbooks.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook calling this method.
    /// </param>
    ///
    /// <remarks>
    /// When aggregation completes, the <see cref="AggregationCompleted" />
    /// event fires.
    ///
    /// <para>
    /// To cancel the analysis, call <see cref="CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    AggregateGraphMetricsAsync
    (
        String sourceFolderPath,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sourceFolderPath) );
        Debug.Assert( Directory.Exists(sourceFolderPath) );
        Debug.Assert(workbook != null);
        AssertValid();

        if (this.IsBusy)
        {
            throw new InvalidOperationException(
                "GraphMetricsAggregator.AggregateGraphMetricsAsync: An"
                + " asynchronous operation is already in progress."
                );
        }

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        AggregateGraphMetricsAsyncArgs oAggregateGraphMetricsAsyncArgs =
            new AggregateGraphMetricsAsyncArgs();

        oAggregateGraphMetricsAsyncArgs.SourceFolderPath = sourceFolderPath;
        oAggregateGraphMetricsAsyncArgs.Workbook = workbook;

        oAggregateGraphMetricsAsyncArgs.WorkbookSettings =
            GetWorkbookSettings(workbook);

        // Create a BackgroundWorker and handle its events.

        m_oBackgroundWorker = new BackgroundWorker();
        m_oBackgroundWorker.WorkerReportsProgress = true;
        m_oBackgroundWorker.WorkerSupportsCancellation = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.ProgressChanged +=
            new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        m_oBackgroundWorker.RunWorkerAsync(oAggregateGraphMetricsAsyncArgs);
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the aggregation started by <see
    /// cref="AggregateGraphMetricsAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// When the aggregation cancels, the <see cref="AggregationCompleted" />
    /// event fires.  The <see cref="AsyncCompletedEventArgs.Cancelled" />
    /// property will be true.
    /// </remarks>
    //*************************************************************************

    public void
    CancelAsync()
    {
        AssertValid();

        if (this.IsBusy)
        {
            m_oBackgroundWorker.CancelAsync();
        }
    }

    //*************************************************************************
    //  Event: AggregationProgressChanged
    //
    /// <summary>
    /// Occurs when progress is made during the aggregation started by <see
    /// cref="AggregateGraphMetricsAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="ProgressChangedEventArgs.UserState" /> argument is a
    /// String describing the progress.  The String is suitable for display to
    /// the user.
    /// </remarks>
    //*************************************************************************

    public event ProgressChangedEventHandler AggregationProgressChanged;


    //*************************************************************************
    //  Event: AggregationCompleted
    //
    /// <summary>
    /// Occurs when the aggregation started by <see
    /// cref="AggregateGraphMetricsAsync" /> completes, is cancelled, or
    /// encounters an error.
    /// </summary>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler AggregationCompleted;


    //*************************************************************************
    //  Method: AggregateGraphMetricsInternal()
    //
    /// <summary>
    /// Aggregates the overall metrics from a folder full of NodeXL workbooks
    /// into a new Excel workbook.
    /// </summary>
    ///
    /// <param name="oAggregateGraphMetricsAsyncArgs">
    /// Contains the arguments needed to asynchronously aggregate overall
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object.
    /// </param>
    //*************************************************************************

    protected void
    AggregateGraphMetricsInternal
    (
        AggregateGraphMetricsAsyncArgs oAggregateGraphMetricsAsyncArgs,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oAggregateGraphMetricsAsyncArgs != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oDoWorkEventArgs != null);
        AssertValid();

        List<OverallMetricsInfo> oOverallMetricsInfos =
            new List<OverallMetricsInfo>();

        OverallMetricsReader oOverallMetricsReader =
            new OverallMetricsReader();

        foreach ( String sFilePath in Directory.GetFiles(
            oAggregateGraphMetricsAsyncArgs.SourceFolderPath, "*.xlsx") )
        {
            if (oBackgroundWorker.CancellationPending)
            {
                oDoWorkEventArgs.Cancel = true;
                return;
            }

            try
            {
                if (!NodeXLWorkbookUtil.FileIsNodeXLWorkbook(sFilePath))
                {
                    continue;
                }
            }
            catch (IOException)
            {
                // Skip any workbooks that are already open, or that have any
                // other problems that prevent them from being opened.

                continue;
            }

            oBackgroundWorker.ReportProgress(0,
                String.Format(
                    "Reading \"{0}\"."
                    ,
                    Path.GetFileName(sFilePath)
                ) );

            OverallMetricsInfo oOverallMetricsInfo;

            for (Int32 iAttempt = 0; iAttempt < 2; iAttempt++)
            {
                // Have overall metrics already been calculated for the
                // workbook?

                if ( TryGetGraphMetricsForOneNodeXLWorkbook(sFilePath,
                    out oOverallMetricsInfo) )
                {
                    // Yes.

                    oOverallMetricsInfos.Add(oOverallMetricsInfo);
                    break;
                }

                if (iAttempt == 0)
                {
                    // No.  Calculate them.

                    TaskAutomator.AutomateOneWorkbookIndirect(sFilePath,
                        oAggregateGraphMetricsAsyncArgs.WorkbookSettings);
                }
            }
        }

        if (oOverallMetricsInfos.Count > 0)
        {
            WriteOverallMetricsToNewWorkbook(
                oAggregateGraphMetricsAsyncArgs.Workbook.Application,
                oOverallMetricsInfos);
        }

        oBackgroundWorker.ReportProgress(0,
            String.Format(
                "Done.  NodeXL workbooks aggregated: {0}."
                ,
                oOverallMetricsInfos.Count
            ) );
    }

    //*************************************************************************
    //  Method: GetWorkbookSettings()
    //
    /// <summary>
    /// Gets workbook settings to use when graph metrics need to be calculated.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook calling this method.
    /// </param>
    ///
    /// <returns>
    /// The workbook settings to use.
    /// </returns>
    //*************************************************************************

    protected String
    GetWorkbookSettings
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        // Temporarily change some settings.

        AutomateTasksUserSettings oAutomateTasksUserSettings =
            new AutomateTasksUserSettings();

        AutomationTasks eOriginalTasksToRun =
            oAutomateTasksUserSettings.TasksToRun;

        oAutomateTasksUserSettings.TasksToRun =
            AutomationTasks.CalculateGraphMetrics;

        oAutomateTasksUserSettings.Save();

        GraphMetricUserSettings oGraphMetricUserSettings =
            new GraphMetricUserSettings();

        GraphMetrics eOriginalGraphMetricsToCalculate =
            oGraphMetricUserSettings.GraphMetricsToCalculate;

        oGraphMetricUserSettings.GraphMetricsToCalculate =
            GraphMetrics.OverallMetrics;

        oGraphMetricUserSettings.Save();

        // Get the workbook settings that include the temporary changes.

        String sWorkbookSettings =
            ( new PerWorkbookSettings(oWorkbook) ).WorkbookSettings;

        // Restore the original settings.

        oAutomateTasksUserSettings.TasksToRun = eOriginalTasksToRun;

        oAutomateTasksUserSettings.Save();

        oGraphMetricUserSettings.GraphMetricsToCalculate =
            eOriginalGraphMetricsToCalculate;

        oGraphMetricUserSettings.Save();

        return (sWorkbookSettings);
    }

    //*************************************************************************
    //  Method: TryGetGraphMetricsForOneNodeXLWorkbook()
    //
    /// <summary>
    /// Attempts to get the overall metrics from one NodeXL workbook.
    /// </summary>
    ///
    /// <param name="sNodeXLWorkbookFilePath">
    /// Path to the NodeXL workbook.
    /// </param>
    ///
    /// <param name="oOverallMetricsInfo">
    /// Where the overall metrics gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetGraphMetricsForOneNodeXLWorkbook
    (
        String sNodeXLWorkbookFilePath,
        out OverallMetricsInfo oOverallMetricsInfo
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sNodeXLWorkbookFilePath) );
        AssertValid();

        oOverallMetricsInfo = null;

        // Create a new instance of Excel.  Do not use the instance that was
        // passed to AggregateGraphMetricsAsync(), because when a NodeXL
        // workbook is opened and closed in Excel, its memory is not released
        // and the machine will eventually run out of memory.

        Application oExcelApplication = new Application();

        if (oExcelApplication == null)
        {
            throw new Exception("Excel couldn't be opened.");
        }

        // ExcelApplicationKiller requires that the application be visible.

        oExcelApplication.Visible = true;

        ExcelApplicationKiller oExcelApplicationKiller =
            new ExcelApplicationKiller(oExcelApplication);

        Workbook oNodeXLWorkbook = null;

        try
        {
            oNodeXLWorkbook = ExcelUtil.OpenWorkbook(sNodeXLWorkbookFilePath,
                oExcelApplication);

            OverallMetrics oOverallMetrics;

            if ( ( new OverallMetricsReader() ).TryReadMetrics(
                oNodeXLWorkbook, out oOverallMetrics ) )
            {
                oOverallMetricsInfo = new OverallMetricsInfo();

                oOverallMetricsInfo.NodeXLWorkbookFileName =
                    Path.GetFileName(sNodeXLWorkbookFilePath);

                oOverallMetricsInfo.OverallMetrics = oOverallMetrics;

                return (true);
            }
        }
        finally
        {
            if (oNodeXLWorkbook != null)
            {
                oNodeXLWorkbook.Close(false, Missing.Value, Missing.Value);
            }

            oExcelApplication.Quit();

            // Quitting the Excel application does not remove it from
            // memory.  Kill its process.

            oExcelApplicationKiller.KillExcelApplication();
        }

        return (false);
    }

    //*************************************************************************
    //  Method: WriteOverallMetricsToNewWorkbook()
    //
    /// <summary>
    /// Writes the aggregated overall metrics into a new Excel workbook.
    /// </summary>
    ///
    /// <param name="oApplicationForNewWorkbook">
    /// Excel Application object to use to create the new Excel workbook.
    /// </param>
    ///
    /// <param name="oOverallMetricsInfos">
    /// Collection of OverallMetricsInfo object, one for each workbook that was
    /// read.
    /// </param>
    //*************************************************************************

    protected void
    WriteOverallMetricsToNewWorkbook
    (
        Microsoft.Office.Interop.Excel.Application oApplicationForNewWorkbook,
        IList<OverallMetricsInfo> oOverallMetricsInfos
    )
    {
        Debug.Assert(oApplicationForNewWorkbook != null);
        Debug.Assert(oOverallMetricsInfos != null);
        AssertValid();

        Workbook oNewWorkbook =
            oApplicationForNewWorkbook.Workbooks.Add(Missing.Value);


        Int32 iOverallMetricsInfos = oOverallMetricsInfos.Count;

        // There are 16 overall metrics, plus one extra column for the file
        // name.

        const Int32 Columns = 1 + 16;

        // There is one extra row for the headers.

        Object [,] aoCellValues =
            ExcelUtil.Get2DArray<Object>(1 + iOverallMetricsInfos, Columns);

        String [] asColumnHeaders = {
            "File Name",
            OverallMetricNames.Directedness,
            OverallMetricNames.Vertices,
            OverallMetricNames.UniqueEdges,
            OverallMetricNames.EdgesWithDuplicates,
            OverallMetricNames.TotalEdges,
            OverallMetricNames.SelfLoops,
            OverallMetricNames.ReciprocatedVertexPairRatio,
            OverallMetricNames.ReciprocatedEdgeRatio,
            OverallMetricNames.ConnectedComponents,
            OverallMetricNames.SingleVertexConnectedComponents,
            OverallMetricNames.MaximumConnectedComponentVertices,
            OverallMetricNames.MaximumConnectedComponentEdges,
            OverallMetricNames.MaximumGeodesicDistance,
            OverallMetricNames.AverageGeodesicDistance,
            OverallMetricNames.GraphDensity,
            OverallMetricNames.Modularity,
            };

        Debug.Assert(asColumnHeaders.Length == Columns);

        for (Int32 iColumn = 0; iColumn < Columns; iColumn++)
        {
            aoCellValues[1, iColumn + 1] = asColumnHeaders[iColumn];
        }

        for (Int32 i = 0; i < iOverallMetricsInfos; i++)
        {
            OverallMetricsInfo oOverallMetricsInfo = oOverallMetricsInfos[i];

            OverallMetrics oOverallMetrics =
                oOverallMetricsInfo.OverallMetrics;

            Object [] aoColumnValues = {
                oOverallMetricsInfo.NodeXLWorkbookFileName,
                oOverallMetrics.Directedness.ToString(),
                oOverallMetrics.Vertices,
                oOverallMetrics.UniqueEdges,
                oOverallMetrics.EdgesWithDuplicates,
                oOverallMetrics.TotalEdges,
                oOverallMetrics.SelfLoops,

                OverallMetricCalculator2.NullableToGraphMetricValue<Double>(
                    oOverallMetrics.ReciprocatedVertexPairRatio),

                OverallMetricCalculator2.NullableToGraphMetricValue<Double>(
                    oOverallMetrics.ReciprocatedEdgeRatio),

                oOverallMetrics.ConnectedComponents,
                oOverallMetrics.SingleVertexConnectedComponents,
                oOverallMetrics.MaximumConnectedComponentVertices,
                oOverallMetrics.MaximumConnectedComponentEdges,

                OverallMetricCalculator2.NullableToGraphMetricValue<Int32>(
                    oOverallMetrics.MaximumGeodesicDistance),

                OverallMetricCalculator2.NullableToGraphMetricValue<Double>(
                    oOverallMetrics.AverageGeodesicDistance),

                OverallMetricCalculator2.NullableToGraphMetricValue<Double>(
                    oOverallMetrics.GraphDensity),

                OverallMetricCalculator2.NullableToGraphMetricValue<Double>(
                    oOverallMetrics.Modularity),
                };

            Debug.Assert(aoColumnValues.Length == Columns);

            for (Int32 iColumn = 0; iColumn < Columns; iColumn++)
            {
                aoCellValues[i + 2, iColumn + 1] = aoColumnValues[iColumn];
            }
        }

        Worksheet oNewWorksheet = (Worksheet)oNewWorkbook.Worksheets[1];

        // Insert the overall metrics into a table.

        Range oTableRange = ExcelUtil.SetRangeValues(
            (Range)oNewWorksheet.Cells[1, 1], aoCellValues );

        ExcelTableUtil.AddTable(oNewWorksheet, oTableRange, null, null);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        AssertValid();

        Debug.Assert(e.Argument is AggregateGraphMetricsAsyncArgs);

        AggregateGraphMetricsAsyncArgs oAggregateGraphMetricsAsyncArgs =
            (AggregateGraphMetricsAsyncArgs)e.Argument;

        AggregateGraphMetricsInternal(oAggregateGraphMetricsAsyncArgs,
            m_oBackgroundWorker, e);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_ProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        ProgressChangedEventHandler oAggregationProgressChanged =
            this.AggregationProgressChanged;

        if (oAggregationProgressChanged != null)
        {
            oAggregationProgressChanged(this, e);
        }
    }

    //*************************************************************************
    //  Method: BackgroundWorker_RunWorkerCompleted()
    //
    /// <summary>
    /// Handles the RunWorkerCompleted event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_RunWorkerCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        RunWorkerCompletedEventHandler oAggregationCompleted =
            this.AggregationCompleted;

        if (oAggregationCompleted != null)
        {
            oAggregationCompleted(this, e);
        }

        m_oBackgroundWorker = null;
    }

    //*************************************************************************
    //  Class: OverallMetricsInfo
    //
    /// <summary>
    /// Stores information about overall metrics for one workbook.
    /// </summary>
    //*************************************************************************

    protected class OverallMetricsInfo
    {
        /// Name of the workbook, without a path.

        public String NodeXLWorkbookFileName;

        /// Overall metrics read from the workbook.

        public OverallMetrics OverallMetrics;
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
        // m_oBackgroundWorker
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Used for asynchronous aggregation.  null if an asynchronous aggregation
    /// is not in progress.

    protected BackgroundWorker m_oBackgroundWorker;


    //*************************************************************************
    //  Embedded class: AggregateGraphMetricsAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously aggregate overall
    /// metrics.
    /// </summary>
    //*************************************************************************

    protected class AggregateGraphMetricsAsyncArgs
    {
        ///
        public String SourceFolderPath;
        ///
        public Microsoft.Office.Interop.Excel.Workbook Workbook;
        ///
        public String WorkbookSettings;
    };
}

}
