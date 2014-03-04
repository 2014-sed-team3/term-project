
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeReciprocationCalculator2
//
/// <summary>
/// Calculates whether the edges in a connected graph are reciprocated.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.EdgeReciprocationCalculator" /> for details on
/// how the metrics are calculated.
/// </remarks>
//*****************************************************************************

public class EdgeReciprocationCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: EdgeReciprocationCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EdgeReciprocationCalculator2" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeReciprocationCalculator2()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="calculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="graphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated or don't need to be
    /// calculated, false if the user wants to cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method periodically checks BackgroundWorker.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// immediately returns false.
    ///
    /// <para>
    /// It also periodically reports progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// userState argument is a <see cref="GraphMetricProgress" /> object.
    /// </para>
    ///
    /// <para>
    /// Calculated metrics for hidden rows are ignored by the caller, because
    /// Excel misbehaves when values are written to hidden cells.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public override Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out GraphMetricColumn [] graphMetricColumns
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(calculateGraphMetricsContext != null);
        AssertValid();

        graphMetricColumns = new GraphMetricColumn[0];

        if (
            graph.Directedness != GraphDirectedness.Directed
            ||
            !calculateGraphMetricsContext.ShouldCalculateGraphMetrics(
                GraphMetrics.EdgeReciprocation)
            )
        {
            return (true);
        }

        // Calculate the reciprocated flag for each edge using the
        // EdgeReciprocationCalculator class in the Algorithms namespace, which
        // knows nothing about Excel.

        Dictionary<Int32, Boolean> oReciprocatedFlags;

        if ( !( new Algorithms.EdgeReciprocationCalculator() ).
            TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker,
                out oReciprocatedFlags) )
        {
            // The user cancelled.

            return (false);
        }

        // Transfer the flags to an array of GraphMetricValue objects.

        List<GraphMetricValueWithID> oReciprocatedValues =
            new List<GraphMetricValueWithID>();

        BooleanConverter oBooleanConverter = new BooleanConverter();

        foreach (IEdge oEdge in graph.Edges)
        {
            // Try to get the row ID stored in the worksheet.

            Int32 iRowID;

            if ( TryGetRowID(oEdge, out iRowID) )
            {
                oReciprocatedValues.Add(
                    new GraphMetricValueWithID(iRowID,
                        oBooleanConverter.GraphToWorkbook(
                        oReciprocatedFlags[oEdge.ID] )
                    ) );
            }
        }

        graphMetricColumns = new GraphMetricColumn [] {

            new GraphMetricColumnWithID( WorksheetNames.Edges,
                TableNames.Edges,
                EdgeTableColumnNames.IsReciprocated,
                ExcelTableUtil.AutoColumnWidth, null,
                CellStyleNames.GraphMetricGood,
                oReciprocatedValues.ToArray()
                ),
                };

        return (true);
    }

    //*************************************************************************
    //  Property: HandlesDuplicateEdges
    //
    /// <summary>
    /// Gets a flag indicating whether duplicate edges are properly handled.
    /// </summary>
    ///
    /// <value>
    /// true if the graph metric calculator handles duplicate edges, false if
    /// duplicate edges should be removed from the graph before the
    /// calculator's <see cref="TryCalculateGraphMetrics" /> method is called.
    /// </value>
    //*************************************************************************

    public override Boolean
    HandlesDuplicateEdges
    {
        get
        {
            AssertValid();

            // Duplicate edges do not cause a problem.

            return (true);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
