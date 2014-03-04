﻿
using System;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricCalculatorBase2
//
/// <summary>
/// Base class for classes that implement <see
/// cref="IGraphMetricCalculator2" />.
/// </summary>
//*****************************************************************************

public abstract class GraphMetricCalculatorBase2 :
    Object, IGraphMetricCalculator2
{
    //*************************************************************************
    //  Constructor: GraphMetricCalculatorBase2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricCalculatorBase2" /> class.
    /// </summary>
    //*************************************************************************

    public GraphMetricCalculatorBase2()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="calculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="graphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated or don't need to be
    /// calculated, false if the user wants to cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method should periodically check BackgroundWorker.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// should immediately return false.
    ///
    /// <para>
    /// It should also periodically report progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// userState argument must be a <see cref="GraphMetricProgress" /> object.
    /// </para>
    ///
    /// <para>
    /// Calculated metrics for hidden rows are ignored by the caller, because
    /// Excel misbehaves when values are written to hidden cells.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public abstract Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out GraphMetricColumn [] graphMetricColumns
    );

    //*************************************************************************
    //  Property: HandlesDuplicateEdges
    //
    /// <summary>
    /// Gets a flag indicating whether duplicate edges are properly handled.
    /// </summary>
    ///
    /// <value>
    /// true if the graph metric calculator handles duplicate edges, false if
    /// duplicate edges should be removed from the graph before the
    /// calculator's <see cref="TryCalculateGraphMetrics" /> method is called.
    /// </value>
    //*************************************************************************

    public virtual Boolean
    HandlesDuplicateEdges
    {
        get
        {
            AssertValid();

            // Most graph metric calculators can't handle duplicate edges.

            return (false);
        }
    }

    //*************************************************************************
    //  Method: NullableToGraphMetricValue()
    //
    /// <summary>
    /// Converts a Nullable to a graph metric value suitable for inserting into
    /// a cell.
    /// </summary>
    ///
    /// <param name="nullable">
    /// The Nullable to convert.
    /// </param>
    ///
    /// <returns>
    /// If the Nullable has a value, the value is returned.  Otherwise, a "not
    /// applicable" string is returned.
    /// </returns>
    //*************************************************************************

    public static Object
    NullableToGraphMetricValue<T>
    (
        Nullable<T> nullable
    )
    where T : struct
    {
        if (nullable.HasValue)
        {
            return (nullable.Value);
        }

        return (GraphMetricCalculatorBase.NotApplicableMessage);
    }

    //*************************************************************************
    //  Method: TryGetRowID()
    //
    /// <summary>
    /// Attempts to get the worksheet row ID for an edge or vertex.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex to get the ID for.
    /// </param>
    ///
    /// <param name="iRowID">
    /// Where the row ID gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// The returned ID is the worksheet row ID, not the ID stored in the edge
    /// or vertex's ID property.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetRowID
    (
        IMetadataProvider oEdgeOrVertex,
        out Int32 iRowID
    )
    {
        Debug.Assert(oEdgeOrVertex != null);

        iRowID = Int32.MinValue;

        // The worksheet row ID is stored in the edge or vertex's Tag, as an
        // Int32.

        if ( oEdgeOrVertex.Tag == null || !(oEdgeOrVertex.Tag is Int32) )
        {
            return (false);
        }

        iRowID = (Int32)oEdgeOrVertex.Tag;

        return (true);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
