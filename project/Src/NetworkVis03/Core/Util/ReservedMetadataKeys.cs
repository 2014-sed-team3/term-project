﻿
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.Core
{
//*****************************************************************************
//  Class: ReservedMetadataKeys
//
/// <summary>
/// Contains metadata keys used by the NodeXL system.
/// </summary>
///
/// <remarks>
/// Some of the metadata keys defined in this class are meant for use by
/// applications that use the NodeXL class libraries.  An application can add
/// the <see cref="PerColor" /> key to a vertex or edge to set its color, for
/// example.  Other keys are used internally by NodeXL and are of no use to
/// applications -- <see cref="LayoutBaseLayoutComplete" /> is used internally
/// by the NodeXL layout classes, for example.
///
/// <para>
/// See <see cref="IMetadataProvider" /> for information on the metadata scheme
/// used by NodeXL.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class ReservedMetadataKeys : Object
{
    //*************************************************************************
    //  General public constants
    //*************************************************************************

    /// <summary>
    /// All reserved keys start with this character.
    /// </summary>

    public static readonly Char FirstChar = '~';

    /// <summary>
    /// Key added to an edge to specify an edge weight.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a Double.
    ///
    /// <para>
    /// Edge weights are used by the FruchtermanReingoldLayout class to
    /// determine the attractive force between an edge's vertices.
    /// </para>
    ///
    /// </remarks>

    public static readonly String EdgeWeight =
        FirstChar + "EW";


    //*************************************************************************
    //  Keys used by NodeXLControl
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex to set the vertex's tooltip when using
    /// NodeXLControl.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a String.
    ///
    /// <para>
    /// For tooltips to be visible, you must set
    /// NodeXLControl.UseVertexToolTips to true.  The default value is false.
    /// </para>
    ///
    /// </remarks>

    public static readonly String PerVertexToolTip =
        FirstChar + "VTT";

    /// <summary>
    /// Key added to the vertex that represents a collapsed group.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is an ICollection&lt;IVertex&gt;.
    ///
    /// <para>
    /// When a group of vertices is collapsed in the NodeXLControl, this key
    /// gets added to the new vertex that represents the collapsed group.  The
    /// value contains the group's vertices, which were all removed from the
    /// graph during the collapse operation.
    /// </para>
    ///
    /// </remarks>

    public static readonly String CollapsedVertices =
        FirstChar + "CV";

    /// <summary>
    /// Key added to the vertex that represents a collapsed group.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is an ICollection&lt;IEdge&gt;.
    ///
    /// <para>
    /// When a group of vertices is collapsed in the NodeXLControl, this key
    /// gets added to the new vertex that represents the collapsed group.  The
    /// value contains the group's internal edges, which were all removed from
    /// the graph during the collapse operation.
    /// </para>
    ///
    /// </remarks>

    public static readonly String CollapsedInternalEdges =
        FirstChar + "CIE";

    /// <summary>
    /// Key added to the vertex that represents a collapsed group.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a <see cref="Smrf.NodeXL.Core.GroupInfo"/> object.
    ///
    /// <para>
    /// When a group of vertices is collapsed in the NodeXLControl, this key
    /// gets added to the new vertex that represents the collapsed group.
    /// </para>
    ///
    /// </remarks>

    public static readonly String CollapsedGroupInfo =
        FirstChar + "CGI";

    /// <summary>
    /// Key added to an edge external to a collapsed group.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is an IVertex.
    ///
    /// <para>
    /// When a group of vertices is collapsed in the NodeXLControl, either this
    /// key or <see cref="OriginalVertex2InEdge" /> gets added to each edge
    /// external to the collapsed group.  The value is the collapsed vertex
    /// that was originally the edge's first or second vertex.  This is used to
    /// reconnect the edge to its original vertex when the group is later
    /// expanded.
    /// </para>
    ///
    /// </remarks>

    public static readonly String OriginalVertex1InEdge =
        FirstChar + "OV1";

    /// <summary>
    /// Key added to an edge external to a collapsed group.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is an IVertex.
    ///
    /// <para>
    /// See <see cref="OriginalVertex1InEdge" />.
    /// </para>
    ///
    /// </remarks>

    public static readonly String OriginalVertex2InEdge =
        FirstChar + "OV2";


    //*************************************************************************
    //  Keys used by GraphDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to a graph to specify how groups and the edges between them
    /// should be drawn when the graph is laid out using groups.
    /// </summary>
    ///
    /// <remarks>
    /// If this key is specified, each of the group rectangles specified in the
    /// value is drawn on top of the graph's background.  The rectangles are
    /// the layout bounds that were used to lay out the graph's groups.  Also,
    /// any combined intergroup edges specified in the value are drawn.
    ///
    /// <para>
    /// The key's value is of type
    /// Smrf.NodeXL.Layouts.GroupLayoutDrawingInfo.
    /// </para>
    ///
    /// </remarks>

    public static readonly String GroupLayoutDrawingInfo =
        FirstChar + "GDGLDI";

    /// <summary>
    /// Key added to a vertex to store information about how the vertex was
    /// drawn.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a
    /// Smrf.NodeXL.Visualization.Wpf.VertexDrawingHistory.
    /// </remarks>

    public static readonly String VertexDrawingHistory =
        FirstChar + "GDVDH";

    /// <summary>
    /// Key added to an edge to store information about how the edge was drawn.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a
    /// Smrf.NodeXL.Visualization.Wpf.EdgeDrawingHistory.
    /// </remarks>

    public static readonly String EdgeDrawingHistory =
        FirstChar + "GDEDH";


    //*************************************************************************
    //  Keys used by VertexDrawer and EdgeDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex or edge to draw it as selected.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is null.
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// When using NodeXLControl, do not use the <see cref="IsSelected" /> key
    /// to select vertex or edges.  Use the selection methods on the control
    /// instead.
    /// </para>
    ///
    /// </remarks>

    public static readonly String IsSelected =
        FirstChar + "S";

    /// <summary>
    /// Key added to a vertex or edge to control its visibility.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a member of the <see cref="VisibilityKeyValue" />
    /// enumeration.
    ///
    /// <para>
    /// If this key is present and its value is <see
    /// cref="VisibilityKeyValue.Filtered" /> or <see
    /// cref="VisibilityKeyValue.Hidden" />, any <see cref="PerAlpha" /> key is
    /// ignored.
    /// </para>
    ///
    /// </remarks>

    public static readonly String Visibility =
        FirstChar + "VEVis";

    /// <summary>
    /// Key added to a vertex or edge to force it to be a specified color.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value can be either a System.Drawing.Color or a 
    /// System.Windows.Media.Color.
    /// </remarks>

    public static readonly String PerColor =
        FirstChar + "PDColor";

    /// <summary>
    /// Key added to a vertex or edge to force it to be a specified
    /// transparency.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a Single between 0 (transparent) and 255 (opaque).
    /// (It's a Single instead of a Byte to reduce rounding errors when alpha
    /// values are converted to and from alpha ranges that don't run from 0 to
    /// 255.)
    ///
    /// <para>
    /// If the <see cref="Visibility" /> key is present and its value is <see
    /// cref="VisibilityKeyValue.Filtered" /> or <see
    /// cref="VisibilityKeyValue.Hidden" />, the <see cref="PerAlpha" /> key is
    /// ignored.
    /// </para>
    ///
    /// </remarks>

    public static readonly String PerAlpha =
        FirstChar + "PDAlpha";


    //*************************************************************************
    //  Keys used by VertexDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex to force it to be a specified shape.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a VertexShape.
    /// </remarks>

    public static readonly String PerVertexShape =
        FirstChar + "VDShape";

    /// <summary>
    /// Key added to a vertex to force it to be a specified radius.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a Single between VertexDrawer.MinimumRadius and
    /// VertexDrawer.MaximumRadius.
    /// </remarks>

    public static readonly String PerVertexRadius =
        FirstChar + "VDRadius";

    /// <summary>
    /// Key added to a vertex to specify the vertex's image.
    /// </summary>
    ///
    /// <remarks>
    /// If the vertex has the shape VertexShape.Image, the vertex is drawn as
    /// the image specified by the <see cref="PerVertexImage" /> key.  If the 
    /// vertex has the shape VertexShape.Image and the <see
    /// cref="PerVertexImage" /> key is missing, the vertex is drawn as a 
    /// VertexShape.Disk.
    ///
    /// <para>
    /// The key's value is a System.Windows.Media.ImageSource.
    /// </para>
    ///
    /// </remarks>

    public static readonly String PerVertexImage =
        FirstChar + "VDImage";

    /// <summary>
    /// Key added to a vertex to specify the vertex's label text.
    /// </summary>
    ///
    /// <remarks>
    /// The effect of this key depends on the vertex's shape.  If the shape is
    /// VertexShape.Label, the vertex is drawn as a rectangle containing the
    /// text specified by the <see cref="PerVertexLabel" /> key.  If the shape
    /// is something other than VertexShape.Label, the vertex is drawn as the
    /// specified shape and the <see cref="PerVertexLabel" /> text is drawn
    /// next to the vertex as an annotation, at the position specified by the
    /// optional <see cref="PerVertexLabelPosition" /> key.
    ///
    /// <para>
    /// If the vertex's shape is VertexShape.Label and the <see
    /// cref="PerVertexLabel" /> key is missing, the vertex is drawn as a
    /// VertexShape.Disk.
    /// </para>
    ///
    /// <para>
    /// The key's value is a System.String.  A value of null or an empty string
    /// is treated as a missing key.
    /// </para>
    ///
    /// </remarks>

    public static readonly String PerVertexLabel =
        FirstChar + "VDLabel";

    /// <summary>
    /// Key added to a vertex to specify the vertex's label position.
    /// </summary>
    ///
    /// <remarks>
    /// This has an effect only if a vertex label is drawn as an annotation.
    /// See <see cref="PerVertexLabel" /> for details.
    ///
    /// <para>
    /// The key's value is a VertexLabelPosition.
    /// </para>
    ///
    /// </remarks>

    public static readonly String PerVertexLabelPosition =
        FirstChar + "VDLabelPos";

    /// <summary>
    /// Key added to a vertex to force a label to have a specified fill color.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value can be either a System.Drawing.Color or a 
    /// System.Windows.Media.Color.  Used only if the vertex has the shape
    /// VertexShape.Label and <see cref="PerVertexLabel" /> is specified.
    /// </remarks>

    public static readonly String PerVertexLabelFillColor =
        FirstChar + "VDLabelFillColor";

    /// <summary>
    /// Key added to a vertex to specify the vertex's label font size.
    /// </summary>
    ///
    /// <remarks>
    /// If the vertex has the shape VertexShape.Label and also has the key <see
    /// cref="PerVertexLabelFontSize" />, the label text is drawn using the
    /// specified font size.
    ///
    /// <para>
    /// This key is not used for a label drawn as an annotation.
    /// </para>
    ///
    /// <para>
    /// The key's value is a Single and is in WPF font size units.
    /// </para>
    ///
    /// </remarks>

    public static readonly String PerVertexLabelFontSize =
        FirstChar + "VDLFS";


    //*************************************************************************
    //  Keys used by EdgeDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to an edge to force it to be a specified width.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a Single between EdgeDrawer.MinimumWidth and
    /// EdgeDrawer.MaximumWidth.
    /// </remarks>

    public static readonly String PerEdgeWidth =
        FirstChar + "EDWidth";

    /// <summary>
    /// Key added to an edge to force it to have a specified style.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is an EdgeStyle.
    /// </remarks>

    public static readonly String PerEdgeStyle =
        FirstChar + "EDStyle";

    /// <summary>
    /// Key added to an edge to force a label to be drawn on top of it.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a System.String that specifies the edge's label.  If
    /// null or empty, no label is drawn.
    /// </remarks>

    public static readonly String PerEdgeLabel =
        FirstChar + "EDLabel";

    /// <summary>
    /// Key added to an edge to force a label to have a specified text color.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value can be either a System.Drawing.Color or a 
    /// System.Windows.Media.Color.  Used only if <see cref="PerEdgeLabel" />
    /// is specified.
    /// </remarks>

    public static readonly String PerEdgeLabelTextColor =
        FirstChar + "EDLabelTextColor";

    /// <summary>
    /// Key added to an edge to force a label to have a specified font size.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a positive Single, in WPF units.  Used only if <see
    /// cref="PerEdgeLabel" /> is specified.
    /// </remarks>

    public static readonly String PerEdgeLabelFontSize =
        FirstChar + "EDLabelFontSize";

    /// <summary>
    /// Key added to an edge to force it to be drawn as a curve.
    /// </summary>
    ///
    /// <remarks>
    /// If the key is present on an edge and the EdgeDrawer.CurveStyle property
    /// is set to EdgeCurveStyle.CurveThroughIntermediatePoints, then the edge
    /// gets drawn as a smooth curve through the specified intermediate points. 
    ///
    /// <para>
    /// The key's value is a one-dimensional array of System.Drawing.PointF
    /// objects.  The array should include only the intermediate points on the
    /// curve; it should NOT include the edge's endpoints.  For example, if the
    /// curve should start at the edge's first vertex, pass through the
    /// intermediate points (53.9,204.1) and (87.2,180.5), and terminate at the
    /// edge's second vertex, then the key's value should be a two-element
    /// array of System.Drawing.PointF objects containing the points
    /// (53.9,204.1) and (87.2,180.5).
    /// </para>
    ///
    /// <para>
    /// If the EdgeDrawer.CurveStyle property is set to
    /// EdgeCurveStyle.CurveThroughIntermediatePoints and the <see
    /// cref="PerEdgeIntermediateCurvePoints" /> key is missing from an edge or
    /// contains a zero-length array, the edge gets drawn as a straight line.
    /// </para>
    ///
    /// <para>
    /// If this key is added to the graph's edges, the <see
    /// cref="GraphHasEdgeIntermediateCurvePoints" /> key should be added to
    /// the graph itself.
    /// </para>
    ///
    /// </remarks>

    public static readonly String PerEdgeIntermediateCurvePoints =
        FirstChar + "EDICP";


    //*************************************************************************
    //  Keys used by LayoutBase
    //*************************************************************************

    /// <summary>
    /// Key added to the graph after it has been laid out.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is null.
    /// </remarks>

    public static readonly String LayoutBaseLayoutComplete =
        FirstChar + "LBLayoutComplete";

    /// <summary>
    /// Key added to the graph to indicate that edges have intermediate curve
    /// points.
    /// </summary>
    ///
    /// <remarks>
    /// This key is added to the graph to indicate that the <see
    /// cref="PerEdgeIntermediateCurvePoints" /> key has been added to the
    /// graph's edges.  When the graph is resized, the layout classes check for
    /// this key to determine whether there are intermediate curve points that
    /// need to be transformed.
    ///
    /// <para>
    /// The key's value is null.
    /// </para>
    ///
    /// </remarks>

    public static readonly String GraphHasEdgeIntermediateCurvePoints =
        FirstChar + "LBGHEICP";


    //*************************************************************************
    //  Keys used by CollapsedGroupDrawingManager
    //*************************************************************************

    /// <summary>
    /// Key added to an edge to save its width before the group it connects to
    /// is collapsed.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a Single between EdgeDrawer.MinimumWidth and
    /// EdgeDrawer.MaximumWidth.
    /// </remarks>

    public static readonly String PreCollapsePerEdgeWidth =
        FirstChar + "CGDMPCEW";

    /// <summary>
    /// Key added to an edge to save its color before the group it connects to
    /// is collapsed.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a System.Drawing.Color.
    /// </remarks>

    public static readonly String PreCollapsePerEdgeColor =
        FirstChar + "CGDMPCEC";


    /// <summary>
    /// Key added to a collapsed group vertex that represents a fan motif.
    /// This is fill color of the fan part of the collapsed group vertex only.
    /// It is not the color of the head part of the collapsed group vertex.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a System.Drawing.Color.
    /// </remarks>

    public static readonly String FanMotifFanColor =
        FirstChar + "CGDMFC";


    //*************************************************************************
    //  Keys used by LayoutBase and NodeXLControl
    //*************************************************************************

    /// <summary>
    /// Key added to a graph to specify group information.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is an array of Smrf.NodeXL.Core.GroupInfo objects
    /// (or objects derived from that class), one for each group.
    ///
    /// <para>
    /// To parition the graph's vertices into groups and lay out each group in
    /// its own box, set the <see cref="GroupInfo" /> key on the graph and set
    /// LayoutBase.LayoutStyle to LayoutStyle.UseGroups.
    /// </para>
    ///
    /// <para>
    /// To expand and collapse vertex groups when using the NodeXLControl, set
    /// the <see cref="GroupInfo" /> key on the graph and call the
    /// NodeXLControl.CollapseGroup() or NodeXLControl.ExpandGroup() methods.
    /// </para>
    ///
    /// </remarks>

    public static readonly String GroupInfo =
        FirstChar + "ALBGLI";


    //*************************************************************************
    //  Keys used by SortableLayoutBase and GraphDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to every vertex to specify the order in which the vertices
    /// are laid out and drawn in the graph.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is of type Single.  You must also add the <see
    /// cref="SortableLayoutAndZOrderSet" /> key to the graph.
    ///
    /// <para>
    /// This key serves two purposes:
    /// </para>
    ///
    /// <list type="bullet">
    ///
    /// <item><term>
    /// If the graph is laid out using a layout class derived from
    /// SortableLayoutBase, the key determines the vertex's layout order when
    /// the graph is laid out.  Vertices with smaller key values are laid out
    /// first.  Other layout classes ignore the key.
    /// </term></item>
    ///
    /// <item><term>
    /// The key always determines the vertex's z-order when the graph is drawn,
    /// regardless of which layout class is used.  Vertices with smaller key
    /// values are drawn first.
    /// </term></item>
    ///
    /// </list>
    ///
    /// <para>
    /// If the <see cref="SortableLayoutAndZOrder" /> key is added to some
    /// vertices but not others, an exception is thrown.
    /// </para>
    ///
    /// </remarks>

    public static readonly String SortableLayoutAndZOrder =
        FirstChar + "SLZO";

    /// <summary>
    /// Key added to the graph to specify that the vertex layout and z-orders
    /// have been set on each vertex with the <see
    /// cref="SortableLayoutAndZOrder" /> key.
    /// </summary>
    ///
    /// <remarks>
    /// The value of the <see cref="SortableLayoutAndZOrderSet" /> key is null.
    /// </remarks>

    public static readonly String SortableLayoutAndZOrderSet =
        FirstChar + "SLZOS";


    //*************************************************************************
    //  Keys used by several layouts
    //*************************************************************************

    /// <summary>
    /// Key added to a graph to tell the layout algorithm to lay out only a
    /// specified set of vertices.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is an ICollection&lt;IVertex&gt; containing the
    /// vertices to lay out.  When this key is present on the graph, the layout
    /// completely ignores the graph's vertices that are not in the specified
    /// collection.
    ///
    /// <para>
    /// By default, the specified vertices are laid out within the full graph
    /// rectangle, but if the <see cref="LayOutTheseVerticesWithinBounds" />
    /// key is also added to the graph, then the specified vertices are laid
    /// out within the rectangle defined by the current locations of the
    /// outermost vertices in the set.
    /// </para>
    ///
    /// </remarks>

    public static readonly String LayOutTheseVerticesOnly =
        FirstChar + "LTheseOnly";

    /// <summary>
    /// Key added to a graph to tell the layout algorithm to lay out only a
    /// specified set of vertices within a bounding rectangle.
    /// </summary>
    ///
    /// <remarks>
    /// The key is used only if the <see cref="LayOutTheseVerticesOnly" /> key
    /// is also added to the graph.  If both keys are present, the specified
    /// vertices are laid out within the rectangle defined by the current
    /// locations of the outermost vertices in the set.
    ///
    /// <para>
    /// The value of the <see cref="LayOutTheseVerticesWithinBounds" /> key is
    /// null.
    /// </para>
    ///
    /// </remarks>

    public static readonly String LayOutTheseVerticesWithinBounds =
        FirstChar + "LTheseOnlyWithin";

    /// <summary>
    /// Key added to a vertex to tell the layout algorithm to leave the vertex
    /// at its current location.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a Boolean.  The layout may include the vertex in its
    /// overall layout calculations, but it cannot move the vertex.
    /// </remarks>

    public static readonly String LockVertexLocation =
        FirstChar + "LLock";


    //*************************************************************************
    //  Keys used by PolarLayout and PolarAbsoluteLayout
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex to specify its location in polar coordinates.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a SinglePolarCoordinates.
    ///
    /// <para>
    /// When using the PolarLayout, the SinglePolarCoordinates.R value should
    /// be between 0.0 and 1.0.  0.0 represents the origin and 1.0 represents
    /// the maximum distance from the origin, which is the smaller of half the
    /// graph rectangle's width or height.  R values less than 0.0 are the same
    /// as 0.0, and R values greater than 1.0 are the same as 1.0.
    /// </para>
    ///
    /// <para>
    /// When using the PolarAbsoluteLayout, the SinglePolarCoordinates.R value
    /// is unrestricted. 0.0 represents the origin, 1 represents one WPF unit
    /// (1/96 inch), and -1 represents one WPF unit in the opposite direction.
    /// </para>
    ///
    /// <para>
    /// The SinglePolarCoordinates.Angle value should be an angle in degrees,
    /// where 0.0 represents points on the positive x-axis and 90.0 represents
    /// points on the positive y-axis.  Any angle is valid.  361.0 degrees is
    /// the same as 1.0 degree, for example, and -1.0 degree is the same as
    /// 359.0 degrees.
    /// </para>
    ///
    /// </remarks>

    public static readonly String PolarLayoutCoordinates =
        FirstChar + "PLCoordinates";


    //*************************************************************************
    //  Keys used by FruchtermanReingoldLayout
    //*************************************************************************

    /// <summary>
    /// Key added to each vertex to temporarily store the vertex's Tag.
    /// </summary>

    public static readonly String FruchtermanReingoldLayoutTagStorage =
        FirstChar + "FRTag";


    //*************************************************************************
    //  Keys used by SugiyamaLayout, SugiyamaVertexDrawer, and
    //  SugiyamaEdgeDrawer
    //*************************************************************************

    /// <summary>
    /// Key added to a graph to save the radius computed by SugiyamaLayout.
    /// </summary>
    ///
    /// <remarks>
    /// SugiyamaLayout uses VertexDrawer.Radius as an initial radius for the
    /// vertex circles, then computes an actual radius that probably differs
    /// from the initial value.  It stores the computed radius in the IGraph's
    /// metadata in a key named SugiyamaComputedRadius  The type of the key's
    /// value is Single.
    /// </remarks>

    public static readonly String SugiyamaComputedRadius =
        FirstChar + "SgComputedRadius";


    /// <summary>
    /// Key added to an edge to save the edge curve points computed by
    /// SugiyamaLayout.
    /// </summary>
    ///
    /// <remarks>
    /// For each edge, SugiyamaLayout computes a curve that extends from the
    /// edge's first vertex to some point near the second vertex, then computes
    /// an endpoint on the second vertex's circle.  It stores an array of curve
    /// points in the edge's metadata in a key named SugiyamaCurvePoints.  The
    /// type of the key's value is PointF[].  The curve points should be drawn
    /// with code similar to the following:
    ///
    /// <code>
    ///
    /// GraphicsPath oGraphicsPath = new GraphicsPath();
    /// 
    /// oGraphicsPath.AddBeziers(aoCurvePoints);
    /// 
    /// oGraphics.DrawPath(oPen, oGraphicsPath);
    ///
    /// </code>
    ///
    /// </remarks>

    public static readonly String SugiyamaCurvePoints =
        FirstChar + "SgCurvePoints";


    /// <summary>
    /// Key added to an edge to save the edge curve endpoint computed by
    /// SugiyamaLayout.
    /// </summary>
    ///
    /// <remarks>
    /// For each edge, SugiyamaLayout computes an endpoint on the second
    /// vertex's circle and stores it in the edge's metadata in a key named
    /// SugiyamaEndpoint.  The type of the key's value is PointF.  The line
    /// from the last curve point to the endpoint should be drawn with code
    /// similar to the following:
    ///
    /// <code>
    ///
    /// oGraphics.DrawLine(oPen,
    ///     aoCurvePoints[aoCurvePoints.Length - 1],
    ///     oEndpoint
    ///     );
    ///
    /// </code>
    ///
    /// </remarks>

    public static readonly String SugiyamaEndpoint =
        FirstChar + "SgEndpoint";


    /// <summary>
    /// Key added to a vertex to temporarily store a NodeXL vertex's
    /// corresponding GLEE node.
    /// </summary>
    ///
    /// <remarks>
    /// This is used by SugiyamaLayout.  The type of the key's value is
    /// Microsoft.Glee.Node.
    /// </remarks>

    public static readonly String SugiyamaGleeNode =
        FirstChar + "SgGleeNode";

    /// <summary>
    /// Key added to an edge to temporarily store a NodeXL edge's corresponding
    /// GLEE edge.
    /// </summary>
    ///
    /// <remarks>
    /// This is used by SugiyamaLayout.  The type of the key's value is
    /// Microsoft.Glee.Edge.
    /// </remarks>

    public static readonly String SugiyamaGleeEdge =
        FirstChar + "SgGleeEdge";


    //*************************************************************************
    //  Keys used by GroupMetadataManager
    //*************************************************************************

    /// <summary>
    /// Key added to a graph to indicate that intergroup edges have been
    /// hidden.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is null.
    ///
    /// <para>
    /// When intergroup edges are hidden, <see cref="IntergroupEdgesHidden" />
    /// gets added to the graph.  Also, each intergroup edge gets a <see
    /// cref="Visibility" /> key that is set to <see
    /// cref="VisibilityKeyValue.Hidden" />, and each such edge also gets a
    /// <see cref="SavedVisibility" /> key that saves the previous visibility.
    /// </para>
    ///
    /// </remarks>

    public static readonly String IntergroupEdgesHidden =
        FirstChar + "GMMIntergroupEdgesHidden";

    /// <summary>
    /// Key added to a hidden intergroup edge to save the visibility it had
    /// before it was hidden.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is a member of the <see cref="VisibilityKeyValue" />
    /// enumeration.
    /// </remarks>
    ///
    /// <seealso cref="IntergroupEdgesHidden" />

    public static readonly String SavedVisibility =
        FirstChar + "GMMSavedVisibility";


    //*************************************************************************
    //  Keys used by ConnectedComponentCalculator
    //*************************************************************************

    /// <summary>
    /// Key added to each vertex in the graph while computing the graph's
    /// strongly connected components.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is of type Int32.
    /// </remarks>

    public static readonly String ConnectedComponentCalculatorIndex
        = FirstChar + "CCCI";

    /// <summary>
    /// Key added to each vertex in the graph while computing the graph's
    /// strongly connected components.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is of type Int32.
    /// </remarks>

    public static readonly String ConnectedComponentCalculatorLowLink
        = FirstChar + "CCCLL";


    //*************************************************************************
    //  Keys used by WorkbookReader and GraphMLGraphAdapter
    //*************************************************************************

    /// <summary>
    /// Key added to a graph to specify the complete set of metadata key values
    /// that may be present on the graph's edges.
    /// </summary>
    ///
    /// <remarks>
    /// When WorkbookReader.ReadWorkbook() is called with a
    /// ReadWorkbookContext.ReadAllEdgeAndVertexColumns value of true, this key
    /// gets added to the graph.  The key's value is of type String[] and the
    /// array contains one string for each possible key value.
    ///
    /// <para>
    /// Also, GraphMLGraphAdapter.LoadGraph() adds this key to the graph to
    /// indicate which keys may have been added to the graph's edges.
    /// </para>
    ///
    /// </remarks>

    public static readonly String AllEdgeMetadataKeys =
        FirstChar + "AllEdgeMetadataKeys";

    /// <summary>
    /// Key added to a graph to specify the complete set of metadata key values
    /// that may be present on the graph's vertices.
    /// </summary>
    ///
    /// <remarks>
    /// When WorkbookReader.ReadWorkbook() is called with a
    /// ReadWorkbookContext.ReadAllEdgeAndVertexColumns value of true, this key
    /// gets added to the graph.  The key's value is of type String[] and the
    /// array contains one string for each possible key value.
    ///
    /// <para>
    /// Also, GraphMLGraphAdapter.LoadGraph() adds this key to the graph to
    /// indicate which keys may have been added to the graph's vertices.
    /// </para>
    ///
    /// </remarks>

    public static readonly String AllVertexMetadataKeys =
        FirstChar + "AllVertexMetadataKeys";


    //*************************************************************************
    //  Keys used by VertexWorksheetReader
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex to mark it as a vertex that should be shown
    /// regardless of whether it is part of an edge.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is null.
    /// </remarks>

    public static readonly String VertexHasVisibilityOfShow =
        FirstChar + "VertexHasVisibilityOfShow";


    //*************************************************************************
    //  Keys used by GraphMLGraphAdapter
    //*************************************************************************

    /// <summary>
    /// Key added to a graph to describe where the graph came from.
    /// </summary>
    ///
    /// <remarks>
    /// GraphMLGraphAdapter.LoadGraph() adds this key to the graph if there is
    /// a description of the graph in the GraphML it loads.
    /// </remarks>

    public static readonly String GraphDescription =
        FirstChar + "GraphDescription";

    /// <summary>
    /// Key added to a graph to specify a suggested title.
    /// </summary>
    ///
    /// <remarks>
    /// GraphMLGraphAdapter.LoadGraph() adds this key to the graph if there is
    /// a suggested title in the GraphML it loads.
    /// </remarks>

    public static readonly String SuggestedTitle =
        FirstChar + "SuggestedTitle";

    /// <summary>
    /// Key added to a graph to specify a suggested file name.
    /// </summary>
    ///
    /// <remarks>
    /// GraphMLGraphAdapter.LoadGraph() adds this key to the graph if there is
    /// a suggested file name in the GraphML it loads.  The file name has no
    /// path or extension.
    /// </remarks>

    public static readonly String SuggestedFileNameNoExtension =
        FirstChar + "SuggestedFileNameNoExtension";


    //*************************************************************************
    //  Keys used by DraggedVertices
    //*************************************************************************

    /// <summary>
    /// Key added to a dragged vertex to store its original location.
    /// </summary>
    ///
    /// <remarks>
    /// The Visualization.Wpf.DraggedVertices class adds this key to each
    /// selected vertex before it is dragged.  The key's value is the vertex's
    /// original location, as a PointF.  The key is removed when the drag
    /// completes.
    /// </remarks>

    public static readonly String DraggedVerticesOriginalLocation =
        FirstChar + "DVOL";


    //*************************************************************************
    //  Keys used by any application
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex or edge to "mark" it.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is true or false.  The meaning of marking is up to the
    /// application.  None of the NodeXL core or visualization components pay
    /// attention to this key.
    /// </remarks>

    public static readonly String Marked =
        FirstChar + "M";


    //*************************************************************************
    //  Keys used by the ExcelTemplate application
    //*************************************************************************

    /// <summary>
    /// Key added to a vertex to add custom menu items to the vertex's
    /// context menu in the graph.
    /// </summary>
    ///
    /// <remarks>
    /// The key's value is an array of KeyValue&lt;String,String&gt;.  For each
    /// element, the key is the custom menu item text and the value is the
    /// custom menu item action.
    /// </remarks>

    public static readonly String CustomContextMenuItems =
        FirstChar + "CMI";
}


//*****************************************************************************
//  Enum: VisibilityKeyValue
//
/// <summary>
/// Values used by the <see cref="ReservedMetadataKeys.Visibility" /> reserved
/// metadata key.
/// </summary>
///
/// <remarks>
/// Setting the <see cref="ReservedMetadataKeys.Visibility" /> key on a vertex
/// or edge to one of these values controls the visibility of the vertex or
/// edge.
/// </remarks>
//*****************************************************************************

public enum
VisibilityKeyValue
{
    /// <summary>
    /// The vertex or edge is drawn normally.
    /// </summary>

    Visible,

    /// <summary>
    /// The vertex or edge is not drawn, even though it participated in the
    /// layout.
    ///
    /// <para>
    /// Any <see cref="ReservedMetadataKeys.PerAlpha" /> key is ignored.
    /// </para>
    ///
    /// </summary>

    Hidden,

    /// <summary>
    /// The vertex or edge is drawn, but with a reduced alpha to indicate that
    /// it has been filtered with a dynamic filter.
    ///
    /// <para>
    /// Any <see cref="ReservedMetadataKeys.PerAlpha" /> key is ignored.
    /// </para>
    ///
    /// </summary>

    Filtered,
}

}
