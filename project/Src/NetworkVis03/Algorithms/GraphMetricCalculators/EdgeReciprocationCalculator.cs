
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: EdgeReciprocationCalculator
//
/// <summary>
/// Calculates whether the edges in a connected graph are reciprocated.
/// </summary>
///
/// <remarks>
/// An edge (A,B) in a directed graph is reciprocated if the graph also has an
/// edge (B,A).  Reciprocation has no meaning for an undirected graph.
/// </remarks>
//*****************************************************************************

public class EdgeReciprocationCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: EdgeReciprocationCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EdgeReciprocationCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeReciprocationCalculator()
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

            return ("edge reciprocation");
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
    /// The graph metrics.  There is one key/value pair for each edge in the
    /// graph.  The key is the IEdge.ID and the value is a Boolean that
    /// indicates whether the edge is reciprocated.
    /// </returns>
    //*************************************************************************

    public Dictionary<Int32, Boolean>
    CalculateGraphMetrics
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        Dictionary<Int32, Boolean> oGraphMetrics;

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
    /// key/value pair for each edge in the graph.  The key is the IEdge.ID and
    /// the value is a Boolean that indicates whether the edge is reciprocated.
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
        out Dictionary<Int32, Boolean> graphMetrics
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        IEdgeCollection oEdges = graph.Edges;

        // The key is an IEdge.ID and the value is a Boolean that indicates
        // whether the edge is reciprocated.

        Dictionary<Int32, Boolean> oReciprocationFlags =
            new Dictionary<Int32, Boolean>(oEdges.Count);

        graphMetrics = oReciprocationFlags;

        // The key is the combined IDs of the edge's vertices.

        HashSet<Int64> oVertexIDPairs = new HashSet<Int64>();

        if (graph.Directedness == GraphDirectedness.Directed)
        {
            foreach (IEdge oEdge in oEdges)
            {
                oVertexIDPairs.Add( GetDictionaryKey(
                    oEdge.Vertex1, oEdge.Vertex2) );
            }

            if ( !ReportProgressAndCheckCancellationPending(
                1, 2, backgroundWorker) )
            {
                return (false);
            }

            foreach (IEdge oEdge in oEdges)
            {
                Boolean bEdgeIsReciprocated = false;

                if (!oEdge.IsSelfLoop)
                {
                    Int64 i64ReversedVerticesKey = GetDictionaryKey(
                        oEdge.Vertex2, oEdge.Vertex1);

                    bEdgeIsReciprocated =
                        oVertexIDPairs.Contains(i64ReversedVerticesKey);
                }

                oReciprocationFlags.Add(oEdge.ID, bEdgeIsReciprocated);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetDictionaryKey()
    //
    /// <summary>
    /// Combines two unique vertex IDs into an Int64 suitable for use as a
    /// dictionary key.
    /// account.
    /// </summary>
    ///
    /// <param name="oVertex1">
    /// The first vertex.
    /// </param>
    ///
    /// <param name="oVertex2">
    /// The second vertex.
    /// </param>
    ///
    /// <returns>
    /// An Int64 suitable for use as a dictionary key.
    /// </returns>
    ///
    /// <remarks>
    /// The vertex pairs (A,B) and (B,A) yield different Int64s.
    /// </remarks>
    //*************************************************************************

    protected Int64
    GetDictionaryKey
    (
        IVertex oVertex1,
        IVertex oVertex2
    )
    {
        Debug.Assert(oVertex1 != null);
        Debug.Assert(oVertex2 != null);
        AssertValid();

        return ( CollectionUtil.GetDictionaryKey(oVertex1.ID, oVertex2.ID,
            true) );
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
