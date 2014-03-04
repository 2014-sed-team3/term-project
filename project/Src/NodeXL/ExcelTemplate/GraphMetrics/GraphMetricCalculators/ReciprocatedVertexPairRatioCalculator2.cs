
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ReciprocatedVertexPairRatioCalculator2
//
/// <summary>
/// Calculates the clustering coefficient for each of the graph's vertices.
/// </summary>
///
/// <remarks>
/// See <see cref="Algorithms.ReciprocatedVertexPairRatioCalculator" /> for
/// details on how reciprocated vertex pair ratios are calculated.
/// </remarks>
//*****************************************************************************

public class ReciprocatedVertexPairRatioCalculator2 :
    GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: ReciprocatedVertexPairRatioCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ReciprocatedVertexPairRatioCalculator2" /> class.
    /// </summary>
    //*************************************************************************

    public ReciprocatedVertexPairRatioCalculator2()
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
                GraphMetrics.ReciprocatedVertexPairRatio)
            )
        {
            return (true);
        }

        // Calculate the metric for each vertex using the
        // ReciprocatedVertexPairRatioCalculator class in the Algorithms
        // namespace, which knows nothing about Excel.

        Dictionary< Int32, Nullable<Double> > oReciprocatedVertexPairRatios;

        if ( !( new Algorithms.ReciprocatedVertexPairRatioCalculator() ).
            TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker,
                out oReciprocatedVertexPairRatios) )
        {
            // The user cancelled.

            return (false);
        }

        graphMetricColumns = new GraphMetricColumn [] {
            CreateGraphMetricColumn(graph, oReciprocatedVertexPairRatios)
            };

        return (true);
    }

    //*************************************************************************
    //  Method: CreateGraphMetricColumn()
    //
    /// <summary>
    /// Copies the calculated metrics to a new <see cref="GraphMetricColumn" />
    /// object.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oReciprocatedVertexPairRatios">
    /// The key is the IVertex.ID and the value is a Nullable Double that is
    /// the vertex's reciprocated vertex pair ratio, or null if it can't be
    /// calculated.
    /// </param>
    ///
    /// <returns>
    /// A new <see cref="GraphMetricColumn" /> object that contains the
    /// calculated metrics.
    /// </returns>
    //*************************************************************************

    protected GraphMetricColumn
    CreateGraphMetricColumn
    (
        IGraph oGraph,
        Dictionary< Int32, Nullable<Double> > oReciprocatedVertexPairRatios
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        List<GraphMetricValueWithID> oReciprocatedVertexPairRatioValues =
            new List<GraphMetricValueWithID>();

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            // Try to get the row ID stored in the worksheet.

            Int32 iRowID;

            if ( TryGetRowID(oVertex, out iRowID) )
            {
                oReciprocatedVertexPairRatioValues.Add(

                    new GraphMetricValueWithID(iRowID, 
                        NullableToGraphMetricValue(
                            oReciprocatedVertexPairRatios[oVertex.ID] )
                        ) );
            }
        }

        return ( new GraphMetricColumnWithID( WorksheetNames.Vertices,
            TableNames.Vertices,
            VertexTableColumnNames.ReciprocatedVertexPairRatio,
            ExcelTableUtil.AutoColumnWidth, null,
            CellStyleNames.GraphMetricGood,
            oReciprocatedVertexPairRatioValues.ToArray()
            ) );
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
