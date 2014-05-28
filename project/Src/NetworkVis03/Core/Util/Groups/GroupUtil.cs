
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Smrf.NodeXL.Core
{
//*****************************************************************************
//  Class: GroupUtil
//
/// <summary>
/// Utility methods for dealing with vertex groups.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class GroupUtil
{
    //*************************************************************************
    //  Method: GraphHasGroups()
    //
    /// <summary>
    /// Returns true if a graph has at least one group.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to check.
    /// </param>
    ///
    /// <returns>
    /// true if the graph has at least one group.
    /// </returns>
    //*************************************************************************

    public static Boolean
    GraphHasGroups
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        GroupInfo [] aoGroupInfo;

        return ( TryGetGroups(graph, out aoGroupInfo) );
    }

    //*************************************************************************
    //  Method: TryGetGroups()
    //
    /// <summary>
    /// Attempts to get a graph's groups.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to get groups from.
    /// </param>
    ///
    /// <param name="groups">
    /// Where the graph's groups get stored if the graph has at least one
    /// group.
    /// </param>
    ///
    /// <returns>
    /// true if the graph has at least one group.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetGroups
    (
        IGraph graph,
        out GroupInfo [] groups
    )
    {
        Debug.Assert(graph != null);

        groups = ( GroupInfo[] )graph.GetValue(
            ReservedMetadataKeys.GroupInfo, typeof( GroupInfo[] ) );

        return (groups != null && groups.Length > 0);
    }

    //*************************************************************************
    //  Method: GetGroups()
    //
    /// <summary>
    /// Gets a graph's groups, or an empty array if the graph has no groups.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to get groups from.
    /// </param>
    ///
    /// <returns>
    /// An array of GroupInfo objects.  If the graph has no groups, an empty
    /// array is returned.
    /// </returns>
    //*************************************************************************

    public static GroupInfo []
    GetGroups
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        GroupInfo [] aoGroups;

        if ( !TryGetGroups(graph, out aoGroups) )
        {
            aoGroups = new GroupInfo[0];
        }

        return (aoGroups);
    }

    //*************************************************************************
    //  Method: GetGroupIndexDictionary
    //
    /// <summary>
    /// Gets a Dictionary that maps vertex IDs to the groups the vertices
    /// belong to.
    /// </summary>
    ///
    /// <param name="groups">
    /// A collection of <see cref="GroupInfo" /> objects, one for each group.
    /// </param>
    ///
    /// <returns>
    /// A new Dictionary.  The key is an IVertex.ID and the value is the
    /// zero-based index of the group the vertex belongs to.
    /// </returns>
    //*************************************************************************

    public static Dictionary<Int32, Int32>
    GetGroupIndexDictionary
    (
        IList<GroupInfo> groups
    )
    {
        Debug.Assert(groups != null);

        Dictionary<Int32, Int32> oGroupIndexDictionary =
            new Dictionary<Int32, Int32>();

        Int32 iGroups = groups.Count;

        for (Int32 i = 0; i < iGroups; i++)
        {
            foreach (IVertex oVertex in groups[i].Vertices)
            {
                oGroupIndexDictionary[oVertex.ID] = i;
            }
        }

        return (oGroupIndexDictionary);
    }

    //*************************************************************************
    //  Method: GetGroupsWithAllVertices()
    //
    /// <summary>
    /// Gets a list of the graph's groups, adding an extra group if necessary
    /// to contain any non-grouped vertices.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to get the groups from.
    /// </param>
    ///
    /// <param name="checkForCollapsedGroups">
    /// If true and a group is collapsed, the group's vertices are ignored and
    /// the vertex that represents the collapsed group is placed in the extra
    /// group mentioned in the Remarks.
    /// </param>
    ///
    /// <returns>
    /// A List containing a GroupInfo object for each of the graph's groups.
    /// See the Remarks.
    /// </returns>
    ///
    /// <remarks>
    /// If the graph has vertices that aren't in a group, they are put in an
    /// extra GroupInfo object that is included in the returned list.  The
    /// returned list therefore contains every vertex in the graph.
    /// </remarks>
    //*************************************************************************

    public static List<GroupInfo>
    GetGroupsWithAllVertices
    (
        IGraph graph,
        Boolean checkForCollapsedGroups
    )
    {
        Debug.Assert(graph != null);

        // This HashSet initially contains all the graph's vertices.  It will
        // eventually contain only vertices that aren't in a group, along with
        // each vertex that represents a collapsed group.

        HashSet<IVertex> oRemainingVertices =
            new HashSet<IVertex>(graph.Vertices);

        List<GroupInfo> oGroupList = new List<GroupInfo>();

        foreach ( GroupInfo oGroupInfo in GetGroups(graph) )
        {
            ICollection<IVertex> oVertices = oGroupInfo.Vertices;

            Debug.Assert(oVertices != null);

            if (oVertices.Count == 0)
            {
                continue;
            }

            if (checkForCollapsedGroups &&
                oVertices.First().ParentGraph == null)
            {
                // The group has been collapsed.  See
                // NodeXLControl.CollapseGroup() for information about how
                // groups are collapsed.

                continue;
            }

            oGroupList.Add(oGroupInfo);

            foreach (IVertex oVertex in oVertices)
            {
                oRemainingVertices.Remove(oVertex);
            }
        }

        if (oRemainingVertices.Count > 0)
        {
            // Add a GroupInfo object for the vertices that aren't in a group.

            GroupInfo oRemainingVertexInformation = new GroupInfo();

            foreach (IVertex oRemainingVertex in oRemainingVertices)
            {
                oRemainingVertexInformation.Vertices.AddLast(oRemainingVertex);
            }

            oGroupList.Add(oRemainingVertexInformation);
        }

        return (oGroupList);
    }

    //*************************************************************************
    //  Method: RemoveIsolatesFromGroups()
    //
    /// <summary>
    /// Removes isolated vertices from a list of the graph's groups.
    /// </summary>
    ///
    /// <param name="groups">
    /// The list containing the graph's groups.
    /// </param>
    ///
    /// <remarks>
    /// This method removes all isolated vertices from <paramref
    /// name="groups" />.  If that leaves one or more GroupInfo objects with
    /// zero vertices, those GroupInfo objects are then removed from the list.
    /// </remarks>
    //*************************************************************************

    public static void
    RemoveIsolatesFromGroups
    (
        List<GroupInfo> groups
    )
    {
        Debug.Assert(groups != null);

        Int32 iGroups = groups.Count;

        for (Int32 i = iGroups - 1; i >= 0; i--)
        {
            LinkedList<IVertex> oVertices = groups[i].Vertices;
            LinkedListNode<IVertex> oLinkedListNode = oVertices.First;

            while (oLinkedListNode != null)
            {
                LinkedListNode<IVertex> oLinkedListNodeToTest =
                    oLinkedListNode;

                oLinkedListNode = oLinkedListNode.Next;

                if (oLinkedListNodeToTest.Value.Degree == 0)
                {
                    oVertices.Remove(oLinkedListNodeToTest);
                }
            }

            if (oVertices.Count == 0)
            {
                groups.RemoveAt(i);
            }
        }
    }
}

}
