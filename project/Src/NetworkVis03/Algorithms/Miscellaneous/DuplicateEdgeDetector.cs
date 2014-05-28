
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: DuplicateEdgeDetector
//
/// <summary>
/// Counts duplicate and unique edges in a graph.
/// </summary>
///
/// <remarks>
/// The <see cref="IIdentityProvider.Name" /> property on each of an
/// edge's vertices is used to test for duplicate edges.
///
/// <para>
/// In a directed graph, (A,B) and (A,B) are considered duplicates.  (A,B) and
/// (B,A) are not duplicates.
/// </para>
///
/// <para>
/// In an undirected graph, (A,B) and (A,B) are considered duplicates.  (A,B)
/// and (B,A) are also duplicates.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DuplicateEdgeDetector
{
    //*************************************************************************
    //  Constructor: DuplicateEdgeDetector()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEdgeDetector" />
    /// class.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to check.
    /// </param>
    //*************************************************************************

    public DuplicateEdgeDetector
    (
        IGraph graph
    )
    {
        m_oGraph = graph;

        m_bEdgesCounted = false;
        m_iUniqueEdges = Int32.MinValue;
        m_iEdgesWithDuplicates = Int32.MinValue;
        m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops = Int32.MinValue;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphContainsDuplicateEdges()
    //
    /// <summary>
    /// Gets a flag indicating whether the graph contains at least one pair
    /// of duplicate edges.
    /// </summary>
    ///
    /// <value>
    /// true if the graph contains at least one pair of duplicate edges.
    /// </value>
    //*************************************************************************

    public Boolean
    GraphContainsDuplicateEdges
    {
        get
        {
            AssertValid();

            // Count the edges and cache the results if they haven't already
            // been counted.

            CountEdges();

            return (m_iEdgesWithDuplicates > 0);
        }
    }

    //*************************************************************************
    //  Property: EdgesWithDuplicates()
    //
    /// <summary>
    /// Gets the number of edges in the graph that have duplicates.
    /// </summary>
    ///
    /// <value>
    /// The number of edges in the graph that have duplicates.
    /// </value>
    //*************************************************************************

    public Int32
    EdgesWithDuplicates
    {
        get
        {
            AssertValid();

            // Count the edges and cache the results if they haven't already
            // been counted.

            CountEdges();

            return (m_iEdgesWithDuplicates);
        }
    }

    //*************************************************************************
    //  Property: UniqueEdges()
    //
    /// <summary>
    /// Gets the number of unique edges in the graph.
    /// </summary>
    ///
    /// <value>
    /// The number of unique edges in the graph.
    /// </value>
    //*************************************************************************

    public Int32
    UniqueEdges
    {
        get
        {
            AssertValid();

            // Count the edges and cache the results if they haven't already
            // been counted.

            CountEdges();

            return (m_iUniqueEdges);
        }
    }

    //*************************************************************************
    //  Property: TotalEdgesAfterMergingDuplicatesNoSelfLoops()
    //
    /// <summary>
    /// Gets the total number of edges the graph would have if its duplicate
    /// edges were merged and all self-loops were removed.
    /// </summary>
    ///
    /// <value>
    /// The total number of edges the graph would have if its duplicate edges
    /// were merged and all self-loops were removed.
    /// </value>
    ///
    /// <remarks>
    /// This class does not actually merge duplicate edges or remove
    /// self-loops.
    /// </remarks>
    //*************************************************************************

    public Int32
    TotalEdgesAfterMergingDuplicatesNoSelfLoops
    {
        get
        {
            AssertValid();

            // Count the edges and cache the results if they haven't already
            // been counted.

            CountEdges();

            return (m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops);
        }
    }

    //*************************************************************************
    //  Method: CountEdges()
    //
    /// <summary>
    /// Counts the edges and cache the results if they haven't already been
    /// counted.
    /// </summary>
    //*************************************************************************

    protected void
    CountEdges()
    {
        AssertValid();

        if (m_bEdgesCounted)
        {
            return;
        }

        m_iUniqueEdges = 0;
        m_iEdgesWithDuplicates = 0;

        IEdgeCollection oEdges = m_oGraph.Edges;

        Boolean bGraphIsDirected =
            (m_oGraph.Directedness == GraphDirectedness.Directed);

        // Create a dictionary of vertex ID pairs.  The key is the vertex ID
        // pair and the value is true if the edge has duplicates or false if it
        // doesn't.

        Dictionary <Int64, Boolean> oVertexIDPairs =
            new Dictionary<Int64, Boolean>(oEdges.Count);

        foreach (IEdge oEdge in oEdges)
        {
            Int64 i64VertexIDPair = EdgeUtil.GetVertexIDPair(oEdge);
            Boolean bEdgeHasDuplicate;

            if ( oVertexIDPairs.TryGetValue(i64VertexIDPair,
                out bEdgeHasDuplicate) )
            {
                if (!bEdgeHasDuplicate)
                {
                    // This is the edge's first duplicate.

                    m_iUniqueEdges--;
                    m_iEdgesWithDuplicates++;

                    oVertexIDPairs[i64VertexIDPair] = true;
                }

                m_iEdgesWithDuplicates++;
            }
            else
            {
                m_iUniqueEdges++;

                oVertexIDPairs.Add(i64VertexIDPair, false);
            }
        }

        m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops = 0;

        foreach (Int64 i64VertexIDPair in oVertexIDPairs.Keys)
        {
            Int32 iVertexID1 = (Int32)(i64VertexIDPair >> 32);
            Int32 iVertexID2 = (Int32)i64VertexIDPair;

            if (iVertexID1 != iVertexID2)
            {
                m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops++;
            }
        }

        m_bEdgesCounted = true;

        AssertValid();
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
        Debug.Assert(m_oGraph != null);

        if (m_bEdgesCounted)
        {
            Debug.Assert(m_iUniqueEdges >= 0);
            Debug.Assert(m_iEdgesWithDuplicates >= 0);
            Debug.Assert(m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops >= 0);
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph to check.

    protected IGraph m_oGraph;

    /// true if the edges have already been counted.

    protected Boolean m_bEdgesCounted;

    /// If m_bEdgesCounted is true, this is the number of unique edges in
    /// m_oGraph.

    protected Int32 m_iUniqueEdges;

    /// If m_bEdgesCounted is true, this is the number of edges in m_oGraph
    /// that have duplicates.

    protected Int32 m_iEdgesWithDuplicates;

    /// If m_bEdgesCounted is true, this is the number of edges that would be
    /// in m_oGraph if its duplicate edges were merged and all self-loops were
    /// removed.

    protected Int32 m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops;
}

}
