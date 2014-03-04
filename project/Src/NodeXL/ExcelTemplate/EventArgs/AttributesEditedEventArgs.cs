
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AttributesEditedEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="TaskPane.AttributesEditedInGraph" /> and <see
/// cref="ThisWorkbook.AttributesEditedInGraph" /> events.
/// </summary>
//*****************************************************************************

public class AttributesEditedEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: AttributesEditedEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AttributesEditedEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="edgeIDs">
    /// Array of IDs of the edges whose attributes were edited in the graph, or
    /// null if edge attributes weren't edited.  The IDs came from the edge
    /// worksheet's ID column.
    /// </param>
    ///
    /// <param name="editedEdgeAttributes">
    /// Edge attributes that were applied to the edges, or null if edge
    /// attributes weren't edited.
    /// </param>
    ///
    /// <param name="vertexIDs">
    /// Array of IDs of the vertices whose attributes were edited in the graph,
    /// or null if vertex attributes weren't edited.  The IDs came from the
    /// vertex worksheet's ID column.
    /// </param>
    ///
    /// <param name="editedVertexAttributes">
    /// Vertex attributes that were applied to the vertices, or null if vertex
    /// attributes weren't edited.
    /// </param>
    ///
    /// <remarks>
    /// If edge attributes were edited, <paramref name="edgeIDs" /> and
    /// <paramref name="editedEdgeAttributes" /> should be non-null.  If vertex
    /// attributes were edited, <paramref name="vertexIDs" /> and <paramref
    /// name="editedVertexAttributes" /> should be non-null.
    /// </remarks>
    //*************************************************************************

    public AttributesEditedEventArgs
    (
        Int32 [] edgeIDs,
        EditedEdgeAttributes editedEdgeAttributes,
        Int32 [] vertexIDs,
        EditedVertexAttributes editedVertexAttributes
    )
    {
        m_aiEdgeIDs = edgeIDs;
        m_oEditedEdgeAttributes = editedEdgeAttributes;
        m_aiVertexIDs = vertexIDs;
        m_oEditedVertexAttributes = editedVertexAttributes;

        AssertValid();
    }

    //*************************************************************************
    //  Property: EdgeIDs
    //
    /// <summary>
    /// Gets an array of IDs of the edges whose attributes were edited.
    /// </summary>
    ///
    /// <value>
    /// An array of IDs of the edges whose attributes were edited, or null if
    /// edge attributes weren't edited.
    /// </value>
    ///
    /// <remarks>
    /// The IDs are those that are stored in the edge worksheet's ID column and
    /// are different from the IEdge.ID values in the graph, which the edge
    /// worksheet knows nothing about.
    /// </remarks>
    //*************************************************************************

    public Int32 []
    EdgeIDs
    {
        get
        {
            AssertValid();

            return (m_aiEdgeIDs);
        }
    }

    //*************************************************************************
    //  Property: EditedEdgeAttributes
    //
    /// <summary>
    /// Gets the edge attributes that were applied to the edges.
    /// </summary>
    ///
    /// <value>
    /// Edge attributes that were applied to the edges, or null if edge
    /// attributes weren't edited.
    /// </value>
    //*************************************************************************

    public EditedEdgeAttributes
    EditedEdgeAttributes
    {
        get
        {
            AssertValid();

            return (m_oEditedEdgeAttributes);
        }
    }

    //*************************************************************************
    //  Property: VertexIDs
    //
    /// <summary>
    /// Gets an array of IDs of the vertices whose attributes were edited.
    /// </summary>
    ///
    /// <value>
    /// An array of IDs of the vertices whose attributes were edited, or null
    /// if vertex attributes weren't edited.
    /// </value>
    ///
    /// <remarks>
    /// The IDs are those that are stored in the vertex worksheet's ID column
    /// and are different from the IVertex.ID values in the graph, which the
    /// vertex worksheet knows nothing about.
    /// </remarks>
    //*************************************************************************

    public Int32 []
    VertexIDs
    {
        get
        {
            AssertValid();

            return (m_aiVertexIDs);
        }
    }

    //*************************************************************************
    //  Property: EditedVertexAttributes
    //
    /// <summary>
    /// Gets the vertex attributes that were applied to the vertices.
    /// </summary>
    ///
    /// <value>
    /// Vertex attributes that were applied to the vertices, or null if vertex
    /// attributes weren't edited.
    /// </value>
    //*************************************************************************

    public EditedVertexAttributes
    EditedVertexAttributes
    {
        get
        {
            AssertValid();

            return (m_oEditedVertexAttributes);
        }
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
        Debug.Assert( (m_aiEdgeIDs == null) ==
            (m_oEditedEdgeAttributes == null) );

        Debug.Assert( (m_aiVertexIDs == null) ==
            (m_oEditedVertexAttributes == null) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Array of IDs of edges whose attributes were edited in the graph, or
    /// null.

    protected Int32 [] m_aiEdgeIDs;

    /// Edge attributes that were applied to the edges, or null.

    protected EditedEdgeAttributes m_oEditedEdgeAttributes;

    /// Array of IDs of vertices whose attributes were edited in the graph, or
    /// null.

    protected Int32 [] m_aiVertexIDs;

    /// Vertex attributes that were applied to the vertices, or null.

    protected EditedVertexAttributes m_oEditedVertexAttributes;
}

}
