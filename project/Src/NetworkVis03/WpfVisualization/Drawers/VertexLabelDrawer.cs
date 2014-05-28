
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexLabelDrawer
//
/// <summary>
/// Draws a vertex label as an annotation.
/// </summary>
///
/// <remarks>
/// This class draws a label next to a vertex, as an annotation.  It does NOT
/// draw vertices that have the shape <see cref="VertexShape.Label" />.
/// </remarks>
//*****************************************************************************

public class VertexLabelDrawer : DrawerBase
{
    //*************************************************************************
    //  Constructor: VertexLabelDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexLabelDrawer" />
    /// class.
    /// </summary>
    ///
    /// <param name="labelPosition">
    /// The default position to use for vertex labels.
    /// </param>
    ///
    /// <param name="backgroundAlpha">
    /// The alpha of the label's background rectangle, as a Byte.
    /// </param>
    //*************************************************************************

    public VertexLabelDrawer
    (
        VertexLabelPosition labelPosition,
        Byte backgroundAlpha
    )
    {
        m_eLabelPosition = labelPosition;
        m_btBackgroundAlpha = backgroundAlpha;

        AssertValid();
    }

    //*************************************************************************
    //  Method: DrawLabel()
    //
    /// <summary>
    /// Draws a vertex label as an annotation.
    /// </summary>
    ///
    /// <summary>
    /// Draws a vertex label as an annotation at a position determined by the
    /// vertex's metadata.
    /// </summary>
    ///
    /// <param name="drawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="vertexDrawingHistory">
    /// Describes how the vertex was drawn.
    /// </param>
    ///
    /// <param name="formattedText">
    /// The FormattedText object to use.  Several properties get changed by
    /// this method.
    /// </param>
    ///
    /// <param name="formattedTextColor">
    /// The color of the FormattedText's foreground brush.
    /// </param>
    //*************************************************************************

    public void
    DrawLabel
    (
        DrawingContext drawingContext,
        GraphDrawingContext graphDrawingContext,
        VertexDrawingHistory vertexDrawingHistory,
        FormattedText formattedText,
        Color formattedTextColor
    )
    {
        Debug.Assert(drawingContext != null);
        Debug.Assert(graphDrawingContext != null);
        Debug.Assert(vertexDrawingHistory != null);
        Debug.Assert(formattedText != null);
        AssertValid();

        DrawLabel(drawingContext, graphDrawingContext, vertexDrawingHistory,
            GetLabelPosition(vertexDrawingHistory.Vertex), formattedText,
            formattedTextColor, true);
    }

    //*************************************************************************
    //  Method: DrawLabel()
    //
    /// <summary>
    /// Draws a vertex label as an annotation at a specified position.
    /// </summary>
    ///
    /// <param name="drawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="vertexDrawingHistory">
    /// Describes how the vertex was drawn.
    /// </param>
    ///
    /// <param name="labelPosition">
    /// The label's position.
    /// </param>
    ///
    /// <param name="formattedText">
    /// The FormattedText object to use.  Several properties get changed by
    /// this method.
    /// </param>
    ///
    /// <param name="formattedTextColor">
    /// The color of the FormattedText's foreground brush.
    /// </param>
    ///
    /// <param name="drawBackground">
    /// If true, a background is drawn behind the label text to make it more
    /// legible.
    /// </param>
    //*************************************************************************

