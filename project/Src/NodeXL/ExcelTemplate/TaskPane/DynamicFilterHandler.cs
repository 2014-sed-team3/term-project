
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterHandler
//
/// <summary>
/// Handle the DynamicFilterColumnsChanged event fired by the <see
/// cref="DynamicFilterDialog" />.
/// </summary>
///
/// <remarks>
/// The <see cref="TaskPane" /> uses this class to handle the events fired when
/// the user changes a filter in the <see cref="DynamicFilterDialog" />.
/// </remarks>
//*****************************************************************************

public static class DynamicFilterHandler : Object
{
    //*************************************************************************
    //  Method: OnDynamicFilterColumnsChanged()
    //
    /// <summary>
    /// Handles the DynamicFilterColumnsChanged event on the <see
    /// cref="DynamicFilterDialog" />.
    /// </summary>
    ///
    /// <param name="dynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="workbook">
    /// The NodeXL workbook.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// The NodeXLControl displaying the graph.
    /// </param>
    ///
    /// <param name="edgeRowIDDictionary">
    /// The key is an edge row ID stored in the edge worksheet and the value is
    /// the corresponding Edge object in the graph.
    /// </param>
    ///
    /// <param name="vertexRowIDDictionary">
    /// The key is an vertex row ID stored in the vertex worksheet and the
    /// value is the corresponding Vertex object in the graph.
    /// </param>
    //*************************************************************************

    public static void
    OnDynamicFilterColumnsChanged
    (
        DynamicFilterDialog dynamicFilterDialog,
        DynamicFilterColumnsChangedEventArgs e,
        Microsoft.Office.Interop.Excel.Workbook workbook,
        NodeXLControl nodeXLControl,
        Dictionary<Int32, IIdentityProvider> edgeRowIDDictionary,
        Dictionary<Int32, IIdentityProvider> vertexRowIDDictionary
    )
    {
        Debug.Assert(dynamicFilterDialog != null);
        Debug.Assert(e != null);
        Debug.Assert(workbook != null);
        Debug.Assert(nodeXLControl != null);
        Debug.Assert(edgeRowIDDictionary != null);
        Debug.Assert(vertexRowIDDictionary != null);

        if (e.DynamicFilterColumns ==
            (DynamicFilterColumns.EdgeTable | DynamicFilterColumns.VertexTable)
            )
        {
            ReadDynamicFilterColumns(dynamicFilterDialog, workbook,
                nodeXLControl, true, edgeRowIDDictionary, vertexRowIDDictionary
                );
        }
        else if (e.DynamicFilterColumns == DynamicFilterColumns.EdgeTable)
        {
            ReadEdgeDynamicFilterColumn(dynamicFilterDialog, workbook,
                nodeXLControl, true, edgeRowIDDictionary);
        }
        else if (e.DynamicFilterColumns == DynamicFilterColumns.VertexTable)
        {
            ReadVertexDynamicFilterColumn(dynamicFilterDialog, workbook,
                nodeXLControl, true, vertexRowIDDictionary);
        }
    }

    //*************************************************************************
    //  Method: ReadDynamicFilterColumns()
    //
    /// <summary>
    /// Updates the graph with the contents of the dynamic filter columns on
    /// the edge and vertex tables.
    /// </summary>
    ///
    /// <param name="dynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    ///
    /// <param name="workbook">
    /// The NodeXL workbook.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// The NodeXLControl displaying the graph.
    /// </param>
    ///
    /// <param name="forceRedraw">
    /// true if the graph should be redrawn after the columns are read.
    /// </param>
    ///
    /// <param name="oEdgeRowIDDictionary">
    /// The key is an edge row ID stored in the edge worksheet and the value is
    /// the corresponding Edge object in the graph.
    /// </param>
    ///
    /// <param name="oVertexRowIDDictionary">
    /// The key is an vertex row ID stored in the vertex worksheet and the
    /// value is the corresponding Vertex object in the graph.
    /// </param>
    //*************************************************************************

