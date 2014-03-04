
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: OverallReciprocationCalculator
//
/// <summary>
/// Calculates the reciprocated vertex pair ratio and the reciprocated edge
/// pair ratio for the graph.
/// </summary>
///
/// <remarks>
/// The calculations skip all self-loops and duplicate edges, which would
/// render the ratios invalid.
/// </remarks>
//*****************************************************************************

public class OverallReciprocationCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: OverallReciprocationCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OverallReciprocationCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public OverallReciprocationCalculator()
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

            return ("overall reciprocation ratios");
        }
    }

    //*************************************************************************
    //  Method: CalculateGraphMetrics()
    //
    /// <summary>
    /// Calculates the graph metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="reciprocatedVertexPairRatio">
    /// This gets set to the ratio of the number of vertex pairs that are
    /// connected with directed edges in both directions to the number of
    /// vertex pairs that are connected with any directed edge, or to null if
    /// not available.
    /// </param>
    ///
    /// <param name="reciprocatedEdgeRatio">
    /// This gets set to the ratio of directed edges that have a reciprocated
    /// edge to the total number of directed edges, or to null if not
    /// available.
    /// </param>
    //*************************************************************************

    public void
    CalculateGraphMetrics
    (
        IGraph graph,
        out Nullable<Double> reciprocatedVertexPairRatio,
        out Nullable<Double> reciprocatedEdgeRatio
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        TryCalculateGraphMetrics(graph, null, out reciprocatedVertexPairRatio,
            out reciprocatedEdgeRatio);
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
    /// <param name="reciprocatedVertexPairRatio">
    /// This gets set if true is returned.  It is the ratio of the number of
    /// vertex pairs that are connected with directed edges in both directions
    /// to the number of vertex pairs that are connected with any directed
    /// edge, or null if not available.
    /// </param>
    ///
    /// <param name="reciprocatedEdgeRatio">
    /// This gets set if true is returned.  It is the ratio of directed edges
    /// that have a reciprocated edge to the total number of directed edges, or
    /// null if not available.
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
        out Nullable<Double> reciprocatedVertexPairRatio,
        out Nullable<Double> reciprocatedEdgeRatio
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        if (graph.Directedness != GraphDirectedness.Directed)
        {
            reciprocatedVertexPairRatio = reciprocatedEdgeRatio = null;
        }
        else
        {
            HashSet<Int64> oVertexIDPairsOrdered, oVertexIDPairsUnordered;

            CountEdges(graph, out oVertexIDPairsOrdered,
                out oVertexIDPairsUnordered);

            Int32 iVertexPairsWithBothDirectedEdges =
                CountVertexPairsWithBothDirectedEdges(
                    graph, oVertexIDPairsOrdered);

            reciprocatedVertexPairRatio = CalculateReciprocatedVertexPairRatio(
                graph, iVertexPairsWithBothDirectedEdges,
                oVertexIDPairsUnordered);

            reciprocatedEdgeRatio = CalculateReciprocatedEdgeRatio(
                graph, iVertexPairsWithBothDirectedEdges);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: CountEdges()
    //
    /// <summary>
    /// Counts edges in the graph.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.  It must be directed.
    /// </param>
    ///
    /// <param name="oVertexIDPairsOrdered">
    /// When the method returns, this will contain the unique concatenations of
    /// the vertex IDs for edges, where the order of vertex IDs is taken into
    /// account.  If there are duplicate edges, they will get added to the
    /// HashSet once only.
    /// </param>
    ///
    /// <param name="oVertexIDPairsUnordered">
    /// When the method returns, this will contain the unique concatenations of
    /// the vertex IDs for edges, where the order of vertex IDs is NOT taken
    /// into account.  If there are duplicate edges, they will get added to the
    /// HashSet once only.
    /// </param>
    ///
    /// <remarks>
    /// This method ignores self-loops and duplicate edges.
    /// </remarks>
    //*************************************************************************

    protected void
    CountEdges
    (
        IGraph oGraph,
        out HashSet<Int64> oVertexIDPairsOrdered,
        out HashSet<Int64> oVertexIDPairsUnordered
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraph.Directedness == GraphDirectedness.Directed);
        AssertValid();

        oVertexIDPairsOrdered = new HashSet<Int64>();
        oVertexIDPairsUnordered = new HashSet<Int64>();

        foreach (IEdge oEdge in oGraph.Edges)
        {
            if (!oEdge.IsSelfLoop)
            {
                Int32 iVertex1ID = oEdge.Vertex1.ID;
                Int32 iVertex2ID = oEdge.Vertex2.ID;

                oVertexIDPairsOrdered.Add( CollectionUtil.GetDictionaryKey(
                    iVertex1ID, iVertex2ID, true) );

                oVertexIDPairsUnordered.Add( CollectionUtil.GetDictionaryKey(
                    iVertex1ID, iVertex2ID, false) );
            }
        }
    }

    //*************************************************************************
    //  Method: CountVertexPairsWithBothDirectedEdges()
    //
    /// <summary>
    /// Counts the vertex pairs that have edges in both directions.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.  It must be directed.
    /// </param>
    ///
    /// <param name="oVertexIDPairsOrdered">
    /// The unique concatenations of the vertex IDs for edges, where the order
    /// of vertex IDs is taken into account.  If there are duplicate edges,
    /// they exist in the HashSet once only.
    /// </param>
    ///
    /// <returns>
    /// The number of vertex pairs that have edges in both directions.
    /// </returns>
    //*************************************************************************

    protected Int32
    CountVertexPairsWithBothDirectedEdges
    (
        IGraph oGraph,
        HashSet<Int64> oVertexIDPairsOrdered
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraph.Directedness == GraphDirectedness.Directed);
        Debug.Assert(oVertexIDPairsOrdered != null);
        AssertValid();

        // Note that each vertex pair connected with both directed edges will
        // get counted twice here.

        Int32 iVertexPairsWithBothDirectedEdges = 0;

        foreach (Int64 oVertexIDPairOrdered in oVertexIDPairsOrdered)
        {
            // Check whether the HashSet contains a vertex pair with the order
            // of the vertex IDs reversed.

            Int32 iVertex1ID, iVertex2ID;

            CollectionUtil.ParseDictionaryKey(oVertexIDPairOrdered,
                out iVertex1ID, out iVertex2ID);

            if ( oVertexIDPairsOrdered.Contains(
                CollectionUtil.GetDictionaryKey(
                    iVertex2ID, iVertex1ID, true) ) )
            {
                iVertexPairsWithBothDirectedEdges++;
            }
        }

        Debug.Assert(iVertexPairsWithBothDirectedEdges % 2 == 0);

        return (iVertexPairsWithBothDirectedEdges / 2);
    }

    //*************************************************************************
    //  Method: CalculateReciprocatedVertexPairRatio()
    //
    /// <summary>
    /// Calculates the graph's reciprocated vertex pair ratio.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.  It must be directed.
    /// </param>
    ///
    /// <param name="iVertexPairsWithBothDirectedEdges">
    /// The number of vertex pairs that have edges in both directions.
    /// </param>
    ///
    /// <param name="oVertexIDPairsUnordered">
    /// The unique concatenations of the vertex IDs for edges, where the order
    /// of vertex IDs is NOT taken into account.  If there are duplicate edges,
    /// they exist in the HashSet once only.
    /// </param>
    ///
    /// <returns>
    /// The graph's reciprocated vertex pair ratio, or null if it can't be
    /// calculated.
    /// </returns>
    //*************************************************************************

    protected Nullable<Double>
    CalculateReciprocatedVertexPairRatio
    (
        IGraph oGraph,
        Int32 iVertexPairsWithBothDirectedEdges,
        HashSet<Int64> oVertexIDPairsUnordered
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraph.Directedness == GraphDirectedness.Directed);
        Debug.Assert(iVertexPairsWithBothDirectedEdges >= 0);
        Debug.Assert(oVertexIDPairsUnordered != null);
        AssertValid();

        // Definition, from Derek Hansen in July 2012:
        //
        // # of diads with both directed edges /
        // # of diads with at least one directed edge
        //
        // Equivalent definition, from http://
        // faculty.ucr.edu/~hanneman/nettext/C8_Embedding.html#reciprocity
        //
        // "...analysts are concerned with the ratio of the number of pairs
        // with a reciprocated tie relative to the number of pairs with any
        // tie."

        Int32 iVertexPairsWithAtLeastOneDirectedEdge =
            oVertexIDPairsUnordered.Count;

        if (iVertexPairsWithAtLeastOneDirectedEdge == 0)
        {
            return (null);
        }

        return ( (Double)iVertexPairsWithBothDirectedEdges /
            (Double)iVertexPairsWithAtLeastOneDirectedEdge);
    }

    //*************************************************************************
    //  Method: CalculateReciprocatedEdgeRatio()
    //
    /// <summary>
    /// Calculates the graph's reciprocated edge ratio.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.  It must be directed.
    /// </param>
    ///
    /// <param name="iVertexPairsWithBothDirectedEdges">
    /// The number of vertex pairs that have edges in both directions.
    /// </param>
    ///
    /// <returns>
    /// The graph's reciprocated edge ratio, or null if it can't be calculated.
    /// </returns>
    //*************************************************************************

    protected Nullable<Double>
    CalculateReciprocatedEdgeRatio
    (
        IGraph oGraph,
        Int32 iVertexPairsWithBothDirectedEdges
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraph.Directedness == GraphDirectedness.Directed);
        Debug.Assert(iVertexPairsWithBothDirectedEdges >= 0);
        AssertValid();

        // Definition, from Derek Hansen in July 2012:
        //
        // # of edges that have a reciprocated "pair" / total number of edges
        //
        // Equivalent definition, from http://
        // faculty.ucr.edu/~hanneman/nettext/C8_Embedding.html#reciprocity
        //
        // "...the number of ties that are involved in reciprocal relations
        // relative to the total number of actual ties (not possible ties)."

        Int32 iTotalEdgesAfterMergingDupcliatesNoSelfLoops =
            new DuplicateEdgeDetector(oGraph)
            .TotalEdgesAfterMergingDuplicatesNoSelfLoops;

        if (iTotalEdgesAfterMergingDupcliatesNoSelfLoops == 0)
        {
            return (null);
        }

        return ( (Double)iVertexPairsWithBothDirectedEdges * 2.0 /
            (Double)iTotalEdgesAfterMergingDupcliatesNoSelfLoops );
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
