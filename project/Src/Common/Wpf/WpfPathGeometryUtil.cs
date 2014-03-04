
using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smrf.WpfGraphicsLib
{
//*****************************************************************************
//  Class: WpfPathGeometryUtil
//
/// <summary>
/// Utility methods for working with WPF PathGeometry objects.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class WpfPathGeometryUtil
{
    //*************************************************************************
    //  Method: GetPathGeometryFromPoints()
    //
    /// <summary>
    /// Creates a PathGeometry from a sequence of points.
    /// </summary>
    ///
    /// <param name="startPoint">
    /// The first point in the sequence.
    /// </param>
    ///
    /// <param name="otherPoints">
    /// The other points in the sequence.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry consisting of LineSegments connecting the specified
    /// points.  The PathGeometry is closed and frozen.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    GetPathGeometryFromPoints
    (
        System.Windows.Point startPoint,
        params Point [] otherPoints
    )
    {
        Debug.Assert(otherPoints != null);

        Int32 iOtherPoints = otherPoints.Length;

        Debug.Assert(iOtherPoints > 0);

        PathFigure oPathFigure = new PathFigure();

        oPathFigure.StartPoint = startPoint;

        PathSegmentCollection oPathSegmentCollection =
            new PathSegmentCollection(iOtherPoints);

        for (Int32 i = 0; i < iOtherPoints; i++)
        {
            oPathSegmentCollection.Add(
                new LineSegment(otherPoints[i], true) );
        }

        oPathFigure.Segments = oPathSegmentCollection;
        oPathFigure.IsClosed = true;
        WpfGraphicsUtil.FreezeIfFreezable(oPathFigure);

        PathGeometry oPathGeometry = new PathGeometry();

        oPathGeometry.Figures.Add(oPathFigure);
        WpfGraphicsUtil.FreezeIfFreezable(oPathGeometry);

        return (oPathGeometry);
    }

    //*************************************************************************
    //  Method: GetDiamond()
    //
    /// <summary>
    /// Creates a PathGeometry that represents a diamond centered on a
    /// specified point.
    /// </summary>
    ///
    /// <param name="center">
    /// The diamond's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the diamond.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry that represents the specified diamond.  The PathGeometry
    /// is frozen.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    GetDiamond
    (
        System.Windows.Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        Double dCenterX = center.X;
        Double dCenterY = center.Y;

        return ( GetPathGeometryFromPoints(
            new Point(dCenterX - halfWidth, dCenterY),
            new Point(dCenterX, dCenterY - halfWidth),
            new Point(dCenterX + halfWidth, dCenterY),
            new Point(dCenterX, dCenterY + halfWidth)
            ) );
    }

    //*************************************************************************
    //  Method: GetTriangle()
    //
    /// <summary>
    /// Creates a PathGeometry that represents a triangle centered on a
    /// specified point.
    /// </summary>
    ///
    /// <param name="center">
    /// The triangle's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the square that bounds the triangle.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry that represents the specified triangle.  The
    /// PathGeometry is frozen.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    GetTriangle
    (
        System.Windows.Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        Rect oTriangleBounds =
            WpfGraphicsUtil.TriangleBoundsFromCenterAndHalfWidth(
                center, halfWidth);

        PathGeometry oPathGeometry = GetPathGeometryFromPoints(
            new Point(center.X, oTriangleBounds.Y),
            oTriangleBounds.BottomRight,
            oTriangleBounds.BottomLeft
            );

        return (oPathGeometry);
    }

    //*************************************************************************
    //  Method: GetTaperedDiamond()
    //
    /// <summary>
    /// Creates a PathGeometry that represents a diamond with inward-tapered
    /// edges, centered on a specified point.
    /// </summary>
    ///
    /// <param name="center">
    /// The tapered diamond's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the tapered diamond.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry that represents the specified tapered diamond.  The
    /// PathGeometry is frozen.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    GetTaperedDiamond
    (
        Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        Double dCenterX = center.X;
        Double dCenterY = center.Y;

        Point oLeft = new Point(dCenterX - halfWidth, dCenterY);
        Point oTop = new Point(dCenterX, dCenterY - halfWidth);
        Point oRight = new Point(dCenterX + halfWidth, dCenterY);
        Point oBottom = new Point(dCenterX, dCenterY + halfWidth);

        // The control points for the Bezier segments were determined
        // experimentally.
        //
        // Note that the two control points for each segment are currently the
        // same points, but this can be modified in the future.

        Double dSmallControlPointOffset = halfWidth * 0.65;
        Double dLargeControlPointOffset = halfWidth * 0.65;

        return ( GetPathGeometryFromPathSegments(oLeft, true,

            new BezierSegment(

                new Point(
                    oLeft.X + dSmallControlPointOffset,
                    oTop.Y + dLargeControlPointOffset
                    ),

                new Point(
                    oLeft.X + dLargeControlPointOffset,
                    oTop.Y + dSmallControlPointOffset
                    ),

                oTop, true
                ),

            new BezierSegment(

                new Point(
                    oRight.X - dLargeControlPointOffset,
                    oTop.Y + dSmallControlPointOffset
                    ),
            
                new Point(
                    oRight.X - dSmallControlPointOffset,
                    oTop.Y + dLargeControlPointOffset
                    ),

                oRight, true
                ),

            new BezierSegment(

                new Point(
                    oRight.X - dSmallControlPointOffset,
                    oBottom.Y - dLargeControlPointOffset
                    ),
            
                new Point(
                    oRight.X - dLargeControlPointOffset,
                    oBottom.Y - dSmallControlPointOffset
                    ),
            
                oBottom, true
                ),

            new BezierSegment(

                new Point(
                    oLeft.X + dLargeControlPointOffset,
                    oBottom.Y - dSmallControlPointOffset
                    ),
        
                new Point(
                    oLeft.X + dSmallControlPointOffset,
                    oBottom.Y - dLargeControlPointOffset
                    ),
        
                oLeft, true
                )
            ) );
    }


    //*************************************************************************
    //  Method: GetRoundedX()
    //
    /// <summary>
    /// Creates a PathGeometry that represents an X with outward-tapered
    /// edges, centered on a specified point.
    /// </summary>
    ///
    /// <param name="center">
    /// The rounded X's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the rounded X.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry that represents the specified rounded X.  The
    /// PathGeometry is frozen.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    GetRoundedX
    (
        Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        Double dCenterX = center.X;
        Double dCenterY = center.Y;

        Point
            topLeft = new Point(dCenterX - halfWidth, dCenterY - halfWidth),
            topRight = new Point(dCenterX + halfWidth, dCenterY - halfWidth),
            bottomRight = new Point(dCenterX + halfWidth, dCenterY + halfWidth),
            bottomLeft = new Point(dCenterX - halfWidth, dCenterY + halfWidth);

        Double middleOffset = .75 * halfWidth;

        Point
           topMiddle = new Point(dCenterX, dCenterY - middleOffset),
           rightMiddle = new Point(dCenterX + middleOffset, dCenterY),
           bottomMiddle = new Point(dCenterX, dCenterY + middleOffset),
           leftMiddle = new Point(dCenterX - middleOffset, dCenterY);

        // The control points for the Bezier segments were determined
        // experimentally.

        Double dSmallControlPointOffset = halfWidth * 0.25;
        Double dLargeControlPointOffset = halfWidth * 0.45;

        return (GetPathGeometryFromPathSegments(topLeft, true,

            new BezierSegment(

                new Point(
                    topLeft.X + dSmallControlPointOffset,
                    topMiddle.Y - dLargeControlPointOffset
                    ),

                new Point(
                    topLeft.X + dLargeControlPointOffset,
                    topMiddle.Y - dSmallControlPointOffset
                    ),

                topMiddle, true
                ),

            new BezierSegment(

                new Point(
                    topRight.X - dLargeControlPointOffset,
                    topMiddle.Y - dSmallControlPointOffset
                    ),

                new Point(
                    topRight.X - dSmallControlPointOffset,
                    topMiddle.Y - dLargeControlPointOffset
                    ),

                topRight, true
                ),

            new BezierSegment(

                new Point(
                    rightMiddle.X + dLargeControlPointOffset,
                    topRight.Y + dSmallControlPointOffset
                    ),

                new Point(
                    rightMiddle.X + dSmallControlPointOffset,
                    topRight.Y + dLargeControlPointOffset
                    ),

                rightMiddle, true
                ),

            new BezierSegment(

                new Point(
                    rightMiddle.X + dSmallControlPointOffset,
                    bottomRight.Y - dLargeControlPointOffset
                ),

                new Point(
                    rightMiddle.X + dLargeControlPointOffset,
                    bottomRight.Y - dSmallControlPointOffset
                ),

                bottomRight, true
                ),

        
            new BezierSegment(

                new Point(
                    bottomRight.X - dSmallControlPointOffset,
                    bottomMiddle.Y + dLargeControlPointOffset
                ),

                new Point(
                    bottomRight.X - dLargeControlPointOffset,
                    bottomMiddle.Y + dSmallControlPointOffset
                ),

                bottomMiddle, true
                ),

        
            new BezierSegment(

                new Point(
                    bottomLeft.X + dLargeControlPointOffset,
                    bottomMiddle.Y + dSmallControlPointOffset
                ),

                new Point(
                    bottomLeft.X + dSmallControlPointOffset,
                    bottomMiddle.Y + dLargeControlPointOffset
                ),

                bottomLeft, true
                ),

        
            new BezierSegment(

                new Point(
                    leftMiddle.X - dLargeControlPointOffset,
                    bottomLeft.Y - dSmallControlPointOffset
                ),

                new Point(
                    leftMiddle.X - dSmallControlPointOffset,
                    bottomLeft.Y - dLargeControlPointOffset
                ),

                leftMiddle, true
                ),

        
            new BezierSegment(

                new Point(
                    leftMiddle.X - dSmallControlPointOffset,
                    topLeft.Y + dLargeControlPointOffset
                ),

                new Point(
                    leftMiddle.X - dLargeControlPointOffset,
                    topLeft.Y + dSmallControlPointOffset
                ),

                topLeft, true
                )
            ));
    }

    //*************************************************************************
    //  Method: GetCircleSegment()
    //
    /// <summary>
    /// Creates a PathGeometry that represents a segment of a circle.
    /// </summary>
    ///
    /// <param name="center">
    /// The circle's center.
    /// </param>
    ///
    /// <param name="radius">
    /// The circle's radius.  Must be greater than zero.
    /// </param>
    ///
    /// <param name="angleRadians">
    /// The angle of the segment, in radians.  Must be between 0 and PI.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry that represents the specified circle segment.  The
    /// PathGeometry is frozen.
    /// </returns>
    ///
    /// <remarks>
    /// The left radius of the sector points up.  The right radius of the
    /// sector is <paramref name="angleRadians" /> clockwise from the left
    /// radius.
    ///
    /// <para>
    /// The line segments that form the circle segment will be stroked when the
    /// circle segment is drawn.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static PathGeometry
    GetCircleSegment
    (
        Point center,
        Double radius,
        Double angleRadians
    )
    {
        Debug.Assert(radius > 0);
        Debug.Assert(angleRadians >= 0);

        // If the following restriction is ever removed, the isLargeArc and
        // sweepDirection arguments to the ArcSegment constructor below will
        // have to be modified based on the angle.

        Debug.Assert(angleRadians <= Math.PI);

        Double dAngleFromXAxisRadians = (Math.PI / 2.0) - angleRadians;
        Double dArcEndX = center.X + radius * Math.Cos(dAngleFromXAxisRadians);
        Double dArcEndY = center.Y - radius * Math.Sin(dAngleFromXAxisRadians);

        return ( GetPathGeometryFromPathSegments(center, true,

            // Add the left radius.

            new LineSegment(new Point(center.X, center.Y - radius), true),

            // Add the arc.

            new ArcSegment(
                new Point(dArcEndX, dArcEndY), new Size(radius, radius), 0,
                false, SweepDirection.Clockwise, true),

            // Add the right radius.

            new LineSegment(new Point(center.X, center.Y), true)
            ) );
    }

    //*************************************************************************
    //  Method: GetQuadraticBezierCurve()
    //
    /// <summary>
    /// Gets a quadratic Bezier curve given the curve's endpoints and a Bezier
    /// control point.
    /// </summary>
    ///
    /// <param name="startPoint">
    /// The curve's start point.
    /// </param>
    ///
    /// <param name="endPoint">
    /// The curve's endpoint.
    /// </param>
    ///
    /// <param name="controlPoint">
    /// The curve's Bezier control point.
    /// </param>
    ///
    /// <returns>
    /// A new, frozen Bezier curve, as a PathGeometry.
    /// </returns>
    //*************************************************************************

    public static PathGeometry
    GetQuadraticBezierCurve
    (
        Point startPoint,
        Point endPoint,
        Point controlPoint
    )
    {
        QuadraticBezierSegment oQuadraticBezierSegment =
            new QuadraticBezierSegment(controlPoint, endPoint, true);

        PathFigure oPathFigure = new PathFigure();
        oPathFigure.StartPoint = startPoint;
        oPathFigure.Segments.Add(oQuadraticBezierSegment);

        PathGeometry oQuadraticBezierCurve = new PathGeometry();
        oQuadraticBezierCurve.Figures.Add(oPathFigure);
        WpfGraphicsUtil.FreezeIfFreezable(oQuadraticBezierCurve);

        return (oQuadraticBezierCurve);
    }

    //*************************************************************************
    //  Method: ReverseQuadraticBezierCurve()
    //
    /// <summary>
    /// Duplicates and reverses a quadratic Bezier curve.
    /// </summary>
    ///
    /// <param name="quadraticBezierCurve">
    /// The quadratic Bezier curve to duplicate and reverse.  This does not get
    /// modified.
    /// </param>
    ///
    /// <returns>
    /// A new frozen, reversed Bezier curve, as a PathGeometry.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates a new quadratic Bezier curve that uses the same
    /// control point as <paramref name="quadraticBezierCurve" />, but with the
    /// endpoints swapped.
    /// </remarks>
    //*************************************************************************

    public static PathGeometry
    ReverseQuadraticBezierCurve
    (
        PathGeometry quadraticBezierCurve
    )
    {
        Debug.Assert(quadraticBezierCurve != null);

        Debug.Assert(quadraticBezierCurve.Figures.Count == 1);
        PathFigure oOriginalPathFigure = quadraticBezierCurve.Figures[0];
        Debug.Assert(oOriginalPathFigure.Segments.Count == 1);

        Debug.Assert(oOriginalPathFigure.Segments[0] is
            QuadraticBezierSegment);

        QuadraticBezierSegment oOriginalQuadraticBezierSegment =
            (QuadraticBezierSegment)oOriginalPathFigure.Segments[0];

        return ( GetQuadraticBezierCurve(
            oOriginalQuadraticBezierSegment.Point2,
            oOriginalPathFigure.StartPoint,
            oOriginalQuadraticBezierSegment.Point1
            ) );
    }

    //*************************************************************************
    //  Method: GetCurveThroughPoints()
    //
    /// <summary>
    /// Gets a continuous curve through a specified set of points.
    /// </summary>
    ///
    /// <param name="points">
    /// The points through which the curve will pass.  Must contain at least
    /// two points.
    /// </param>
    ///
    /// <param name="tension">
    /// Determines the amount of curvature.  Can be any value, including
    /// negative values.  Zero yields straight lines.
    /// </param>
    ///
    /// <param name="tolerance">
    /// Determines the accuracy of the polyline approximation to a continuous
    /// curve.  Each line segment in the polyline has a length of approximately
    /// tolerance WPF units.  Must be greater than zero.
    /// </param>
    ///
    /// <returns>
    /// A new frozen curve, as a PathGeometry.
    /// </returns>
    ///
    /// <remarks>
    /// This method and <see cref="AddPointsToPolyLineSegment" /> were adapted
    /// from Charles Petzold's article "Canonical Splines in WPF and
    /// Silverlight" at http://www.charlespetzold.com/blog/2009/01/
    /// Canonical-Splines-in-WPF-and-Silverlight.html.  The methods here are
    /// considerably simpler because they don't offer all of the features
    /// presented in the Peztold article.
    /// </remarks>
    //*************************************************************************

    public static PathGeometry
    GetCurveThroughPoints
    (
        IList<Point> points,
        Double tension,
        Double tolerance
    )
    {
        Debug.Assert(points != null);
        Debug.Assert(points.Count >= 2);
        Debug.Assert(tolerance > 0);

        PolyLineSegment oPolyLineSegment = new PolyLineSegment();

        if (points.Count == 2)
        {
            AddPointsToPolyLineSegment(oPolyLineSegment, points[0], points[0],
                points[1], points[1], tension, tolerance);
        }
        else
        {
            Int32 iPoints = points.Count;

            for (Int32 i = 0; i < iPoints; i++)
            {
                if (i == 0)
                {
                    AddPointsToPolyLineSegment(oPolyLineSegment, points[0],
                        points[0], points[1], points[2], tension, tolerance);
                }

                else if (i == iPoints - 2)
                {
                    AddPointsToPolyLineSegment(oPolyLineSegment, points[i - 1],
                        points[i], points[i + 1], points[i + 1], tension,
                        tolerance);
                }
                else if (i != iPoints - 1)
                {
                    AddPointsToPolyLineSegment(oPolyLineSegment, points[i - 1],
                        points[i], points[i + 1], points[i + 2], tension,
                        tolerance);
                }
            }
        }

        return ( GetPathGeometryFromPathSegments(
            points[0], false, oPolyLineSegment) );
    }

    //*************************************************************************
    //  Method: AddPointsToPolyLineSegment()
    //
    /// <summary>
    /// Adds points to a PolyLineSegment while getting a curve through a
    /// specified set of points.
    /// </summary>
    ///
    /// <param name="oPolyLineSegment">
    /// The segment to add the points to.
    /// </param>
    ///
    /// <param name="oPoint0">
    /// The point preceding <paramref name="oPoint1" />.
    /// </param>
    ///
    /// <param name="oPoint1">
    /// The start point for adding points.
    /// </param>
    ///
    /// <param name="oPoint2">
    /// The end point for adding points.
    /// </param>
    ///
    /// <param name="oPoint3">
    /// The point following <paramref name="oPoint2" />.
    /// </param>
    ///
    /// <param name="dTension">
    /// Determines the amount of curvature.  Can be any value, including
    /// negative values.  Zero yields straight lines.
    /// </param>
    ///
    /// <param name="dTolerance">
    /// Determines the accuracy of the polyline approximation to a continuous
    /// curve.  Each line segment in the polyline has a length of approximately
    /// tolerance WPF units.  Must be greater than zero.
    /// </param>
    ///
    /// <remarks>
    /// See <see cref="GetCurveThroughPoints" /> for details on where this code
    /// came from.
    /// </remarks>
    //*************************************************************************

    private static void
    AddPointsToPolyLineSegment
    (
        PolyLineSegment oPolyLineSegment,
        Point oPoint0,
        Point oPoint1,
        Point oPoint2,
        Point oPoint3,
        Double dTension,
        Double dTolerance
    )
    {
        Debug.Assert(oPolyLineSegment != null);
        Debug.Assert(dTolerance > 0);

        Int32 iPoints = (Int32)( (Math.Abs(oPoint1.X - oPoint2.X) +
            Math.Abs(oPoint1.Y - oPoint2.Y) ) / dTolerance);

        PointCollection oPolyLineSegmentPoints = oPolyLineSegment.Points;

        if (iPoints <= 2)
        {
            oPolyLineSegmentPoints.Add(oPoint2);
        }
        else
        {
            Double dSX1 = dTension * (oPoint2.X - oPoint0.X);
            Double dSY1 = dTension * (oPoint2.Y - oPoint0.Y);
            Double dSX2 = dTension * (oPoint3.X - oPoint1.X);
            Double dSY2 = dTension * (oPoint3.Y - oPoint1.Y);

            Double dAX = dSX1 + dSX2 + 2 * oPoint1.X - 2 * oPoint2.X;
            Double dAY = dSY1 + dSY2 + 2 * oPoint1.Y - 2 * oPoint2.Y;
            Double dBX = -2 * dSX1 - dSX2 - 3 * oPoint1.X + 3 * oPoint2.X;
            Double dBY = -2 * dSY1 - dSY2 - 3 * oPoint1.Y + 3 * oPoint2.Y;

            Double dCX = dSX1;
            Double dCY = dSY1;
            Double dDX = oPoint1.X;
            Double dDY = oPoint1.Y;

            // Note that this starts at 1, not 0.

            for (int i = 1; i < iPoints; i++)
            {
                Double t = (Double)i / (iPoints - 1);

                Point oPoint = new Point(
                    dAX * t * t * t + dBX * t * t + dCX * t + dDX,
                    dAY * t * t * t + dBY * t * t + dCY * t + dDY
                    );

                oPolyLineSegmentPoints.Add(oPoint);
            }
        }
    }

    //*************************************************************************
    //  Method: GetPathGeometryFromPathSegments()
    //
    /// <summary>
    /// Creates a PathGeometry from a start point and a collection of
    /// PathSegment objects.
    /// </summary>
    ///
    /// <param name="oStartPoint">
    /// The PathGeometry's start point.
    /// </param>
    ///
    /// <param name="bPathFigureIsFilled">
    /// true if the PathFigure that contains the PathSegment objects should be
    /// filled.
    /// </param>
    ///
    /// <param name="aoPathSegments">
    /// The PathSegment objects to use.
    /// </param>
    ///
    /// <returns>
    /// A PathGeometry created from <paramref name="aoPathSegments" />.  The
    /// PathGeometry is frozen.
    /// </returns>
    //*************************************************************************

    private static PathGeometry
    GetPathGeometryFromPathSegments
    (
        Point oStartPoint,
        Boolean bPathFigureIsFilled,
        params PathSegment[] aoPathSegments
    )
    {
        Debug.Assert(aoPathSegments != null);

        PathFigure oPathFigure = new PathFigure();
        oPathFigure.StartPoint = oStartPoint;
        oPathFigure.IsFilled = bPathFigureIsFilled;
        PathSegmentCollection oSegments = oPathFigure.Segments;

        foreach (PathSegment oPathSegment in aoPathSegments)
        {
            WpfGraphicsUtil.FreezeIfFreezable(oPathSegment);
            oSegments.Add(oPathSegment);
        }

        WpfGraphicsUtil.FreezeIfFreezable(oPathFigure);
        PathGeometry oPathGeometry = new PathGeometry();
        oPathGeometry.Figures.Add(oPathFigure);
        WpfGraphicsUtil.FreezeIfFreezable(oPathGeometry);

        return (oPathGeometry);
    }

    //*************************************************************************
    //  Public fields
    //*************************************************************************

    /// Used in clover width calculations.

    public static Double SqrtOf2 = Math.Sqrt(2);
}

}