    public static void
    ReadDynamicFilterColumns
    (
        DynamicFilterDialog dynamicFilterDialog,
        Microsoft.Office.Interop.Excel.Workbook workbook,
        NodeXLControl nodeXLControl,
        Boolean forceRedraw,
        Dictionary<Int32, IIdentityProvider> oEdgeRowIDDictionary,
        Dictionary<Int32, IIdentityProvider> oVertexRowIDDictionary
    )
    {
        Debug.Assert(dynamicFilterDialog != null);
        Debug.Assert(workbook != null);
        Debug.Assert(nodeXLControl != null);
        Debug.Assert(oEdgeRowIDDictionary != null);
        Debug.Assert(oVertexRowIDDictionary != null);

        ReadEdgeDynamicFilterColumn(dynamicFilterDialog, workbook,
            nodeXLControl, false, oEdgeRowIDDictionary);

        ReadVertexDynamicFilterColumn(dynamicFilterDialog, workbook,
            nodeXLControl, forceRedraw, oVertexRowIDDictionary);
    }

    //*************************************************************************
    //  Method: ReadFilteredAlpha()
    //
    /// <summary>
    /// Reads the filtered alpha specified by the user in the <see
    /// cref="DynamicFilterDialog" /> and applies it to the NodeXLControl.
    /// </summary>
    ///
    /// <param name="dynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// The NodeXLControl displaying the graph.
    /// </param>
    ///
    /// <param name="forceRedraw">
    /// true if the graph should be redrawn after the filtered alpha is read.
    /// </param>
    //*************************************************************************

    public static void
    ReadFilteredAlpha
    (
        DynamicFilterDialog dynamicFilterDialog,
        NodeXLControl nodeXLControl,
        Boolean forceRedraw
    )
    {
        Debug.Assert(dynamicFilterDialog != null);
        Debug.Assert(nodeXLControl != null);

        nodeXLControl.FilteredAlpha =
            ( new AlphaConverter() ).WorkbookToGraphAsByte(
                dynamicFilterDialog.FilteredAlpha);

        if (forceRedraw)
        {
            nodeXLControl.DrawGraph();
        }
    }

    //*************************************************************************
    //  Delegate: EdgeOrVertexCanBeMadeVisibleHandler
    //
    /// <summary>
    /// Represents a method that will be called by <see
    /// cref="ReadDynamicFilterColumn" /> to determine whether an edge or
    /// vertex can be made visible.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex.
    /// </param>
    ///
    /// <param name="oFilteredEdgeIDs">
    /// HashSet of edge IDs that have been filtered.
    /// </param>
    ///
    /// <param name="oFilteredVertexIDs">
    /// HashSet of vertex IDs that have been filtered.
    /// </param>
    ///
    /// <returns>
    /// true if the edge or vertex can be made visible.
    /// </returns>
    //*************************************************************************

    private delegate Boolean
    EdgeOrVertexCanBeMadeVisibleHandler
    (
        Object oEdgeOrVertex,
        HashSet<Int32> oFilteredEdgeIDs,
        HashSet<Int32> oFilteredVertexIDs
    );


    //*************************************************************************
    //  Delegate: EdgeOrVertexFilteredHandler
    //
    /// <summary>
    /// Represents a method that will be called by <see
    /// cref="ReadDynamicFilterColumn" /> when an edge or vertex is made
    /// visible or filtered.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex that was made visible or filtered.
    /// </param>
    ///
    /// <param name="bMadeVisible">
    /// true if the edge or vertex was made visible, false if it was filtered.
    /// </param>
    ///
    /// <param name="oDynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    //*************************************************************************

    private delegate void
    EdgeOrVertexFilteredHandler
    (
        Object oEdgeOrVertex,
        Boolean bMadeVisible,
        DynamicFilterDialog oDynamicFilterDialog
    );


    //*************************************************************************
    //  Method: ReadEdgeDynamicFilterColumn()
    //
    /// <summary>
    /// Updates the graph with the contents of the dynamic filter column on
    /// the edge table.
    /// </summary>
    ///
    /// <param name="oDynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook.
    /// </param>
    ///
    /// <param name="oNodeXLControl">
    /// The NodeXLControl displaying the graph.
    /// </param>
    ///
    /// <param name="bForceRedraw">
    /// true if the graph should be redrawn after the column is read.
    /// </param>
    ///
    /// <param name="oEdgeRowIDDictionary">
    /// The key is an edge row ID stored in the edge worksheet and the value is
    /// the corresponding Edge object in the graph.
    /// </param>
    //*************************************************************************

    private static void
    ReadEdgeDynamicFilterColumn
    (
        DynamicFilterDialog oDynamicFilterDialog,
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        NodeXLControl oNodeXLControl,
        Boolean bForceRedraw,
        Dictionary<Int32, IIdentityProvider> oEdgeRowIDDictionary
    )
    {
        Debug.Assert(oDynamicFilterDialog != null);
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oNodeXLControl != null);
        Debug.Assert(oEdgeRowIDDictionary != null);

