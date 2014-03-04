﻿
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
//  Class: DiamondVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a <see
/// cref="VertexShape.Diamond" />.
/// </summary>
//*****************************************************************************

public class DiamondVertexDrawingHistory : VertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: DiamondVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DiamondVertexDrawingHistory" />
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

    public DiamondVertexDrawingHistory
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
    //  Property: HalfWidth
    //
    /// <summary>
    /// Gets the half-width of the diamond that was drawn.
    /// </summary>
    ///
    /// <value>
    /// The half-width of the diamond that was drawn, as a Double.
    /// </value>
    //*************************************************************************

    public Double
    HalfWidth
    {
        get
        {
            AssertValid();

            return (m_dHalfWidth);
        }
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
        return ( WpfPathGeometryUtil.GetDiamond(
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

        Point oVertexLocation = this.VertexLocation;

        // A diamond is just a rotated square, so the
        // VertexDrawingHistory.GetEdgePointOnRectangle() can be used if the
        // diamond and the other vertex location are first rotated 45 degrees
        // about the diamond's center.

        Double dHalfSquareWidth = m_dHalfWidth / Math.Sqrt(2.0);

        Rect oRotatedDiamond = new Rect(
            oVertexLocation.X - dHalfSquareWidth,
            oVertexLocation.Y - dHalfSquareWidth,
            2.0 * dHalfSquareWidth,
            2.0 * dHalfSquareWidth
            );

        Matrix oMatrix = WpfGraphicsUtil.GetRotatedMatrix(oVertexLocation, 45);
        Point oRotatedOtherVertexLocation = oMatrix.Transform(otherEndpoint);

        Point oRotatedEdgeEndpoint;
        
        GetEdgeEndpointOnRectangle(oVertexLocation, oRotatedDiamond,
            oRotatedOtherVertexLocation, out oRotatedEdgeEndpoint);

        // Now rotate the computed edge endpoint in the other direction.

        oMatrix = WpfGraphicsUtil.GetRotatedMatrix(oVertexLocation, -45);

        edgeEndpoint = oMatrix.Transform(oRotatedEdgeEndpoint);
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
