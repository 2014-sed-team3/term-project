
using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Class: GridLayout
//
/// <summary>
/// Lays out a graph by placing the vertices on a grid.
/// </summary>
///
/// <remarks>
/// This layout places a graph's vertices on a simple grid.
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

public class GridLayout : SortableLayoutBase
{
    //*************************************************************************
    //  Constructor: GridLayout()
    //
    /// <summary>
    /// Initializes a new instance of the GridLayout class.
    /// </summary>
    //*************************************************************************

    public GridLayout()
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

        RectangleF oRectangleF = layoutContext.GraphRectangle;

        Debug.Assert(oRectangleF.Width > 0 && oRectangleF.Height > 0);

        // Get the number of rows and columns to use in the grid.

        Int32 iRows, iColumns;

        GetRowsAndColumns(verticesToLayOut, layoutContext,
            out iRows, out iColumns);

        Debug.Assert(iRows > 0);
        Debug.Assert(iColumns > 0);
        Debug.Assert(iColumns * iRows >= verticesToLayOut.Count);

        // Get the distances between vertices;

        Double dRowSpacing    = (Double)oRectangleF.Height / (Double)iRows;
        Double dColumnSpacing = (Double)oRectangleF.Width  / (Double)iColumns;

        // Set the location on each vertex.  The vertices are placed at the
        // centers of the grid boxes.

        Double dXStart = oRectangleF.Left + (dColumnSpacing / 2.0);

        Double dX = dXStart;
        Double dY = oRectangleF.Top + (dRowSpacing / 2.0);

        Int32 iColumn = 0;

        foreach (IVertex oVertex in verticesToLayOut)
        {
            if ( !VertexIsLocked(oVertex) )
            {
                oVertex.Location = new PointF( (Single)dX,  (Single)dY );
            }

            iColumn++;

            if (iColumn >= iColumns)
            {
                dX = dXStart;
                dY += dRowSpacing;

                iColumn = 0;
            }
            else
            {
                dX += dColumnSpacing;
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetRowsAndColumns()
    //
    /// <summary>
    /// Gets the number of rows and columns to use in the grid.
    /// </summary>
    ///
    /// <param name="oVerticesToLayOut">
    /// Vertices to lay out.
    /// </param>
    ///
    /// <param name="oLayoutContext">
    /// Provides access to objects needed to lay out the graph.
    /// </param>
    ///
    /// <param name="iRows">
    /// Where the number of grid rows gets stored.
    /// </param>
    ///
    /// <param name="iColumns">
    /// Where the number of grid columns gets stored.
    /// </param>
    //*************************************************************************

    protected void
    GetRowsAndColumns
    (
        ICollection<IVertex> oVerticesToLayOut,
        LayoutContext oLayoutContext,
        out Int32 iRows,
        out Int32 iColumns
    )
    {
        Debug.Assert(oVerticesToLayOut != null);
        Debug.Assert(oLayoutContext != null);
        AssertValid();

        #if false

        Some definitions:

            W = rectangle width

            H = rectangle height

            A = rectangle aspect ratio = W / H

            V = number of vertices in graph

            R = number of grid rows

            C = number of grid columns


        First simulataneous equation, allowing R and C to be fractional for
        now:

            R * C = V


        Second simulataneous equation:

            C / R = A


        Combining these equations yields:

                       1/2
            C = (V * A)


        #endif

        Int32 V = oVerticesToLayOut.Count;

        // Compute the aspect ratio.

        RectangleF oRectangleF = oLayoutContext.GraphRectangle;
        Debug.Assert(oRectangleF.Height != 0);
        Double A = oRectangleF.Width / oRectangleF.Height;

        Double C = Math.Sqrt(V * A);
        Debug.Assert(A != 0);
        Double R = C / A;

        // Try the floor/ceiling combinations.

        // C floor, R floor

        iColumns = (Int32)Math.Floor(C);
        iRows = (Int32)Math.Floor(R);

        if ( RowsAndColumnsAreSufficient(iRows, iColumns, V) )
        {
            return;
        }

        // C floor, R ceiling

        iRows++;

        if ( RowsAndColumnsAreSufficient(iRows, iColumns, V) )
        {
            return;
        }

        // C ceiling, R floor

        iColumns = (Int32)Math.Ceiling(C);
        iRows = (Int32)Math.Floor(R);

        if ( RowsAndColumnsAreSufficient(iRows, iColumns, V) )
        {
            return;
        }

        // C ceiling, R ceiling

        iRows++;

        Debug.Assert( RowsAndColumnsAreSufficient(iRows, iColumns, V) );
    }

    //*************************************************************************
    //  Method: RowsAndColumnsAreSufficient()
    //
    /// <summary>
    /// Determines whether a calculated number of rows and columns are
    /// sufficient to display all the vertices.
    /// </summary>
    ///
    /// <param name="iRows">
    /// Calculated number of rows.
    /// </param>
    ///
    /// <param name="iColumns">
    /// Calculated number of columns.
    /// </param>
    ///
    /// <param name="iVertices">
    /// Number of vertices.
    /// </param>
    ///
    /// <returns>
    /// true if the rows and columns are sufficient.
    /// </returns>
    //*************************************************************************

    protected Boolean
    RowsAndColumnsAreSufficient
    (
        Int32 iRows,
        Int32 iColumns,
        Int32 iVertices
    )
    {
        Debug.Assert(iRows >= 0);
        Debug.Assert(iColumns >= 0);
        Debug.Assert(iVertices >= 0);
        AssertValid();

        return (iColumns * iRows >= iVertices);
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
