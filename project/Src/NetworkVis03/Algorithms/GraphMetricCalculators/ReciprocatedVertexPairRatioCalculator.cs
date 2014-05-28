
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: ReciprocatedVertexPairRatioCalculator
//
/// <summary>
/// Calculates the reciprocated vertex pair ratio for each of the graph's
/// vertices.
/// </summary>
///
/// <remarks>
/// The reciprocated vertex pair ratio for a vertex in a directed graph is the
/// number of adjacent vertices the vertex is connected to with edges in both
/// directions, divided by the number of adjacent vertices.  It is not
/// calculated for an undirected graph.
/// 
/// <para>
/// The calculations can handle self-loops, but they are rendered invalid if
/// the graph has duplicate edges.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ReciprocatedVertexPairRatioCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: ReciprocatedVertexPairRatioCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ReciprocatedVertexPairRatioCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public ReciprocatedVertexPairRatioCalculator()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricDescription
    //
    /// <summary>
    /// Gets a description of the graph metrics calculated by the
    /// implementation.
    /// </summary>
    ///
    /// <value>
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </value>
    //*************************************************************************

    public override String
    GraphMetricDescription
    {
        get
        {
            AssertValid();

            return ("reciprocated vertex pair ratios");
        }
    }

    //*************************************************************************
    //  Method: CalculateGraphMetrics()
    //
    /// <summary>
    /// Calculate the graph metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <returns>
    /// The graph metrics.  There is one key/value pair for each vertex in the
    /// graph.  The key is the IVertex.ID and the value is a Nullable Double
    /// that is the vertex's reciprocated vertex pair ratio, or null if it
    /// can't be calculated.
    /// </returns>
    //*************************************************************************

    public Dictionary< Int32, Nullable<Double> >
    CalculateGraphMetrics
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        Dictionary< Int32, Nullable<Double> > oGraphMetrics;

        TryCalculateGraphMetrics(graph, null, out oGraphMetrics);

        return (oGraphMetrics);
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate the graph metrics while optionally running on a
    /// background thread.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="graphMetrics">
    /// Where the graph metrics get stored if true is returned.  There is one
    /// key/value pair for each vertex in the graph.  The key is the IVertex.ID
    /// and the value is a Nullable Double that is the vertex's reciprocated
    /// vertex pair ratio, or null if it can't be calculated.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        BackgroundWorker backgroundWorker,
        out Dictionary< Int32, Nullable<Double> > graphMetrics
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        IVertexCollection oVertices = graph.Vertices;
        Int32 iVertices = oVertices.Count;
        Int32 iCalculations = 0;

        // The key is an IVertex.ID and the value is the vertex's reciprocated
        // vertex pair ratio, or null.

        Dictionary< Int32, Nullable<Double> > oReciprocatedVertexPairRatios =
            new Dictionary< Int32, Nullable<Double> >(oVertices.Count);

        graphMetrics = oReciprocatedVertexPairRatios;

        if (graph.Directedness == GraphDirectedness.Directed)
        {
            // Contains a key for each of the graph's unique edges.  The key is
            // the edge's ordered vertex ID pair.

            HashSet<Int64> oVertexIDPairs = GetVertexIDPairs(graph);

            foreach (IVertex oVertex in oVertices)
            {
                // Check for cancellation and report progress every
                // VerticesPerProgressReport calculations.

                if (
                    (iCalculations % VerticesPerProgressReport == 0)
                    &&
                    !ReportProgressAndCheckCancellationPending(
                        iCalculations, iVertices, backgroundWorker)
                    )
                {
                    return (false);
                }

                oReciprocatedVertexPairRatios.Add(oVertex.ID,
                    CalculateReciprocatedVertexPairRatio(
                        oVertex, oVertexIDPairs) );

                iCalculations++;
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetVertexIDPairs()
    //
    /// <summary>
    /// Gets a HashSet that contains the ordered pair of the vertex IDs for the
    /// graph's edges.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <returns>
    /// A HashSet that contains one key for each of the graph's unique,
    /// non-self-loop edges.  The key is the edge's ordered vertex ID pair.
    /// </returns>
    //*************************************************************************

    protected HashSet<Int64>
    GetVertexIDPairs
    (
        IGraph oGraph
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        HashSet<Int64> oVertexIDPairs = new HashSet<Int64>();

        foreach (IEdge oEdge in oGraph.Edges)
        {
            if (!oEdge.IsSelfLoop)
            {
                oVertexIDPairs.Add( CollectionUtil.GetDictionaryKey(
                    oEdge.Vertex1.ID, oEdge.Vertex2.ID, true) );
            }
        }

        return (oVertexIDPairs);
    }

    //*************************************************************************
    //  Method: CalculateReciprocatedVertexPairRatio()
    //
    /// <summary>
    /// Calculates the reciprocated vertex pair ratio for one vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to calculate the ratio for.
    /// </param>
    ///
    /// <param name="oVertexIDPairs">
    /// Contains a key for each of the graph's unique edges.  The key is the
    /// edge's ordered vertex ID pair.
    /// </param>
    ///
    /// <returns>
    /// The reciprocated vertex pair ratio, or null if it can't be calculated.
    /// </returns>
    //*************************************************************************

    protected Nullable<Double>
    CalculateReciprocatedVertexPairRatio
    (
        IVertex oVertex,
        HashSet<Int64> oVertexIDPairs
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oVertexIDPairs != null);
        AssertValid();

        Int32 iAdjacentVerticesWithBothEdges = 0;
        Int32 iAdjacentVertices = 0;

        foreach (IVertex oAdjacentVertex in oVertex.AdjacentVertices)
        {
            // Skip self-loops.

            if (oAdjacentVertex != oVertex)
            {
                iAdjacentVertices++;

                // Check for edges in both directions.

                if (
                    oVertexIDPairs.Contains( CollectionUtil.GetDictionaryKey(
                        oVertex.ID, oAdjacentVertex.ID, true) )
                    &&
                    oVertexIDPairs.Contains( CollectionUtil.GetDictionaryKey(
                        oAdjacentVertex.ID, oVertex.ID, true) )
                    )
                {
                    iAdjacentVerticesWithBothEdges++;
                }
            }
        }

        if (iAdjacentVertices == 0)
        {
            return (null);
        }

        return ( (Double)iAdjacentVerticesWithBothEdges /
            (Double)iAdjacentVertices );
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
    //  Protected constants
    //*************************************************************************

    /// Number of vertices that are processed before progress is reported and
    /// the cancellation flag is checked.

    protected const Int32 VerticesPerProgressReport = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
