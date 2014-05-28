
using System;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Interface: ILayout
//
/// <summary>
/// Supports laying out a graph within a rectangle.
/// </summary>
///
/// <remarks>
/// A class that implements this interface is responsible for laying out a
/// graph within a specified rectangle by setting the <see
/// cref="IVertex.Location" /> property on all of the graph's vertices, and
/// optionally adding geometry metadata to the graph, vertices, or edges.
/// Laying out a graph is the first step in drawing it.
/// </remarks>
///
/// <para>
/// The interface has both synchronous (<see cref="LayOutGraph" />) and
/// asynchronous (<see cref="LayOutGraphAsync" />) methods for laying out a
/// graph.
/// </para>
///
/// <para>
/// The asynchronous semantics follow the guidelines outlined in the article
/// "Multithreaded Programming with the Event-based Asynchronous Pattern" in
/// the .NET Framework Developer's Guide.  <see cref="LayOutGraphAsync" />
/// starts the layout on a worker thread and returns immediately.  The <see
/// cref="LayOutGraphCompleted" /> event fires when the layout is complete, an
/// error occurs, or the layout is cancelled.  <see
/// cref="LayOutGraphAsyncCancel" /> cancels the layout.
/// </para>
//*****************************************************************************

public interface ILayout
{
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
    /// to zero.  The units are determined by the <see cref="Graphics" />
    /// object used to draw the graph.  The default value is 0.
    /// </value>
    ///
    /// <remarks>
    /// If the graph rectangle passed to <see cref="LayOutGraph" /> is {L=0,
    /// T=0, R=50, B=30} and the <see cref="Margin" /> is 5, for example, then
    /// the graph is laid out within the rectangle {L=5, T=5, R=45, B=25}.
    /// </remarks>
    //*************************************************************************

    Int32
    Margin
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: LayoutStyle
    //
    /// <summary>
    /// Gets or sets the style to use when laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The style to use when laying out the graph.
    /// </value>
    //*************************************************************************

    LayoutStyle
    LayoutStyle
    {
        get;
        set;
    }

    //*************************************************************************
    //  Property: BoxLayoutAlgorithm
    //
    /// <summary>
    /// Gets or sets the box layout algorithm to use when laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The box layout algorithm to use when laying out the graph.
    /// </value>
    //*************************************************************************

    BoxLayoutAlgorithm
    BoxLayoutAlgorithm
    {
        get;
        set;
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

    Boolean
    SupportsOutOfBoundsVertices
    {
        get;
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
    /// than or equal to 0.  If 0, group rectangles aren't drawn.
    /// </value>
    ///
    /// <remarks>
    /// This property is ignored if <see cref="LayoutStyle" /> is not <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseGroups" />.
    /// </remarks>
    //*************************************************************************

    Double
    GroupRectanglePenWidth
    {
        get;
        set;
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

    IntergroupEdgeStyle
    IntergroupEdgeStyle
    {
        get;
        set;
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
    /// true to attempt to improve the appearance of groups.
    /// </value>
    ///
    /// <remarks>
    /// This property is ignored if <see cref="LayoutStyle" /> is not <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseGroups" />.
    /// </remarks>
    //*************************************************************************

    Boolean
    ImproveLayoutOfGroups
    {
        get;
        set;
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

    Boolean
    SupportsBinning
    {
        get;
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
    /// The maximum number of vertices a binned component can have.
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

    Int32
    MaximumVerticesPerBin
    {
        get;
        set;
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
    /// The height and width of each bin, in graph rectangle units.
    /// </value>
    ///
    /// <remarks>
    /// This property is ignored if <see cref="LayoutStyle" /> is not <see
    /// cref="Smrf.NodeXL.Layouts.LayoutStyle.UseBinning" />.
    /// </remarks>
    //*************************************************************************

    Int32
    BinLength
    {
        get;
        set;
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

    Boolean
    IsBusy
    {
        get;
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

    void
    LayOutGraph
    (
        IGraph graph,
        LayoutContext layoutContext
    );

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
    /// This method asynchronously lays out the graph <paramref
    /// name="graph" />.  It returns immediately.  A worker thread sets the
    /// <see cref="IVertex.Location" /> property on all of the graph's
    /// vertices, and optionally adds geometry metadata to the graph, vertices,
    /// or edges.
    ///
    /// <para>
    /// The <see cref="LayOutGraphCompleted" /> event fires when the layout is
    /// complete, an error occurs, or the layout is cancelled.  <see
    /// cref="LayOutGraphAsyncCancel" /> cancels the layout.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    void
    LayOutGraphAsync
    (
        IGraph graph,
        LayoutContext layoutContext
    );

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

    void
    LayOutGraphAsyncCancel();

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
    /// method can be used to transform the graph's layout from one rectangle
    /// to another.  <paramref name="originalLayoutContext" /> contains the
    /// original graph rectangle, and <paramref name="newLayoutContext" />
    /// contains the new graph rectangle.  The implementation should transform
    /// all the graph's vertex locations from the original rectangle to the new
    /// one.  If <see cref="LayOutGraph" /> added geometry metadata to the
    /// graph, the implementation should also transform that metadata.
    /// </remarks>
    //*************************************************************************

    void
    TransformLayout
    (
        IGraph graph,
        LayoutContext originalLayoutContext,
        LayoutContext newLayoutContext
    );

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

    void
    OnVertexMove
    (
        IVertex vertex
    );

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

    event AsyncCompletedEventHandler LayOutGraphCompleted;
}


}
