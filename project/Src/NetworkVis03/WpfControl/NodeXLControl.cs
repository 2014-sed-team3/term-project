﻿
// Define TRACE_LAYOUT_AND_DRAW to write layout and drawing information to the
// debug output.

// #define TRACE_LAYOUT_AND_DRAW


using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;
using Smrf.NodeXL.Algorithms;
using Smrf.GraphicsLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: NodeXLControl
//
/// <summary>
/// Lays out and renders a NodeXL graph in a WPF or Windows Forms desktop
/// application.
/// </summary>
///
/// <remarks>
///
/// <h3>Overview</h3>
///
/// <see cref="NodeXLControl" /> is one of several classes that render a NodeXL
/// graph.  It derives from FrameworkElement and is meant for use in WPF
/// desktop applications.
///
/// <para>
/// If you need a graph rendered as a Visual without the overhead of
/// FrameworkElement, use <see cref="NodeXLVisual" /> instead.  Note that <see
/// cref="NodeXLVisual" /> does not lay out the graph before drawing it.
/// </para>
///
/// <para>
/// You can also use <see cref="Wpf.GraphDrawer" />, which is a low-level class
/// used by both <see cref="NodeXLControl" /> and <see cref="NodeXLVisual" />.
/// <see cref="Wpf.GraphDrawer" /> cannot be directly rendered, however, and
/// requires a custom wrapper that hosts its GraphDrawer.<see
/// cref="Wpf.GraphDrawer.VisualCollection" /> object.  Also, it does not lay
/// out the graph before drawing it.
/// </para>
///
/// <para>
/// To use <see cref="NodeXLControl" />, populate the graph exposed by the <see
/// cref="NodeXLControl.Graph" /> property, then call <see
/// cref="DrawGraph(Boolean)" />.  See the sample code below.
/// </para>
///
/// <h3>Vertex and Edge Appearance</h3>
///
/// <para>
/// The default appearance of the graph's vertices is determined by the
/// following properties:
/// </para>
///
/// <list type="bullet">
/// <item><see cref="VertexColor" /></item>
/// <item><see cref="VertexSelectedColor" /></item>
/// <item><see cref="VertexShape" /></item>
/// <item><see cref="VertexRadius" /></item>
/// <item><see cref="VertexLabelFillColor" /></item>
/// <item><see cref="VertexLabelPosition" /></item>
/// </list>
///
/// <para>
/// The default appearance of the graph's edges is determined by the following
/// properties:
/// </para>
///
/// <list type="bullet">
/// <item><see cref="EdgeColor" /></item>
/// <item><see cref="EdgeSelectedColor" /></item>
/// <item><see cref="EdgeWidth" /></item>
/// <item><see cref="EdgeRelativeArrowSize" /></item>
/// </list>
///
/// <para>
/// The appearance of an individual vertex can be overridden by adding
/// appropriate metadata to the vertex via <see
/// cref="IMetadataProvider.SetValue" />.  The following metadata keys can be
/// used:
/// </para>
///
/// <list type="bullet">
/// <item><see cref="ReservedMetadataKeys.Visibility" /></item>
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexShape" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexRadius" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabel" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabelFillColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexLabelPosition" /></item>
/// <item><see cref="ReservedMetadataKeys.PerVertexImage" /></item>
/// </list>
///
/// <para>
/// Similarly, the appearance of an individual edge can be overridden by adding
/// appropriate metadata to the edge.  The following metadata keys can be used:
/// </para>
///
/// <list type="bullet">
/// <item><see cref="ReservedMetadataKeys.Visibility" /></item>
/// <item><see cref="ReservedMetadataKeys.PerColor" /></item>
/// <item><see cref="ReservedMetadataKeys.PerAlpha" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeWidth" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeStyle" /></item>
/// <item><see cref="ReservedMetadataKeys.PerEdgeLabel" /></item>
/// </list>
///
/// <h3>Shapes, Labels, and Images</h3>
///
/// <para>
/// By default, vertices are drawn as the shape specified by the <see
/// cref="VertexShape" /> property.  The shape of an individual vertex can be
/// overridden with the <see cref="ReservedMetadataKeys.PerVertexShape" />
/// metadata key.
/// </para>
///
/// <para>
/// To draw an individual vertex as a rectangle containing text, set the <see
/// cref="ReservedMetadataKeys.PerVertexShape" /> key to <see
/// cref="Wpf.VertexShape.Label" /> and set the <see
/// cref="ReservedMetadataKeys.PerVertexLabel" /> key to the label text.  The
/// rectangle's fill color can be controlled with the <see
/// cref="ReservedMetadataKeys.PerVertexLabelFillColor" /> key.
/// </para>
///
/// <para>
/// To annotate other vertex shapes with text, set the <see
/// cref="ReservedMetadataKeys.PerVertexLabel" /> key to the annotation text.
/// The text gets drawn next to the vertex at the position specified by <see
/// cref="VertexLabelPosition" />.  (The <see
/// cref="ReservedMetadataKeys.PerVertexLabel" /> key serves two purposes: it
/// is the text inside the rectangle when the vertex has the shape <see
/// cref="Wpf.VertexShape.Label" />, and it is the annotation text next to the
/// vertex when the vertex has one of the other shapes.  You cannot annotate a
/// vertex whose shape is <see cref="Wpf.VertexShape.Label" />.)
/// </para>
///
/// <para>
/// To draw an individual vertex as an image, set the <see
/// cref="ReservedMetadataKeys.PerVertexShape" /> to <see
/// cref="Wpf.VertexShape.Image" /> and set the <see
/// cref="ReservedMetadataKeys.PerVertexImage" /> key to the image.
/// </para>
///
/// <h3>Selecting Vertices and Edges</h3>
///
/// <para>
/// One or more vertices and their incident edges can be selected with the
/// mouse.  See the <see cref="MouseMode" /> property for details.
/// </para>
///
/// <para>
/// To programatically select and deselect vertices and edges, use the <see
/// cref="SetVertexSelected" />, <see cref="SetEdgeSelected" />, <see
/// cref="SetSelected" />, <see cref="SelectAll" />, and <see
/// cref="DeselectAll" /> methods.  To determine which vertices and edges are
/// selected, use the <see cref="SelectedVertices" /> and <see
/// cref="SelectedEdges" /> properties.
/// </para>
///
/// <para>
/// <b>Important Note:</b>: Do not use the <see
/// cref="ReservedMetadataKeys.IsSelected" /> metadata key to select vertex or
/// edges.  Use the selection methods on this control instead.
/// </para>
///
/// <h3>Zoom and Scale</h3>
///
/// <para>
/// The graph can be zoomed either programatically or with the mouse.  See
/// <see cref="GraphZoom" /> for details.
/// </para>
///
/// <para>
/// The size of the graph's vertices and edges can controlled with <see
/// cref="GraphScale" />.
/// </para>
///
/// <h3>Vertex Tooltips</h3>
///
/// <para>
/// A tooltip can be displayed when the mouse hovers over a vertex.  The
/// tooltip can be simple text or a custom UIElement containing arbitrary
/// content.  See <see cref="ShowVertexToolTips" /> for details.
/// </para>
///
/// <h3>Graph Layout Algorithm</h3>
///
/// <para>
/// By default, the control uses a force-directed Fruchterman-Reingold
/// algorithm to lay out the graph.  Use the <see cref="Layout" /> property to
/// specify a different layout.
/// </para>
///
/// <h3>Using NodeXLControl in WPF Applications</h3>
///
/// <example>
/// Here is sample C# code that populates a <see cref="NodeXLControl" /> graph
/// with several vertices and edges.  It's assumed that a <see
/// cref="NodeXLControl" /> named nodeXLControl1 has been added to the WPF
/// Window in the Visual Studio designer.
///
/// <code>
/**
using System;
using System.Windows;
using System.Windows.Media;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;

namespace WpfApplication1
{
public partial class Window1 : Window
{
    public Window1()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        PopulateAndDrawGraph();
    }

    protected void PopulateAndDrawGraph()
    {
        // Get the graph's vertex collection.

        IVertexCollection oVertices = nodeXLControl1.Graph.Vertices;

        // Add three vertices.

        IVertex oVertexA = oVertices.Add();
        IVertex oVertexB = oVertices.Add();
        IVertex oVertexC = oVertices.Add();

        // Change the color, radius, and shape of vertex A.

        oVertexA.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 255, 0, 255));

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexRadius, 20F);

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.Sphere);

        // Draw vertex B as a Label, which is a rectangle containing text.

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.Label);

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexLabel, "Label");

        // Set the label's text and fill colors.

        oVertexB.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 220, 220, 220));

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexLabelFillColor,
            Color.FromArgb(255, 0, 0, 0));

        // Annotate vertex C with text that is drawn outside the vertex.  All
        // shapes except Label can be annotated.

        oVertexC.SetValue(ReservedMetadataKeys.PerVertexLabel, "Annotation");

        // Get the graph's edge collection.

        IEdgeCollection oEdges = nodeXLControl1.Graph.Edges;

        // Connect the vertices with directed edges.

        IEdge oEdge1 = oEdges.Add(oVertexA, oVertexB, true);
        IEdge oEdge2 = oEdges.Add(oVertexB, oVertexC, true);
        IEdge oEdge3 = oEdges.Add(oVertexC, oVertexA, true);

        // Customize their appearance.

        oEdge1.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 55, 125, 98));

        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeWidth, 3F);
        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeLabel, "This is edge 1");

        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeWidth, 5F);
        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeLabel, "This is edge 2");

        oEdge3.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 0, 255, 0));

        nodeXLControl1.DrawGraph(true);
    }
}
}
*/
/// </code>
/// </example>
///
/// <h3>Using NodeXLControl in Windows Forms Applications</h3>
///
/// <example>
/// <see cref="NodeXLControl" /> can be used in Windows Forms applications by
/// embedding it within a Windows.Forms.Integration.ElementHost control, as in
/// the following sample code:
///
/// <code>
/**
using System;
using System.Windows.Forms;
using System.Drawing;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;

namespace WindowsFormsApplication1
{
public partial class Form1 : Form
{
    private NodeXLControl nodeXLControl1;

    public Form1()
    {
        InitializeComponent();

        nodeXLControl1 = new NodeXLControl();
        elementHost1.Child = nodeXLControl1;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        PopulateAndDrawGraph();
    }

    protected void PopulateAndDrawGraph()
    {
        // Get the graph's vertex collection.

        IVertexCollection oVertices = nodeXLControl1.Graph.Vertices;

        // Add three vertices.

        IVertex oVertexA = oVertices.Add();
        IVertex oVertexB = oVertices.Add();
        IVertex oVertexC = oVertices.Add();

        // Change the color, radius, and shape of vertex A.

        oVertexA.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 255, 0, 255));

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexRadius, 20F);

        oVertexA.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.Sphere);

        // Draw vertex B as a Label, which is a rectangle containing text.

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.Label);

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexLabel, "Label");

        // Set the label's text and fill colors.

        oVertexB.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 220, 220, 220));

        oVertexB.SetValue(ReservedMetadataKeys.PerVertexLabelFillColor,
            Color.FromArgb(255, 0, 0, 0));

        // Annotate vertex C with text that is drawn outside the vertex.  All
        // shapes except Label can be annotated.

        oVertexC.SetValue(ReservedMetadataKeys.PerVertexLabel, "Annotation");

        // Get the graph's edge collection.

        IEdgeCollection oEdges = nodeXLControl1.Graph.Edges;

        // Connect the vertices with directed edges.

        IEdge oEdge1 = oEdges.Add(oVertexA, oVertexB, true);
        IEdge oEdge2 = oEdges.Add(oVertexB, oVertexC, true);
        IEdge oEdge3 = oEdges.Add(oVertexC, oVertexA, true);

        // Customize their appearance.

        oEdge1.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 55, 125, 98));

        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeWidth, 3F);
        oEdge1.SetValue(ReservedMetadataKeys.PerEdgeLabel, "This is edge 1");

        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeWidth, 5F);
        oEdge2.SetValue(ReservedMetadataKeys.PerEdgeLabel, "This is edge 2");

        oEdge3.SetValue(ReservedMetadataKeys.PerColor,
            Color.FromArgb(255, 0, 255, 0));

        nodeXLControl1.DrawGraph(true);
    }
}
}
*/
/// </code>
/// </example>
///
/// <h3>Future Work</h3>
///
/// <para>
/// This is the first WPF version of the NodeXLControl.  It replaces a previous
/// Windows Forms implementation.  This version provides the functionality
/// needed by the NodeXL Excel Template project, but does not yet take
/// advantage of WPF features such as dependency properties, routed events, and
/// so on.  Additional WPF features may be added in future versions, depending
/// on resource availability and how much demand there is for them.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class NodeXLControl : FrameworkElement
{
    //*************************************************************************
    //  Static constructor: NodeXLControl()
    //
    /// <summary>
    /// Static constructor for the <see cref="NodeXLControl" /> class.
    /// </summary>
    //*************************************************************************

    static NodeXLControl()
    {
        // If this isn't done, the Keyboard class can't be used to detect which
        // keys are pressed.

        FocusableProperty.OverrideMetadata( typeof(NodeXLControl),
            new UIPropertyMetadata(true) );
    }

    //*************************************************************************
    //  Constructor: NodeXLControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NodeXLControl" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLControl()
    {
        m_oGraph = new Graph();
        CreateGraphDrawer();
        m_fEdgeBundlerStraightening = 0.15F;

        m_oLayout = new FruchtermanReingoldLayout();
        OnNewLayout(m_oLayout);

        m_oLastLayoutContext =
            new LayoutContext(System.Drawing.Rectangle.Empty);

        m_oLastGraphDrawingContext = null;

        m_eLayoutState = LayoutState.Stable;

        m_eMouseMode = MouseMode.Select;
        m_bMouseAlsoSelectsIncidentEdges = true;
        m_bAllowVertexDrag = true;

        m_oVerticesBeingDragged = null;
        m_oMarqueeBeingDragged = null;
        m_oTranslationBeingDragged = null;

        m_oSelectedVertices = new HashSet<IVertex>();
        m_oSelectedEdges = new HashSet<IEdge>();
        m_oCollapsedGroups = new Dictionary<String, IVertex>();
        m_oDoubleClickedVertexInfo = null;

        m_bShowVertexToolTips = false;
        m_oLastMouseMoveLocation = new Point(-1, -1);

        // Create a helper object for displaying vertex tooltips.

        CreateVertexToolTipTracker();
        m_oVertexToolTip = null;

        m_bGraphZoomCentered = false;

        this.AddLogicalChild(m_oGraphDrawer.VisualCollection);

        CreateTransforms();

        // Prevent a focus rectangle from being drawn around the control when
        // it captures keyboard focus.  The focus rectangle does not behave
        // properly when the layout and render transforms are applied --
        // sometimes the rectangle disappears, and sometimes it gets magnified
        // by the render layout.

        this.FocusVisualStyle = null;

        // AssertValid();
    }

    #if false 
    //*************************************************************************
    //  Destructor: NodeXLControl()
    //
    /// <summary>
    /// Destructor forthe <see cref="NodeXLControl" /> class.
    /// </summary>
    //*************************************************************************

    ~NodeXLControl()
    {
        // TODO: This is wrong.  Where should ToolTipTracker.Dispose() be
        // called from?

        // Prevent ToolTipTracker's timer-based events from firing after the
        // control no longer has a handle.

        if (m_oVertexToolTipTracker != null)
        {
            m_oVertexToolTipTracker.Dispose();
        }
    }
    #endif

    //*************************************************************************
    //  Property: Graph
    //
    /// <summary>
    /// Gets or sets the graph to draw.
    /// </summary>
    ///
    /// <value>
    /// The graph to draw, as an <see cref="IGraph" />.
    /// </value>
    ///
    /// <remarks>
    /// After the graph is populated or modified, you must call <see
    /// cref="DrawGraph(Boolean)" /> to draw it.
    ///
    /// <para>
    /// An exception is thrown if this property is set while an asynchronous
    /// layout is in progress.  Check <see cref="IsLayingOutGraph" /> before
    /// using this property.
    /// </para>
    ///
    /// <para>
    /// Do not set this property to a graph that is already owned by another
    /// graph drawer.  If you want to simultaneously draw the same graph with
    /// two different graph drawers, make a copy of the graph using
    /// IGraph.<see cref="IGraph.Clone(Boolean, Boolean)" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public IGraph
    Graph
    {
        get
        {
            AssertValid();

            return (m_oGraph);
        }

        set
        {
            const String PropertyName = "Graph";

            this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);
            CheckIfLayingOutGraph(PropertyName);

            DeselectAll();
            m_oCollapsedGroups.Clear();

            m_oGraph = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the object to use to lay out the graph.
    /// </summary>
    ///
    /// <value>
    /// The object to use to lay out the graph, as an <see cref="ILayout" />.
    /// The default value is a <see cref="FruchtermanReingoldLayout" /> object.
    /// </value>
    ///
    /// <remarks>
    /// An exception is thrown if this property is set while an asynchronous
    /// layout is in progress.  Check <see cref="IsLayingOutGraph" /> before
    /// using this property.
    ///
    /// <para>
    /// This property can be set to any object that implements <see
    /// cref="ILayout" />, whether it is provided by NodeXL or implemented
    /// by the application.  For a list of provided layout classes, see <see
    /// cref="LayoutBase" />.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <example>
    /// The example shows how to lay out the graph as a grid:
    ///
    /// <code>
    /// Debug.Assert(!nodeXLControl.IsLayingOutGraph);
    /// nodeXLControl.Layout = new GridLayout();
    /// </code>
    ///
    /// </example>
    //*************************************************************************

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public ILayout
    Layout
    {
        get
        {
            AssertValid();

            return (m_oLayout);
        }

        set
        {
            const String PropertyName = "Layout";

            this.ArgumentChecker.CheckPropertyNotNull(PropertyName, value);
            CheckIfLayingOutGraph(PropertyName);

            m_oLayout = value;

            OnNewLayout(m_oLayout);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BackColor
    //
    /// <summary>
    /// Gets or sets the graph's background color.
    /// </summary>
    ///
    /// <value>
    /// The graph's background color, as a <see
    /// cref="System.Windows.Media.Color" />.  The default value is
    /// SystemColors.<see cref="SystemColors.WindowColor" />.
    /// </value>
    ///
    /// <remarks>
    /// When the graph is drawn, the background color specified by this
    /// property is drawn first, followed by any background image specified by
    /// <see cref="BackgroundImage" />, followed by the graph itself.
    ///
    /// <para>
    /// This is called BackColor instead of BackgroundColor for consistency
    /// with the rest of the .NET Framework.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="BackgroundImage" />
    //*************************************************************************

    public Color
    BackColor
    {
        get
        {
            AssertValid();

            return (m_oGraphDrawer.BackColor);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            m_oGraphDrawer.BackColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BackgroundImage
    //
    /// <summary>
    /// Gets or sets the graph's background image.
    /// </summary>
    ///
    /// <value>
    /// The graph's background image, as an ImageSource, or null for no
    /// background image.  The default value is null.
    /// </value>
    ///
    /// <seealso cref="BackColor" />
    //*************************************************************************

    public ImageSource
    BackgroundImage
    {
        get
        {
            AssertValid();

            return (m_oGraphDrawer.BackgroundImage);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            m_oGraphDrawer.BackgroundImage = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexShape
    //
    /// <summary>
    /// Gets or sets the default shape of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The default shape of the vertices, as a <see cref="Wpf.VertexShape" />.
    /// The default value is <see cref="Wpf.VertexShape.Disk" />.
    /// </value>
    ///
    /// <remarks>
    /// The default shape of a vertex can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexShape" /> key on the vertex.
    /// </remarks>
    //*************************************************************************

    public VertexShape
    VertexShape
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.Shape);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.Shape = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexRadius
    //
    /// <summary>
    /// Gets or sets the default radius of the vertices.
    /// </summary>
    ///
    /// <value>
    /// The default radius of the vertices, as a <see cref="Double" />.  Must
    /// be between <see cref="Wpf.VertexDrawer.MinimumRadius" /> and <see
    /// cref="Wpf.VertexDrawer.MaximumRadius" />, inclusive.  The default value
    /// is 3.0.
    /// </value>
    ///
    /// <remarks>
    /// The default radius of a vertex can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexRadius" /> key on the vertex.
    /// </remarks>
    //*************************************************************************

    public Double
    VertexRadius
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.Radius);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.Radius = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexColor
    //
    /// <summary>
    /// Gets or sets the default color of unselected vertices.
    /// </summary>
    ///
    /// <value>
    /// The default color of unselected vertices, as a <see cref="Color" />.
    /// The default value is <see cref="SystemColors.WindowTextColor" />.
    /// </value>
    ///
    /// <remarks>
    /// The default color of an unselected vertex can be overridden by setting
    /// the <see cref="ReservedMetadataKeys.PerColor" /> key on the vertex.
    /// The key's value can be of type System.Drawing.Color or
    /// System.Windows.Media.Color.
    /// </remarks>
    ///
    /// <seealso cref="VertexSelectedColor" />
    //*************************************************************************

    public Color
    VertexColor
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.Color);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.Color = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexSelectedColor
    //
    /// <summary>
    /// Gets or sets the color of selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The color of selected vertices, as a <see cref="Color" />.  The default
    /// value is <see cref="SystemColors.HighlightColor" />.
    /// </value>
    ///
    /// <remarks>
    /// The color of selected vertices cannot be set on a per-vertex basis.
    /// </remarks>
    ///
    /// <seealso cref="VertexColor" />
    //*************************************************************************

    public Color
    VertexSelectedColor
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.SelectedColor);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.SelectedColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelFillColor
    //
    /// <summary>
    /// Gets or sets the default fill color to use for vertices that have the
    /// Label shape.
    /// </summary>
    ///
    /// <value>
    /// The default fill color to use.  The default is
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
    VertexLabelFillColor
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.LabelFillColor);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.LabelFillColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelPosition
    //
    /// <summary>
    /// Gets or sets the default position of a vertex label drawn as an
    /// annotation.
    /// </summary>
    ///
    /// <value>
    /// The default position of a vertex label drawn as an annotation.  The
    /// default is <see cref="Wpf.VertexLabelPosition.TopRight" />.
    /// </value>
    ///
    /// <remarks>
    /// This property is not used when drawing vertices that have the shape
    /// <see cref="Wpf.VertexShape.Label" />.
    ///
    /// <para>
    /// The default vertex label position can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerVertexLabelPosition" /> key on the
    /// vertex.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public VertexLabelPosition
    VertexLabelPosition
    {
        get
        {
            AssertValid();

            return (this.VertexDrawer.LabelPosition);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.LabelPosition = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeWidth
    //
    /// <summary>
    /// Gets or sets the default edge width.
    /// </summary>
    ///
    /// <value>
    /// The default edge width, as a <see cref="Double" />.  Must be between
    /// <see cref="Wpf.EdgeDrawer.MinimumWidth" /> and <see
    /// cref="Wpf.EdgeDrawer.MaximumWidth" />, inclusive.  The default value
    /// is 1.
    /// </value>
    ///
    /// <remarks>
    /// The default edge width can be overridden by setting the <see
    /// cref="ReservedMetadataKeys.PerEdgeWidth" /> key on the edge.
    /// </remarks>
    //*************************************************************************

    public Double
    EdgeWidth
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.Width);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.Width = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeColor
    //
    /// <summary>
    /// Gets or sets the default color of unselected edges.
    /// </summary>
    ///
    /// <value>
    /// The default color of unselected edges, as a <see cref="Color" />.  The
    /// default value is <see cref="SystemColors.WindowTextColor" />.
    /// </value>
    ///
    /// <remarks>
    /// The default color of an unselected edge can be overridden by setting
    /// the <see cref="ReservedMetadataKeys.PerColor" /> key on the edge.  The
    /// key's value can be of type System.Drawing.Color or
    /// System.Windows.Media.Color.
    /// </remarks>
    ///
    /// <seealso cref="EdgeSelectedColor" />
    //*************************************************************************

    public Color
    EdgeColor
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.Color);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.Color = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeSelectedColor
    //
    /// <summary>
    /// Gets or sets the color of selected edges.
    /// </summary>
    ///
    /// <value>
    /// The color of selected edges, as a <see cref="Color" />.  The default
    /// value is <see cref="SystemColors.HighlightColor" />.
    /// </value>
    ///
    /// <remarks>
    /// The color of selected edges cannot be set on a per-vertex basis.
    /// </remarks>
    ///
    /// <seealso cref="EdgeColor" />
    //*************************************************************************

    public Color
    EdgeSelectedColor
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.SelectedColor);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.SelectedColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeRelativeArrowSize
    //
    /// <summary>
    /// Gets or sets the relative size of arrowheads on directed edges.
    /// </summary>
    ///
    /// <value>
    /// The relative size of arrowheads, as a <see cref="Double" />.  Must be
    /// between <see cref="Wpf.EdgeDrawer.MinimumRelativeArrowSize" /> and <see
    /// cref="Wpf.EdgeDrawer.MaximumRelativeArrowSize" />, inclusive.  The
    /// default value is 3.
    /// </value>
    ///
    /// <remarks>
    /// The value is relative to <see cref="EdgeWidth" />.  If the width is
    /// increased, the arrow size is increased proportionally.
    /// </remarks>
    //*************************************************************************

    public Double
    EdgeRelativeArrowSize
    {
        get
        {
            AssertValid();

            return (this.EdgeDrawer.RelativeArrowSize);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.EdgeDrawer.RelativeArrowSize = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeBundlerStraightening
    //
    /// <summary>
    /// Gets or sets a value that determines how much straightening is applied
    /// when edges are bundled.
    /// </summary>
    ///
    /// <value>
    /// A value between <see cref="MinimumEdgeBundlerStraightening" /> and <see
    /// cref="MaximumEdgeBundlerStraightening" />, where larger values result
    /// in more edge straightening.  The default value is 0.15.
    /// </value>
    ///
    /// <remarks>
    /// This is used only when the <see cref="Wpf.EdgeDrawer.CurveStyle" />
    /// property on the <see cref="EdgeDrawer" /> object is set to <see
    /// cref="EdgeCurveStyle.CurveThroughIntermediatePoints" />, in which case
    /// the <see cref="NodeXLControl" /> uses an internal <see
    /// cref="EdgeBundler" /> object to calculate the intermediate points.
    /// </remarks>
    //*************************************************************************

    public Single
    EdgeBundlerStraightening
    {
        get
        {
            AssertValid();

            return (m_fEdgeBundlerStraightening);
        }

        set
        {
            const String PropertyName = "EdgeBundlerStraightening";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumEdgeBundlerStraightening,
                MaximumEdgeBundlerStraightening);

            if (value != m_fEdgeBundlerStraightening)
            {
                // It is okay to change this property while the graph is being
                // laid out, because the property is not used until the layout
                // completes.

                m_fEdgeBundlerStraightening = value;
                BundleAllEdgesIfAppropriate();
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FilteredAlpha
    //
    /// <summary>
    /// Gets or sets the alpha value to use for vertices and edges that are
    /// filtered.
    /// </summary>
    ///
    /// <value>
    /// The alpha value to use for vertices and edges that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> value of <see
    /// cref="VisibilityKeyValue.Filtered" />.  Must be between 0 (invisible)
    /// and 255 (opaque).  The default value is 10.
    /// </value>
    //*************************************************************************

    public Byte
    FilteredAlpha
    {
        get
        {
            AssertValid();

            Debug.Assert(this.VertexDrawer.FilteredAlpha ==
                this.EdgeDrawer.FilteredAlpha);

            return (this.VertexDrawer.FilteredAlpha);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the property is not used until OnRender() is
            // called.

            this.VertexDrawer.FilteredAlpha = this.EdgeDrawer.FilteredAlpha =
                value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MouseMode
    //
    /// <summary>
    /// Gets or sets a value that determines how the mouse can be used to
    /// interact with the graph.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="Visualization.Wpf.MouseMode" /> value.  The default value
    /// is <see cref="Visualization.Wpf.MouseMode.Select" />.
    /// </value>
    ///
    /// <remarks>
    /// The mouse behavior is also affected by the <see
    /// cref="MouseAlsoSelectsIncidentEdges" /> property.
    ///
    /// <para>
    /// When this property is set to <see
    /// cref="Visualization.Wpf.MouseMode.Select" />, clicking on a vertex
    /// results in the following sequence:
    /// </para>
    ///
    /// <list type="bullet">
    ///
    /// <item><description>
    /// The <see cref="GraphMouseDown" /> event fires.
    /// </description></item>
    ///
    /// <item><description>
    /// The <see cref="VertexClick" /> event fires.
    /// </description></item>
    ///
    /// <item><description>
    /// The vertex and possibly its incident edges are redrawn as selected or
    /// unselected.
    /// </description></item>
    ///
    /// <item><description>
    /// The <see cref="SelectionChanged" /> event fires.
    /// </description></item>
    ///
    /// <item><description>
    /// The <see cref="GraphMouseUp" /> event fires.
    /// </description></item>
    ///
    /// </list>
    ///
    /// <para>
    /// When this property is set to <see
    /// cref="Visualization.Wpf.MouseMode.DoNothing" />, clicking on a vertex
    /// results in the following sequence:
    /// </para>
    ///
    /// <list type="bullet">
    ///
    /// <item><description>
    /// The <see cref="GraphMouseDown" /> event fires.
    /// </description></item>
    ///
    /// <item><description>
    /// The <see cref="VertexClick" /> event fires.
    /// </description></item>
    ///
    /// <item><description>
    /// The <see cref="GraphMouseUp" /> event fires.
    /// </description></item>
    ///
    /// </list>
    ///
    /// <para>
    /// Set this property to <see
    /// cref="Visualization.Wpf.MouseMode.DoNothing" /> if you want to disable
    /// all mouse interactions, or if you want to customize the click behavior.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    [Category("Behavior")]

    public MouseMode
    MouseMode
    {
        get
        {
            AssertValid();

            return (m_eMouseMode);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the mouse is ignored until graph drawing is
            // complete.

            m_eMouseMode = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MouseAlsoSelectsIncidentEdges
    //
    /// <summary>
    /// Gets or sets a flag specifying whether selecting or deselecting a
    /// vertex with the mouse also selects or deselects its incident edges.
    /// </summary>
    ///
    /// <value>
    /// true if selecting or deselecting a vertex with the mouse also selects
    /// or deselects its incident edges, false if the incident edges are not
    /// affected.
    /// </value>
    ///
    /// <remarks>
    /// See the <see cref="MouseMode" /> property for details on how vertices
    /// can be selected and deselected with the mouse.
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    [Category("Behavior")]

    public Boolean
    MouseAlsoSelectsIncidentEdges
    {
        get
        {
            AssertValid();

            return (m_bMouseAlsoSelectsIncidentEdges);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the mouse is ignored until graph drawing is
            // complete.

            m_bMouseAlsoSelectsIncidentEdges = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AllowVertexDrag
    //
    /// <summary>
    /// Gets or sets a flag indicating whether selected vertices can be moved
    /// by dragging them with the mouse.
    /// </summary>
    ///
    /// <value>
    /// true if selected vertices can be moved by dragging them with the mouse,
    /// false otherwise.  The default value is true.
    /// </value>
    ///
    /// <remarks>
    /// When this property is true and <see cref="MouseMode" /> is set to <see
    /// cref="Visualization.Wpf.MouseMode.Select" /> or <see
    /// cref="Visualization.Wpf.MouseMode.AddToSelection" />, the user
    /// can move the selected vertices by clicking one of them and dragging
    /// them with the mouse.
    ///
    /// <para>
    /// The dragged vertices and their incident edges are redrawn, but no other
    /// vertices or edges are affected.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    [Category("Behavior")]

    public Boolean
    AllowVertexDrag
    {
        get
        {
            AssertValid();

            return (m_bAllowVertexDrag);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the mouse is ignored until graph drawing is
            // complete.

            m_bAllowVertexDrag = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowVertexToolTips
    //
    /// <summary>
    /// Gets or sets a value indicating whether vertex tooltips should be
    /// shown.
    /// </summary>
    ///
    /// <value>
    /// true to show vertex tooltips.  The default value is false.
    /// </value>
    ///
    /// <remarks>
    /// A vertex tooltip is a tootip that appears when the mouse is hovered
    /// over a vertex.  Each vertex has its own tooltip.
    ///
    /// <para>
    /// To use simple text for tooltips, set <see cref="ShowVertexToolTips" />
    /// to true, then use Vertex.<see cref="IMetadataProvider.SetValue" /> to
    /// assign a tooltip string to each of the graph's vertices.  The key must
    /// be the reserved key ReservedMetadataKeys.<see
    /// cref="ReservedMetadataKeys.PerVertexToolTip" /> and the value must be
    /// the tooltip string for the vertex.
    /// </para>
    ///
    /// <para>
    /// To use a custom UIElement for tooltips instead of simple text, set <see
    /// cref="ShowVertexToolTips" /> to true, then handle the <see
    /// cref="PreviewVertexToolTipShown" /> event.  In your event handler, set
    /// the event argument's <see
    /// cref="VertexToolTipShownEventArgs.VertexToolTip" /> to a UIElement that
    /// you create.  You can use the event argument's <see
    /// cref="VertexEventArgs.Vertex" /> property to customize the UIElement
    /// based on which vertex was hovered over.
    /// </para>
    ///
    /// <para>
    /// The <see cref="VertexMouseHover" /> and <see cref="VertexMouseLeave" />
    /// events fires regardless of whether vertex tooltips are shown.
    /// </para>
    ///
    /// <para>
    /// Note that vertex tooltips are entirely independent of the standard
    /// tooltip exposed by FrameworkElement.ToolTip.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="PreviewVertexToolTipShown" />
    /// <seealso cref="VertexToolTipShownEventArgs" />
    //*************************************************************************

    [Category("Mouse")]

    public Boolean
    ShowVertexToolTips
    {
        get
        {
            AssertValid();

            return (m_bShowVertexToolTips);
        }

        set
        {
            // It is okay to change this property while the graph is being laid
            // out, because the mouse is ignored until graph drawing is
            // complete.

            if (!value)
            {
                // Remove any tooltip that might exist and reset the helper
                // object that figures out when to show tooltips.

                ResetVertexToolTipTracker();
            }

            m_bShowVertexToolTips = value;

            AssertValid();
        }
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
    /// Must be between <see cref="MinimumGraphScale" /> and <see
    /// cref="MaximumGraphScale" />.  The default value is 1.0.
    /// </value>
    ///
    /// <remarks>
    /// If the value is anything besides 1.0, the graph's vertices and edges
    /// are shrunk while their positions remain the same.  If it is set to 0.5,
    /// for example, the vertices are half their normal size and the edges are
    /// half their normal width.  The overall size of the graph is not
    /// affected.
    ///
    /// <para>
    /// The <see cref="GraphScaleChanged" /> event fires when this property is
    /// changed.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    [Category("View")]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public Double
    GraphScale
    {
        get
        {
            AssertValid();

            return (m_oGraphDrawer.GraphScale);
        }

        set
        {
            const String PropertyName = "GraphScale";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumGraphScale, MaximumGraphScale);

            m_oGraphDrawer.GraphScale = value;

            DrawGraph(false);

            FireGraphScaleChanged();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GraphZoom
    //
    /// <summary>
    /// Gets or sets a value that determines the zoom level of the graph.
    /// </summary>
    ///
    /// <value>
    /// A value that determines the zoom level of the graph.  Must be between
    /// <see cref="MinimumGraphZoom" /> and <see cref="MaximumGraphZoom" />.
    /// The default value is 1.0.
    /// </value>
    ///
    /// <remarks>
    /// This property gets set automatically when the user spins the mouse
    /// wheel.
    ///
    /// <para>
    /// The <see cref="GraphZoomChanged" /> event fires when this property is
    /// changed, either programatically or with the mouse wheel.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    [Category("View")]
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public Double
    GraphZoom
    {
        get
        {
            AssertValid();

            ScaleTransform oScaleTransformForRender =
                this.ScaleTransformForRender;

            Debug.Assert(oScaleTransformForRender.ScaleX ==
                oScaleTransformForRender.ScaleY);

            return (oScaleTransformForRender.ScaleX);
        }

        set
        {
            const String PropertyName = "GraphZoom";

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                MinimumGraphZoom, MaximumGraphZoom);

            SetGraphZoom(value, true);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedVertices
    //
    /// <summary>
    /// Gets a collection of the graph's selected vertices.
    /// </summary>
    ///
    /// <value>
    /// A collection of the graph's selected vertices.
    /// </value>
    ///
    /// <remarks>
    /// If there are no selected vertices, the returned collection has zero
    /// elements.  The returned value is never null.
    ///
    /// <para>
    /// The returned collection should be considered read-only.  To select a
    /// vertex, use <see cref="SetVertexSelected" /> or a related method.
    /// </para>
    ///
    /// <para>
    /// The returned collection includes vertices that represents groups that
    /// have been collapsed using <see cref="CollapseGroup" />.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public ICollection<IVertex>
    SelectedVertices
    {
        get
        {
            AssertValid();

            // Make a copy of the collection.  This allows the caller to pass
            // the collection back to SetSelected(), for example, which clears
            // m_oSelectedVertices before selecting the specified edges and
            // vertices.

            return ( m_oSelectedVertices.ToArray() );
        }
    }

    //*************************************************************************
    //  Property: SelectedEdges
    //
    /// <summary>
    /// Gets a collection of the graph's selected edges.
    /// </summary>
    ///
    /// <value>
    /// A collection of the graph's selected edges.
    /// </value>
    ///
    /// <remarks>
    /// If there are no selected edges, the returned collection has zero
    /// elements.  The returned value is never null.
    ///
    /// <para>
    /// The returned collection should be considered read-only.  To select an
    /// edge, use <see cref="SetEdgeSelected" /> or a related method.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.ReadOnly(true)]

    public ICollection<IEdge>
    SelectedEdges
    {
        get
        {
            AssertValid();

            // Make a copy of the collection.  This allows the caller to pass
            // the collection back to SetSelected(), for example, which clears
            // m_oSelectedEdges before selecting the specified edges and
            // vertices.

            return ( m_oSelectedEdges.ToArray() );
        }
    }

    //*************************************************************************
    //  Property: IsLayingOutGraph
    //
    /// <summary>
    /// Gets a value indicating whether the graph is being laid out.
    /// </summary>
    ///
    /// <value>
    /// true if the graph is being laid out.
    /// </value>
    ///
    /// <remarks>
    /// If you call <see cref="DrawGraph(Boolean)" /> with a layOutGraphFirst
    /// argument of true, the graph is laid out asynchronously before being
    /// drawn.  Several properties and methods, such as <see cref="Graph" />
    /// and <see cref="Layout" />, cannot be accessed while the graph is being
    /// laid out.  Check <see cref="IsLayingOutGraph" /> or monitor the <see
    /// cref="LayingOutGraph" /> and <see cref="GraphLaidOut" /> events before
    /// accessing those properties and methods.
    ///
    /// <para>
    /// The <see cref="LayingOutGraph" /> event fires before the graph layout
    /// begins.  The <see cref="GraphLaidOut" /> event fires after the graph
    /// layout completes.
    /// </para>
    ///
    /// <para>
    /// Typically, an application will populate and draw the graph in the load
    /// event of the Window or Form, and use the <see cref="LayingOutGraph" />
    /// and <see cref="GraphLaidOut" /> events to disable and enable any
    /// controls that might be used to lay out and draw the graph again.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsLayingOutGraph
    {
        get
        {
            AssertValid();

            return (m_eLayoutState != LayoutState.Stable);
        }
    }

    //*************************************************************************
    //  Property: GraphDrawer
    //
    /// <summary>
    /// Do not use this property.  It is for internal use only.
    /// </summary>
    ///
    /// <value>
    /// Do not use this property.  It is for internal use only.
    /// </value>
    //*************************************************************************

    public GraphDrawer
    GraphDrawer
    {
        // This is for use by
        // ExcelTemplate.GeneralUserSettings.TransferToNodeXLControl().

        get
        {
            AssertValid();

            return (m_oGraphDrawer);
        }
    }

    //*************************************************************************
    //  Property: CollapsedGroups()
    //
    /// <summary>
    /// Gets the collapsed groups.
    /// </summary>
    ///
    /// <returns>
    /// A read-only Dictionary of the groups that are collapsed.  The key is
    /// the group name that was passed to <see cref="CollapseGroup" /> and the
    /// value is the vertex that represents the collapsed group.
    /// </returns>
    ///
    /// <remarks>
    /// The Dictionary must not be modified.  To collapse a group, call <see
    /// cref="CollapseGroup" />.
    /// </remarks>
    ///
    /// <seealso cref="CollapseGroup" />
    /// <seealso cref="ExpandGroup" />
    /// <seealso cref="IsCollapsedGroup" />
    /// <seealso cref="SelectCollapsedGroup" />
    //*************************************************************************

    public Dictionary<String, IVertex>
    CollapsedGroups
    {
        get
        {
            AssertValid();

            return (m_oCollapsedGroups);
        }
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
        AssertValid();

        // It is okay to change the font while the graph is being laid out,
        // because the font is not used until OnRender() is called.

        this.VertexDrawer.SetFont(typeface, fontSize);
    }

    //*************************************************************************
    //  Method: SetVertexSelected()
    //
    /// <summary>
    /// Selects or deselects a vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// Vertex to select or deselect.  Can't be null.
    /// </param>
    ///
    /// <param name="selected">
    /// true to select <paramref name="vertex" />, false to deselect it.
    /// </param>
    ///
    /// <param name="alsoIncidentEdges">
    /// true to also select or deselect the vertex's incident edges, false to
    /// leave the incident edges alone.
    /// </param>
    ///
    /// <remarks>
    /// Selecting or deselecting a vertex does not affect the selected state of
    /// the other vertices.
    ///
    /// <para>
    /// To select a set of vertices and edges, use the <see
    /// cref="SetSelected" /> method instead.
    /// </para>
    ///
    /// <para>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </para>
    ///
    /// <para>
    /// If the vertex is hidden (meaning it has a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Hidden" />), this method will not select the
    /// vertex.
    /// </para>
    ///
    /// <para>
    /// If the vertex is filtered (meaning it has a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Filtered" /> and <see cref="FilteredAlpha" />
    /// is zero, this method will not select the vertex.
    /// </para>
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    public void
    SetVertexSelected
    (
        IVertex vertex,
        Boolean selected,
        Boolean alsoIncidentEdges
    )
    {
        AssertValid();

        const String MethodName = "SetVertexSelected";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertex", vertex);

        // The reason that vertices and edges can't be selected while the graph
        // is being laid out is that selecting them involves modifying their
        // metadata.  The graph layout code, which runs on a worker thread, is
        // allowed to modify metadata during the layout, so having the worker
        // thread and this foreground thread modifying metadata simultaneously
        // would lead to synchronization clashes.
        //
        // A solution would be to make the metadata implementation thread-safe.

        CheckIfLayingOutGraph(MethodName);

        // Update the selected state of the vertex and its incident edges.

        SetVertexSelectedInternal(vertex, selected);

        if (alsoIncidentEdges)
        {
            foreach (IEdge oEdge in vertex.IncidentEdges)
            {
                SetEdgeSelectedInternal(oEdge, selected);
            }
        }

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: SetEdgeSelected()
    //
    /// <summary>
    /// Selects or deselects an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// Edge to select or deselect.  Can't be null.
    /// </param>
    ///
    /// <param name="selected">
    /// true to select <paramref name="edge" />, false to deselect it.
    /// </param>
    ///
    /// <param name="alsoAdjacentVertices">
    /// true to also select or deselect the edge's adjacent vertices, false to
    /// leave the adjacent vertices alone.
    /// </param>
    ///
    /// <remarks>
    /// Selecting or deselecting an edge does not affect the selected state of
    /// the other edges.
    ///
    /// <para>
    /// To select a set of vertices and edges, use the <see
    /// cref="SetSelected" /> method instead.
    /// </para>
    ///
    /// <para>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </para>
    ///
    /// <para>
    /// If the edge is hidden (meaning it has a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Hidden" />, this method will not select the
    /// edge.
    /// </para>
    ///
    /// <para>
    /// If the edge is filtered (meaning it has a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Filtered" />) and <see cref="FilteredAlpha" />
    /// is zero, this method will not select the edge.
    /// </para>
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    public void
    SetEdgeSelected
    (
        IEdge edge,
        Boolean selected,
        Boolean alsoAdjacentVertices
    )
    {
        AssertValid();

        const String MethodName = "SetEdgeSelected";

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "edge", edge);
        CheckIfLayingOutGraph(MethodName);

        // Update the selected state of the edge and its adjacent vertices.

        SetEdgeSelectedInternal(edge, selected);

        if (alsoAdjacentVertices)
        {
            SetVertexSelectedInternal(edge.Vertices[0], selected);
            SetVertexSelectedInternal(edge.Vertices[1], selected);
        }

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: SetSelected()
    //
    /// <summary>
    /// Selects a set of vertices and edges.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Collection of zero or more vertices to select.
    /// </param>
    ///
    /// <param name="edges">
    /// Collection of zero or more edges to select.
    /// </param>
    ///
    /// <remarks>
    /// This method deselects any selected vertices and edges, then selects the
    /// vertices and edges specified in <paramref name="vertices" /> and
    /// <paramref name="edges" />.  It is more efficient than making multiple
    /// calls to <see cref="SetVertexSelected" /> and <see
    /// cref="SetEdgeSelected" />.
    ///
    /// <para>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </para>
    ///
    /// <para>
    /// Hidden vertices and edges (those that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Hidden" />) do not get selected.
    /// </para>
    ///
    /// <para>
    /// Filtered vertices and edges (those that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Filtered" />) do not get selected if <see
    /// cref="FilteredAlpha" /> is zero.
    /// </para>
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    public void
    SetSelected
    (
        IEnumerable<IVertex> vertices,
        IEnumerable<IEdge> edges
    )
    {
        AssertValid();

        const String MethodName = "SetSelected";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertices", vertices);

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "edges", edges);

        CheckIfLayingOutGraph(MethodName);

        // Clear the selection.

        SetAllVerticesSelected(false);
        SetAllEdgesSelected(false);

        // Update the selected state of the specified vertices and edges.

        foreach (IVertex oVertex in vertices)
        {
            SetVertexSelectedInternal(oVertex, true);
        }

        foreach (IEdge oEdge in edges)
        {
            SetEdgeSelectedInternal(oEdge, true);
        }

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: SelectAll()
    //
    /// <summary>
    /// Selects all vertices and edges.
    /// </summary>
    ///
    /// <remarks>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
    /// <para>
    /// Hidden vertices and edges (those that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Hidden" />) do not get selected.
    /// </para>
    ///
    /// <para>
    /// Filtered vertices and edges (those that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Filtered" />) do not get selected if <see
    /// cref="FilteredAlpha" /> is zero.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    public void
    SelectAll()
    {
        AssertValid();

        const String MethodName = "SelectAll";

        CheckIfLayingOutGraph(MethodName);

        SetAllSelected(true);
    }

    //*************************************************************************
    //  Method: DeselectAll()
    //
    /// <summary>
    /// Deselects all vertices and edges.
    /// </summary>
    ///
    /// <remarks>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    public void
    DeselectAll()
    {
        AssertValid();

        const String MethodName = "DeselectAll";

        CheckIfLayingOutGraph(MethodName);

        // Do nothing if nothing is selected.

        if (m_oSelectedVertices.Count == 0 && m_oSelectedEdges.Count == 0)
        {
            return;
        }

        SetAllSelected(false);
    }

    //*************************************************************************
    //  Method: InvertSelection()
    //
    /// <summary>
    /// Inverts the selection.
    /// </summary>
    ///
    /// <remarks>
    /// This method deselects all selected vertices and edges, and selects all
    /// unselected vertices and edges.
    ///
    /// <para>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </para>
    ///
    /// <para>
    /// <b>Important Note:</b>
    /// </para>
    ///
    /// <para>
    /// Do not use the <see cref="ReservedMetadataKeys.IsSelected" /> key to
    /// select vertex or edges.  Use the selection methods on this control
    /// instead.
    /// </para>
    ///
    /// <para>
    /// Hidden vertices and edges (those that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Hidden" />) do not get selected.
    /// </para>
    ///
    /// <para>
    /// Filtered vertices and edges (those that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> key value of <see
    /// cref="VisibilityKeyValue.Filtered" />) do not get selected if <see
    /// cref="FilteredAlpha" /> is zero.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    /// <seealso cref="SelectionChanged" />
    //*************************************************************************

    public void
    InvertSelection()
    {
        AssertValid();

        const String MethodName = "InvertSelection";

        CheckIfLayingOutGraph(MethodName);

        foreach (IVertex oVertex in m_oGraph.Vertices)
        {
            SetVertexSelectedInternal( oVertex,
                !VertexOrEdgeIsSelected(oVertex) );
        }

        foreach (IEdge oEdge in m_oGraph.Edges)
        {
            SetEdgeSelectedInternal( oEdge, !VertexOrEdgeIsSelected(oEdge) );
        }

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: VertexOrEdgeIsSelected()
    //
    /// <summary>
    /// Returns a flag indicating whether a vertex or edge is selected.
    /// </summary>
    ///
    /// <param name="vertexOrEdge">
    /// Vertex or edge to check, as an <see cref="IMetadataProvider" />.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="vertexOrEdge" /> is selected.
    /// </returns>
    //*************************************************************************

    public Boolean
    VertexOrEdgeIsSelected
    (
        IMetadataProvider vertexOrEdge
    )
    {
        Debug.Assert(vertexOrEdge != null);

        return ( vertexOrEdge.ContainsKey(ReservedMetadataKeys.IsSelected) );
    }

    //*************************************************************************
    //  Method: TryGetVertexFromPoint()
    //
    /// <summary>
    /// Attempts to get the vertex containing a specified <see cref="Point" />.
    /// </summary>
    ///
    /// <param name="point">
    /// Point to get a vertex for.
    /// </param>
    ///
    /// <param name="vertex">
    /// Where the <see cref="IVertex" /> object gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex containing the point was found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method looks for a vertex that contains <paramref name="point" />.
    /// If there is such a vertex, the vertex is stored at <paramref
    /// name="vertex" /> and true is returned.  Otherwise, <paramref
    /// name="vertex" /> is set to null and false is returned.
    ///
    /// <para>
    /// false is returned if an asynchronous drawing is in progress.  Check
    /// <see cref="IsLayingOutGraph" /> before calling this method.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetVertexFromPoint
    (
        Point point,
        out IVertex vertex
    )
    {
        AssertValid();

        vertex = null;

        if (this.IsLayingOutGraph)
        {
            return false;
        }

        return ( m_oGraphDrawer.TryGetVertexFromPoint(point, out vertex) );
    }

    //*************************************************************************
    //  Method: CollapseGroup()
    //
    /// <summary>
    /// Collapses a group of vertices.
    /// </summary>
    ///
    /// <param name="groupName">
    /// The name of the group to collapse.  This must be the name of a group
    /// that was stored in the graph's metadata within the <see
    /// cref="ReservedMetadataKeys.GroupInfo" /> key.
    /// </param>
    ///
    /// <param name="redrawGroupImmediately">
    /// true to redraw the collapsed group immediately.  If false, the
    /// collapsed group won't be redrawn until <see cref="DrawGraph()"/> is
    /// called.
    /// </param>
    ///
    /// <remarks>
    /// This method replaces the group of vertices specified by <paramref
    /// name="groupName" /> with a new "group vertex."  A group vertex has the
    /// color and shape of the group's first vertex, and its size is determined
    /// by the number of vertices in the group.  It has a plus sign in its
    /// center to distinguish it from regular vertices.
    ///
    /// <para>
    /// Internal edges that connected the group's vertices to each other are
    /// removed, and external edges that connected the group's vertices to
    /// vertices outside the group are reconnected to the new group vertex.
    /// </para>
    ///
    /// <para>
    /// Call <see cref="ExpandGroup" /> to expand the group again.
    /// </para>
    ///
    /// <para>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </para>
    ///
    /// <para>
    /// If the graph's metadata does not contain the <see
    /// cref="ReservedMetadataKeys.GroupInfo" /> key, or it does contain the
    /// key but there is not a unique group with the specified name, this
    /// method does nothing.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="ExpandGroup" />
    /// <seealso cref="IsCollapsedGroup" />
    /// <seealso cref="SelectCollapsedGroup" />
    /// <seealso cref="CollapsedGroups" />
    //*************************************************************************

    public void
    CollapseGroup
    (
        String groupName,
        Boolean redrawGroupImmediately
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(groupName) );
        AssertValid();

        CheckIfLayingOutGraph("CollapseGroup");

        IGraph oGraph = this.Graph;

        if ( !GroupUtil.GraphHasGroups(oGraph) )
        {
            return;
        }

        GroupInfo oGroupToCollapse;

        try
        {
            oGroupToCollapse = GroupUtil.GetGroups(oGraph).Single(
                oGroupInfo => oGroupInfo.Name == groupName);
        }
        catch (InvalidOperationException)
        {
            // There is no such group, or there are two or more such groups.

            return;
        }

        CollapseGroupInternal(oGroupToCollapse, redrawGroupImmediately);
    }

    //*************************************************************************
    //  Method: ExpandGroup()
    //
    /// <summary>
    /// Expands a group of collapsed vertices.
    /// </summary>
    ///
    /// <param name="groupName">
    /// The name of the group.  This must be a name that was previously passed
    /// to <see cref="CollapseGroup" />.
    /// </param>
    ///
    /// <param name="redrawGroupImmediately">
    /// true to redraw the expanded group immediately.  If false, the expanded
    /// group won't be redrawn until <see cref="DrawGraph()"/> is called.
    /// </param>
    ///
    /// <remarks>
    /// This method restores the group of vertices that were collapsed with
    /// <see cref="CollapseGroup" />.
    ///
    /// <para>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="CollapseGroup" />
    /// <seealso cref="IsCollapsedGroup" />
    /// <seealso cref="SelectCollapsedGroup" />
    /// <seealso cref="CollapsedGroups" />
    //*************************************************************************

    public void
    ExpandGroup
    (
        String groupName,
        Boolean redrawGroupImmediately
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(groupName) );
        AssertValid();

        CheckIfLayingOutGraph("ExpandGroup");

        IVertex oCollapsedGroupVertex;

        if ( !m_oCollapsedGroups.TryGetValue(groupName,
            out oCollapsedGroupVertex) )
        {
            return;
        }

        Boolean bCollapsedGroupVertexWasSelected =
            VertexOrEdgeIsSelected(oCollapsedGroupVertex);

        // Retrieve the collapsed vertices and collapsed internal edges that
        // were stored on the group vertex by CollapseGroup().

        ICollection<IVertex> oCollapsedVertices =
            ( ICollection<IVertex> )oCollapsedGroupVertex.GetRequiredValue(
                ReservedMetadataKeys.CollapsedVertices,
                typeof( ICollection<IVertex> ) );

        ICollection<IEdge> oCollapsedInternalEdges =
            ( ICollection<IEdge> )oCollapsedGroupVertex.GetRequiredValue(
                ReservedMetadataKeys.CollapsedInternalEdges,
                typeof( ICollection<IEdge> ) );

        // Restore the collapsed vertices and internal edges.
        //
        // The selection state of the collapsed vertices and internal edges
        // should be the same as that of the collapsed group vertex.

        Random oRandom = new Random();

        foreach (IVertex oCollapsedVertex in oCollapsedVertices)
        {
            MarkVertexOrEdgeAsSelected(oCollapsedVertex,
                bCollapsedGroupVertexWasSelected);

            AddVertexDuringGroupExpand(oCollapsedVertex,
                redrawGroupImmediately, oRandom);
        }

        foreach (IEdge oCollapsedInternalEdge in oCollapsedInternalEdges)
        {
            MarkVertexOrEdgeAsSelected(oCollapsedInternalEdge,
                bCollapsedGroupVertexWasSelected);

            AddEdgeDuringGroupCollapseOrExpand(oCollapsedInternalEdge,
                redrawGroupImmediately);
        }

        Boolean bGraphIsDirected =
            (m_oGraph.Directedness == GraphDirectedness.Directed);

        // Remove the group vertex's incident edges.

        foreach (IEdge oIncidentEdge in oCollapsedGroupVertex.IncidentEdges)
        {
            IVertex oAdjacentVertex = oIncidentEdge.GetAdjacentVertex(
                oCollapsedGroupVertex);

            IEdge oExternalEdgeClone = null;
            Object oOriginalVertexAsObject;

            if ( oIncidentEdge.TryGetValue(
                ReservedMetadataKeys.OriginalVertex2InEdge + groupName,
                typeof(IVertex), out oOriginalVertexAsObject) )
            {
                IVertex oOriginalVertex = (IVertex)oOriginalVertexAsObject;

                // The incident edge is a clone of an original external edge.
                // Clone it again, this time connecting it back to its original
                // vertex.

                oExternalEdgeClone = oIncidentEdge.Clone(true, true,
                    oAdjacentVertex, oOriginalVertex, bGraphIsDirected);

                oExternalEdgeClone.RemoveKey(
                    ReservedMetadataKeys.OriginalVertex2InEdge + groupName);
            }
            else if ( oIncidentEdge.TryGetValue(
                ReservedMetadataKeys.OriginalVertex1InEdge + groupName,
                typeof(IVertex), out oOriginalVertexAsObject) )
            {
                IVertex oOriginalVertex = (IVertex)oOriginalVertexAsObject;

                oExternalEdgeClone = oIncidentEdge.Clone(true, true,
                    oOriginalVertex, oAdjacentVertex, bGraphIsDirected);

                oExternalEdgeClone.RemoveKey(
                    ReservedMetadataKeys.OriginalVertex1InEdge + groupName);
            }

            RemoveEdgeDuringGroupCollapseOrExpand(oIncidentEdge,
                redrawGroupImmediately);

            if (oExternalEdgeClone != null)
            {
                CollapsedGroupDrawingManager.RestoreExternalEdge(
                    oExternalEdgeClone);

                MarkVertexOrEdgeAsSelected(oExternalEdgeClone,
                    bCollapsedGroupVertexWasSelected);

                AddEdgeDuringGroupCollapseOrExpand(oExternalEdgeClone,
                    redrawGroupImmediately);
            }
        }

        // Remove the group vertex.

        RemoveVertexDuringGroupCollapseOrExpand(oCollapsedGroupVertex,
            redrawGroupImmediately);

        m_oCollapsedGroups.Remove(groupName);
    }

    //*************************************************************************
    //  Method: IsCollapsedGroup()
    //
    /// <summary>
    /// Determines whether a group is collapsed.
    /// </summary>
    ///
    /// <param name="groupName">
    /// The name of the group to check.
    /// </param>
    ///
    /// <returns>
    /// true if the specified group is collapsed.
    /// </returns>
    ///
    /// <remarks>
    /// This method returns true if the group with the name <paramref
    /// name="groupName" /> has been collapsed by <see cref="CollapseGroup" />.
    /// </remarks>
    ///
    /// <seealso cref="CollapseGroup" />
    /// <seealso cref="ExpandGroup" />
    /// <seealso cref="SelectCollapsedGroup" />
    /// <seealso cref="CollapsedGroups" />
    //*************************************************************************

    public Boolean
    IsCollapsedGroup
    (
        String groupName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(groupName) );
        AssertValid();

        return ( m_oCollapsedGroups.ContainsKey(groupName) );
    }

    //*************************************************************************
    //  Method: SelectCollapsedGroup()
    //
    /// <summary>
    /// Selects the vertex that represents a collapsed group.
    /// </summary>
    ///
    /// <param name="groupName">
    /// The name of the group to select.  This must be a name that was
    /// previously passed to <see cref="CollapseGroup" />.
    /// </param>
    ///
    /// <seealso cref="CollapseGroup" />
    /// <seealso cref="ExpandGroup" />
    /// <seealso cref="IsCollapsedGroup" />
    /// <seealso cref="CollapsedGroups" />
    //*************************************************************************

    public void
    SelectCollapsedGroup
    (
        String groupName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(groupName) );
        AssertValid();

        IVertex oGroupVertex;

        if ( m_oCollapsedGroups.TryGetValue(groupName, out oGroupVertex) )
        {
            SetVertexSelectedInternal(oGroupVertex, true);
        }
    }

    //*************************************************************************
    //  Method: CopyGraphToBitmap()
    //
    /// <summary>
    /// Creates a bitmap image of the graph.
    /// </summary>
    ///
    /// <param name="bitmapWidthPx">
    /// Width of the bitmap image, in pixels.  Must be greater than 0.
    /// </param>
    ///
    /// <param name="bitmapHeightPx">
    /// Height of the bitmap image, in pixels.  Must be greater than 0.
    /// </param>
    ///
    /// <returns>
    /// A bitmap image of the graph displayed within the control, with the
    /// specified dimensions.
    /// </returns>
    ///
    /// <remarks>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </remarks>
    //*************************************************************************

    public System.Drawing.Bitmap
    CopyGraphToBitmap
    (
        Int32 bitmapWidthPx,
        Int32 bitmapHeightPx
    )
    {
        const String MethodName = "CopyGraphToBitmap";

        this.ArgumentChecker.CheckArgumentPositive(MethodName, "bitmapWidthPx",
            bitmapWidthPx);

        this.ArgumentChecker.CheckArgumentPositive(MethodName, "bitmapHeightPx",
            bitmapHeightPx);

        CheckIfLayingOutGraph(MethodName);

        // Save the current vertex locations.

        LayoutSaver oLayoutSaver = new LayoutSaver(this.Graph);

        // Adjust the control's graph scale so that the graph's vertices and
        // edges will be the same relative size in the image that they are in
        // the control.

        GraphImageScaler oGraphImageScaler = new GraphImageScaler(this);

        oGraphImageScaler.SetGraphScale(bitmapWidthPx, bitmapHeightPx,
            WpfGraphicsUtil.GetScreenDpi(this).Width);

        // Adjust the control's transforms so that the image will be centered
        // on the same point on the graph that the control is centered on.

        GraphImageCenterer oGraphImageCenterer = new GraphImageCenterer(this);

        oGraphImageCenterer.CenterGraphImage(
            new Size(bitmapWidthPx, bitmapHeightPx) );

        // Transform the graph's layout to the specified size.

        Double dOriginalActualWidth = this.ActualWidth;
        Double dOriginalActualHeight = this.ActualHeight;

        Rect oBitmapRectangle = new Rect(0, 0,
            (Double)bitmapWidthPx, (Double)bitmapHeightPx);

        TransformLayout(oBitmapRectangle);

        Debug.Assert(m_eLayoutState == LayoutState.Stable);

        DrawGraph(oBitmapRectangle);

        System.Drawing.Bitmap oBitmap = WpfGraphicsUtil.VisualToBitmap(this,
            bitmapWidthPx, bitmapHeightPx);

        // Restore the original layout.
        //
        // NOTE:
        //
        // Don't try calling TransformLayout() again using the original
        // rectangle.  The first call to TransformLayout() lost "resolution" if
        // the layout was transformed to a smaller rectangle, and attempting to
        // reverse the transform will yield poor results.

        oLayoutSaver.RestoreLayout();

        oGraphImageScaler.RestoreGraphScale();

        oBitmapRectangle =
            new Rect(0, 0, dOriginalActualWidth, dOriginalActualHeight);

        Debug.Assert(m_eLayoutState == LayoutState.Stable);

        DrawGraph(oBitmapRectangle);

        oGraphImageCenterer.RestoreCenter();

        return (oBitmap);
    }

    //*************************************************************************
    //  Method: DrawGraph()
    //
    /// <overloads>
    /// Draws the graph.
    /// </overloads>
    ///
    /// <summary>
    /// Draws the graph without laying it out first.
    /// </summary>
    //*************************************************************************

    public void
    DrawGraph()
    {
        AssertValid();

        DrawGraph(false);
    }

    //*************************************************************************
    //  Method: DrawGraph()
    //
    /// <summary>
    /// Draws the graph after optionally laying it out.
    /// </summary>
    ///
    /// <param name="layOutGraphFirst">
    /// If true, the graph is laid out again before it is drawn.  If false, the
    /// graph is drawn using the current vertex locations.
    /// </param>
    ///
    /// <remarks>
    /// Graph layout occurs asynchronously after this method is called with a
    /// <paramref name="layOutGraphFirst" /> argument of true.  See the <see
    /// cref="IsLayingOutGraph" /> property for details.
    ///
    /// <para>
    /// If the graph is currently being laid out, this method does nothing.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    DrawGraph
    (
        Boolean layOutGraphFirst
    )
    {
        AssertValid();

        if (this.IsLayingOutGraph)
        {
            return;
        }

        if (layOutGraphFirst)
        {
            m_eLayoutState = LayoutState.LayoutRequired;

            // Don't just call LayOutOrDrawGraph() here, as is done in the else
            // clause.  Assuming that this method is being called at
            // application startup time, the control has not yet gone through
            // its measure cycle.  If LayOutOrDrawGraph() were called instead
            // of InvalidateVisual(), the actual width and height of this
            // control would be zero when the layout begins, and the graph
            // wouldn't get laid out properly.

            this.InvalidateVisual();
        }
        else
        {
            m_eLayoutState = LayoutState.LayoutCompleted;
            LayOutOrDrawGraph();
        }
    }

    //*************************************************************************
    //  Method: ClearGraph()
    //
    /// <summary>
    /// Clears the control's graph.
    /// </summary>
    ///
    /// <remarks>
    /// This method discards the control's graph, including all of its vertices
    /// and edges, and replaces it with a new, empty graph.  Any selection is
    /// also cleared.
    ///
    /// <para>
    /// This should be used instead of clearing the current graph's vertex and
    /// edge collections, which can cause unpredictable side effects.
    /// </para>
    ///
    /// <para>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ClearGraph()
    {
        AssertValid();

        this.Graph = new Graph();
    }

    //*************************************************************************
    //  Method: SnapVerticesToGrid()
    //
    /// <summary>
    /// Snaps the graph's vertices to a grid.
    /// </summary>
    ///
    /// <param name="gridSize">
    /// Distance between gridlines, in WPF units.  Must be greater than zero.
    /// </param>
    ///
    /// <remarks>
    /// This method can be used to separate vertices that obscure each other
    /// by snapping them to the nearest grid location, using a specified grid
    /// size.  The graph should be laid out before this method is called, and
    /// it must be drawn again using <see cref="DrawGraph()" /> to get the
    /// vertices to appear in their new locations.
    ///
    /// <para>
    /// An exception is thrown if the graph is being laid out when this method
    /// is called.  Check the <see cref="IsLayingOutGraph" /> property before
    /// calling this.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    SnapVerticesToGrid
    (
        Int32 gridSize
    )
    {
        AssertValid();

        const String MethodName = "SnapVerticesToGrid";

        this.ArgumentChecker.CheckArgumentPositive(MethodName, "gridSize",
            gridSize);

        CheckIfLayingOutGraph(MethodName);

        VertexGridSnapper.SnapVerticesToGrid(this.Graph, gridSize);
        BundleAllEdgesIfAppropriate();
    }

    //*************************************************************************
    //  Event: SelectionChanged
    //
    /// <summary>
    /// Occurs when the selection state of a vertex or edge changes.
    /// </summary>
    ///
    /// <remarks>
    /// This event occurs when one or more of the graph's vertices or edges are
    /// selected or deselected.  Updated arrays of the graph's selected
    /// vertices and edges can be obtained from the <see
    /// cref="SelectedVertices" /> and <see cref="SelectedEdges" /> properties.
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    //*************************************************************************

    [Category("Property Changed")]

    public event EventHandler SelectionChanged;


    //*************************************************************************
    //  Event: GraphMouseDown
    //
    /// <summary>
    /// Occurs when the mouse pointer is within the graph and a mouse button
    /// is pressed.
    /// </summary>
    ///
    /// <remarks>
    /// See <see cref="MouseMode" /> and <see
    /// cref="MouseAlsoSelectsIncidentEdges" /> for details on how vertices are
    /// selected with the mouse.
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SetVertexSelected" />
    /// <seealso cref="SetEdgeSelected" />
    /// <seealso cref="SetSelected" />
    /// <seealso cref="SelectAll" />
    /// <seealso cref="DeselectAll" />
    /// <seealso cref="InvertSelection" />
    /// <seealso cref="VertexOrEdgeIsSelected" />
    /// <seealso cref="SelectedVertices" />
    /// <seealso cref="SelectedEdges" />
    //*************************************************************************

    [Category("Mouse")]

    public event GraphMouseButtonEventHandler GraphMouseDown;


    //*************************************************************************
    //  Event: GraphMouseUp
    //
    /// <summary>
    /// Occurs when the mouse pointer is within the graph and a mouse button
    /// is released.
    /// </summary>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SelectedVertices" />
    //*************************************************************************

    [Category("Mouse")]

    public event GraphMouseButtonEventHandler GraphMouseUp;


    //*************************************************************************
    //  Event: VertexClick
    //
    /// <summary>
    /// Occurs when a vertex is clicked.
    /// </summary>
    ///
    /// <remarks>
    /// In your event handler, do not change the selected state of the clicked
    /// vertex.  That happens automatically.  An exception is thrown if you
    /// attempt to do this.
    /// </remarks>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SelectedVertices" />
    //*************************************************************************

    [Category("Mouse")]

    public event VertexEventHandler VertexClick;


    //*************************************************************************
    //  Event: VertexDoubleClick
    //
    /// <summary>
    /// Occurs when a vertex is double-clicked.
    /// </summary>
    ///
    /// <seealso cref="MouseMode" />
    /// <seealso cref="MouseAlsoSelectsIncidentEdges" />
    /// <seealso cref="SelectedVertices" />
    //*************************************************************************

    [Category("Mouse")]

    public event VertexEventHandler VertexDoubleClick;


    //*************************************************************************
    //  Event: VertexMouseHover
    //
    /// <summary>
    /// Occurs when the mouse pointer hovers over a vertex.
    /// </summary>
    ///
    /// <remarks>
    /// This event occurs when the mouse pointer hovers over a vertex.  If the
    /// mouse is moved to another vertex, this event fires again.  If the mouse
    /// is left hovering over the vertex for a predetermined period or is moved
    /// away from the vertex, a <see cref="VertexMouseLeave" /> event occurs.
    ///
    /// <para>
    /// If <see cref="ShowVertexToolTips" /> is true, hovering the mouse over a
    /// vertex causes the <see cref="VertexMouseHover" /> event to fire,
    /// followed by <see cref="PreviewVertexToolTipShown" />.  If <see
    /// cref="ShowVertexToolTips" /> is false, only the <see
    /// cref="VertexMouseHover" /> event fires.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="VertexMouseLeave" />
    //*************************************************************************

    [Category("Mouse")]

    public event VertexEventHandler VertexMouseHover;


    //*************************************************************************
    //  Event: VertexMouseLeave
    //
    /// <summary>
    /// Occurs when the mouse pointer is left hovering over a vertex for a
    /// predetermined period or is moved away from all vertices.
    /// </summary>
    ///
    /// <remarks>
    /// Hovering the mouse over a vertex causes the <see
    /// cref="VertexMouseHover" /> event to fire.  If the mouse is left
    /// hovering over the vertex for a predetermined period or is moved away
    /// from the vertex, the <see cref="VertexMouseLeave" /> event fires.
    /// </remarks>
    ///
    /// <seealso cref="VertexMouseHover" />
    //*************************************************************************

    [Category("Mouse")]

    public event EventHandler VertexMouseLeave;


    //*************************************************************************
    //  Event: PreviewVertexToolTipShown
    //
    /// <summary>
    /// Occurs when the mouse pointer hovers over a vertex but before a vertex
    /// tooltip is shown.
    /// </summary>
    ///
    /// <remarks>
    /// See <see cref="ShowVertexToolTips" /> for information on how to show
    /// and customize vertex tooltips.
    /// </remarks>
    //*************************************************************************

    [Category("Mouse")]

    public event VertexToolTipShownEventHandler PreviewVertexToolTipShown;


    //*************************************************************************
    //  Event: GraphZoomChanged
    //
    /// <summary>
    /// Occurs when the <see cref="GraphZoom" /> property is changed.
    /// </summary>
    ///
    /// <remarks>
    /// This event occurs when <see cref="GraphZoom" /> is changed
    /// programatically or with the mouse wheel.
    /// </remarks>
    //*************************************************************************

    [Category("Mouse")]

    public event EventHandler GraphZoomChanged;


    //*************************************************************************
    //  Event: GraphScaleChanged
    //
    /// <summary>
    /// Occurs when the <see cref="GraphScale" /> property is changed.
    /// </summary>
    ///
    /// <remarks>
    /// This event occurs when <see cref="GraphScale" /> is changed.
    /// </remarks>
    //*************************************************************************

    [Category("Mouse")]

    public event EventHandler GraphScaleChanged;


    //*************************************************************************
    //  Event: GraphTranslationChanged
    //
    /// <summary>
    /// Occurs when the graph is moved with the mouse.
    /// </summary>
    //*************************************************************************

    [Category("Mouse")]

    public event EventHandler GraphTranslationChanged;


    //*************************************************************************
    //  Event: LayingOutGraph
    //
    /// <summary>
    /// Occurs before graph layout begins.
    /// </summary>
    ///
    /// <remarks>
    /// Graph layout occurs asynchronously.  This event fires before graph
    /// layout begins.
    ///
    /// <para>
    /// The <see cref="GraphLaidOut" /> event fires after the graph layout is
    /// complete.
    /// </para>
    ///
    /// <para>
    /// The <see cref="IsLayingOutGraph" /> property can also be used to
    /// determine whether the graph is being laid out.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    [Category("Action")]

    public event EventHandler LayingOutGraph;


    //*************************************************************************
    //  Event: GraphLaidOut
    //
    /// <summary>
    /// Occurs after graph layout completes.
    /// </summary>
    ///
    /// <remarks>
    /// Graph layout occurs asynchronously.  This event fires when the graph
    /// is successfully laid out or an error occurs.
    ///
    /// <para>
    /// Check the <see cref="AsyncCompletedEventArgs.Error" /> property to
    /// determine whether an error occurred while laying out the graph.
    /// </para>
    ///
    /// <para>
    /// The <see cref="LayingOutGraph" /> event fires before graph layout
    /// begins.
    /// </para>
    ///
    /// <para>
    /// The <see cref="IsLayingOutGraph" /> property can also be used to
    /// determine whether the graph is being laid out.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    [Category("Action")]

    public event AsyncCompletedEventHandler GraphLaidOut;


    //*************************************************************************
    //  Event: VerticesMoved
    //
    /// <summary>
    /// Occurs after one or more vertices are moved to a new location with the
    /// mouse.
    /// </summary>
    ///
    /// <remarks>
    /// This event is fired when the user releases the mouse button after
    /// dragging one or more vertices to a new location.
    /// </remarks>
    //*************************************************************************

    [Category("Action")]

    public event VerticesMovedEventHandler VerticesMoved;


    //*************************************************************************
    //  Enum: LayoutState
    //
    /// <summary>
    /// Indicates the state of the graph's layout.
    /// </summary>
    //*************************************************************************

    protected enum
    LayoutState
    {
        /// <summary>
        /// The graph is empty, or it's layout is complete and it has been
        /// drawn.
        /// </summary>

        Stable,

        /// <summary>
        /// The graph needs to be laid out.
        /// </summary>

        LayoutRequired,

        /// <summary>
        /// The graph is being asynchronously laid out.
        /// </summary>

        LayingOut,


        /// <summary>
        /// The asynchronous layout has completed and now the graph needs to be
        /// drawn.
        /// </summary>

        LayoutCompleted,

        /// <summary>
        /// Same as Stable, but the control has been resized and now the
        /// graph's layout needs to be transformed to the new size.
        /// </summary>

        TransformRequired,
    }

    //*************************************************************************
    //  Property: VertexDrawer
    //
    /// <summary>
    /// Gets the <see cref="Wpf.VertexDrawer" /> used to draw the graph's
    /// vertices.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="Wpf.VertexDrawer" /> used to draw the graph's vertices.
    /// </value>
    //*************************************************************************

    protected VertexDrawer
    VertexDrawer
    {
        get
        {
            // AssertValid();

            return (m_oGraphDrawer.VertexDrawer);
        }
    }

    //*************************************************************************
    //  Property: EdgeDrawer
    //
    /// <summary>
    /// Gets the <see cref="Wpf.EdgeDrawer" /> used to draw the graph's edges.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="Wpf.EdgeDrawer" /> used to draw the graph's edges.
    /// </value>
    //*************************************************************************

    protected EdgeDrawer
    EdgeDrawer
    {
        get
        {
            AssertValid();

            return (m_oGraphDrawer.EdgeDrawer);
        }
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets the rectangle that defines the bounds of the graph.
    /// </summary>
    ///
    /// <value>
    /// The rectangle that defines the bounds of the graph.
    /// </value>
    ///
    /// <remarks>
    /// The rectangle's dimensions are not affected by either of the transforms
    /// used for the control's render transform, <see
    /// cref="ScaleTransformForRender" /> or <see
    /// cref="TranslateTransformForRender" />.  See <see
    /// cref="CreateTransforms" /> for details. 
    /// </remarks>
    //*************************************************************************

    protected Rect
    GraphRectangle
    {
        get
        {
            return (
                new Rect( new Size(this.ActualWidth, this.ActualHeight) ) );
        }
    }

    //*************************************************************************
    //  Property: ScaleTransformForRender
    //
    /// <summary>
    /// Gets the ScaleTransform used for the control's render transform.
    /// </summary>
    ///
    /// <value>
    /// A ScaleTransform object that controls the graph's zoom.
    /// </value>
    //*************************************************************************

    protected ScaleTransform
    ScaleTransformForRender
    {
        get
        {
            AssertValid();

            Debug.Assert(this.RenderTransform is TransformGroup);

            TransformGroup oTransformGroup =
                (TransformGroup)this.RenderTransform;

            Debug.Assert(oTransformGroup.Children.Count == 2);
            Debug.Assert(oTransformGroup.Children[0] is ScaleTransform);

            return ( (ScaleTransform)oTransformGroup.Children[0] );
        }
    }

    //*************************************************************************
    //  Property: TranslateTransformForRender
    //
    /// <summary>
    /// Gets the TranslateTransform used for the control's render transform.
    /// </summary>
    ///
    /// <value>
    /// A TranslateTransform object that controls the position of the graph.
    /// </value>
    //*************************************************************************

    protected TranslateTransform
    TranslateTransformForRender
    {
        get
        {
            AssertValid();

            Debug.Assert(this.RenderTransform is TransformGroup);

            TransformGroup oTransformGroup =
                (TransformGroup)this.RenderTransform;

            Debug.Assert(oTransformGroup.Children.Count == 2);
            Debug.Assert(oTransformGroup.Children[1] is TranslateTransform);

            return ( (TranslateTransform)oTransformGroup.Children[1] );
        }
    }

    //*************************************************************************
    //  Property: ShouldBundleEdges()
    //
    /// <summary>
    /// Gets a flag specifying whether edges should be bundled.
    /// </summary>
    ///
    /// <remarks>
    /// true if edges should be bundled.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    ShouldBundleEdges
    {
        get
        {
            AssertValid();

            return (m_oGraph.Edges.Count > 0
                &&
                this.EdgeDrawer.CurveStyle ==
                    EdgeCurveStyle.CurveThroughIntermediatePoints
                );
        }
    }

    //*************************************************************************
    //  Property: ClassName
    //
    /// <summary>
    /// Gets the full name of the class.
    /// </summary>
    ///
    /// <value>
    /// The full name of the class, suitable for use in error messages.
    /// </value>
    //*************************************************************************

    protected String
    ClassName
    {
        get
        {
            return (this.GetType().FullName);
        }
    }

    //*************************************************************************
    //  Property: ArgumentChecker
    //
    /// <summary>
    /// Gets a new initialized <see cref="ArgumentChecker" /> object.
    /// </summary>
    ///
    /// <value>
    /// A new initialized <see cref="ArgumentChecker" /> object.
    /// </value>
    ///
    /// <remarks>
    /// The returned object can be used to check the validity of property
    /// values and method parameters.
    /// </remarks>
    //*************************************************************************

    internal ArgumentChecker
    ArgumentChecker
    {
        get
        {
            return ( new ArgumentChecker(this.ClassName) );
        }
    }

    //*************************************************************************
    //  Property: VisualChildrenCount
    //
    /// <summary>
    /// Gets the number of visual child elements within this element.
    /// </summary>
    ///
    /// <value>
    /// The number of visual child elements for this element.
    /// </value>
    //*************************************************************************

    protected override Int32
    VisualChildrenCount
    {
        get
        {
            return (m_oGraphDrawer.VisualCollection.Count);
        }
    }

    //*************************************************************************
    //  Method: CollapseGroupInternal()
    //
    /// <summary>
    /// Collapses a group of vertices.
    /// </summary>
    ///
    /// <param name="oGroupToCollapse">
    /// The group to collapse.
    /// </param>
    ///
    /// <param name="bRedrawGroupImmediately">
    /// true to redraw the collapsed group immediately.
    /// </param>
    //*************************************************************************

    protected void
    CollapseGroupInternal
    (
        GroupInfo oGroupToCollapse,
        Boolean bRedrawGroupImmediately
    )
    {
        Debug.Assert(oGroupToCollapse != null);
        AssertValid();

        String groupName = oGroupToCollapse.Name;
        ICollection<IVertex> oVerticesInGroup = oGroupToCollapse.Vertices;

        if (IsCollapsedGroup(groupName) || oVerticesInGroup.Count == 0)
        {
            return;
        }

        // Create a vertex that will represent the collapsed group.

        IVertex oCollapsedGroupVertex = m_oGraph.Vertices.Add();

        SetGroupVertexAttributes(oGroupToCollapse, oCollapsedGroupVertex,
            oVerticesInGroup);

        // Save the collapsed vertices on the new group vertex.  These will be
        // used by ExpandGroup() if the group is later expanded.

        oCollapsedGroupVertex.SetValue(ReservedMetadataKeys.CollapsedVertices,
            oVerticesInGroup);

        // Store the vertices in a HashSet that will allow the code below to
        // quickly determine whether an edge is an internal edge, meaning that
        // both its vertices are in the group.

        HashSet<IVertex> oVerticesToCollapse =
            new HashSet<IVertex>(oVerticesInGroup);

        LinkedList<IEdge> oCollapsedInternalEdges = new LinkedList<IEdge>();
        LinkedList<IEdge> oExternalEdgeClones = new LinkedList<IEdge>();

        Boolean bGraphIsDirected =
            (m_oGraph.Directedness == GraphDirectedness.Directed);

        foreach (IVertex oVertexToCollapse in oVerticesInGroup)
        {
            foreach (IEdge oIncidentEdge in oVertexToCollapse.IncidentEdges)
            {
                IVertex oAdjacentVertex = oIncidentEdge.GetAdjacentVertex(
                    oVertexToCollapse);

                if ( oVerticesToCollapse.Contains(oAdjacentVertex) )
                {
                    // The incident edge is internal.  Save the edge in
                    // metadata on the group vertex so that ExpandGroup() can
                    // restore the edge by adding it back to the graph.

                    oCollapsedInternalEdges.AddLast(oIncidentEdge);
                }
                else
                {
                    // The incident edge connects to a vertex not in the group.
                    // In NodeXL, an edge can't be given new vertices.
                    // Instead, draw a cloned edge between the adjacent vertex
                    // and oCollapsedGroupVertex.

                    IEdge oExternalEdgeClone = null;

                    if ( oAdjacentVertex == oIncidentEdge.Vertices[0] )
                    {
                        oExternalEdgeClone = oIncidentEdge.Clone(true, true,
                            oAdjacentVertex, oCollapsedGroupVertex,
                            bGraphIsDirected);

                        // Save the collapsed vertex as metadata on the cloned
                        // edge.  ExpandGroup() will use this to connect to
                        // the collapsed vertex again.

                        oExternalEdgeClone.SetValue(
                            ReservedMetadataKeys.OriginalVertex2InEdge
                                + groupName,
                            oVertexToCollapse);
                    }
                    else
                    {
                        oExternalEdgeClone = oIncidentEdge.Clone(true, true,
                            oCollapsedGroupVertex, oAdjacentVertex,
                            bGraphIsDirected);

                        oExternalEdgeClone.SetValue(
                            ReservedMetadataKeys.OriginalVertex1InEdge
                                + groupName,
                            oVertexToCollapse);
                    }

                    oExternalEdgeClones.AddLast(oExternalEdgeClone);
                }

                RemoveEdgeDuringGroupCollapseOrExpand(oIncidentEdge,
                    bRedrawGroupImmediately);
            }

            RemoveVertexDuringGroupCollapseOrExpand(oVertexToCollapse,
                bRedrawGroupImmediately);
        }

        // The drawing of the new vertices and edges must be done in a
        // particular order to satisfy the needs of the
        // CollapsedGroupDrawingManager class, which the drawing code uses to
        // modify the appearance of a collapsed group and its external edges.
        //
        // First, add the cloned external edges to the graph, but don't draw
        // them yet.

        foreach (IEdge oExternalEdgeClone in oExternalEdgeClones)
        {
            AddEdgeDuringGroupCollapseOrExpand(oExternalEdgeClone, false);
        }

        // Now draw the collapsed group vertex.  Before the vertex is drawn,
        // CollapsedGroupDrawingManager.PreDrawVertex() might add metadata to
        // the vertex and its incident edges to change their appearance.
        // That's why the cloned external edges can't be drawn yet.

        if (bRedrawGroupImmediately && m_oLastGraphDrawingContext != null)
        {
            m_oGraphDrawer.DrawNewVertex(oCollapsedGroupVertex,
                m_oLastGraphDrawingContext);
        }

        // Now draw the cloned external edges.

        foreach (IEdge oExternalEdgeClone in oExternalEdgeClones)
        {
            if (m_oLastGraphDrawingContext != null)
            {
                m_oGraphDrawer.DrawNewEdge(
                    oExternalEdgeClone, m_oLastGraphDrawingContext);
            }
        }

        // Save the collapsed internal edges on the new group vertex.  These
        // will be used by ExpandGroup() if the group is later expanded.

        oCollapsedGroupVertex.SetValue(
            ReservedMetadataKeys.CollapsedInternalEdges,
            oCollapsedInternalEdges);

        m_oCollapsedGroups.Add(groupName, oCollapsedGroupVertex);
    }

    //*************************************************************************
    //  Method: SetGroupVertexAttributes()
    //
    /// <summary>
    /// Sets attributes on the vertex that represents a collapsed group.
    /// </summary>
    ///
    /// <param name="oCollapsedGroup">
    /// The collapsed group.
    /// </param>
    ///
    /// <param name="oCollapsedGroupVertex">
    /// The vertex that represents the collapsed group.
    /// </param>
    ///
    /// <param name="oVerticesToCollapse">
    /// The vertices represented by <paramref name="oCollapsedGroupVertex" />.
    /// It's assumed that the collection is not empty.
    /// </param>
    ///
    /// <remarks>
    /// This method gets called when a group of vertices is collapsed and
    /// replaced with a group vertex.
    /// </remarks>
    //*************************************************************************

    protected void
    SetGroupVertexAttributes
    (
        GroupInfo oCollapsedGroup,
        IVertex oCollapsedGroupVertex,
        ICollection<IVertex> oVerticesToCollapse
    )
    {
        Debug.Assert(oCollapsedGroup != null);
        Debug.Assert(oCollapsedGroupVertex != null);
        Debug.Assert(oVerticesToCollapse != null);
        Debug.Assert(oVerticesToCollapse.Count > 0);
        AssertValid();

        // If the group has no collapsed location, arbitrarily use the location
        // of the first vertex in the group.

        oCollapsedGroupVertex.Location = 
            oCollapsedGroup.CollapsedLocation ??
            oVerticesToCollapse.First().Location;

        Boolean bAllVerticesSelected = true;
        Boolean bAllVerticesHidden = true;

        foreach (IVertex oVertex in oVerticesToCollapse)
        {
            if ( !VertexOrEdgeIsSelected(oVertex) )
            {
                bAllVerticesSelected = false;
            }

            if (VertexDrawer.GetVisibility(oVertex) !=
                VisibilityKeyValue.Hidden)
            {
                bAllVerticesHidden = false;
            }
        }

        // If all of the group's vertices are selected or hidden, select or
        // hide the collapsed group vertex.

        if (bAllVerticesSelected)
        {
            MarkVertexOrEdgeAsSelected(oCollapsedGroupVertex, true);
            m_oSelectedVertices.Add(oCollapsedGroupVertex);
        }

        if (bAllVerticesHidden)
        {
            oCollapsedGroupVertex.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Hidden);
        }

        // Store the group information on the collapsed group vertex.  This
        // gets used by VertexDrawer when the collapsed group vertex is drawn.

        oCollapsedGroupVertex.SetValue(
            ReservedMetadataKeys.CollapsedGroupInfo, oCollapsedGroup);

        // In case a sort order has been specified for the graph's vertices,
        // arbitrarily put the group vertex at the top of the sort order.
        //
        // This is only to avoid an exception that SortableLayoutBase throws
        // when it encounters a vertex that is missing the
        // SortableLayoutAndZOrder key.

        oCollapsedGroupVertex.SetValue(
            ReservedMetadataKeys.SortableLayoutAndZOrder, Single.MaxValue);
    }

    //*************************************************************************
    //  Method: AddEdgeDuringGroupCollapseOrExpand()
    //
    /// <summary>
    /// Adds an edge to the graph while a group is being collapsed or expanded.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to add to the graph.
    /// </param>
    ///
    /// <param name="bDrawEdge">
    /// true to draw the edge immediately.
    /// </param>
    //*************************************************************************

    protected void
    AddEdgeDuringGroupCollapseOrExpand
    (
        IEdge oEdge,
        Boolean bDrawEdge
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        m_oGraph.Edges.Add(oEdge);

        if ( VertexOrEdgeIsSelected(oEdge) )
        {
            m_oSelectedEdges.Add(oEdge);
        }

        if (bDrawEdge && m_oLastGraphDrawingContext != null)
        {
            m_oGraphDrawer.DrawNewEdge(oEdge, m_oLastGraphDrawingContext);
        }
    }

    //*************************************************************************
    //  Method: AddVertexDuringGroupExpand()
    //
    /// <summary>
    /// Adds a vertex to the graph while a group is being expanded.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to add to the graph.
    /// </param>
    ///
    /// <param name="bDrawVertex">
    /// true to draw the vertex immediately.
    /// </param>
    ///
    /// <param name="oRandom">
    /// If the added vertex has never been laid out and <paramref
    /// name="bDrawVertex" /> is true, the vertex is given a random location
    /// using this Random object.
    /// </param>
    //*************************************************************************

    protected void
    AddVertexDuringGroupExpand
    (
        IVertex oVertex,
        Boolean bDrawVertex,
        Random oRandom
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oRandom != null);
        AssertValid();

        m_oGraph.Vertices.Add(oVertex);

        if ( VertexOrEdgeIsSelected(oVertex) )
        {
            m_oSelectedVertices.Add(oVertex);
        }

        if (bDrawVertex && m_oLastGraphDrawingContext != null)
        {
            if (oVertex.Location == System.Drawing.PointF.Empty)
            {
                // The vertex has never been laid out.  This occurs when the
                // vertex is in a group that was collapsed before the graph was
                // drawn.  Give the vertex a random location.

                Rect oGraphRectangleMinusMargin =
                    m_oLastGraphDrawingContext.GraphRectangleMinusMargin;

                oVertex.Location = new System.Drawing.PointF(

                    oRandom.Next(
                        (Int32)Math.Ceiling(oGraphRectangleMinusMargin.Left),
                        (Int32)oGraphRectangleMinusMargin.Right
                        ),

                    oRandom.Next(
                        (Int32)Math.Ceiling(oGraphRectangleMinusMargin.Top),
                        (Int32)oGraphRectangleMinusMargin.Bottom
                        )
                    );
            }

            m_oGraphDrawer.DrawNewVertex(oVertex, m_oLastGraphDrawingContext);
        }
    }

    //*************************************************************************
    //  Method: RemoveEdgeDuringGroupCollapseOrExpand()
    //
    /// <summary>
    /// Removes an edge from the graph while a group is being collapsed or
    /// expanded.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to remove from the graph.
    /// </param>
    ///
    /// <param name="bUndrawEdge">
    /// true to undraw the edge immediately.
    /// </param>
    //*************************************************************************

    protected void
    RemoveEdgeDuringGroupCollapseOrExpand
    (
        IEdge oEdge,
        Boolean bUndrawEdge
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        if (bUndrawEdge && m_oLastGraphDrawingContext != null)
        {
            m_oGraphDrawer.UndrawEdge(oEdge, m_oLastGraphDrawingContext);
        }

        m_oGraph.Edges.Remove(oEdge);
        m_oSelectedEdges.Remove(oEdge);
    }

    //*************************************************************************
    //  Method: RemoveVertexDuringGroupCollapseOrExpand()
    //
    /// <summary>
    /// Removes a vertex from the graph while a group is being collapsed or
    /// expanded.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to remove from the graph.
    /// </param>
    ///
    /// <param name="bUndrawVertex">
    /// true to undraw the vertex immediately.
    /// </param>
    //*************************************************************************

    protected void
    RemoveVertexDuringGroupCollapseOrExpand
    (
        IVertex oVertex,
        Boolean bUndrawVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        if (bUndrawVertex && m_oLastGraphDrawingContext != null)
        {
            m_oGraphDrawer.UndrawVertex(oVertex, m_oLastGraphDrawingContext);
        }

        m_oGraph.Vertices.Remove(oVertex);
        m_oSelectedVertices.Remove(oVertex);
    }

    //*************************************************************************
    //  Method: UpdateCollapsedGroupLocations()
    //
    /// <summary>
    /// Updates the collapsed group locations stored within a collection of
    /// vertices.
    /// </summary>
    ///
    /// <param name="oVertices">
    /// Collection of vertices, some of which may represent collapsed groups.
    /// </param>
    ///
    /// <remarks>
    /// For each vertex in <paramref name="oVertices" /> that represents a
    /// collapsed group, this method updates the <see
    /// cref="GroupInfo.CollapsedLocation" /> property for the group.
    /// </remarks>
    //*************************************************************************

    protected void
    UpdateCollapsedGroupLocations
    (
        IEnumerable<IVertex> oVertices
    )
    {
        Debug.Assert(oVertices != null);
        AssertValid();

        if (m_oCollapsedGroups.Count > 0)
        {
            // This is a way to quickly determine whether a vertex represents a
            // collapsed group.  The alternative is to search every vertex for
            // the ReservedMetadataKeys.CollapsedGroupInfo key, which would be
            // slower.

            HashSet<IVertex> oCollapsedGroupVertices =
                new HashSet<IVertex>(m_oCollapsedGroups.Values);

            foreach (IVertex oVertex in oVertices)
            {
                if ( oCollapsedGroupVertices.Contains(oVertex) )
                {
                    GroupInfo oCollapsedGroup =
                        (GroupInfo)oVertex.GetRequiredValue(
                            ReservedMetadataKeys.CollapsedGroupInfo,
                            typeof(GroupInfo) );

                    oCollapsedGroup.CollapsedLocation = oVertex.Location;
                }
            }
        }
    }

    //*************************************************************************
    //  Method: MoveSelectedVertices()
    //
    /// <summary>
    /// Moves any selected vertices by a specified amount.
    /// </summary>
    ///
    /// <param name="fXDistance">
    /// The distance to move the vertices along the X axis.  Can be negative.
    /// </param>
    ///
    /// <param name="fYDistance">
    /// The distance to move the vertices along the Y axis.  Can be negative.
    /// </param>
    //*************************************************************************

    protected void
    MoveSelectedVertices
    (
        Single fXDistance,
        Single fYDistance
    )
    {
        AssertValid();

        if (m_oSelectedVertices.Count == 0)
        {
            return;
        }

        foreach (IVertex oSelectedVertex in m_oSelectedVertices)
        {
            System.Drawing.PointF oOldLocation = oSelectedVertex.Location;

            oSelectedVertex.Location = new System.Drawing.PointF(
                oOldLocation.X + fXDistance,
                oOldLocation.Y + fYDistance
                );
        }

        RebundleIncidentEdgesIfAppropriate(m_oSelectedVertices);
        UpdateCollapsedGroupLocations(m_oSelectedVertices);
        this.DrawGraph();
        FireVerticesMoved(m_oSelectedVertices);
    }

    //*************************************************************************
    //  Method: OnNewLayout()
    //
    /// <summary>
    /// Performs required tasks when a new layout is used.
    /// </summary>
    ///
    /// <param name="oNewLayout">
    /// The new layout object.
    /// </param>
    //*************************************************************************

    protected void
    OnNewLayout
    (
        ILayout oNewLayout
    )
    {
        // AssertValid();
        Debug.Assert(oNewLayout != null);

        this.VertexDrawer.LimitVerticesToBounds =
            !oNewLayout.SupportsOutOfBoundsVertices;

        oNewLayout.LayOutGraphCompleted +=
            new AsyncCompletedEventHandler(this.Layout_LayOutGraphCompleted);
    }

    //*************************************************************************
    //  Method: CreateGraphDrawer()
    //
    /// <summary>
    /// Create a GraphDrawer object.
    /// </summary>
    //*************************************************************************

    protected void
    CreateGraphDrawer()
    {
        m_oGraphDrawer = new GraphDrawer(this);

        m_oGraphDrawer.EdgeDrawer.CurveStyleChanged +=
            new EventHandler(EdgeDrawer_CurveStyleChanged);

        // AssertValid();
    }

    //*************************************************************************
    //  Method: CreateVertexToolTipTracker()
    //
    /// <summary>
    /// Creates a helper object for displaying vertex tooltips and registers
    /// event handlers with it.
    /// </summary>
    //*************************************************************************

    protected void
    CreateVertexToolTipTracker()
    {
        m_oVertexToolTipTracker = new WpfToolTipTracker();

        m_oVertexToolTipTracker.HideDelayMs = VertexToolTipHideDelayMs;

        m_oVertexToolTipTracker.ShowToolTip +=
            new WpfToolTipTracker.ToolTipTrackerEvent(
                this.VertexToolTipTracker_ShowToolTip);

        m_oVertexToolTipTracker.HideToolTip +=
            new WpfToolTipTracker.ToolTipTrackerEvent(
                this.VertexToolTipTracker_HideToolTip);
    }

    //*************************************************************************
    //  Method: CreateDefaultVertexToolTip()
    //
    /// <summary>
    /// Creates a UIElement to use as a default vertex tooltip.
    /// </summary>
    ///
    /// <param name="sToolTip">
    /// The tooltip string to use.  Can be empty but not null.
    /// </param>
    ///
    /// <returns>
    /// A UIElement.
    /// </returns>
    //*************************************************************************

    protected UIElement
    CreateDefaultVertexToolTip
    (
        String sToolTip
    )
    {
        Debug.Assert(sToolTip != null);
        AssertValid();

        // Use a Label as a default tooltip.

        System.Windows.Controls.Label oLabel =
            new System.Windows.Controls.Label();

        oLabel.Background = SystemColors.InfoBrush;
        oLabel.BorderBrush = SystemColors.WindowFrameBrush;
        oLabel.BorderThickness = new Thickness(1);
        oLabel.Padding = new Thickness(3, 2, 3, 1);
        oLabel.Content = sToolTip;

        return (oLabel);
    }

    //*************************************************************************
    //  Method: CreateEdgeBundler()
    //
    /// <summary>
    /// Creates a new <see cref="EdgeBundler" /> object.
    /// </summary>
    ///
    /// <returns>
    /// A new <see cref="EdgeBundler" /> object.
    /// </returns>
    //*************************************************************************

    protected EdgeBundler
    CreateEdgeBundler()
    {
        EdgeBundler oEdgeBundler = new EdgeBundler();
        oEdgeBundler.Straightening = m_fEdgeBundlerStraightening;

        return (oEdgeBundler);
    }

    //*************************************************************************
    //  Method: BundleAllEdgesIfAppropriate()
    //
    /// <summary>
    /// Bundles all edges if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    BundleAllEdgesIfAppropriate()
    {
        AssertValid();

        if (this.ShouldBundleEdges)
        {
            // For this CurveStyle value, NodeXLControl uses the EdgeBundler
            // class to calculate and store intermediate edge points.  See
            // EdgeDrawer.CurveStyle for more details.

            System.Drawing.Rectangle oLayoutRectangleMinusMargin;

            if ( TryGetLayoutRectangleMinusMargin(
                out oLayoutRectangleMinusMargin) )
            {
                CreateEdgeBundler().BundleAllEdges(m_oGraph,
                    oLayoutRectangleMinusMargin);

                m_oGraph.SetValue(
                    ReservedMetadataKeys.GraphHasEdgeIntermediateCurvePoints,
                    null);

                return;
            }
        }

        m_oGraph.RemoveKey(
            ReservedMetadataKeys.GraphHasEdgeIntermediateCurvePoints);
    }

    //*************************************************************************
    //  Method: RebundleIncidentEdgesIfAppropriate()
    //
    /// <summary>
    /// Rebundles the edges that are incident to a specified set of vertices,
    /// if appropriate.
    /// </summary>
    ///
    /// <param name="oVertices">
    /// The vertices whose edges should be rebundled.
    /// </param>
    ///
    /// <remarks>
    /// This method uses the <see cref="EdgeBundler" /> class to recalculate
    /// the intermediate curve points for all edges incident to the specified
    /// vertices.
    /// </remarks>
    //*************************************************************************

    protected void
    RebundleIncidentEdgesIfAppropriate
    (
        IEnumerable<IVertex> oVertices
    )
    {
        Debug.Assert(oVertices != null);
        AssertValid();

        if (!this.ShouldBundleEdges)
        {
            return;
        }

        System.Drawing.Rectangle oLayoutRectangleMinusMargin;

        if ( !TryGetLayoutRectangleMinusMargin(
            out oLayoutRectangleMinusMargin) )
        {
            return;
        }

        // Don't rebundle the same edge twice, because rebundling takes a long
        // time.

        Dictionary<Int32, IEdge> oUniqueEdges = new Dictionary<Int32, IEdge>();

        foreach (IVertex oVertex in oVertices)
        {
            foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
            {
                oUniqueEdges[oIncidentEdge.ID] = oIncidentEdge;
            }
        }

        CreateEdgeBundler().BundleEdges(this.Graph, oUniqueEdges.Values,
            oLayoutRectangleMinusMargin);
    }

    //*************************************************************************
    //  Method: CreateTransforms()
    //
    /// <summary>
    /// Creates the transforms that control zoom and scale.
    /// </summary>
    //*************************************************************************

    protected void
    CreateTransforms()
    {
        // The zoom is controlled by a ScaleTransform used as the first of two
        // render transforms.  If its ScaleX and ScaleY properties are set to
        // 2.0, for example, the graph is rendered twice as large as normal and
        // doesn't fit within the control.
        //
        // This ScaleTransform does not affect the ActualWidth and ActualHeight
        // properties.

        TransformGroup oTransformGroup = new TransformGroup();

        oTransformGroup.Children.Add( new ScaleTransform() );

        // The zoom center is controlled by a TranslateTransform used as a
        // second render transform.  If its X property is set to -100, for
        // example, then the graph, which has been scaled by the group's first
        // transform, is translated to the left by 100 scaled units.

        oTransformGroup.Children.Add( new TranslateTransform() );

        this.RenderTransform = oTransformGroup;

        // Note that mouse coordinates as reported by
        // MouseEventArgs.GetLocation() are affected by both transforms.
        // Because of this, it is sometimes more convenient to convert the
        // mouse coordinates to screen coordinates, which are not affected by
        // any of the transforms.
    }

    //*************************************************************************
    //  Method: SetGraphZoom()
    //
    /// <summary>
    /// Sets a value that determines the zoom level of the graph.
    /// </summary>
    ///
    /// <param name="dGraphZoom">
    /// A value that determines the zoom level of the graph.  Must be between
    /// <see cref="MinimumGraphZoom" /> and <see cref="MaximumGraphZoom" />.
    /// </param>
    ///
    /// <param name="bLimitTranslation">
    /// If true, the TranslateTransform used for rendering is limited to
    /// prevent the graph from being moved too far by the zoom.
    /// </param>
    //*************************************************************************

    protected void
    SetGraphZoom
    (
        Double dGraphZoom,
        Boolean bLimitTranslation
    )
    {
        Debug.Assert(dGraphZoom >= MinimumGraphZoom);
        Debug.Assert(dGraphZoom <= MaximumGraphZoom);
        AssertValid();

        ResetVertexToolTipTracker();

        // See CreateTransforms() for details on how the graph zoom works.

        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;

        oScaleTransformForRender.ScaleX =
            oScaleTransformForRender.ScaleY = dGraphZoom;

        if (bLimitTranslation)
        {
            LimitTranslation();
        }

        FireGraphZoomChanged();

        AssertValid();
    }

    //*************************************************************************
    //  Method: CenterGraphZoom()
    //
    /// <summary>
    /// Sets the center of the graph's zoom to the center of the control.
    /// </summary>
    ///
    /// <remarks>
    /// This method uses ActualWidth and ActualHeight, which are valid only
    /// after a WPF layout cycle.  If this is called before a WPF layout cycle
    /// completes, the zoom center is set to the control's origin.
    /// </remarks>
    //*************************************************************************

    protected void
    CenterGraphZoom()
    {
        AssertValid();

        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;

        oScaleTransformForRender.CenterX = this.ActualWidth / 2.0;
        oScaleTransformForRender.CenterY = this.ActualHeight / 2.0;
    }

    //*************************************************************************
    //  Method: LimitTranslation()
    //
    /// <overloads>
    /// Prevents the graph from being moved too far.
    /// </overloads>
    ///
    /// <summary>
    /// Prevents the graph from being moved too far by adjusting the
    /// TranslateTransform used for rendering.
    /// </summary>
    //*************************************************************************

    protected void
    LimitTranslation()
    {
        AssertValid();

        TranslateTransform oTranslateTransformForRender =
            this.TranslateTransformForRender;

        Double dTranslateX = oTranslateTransformForRender.X;
        Double dTranslateY = oTranslateTransformForRender.Y;

        LimitTranslation(ref dTranslateX, ref dTranslateY);

        oTranslateTransformForRender.X = dTranslateX;
        oTranslateTransformForRender.Y = dTranslateY;

        FireGraphTranslationChanged();
    }

    //*************************************************************************
    //  Method: LimitTranslation()
    //
    /// <summary>
    /// Prevents the graph from being moved too far, given a pair of proposed
    /// translation distances.
    /// </summary>
    ///
    /// <param name="dTranslateX">
    /// On input, this is the proposed TranslateTransform.X property.  On
    /// output, it is set to a value that will prevent the graph from being
    /// moved too far.
    /// </param>
    ///
    /// <param name="dTranslateY">
    /// On input, this is the proposed TranslateTransform.Y property.  On
    /// output, it is set to a value that will prevent the graph from being
    /// moved too far.
    /// </param>
    ///
    /// <remarks>
    /// The caller should modify the TranslateTransform with the modified
    /// <paramref name="dTranslateX" /> and <paramref name="dTranslateY" />
    /// values.
    /// </remarks>
    //*************************************************************************

    protected void
    LimitTranslation
    (
        ref Double dTranslateX,
        ref Double dTranslateY
    )
    {
        AssertValid();

        // See CreateTransforms() for details on the transforms used by this
        // class.

        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;

        Double dScaleTransformForRenderCenterX =
            oScaleTransformForRender.CenterX;

        Double dScaleTransformForRenderCenterY =
            oScaleTransformForRender.CenterY;

        Double dRenderScale = oScaleTransformForRender.ScaleX;
        Rect oGraphRectangle = this.GraphRectangle;

        Double dRenderScaleMinus1 = dRenderScale - 1.0;

        dTranslateX = Math.Min(
            dScaleTransformForRenderCenterX * dRenderScaleMinus1,
            dTranslateX
            );

        dTranslateX = Math.Max(

            -(oGraphRectangle.Width - dScaleTransformForRenderCenterX)
                * dRenderScaleMinus1,

            dTranslateX
            );

        dTranslateY = Math.Min(

            dScaleTransformForRenderCenterY * dRenderScaleMinus1,
            dTranslateY
            );

        dTranslateY = Math.Max(

            -(oGraphRectangle.Height - dScaleTransformForRenderCenterY)
                * dRenderScaleMinus1,

            dTranslateY
            );
    }

    //*************************************************************************
    //  Method: GetVisualChild()
    //
    /// <summary>
    /// Returns a child at the specified index from a collection of child
    /// elements. 
    /// </summary>
    ///
    /// <param name="index">
    /// The zero-based index of the requested child element in the collection.
    /// </param>
    ///
    /// <returns>
    /// The requested child element.
    /// </returns>
    //*************************************************************************

    protected override Visual
    GetVisualChild
    (
        Int32 index
    )
    {
        Debug.Assert(index >= 0);

        return ( m_oGraphDrawer.VisualCollection[index] );
    }

    //*************************************************************************
    //  Method: LayOutOrDrawGraph()
    //
    /// <summary>
    /// Lays out or draws the graph, depending on the current layout state.
    /// </summary>
    //*************************************************************************

    protected void
    LayOutOrDrawGraph()
    {
        AssertValid();

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: LayOutOrDrawGraph(), m_eLayoutState = "
            + m_eLayoutState);
        #endif

        Rect oGraphRectangle = this.GraphRectangle;

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: LayOutOrDrawGraph(), size = " +
            oGraphRectangle.Width + " , " + oGraphRectangle.Height);
        #endif

        System.Drawing.Rectangle oGraphRectangle2 =
            WpfGraphicsUtil.RectToRectangle(oGraphRectangle);

        switch (m_eLayoutState)
        {
            case LayoutState.Stable:

                break;

            case LayoutState.LayoutRequired:

                Debug.Assert(!m_oLayout.IsBusy);

                FireLayingOutGraph();

                m_oLastLayoutContext = new LayoutContext(oGraphRectangle2);

                m_eLayoutState = LayoutState.LayingOut;

                if (m_oLayout is SortableLayoutBase)
                {
                    // If the vertex layout order has been set, tell the layout
                    // object to sort the vertices before laying them out.

                    ( (SortableLayoutBase)m_oLayout ).
                        UseMetadataVertexSorter(m_oGraph);
                }

                // Start an asynchronous layout.  The m_oLayout object will
                // fire a LayOutGraphCompleted event when it is done.

                m_oLayout.LayOutGraphAsync(
                    m_oGraph, m_oLastLayoutContext);

                break;

            case LayoutState.LayingOut:

                break;

            case LayoutState.LayoutCompleted:

                // The asynchronous layout has completed and now the graph
                // needs to be drawn.

                m_eLayoutState = LayoutState.Stable;

                // Has the size of the control changed since the layout was
                // started?

                System.Drawing.Rectangle oLastGraphRectangle =
                    m_oLastLayoutContext.GraphRectangle;

                if (
                    oLastGraphRectangle.Width != oGraphRectangle2.Width
                    ||
                    oLastGraphRectangle.Height != oGraphRectangle2.Height
                    )
                {
                    // Yes.  Transform the layout to the new size.

                    #if TRACE_LAYOUT_AND_DRAW
                    Debug.WriteLine("NodeXLControl: Transforming layout.");
                    #endif

                    m_oLastLayoutContext = TransformLayout(oGraphRectangle);
                }

                DrawGraph(oGraphRectangle);

                break;

            case LayoutState.TransformRequired:

                // The control has been resized and now the graph's layout
                // needs to be transformed to the new size.

                m_oLastLayoutContext = TransformLayout(oGraphRectangle);

                m_eLayoutState = LayoutState.Stable;

                DrawGraph(oGraphRectangle);

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: DrawGraph()
    //
    /// <summary>
    /// Draws the graph in a specified rectangle without laying it out first.
    /// </summary>
    ///
    /// <param name="oGraphRectangle">
    /// Rectangle to draw the graph within.
    /// </param>
    //*************************************************************************

    protected void
    DrawGraph
    (
        Rect oGraphRectangle
    )
    {
        AssertValid();

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: DrawGraph(), oGraphRectangle = "
            + oGraphRectangle);
        #endif

        m_oLastGraphDrawingContext = new GraphDrawingContext(
            oGraphRectangle, m_oLayout.Margin, m_oGraphDrawer.BackColor);

        m_oGraphDrawer.DrawGraph(m_oGraph, m_oLastGraphDrawingContext);
    }

    //*************************************************************************
    //  Method: TransformLayout()
    //
    /// <summary>
    /// Transforms the graph's layout to a new size.
    /// </summary>
    ///
    /// <param name="oNewGraphRectangle">
    /// The new size.
    /// </param>
    ///
    /// <returns>
    /// The new LayoutContext object that was used to transform the layout.
    /// </returns>
    //*************************************************************************

    protected LayoutContext
    TransformLayout
    (
        Rect oNewGraphRectangle
    )
    {
        AssertValid();

        LayoutContext oNewLayoutContext = new LayoutContext(
            WpfGraphicsUtil.RectToRectangle(oNewGraphRectangle) );

        m_oLayout.TransformLayout(m_oGraph,
            m_oLastLayoutContext, oNewLayoutContext);

        return (oNewLayoutContext);
    }

    //*************************************************************************
    //  Method: TryGetLayoutRectangleMinusMargin()
    //
    /// <summary>
    /// Attempts to get the rectangle the graph was laid out within, reduced on
    /// all sides by the margin.
    /// </summary>
    ///
    /// <param name="oLayoutRectangleMinusMargin">
    /// Where the rectangle gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the rectangle was obtained, false if the control is narrower or
    /// shorter than twice the margin.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetLayoutRectangleMinusMargin
    (
        out System.Drawing.Rectangle oLayoutRectangleMinusMargin
    )
    {
        AssertValid();

        // Note that between this class, LayoutBase, and GraphDrawingContext,
        // there is some duplication of margin-subtracting code.  That's
        // because layout-related code uses System.Drawing rectangles to avoid
        // WPF dependencies, while drawing code uses WPF rectangles.

        Int32 iMargin = m_oLayout.Margin;

        oLayoutRectangleMinusMargin = m_oLastLayoutContext.GraphRectangle;
        oLayoutRectangleMinusMargin.Inflate(-iMargin, -iMargin);

        return (oLayoutRectangleMinusMargin.Width > 0 &&
            oLayoutRectangleMinusMargin.Height > 0);
    }

    //*************************************************************************
    //  Method: SetAllSelected()
    //
    /// <summary>
    /// Selects or deselects all vertices and edges.
    /// </summary>
    ///
    /// <param name="bSelect">
    /// true to select, false to deselect.
    /// </param>
    //*************************************************************************

    protected void
    SetAllSelected
    (
        Boolean bSelect
    )
    {
        AssertValid();

        SetAllVerticesSelected(bSelect);
        SetAllEdgesSelected(bSelect);

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: SetAllVerticesSelected()
    //
    /// <summary>
    /// Sets the selected state of all vertices.
    /// </summary>
    ///
    /// <param name="bSelected">
    /// true to select all vertices, false to deselect them.
    /// </param>
    //*************************************************************************

    protected void
    SetAllVerticesSelected
    (
        Boolean bSelected
    )
    {
        AssertValid();

        if (bSelected)
        {
            foreach (IVertex oVertex in this.Graph.Vertices)
            {
                SetVertexSelectedInternal(oVertex, true);
            }
        }
        else
        {
            // Do not directly iterate m_oSelectedVertices here.  Keys may be
            // removed by SetVertexSelectedInternal() and you can't modify a
            // collection while it's being iterated.  Copy the collection to
            // an array first.

            foreach ( IVertex oSelectedVertex in
                m_oSelectedVertices.ToArray() )
            {
                SetVertexSelectedInternal(oSelectedVertex, false);
            }
        }
    }

    //*************************************************************************
    //  Method: SetAllEdgesSelected()
    //
    /// <summary>
    /// Sets the selected state of all edges and updates the internal
    /// collection of selected edges.
    /// </summary>
    ///
    /// <param name="bSelected">
    /// true to select all edges, false to deselect them.
    /// </param>
    //*************************************************************************

    protected void
    SetAllEdgesSelected
    (
        Boolean bSelected
    )
    {
        AssertValid();

        if (bSelected)
        {
            foreach (IEdge oEdge in this.Graph.Edges)
            {
                SetEdgeSelectedInternal(oEdge, true);
            }
        }
        else
        {
            // Do not directly iterate m_oSelectedEdges here.  Keys may be
            // removed by SetEdgeSelectedInternal() and you can't modify a
            // collection while it's being iterated.  Copy the collection to
            // an array first.

            foreach ( IEdge oSelectedEdge in m_oSelectedEdges.ToArray() )
            {
                SetEdgeSelectedInternal(oSelectedEdge, false);
            }
        }
    }

    //*************************************************************************
    //  Method: SetVertexSelectedInternal()
    //
    /// <summary>
    /// Performs all tasks required to select or deselect a vertex.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// Vertex to select or deselect.  Can't be null.
    /// </param>
    ///
    /// <param name="bSelected">
    /// true to select <paramref name="oVertex" />, false to deselect it.
    /// </param>
    ///
    /// <remarks>
    /// If the vertex can't be selected or deselected, this method does
    /// nothing.
    /// </remarks>
    //*************************************************************************

    protected void
    SetVertexSelectedInternal
    (
        IVertex oVertex,
        Boolean bSelected
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        if ( !VertexOrEdgeCanBeSelected(oVertex, bSelected) )
        {
            return;
        }

        // Modify the vertex's metadata to mark it as selected or unselected.

        MarkVertexOrEdgeAsSelected(oVertex, bSelected);

        // Modify the collection of selected vertices.

        if (bSelected)
        {
            m_oSelectedVertices.Add(oVertex);
        }
        else
        {
            m_oSelectedVertices.Remove(oVertex);
        }

        // Redraw the vertex using its modified metadata.

        Debug.Assert(m_oLastGraphDrawingContext != null);

        m_oGraphDrawer.RedrawVertex(oVertex, m_oLastGraphDrawingContext);
    }

    //*************************************************************************
    //  Method: SetEdgeSelectedInternal()
    //
    /// <summary>
    /// Performs all tasks required to select or deselect an edge.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// Edge to select or deselect.  Can't be null.
    /// </param>
    ///
    /// <param name="bSelected">
    /// true to select <paramref name="oEdge" />, false to deselect it.
    /// </param>
    ///
    /// <remarks>
    /// If the edge can't be selected or deselected, this method does nothing.
    /// </remarks>
    //*************************************************************************

    protected void
    SetEdgeSelectedInternal
    (
        IEdge oEdge,
        Boolean bSelected
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        if ( !VertexOrEdgeCanBeSelected(oEdge, bSelected) )
        {
            return;
        }

        // Modify the edge's metadata to mark it as selected or unselected.

        MarkVertexOrEdgeAsSelected(oEdge, bSelected);

        // Modify the collection of selected edges.

        if (bSelected)
        {
            m_oSelectedEdges.Add(oEdge);
        }
        else
        {
            m_oSelectedEdges.Remove(oEdge);
        }

        // Redraw the edge using its modified metadata.

        Debug.Assert(m_oLastGraphDrawingContext != null);

        m_oGraphDrawer.RedrawEdge(oEdge, m_oLastGraphDrawingContext);
    }

    //*************************************************************************
    //  Method: VertexOrEdgeCanBeSelected()
    //
    /// <summary>
    /// Determines whether a vertex or edge can be selected or deselected.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// Vertex or edge to test, as an <see cref="IMetadataProvider" />.
    /// </param>
    ///
    /// <param name="bSelected">
    /// true to test whether <paramref name="oVertexOrEdge" /> can be selected,
    /// false to test whether it can be deselected.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex or edge can be selected or deselected.
    /// </returns>
    //*************************************************************************

    protected Boolean
    VertexOrEdgeCanBeSelected
    (
        IMetadataProvider oVertexOrEdge,
        Boolean bSelected
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        AssertValid();

        if (!bSelected)
        {
            // A vertex or edge can always be deselected.

            return (true);
        }

        switch ( VertexAndEdgeDrawerBase.GetVisibility(oVertexOrEdge) )
        {
            case VisibilityKeyValue.Visible:

                return (true);

            case VisibilityKeyValue.Hidden:

                return (false);

            case VisibilityKeyValue.Filtered:

                // A filtered vertex or edge can be selected as long as it is
                // not completely transparent.

                return (this.EdgeDrawer.FilteredAlpha > 0);

            default:

                Debug.Assert(false);
                return (true);
        }
    }

    //*************************************************************************
    //  Method: MarkVertexOrEdgeAsSelected()
    //
    /// <summary>
    /// Modifies the metadata of a vertex or edge to mark it as selected or
    /// unselected.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// Vertex or edge to mark, as an <see cref="IMetadataProvider" />.
    /// </param>
    ///
    /// <param name="bSelected">
    /// true to mark <paramref name="oVertexOrEdge" /> as selected, false for
    /// unselected.
    /// </param>
    //*************************************************************************

    protected void
    MarkVertexOrEdgeAsSelected
    (
        IMetadataProvider oVertexOrEdge,
        Boolean bSelected
    )
    {
        Debug.Assert(oVertexOrEdge != null);

        if (bSelected)
        {
            oVertexOrEdge.SetValue(ReservedMetadataKeys.IsSelected, null);
        }
        else
        {
            oVertexOrEdge.RemoveKey(ReservedMetadataKeys.IsSelected);
        }
    }

    //*************************************************************************
    //  Method: ResetVertexToolTipTracker()
    //
    /// <summary>
    /// Removes any vertex tooltip that might exist and resets the helper
    /// object that figures out when to show tooltips.
    /// </summary>
    //*************************************************************************

    protected void
    ResetVertexToolTipTracker()
    {
        AssertValid();

        RemoveVertexToolTip();
        m_oVertexToolTipTracker.Reset();
        m_oLastMouseMoveLocation = new Point(-1, -1);
    }

    //*************************************************************************
    //  Method: RemoveVertexToolTip()
    //
    /// <summary>
    /// Removes any vertex tooltip that might exist.
    /// </summary>
    //*************************************************************************

    protected void
    RemoveVertexToolTip()
    {
        AssertValid();

        if (m_oVertexToolTip != null)
        {
            m_oGraphDrawer.RemoveVisualFromTopOfGraph(m_oVertexToolTip);
            m_oVertexToolTip = null;
        }
    }

    //*************************************************************************
    //  Method: GetBackgroundContrastColor()
    //
    /// <summary>
    /// Gets a color that contrasts with the background.
    /// </summary>
    ///
    /// <returns>
    /// A contrastring color.
    /// </returns>
    //*************************************************************************

    protected Color
    GetBackgroundContrastColor()
    {
        AssertValid();

        Color oBackColor = this.BackColor;

        return ( Color.FromArgb(
            255,
            (Byte)Math.Abs(oBackColor.R - 255),
            (Byte)Math.Abs(oBackColor.G - 255),
            (Byte)Math.Abs(oBackColor.B - 255)
            ) );
    }

    //*************************************************************************
    //  Method: CheckIfLayingOutGraph()
    //
    /// <summary>
    /// Throws an exception if a layout is in progress.
    /// </summary>
    ///
    /// <param name="sMethodOrPropertyName">
    /// Name of the method or property calling this method.
    /// </param>
    //*************************************************************************

    protected void
    CheckIfLayingOutGraph
    (
        String sMethodOrPropertyName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMethodOrPropertyName) );
        AssertValid();

        if (this.IsLayingOutGraph)
        {
            throw new InvalidOperationException(String.Format(

                "{0}.{1}: An asynchronous layout is in progress.  Check the"
                + " IsLayingOutGraph property before calling this."
                ,
                this.ClassName,
                sMethodOrPropertyName
                ) );
        }
    }

    //*************************************************************************
    //  Method: CheckForVertexDragOnMouseMove()
    //
    /// <summary>
    /// Checks for a vertex drag operation during the MouseMove event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForVertexDragOnMouseMove
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();
        Point oMouseLocation;

        if ( !DragIsInProgress(m_oVerticesBeingDragged, oMouseEventArgs,
            new MouseButtonState[]{oMouseEventArgs.LeftButton},
            out oMouseLocation) )
        {
            return;
        }

        // Remove from the top of the graph any dragged vertices drawn during
        // the previous MouseMove event.

        RemoveVisualFromTopOfGraph(m_oVerticesBeingDragged);

        // Create a new Visual to represent the dragged vertices and add it on
        // top of the graph.

        m_oVerticesBeingDragged.CreateVisual(oMouseLocation,
            GetBackgroundContrastColor(), m_oGraphDrawer.VertexDrawer);

        m_oGraphDrawer.AddVisualOnTopOfGraph(m_oVerticesBeingDragged.Visual);
    }

    //*************************************************************************
    //  Method: CheckForVertexDragOnMouseUp()
    //
    /// <summary>
    /// Checks for a vertex drag operation during the MouseUp event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForVertexDragOnMouseUp
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        if ( m_oVerticesBeingDragged != null &&
            m_oVerticesBeingDragged.OnMouseUp() )
        {
            Boolean bEscapeKeyIsPressed = this.EscapeKeyIsPressed();

            // Remove from the top of the graph any dragged vertices drawn
            // during the previous MouseMove event.

            RemoveVisualFromTopOfGraph(m_oVerticesBeingDragged);

            if (bEscapeKeyIsPressed)
            {
                m_oVerticesBeingDragged.CancelDrag();
            }

            // Remove metadata that VerticesBeingDragged added to the dragged
            // vertices.

            m_oVerticesBeingDragged.RemoveMetadataFromVertices();

            if (!bEscapeKeyIsPressed)
            {
                // If edge bundling is being used, rebundle the edges that are
                // attached to the dragged vertices.

                RebundleIncidentEdgesIfAppropriate(
                    m_oVerticesBeingDragged.Vertices);
            }

            // m_oVerticesBeingDragged may have moved the selected vertices, so
            // redraw the graph.

            DrawGraph();

            if (!bEscapeKeyIsPressed)
            {
                IVertex [] aoDraggedVertices =
                    m_oVerticesBeingDragged.Vertices;

                UpdateCollapsedGroupLocations(aoDraggedVertices);
                FireVerticesMoved(aoDraggedVertices);
            }
        }

        m_oVerticesBeingDragged = null;
    }

    //*************************************************************************
    //  Method: CheckForMarqueeDragOnMouseMove()
    //
    /// <summary>
    /// Checks for a marquee drag operation during the MouseMove event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForMarqueeDragOnMouseMove
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        Point oMouseLocation;

        if ( !DragIsInProgress(m_oMarqueeBeingDragged, oMouseEventArgs,
            new MouseButtonState[]{oMouseEventArgs.LeftButton},
            out oMouseLocation) )
        {
            return;
        }

        // Remove from the top of the graph any marquee drawn during the
        // previous MouseMove event.

        RemoveVisualFromTopOfGraph(m_oMarqueeBeingDragged);

        // Create a new marquee and add it on top of the graph.

        m_oMarqueeBeingDragged.CreateVisual( oMouseLocation,
            GetBackgroundContrastColor() );

        m_oGraphDrawer.AddVisualOnTopOfGraph(m_oMarqueeBeingDragged.Visual);

        // Set the cursor, which depends on optional key presses.

        this.Cursor = GetCursorForMarqueeDrag();
    }

    //*************************************************************************
    //  Method: CheckForMarqueeDragOnMouseUp()
    //
    /// <summary>
    /// Checks for a marquee drag operation during the MouseUp event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForMarqueeDragOnMouseUp
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        if ( m_oMarqueeBeingDragged != null &&
            m_oMarqueeBeingDragged.OnMouseUp() )
        {
            // Remove from the top of the graph any marquee drawn during the
            // previous MouseMove event.

            RemoveVisualFromTopOfGraph(m_oMarqueeBeingDragged);

            if ( !this.EscapeKeyIsPressed() )
            {
                // Select or deselect the marqueed vertices.

                SelectMarqueedVertices();
            }
        }

        m_oMarqueeBeingDragged = null;
    }

    //*************************************************************************
    //  Method: CheckForTranslationDragOnMouseMove()
    //
    /// <summary>
    /// Checks for a translation drag operation during the MouseMove event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForTranslationDragOnMouseMove
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        Point oMouseLocation;

        if ( !DragIsInProgress(m_oTranslationBeingDragged, oMouseEventArgs,

            new MouseButtonState[]{oMouseEventArgs.LeftButton,
                oMouseEventArgs.MiddleButton},

            out oMouseLocation) )
        {
            return;
        }

        // Adjust the TranslateTransform in response to the mouse move.

        Double dNewTranslateX, dNewTranslateY;

        m_oTranslationBeingDragged.GetTranslationDistances(
            this.PointToScreen(oMouseLocation),
            out dNewTranslateX, out dNewTranslateY);

        // Prevent the graph from being moved too far.

        LimitTranslation(ref dNewTranslateX, ref dNewTranslateY);

        // Move the graph.

        TranslateTransform oTranslateTransform =
            this.TranslateTransformForRender;

        oTranslateTransform.X = dNewTranslateX;
        oTranslateTransform.Y = dNewTranslateY;

        FireGraphTranslationChanged();
    }

    //*************************************************************************
    //  Method: CheckForTranslationDragOnMouseUp()
    //
    /// <summary>
    /// Checks for a translation drag operation during the MouseUp event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForTranslationDragOnMouseUp
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        // (Nothing further needs to be done for a translation drag on mouse
        // up.)

        m_oTranslationBeingDragged = null;
    }

    //*************************************************************************
    //  Method: CheckForToolTipsOnMouseMove()
    //
    /// <summary>
    /// Checks whether tooltip-related actions need to be taken during the
    /// MouseMove event.
    /// </summary>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    protected void
    CheckForToolTipsOnMouseMove
    (
        MouseEventArgs oMouseEventArgs
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        AssertValid();

        // Don't do anything if the mouse is being dragged.

        if ( DragMightBeInProgress() )
        {
            return;
        }

        Point oMouseLocation = oMouseEventArgs.GetPosition(this);

        if (oMouseLocation.X == m_oLastMouseMoveLocation.X &&
            oMouseLocation.Y == m_oLastMouseMoveLocation.Y)
        {
            return;
        }

        m_oLastMouseMoveLocation = oMouseLocation;

        // If the mouse is over a vertex, get the vertex object.  (oVertex will
        // get set to null if the mouse is not over a vertex.)

        IVertex oVertex;

        m_oGraphDrawer.TryGetVertexFromPoint(oMouseLocation, out oVertex);

        // Update the tooltip tracker.

        m_oVertexToolTipTracker.OnMouseMoveOverObject(oVertex);
    }

    //*************************************************************************
    //  Method: EscapeKeyIsPressed()
    //
    /// <summary>
    /// Determines whether the Escape key is pressed.
    /// </summary>
    ///
    /// <returns>
    /// true if the Escape key is pressed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    EscapeKeyIsPressed()
    {
        AssertValid();

        return ( Keyboard.IsKeyDown(Key.Escape) );
    }

    //*************************************************************************
    //  Method: ControlKeyIsPressed()
    //
    /// <summary>
    /// Determines whether a Control key is pressed.
    /// </summary>
    ///
    /// <returns>
    /// true if a Control key is pressed.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ControlKeyIsPressed()
    {
        AssertValid();

        return ( (Keyboard.Modifiers & ModifierKeys.Control) != 0 );
    }

    //*************************************************************************
    //  Method: StartTranslationDrag()
    //
    /// <summary>
    /// Starts a translation drag operation.
    /// </summary>
    ///
    /// <param name="oMouseLocation">
    /// Mouse location, relative to the control.
    /// </param>
    //*************************************************************************

    protected void
    StartTranslationDrag
    (
        Point oMouseLocation
    )
    {
        AssertValid();

        // Save the mouse location for use within the MouseMove event.

        TranslateTransform oTranslateTransform =
            this.TranslateTransformForRender;

        m_oTranslationBeingDragged = new DraggedTranslation(oMouseLocation,
            this.PointToScreen(oMouseLocation), oTranslateTransform.X,
            oTranslateTransform.Y);

        this.Cursor = Cursors.Hand;
        Mouse.Capture(this);
    }

    //*************************************************************************
    //  Method: DragIsInProgress()
    //
    /// <summary>
    /// Determines whether a particular type of mouse drag is in progress.
    /// </summary>
    ///
    /// <param name="oMouseDrag">
    /// Object that represents the drag operation, or null if the drag
    /// operation hasn't begun.
    /// </param>
    ///
    /// <param name="oMouseEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    ///
    /// <param name="aeMouseButtonStates">
    /// Array of button states, one for each button that can be pressed to
    /// allow the drag to be considered in progress.
    /// </param>
    ///
    /// <param name="oMouseLocation">
    /// Where the mouse location get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified mouse drag is in progress.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DragIsInProgress
    (
        MouseDrag oMouseDrag,
        MouseEventArgs oMouseEventArgs,
        MouseButtonState [] aeMouseButtonStates,
        out Point oMouseLocation
    )
    {
        Debug.Assert(oMouseEventArgs != null);
        Debug.Assert(aeMouseButtonStates != null);
        Debug.Assert(aeMouseButtonStates.Length > 0);
        AssertValid();

        oMouseLocation = new Point();

        if (oMouseDrag != null)
        {
            foreach (MouseButtonState eMouseButtonState in aeMouseButtonStates)
            {
                if (eMouseButtonState == MouseButtonState.Pressed)
                {
                    oMouseLocation = oMouseEventArgs.GetPosition(this);

                    return ( oMouseDrag.OnMouseMove(oMouseLocation) );
                }
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: DragMightBeInProgress()
    //
    /// <summary>
    /// Determines whether any kind of mouse drag is in progress or will be
    /// in progress if the user moves the mouse.
    /// </summary>
    ///
    /// <returns>
    /// true if a mouse drag is in progress, or if a MouseDrag object has been
    /// created and is waiting for the user to move the mouse to begin a drag.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DragMightBeInProgress()
    {
        AssertValid();

        return (m_oVerticesBeingDragged != null ||
            m_oMarqueeBeingDragged != null ||
            m_oTranslationBeingDragged != null
            );
    }

    //*************************************************************************
    //  Method: RemoveVisualFromTopOfGraph()
    //
    /// <summary>
    /// Removes from the top of the graph any Visual drawn during a MouseMove
    /// event.
    /// </summary>
    ///
    /// <param name="oMouseDragWithVisual">
    /// Object that represents the drag operation, or null if the drag
    /// operation hasn't begun.
    /// </param>
    //*************************************************************************

    protected void
    RemoveVisualFromTopOfGraph
    (
        MouseDragWithVisual oMouseDragWithVisual
    )
    {
        AssertValid();

        if (oMouseDragWithVisual == null)
        {
            return;
        }

        Visual oVisual = oMouseDragWithVisual.Visual;

        if (oVisual != null)
        {
            m_oGraphDrawer.RemoveVisualFromTopOfGraph(oVisual);
        }
    }

    //*************************************************************************
    //  Method: GetCursorForMarqueeDrag()
    //
    /// <summary>
    /// Returns a cursor to use for the control while a marquee drag operation
    /// is occurring.
    /// </summary>
    ///
    /// <returns>
    /// The cursor to use.
    /// </returns>
    //*************************************************************************

    protected Cursor
    GetCursorForMarqueeDrag()
    {
        AssertValid();

        String sResourceName = null;

        // When a control key is pressed, the selected state of the marqueed
        // vertices should be inverted, and so a default cursor should be used.

        if ( !ControlKeyIsPressed() )
        {
            if (m_eMouseMode == MouseMode.AddToSelection)
            {
                sResourceName = "MarqueeAdd.cur";
            }
            else if (m_eMouseMode == MouseMode.SubtractFromSelection)
            {
                sResourceName = "MarqueeSubtract.cur";
            }
        }

        if (sResourceName == null)
        {
            sResourceName = "Marquee.cur";
        }

        Stream oCursorResourceStream =
            Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Smrf.NodeXL.Control.Wpf.Images." + sResourceName);

        return ( new Cursor(oCursorResourceStream) );
    }

    //*************************************************************************
    //  Method: SelectMarqueedVertices()
    //
    /// <summary>
    /// Selects or deselects the marqueed vertices.
    /// </summary>
    //*************************************************************************

    protected void
    SelectMarqueedVertices()
    {
        AssertValid();

        Debug.Assert(m_oMarqueeBeingDragged != null);

        // Create HashSets that will contain the new selection.  HashSets are
        // used instead of lists or arrays to prevent the same vertex or edge
        // from being added twice.

        HashSet<IVertex> oVerticesToSelect;
        HashSet<IEdge> oEdgesToSelect;

        Boolean bMouseModeIsAddToSelection =
            (m_eMouseMode == MouseMode.AddToSelection);

        Boolean bMouseModeIsSubtractFromSelection =
            (m_eMouseMode == MouseMode.SubtractFromSelection);

        Boolean bControlKeyIsPressed = ControlKeyIsPressed();

        if (bMouseModeIsAddToSelection || bMouseModeIsSubtractFromSelection ||
            bControlKeyIsPressed)
        {
            // The new selection gets added to or subtracted from the old
            // selection.

            oVerticesToSelect = new HashSet<IVertex>(m_oSelectedVertices);
            oEdgesToSelect = new HashSet<IEdge>(m_oSelectedEdges);
        }
        else
        {
            // The new selection replaces the old selection.

            oVerticesToSelect = new HashSet<IVertex>();
            oEdgesToSelect = new HashSet<IEdge>();
        }

        // Loop through the vertices that intersect the marquee rectangle.

        Rect oMarqueeRectangle = m_oMarqueeBeingDragged.MarqueeRectangle;

        foreach ( IVertex oMarqueedVertex in
            m_oGraphDrawer.GetVerticesFromRectangle(oMarqueeRectangle) )
        {
            Boolean bAddToSelection = true;

            if (bControlKeyIsPressed)
            {
                // When a control key is pressed, the selected state of the
                // marqueed vertices should be inverted.

                bAddToSelection = !VertexOrEdgeIsSelected(oMarqueedVertex);
            }
            else if (bMouseModeIsSubtractFromSelection)
            {
                bAddToSelection = false;
            }

            if (bAddToSelection)
            {
                oVerticesToSelect.Add(oMarqueedVertex);
            }
            else
            {
                oVerticesToSelect.Remove(oMarqueedVertex);
            }

            if (m_bMouseAlsoSelectsIncidentEdges)
            {
                // Also loop through the vertex's incident edges.

                foreach (IEdge oEdge in oMarqueedVertex.IncidentEdges)
                {
                    if (bAddToSelection)
                    {
                        oEdgesToSelect.Add(oEdge);
                    }
                    else
                    {
                        oEdgesToSelect.Remove(oEdge);
                    }
                }
            }
        }

        SetSelected(oVerticesToSelect, oEdgesToSelect);
    }

    //*************************************************************************
    //  Method: ZoomViaMouse()
    //
    /// <summary>
    /// Zooms the graph via the mouse.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseEventArgs that contains the event data.
    /// </param>
    ///
    /// <param name="dGraphZoomFactor">
    /// Factor by which to multiply the current value of <see
    /// cref="GraphZoom" />.  Must be greater than zero.
    /// </param>
    //*************************************************************************

    protected void
    ZoomViaMouse
    (
        MouseEventArgs e,
        Double dGraphZoomFactor
    )
    {
        Debug.Assert(e != null);
        Debug.Assert(dGraphZoomFactor > 0);
        AssertValid();

        // Do nothing if the drawing isn't in a stable state or a drag is in
        // progress.

        if ( this.IsLayingOutGraph || DragMightBeInProgress() )
        {
            return;
        }

        Double dGraphZoom = this.GraphZoom;

        Double dNewGraphZoom = dGraphZoom * dGraphZoomFactor;
        dNewGraphZoom = Math.Min(dNewGraphZoom, MaximumGraphZoom);
        dNewGraphZoom = Math.Max(dNewGraphZoom, MinimumGraphZoom);

        if (dNewGraphZoom == dGraphZoom)
        {
            return;
        }

        // Set the center of the zoom to the mouse position.  Note that the
        // mouse position is affected by the ScaleTransform used for this
        // control's layout transform and needs to be adjusted for this.

        Point oMousePosition = e.GetPosition(this);

        ScaleTransform oScaleTransformForRender = this.ScaleTransformForRender;
        oScaleTransformForRender.CenterX = oMousePosition.X;
        oScaleTransformForRender.CenterY = oMousePosition.Y;

        // Zoom the graph.

        SetGraphZoom(dNewGraphZoom, false);

        // That caused the point under the mouse to shift.  Adjust the
        // translation to shift the point back.

        Point oNewMousePosition = e.GetPosition(this);

        TranslateTransform oTranslateTransformForRender =
            this.TranslateTransformForRender;

        oTranslateTransformForRender.X +=
            (oNewMousePosition.X - oMousePosition.X) * dNewGraphZoom;

        oTranslateTransformForRender.Y +=
            (oNewMousePosition.Y - oMousePosition.Y) * dNewGraphZoom;

        LimitTranslation();
    }

    //*************************************************************************
    //  Method: FireSelectionChanged()
    //
    /// <summary>
    /// Fires the <see cref="SelectionChanged" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireSelectionChanged()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.SelectionChanged);
    }

    //*************************************************************************
    //  Method: FireGraphMouseDown()
    //
    /// <summary>
    /// Fires the <see cref="GraphMouseDown" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oMouseButtonEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Clicked vertex if the user clicked on a vertex, or null if he didn't.
    /// </param>
    //*************************************************************************

    protected void
    FireGraphMouseDown
    (
        MouseButtonEventArgs oMouseButtonEventArgs,
        IVertex oVertex
    )
    {
        Debug.Assert(oMouseButtonEventArgs != null);
        AssertValid();

        FireGraphMouseButtonEvent(this.GraphMouseDown, oMouseButtonEventArgs,
            oVertex);
    }

    //*************************************************************************
    //  Method: FireGraphMouseUp()
    //
    /// <summary>
    /// Fires the <see cref="GraphMouseUp" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oMouseButtonEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Clicked vertex if the user clicked on a vertex, or null if he didn't.
    /// </param>
    //*************************************************************************

    protected void
    FireGraphMouseUp
    (
        MouseButtonEventArgs oMouseButtonEventArgs,
        IVertex oVertex
    )
    {
        Debug.Assert(oMouseButtonEventArgs != null);
        AssertValid();

        FireGraphMouseButtonEvent(this.GraphMouseUp, oMouseButtonEventArgs,
            oVertex);
    }

    //*************************************************************************
    //  Method: FireVertexClick()
    //
    /// <summary>
    /// Fires the <see cref="VertexClick" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// Clicked vertex.  Can't be null.
    /// </param>
    //*************************************************************************

    protected void
    FireVertexClick
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        FireVertexEvent(this.VertexClick, oVertex);
    }

    //*************************************************************************
    //  Method: FireVertexDoubleClick()
    //
    /// <summary>
    /// Fires the <see cref="VertexDoubleClick" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// Double-clicked vertex.  Can't be null.
    /// </param>
    //*************************************************************************

    protected void
    FireVertexDoubleClick
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        FireVertexEvent(this.VertexDoubleClick, oVertex);
    }

    //*************************************************************************
    //  Method: FireVertexMouseHover()
    //
    /// <summary>
    /// Fires the <see cref="VertexMouseHover" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// Hovered vertex.  Can't be null.
    /// </param>
    //*************************************************************************

    protected void
    FireVertexMouseHover
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        FireVertexEvent(this.VertexMouseHover, oVertex);
    }

    //*************************************************************************
    //  Method: FireVertexMouseLeave()
    //
    /// <summary>
    /// Fires the <see cref="VertexMouseLeave" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireVertexMouseLeave()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.VertexMouseLeave);
    }

    //*************************************************************************
    //  Method: FireGraphZoomChanged()
    //
    /// <summary>
    /// Fires the <see cref="GraphZoomChanged" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireGraphZoomChanged()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.GraphZoomChanged);
    }

    //*************************************************************************
    //  Method: FireGraphScaleChanged()
    //
    /// <summary>
    /// Fires the <see cref="GraphScaleChanged" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireGraphScaleChanged()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.GraphScaleChanged);
    }

    //*************************************************************************
    //  Method: FireGraphTranslationChanged()
    //
    /// <summary>
    /// Fires the <see cref="GraphTranslationChanged" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireGraphTranslationChanged()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.GraphTranslationChanged);
    }

    //*************************************************************************
    //  Method: FireVerticesMoved()
    //
    /// <summary>
    /// Fires the <see cref="VerticesMoved" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oMovedVertices">
    /// A collection of one or more vertices that were moved.
    /// </param>
    //*************************************************************************

    protected void
    FireVerticesMoved
    (
        ICollection<IVertex> oMovedVertices
    )
    {
        Debug.Assert(oMovedVertices != null);
        AssertValid();

        VerticesMovedEventHandler oVerticesMoved = this.VerticesMoved;

        if (oVerticesMoved != null)
        {
            oVerticesMoved( this,
                new VerticesMovedEventArgs(oMovedVertices) );
        }
    }

    //*************************************************************************
    //  Method: FireLayingOutGraph()
    //
    /// <summary>
    /// Fires the <see cref="LayingOutGraph" /> event if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    FireLayingOutGraph()
    {
        AssertValid();

        EventUtil.FireEvent(this, this.LayingOutGraph);
    }

    //*************************************************************************
    //  Method: FireGraphLaidOut()
    //
    /// <summary>
    /// Fires the <see cref="GraphLaidOut" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oAsyncCompletedEventArgs">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    FireGraphLaidOut
    (
        AsyncCompletedEventArgs oAsyncCompletedEventArgs
    )
    {
        Debug.Assert(oAsyncCompletedEventArgs != null);
        AssertValid();

        AsyncCompletedEventHandler oGraphLaidOut = this.GraphLaidOut;

        if (oGraphLaidOut != null)
        {
            oGraphLaidOut(this, oAsyncCompletedEventArgs);
        }
    }

    //*************************************************************************
    //  Method: FirePreviewVertexToolTipShown()
    //
    /// <summary>
    /// Fires the <see cref="PreviewVertexToolTipShown" /> event if
    /// appropriate.
    /// </summary>
    ///
    /// <param name="oVertexToolTipShownEventArgs">
    /// Event arguments.  The event handler may modify the <see
    /// cref="VertexToolTipShownEventArgs.VertexToolTip" /> property.
    /// </param>
    //*************************************************************************

    protected void
    FirePreviewVertexToolTipShown
    (
        VertexToolTipShownEventArgs oVertexToolTipShownEventArgs
    )
    {
        Debug.Assert(oVertexToolTipShownEventArgs != null);
        AssertValid();

        VertexToolTipShownEventHandler oVertexToolTipShownEventHandler =
            this.PreviewVertexToolTipShown;

        if (oVertexToolTipShownEventHandler != null)
        {
            oVertexToolTipShownEventHandler(this,
                oVertexToolTipShownEventArgs);
        }

    }

    //*************************************************************************
    //  Method: FireVertexEvent()
    //
    /// <summary>
    /// Fires an event with the signature VertexEventHandler.
    /// </summary>
    ///
    /// <param name="oVertexEventHandler">
    /// Event handler, or null if the event isn't being handled.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex associated with the event.  Can't be null.
    /// </param>
    //*************************************************************************

    protected void
    FireVertexEvent
    (
        VertexEventHandler oVertexEventHandler,
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        if (oVertexEventHandler != null)
        {
            oVertexEventHandler( this, new VertexEventArgs(oVertex) );
        }
    }

    //*************************************************************************
    //  Method: FireGraphMouseButtonEvent()
    //
    /// <summary>
    /// Fires an event with the signature GraphMouseButtonEventHandler.
    /// </summary>
    ///
    /// <param name="oGraphMouseButtonEventHandler">
    /// Event handler, or null if the event isn't being handled.
    /// </param>
    ///
    /// <param name="oMouseButtonEventArgs">
    /// Standard mouse event arguments.
    /// </param>
    ///
    /// <param name="oVertex">
    /// Vertex associated with the event.  Can be null.
    /// </param>
    //*************************************************************************

    protected void
    FireGraphMouseButtonEvent
    (
        GraphMouseButtonEventHandler oGraphMouseButtonEventHandler,
        MouseButtonEventArgs oMouseButtonEventArgs,
        IVertex oVertex
    )
    {
        Debug.Assert(oMouseButtonEventArgs != null);
        AssertValid();

        if (oGraphMouseButtonEventHandler != null)
        {
            oGraphMouseButtonEventHandler( this,
                new GraphMouseButtonEventArgs(
                    oMouseButtonEventArgs, oVertex) );
        }
    }

    //*************************************************************************
    //  Method: MeasureOverride()
    //
    /// <summary>
    /// When overridden in a derived class, measures the size in layout
    /// required for child elements and determines a size for the
    /// FrameworkElement-derived class. 
    /// </summary>
    ///
    /// <param name="availableSize">
    /// The available size that this element can give to child elements.
    /// Infinity can be specified as a value to indicate that the element will
    /// size to whatever content is available.
    /// </param>
    ///
    /// <returns>
    /// The size that this element determines it needs during layout, based on
    /// its calculations of child element sizes.
    /// </returns>
    //*************************************************************************

    protected override Size
    MeasureOverride
    (
        Size availableSize
    )
    {
        AssertValid();

        if (m_oVertexToolTip != null)
        {
            m_oVertexToolTip.Measure(availableSize);
        }

        Size oDesiredSize;

        if (availableSize.Width == Double.PositiveInfinity ||
            availableSize.Height == Double.PositiveInfinity)
        {
            oDesiredSize = new Size(1000, 1000);
        }
        else
        {
            oDesiredSize = availableSize;
        }

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: MeasureOverride: availableSize=" +
            availableSize + ", DesiredSize=" + oDesiredSize);
        #endif

        return (oDesiredSize);
    }

    //*************************************************************************
    //  Method: ArrangeOverride()
    //
    /// <summary>
    /// When overridden in a derived class, positions child elements and
    /// determines a size for a FrameworkElement derived class. 
    /// </summary>
    ///
    /// <param name="finalSize">
    /// The final area within the parent that this element should use to
    /// arrange itself and its children.
    /// </param>
    ///
    /// <returns>
    /// The actual size used.
    /// </returns>
    //*************************************************************************

    protected override Size
    ArrangeOverride
    (
        Size finalSize
    )
    {
        AssertValid();

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: ArrangeOverride: finalSize=" +
            finalSize);
        #endif

        if (m_oVertexToolTip != null)
        {
            // The height of the visible cursor is needed here, but that
            // doesn't seem to be available in any API.  You can get the cursor
            // size, but that size represents the entire cursor, part of which
            // may be transparent.  Several posts, including the following,
            // indicate that getting the visible height may not be practical:
            //
            // http://www.codeguru.com/forum/showthread.php?threadid=449040
            //
            // As a workaround, use just a hard-coded fraction of the cursor
            // height.  This works fine for the standard and large cursors.  If
            // extra large cursors are used, the cursor intrudes a bit into the
            // tooltip.
            //
            // This has been tested on both 96 and 120 DPI screen resolutions.

            Double dCursorHeight = SystemParameters.CursorHeight * 0.75;

            // Note that Mouse.GetPosition() retrieves a position that is
            // relative to the control's origin, which won't be the viewport
            // origin if the control is zoomed or panned by the render
            // transform.  We want a position relative to the viewpoint origin,
            // so transform the returned point.

            Point oMousePosition = this.RenderTransform.Transform(
                Mouse.GetPosition(this) );

            Rect oToolTipRectangle = new Rect(
                new Point(oMousePosition.X, oMousePosition.Y + dCursorHeight),
                m_oVertexToolTip.DesiredSize);

            // Limit the tooltip to the graph rectangle.

            Rect oBoundedToolTipRectangle =
                WpfGraphicsUtil.MoveRectangleWithinBounds(oToolTipRectangle,
                    new Rect(new Point(0, 0), finalSize), true);

            if (oBoundedToolTipRectangle.Bottom == finalSize.Height)
            {
                // The tooltip is at the bottom of the graph rectangle, where
                // it would be partially obscured by the cursor.  Move it so
                // its bottom is at the top of the cursor.

                oBoundedToolTipRectangle.Offset(0, 
                    oMousePosition.Y - oBoundedToolTipRectangle.Bottom
                    );
            }

            // The position has been computed relative to the viewport origin.
            // Transform it to be relative to the control's origin.

            oBoundedToolTipRectangle = new Rect(

                this.RenderTransform.Inverse.Transform(
                    oBoundedToolTipRectangle.Location),

                    oBoundedToolTipRectangle.Size
                );

            m_oVertexToolTip.Arrange(oBoundedToolTipRectangle);
        }

        return (finalSize);
    }

    //*************************************************************************
    //  Method: OnRender()
    //
    /// <summary>
    /// Renders the control.
    /// </summary>
    ///
    /// <param name="drawingContext">
    /// The drawing instructions for a specific element. This context is
    /// provided to the layout system.
    /// </param>
    //*************************************************************************

    protected override void
    OnRender
    (
        DrawingContext drawingContext
    )
    {
        AssertValid();

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: OnRender(), m_eLayoutState = " +
            m_eLayoutState);
        #endif

        LayOutOrDrawGraph();
    }

    //*************************************************************************
    //  Method: OnRenderSizeChanged()
    //
    /// <summary>
    /// Raises the SizeChanged event, using the specified information as part
    /// of the eventual event data.
    /// </summary>
    ///
    /// <param name="sizeInfo">
    /// Details of the old and new size involved in the change.
    /// </param>
    //*************************************************************************

    protected override void
    OnRenderSizeChanged
    (
        SizeChangedInfo sizeInfo
    )
    {
        AssertValid();

        // Note that this method gets called when the layout transform is
        // modified, not just when the control is resized.

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: OnRenderSizeChanged(),"
            + " sizeInfo.NewSize = " + sizeInfo.NewSize);
        #endif

        base.OnRenderSizeChanged(sizeInfo);

        if (sizeInfo.NewSize.Width == 0 || sizeInfo.NewSize.Height == 0)
        {
            // The control is in an intermediate state.  Its size will
            // eventually stabilize to a non-empty rectangle.

            #if TRACE_LAYOUT_AND_DRAW
            Debug.WriteLine("NodeXLControl: OnRenderSizeChanged() skipped");
            #endif

            return;
        }

        // The next block is an ugly workaround for the following problem.
        //
        // The center of the graph zoom should initially be placed at the
        // center of the control, so that if the zoom is first increased via
        // the GraphZoom property, the control will zoom from the center.  (If
        // the zoom is first increased via the mouse wheel, the zoom center
        // will be placed at the mouse location.)  This can't be done until the
        // final control size is known.
        //
        // A Loaded handler would be a logical place to do this, but the final
        // control size is not always available during the Loaded event,
        // depending on how the control is anchored or docked within its
        // parent.
        //
        // A workaround would be to set this control's RenderTransformOrigin to
        // (0.5, 0.5) when the transforms are first created, but the control's
        // transforms are designed to work with this property left at its
        // default value of the origin.
        //
        // TODO: Fix this!

        if (!m_bGraphZoomCentered && this.IsLoaded)
        {
            m_bGraphZoomCentered = true;
            CenterGraphZoom();
        }

        if (m_eLayoutState != LayoutState.Stable)
        {
            // The new size will be detected by the LayoutState.LayoutCompleted
            // case in OnRender().

            return;
        }

        // Make sure the size has actually changed.  This event fires once at
        // startup and should be ignored then.

        System.Drawing.Rectangle oLastGraphRectangle =
            m_oLastLayoutContext.GraphRectangle;

        System.Drawing.Rectangle oNewGraphRectangle =
            WpfGraphicsUtil.RectToRectangle( new Rect(sizeInfo.NewSize) );

        if (
            oLastGraphRectangle.Width != oNewGraphRectangle.Width
            ||
            oLastGraphRectangle.Height != oNewGraphRectangle.Height
            )
        {
            m_eLayoutState = LayoutState.TransformRequired;
            LayOutOrDrawGraph();

            // For the case where this method was called because the layout
            // transform was modified, make sure the graph wasn't moved too
            // far by the transform.

            LimitTranslation();
        }
    }

    //*************************************************************************
    //  Method: OnKeyDown()
    //
    /// <summary>
    /// Handles the KeyDown event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnKeyDown
    (
        System.Windows.Input.KeyEventArgs e
    )
    {
        AssertValid();

        if (this.IsLayingOutGraph)
        {
            return;
        }

        // Check for an arrow key.  An arrow key by itself moves the selected
        // vertices a small distance.  An arrow key combined with a shift key
        // moves them a larger distance.

        const Single SmallDistance = 1;
        const Single LargeDistance = 10;

        Single fMoveDistance =
            ( Keyboard.IsKeyDown(Key.LeftShift) ||
            Keyboard.IsKeyDown(Key.RightShift) ) ?
            LargeDistance : SmallDistance;

        Key eKey = e.Key;

        switch (eKey)
        {
            case Key.Left:

                MoveSelectedVertices(-fMoveDistance, 0);
                return;

            case Key.Right:

                MoveSelectedVertices(fMoveDistance, 0);
                return;

            case Key.Up:

                MoveSelectedVertices(0, -fMoveDistance);
                return;

            case Key.Down:

                MoveSelectedVertices(0, fMoveDistance);
                return;

            default:

                break;
        }

        base.OnKeyDown(e);
    }

    //*************************************************************************
    //  Method: OnMouseDown()
    //
    /// <summary>
    /// Handles the MouseDown event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseButtonEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseDown
    (
        MouseButtonEventArgs e
    )
    {
        AssertValid();

        // Do nothing if the drawing isn't in a stable state.

        if (this.IsLayingOutGraph)
        {
            return;
        }

        // Remove any vertex tooltip that might exist and reset the helper
        // object that figures out when to show tooltips.

        ResetVertexToolTipTracker();

        if ( DragMightBeInProgress() )
        {
            // This can occur if the user clicks another button while dragging
            // with the left button.

            return;
        }

        // Check whether the user clicked on a vertex.

        Point oMouseLocation = e.GetPosition(this);
        IVertex oClickedVertex;

        Boolean bVertexClicked =
            TryGetVertexFromPoint(oMouseLocation, out oClickedVertex);

        FireGraphMouseDown(e, oClickedVertex);

        if (bVertexClicked)
        {
            FireVertexClick(oClickedVertex);

            if (e.ClickCount == 2)
            {
                FireVertexDoubleClick(oClickedVertex);
            }
        }

        // Some drag operations can be cancelled with the Escape key, so
        // capture keyboard focus.

        Keyboard.Focus(this);

        if (m_eMouseMode == MouseMode.DoNothing)
        {
            return;
        }

        switch (e.ChangedButton)
        {
            case MouseButton.Left:

                OnMouseDownLeft(e, oMouseLocation, oClickedVertex);
                break;

            case MouseButton.Middle:

                OnMouseDownMiddle(oMouseLocation);
                break;

            case MouseButton.Right:

                OnMouseDownRight(oMouseLocation, oClickedVertex);
                break;

            default:

                break;
        }
    }

    //*************************************************************************
    //  Method: OnMouseDownLeft()
    //
    /// <summary>
    /// Handles the MouseDown event for the left mouse button.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseButtonEventArgs that contains the event data.
    /// </param>
    ///
    /// <param name="oMouseLocation">
    /// Mouse location, relative to the control.
    /// </param>
    ///
    /// <param name="oClickedVertex">
    /// The vertex that was clicked, or null if an empty area of the graph was
    /// clicked.
    /// </param>
    //*************************************************************************

    protected void
    OnMouseDownLeft
    (
        MouseButtonEventArgs e,
        Point oMouseLocation,
        IVertex oClickedVertex
    )
    {
        AssertValid();

        if ( m_eMouseMode == MouseMode.Translate ||
            Keyboard.IsKeyDown(Key.Space) )
        {
            // The user might want to translate the zoomed graph by dragging
            // with the mouse.

            StartTranslationDrag(oMouseLocation);
            return;
        }

        if (m_eMouseMode == MouseMode.ZoomIn)
        {
            ZoomViaMouse(e, MouseLeftClickZoomFactor);
            return;
        }

        if (m_eMouseMode == MouseMode.ZoomOut)
        {
            ZoomViaMouse(e, 1.0 / MouseLeftClickZoomFactor);
            return;
        }

        if (oClickedVertex == null)
        {
            // The user clicked on part of the graph not covered by a vertex.

            OnMouseDownLeftVertexNotClicked(oMouseLocation);
            return;
        }

        // If the control key is pressed, clicking a vertex should invert its
        // selected state.  Otherwise...
        //
        // In Select mode, clicking an unselected vertex should clear the
        // selection and then select the vertex.  Clicking a selected vertex
        // should leave the vertex selected.
        //
        // In AddToSelection mode, clicking a vertex should select it.
        //
        // In SubtractFromSelection mode, clicking a vertex should deselect
        // it.

        Boolean bClickedVertexIsSelected = VertexOrEdgeIsSelected(
            oClickedVertex);

        Boolean bSelectClickedVertex = true;

        if ( ControlKeyIsPressed() )
        {
            bSelectClickedVertex = !bClickedVertexIsSelected;
        }
        else if (m_eMouseMode == MouseMode.Select)
        {
            if (!bClickedVertexIsSelected)
            {
                SetAllVerticesSelected(false);
                SetAllEdgesSelected(false);
            }
        }
        else if (m_eMouseMode == MouseMode.SubtractFromSelection)
        {
            bSelectClickedVertex = false;
        }

        SetVertexSelected(oClickedVertex, bSelectClickedVertex,
            m_bMouseAlsoSelectsIncidentEdges);

        // In Select and AddToSelection mode, double-clicking a vertex provides
        // special behavior.

        if (
            Keyboard.Modifiers != 0
            ||
            (m_eMouseMode != MouseMode.Select &&
                m_eMouseMode != MouseMode.AddToSelection)
            )
        {
            m_oDoubleClickedVertexInfo = null;
        }
        else if (e.ClickCount == 2)
        {
            OnVertexDoubleClickLeft(oClickedVertex);
        }
        else if (m_oDoubleClickedVertexInfo != null &&
            m_oDoubleClickedVertexInfo.Vertex != oClickedVertex)
        {
            // (The above "else if" test is required because double-clicking a
            // vertex results in this method being called twice: once with
            // e.ClickCount = 1, and again with e.ClickCount = 2.  We don't
            // want to clear m_oDoubleClickedVertexInfo unnecessarily during
            // this sequence.)

            m_oDoubleClickedVertexInfo = null;
        }

        if (
            m_bAllowVertexDrag
            &&
            (m_eMouseMode == MouseMode.Select ||
                m_eMouseMode == MouseMode.AddToSelection)
            &&
            m_oSelectedVertices.Count > 0
            )
        {
            // The user might want to drag the selected vertices.  Save the
            // mouse location for use within the MouseMove event.

            m_oVerticesBeingDragged = new DraggedVertices(
                m_oSelectedVertices.ToArray(), oMouseLocation,
                this.GraphRectangle, m_oLayout.Margin);

            this.Cursor = Cursors.ScrollAll;
            Mouse.Capture(this);
        }
    }

    //*************************************************************************
    //  Method: OnMouseDownLeftVertexNotClicked()
    //
    /// <summary>
    /// Handles the MouseDown event for the left mouse button when the user
    /// clicked on part of the graph not covered by a vertex.
    /// </summary>
    ///
    /// <param name="oMouseLocation">
    /// Mouse location, relative to the control.
    /// </param>
    //*************************************************************************

    protected void
    OnMouseDownLeftVertexNotClicked
    (
        Point oMouseLocation
    )
    {
        AssertValid();

        m_oDoubleClickedVertexInfo = null;

        // When a control key is pressed, the selected state of the marqueed
        // vertices should be inverted.

        if ( m_eMouseMode == MouseMode.Select && !ControlKeyIsPressed() )
        {
            DeselectAll();
        }

        if (this.Graph.Vertices.Count > 0)
        {
            // The user might want to drag a marquee.  Save the mouse location
            // for use within the MouseMove event.

            m_oMarqueeBeingDragged = new DraggedMarquee(oMouseLocation,
                this.GraphRectangle, m_oLayout.Margin);

            this.Cursor = GetCursorForMarqueeDrag();
            Mouse.Capture(this);
        }
    }

    //*************************************************************************
    //  Method: OnMouseDownMiddle()
    //
    /// <summary>
    /// Handles the MouseDown event for the middle mouse button.
    /// </summary>
    ///
    /// <param name="oMouseLocation">
    /// Mouse location, relative to the control.
    /// </param>
    //*************************************************************************

    protected void
    OnMouseDownMiddle
    (
        Point oMouseLocation
    )
    {
        AssertValid();

        // The user might want to translate the zoomed graph by dragging with
        // the mouse.

        StartTranslationDrag(oMouseLocation);
    }

    //*************************************************************************
    //  Method: OnMouseDownRight()
    //
    /// <summary>
    /// Handles the MouseDown event for the right mouse button.
    /// </summary>
    ///
    /// <param name="oMouseLocation">
    /// Mouse location, relative to the control.
    /// </param>
    ///
    /// <param name="oClickedVertex">
    /// The vertex that was clicked, or null if an empty area of the graph was
    /// clicked.
    /// </param>
    //*************************************************************************

    protected void
    OnMouseDownRight
    (
        Point oMouseLocation,
        IVertex oClickedVertex
    )
    {
        AssertValid();

        if (oClickedVertex == null)
        {
            // Right-clicking a part of the graph not covered by a vertex
            // should do nothing.

            return;
        }

        switch (m_eMouseMode)
        {
            case MouseMode.Select:
            case MouseMode.AddToSelection:

                break;

            default:

                return;
        }

        // In Select mode, right-clicking an unselected vertex should clear the
        // selection and then select the vertex.
        //
        // In AddToSelection mode, right-clicking an unselected vertex should
        // select the vertex.

        if (
            m_eMouseMode == MouseMode.Select &&
            !VertexOrEdgeIsSelected(oClickedVertex)
            )
        {
            SetAllVerticesSelected(false);
            SetAllEdgesSelected(false);
        }

        SetVertexSelected(oClickedVertex, true,
            m_bMouseAlsoSelectsIncidentEdges);
    }

    //*************************************************************************
    //  Method: OnMouseMove()
    //
    /// <summary>
    /// Handles the MouseMove event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseMove
    (
        MouseEventArgs e
    )
    {
        AssertValid();

        // Do nothing if the drawing isn't in a stable state.

        if (this.IsLayingOutGraph)
        {
            return;
        }

        // Check for a mouse drag that might be in progress.

        CheckForVertexDragOnMouseMove(e);
        CheckForMarqueeDragOnMouseMove(e);
        CheckForTranslationDragOnMouseMove(e);

        // Check whether tooltip-related actions need to be taken.

        CheckForToolTipsOnMouseMove(e);
    }

    //*************************************************************************
    //  Method: OnMouseUp()
    //
    /// <summary>
    /// Handles the MouseUp event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseButtonEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseUp
    (
        MouseButtonEventArgs e
    )
    {
        AssertValid();

        // Restore the default cursor and release the mouse capture.

        this.Cursor = null;
        Mouse.Capture(null);

        // Do nothing if the drawing isn't in a stable state.

        if (this.IsLayingOutGraph)
        {
            return;
        }

        // Check whether the user clicked on a vertex.

        Point oMouseLocation = e.GetPosition(this);
        IVertex oClickedVertex;

        TryGetVertexFromPoint(oMouseLocation, out oClickedVertex);

        FireGraphMouseUp(e, oClickedVertex);

        // If it was the right button that was released, do nothing else.

        if (e.LeftButton == MouseButtonState.Released)
        {
            // Check for a mouse drag that might be in progress.

            CheckForVertexDragOnMouseUp(e);
            CheckForMarqueeDragOnMouseUp(e);
            CheckForTranslationDragOnMouseUp(e);
        }
    }

    //*************************************************************************
    //  Method: OnMouseLeave()
    //
    /// <summary>
    /// Handles the MouseLeave event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseLeave
    (
        MouseEventArgs e
    )
    {
        AssertValid();

        // Remove any vertex tooltip that might exist and reset the helper
        // object that figures out when to show tooltips.

        ResetVertexToolTipTracker();
    }

    //*************************************************************************
    //  Method: OnMouseWheel()
    //
    /// <summary>
    /// Handles the MouseWheel event.
    /// </summary>
    ///
    /// <param name="e">
    /// The MouseWheelEventArgs that contains the event data.
    /// </param>
    //*************************************************************************

    protected override void
    OnMouseWheel
    (
        MouseWheelEventArgs e
    )
    {
        AssertValid();

        if (m_eMouseMode == MouseMode.DoNothing)
        {
            return;
        }

        ZoomViaMouse(e, e.Delta > 0 ?
            MouseWheelZoomFactor : 1 / MouseWheelZoomFactor);

        e.Handled = true;
    }

    //*************************************************************************
    //  Method: OnVertexDoubleClickLeft()
    //
    /// <summary>
    /// Performs tasks required when a vertex is double-clicked with the left
    /// mouse button.
    /// </summary>
    ///
    /// <param name="oDoubleClickedVertex">
    /// The vertex that was double-clicked.
    /// </param>
    //*************************************************************************

    protected void
    OnVertexDoubleClickLeft
    (
        IVertex oDoubleClickedVertex
    )
    {
        Debug.Assert(oDoubleClickedVertex != null);
        AssertValid();

        // Double-clicking a vertex the first time should select the vertex's
        // level-1 subgraph, double-clicking it again should select its level-2
        // subgraph, and so on.

        if (m_oDoubleClickedVertexInfo == null ||
            m_oDoubleClickedVertexInfo.Vertex != oDoubleClickedVertex)
        {
            // Note that the DoubleClickedVertexInfo constructor sets the
            // Levels property to 0.

            m_oDoubleClickedVertexInfo = new DoubleClickedVertexInfo(
                oDoubleClickedVertex);
        }

        m_oDoubleClickedVertexInfo.Levels++;

        Dictionary<IVertex, Int32> oSubgraphVertices;
        HashSet<IEdge> oSubgraphEdges;

        SubgraphCalculator.GetSubgraph(oDoubleClickedVertex,
            m_oDoubleClickedVertexInfo.Levels,
            m_bMouseAlsoSelectsIncidentEdges, out oSubgraphVertices,
            out oSubgraphEdges);

        foreach (IVertex oSubgraphVertex in oSubgraphVertices.Keys)
        {
            SetVertexSelectedInternal(oSubgraphVertex, true);
        }

        foreach (IEdge oSubgraphEdge in oSubgraphEdges)
        {
            SetEdgeSelectedInternal(oSubgraphEdge, true);
        }

        FireSelectionChanged();
    }

    //*************************************************************************
    //  Method: Layout_LayOutGraphCompleted()
    //
    /// <summary>
    /// Handles the LayOutGraphCompleted event on the m_oLayout object.
    /// </summary>
    ///
    /// <param name="oSender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="oAsyncCompletedEventArgs">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    Layout_LayOutGraphCompleted
    (
        Object oSender,
        AsyncCompletedEventArgs oAsyncCompletedEventArgs
    )
    {
        AssertValid();

        #if TRACE_LAYOUT_AND_DRAW
        Debug.WriteLine("NodeXLControl: Layout_LayOutGraphCompleted()");
        #endif

        if (oAsyncCompletedEventArgs.Error == null)
        {
            m_eLayoutState = LayoutState.LayoutCompleted;

            // The asynchronous layout has completed and now the graph needs to
            // be drawn.

            UpdateCollapsedGroupLocations(m_oGraph.Vertices);
            BundleAllEdgesIfAppropriate();

            LayOutOrDrawGraph();
        }
        else
        {
            m_eLayoutState = LayoutState.Stable;
        }

        FireGraphLaidOut(oAsyncCompletedEventArgs);
    }

    //*************************************************************************
    //  Method: EdgeDrawer_CurveStyleChanged()
    //
    /// <summary>
    /// Handles the CurveStyleChanged event on the EdgeDrawer object.
    /// </summary>
    ///
    /// <param name="oSender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="oEventArgs">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    EdgeDrawer_CurveStyleChanged
    (
        Object oSender,
        EventArgs oEventArgs
    )
    {
        AssertValid();

        BundleAllEdgesIfAppropriate();
    }

    //*************************************************************************
    //  Method: VertexToolTipTracker_ShowToolTip()
    //
    /// <summary>
    /// Handles the ShowToolTip event on the m_oVertexToolTipTracker object.
    /// </summary>
    ///
    /// <param name="oSource">
    /// Standard event arguments.
    /// </param>
    ///
    /// <param name="oToolTipTrackerEventArgs">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    private void
    VertexToolTipTracker_ShowToolTip
    (
        Object oSource,
        ToolTipTrackerEventArgs oToolTipTrackerEventArgs
    )
    {
        AssertValid();

        // Get the vertex that was hovered over.

        Debug.Assert(oToolTipTrackerEventArgs.Object is IVertex);

        IVertex oVertex = (IVertex)oToolTipTrackerEventArgs.Object;

        // Fire a VertexMouseHover event in case the application wants to take
        // additional action.

        FireVertexMouseHover(oVertex);

        if (!m_bShowVertexToolTips)
        {
            // Vertex tooltips aren't being shown, so no additional action is
            // required.

            return;
        }

        // Give PreviewVertexToolTipShown event handlers an opportunity to
        // override the default tooltip.

        VertexToolTipShownEventArgs oVertexToolTipShownEventArgs =
            new VertexToolTipShownEventArgs(oVertex);

        FirePreviewVertexToolTipShown(oVertexToolTipShownEventArgs);

        m_oVertexToolTip = oVertexToolTipShownEventArgs.VertexToolTip;

        if (m_oVertexToolTip == null)
        {
            // There was no default tooltip override.  Does the vertex have a
            // tooltip string stored in its metadata?

            Object oVertexToolTipAsObject;

            if ( !oVertex.TryGetValue(ReservedMetadataKeys.PerVertexToolTip,
                typeof(String), out oVertexToolTipAsObject) )
            {
                // No.  Nothing needs to be done.

                return;
            }

            // Create a default tooltip.

            m_oVertexToolTip = CreateDefaultVertexToolTip(
                (String)oVertexToolTipAsObject);
        }

        // The tooltip shouldn't zoom.  Compensate for a zoomed graph by
        // scaling the tooltip with the graph zoom's inverse.

        Double dScale = 1.0 / this.ScaleTransformForRender.ScaleX;
        m_oVertexToolTip.RenderTransform = new ScaleTransform(dScale, dScale);

        m_oGraphDrawer.AddVisualOnTopOfGraph(m_oVertexToolTip);

        // If this isn't called, MeasureOverride() may not be called and
        // m_oVertexToolTip won't get sized.

        this.InvalidateMeasure();
    }

    //*************************************************************************
    //  Method: VertexToolTipTracker_HideToolTip()
    //
    /// <summary>
    /// Handles the HideToolTip event on the m_oVertexToolTipTracker object.
    /// </summary>
    ///
    /// <param name="oSource">
    /// Standard event arguments.
    /// </param>
    ///
    /// <param name="oToolTipTrackerEventArgs">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    private void
    VertexToolTipTracker_HideToolTip
    (
        Object oSource,
        ToolTipTrackerEventArgs oToolTipTrackerEventArgs
    )
    {
        AssertValid();

        FireVertexMouseLeave();

        if (m_bShowVertexToolTips)
        {
            RemoveVertexToolTip();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        Debug.Assert(m_oGraph != null);
        Debug.Assert(m_oGraphDrawer != null);

        Debug.Assert(m_fEdgeBundlerStraightening >=
            MinimumEdgeBundlerStraightening);

        Debug.Assert(m_fEdgeBundlerStraightening <=
            MaximumEdgeBundlerStraightening);

        Debug.Assert(m_oLayout != null);
        Debug.Assert(m_oLastLayoutContext != null);
        // m_oLastGraphDrawingContext
        // m_eLayoutState
        // m_eMouseMode
        // m_bMouseAlsoSelectsIncidentEdges
        // m_bAllowVertexDrag
        // m_oVerticesBeingDragged
        // m_oMarqueeBeingDragged
        // m_oTranslationBeingDragged
        Debug.Assert(m_oSelectedVertices != null);
        Debug.Assert(m_oSelectedEdges != null);
        Debug.Assert(m_oCollapsedGroups != null);
        // m_oDoubleClickedVertexInfo
        // m_bShowVertexToolTips
        // m_oLastMouseMoveLocation
        Debug.Assert(m_oVertexToolTipTracker != null);
        // m_oVertexToolTip
        // m_bGraphZoomCentered
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value of the <see cref="GraphScale" /> property.  The value is
    /// 0.01.
    /// </summary>

    public const Double MinimumGraphScale = GraphDrawer.MinimumGraphScale;

    /// <summary>
    /// Maximum value of the <see cref="GraphScale" /> property.  The value is
    /// 10.0.
    /// </summary>

    public const Double MaximumGraphScale = GraphDrawer.MaximumGraphScale;

    /// <summary>
    /// Minimum value of the <see cref="GraphZoom" /> property.  The value is
    /// 1.0.
    /// </summary>

    public const Double MinimumGraphZoom = 1.0;

    /// <summary>
    /// Maximum value of the <see cref="GraphZoom" /> property.  The value is
    /// 10.0.
    /// </summary>

    public const Double MaximumGraphZoom = 10.0;

    /// <summary>
    /// Minimum value of the <see cref="EdgeBundlerStraightening" /> property.
    /// The value is 0.
    /// </summary>

    public const Single MinimumEdgeBundlerStraightening = 0F;

    /// <summary>
    /// Maximum value of the <see cref="EdgeBundlerStraightening" /> property.
    /// The value is 1.0.
    /// </summary>

    public const Single MaximumEdgeBundlerStraightening = 1.0F;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Zoom factor to use when the user zooms in with a mouse click.

    protected const Double MouseLeftClickZoomFactor = 1.50;

    /// Zoom factor to use when the user zooms in with the mouse wheel.

    protected const Double MouseWheelZoomFactor = 1.10;

    /// Number of milliseconds to wait before hiding a vertex tooltip.

    protected const Int32 VertexToolTipHideDelayMs = 60 * 60 * 1000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph being drawn.

    protected IGraph m_oGraph;

    /// Draws the graph onto a collection of Visual objects.

    protected GraphDrawer m_oGraphDrawer;

    /// Determines how much straightening is applied when edges are bundled.

    protected Single m_fEdgeBundlerStraightening;

    /// Object used to lay out the graph.

    protected ILayout m_oLayout;

    /// Layout context most recently used to lay out the graph.

    protected LayoutContext m_oLastLayoutContext;

    /// GraphDrawingContext most recently used to draw the graph, or null if
    /// the graph hasn't been drawn yet.

    protected GraphDrawingContext m_oLastGraphDrawingContext;

    /// Indicates the state of the graph's layout.

    protected LayoutState m_eLayoutState;

    /// Determines how the mouse can be used to interact with the graph.

    protected MouseMode m_eMouseMode;

    /// true if selecting or deselecting a vertex with the mouse also selects
    /// or deselects its incident edges.

    protected Boolean m_bMouseAlsoSelectsIncidentEdges;

    /// true if a vertex can be moved by dragging it with the mouse.

    protected Boolean m_bAllowVertexDrag;

    /// Vertex the user is dragging with the mouse, or null if a vertex isn't
    /// being dragged.

    protected DraggedVertices m_oVerticesBeingDragged;

    /// Marquee the user is dragging with the mouse, or null if a marquee isn't
    /// being dragged.

    protected DraggedMarquee m_oMarqueeBeingDragged;

    /// The translation the user is dragging with the mouse, or null if a
    /// translation isn't being dragged.

    protected DraggedTranslation m_oTranslationBeingDragged;

    /// Selected vertices and edges.  HashSets are used instead of lists or
    /// arrays to prevent the same vertex or edge from being added twice.  The
    /// keys are IVertex or IEdge.

    protected HashSet<IVertex> m_oSelectedVertices;
    ///
    protected HashSet<IEdge> m_oSelectedEdges;

    /// Dictionary of the groups that are collapsed.  The key is the group name
    /// and the value is the vertex that represents the collapsed group.

    protected Dictionary<String, IVertex> m_oCollapsedGroups;

    /// Information about the vertex that was just double-clicked, or null if
    /// a vertex wasn't just double-clicked.

    protected DoubleClickedVertexInfo m_oDoubleClickedVertexInfo;

    /// true to show vertex tooltips.

    protected Boolean m_bShowVertexToolTips;

    /// Location of the mouse during the most recent MouseMove event, or
    /// (-1,-1) if that event hasn't fired yet.

    protected Point m_oLastMouseMoveLocation;

    /// Helper object for figuring out when to show vertex tooltips.

    private WpfToolTipTracker m_oVertexToolTipTracker;

    /// The vertex tooltip being displayed, or null if no vertex tooltip is
    /// being displayed.

    protected UIElement m_oVertexToolTip;

    /// See OnRenderSizeChanged().

    protected Boolean m_bGraphZoomCentered;
}

}
