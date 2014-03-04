
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TopNByMetricCalculator2
//
/// <summary>
/// Calculates top-N-by metrics for the graph.
/// </summary>
///
/// <remarks>
/// This calculator will find, for example, the top 10 vertex names ranked by 
/// betweenness centrality, and the top 20 vertex names ranked by PageRank.
/// Its <see cref="IGraphMetricCalculator2.TryCalculateGraphMetrics" />
/// implementation returns multiple column pairs, one pair for each top-N-by
/// table that needs to be written to the top-N-by worksheet.
///
/// <para>
/// This graph metric calculator differs from most other calculators in that it
/// reads arbitrary columns in the Excel workbook.  The other calculators look
/// only at how the graph's vertices are connected to each other.  Therefore,
/// there is no corresponding lower-level TopNByMetricCalculator class in the
/// <see cref="Smrf.NodeXL.Algorithms" /> namespace, and the top-N-by metrics
/// cannot be calculated outside of this ExcelTemplate project.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TopNByMetricCalculator2 : TopItemsCalculatorBase2
{
    //*************************************************************************
    //  Constructor: TopNByMetricCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TopNByMetricCalculator2" /> class.
    /// </summary>
    //*************************************************************************

    public TopNByMetricCalculator2()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricToCalculate
    //
    /// <summary>
    /// Gets the graph metric that will be calculated.
    /// </summary>
    ///
    /// <value>
    /// The graph metric that will be calculated, as a <see
    /// cref="GraphMetrics" /> flag.
    /// </value>
    //*************************************************************************

    protected override GraphMetrics
    GraphMetricToCalculate
    {
        get
        {
            AssertValid();

            return (GraphMetrics.TopNBy);
        }
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetricsInternal()
    //
    /// <summary>
    /// Attempts to calculate metrics for the entire List of
    /// TopNByMetricUserSettings objects.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that gets populated by this
    /// method if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected override Boolean
    TryCalculateGraphMetricsInternal
    (
        IGraph oGraph,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        AssertValid();

        Int32 iTopNByMetricsIndex = 0;

        foreach (TopNByMetricUserSettings oTopNByMetricUserSettings in
            oCalculateGraphMetricsContext.GraphMetricUserSettings.
                TopNByMetricsToCalculate)
        {
            if (oCalculateGraphMetricsContext.BackgroundWorker
                .CancellationPending)
            {
                // The user cancelled.

                return (false);
            }

            if (
                oTopNByMetricUserSettings.WorksheetName !=
                    WorksheetNames.Vertices
                ||
                oTopNByMetricUserSettings.TableName != TableNames.Vertices
                ||
                oTopNByMetricUserSettings.ItemNameColumnName !=
                    VertexTableColumnNames.VertexName
                )
            {
                // This currently works only for the vertex worksheet.
                // GraphMetricCalculationManager.ReadWorkbook() will have to be
                // modified to read the cell values in other worksheets when
                // top-N-by metrics are extended to support other worksheets.

                throw new NotSupportedException();
            }

            AddColumnsForRankedVertices(oGraph.Vertices,
                oTopNByMetricUserSettings.RankedColumnName,
                oTopNByMetricUserSettings.N,
                WorksheetNames.TopNByMetrics,
                TableNames.TopNByMetricsRoot + (iTopNByMetricsIndex + 1),
                oTopNByMetricUserSettings.ToString(),
                oTopNByMetricUserSettings.RankedColumnName,
                oGraphMetricColumns);

            iTopNByMetricsIndex++;
        }

        return (true);
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
