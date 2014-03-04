
using System;
using System.Text;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphSummarizer
//
/// <summary>
/// Generates a summary of the graph.
/// </summary>
///
/// <remarks>
/// Call <see cref="TrySummarizeGraph(Microsoft.Office.Interop.Excel.Workbook,
/// out String)" /> or <see cref="SummarizeGraph(
/// Microsoft.Office.Interop.Excel.Workbook)" /> to get a string that
/// summarizes the graph.
/// </remarks>
//*****************************************************************************

public static class GraphSummarizer : Object
{
    //*************************************************************************
    //  Method: TrySummarizeGraph()
    //
    /// <overloads>
    /// Gets a string that summarizes the graph, or shows a message that a
    /// summary isn't available.
    /// </overloads>
    ///
    /// <summary>
    /// Gets a string that summarizes the graph, or shows a message that a
    /// summary isn't available, given a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook containing the graph.
    /// </param>
    ///
    /// <param name="graphSummary">
    /// Where the graph summary gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph summary was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If a graph summary is available, it gets stored at <paramref
    /// name="graphSummary" /> and true is returned.  Otherwise, a message is
    /// shown and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TrySummarizeGraph
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        out String graphSummary
    )
    {
        Debug.Assert(workbook != null);

        graphSummary = SummarizeGraph(workbook);

        return ( ShowWarningIfNoGraphSummary(graphSummary) );
    }

    //*************************************************************************
    //  Method: TrySummarizeGraph()
    //
    /// <summary>
    /// Gets a string that summarizes the graph, or shows a message that a
    /// summary isn't available, given various summary components.
    /// </summary>
    ///
    /// <param name="graphHistory">
    /// Stores attributes that describe how the graph was created.
    /// </param>
    ///
    /// <param name="autoFillWorkbookResults">
    /// Stores the results of a call to <see
    /// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
    /// </param>
    ///
    /// <param name="overallMetrics">
    /// Stores the overall metrics for a graph, or null or an empty string if
    /// not available.
    /// </param>
    ///
    /// <param name="topNByMetrics">
    /// The graph's top-N-by metrics, as a descriptive string, or null or an
    /// empty string if not available.
    /// </param>
    ///
    /// <param name="twitterSearchNetworkTopItems">
    /// The top items in the tweets within a Twitter search network, as a
    /// descriptive string, or null or an empty string if not available.
    /// </param>
    ///
    /// <param name="graphSummary">
    /// Where the graph summary gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph summary was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If a graph summary is available, it gets stored at <paramref
    /// name="graphSummary" /> and true is returned.  Otherwise, a message is
    /// shown and false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TrySummarizeGraph
    (
        GraphHistory graphHistory,
        AutoFillWorkbookResults autoFillWorkbookResults,
        OverallMetrics overallMetrics,
        String topNByMetrics,
        String twitterSearchNetworkTopItems,
        out String graphSummary
    )
    {
        Debug.Assert(graphHistory != null);
        Debug.Assert(autoFillWorkbookResults != null);

        graphSummary = SummarizeGraphInternal(graphHistory,
            autoFillWorkbookResults, overallMetrics, topNByMetrics,
            twitterSearchNetworkTopItems);

        return ( ShowWarningIfNoGraphSummary(graphSummary) );
    }

    //*************************************************************************
    //  Method: SummarizeGraph()
    //
    /// <summary>
    /// Gets a string that summarizes the graph, given a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook containing the graph.
    /// </param>
    ///
    /// <returns>
    /// A string that summarizes the graph.  If a summary isn't available, an
    /// empty string is returned.
    /// </returns>
    //*************************************************************************

    public static String
    SummarizeGraph
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        PerWorkbookSettings oPerWorkbookSettings =
            new PerWorkbookSettings(workbook);

        OverallMetrics oOverallMetrics;

        ( new OverallMetricsReader() ).TryReadMetrics(
            workbook, out oOverallMetrics);

        String sTopNByMetrics;

        ( new TopNByMetricsReader() ).TryReadMetrics(
            workbook, out sTopNByMetrics);

        String sTwitterSearchNetworkTopItems;

        ( new TwitterSearchNetworkTopItemsReader() ).TryReadMetrics(
            workbook, out sTwitterSearchNetworkTopItems);

        return ( GraphSummarizer.SummarizeGraphInternal(
            oPerWorkbookSettings.GraphHistory,
            oPerWorkbookSettings.AutoFillWorkbookResults, oOverallMetrics,
            sTopNByMetrics, sTwitterSearchNetworkTopItems) );
    }

    //*************************************************************************
    //  Method: SummarizeGraphInternal()
    //
    /// <summary>
    /// Gets a string that summarizes the graph, given various summary
    /// components.
    /// </summary>
    ///
    /// <param name="oGraphHistory">
    /// Stores attributes that describe how the graph was created.
    /// </param>
    ///
    /// <param name="oAutoFillWorkbookResults">
    /// Stores the results of a call to <see
    /// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
    /// </param>
    ///
    /// <param name="oOverallMetrics">
    /// Stores the overall metrics for a graph, or null if not available.
    /// </param>
    ///
    /// <param name="oTopNByMetrics">
    /// The graph's top-N-by metrics, as a descriptive string, or null if not
    /// available.
    /// </param>
    ///
    /// <param name="oTwitterSearchNetworkTopItems">
    /// The top items in the tweets within a Twitter search network, as a
    /// descriptive string, or null or an empty string if not available.
    /// </param>
    ///
    /// <returns>
    /// A string that summarizes the graph.  If a summary isn't available, an
    /// empty string is returned.
    /// </returns>
    //*************************************************************************

    private static String
    SummarizeGraphInternal
    (
        GraphHistory oGraphHistory,
        AutoFillWorkbookResults oAutoFillWorkbookResults,
        OverallMetrics oOverallMetrics,
        String oTopNByMetrics,
        String oTwitterSearchNetworkTopItems
    )
    {
        Debug.Assert(oGraphHistory != null);
        Debug.Assert(oAutoFillWorkbookResults != null);

        StringBuilder oStringBuilder = new StringBuilder();

        AppendGraphHistoryValues(oGraphHistory, oStringBuilder,
            GraphHistoryKeys.ImportDescription,
            GraphHistoryKeys.GraphDirectedness,
            GraphHistoryKeys.GroupingDescription,
            GraphHistoryKeys.LayoutAlgorithm
            );

        StringUtil.AppendLineAfterEmptyLine( oStringBuilder,
            oAutoFillWorkbookResults.ConvertToSummaryString() );

        if (oOverallMetrics != null)
        {
            StringUtil.AppendLineAfterEmptyLine(oStringBuilder,
                "Overall Graph Metrics:");

            oStringBuilder.AppendLine(
                oOverallMetrics.ConvertToSummaryString() );
        }

        StringUtil.AppendLineAfterEmptyLine(oStringBuilder, oTopNByMetrics);

        StringUtil.AppendLineAfterEmptyLine(oStringBuilder,
            oTwitterSearchNetworkTopItems);

        AppendGraphHistoryValues(oGraphHistory, oStringBuilder,
            GraphHistoryKeys.Comments
            );

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: AppendGraphHistoryValues()
    //
    /// <summary>
    /// Appends graph history values to the graph history if the values are
    /// available.
    /// </summary>
    ///
    /// <param name="oGraphHistory">
    /// Stores attributes that describe how the graph was created.
    /// </param>
    ///
    /// <param name="oStringBuilder">
    /// Object used to build the graph history.
    /// </param>
    ///
    /// <param name="asGraphHistoryKeys">
    /// Array of the keys for the values to append.
    /// </param>
    //*************************************************************************

    private static void
    AppendGraphHistoryValues
    (
        GraphHistory oGraphHistory,
        StringBuilder oStringBuilder,
        params String [] asGraphHistoryKeys
    )
    {
        Debug.Assert(oGraphHistory != null);
        Debug.Assert(oStringBuilder != null);
        Debug.Assert(asGraphHistoryKeys != null);

        String sGraphHistoryValue;

        foreach (String sGraphHistoryKey in asGraphHistoryKeys)
        {
            if ( oGraphHistory.TryGetValue(sGraphHistoryKey,
                out sGraphHistoryValue) )
            {
                StringUtil.AppendLineAfterEmptyLine(oStringBuilder,
                    sGraphHistoryValue);
            }
        }
    }

    //*************************************************************************
    //  Method: ShowWarningIfNoGraphSummary()
    //
    /// <summary>
    /// Shows a message if a graph summary isn't available.
    /// </summary>
    ///
    /// <param name="sGraphSummary">
    /// The graph summary.  Can be empty but not null.
    /// </param>
    ///
    /// <returns>
    /// true if the graph summary is not empty.
    /// </returns>
    //*************************************************************************

    private static Boolean
    ShowWarningIfNoGraphSummary
    (
        String sGraphSummary
    )
    {
        Debug.Assert(sGraphSummary != null);

        if (sGraphSummary.Length == 0)
        {
            FormUtil.ShowWarning("A graph summary is not available.");
            return (false);
        }

        return (true);
    }
}

}
