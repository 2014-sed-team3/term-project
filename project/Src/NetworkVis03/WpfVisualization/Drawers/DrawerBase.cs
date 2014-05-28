
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using Smrf.NodeXL.Core;
using Smrf.WpfGraphicsLib;
using Smrf.AppLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: DrawerBase
//
/// <summary>
/// Base class for classes that perform drawing operations.
/// </summary>
//*****************************************************************************

public class DrawerBase : VisualizationBase
{
    //*************************************************************************
    //  Constructor: DrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DrawerBase" /> class.
    /// </summary>
    //*************************************************************************

    public DrawerBase()
    {
        m_dGraphScale = 1.0;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: GraphScale
    //
    /// <summary>
    /// Gets or sets a value that determines the scale of the graph's vertices
    /// and edges.
    /// </summary>
    ///
    /// <value>
    /// A value that determines the scale of the graph's vertices and edges.
    /// Must be between <see cref="GraphDrawer.MinimumGraphScale" /> and <see
    /// cref="GraphDrawer.MaximumGraphScale" />.  The default value is 1.0.
    /// </value>
    ///
    /// <remarks>
    /// If the value is anything besides 1.0, the graph's vertices and edges
    /// are shrunk while their positions remain the same.  If it is set to 0.5,
    /// for example, the vertices are half their normal size and the edges are
    /// half their normal width.  The overall size of the graph is not
    /// affected.
    /// </remarks>
    //*************************************************************************

    public Double
    GraphScale
    {
        get
        {
            AssertValid();

            return (m_dGraphScale);
        }

        set
        {
            m_dGraphScale = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TryGetVertexDrawingHistory()
    //
    /// <summary>
    /// Attempts to get a VertexDrawingHistory from a vertex's metadata.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the VertexDrawingHistory for.
    /// </param>
    ///
    /// <param name="oVertexDrawingHistory">
    /// Where the VertexDrawingHistory gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex contains a VertexDrawingHistory.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetVertexDrawingHistory
    (
        IVertex oVertex,
        out VertexDrawingHistory oVertexDrawingHistory
    )
    {
        Debug.Assert(oVertex != null);

        oVertexDrawingHistory = null;
        Object oVertexDrawingHistoryAsObject;

        if ( !oVertex.TryGetValue(ReservedMetadataKeys.VertexDrawingHistory,
            typeof(VertexDrawingHistory), out oVertexDrawingHistoryAsObject) )
        {
            return (false);
        }

        oVertexDrawingHistory =
            (VertexDrawingHistory)oVertexDrawingHistoryAsObject;

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetEdgeDrawingHistory()
    //
    /// <summary>
    /// Attempts to get an EdgeDrawingHistory from an edge's metadata.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to get the EdgeDrawingHistory for.
    /// </param>
    ///
    /// <param name="oEdgeDrawingHistory">
    /// Where the EdgeDrawingHistory gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the edge contains an EdgeDrawingHistory.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetEdgeDrawingHistory
    (
        IEdge oEdge,
        out EdgeDrawingHistory oEdgeDrawingHistory
    )
    {
        Debug.Assert(oEdge != null);

        oEdgeDrawingHistory = null;
        Object oEdgeDrawingHistoryAsObject;

        if ( !oEdge.TryGetValue(ReservedMetadataKeys.EdgeDrawingHistory,
            typeof(EdgeDrawingHistory), out oEdgeDrawingHistoryAsObject) )
        {
            return (false);
        }

        oEdgeDrawingHistory = (EdgeDrawingHistory)oEdgeDrawingHistoryAsObject;
        return (true);
    }


    //*************************************************************************
    //  Method: TryGetColorValue()
    //
    /// <summary>
    /// Attempts to get a color from a graph, vertex, or edge's metadata.
    /// </summary>
    ///
    /// <param name="oMetadataProvider">
    /// The graph, vertex, or edge to get the color for.
    /// </param>
    ///
    /// <param name="sKey">
    /// The color's key.
    /// </param>
    ///
    /// <param name="oColor">
    /// Where the color gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph, vertex, or edge contains the specified color key.
    /// </returns>
    ///
    /// <remarks>
    /// The value of the specified key can be of type
    /// System.Windows.Media.Color or System.Drawing.Color.  If it is of type
    /// System.Drawing.Color, it gets converted to type
    /// System.Windows.Media.Color.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetColorValue
    (
        IMetadataProvider oMetadataProvider,
        String sKey,
        out Color oColor
    )
    {
        Debug.Assert(oMetadataProvider != null);
        AssertValid();

        oColor = Color.FromRgb(0, 0, 0);

        Object oColorAsObject;

        if ( !oMetadataProvider.TryGetValue(sKey, out oColorAsObject) )
        {
            return (false);
        }

        if ( typeof(System.Windows.Media.Color).IsInstanceOfType(
            oColorAsObject) )
        {
            oColor = (System.Windows.Media.Color)oColorAsObject;
        }
        else if ( typeof(System.Drawing.Color).IsInstanceOfType(
            oColorAsObject) )
        {
            oColor = WpfGraphicsUtil.ColorToWpfColor(
                (System.Drawing.Color)oColorAsObject );
        }
        else
        {
            throw new InvalidOperationException( String.Format(

                "The graph, vertex, or edge value with the key \"{0}\" is of"
                + " type {1}.  The expected type is either"
                + " System.Windows.Media.Color or System.Drawing.Color."
                ,
                sKey,
                oColorAsObject.GetType().FullName
                ) );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: CreateFrozenSolidColorBrush()
    //
    /// <summary>
    /// Creates a SolidColorBrush and freezes it.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The brush color.
    /// </param>
    ///
    /// <returns>
    /// A new frozen SolidColorBrush.
    /// </returns>
    //*************************************************************************

    protected SolidColorBrush
    CreateFrozenSolidColorBrush
    (
        Color oColor
    )
    {
        // AssertValid();

        SolidColorBrush oSolidColorBrush = new SolidColorBrush(oColor);

        WpfGraphicsUtil.FreezeIfFreezable(oSolidColorBrush);

        return (oSolidColorBrush);
    }

    //*************************************************************************
    //  Method: CreateFrozenPen()
    //
    /// <overloads>
    /// Creates a Pen and freezes it.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a Pen with a specified thickness and dash style, and freezes
    /// it.
    /// </summary>
    ///
    /// <param name="oBrush">
    /// The brush to use.
    /// </param>
    ///
    /// <param name="dThickness">
    /// The pen thickness.
    /// </param>
    ///
    /// <param name="oDashStyle">
    /// The pen's dash style.
    /// </param>
    ///
    /// <returns>
    /// A new frozen Pen.
    /// </returns>
    //*************************************************************************

    protected Pen
    CreateFrozenPen
    (
        Brush oBrush,
        Double dThickness,
        DashStyle oDashStyle
    )
    {
        Debug.Assert(oBrush != null);
        Debug.Assert(dThickness > 0);
        Debug.Assert(oDashStyle != null);
        // AssertValid();

        return ( CreateFrozenPen(oBrush, dThickness, oDashStyle,
            PenLineCap.Flat) );
    }

    //*************************************************************************
    //  Method: CreateFrozenPen()
    //
    /// <summary>
    /// Creates a Pen with a specified thickness, dash style and end line cap,
    /// and freezes it.
    /// </summary>
    ///
    /// <param name="oBrush">
    /// The brush to use.
    /// </param>
    ///
    /// <param name="dThickness">
    /// The pen thickness.
    /// </param>
    ///
    /// <param name="oDashStyle">
    /// The pen's dash style.
    /// </param>
    ///
    /// <param name="eLineCap">
    /// The line cap to use for both ends of the pen.
    /// </param>
    ///
    /// <returns>
    /// A new frozen Pen.
    /// </returns>
    //*************************************************************************

    protected Pen
    CreateFrozenPen
    (
        Brush oBrush,
        Double dThickness,
        DashStyle oDashStyle,
        PenLineCap eLineCap
    )
    {
        Debug.Assert(oBrush != null);
        Debug.Assert(dThickness > 0);
        Debug.Assert(oDashStyle != null);
        // AssertValid();

        Pen oPen = new Pen(oBrush, dThickness);
        oPen.DashStyle = oDashStyle;
        oPen.StartLineCap = eLineCap;
        oPen.EndLineCap = eLineCap;
        WpfGraphicsUtil.FreezeIfFreezable(oPen);

        return (oPen);
    }

    //*************************************************************************
    //  Method: DrawLabelBackground()
    //
    /// <summary>
    /// Draws a background behind a label to make it more legible.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oFormattedText">
    /// The FormattedText object to use.
    /// </param>
    ///
    /// <param name="oFormattedTextColor">
    /// The color of the FormattedText's foreground brush.
    /// </param>
    ///
    /// <param name="btBackgroundAlpha">
    /// Alpha to use for the label's background rectangle.
    /// </param>
    ///
    /// <param name="oTextOrigin">
    /// The point at which the label will be drawn.
    /// </param>
    //*************************************************************************

    protected void
    DrawLabelBackground
    (
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        FormattedText oFormattedText,
        Color oFormattedTextColor,
        Byte btBackgroundAlpha,
        Point oTextOrigin
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oFormattedText != null);
        AssertValid();

        if (oFormattedText.Width == 0 || oFormattedText.Height == 0)
        {
            return;
        }

        // Note: Don't make the background any more opaque than the text, which
        // might be translucent itself.

        Color oBackgroundColor = WpfGraphicsUtil.SetWpfColorAlpha(
            oGraphDrawingContext.BackColor,
            Math.Min(btBackgroundAlpha, oFormattedTextColor.A) );

        SolidColorBrush oBackgroundBrush = CreateFrozenSolidColorBrush(
            oBackgroundColor);

        Rect oBackgroundRectangle = WpfGraphicsUtil.GetFormattedTextBounds(
            oFormattedText, oTextOrigin);

        // Draw a rounded rectangle with a small amount of padding.

        const Int32 Padding = 1;
        const Int32 Radius = 2;
        oBackgroundRectangle.Inflate(Padding, Padding);

        oDrawingContext.DrawRoundedRectangle(oBackgroundBrush, null,
            oBackgroundRectangle, Radius, Radius);
    }

    //*************************************************************************
    //  Method: GetBezierControlPoint()
    //
    /// <summary>
    /// Gets the control point to use to draw a quadratic Bezier curve.
    /// </summary>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oEndpoint1">
    /// The curve's first endpoint.
    /// </param>
    ///
    /// <param name="oEndpoint2">
    /// The curve's second endpoint.
    /// </param>
    ///
    /// <param name="dBezierDisplacementFactor">
    /// Distance of a line's Bezier control point from the midpoint of a
    /// straight line connecting the endpoints, expressed as a multiple of the
    /// straight line's length.  Must be greater than or equal to zero.
    /// </param>
    ///
    /// <returns>
    /// The control point to use.
    /// </returns>
    //*************************************************************************

    protected Point
    GetBezierControlPoint
    (
        GraphDrawingContext oGraphDrawingContext,
        Point oEndpoint1,
        Point oEndpoint2,
        Double dBezierDisplacementFactor
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(dBezierDisplacementFactor >= 0);
        AssertValid();

        // This method finds the midpoint of the straight line between the two
        // endpoints, then calculates a point that is displaced from the
        // midpoint at a right angle.  This is analagous to pulling a taut
        // string at its midpoint.
        //
        // The calculations are based on the anonymous post "How Can I
        // Calculate The Cartesian Coordinates Of A The Third Corner Of A
        // Triangle If I Have The Lengths Of All Three Sides And The
        // Coordinates Of The First Two Corners?" at
        // http://www.blurtit.com/q9044151.html.

        // Point a, the first endpoint, is one vertex of a right triangle.

        Double dPointAX = oEndpoint1.X;
        Double dPointAY = oEndpoint1.Y;

        // Point b, the midpoint of the line between the two endpoints, is
        // another vertex of the right triangle.  The angle at b is 90 degrees.

        Double dPointBX = dPointAX + (oEndpoint2.X - dPointAX) / 2.0;
        Double dPointBY = dPointAY + (oEndpoint2.Y - dPointAY) / 2.0;

        // Side C connects points a and b.

        Double dSideCLength = WpfGraphicsUtil.GetDistanceBetweenPoints(
            new Point(dPointBX, dPointBY), oEndpoint1);

        // Side A connects points b and c, where c is the point we need to
        // calculate.  Make the length of A, which is the displacement
        // mentioned above, proportional to the length of the line between the
        // two endpoints, so that a longer line gets displaced more than a
        // shorter line.

        Double dSideALength = dSideCLength * dBezierDisplacementFactor;

        // Calculate the angle of the line between the two endpoints.

        Double dAbsAtan2 = Math.Abs( Math.Atan2(
            Math.Max(oEndpoint2.Y, dPointAY) - Math.Min(oEndpoint2.Y, dPointAY),
            Math.Max(oEndpoint2.X, dPointAX) - Math.Min(oEndpoint2.X, dPointAX)
            ) );

        Rect oGraphRectangle = oGraphDrawingContext.GraphRectangle;

        if (dAbsAtan2 >= Math.PI / 4.0 && dAbsAtan2 <= 3.0 * Math.PI / 4.0)
        {
            // The line between the two endpoints is closer to vertical than
            // horizontal.
            //
            // As explained in the post mentioned above, the length of side A
            // can be negative or positive, depending on which direction point
            // c should be displaced.  The following adjustments to
            // dSideALength were determined experimentally.

            if (oEndpoint2.Y > dPointAY)
            {
                dSideALength *= -1.0;
            }

            if (dPointBX - oGraphRectangle.Left <
                oGraphRectangle.Right - dPointBX)
            {
                dSideALength *= -1.0;
            }
        }
        else
        {
            // The line between the two endpoints is closer to horizontal than
            // vertical.

            if (oEndpoint2.X < dPointAX)
            {
                dSideALength *= -1.0;
            }

            if (dPointBY - oGraphRectangle.Top <
                oGraphRectangle.Bottom - dPointBY)
            {
                dSideALength *= -1.0;
            }
        }

        // Calculate point c.

        Double dPointCX = dPointBX +
            ( dSideALength * (dPointAY - dPointBY) ) / dSideCLength;

        Double dPointCY = dPointBY +
            ( dSideALength * (dPointBX - dPointAX) ) / dSideCLength;

        // Don't let point c fall outside the graph's margins.

        return ( WpfGraphicsUtil.MovePointWithinBounds(
            new Point(dPointCX, dPointCY),
            oGraphDrawingContext.GraphRectangleMinusMargin) );
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

        Debug.Assert(m_dGraphScale >= GraphDrawer.MinimumGraphScale);
        Debug.Assert(m_dGraphScale <= GraphDrawer.MaximumGraphScale);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Determines the scale of the graph's vertices and edges.

    protected Double m_dGraphScale;
}

}
