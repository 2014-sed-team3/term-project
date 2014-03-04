﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLControlUtil
//
/// <summary>
/// Utility methods for working with the <see cref="NodeXLControl" />.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class NodeXLControlUtil
{
    //*************************************************************************
    //  Method: GetSelectedEdgeRowIDs()
    //
    /// <summary>
    /// Gets the row IDs of the selected edges.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to get the selected edges from.
    /// </param>
    ///
    /// <returns>
    /// A collection of the row IDs of the selected edges.  The row IDs are
    /// those that came from the edge worksheet's ID column.  They are NOT
    /// IEdge.ID values.
    /// </returns>
    //*************************************************************************

    public static ICollection<Int32>
    GetSelectedEdgeRowIDs
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        LinkedList<Int32> oSelectedEdgeRowIDs = new LinkedList<Int32>();

        foreach (IEdge oSelectedEdge in nodeXLControl.SelectedEdges)
        {
            // The IDs from the worksheet are stored in the edge Tags.

            if ( !(oSelectedEdge.Tag is Int32) )
            {
                // The ID column is optional, so edges may not have their Tag
                // set.

                continue;
            }

            oSelectedEdgeRowIDs.AddLast( (Int32)oSelectedEdge.Tag );
        }

        return (oSelectedEdgeRowIDs);
    }

    //*************************************************************************
    //  Method: GetSelectedVertexRowIDs()
    //
    /// <summary>
    /// Gets the row IDs of the selected vertices.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to get the selected vertices from.
    /// </param>
    ///
    /// <returns>
    /// A collection of the row IDs of the selected vertices.  The row IDs are
    /// those that came from the vertex worksheet's ID column.  They are NOT
    /// IVertex.ID values.
    /// </returns>
    //*************************************************************************

    public static ICollection<Int32>
    GetSelectedVertexRowIDs
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        LinkedList<Int32> oSelectedVertexIDs = new LinkedList<Int32>();

        foreach (IVertex oSelectedVertex in nodeXLControl.SelectedVertices)
        {
            // The IDs from the worksheet are stored in the vertex Tags.

            if ( !(oSelectedVertex.Tag is Int32) )
            {
                // The ID column is optional, so vertices may not have their
                // Tag set.

                continue;
            }

            oSelectedVertexIDs.AddLast( (Int32)oSelectedVertex.Tag );
        }

        return (oSelectedVertexIDs);
    }

    //*************************************************************************
    //  Method: GetSelectedVerticesAsHashSet()
    //
    /// <summary>
    /// Gets the selected vertices as a HashSet.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to get the selected vertices from.
    /// </param>
    ///
    /// <returns>
    /// A HashSet of selected vertices.  The key is the IVertex.
    /// </returns>
    //*************************************************************************

    public static HashSet<IVertex>
    GetSelectedVerticesAsHashSet
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        HashSet<IVertex> oSelectedVertices = new HashSet<IVertex>();

        foreach (IVertex oSelectedVertex in nodeXLControl.SelectedVertices)
        {
            oSelectedVertices.Add(oSelectedVertex);
        }

        return (oSelectedVertices);
    }

    //*************************************************************************
    //  Method: GetSelectedVerticesAsDictionary()
    //
    /// <summary>
    /// Gets the selected vertices as a dictionary.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to get the selected vertices from.
    /// </param>
    ///
    /// <returns>
    /// A dictionary of selected vertices.  The key is the IVertex.ID and the
    /// value is the IVertex.
    /// </returns>
    //*************************************************************************

    public static Dictionary<Int32, IVertex>
    GetSelectedVerticesAsDictionary
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        Dictionary<Int32, IVertex> oSelectedVertices =
            new Dictionary<Int32, IVertex>();

        foreach (IVertex oSelectedVertex in nodeXLControl.SelectedVertices)
        {
            oSelectedVertices[oSelectedVertex.ID] = oSelectedVertex;
        }

        return (oSelectedVertices);
    }

    //*************************************************************************
    //  Method: GetSelectedEdgesAsHashSet()
    //
    /// <summary>
    /// Gets the selected edges as a HashSet.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to get the selected edges from.
    /// </param>
    ///
    /// <returns>
    /// A HashSet of selected edges.  The key is the IEdge.
    /// </returns>
    //*************************************************************************

    public static HashSet<IEdge>
    GetSelectedEdgesAsHashSet
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        HashSet<IEdge> oSelectedEdges = new HashSet<IEdge>();

        foreach (IEdge oSelectedEdge in nodeXLControl.SelectedEdges)
        {
            oSelectedEdges.Add(oSelectedEdge);
        }

        return (oSelectedEdges);
    }

    //*************************************************************************
    //  Method: GetSelectedEdgesAsDictionary()
    //
    /// <summary>
    /// Gets the selected edges as a dictionary.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to get the selected edges from.
    /// </param>
    ///
    /// <returns>
    /// A dictionary of selected edges.  The key is the IEdge.ID and the value
    /// is the IEdge.
    /// </returns>
    //*************************************************************************

    public static Dictionary<Int32, IEdge>
    GetSelectedEdgesAsDictionary
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        Dictionary<Int32, IEdge> oSelectedEdges =
            new Dictionary<Int32, IEdge>();

        foreach (IEdge oSelectedEdge in nodeXLControl.SelectedEdges)
        {
            oSelectedEdges[oSelectedEdge.ID] = oSelectedEdge;
        }

        return (oSelectedEdges);
    }

    //*************************************************************************
    //  Method: GetVisibleVertices()
    //
    /// <summary>
    /// Gets a collection of visible vertices.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// Control to get the vertices from.
    /// </param>
    ///
    /// <returns>
    /// A collection of visible vertices.  The collection may be empty.
    /// </returns>
    //*************************************************************************

    public static ICollection<IVertex>
    GetVisibleVertices
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        List<IVertex> oVisibleVertices = new List<IVertex>();

        foreach (IVertex oVertex in nodeXLControl.Graph.Vertices)
        {
            Object oVisibilityKeyValue;

            if (
                !oVertex.TryGetValue(ReservedMetadataKeys.Visibility,
                    typeof(VisibilityKeyValue), out oVisibilityKeyValue)
                ||
                (VisibilityKeyValue)oVisibilityKeyValue ==
                    VisibilityKeyValue.Visible
                )
            {
                oVisibleVertices.Add(oVertex);
            }
        }

        return (oVisibleVertices);
    }

    //*************************************************************************
    //  Method: SelectSubgraphs()
    //
    /// <summary>
    /// Selects the subgraphs for one or more vertices.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The NodeXLControl whose subgraphs should be selected.
    /// </param>
    ///
    /// <param name="verticesToSelectSubgraphsFor">
    /// Zero ore more vertices whose subgraphs should be selected.
    /// </param>
    ///
    /// <param name="levels">
    /// The number of levels of adjacent vertices to select in each subgraph.
    /// Must be a multiple of 0.5.  If 0, a subgraph includes just the vertex;
    /// if 1, it includes the vertex and its adjacent vertices; if 2, it
    /// includes the vertex, its adjacent vertices, and their adjacent
    /// vertices; and so on.  The difference between N.5 and N.0 is that N.5
    /// includes any edges connecting the outermost vertices to each other,
    /// whereas N.0 does not.  1.5, for example, includes any edges that
    /// connect the vertex's adjacent vertices to each other, whereas 1.0
    /// includes only those edges that connect the adjacent vertices to the
    /// vertex.
    /// </param>
    ///
    /// <param name="selectConnectingEdges">
    /// true to select the subgraphs' connecting edges.
    /// </param>
    //*************************************************************************

    public static void
    SelectSubgraphs
    (
        NodeXLControl nodeXLControl,
        IEnumerable<IVertex> verticesToSelectSubgraphsFor,
        Decimal levels,
        Boolean selectConnectingEdges
    )
    {
        Debug.Assert(nodeXLControl != null);
        Debug.Assert(verticesToSelectSubgraphsFor != null);
        Debug.Assert(levels >= 0);
        Debug.Assert(Decimal.Remainder(levels, 0.5M) == 0M);

        // Create HashSets for all of the vertices and edges that will be
        // selected.  The key is the IVertex or IEdge.  HashSets are used to
        // prevent the same vertex or edge from being selected twice.

        HashSet<IVertex> oAllSelectedVertices = new HashSet<IVertex>();
        HashSet<IEdge> oAllSelectedEdges = new HashSet<IEdge>();

        foreach (IVertex oVertexToSelectSubgraphFor in
            verticesToSelectSubgraphsFor)
        {
            // These are similar collections for the vertices and edges that
            // will be selected for this subgraph only.

            Dictionary<IVertex, Int32> oThisSubgraphSelectedVertices;
            HashSet<IEdge> oThisSubgraphSelectedEdges;

            SubgraphCalculator.GetSubgraph(oVertexToSelectSubgraphFor, levels,
                selectConnectingEdges, out oThisSubgraphSelectedVertices,
                out oThisSubgraphSelectedEdges);

            // Consolidate the subgraph's selected vertices and edges into the
            // "all" dictionaries.

            foreach (IVertex oVertex in oThisSubgraphSelectedVertices.Keys)
            {
                oAllSelectedVertices.Add(oVertex);
            }

            foreach (IEdge oEdge in oThisSubgraphSelectedEdges)
            {
                oAllSelectedEdges.Add(oEdge);
            }
        }

        // Replace the selection.

        nodeXLControl.SetSelected(oAllSelectedVertices, oAllSelectedEdges);
    }
}

}
