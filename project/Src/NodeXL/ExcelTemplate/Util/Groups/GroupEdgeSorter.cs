
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupEdgeSorter
//
/// <summary>
/// Sorts a graph's edges by the groups they are in.
/// </summary>
///
/// <remarks>
/// An edge is considered to be in a group if its first vertex is in the
/// group.
/// </remarks>
//*****************************************************************************

public static class GroupEdgeSorter
{
    //*************************************************************************
    //  Method: SortGroupEdges()
    //
    /// <summary>
    /// Sorts a graph's edges by the groups they are in.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to get the group information from.
    /// </param>
    ///
    /// <param name="maximumGroups">
    /// Maximum number of groups to get, or Int32.MaxValue for all groups.
    /// </param>
    ///
    /// <param name="includeDummyGroupForEntireGraph">
    /// true to include a dummy group for entire graph.  The dummy group has a
    /// <see cref="GroupEdgeInfo.GroupInfo" /> value of null.  If true, the
    /// dummy group is guaranteed to be the first item in the returned
    /// collection.
    /// </param>
    ///
    /// <param name="includeDummyGroupForUngroupedEdges">
    /// true to include a dummy group that contains the edges that are not
    /// contained in any real groups.  The dummy group has a <see
    /// cref="GroupEdgeInfo.GroupInfo" /> value of null.  This can be used only
    /// if <paramref name="maximumGroups" /> is Int32.MaxValue;
    /// </param>
    ///
    /// <returns>
    /// A List of <see cref="GroupEdgeInfo" /> objects, one for each of the
    /// graph's top groups, where the groups are sorted by descending vertex
    /// count.  The List optionally contains a dummy entry that simulates a
    /// group that contains all the graph's edges, or a dummy group that
    /// contains the edges that are not contained in any real groups.
    /// </returns>
    ///
    /// <remarks>
    /// The returned List may be empty, but null is never returned.
    /// </remarks>
    //*************************************************************************

    public static List<GroupEdgeInfo>
    SortGroupEdges
    (
        IGraph graph,
        Int32 maximumGroups,
        Boolean includeDummyGroupForEntireGraph,
        Boolean includeDummyGroupForUngroupedEdges
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(maximumGroups > 0);

        // Filter out duplicate edges.

        IEnumerable<IEdge> oFilteredEdges =
            EdgeFilter.FilterEdgesByImportedID(graph.Edges);

        // Sort the filtered edges by group.

        List<GroupEdgeInfo> oGroupEdgeInfos =
            SortFilteredEdgesByGroup(graph, maximumGroups, oFilteredEdges);

        if (includeDummyGroupForUngroupedEdges)
        {
            // This option makes sense only if all groups were asked for.

            Debug.Assert(maximumGroups == Int32.MaxValue);

            // Append a GroupEdgeInfo object that contains the edges that are
            // not contained in any real groups.

            oGroupEdgeInfos.Add(

                new GroupEdgeInfo(
                    GetUngroupedEdges(oFilteredEdges, oGroupEdgeInfos),
                    DummyGroupNameForUngroupedEdges)
                    );
        }

        if (includeDummyGroupForEntireGraph)
        {
            // Insert a GroupEdgeInfo object that contains all the graph's
            // edges.  Note that this must be the first item in the returned
            // collection.

            oGroupEdgeInfos.Insert(0,

                new GroupEdgeInfo(oFilteredEdges,
                    DummyGroupNameForEntireGraph)
                    );
        }

        return (oGroupEdgeInfos);
    }

    //*************************************************************************
    //  Method: SortFilteredEdgesByGroup()
    //
    /// <summary>
    /// Sorts a graph's filtered edges by the groups they are in.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to get the group information from.
    /// </param>
    ///
    /// <param name="iMaximumGroups">
    /// Maximum number of groups to get, or Int32.MaxValue for all groups.
    /// </param>
    ///
    /// <param name="oFilteredEdges">
    /// Filtered edges.
    /// </param>
    ///
    /// <returns>
    /// A List of GroupEdgeInfo objects, one for each of the graph's top
    /// groups, where the groups are sorted by descending vertex count.  The
    /// List may be empty, but null is never returned.
    /// </returns>
    //*************************************************************************

    private static List<GroupEdgeInfo>
    SortFilteredEdgesByGroup
    (
        IGraph oGraph,
        Int32 iMaximumGroups,
        IEnumerable<IEdge> oFilteredEdges
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(iMaximumGroups > 0);
        Debug.Assert(oFilteredEdges != null);

        List<GroupEdgeInfo> oGroupEdgeInfos = new List<GroupEdgeInfo>();

        // Get the graph's groups.

        GroupInfo [] aoGroups;

        if ( GroupUtil.TryGetGroups(oGraph, out aoGroups) )
        {
            // For each top group, we need a collection of the edges whose
            // first vertex is in the group.

            // This LinkedList will contain the graph's filtered edges, sorted
            // by their first vertex.  A null entry is inserted between each
            // set of edges.

            LinkedList<IEdge> oSortedEdges;

            // The key is a vertex, and the value is the first entry in
            // oSortedEdges for the vertex.

            Dictionary< IVertex, LinkedListNode<IEdge> > oFirstNodes;

            SortEdgesByVertex1(oFilteredEdges, out oSortedEdges,
                out oFirstNodes);

            List<IEdge> oEdgesInGroup = new List<IEdge>();

            // Sort the groups by descending vertex count.

            foreach ( ExcelTemplateGroupInfo oGroupInfo in
                ExcelTemplateGroupUtil.GetTopGroups(aoGroups, iMaximumGroups) )
            {
                oEdgesInGroup.Clear();

                foreach (IVertex oVertex in oGroupInfo.Vertices)
                {
                    // Loop through the edges that have oVertex as their first
                    // vertex.

                    LinkedListNode<IEdge> oNodeForVertex;

                    if ( oFirstNodes.TryGetValue(oVertex, out oNodeForVertex) )
                    {
                        while (oNodeForVertex.Value != null)
                        {
                            oEdgesInGroup.Add(oNodeForVertex.Value);
                            oNodeForVertex = oNodeForVertex.Next;
                        }
                    }
                }

                oGroupEdgeInfos.Add( new GroupEdgeInfo(
                    oEdgesInGroup.ToArray(), oGroupInfo) );
            }
        }

        return (oGroupEdgeInfos);
    }