    public void
    DrawLabel
    (
        DrawingContext drawingContext,
        GraphDrawingContext graphDrawingContext,
        VertexDrawingHistory vertexDrawingHistory,
        VertexLabelPosition labelPosition,
        FormattedText formattedText,
        Color formattedTextColor,
        Boolean drawBackground
    )
    {
        Debug.Assert(drawingContext != null);
        Debug.Assert(graphDrawingContext != null);
        Debug.Assert(vertexDrawingHistory != null);
        Debug.Assert(formattedText != null);
        AssertValid();

        if (labelPosition == VertexLabelPosition.Nowhere)
        {
            return;
        }

        // The alignment needs to be set before the width and height of the
        // FormattedText are obtained.

        SetTextAlignment(formattedText, labelPosition);

        // You can't use FormattedText.Width to get the width of the actual
        // text when text wrapping is enabled (FormattedText.MaxTextWidth > 0).
        // Instead, use a method that takes wrapping into account.

        Double dLabelWidth = WpfGraphicsUtil.GetFormattedTextSize(
            formattedText).Width;

        Double dLabelHeight = formattedText.Height;

        // This is the point where the label will be drawn using
        // DrawingContext.Draw().  It initially assumes a text height of zero,
        // a margin of zero, and no adjustment for alignment (see
        // dAdjustmentForTextAlignmentX below), but this will be modified
        // appropriately within the switch statement below.

        Point oDraw = vertexDrawingHistory.GetLabelLocation(labelPosition);
        Double dDrawX = oDraw.X;
        Double dDrawY = oDraw.Y;

        // These are the left and right bounds of the rectangle where the text
        // will actually appear.

        Double dLabelBoundsLeft = 0;
        Double dLabelBoundsRight = 0;

        AdjustForTextAlignment(dLabelWidth, dLabelHeight, labelPosition,
            formattedText, ref dDrawX, ref dDrawY, ref dLabelBoundsLeft,
            ref dLabelBoundsRight);

        // Don't let the text exceed the bounds of the graph rectangle.

        dDrawX = StayWithinHorizontalBounds(
            dDrawX, graphDrawingContext, dLabelBoundsLeft, dLabelBoundsRight);

        Double dLabelBoundsTop = dDrawY;
        Double dLabelBoundsBottom = dDrawY + dLabelHeight;

        dDrawY = StayWithinVerticalBounds(
            dDrawY, graphDrawingContext, dLabelBoundsTop, dLabelBoundsBottom);

        if (drawBackground)
        {
            dLabelBoundsLeft = StayWithinHorizontalBounds(
                dLabelBoundsLeft, graphDrawingContext,
                dLabelBoundsLeft, dLabelBoundsRight);

            DrawLabelBackground( drawingContext, graphDrawingContext,
                formattedText, formattedTextColor, m_btBackgroundAlpha,
                new Point(dLabelBoundsLeft, dDrawY) );
        }

        drawingContext.DrawText( formattedText, new Point(dDrawX, dDrawY) );
    }

    //*************************************************************************
    //  Method: AdjustForTextAlignment()
    //
    /// <summary>
    /// Adjusts several drawing parameters to compensate for the way
    /// DrawingContext.DrawText() positions centered and right-justified text.
    /// </summary>
    ///
    /// <param name="dLabelWidth">
    /// The label's width.
    /// </param>
    ///
    /// <param name="dLabelHeight">
    /// The label's height.
    /// </param>
    ///
    /// <param name="eLabelPosition">
    /// The label's position.
    /// </param>
    ///
    /// <param name="oFormattedText">
    /// The FormattedText object to use.
    /// </param>
    ///
    /// <param name="dDrawX">
    /// The x-coordinate where the label will be drawn using
    /// DrawingContext.Draw().
    /// </param>
    ///
    /// <param name="dDrawY">
    /// The y-coordinate where the label will be drawn using
    /// DrawingContext.Draw().
    /// </param>
    ///
    /// <param name="dLabelBoundsLeft">
    /// The left bounds of the rectangle where the text will actually appear.
    /// </param>
    ///
    /// <param name="dLabelBoundsRight">
    /// The right bounds of the rectangle where the text will actually appear.
    /// </param>
    //*************************************************************************

