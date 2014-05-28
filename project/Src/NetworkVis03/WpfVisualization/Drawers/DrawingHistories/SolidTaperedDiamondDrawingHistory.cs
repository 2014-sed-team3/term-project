
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.GraphicsLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: SolidTaperedDiamondVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a <see
/// cref="VertexShape.SolidTaperedDiamond" />.
/// </summary>
//*****************************************************************************

public class SolidTaperedDiamondVertexDrawingHistory : VertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: SolidTaperedDiamondVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SolidTaperedDiamondVertexDrawingHistory" />
    /// class.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was drawn.
    /// </param>
    ///
    /// <param name="drawingVisual">
    /// The DrawingVisual object that was used to draw the vertex.
    /// </param>
    ///
    /// <param name="drawnAsSelected">
    /// true if the vertex was drawn as selected.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// The half-width of the diamond that was drawn for <paramref
    /// name="vertex" />.
    /// </param>
    //*************************************************************************

    public SolidTaperedDiamondVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Double halfWidth
    )
    : base(vertex, drawingVisual, drawnAsSelected)
    {
        m_dHalfWidth = halfWidth;

        // AssertValid();
    }

    //*************************************************************************
    //  Method: GetBounds()
    //
    /// <summary>
    /// Gets the bounds of the object that was drawn.
    /// </summary>
    ///
    /// <returns>
    /// The object's bounds, as a Geometry.
    /// </returns>
    ///
    /// <remarks>
    /// The object's bounds defines the object's boundary.  It is not
    /// necessarily rectangular.
    /// </remarks>
    //*************************************************************************

    public override Geometry
    GetBounds()
    {
        return ( WpfPathGeometryUtil.GetTaperedDiamond(
            this.VertexLocation, m_dHalfWidth) );
    }

    //*************************************************************************
    //  Method: GetEdgeEndpoint()
    //
    /// <summary>
    /// Gets the endpoint of an edge that is connected to <see
    /// cref="Vertex" />.
    /// </summary>
    ///
    /// <param name="otherEndpoint">
    /// The edge's other endpoint.
    /// </param>
    ///
    /// <param name="edgeEndpoint">
    /// Where the edge endpoint gets stored.  The endpoint is somewhere on <see
    /// cref="Vertex" />.
    /// </param>
    //*************************************************************************

    public override void
    GetEdgeEndpoint
    (
        Point otherEndpoint,
        out Point edgeEndpoint
    )
    {
        AssertValid();

        // Ideally, this method would calculate an intersection point such that
        // the edge would be "aimed toward" the center but terminate on the
        // diamond's bounds, but those calculations are probably very complex.
        //
        // For now, just use the center of the diamond.

        edgeEndpoint = this.VertexLocation;
    }

    //*************************************************************************
    //  Method: GetSelfLoopEndpoint()
    //
    /// <summary>
    /// Gets the endpoint of an edge that is connected to <see
    /// cref="VertexDrawingHistory.Vertex" /> and is a self-loop.
    /// </summary>
    ///
    /// <param name="farthestGraphRectangleEdge">
    /// The edge of the graph rectangle that is farthest from <see
    /// cref="VertexDrawingHistory.Vertex" />.
    /// </param>
    ///
    /// <returns>
    /// The self-loop endpoint.  The endpoint is somewhere on <see
    /// cref="VertexDrawingHistory.Vertex" />.
    /// </returns>
    ///
    /// <remarks>
    /// A self-loop is an edge that connects a vertex to itself.  This method
    /// determines the single endpoint of the self-loop, which gets drawn as a
    /// line looping back to its starting point.
    /// </remarks>
    //*************************************************************************

    public override Point
    GetSelfLoopEndpoint
    (
        RectangleEdge farthestGraphRectangleEdge
    )
    {
        AssertValid();

        return ( GetSelfLoopEndpointOnRectangle(
            GetBoundingSquare(m_dHalfWidth), farthestGraphRectangleEdge) );
    }

    //*************************************************************************
    //  Method: GetLabelLocation()
    //
    /// <summary>
    /// Gets the location at which an annotation label should be drawn.
    /// </summary>
    ///
    /// <param name="labelPosition">
    /// The position of the annotation label.
    /// </param>
    ///
    /// <returns>
    /// The point at which an annotation label should be drawn.
    /// </returns>
    ///
    /// <remarks>
    /// The returned point assumes that the label text height is zero and that
    /// there is zero margin between the vertex and the label.  The caller must
    /// adjust the point for the actual text height and any margin.
    /// </remarks>
    //*************************************************************************

    public override Point
    GetLabelLocation
    (
        VertexLabelPosition labelPosition
    )
    {
        AssertValid();

        return ( GetLabelLocationOnDiamond(labelPosition, m_dHalfWidth) );
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

        Debug.Assert(m_dHalfWidth >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The half-width of the diamond that was drawn.

    protected Double m_dHalfWidth;
}

}
