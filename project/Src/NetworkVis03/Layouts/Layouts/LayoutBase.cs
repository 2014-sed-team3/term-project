
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;
using Smrf.GraphicsLib;
using MIConvexHull;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Class: LayoutBase
//
/// <summary>
/// Base class for layouts.
/// </summary>
///
/// <remarks>
/// This abstract class can serve as a base class for <see
/// cref="ILayout" /> implementations.  Its implementations of the <see
/// cref="ILayout" /> public methods provide error checking but defer the
/// actual work to protected abstract methods.
/// </remarks>
//*****************************************************************************

public abstract class LayoutBase : LayoutsBase, ILayout
{
    //*************************************************************************
    //  Constructor: LayoutBase()
    //
    /// <summary>
    /// Initializes a new instance of the LayoutBase class.
    /// </summary>
    //*************************************************************************

    public LayoutBase()
    {
        m_iMargin = 6;
        m_eLayoutStyle = LayoutStyle.Normal;
        m_dGroupRectanglePenWidth = 1.0;
        m_eIntergroupEdgeStyle = IntergroupEdgeStyle.Show;
        m_bImproveLayoutOfGroups = false;
        m_iMaximumVerticesPerBin = 3;
        m_iBinLength = 16;

        // Create the BackgroundWorker used by LayOutGraphAsync() and handle
        // its DoWork event.

        m_oBackgroundWorker = new BackgroundWorker();

        m_oBackgroundWorker.WorkerSupportsCancellation  = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            this.BackgroundWorker_DoWork);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                this.BackgroundWorker_RunWorkerCompleted);

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
    /// Gets or sets the margin to subtract from each edge of the graph
    /// rectangle before laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The margin to subtract from each edge.  Must be greater than or equal
    /// to zero.  The default value is 6.
    /// </value>
    ///
    /// <remarks>
    /// If the graph rectangle passed to <see cref="LayOutGraph" /> is {L=0,
    /// T=0, R=50, B=30} and the <see cref="Margin" /> is 5, for example, then
    /// the graph is laid out within the rectangle {L=5, T=5, R=45, B=25}.
    /// </remarks>
    //*************************************************************************

    public Int32
    Margin
    {
        get
        {
            AssertValid();

            return (m_iMargin);
        }

        set
        {
            const String PropertyName = "Margin";

            this.ArgumentChecker.CheckPropertyNotNegative(PropertyName, value);

            if (value == m_iMargin)
            {
                return;
            }

            m_iMargin = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LayoutStyle
    //
    /// <summary>
    /// Gets or sets the style to use when laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The style to use when laying out the graph.  The default value is
    /// <see cref="Smrf.NodeXL.Layouts.LayoutStyle.Normal" />.
    /// </value>
    ///
    /// <remarks>
    /// If you set this property to <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseGroups" />, you must add
    /// group information to the graph to tell the layout class which vertices
    /// are in which groups.
    ///
    /// <example>
    /// Here is sample C# code that puts each of the graph's vertices in a
    /// group and tells the layout class to lay out each group in its own box.
    /// It assumes that you are using a NodeXLControl.
    ///
    /// <code>
    /// // For demonstration purposes, put each vertex in a random group.
    /// 
    /// const Int32 Groups = 5;
    /// 
    /// GroupInfo [] groupInfo = new GroupInfo[Groups];
    /// 
    /// for (Int32 i = 0; i &lt; Groups; i++)
    /// {
    ///     groupInfo[i] = new GroupInfo();
    /// }
    /// 
    /// Random random = new Random();
    /// 
    /// foreach (IVertex vertex in nodeXLControl.Graph.Vertices)
    /// {
    ///     groupInfo[ random.Next(0, Groups - 1) ].Vertices.AddLast(vertex);
    /// }
    /// 
    /// // Store the group information as metadata on the graph.
    /// 
    /// nodeXLControl.Graph.SetValue(ReservedMetadataKeys.GroupInfo, groupInfo);
    /// 
    /// // Tell the layout class to use the group information.
    /// 
    /// nodeXLControl.Layout.LayoutStyle = LayoutStyle.UseGroups;
    /// </code>
    /// </example>
    /// </remarks>
    //*************************************************************************

    public LayoutStyle
    LayoutStyle
    {
        get
        {
            AssertValid();

            return (m_eLayoutStyle);
        }

        set
        {
            if (value == m_eLayoutStyle)
            {
                return;
            }

            m_eLayoutStyle = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BoxLayoutAlgorithm
    //
    /// <summary>
    /// Gets or sets the box layout algorithm to use when laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The box layout algorithm to use when laying out the graph.  The default value is
    /// <see cref="Smrf.NodeXL.Layouts.BoxLayoutAlgorithm.Treemap" />.
    /// </value>
    ///
    //*************************************************************************

    public BoxLayoutAlgorithm
    BoxLayoutAlgorithm
    {
        get
        {
            AssertValid();

            return (m_eBoxLayoutAlgorithm);
        }

        set
        {
            if (value == m_eBoxLayoutAlgorithm)
            {
                return;
            }

            m_eBoxLayoutAlgorithm = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SupportsOutOfBoundsVertices
    //
    /// <summary>
    /// Gets a flag indicating whether vertices laid out by the class can fall
    /// outside the graph bounds.
    /// </summary>
    ///
    /// <value>
    /// true if the vertices can fall outside the graph bounds.
    /// </value>
    ///
    /// <remarks>
    /// If true, the <see cref="IVertex.Location" /> of the laid-out vertices
    /// may be within the graph rectangle's margin or outside the graph
    /// rectangle.  If false, the vertex locations are always within the
    /// margin.
    /// </remarks>
    //*************************************************************************

    public virtual Boolean
    SupportsOutOfBoundsVertices
    {
        get
        {
            return (false);
        }
    }

    //*************************************************************************
    //  Property: GroupRectanglePenWidth
    //
    /// <summary>
    /// Gets or sets the width of the pen used to draw group rectangles.
    /// </summary>
    ///
    /// <value>
    /// The width of the pen used to draw group rectangles.  Must be greater
    /// than or equal to 0.  If 0, group rectangles aren't drawn.  The default
    /// value is 1.0.
    /// </value>
    ///
    /// <remarks>
    /// This property is ignored if <see cref="LayoutStyle" /> is not <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseGroups" />.
    /// </remarks>
    //*************************************************************************

    public Double
    GroupRectanglePenWidth
    {
        get
        {
            AssertValid();

            return (m_dGroupRectanglePenWidth);
        }

        set
        {
            m_dGroupRectanglePenWidth = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: IntergroupEdgeStyle
    //
    /// <summary>
    /// Gets or sets a value that specifies how the edges that connect vertices
    /// in different groups should be shown.
    /// </summary>
    ///
    /// <value>
    /// An <see cref="Smrf.NodeXL.Layouts.IntergroupEdgeStyle" /> value.
    /// </value>
    ///
    /// <remarks>
    /// This property is ignored if <see cref="LayoutStyle" /> is not <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseGroups" />.
    /// </remarks>
    //*************************************************************************

    public IntergroupEdgeStyle
    IntergroupEdgeStyle
    {
        get
        {
            AssertValid();

            return (m_eIntergroupEdgeStyle);
        }

        set
        {
            m_eIntergroupEdgeStyle = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ImproveLayoutOfGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the layout should attempt to
    /// improve the appearance of groups.
    /// </summary>
    ///
    /// <value>
    /// true to attempt to improve the appearance of groups.  The default value
    /// is false.
    /// </value>
    ///
    /// <remarks>
    /// This property is ignored if <see cref="LayoutStyle" /> is not <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseGroups" />.
    ///
    /// <para>
    /// If true, groups that don't have many vertices are laid out using a Grid
    /// layout.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    ImproveLayoutOfGroups
    {
        get
        {
            AssertValid();

            return (m_bImproveLayoutOfGroups);
        }

        set
        {
            m_bImproveLayoutOfGroups = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SupportsBinning
    //
    /// <summary>
    /// Gets a flag indicating whether binning can be used when the entire
    /// graph is laid out.
    /// </summary>
    ///
    /// <value>
    /// true if binning can be used.
    /// </value>
    //*************************************************************************

    public virtual Boolean
    SupportsBinning
    {
        get
        {
            AssertValid();

            return (true);
        }
    }

    //*************************************************************************
    //  Property: MaximumVerticesPerBin
    //
    /// <summary>
    /// Gets or sets the maximum number of vertices a binned component can
    /// have.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of vertices a binned component can have.  The
    /// default value is 3.
    /// </value>
    ///
    /// <remarks>
    /// If <see cref="LayoutStyle" /> is <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseBinning" /> and a
    /// strongly connected component of the graph has <see
    /// cref="MaximumVerticesPerBin" /> vertices or fewer, the component is
    /// placed in a bin.
    /// </remarks>
    //*************************************************************************

    public Int32
    MaximumVerticesPerBin
    {
        get
        {
            AssertValid();

            return (m_iMaximumVerticesPerBin);
        }

        set
        {
            m_iMaximumVerticesPerBin = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BinLength
    //
    /// <summary>
    /// Gets or sets the height and width of each bin, in graph rectangle
    /// units.
    /// </summary>
    ///
    /// <value>
    /// The height and width of each bin, in graph rectangle units.  The
    /// default value is 16.
    /// </value>
    ///
    /// <remarks>
    /// This property is ignored if <see cref="LayoutStyle" /> is not <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseBinning" />.
    /// </remarks>
    //*************************************************************************

    public Int32
    BinLength
    {
        get
        {
            AssertValid();

            return (m_iBinLength);
        }

        set
        {
            m_iBinLength = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a value indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            AssertValid();

            return (m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: LayOutGraph()
    //
    /// <summary>
    /// Lays out a graph synchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <remarks>
    /// This method lays out the graph <paramref name="graph" /> by setting the
    /// <see cref="IVertex.Location" /> property on all of the graph's
    /// vertices, and optionally adding geometry metadata to the graph,
    /// vertices, or edges.
    /// </remarks>
    //*************************************************************************

    public void
    LayOutGraph
    (
        IGraph graph,
        LayoutContext layoutContext
    )
    {
        AssertValid();

        const String MethodName = "LayOutGraph";

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "graph", graph);

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "layoutContext",
            layoutContext);

        LayOutGraphInternal(graph, layoutContext, null, null);
    }

    //*************************************************************************
    //  Method: LayOutGraphAsync()
    //
    /// <summary>
    /// Lays out a graph asynchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <remarks>
    /// This method asynchronously lays out the graph <paramref name="graph" />
    /// by setting the <see cref="IVertex.Location" /> property on all of the
    /// graph's vertices, and optionally adding geometry metadata to the graph,
    /// vertices, or edges.  It starts a worker thread and then returns
    /// immediately.
    ///
    /// <para>
    /// The <see cref="LayOutGraphCompleted" /> event fires when the layout is
    /// complete, an error occurs, or the layout is cancelled.  <see
    /// cref="LayOutGraphAsyncCancel" /> cancels the layout.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    LayOutGraphAsync
    (
        IGraph graph,
        LayoutContext layoutContext
    )
    {
        AssertValid();

        const String MethodName = "LayOutGraphAsync";

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "graph", graph);

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "layoutContext", layoutContext);

        if (m_oBackgroundWorker.IsBusy)
        {
            throw new InvalidOperationException(String.Format(

                "{0}.{1}: A layout is already in progress."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        // Start a worker thread, then return immediately.

        m_oBackgroundWorker.RunWorkerAsync(
            new LayOutGraphAsyncArguments(graph, layoutContext) );
    }

    //*************************************************************************
    //  Method: LayOutGraphAsyncCancel()
    //
    /// <summary>
    /// Cancels the layout started by <see cref="LayOutGraphAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// The layout may or may not cancel, but the <see
    /// cref="LayOutGraphCompleted" /> event is guaranteed to fire.  The <see
    /// cref="AsyncCompletedEventArgs" /> object passed to the event handler
    /// contains a <see cref="AsyncCompletedEventArgs.Cancelled" /> property
    /// that indicates whether the cancellation occurred.
    ///
    /// <para>
    /// If a layout is not in progress, this method does nothing.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    LayOutGraphAsyncCancel()
    {
        AssertValid();

        if (!m_oBackgroundWorker.IsBusy)
        {
            return;
        }

        m_oBackgroundWorker.CancelAsync();
    }

    //*************************************************************************
    //  Method: TransformLayout()
    //
    /// <summary>
    /// Transforms a graph's current layout.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose layout needs to be transformed.
    /// </param>
    ///
    /// <param name="originalLayoutContext">
    /// <see cref="LayoutContext" /> object that was passed to the most recent
    /// call to <see cref="LayOutGraph" />.
    /// </param>
    ///
    /// <param name="newLayoutContext">
    /// Provides access to the new graph rectangle.
    /// </param>
    ///
    /// <remarks>
    /// After a graph has been laid out by <see cref="LayOutGraph" />, this
    /// method can be used to transform the graph's layout from the original
    /// graph rectangle to another.  <paramref name="originalLayoutContext" />
    /// contains the original graph rectangle, and <paramref
    /// name="newLayoutContext" /> contains the new graph rectangle.  This
    /// method transforms all the graph's vertex locations from the original
    /// rectangle to the new one.  If <see cref="LayOutGraph" /> added geometry
    /// metadata to the graph, this method also transforms that metadata.
    /// </remarks>
    //*************************************************************************

    public void
    TransformLayout
    (
        IGraph graph,
        LayoutContext originalLayoutContext,
        LayoutContext newLayoutContext
    )
    {
        AssertValid();

        const String MethodName = "TransformLayout";

        this.ArgumentChecker.CheckArgumentNotNull(MethodName, "graph", graph);

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "originalLayoutContext", originalLayoutContext);

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "newLayoutContext", newLayoutContext);

        if (graph.Vertices.Count == 0)
        {
            return;
        }

        TransformLayoutCore(graph, originalLayoutContext, newLayoutContext);
    }

    //*************************************************************************
    //  Method: OnVertexMove()
    //
    /// <summary>
    /// Processes a vertex that was moved after the graph was laid out.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was moved.
    /// </param>
    ///
    /// <remarks>
    /// An application may allow the user to move a vertex after the graph has
    /// been laid out by <see cref="LayOutGraph" />.  This method is called
    /// after the application has changed the <see cref="IVertex.Location" />
    /// property on <paramref name="vertex" />.  If <see cref="LayOutGraph" />
    /// added geometry metadata to the graph, vertices, or edges, <see
    /// cref="OnVertexMove" /> should modify the metadata if necessary.
    /// </remarks>
    //*************************************************************************

    public void
    OnVertexMove
    (
        IVertex vertex
    )
    {
        AssertValid();

        const String MethodName = "OnVertexMove";

        this.ArgumentChecker.CheckArgumentNotNull(
            MethodName, "vertex", vertex);

        OnVertexMoveCore(vertex);
    }

    //*************************************************************************
    //  Event: LayOutGraphCompleted
    //
    /// <summary>
    /// Occurs when a layout started by <see cref="LayOutGraphAsync" />
    /// completes, is cancelled, or ends with an error.
    /// </summary>
    ///
    /// <remarks>
    /// The event fires on the thread on which <see cref="LayOutGraphAsync" />
    /// was called.
    /// </remarks>
    //*************************************************************************

    public event AsyncCompletedEventHandler LayOutGraphCompleted;


    //*************************************************************************
    //  Method: LayOutGraphCore()
    //
    /// <summary>
    /// Lays out a graph synchronously or asynchronously.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.  The graph is guaranteed to have at least one vertex.
    /// </param>
    ///
    /// <param name="verticesToLayOut">
    /// Vertices to lay out.  The collection is guaranteed to have at least one
    /// vertex.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> is guaranteed to have non-zero
    /// width and height.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method
    /// if the graph is being laid out asynchronously, or null if the graph is
    /// being laid out synchronously.
    /// </param>
    ///
    /// <returns>
    /// true if the layout was successfully completed, false if the layout was
    /// cancelled.  The layout can be cancelled only if the graph is being laid
    /// out asynchronously.
    /// </returns>
    ///
    /// <remarks>
    /// This method lays out the graph <paramref name="graph" /> either
    /// synchronously (if <paramref name="backgroundWorker" /> is null) or
    /// asynchronously (if (<paramref name="backgroundWorker" /> is not null)
    /// by setting the the <see cref="IVertex.Location" /> property on the
    /// vertices in <paramref name="verticesToLayOut" /> and optionally adding
    /// geometry metadata to the graph, vertices, or edges.
    ///
    /// <para>
    /// In the asynchronous case, the <see
    /// cref="BackgroundWorker.CancellationPending" /> property on the
    /// <paramref name="backgroundWorker" /> object should be checked before
    /// each layout iteration.  If it's true, the method should immediately
    /// return false.
    /// </para>
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected abstract Boolean
    LayOutGraphCore
    (
        IGraph graph,
        ICollection<IVertex> verticesToLayOut,
        LayoutContext layoutContext,
        BackgroundWorker backgroundWorker
    );

    //*************************************************************************
    //  Method: GetAdjustedLayoutContext()
    //
    /// <summary>
    /// Gets an adjusted layout context object to use when laying out the
    /// graph.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph being laid out.
    /// </param>
    ///
    /// <param name="oOriginalLayoutContext">
    /// The original layout context passed to the layout method.
    /// </param>
    ///
    /// <param name="oAdjustedLayoutContext">
    /// If true is returned, this gets set to a copy of <paramref
    /// name="oOriginalLayoutContext" /> that has been adjusted.
    /// </param>
    ///
    /// <returns>
    /// true if the graph can be laid out, false if it can't be.
    /// </returns>
    ///
    /// <remarks>
    /// This method adjusts the graph rectangle stored in <paramref
    /// name="oOriginalLayoutContext" /> according to the <see
    /// cref="LayoutBase.Margin" /> setting and the presence of a <see
    /// cref="ReservedMetadataKeys.LayOutTheseVerticesWithinBounds" /> key on
    /// the graph.  If subtracting the margin results in a non-positive width
    /// or height, false is returned.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    GetAdjustedLayoutContext
    (
        IGraph oGraph,
        LayoutContext oOriginalLayoutContext,
        out LayoutContext oAdjustedLayoutContext
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oOriginalLayoutContext != null);
        AssertValid();

        oAdjustedLayoutContext = null;
        Rectangle oAdjustedRectangle = oOriginalLayoutContext.GraphRectangle;

        if (
            oGraph.ContainsKey(
                ReservedMetadataKeys.LayOutTheseVerticesWithinBounds)
            &&
            oGraph.ContainsKey(
                ReservedMetadataKeys.LayOutTheseVerticesOnly)
            )
        {
            // Get the bounding rectangle of the specified vertices.

            Single fMinimumX = Single.MaxValue;
            Single fMaximumX = Single.MinValue;
            Single fMinimumY = Single.MaxValue;
            Single fMaximumY = Single.MinValue;
            
            foreach ( IVertex oVertex in GetVerticesToLayOut(oGraph) )
            {
                PointF oLocation = oVertex.Location;
                Single fX = oLocation.X;
                Single fY = oLocation.Y;

                fMinimumX = Math.Min(fX, fMinimumX);
                fMaximumX = Math.Max(fX, fMaximumX);
                fMinimumY = Math.Min(fY, fMinimumY);
                fMaximumY = Math.Max(fY, fMaximumY);
            }

            if (fMinimumX != Single.MaxValue)
            {
                oAdjustedRectangle = Rectangle.FromLTRB(
                    (Int32)Math.Ceiling(fMinimumX),
                    (Int32)Math.Ceiling(fMinimumY),
                    (Int32)Math.Floor(fMaximumX),
                    (Int32)Math.Floor(fMaximumY) );
            }
        }
        else
        {
            oAdjustedRectangle.Inflate(-m_iMargin, -m_iMargin);
        }

        if (oAdjustedRectangle.Width > 0 && oAdjustedRectangle.Height > 0)
        {
            oAdjustedLayoutContext = new LayoutContext(oAdjustedRectangle);
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetRectangleCenterAndHalfSize()
    //
    /// <summary>
    /// Gets the center of a rectangle and the minimum of half its width and
    /// half its height.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// The rectangle to use.
    /// </param>
    ///
    /// <param name="centerX">
    /// The x-coordinate of the center of <paramref name="rectangle" />.
    /// </param>
    ///
    /// <param name="centerY">
    /// The y-coordinate of the center of <paramref name="rectangle" />.
    /// </param>
    ///
    /// <param name="halfSize">
    /// If the width of <paramref name="rectangle" /> is less than its height,
    /// half the width gets stored here.  Otherwise, half the height gets
    /// stored here.
    /// </param>
    ///
    /// <remarks>
    /// This method can be used by layouts that are centered and symetrical.
    /// </remarks>
    //*************************************************************************

    protected void
    GetRectangleCenterAndHalfSize
    (
        Rectangle rectangle,
        out Double centerX,
        out Double centerY,
        out Double halfSize
    )
    {
        AssertValid();

        Double fHalfWidth = (Double)rectangle.Width / 2.0;
        Double fHalfHeight = (Double)rectangle.Height / 2.0;

        centerX = rectangle.Left + fHalfWidth;
        centerY = rectangle.Top + fHalfHeight;

        halfSize = Math.Min(fHalfWidth, fHalfHeight);
    }

    //*************************************************************************
    //  Method: GetVerticesToLayOut()
    //
    /// <summary>
    /// Gets the vertices to lay out.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph that is being laid out.
    /// </param>
    ///
    /// <returns>
    /// The vertices to lay out.
    /// </returns>
    //*************************************************************************

    protected ICollection<IVertex>
    GetVerticesToLayOut
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);
        AssertValid();

        // If the LayOutTheseVerticesOnly key is present on the graph, its
        // value is an ICollection<IVertex> of vertices to lay out.

        Object oVerticesToLayOut;

        if ( graph.TryGetValue(ReservedMetadataKeys.LayOutTheseVerticesOnly,
            typeof( ICollection<IVertex> ), out oVerticesToLayOut) )
        {
            return ( ( ICollection<IVertex> )oVerticesToLayOut );
        }

        // The key isn't present.  Use the graph's entire vertex collection.

        return (graph.Vertices);
    }

    //*************************************************************************
    //  Method: GetEdgesToLayOut()
    //
    /// <summary>
    /// Gets the edges to lay out.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph that is being laid out.
    /// </param>
    ///
    /// <param name="verticesToLayOut">
    /// The vertices being laid out.
    /// </param>
    ///
    /// <returns>
    /// The edges to lay out.
    /// </returns>
    ///
    /// <remarks>
    /// If the derived class needs a list of the edges that connect only those
    /// vertices being laid out, it should use this method to get the list.
    /// </remarks>
    //*************************************************************************

    protected ICollection<IEdge>
    GetEdgesToLayOut
    (
        IGraph graph,
        ICollection<IVertex> verticesToLayOut
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(verticesToLayOut != null);
        AssertValid();

        if (verticesToLayOut.Count == graph.Vertices.Count)
        {
            // The entire graph is being laid out.

            return (graph.Edges);
        }

        // Create a HashSet of the vertices to lay out.  The key is the vertex
        // ID.

        HashSet<Int32> oVertexIDHashSet = new HashSet<Int32>();

        foreach (IVertex oVertex in verticesToLayOut)
        {
            oVertexIDHashSet.Add(oVertex.ID);
        }

        // Create a dictionary of the edges to lay out.  The key is the edge ID
        // and the value is the edge.  Only those edges that connect two
        // vertices being laid out should be included.  A dictionary is used to
        // prevent the same edge from being included twice.

        Dictionary<Int32, IEdge> oEdgeIDDictionary =
            new Dictionary<Int32, IEdge>();

        foreach (IVertex oVertex in verticesToLayOut)
        {
            foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
            {
                IVertex oAdjacentVertex =
                    oIncidentEdge.GetAdjacentVertex(oVertex);

                if ( oVertexIDHashSet.Contains(oAdjacentVertex.ID) )
                {
                    oEdgeIDDictionary[oIncidentEdge.ID] = oIncidentEdge;
                }
            }
        }

        return (oEdgeIDDictionary.Values);
    }

    //*************************************************************************
    //  Method: RandomizeVertexLocations()
    //
    /// <overloads>
    /// Randomly distributes the vertex locations in a graph.
    /// </overloads>
    ///
    /// <summary>
    /// Randomly distributes the vertex locations in a graph using a
    /// time-dependent default seed value.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose vertices need to be randomized.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> must have non-zero width and
    /// height.
    /// </param>
    ///
    /// <remarks>
    /// If a vertex has a metadata key of <see
    /// cref="ReservedMetadataKeys.LockVertexLocation" /> with the value true,
    /// its location is left unmodified.
    /// </remarks>
    //*************************************************************************

    protected void
    RandomizeVertexLocations
    (
        IGraph graph,
        LayoutContext layoutContext
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(layoutContext != null);
        AssertValid();

        RandomizeVertexLocations(graph.Vertices, layoutContext, new Random() );
    }

    //*************************************************************************
    //  Method: RandomizeVertexLocations()
    //
    /// <summary>
    /// Randomly distributes the vertex locations in a graph using a specified
    /// seed value.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose vertices need to be randomized.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> must have non-zero width and
    /// height.
    /// </param>
    ///
    /// <param name="seed">
    /// A number used to calculate a starting value for the pseudo-random
    /// number sequence. If a negative number is specified, the absolute value
    /// of the number is used. 
    /// </param>
    ///
    /// <remarks>
    /// If a vertex has a metadata key of <see
    /// cref="ReservedMetadataKeys.LockVertexLocation" /> with the value true,
    /// its location is left unmodified.
    /// </remarks>
    //*************************************************************************

    protected void
    RandomizeVertexLocations
    (
        IGraph graph,
        LayoutContext layoutContext,
        Int32 seed
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(layoutContext != null);
        AssertValid();

        RandomizeVertexLocations( graph.Vertices, layoutContext,
            new Random(seed) );
    }

    //*************************************************************************
    //  Method: RandomizeVertexLocations()
    //
    /// <summary>
    /// Randomly distributes the vertex locations in a graph using a specified
    /// random number generator.
    /// </summary>
    ///
    /// <param name="vertices">
    /// Vertices that need to be randomized.
    /// </param>
    ///
    /// <param name="layoutContext">
    /// Provides access to objects needed to lay out the graph.  The <see
    /// cref="LayoutContext.GraphRectangle" /> must have non-zero width and
    /// height.
    /// </param>
    ///
    /// <param name="random">
    /// Random number generator.
    /// </param>
    ///
    /// <remarks>
    /// If a vertex has a metadata key of <see
    /// cref="ReservedMetadataKeys.LockVertexLocation" /> with the value true,
    /// its location is left unmodified.
    /// </remarks>
    //*************************************************************************

    protected void
    RandomizeVertexLocations
    (
        ICollection<IVertex> vertices,
        LayoutContext layoutContext,
        Random random
    )
    {
        Debug.Assert(vertices != null);
        Debug.Assert(layoutContext != null);
        Debug.Assert(random != null);
        AssertValid();

        Rectangle oRectangle = layoutContext.GraphRectangle;

        Debug.Assert(oRectangle.Width > 0);
        Debug.Assert(oRectangle.Height > 0);

        Int32 iLeft = oRectangle.Left;
        Int32 iRight = oRectangle.Right;
        Int32 iTop = oRectangle.Top;
        Int32 iBottom = oRectangle.Bottom;

        foreach (IVertex oVertex in vertices)
        {
            if ( VertexIsLocked(oVertex) )
            {
                continue;
            }

            Int32 iX = random.Next(iLeft, iRight + 1);
            Int32 iY = random.Next(iTop, iBottom + 1);

            oVertex.Location = new PointF(iX, iY);
        }
    }

    //*************************************************************************
    //  Method: VertexIsLocked()
    //
    /// <summary>
    /// Returns a flag indicating whether the vertex is locked.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to check.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex is locked.
    /// </returns>
    ///
    /// <remarks>
    /// A locked vertex's location should not be modified by the layout,
    /// although the vertex may be included in layout calculations.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    VertexIsLocked
    (
        IVertex oVertex
    )
    {
        Debug.Assert(oVertex != null);
        AssertValid();

        Object oLockVertexLocation;

        return (oVertex.TryGetValue(ReservedMetadataKeys.LockVertexLocation,
            typeof(Boolean), out oLockVertexLocation) &&
            (Boolean)oLockVertexLocation);
    }

    //*************************************************************************
    //  Method: TransformLayoutCore()
    //
    /// <summary>
    /// Transforms a graph's current layout.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph whose layout needs to be transformed.
    /// </param>
    ///
    /// <param name="originalLayoutContext">
    /// <see cref="LayoutContext" /> object that was passed to the most recent
    /// call to <see cref="LayOutGraph" />.
    /// </param>
    ///
    /// <param name="newLayoutContext">
    /// Provides access to objects needed to transform the graph's layout.
    /// </param>
    ///
    /// <remarks>
    /// After a graph has been laid out by <see cref="LayOutGraph" />, this
    /// method may get called to transform the graph's layout from one rectangle
    /// to another.  <paramref name="originalLayoutContext" /> contains the
    /// original graph rectangle, and <paramref name="newLayoutContext" />
    /// contains the new graph rectangle.  This base-class implementation
    /// transforms all the graph's vertex locations from the original rectangle
    /// to the new one.  If the derived <see cref="LayOutGraphCore" />
    /// implementation added geometry metadata to the graph, the derived class
    /// should override this method, transform the geometry metadata, and call
    /// this base-class implementation to transform the graph's vertex
    /// locations.
    ///
    /// <para>
    /// The arguments have already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected virtual void
    TransformLayoutCore
    (
        IGraph graph,
        LayoutContext originalLayoutContext,
        LayoutContext newLayoutContext
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(originalLayoutContext != null);
        Debug.Assert(newLayoutContext != null);
        AssertValid();

        Matrix oTransformationMatrix = LayoutUtil.GetRectangleTransformation(
            originalLayoutContext.GraphRectangle,
            newLayoutContext.GraphRectangle
            );

        LayoutUtil.TransformVertexLocations(graph, oTransformationMatrix);

        if ( graph.ContainsKey(
            ReservedMetadataKeys.GraphHasEdgeIntermediateCurvePoints) )
        {
            TransformIntermediateCurvePoints(graph, oTransformationMatrix);
        }

        GroupMetadataManager.TransformGroupRectangles(
            graph, originalLayoutContext, newLayoutContext);
    }

    //*************************************************************************
    //  Method: TransformIntermediateCurvePoints()
    //
    /// <summary>
    /// Transforms the intermediate curve points stored on a graph's edges.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph whose layout needs to be transformed.
    /// </param>
    ///
    /// <param name="oTransformationMatrix">
    /// The matrix to use for the transformation.
    /// </param>
    //*************************************************************************

    protected void
    TransformIntermediateCurvePoints
    (
        IGraph oGraph,
        Matrix oTransformationMatrix
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oTransformationMatrix != null);
        AssertValid();

        // If an edge has an array of intermediate curve points stored on it,
        // transform the points.  Multiple edges may share one array of curve
        // points, so prevent the same array from being transformed twice by
        // using a HashSet.

        HashSet<System.Drawing.PointF[]> oTransformedArrays =
            new HashSet<System.Drawing.PointF[]>();

        System.Drawing.PointF [] aoIntermediateCurvePoints;

        foreach (IEdge oEdge in oGraph.Edges)
        {
            if (EdgeUtil.TryGetIntermediateCurvePoints(oEdge,
                    out aoIntermediateCurvePoints) 
                &&
                !oTransformedArrays.Contains(aoIntermediateCurvePoints)
                )
            {
                oTransformationMatrix.TransformPoints(
                    aoIntermediateCurvePoints);

                oTransformedArrays.Add(aoIntermediateCurvePoints);
            }
        }
    }

    //*************************************************************************
    //  Method: OnVertexMoveCore()
    //
    /// <summary>
    /// Processes a vertex that was moved after the graph was laid out.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was moved.
    /// </param>
    ///
    /// <remarks>
    /// An application may allow the user to move a vertex after the graph has
    /// been laid out by <see cref="LayOutGraphCore" />.  This method is called
    /// after the application has changed the <see cref="IVertex.Location" />
    /// property on <paramref name="vertex" />.  If <see
    /// cref="LayOutGraphCore" /> added geometry metadata to the graph,
    /// vertices, or edges, <see cref="OnVertexMoveCore" /> should modify the
    /// metadata if necessary.
    ///
    /// <para>
    /// This base-class implementation does nothing.
    /// </para>
    ///
    /// <para>
    /// The argument has already been checked for validity.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected virtual void
    OnVertexMoveCore
    (
        IVertex vertex
    )
    {
        AssertValid();

        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: LayOutGraphInternal()
    //
    /// <summary>
    /// Synchronously or asynchronously lays out a graph.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to lay out.
    /// </param>
    ///
    /// <param name="oLayoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method,
    /// or null if the graph is being laid out synchronously.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// Asynchronous event arguments, or null if the graph is being laid out
    /// synchronously.
    /// </param>
    //*************************************************************************

    protected void
    LayOutGraphInternal
    (
        IGraph oGraph,
        LayoutContext oLayoutContext,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oLayoutContext != null);
        AssertValid();

        // Perform metadata-related tasks.

        GroupMetadataManager.OnLayoutBegin(oGraph);

        LayoutContext oAdjustedLayoutContext;

        if ( !GetAdjustedLayoutContext(oGraph, oLayoutContext,
            out oAdjustedLayoutContext) )
        {
            return;
        }

        // Honor the optional LayOutTheseVerticesOnly key on the graph.

        ICollection<IVertex> oVerticesToLayOut = GetVerticesToLayOut(oGraph);
        Int32 iVerticesToLayOut = oVerticesToLayOut.Count;

        if (iVerticesToLayOut == 0)
        {
            return;
        }

        // Implementation note:
        //
        // The UseGroups and UseBinning layout styles are not currently
        // supported for synchronous layout via LayoutBase.LayOutGraph(),
        // because they are a bit complicated and aren't needed by the
        // ExcelTemplate project.

        // Groups and binning can be used only if the entire graph is being
        // laid out.

        if (iVerticesToLayOut == oGraph.Vertices.Count)
        {
            if (m_eLayoutStyle == LayoutStyle.UseGroups &&
                GroupUtil.GraphHasGroups(oGraph))
            {
                List<GroupInfo> groupsToLayOut = GetGroupsToLayOut(oGraph);

                // Pick the treemap layout if we only have one group. We could change this to <= 2 groups, but
                // that can cause tiny slivers for very small groups
                if (m_eBoxLayoutAlgorithm == BoxLayoutAlgorithm.Treemap || groupsToLayOut.Count <= 1)
                {
                    if (!LayOutGraphUsingGroups(oGraph, groupsToLayOut, oAdjustedLayoutContext,
                    oBackgroundWorker))
                    {
                        // LayOutGraphAsyncCancel() was called.

                        oDoWorkEventArgs.Cancel = true;
                    }

                    oVerticesToLayOut = new IVertex[0];
                }
                else if (m_eBoxLayoutAlgorithm == BoxLayoutAlgorithm.PackedRectangles)
                {
                    if (!LayOutGraphUsingPackedRectangles(oGraph, groupsToLayOut, oAdjustedLayoutContext,
                        oBackgroundWorker))
                    {
                        // LayOutGraphAsyncCancel() was called.
                        oDoWorkEventArgs.Cancel = true;
                    }

                    oVerticesToLayOut = new IVertex[0];
                }
                else if (m_eBoxLayoutAlgorithm == BoxLayoutAlgorithm.ForceDirected)
                {
                    if (!LayOutGraphUsingForceDirectedGroups(oGraph, groupsToLayOut, oAdjustedLayoutContext,
                        oBackgroundWorker))
                    {
                        // LayOutGraphAsyncCancel() was called.

                        oDoWorkEventArgs.Cancel = true;
                    }

                    oVerticesToLayOut = new IVertex[0];
                }
                else
                {
                    throw new NotImplementedException("There is no group-in-a-box layout algorithm defined for '" + m_eBoxLayoutAlgorithm + "'");
                }
            }
            else if ((m_eLayoutStyle == LayoutStyle.UseBinning) &&
                this.SupportsBinning)
            {
                // Lay out the graph's smaller components in bins.

                GraphBinner oGraphBinner = new GraphBinner();
                oGraphBinner.MaximumVerticesPerBin = m_iMaximumVerticesPerBin;
                oGraphBinner.BinLength = m_iBinLength;

                ICollection<IVertex> oRemainingVertices;
                Rectangle oRemainingRectangle;

                if (oGraphBinner.LayOutSmallerComponentsInBins(oGraph,
                    oVerticesToLayOut, oAdjustedLayoutContext,
                    out oRemainingVertices, out oRemainingRectangle))
                {
                    // The remaining vertices need to be laid out in the
                    // remaining rectangle.

                    oVerticesToLayOut = oRemainingVertices;

                    oAdjustedLayoutContext =
                        new LayoutContext(oRemainingRectangle);
                }
                else
                {
                    // There are no remaining vertices, or there is no space
                    // left.

                    oVerticesToLayOut = new IVertex[0];
                }
            }
        }

        if (oVerticesToLayOut.Count > 0)
        {
            // Let the derived class do the work.

            if (!LayOutGraphCore(oGraph, oVerticesToLayOut,
                oAdjustedLayoutContext, oBackgroundWorker))
            {
                // LayOutGraphAsyncCancel() was called.

                oDoWorkEventArgs.Cancel = true;
                return;
            }
        }

        LayoutMetadataUtil.MarkGraphAsLaidOut(oGraph);
    }

    //*************************************************************************
    //  Method: LayOutGraphUsingGroups()
    //
    /// <summary>
    /// Synchronously or asynchronously lays out a graph using groups.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to lay out.
    /// </param>
    ///
    /// <param name="oGroupsToLayout">
    /// A List containing a GroupInfo object for each group of vertices that
    /// should be laid out in a rectangle, sorted by the number of vertices in
    /// the group, in descending order.
    /// </param>
    ///
    /// <param name="oAdjustedLayoutContext">
    /// The LayoutContext to use.  This has already been adjusted for the
    /// layout margin.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method,
    /// or null if the graph is being laid out synchronously.
    /// </param>
    ///
    /// <returns>
    /// true if the layout was successfully completed, false if the layout was
    /// cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    LayOutGraphUsingGroups
    (
        IGraph oGraph,
        List<GroupInfo> oGroupsToLayout,
        LayoutContext oAdjustedLayoutContext,
        BackgroundWorker oBackgroundWorker
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oAdjustedLayoutContext != null);

        // Calculate a rectangle for each group.

        GroupRectangleCalculator.CalculateGroupRectangles(
            oAdjustedLayoutContext.GraphRectangle, oGroupsToLayout);

        Int32 iGroups = oGroupsToLayout.Count;

        for (Int32 i = 0; i < iGroups; i++)
        {
            GroupInfo oGroupInfo = oGroupsToLayout[i];

            Rectangle oGroupRectangle = oGroupInfo.Rectangle;
            oGroupRectangle.Inflate(-m_iMargin, -m_iMargin);

            // Do not use Rectangle.IsEmpty() here.  It returns true only if
            // the rectangle is at the origin and has zero width and height.

            if (oGroupRectangle.Width > 0 && oGroupRectangle.Height > 0)
            {
                ICollection<IVertex> oVerticesInGroup = oGroupInfo.Vertices;

                if (!GetLayoutToUseForGroup(oGraph, oVerticesInGroup).
                    LayOutGraphCore(oGraph, oVerticesInGroup,
                        new LayoutContext(oGroupRectangle), oBackgroundWorker)
                    )
                {
                    // LayOutGraphAsyncCancel() was called.

                    return (false);
                }
            }
            else
            {
                oGroupRectangle = oGroupInfo.Rectangle;
                PointF oVertexLocation;

                if (oGroupRectangle.Width > 0 && oGroupRectangle.Height > 0)
                {
                    // Put the group's vertices at the rectangle's center.

                    oVertexLocation = new PointF(
                        oGroupRectangle.Left + oGroupRectangle.Width / 2,
                        oGroupRectangle.Top + oGroupRectangle.Height / 2
                        );
                }
                else
                {
                    // Put the group's vertices at the rectangle's upper-left
                    // corner.

                    oVertexLocation = new PointF(
                        oGroupRectangle.X, oGroupRectangle.Y
                        );
                }

                foreach (IVertex oVertex in oGroupInfo.Vertices)
                {
                    oVertex.Location = oVertexLocation;
                }
            }
        }

        // Perform metadata-related tasks.

        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph, oGroupsToLayout,
            this.GroupRectanglePenWidth, this.IntergroupEdgeStyle);

        return (true);
    }

    //*************************************************************************
    //  Method: LayOutGraphUsingPackedRectangles()
    //
    /// <summary>
    /// Synchronously or asynchronously lays out a graph using groups.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to lay out.
    /// </param>
    ///
    /// <param name="oGroupsToLayout">
    /// A List containing a GroupInfo object for each group of vertices that
    /// should be laid out in a rectangle, sorted by the number of vertices in
    /// the group, in descending order.
    /// </param>
    /// 
    /// <param name="oAdjustedLayoutContext">
    /// The LayoutContext to use.  This has already been adjusted for the
    /// layout margin.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method,
    /// or null if the graph is being laid out synchronously.
    /// </param>
    ///
    /// <returns>
    /// true if the layout was successfully completed, false if the layout was
    /// cancelled.
    /// </returns>
    //*************************************************************************
    protected Boolean
    LayOutGraphUsingPackedRectangles
    (
        IGraph oGraph,
        List<GroupInfo> oGroupsToLayout,
        LayoutContext oAdjustedLayoutContext,
        BackgroundWorker oBackgroundWorker
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oAdjustedLayoutContext != null);

        // Calculate a rectangle for each group.
        double alpha = 0.99;
        PackedGroupRectangleCalculator.CalculateGroupRectangles(
            oAdjustedLayoutContext.GraphRectangle, oGroupsToLayout, alpha); //ETM: groupsToLayOut is sorted in decreasing order of size

        Int32 iGroups = oGroupsToLayout.Count;

        for (Int32 i = 0; i < iGroups; i++)
        {
            GroupInfo oGroupInfo = oGroupsToLayout[i];

            Rectangle oGroupRectangle = oGroupInfo.Rectangle;
            oGroupRectangle.Inflate(-m_iMargin, -m_iMargin);

            // Do not use Rectangle.IsEmpty() here.  It returns true only if
            // the rectangle is at the origin and has zero width and height.

            if (oGroupRectangle.Width > 0 && oGroupRectangle.Height > 0)
            {
                ICollection<IVertex> oVerticesInGroup = oGroupInfo.Vertices;

                if (!GetLayoutToUseForGroup(oGraph, oVerticesInGroup).
                    LayOutGraphCore(oGraph, oVerticesInGroup,
                        new LayoutContext(oGroupRectangle), oBackgroundWorker)
                    )
                {
                    // LayOutGraphAsyncCancel() was called.

                    return (false);
                }
            }
            else
            {
                oGroupRectangle = oGroupInfo.Rectangle;
                PointF oVertexLocation;

                if (oGroupRectangle.Width > 0 && oGroupRectangle.Height > 0)
                {
                    // Put the group's vertices at the rectangle's center.

                    oVertexLocation = new PointF(
                        oGroupRectangle.Left + oGroupRectangle.Width / 2,
                        oGroupRectangle.Top + oGroupRectangle.Height / 2
                        );
                }
                else
                {
                    // Put the group's vertices at the rectangle's upper-left
                    // corner.

                    oVertexLocation = new PointF(
                        oGroupRectangle.X, oGroupRectangle.Y
                        );
                }

                foreach (IVertex oVertex in oGroupInfo.Vertices)
                {
                    oVertex.Location = oVertexLocation;
                }
            }
        }

        // Perform metadata-related tasks.
        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph, oGroupsToLayout,
            this.GroupRectanglePenWidth, this.IntergroupEdgeStyle);

        return (true);
    }

    //*************************************************************************
    //  Method: LayOutGraphUsingForceDirectedGroups()
    //
    /// <summary>
    /// Synchronously or asynchronously lays out a graph using groups.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to lay out.
    /// </param>
    ///
    /// <param name="oGroupsToLayout">
    /// A List containing a GroupInfo object for each group of vertices that
    /// should be laid out in a rectangle, sorted by the number of vertices in
    /// the group, in descending order.
    /// </param>
    /// 
    /// <param name="oAdjustedLayoutContext">
    /// The LayoutContext to use.  This has already been adjusted for the
    /// layout margin.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// <see cref="BackgroundWorker" /> whose worker thread called this method,
    /// or null if the graph is being laid out synchronously.
    /// </param>
    ///
    /// <returns>
    /// true if the layout was successfully completed, false if the layout was
    /// cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    LayOutGraphUsingForceDirectedGroups
    (
        IGraph oGraph,
        List<GroupInfo> oGroupsToLayout,
        LayoutContext oAdjustedLayoutContext,
        BackgroundWorker oBackgroundWorker
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oAdjustedLayoutContext != null);

        /////////////////////////////////////////////////////
        // Get the initial position for the group vertices //
        /////////////////////////////////////////////////////

        // Determine how much of the screen to use for groups
        Single areaPerVertex = (Single)GroupRectangleCalculator.GetAreaPerVertex(oAdjustedLayoutContext.GraphRectangle, oGroupsToLayout);
        areaPerVertex /= 2f; // Customise this value if necessary to have more whitespace between groups -- expose as a user control?

        // Create a graph for the groups

        Graph groupInitialGraph = new Graph(GraphDirectedness.Undirected);

        // Convert GroupInfos into vertices that can be laid out
        //IEnumerable<GroupVertex> groupInitialGraphVertices = groupsToLayOut.Select<GroupInfo, GroupVertex>(g => new GroupVertex(g, groupInitialGraph));
        List<GroupVertex> groupInitialGraphVertices = new List<GroupVertex>();

        foreach (GroupInfo groupInfo in oGroupsToLayout)
        {
            GroupVertex groupVertex = new GroupVertex(groupInfo, areaPerVertex);
            groupInitialGraphVertices.Add(groupVertex);
            groupInitialGraph.Vertices.Add(groupVertex);
        }

        Int32 iGroups = oGroupsToLayout.Count;

        // Add edges between groups
        IVertexCollection oVertices = groupInitialGraph.Vertices;
        IEdgeCollection oEdges = groupInitialGraph.Edges;

        IList<IntergroupEdgeInfo> intergroupEdgeInfos = new IntergroupEdgeCalculator().CalculateGraphMetrics(oGraph, oGroupsToLayout, false);
        foreach (IntergroupEdgeInfo interGroupEdgeInfo in intergroupEdgeInfos)
        {
            IVertex groupVertex1 = oVertices.ElementAt<IVertex>(interGroupEdgeInfo.Group1Index);
            IVertex groupVertex2 = oVertices.ElementAt<IVertex>(interGroupEdgeInfo.Group2Index);

            if (groupVertex1 != groupVertex2)
            {
                oEdges.Add(groupVertex1, groupVertex2, false);
            }
        }

        // Use Harel-Koren to compute initial group positions, stored in groupVertex.Location
        ILayout oLayout = new HarelKorenFastMultiscaleLayout();
        oLayout.LayOutGraph(groupInitialGraph, oAdjustedLayoutContext);

        foreach (GroupVertex groupVertex in groupInitialGraph.Vertices)
        {
            // If we have too few groups, automatically push them into the AdjustedLayoutContext
            if (groupInitialGraph.Vertices.Count <= 3)
            {
                groupVertex.updateGroupInfoRectangleFromLocation();
                groupVertex.groupInfo.Rectangle = GraphicsUtil.RectangleFToRectangle(GraphicsUtil.MoveRectangleWithinBounds(groupVertex.groupInfo.Rectangle, oAdjustedLayoutContext.GraphRectangle, false), 1);
                RectangleF r = groupVertex.groupInfo.Rectangle;
                groupVertex.Location = new PointF(r.X + r.Width / 2, r.Y + r.Height / 2);
            }
            // Update the meta-node locations
            groupVertex.updateGroupInfoLocationFromLocation();
        }

        /////////////////////////
        // Remove node overlap //
        /////////////////////////

        //  while ( overlaps exist along proximity graph edges )
        //  {
        //      Create proximity graph (delaunay triangulation)
        //      Minimize proxmity graph overlap stress function
        //  }
        //
        //  while ( any overlaps exist )
        //  {
        //      Create proximity graph (delaunay triangulation)
        //      Find overlaps (scan-line alg)
        //      Augment proximity graph with edges for overlaps
        //      Minimize proxmity graph overlap stress function
        //  }

        Boolean initialOverlapInProximityGraph;
        Int32 iteration = 0;
        Graph lastGraphAfterFirstPhase = groupInitialGraph;
        do
        {
            Graph groupProximityGraph = createGroupVertexProximityGraph(lastGraphAfterFirstPhase);
            lastGraphAfterFirstPhase = groupProximityGraph;
            initialOverlapInProximityGraph = proximityGraphStressMajorization(groupProximityGraph, 1000);
            iteration++;
        } while (initialOverlapInProximityGraph == true);
        

        ///////////////////////////////////////////////////////////////////
        // Transform the layout to get the boxes within the LayoutContext//
        ///////////////////////////////////////////////////////////////////

        Rectangle boundingRectangle = oAdjustedLayoutContext.GraphRectangle;

        Int32 overallLeftOffset = Int32.MinValue;
        Int32 overallTopOffset = Int32.MinValue;
        Int32 overallRightOffset = Int32.MinValue;
        Int32 overallBottomOffset = Int32.MinValue;

        foreach (GroupInfo groupInfo in oGroupsToLayout)
        {
            Rectangle groupRectangle = groupInfo.Rectangle;

            Int32 rectangleLeftOffset = boundingRectangle.Left - groupRectangle.Left;
            if (rectangleLeftOffset > overallLeftOffset)
            {
                overallLeftOffset = rectangleLeftOffset;
            }
            Int32 rectangleTopOffset = boundingRectangle.Top - groupRectangle.Top;
            if (rectangleTopOffset > overallTopOffset)
            {
                overallTopOffset = rectangleTopOffset;
            }

            Int32 rectangleRightOffset = groupRectangle.Right - boundingRectangle.Right;
            if (rectangleRightOffset > overallRightOffset)
            {
                overallRightOffset = rectangleRightOffset;
            }
            Int32 rectangleBottomOffset = groupRectangle.Bottom - boundingRectangle.Bottom;
            if (rectangleBottomOffset > overallBottomOffset)
            {
                overallBottomOffset = rectangleBottomOffset;
            }
        }

        Rectangle newBoundingRectangle = new Rectangle(
            boundingRectangle.X - overallLeftOffset, boundingRectangle.Y - overallTopOffset,
            boundingRectangle.Width + overallLeftOffset + overallRightOffset, boundingRectangle.Height + overallTopOffset + overallBottomOffset);

        // Perform metadata-related tasks.

        GroupMetadataManager.OnLayoutUsingGroupsEnd(lastGraphAfterFirstPhase, oGroupsToLayout,
            this.GroupRectanglePenWidth, this.IntergroupEdgeStyle);

        lastGraphAfterFirstPhase.SetValue(ReservedMetadataKeys.GroupLayoutDrawingInfo,
            new GroupLayoutDrawingInfo(oGroupsToLayout, this.GroupRectanglePenWidth, null));

        //TransformLayout(oGraph, new LayoutContext(newBoundingRectangle), oAdjustedLayoutContext);
        TransformLayout(lastGraphAfterFirstPhase, new LayoutContext(newBoundingRectangle), oAdjustedLayoutContext);

        ///////////////////////////////////
        // Finish with individual groups //
        ///////////////////////////////////

        for (Int32 i = 0; i < iGroups; i++)
        {
            GroupInfo oGroupInfo = oGroupsToLayout[i];

            Rectangle oGroupRectangle = oGroupInfo.Rectangle;
            oGroupRectangle.Inflate(-m_iMargin, -m_iMargin);

            // Do not use Rectangle.IsEmpty() here.  It returns true only if
            // the rectangle is at the origin and has zero width and height.

            if (oGroupRectangle.Width > 0 && oGroupRectangle.Height > 0)
            {
                ICollection<IVertex> oVerticesInGroup = oGroupInfo.Vertices;

                if ( !GetLayoutToUseForGroup(oGraph, oVerticesInGroup).
                    LayOutGraphCore(oGraph, oVerticesInGroup,
                        new LayoutContext(oGroupRectangle), oBackgroundWorker)
                    )
                {
                    // LayOutGraphAsyncCancel() was called.

                    return (false);
                }
            }
            else
            {
                oGroupRectangle = oGroupInfo.Rectangle;
                PointF oVertexLocation;

                if (oGroupRectangle.Width > 0 && oGroupRectangle.Height > 0)
                {
                    // Put the group's vertices at the rectangle's center.

                    oVertexLocation = new PointF(
                        oGroupRectangle.Left + oGroupRectangle.Width / 2,
                        oGroupRectangle.Top + oGroupRectangle.Height / 2
                        );
                }
                else
                {
                    // Put the group's vertices at the rectangle's upper-left
                    // corner.

                    oVertexLocation = new PointF(
                        oGroupRectangle.X, oGroupRectangle.Y
                        );
                }

                foreach (IVertex oVertex in oGroupInfo.Vertices)
                {
                    oVertex.Location = oVertexLocation;
                }
            }
        }

        // Perform metadata-related tasks.

        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph, oGroupsToLayout,
            this.GroupRectanglePenWidth, this.IntergroupEdgeStyle);

        return (true);
    }

    //*************************************************************************
    //  Method: createGroupVertexProximityGraph()
    //
    /// <summary>
    /// Creates the proximity graph (delaunay triangulation) of the node
    /// positions in the group graph.
    /// </summary>
    /// <param name="graph">
    /// The group graph to create a proximity graph from.
    /// </param>
    /// 
    /// <returns>
    /// The proximity graph.
    /// </returns>
    //*************************************************************************

    private static Graph createGroupVertexProximityGraph(IGraph graph)
    {
        Graph groupProximityGraph = new Graph(GraphDirectedness.Undirected);
        List<GroupVertex> groupProximityGraphVertices = new List<GroupVertex>();
            
        foreach (GroupVertex groupVertex in graph.Vertices)
        {
            // Update group collapsed location with the result of the initial layout
            //groupVertex.updateGroupInfoLocationAndRectangleFromLocation(areaPerVertex);
            groupVertex.updatePositionFromLocation();
        }

        foreach (GroupVertex groupVertex in graph.Vertices)
        {
            GroupVertex newGroupVertex = new GroupVertex(groupVertex);
            groupProximityGraphVertices.Add(newGroupVertex);
            groupProximityGraph.Vertices.Add(newGroupVertex);
        }

        // If we have a degenerate case do nothing
        if (graph.Vertices.Count == 0)
        {
            // Do nothing
        }
        // If we have fewer group vertices/points than the delaunay triangulation code can handle (0-3) do it manually
        else if (groupProximityGraphVertices.Count <= 3)
        {
            // Connect all the vertices in a clique
            for (Int32 i = 0; i < groupProximityGraphVertices.Count; i++)
            {
                IVertex groupVertex1 = groupProximityGraphVertices.ElementAt(i);

                for (Int32 j = i + 1; j < groupProximityGraphVertices.Count; j++)
                {
                    IVertex groupVertex2 = groupProximityGraphVertices.ElementAt(j);

                    groupProximityGraph.Edges.Add(new Edge(groupVertex1, groupVertex2, false));
                }
            }
        }
        else
        {
            // Use the node positions stored in GroupVertex.Position for the delaunay triangulation
            // Ensure the position has been set ahead of time (e.g., using groupVertex.updatePositionFromLocation())
            ITriangulation<GroupVertex, DefaultTriangulationCell<GroupVertex>> delaunayTriangulation = Triangulation.CreateDelaunay<GroupVertex>(groupProximityGraphVertices);

            // Each delaunay triangulation cell is a triangle with our group vertices as its vertices
            // Add edges to the proximity graph from the triangle edges
            foreach (DefaultTriangulationCell<GroupVertex> cell in delaunayTriangulation.Cells)
            {
                Debug.Assert(cell.Vertices.Count() == 3);

                // Add edges for the three triangle faces
                groupProximityGraph.Edges.Add(new Edge(cell.Vertices[0], cell.Vertices[1], false));
                groupProximityGraph.Edges.Add(new Edge(cell.Vertices[0], cell.Vertices[2], false));
                groupProximityGraph.Edges.Add(new Edge(cell.Vertices[1], cell.Vertices[2], false));
            }

            // Two triangles can share an edge, so remove one of them
            groupProximityGraph.Edges.RemoveDuplicates();

            //// Update group vertex location with the positions from the delaunay triangulation
            //foreach (GroupVertex groupVertex in groupProximityGraph.Vertices)
            //{
            //    groupVertex.updateLocationFromPosition();
            //}
        }
        return groupProximityGraph;
    }

    //*************************************************************************
    //  Method: proximityGraphStressMajorization()
    //
    /// <summary>
    /// Run iterations of stress majorization to eliminate overlap on a 
    /// proximity graph.
    /// </summary>
    /// 
    /// <param name="groupProximityGraph">
    /// The proximity graph to run stress majorization on.
    /// </param>
    /// 
    /// <param name="maxIterations">
    /// The maximum number of interations of stress majorization to run.
    /// </param>
    /// 
    /// <returns>
    /// Whether there was any overlap initially.
    /// </returns>
    //*************************************************************************

    private Boolean
    proximityGraphStressMajorization
    (
        IGraph groupProximityGraph,
        int maxIterations
    )
    {
        // Graph X = n x 2 matrix. X_i refers to a node's points, X^(1) is the horizontal axis and X^(2) is the vertical. X(t) refers to a time step.

        // CG ALGORITHM
        // δ_{ij} = w_{ij} * d_{ij}
        // Weighted Laplacian L^w = n x n matrix. Constant througout solving
        // L^X defined p.241. Recomputed at each iteration.
        // Iteratively solve:
        //      L^w * X(t+1)^a = L^{X(t)} * X(t)^a
        //      constant n x n matrix * n x 1 vector = n x n matrix * n x 1 vector
        //      n x 1 vector - n x 1 vector = 0
        //      n x 1 vector = 0
        //      for
        //      a = 1,2

        // LOCAL ALGORITHM -- the one we use here
        Boolean currentOverlapInProximityGraph = true;
        Boolean initialOverlapInProximityGraph = false;
        int i;
        for (i = 0; i < maxIterations && currentOverlapInProximityGraph; i++)
        {
            currentOverlapInProximityGraph = false;

            foreach (GroupVertex groupVertex in groupProximityGraph.Vertices)
            {
                groupVertex.OldLocation = groupVertex.Location;
            }

            foreach (GroupVertex groupVertex1 in groupProximityGraph.Vertices)
            {
                Single xNumerator = 0.0f;
                Single yNumerator = 0.0f;
                Single denominator = 0.0f;

                foreach (Edge edge in groupVertex1.IncidentEdges) // O(n) operation... // Each edge seen twice...
                {
                    GroupVertex groupVertex2 = (GroupVertex)edge.GetAdjacentVertex(groupVertex1);

                    Debug.Assert(groupVertex1 != groupVertex2);// If this happens we have an infinite loop...

                    // Calculate an overlap factor for each pair of nodes connected by an edge in the proxmity graph

                    Rectangle groupRectangle1 = groupVertex1.groupInfo.Rectangle;
                    Rectangle groupRectangle2 = groupVertex2.groupInfo.Rectangle;

                    Single widthOverlapRatio =
                        ((Single)groupRectangle1.Width / 2.0f + (Single)groupRectangle2.Width / 2.0f) /
                        Math.Abs(groupVertex1.OldLocation.X - groupVertex2.OldLocation.X);

                    Single heightOverlapRatio =
                        ((Single)groupRectangle1.Height / 2.0f + (Single)groupRectangle2.Height / 2.0f) /
                        Math.Abs(groupVertex1.OldLocation.Y - groupVertex2.OldLocation.Y);

                    // t_{ij} -- Overlap factor
                    Single overlapFactor = Math.Max(Math.Min(widthOverlapRatio, heightOverlapRatio), 1.0f);

                    if (!isApproximatelyEqual(overlapFactor - 1.0f, 0.0f, 0.0000001f))
                    {
                        //Debug.Print("Overlap: " + groupVertex1.Name + " " + groupVertex2.Name);
                        currentOverlapInProximityGraph = true;
                        initialOverlapInProximityGraph = true;
                    }

                    // s_max -- Max overlap removal per interaction, which must be > 1. Gansner & Hu 2009 report that 1.5 is good.
                    Single maxOverlapRemovalPerIteration = 1.5f;

                    // s_{ij} -- Scaling factor for the overlap factor
                    Single overlapFactorScalingFactor = Math.Min(overlapFactor, maxOverlapRemovalPerIteration);

                    Single distanceBetweenVertices = GraphicsUtil.DistanceBetweenTwoPointFs(groupVertex1.OldLocation, groupVertex2.OldLocation);

                    // d_{ij} -- Edge ideal distance
                    Single edgeIdealDistance = overlapFactorScalingFactor * distanceBetweenVertices;

                    // w_{ij} -- Scaling factor. The double bars around the d_{ij} term in the paper are typos
                    Single scalingFactor = 1.0f / edgeIdealDistance / edgeIdealDistance;

                    Single xDifference = groupVertex1.OldLocation.X - groupVertex2.OldLocation.X;
                    Single yDifference = groupVertex1.OldLocation.Y - groupVertex2.OldLocation.Y;

                    Single xPart = scalingFactor * (groupVertex2.OldLocation.X + edgeIdealDistance * xDifference * inverse(distanceBetweenVertices));
                    Single yPart = scalingFactor * (groupVertex2.OldLocation.Y + edgeIdealDistance * yDifference * inverse(distanceBetweenVertices));
                        
                    xNumerator += xPart;
                    yNumerator += yPart;

                    denominator += scalingFactor;
                }

                groupVertex1.Location = new PointF(xNumerator / denominator, yNumerator / denominator);
            }
        }

        return initialOverlapInProximityGraph;
    }


    //*************************************************************************
    //  Method: inverse()
    //
    /// <summary>
    /// Simply calculates the inverse of a value.
    /// </summary>
    /// 
    /// <param name="val">
    /// The value to inverse.
    /// </param>
    /// 
    /// <returns>
    /// The inverse.
    /// </returns>
    //*************************************************************************

    private Single
    inverse
    (
        Single val
    )
    {
        if (isApproximatelyEqual(val, 0.0f, 0.0000001f))
        {
            return 0.0f;
        } 
        else 
        {
            return 1.0f / val;
        }
    }

    //*************************************************************************
    //  Method: isApproximatelyEqual()
    //
    /// <summary>
    /// Determines whether two values are within epsilon of each other.
    /// </summary>
    /// 
    /// <param name="value1">
    /// Value A
    /// </param>
    /// 
    /// <param name="value2">
    /// Valube B
    /// </param>
    /// 
    /// <param name="epsilon">
    /// The largest difference A-B where A and B are still approximately equal.
    /// </param>
    //*************************************************************************

    private static bool 
    isApproximatelyEqual
    (
        Single value1,
        Single value2,
        Single epsilon
    )
    {
        // If they are equal anyway, just return True. 
        if (value1.Equals(value2))
        {
            return true;
        }

        // Handle NaN, Infinity. 
        if (Single.IsInfinity(value1) | Single.IsNaN(value1))
        {
            return value1.Equals(value2);
        }
        else if (Single.IsInfinity(value2) | Single.IsNaN(value2))
        {
            return value1.Equals(value2);
        }

        // Handle zero to avoid division by zero 
        Single divisor = Math.Max(value1, value2);
        if (divisor.Equals(0))
        {
            divisor = Math.Min(value1, value2);
        }

        return Math.Abs(value1 - value2) / divisor <= epsilon;
    }

    //*************************************************************************
    //  Class: GroupVertex
    //
    /// <summary>
    /// A special representation of a group used for calculating the 
    /// Force-Directed Group-in-a-Box layout.
    /// </summary>
    //*************************************************************************

    class GroupVertex : Vertex, MICHIVertex
    {
        public GroupInfo groupInfo;
        private Single areaPerVertex;

        public GroupVertex(GroupInfo groupInfo, Single areaPerVertex)
        {
            this.Name = groupInfo.Name;
            this.groupInfo = groupInfo;
            this.areaPerVertex = areaPerVertex;
        }

        public GroupVertex(GroupVertex other)
            : this(other.groupInfo, other.areaPerVertex)
        {
            // Position and location may not exist...
            Position = new double[] { other.Position[0], other.Position[1] };
            Location = new PointF(other.Location.X, other.Location.Y);
        }

        public double[] Position { get; set; }

        /// <summary>
        /// Updates the MIConvexHull <see cref="MICHIVertex" /> Position using the NodeXL <see cref="Vertex" /> Location
        /// </summary>
        public void updatePositionFromLocation()
        {
            Position = new double[] { Location.X, Location.Y };
        }

        /// <summary>
        /// Updates the NodeXL <see cref="Vertex" /> Location using the MIConvexHull <see cref="MICHIVertex" /> Position
        /// </summary>
        public void updateLocationFromPosition()
        {
            double[] pos = Position;
            Location = new PointF((float)pos[0], (float)pos[1]);
        }

        /// <summary>
        /// Updates the <see cref="GroupInfo" /> CollapsedLocation using the <see cref="Vertex" /> Location
        /// as well as the GroupInfo.Rectangle
        /// </summary>
        public void updateGroupInfoLocationFromLocation
        (
        )
        {
            groupInfo.CollapsedLocation = Location;
            //updateGroupInfoRectangleFromLocation(areaPerVertex);
        }

        public void updateGroupInfoRectangleFromLocation
        (
        )
        {
            // Create a square box around the group
            Single x = Location.X;
            Single y = Location.Y;
            Single area = areaPerVertex * (Single)groupInfo.Vertices.Count;
            float width = (float)Math.Sqrt(area);
            float height = width;

            RectangleF groupRectangle = new RectangleF(x - width / 2, y - height / 2, width, height);

            //groupRectangle = GraphicsUtil.MoveRectangleWithinBounds(groupRectangle, oAdjustedLayoutContext.GraphRectangle, false);

            groupInfo.Rectangle = GraphicsUtil.RectangleFToRectangle(groupRectangle, 1);
        }

        public new PointF 
        Location
        {
            get
            {
                AssertValid();

                return (m_oLocation);
            }

            set
            {
                m_oLocation = value;
                updateGroupInfoRectangleFromLocation();
                AssertValid();
            }
        }

        public PointF
        OldLocation
        {
            get
            {
                AssertValid();

                return (m_oOldLocation);
            }

            set
            {
                m_oOldLocation = value;

                AssertValid();
            }
        }

        protected PointF m_oOldLocation;
    }


    //*************************************************************************
    //  Method: GetGroupsToLayOut()
    //
    /// <summary>
    /// Lays out a graph on a BackgroundWorker thread using groups.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to lay out.
    /// </param>
    ///
    /// <returns>
    /// A List containing a GroupInfo object for each group of vertices that
    /// should be laid out in a rectangle, sorted by the number of vertices in
    /// the group, in descending order.
    /// </returns>
    ///
    /// <remarks>
    /// This method analyzes the graph's groups and creates a new List of
    /// groups from them.  Empty and collapsed groups are skipped, and any
    /// vertices that aren't in a group are put into a new group.  The list is
    /// then sorted.
    /// </remarks>
    //*************************************************************************

    protected List<GroupInfo>
    GetGroupsToLayOut
    (
        IGraph oGraph
    )
    {
        Debug.Assert(oGraph != null);

        List<GroupInfo> oGroupsToLayOut =
            GroupUtil.GetGroupsWithAllVertices(oGraph, true);

        oGroupsToLayOut.Sort(
            (a,b) => -a.Vertices.Count.CompareTo(b.Vertices.Count) );

        return (oGroupsToLayOut);
    }

    //*************************************************************************
    //  Method: GetLayoutToUseForGroup()
    //
    /// <summary>
    /// Gets the <see cref="LayoutBase" /> object to use to lay out a group.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to lay out.  The graph is guaranteed to have at least one vertex.
    /// </param>
    ///
    /// <param name="oVerticesInGroup">
    /// Vertices in the group.  The collection is guaranteed to have at least
    /// one vertex.
    /// </param>
    ///
    /// <returns>
    /// The <see cref="LayoutBase" /> object to use to lay out the group.
    /// </returns>
    //*************************************************************************

    protected LayoutBase
    GetLayoutToUseForGroup
    (
        IGraph oGraph,
        ICollection<IVertex> oVerticesInGroup
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oVerticesInGroup != null);
        Debug.Assert(oVerticesInGroup.Count > 0);
        AssertValid();

        if ( m_bImproveLayoutOfGroups &&
            UseGridLayoutForGroup(oVerticesInGroup) )
        {
            // Use a grid layout, and sort the vertices by layout order.

            GridLayout oGridLayout = new GridLayout();
            oGridLayout.UseMetadataVertexSorter(oGraph);

            return (oGridLayout);
        }

        // Use the layout implemented by the derived class.

        return (this);
    }

    //*************************************************************************
    //  Method: UseGridLayoutForGroup()
    //
    /// <summary>
    /// Returns a flag indicating whether a grid layout should be used to lay
    /// out a group.
    /// </summary>
    ///
    /// <param name="oVerticesInGroup">
    /// Vertices in the group.  The collection is guaranteed to have at least
    /// one vertex.
    /// </param>
    ///
    /// <returns>
    /// true if a grid layout should be used.
    /// </returns>
    //*************************************************************************

    protected Boolean
    UseGridLayoutForGroup
    (
        ICollection<IVertex> oVerticesInGroup
    )
    {
        Debug.Assert(oVerticesInGroup != null);
        Debug.Assert(oVerticesInGroup.Count > 0);
        AssertValid();

        // Create a new subgraph from the vertices in the group and the edges
        // that connect them, then count the edges that aren't self-loops.

        IGraph oSubgraph = SubgraphCalculator.GetSubgraphAsNewGraph(
            oVerticesInGroup);

        return (oSubgraph.Edges.Count(oEdge => !oEdge.IsSelfLoop)
            <= MaximumGroupEdgeCountToGrid);
    }

    //*************************************************************************
    //  Method: FireLayOutGraphCompleted()
    //
    /// <summary>
    /// Fires the <see cref="LayOutGraphCompleted" /> event if appropriate.
    /// </summary>
    ///
    /// <param name="oAsyncCompletedEventArgs">
    /// An <see cref="AsyncCompletedEventArgs" /> that contains the event data.
    /// </param>
    //*************************************************************************

    protected void
    FireLayOutGraphCompleted
    (
        AsyncCompletedEventArgs oAsyncCompletedEventArgs
    )
    {
        AssertValid();

        AsyncCompletedEventHandler oEventHandler =
            this.LayOutGraphCompleted;

        if (oEventHandler != null)
        {
            oEventHandler(this, oAsyncCompletedEventArgs);
        }
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_DoWork
    (
        Object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(sender != null);
        Debug.Assert(e != null);
        AssertValid();

        Debug.Assert(e.Argument is LayOutGraphAsyncArguments);

        LayOutGraphAsyncArguments oLayOutGraphAsyncArguments =
            (LayOutGraphAsyncArguments)e.Argument;

        Debug.Assert(sender is BackgroundWorker);

        LayOutGraphInternal(oLayOutGraphAsyncArguments.Graph,
            oLayOutGraphAsyncArguments.LayoutContext, (BackgroundWorker)sender,
            e);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_RunWorkerCompleted()
    //
    /// <summary>
    /// Handles the RunWorkerCompleted event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    BackgroundWorker_RunWorkerCompleted
    (
        Object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        FireLayOutGraphCompleted(e);
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

        Debug.Assert(m_iMargin >= 0);
        // m_eLayoutStyle
        Debug.Assert(m_dGroupRectanglePenWidth >= 0);
        // m_eIntergroupEdgeStyle
        // m_bImproveLayoutOfGroups
        Debug.Assert(m_iMaximumVerticesPerBin >= 1);
        Debug.Assert(m_iBinLength >= 1);
        Debug.Assert(m_oBackgroundWorker != null);
    }

    //*************************************************************************
    //  Method: GetGroupsToLayOutConnectivity()
    //
    /// <summary>
    /// Gets the groups from the graph and ensures they have connectivity
    /// calculated.
    /// </summary>
    /// 
    /// <param name="oGraph">
    /// The graph to examine.
    /// </param>
    /// 
    /// <returns>
    /// A list of GroupInfo objects from the graph sorted by connectivity.
    /// </returns>
    //*************************************************************************

    protected List<GroupInfo>
    GetGroupsToLayOutConnectivity
    (
        IGraph oGraph
    )
    {
        Debug.Assert(oGraph != null);
        List<GroupInfo> oGroupsToLayOut = GroupUtil.GetGroupsWithAllVertices(oGraph, true);
        List<GroupInfo> filteredList = new List<GroupInfo>();
        //iterate through groups -> 
        //get intergroup edge number of each group
        //sort based on 

        int connectivity;
        //calculate connectivity of each group relative to another group and then sort according to that
        foreach (GroupInfo g in oGroupsToLayOut)
        {
            connectivity = 0;
            foreach (GroupInfo g2 in oGroupsToLayOut)
            {

                if (!g.Equals(g2))
                {
                System.Diagnostics.Debug.Print(g.Label + " " + getConnectivityBetween(g, g2, oGraph).ToString() + " " + g2.Label);
                    if (getConnectivityBetween(g, g2, oGraph)){
                    connectivity += 1;
                    System.Diagnostics.Debug.Print(g.Label + " is connected to " + g2.Label);
                    }
                }
            }
            g.Connectivity = connectivity;
            System.Diagnostics.Debug.Print("Total connectivity of " + g.Label + "=" + connectivity);

            if (!(g.Connectivity < 1 && g.Vertices.Count < 2))
            {                 
                filteredList.Add(g);
            }else{
            oGraph.Vertices.Remove(g.Vertices.First.Value.ID);
            }
        }
        filteredList.Sort(
            (a, b) => -a.CompareConnectivity(b));
        return (filteredList);
    }

    //*************************************************************************
    //  Method: getConnectivityBetween()
    //
    /// <summary>
    /// Get whether there is connectivity between two groups in a graph.
    /// </summary>
    /// 
    /// <param name="g1">
    /// Group 1
    /// </param>
    /// 
    /// <param name="g2">
    /// Group 2
    /// </param>
    /// 
    /// <param name="oGraph">
    /// The graph these groups are in
    /// </param>
    /// 
    /// <returns>
    /// True if there is connectivity, false otherwise.
    /// </returns>
    //*************************************************************************

    public bool getConnectivityBetween(GroupInfo g1, GroupInfo g2, IGraph oGraph)
    {

        IntergroupEdgeCalculator m_oIntergroupEdgeCalculator = new IntergroupEdgeCalculator();

        // Currently uses undirected graph
        IList<IntergroupEdgeInfo> oIntergroupEdges =
            m_oIntergroupEdgeCalculator.CalculateGraphMetrics(oGraph,
                new GroupInfo[] { g1, g2 }, false);

        foreach (IntergroupEdgeInfo e in oIntergroupEdges)
        {
            if (e.Group1Index == 0 && e.Group2Index == 1)
            {
                return e.Edges > 0;
        }
        }
        return false;
    }

    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum number of non-self-loop edges in a group for the group to be
    /// laid out in a grid when m_bImproveLayoutOfGroups is true.

    protected Single MaximumGroupEdgeCountToGrid = 3;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Margin to subtract from the graph rectangle before laying out the
    /// graph.

    protected Int32 m_iMargin;

    /// The style to use when laying out the graph.

    protected LayoutStyle m_eLayoutStyle;

    /// The algorithm to use when laying out groups.

    protected BoxLayoutAlgorithm m_eBoxLayoutAlgorithm;

    /// The width of the pen used to draw group rectangles.

    protected Double m_dGroupRectanglePenWidth;

    /// Specifies how the edges that connect vertices in different groups
    /// should be shown.

    protected IntergroupEdgeStyle m_eIntergroupEdgeStyle;

    /// true to attempt to improve the appearance of groups.

    protected Boolean m_bImproveLayoutOfGroups;

    /// The maximum number of vertices a binned component can have.

    protected Int32 m_iMaximumVerticesPerBin;

    /// Height and width of each bin, in graph rectangle units.

    protected Int32 m_iBinLength;

    /// BackgroundWorker used by LayOutGraphAsync().

    protected BackgroundWorker m_oBackgroundWorker;
}

}
