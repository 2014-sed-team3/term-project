﻿
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;
using Smrf.WpfGraphicsLib;
using Smrf.AppLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: GraphDrawer
//
/// <summary>
/// Draws a NodeXL graph onto a collection of Visual objects.
/// </summary>
///
/// <remarks>
/// This is used to draw a NodeXL graph in a WPF application.  It contains a
/// collection of Visual objects that represent the graph's vertices and
/// edges.  Call <see cref="DrawGraph" /> to draw a laid-out NodeXL graph onto
/// the contained <see cref="GraphDrawer.VisualCollection" />.
///
/// <para>
/// <see cref="GraphDrawer" /> does not lay out the graph.  You should lay out
/// the graph using one of the provided layout classes before calling <see
/// cref="DrawGraph" />.
/// </para>
///
/// <para>
/// A <see cref="GraphDrawer" /> cannot be directly rendered and is typically
/// not used directly by an application.  Applications typically use one of two
/// other NodeXL graph-drawing classes:
/// </para>
///
/// <list type="number">
///
/// <item><description>
/// NodeXLControl, which is a FrameworkElement that wraps a <see
/// cref="GraphDrawer" /> and hosts its <see
/// cref="GraphDrawer.VisualCollection" />.  NodeXLControl is meant for use in
/// WPF desktop applications.  It automatically lays out the graph before
/// drawing it.
/// </description></item>
///
/// <item><description>
/// <see cref="NodeXLVisual" />, which is a Visual that wraps a <see
/// cref="GraphDrawer" /> and hosts its <see
/// cref="GraphDrawer.VisualCollection" />.  This is a lower-level alternative
/// to NodeXLControl and can be used anywhere a Visual is more appropriate than
/// a FrameworkElement.  Like <see cref="GraphDrawer" />, <see
/// cref="NodeXLVisual" /> does not lay out the graph before drawing it.
/// </description></item>
///
/// </list>
///
/// <para>
/// If you do use <see cref="GraphDrawer" /> directly, rendering the graph
/// requires a custom wrapper that hosts the GraphDrawer.<see
/// cref="GraphDrawer.VisualCollection" /> object.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphDrawer : DrawerBase
{
    //*************************************************************************
    //  Constructor: GraphDrawer()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDrawer" /> class.
    /// </summary>
    ///
    /// <param name="parentVisual">
    /// The parent of the contained <see
    /// cref="GraphDrawer.VisualCollection" />.  This is usually a
    /// FrameworkElement that is hosting the collection.
    /// </param>
    //*************************************************************************

    public GraphDrawer
    (
        Visual parentVisual
    )
    {
        Debug.Assert(parentVisual != null);

        m_oVisualCollection = new VisualCollection(parentVisual);
        m_oAllVertexDrawingVisuals = null;
        m_oUnselectedEdgeDrawingVisuals = null;
        m_oSelectedEdgeDrawingVisuals = null;
        m_oVertexDrawer = new VertexDrawer();
        m_oEdgeDrawer = new EdgeDrawer();
        m_oGroupDrawer = new GroupDrawer();
        m_oBackColor = SystemColors.WindowColor;
        m_oBackgroundImage = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VisualCollection
    //
    /// <summary>
    /// Gets the VisualCollection that contains the Visual objects representing
    /// the graph's vertices and edges.
    /// </summary>
    ///
    /// <value>
    /// The VisualCollection that contains the Visual objects representing the
    /// graph's vertices and edges.
    /// </value>
    ///
    /// <remarks>
    /// This should be treated as read-only and used only to host the graph in
    /// a FrameworkElement.  Its contents should not be modified.  If you want
    /// to add a Visual on top of the graph, call <see
    /// cref="AddVisualOnTopOfGraph" /> after calling <see cref="DrawGraph" />.
    ///
    /// <para>
    /// <see cref="DrawGraph" /> draws a NodeXL graph onto this collection.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public VisualCollection
    VisualCollection
    {
        get
        {
            AssertValid();

            return (m_oVisualCollection);
        }
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
    ///
    /// <remarks>
    /// This property is provided to allow the caller to access the <see
    /// cref="Wpf.VertexDrawer" /> properties and methods that affect the
    /// graph's appearance, such as <see cref="Wpf.VertexDrawer.Shape" />.
    /// </remarks>
    //*************************************************************************

    public VertexDrawer
    VertexDrawer
    {
        get
        {
            AssertValid();

            return (m_oVertexDrawer);
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
    ///
    /// <remarks>
    /// This property is provided to allow the caller to access the <see
    /// cref="Wpf.EdgeDrawer" /> properties that affect the graph's appearance,
    /// such as <see cref="Wpf.EdgeDrawer.Width" />.
    /// </remarks>
    //*************************************************************************

    public EdgeDrawer
    EdgeDrawer
    {
        get
        {
            AssertValid();

            return (m_oEdgeDrawer);
        }
    }

    //*************************************************************************
    //  Property: GroupDrawer
    //
    /// <summary>
    /// Gets the <see cref="Wpf.GroupDrawer" /> used to draw the graph's
    /// groups.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="Wpf.GroupDrawer" /> used to draw the graph's groups.
    /// </value>
    ///
    /// <remarks>
    /// This property is provided to allow the caller to access the <see
    /// cref="Wpf.GroupDrawer" /> properties and methods that affect the
    /// graph's appearance, such as <see cref="Wpf.GroupDrawer.SetFont" />.
    /// </remarks>
    //*************************************************************************

    public GroupDrawer
    GroupDrawer
    {
        get
        {
            AssertValid();

            return (m_oGroupDrawer);
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

            return (m_oBackColor);
        }

        set
        {
            m_oBackColor = value;

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

            return (m_oBackgroundImage);
        }

        set
        {
            m_oBackgroundImage = value;

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
    /// </remarks>
    //*************************************************************************

    public new Double
    GraphScale
    {
        get
        {
            AssertValid();

            Debug.Assert(m_oVertexDrawer.GraphScale ==
                m_oEdgeDrawer.GraphScale);

            Debug.Assert(m_oVertexDrawer.GraphScale ==
                m_oGroupDrawer.GraphScale);

            return (m_oVertexDrawer.GraphScale);
        }

        set
        {
            m_oVertexDrawer.GraphScale = m_oEdgeDrawer.GraphScale =
                m_oGroupDrawer.GraphScale = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: DrawGraph()
    //
    /// <summary>
    /// Draws a laid-out graph onto the contained collection of Visual objects.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to draw onto the contained collection of Visual objects.  The
    /// graph should have already been laid out.  You can use one of the
    /// supplied layout classes to do this.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// If you want to add a Visual on top of the graph, call <see
    /// cref="AddVisualOnTopOfGraph" /> after this method returns.
    ///
    /// <para>
    /// The collection of Visual objects is accessible via the <see
    /// cref="GraphDrawer.VisualCollection" /> property.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    DrawGraph
    (
        IGraph graph,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        // Implementation note:
        //
        // In a previous GDI+ implementation of this graph drawer, the edges
        // had to be drawn first to allow the vertices to cover the ends of the
        // edges.  That required a complex three-step drawing process: 1) allow
        // the vertex drawer to move each vertex if necessary to prevent the
        // vertex from falling outside the graph rectangle; 2) draw the edges
        // using the moved vertex locations; and 3) draw the vertices.
        //
        // This WPF implementation is simpler, for two reasons:
        //
        // 1. WPF uses retained-mode graphics, so covering the ends of the
        //    edges can be accomplished simply by adding
        //    m_oUnselectedEdgeDrawingVisuals to m_oVisualCollection before
        //    adding m_oAllVertexDrawingVisuals.  That means that the vertices
        //    can be drawn onto m_oAllVertexDrawingVisuals first, and the
        //    vertex drawer can move the vertices as necessary before drawing
        //    them.  A three-step process is no longer required.
        //
        // 2. The edges in this implementation don't actually need to be
        //    covered, because they terminate at the vertex boundaries instead
        //    of the vertex centers, as in the GDI+ implementation.

        m_oVisualCollection.Clear();

        DrawBackground(graph, graphDrawingContext);

        Visual oGroupVisual;

        if ( m_oGroupDrawer.TryDrawGroupRectangles(graph, graphDrawingContext,
            out oGroupVisual) )
        {
            m_oVisualCollection.Add(oGroupVisual);
        }

        if ( m_oGroupDrawer.TryDrawCombinedIntergroupEdges(
            graph, graphDrawingContext, out oGroupVisual) )
        {
            m_oVisualCollection.Add(oGroupVisual);
        }

        m_oAllVertexDrawingVisuals = new DrawingVisual();
        m_oUnselectedEdgeDrawingVisuals = new DrawingVisual();
        m_oSelectedEdgeDrawingVisuals = new DrawingVisual();

        // Draw the vertices after moving them if necessary.  Each vertex needs
        // to be individually hit-tested and possibly redrawn by
        // RedrawVertex(), so each vertex is put into its own DrawingVisual
        // that becomes a child of m_oAllVertexDrawingVisuals.
        //
        // Selected vertices should always be on top of unselected vertices, so
        // draw them first.  Note that this could also be accomplished by using
        // two DrawingVisual objects, m_oUnselectedVertexDrawingVisuals and
        // m_oSelectedVertexDrawingVisuals, in a manner similar to what is done
        // for edges.  However, vertices have to be hit-tested, and having two
        // DrawingVisual objects for two sets of vertices would complicate and
        // slow down hit-testing, so this solution is the simpler one.

        LinkedList<IVertex> oSelectedVertices = new LinkedList<IVertex>();

        foreach ( IVertex oVertex in GetVerticesToDraw(graph) )
        {
            if ( m_oVertexDrawer.GetDrawAsSelected(oVertex) )
            {
                oSelectedVertices.AddLast(oVertex);
            }
            else
            {
                DrawVertex(oVertex, graphDrawingContext);
            }
        }

        foreach (IVertex oVertex in oSelectedVertices)
        {
            DrawVertex(oVertex, graphDrawingContext);
        }

        oSelectedVertices = null;

        // Draw the edges.  The edges don't need to be hit-tested, but they
        // might need to be redrawn by RedrawEdge(), so each edge is put into
        // its own DrawingVisual that becomes a child of either
        // m_oUnselectedEdgeDrawingVisuals or m_oSelectedEdgeDrawingVisuals.

        foreach (IEdge oEdge in graph.Edges)
        {
            DrawEdge(oEdge, graphDrawingContext);
        }

        // Selected edges need to be drawn on top of all the vertices
        // (including selected vertices) to guarantee that they will be
        // visible; hence the addition order seen here.

        m_oVisualCollection.Add(m_oUnselectedEdgeDrawingVisuals);
        m_oVisualCollection.Add(m_oAllVertexDrawingVisuals);
        m_oVisualCollection.Add(m_oSelectedEdgeDrawingVisuals);

        if ( m_oGroupDrawer.TryDrawGroupLabels(graph, graphDrawingContext,
            out oGroupVisual) )
        {
            m_oVisualCollection.Add(oGroupVisual);
        }
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

        if (m_oAllVertexDrawingVisuals == null)
        {
            // The graph hasn't been drawn yet.

            return (false);
        }

        // The vertices are represented by DrawingVisual child objects of
        // m_oAllVertexDrawingVisuals.

        HitTestResult oHitTestResult =
            m_oAllVertexDrawingVisuals.HitTest(point);

        if (oHitTestResult != null)
        {
            DependencyObject oVisualHit = oHitTestResult.VisualHit;

            if ( typeof(DrawingVisual).IsInstanceOfType(oVisualHit) )
            {
                // Retrieve the vertex.

                vertex = RetrieveVertexFromDrawingVisual(
                    (DrawingVisual)oVisualHit);

                return (true);
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetVerticesFromRectangle()
    //
    /// <summary>
    /// Gets any vertices that intersect a specified <see cref="Rect" />.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// Rectangle to use.
    /// </param>
    ///
    /// <returns>
    /// A collection of vertices that intersect <paramref name="rectangle" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method return a collection of all vertices that intersect
    /// <paramref name="rectangle" />.  The returned collection may be empty
    /// but is never null.
    /// </remarks>
    //*************************************************************************

    public ICollection<IVertex>
    GetVerticesFromRectangle
    (
        Rect rectangle
    )
    {
        AssertValid();

        LinkedList<IVertex> oVertices = new LinkedList<IVertex>();

        if (m_oAllVertexDrawingVisuals != null)
        {
            // Hit-test using an anonymous delegate to avoid having to create
            // a named callback method.

            m_oAllVertexDrawingVisuals.HitTest(null,
            
                delegate (HitTestResult hitTestResult)
                {
                    DependencyObject oVisualHit = hitTestResult.VisualHit;

                    if ( typeof(DrawingVisual).IsInstanceOfType(oVisualHit) )
                    {
                        // Retrieve the vertex.

                        oVertices.AddLast( RetrieveVertexFromDrawingVisual(
                            (DrawingVisual)oVisualHit) );
                    }

                    return (HitTestResultBehavior.Continue);
                }, 

                new GeometryHitTestParameters(
                    new RectangleGeometry(rectangle) )
                );
        }

        return (oVertices);
    }

    //*************************************************************************
    //  Method: RedrawVertex()
    //
    /// <summary>
    /// Redraws a vertex that was drawn by <see cref="DrawGraph" />.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to redraw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.  This
    /// must be the same object that was passed to <see cref="DrawGraph" /> the
    /// last time the entire graph was drawn.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to redraw a vertex whose attributes (such as its
    /// selected state) have changed without incurring the overhead of
    /// redrawing the entire graph.
    /// </remarks>
    //*************************************************************************

    public void
    RedrawVertex
    (
        IVertex vertex,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(vertex != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        UndrawVertex(vertex, graphDrawingContext);
        DrawVertex(vertex, graphDrawingContext);
    }

    //*************************************************************************
    //  Method: RedrawEdge()
    //
    /// <summary>
    /// Redraws an edge that was drawn by <see cref="DrawGraph" />.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to redraw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.  This
    /// must be the same object that was passed to <see cref="DrawGraph" /> the
    /// last time the entire graph was drawn.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to redraw an edge whose attributes (such as its
    /// selected state) have changed without incurring the overhead of
    /// redrawing the entire graph.
    /// </remarks>
    //*************************************************************************

    public void
    RedrawEdge
    (
        IEdge edge,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(edge != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        UndrawEdge(edge, graphDrawingContext);
        DrawEdge(edge, graphDrawingContext);
    }

    //*************************************************************************
    //  Method: DrawNewVertex()
    //
    /// <summary>
    /// Draws a vertex that has been added to the graph but not yet drawn by
    /// <see cref="DrawGraph" />.
    /// </summary>
    ///
    /// <param name="newVertex">
    /// The new vertex to draw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.  This
    /// must be the same object that was passed to <see cref="DrawGraph" /> the
    /// last time the entire graph was drawn.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to draw a new vertex without incurring the overhead of
    /// redrawing the entire graph.
    /// </remarks>
    //*************************************************************************

    public void
    DrawNewVertex
    (
        IVertex newVertex,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(newVertex != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        DrawVertex(newVertex, graphDrawingContext);
    }

    //*************************************************************************
    //  Method: DrawNewEdge()
    //
    /// <summary>
    /// Draws an edge that has been added to the graph but not yet drawn by
    /// <see cref="DrawGraph" />.
    /// </summary>
    ///
    /// <param name="newEdge">
    /// The new edge to draw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.  This
    /// must be the same object that was passed to <see cref="DrawGraph" /> the
    /// last time the entire graph was drawn.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to draw a new edge without incurring the overhead of
    /// redrawing the entire graph.
    /// </remarks>
    //*************************************************************************

    public void
    DrawNewEdge
    (
        IEdge newEdge,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(newEdge != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        DrawEdge(newEdge, graphDrawingContext);
    }

    //*************************************************************************
    //  Method: UndrawVertex()
    //
    /// <summary>
    /// "Undraws" a vertex that was drawn by <see cref="DrawGraph" />.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to remove from the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.  This
    /// must be the same object that was passed to <see cref="DrawGraph" /> the
    /// last time the entire graph was drawn.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to undraw a vertex without incurring the overhead of
    /// redrawing the entire graph.
    /// </remarks>
    //*************************************************************************

    public void
    UndrawVertex
    (
        IVertex vertex,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(vertex != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        // Retrieve the VertexDrawingHistory object for the vertex, if one
        // exists.  (If the vertex was previously hidden, there won't be a
        // VertexDrawingHistory object for it.)

        VertexDrawingHistory oVertexDrawingHistory;

        if ( TryGetVertexDrawingHistory(vertex, out oVertexDrawingHistory) )
        {
            // Remove the VertexDrawingHistory object from the vertex.

            vertex.RemoveKey(ReservedMetadataKeys.VertexDrawingHistory);

            // Remove the vertex's DrawingVisual object, which will cause the
            // vertex to disappear.

            m_oAllVertexDrawingVisuals.Children.Remove(
                oVertexDrawingHistory.DrawingVisual);
        }
    }

    //*************************************************************************
    //  Method: UndrawEdge()
    //
    /// <summary>
    /// "Undraws" an edge that was drawn by <see cref="DrawGraph" />.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to remove from the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="graphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.  This
    /// must be the same object that was passed to <see cref="DrawGraph" /> the
    /// last time the entire graph was drawn.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to undraw an edge without incurring the overhead of
    /// redrawing the entire graph.
    /// </remarks>
    //*************************************************************************

    public void
    UndrawEdge
    (
        IEdge edge,
        GraphDrawingContext graphDrawingContext
    )
    {
        Debug.Assert(edge != null);
        Debug.Assert(graphDrawingContext != null);
        AssertValid();

        // Retrieve the EdgeDrawingHistory object for the edge, if one exists.
        // (If the edge was previously hidden, there won't be an
        // EdgeDrawingHistory object for it.)

        EdgeDrawingHistory oEdgeDrawingHistory;

        if ( TryGetEdgeDrawingHistory(edge, out oEdgeDrawingHistory) )
        {
            // Remove the EdgeDrawingHistory object from the edge.

            edge.RemoveKey(ReservedMetadataKeys.EdgeDrawingHistory);

            // Remove the edge's DrawingVisual object, which will cause the
            // edge to disappear.

            GetEdgeDrawingVisuals(oEdgeDrawingHistory).Children.Remove(
                oEdgeDrawingHistory.DrawingVisual);
        }
    }

    //*************************************************************************
    //  Method: AddVisualOnTopOfGraph()
    //
    /// <summary>
    /// Temporarily adds a caller-supplied Visual on top of the drawn graph.
    /// </summary>
    ///
    /// <param name="visual">
    /// The Visual to add on top of the graph.
    /// </param>
    ///
    /// <remarks>
    /// Call this method after calling <see cref="DrawGraph" /> to add a Visual
    /// on top of the graph.  This is useful for adding a temporary tooltip,
    /// dragging marquee, or other Visual object to the graph.
    ///
    /// <para>
    /// The added Visual gets removed when <see cref="DrawGraph" /> is called
    /// again.  You can also remove the Visual without redrawing the graph by
    /// calling <see cref="RemoveVisualFromTopOfGraph" />.
    /// </para>
    ///
    /// <para>
    /// An InvalidOperationException is thrown if the Visual has already been
    /// added.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    AddVisualOnTopOfGraph
    (
        Visual visual
    )
    {
        Debug.Assert(visual != null);
        AssertValid();

        const String MethodName = "AddVisualOnTopOfGraph";

        if ( m_oVisualCollection.Contains(visual) )
        {
            Debug.Assert(false);

            throw new InvalidOperationException( String.Format(

                "{0}.{1}: Visual has already been added."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        m_oVisualCollection.Add(visual);
    }

    //*************************************************************************
    //  Method: RemoveVisualFromTopOfGraph()
    //
    /// <summary>
    /// Removes the caller-supplied Visual from the top of the graph.
    /// </summary>
    ///
    /// <param name="visual">
    /// The Visual to remove from the top of the graph.
    /// </param>
    ///
    /// <remarks>
    /// Use this method to remove the Visual added to the top of the graph by
    /// <see cref="AddVisualOnTopOfGraph" /> without redrawing the graph.  The
    /// Visual also gets removed if <see cref="DrawGraph" /> is called again.
    ///
    /// <para>
    /// If the Visual doesn't exist in the VisualCollection, which will occur
    /// if <see cref="DrawGraph" /> was called after the Visual was added, this
    /// method does nothing.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    RemoveVisualFromTopOfGraph
    (
        Visual visual
    )
    {
        Debug.Assert(visual != null);
        AssertValid();

        // The documentation doesn't say what Remove() does if the Visual isn't
        // contained in the collection, so check first.

        if ( m_oVisualCollection.Contains(visual) )
        {
            m_oVisualCollection.Remove(visual);
        }
    }

    //*************************************************************************
    //  Method: DrawBackground()
    //
    /// <summary>
    /// Draws the graph's background.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph being drawn.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    //*************************************************************************

    protected void
    DrawBackground
    (
        IGraph oGraph,
        GraphDrawingContext oGraphDrawingContext
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        // Draw the background color, followed by the background image if one
        // was specified.

        DrawingVisual oBackgroundDrawingVisual = new DrawingVisual();

        using ( DrawingContext oDrawingContext =
            oBackgroundDrawingVisual.RenderOpen() )
        {
            oDrawingContext.DrawRectangle(
                CreateFrozenSolidColorBrush(m_oBackColor), null,
                oGraphDrawingContext.GraphRectangle);

            if (m_oBackgroundImage != null)
            {
                oDrawingContext.DrawImage( m_oBackgroundImage,
                    new Rect( new Size(m_oBackgroundImage.Width,
                        m_oBackgroundImage.Height) ) );
            }
        }

        m_oVisualCollection.Add(oBackgroundDrawingVisual);
    }

    //*************************************************************************
    //  Method: GetVerticesToDraw()
    //
    /// <summary>
    /// Gets a collection of vertices to draw, sorted if necessary.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph being drawn.
    /// </param>
    ///
    /// <returns>
    /// A collection of vertices to draw, sorted if necessary.
    /// </returns>
    ///
    /// <remarks>
    /// This method checks whether a sort order has been specified for the
    /// graph's vertices.  If so, the vertices are sorted and returned in a new
    /// collection.  Otherwise, the graph's unsorted vertex collection is
    /// returned.
    /// </remarks>
    //*************************************************************************

    protected ICollection<IVertex>
    GetVerticesToDraw
    (
        IGraph oGraph
    )
    {
        Debug.Assert(oGraph != null);

        // Note that the ReservedMetadataKeys.SortableLayoutAndZOrderSet key is
        // used for two purposes:
        //
        // 1. It tells SortableLayoutBase to lay out the graph's vertices in a
        //    specified order.
        //
        // 2. It tells this GraphDrawer to draw the graph's vertices in the
        //    same order.

        if ( oGraph.ContainsKey(
            ReservedMetadataKeys.SortableLayoutAndZOrderSet) )
        {
            return ( ( new ByMetadataVertexSorter<Single>(
                ReservedMetadataKeys.SortableLayoutAndZOrder) ).Sort(
                    oGraph.Vertices) );
        }

        return (oGraph.Vertices);
    }

    //*************************************************************************
    //  Method: DrawVertex()
    //
    /// <summary>
    /// Draws a vertex onto the contained collection of Visual objects.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to draw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a DrawingVisual for the vertex to
    /// m_oAllVertexDrawingVisuals and adds a VertexDrawingHistory to the
    /// vertex's metadata.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawVertex
    (
        IVertex oVertex,
        GraphDrawingContext oGraphDrawingContext
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        VertexDrawingHistory oVertexDrawingHistory;

        if ( m_oVertexDrawer.TryDrawVertex(oVertex, oGraphDrawingContext,
            out oVertexDrawingHistory) )
        {
            Debug.Assert(oVertexDrawingHistory != null);

            DrawingVisual oVertexChildDrawingVisual =
                oVertexDrawingHistory.DrawingVisual;

            m_oAllVertexDrawingVisuals.Children.Add(oVertexChildDrawingVisual);

            oVertex.SetValue(ReservedMetadataKeys.VertexDrawingHistory,
                oVertexDrawingHistory);

            // Save the vertex on the DrawingVisual for later retrieval.

            SaveVertexOnDrawingVisual(oVertex, oVertexChildDrawingVisual);
        }
    }

    //*************************************************************************
    //  Method: DrawEdge()
    //
    /// <summary>
    /// Draws an edge onto the contained collection of Visual objects.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to draw onto the contained collection of Visual objects.
    /// </param>
    ///
    /// <param name="oGraphDrawingContext">
    /// Provides access to objects needed for graph-drawing operations.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a DrawingVisual for the edge to either
    /// m_oUnselectedEdgeDrawingVisuals or m_oSelectedEdgeDrawingVisuals, and
    /// adds an EdgeDrawingHistory to the edge's metadata.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawEdge
    (
        IEdge oEdge,
        GraphDrawingContext oGraphDrawingContext
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(oGraphDrawingContext != null);
        AssertValid();

        EdgeDrawingHistory oEdgeDrawingHistory;

        if ( m_oEdgeDrawer.TryDrawEdge(oEdge, oGraphDrawingContext,
            out oEdgeDrawingHistory) )
        {
            Debug.Assert(oEdgeDrawingHistory != null);

            GetEdgeDrawingVisuals(oEdgeDrawingHistory).Children.Add(
                oEdgeDrawingHistory.DrawingVisual);

            // Save the EdgeDrawingHistory object.

            oEdge.SetValue(ReservedMetadataKeys.EdgeDrawingHistory,
                oEdgeDrawingHistory);
        }
    }

    //*************************************************************************
    //  Method: GetEdgeDrawingVisuals()
    //
    /// <summary>
    /// Gets a DrawingVisual that contains all the visuals for either 
    /// unselected edges or selected edges.
    /// </summary>
    ///
    /// <param name="oEdgeDrawingHistory">
    /// Determines which DrawingVisual gets returned.
    /// </param>
    ///
    /// <returns>
    /// If <paramref name="oEdgeDrawingHistory" /> indicates that the edge was
    /// drawn as unselected, then the DrawingVisual that contains all the
    /// visuals for unselected edges is returned.  Otherwise, the DrawingVisual
    /// that contains all the visuals for selected edges is returned.
    /// </returns>
    //*************************************************************************

    protected DrawingVisual
    GetEdgeDrawingVisuals
    (
        EdgeDrawingHistory oEdgeDrawingHistory
    )
    {
        Debug.Assert(oEdgeDrawingHistory != null);
        AssertValid();

        return (oEdgeDrawingHistory.DrawnAsSelected ?
            m_oSelectedEdgeDrawingVisuals : m_oUnselectedEdgeDrawingVisuals);
    }

    //*************************************************************************
    //  Method: SaveVertexOnDrawingVisual()
    //
    /// <summary>
    /// Saves a vertex on the DrawingVisual with which the vertex was drawn.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex that was drawn.
    /// </param>
    ///
    /// <param name="oDrawingVisual">
    /// The DrawingVisual with which <paramref name="oVertex" /> was drawn.
    /// </param>
    ///
    /// <remarks>
    /// The vertex can be retrieved from the DrawingVisual with <see
    /// cref="RetrieveVertexFromDrawingVisual" />.
    /// </remarks>
    //*************************************************************************

    protected void
    SaveVertexOnDrawingVisual
    (
        IVertex oVertex,
        DrawingVisual oDrawingVisual
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oDrawingVisual != null);
        AssertValid();

        // DrawingVisual has no Tag property, so use FrameworkElement's Tag
        // property as an attached property.

        oDrawingVisual.SetValue(FrameworkElement.TagProperty, oVertex);
    }

    //*************************************************************************
    //  Method: RetrieveVertexFromDrawingVisual()
    //
    /// <summary>
    /// Retrieves a vertex from the DrawingVisual with which the vertex was
    /// drawn.
    /// </summary>
    ///
    /// <param name="oDrawingVisual">
    /// The DrawingVisual to retrieve a vertex from.
    /// </param>
    ///
    /// <returns>
    /// The vertex that was drawn with <paramref name="oDrawingVisual" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method retrieves the vertex that was saved on a DrawingVisual by
    /// <see cref="SaveVertexOnDrawingVisual" />.
    /// </remarks>
    //*************************************************************************

    protected IVertex
    RetrieveVertexFromDrawingVisual
    (
        DrawingVisual oDrawingVisual
    )
    {
        Debug.Assert(oDrawingVisual != null);
        AssertValid();

        Object oVertexAsObject =
            oDrawingVisual.GetValue(FrameworkElement.TagProperty);

        Debug.Assert(oVertexAsObject is IVertex);

        return ( (IVertex)oVertexAsObject );
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

        Debug.Assert(m_oVisualCollection != null);
        // m_oAllVertexDrawingVisuals
        // m_oUnselectedEdgeDrawingVisuals
        // m_oSelectedEdgeDrawingVisuals
        Debug.Assert(m_oVertexDrawer != null);
        Debug.Assert(m_oEdgeDrawer != null);
        Debug.Assert(m_oGroupDrawer != null);
        // m_oBackColor
        // m_oBackgroundImage
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value of the <see cref="GraphScale" /> property.
    /// </summary>

    public const Double MinimumGraphScale = 0.01;

    /// <summary>
    /// Maximum value of the <see cref="GraphScale" /> property.
    /// </summary>

    public const Double MaximumGraphScale = 10.0;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Collection of Visual objects that represent the graph's vertices and
    /// edges.

    protected VisualCollection m_oVisualCollection;

    /// Visual that contains all the vertex visuals, or null if the graph
    /// hasn't been drawn yet.

    protected DrawingVisual m_oAllVertexDrawingVisuals;

    /// Visual that contains all the visuals for unselected edges, or null if
    /// the graph hasn't been drawn yet.

    protected DrawingVisual m_oUnselectedEdgeDrawingVisuals;

    /// Visual that contains all the visuals for selected edges, or null if the
    /// graph hasn't been drawn yet.

    protected DrawingVisual m_oSelectedEdgeDrawingVisuals;

    /// Draws the graph's vertices.

    protected VertexDrawer m_oVertexDrawer;

    /// Draws the graph's edges.

    protected EdgeDrawer m_oEdgeDrawer;

    /// Draws the graph's groups.

    protected GroupDrawer m_oGroupDrawer;

    /// Background color.

    protected Color m_oBackColor;

    /// Background image, or null for no image.

    protected ImageSource m_oBackgroundImage;
}

}
