﻿
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;
using Smrf.GraphicsLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: TriangleVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a <see
/// cref="VertexShape.Triangle" />.
/// </summary>
//*****************************************************************************

public class TriangleVertexDrawingHistory : VertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: TriangleVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TriangleVertexDrawingHistory" />
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
    /// The half-width of the triangle that was drawn for <paramref
    /// name="vertex" />.
    /// </param>
    //*************************************************************************

    public TriangleVertexDrawingHistory
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
    /// Gets the half-width of the triangle that was drawn.
    /// </summary>
    ///
    /// <value>
    /// The half-width of the triangle that was drawn, as a Double.
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
        return (WpfPathGeometryUtil.GetTriangle(
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

        // Instead of doing geometry calculations similar to what is done in 
        // VertexDrawingHistory.GetEdgePointOnRectangle(), make use of that
        // method by making the triangle look like a rectangle.  First, figure
        // out how to rotate the triangle about the vertex location so that the
        // side containing the endpoint is vertical and to the right of the
        // vertex location.

        Double dEdgeAngle = WpfGraphicsUtil.GetAngleBetweenPointsRadians(
            oVertexLocation, otherEndpoint);

        Double dEdgeAngleDegrees = MathUtil.RadiansToDegrees(dEdgeAngle);

        Double dAngleToRotateDegrees;

        if (dEdgeAngleDegrees >= -30.0 && dEdgeAngleDegrees < 90.0)
        {
            dAngleToRotateDegrees = 30.0;
        }
        else if (dEdgeAngleDegrees >= -150.0 && dEdgeAngleDegrees < -30.0)
        {
            dAngleToRotateDegrees = 270.0;
        }
        else
        {
            dAngleToRotateDegrees = 150.0;
        }

        // Now create a rotated rectangle that is centered on the vertex
        // location and that has the vertical, endpoint-containing triangle
        // side as the rectangle's right edge.

        Double dWidth = 2.0 * m_dHalfWidth;

        Rect oRotatedRectangle = new Rect(
            oVertexLocation.X,
            oVertexLocation.Y - m_dHalfWidth,
            dWidth * WpfGraphicsUtil.Tangent30Degrees,
            dWidth
            );

        Matrix oMatrix = WpfGraphicsUtil.GetRotatedMatrix(oVertexLocation,
            dAngleToRotateDegrees);

        // Rotate the other vertex location.

        Point oRotatedOtherVertexLocation = oMatrix.Transform(otherEndpoint);

        // GetEdgeEndpointOnRectangle will compute an endpoint on the
        // rectangle's right edge.

        Point oRotatedEdgeEndpoint;
        
        GetEdgeEndpointOnRectangle(oVertexLocation, oRotatedRectangle,
            oRotatedOtherVertexLocation, out oRotatedEdgeEndpoint);

        // Now rotate the edge endpoint in the other direction.

        oMatrix = WpfGraphicsUtil.GetRotatedMatrix(oVertexLocation,
            -dAngleToRotateDegrees);

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

        // Return a point at one of the triangle's corners.

        Point oVertexLocation = this.VertexLocation;
        Double dVertexX = oVertexLocation.X;
        Double dVertexY = oVertexLocation.Y;

        Double dX = dVertexX;
        Double dY = dVertexY + m_dHalfWidth * WpfGraphicsUtil.Tangent30Degrees;

        switch (farthestGraphRectangleEdge)
        {
            case RectangleEdge.Top:

                dY = dVertexY - m_dHalfWidth / WpfGraphicsUtil.Cosine30Degrees;
                break;

            case RectangleEdge.Left:

                dX -= m_dHalfWidth;
                break;

            case RectangleEdge.Right:

                dX += m_dHalfWidth;
                break;

            case RectangleEdge.Bottom:

                break;

            default:

                Debug.Assert(false);
                break;
        }

        return ( new Point(dX, dY) );
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

        Rect oVertexBounds =
            WpfGraphicsUtil.TriangleBoundsFromCenterAndHalfWidth(
                this.VertexLocation, m_dHalfWidth);

        Double dCenterX = oVertexBounds.Left + oVertexBounds.Width / 2.0;

        // These were determined experimentally to avoid trigonometry method
        // calls and to make the labels look good.

        Double dCenterY = oVertexBounds.Top +
            1.2 * (oVertexBounds.Height / 2.0);

        Double dOffsetX = m_dHalfWidth * 1.0;

        switch (labelPosition)
        {
            case VertexLabelPosition.TopLeft:
            case VertexLabelPosition.TopCenter:
            case VertexLabelPosition.TopRight:

                return ( new Point(dCenterX, oVertexBounds.Top) );

            case VertexLabelPosition.MiddleLeft:

                return ( new Point(dCenterX - dOffsetX, dCenterY) );

            case VertexLabelPosition.MiddleCenter:

                return ( new Point(dCenterX, dCenterY) );

            case VertexLabelPosition.MiddleRight:

                return ( new Point(dCenterX + dOffsetX, dCenterY) );

            case VertexLabelPosition.BottomLeft:

                return (oVertexBounds.BottomLeft);

            case VertexLabelPosition.BottomCenter:

                return ( new Point(dCenterX, oVertexBounds.Bottom) );

            case VertexLabelPosition.BottomRight:

                return (oVertexBounds.BottomRight);

            default:

                Debug.Assert(false);
                return (this.VertexLocation);
        }
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

    /// The half-width of the triangle that was drawn.

    protected Double m_dHalfWidth;
}

}
