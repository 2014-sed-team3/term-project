﻿
using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Class: CircleLayout
//
/// <summary>
/// Lays out a graph by placing the vertices on a circle.
/// </summary>
///
/// <remarks>
/// This layout places a graph's vertices on a circle whose diameter is the
/// smaller of the rectangle's dimensions.
///
/// <para>
/// If the graph has a metadata key of <see
/// cref="ReservedMetadataKeys.LayOutTheseVerticesOnly" />, only the vertices
/// specified in the value's IVertex collection are laid out and all other
/// vertices are completely ignored.
/// </para>
///
/// <para>
/// If a vertex has a metadata key of <see
/// cref="ReservedMetadataKeys.LockVertexLocation" /> with a value of true, it
/// is included in layout calculations but its own location is left unmodified.
/// </para>
///
/// <para>
/// If you want the vertices to be placed in a certain order, set the <see
/// cref="SortableLayoutBase.VertexSorter" /> property to an object that will
/// sort them.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class CircleLayout : SortableLayoutBase
{
    //*************************************************************************
    //  Constructor: CircleLayout()
    //
    /// <summary>
    /// Initializes a new instance of the CircleLayout class.
    /// </summary>
    //*************************************************************************

    public CircleLayout()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: LayOutGraphCoreSorted()
    //
    /// <summary>
    /// Lays out a graph synchronously or asynchronously using specified
    /// vertices that may be sorted.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to lay out.
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

    protected override Boolean
    LayOutGraphCoreSorted
    (
        IGraph graph,
        ICollection<IVertex> verticesToLayOut,
        LayoutContext layoutContext,
        BackgroundWorker backgroundWorker
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(verticesToLayOut != null);
        Debug.Assert(verticesToLayOut.Count > 0);
        Debug.Assert(layoutContext != null);
        AssertValid();

        if (backgroundWorker != null && backgroundWorker.CancellationPending)
        {
            return (false);
        }

        Int32 iVertices = verticesToLayOut.Count;

        // The vertices are placed at equal angles around the circle's
        // circumference.

        Double dAngleBetweenVerticesRadians =
            (2 * Math.PI) / (Double)iVertices;

        Double dCenterX, dCenterY, dRadius;

        GetRectangleCenterAndHalfSize(layoutContext.GraphRectangle,
            out dCenterX, out dCenterY, out dRadius);

        Double dAngleRadians = 0;

        foreach (IVertex oVertex in verticesToLayOut)
        {
            if ( !VertexIsLocked(oVertex) )
            {
                Double dX = dCenterX + dRadius * Math.Cos(dAngleRadians);
                Double dY = dCenterY + dRadius * Math.Sin(dAngleRadians);

                oVertex.Location = new PointF( (Single)dX, (Single)dY );
            }

            dAngleRadians += dAngleBetweenVerticesRadians;
        }

        return (true);
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

        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