    protected void
    AdjustForTextAlignment
    (
        Double dLabelWidth,
        Double dLabelHeight,
        VertexLabelPosition eLabelPosition,
        FormattedText oFormattedText,
        ref Double dDrawX,
        ref Double dDrawY,
        ref Double dLabelBoundsLeft,
        ref Double dLabelBoundsRight
    )
    {
        Debug.Assert(dLabelWidth >= 0);
        Debug.Assert(dLabelHeight >= 0);
        Debug.Assert(oFormattedText != null);
        AssertValid();

        Double dHalfLabelHeight = dLabelHeight / 2.0;
        Double dHalfLabelWidth = dLabelWidth / 2.0;
        Double dMaxTextWidth = oFormattedText.MaxTextWidth;
        Double dHalfMaxTextWidth = dMaxTextWidth / 2.0;

        // This is the adjustment that needs to be made to the x-coordinate
        // passed to DrawingContext.DrawText() to compensate for the way
        // centered and right-justified text is positioned.
        //
        // When wrapping is turned off (FormattedText.MaxTextWidth = 0) and
        // FormattedText.TextAlignment = TextAlignment.Center, for example,
        // the text gets centered horizontally at the specified drawing point.
        // When wrapping is turned on, however (FormattedText.MaxTextWidth
        // > 0), the text gets centered horizontally at the point halfway
        // between the specified drawing point and the MaxTextWidth value.

        Double dAdjustmentForTextAlignmentX = 0;

        switch (eLabelPosition)
        {
            case VertexLabelPosition.TopLeft:

                dAdjustmentForTextAlignmentX = -dMaxTextWidth;
                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.TopCenter:

                dAdjustmentForTextAlignmentX = -dHalfMaxTextWidth;
                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.TopRight:

                dDrawY -= (dLabelHeight + VerticalMargin);
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            case VertexLabelPosition.MiddleLeft:

                dAdjustmentForTextAlignmentX = -dMaxTextWidth;
                dDrawX -= HorizontalMargin;
                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.MiddleCenter:

                dAdjustmentForTextAlignmentX = -dHalfMaxTextWidth;
                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.MiddleRight:

                dDrawX += HorizontalMargin;
                dDrawY -= dHalfLabelHeight;
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            case VertexLabelPosition.BottomLeft:

                dAdjustmentForTextAlignmentX = -dMaxTextWidth;
                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX - dLabelWidth;
                dLabelBoundsRight = dDrawX;

                break;

            case VertexLabelPosition.BottomCenter:

                dAdjustmentForTextAlignmentX = -dHalfMaxTextWidth;
                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX - dHalfLabelWidth;
                dLabelBoundsRight = dDrawX + dHalfLabelWidth;

                break;

            case VertexLabelPosition.BottomRight:

                dDrawY += VerticalMargin;
                dLabelBoundsLeft = dDrawX;
                dLabelBoundsRight = dDrawX + dLabelWidth;

                break;

            case VertexLabelPosition.Nowhere:

                // (This was handled at the top of the method.)

            default:

                Debug.Assert(false);
                break;
        }

        dDrawX += dAdjustmentForTextAlignmentX;
    }

    //*************************************************************************
    //  Method: StayWithinHorizontalBounds()
    //
    /// <summary>
    /// Adjusts an x-coordinate so it stays within the graph rectangle.
    /// </summary>
    ///
    /// <param name="dX">
    /// The x-coordinate to adjust if necessary.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="dLabelBoundsLeft">
    /// The left bounds of the rectangle where the text will actually appear.
    /// </param>
    ///
    /// <param name="dLabelBoundsRight">
    /// The right bounds of the rectangle where the text will actually appear.
    /// </param>
    ///
    /// <returns>
    /// The adjusted x-coordinate.
    /// </returns>
    //*************************************************************************

    protected Double
    StayWithinHorizontalBounds
    (
        Double dX,
        GraphDrawingContext oGraphDrawingContext,
        Double dLabelBoundsLeft,
        Double dLabelBoundsRight
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        Rect oGraphRectangleMinusMargin =
            oGraphDrawingContext.GraphRectangleMinusMargin;

        dX += Math.Max(0, oGraphRectangleMinusMargin.Left - dLabelBoundsLeft);

        dX -= Math.Max(0, dLabelBoundsRight - oGraphRectangleMinusMargin.Right);

        return (dX);
    }

