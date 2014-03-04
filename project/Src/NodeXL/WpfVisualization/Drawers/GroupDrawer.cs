
using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;
using Smrf.WpfGraphicsLib;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: GroupDrawer
//
/// <summary>
/// Provides methods that draw a graph's groups.
/// </summary>
///
/// <remarks>
/// This class provides methods for drawing groups when the user has specified
/// a layout style of <see cref="LayoutStyle.UseGroups" />.  This is sometimes
/// called "group in a box."
///
/// <para>
/// Call the <see cref="TryDrawGroupRectangles" />, <see
/// cref="TryDrawCombinedIntergroupEdges" /> and <see
/// cref="TryDrawGroupLabels" /> methods to draw the group elements.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GroupDrawer : DrawerBase
{
    //*************************************************************************
    //  Constructor: GroupDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public GroupDrawer()
    {
        m_oLabelTextColor = SystemColors.WindowTextColor;
        m_eLabelPosition = VertexLabelPosition.MiddleCenter;
        m_dLabelScale = 1.0;
        m_oFormattedTextManager = new FormattedTextManager();

        AssertValid();
    }

    //*************************************************************************
    //  Property: LabelTextColor
    //
    /// <summary>
    /// Gets or sets the color to use for group label text.
    /// </summary>
    ///
    /// <value>
    /// The color of group label text, as a <see cref="Color" />.  The default
    /// value is <see cref="SystemColors.WindowTextColor" />.
    /// </value>
    //*************************************************************************

    public Color
    LabelTextColor
    {
        get
        {
            AssertValid();

            return (m_oLabelTextColor);
        }

        set
        {
            m_oLabelTextColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelPosition
    //
    /// <summary>
    /// Gets or sets the position of group labels.
    /// </summary>
    ///
    /// <value>
    /// The position of group labels.  The default is <see
    /// cref="VertexLabelPosition.MiddleCenter" />.
    /// </value>
    //*************************************************************************

    public VertexLabelPosition
    LabelPosition
    {
        get
        {
            AssertValid();

            return (m_eLabelPosition);
        }

        set
        {
            m_eLabelPosition = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelScale
    //
    /// <summary>
    /// Gets or sets a value that determines the scale of the group labels.
    /// </summary>
    ///
    /// <value>
    /// A value that determines the scale of the group labels.  Must be between
    /// <see cref="GraphDrawer.MinimumGraphScale" /> and <see
    /// cref="GraphDrawer.MaximumGraphScale" />.  The default value is 1.0.
    /// </value>
    ///
    /// <remarks>
    /// If the value is anything besides 1.0, the group labels are shrunk while
    /// their positions remain the same.  If it is set to 0.5, for example, the
    /// labels are half their normal size.
    ///
    /// <para>
    /// Group labels have their own scale, distinct from the <see
    /// cref="DrawerBase.GraphScale" /> used for other graph elements.  That's
    /// because group labels should stay the same size in the NodeXLControl as
    /// the graph scale is changed, in which case the default value of 1.0 is
    /// used, but they should be scaled with other graph elements when an image
    /// of the graph is saved to a bitmap.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Double
    LabelScale
    {
        get
        {
            AssertValid();

            return (m_dLabelScale);
        }

        set
        {
            m_dLabelScale = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TryDrawGroupRectangles()
    //
    /// <summary>
    /// Draws the graph's group rectangles.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph being drawn.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="visual">
    /// Where a new Visual containing the group rectangles gets stored if true
    /// is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the group rectangles were drawn.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryDrawGroupRectangles
    (
        IGraph graph,
        GraphDrawingContext graphDrawingContext,
        out Visual visual
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        visual = null;

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        if (
            !GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
                graph, out oGroupLayoutDrawingInfo)
            ||
            oGroupLayoutDrawingInfo.PenWidth == 0
            )
        {
            return (false);
        }

        DrawingVisual oGroupRectangleDrawingVisual = new DrawingVisual();

        using ( DrawingContext oDrawingContext =
            oGroupRectangleDrawingVisual.RenderOpen() )
        {
            // Note: Don't try to use an alpha value of anything except 255
            // for the rectangle colors.  The rectangles overlap, and
            // transparent overlapping rectangles would have uneven opacities.

            Color oColor = GetContrastingColor(graphDrawingContext, 255,
                false);

            // Note that 1.0 is used where the GraphScale would normally be
            // used.  Group rectangles don't get scaled.

            Pen oPen = CreateFrozenPen(CreateFrozenSolidColorBrush(oColor),
                oGroupLayoutDrawingInfo.PenWidth * 1.0, DashStyles.Solid);

            foreach (GroupInfo oGroupInfo in
                oGroupLayoutDrawingInfo.GroupsToDraw)
            {
                Rect oGroupRectangle;

                if ( TryGetGroupRectangle(oGroupInfo, out oGroupRectangle) )
                {
                    WpfGraphicsUtil.DrawPixelAlignedRectangle(oDrawingContext,
                        null, oPen, oGroupRectangle);
                }
            }
        }

        visual = oGroupRectangleDrawingVisual;
        return (true);
    }

    //*************************************************************************
    //  Method: TryDrawCombinedIntergroupEdges()
    //
    /// <summary>
    /// Attempts to draw all combined intergroup edges.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph being drawn.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="visual">
    /// Where a new Visual containing the combined intergroup edges gets stored
    /// if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the group rectangles were drawn.
    /// </returns>
    ///
    /// <remarks>
    /// (When intergroup edges are combined, all edges between groups A and B
    /// are hidden.  In their place, a single combined edge is drawn between
    /// the two group rectangles.)
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryDrawCombinedIntergroupEdges
    (
        IGraph graph,
        GraphDrawingContext graphDrawingContext,
        out Visual visual
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        visual = null;

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        if ( !GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            graph, out oGroupLayoutDrawingInfo) )
        {
            return (false);
        }

        IEnumerable<IntergroupEdgeInfo> oCombinedIntergroupEdges =
            oGroupLayoutDrawingInfo.CombinedIntergroupEdges;

        if (oCombinedIntergroupEdges == null)
        {
            return (false);
        }

        DrawingVisual oCombinedIntergroupEdgeDrawingVisual =
            new DrawingVisual();

        using ( DrawingContext oDrawingContext =
            oCombinedIntergroupEdgeDrawingVisual.RenderOpen() )
        {
            foreach (IntergroupEdgeInfo oCombinedIntergroupEdge in
                oCombinedIntergroupEdges)
            {
                DrawCombinedIntergroupEdge(oDrawingContext,
                    graphDrawingContext, oCombinedIntergroupEdge,
                    oGroupLayoutDrawingInfo);
            }
        }

        visual = oCombinedIntergroupEdgeDrawingVisual;
        return (true);
    }

    //*************************************************************************
    //  Method: TryDrawGroupLabels()
    //
    /// <summary>
    /// Attempts to draw the graph's group labels.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph being drawn.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="visual">
    /// Where a new Visual containing the drawn labels gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the group labels were drawn.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryDrawGroupLabels
    (
        IGraph graph,
        GraphDrawingContext graphDrawingContext,
        out Visual visual
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        visual = null;

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        if (
            m_eLabelPosition == VertexLabelPosition.Nowhere
            ||
            !GroupMetadataManager.TryGetGroupLayoutDrawingInfo(graph,
                out oGroupLayoutDrawingInfo)
            )
        {
            return (false);
        }

        DrawingVisual oGroupLabelDrawingVisual = new DrawingVisual();

        using ( DrawingContext oDrawingContext =
            oGroupLabelDrawingVisual.RenderOpen() )
        {
            foreach (GroupInfo oGroupInfo in
                oGroupLayoutDrawingInfo.GroupsToDraw)
            {
                DrawGroupLabel(oDrawingContext, graphDrawingContext,
                    oGroupInfo);
            }
        }

        visual = oGroupLabelDrawingVisual;
        return (true);
    }

    //*************************************************************************
    //  Method: SetFont()
    //
    /// <summary>
    /// Sets the font used to draw labels.
    /// </summary>
    ///
    /// <param name="typeface">
    /// The Typeface to use.
    /// </param>
    ///
    /// <param name="fontSize">
    /// The font size to use, in WPF units.
    /// </param>
    ///
    /// <remarks>
    /// The default font is the SystemFonts.MessageFontFamily at size 10.
    /// </remarks>
    //*************************************************************************

    public void
    SetFont
    (
        Typeface typeface,
        Double fontSize
    )
    {
        Debug.Assert(typeface != null);
        Debug.Assert(fontSize > 0);
        AssertValid();

        m_oFormattedTextManager.SetFont(typeface, fontSize);
    }

    //*************************************************************************
    //  Method: DrawCombinedIntergroupEdge()
    //
    /// <summary>
    /// Draws one combined intergroup edge.
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
    /// <param name="oCombinedIntergroupEdge">
    /// Represents a set of edges between two groups that should be drawn as a
    /// single edge.
    /// </param>
    ///
    /// <param name="oGroupLayoutDrawingInfo">
    /// Group drawing information.
    /// </param>
    //*************************************************************************

    protected void
    DrawCombinedIntergroupEdge
    (
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        IntergroupEdgeInfo oCombinedIntergroupEdge,
        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oCombinedIntergroupEdge != null);
        Debug.Assert(oGroupLayoutDrawingInfo != null);
        AssertValid();

        Rect oGroupRectangle1, oGroupRectangle2;

        if (
            !TryGetGroupRectangle(

                oGroupLayoutDrawingInfo.GroupsToDraw[
                    oCombinedIntergroupEdge.Group1Index],

                out oGroupRectangle1)
            ||
            !TryGetGroupRectangle(

                oGroupLayoutDrawingInfo.GroupsToDraw[
                    oCombinedIntergroupEdge.Group2Index],

                out oGroupRectangle2)
            )
        {
            return;
        }

        Point oGroupRectangle1Center =
            WpfGraphicsUtil.GetRectCenter(oGroupRectangle1);

        Point oGroupRectangle2Center =
            WpfGraphicsUtil.GetRectCenter(oGroupRectangle2);

        Point oBezierControlPoint = GetBezierControlPoint(oGraphDrawingContext,
            oGroupRectangle1Center, oGroupRectangle2Center,
            CombinedIntergroupEdgeBezierDisplacementFactor
            );

        PathGeometry oBezierCurve =
            WpfPathGeometryUtil.GetQuadraticBezierCurve(oGroupRectangle1Center,
                oGroupRectangle2Center, oBezierControlPoint);

        Color oColor = GetContrastingColor(
            oGraphDrawingContext, CombinedIntergroupEdgeAlpha, true);

        Pen oPen = CreateFrozenPen(CreateFrozenSolidColorBrush(oColor),

            GetCombinedIntergroupEdgePenWidth(oCombinedIntergroupEdge) *
                this.GraphScale,

            DashStyles.Solid, PenLineCap.Round);

        oDrawingContext.DrawGeometry(null, oPen, oBezierCurve);
    }

    //*************************************************************************
    //  Method: DrawGroupLabel()
    //
    /// <summary>
    /// Draws a group label.
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
    /// <param name="oGroupInfo">
    /// Stores information about one group.
    /// </param>
    ///
    /// <remarks>
    /// It's assumed that <see cref="LabelPosition" /> is not <see
    /// cref="VertexLabelPosition.Nowhere" />.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawGroupLabel
    (
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        GroupInfo oGroupInfo
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oGroupInfo != null);
        Debug.Assert(m_eLabelPosition != VertexLabelPosition.Nowhere);
        AssertValid();

        String sLabel = oGroupInfo.Label;
        Rect oGroupRectangle;

        if (
            String.IsNullOrEmpty(sLabel)
            ||
            !TryGetGroupRectangle(oGroupInfo, out oGroupRectangle)
            )
        {
            return;
        }

        FormattedText oFormattedText =
            m_oFormattedTextManager.CreateFormattedText(sLabel,
                m_oLabelTextColor, m_dLabelScale);

        oFormattedText.MaxTextWidth = oGroupRectangle.Width;
        oFormattedText.MaxTextHeight = oGroupRectangle.Height;

        // The alignment needs to be set before the width and height of the
        // FormattedText are obtained.

        SetTextAlignment(oFormattedText, m_eLabelPosition);

        // You can't use FormattedText.Width to get the width of the actual
        // text when text wrapping is enabled (FormattedText.MaxTextWidth > 0).
        // Instead, use a method that takes wrapping into account.

        Double dLabelWidth =
            WpfGraphicsUtil.GetFormattedTextSize(oFormattedText).Width;

        Double dLabelHeight = oFormattedText.Height;

        Double dGroupRectangleWidth = oGroupRectangle.Width;
        Double dGroupRectangleHeight = oGroupRectangle.Height;
        Double dMaxTextWidth = oFormattedText.MaxTextWidth;

        Double dTextOffsetXForCenter =
            (dGroupRectangleWidth - dMaxTextWidth) / 2.0;

        Double dTextOffsetYForMiddle =
            (dGroupRectangleHeight - dLabelHeight) / 2.0;

        Double dTextOffsetYForBottom =
            dGroupRectangleHeight - dLabelHeight - LabelVerticalMargin;

        Point oTextOrigin = oGroupRectangle.Location;
        Double dTextOffsetX = 0;
        Double dTextOffsetY = 0;

        switch (m_eLabelPosition)
        {
            case VertexLabelPosition.TopLeft:

                dTextOffsetX = LabelHorizontalMargin;
                dTextOffsetY = LabelVerticalMargin;
                break;

            case VertexLabelPosition.TopCenter:

                dTextOffsetX = dTextOffsetXForCenter;
                dTextOffsetY = LabelVerticalMargin;

                break;

            case VertexLabelPosition.TopRight:

                dTextOffsetX = -LabelHorizontalMargin;
                dTextOffsetY = LabelVerticalMargin;

                break;

            case VertexLabelPosition.MiddleLeft:

                dTextOffsetX = LabelHorizontalMargin;
                dTextOffsetY = dTextOffsetYForMiddle;

                break;

            case VertexLabelPosition.MiddleCenter:

                dTextOffsetX = dTextOffsetXForCenter;
                dTextOffsetY = dTextOffsetYForMiddle;

                break;

            case VertexLabelPosition.MiddleRight:

                dTextOffsetX = -LabelHorizontalMargin;
                dTextOffsetY = dTextOffsetYForMiddle;

                break;

            case VertexLabelPosition.BottomLeft:

                dTextOffsetX = LabelHorizontalMargin;
                dTextOffsetY = dTextOffsetYForBottom;

                break;

            case VertexLabelPosition.BottomCenter:


                dTextOffsetX = dTextOffsetXForCenter;
                dTextOffsetY = dTextOffsetYForBottom;

                break;

            case VertexLabelPosition.BottomRight:

                dTextOffsetX = -LabelHorizontalMargin;
                dTextOffsetY = dTextOffsetYForBottom;

                break;

            default:

                Debug.Assert(false);
                break;
        }

        oTextOrigin.Offset(dTextOffsetX, dTextOffsetY);

        DrawLabelBackground(oDrawingContext, oGraphDrawingContext,
            oFormattedText, m_oLabelTextColor, LabelBackgroundAlpha,
            oTextOrigin);

        oDrawingContext.DrawText(oFormattedText, oTextOrigin);
    }

    //*************************************************************************
    //  Method: SetTextAlignment()
    //
    /// <summary>
    /// Sets the TextAlignment property on a FormattedText object being used to
    /// draw a label.
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

                // eTextAlignment = TextAlignment.Left;
                break;

            case VertexLabelPosition.TopCenter:
            case VertexLabelPosition.MiddleCenter:
            case VertexLabelPosition.BottomCenter:

                eTextAlignment = TextAlignment.Center;
                break;

            case VertexLabelPosition.TopRight:
            case VertexLabelPosition.MiddleRight:
            case VertexLabelPosition.BottomRight:

                eTextAlignment = TextAlignment.Right;
                break;

            default:

                Debug.Assert(false);
                break;
        }

        oFormattedText.TextAlignment = eTextAlignment;
    }

    //*************************************************************************
    //  Method: GetCombinedIntergroupEdgePenWidth()
    //
    /// <summary>
    /// Gets the pen width to use when drawing an intergroup edge.
    /// </summary>
    ///
    /// <param name="oCombinedIntergroupEdge">
    /// Represents a set of edges between two groups that should be drawn as a
    /// single edge.
    /// </param>
    ///
    /// <returns>
    /// The pen width to use, in WPF units.
    /// </returns>
    //*************************************************************************

    protected Double
    GetCombinedIntergroupEdgePenWidth
    (
        IntergroupEdgeInfo oCombinedIntergroupEdge
    )
    {
        Debug.Assert(oCombinedIntergroupEdge != null);
        AssertValid();

        return (MathUtil.TransformValueToRange(
            (Single)oCombinedIntergroupEdge.EdgeWeightSum,
            CombinedIntergroupEdgeMinimumCount,
            CombinedIntergroupEdgeMaximumCount,
            CombinedIntergroupEdgeMinimumWidth,
            CombinedIntergroupEdgeMaximumWidth
            ) );
    }

    //*************************************************************************
    //  Method: GetContrastingColor()
    //
    /// <summary>
    /// Gets a color that contrasts with the graph's background.
    /// </summary>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="btAlpha">
    /// Alpha to use for the color.
    /// </param>
    ///
    /// <param name="bUseMaximumContrast">
    /// true to get a color that provides maximum contrast with the graph's
    /// background, false to get a color that provides less contrast.
    /// </param>
    ///
    /// <returns>
    /// A contrasting color.
    /// </returns>
    //*************************************************************************

    protected Color
    GetContrastingColor
    (
        GraphDrawingContext oGraphDrawingContext,
        Byte btAlpha,
        Boolean bUseMaximumContrast
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        return ( WpfGraphicsUtil.SetWpfColorAlpha(
            WpfGraphicsUtil.GetContrastingColor(
                oGraphDrawingContext.BackColor,
                bUseMaximumContrast), btAlpha) );
    }

    //*************************************************************************
    //  Method: TryGetGroupRectangle()
    //
    /// <summary>
    /// Attempts to get a group's rectangle.
    /// </summary>
    ///
    /// <param name="oGroupInfo">
    /// Stores information about one group.
    /// </param>
    ///
    /// <param name="oGroupRectangle">
    /// Where the group rectangle gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the rectangle is not empty.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetGroupRectangle
    (
        GroupInfo oGroupInfo,
        out Rect oGroupRectangle
    )
    {
        Debug.Assert(oGroupInfo != null);
        AssertValid();

        oGroupRectangle = WpfGraphicsUtil.RectangleToRect(
            oGroupInfo.Rectangle);

        return (oGroupRectangle.Width > 0 && oGroupRectangle.Height > 0);
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

        // m_oLabelTextColor
        // m_eLabelPosition
        Debug.Assert(m_dLabelScale >= GraphDrawer.MinimumGraphScale);
        Debug.Assert(m_dLabelScale <= GraphDrawer.MaximumGraphScale);
        Debug.Assert(m_oFormattedTextManager != null);
    }


    //*************************************************************************
    //  Combined intergroup edge protected constants
    //*************************************************************************

    /// Alpha value to use when drawing combined intergroup edges.

    protected const Int32 CombinedIntergroupEdgeAlpha = 50;

    /// Minimum width of the edge that represents a set of combined intergroup
    /// edges.

    protected const Single CombinedIntergroupEdgeMinimumWidth = 3;

    /// Maximum width of the edge that represents a set of combined intergroup
    /// edges.

    protected const Single CombinedIntergroupEdgeMaximumWidth = 36;

    /// Number of intergroup edges that correspond to a combined intergroup
    /// edge width of CombinedIntergroupEdgeMinimumWidth.

    protected const Int32 CombinedIntergroupEdgeMinimumCount = 1;

    /// Number of intergroup edges that correspond to a combined intergroup
    /// edge width of CombinedIntergroupEdgeMaximumWidth.

    protected const Int32 CombinedIntergroupEdgeMaximumCount = 10;

    /// Determines the edge curvature to use when drawing combined intergroup
    /// edges.

    protected const Double CombinedIntergroupEdgeBezierDisplacementFactor =
        0.5;


    //*************************************************************************
    //  Group label protected constants
    //*************************************************************************

    /// Horizontal and vertical margins used between a group label and a group
    /// box.

    protected const Double LabelHorizontalMargin = 3;
    ///
    protected const Double LabelVerticalMargin = 2;

    /// Alpha of the group label background rectangles.

    protected const Byte LabelBackgroundAlpha = 120;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Color to use for group label text.

    protected Color m_oLabelTextColor;

    /// Position of group labels.

    protected VertexLabelPosition m_eLabelPosition;

    /// Determines the scale of the group labels.

    protected Double m_dLabelScale;

    /// Manages the creation of FormattedText objects.

    protected FormattedTextManager m_oFormattedTextManager;
}

}