    //*************************************************************************
    //  Method: SortEdgesByVertex1()
    //
    /// <summary>
    /// Sorts the graph's edges by their first vertex.
    /// </summary>
    ///
    /// <param name="oFilteredEdges">
    /// The graph's edges, with edges that have duplicate imported IDs removed.
    /// </param>
    ///
    /// <param name="oSortedEdges">
    /// Gets set to a LinkedList that contains the graph's filtered edges,
    /// sorted by their first vertex.  A null entry is inserted between each
    /// set of edges.
    /// </param>
    ///
    /// <param name="oFirstNodes">
    /// Gets set to a dictionary.  The key is a vertex, and the value is the
    /// first node in <paramref name="oSortedEdges" /> for the vertex.
    /// </param>
    //*************************************************************************

    private static void
    SortEdgesByVertex1
    (
        IEnumerable<IEdge> oFilteredEdges,
        out LinkedList<IEdge> oSortedEdges,
        out Dictionary< IVertex, LinkedListNode<IEdge> > oFirstNodes
    )
    {
        Debug.Assert(oFilteredEdges != null);

        oSortedEdges = new LinkedList<IEdge>();
        oFirstNodes = new Dictionary< IVertex, LinkedListNode<IEdge> >();

        foreach (IEdge oEdge in oFilteredEdges)
        {
            IVertex oVertex1 = oEdge.Vertex1;
            LinkedListNode<IEdge> oFirstNodeForVertex;

            if ( oFirstNodes.TryGetValue(oVertex1, out oFirstNodeForVertex) )
            {
                oSortedEdges.AddAfter(oFirstNodeForVertex, oEdge);
            }
            else
            {
                oSortedEdges.AddFirst( (IEdge)null );
                oFirstNodeForVertex = oSortedEdges.AddFirst(oEdge);
                oFirstNodes.Add(oVertex1, oFirstNodeForVertex);
            }
        }

        #if false  // For testing.
        {
            foreach (IVertex oVertex in oFirstNodes.Keys)
            {
                Debug.WriteLine("First vertex: " + oVertex.Name);
                LinkedListNode<IEdge> oFirstNode = oFirstNodes[oVertex];

                while (oFirstNode.Value != null)
                {
                    Debug.WriteLine("    Edge: "
                        + oFirstNode.Value.Vertex1.Name + ", "
                        + oFirstNode.Value.Vertex2.Name);

                    oFirstNode = oFirstNode.Next;
                }
            }
        }
        #endif
    }

    //*************************************************************************
    //  Method: GetUngroupedEdges()
    //
    /// <summary>
    /// Get a collection of edges that don't belong to a real group.
    /// </summary>
    ///
    /// <param name="oFilteredEdges">
    /// The graph's edges, with edges that have duplicate imported IDs removed.
    /// </param>
    ///
    /// <param name="oGroupEdgeInfos">
    /// List of real groups.
    /// </param>
    ///
    /// <returns>
    /// A collection of edges that don't belong to a real group.
    /// </returns>
    //*************************************************************************

    private static IEnumerable<IEdge>
    GetUngroupedEdges
    (
        IEnumerable<IEdge> oFilteredEdges,
        List<GroupEdgeInfo> oGroupEdgeInfos
    )
    {
        Debug.Assert(oFilteredEdges != null);
        Debug.Assert(oGroupEdgeInfos != null);

        // Get the edges in the real groups.

        HashSet<IEdge> oEdgesInGroups = new HashSet<IEdge>();

        foreach (GroupEdgeInfo oGroupEdgeInfo in oGroupEdgeInfos)
        {
            foreach (IEdge oEdge in oGroupEdgeInfo.Edges)
            {
                oEdgesInGroups.Add(oEdge);
            }
        }

        List<IEdge> oUngroupedEdges = new List<IEdge>();

        foreach (IEdge oEdge in oFilteredEdges)
        {
            if ( !oEdgesInGroups.Contains(oEdge) )
            {
                oUngroupedEdges.Add(oEdge);
            }
        }

        return ( oUngroupedEdges.ToArray() );
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// The name used for the dummy group that contains all the graph's edges.

    public const String DummyGroupNameForEntireGraph = "(Entire graph)";

    /// The name used for the dummy group that contains the edges that are not
    /// contained in any real groups.

    public const String DummyGroupNameForUngroupedEdges = "(Not in a group)";
}

}
