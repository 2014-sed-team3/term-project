
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Media.Effects;
using Smrf.NodeXL.Core;
using Smrf.AppLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexDrawer
//
/// <summary>
/// Draws a graph's vertices.
/// </summary>
///
/// <remarks>
/// This class is responsible for drawing a graph's vertices.  The default
/// vertex appearance is determined by this class's properties.  The
/// appearance of an individual vertex can be overridden by adding appropriate
/// metadata to the vertex.  The following metadata keys are honored:
///
/// <list type="bullet">
///
/// <item><see cref="ReservedMetadataKeys.Visibility" /></item>
/// <item><see cref="ReservedMetadataKeys.IsSelected" /></item>
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexShape" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexRadius" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabel" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabelFillColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabelPosition" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexImage" /></item>
///
/// </list>
///
/// <para>
/// If <see cref="VertexAndEdgeDrawerBase.UseSelection" /> is true, a vertex is
/// drawn using <see cref="VertexAndEdgeDrawerBase.Color" /> or <see
/// cref="VertexAndEdgeDrawerBase.SelectedColor" />, depending on whether the
/// vertex has been marked as selected.  If <see
/// cref="VertexAndEdgeDrawerBase.UseSelection" /> is false, <see
/// cref="VertexAndEdgeDrawerBase.Color" /> is used regardless of whether the
/// vertex has been marked as selected.
/// </para>
///
/// <para>
/// If a vertex has the shape <see cref="VertexShape.Label" /> and has the
/// <see cref="ReservedMetadataKeys.PerVertexLabel" /> key, the vertex is drawn
/// as a rectangle containing the text specified by the <see
/// cref="ReservedMetadataKeys.PerVertexLabel" /> key's value.  The default
/// color of the text and the rectangle's outline is <see
/// cref="VertexAndEdgeDrawerBase.Color" />, but can be overridden with the
/// <see cref="ReservedMetadataKeys.PerColor" /> key.  The default fill color
/// of the rectangle is <see cref="LabelFillColor" />, but can be overridden
/// with the <see cref="ReservedMetadataKeys.PerVertexLabelFillColor" /> key.
/// </para>
///
/// <para>
/// If a vertex has a shape other than <see cref="VertexShape.Label" /> and
/// has the <see cref="ReservedMetadataKeys.PerVertexLabel" /> key, the vertex
/// is drawn as the specified shape, and the text specified by the <see
/// cref="ReservedMetadataKeys.PerVertexLabel" /> key's value is drawn next to
/// the vertex as an annotation at the position determined by <see
/// cref="VertexDrawer.LabelPosition" />.
/// </para>
///
/// <para>
/// If a vertex has the shape <see cref="VertexShape.Image" /> and has the <see
/// cref="ReservedMetadataKeys.PerVertexImage" /> key, the vertex is drawn as
/// the image specified by the <see
/// cref="ReservedMetadataKeys.PerVertexImage" /> key's value.
/// </para>
///
/// <para>
/// The values of the <see cref="ReservedMetadataKeys.PerColor" /> and <see
/// cref="ReservedMetadataKeys.PerVertexLabelFillColor" /> keys can be of type
/// System.Windows.Media.Color or System.Drawing.Color.
/// </para>
///
/// <para>
/// When drawing the graph, call <see cref="TryDrawVertex" /> for each of the
/// graph's vertices.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class VertexDrawer : VertexAndEdgeDrawerBase
{
    //*************************************************************************
    //  Constructor: VertexDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexDrawer" /> class.
    /// </summary>
    //*************************************************************************

    public VertexDrawer()
    {
        m_eShape = VertexShape.Disk;
        m_dRadius = 3.0;
        m_eEffect = VertexEffect.None;
        m_dRelativeOuterGlowSize = 3.0;
        m_oLabelFillColor = SystemColors.WindowColor;
        m_eLabelPosition = VertexLabelPosition.TopRight;
        m_bLabelWrapText = true;
        m_dLabelWrapMaxTextWidth = 300;
        m_bLimitVerticesToBounds = true;
        m_btBackgroundAlpha = 220;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Shape
    //
    /// <summary>
    /// Gets or sets the default shape of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The default shape of the vertices, as a <see cref="VertexShape" />.
    /// The default value is <see cref="VertexShape.Disk" />.
    /// </value>
    ///
    /// <remarks>
    /// The default shape of a vertex can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexShape" /> key on the vertex.
    /// </remarks>
    //*************************************************************************

    public VertexShape
    Shape
    {
        get
        {
            AssertValid();

            return (m_eShape);
        }

        set
        {
            const String PropertyName = "Shape";

            this.ArgumentChecker.CheckPropertyIsDefined(
                PropertyName, value, typeof(VertexShape) );

            m_eShape = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Radius
    //
    /// <summary>
    /// Gets or sets the default radius of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The default radius of the vertices, as a <see cref="Double" />.  Must
    /// be between <see cref="MinimumRadius" /> and <see
    /// cref="MaximumRadius" />, inclusive.  The default value is 3.0.
    /// </value>
    ///
    /// <remarks>
    /// The default radius of a vertex can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexRadius" /> key on the vertex.
    /// </remarks>
    //*************************************************************************

    public Double
    Radius
    {
        get
        {
            AssertValid();

            return (m_dRadius);
        }

        set
        {
            const String PropertyName = "Radius";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumRadius, MaximumRadius);

            m_dRadius = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Effect
    //
    /// <summary>
    /// Gets or sets the effect to apply to all vertices.
    /// </summary>
    ///
    /// <value>
    /// The effect to apply to all vertices, as a <see cref="VertexEffect" />.
    /// The default value is <see cref="VertexEffect.None" />.
    /// </value>
    ///
    /// <remarks>
    /// Setting this to anything other than <see cref="VertexEffect.None" />
    /// will result in slow performance.
    /// </remarks>
    ///
    /// <seealso cref="RelativeOuterGlowSize" />
    //*************************************************************************

    public VertexEffect
    Effect
    {
        get
        {
            AssertValid();

            return (m_eEffect);
        }

        set
        {
            const String PropertyName = "Effect";

            this.ArgumentChecker.CheckPropertyIsDefined(
                PropertyName, value, typeof(VertexEffect) );

            m_eEffect = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: RelativeOuterGlowSize
    //
    /// <summary>
    /// Gets or sets the relative size of the outer glow effect.
    /// </summary>
    ///
    /// <value>
    /// The relative size of the outer glow when <see cref="Effect"/> is set to
    /// <see cref="VertexEffect.OuterGlow" />.  Must be between <see
    /// cref="MinimumRelativeOuterGlowSize" /> and <see
    /// cref="MaximumRelativeOuterGlowSize" />, inclusive.  The default value
    /// is 3.0.
    /// </value>
    ///
    /// <remarks>
    /// The value is relative to <see cref="Radius" />.  If the radius is
    /// increased, the outer glow size is increased proportionally.
    /// </remarks>
    //*************************************************************************

    public Double
    RelativeOuterGlowSize
    {
        get
        {
            AssertValid();

            return (m_dRelativeOuterGlowSize);
        }

        set
        {
            const String PropertyName = "RelativeOuterGlowSize";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumRelativeOuterGlowSize, MaximumRelativeOuterGlowSize);

            m_dRelativeOuterGlowSize = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelFillColor
    //
    /// <summary>
    /// Gets or sets the default fill color to use for a vertex that has the
    /// shape Label.
    /// </summary>
    ///
    /// <value>
    /// The default fill color to use for labels.  The default is
    /// SystemColors.WindowColor.
    /// </value>
    ///
    /// <remarks>
    /// <see cref="Color" /> is used for the label text and outline.
    ///
    /// <para>
    /// The default fill color of a vertex can be overridden by setting the
    /// <see cref="ReservedMetadataKeys.PerVertexLabelFillColor" /> key on the
    /// vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Color
    LabelFillColor
    {
        get
        {
            AssertValid();

            return (m_oLabelFillColor);
        }

        set
        {
            m_oLabelFillColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelPosition
    //
    /// <summary>
    /// Gets or sets the default position of a vertex label drawn as an
    /// annotation.
    /// </summary>
    ///
    /// <value>
    /// The default position of a vertex label drawn as an annotation.  The
    /// default is <see cref="VertexLabelPosition.TopRight" />.
    /// </value>
    ///
    /// <remarks>
    /// This property is not used when drawing vertices that have the shape
    /// <see cref="VertexShape.Label" />.
    /// </remarks>
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
    //  Property: LabelWrapText
    //
    /// <summary>
    /// Gets or sets a flag indicating whether vertex label text should be
    /// wrapped.
    /// </summary>
    ///
    /// <value>
    /// true to wrap vertex label text.  The default is true.
    /// </value>
    ///
    /// <remarks>
    /// If true, the label text is wrapped at approximately <see
    /// cref="LabelWrapMaxTextWidth"/> WPF units.
    /// </remarks>
    //*************************************************************************

    public Boolean
    LabelWrapText
    {
        get
        {
            AssertValid();

            return (m_bLabelWrapText);
        }

        set
        {
            m_bLabelWrapText = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelWrapMaxTextWidth
    //
    /// <summary>
    /// Gets or sets the maximum line length for vertex label text.
    /// </summary>
    ///
    /// <value>
    /// The maximum line length for vertex label text, in WPF units.  Must be
    /// greater than zero.  The default is 300.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="LabelWrapText" /> is true.
    /// </remarks>
    //*************************************************************************

    public Double
    LabelWrapMaxTextWidth
    {
        get
        {
            AssertValid();

            return (m_dLabelWrapMaxTextWidth);
        }

        set
        {
            m_dLabelWrapMaxTextWidth = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LimitVerticesToBounds
    //
    /// <summary>
    /// Gets or sets a flag indicating whether vertices can be drawn outside
    /// the graph bounds.
    /// </summary>
    ///
    /// <value>
    /// true if vertices shouldn't be drawn outside the graph bounds.  The
    /// default is true.
    /// </value>
    ///
    /// <remarks>
    /// If true, any vertex whose location is within the graph rectangle margin
    /// or outside the graph rectangle is moved within the margin before it is
    /// drawn.  If true, the vertex is not moved and is drawn in the specified
    /// location.
    /// </remarks>
    //*************************************************************************

    public Boolean
    LimitVerticesToBounds
    {
        get
        {
            AssertValid();

            return (m_bLimitVerticesToBounds);
        }

        set
        {
            m_bLimitVerticesToBounds = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BackgroundAlpha
    //
    /// <summary>
    /// Gets or sets the alpha of the label's background rectangle when the
    /// label is drawn as an annotation.
    /// </summary>
    ///
    /// <value>
    /// The alpha of the label's background rectangle, as a Byte.  The default
    /// value is 220.
    /// </value>
    ///
    /// <remarks>
    /// When a label is drawn as an annotation, a background rectangle is drawn
    /// behind the label to make the label more legible when it is on top of
    /// other objects.  If you don't want a background rectangle to be drawn,
    /// set this to 0.
    /// </remarks>
    //*************************************************************************

    public Byte
    BackgroundAlpha
    {
        get
        {
            AssertValid();

            return (m_btBackgroundAlpha);
        }

        set
        {
            m_btBackgroundAlpha = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TryDrawVertex()
    //
    /// <summary>
    /// Draws a vertex after moving it if necessary.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="vertexDrawingHistory">
    /// Where a <see cref="VertexDrawingHistory" /> object that retains
    /// information about how the vertex was drawn gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex was drawn, false if the vertex is hidden.
    /// </returns>
    ///
    /// <remarks>
    /// This method should be called repeatedly while a graph is being drawn,
    /// once for each of the graph's vertices.  The <see
    /// cref="IVertex.Location" /> property on all of the graph's vertices must
    /// be set by ILayout.LayOutGraph before this method is called.
    ///
    /// <para>
    /// If the vertex falls outside the graph rectangle, it gets moved before
    /// being drawn.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryDrawVertex
    (
        IVertex vertex,
        GraphDrawingContext graphDrawingContext,
        out VertexDrawingHistory vertexDrawingHistory
    )
    {
        AssertValid();

        vertexDrawingHistory = null;

        CheckDrawVertexArguments(vertex, graphDrawingContext);

        if (graphDrawingContext.GraphRectangleMinusMarginIsEmpty)
        {
            return (false);
        }

        // If the vertex is hidden, do nothing.

        VisibilityKeyValue eVisibility = GetVisibility(vertex);

        if (eVisibility == VisibilityKeyValue.Hidden)
        {
            return (false);
        }

        // Check whether the vertex represents a collapsed group and perform
        // collapsed group tasks if necessary.

        CollapsedGroupDrawingManager oCollapsedGroupDrawingManager =
            new CollapsedGroupDrawingManager();

        oCollapsedGroupDrawingManager.PreDrawVertex(vertex);

        // Check for a per-vertex label.

        Object oLabelAsObject;
        String sLabel = null;

        if ( vertex.TryGetValue(ReservedMetadataKeys.PerVertexLabel,
            typeof(String), out oLabelAsObject) )
        {
            sLabel = (String)oLabelAsObject;

            if ( String.IsNullOrEmpty(sLabel) )
            {
                sLabel = null;
            }
            else
            {
                sLabel = TruncateLabel(sLabel);
            }
        }

        Boolean bDrawAsSelected = GetDrawAsSelected(vertex);
        Point oLocation = WpfGraphicsUtil.PointFToWpfPoint(vertex.Location);
        DrawingVisualPlus oDrawingVisual = new DrawingVisualPlus();
        VertexShape eShape = GetShape(vertex);

        VertexLabelDrawer oVertexLabelDrawer =
            new VertexLabelDrawer(m_eLabelPosition, m_btBackgroundAlpha);

        using ( DrawingContext oDrawingContext = oDrawingVisual.RenderOpen() )
        {
            if (eShape == VertexShape.Label)
            {
                if (sLabel != null)
                {
                    // Draw the vertex as a label.

                    vertexDrawingHistory = DrawLabelShape(vertex,
                        graphDrawingContext, oDrawingContext, oDrawingVisual,
                        eVisibility, bDrawAsSelected, sLabel,
                        oCollapsedGroupDrawingManager);
                }
                else
                {
                    // Default to something usable.

                    eShape = VertexShape.Disk;
                }
            }
            else if (eShape == VertexShape.Image)
            {
                Object oImageSourceAsObject;

                if (vertex.TryGetValue(ReservedMetadataKeys.PerVertexImage,
                    typeof(ImageSource), out oImageSourceAsObject)
                    )
                {
                    // Draw the vertex as an image.

                    vertexDrawingHistory = DrawImageShape(vertex,
                        graphDrawingContext, oDrawingContext, oDrawingVisual,
                        eVisibility, bDrawAsSelected, sLabel,
                        (ImageSource)oImageSourceAsObject, oVertexLabelDrawer,
                        oCollapsedGroupDrawingManager);
                }
                else
                {
                    // Default to something usable.

                    eShape = VertexShape.Disk;
                }
            }

            if (vertexDrawingHistory == null)
            {
                // Draw the vertex as a simple shape.

                vertexDrawingHistory = DrawSimpleShape(vertex, eShape,
                    graphDrawingContext, oDrawingContext, oDrawingVisual,
                    eVisibility, bDrawAsSelected, sLabel, oVertexLabelDrawer,
                    oCollapsedGroupDrawingManager);
            }

            // Perform collapsed group tasks if necessary.

            oCollapsedGroupDrawingManager.PostDrawVertex(eShape,
                GetColor(vertex, eVisibility, bDrawAsSelected),
                graphDrawingContext, oDrawingContext, bDrawAsSelected,
                m_dGraphScale, oVertexLabelDrawer, m_oFormattedTextManager,
                vertexDrawingHistory);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: DrawSimpleShape()
    //
    /// <summary>
    /// Draws a vertex as a simple shape.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
    /// </param>
    ///
    /// <param name="eShape">
    /// The vertex shape to use.  Can't be <see cref="VertexShape.Image" /> or
    /// <see cref="VertexShape.Label" />.
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
    /// <param name="oDrawingVisual">
    /// The <see cref="DrawingVisual" /> object from which <paramref
    /// name="oDrawingContext" /> was obtained.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the vertex as selected.
    /// </param>
    ///
    /// <param name="sAnnotation">
    /// The annotation to draw next to the shape, or null if there is no
    /// annotation.
    /// </param>
    ///
    /// <param name="oVertexLabelDrawer">
    /// Object that draws a vertex label as an annotation.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    ///
    /// <param name="oCollapsedGroupDrawingManager">
    /// Manages the drawing of the vertex that represents a collapsed group.
    /// </param>
    ///
    /// <remarks>
    /// "Simple" means "not <see cref="VertexShape.Image" /> and not <see
    /// cref="VertexShape.Label" />."
    /// </remarks>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawSimpleShape
    (
        IVertex oVertex,
        VertexShape eShape,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisualPlus oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sAnnotation,
        VertexLabelDrawer oVertexLabelDrawer,
        CollapsedGroupDrawingManager oCollapsedGroupDrawingManager
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert(oVertexLabelDrawer != null);
        Debug.Assert(oCollapsedGroupDrawingManager != null);
        AssertValid();

        Double dRadius = GetRadius(oVertex);
        Color oColor = GetColor(oVertex, eVisibility, bDrawAsSelected);
        Point oVertexLocation = GetVertexLocation(oVertex);

        oDrawingVisual.SetEffect( GetSimpleShapeEffect(
            oGraphDrawingContext, dRadius, oColor) );

        Rect oVertexBounds;

        if (eShape == VertexShape.Triangle ||
            eShape == VertexShape.SolidTriangle)
        {
            oVertexBounds =
                WpfGraphicsUtil.TriangleBoundsFromCenterAndHalfWidth(
                    oVertexLocation, dRadius);
        }
        else if (eShape == VertexShape.SolidTaperedDiamond)
        {
            // Note that the bounds of a tapered diamond can't be calculated
            // using simple equations.  Instead, create the tapered diamond and
            // let WPF compute the bounds.
            //
            // There is some inefficiency here, because the tapered diamond
            // gets created again before it is drawn, in its possibly-moved
            // location.

            oVertexBounds = WpfPathGeometryUtil.GetTaperedDiamond(
                oVertexLocation, dRadius).Bounds;
        }
        else if (eShape == VertexShape.SolidRoundedX)
        {
            // Note that the bounds of a rounded X can't be calculated
            // using simple equations.  Instead, create the rounded X and
            // let WPF compute the bounds.
            //
            // There is some inefficiency here, because the rounded X
            // gets created again before it is drawn, in its possibly-moved
            // location.

            oVertexBounds = WpfPathGeometryUtil.GetRoundedX(
                oVertexLocation, dRadius).Bounds;
        }
        else
        {
            oVertexBounds = WpfGraphicsUtil.SquareFromCenterAndHalfWidth(
                oVertexLocation, dRadius);
        }

        // Move the vertex if it falls outside the graph rectangle.

        MoveVertexIfNecessary(oVertex, oGraphDrawingContext,
            oCollapsedGroupDrawingManager, ref oVertexBounds);

        oVertexLocation = GetVertexLocation(oVertex);
        VertexDrawingHistory oVertexDrawingHistory = null;

        // Note that for the "hollow" shapes -- Circle, Square, Diamond, and
        // Triangle -- Brushes.Transparent is used instead of a null brush.
        // This allows the entire area of these shapes to be hit-tested.  Using
        // a null brush would cause hit-testing to fail if the shapes'
        // interiors were clicked.

        switch (eShape)
        {
            case VertexShape.Circle:
            case VertexShape.Disk:

                Boolean bIsDisk = (eShape == VertexShape.Disk);

                oDrawingContext.DrawEllipse(
                    bIsDisk ? GetBrush(oColor) : Brushes.Transparent,
                    bIsDisk ? null : GetPen(oColor, DefaultPenThickness),
                    oVertexLocation, dRadius, dRadius
                    );

                oVertexDrawingHistory = bIsDisk ?

                    new DiskVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius)
                    :
                    new CircleVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            case VertexShape.Sphere:

                RadialGradientBrush oRadialGradientBrush =
                    new RadialGradientBrush();

                oRadialGradientBrush.GradientOrigin =
                    oRadialGradientBrush.Center = new Point(0.3, 0.3);

                GradientStopCollection oGradientStops =
                    oRadialGradientBrush.GradientStops;

                oGradientStops.Add( new GradientStop(
                    WpfGraphicsUtil.SetWpfColorAlpha(Colors.White, oColor.A),
                    0.0) );

                oGradientStops.Add( new GradientStop(oColor, 1.0) );

                WpfGraphicsUtil.FreezeIfFreezable(oRadialGradientBrush);

                oDrawingContext.DrawEllipse(oRadialGradientBrush, null,
                    oVertexLocation, dRadius, dRadius);

                oVertexDrawingHistory = new SphereVertexDrawingHistory(
                    oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            case VertexShape.Square:

                WpfGraphicsUtil.DrawPixelAlignedRectangle(oDrawingContext,
                    Brushes.Transparent, GetPen(oColor, DefaultPenThickness),
                    oVertexBounds);

                oVertexDrawingHistory = new SquareVertexDrawingHistory(oVertex,
                    oDrawingVisual, bDrawAsSelected, oVertexBounds);

                break;

            case VertexShape.SolidSquare:

                oDrawingContext.DrawRectangle(GetBrush(oColor), null,
                    oVertexBounds);

                oVertexDrawingHistory = new SolidSquareVertexDrawingHistory(
                    oVertex, oDrawingVisual, bDrawAsSelected, oVertexBounds);

                break;

            case VertexShape.Diamond:
            case VertexShape.SolidDiamond:

                Boolean bIsSolidDiamond = (eShape == VertexShape.SolidDiamond);

                PathGeometry oDiamond =
                    WpfPathGeometryUtil.GetDiamond(oVertexLocation, dRadius);

                oDrawingContext.DrawGeometry(

                    bIsSolidDiamond ? GetBrush(oColor) : Brushes.Transparent,

                    bIsSolidDiamond ? null :
                        GetPen(oColor, DefaultPenThickness),

                    oDiamond
                    );

                oVertexDrawingHistory = bIsSolidDiamond ?

                    new SolidDiamondVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius)
                    :
                    new DiamondVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            case VertexShape.Triangle:
            case VertexShape.SolidTriangle:

                Boolean bIsSolidTriangle =
                    (eShape == VertexShape.SolidTriangle);

                PathGeometry oTriangle =
                    WpfPathGeometryUtil.GetTriangle(oVertexLocation, dRadius);

                oDrawingContext.DrawGeometry(

                    bIsSolidTriangle ? GetBrush(oColor) : Brushes.Transparent,

                    bIsSolidTriangle ? null :
                        GetPen(oColor, DefaultPenThickness),

                    oTriangle
                    );

                oVertexDrawingHistory = bIsSolidTriangle ?

                    new SolidTriangleVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius)
                    :
                    new TriangleVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            case VertexShape.SolidTaperedDiamond:

                Geometry oTaperedDiamond =
                    WpfPathGeometryUtil.GetTaperedDiamond(
                        oVertexLocation, dRadius);

                // Note that as of August 2012, the tapered diamond shape is
                // used only for collapsed connector motifs.  Collapsed motifs
                // have an outline, so draw an outline here.

                oDrawingContext.DrawGeometry(GetBrush(oColor),

                    GetPen( CollapsedGroupDrawingManager
                        .GetCollapsedMotifOutlineColor(oColor.A),
                        DefaultPenThickness),

                    oTaperedDiamond);

                oVertexDrawingHistory =
                    new SolidTaperedDiamondVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            case VertexShape.SolidRoundedX:

                Geometry oRoundedX =
                    WpfPathGeometryUtil.GetRoundedX(
                        oVertexLocation, dRadius);

                // Note that as of August 2012, the rounded X shape is
                // used only for collapsed clique motifs.  Collapsed motifs
                // have an outline, so draw an outline here.

                oDrawingContext.DrawGeometry(GetBrush(oColor),

                    GetPen(CollapsedGroupDrawingManager
                        .GetCollapsedMotifOutlineColor(oColor.A),
                        DefaultPenThickness),

                    oRoundedX);

                oVertexDrawingHistory =
                    new SolidRoundedXVertexDrawingHistory(
                        oVertex, oDrawingVisual, bDrawAsSelected, dRadius);

                break;

            default:

                Debug.Assert(false);
                break;
        }

        if (sAnnotation != null)
        {
            oVertexLabelDrawer.DrawLabel(oDrawingContext,
                oGraphDrawingContext, oVertexDrawingHistory,

                CreateFormattedTextWithWrap(sAnnotation, oColor,
                    m_oFormattedTextManager.FontSize),

                oColor);
        }

        Debug.Assert(oVertexDrawingHistory != null);

        return (oVertexDrawingHistory);
    }

    //*************************************************************************
    //  Method: DrawImageShape()
    //
    /// <summary>
    /// Draws a vertex as a specified image.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
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
    /// <param name="oDrawingVisual">
    /// The <see cref="DrawingVisual" /> object from which <paramref
    /// name="oDrawingContext" /> was obtained.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the vertex as selected.
    /// </param>
    ///
    /// <param name="sAnnotation">
    /// The annotation to draw next to the image, or null if there is no
    /// annotation.
    /// </param>
    ///
    /// <param name="oImageSource">
    /// The image to draw.
    /// </param>
    ///
    /// <param name="oVertexLabelDrawer">
    /// Object that draws a vertex label as an annotation.
    /// </param>
    ///
    /// <param name="oCollapsedGroupDrawingManager">
    /// Manages the drawing of the vertex that represents a collapsed group.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawImageShape
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisualPlus oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sAnnotation,
        ImageSource oImageSource,
        VertexLabelDrawer oVertexLabelDrawer,
        CollapsedGroupDrawingManager oCollapsedGroupDrawingManager
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert(oImageSource != null);
        Debug.Assert(oVertexLabelDrawer != null);
        Debug.Assert(oCollapsedGroupDrawingManager != null);
        AssertValid();

        // Move the vertex if it falls outside the graph rectangle.

        Rect oVertexRectangle = GetVertexRectangle(
            GetVertexLocation(oVertex), oImageSource.Width * m_dGraphScale,
            oImageSource.Height * m_dGraphScale);

        MoveVertexIfNecessary(oVertex, oGraphDrawingContext,
            oCollapsedGroupDrawingManager, ref oVertexRectangle);

        Byte btAlpha = 255;

        if (!bDrawAsSelected)
        {
            // Check for a non-opaque alpha value.

            btAlpha = GetAlpha(oVertex, eVisibility, btAlpha);
        }

        VertexDrawingHistory oVertexDrawingHistory =
            new ImageVertexDrawingHistory(oVertex, oDrawingVisual,
                bDrawAsSelected, oVertexRectangle);

        if (btAlpha > 0)
        {
            oDrawingContext.DrawImage(oImageSource, oVertexRectangle);

            Color oColor = GetColor(oVertex, eVisibility, bDrawAsSelected);

            oDrawingVisual.SetEffect( GetRectangleEffect(
                oGraphDrawingContext, oColor) );

            // Draw an outline rectangle.

            WpfGraphicsUtil.DrawPixelAlignedRectangle(oDrawingContext, null,
                GetPen(oColor, DefaultPenThickness), oVertexRectangle);

            if (btAlpha < 255)
            {
                // Real transparency can't be achieved with arbitrary images,
                // so simulate transparency by drawing on top of the image with
                // a translucent brush the same color as the graph's
                // background.
                //
                // This really isn't a good solution.  Is there are better way
                // to simulate transparency?

                Color oTranslucentColor = oGraphDrawingContext.BackColor;
                oTranslucentColor.A = (Byte)( (Byte)255 - btAlpha );

                oDrawingContext.DrawRectangle(
                    CreateFrozenSolidColorBrush(oTranslucentColor), null,
                        oVertexRectangle);
            }

            if (sAnnotation != null)
            {
                oVertexLabelDrawer.DrawLabel(oDrawingContext,
                    oGraphDrawingContext, oVertexDrawingHistory,

                    CreateFormattedTextWithWrap(sAnnotation, oColor,
                        m_oFormattedTextManager.FontSize),

                    oColor
                    );
            }
        }

        return (oVertexDrawingHistory);
    }

    //*************************************************************************
    //  Method: DrawLabelShape()
    //
    /// <summary>
    /// Draws a vertex as a label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw.
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
    /// <param name="oDrawingVisual">
    /// The <see cref="DrawingVisual" /> object from which <paramref
    /// name="oDrawingContext" /> was obtained.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the vertex as selected.
    /// </param>
    ///
    /// <param name="sLabel">
    /// The label to draw.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="oCollapsedGroupDrawingManager">
    /// Manages the drawing of the vertex that represents a collapsed group.
    /// </param>
    ///
    /// <returns>
    /// A VertexDrawingHistory object that retains information about how the
    /// vertex was drawn.
    /// </returns>
    //*************************************************************************

    protected VertexDrawingHistory
    DrawLabelShape
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        DrawingVisualPlus oDrawingVisual,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected,
        String sLabel,
        CollapsedGroupDrawingManager oCollapsedGroupDrawingManager
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert( !String.IsNullOrEmpty(sLabel) );
        Debug.Assert(oCollapsedGroupDrawingManager != null);
        AssertValid();

        // Figure out what colors to use.

        Color oOutlineColor;

        Color oTextColor = GetColor(oVertex, eVisibility, false);

        Color oFillColor = GetColor(oVertex, eVisibility,
            ReservedMetadataKeys.PerVertexLabelFillColor, m_oLabelFillColor,
            true);

        if (bDrawAsSelected)
        {
            // The outline color is always the selected color.

            oOutlineColor = m_oSelectedColor;

            // The text color is the default or per-vertex color with no alpha.

            oTextColor.A = 255;

            // The fill color is the default or per-vertex fill color with no
            // alpha.

            oFillColor.A = 255;
        }
        else
        {
            // The outline color is the default or per-vertex color with alpha.

            oOutlineColor = oTextColor;

            // The text color is the default or per-vertex color with alpha.

            // The fill color is the default or per-vertex fill color with
            // alpha.
        }

        Double dLabelFontSize = GetLabelFontSize(oVertex);

        FormattedText oFormattedText = CreateFormattedTextWithWrap(
            sLabel, oTextColor, dLabelFontSize);

        Rect oVertexRectangle = GetVertexRectangle(
            GetVertexLocation(oVertex), oFormattedText.Width,
            oFormattedText.Height);

        // Pad the text.

        Rect oVertexRectangleWithPadding = oVertexRectangle;
        Double dLabelPadding = GetLabelPadding(dLabelFontSize);

        oVertexRectangleWithPadding.Inflate(dLabelPadding,
            dLabelPadding * 0.7);

        if (m_oFormattedTextManager.Typeface.Style != FontStyles.Normal)
        {
            // This is a hack to move the right edge of the padded rectangle
            // to the right to adjust for wider italic text, which
            // FormattedText.Width does not account for.  What is the correct
            // way to do this?  It might involve the FormattedText.Overhang*
            // properties, but I'll be darned if I can understand how those
            // properties work.

            Double dItalicCompensation = dLabelFontSize / 7.0;
            oVertexRectangleWithPadding.Inflate(dItalicCompensation, 0);
            oVertexRectangleWithPadding.Offset(dItalicCompensation, 0);
        }

        // Move the vertex if it falls outside the graph rectangle.

        Double dOriginalVertexRectangleWithPaddingX =
            oVertexRectangleWithPadding.X;

        Double dOriginalVertexRectangleWithPaddingY =
            oVertexRectangleWithPadding.Y;

        MoveVertexIfNecessary(oVertex, oGraphDrawingContext,
            oCollapsedGroupDrawingManager, ref oVertexRectangleWithPadding);

        oVertexRectangle.Offset(

            oVertexRectangleWithPadding.X -
                dOriginalVertexRectangleWithPaddingX,

            oVertexRectangleWithPadding.Y -
                dOriginalVertexRectangleWithPaddingY
            );

        oDrawingVisual.SetEffect( GetRectangleEffect(
            oGraphDrawingContext, oOutlineColor) );

        // Draw the padded rectangle, then the text.

        WpfGraphicsUtil.DrawPixelAlignedRectangle(oDrawingContext,
            GetBrush(oFillColor), GetPen(oOutlineColor, DefaultPenThickness),
            oVertexRectangleWithPadding);

        oDrawingContext.DrawText(oFormattedText, oVertexRectangle.Location);

        // Return information about how the vertex was drawn.

        return ( new LabelVertexDrawingHistory(oVertex, oDrawingVisual,
            bDrawAsSelected, oVertexRectangleWithPadding) );
    }

    //*************************************************************************
    //  Method: MoveVertexIfNecessary()
    //
    /// <summary>
    /// Moves a vertex if necessary.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to move if necessary.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oCollapsedGroupDrawingManager">
    /// Manages the drawing of the vertex that represents a collapsed group.
    /// </param>
    ///
    /// <param name="oVertexBounds">
    /// The rectangle defining the bounds of the vertex.  This gets moved if
    /// necessary.  It does not get resized.
    /// </param>
    ///
    /// <remarks>
    /// If the vertex falls within the margin or outside the graph rectangle,
    /// the IVertex.Location property and <paramref name="oVertexBounds" /> get
    /// updated.
    /// </remarks>
    //*************************************************************************

    protected void
    MoveVertexIfNecessary
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext,
        CollapsedGroupDrawingManager oCollapsedGroupDrawingManager,
        ref Rect oVertexBounds
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oCollapsedGroupDrawingManager != null);
        AssertValid();

        if (!m_bLimitVerticesToBounds ||
            oGraphDrawingContext.GraphRectangleMinusMarginIsEmpty)
        {
            // The vertex shouldn't be moved.

            return;
        }

        // First, assume that this is a normal vertex and not a vertex that
        // represents a collapsed group.  Move the vertex bounds within the
        // bounds of the graph rectangle's margin.

        Rect oMovedVertexBounds = WpfGraphicsUtil.MoveRectangleWithinBounds(
            oVertexBounds, oGraphDrawingContext.GraphRectangleMinusMargin,
            false);

        // If the vertex actually represents a collapsed group, move it further
        // if necessary to accomodate the additional elements that
        // CollapsedGroupmanager.PostDrawVertex() may draw on top of the
        // vertex.

        oCollapsedGroupDrawingManager.MoveVertexBoundsIfNecessary(
            oGraphDrawingContext, m_dGraphScale, ref oMovedVertexBounds);

        oVertex.Location = System.Drawing.PointF.Add( oVertex.Location,
            new System.Drawing.SizeF(
                (Single)(oMovedVertexBounds.X - oVertexBounds.X),
                (Single)(oMovedVertexBounds.Y - oVertexBounds.Y)
                ) );

        oVertexBounds = oMovedVertexBounds;
    }

    //*************************************************************************
    //  Method: CreateFormattedTextWithWrap()
    //
    /// <summary>
    /// Creates a FormattedText object with wrapping, if appropriate.
    /// </summary>
    ///
    /// <param name="sText">
    /// The text to draw.  Can't be null.
    /// </param>
    ///
    /// <param name="oColor">
    /// The text color.
    /// </param>
    ///
    /// <param name="dFontSize">
    /// The font size to use, in WPF units.
    /// </param>
    //*************************************************************************

    protected FormattedText
    CreateFormattedTextWithWrap
    (
        String sText,
        Color oColor,
        Double dFontSize
    )
    {
        Debug.Assert(sText != null);
        Debug.Assert(dFontSize >= 0);

        FormattedText oFormattedText =
            m_oFormattedTextManager.CreateFormattedText(sText, oColor,
                dFontSize, m_dGraphScale);

        if (this.m_bLabelWrapText)
        {
            oFormattedText.MaxTextWidth =
                m_dLabelWrapMaxTextWidth * m_dGraphScale;

            oFormattedText.MaxTextHeight = MaximumLabelHeight * m_dGraphScale;
        }
        else
        {
            oFormattedText.MaxLineCount = 1;

            // (Leave the MaxTextWidth property set to the default value of 0.)
        }

        return (oFormattedText);
    }

    //*************************************************************************
    //  Method: GetShape()
    //
    /// <summary>
    /// Gets the shape of a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the shape for.
    /// </param>
    ///
    /// <returns>
    /// The shape of the vertex.
    /// </returns>
    //*************************************************************************

    protected VertexShape
    GetShape
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default shape.

        VertexShape eShape = m_eShape;

        Object oPerVertexShapeAsObject;

        // Check for a per-vertex shape.

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexShape,
            typeof(VertexShape), out oPerVertexShapeAsObject) )
        {
            eShape = (VertexShape)oPerVertexShapeAsObject;
        }

        return (eShape);
    }

    //*************************************************************************
    //  Method: GetRadius()
    //
    /// <summary>
    /// Gets the radius of a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the radius for.
    /// </param>
    ///
    /// <returns>
    /// The radius of the vertex.
    /// </returns>
    //*************************************************************************

    protected Double
    GetRadius
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default radius.

        Double dRadius = m_dRadius;

        Object oPerVertexRadiusAsObject;

        // Check for a per-vertex radius.  Note that the radius is stored as a
        // Single in the vertex's metadata to reduce memory usage.

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexRadius,
            typeof(Single), out oPerVertexRadiusAsObject) )
        {
            dRadius = (Double)(Single)oPerVertexRadiusAsObject;

            if (dRadius < MinimumRadius || dRadius > MaximumRadius)
            {
                throw new FormatException( String.Format(

                    "{0}: The vertex with the ID {1} has an out-of-range"
                    + " ReservedMetadataKeys.PerVertexRadius value.  Valid"
                    + " values are between {2} and {3}."
                    ,
                    this.ClassName,
                    oVertex.ID,
                    MinimumRadius,
                    MaximumRadius
                    ) );
            }
        }

        return (dRadius * m_dGraphScale);
    }

    //*************************************************************************
    //  Method: GetLabelFontSize()
    //
    /// <summary>
    /// Gets the font size to use for a vertex with the shape Label.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the label font size for.
    /// </param>
    ///
    /// <returns>
    /// The label font size to use, in WPF units.
    /// </returns>
    //*************************************************************************

    protected Double
    GetLabelFontSize
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        // Start with the default font size.

        Double dLabelFontSize = m_oFormattedTextManager.FontSize;

        Object oPerVertexLabelFontSizeAsObject;

        // Check for a per-vertex font size.  Note that the font size is stored
        // as a Single in the vertex's metadata to reduce memory usage.

        if ( oVertex.TryGetValue(ReservedMetadataKeys.PerVertexLabelFontSize,
            typeof(Single), out oPerVertexLabelFontSizeAsObject) )
        {
            dLabelFontSize = (Double)(Single)oPerVertexLabelFontSizeAsObject;

            if (dLabelFontSize <= 0)
            {
                throw new FormatException( String.Format(

                    "{0}: The vertex with the ID {1} has a non-positive"
                    + " ReservedMetadataKeys.PerVertexLabelFontSize value."
                    ,
                    this.ClassName,
                    oVertex.ID
                    ) );
            }
        }

        return (dLabelFontSize * m_dGraphScale);
    }

    //*************************************************************************
    //  Method: GetLabelPadding()
    //
    /// <summary>
    /// Gets the text padding to use for a vertex with the shape Label.
    /// </summary>
    ///
    /// <param name="dLabelFontSize">
    /// The label font size being used, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// The text padding to use, in WPF units.
    /// </returns>
    //*************************************************************************

    protected Double
    GetLabelPadding
    (
        Double dLabelFontSize
    )
    {
        Debug.Assert(dLabelFontSize >= 0);
        AssertValid();

        // Make the padding larger for smaller fonts than for larger fonts.
        // These linear-interpolation points were selected to satisfy some
        // padding specifications in a NodeXL design document.

        const Double FontSizeA = 14.0;
        const Double FontSizeB = 39.4;
        const Double LabelPaddingA = 3.0;
        const Double LabelPaddingB = 5.0;

        Double dLabelPadding =
            LabelPaddingA + (dLabelFontSize - FontSizeA) *
            (LabelPaddingB - LabelPaddingA) / (FontSizeB - FontSizeA);

        dLabelPadding = Math.Max(0, dLabelPadding);

        return (dLabelPadding * m_dGraphScale);
    }

    //*************************************************************************
    //  Method: GetVertexRectangle()
    //
    /// <summary>
    /// Gets a rectangle to use to draw a vertex.
    /// </summary>
    ///
    /// <param name="oLocation">
    /// The vertex's location.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The width of the vertex's rectangle.
    /// </param>
    ///
    /// <param name="dHeight">
    /// The height of the vertex's rectangle.
    /// </param>
    ///
    /// <returns>
    /// A rectangle centered on <paramref name="oLocation" />.
    /// </returns>
    //*************************************************************************

    protected Rect
    GetVertexRectangle
    (
        Point oLocation,
        Double dWidth,
        Double dHeight
    )
    {
        Debug.Assert(dWidth >= 0);
        Debug.Assert(dHeight >= 0);
        AssertValid();

        return new Rect(

            new Point(oLocation.X - dWidth / 2.0,
                oLocation.Y - dHeight / 2.0),
                        
            new Size(dWidth, dHeight)
            );
    }

    //*************************************************************************
    //  Method: GetVertexLocation()
    //
    /// <summary>
    /// Gets the location of a vertex, as a System.Windows.Point.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to get the location of.
    /// </param>
    ///
    /// <returns>
    /// The IVertex.Location property, converted to a System.Windows.Point.
    /// </returns>
    //*************************************************************************

    protected Point
    GetVertexLocation
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        return ( WpfGraphicsUtil.PointFToWpfPoint(oVertex.Location) );
    }

    //*************************************************************************
    //  Method: GetSimpleShapeEffect()
    //
    /// <summary>
    /// Gets the Effect object to use when drawing a simple shape.
    /// </summary>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="dRadius">
    /// The vertex radius.
    /// </param>
    ///
    /// <param name="oColor">
    /// The vertex color.
    /// </param>
    ///
    /// <returns>
    /// The Effect object to use, or null to use no Effect.
    /// </returns>
    //*************************************************************************

    protected Effect
    GetSimpleShapeEffect
    (
        GraphDrawingContext oGraphDrawingContext,
        Double dRadius,
        Color oColor
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(dRadius >= 0);
        AssertValid();

        switch (m_eEffect)
        {
            case VertexEffect.None:

                return (null);

            case VertexEffect.OuterGlow:

                // The following mapping was determined experimentally.

                Double dGlowSize = MathUtil.TransformValueToRange(
                    (Single)(dRadius * m_dRelativeOuterGlowSize),
                    3F, 300F,
                    4F, 180F
                    );

                return ( GetEffectForOuterGlow(oColor, dGlowSize, 0.9) );

            case VertexEffect.DropShadow:

                // The following mapping was determined experimentally.

                Double dShadowDepth = MathUtil.TransformValueToRange(
                    (Single)(dRadius),
                    3F, 50F,
                    4F, 20F
                    );

                return ( GetEffectForDropShadow(oGraphDrawingContext,
                    dShadowDepth, 0.6) );

            default:

                Debug.Assert(false);
                return (null);
        }
    }

    //*************************************************************************
    //  Method: GetRectangleEffect()
    //
    /// <summary>
    /// Gets the Effect object to use when drawing an image or label shape.
    /// </summary>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="oColor">
    /// The vertex color.
    /// </param>
    ///
    /// <returns>
    /// The Effect object to use, or null to use no Effect.
    /// </returns>
    //*************************************************************************

    protected Effect
    GetRectangleEffect
    (
        GraphDrawingContext oGraphDrawingContext,
        Color oColor
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        switch (m_eEffect)
        {
            case VertexEffect.None:

                return (null);

            case VertexEffect.OuterGlow:

                // The following mapping was determined experimentally.

                Double dGlowSize = MathUtil.TransformValueToRange(
                    (Single)m_dRelativeOuterGlowSize,
                    (Single)MinimumRelativeOuterGlowSize,
                    (Single)MaximumRelativeOuterGlowSize,
                    4.0F, 50F
                    );

                return ( GetEffectForOuterGlow(oColor, dGlowSize, 0.9) );

            case VertexEffect.DropShadow:

                return ( GetEffectForDropShadow(oGraphDrawingContext,
                    7.0, 0.4) );

            default:

                Debug.Assert(false);
                return (null);
        }
    }

    //*************************************************************************
    //  Method: GetEffectForOuterGlow()
    //
    /// <summary>
    /// Gets a new Effect object that to use for an outer glow.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The glow color to use.
    /// </param>
    ///
    /// <param name="dGlowSize">
    /// The glow size to use.
    /// </param>
    ///
    /// <param name="dOpacity">
    /// The opacity to use.  Must be between 0 and 1.0.
    /// </param>
    ///
    /// <returns>
    /// A new Effect object.
    /// </returns>
    //*************************************************************************

    protected Effect
    GetEffectForOuterGlow
    (
        Color oColor,
        Double dGlowSize,
        Double dOpacity
    )
    {
        Debug.Assert(dGlowSize >= 0);
        Debug.Assert(dOpacity >= 0.0);
        Debug.Assert(dOpacity <= 1.0);
        AssertValid();

        // WPF has no outer glow effect (except for the deprecated
        // OuterGlowBitmapEffect), but one can be simulated by using a drop
        // shadow with zero shadow depth.

        return ( GetDropShadowEffect(oColor, 0, dGlowSize, dOpacity) );
    }

    //*************************************************************************
    //  Method: GetEffectForDropShadow()
    //
    /// <summary>
    /// Gets a new Effect object to use for a drop shadow.
    /// </summary>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="dShadowDepth">
    /// The shadow depth to use.
    /// </param>
    ///
    /// <param name="dOpacity">
    /// The opacity to use.  Must be between 0 and 1.0.
    /// </param>
    ///
    /// <returns>
    /// A new Effect object.
    /// </returns>
    //*************************************************************************

    protected Effect
    GetEffectForDropShadow
    (
        GraphDrawingContext oGraphDrawingContext,
        Double dShadowDepth,
        Double dOpacity
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(dShadowDepth >= 0.0);
        Debug.Assert(dOpacity >= 0.0);
        Debug.Assert(dOpacity <= 1.0);
        AssertValid();

        Color oColor = WpfGraphicsUtil.GetContrastingColor(
            oGraphDrawingContext.BackColor);

        return ( GetDropShadowEffect(oColor, dShadowDepth, 5.0, dOpacity) );
    }

    //*************************************************************************
    //  Method: GetDropShadowEffect()
    //
    /// <summary>
    /// Gets a new DropShadowEffect object.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The color to use.
    /// </param>
    ///
    /// <param name="dShadowDepth">
    /// The shadow depth to use.
    /// </param>
    ///
    /// <param name="dBlurRadius">
    /// The blur radius to use.
    /// </param>
    ///
    /// <param name="dOpacity">
    /// The opacity to use.  Must be between 0 and 1.0.
    /// </param>
    ///
    /// <returns>
    /// A new DropShadowEffect object.
    /// </returns>
    //*************************************************************************

    protected DropShadowEffect
    GetDropShadowEffect
    (
        Color oColor,
        Double dShadowDepth,
        Double dBlurRadius,
        Double dOpacity
    )
    {
        Debug.Assert(dShadowDepth >= 0.0);
        Debug.Assert(dBlurRadius >= 0.0);
        Debug.Assert(dOpacity >= 0.0);
        Debug.Assert(dOpacity <= 1.0);
        AssertValid();

        DropShadowEffect oDropShadowEffect = new DropShadowEffect();

        oDropShadowEffect.Color = oColor;
        oDropShadowEffect.ShadowDepth = dShadowDepth;
        oDropShadowEffect.BlurRadius = dBlurRadius;
        oDropShadowEffect.Opacity = dOpacity;
        WpfGraphicsUtil.FreezeIfFreezable(oDropShadowEffect);

        return (oDropShadowEffect);
    }

    //*************************************************************************
    //  Method: CheckDrawVertexArguments()
    //
    /// <summary>
    /// Checks the arguments to <see cref="TryDrawVertex" />.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex that will eventually be drawn.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// An exception is thrown if one of the arguments is invalid.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckDrawVertexArguments
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext
    )
    {
        AssertValid();

        const String MethodName = "TryDrawVertex";
        const String VertexArgumentName = "vertex";

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNull(MethodName, VertexArgumentName,
            oVertex);

        oArgumentChecker.CheckArgumentNotNull(MethodName,
            "graphDrawingContext", oGraphDrawingContext);

        if (oVertex.ParentGraph == null)
        {
            oArgumentChecker.ThrowArgumentException(
                MethodName, VertexArgumentName,
                "The vertex doesn't belong to a graph.  It can't be drawn."
                );
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

        // m_eShape
        Debug.Assert(m_dRadius >= MinimumRadius);
        Debug.Assert(m_dRadius <= MaximumRadius);
        // m_eEffect
        Debug.Assert(m_dRelativeOuterGlowSize >= MinimumRelativeOuterGlowSize);
        Debug.Assert(m_dRelativeOuterGlowSize <= MaximumRelativeOuterGlowSize);
        // m_oLabelFillColor
        // m_eLabelPosition
        // m_bLabelWrapText
        Debug.Assert(m_dLabelWrapMaxTextWidth > 0);
        // m_bLimitVerticesToBounds
        // m_btBackgroundAlpha
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value of the <see cref="Radius" /> property.  The value is 0.1.
    /// </summary>

    public static Double MinimumRadius = 0.1;

    /// <summary>
    /// Maximum value of the <see cref="Radius" /> property.  The value is
    /// 549.0.
    /// </summary>
    //
    // Note on where the value of 549.0 came from:
    //
    // Until NodeXL version 1.0.1.105, vertex sizes in the Excel template
    // ranged from 1 to 10, which corresponded to MinimumRadius=0.1 and
    // MaximumRadius=50.0.  In version 1.0.1.105, larger vertices were
    // requested.  The vertex size range in the Excel template was changed to
    // 1 to 100, and MaximumRadius was changed from 50.0 to 549.0.  With these
    // new numbers, a vertex size of 10 in the Excel template still corresponds
    // to 50.0 in this class, thus preserving compatibility with older
    // workbooks.

    public static Double MaximumRadius = 549.0;

    /// <summary>
    /// Minimum value of the <see cref="RelativeOuterGlowSize" /> property.
    /// The value is 1.0.
    /// </summary>

    public static Double MinimumRelativeOuterGlowSize = 1.0;

    /// <summary>
    /// Maximum value of the <see cref="RelativeOuterGlowSize" /> property.
    /// The value is 10.0.
    /// </summary>

    public static Double MaximumRelativeOuterGlowSize = 10.0;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum height of a label shape, not including the label padding, in
    /// device-independent units.

    protected const Double MaximumLabelHeight = 550;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Default shape of the vertices.

    protected VertexShape m_eShape;

    /// Default radius of the vertices.

    protected Double m_dRadius;

    /// Bitmap effect to apply to all vertices.

    protected VertexEffect m_eEffect;

    /// Factor that determines the size of the outer glow effect.

    protected Double m_dRelativeOuterGlowSize;

    /// Default fill color to use for labels.

    protected Color m_oLabelFillColor;

    /// Default position of vertex labels drawn as annotations.

    protected VertexLabelPosition m_eLabelPosition;

    /// true to wrap vertex label text.

    protected Boolean m_bLabelWrapText;

    /// The maximum line length for vertex label text, in WPF units.

    protected Double m_dLabelWrapMaxTextWidth;

    /// true if vertices shouldn't be drawn outside the graph bounds.

    protected Boolean m_bLimitVerticesToBounds;

    /// Alpha of the label's background rectangle for labels drawn as
    /// annotations.

    protected Byte m_btBackgroundAlpha;
}


//*****************************************************************************
//  Enum: VertexShape
//
/// <summary>
/// Specifies the shape of a vertex.
/// </summary>
//*****************************************************************************

public enum
VertexShape
{
    /// <summary>
    /// The vertex is drawn as a circle.
    /// </summary>

    Circle = 0,

    /// <summary>
    /// The vertex is drawn as a disk.
    /// </summary>

    Disk = 1,

    /// <summary>
    /// The vertex is drawn as a sphere.
    /// </summary>

    Sphere = 2,

    /// <summary>
    /// The vertex is drawn as a square.
    /// </summary>

    Square = 3,

    /// <summary>
    /// The vertex is drawn as a solid square.
    /// </summary>

    SolidSquare = 4,

    /// <summary>
    /// The vertex is drawn as a diamond.
    /// </summary>

    Diamond = 5,

    /// <summary>
    /// The vertex is drawn as a solid diamond.
    /// </summary>

    SolidDiamond = 6,

    /// <summary>
    /// The vertex is drawn as an equilateral triangle.
    /// </summary>

    Triangle = 7,

    /// <summary>
    /// The vertex is drawn as a solid equilateral triangle.
    /// </summary>

    SolidTriangle = 8,

    /// <summary>
    /// The vertex is drawn as an image.
    ///
    /// <para>
    /// The image is obtained from the <see
    /// cref="ReservedMetadataKeys.PerVertexImage" /> metadata key.  If the
    /// <see cref="ReservedMetadataKeys.PerVertexImage" /> key is missing, the
    /// vertex is drawn as a <see cref="Disk" />.
    /// </para>
    ///
    /// </summary>

    Image = 9,

    /// <summary>
    /// The vertex is drawn as a label.
    ///
    /// <para>
    /// The label text is obtained from the <see
    /// cref="ReservedMetadataKeys.PerVertexLabel" /> metadata key.  If the
    /// <see cref="ReservedMetadataKeys.PerVertexLabel" /> key is missing, the
    /// vertex is drawn as a <see cref="Disk" />.
    /// </para>
    ///
    /// </summary>

    Label = 10,

    /// <summary>
    /// The vertex is drawn as a solid diamond with edges that are tapered
    /// inward.
    /// </summary>

    SolidTaperedDiamond = 11,

    /// <summary>
    /// The vertex is drawn as a solid rounded X with edges that are tapered
    /// outward.
    /// </summary>

    SolidRoundedX = 12,


    // If a new shape is added, the following must be done:
    //
    // 1. Update the drawing code in this class.
    //
    // 2. Add new entries to the Valid Vertex Shapes column on the Misc
    //    worksheet in the NodeXLGraph.xltx Excel template.
    //
    // 3. Add a button to the Vertex Shape menu in the Excel ribbon, which is
    //    implemented in the Ribbon.cs file.
}


//*****************************************************************************
//  Enum: VertexEffect
//
/// <summary>
/// Specifies the WPF effect to apply to vertices.
/// </summary>
//*****************************************************************************

public enum
VertexEffect
{
    /// <summary>
    /// No effect is applied.
    /// </summary>

    None,

    /// <summary>
    /// A diffuse halo is drawn behind the vertex.
    /// </summary>

    OuterGlow,

    /// <summary>
    /// An offset shadow is drawn behind the vertex.
    /// </summary>

    DropShadow,
}


//*****************************************************************************
//  Enum: VertexLabelPosition
//
/// <summary>
/// Specifies the position of a vertex label drawn as an annotation.
/// </summary>
///
/// <remarks>
/// The positions are used only for vertex labels drawn as an annotation.  They
/// are not used when a vertex has the shape <see cref="VertexShape.Label" />.
///
/// <para>
/// The horizontal positions are called Left, Center, and Right.  The vertical
/// positions are called Top, Middle, and Bottom.  The label positions with
/// respect to the vertex's rectangle are shown above.
/// </para>
///
/// </remarks>
///
/// <example>
/// <code>
///                 TopCenter
///      TopLeft  --------------  TopRight
///              |              |
///              |              |
///   MiddleLeft | MiddleCenter | MiddleRight
///              |              |
///              |              |
///   BottomLeft  --------------  BottomRight
///                BottomCenter
/// </code>
/// </example>
//*****************************************************************************

public enum
VertexLabelPosition
{
    /// <summary>
    /// The label is right-justified.  See the position in the above drawing.
    /// </summary>

    TopLeft = 0,

    /// <summary>
    /// The label is center-justified.  See the position in the above drawing.
    /// </summary>

    TopCenter = 1,

    /// <summary>
    /// The label is left-justified.  See the position in the above drawing.
    /// </summary>

    TopRight = 2,

    /// <summary>
    /// The label is right-justified.  See the position in the above drawing.
    /// </summary>

    MiddleLeft = 3,

    /// <summary>
    /// The label is center-justified.  See the position in the above drawing.
    /// </summary>

    MiddleCenter = 4,

    /// <summary>
    /// The label is left-justified.  See the position in the above drawing.
    /// </summary>

    MiddleRight = 5,

    /// <summary>
    /// The label is right-justified.  See the position in the above drawing.
    /// </summary>

    BottomLeft = 6,

    /// <summary>
    /// The label is center-justified.  See the position in the above drawing.
    /// </summary>

    BottomCenter = 7,

    /// <summary>
    /// The label is left-justified.  See the position in the above drawing.
    /// </summary>

    BottomRight = 8,

    /// <summary>
    /// The label isn't drawn.
    /// </summary>

    Nowhere = 9,
}
}
