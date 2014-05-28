
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: EdgeDrawingHistory
//
/// <summary>
/// Retains information about how one edge was drawn.
/// </summary>
//*****************************************************************************

public class EdgeDrawingHistory : DrawingHistory
{
    //*************************************************************************
    //  Constructor: EdgeDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeDrawingHistory" />
    /// class.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge that was drawn.
    /// </param>
    ///
    /// <param name="drawingVisual">
    /// The DrawingVisual object that was used to draw the edge.
    /// </param>
    ///
    /// <param name="drawnAsSelected">
    /// true if the edge was drawn as selected.
    /// </param>
    ///
    /// <param name="width">
    /// The width of <paramref name="edge" />, in WPF units.
    /// </param>
    //*************************************************************************

    public EdgeDrawingHistory
    (
        IEdge edge,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Double width
    )
    : base(drawingVisual, drawnAsSelected)
    {
        m_oEdge = edge;
        m_dWidth = width;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Edge
    //
    /// <summary>
    /// Gets the edge that was drawn.
    /// </summary>
    ///
    /// <value>
    /// The edge that was drawn, as an <see cref="IEdge" />
    /// </value>
    //*************************************************************************

    public IEdge
    Edge
    {
        get
        {
            AssertValid();

            return (m_oEdge);
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
    ///
    /// <para>
    /// Do not call this method if the edge is a self-loop.  It cannot
    /// calculate the bounds of a self-loop and will throw an exception.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public override Geometry
    GetBounds()
    {
        AssertValid();

        if (m_oEdge.IsSelfLoop)
        {
            Debug.Assert(false);
            throw new InvalidOperationException("Edge is self-loop.");
        }

        // Start with a rectangle that has the same dimensions as the edge, but
        // that starts at the origin and has an angle of zero.

        Point oVertex1Location = WpfGraphicsUtil.PointFToWpfPoint(
            m_oEdge.Vertex1.Location);

        Point oVertex2Location = WpfGraphicsUtil.PointFToWpfPoint(
            m_oEdge.Vertex2.Location);

        Double dLength = WpfGraphicsUtil.GetDistanceBetweenPoints(
            oVertex1Location, oVertex2Location);

        Double dAngleDegrees = MathUtil.RadiansToDegrees(
            WpfGraphicsUtil.GetAngleBetweenPointsRadians(
                oVertex1Location, oVertex2Location) );

        Double dHalfWidth = m_dWidth / 2.0;

        Point[] ao4BoundingPoints = new Point[4] {
            new Point(      0, -dHalfWidth),
            new Point(dLength, -dHalfWidth),
            new Point(dLength,  dHalfWidth),
            new Point(      0,  dHalfWidth),
            };

        // Rotate the rectangle so it is at the same angle as the edge.

        TransformPoints(new RotateTransform(dAngleDegrees), ao4BoundingPoints);

        // Translate the rotated rectangle to the location of the edge's first
        // endpoint.

        TransformPoints(
            new TranslateTransform(oVertex1Location.X, oVertex1Location.Y),
            ao4BoundingPoints);

        // Create a PathGeometry from the bounding points.

        return ( WpfPathGeometryUtil.GetPathGeometryFromPoints(
            ao4BoundingPoints[0],
            ao4BoundingPoints[1],
            ao4BoundingPoints[2],
            ao4BoundingPoints[3]
            ) );
    }

    //*************************************************************************
    //  Method: TransformPoints()
    //
    /// <summary>
    /// Transforms an array of 4 bounding points that define the edge.
    /// </summary>
    ///
    /// <param name="oTransform">
    /// The Transform object to use.
    /// </param>
    ///
    /// <param name="ao4BoundingPoints">
    /// The 4 bounding points that define the edge.
    /// </param>
    //*************************************************************************

    protected void
    TransformPoints
    (
        Transform oTransform,
        Point [] ao4BoundingPoints
    )
    {
        Debug.Assert(oTransform != null);
        Debug.Assert(ao4BoundingPoints != null);
        Debug.Assert(ao4BoundingPoints.Length == 4);
        AssertValid();

        for (Int32 i = 0; i < 4; i++)
        {
            ao4BoundingPoints[i] =
                oTransform.Transform( ao4BoundingPoints[i] );
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

        Debug.Assert(m_oEdge != null);
        Debug.Assert(m_dWidth >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The edge that was drawn.

    protected IEdge m_oEdge;

    /// The width of the edge.

    protected Double m_dWidth;
}

}
