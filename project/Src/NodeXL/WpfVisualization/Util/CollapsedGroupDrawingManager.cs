
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.WpfGraphicsLib;
using Smrf.AppLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: CollapsedGroupDrawingManager
//
/// <summary>
/// Manages the drawing of the vertex that represents a collapsed group and its
/// incident edges.
/// </summary>
///
/// <remarks>
/// Call <see cref="PreDrawVertex" /> before drawing a vertex, and <see
/// cref="PostDrawVertex" /> after drawing the vertex.  Call <see
/// cref="RestoreExternalEdge" /> after expanding a collapsed group.
/// </remarks>
//*****************************************************************************

public class CollapsedGroupDrawingManager : VisualizationBase
{
    //*************************************************************************
    //  Constructor: CollapsedGroupDrawingManager()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CollapsedGroupDrawingManager" /> class.
    /// </summary>
    //*************************************************************************

    public CollapsedGroupDrawingManager()
    {
        m_oCollapsedGroupVertex = null;
        m_oCollapsedGroup = null;
        m_oCollapsedGroupAttributes = null;

        AssertValid();
    }

    //*************************************************************************
    //  Method: PreDrawVertex()
    //
    /// <summary>
    /// Performs tasks required before a vertex is drawn.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that is about to be drawn.  This may or may not be a vertex
    /// that represents a collapsed group.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="vertex" /> is a vertex that represents a collapsed
    /// group, this method performs pre-drawing tasks that are required to make
    /// the vertex look like a collapsed group.  Otherwise, this method does
    /// nothing.
    /// </remarks>
    //*************************************************************************

    public void
    PreDrawVertex
    (
        IVertex vertex
    )
    {
        Debug.Assert(vertex != null);
        AssertValid();

        // Check whether the vertex represents a collapsed group.

        Object oCollapsedGroupAsObject;

        if ( vertex.TryGetValue(ReservedMetadataKeys.CollapsedGroupInfo,
            typeof(GroupInfo), out oCollapsedGroupAsObject) )
        {
            // Yes.  Get the group information.

            m_oCollapsedGroupVertex = vertex;
            m_oCollapsedGroup = (GroupInfo)oCollapsedGroupAsObject;

            if ( String.IsNullOrEmpty(m_oCollapsedGroup.CollapsedAttributes) )
            {
                // No attributes were specified for the collapsed group.  Set
                // attributes on the collapsed group vertex that will cause it
                // to be drawn in a default manner.

                m_oCollapsedGroupAttributes = new CollapsedGroupAttributes();
                SetDefaultAttributesOnCollapsedGroup();
            }
            else
            {
                m_oCollapsedGroupAttributes =
                    CollapsedGroupAttributes.FromString(
                        m_oCollapsedGroup.CollapsedAttributes);

                SetSpecifiedAttributesOnCollapsedGroup();
            }
        }
        else
        {
            m_oCollapsedGroupVertex = null;
            m_oCollapsedGroup = null;
            m_oCollapsedGroupAttributes = null;
        }
    }

    //*************************************************************************
    //  Method: MoveVertexBoundsIfNecessary()
    //
    /// <summary>
    /// Moves a vertex's bounding rectangle if necessary.
    /// </summary>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="graphScale">
    /// Determines the scale of the graph's vertices and edges.
    /// </param>
    ///
    /// <param name="vertexBounds">
    /// The rectangle defining the bounds of the vertex.  This gets moved if
    /// necessary.  It does not get resized.
    /// </param>
    ///
    /// <remarks>
    /// If a later call to <see cref="PostDrawVertex" /> will draw additional
    /// elements that fall within the margin or outside the graph rectangle,
    /// this method moves <paramref name="vertexBounds" />.
    /// </remarks>
    //*************************************************************************

    public void
    MoveVertexBoundsIfNecessary
    (
        GraphDrawingContext graphDrawingContext,
        Double graphScale,
        ref Rect vertexBounds
    )
    {
        Debug.Assert(graphDrawingContext != null);
        Debug.Assert(graphScale >= GraphDrawer.MinimumGraphScale);
        Debug.Assert(graphScale <= GraphDrawer.MaximumGraphScale);
        AssertValid();

        // Additional elements are drawn only for the fan motif.

        String sType;

        if (
            m_oCollapsedGroupVertex != null
            &&
            m_oCollapsedGroupAttributes.TryGetValue(
                CollapsedGroupAttributeKeys.Type, out sType)
            &&
            sType == CollapsedGroupAttributeValues.FanMotifType
            )
        {
            // Move the fan bounds within the bounds of the graph rectangle's
            // margin.

            Rect oFanBounds = GetCircleSegment(vertexBounds,
                GetFanMotifAngleRadians(), graphScale).Bounds;

            Rect oMovedFanBounds = WpfGraphicsUtil.MoveRectangleWithinBounds(
                oFanBounds, graphDrawingContext.GraphRectangleMinusMargin,
                false);

            // Now move the vertex bounds by the same amount.

            vertexBounds = Rect.Offset(vertexBounds,
                oMovedFanBounds.Left - oFanBounds.Left,
                oMovedFanBounds.Top - oFanBounds.Top
                );
        }
    }