    //*************************************************************************
    //  Method: StayWithinVerticalBounds()
    //
    /// <summary>
    /// Adjusts a y-coordinate so it stays within the graph rectangle.
    /// </summary>
    ///
    /// <param name="dY">
    /// The y-coordinate to adjust if necessary.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="dLabelBoundsTop">
    /// The top bounds of the rectangle where the text will actually appear.
    /// </param>
    ///
    /// <param name="dLabelBoundsBottom">
    /// The bottom bounds of the rectangle where the text will actually appear.
    /// </param>
    ///
    /// <returns>
    /// The adjusted y-coordinate.
    /// </returns>
    //*************************************************************************

    protected Double
    StayWithinVerticalBounds
    (
        Double dY,
        GraphDrawingContext oGraphDrawingContext,
        Double dLabelBoundsTop,
        Double dLabelBoundsBottom
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        Rect oGraphRectangleMinusMargin =
            oGraphDrawingContext.GraphRectangleMinusMargin;

        dY += Math.Max(0, oGraphRectangleMinusMargin.Top - dLabelBoundsTop);

        dY -= Math.Max(0,
            dLabelBoundsBottom - oGraphRectangleMinusMargin.Bottom);

        return (dY);
    }

    //*************************************************************************
    //  Method: GetLabelPosition()
    //
    /// <summary>
    /// Gets the position of a vertex label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the label position for.
    /// </param>
    ///
    /// <returns>
    /// The vertex's label position.
    /// </returns>
    //*************************************************************************

    protected VertexLabelPosition
    GetLabelPosition
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default position.

        VertexLabelPosition eLabelPosition = m_eLabelPosition;

        // Check for a per-vertex label position.

        Object oPerVertexLabelPositionAsObject;

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexLabelPosition,
            typeof(VertexLabelPosition), out oPerVertexLabelPositionAsObject) )
        {
            eLabelPosition =
                (VertexLabelPosition)oPerVertexLabelPositionAsObject;
        }

        return (eLabelPosition);
    }

    //*************************************************************************
    //  Method: SetTextAlignment()
    //
    /// <summary>
    /// Sets the TextAlignment property on a FormattedText object.
    /// </summary>
    ///
    /// <param name="oFormattedText">
    /// The FormattedText object to set the TextAlignment property on.
    /// </param>
    ///
    /// <param name="eLabelPosition">
    /// The label's position.  Can't be VertexLabelPosition.Nowhere.
    /// </param>
    //*************************************************************************

    protected void
    SetTextAlignment
    (
        FormattedText oFormattedText,
        VertexLabelPosition eLabelPosition
    )
    {
        Debug.Assert(oFormattedText != null);
        Debug.Assert(eLabelPosition != VertexLabelPosition.Nowhere);
        AssertValid();

        TextAlignment eTextAlignment = TextAlignment.Left;

        switch (eLabelPosition)
        {
            case VertexLabelPosition.TopLeft:
            case VertexLabelPosition.MiddleLeft:
            case VertexLabelPosition.BottomLeft:

                eTextAlignment = TextAlignment.Right;
                break;

            case VertexLabelPosition.TopCenter:
            case VertexLabelPosition.MiddleCenter:
            case VertexLabelPosition.BottomCenter:

                eTextAlignment = TextAlignment.Center;
                break;

            case VertexLabelPosition.TopRight:
            case VertexLabelPosition.MiddleRight:
            case VertexLabelPosition.BottomRight:

                // eTextAlignment = TextAlignment.Left;
                break;

            case VertexLabelPosition.Nowhere:
            default:

                Debug.Assert(false);
                break;
        }

        oFormattedText.TextAlignment = eTextAlignment;
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

        // m_btBackgroundAlpha
        // m_eLabelPosition
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Horizontal margin used between the vertex bounds and the label in some
    /// cases.

    protected const Double HorizontalMargin = 3;

    /// Vertical margin used between the vertex bounds and the label in some
    /// cases.

    protected const Double VerticalMargin = 2;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Alpha of the label's background rectangle.

    protected Byte m_btBackgroundAlpha;

    /// Default position of vertex labels drawn as annotations.

    protected VertexLabelPosition m_eLabelPosition;
}
}
