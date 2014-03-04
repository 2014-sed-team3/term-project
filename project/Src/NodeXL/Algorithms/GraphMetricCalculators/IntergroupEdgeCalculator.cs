
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: IntergroupEdgeCalculator
//
/// <summary>
/// Given a collection of vertex groups, this class counts the number of edges
/// and the sum of edge weights for the edges between pairs of groups, and for
/// the edges within each group.
/// </summary>
//*****************************************************************************

public class IntergroupEdgeCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Constructor: IntergroupEdgeCalculator()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="IntergroupEdgeCalculator" /> class.
    /// </summary>
    //*************************************************************************

    public IntergroupEdgeCalculator()
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

            return ("group edges");
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
    /// <param name="groups">
    /// A collection of <see cref="GroupInfo" /> objects, one for each group.
    /// </param>
    ///
    /// <param name="useDirectedness">
    /// true if the graph's directedness should be taken into account.
    /// </param>
    ///
    /// <returns>
    /// The graph metrics.  There is one <see cref="IntergroupEdgeInfo" />
    /// object in the collection for each pair of groups in <paramref
    /// name="groups" /> that have edges between them, and one object for each
    /// group that has edges within it.  Pairs of groups that do not have edges
    /// between them are not included in the collection, nor are groups that do
    /// not have edges within them.
    /// </returns>
    //*************************************************************************

    public IList<IntergroupEdgeInfo>
    CalculateGraphMetrics
    (
        IGraph graph,
        IList<GroupInfo> groups,
        Boolean useDirectedness
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(groups != null);
        AssertValid();

        IList<IntergroupEdgeInfo> oGraphMetrics;

        TryCalculateGraphMetrics(graph, null, groups, useDirectedness,
            out oGraphMetrics);

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
    /// <param name="groups">
    /// A collection of <see cref="GroupInfo" /> objects, one for each group.
    /// </param>
    ///
    /// <param name="useDirectedness">
    /// true if the graph's directedness should be taken into account.
    /// </param>
    ///
    /// <param name="graphMetrics">
    /// Where the graph metrics get stored if true is returned.  There is one
    /// <see cref="IntergroupEdgeInfo" /> object in the collection for each
    /// pair of groups in <paramref name="groups" /> that have edges between
    /// them, and one object for each group that has edges within it.  Pairs of
    /// groups that do not have edges between them are not included in the
    /// collection, nor are groups that do not have edges within them.  The
    /// collection is sorted first by <see
    /// cref="IntergroupEdgeInfo.Group1Index" />, then by <see
    /// cref="IntergroupEdgeInfo.Group2Index" />.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="useDirectedness" /> is false or the graph is
    /// undirected, all edges between groups A and B are returned in a single
    /// <see cref="IntergroupEdgeInfo" /> object.
    ///
    /// <para>
    /// If <paramref name="useDirectedness" /> is true and the graph is
    /// directed, edges from group A to group B are returned in one <see
    /// cref="IntergroupEdgeInfo" /> object, and edges from group B to group A
    /// are returned in another <see cref="IntergroupEdgeInfo" /> object.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        BackgroundWorker backgroundWorker,
        IList<GroupInfo> groups,
        Boolean useDirectedness,
        out IList<IntergroupEdgeInfo> graphMetrics
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(groups != null);
        AssertValid();

        List<IntergroupEdgeInfo> oIntergroupEdges =
            new List<IntergroupEdgeInfo>();

        graphMetrics = oIntergroupEdges;

        Int32 iGroups = groups.Count;

        // The key is an IVertex.ID and the value is the zero-based index of
        // the group the vertex belongs to.

        Dictionary<Int32, Int32> oGroupIndexDictionary =
            GroupUtil.GetGroupIndexDictionary(groups);

        Boolean bGraphIsDirected =
            (graph.Directedness == GraphDirectedness.Directed);

        for (Int32 iGroup1Index = 0; iGroup1Index < iGroups; iGroup1Index++)
        {
            if ( !ReportProgressAndCheckCancellationPending(
                iGroup1Index, iGroups, backgroundWorker) )
            {
                return (false);
            }

            GroupInfo oGroup1 = groups[iGroup1Index];

            // The key is the index of another group and the value is the
            // IntergroupEdgeInfo object that counts all of the edges from
            // group 1 to that other group.

            Dictionary<Int32, IntergroupEdgeInfo> oIntergroupEdgeIndexes =
                new Dictionary<Int32, IntergroupEdgeInfo>();

            foreach (IVertex oVertexInGroup1 in oGroup1.Vertices)
            {
                foreach (IEdge oIncidentEdge in oVertexInGroup1.IncidentEdges)
                {
                    Int32 iGroup2Index;

                    if ( IncidentEdgeShouldBeCounted(oIncidentEdge,
                        oVertexInGroup1, iGroup1Index, oGroupIndexDictionary,
                        useDirectedness, bGraphIsDirected, out iGroup2Index) )
                    {
                        CountIncidentEdge(oIncidentEdge, iGroup1Index,
                            iGroup2Index, oIntergroupEdges,
                            oIntergroupEdgeIndexes);
                    }
                }
            }
        }

        oIntergroupEdges.Sort(CompareIntergroupEdges);

        return (true);
    }

    //*************************************************************************
    //  Method: IncidentEdgeShouldBeCounted
    //
    /// <summary>
    /// Determines whether an incident edge should be counted.
    /// </summary>
    ///
    /// <param name="oIncidentEdge">
    /// The incident edge to test.
    /// </param>
    ///
    /// <param name="oVertexInGroup1">
    /// The vertex that <paramref name="oIncidentEdge" /> is incident to.
    /// </param>
    ///
    /// <param name="iGroup1Index">
    /// The group index of <paramref name="oVertexInGroup1" />.
    /// </param>
    ///
    /// <param name="oGroupIndexDictionary">
    /// The key is the IVertex.ID and the value is the zero-based index of the
    /// group the vertex belongs to.
    /// </param>
    ///
    /// <param name="bUseDirectedness">
    /// true if the graph's directedness should be taken into account.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed.
    /// </param>
    ///
    /// <param name="iGroup2Index">
    /// Where the group index of the incident edge's other vertex gets stored
    /// if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the incident edge should be counted.
    /// </returns>
    //*************************************************************************

    protected Boolean
    IncidentEdgeShouldBeCounted
    (
        IEdge oIncidentEdge,
        IVertex oVertexInGroup1,
        Int32 iGroup1Index,
        Dictionary<Int32, Int32> oGroupIndexDictionary,
        Boolean bUseDirectedness,
        Boolean bGraphIsDirected,
        out Int32 iGroup2Index
    )
    {
        Debug.Assert(oIncidentEdge != null);
        Debug.Assert(oVertexInGroup1 != null);
        Debug.Assert(iGroup1Index >= 0);
        Debug.Assert(oGroupIndexDictionary != null);
        AssertValid();

        IVertex oVertex2 = oIncidentEdge.GetAdjacentVertex(oVertexInGroup1);

        if ( !oGroupIndexDictionary.TryGetValue(
            oVertex2.ID, out iGroup2Index) )
        {
            // The edge's second vertex is not in a group.

            return (false);
        }

        Boolean bEdgesFirstVertexIsVertexInGroup1 =
            (oIncidentEdge.Vertex1 == oVertexInGroup1);

        if (iGroup1Index == iGroup2Index)
        {
            // The edge's vertices are in the same group.  Count the edge only
            // if its first vertex is oVertexInGroup1.  That way, when the same
            // edge is passed to this method again as an incident edge of the
            // edge's second vertex, it won't be counted again.

            return (bEdgesFirstVertexIsVertexInGroup1);
        }
        else if (!bUseDirectedness || !bGraphIsDirected)
        {
            // All edges between group 1 and group 2 should be counted in a
            // single IntergroupEdgeInfo object.  Count the edge only if its
            // second vertex is in a group whose index is greater than or equal
            // to the group index of its first vertex.  (The equal case handles
            // edges whose vertices are within the same group, including
            // self-loops.)  That way, when the same edge is passed to this
            // method again as an incident edge of the edge's second vertex, it
            // won't be counted again.

            return (iGroup2Index >= iGroup1Index);
        }
        else
        {
            // Edges from group 1 to group 2 should be returned in one object,
            // and edges from group 2 to group 1 should be returned in another
            // IntergroupEdgeInfo object.  Count the edge only if its first
            // vertex is in group 1.  That way, when the same edge is passed to
            // this method again as an incident edge of the edge's second
            // vertex, it won't be counted again.

            return (bEdgesFirstVertexIsVertexInGroup1);
        }
    }

    //*************************************************************************
    //  Method: CountIncidentEdge()
    //
    /// <summary>
    /// Counts an incident edge.
    /// </summary>
    ///
    /// <param name="oIntergroupEdge">
    /// The edge to count.
    /// </param>
    ///
    /// <param name="iGroup1Index">
    /// The first group the edge connects to.
    /// </param>
    ///
    /// <param name="iGroup2Index">
    /// The second group the edge connects to.  This can be the same as
    /// <paramref name="iGroup1Index" />.
    /// </param>
    ///
    /// <param name="oIntergroupEdges">
    /// A list of IntergroupEdge objects that have already been created.  This
    /// method adds a new object to the list when necessary.
    /// </param>
    ///
    /// <param name="oIntergroupEdgeIndexes">
    /// The key is the index of the second group that edges in <paramref
    /// name="iGroup1Index" /> connect to and the value is the
    /// IntergroupEdgeInfo object that counts all of the edges from <paramref
    /// name="iGroup1Index" /> to the second group.
    /// </param>
    //*************************************************************************

    protected void
    CountIncidentEdge
    (
        IEdge oIntergroupEdge,
        Int32 iGroup1Index,
        Int32 iGroup2Index,
        IList<IntergroupEdgeInfo> oIntergroupEdges,
        Dictionary<Int32, IntergroupEdgeInfo> oIntergroupEdgeIndexes
    )
    {
        Debug.Assert(oIntergroupEdge != null);
        Debug.Assert(iGroup1Index >= 0);
        Debug.Assert(iGroup2Index >= 0);
        Debug.Assert(oIntergroupEdges != null);
        Debug.Assert(oIntergroupEdgeIndexes != null);
        AssertValid();

        Double dPositiveEdgeWeight = EdgeUtil.GetPositiveEdgeWeight(
            oIntergroupEdge);

        // Does an object already exist for the iGroup1Index/iGroup2Index pair?

        IntergroupEdgeInfo oIntergroupEdgeInfo;

        if ( oIntergroupEdgeIndexes.TryGetValue(iGroup2Index,
            out oIntergroupEdgeInfo) )
        {
            // Yes.

            oIntergroupEdgeInfo.Edges++;
            oIntergroupEdgeInfo.EdgeWeightSum += dPositiveEdgeWeight;
        }
        else
        {
            // No.  Create one.

            oIntergroupEdgeInfo = new IntergroupEdgeInfo(
                iGroup1Index, iGroup2Index, 1, dPositiveEdgeWeight);

            oIntergroupEdges.Add(oIntergroupEdgeInfo);
            oIntergroupEdgeIndexes.Add(iGroup2Index, oIntergroupEdgeInfo);
        }
    }

    //*************************************************************************
    //  Method: CompareIntergroupEdges()
    //
    /// <summary>
    /// Compares two IntergroupEdgeInfo objects.
    /// </summary>
    ///
    /// <param name="oIntergroupEdge1">
    /// The first object to compare.
    /// </param>
    ///
    /// <param name="oIntergroupEdge2">
    /// The second object to compare.
    /// </param>
    ///
    /// <returns>
    /// Standard sort results.
    /// </returns>
    //*************************************************************************

    protected static Int32
    CompareIntergroupEdges
    (
        IntergroupEdgeInfo oIntergroupEdge1,
        IntergroupEdgeInfo oIntergroupEdge2
    )
    {
        Debug.Assert(oIntergroupEdge1 != null);
        Debug.Assert(oIntergroupEdge2 != null);

        // Sort first by Group1Index, then Group2Index.

        Int32 iReturnValue = oIntergroupEdge1.Group1Index.CompareTo(
            oIntergroupEdge2.Group1Index);

        if (iReturnValue == 0)
        {
            iReturnValue = oIntergroupEdge1.Group2Index.CompareTo(
                oIntergroupEdge2.Group2Index);
        }

        return (iReturnValue);
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