    //*************************************************************************
    //  Method: PostDrawVertex()
    //
    /// <summary>
    /// Performs tasks required after a vertex is drawn.
    /// </summary>
    ///
    /// <param name="vertexShape">
    /// The shape of the vertex.
    /// </param>
    ///
    /// <param name="vertexColor">
    /// The color of the vertex.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <param name="drawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="drawAsSelected">
    /// true if the vertex is selected.
    /// </param>
    ///
    /// <param name="graphScale">
    /// Determines the scale of the graph's vertices and edges.
    /// </param>
    ///
    /// <param name="vertexLabelDrawer">
    /// Object that draws a vertex label as an annotation.
    /// </param>
    ///
    /// <param name="formattedTextManager">
    /// Object that manages the creation of FormattedText objects.
    /// </param>
    ///
    /// <param name="vertexDrawingHistory">
    /// A <see cref="VertexDrawingHistory" /> object that retains information
    /// about how the vertex was drawn.
    /// </param>
    ///
    /// <remarks>
    /// If the vertex that was passed to <see cref="PreDrawVertex" /> is a
    /// vertex that represents a collapsed group, this method performs
    /// post-drawing tasks that are required to make the vertex look like a
    /// collapsed group.  Otherwise, this method does nothing.
    /// </remarks>
    //*************************************************************************

    public void
    PostDrawVertex
    (
        VertexShape vertexShape,
        Color vertexColor,
        GraphDrawingContext graphDrawingContext,
        DrawingContext drawingContext,
        Boolean drawAsSelected,
        Double graphScale,
        VertexLabelDrawer vertexLabelDrawer,
        FormattedTextManager formattedTextManager,
        VertexDrawingHistory vertexDrawingHistory
    )
    {
        Debug.Assert(graphDrawingContext != null);
        Debug.Assert(drawingContext != null);
        Debug.Assert(graphScale >= GraphDrawer.MinimumGraphScale);
        Debug.Assert(graphScale <= GraphDrawer.MaximumGraphScale);
        Debug.Assert(vertexLabelDrawer != null);
        Debug.Assert(formattedTextManager != null);
        Debug.Assert(vertexDrawingHistory != null);
        AssertValid();

        if (m_oCollapsedGroupVertex == null)
        {
            // The vertex that was drawn is not a collapsed group vertex.  Do
            // nothing.

            return;
        }

        if (m_oCollapsedGroupAttributes.Count == 0)
        {
            // The vertex that was drawn represents a collapsed group, but no
            // attributes were specified for it.  By default, such a vertex
            // gets a plus sign drawn on top of it.

            DrawPlusSign(vertexShape, vertexColor, graphDrawingContext,
                drawingContext, graphScale, vertexLabelDrawer,
                formattedTextManager, vertexDrawingHistory);
        }
        else
        {
            // Check whether this this is a collapsed motif.

            switch ( m_oCollapsedGroupAttributes.GetGroupType() )
            {
                case CollapsedGroupAttributeValues.FanMotifType:

                    DrawFanMotifFan(vertexColor, drawingContext,
                        drawAsSelected, graphScale, vertexDrawingHistory);

                    break;

                default:

                    // Do nothing.  This includes the D-connector motif case,
                    // which requires no post-drawing action.

                    break;
            }
        }
    }

    //*************************************************************************
    //  Method: RestoreExternalEdge()
    //
    /// <summary>
    /// Restores metadata on a group's external edge after the group is
    /// expanded.
    /// </summary>
    ///
    /// <param name="expandedExternalEdge">
    /// An edge that connects one of a group's vertices with a vertex not in
    /// the group.
    /// </param>
    ///
    /// <remarks>
    /// This method reverses any metadata changes to an edge that may have been
    /// made by <see cref="PreDrawVertex" />.  It should be called for each of
    /// a group's external edges when the group is expanded.
    /// </remarks>
    //*************************************************************************

    public static void
    RestoreExternalEdge
    (
        IEdge expandedExternalEdge
    )
    {
        Debug.Assert(expandedExternalEdge != null);

        // If this class saved the edge's width before it was collapsed,
        // restore it.

        Object oPreCollapsePerEdgeWidthAsObject;

        if ( expandedExternalEdge.TryGetValue(
            ReservedMetadataKeys.PreCollapsePerEdgeWidth, typeof(Single),
            out oPreCollapsePerEdgeWidthAsObject) )
        {
            expandedExternalEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth,
                oPreCollapsePerEdgeWidthAsObject);
        }

        // If this class saved the edge's color before it was collapsed,
        // restore it.

        Object oPreCollapsePerEdgeColorAsObject;

