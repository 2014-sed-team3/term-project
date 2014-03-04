
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Class: GroupMetadataManager
//
/// <summary>
/// Manages the metadata that gets stored on the graph during layout when <see
/// cref="LayoutBase.LayoutStyle" /> is set to <see
/// cref="LayoutStyle.UseGroups" />.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class GroupMetadataManager
{
    //*************************************************************************
    //  Method: OnLayoutBegin
    //
    /// <summary>
    /// Performs required tasks when a layout begins.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph being laid out.
    /// </param>
    ///
    /// <remarks>
    /// This method should be called at the beginning of the layout process,
    /// regardless of the value of <see cref="LayoutBase.LayoutStyle" />.
    /// </remarks>
    //*************************************************************************

    public static void
    OnLayoutBegin
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        // Remove the group drawing information that may have been added by
        // OnLayoutUsingGroupsEnd().  Another set of group drawing information
        // will get added again by that method if they are needed.

        graph.RemoveKey(ReservedMetadataKeys.GroupLayoutDrawingInfo);

        if ( graph.RemoveKey( ReservedMetadataKeys.IntergroupEdgesHidden) )
        {
            // OnLayoutUsingGroupsComplete hid some edges.  Restore the
            // visibilities those edges had before they were hidden.

            foreach (IEdge oEdge in graph.Edges)
            {
                VisibilityKeyValue eSavedVisibility;

                if ( TryGetEdgeVisibility(oEdge,
                    ReservedMetadataKeys.SavedVisibility,
                    out eSavedVisibility) )
                {
                    if (eSavedVisibility == VisibilityKeyValue.Visible)
                    {
                        // Visible is the value assumed when there is no
                        // Visibility key, so there is no need to add a
                        // Visible value.  Just remove the Hidden value.

                        oEdge.RemoveKey(ReservedMetadataKeys.Visibility);
                    }
                    else
                    {
                        oEdge.SetValue(ReservedMetadataKeys.Visibility,
                            eSavedVisibility);
                    }

                    oEdge.RemoveKey(ReservedMetadataKeys.SavedVisibility);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: OnLayoutUsingGroupsEnd
    //
    /// <summary>
    /// Performs required tasks when a layout with the style <see
    /// cref="LayoutStyle.UseGroups" /> ends.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph being laid out.
    /// </param>
    ///
    /// <param name="laidOutGroups">
    /// List of <see cref="GroupInfo" /> objects, one for each group of
    /// vertices that was laid out.  These are sorted in descending order of
    /// the number of vertices in each group.  There is at least one vertex in
    /// each group.
    /// </param>
    ///
    /// <param name="groupRectanglePenWidth">
    /// The width of the pen used to draw group rectangles.  If 0, group
    /// rectangles aren't drawn.
    /// </param>
    ///
    /// <param name="intergroupEdgeStyle">
    /// Specifies how the edges that connect vertices in different groups
    /// should be shown.
    /// </param>
    ///
    /// <remarks>
    /// This method should be called at the end of the layout process if <see
    /// cref="LayoutBase.LayoutStyle" /> is <see
    /// cref="LayoutStyle.UseGroups" />.
    /// </remarks>
    //*************************************************************************

    public static void
    OnLayoutUsingGroupsEnd
    (
        IGraph graph,
        IList<GroupInfo> laidOutGroups,
        Double groupRectanglePenWidth,
        IntergroupEdgeStyle intergroupEdgeStyle
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(laidOutGroups != null);
        Debug.Assert(groupRectanglePenWidth >= 0);

        List<IntergroupEdgeInfo> oCombinedIntergroupEdges = null;

        if (intergroupEdgeStyle == IntergroupEdgeStyle.Combine)
        {
            // Get a collection of IntergroupEdgeInfo objects, one for the
            // edges between each pair of groups and one for the edges within
            // each group.

            IEnumerable<IntergroupEdgeInfo> oAllIntergroupEdges =
                ( new IntergroupEdgeCalculator() ).CalculateGraphMetrics(
                    graph, laidOutGroups, false);

            // Filter out the objects for the edges within each group.

            oCombinedIntergroupEdges = new List<IntergroupEdgeInfo>(
                oAllIntergroupEdges.Where(
                    oIntergroupEdge =>
                    oIntergroupEdge.Group1Index != oIntergroupEdge.Group2Index
                    ) );
        }

        if (intergroupEdgeStyle == IntergroupEdgeStyle.Hide ||
            intergroupEdgeStyle == IntergroupEdgeStyle.Combine)
        {
            // Intergroup edges need to be hidden.

            Boolean bEdgeHidden = false;
            Int32 iLaidOutGroups = laidOutGroups.Count;

            // The key is an IVertex.ID and the value is the zero-based index
            // of the laid-out group the vertex belongs to.

            Dictionary<Int32, Int32> oGroupIndexDictionary =
                GroupUtil.GetGroupIndexDictionary(laidOutGroups);

            for (Int32 iGroupIndex = 0; iGroupIndex < iLaidOutGroups;
                iGroupIndex++)
            {
                GroupInfo oGroup = laidOutGroups[iGroupIndex];

                foreach (IVertex oVertex in oGroup.Vertices)
                {
                    foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
                    {
                        if ( IncidentEdgeShouldBeHidden(oIncidentEdge, oVertex,
                            iGroupIndex, oGroupIndexDictionary) )
                        {
                            HideEdge(oIncidentEdge);
                            bEdgeHidden = true;
                        }
                    }
                }
            }

            if (bEdgeHidden)
            {
                graph.SetValue(ReservedMetadataKeys.IntergroupEdgesHidden,
                    null);
            }
        }

        graph.SetValue( ReservedMetadataKeys.GroupLayoutDrawingInfo,
            new GroupLayoutDrawingInfo(laidOutGroups, groupRectanglePenWidth,
                oCombinedIntergroupEdges) );
    }

    //*************************************************************************
    //  Method: TryGetGroupLayoutDrawingInfo
    //
    /// <summary>
    /// Attempts to get group drawing information stored in a graph's metadata.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to get the information from.
    /// </param>
    ///
    /// <param name="groupLayoutDrawingInfo">
    /// Where the information get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the information was obtained.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetGroupLayoutDrawingInfo
    (
        IGraph graph,
        out GroupLayoutDrawingInfo groupLayoutDrawingInfo
    )
    {
        Debug.Assert(graph != null);

        groupLayoutDrawingInfo = null;
        Object oGroupLayoutDrawingInfoAsObject;

        if ( graph.TryGetValue(ReservedMetadataKeys.GroupLayoutDrawingInfo,
            typeof(GroupLayoutDrawingInfo),
            out oGroupLayoutDrawingInfoAsObject) )
        {
            groupLayoutDrawingInfo =
                (GroupLayoutDrawingInfo)oGroupLayoutDrawingInfoAsObject;

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TransformGroupRectangles()
    //
    /// <summary>
    /// Transforms a graph's group rectangles if they exist.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose layout needs to be transformed.
    /// </param>
    ///
    /// <param name="originalLayoutContext">
    /// <see cref="LayoutContext" /> object that was passed to the most recent
    /// call to <see cref="ILayout.LayOutGraph" />.
    /// </param>
    ///
    /// <param name="newLayoutContext">
    /// Provides access to objects needed to transform the graph's layout.
    /// </param>
    //*************************************************************************

    public static void
    TransformGroupRectangles
    (
        IGraph graph,
        LayoutContext originalLayoutContext,
        LayoutContext newLayoutContext
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(originalLayoutContext != null);
        Debug.Assert(newLayoutContext != null);

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        if ( TryGetGroupLayoutDrawingInfo(graph, out oGroupLayoutDrawingInfo) )
        {
            // Replace the metadata value's group rectangles with a transformed
            // set of group rectangles.

            Matrix oTransformationMatrix =
                LayoutUtil.GetRectangleTransformation(
                    originalLayoutContext.GraphRectangle,
                    newLayoutContext.GraphRectangle
                    );

            foreach (GroupInfo oGroupInfo in
                oGroupLayoutDrawingInfo.GroupsToDraw)
            {
                oGroupInfo.Rectangle = LayoutUtil.TransformRectangle(
                    oGroupInfo.Rectangle, oTransformationMatrix);
            }
        }
    }

    //*************************************************************************
    //  Method: IncidentEdgeShouldBeHidden
    //
    /// <summary>
    /// Determines whether an incident edge should be hidden.
    /// </summary>
    ///
    /// <param name="oIncidentEdge">
    /// The incident edge to test.
    /// </param>
    ///
    /// <param name="oVertex">
    /// The vertex that <paramref name="oIncidentEdge" /> is incident to.
    /// </param>
    ///
    /// <param name="iGroupIndex">
    /// The index of <paramref name="oVertex" /> in <paramref
    /// name="oGroupIndexDictionary" />.
    /// </param>
    ///
    /// <param name="oGroupIndexDictionary">
    /// The key is the IVertex.ID and the value is the zero-based index of the
    /// laid-out group the vertex belongs to.
    /// </param>
    ///
    /// <returns>
    /// true if the incident edge should be hidden.
    /// </returns>
    //*************************************************************************

    private static Boolean
    IncidentEdgeShouldBeHidden
    (
        IEdge oIncidentEdge,
        IVertex oVertex,
        Int32 iGroupIndex,
        Dictionary<Int32, Int32> oGroupIndexDictionary
    )
    {
        Debug.Assert(oIncidentEdge != null);
        Debug.Assert(oVertex != null);
        Debug.Assert(iGroupIndex >= 0);
        Debug.Assert(oGroupIndexDictionary != null);

        Int32 iOtherGroupIndex = oGroupIndexDictionary[
            oIncidentEdge.GetAdjacentVertex(oVertex).ID];

        return (

            // The edge's vertices are not in the same group.

            iOtherGroupIndex != iGroupIndex
            &&

            // The edge has not already been hidden.

            !oIncidentEdge.ContainsKey(ReservedMetadataKeys.SavedVisibility)
            );
    }

    //*************************************************************************
    //  Method: HideEdge
    //
    /// <summary>
    /// Hides an edge and saves its previous visibility.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to hide.
    /// </param>
    //*************************************************************************

    private static void
    HideEdge
    (
        IEdge oEdge
    )
    {
        Debug.Assert(oEdge != null);

        VisibilityKeyValue eSavedVisibility;

        if ( !TryGetEdgeVisibility(oEdge, ReservedMetadataKeys.Visibility,
            out eSavedVisibility) )
        {
            eSavedVisibility = VisibilityKeyValue.Visible;
        }

        oEdge.SetValue(ReservedMetadataKeys.SavedVisibility, eSavedVisibility);

        oEdge.SetValue(ReservedMetadataKeys.Visibility,
            VisibilityKeyValue.Hidden);
    }

    //*************************************************************************
    //  Method: TryGetEdgeVisibility
    //
    /// <summary>
    /// Attempts to get a visibility value from an edge.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to get the visibility value from.
    /// </param>
    ///
    /// <param name="sKey">
    /// Name of the key where the visibility is stored.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// Where the visibility value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the visibility was obtained.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetEdgeVisibility
    (
        IEdge oEdge,
        String sKey,
        out VisibilityKeyValue eVisibility
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );

        eVisibility = VisibilityKeyValue.Visible;

        Object oVisibilityAsObject;

        if ( oEdge.TryGetValue(sKey, typeof(VisibilityKeyValue),
            out oVisibilityAsObject) )
        {
            eVisibility = (VisibilityKeyValue)oVisibilityAsObject;
            return (true);
        }

        return (false);
    }
}

}