        ReadDynamicFilterColumn(oDynamicFilterDialog, oWorkbook,
            oNodeXLControl, WorksheetNames.Edges, TableNames.Edges,
            oEdgeRowIDDictionary, GetFilteredEdgeIDs(oDynamicFilterDialog),
            EdgeCanBeMadeVisible, OnEdgeFiltered, bForceRedraw);
    }

    //*************************************************************************
    //  Method: ReadVertexDynamicFilterColumn()
    //
    /// <summary>
    /// Updates the graph with the contents of the dynamic filter column on
    /// the vertex table.
    /// </summary>
    ///
    /// <param name="oDynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook.
    /// </param>
    ///
    /// <param name="oNodeXLControl">
    /// The NodeXLControl displaying the graph.
    /// </param>
    ///
    /// <param name="bForceRedraw">
    /// true if the graph should be redrawn after the column is read.
    /// </param>
    ///
    /// <param name="oVertexRowIDDictionary">
    /// The key is an vertex row ID stored in the vertex worksheet and the
    /// value is the corresponding Vertex object in the graph.
    /// </param>
    //*************************************************************************

    private static void
    ReadVertexDynamicFilterColumn
    (
        DynamicFilterDialog oDynamicFilterDialog,
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        NodeXLControl oNodeXLControl,
        Boolean bForceRedraw,
        Dictionary<Int32, IIdentityProvider> oVertexRowIDDictionary
    )
    {
        Debug.Assert(oDynamicFilterDialog != null);
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oNodeXLControl != null);
        Debug.Assert(oVertexRowIDDictionary != null);

        ReadDynamicFilterColumn(oDynamicFilterDialog, oWorkbook,
            oNodeXLControl, WorksheetNames.Vertices, TableNames.Vertices,
            oVertexRowIDDictionary, GetFilteredVertexIDs(oDynamicFilterDialog),
            VertexCanBeMadeVisible, OnVertexFiltered, bForceRedraw);
    }

    //*************************************************************************
    //  Method: ReadDynamicFilterColumn()
    //
    /// <summary>
    /// Updates the graph with the contents of the dynamic filter column on
    /// the edge or vertex table.
    /// </summary>
    ///
    /// <param name="oDynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook.
    /// </param>
    ///
    /// <param name="oNodeXLControl">
    /// The NodeXLControl displaying the graph.
    /// </param>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table.
    /// </param>
    ///
    /// <param name="oRowIDDictionary">
    /// The key is an edge or vertex row ID stored in the edge or vertex
    /// worksheet and the value is the corresponding Edge or Vertex object in
    /// the graph.
    /// </param>
    ///
    /// <param name="oFilteredIDs">
    /// HashSet of edge or vertex IDs that have been filtered.  The HashSet is
    /// first cleared and then populated with the IEdge.ID or IVertex.ID of
    /// each edge or vertex that is filtered.
    /// </param>
    ///
    /// <param name="oOnEdgeOrVertexCanBeMadeVisible">
    /// Method to call to determine if an edge or vertex can be made visible.
    /// </param>
    ///
    /// <param name="oOnEdgeOrVertexFiltered">
    /// Method to call as each edge or vertex is made visible or filtered.
    /// </param>
    ///
    /// <param name="bForceRedraw">
    /// true if the graph should be redrawn after the column is read.
    /// </param>
    //*************************************************************************

    private static void
    ReadDynamicFilterColumn
    (
        DynamicFilterDialog oDynamicFilterDialog,
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        NodeXLControl oNodeXLControl,
        String sWorksheetName,
        String sTableName,
        Dictionary<Int32, IIdentityProvider> oRowIDDictionary,
        HashSet<Int32> oFilteredIDs,
        EdgeOrVertexCanBeMadeVisibleHandler oOnEdgeOrVertexCanBeMadeVisible,
        EdgeOrVertexFilteredHandler oOnEdgeOrVertexFiltered,
        Boolean bForceRedraw
    )
    {
        Debug.Assert(oDynamicFilterDialog != null);
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oNodeXLControl != null);
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert(oRowIDDictionary != null);
        Debug.Assert(oFilteredIDs != null);
        Debug.Assert(oOnEdgeOrVertexCanBeMadeVisible != null);
        Debug.Assert(oOnEdgeOrVertexFiltered != null);

        if (oNodeXLControl.IsLayingOutGraph)
        {
            return;
        }

        oFilteredIDs.Clear();

        // The dynamic filter column on the edge or vertex table contains
        // Booleans indicating whether the edge or vertex should be made
        // visible.

        // Get the data in the ID and dynamic filter columns.

        Object [,] oIDColumnValues, oDynamicFilterColumnValues;

        if ( !TryGetIDAndDynamicFilterValues(oWorkbook, sWorksheetName,
            sTableName, out oIDColumnValues, out oDynamicFilterColumnValues) )
        {
            return;
        }

        HashSet<Int32> oFilteredVertexIDs =
            GetFilteredVertexIDs(oDynamicFilterDialog);

        Int32 iRows = oIDColumnValues.GetUpperBound(0);
        Debug.Assert( iRows == oDynamicFilterColumnValues.GetUpperBound(0) );

        for (Int32 iOneBasedRow = 1; iOneBasedRow <= iRows; iOneBasedRow++)
        {
            Object oID = oIDColumnValues[iOneBasedRow, 1];

            Object oDynamicFilter =
                oDynamicFilterColumnValues[iOneBasedRow, 1];

            IIdentityProvider oEdgeOrVertex;

            if (
                oID is Double
                &&
                oRowIDDictionary.TryGetValue( (Int32)(Double)oID,
                    out oEdgeOrVertex )
                &&
                oDynamicFilter is Boolean
                )
            {
                Debug.Assert(oEdgeOrVertex is IMetadataProvider);

                IMetadataProvider oEdgeOrVertex2 =
                    (IMetadataProvider)oEdgeOrVertex;

                Boolean bMakeVisible = (Boolean)oDynamicFilter;

                if (!bMakeVisible)
                {
                    oFilteredIDs.Add(oEdgeOrVertex.ID);
                }
                else if ( !oOnEdgeOrVertexCanBeMadeVisible(
                    oEdgeOrVertex,
                    GetFilteredEdgeIDs(oDynamicFilterDialog),
                    oFilteredVertexIDs
                    ) )
                {
                    bMakeVisible = false;
                }

                // Filter or make visible the edge or vertex, then call the
                // handler specified by the caller.

                DynamicallyFilterEdgeOrVertex(oEdgeOrVertex2, bMakeVisible);

                oOnEdgeOrVertexFiltered(oEdgeOrVertex2, bMakeVisible,
                    oDynamicFilterDialog);
            }
        }

        if (bForceRedraw)
        {
            oNodeXLControl.DrawGraph();
        }
    }

    //*************************************************************************
    //  Method: EdgeCanBeMadeVisible()
    //
    /// <summary>
    /// Determines whether an edge can be made visible.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge.
    /// </param>
    ///
    /// <param name="oFilteredEdgeIDs">
    /// HashSet of edge IDs that have been filtered.
    /// </param>
    ///
    /// <param name="oFilteredVertexIDs">
    /// HashSet of vertex IDs that have been filtered.
    /// </param>
    ///
    /// <returns>
    /// true if the edge can be made visible.
    /// </returns>
    //*************************************************************************

    private static Boolean
    EdgeCanBeMadeVisible
    (
        Object oEdge,
        HashSet<Int32> oFilteredEdgeIDs,
        HashSet<Int32> oFilteredVertexIDs
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oFilteredEdgeIDs != null);
        Debug.Assert(oFilteredVertexIDs != null);

        // Don't make the edge visible if either of its vertices was filtered
        // by a vertex filter.

        IVertex [] aoVertices = ( (IEdge)oEdge ).Vertices;

        return (
            !oFilteredVertexIDs.Contains(aoVertices[0].ID)
            &&
            !oFilteredVertexIDs.Contains(aoVertices[1].ID)
            );
    }

    //*************************************************************************
    //  Method: VertexCanBeMadeVisible()
    //
    /// <summary>
    /// Determines whether a vertex can be made visible.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex.
    /// </param>
    ///
    /// <param name="oFilteredEdgeIDs">
    /// HashSet of edge IDs that have been filtered.
    /// </param>
    ///
    /// <param name="oFilteredVertexIDs">
    /// HashSet of vertex IDs that have been filtered.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex can be made visible.
    /// </returns>
    //*************************************************************************

    private static Boolean
    VertexCanBeMadeVisible
    (
        Object oVertex,
        HashSet<Int32> oFilteredEdgeIDs,
        HashSet<Int32> oFilteredVertexIDs
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oFilteredEdgeIDs != null);
        Debug.Assert(oFilteredVertexIDs != null);

        // Don't make the vertex visible if all of its edges are filtered.

        ICollection<IEdge> oIncidentEdges =
            ( (Vertex)oVertex ).IncidentEdges;

        if (oIncidentEdges.Count == 0)
        {
            // Note that the All() method called below returns true if the
            // collection is empty, so an isolated vertex needs special
            // treatment here.

            return (true);
        }

        return ( !oIncidentEdges.All(

            oIncidentEdge => oFilteredEdgeIDs.Contains(oIncidentEdge.ID)
            ) );
    }

    //*************************************************************************
    //  Method: OnEdgeFiltered
    //
    /// <summary>
    /// Gets called by <see cref="ReadDynamicFilterColumn" /> when an edge is
    /// made visible or filtered.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge that was made visible or filtered.
    /// </param>
    ///
    /// <param name="bMadeVisible">
    /// true if the edge was made visible, false if it was filtered.
    /// </param>
    ///
    /// <param name="oDynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    //*************************************************************************

    private static void
    OnEdgeFiltered
    (
        Object oEdge,
        Boolean bMadeVisible,
        DynamicFilterDialog oDynamicFilterDialog
    )
    {
        Debug.Assert(oEdge is IEdge);
        Debug.Assert(oDynamicFilterDialog != null);

        IEdge oEdge2 = (IEdge)oEdge;

        if (bMadeVisible)
        {
            // When an edge is made visible, its vertices should also be made
            // visible.

            IVertex [] aoVertices = oEdge2.Vertices;

            DynamicallyFilterEdgeOrVertex(aoVertices[0], true);
            DynamicallyFilterEdgeOrVertex(aoVertices[1], true);
        }
        else
        {
            // When an edge is filtered, its vertices should not be modified,
            // unless a vertex becomes an isolate due to filtering.  In that
            // case, the vertex should be filtered.

            HashSet<Int32> oFilteredEdgeIDs =
                GetFilteredEdgeIDs(oDynamicFilterDialog);

            foreach (IVertex oAdjacentVertex in oEdge2.Vertices)
            {
                // Note that each adjacent vertex always has at least one
                // incident edge, so special-case code to handle the All()
                // method's behavior for empty collections is not required
                // here.

                if ( oAdjacentVertex.IncidentEdges.All(

                    oIncidentEdge => oFilteredEdgeIDs.Contains(
                        oIncidentEdge.ID)
                    ) )
                {
                    DynamicallyFilterEdgeOrVertex(oAdjacentVertex, false);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: OnVertexFiltered
    //
    /// <summary>
    /// Gets called by <see cref="ReadDynamicFilterColumn" /> when a vertex is
    /// made visible or filtered.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex that was made visible or filtered.
    /// </param>
    ///
    /// <param name="bMadeVisible">
    /// true if the vertex was made visible, false if it was filtered.
    /// </param>
    ///
    /// <param name="oDynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    //*************************************************************************

    private static void
    OnVertexFiltered
    (
        Object oVertex,
        Boolean bMadeVisible,
        DynamicFilterDialog oDynamicFilterDialog
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oDynamicFilterDialog != null);

        // When a vertex is filtered, its incident edges should also be
        // filtered.  When a vertex is made visible, its incident edges should
        // also be made visible, unless 1) the edge was filtered by an edge
        // filter; or 2) the adjacent vertex was filtered by a vertex filter.

        HashSet<Int32> oFilteredEdgeIDs =
            GetFilteredEdgeIDs(oDynamicFilterDialog);

        HashSet<Int32> oFilteredVertexIDs =
            GetFilteredVertexIDs(oDynamicFilterDialog);

        Debug.Assert(oVertex is IVertex);

        IVertex oVertex2 = (IVertex)oVertex;

        foreach (IEdge oEdge in oVertex2.IncidentEdges)
        {
            if (bMadeVisible)
            {
                if (
                    oFilteredEdgeIDs.Contains(oEdge.ID)
                    ||
                    oFilteredVertexIDs.Contains(
                        oEdge.GetAdjacentVertex(oVertex2).ID)
                    )
                {
                    continue;
                }
            }

            DynamicallyFilterEdgeOrVertex(oEdge, bMadeVisible);
        }
    }

    //*************************************************************************
    //  Method: GetFilteredEdgeIDs
    //
    /// <summary>
    /// Gets a HashSet of edges that have been dynamically filtered by edge
    /// filters.
    /// </summary>
    ///
    /// <param name="oDynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    ///
    /// <returns>
    /// A HashSet of IEdge.ID values.
    /// used.
    /// </returns>
    ///
    /// <remarks>
    /// The HashSet is available only when the m_oDynamicFilterDialog dialog is
    /// open.
    /// </remarks>
    //*************************************************************************

    private static HashSet<Int32>
    GetFilteredEdgeIDs
    (
        DynamicFilterDialog oDynamicFilterDialog
    )
    {
        Debug.Assert(oDynamicFilterDialog != null);
        Debug.Assert( oDynamicFilterDialog.Tag is HashSet<Int32>[] );

        return (
            ( ( HashSet<Int32>[] )oDynamicFilterDialog.Tag )[0]
            );
    }

    //*************************************************************************
    //  Method: GetFilteredVertexIDs
    //
    /// <summary>
    /// Gets a HashSet of vertices that have been dynamically filtered by
    /// vertex filters.
    /// </summary>
    ///
    /// <param name="oDynamicFilterDialog">
    /// The <see cref="DynamicFilterDialog" /> that fired the event.
    /// </param>
    ///
    /// <returns>
    /// A HashSet of IVertex.ID values.
    /// </returns>
    ///
    /// <remarks>
    /// The HashSet is available only when the m_oDynamicFilterDialog dialog is
    /// open.
    /// </remarks>
    //*************************************************************************

    private static HashSet<Int32>
    GetFilteredVertexIDs
    (
        DynamicFilterDialog oDynamicFilterDialog
    )
    {
        Debug.Assert(oDynamicFilterDialog != null);
        Debug.Assert( oDynamicFilterDialog.Tag is HashSet<Int32>[] );

        return (
            ( ( HashSet<Int32>[] )oDynamicFilterDialog.Tag )[1]
            );
    }

    //*************************************************************************
    //  Method: DynamicallyFilterEdgeOrVertex
    //
    /// <summary>
    /// Makes visible or filters an edge or vertex in response to a change in a
    /// dynamic filter.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex to make visible or filter.
    /// </param>
    ///
    /// <param name="bMakeVisible">
    /// true to make the edge or vertex visible, false to filter it.
    /// </param>
    //*************************************************************************

    private static void
    DynamicallyFilterEdgeOrVertex
    (
        IMetadataProvider oEdgeOrVertex,
        Boolean bMakeVisible
    )
    {
        Debug.Assert(oEdgeOrVertex != null);

        // Don't do anything if the edge or vertex is hidden.

        Object oVisibilityKeyValue;

        if (
            oEdgeOrVertex.TryGetValue(ReservedMetadataKeys.Visibility,
                typeof(VisibilityKeyValue), out oVisibilityKeyValue)
            &&
            (VisibilityKeyValue)oVisibilityKeyValue ==
                VisibilityKeyValue.Hidden
            )
        {
            return;
        }

        if (bMakeVisible)
        {
            // Remove the key that controls visibility.  Without the key, the
            // edge or vertex is visible by default.

            oEdgeOrVertex.RemoveKey(ReservedMetadataKeys.Visibility);
        }
        else
        {
            // Set the key to Filtered.

            oEdgeOrVertex.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Filtered);
        }
    }

    //*************************************************************************
    //  Method: TryGetIDAndDynamicFilterValues()
    //
    /// <summary>
    /// Tries to get the values from the ID and dynamic filter columns on a
    /// specified table.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook.
    /// </param>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table.
    /// </param>
    ///
    /// <param name="oIDColumnValues">
    /// Where the values from the ID column get stored.
    /// </param>
    ///
    /// <param name="oDynamicFilterColumnValues">
    /// Where the values from the dynamic filter column get stored.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetIDAndDynamicFilterValues
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        String sWorksheetName,
        String sTableName,
        out Object [,] oIDColumnValues,
        out Object [,] oDynamicFilterColumnValues
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );

        oIDColumnValues = oDynamicFilterColumnValues = null;

        ListObject oTable;
        Range oIDColumnData, oDynamicFilterColumnData;

        return (
            ExcelTableUtil.TryGetTable(oWorkbook, sWorksheetName, sTableName,
                out oTable)
            &&
            ExcelTableUtil.TryGetTableColumnDataAndValues(oTable,
                CommonTableColumnNames.ID, out oIDColumnData,
                out oIDColumnValues)
            &&
            ExcelTableUtil.TryGetTableColumnDataAndValues(oTable,
                CommonTableColumnNames.DynamicFilter,
                out oDynamicFilterColumnData, out oDynamicFilterColumnValues)
            );
    }
}

}