        if ( expandedExternalEdge.TryGetValue(
            ReservedMetadataKeys.PreCollapsePerEdgeColor,
            typeof(System.Drawing.Color),
            out oPreCollapsePerEdgeColorAsObject) )
        {
            expandedExternalEdge.SetValue(ReservedMetadataKeys.PerColor,
                oPreCollapsePerEdgeColorAsObject);
        }
    }

    //*************************************************************************
    //  Method: GetCollapsedMotifOutlineColor()
    //
    /// <summary>
    /// Gets the color to use for the outline of a collapsed motif.
    /// </summary>
    ///
    /// <param name="alpha">
    /// The alpha to use for the outline.
    /// </param>
    ///
    /// <returns>
    /// The color to use for the outline.
    /// </returns>
    //*************************************************************************

    public static Color
    GetCollapsedMotifOutlineColor
    (
        Byte alpha
    )
    {
        return ( WpfGraphicsUtil.SetWpfColorAlpha(Colors.DimGray, alpha) );
    }

    //*************************************************************************
    //  Method: SetDefaultAttributesOnCollapsedGroup()
    //
    /// <summary>
    /// Sets attributes on the vertex that represents a collapsed group that
    /// will cause it to be drawn in a default manner.
    /// </summary>
    //*************************************************************************

    protected void
    SetDefaultAttributesOnCollapsedGroup()
    {
        Debug.Assert(m_oCollapsedGroup != null);
        Debug.Assert(m_oCollapsedGroupVertex != null);
        AssertValid();

        // Get the vertices that were collapsed into the collapsed group
        // vertex.

        ICollection<IVertex> oCollapsedVertices = m_oCollapsedGroup.Vertices;

        Debug.Assert(oCollapsedVertices != null);
        Debug.Assert(oCollapsedVertices.Count > 0);

        // Use the color and shape of the first vertex in the group.

        IVertex oFirstVertexToCollapse = oCollapsedVertices.First();

        Object oColor;

        // Note: Don't check the type of the color here, because it can be
        // either of two types.

        if ( oFirstVertexToCollapse.TryGetValue(
            ReservedMetadataKeys.PerColor, out oColor) )
        {
            m_oCollapsedGroupVertex.SetValue(ReservedMetadataKeys.PerColor,
                oColor);
        }

        Object oShape;

        if ( oFirstVertexToCollapse.TryGetValue(
            ReservedMetadataKeys.PerVertexShape, typeof(VertexShape),
            out oShape) )
        {
            m_oCollapsedGroupVertex.SetValue(
                ReservedMetadataKeys.PerVertexShape, (VertexShape)oShape);
        }

        // Use a size determined by the number of collapsed vertices.

        Single fRadius = (Single)Math.Min(
            8F + oCollapsedVertices.Count * 1F,
            VertexDrawer.MaximumRadius / 12.0F);

        m_oCollapsedGroupVertex.SetValue(ReservedMetadataKeys.PerVertexRadius,
            fRadius);

        // Add the group name as a label.

        String sLabel = m_oCollapsedGroup.Label;

        if ( !String.IsNullOrEmpty(sLabel) )
        {
            m_oCollapsedGroupVertex.SetValue(
                ReservedMetadataKeys.PerVertexLabel, sLabel);
        }
    }

    //*************************************************************************
    //  Method: SetSpecifiedAttributesOnCollapsedGroup()
    //
    /// <summary>
    /// Sets attributes on the vertex that represents a collapsed group that
    /// will cause it to be drawn in the manner specified in the <see
    /// cref="GroupInfo" /> object.
    /// </summary>
    //*************************************************************************

    protected void
    SetSpecifiedAttributesOnCollapsedGroup()
    {
        Debug.Assert(m_oCollapsedGroup != null);

        Debug.Assert( !String.IsNullOrEmpty(
            m_oCollapsedGroup.CollapsedAttributes) );

        Debug.Assert(m_oCollapsedGroupVertex != null);
        Debug.Assert(m_oCollapsedGroupAttributes != null);

        AssertValid();

        // Check whether this is this a collapsed motif.

        switch ( m_oCollapsedGroupAttributes.GetGroupType() )
        {
            case CollapsedGroupAttributeValues.FanMotifType:

                SetCollapsedFanMotifAttributes();
                break;

            case CollapsedGroupAttributeValues.DConnectorMotifType:

                SetCollapsedDConnectorMotifAttributes();
                break;

            case CollapsedGroupAttributeValues.CliqueMotifType:

                SetCollapsedCliqueMotifAttributes();
                break;

            default:

                // Behave gracefully.  The collapsed group vertex will be
                // drawn as a default vertex.

                break;
        }
    }

    //*************************************************************************
    //  Method: SetCollapsedCliqueMotifAttributes()
    //
    /// <summary>
    /// Sets attributes on the vertex that represents a collapsed clique motif.
    /// </summary>
    //*************************************************************************

    protected void
    SetCollapsedCliqueMotifAttributes()
    {
        Debug.Assert(m_oCollapsedGroup != null);
        Debug.Assert(m_oCollapsedGroupVertex != null);
        Debug.Assert(m_oCollapsedGroupAttributes != null);
        AssertValid();

        // Get the vertices that were collapsed into the collapsed group
        // vertex.

        ICollection<IVertex> oCollapsedVertices = m_oCollapsedGroup.Vertices;

        Debug.Assert(oCollapsedVertices != null);
        Debug.Assert(oCollapsedVertices.Count > 0);

        // Use a rounded X for the collapsed group vertex.

        m_oCollapsedGroupVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.SolidRoundedX);

        // Use the color specified in the collapsed attributes.

        SetVertexColorFromCollapsedGroupAttributes(
            ReservedMetadataKeys.PerColor);

        Int32 iCliqueVertices;
        Double dCliqueScale;

        if (
            !m_oCollapsedGroupAttributes.TryGetValue(
                CollapsedGroupAttributeKeys.CliqueVertices,
                out iCliqueVertices)
            ||
            !m_oCollapsedGroupAttributes.TryGetValue(
                CollapsedGroupAttributeKeys.CliqueScale, out dCliqueScale)
            )
        {
            return;
        }

        // Scale the size by a scale factor stored in the collapsed group
        // attributes.  The scale factor ranges from 0 to 1.0.

        Single fRadius = MathUtil.TransformValueToRange(
            (Single)dCliqueScale,
            0, 1.0F,
            (Single)MinimumCliqueRadius,
            (Single)MaximumCliqueRadius
            );

        m_oCollapsedGroupVertex.SetValue(
            ReservedMetadataKeys.PerVertexRadius, fRadius);

        m_oCollapsedGroupVertex.SetValue(
            ReservedMetadataKeys.PerVertexToolTip,
            GetCollapsedCliqueMotifToolTip(iCliqueVertices)
            );
    }


    //*************************************************************************
    //  Method: SetCollapsedFanMotifAttributes()
    //
    /// <summary>
    /// Sets attributes on the vertex that represents a collapsed fan motif.
    /// </summary>
    //*************************************************************************

    protected void
    SetCollapsedFanMotifAttributes()
    {
        Debug.Assert(m_oCollapsedGroup != null);
        Debug.Assert(m_oCollapsedGroupVertex != null);
        Debug.Assert(m_oCollapsedGroupAttributes != null);
        AssertValid();

        // The vertex that represents the collapsed group should look like the
        // fan motif's head vertex.  Find the head vertex in the collection of
        // the group's vertices.

        String sHeadVertexName;
        Int32 iLeafVertices;

        if (
            !m_oCollapsedGroupAttributes.TryGetValue(
                CollapsedGroupAttributeKeys.HeadVertexName,
                out sHeadVertexName)
            ||
            !m_oCollapsedGroupAttributes.TryGetValue(
                CollapsedGroupAttributeKeys.LeafVertices, out iLeafVertices)
            )
        {
            // Behave gracefully.

            return;
        }

        Single radius = 0.1f;
        m_oCollapsedGroupVertex.SetValue(ReservedMetadataKeys.PerVertexRadius, radius);

        // The head part of the collapsed group vertex should have the same
        // color as the head vertex.  That color was just copied to the
        // collapsed group vertex.
        //
        // The fan part of the collapsed group vertex should be colored using
        // the color specified in the collapsed attributes for the fan motif.
        // Store this color in a special FanMotifFanColor key on the collapsed
        // group vertex.

        SetVertexColorFromCollapsedGroupAttributes(
            ReservedMetadataKeys.FanMotifFanColor);

        // Sample tooltip:
        //
        // Fan motif: 5 vertices with head vertex "HeadVertexName"

        m_oCollapsedGroupVertex.SetValue(
            ReservedMetadataKeys.PerVertexToolTip,

            String.Format(
                "Fan motif: {0} leaf vertices with head vertex \"{1}\""
                ,
                iLeafVertices,
                sHeadVertexName
                )
            );
    }

    //*************************************************************************
    //  Method: SetCollapsedDConnectorMotifAttributes()
    //
    /// <summary>
    /// Sets attributes on the vertex that represents a collapsed D-connector
    /// motif
    /// </summary>
    //*************************************************************************

    protected void
    SetCollapsedDConnectorMotifAttributes()
    {
        Debug.Assert(m_oCollapsedGroup != null);
        Debug.Assert(m_oCollapsedGroupVertex != null);
        Debug.Assert(m_oCollapsedGroupAttributes != null);
        AssertValid();

        // Get the vertices that were collapsed into the collapsed group
        // vertex.

        ICollection<IVertex> oCollapsedVertices = m_oCollapsedGroup.Vertices;

        Debug.Assert(oCollapsedVertices != null);
        Debug.Assert(oCollapsedVertices.Count > 0);

        // Use a tapered diamond for the collapsed group vertex.

        m_oCollapsedGroupVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.SolidTaperedDiamond);

        // Use the color specified in the collapsed attributes.

        SetVertexColorFromCollapsedGroupAttributes(
            ReservedMetadataKeys.PerColor);

        Int32 iAnchorVertices, iSpanVertices;
        Double dSpanScale;

        if (
            !m_oCollapsedGroupAttributes.TryGetValue(
                CollapsedGroupAttributeKeys.AnchorVertices,
                out iAnchorVertices)
            ||
            !m_oCollapsedGroupAttributes.TryGetValue(
                CollapsedGroupAttributeKeys.SpanVertices, out iSpanVertices)
            ||
            !m_oCollapsedGroupAttributes.TryGetValue(
                CollapsedGroupAttributeKeys.SpanScale, out dSpanScale)
            )
        {
            return;
        }

        // Scale the size by a scale factor stored in the collapsed group
        // attributes.  The scale factor ranges from 0 to 1.0.

        Single fRadius = MathUtil.TransformValueToRange(
            (Single)dSpanScale,
            0, 1.0F,
            (Single)MinimumDConnectorRadius,
            (Single)MaximumDConnectorRadius
            );

        m_oCollapsedGroupVertex.SetValue(
            ReservedMetadataKeys.PerVertexRadius, fRadius);

        m_oCollapsedGroupVertex.SetValue(
            ReservedMetadataKeys.PerVertexToolTip,
            GetCollapsedDConnectorMotifToolTip(iAnchorVertices, iSpanVertices)
            );

        SetCollapsedDConnectorMotifEdgeAttributes(iAnchorVertices);
    }

    //*************************************************************************
    //  Method: SetCollapsedDConnectorMotifEdgeAttributes()
    //
    /// <summary>
    /// Sets attributes on the edges incident to the vertex that represents a
    /// collapsed D-connector motif
    /// </summary>
    ///
    /// <param name="iAnchorVertices">
    /// Number of anchor vertices in the motif.
    /// </param>
    //*************************************************************************

    protected void
    SetCollapsedDConnectorMotifEdgeAttributes
    (
        Int32 iAnchorVertices
    )
    {
        Debug.Assert(iAnchorVertices >= 0);
        Debug.Assert(m_oCollapsedGroup != null);
        Debug.Assert(m_oCollapsedGroupVertex != null);
        Debug.Assert(m_oCollapsedGroupAttributes != null);
        AssertValid();

        // The key is the name of an anchor vertex and the value is the anchor
        // vertex's zero-based index.

        Dictionary<String, Int32> oAnchorVertexIndexes =
            GetAnchorVertexIndexes(iAnchorVertices);

        // Each of the collapsed group vertex's incident edges is connected to
        // one of the D-connector motif's anchor vertices.

        foreach (IEdge oIncidentEdge in m_oCollapsedGroupVertex.IncidentEdges)
        {
            // Determine which anchor vertex is on the other end of this edge.

            String sAdjacentVertexName = oIncidentEdge.GetAdjacentVertex(
                m_oCollapsedGroupVertex).Name;

            Int32 iAnchorVertexIndex;

            if (
                sAdjacentVertexName != null
                &&
                oAnchorVertexIndexes.TryGetValue(sAdjacentVertexName,
                    out iAnchorVertexIndex)
                )
            {
                // Retrieve the color that was calculated for the edges
                // connected to this anchor vertex, then assign the color to
                // the edge.

                System.Drawing.Color oAnchorVertexEdgeColor;

                if ( TryGetColor(

                    CollapsedGroupAttributeKeys.GetAnchorVertexEdgeColorKey(
                        iAnchorVertexIndex),

                    out oAnchorVertexEdgeColor) )
                {
                    SetCollapsedDConnectorMotifEdgeColor(oIncidentEdge,
                        oAnchorVertexEdgeColor);
                }

                // Ditto for the edge width.

                Double dAnchorVertexEdgeWidth;

                if ( m_oCollapsedGroupAttributes.TryGetValue(

                    CollapsedGroupAttributeKeys.GetAnchorVertexEdgeWidthKey(
                        iAnchorVertexIndex),

                    out dAnchorVertexEdgeWidth) )
                {
                    SetCollapsedDConnectorMotifEdgeWidth(oIncidentEdge,
                        dAnchorVertexEdgeWidth);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: GetAnchorVertexIndexes()
    //
    /// <summary>
    /// Gets a dictionary that maps anchor vertex names to anchor vertex
    /// indexes.
    /// </summary>
    ///
    /// <param name="iAnchorVertices">
    /// Number of anchor vertices in the motif.
    /// </param>
    ///
    /// <returns>
    /// The key is the name of an anchor vertex and the value is the anchor
    /// vertex's zero-based index.
    /// </returns>
    //*************************************************************************

    protected Dictionary<String, Int32>
    GetAnchorVertexIndexes
    (
        Int32 iAnchorVertices
    )
    {
        Debug.Assert(iAnchorVertices >= 0);
        Debug.Assert(m_oCollapsedGroup != null);
        Debug.Assert(m_oCollapsedGroupVertex != null);
        Debug.Assert(m_oCollapsedGroupAttributes != null);
        AssertValid();

        Dictionary<String, Int32> oAnchorVertexIndexes =
            new Dictionary<String, Int32>(iAnchorVertices);

        for (Int32 iAnchorVertexIndex = 0;
            iAnchorVertexIndex < iAnchorVertices;
            iAnchorVertexIndex++)
        {
            String sAnchorVertexName;

            if ( m_oCollapsedGroupAttributes.TryGetValue(

                CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(
                    iAnchorVertexIndex),

                out sAnchorVertexName) )
            {
                oAnchorVertexIndexes[sAnchorVertexName] = iAnchorVertexIndex;
            }
        }

        return (oAnchorVertexIndexes);
    }

    //*************************************************************************
    //  Method: GetAnchorVertexNames()
    //
    /// <summary>
    /// Gets a collection of the anchor vertex names.
    /// </summary>
    ///
    /// <param name="iAnchorVertices">
    /// Number of anchor vertices in the motif.
    /// </param>
    ///
    /// <returns>
    /// The names of the anchor vertices.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<String>
    GetAnchorVertexNames
    (
        Int32 iAnchorVertices
    )
    {
        Debug.Assert(iAnchorVertices >= 0);
        AssertValid();

        return ( GetAnchorVertexIndexes(iAnchorVertices).Keys );
    }

    //*************************************************************************
    //  Method: SetCollapsedDConnectorMotifEdgeWidth()
    //
    /// <summary>
    /// Sets the width attribute on an edge incident to the vertex that
    /// represents a collapsed D-connector motif.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to set the width on.
    /// </param>
    ///
    /// <param name="dWidth">
    /// The edge width.
    /// </param>
    //*************************************************************************

    protected void
    SetCollapsedDConnectorMotifEdgeWidth
    (
        IEdge oEdge,
        Double dWidth
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        // If the edge already has a width, save it.  This will be restored by
        // RestoreExternalEdge().

        Object oPerEdgeWidthAsObject;

        if ( oEdge.TryGetValue(ReservedMetadataKeys.PerEdgeWidth,
            typeof(Single), out oPerEdgeWidthAsObject) )
        {
            oEdge.SetValue(ReservedMetadataKeys.PreCollapsePerEdgeWidth,
                oPerEdgeWidthAsObject);
        }

        // Set the new width.

        oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth, (Single)dWidth);
    }

    //*************************************************************************
    //  Method: SetCollapsedDConnectorMotifEdgeColor()
    //
    /// <summary>
    /// Sets the color attribute on an edge incident to the vertex that
    /// represents a collapsed D-connector motif.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to set the color on.
    /// </param>
    ///
    /// <param name="oColor">
    /// The edge color.
    /// </param>
    //*************************************************************************

    protected void
    SetCollapsedDConnectorMotifEdgeColor
    (
        IEdge oEdge,
        System.Drawing.Color oColor
    )
    {
        AssertValid();

        // If the edge already has a color, save it.  This will be restored by
        // RestoreExternalEdge().

        Object oPerColorAsObject;

        if ( oEdge.TryGetValue(ReservedMetadataKeys.PerColor,
            typeof(System.Drawing.Color), out oPerColorAsObject) )
        {
            oEdge.SetValue(ReservedMetadataKeys.PreCollapsePerEdgeColor,
                oPerColorAsObject);
        }

        // Set the new color.

        oEdge.SetValue(ReservedMetadataKeys.PerColor, oColor);
    }

    //*************************************************************************
    //  Method: GetCollapsedDConnectorMotifToolTip()
    //
    /// <summary>
    /// Gets the tooltip to use on the vertex that represents a collapsed
    /// D-connector motif
    /// </summary>
    ///
    /// <param name="iAnchorVertices">
    /// Number of anchor vertices in the motif.
    /// </param>
    ///
    /// <param name="iSpanVertices">
    /// Number of span vertices in the motif.
    /// </param>
    ///
    /// <remarks>
    /// The tooltip to use.
    /// </remarks>
    //*************************************************************************

    protected String
    GetCollapsedDConnectorMotifToolTip
    (
        Int32 iAnchorVertices,
        Int32 iSpanVertices
    )
    {
        Debug.Assert(iAnchorVertices >= 0);
        Debug.Assert(iSpanVertices >= 0);
        AssertValid();

        // Sample tooltip:
        //
        // 4-connector motif: 5 span vertices anchored by "AnchorVertex1Name",
        // "AnchorVertex2Name", "AnchorVertex3Name", ...

        StringBuilder oToolTip = new StringBuilder();

        oToolTip.AppendFormat(
            "{0}-connector motif: {1} span vertices anchored by ",
            iAnchorVertices,
            iSpanVertices
            );

        Boolean bAppendComma = false;

        int addedVertices = 0;
        foreach ( String sAnchorVertexName in
            GetAnchorVertexNames(iAnchorVertices) )
        {
            // Only add three anchor vertices for a connector to prevent the name from growing too large
            addedVertices++;
            if (addedVertices > 3)
            {
                oToolTip.Append(", ...");
                break;
            }

            oToolTip.AppendFormat(
                "{0}\"{1}\""
                ,
                bAppendComma ? ", " : String.Empty,
                sAnchorVertexName
                );

            bAppendComma = true;
        }

        return ( oToolTip.ToString() );
    }

    //*************************************************************************
    //  Method: GetCollapsedCliqueMotifToolTip()
    //
    /// <summary>
    /// Gets the tooltip to use on the vertex that represents a collapsed
    /// clique motif
    /// </summary>
    ///
    /// <param name="iCliqueVertices">
    /// Number of clique vertices in the motif.
    /// </param>
    ///
    /// <remarks>
    /// The tooltip to use.
    /// </remarks>
    //*************************************************************************

    protected String
    GetCollapsedCliqueMotifToolTip
    (
        Int32 iCliqueVertices
    )
    {
        Debug.Assert(iCliqueVertices >= 0);
        AssertValid();

        // Sample tooltip:
        //
        // 5-clique motif: 5 member vertices

        StringBuilder oToolTip = new StringBuilder();

        oToolTip.AppendFormat(
            "{0}-clique motif: {0} member vertices"
            ,
            iCliqueVertices
            );

        return (oToolTip.ToString());
    }

    //*************************************************************************
    //  Method: SetVertexColorFromCollapsedGroupAttributes()
    //
    /// <summary>
    /// Sets a color attribute on the vertex that represents a collapsed motif.
    /// </summary>
    ///
    /// <param name="sKeyToSet">
    /// The key to set on the vertex.
    /// </param>
    ///
    /// <remarks>
    /// If the attributes that describe how the collapsed group should be drawn
    /// specify a color, this method sets the color on the vertex that
    /// represents a collapsed motif.
    /// </remarks>
    //*************************************************************************

    protected void
    SetVertexColorFromCollapsedGroupAttributes
    (
        String sKeyToSet
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sKeyToSet) );
        AssertValid();

        System.Drawing.Color oColor;

        if ( TryGetColor(CollapsedGroupAttributeKeys.VertexColor, out oColor) )
        {
            m_oCollapsedGroupVertex.SetValue(sKeyToSet, oColor);
        }
    }

    //*************************************************************************
    //  Method: DrawPlusSign()
    //
    /// <summary>
    /// Draws a plus sign on top of the collapsed group vertex.
    /// </summary>
    ///
    /// <param name="eVertexShape">
    /// The shape of the vertex.
    /// </param>
    ///
    /// <param name="oVertexColor">
    /// The color of the vertex.
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
    /// <param name="dGraphScale">
    /// Determines the scale of the graph's vertices and edges.
    /// </param>
    ///
    /// <param name="oVertexLabelDrawer">
    /// Object that draws a vertex label as an annotation.
    /// </param>
    ///
    /// <param name="oFormattedTextManager">
    /// Object that manages the creation of FormattedText objects.
    /// </param>
    ///
    /// <param name="oVertexDrawingHistory">
    /// A <see cref="VertexDrawingHistory" /> object that retains information
    /// about how the vertex was drawn.
    /// </param>
    //*************************************************************************

    protected void
    DrawPlusSign
    (
        VertexShape eVertexShape,
        Color oVertexColor,
        GraphDrawingContext oGraphDrawingContext,
        DrawingContext oDrawingContext,
        Double dGraphScale,
        VertexLabelDrawer oVertexLabelDrawer,
        FormattedTextManager oFormattedTextManager,
        VertexDrawingHistory oVertexDrawingHistory
    )
    {
        Debug.Assert(oGraphDrawingContext != null);
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(dGraphScale >= GraphDrawer.MinimumGraphScale);
        Debug.Assert(dGraphScale <= GraphDrawer.MaximumGraphScale);
        Debug.Assert(oVertexLabelDrawer != null);
        Debug.Assert(oFormattedTextManager != null);
        Debug.Assert(oVertexDrawingHistory != null);
        AssertValid();

        Color oFillColor;

        switch (eVertexShape)
        {
            case VertexShape.Circle:
            case VertexShape.Square:
            case VertexShape.Diamond:
            case VertexShape.Triangle:

                // The fill color is the color of the background.  Adjust the
                // fill color for the opacity of the vertex.

                oFillColor = WpfGraphicsUtil.SetWpfColorAlpha(
                    oGraphDrawingContext.BackColor, oVertexColor.A);

                break;

            default:

                oFillColor = oVertexColor;
                break;
        }

        Color oContrastingColor =
            WpfGraphicsUtil.GetContrastingColor(oFillColor);

        // The font size used below was chosen so that it is large enough to be
        // easily readable, but small enough to fit within the smallest
        // collapsed group vertex created by this class.

        oVertexLabelDrawer.DrawLabel(oDrawingContext, oGraphDrawingContext,
            oVertexDrawingHistory, VertexLabelPosition.MiddleCenter,

            oFormattedTextManager.CreateFormattedText("+", oContrastingColor,
                15.0, dGraphScale),

            oContrastingColor, false);
    }

    //*************************************************************************
    //  Method: DrawFanMotifFan()
    //
    /// <summary>
    /// Draws the fan part of a fan motif.
    /// </summary>
    ///
    /// <param name="oHeadColor">
    /// The color of the head part of the collapsed group vertex.
    /// </param>
    ///
    /// <param name="oDrawingContext">
    /// The DrawingContext to use.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true to draw the fan as selected.
    /// </param>
    ///
    /// <param name="dGraphScale">
    /// Determines the scale of the graph's vertices and edges.
    /// </param>
    ///
    /// <param name="oVertexDrawingHistory">
    /// A <see cref="VertexDrawingHistory" /> object that retains information
    /// about how the vertex was drawn.
    /// </param>
    ///
    /// <remarks>
    /// This method draws a fan motif's leaf vertices as a pie slice on top of
    /// the collapsed group vertex.  The original part of the collapsed group
    /// vertex, which represents the fan motif's head vertex, is called the
    /// "head," and the part the represents the leaf vertices is called the
    /// "fan."
    /// </remarks>
    //*************************************************************************

    protected void
    DrawFanMotifFan
    (
        Color oHeadColor,
        DrawingContext oDrawingContext,
        Boolean bDrawAsSelected,
        Double dGraphScale,
        VertexDrawingHistory oVertexDrawingHistory
    )
    {
        Debug.Assert(oDrawingContext != null);
        Debug.Assert(dGraphScale >= GraphDrawer.MinimumGraphScale);
        Debug.Assert(dGraphScale <= GraphDrawer.MaximumGraphScale);
        Debug.Assert(oVertexDrawingHistory != null);
        AssertValid();

        // The fill color to use for the fan was set in PreDrawVertex().

        Object oFanColorAsObject;

        if ( !m_oCollapsedGroupVertex.TryGetValue(
            ReservedMetadataKeys.FanMotifFanColor,
            typeof(System.Drawing.Color), out oFanColorAsObject) )
        {
            return;
        }

        // If the collapsed group vertex is selected, the entire vertex,
        // including the fan drawn here, should be drawn using oHeadColor,
        // which the caller has set to the selected color.

        Color oFanColor = bDrawAsSelected ? oHeadColor :
            WpfGraphicsUtil.ColorToWpfColor(
                (System.Drawing.Color)oFanColorAsObject);

        Double dAngleRadians = GetFanMotifAngleRadians();
        Rect oVertexBounds = oVertexDrawingHistory.GetBounds().Bounds;

        Brush oFillBrush = new SolidColorBrush(oFanColor);
        WpfGraphicsUtil.FreezeIfFreezable(oFillBrush);

        Color oPenColor = GetCollapsedMotifOutlineColor(255);
        Brush oPenBrush = new SolidColorBrush(oPenColor);
        WpfGraphicsUtil.FreezeIfFreezable(oPenBrush);

        Pen oPen = new Pen(oPenBrush, 2.0 * dGraphScale);
        WpfGraphicsUtil.FreezeIfFreezable(oPen);

        oDrawingContext.DrawGeometry( oFillBrush, oPen, 
            GetCircleSegment(oVertexBounds, dAngleRadians, dGraphScale) );
    }

    //*************************************************************************
    //  Method: GetCircleSegment()
    //
    /// <summary>
    /// Gets a circle segment for drawing the fan part of a fan motif.
    /// </summary>
    ///
    /// <param name="oVertexBounds">
    /// The rectangle defining the bounds of the vertex.
    /// </param>
    ///
    /// <param name="dAngleRadians">
    /// The angle of the segment, in radians.  Must be between 0 and PI.
    /// </param>
    ///
    /// <param name="dGraphScale">
    /// Determines the scale of the graph's vertices and edges.
    /// </param>
    ///
    /// <returns>
    /// A circle segment for drawing the fan part of a fan motif, as a
    /// PathGeometry.
    /// </returns>
    //*************************************************************************

    protected PathGeometry
    GetCircleSegment
    (
        Rect oVertexBounds,
        Double dAngleRadians,
        Double dGraphScale
    )
    {
        Debug.Assert(dGraphScale >= GraphDrawer.MinimumGraphScale);
        Debug.Assert(dGraphScale <= GraphDrawer.MaximumGraphScale);
        AssertValid();

        return ( WpfPathGeometryUtil.GetCircleSegment(
            WpfGraphicsUtil.GetRectCenter(oVertexBounds),
            FanMotifRadius * dGraphScale, dAngleRadians) );
    }

    //*************************************************************************
    //  Method: GetFanMotifAngleRadians()
    //
    /// <summary>
    /// Gets the angle to use for the fan in a fan motif.
    /// </summary>
    ///
    /// <returns>
    /// The angle to use when drawing the fan in a fan motif, in radians.
    /// </returns>
    //*************************************************************************

    protected Double
    GetFanMotifAngleRadians()
    {
        Debug.Assert(m_oCollapsedGroupAttributes != null);
        AssertValid();

        // Scale the fan's arc by a scale factor stored in the collapsed group
        // attributes.  The scale factor ranges from 0 to 1.0.

        Double dArcScale;

        if ( !m_oCollapsedGroupAttributes.TryGetValue(
            CollapsedGroupAttributeKeys.ArcScale, out dArcScale) )
        {
            // Behave gracefully.

            dArcScale = 0.5;
        }

        Double dAngleRadians = MathUtil.TransformValueToRange(
            (Single)dArcScale,
            0, 1.0F,
            (Single)MinimumFanMotifAngleRadians,
            (Single)MaximumFanMotifAngleRadians
            );

        return (dAngleRadians);
    }

    //*************************************************************************
    //  Method: TryGetColor()
    //
    /// <summary>
    /// Attempts to get a color from the collapsed group attributes.
    /// </summary>
    ///
    /// <param name="sKey">
    /// Name of the color key.
    /// </param>
    ///
    /// <param name="oColor">
    /// Where the color gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified color was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetColor
    (
        String sKey,
        out System.Drawing.Color oColor
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        AssertValid();

        oColor = System.Drawing.Color.Black;

        String sColor;

        if ( m_oCollapsedGroupAttributes.TryGetValue(sKey, out sColor) )
        {
            System.Drawing.ColorConverter oColorConverter =
                new System.Drawing.ColorConverter();

            try
            {
                oColor = (System.Drawing.Color)
                    oColorConverter.ConvertFromString(sColor);

                return (true);
            }
            catch (Exception)
            {
                // (Format errors raise a System.Exception with an inner
                // exception of type FormatException.  Go figure.)
            }
        }

        return (false);
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

        // m_oCollapsedGroupVertex
        // m_oCollapsedGroup
        // m_oCollapsedGroupAttributes
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Radius of a fan motif's fan.

    protected Double FanMotifRadius = 60.0;

    /// Minimum angle of a fan motif's fan, in radians.

    protected Double MinimumFanMotifAngleRadians = 10.0 * Math.PI / 180.0;

    /// Maximum angle of a fan motif's fan, in radians.

    protected Double MaximumFanMotifAngleRadians = 120.0 * Math.PI / 180.0;

    /// Minimum radius of a D-connector motif's span, in WPF units.

    protected Double MinimumDConnectorRadius = 30.0;

    /// Maximum radius of a D-connector motif's span, in WPF units.

    protected Double MaximumDConnectorRadius = 60.0;

    /// Minimum radius of a clique motif's rounded X, in WPF units.
    /// Scaling by Math.Sqrt(2) used to ensure the clique diagonal radius is
    /// the same as the D-connector motif span radius. However, the area of the
    /// clique rounded X is approximtely 26% larger due to convex vs. concave 
    /// curves. To make areas equal multiply radius by approximately 
    /// 0.6296404 instead.

    protected Double MinimumCliqueRadius = 30.0 / Math.Sqrt(2);

    /// Maximum radius of a clique motif's rounded X, in WPF units.
    /// Scaling by Math.Sqrt(2) used to ensure the clique diagonal radius is
    /// the same as the D-connector motif span radius. However, the area of the
    /// clique rounded X is approximtely 26% larger due to convex vs. concave 
    /// curves. To make areas equal multiply radius by approximately 
    /// 0.6296404 instead.
    
    protected Double MaximumCliqueRadius = 60.0 / Math.Sqrt(2);


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The vertex that represents a collapsed group, or null if the vertex
    /// being drawn is a regular vertex.

    protected IVertex m_oCollapsedGroupVertex;

    /// The collapsed group that the vertex being drawn represents, or null if
    /// the vertex being drawn is a regular vertex.

    protected GroupInfo m_oCollapsedGroup;

    /// Attributes that describe how the collapsed group should be drawn, or
    /// null if the vertex being drawn is a regular vertex.

    protected CollapsedGroupAttributes m_oCollapsedGroupAttributes;
}

}
