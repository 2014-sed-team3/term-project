
using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.GraphicsLib;
using Smrf.WpfGraphicsLib;
using Smrf.AppLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: EdgeDrawer
//
/// <summary>
/// Draws a graph's edges.
/// </summary>
///
/// <remarks>
/// This class is responsible for drawing a graph's edges.  The default edge
/// appearance is determined by this class's properties.  The appearance of an
/// individual edge can be overridden by adding appropriate metadata to the
/// edge.  The following metadata keys are honored:
///
/// <list type="bullet">
///
/// <item><see cref="ReservedMetadataKeys.Visibility" /></item>
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeWidth" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeStyle" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeLabel" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeLabelTextColor" /></item>
/// <item><see cref="ReservedMetadataKeys.IsSelected" /></item>
///
/// </list>
///
/// <para>
/// The values of the <see cref="ReservedMetadataKeys.PerColor" /> and 
/// <see cref="ReservedMetadataKeys.PerEdgeLabelTextColor" /> keys can be of
/// type System.Windows.Media.Color or System.Drawing.Color.
/// </para>
///
/// <para>
/// If <see cref="VertexAndEdgeDrawerBase.UseSelection" /> is true, an edge is
/// drawn using <see cref="VertexAndEdgeDrawerBase.Color" /> or <see
/// cref="VertexAndEdgeDrawerBase.SelectedColor" />, depending on whether the
/// edge has been marked as selected.  If <see
/// cref="VertexAndEdgeDrawerBase.UseSelection" /> is false, <see
/// cref="VertexAndEdgeDrawerBase.Color" /> is used regardless of whether the
/// edge has been marked as selected.
/// </para>
///
/// <para>
/// When drawing the graph, call <see cref="TryDrawEdge" /> for each of the
/// graph's edges.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class EdgeDrawer : VertexAndEdgeDrawerBase
{
    //*************************************************************************
    //  Constructor: EdgeDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeDrawer()
    {
        m_dWidth = 1;
        m_eCurveStyle = EdgeCurveStyle.Straight;
        m_dBezierDisplacementFactor = 0.2;
        m_bDrawArrowOnDirectedEdge = true;
        m_dRelativeArrowSize = 3.0;
        m_oLabelTextColor = SystemColors.WindowTextColor;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Width
    //
    /// <summary>
    /// Gets or sets the default edge width.
    /// </summary>
    ///
    /// <value>
    /// The default edge width, as a <see cref="Double" />.  Must be between
    /// <see cref="MinimumWidth" /> and <see cref="MaximumWidth" />, inclusive.
    /// The default value is 1.
    /// </value>
    ///
    /// <remarks>
    /// The default edge width can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerEdgeWidth" /> key on the edge.
    /// </remarks>
    //*************************************************************************

    public Double
    Width
    {
        get
        {
            AssertValid();

            return (m_dWidth);
        }

        set
        {
            const String PropertyName = "Width";

            this.ArgumentChecker.CheckPropertyInRange(
                PropertyName, value, MinimumWidth, MaximumWidth);

            m_dWidth = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DrawArrowOnDirectedEdge
    //
    /// <summary>
    /// Gets or sets a flag that determines whether an arrow should be drawn on
    /// directed edges.
    /// </summary>
    ///
    /// <value>
    /// true to draw an arrow on directed edges, false otherwise.  The default
    /// value is true.
    /// </value>
    ///
    /// <remarks>
    /// By default, an edge with <see cref="IEdge.IsDirected" /> set to true is
    /// drawn with an arrow pointing to the front vertex.  If this property is
    /// set to false, the arrow is not drawn.
    /// </remarks>
    //*************************************************************************

    public Boolean
    DrawArrowOnDirectedEdge
    {
        get
        {
            AssertValid();

            return (m_bDrawArrowOnDirectedEdge);
        }

        set
        {
            m_bDrawArrowOnDirectedEdge = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: RelativeArrowSize
    //
    /// <summary>
    /// Gets or sets the relative size of arrowheads on directed edges.
    /// </summary>
    ///
    /// <value>
    /// The relative size of arrowheads, as a <see cref="Double" />.  Must be
    /// between <see cref="MinimumRelativeArrowSize" /> and <see
    /// cref="MaximumRelativeArrowSize" />, inclusive.  The default value is 3.
    /// </value>
    ///
    /// <remarks>
    /// The value is relative to <see cref="Width" />.  If the width is
    /// increased, the arrow size is increased proportionally.
    /// </remarks>
    //*************************************************************************

    public Double
    RelativeArrowSize
    {
        get
        {
            AssertValid();

            return (m_dRelativeArrowSize);
        }

        set
        {
            const String PropertyName = "RelativeArrowSize";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumRelativeArrowSize, MaximumRelativeArrowSize);

            m_dRelativeArrowSize = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelTextColor
    //
    /// <summary>
    /// Gets or sets the default text color to use for edge labels.
    /// </summary>
    ///
    /// <value>
    /// The default text color to use for labels.  The default is
    /// SystemColors.WindowTextColor.
    /// </value>
    ///
    /// <remarks>
    /// <see cref="Color" /> is used for the edge itself.
    ///
    /// <para>
    /// The default text color of an edge label can be overridden by setting
    /// the <see cref="ReservedMetadataKeys.PerEdgeLabelTextColor" /> key on
    /// the edge.
    /// </para>
    ///
    /// </remarks>
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
    //  Property: CurveStyle
    //
    /// <summary>
    /// Gets or sets a value that specifies how edges are curved.
    /// </summary>
    ///
    /// <value>
    /// An <see cref="EdgeCurveStyle" /> value that specifies how edges are
    /// curved.  The default is <see cref="EdgeCurveStyle.Straight" />.
    /// </value>
    ///
    /// <remarks>
    /// If you set this property to EdgeCurveStyle.<see
    /// cref="EdgeCurveStyle.CurveThroughIntermediatePoints" />, intermediate
    /// edge points should be stored on each edge using the
    /// ReservedMetadataKeys.<see
    /// cref="ReservedMetadataKeys.PerEdgeIntermediateCurvePoints" /> key, and
    /// the <see
    /// cref="ReservedMetadataKeys.GraphHasEdgeIntermediateCurvePoints" /> key
    /// should be stored on the graph itself.  If you are using the
    /// NodeXLControl, then the control automatically calculates and stores the
    /// intermediate edge points for you using an internal <see
    /// cref="NodeXL.Layouts.EdgeBundler" /> object.  EdgeBundler "bundles"
    /// edges to reduce visual clutter.
    ///
    /// <para>
    /// If you set this property to EdgeCurveStyle.<see
    /// cref="EdgeCurveStyle.CurveThroughIntermediatePoints" /> and you are
    /// <i>not</i> using the NodeXLControl, then you must calculate and store
    /// the intermediate edge points yourself.  You can use an <see
    /// cref="NodeXL.Layouts.EdgeBundler" /> object to do this, or you can
    /// perform your own calculations.
    /// </para>
    ///
    /// <para>
    /// Changing this property fires the <see cref="CurveStyleChanged" />
    /// event.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public EdgeCurveStyle
    CurveStyle
    {
        get
        {
            AssertValid();

            return (m_eCurveStyle);
        }

        set
        {
            if (m_eCurveStyle == value)
            {
                return;
            }

            m_eCurveStyle = value;

            EventUtil.FireEvent(this, this.CurveStyleChanged);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BezierDisplacementFactor
    //
    /// <summary>
    /// Gets or sets a factor that determines the edge curvature when edges are
    /// drawn as Bezier curves.
    /// </summary>
    ///
    /// <value>
    /// A factor that determines the curvature of Bezier curves.  Must be
    /// greater than or equal to zero.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="CurveStyle" /> is set to <see
    /// cref="EdgeCurveStyle.Bezier" />.  It is the distance of the edge's
    /// Bezier control point from the midpoint of a straight line connecting
    /// the edge's endpoints, expressed as a multiple of the straight line's
    /// length.
    ///
    /// <para>
    /// If the value is zero, edges are drawn as straight lines.  You can also
    /// specify that straight lines should be drawn by setting <see
    /// cref="CurveStyle" /> to <see cref="EdgeCurveStyle.Straight" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Double
    BezierDisplacementFactor
    {
        get
        {
            AssertValid();

            return (m_dBezierDisplacementFactor);
        }

        set
        {
            m_dBezierDisplacementFactor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TryDrawEdge()
    //
    /// <summary>
    /// Draws an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to draw.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="edgeDrawingHistory">
    /// Where an <see cref="EdgeDrawingHistory" /> object that retains
    /// information about how the edge was drawn gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the edge was drawn, false if the edge is hidden.
    /// </returns>
    ///
    /// <remarks>
    /// This method should be called repeatedly while a graph is being drawn,
    /// once for each of the graph's edges.  The <see
    /// cref="IVertex.Location" /> property on all of the graph's vertices must
    /// be set by ILayout.LayOutGraph before this method is called.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryDrawEdge
    (
        IEdge edge,
        GraphDrawingContext graphDrawingContext,
        out EdgeDrawingHistory edgeDrawingHistory
    )
    {
        Debug.Assert(edge != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        edgeDrawingHistory = null;

        if (graphDrawingContext.GraphRectangleMinusMarginIsEmpty)
        {
            return (false);
        }

        // If the edge is hidden, do nothing.

        VisibilityKeyValue eVisibility = GetVisibility(edge);

        if (eVisibility == VisibilityKeyValue.Hidden)
        {
            return (false);
        }

        DrawingVisual oDrawingVisual = new DrawingVisual();

        using ( DrawingContext oDrawingContext = oDrawingVisual.RenderOpen() )
        {
            Boolean bDrawAsSelected = GetDrawAsSelected(edge);
            Color oColor = GetColor(edge, eVisibility, bDrawAsSelected);
            Double dWidth = GetWidth(edge);
            DashStyle oDashStyle = GetDashStyle(edge, dWidth, bDrawAsSelected);

            Boolean bDrawArrow =
                (m_bDrawArrowOnDirectedEdge && edge.IsDirected);

            IVertex oVertex1 = edge.Vertex1;
            IVertex oVertex2 = edge.Vertex2;

            if (edge.IsSelfLoop)
            {
                if ( !TryDrawSelfLoop(oVertex1, oDrawingContext,
                    graphDrawingContext, oColor, dWidth, bDrawArrow) )
                {
                    // The edge's vertex is hidden, so the edge should be
                    // hidden also.

                    return (false);
                }
            }
            else
            {
                VertexDrawingHistory oVertex1DrawingHistory,
                    oVertex2DrawingHistory;

                Point oEdgeEndpoint1, oEdgeEndpoint2;

                if (
                    !TryGetVertexInformation(oVertex1, oVertex2,
                        out oVertex1DrawingHistory, out oVertex2DrawingHistory,
                        out oEdgeEndpoint1, out oEdgeEndpoint2)
                    ||
                    oEdgeEndpoint1 == oEdgeEndpoint2
                    )
                {
                    // One of the edge's vertices is hidden, so the edge should
                    // be hidden also, or the edge has zero length.

                    return (false);
                }

                Pen oPen = GetPen(oColor, dWidth, oDashStyle);

                Object oLabelAsObject;
                String sLabel = null;

                if ( edge.TryGetValue(ReservedMetadataKeys.PerEdgeLabel,
                    typeof(String), out oLabelAsObject)
                    && oLabelAsObject != null)
                {
                    sLabel = (String)oLabelAsObject;
                }

                if (m_eCurveStyle ==
                    EdgeCurveStyle.CurveThroughIntermediatePoints)
                {
                    DrawCurveThroughIntermediatePoints(
                        edge, graphDrawingContext, oDrawingContext,
                        oVertex1DrawingHistory, oVertex2DrawingHistory,
                        oEdgeEndpoint1, oEdgeEndpoint2, oPen);
                }
                else if (this.ShouldDrawBezierCurve)
                {
                    DrawBezierCurve(
                        edge, graphDrawingContext, oDrawingContext,
                        oEdgeEndpoint1, oEdgeEndpoint2, bDrawAsSelected,
                        bDrawArrow, oPen, oColor, dWidth, eVisibility, sLabel);
                }
                else
                {
                    DrawStraightEdge(
                        edge, graphDrawingContext, oDrawingContext,
                        oEdgeEndpoint1, oEdgeEndpoint2, bDrawAsSelected,
                        bDrawArrow, oPen, oColor, dWidth, eVisibility, sLabel);
                }
            }

            // Retain information about the edge that was drawn.

            edgeDrawingHistory = new EdgeDrawingHistory(
                edge, oDrawingVisual, bDrawAsSelected, dWidth);

            return (true);
        }
    }

    //*************************************************************************
    //  Event: CurveStyleChanged
    //
    /// <summary>
    /// Occurs when the <see cref="CurveStyle" /> property is changed.
    /// </summary>
    //*************************************************************************

    public event EventHandler CurveStyleChanged;


    //*************************************************************************
    //  Property: ShouldDrawBezierCurve
    //
    /// <summary>
    /// Gets a flag indicating whether edges should be drawn as Bezier curves.
    /// </summary>
    ///
    /// <value>
    /// true to draw edges as Bezier curves, false to draw them as straight
    /// lines.
    /// </value>
    //*************************************************************************

    protected Boolean
    ShouldDrawBezierCurve
    {
        get
        {
            AssertValid();

            // If the displacement factor is 0, default to a straight line.

            return ( (m_eCurveStyle == EdgeCurveStyle.Bezier) &&
                m_dBezierDisplacementFactor > 0 );
        }
    }

    //*************************************************************************
    //  Method: TryDrawSelfLoop()
    //
    /// <summary>
    /// Attempts to draw an edge that is a self-loop.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to connect to itself.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oColor">
    /// The edge color.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The edge width.
    /// </param>
    ///
    /// <param name="bDrawArrow">
    /// true if an arrow should be drawn on the self-loop.
    /// </param>
    ///
    /// <returns>
    /// true if the self loop was drawn, false if the edge's vertex is hidden.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryDrawSelfLoop
    (
        IVertex oVertex,
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        Color oColor,
        Double dWidth,
        Boolean bDrawArrow
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(dWidth >= 0);
        AssertValid();

        // Retrieve the information about how the vertex was drawn.

        VertexDrawingHistory oVertexDrawingHistory;

        if ( !TryGetVertexDrawingHistory(oVertex, out oVertexDrawingHistory) )
        {
            // The edge's vertex is hidden, so the edge should be hidden also.

            return (false);
        }

        // Determine the edge of the graph rectangle that is farthest from the
        // vertex.

        Point oVertexLocation = oVertexDrawingHistory.VertexLocation;

        RectangleEdge eFarthestGraphRectangleEdge =
            WpfGraphicsUtil.GetFarthestRectangleEdge(oVertexLocation,
            oGraphDrawingContext.GraphRectangle);

        // Get the point on the vertex at which to draw the self-loop.

        Point oSelfLoopEndpoint = oVertexDrawingHistory.GetSelfLoopEndpoint(
            eFarthestGraphRectangleEdge);

        DrawSelfLoopAt(oDrawingContext, oGraphDrawingContext, oColor, dWidth,
            oSelfLoopEndpoint, eFarthestGraphRectangleEdge, bDrawArrow);

        return (true);
    }

    //*************************************************************************
    //  Method: DrawSelfLoopAt()
    //
    /// <summary>
    /// Draws an edge that is a self-loop at a specified endpoint.
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
    /// <param name="oColor">
    /// The edge color.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The edge width.
    /// </param>
    ///
    /// <param name="oSelfLoopEndpoint">
    /// The point on the vertex at which to draw the self-loop.
    /// </param>
    ///
    /// <param name="eFarthestGraphRectangleEdge">
    /// The edge of the graph rectangle that is farthest from the vertex.
    /// </param>
    ///
    /// <param name="bDrawArrow">
    /// true if an arrow should be drawn on the self-loop.
    /// </param>
    //*************************************************************************

    protected void
    DrawSelfLoopAt
    (
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        Color oColor,
        Double dWidth,
        Point oSelfLoopEndpoint,
        RectangleEdge eFarthestGraphRectangleEdge,
        Boolean bDrawArrow
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(dWidth >= 0);
        AssertValid();

        // The self-loop is drawn as a circle.  Figure out the location of the
        // circle's center and the tip of the arrow, if there is an arrow.

        Double dCircleX, dCircleY, dArrowTipX, dArrowTipY, dArrowAngle;

        dCircleX = dArrowTipX = oSelfLoopEndpoint.X;
        dCircleY = dArrowTipY = oSelfLoopEndpoint.Y;
        Double dSelfLoopCircleDiameter = 2.0 * SelfLoopCircleRadius;
        dArrowAngle = 0;

        switch (eFarthestGraphRectangleEdge)
        {
            case RectangleEdge.Top:

                dCircleY -= SelfLoopCircleRadius;
                dArrowTipY -= dSelfLoopCircleDiameter;
                break;

            case RectangleEdge.Left:

                dCircleX -= SelfLoopCircleRadius;
                dArrowTipX -= dSelfLoopCircleDiameter;
                dArrowAngle = Math.PI / 2.0;  // (90 degrees.)
                break;

            case RectangleEdge.Right:

                dCircleX += SelfLoopCircleRadius;
                dArrowTipX += dSelfLoopCircleDiameter;
                dArrowAngle = -Math.PI / 2.0;  // (-90 degrees.)
                break;

            case RectangleEdge.Bottom:

                dCircleY += SelfLoopCircleRadius;
                dArrowTipY += dSelfLoopCircleDiameter;
                dArrowAngle = Math.PI;  // (180 degrees.)
                break;

            default:

                Debug.Assert(false);
                break;
        }

        oDrawingContext.DrawEllipse(null, GetPen(oColor, dWidth),
            new Point(dCircleX, dCircleY), SelfLoopCircleRadius,
            SelfLoopCircleRadius);

        if (bDrawArrow)
        {
            // Rotate the arrow slightly to adjust to the circular shape of the
            // edge connected to it.

            dArrowAngle += Math.PI / 13.0;

            DrawArrow(oDrawingContext, new Point(dArrowTipX, dArrowTipY),
                dArrowAngle, oColor, dWidth);
        }
    }

    //*************************************************************************
    //  Method: DrawStraightEdge()
    //
    /// <summary>
    /// Draws a straight edge.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to draw.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint1">
    /// The edge's first endpoint.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint2">
    /// The edge's second endpoint.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the edge as selected.
    /// </param>
    ///
    /// <param name="bDrawArrow">
    /// true if an arrow should be drawn on the edge.
    /// </param>
    ///
    /// <param name="oPen">
    /// The Pen to use.
    /// </param>
    ///
    /// <param name="oColor">
    /// The edge color.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The edge width.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The edge's visibility.
    /// </param>
    ///
    /// <param name="sLabel">
    /// The edge's label.  Can be empty or null.
    /// </param>
    //*************************************************************************

    protected void
    DrawStraightEdge
    (
        IEdge oEdge,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        Point oEdgeEndpoint1,
        Point oEdgeEndpoint2,
        Boolean bDrawAsSelected,
        Boolean bDrawArrow,
        Pen oPen,
        Color oColor,
        Double dWidth,
        VisibilityKeyValue eVisibility,
        String sLabel
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oPen != null);
        AssertValid();

        if (bDrawArrow)
        {
            // Draw the arrow and set the second endpoint to the center of the
            // flat end of the arrow.

            Double dArrowAngle = WpfGraphicsUtil.GetAngleBetweenPointsRadians(
                oEdgeEndpoint1, oEdgeEndpoint2);

            oEdgeEndpoint2 = DrawArrow(oDrawingContext, oEdgeEndpoint2,
                dArrowAngle, oColor, dWidth);
        }

        oDrawingContext.DrawLine(oPen, oEdgeEndpoint1, oEdgeEndpoint2);

        if ( !String.IsNullOrEmpty(sLabel) )
        {
            DrawLabel(oEdge, oDrawingContext, oGraphDrawingContext,
                bDrawAsSelected, oColor, eVisibility, oEdgeEndpoint1,
                oEdgeEndpoint2, null, new Point(), sLabel);
        }
    }

    //*************************************************************************
    //  Method: DrawBezierCurve()
    //
    /// <summary>
    /// Draws an edge as a quadratic Bezier curve.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to draw.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint1">
    /// The edge's first endpoint.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint2">
    /// The edge's second endpoint.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the edge as selected.
    /// </param>
    ///
    /// <param name="bDrawArrow">
    /// true if an arrow should be drawn on the edge.
    /// </param>
    ///
    /// <param name="oPen">
    /// The Pen to use.
    /// </param>
    ///
    /// <param name="oColor">
    /// The edge color.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The edge width.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The edge's visibility.
    /// </param>
    ///
    /// <param name="sLabel">
    /// The edge's label.  Can be empty or null.
    /// </param>
    //*************************************************************************

    protected void
    DrawBezierCurve
    (
        IEdge oEdge,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        Point oEdgeEndpoint1,
        Point oEdgeEndpoint2,
        Boolean bDrawAsSelected,
        Boolean bDrawArrow,
        Pen oPen,
        Color oColor,
        Double dWidth,
        VisibilityKeyValue eVisibility,
        String sLabel
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oPen != null);
        AssertValid();

        Point oBezierControlPoint = GetBezierControlPoint(oGraphDrawingContext,
            oEdgeEndpoint1, oEdgeEndpoint2, m_dBezierDisplacementFactor);

        if (bDrawArrow)
        {
            // When the edge is a Bezier curve, the arrow should be aligned
            // along the tangent of the curve at the second endpoint.  The
            // tangent runs from the second endpoint to the Bezier control
            // point.

            Double dArrowAngle = WpfGraphicsUtil.GetAngleBetweenPointsRadians(
                oBezierControlPoint, oEdgeEndpoint2);

            // Draw the arrow and set the second endpoint to the center of the
            // flat end of the arrow.  Note that in the Bezier case, moving the
            // second endpoint causes a small distortion of the Bezier curve.

            oEdgeEndpoint2 = DrawArrow(oDrawingContext, oEdgeEndpoint2,
                dArrowAngle, oColor, dWidth);
        }

        PathGeometry oBezierCurve =
            WpfPathGeometryUtil.GetQuadraticBezierCurve(oEdgeEndpoint1,
                oEdgeEndpoint2, oBezierControlPoint);

        oDrawingContext.DrawGeometry(null, oPen, oBezierCurve);

        if ( !String.IsNullOrEmpty(sLabel) )
        {
            DrawLabel(oEdge, oDrawingContext, oGraphDrawingContext,
                bDrawAsSelected, oColor, eVisibility, oEdgeEndpoint1,
                oEdgeEndpoint2, oBezierCurve, oBezierControlPoint, sLabel);
        }
    }

    //*************************************************************************
    //  Method: DrawCurveThroughIntermediatePoints()
    //
    /// <summary>
    /// Draws an edge as a curve through the intermediate points specified in
    /// the edge's metadata.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to draw.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oVertex1DrawingHistory">
    /// The drawing history for the edge's first endpoint.
    /// </param>
    ///
    /// <param name="oVertex2DrawingHistory">
    /// The drawing history for the edge's second endpoint.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint1">
    /// The edge's first endpoint.  This was calculated as if the edge was
    /// straight.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint2">
    /// The edge's second endpoint.  This was calculated as if the edge was
    /// straight.
    /// </param>
    ///
    /// <param name="oPen">
    /// The Pen to use.
    /// </param>
    //*************************************************************************

    protected void
    DrawCurveThroughIntermediatePoints
    (
        IEdge oEdge,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        VertexDrawingHistory oVertex1DrawingHistory,
        VertexDrawingHistory oVertex2DrawingHistory,
        Point oEdgeEndpoint1,
        Point oEdgeEndpoint2,
        Pen oPen
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oVertex1DrawingHistory != null);
        Debug.Assert(oVertex2DrawingHistory != null);
        Debug.Assert(oPen != null);
        AssertValid();

        // Note: Don't attempt to draw an arrow in this case.

        // Create a list of intermediate points, excluding those that fall
        // within the vertex bounds.  An edge always terminates on the vertex
        // bounds, so we don't want it venturing into the vertex itself.

        List<Point> oCurvePoints = FilterIntermediatePoints(oEdge,
            oVertex1DrawingHistory, oVertex2DrawingHistory);

        Int32 iCurvePoints = oCurvePoints.Count;

        if (iCurvePoints == 0)
        {
            // Just draw a straight line.

            oDrawingContext.DrawLine(oPen, oEdgeEndpoint1, oEdgeEndpoint2);
            return;
        }

        #if false
        // Draw intermediate points for testing.

        foreach (Point oPoint in oCurvePoints)
        {
            oDrawingContext.DrawEllipse(Brushes.Green, null, oPoint, 2, 2);
        }
        #endif

        // The endpoints were originally calculated as if the edge was a
        // straight line between the two vertices.  Recalculate them so they
        // connect more smoothly to the adjacent intermediate curve points.

        oVertex1DrawingHistory.GetEdgeEndpoint(oCurvePoints[0],
            out oEdgeEndpoint1);

        oVertex2DrawingHistory.GetEdgeEndpoint(oCurvePoints[iCurvePoints - 1],
            out oEdgeEndpoint2);

        oCurvePoints.Insert(0, oEdgeEndpoint1);
        oCurvePoints.Add(oEdgeEndpoint2);

        PathGeometry oCurveThroughPoints =
            WpfPathGeometryUtil.GetCurveThroughPoints(oCurvePoints, 0.5,
                CurveThroughIntermediatePointsTolerance);

        oDrawingContext.DrawGeometry(null, oPen, oCurveThroughPoints);

        // Note: Don't attempt to draw a label in this case.
    }

    //*************************************************************************
    //  Method: FilterIntermediatePoints()
    //
    /// <summary>
    /// Get a list of an edge's intermediate curve points, excluding those that
    /// fall within the vertex bounds.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to draw.
    /// </param>
    ///
    /// <param name="oVertex1DrawingHistory">
    /// The drawing history for the edge's first endpoint.
    /// </param>
    ///
    /// <param name="oVertex2DrawingHistory">
    /// The drawing history for the edge's second endpoint.
    /// </param>
    ///
    /// <remarks>
    /// A filtered list of intermediate curve points.  The list may be empty.
    /// </remarks>
    //*************************************************************************

    protected List<Point>
    FilterIntermediatePoints
    (
        IEdge oEdge,
        VertexDrawingHistory oVertex1DrawingHistory,
        VertexDrawingHistory oVertex2DrawingHistory
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oVertex1DrawingHistory != null);
        Debug.Assert(oVertex2DrawingHistory != null);
        AssertValid();

        List<Point> oFilteredIntermediatePoints = new List<Point>();
        System.Drawing.PointF [] aoIntermediateCurvePoints;

        DrawingVisual oVertex1DrawingVisual =
            oVertex1DrawingHistory.DrawingVisual;

        DrawingVisual oVertex2DrawingVisual =
            oVertex2DrawingHistory.DrawingVisual;

        if ( EdgeUtil.TryGetIntermediateCurvePoints(oEdge,
            out aoIntermediateCurvePoints) )
        {
            foreach (System.Drawing.PointF oIntermediateCurvePoint in 
                aoIntermediateCurvePoints)
            {
                Point oIntermediateCurveWpfPoint =
                    WpfGraphicsUtil.PointFToWpfPoint(oIntermediateCurvePoint);

                // Check whether the point falls within the bounds of either
                // vertex.
                //
                // This could be done more quickly but with more code by
                // selectively hit-testing only vertex 1 or vertex 2 as we move
                // along the curve.  However, the hit-testing is so fast that
                // it's probably not worth the extra complexity.  On one test
                // machine, for example, 200,000 hit tests took about 24 ms,
                // and that didn't vary much with vertex shape or size.

                if (
                    oVertex1DrawingVisual.HitTest(
                        oIntermediateCurveWpfPoint) == null
                    &&
                    oVertex2DrawingVisual.HitTest(
                        oIntermediateCurveWpfPoint) == null
                    )
                {
                    oFilteredIntermediatePoints.Add(
                        oIntermediateCurveWpfPoint);
                }
            }
        }

        return (oFilteredIntermediatePoints);
    }

    //*************************************************************************
    //  Method: GetWidth()
    //
    /// <summary>
    /// Gets the width of an edge.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to get the width for.
    /// </param>
    ///
    /// <returns>
    /// The width of the edge.
    /// </returns>
    //*************************************************************************

    protected Double
    GetWidth
    (
        IEdge oEdge
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        // Start with the default width.

        Double dWidth = m_dWidth;

        Object oPerEdgeWidthAsObject;

        // Check for a per-edge width.  Note that the width is stored as a
        // Single in the edge's metadata to reduce memory usage.

        if ( oEdge.TryGetValue(ReservedMetadataKeys.PerEdgeWidth,
            typeof(Single), out oPerEdgeWidthAsObject) )
        {
            dWidth = (Double)(Single)oPerEdgeWidthAsObject;

            if (dWidth < MinimumWidth || dWidth > MaximumWidth)
            {
                throw new FormatException( String.Format(

                    "{0}: The edge with the ID {1} has an out-of-range {2}"
                    + " value.  Valid values are between {3} and {4}."
                    ,
                    this.ClassName,
                    oEdge.ID,
                    "ReservedMetadataKeys.PerEdgeWidth",
                    MinimumWidth,
                    MaximumWidth
                    ) );
            }
        }

        return (dWidth * m_dGraphScale);
    }

    //*************************************************************************
    //  Method: GetDashStyle()
    //
    /// <summary>
    /// Gets the DashStyle of an edge.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to get the DashStyle for.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The edge width.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the edge as selected.
    /// </param>
    ///
    /// <returns>
    /// The DashStyle of the edge.
    /// </returns>
    //*************************************************************************

    protected DashStyle
    GetDashStyle
    (
        IEdge oEdge,
        Double dWidth,
        Boolean bDrawAsSelected
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(dWidth >= 0);
        AssertValid();

        // Note:
        //
        // An early implementation used the predefined DashStyle objects
        // provided by the DashStyles class, but those did not look good at
        // smaller edge widths.  This implementation builds the DashStyle's
        // Dashes collection from scratch.

        Double [] adDashes = null;

        if (!bDrawAsSelected)
        {
            Object oPerEdgeStyleAsObject;

            // Check for a per-edge style.

            if ( oEdge.TryGetValue(ReservedMetadataKeys.PerEdgeStyle,
                typeof(EdgeStyle), out oPerEdgeStyleAsObject) )
            {
                switch ( (EdgeStyle)oPerEdgeStyleAsObject )
                {
                    case EdgeStyle.Solid:

                        break;

                    case EdgeStyle.Dash:

                        adDashes = new Double[] {4.0, 2.0};
                        break;

                    case EdgeStyle.Dot:

                        adDashes = new Double[] {1.0, 2.0};
                        break;

                    case EdgeStyle.DashDot:

                        adDashes = new Double[] {4.0, 2.0, 1.0, 2.0};
                        break;

                    case EdgeStyle.DashDotDot:

                        adDashes = new Double[] {4.0, 2.0, 1.0, 2.0, 1.0, 2.0};
                        break;

                    default:

                        throw new FormatException( String.Format(

                            "{0}: The edge with the ID {1} has an invalid {2}"
                            + " value."
                            ,
                            this.ClassName,
                            oEdge.ID,
                            "ReservedMetadataKeys.PerEdgeStyle"
                            ) );
                }
            }
        }

        if (adDashes == null)
        {
            return (DashStyles.Solid);
        }

        DashStyle oDashStyle = new DashStyle();
        oDashStyle.Dashes = new DoubleCollection(adDashes);
        WpfGraphicsUtil.FreezeIfFreezable(oDashStyle);

        return (oDashStyle);
    }

    //*************************************************************************
    //  Method: GetLabelFontSize()
    //
    /// <summary>
    /// Gets the font size to use for an edge's label.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to get the font size for.
    /// </param>
    ///
    /// <returns>
    /// The font size to use, in WPF units.
    /// </returns>
    //*************************************************************************

    protected Double
    GetLabelFontSize
    (
        IEdge oEdge
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        // Start with the default font size.

        Double dLabelFontSize = m_oFormattedTextManager.FontSize;

        Object oPerEdgeLabelFontSizeAsObject;

        // Check for a per-edge label font size.  Note that the font size is
        // stored as a Single in the edge's metadata to reduce memory usage.

        if ( oEdge.TryGetValue(ReservedMetadataKeys.PerEdgeLabelFontSize,
            typeof(Single), out oPerEdgeLabelFontSizeAsObject) )
        {
            dLabelFontSize = (Double)(Single)oPerEdgeLabelFontSizeAsObject;

            if (dLabelFontSize <= 0)
            {
                throw new FormatException( String.Format(

                    "{0}: The edge with the ID {1} has an out-of-range {2}"
                    + " value.  Values must be greater than zero."
                    ,
                    this.ClassName,
                    oEdge.ID,
                    "ReservedMetadataKeys.PerEdgeLabelFontSize"
                    ) );
            }
        }

        return (dLabelFontSize);
    }

    //*************************************************************************
    //  Method: GetLabelTextColor()
    //
    /// <summary>
    /// Gets the text color to use for an edge's label.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to draw the label for.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the label as selected.
    /// </param>
    ///
    /// <param name="oEdgeColor">
    /// The edge's color.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The edge's visibility.
    /// </param>
    //*************************************************************************

    protected Color
    GetLabelTextColor
    (
        IEdge oEdge,
        Boolean bDrawAsSelected,
        Color oEdgeColor,
        VisibilityKeyValue eVisibility
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        Color oLabelTextColor;

        if (bDrawAsSelected)
        {
            // The edge color was calculated using the selected state.

            oLabelTextColor = oEdgeColor;
        }
        else
        {
            oLabelTextColor = GetColor(oEdge, eVisibility,
                ReservedMetadataKeys.PerEdgeLabelTextColor, m_oLabelTextColor,
                false);

            oLabelTextColor = WpfGraphicsUtil.SetWpfColorAlpha(oLabelTextColor,
                oEdgeColor.A);
        }

        return (oLabelTextColor);
    }

    //*************************************************************************
    //  Method: TryGetVertexInformation()
    //
    /// <summary>
    /// Attempts to get information about the edge's vertices.
    /// </summary>
    ///
    /// <param name="oVertex1">
    /// The edge's first vertex.
    /// </param>
    ///
    /// <param name="oVertex2">
    /// The edge's second vertex.
    /// </param>
    ///
    /// <param name="oVertex1DrawingHistory">
    /// Where the drawing history for the edge's first endpoint gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <param name="oVertex2DrawingHistory">
    /// Where the drawing history for the edge's second endpoint gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint1">
    /// Where the edge's first endpoint gets stored if true is returned.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint2">
    /// Where the edge's second endpoint gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the information was obtained, false if one of the edge's
    /// vertices is hidden.
    /// </returns>
    ///
    /// <remarks>
    /// The edge's first endpoint is the endpoint on the <paramref
    /// name="oVertex1" /> side of the edge.  The edge's second endpoint is the
    /// endpoint on the <paramref name="oVertex2" /> side of the edge.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetVertexInformation
    (
        IVertex oVertex1,
        IVertex oVertex2,
        out VertexDrawingHistory oVertex1DrawingHistory,
        out VertexDrawingHistory oVertex2DrawingHistory,
        out Point oEdgeEndpoint1,
        out Point oEdgeEndpoint2
    )
    {
        Debug.Assert(oVertex1 != null);
        Debug.Assert(oVertex2 != null);
        AssertValid();

        oVertex1DrawingHistory = oVertex2DrawingHistory = null;
        oEdgeEndpoint1 = oEdgeEndpoint2 = new Point();

        // Retrieve the information about how the vertices were drawn.

        if (
            !TryGetVertexDrawingHistory(oVertex1, out oVertex1DrawingHistory)
            ||
            !TryGetVertexDrawingHistory(oVertex2, out oVertex2DrawingHistory)
            )
        {
            // One of the edge's vertices is hidden.

            return (false);
        }

        // The drawing histories determine the edge endpoints.  For example, if
        // oVertex1 was drawn as a circle, then oVertex1DrawingHistory is a
        // CircleVertexDrawingHistory that knows to put its endpoint on the
        // circle itself and not at the circle's center.

        oVertex1DrawingHistory.GetEdgeEndpoint(
            oVertex2DrawingHistory.VertexLocation, out oEdgeEndpoint1);

        oVertex2DrawingHistory.GetEdgeEndpoint(
            oVertex1DrawingHistory.VertexLocation, out oEdgeEndpoint2);

        return (true);
    }

    //*************************************************************************
    //  Method: DrawArrow()
    //
    /// <summary>
    /// Draws an arrow whose tip is at a specified point.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oArrowTipLocation">
    /// Where the tip of the arrow gets drawn.
    /// </param>
    ///
    /// <param name="dArrowAngle">
    /// The angle of the arrow.  Ranges between 0 and PI radians (0 to 180
    /// degrees) and 0 to -PI radians (0 to -180 degrees).  If 0, the arrow
    /// points to the right.
    /// </param>
    ///
    /// <param name="oColor">
    /// The color of the arrow.
    /// </param>
    ///
    /// <param name="dEdgeWidth">
    /// The width of the edge that will connect to the arrow.
    /// </param>
    ///
    /// <returns>
    /// The point at the center of the flat end of the arrow.  This can be used
    /// when drawing the line that connects to the arrow.  (Don't draw a line
    /// to <paramref name="oArrowTipLocation" />, because the line's endcap
    /// will overlap the tip of the arrow.)
    /// </returns>
    //*************************************************************************

    protected Point
    DrawArrow
    (
        DrawingContext oDrawingContext,
        Point oArrowTipLocation,
        Double dArrowAngle,
        Color oColor,
        Double dEdgeWidth
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(dEdgeWidth > 0);
        AssertValid();

        // Compute the arrow's dimensions.  The width factor is arbitrary and
        // was determined experimentally.

        const Double WidthFactor = 1.5;

        Double dArrowTipX = oArrowTipLocation.X;
        Double dArrowTipY = oArrowTipLocation.Y;
        Double dArrowWidth = WidthFactor * dEdgeWidth * m_dRelativeArrowSize;
        Double dArrowHalfHeight = dArrowWidth / 2.0;
        Double dX = dArrowTipX - dArrowWidth;

        // Compute the arrow's three points as if the arrow were at an angle of
        // zero degrees, then use a rotated transform to adjust for the actual
        // specified angle.

        Point [] aoPoints = new Point [] {

            // Index 0: Arrow tip.

            oArrowTipLocation,

            // Index 1: Arrow bottom.

            new Point(dX, dArrowTipY - dArrowHalfHeight),

            // Index 2: Arrow top.

            new Point(dX, dArrowTipY + dArrowHalfHeight),

            // Index 3: Center of the flat end of the arrow.
            //
            // Note: The 0.2 is to avoid a gap between the edge endcap and the
            // flat end of the arrow, but it sometimes causes the two to
            // overlap slightly, and that can show if the edge isn't opaque.
            // What is the correct way to get the endcap to merge invisibly
            // with the arrow?

            new Point(dX + 0.2, dArrowTipY)
            };

        Matrix oMatrix = WpfGraphicsUtil.GetRotatedMatrix( oArrowTipLocation,
            -MathUtil.RadiansToDegrees(dArrowAngle) );

        oMatrix.Transform(aoPoints);

        PathGeometry oArrow = WpfPathGeometryUtil.GetPathGeometryFromPoints(
            aoPoints[0], aoPoints[1], aoPoints[2] );

        oDrawingContext.DrawGeometry(GetBrush(oColor), null, oArrow);

        return ( aoPoints[3] );
    }

    //*************************************************************************
    //  Method: DrawLabel()
    //
    /// <summary>
    /// Draws an edge's label.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to draw the label for.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the label as selected.
    /// </param>
    ///
    /// <param name="oEdgeColor">
    /// The edge's color.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The edge's visibility.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint1">
    /// The edge's first endpoint.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint2">
    /// The edge's second endpoint.
    /// </param>
    ///
    /// <param name="oBezierCurve">
    /// The Bezier curve used for the edge, or null if the edge is straight.
    /// </param>
    ///
    /// <param name="oBezierControlPoint">
    /// The Bezier control point used for the edge.  Not used if the edge is
    /// straight.
    /// </param>
    ///
    /// <param name="sLabel">
    /// The edge's label.  Can be empty but not null.
    /// </param>
    ///
    /// <remarks>
    /// This method will not properly draw a label for a self-loop.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawLabel
    (
        IEdge oEdge,
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        Boolean bDrawAsSelected,
        Color oEdgeColor,
        VisibilityKeyValue eVisibility,
        Point oEdgeEndpoint1,
        Point oEdgeEndpoint2,
        PathGeometry oBezierCurve,
        Point oBezierControlPoint,
        String sLabel
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(sLabel != null);
        AssertValid();

        if (sLabel.Length == 0)
        {
            return;
        }

        sLabel = TruncateLabel(sLabel);

        Double dFontSize = GetLabelFontSize(oEdge);

        Color oLabelTextColor = GetLabelTextColor(oEdge, bDrawAsSelected,
            oEdgeColor, eVisibility);

        FormattedText oFormattedText =
            m_oFormattedTextManager.CreateFormattedText(sLabel,
                oLabelTextColor, dFontSize, m_dGraphScale);

        oFormattedText.Trimming = TextTrimming.CharacterEllipsis;

        // Don't let the FormattedText class break long lines when drawing
        // Bezier curves.  DrawBezierLabel(), which draws the label
        // character-by-character, is unable to tell where such line breaks
        // occur, because FormattedText doesn't expose this information.
        //
        // For consistency, do the same when drawing straight edges.

        oFormattedText.MaxLineCount = sLabel.Count(c => c == '\n') + 1;

        // The ends of the label text are between one and two "buffer units"
        // from the ends of the edge.  The buffer unit is the width of an
        // arbitrary character.

        Double dBufferWidth =
            m_oFormattedTextManager.CreateFormattedText("i", oLabelTextColor,
                dFontSize, m_dGraphScale).Width;

        Double dEdgeLength = WpfGraphicsUtil.GetDistanceBetweenPoints(
            oEdgeEndpoint1, oEdgeEndpoint2);

        Double dEdgeLengthMinusBuffers = dEdgeLength - 2 * dBufferWidth;

        if (dEdgeLengthMinusBuffers <= 0)
        {
            return;
        }

        // Determine where to draw the label text.

        Double dTextWidth = oFormattedText.Width;
        Double dLabelOriginAsFractionOfEdgeLength;

        if (dTextWidth > dEdgeLengthMinusBuffers)
        {
            // The label text should start one buffer unit from the first
            // endpoint, and terminate with ellipses approximately one buffer
            // length from the second endpoint.

            dLabelOriginAsFractionOfEdgeLength = dBufferWidth / dEdgeLength;

            oFormattedText.MaxTextWidth = dEdgeLengthMinusBuffers;
        }
        else
        {
            // The label should be centered along the edge's length.

            dLabelOriginAsFractionOfEdgeLength = 
                ( (dEdgeLength - dTextWidth) / 2.0 ) / dEdgeLength;
        }

        // Note: Don't make the translucent rectangle any more opaque than the
        // edge, which might be translucent itself.

        Color oTranslucentRectangleColor = WpfGraphicsUtil.SetWpfColorAlpha(
            oGraphDrawingContext.BackColor,
            Math.Min(LabelBackgroundAlpha, oEdgeColor.A)
            );

        if (this.ShouldDrawBezierCurve)
        {
            DrawBezierLabel(oDrawingContext, oGraphDrawingContext,
                oFormattedText, oEdgeEndpoint1, oEdgeEndpoint2, oBezierCurve,
                dLabelOriginAsFractionOfEdgeLength, dEdgeLength,
                dBufferWidth, oLabelTextColor, oTranslucentRectangleColor,
                dFontSize);
        }
        else
        {
            DrawStraightLabel(oDrawingContext, oGraphDrawingContext,
                oFormattedText, oEdgeEndpoint1, oEdgeEndpoint2,
                dLabelOriginAsFractionOfEdgeLength, dEdgeLength, dBufferWidth,
                oTranslucentRectangleColor);
        }
    }

    //*************************************************************************
    //  Method: DrawStraightLabel()
    //
    /// <summary>
    /// Draws an edge's label when the edge is straight.
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
    /// The FormattedText object for the label.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint1">
    /// The edge's first endpoint.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint2">
    /// The edge's second endpoint.
    /// </param>
    ///
    /// <param name="dLabelOriginAsFractionOfEdgeLength">
    /// The label's leftmost point, as a fraction of the edge length.
    /// </param>
    ///
    /// <param name="dEdgeLength">
    /// The edge's length.
    /// </param>
    ///
    /// <param name="dBufferWidth">
    /// The minimum distance between the end of the edge and the end of the
    /// label.
    /// </param>
    ///
    /// <param name="oTranslucentRectangleColor">
    /// The color to use for the translucent rectangle behind the label.
    /// </param>
    //*************************************************************************

    protected void
    DrawStraightLabel
    (
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        FormattedText oFormattedText,
        Point oEdgeEndpoint1,
        Point oEdgeEndpoint2,
        Double dLabelOriginAsFractionOfEdgeLength,
        Double dEdgeLength,
        Double dBufferWidth,
        Color oTranslucentRectangleColor
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oFormattedText != null);
        Debug.Assert(dLabelOriginAsFractionOfEdgeLength >= 0);
        Debug.Assert(dEdgeLength >= 0);
        Debug.Assert(dBufferWidth >= 0);
        AssertValid();

        if (oEdgeEndpoint2.X < oEdgeEndpoint1.X)
        {
            // Don't let text be drawn upside-down.

            WpfGraphicsUtil.SwapPoints(ref oEdgeEndpoint1, ref oEdgeEndpoint2);
        }

        // To avoid trigonometric calculations, use a RotateTransform to make
        // the edge look as if it is horizontal, with oEdgeEndpoint2 to the
        // right of oEdgeEndpoint1.

        Double dEdgeAngleDegrees = MathUtil.RadiansToDegrees(
            WpfGraphicsUtil.GetAngleBetweenPointsRadians(
                oEdgeEndpoint1, oEdgeEndpoint2) );

        RotateTransform oRotateTransform = new RotateTransform(
            dEdgeAngleDegrees, oEdgeEndpoint1.X, oEdgeEndpoint1.Y);

        oRotateTransform.Angle = -dEdgeAngleDegrees;
        oDrawingContext.PushTransform(oRotateTransform);

        Double dTextWidth = oFormattedText.Width;
        Double dEdgeLengthMinusBuffers = dEdgeLength - 2 * dBufferWidth;

        Point oLabelOrigin = oEdgeEndpoint1;

        // The text should be vertically centered on the edge.

        oLabelOrigin.Offset(dLabelOriginAsFractionOfEdgeLength * dEdgeLength,
            -oFormattedText.Height / 2.0);

        // Determine where to draw a translucent rectangle behind the text.
        // The translucent rectangle serves to obscure, but not completely
        // hide, the underlying edge.

        Rect oTranslucentRectangle;

        if (dTextWidth > dEdgeLengthMinusBuffers)
        {
            // The translucent rectangle should be the same width as the text.

            oTranslucentRectangle = new Rect( oLabelOrigin,
                new Size(dEdgeLengthMinusBuffers, oFormattedText.Height) );
        }
        else
        {
            // The label is centered along the edge's length.

            // The translucent rectangle should extend between zero and one
            // buffer units beyond the ends of the text.  This provides a
            // margin between the text and the unobscured edge, if there is
            // enough space.

            oTranslucentRectangle = new Rect( oLabelOrigin,

                new Size(
                    Math.Min(dTextWidth + 2 * dBufferWidth,
                        dEdgeLengthMinusBuffers),

                    oFormattedText.Height)
                );

            oTranslucentRectangle.Offset(-dBufferWidth, 0);
        }

        DrawTranslucentRectangle(oDrawingContext, oTranslucentRectangle,
            oTranslucentRectangleColor);

        oDrawingContext.DrawText(oFormattedText, oLabelOrigin);

        oDrawingContext.Pop();
    }

    //*************************************************************************
    //  Method: DrawBezierLabel()
    //
    /// <summary>
    /// Draws an edge's label when the edge is a Bezier curve.
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
    /// The FormattedText object for the label.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint1">
    /// The edge's first endpoint.
    /// </param>
    ///
    /// <param name="oEdgeEndpoint2">
    /// The edge's second endpoint.
    /// </param>
    ///
    /// <param name="oBezierCurve">
    /// The Bezier curve used for the edge.
    /// </param>
    ///
    /// <param name="dLabelOriginAsFractionOfEdgeLength">
    /// The label's leftmost point, as a fraction of the edge length.
    /// </param>
    ///
    /// <param name="dEdgeLength">
    /// The edge's length.
    /// </param>
    ///
    /// <param name="dBufferWidth">
    /// The minimum distance between the end of the edge and the end of the
    /// label.
    /// </param>
    ///
    /// <param name="oLabelTextColor">
    /// The color to use for the label's text.
    /// </param>
    ///
    /// <param name="oTranslucentRectangleColor">
    /// The color to use for the translucent rectangle behind the label.
    /// </param>
    ///
    /// <param name="dFontSize">
    /// The font size to use for the label's text.
    /// </param>
    //*************************************************************************

    protected void
    DrawBezierLabel
    (
        DrawingContext oDrawingContext,
        GraphDrawingContext oGraphDrawingContext,
        FormattedText oFormattedText,
        Point oEdgeEndpoint1,
        Point oEdgeEndpoint2,
        PathGeometry oBezierCurve,
        Double dLabelOriginAsFractionOfEdgeLength,
        Double dEdgeLength,
        Double dBufferWidth,
        Color oLabelTextColor,
        Color oTranslucentRectangleColor,
        Double dFontSize
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oFormattedText != null);
        Debug.Assert(oBezierCurve != null);
        Debug.Assert(dLabelOriginAsFractionOfEdgeLength >= 0);
        Debug.Assert(dEdgeLength >= 0);
        Debug.Assert(dBufferWidth >= 0);
        Debug.Assert(dFontSize > 0);
        AssertValid();

        // This method uses a modified version of the technique described in
        // "Render Text On A Path With WPF," by Charles Petzold, at
        // http://msdn.microsoft.com/en-us/magazine/dd263097.aspx.

        if (oEdgeEndpoint2.X < oEdgeEndpoint1.X)
        {
            // Don't let text be drawn upside-down.

            WpfGraphicsUtil.SwapPoints(ref oEdgeEndpoint1, ref oEdgeEndpoint2);

            oBezierCurve = WpfPathGeometryUtil.ReverseQuadraticBezierCurve(
                oBezierCurve);
        }

        Double dTextWidth = oFormattedText.Width;
        Double dTextWidthToEdgeLengthRatio = dTextWidth / dEdgeLength;

        Double dLineOffset = 0;
        Point oOrigin = new Point(0, 0);

        String [] asLines =
            oFormattedText.Text.Split( new char[] {'\r', '\n'} );

        foreach (String sLine in asLines)
        {
            // The label characters will be drawn one by one using positions
            // computed by PathGeometry.GetPointAtFractionLength().  The first
            // character will be at dLabelOriginAsFractionOfEdgeLength.

            Double dFractionOfEdgeLength = dLabelOriginAsFractionOfEdgeLength;

            Boolean bUsedEllipses = false;

            foreach (Char c in sLine)
            {
                if (bUsedEllipses)
                {
                    break;
                }

                Point oPointAtFractionLength;
                Point oPointTangent;

                oBezierCurve.GetPointAtFractionLength(dFractionOfEdgeLength,
                    out oPointAtFractionLength, out oPointTangent);

                Double dCharacterAngleDegrees = MathUtil.RadiansToDegrees(
                    Math.Atan2(oPointTangent.Y, oPointTangent.X) );

                String sChar = c.ToString();

                if ( (oPointAtFractionLength - oEdgeEndpoint2).Length <=
                    6.0 * dBufferWidth)
                {
                    // There is probably not enough room for the rest of the
                    // string.  Terminate it with ellipses.  (The buffer width
                    // multiple was determined experimentally.)

                    bUsedEllipses = true;
                    sChar = "...";
                }

                FormattedText oCharacterFormattedText =
                    m_oFormattedTextManager.CreateFormattedText(
                        sChar, oLabelTextColor, dFontSize, m_dGraphScale);

                Double dCharacterWidth =
                    oCharacterFormattedText.WidthIncludingTrailingWhitespace;

                Double dCharacterHeight = oCharacterFormattedText.Height;

                // Apply a RotateTransform to make the character's base
                // approximately parallel to the Bezier curve's tangent, and a
                // TranslateTransform to vertically position the character.

                oDrawingContext.PushTransform( new TranslateTransform(
                    oPointAtFractionLength.X,

                    oPointAtFractionLength.Y - (oFormattedText.Height / 2.0)
                        + dLineOffset
                    ) );

                oDrawingContext.PushTransform( new RotateTransform(
                    dCharacterAngleDegrees, 0,
                    (oFormattedText.Height / 2.0) - dLineOffset
                    ) );

                // A translucent rectangle is drawn for each character.
                // Rounding errors cause the underlying edge to show through
                // faintly between the rectangles, which is a bug.  How to fix
                // this?

                Rect oTranslucentRectangle = new Rect( oOrigin,
                    new Size(dCharacterWidth, dCharacterHeight) );

                DrawTranslucentRectangle(oDrawingContext,
                    oTranslucentRectangle, oTranslucentRectangleColor);

                oDrawingContext.DrawText(oCharacterFormattedText, oOrigin);

                oDrawingContext.Pop();
                oDrawingContext.Pop();

                dFractionOfEdgeLength += dCharacterWidth / dEdgeLength;
            }

            dLineOffset += oFormattedText.Height / asLines.Length;
        }
    }

    //*************************************************************************
    //  Method: DrawTranslucentRectangle()
    //
    /// <summary>
    /// Draws the translucent rectangle behind an edge's label.
    /// </summary>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="oRectangle">
    /// The rectangle to draw.
    /// </param>
    ///
    /// <param name="oColor">
    /// The fill color to use.
    /// </param>
    //*************************************************************************

    protected void
    DrawTranslucentRectangle
    (
        DrawingContext oDrawingContext,
        Rect oRectangle,
        Color oColor
    )
    {
        Debug.Assert(oDrawingContext != null);
        AssertValid();

        oDrawingContext.DrawRectangle(GetBrush(oColor), null, oRectangle);
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

        Debug.Assert(m_dWidth >= MinimumWidth);
        Debug.Assert(m_dWidth <= MaximumWidth);

        // m_eCurveStyle
        Debug.Assert(m_dBezierDisplacementFactor >= 0);
        // m_bDrawArrowOnDirectedEdge

        Debug.Assert(m_dRelativeArrowSize >= MinimumRelativeArrowSize);
        Debug.Assert(m_dRelativeArrowSize <= MaximumRelativeArrowSize);

        // m_oLabelTextColor
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value of the <see cref="Width" /> property.  The value is 1.
    /// </summary>

    public static Double MinimumWidth = 1;

    /// <summary>
    /// Maximum value of the <see cref="Width" /> property.  The value is 20.
    /// </summary>

    public static Double MaximumWidth = 20;

    /// <summary>
    /// Minimum value of the <see cref="RelativeArrowSize" /> property.  The
    /// value is 0.
    /// </summary>

    public static Double MinimumRelativeArrowSize = 0;

    /// <summary>
    /// Maximum value of the <see cref="RelativeArrowSize" /> property.  The
    /// value is 20.
    /// </summary>

    public static Double MaximumRelativeArrowSize = 20;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Radius of a self-loop edge, which is drawn as a circle.

    protected const Double SelfLoopCircleRadius = 10;

    /// This is roughly the length of each line segment in the polyline
    /// approximation to a continuous curve used by
    /// DrawCurveThroughIntermediatePoints(), in WPF units.  The smaller the
    /// number the smoother the curve, but the slower the performance.

    protected const Int32 CurveThroughIntermediatePointsTolerance = 8;

    /// Alpha value for the background of edge labels.

    protected const Byte LabelBackgroundAlpha = (Byte)( (80F / 100F) * 255F );


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Default edge width.

    protected Double m_dWidth;

    /// The curve style of the edges.

    protected EdgeCurveStyle m_eCurveStyle;

    /// Determines the edge curvature when m_eCurveStyle is Bezier.

    protected Double m_dBezierDisplacementFactor;

    /// true to draw an arrow on directed edges.

    protected Boolean m_bDrawArrowOnDirectedEdge;

    /// Width and height of arrows, relative to the pen width.

    protected Double m_dRelativeArrowSize;

    /// Default text color to use for labels.

    protected Color m_oLabelTextColor;
}


//*****************************************************************************
//  Enum: EdgeStyle
//
/// <summary>
/// Specifies the style of an edge.
/// </summary>
//*****************************************************************************

public enum
EdgeStyle
{
    /// <summary>
    /// The edge is drawn as a solid line.
    /// </summary>

    Solid,

    /// <summary>
    /// The edge is drawn as a dashed line.
    /// </summary>

    Dash,

    /// <summary>
    /// The edge is drawn as a dotted line.
    /// </summary>

    Dot,

    /// <summary>
    /// The edge is drawn as a dash-dot line.
    /// </summary>

    DashDot,

    /// <summary>
    /// The edge is drawn as a dash-dot-dot line.
    /// </summary>

    DashDotDot,

    // If a new style is added, the following must be done:
    //
    // 1. Update the drawing code in this class.
    //
    // 2. Add new entries to the Valid Edge Styles column on the Misc table
    //    of the NodeXLGraph.xltx Excel template.
}


//*****************************************************************************
//  Enum: EdgeCurveStyle
//
/// <summary>
/// Specifies how edges are curved.
/// </summary>
//*****************************************************************************

public enum
EdgeCurveStyle
{
    /// <summary>
    /// Each edge is drawn as a straight line.
    /// </summary>

    Straight,

    /// <summary>
    /// Each edge is drawn as a quadratic Bezier curve.
    /// </summary>

    Bezier,

    /// <summary>
    /// Each edge is drawn as a smooth curve through a set of intermediate
    /// points stored in the edge's metadata.  See EdgeDrawer.<see
    /// cref="EdgeDrawer.CurveStyle" /> for more information.
    /// </summary>

    CurveThroughIntermediatePoints,
}

}
