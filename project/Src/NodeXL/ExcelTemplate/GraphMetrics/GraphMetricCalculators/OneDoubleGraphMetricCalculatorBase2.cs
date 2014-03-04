﻿
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OneDoubleGraphMetricCalculatorBase
//
/// <summary>
/// Calculates one graph metric of type Double for each of the graph's
/// vertices.
/// </summary>
///
/// <remarks>
/// This is the base class for several derived classes that calculate a graph
/// metric of type Double.
/// </remarks>
//*****************************************************************************

public abstract class OneDoubleGraphMetricCalculatorBase :
    GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: OneDoubleGraphMetricCalculatorBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OneDoubleGraphMetricCalculatorBase" /> class.
    /// </summary>
    //*************************************************************************

    public OneDoubleGraphMetricCalculatorBase()
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
    /// <param name="oneDoubleGraphMetricCalculator">
    /// Graph metric calculator in the Algorithms namespace that calculates
    /// one graph metric of type Double.
    /// </param>
    ///
    /// <param name="graphMetric">
    /// The graph metric to calculate.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to write to in the vertex table.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the column, in characters, or <see
    /// cref="Smrf.AppLib.ExcelTableUtil.
    /// AutoColumnWidth" /> to set the width automatically.
    /// </param>
    ///
    /// <param name="style">
    /// Style of the column, or null to apply Excel's normal style.  Sample:
    /// "Bad".
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
    /// This method periodically checks BackgroundWorker.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// immediately returns false.
    ///
    /// <para>
    /// It also periodically reports progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// userState argument is a <see cref="GraphMetricProgress" /> object.
    /// </para>
    ///
    /// <para>
    /// Calculated metrics for hidden rows are ignored by the caller, because
    /// Excel misbehaves when values are written to hidden cells.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,

        Algorithms.OneDoubleGraphMetricCalculatorBase
            oneDoubleGraphMetricCalculator,

        GraphMetrics graphMetric,
        String columnName,
        Double columnWidthChars,
        String style,
        out GraphMetricColumn [] graphMetricColumns
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(calculateGraphMetricsContext != null);
        Debug.Assert(oneDoubleGraphMetricCalculator != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Debug.Assert(columnWidthChars == ExcelTableUtil.AutoColumnWidth ||
            columnWidthChars > 0);

        AssertValid();

        graphMetricColumns = new GraphMetricColumn[0];

        if ( !calculateGraphMetricsContext.ShouldCalculateGraphMetrics(
            graphMetric) )
        {
            return (true);
        }

        // Calculate the graph metrics for each vertex using the
        // OneDoubleGraphMetricCalculatorBase object, which knows nothing about
        // Excel.

        Dictionary<Int32, Double> oGraphMetrics;

        if ( !oneDoubleGraphMetricCalculator.TryCalculateGraphMetrics(graph,
            calculateGraphMetricsContext.BackgroundWorker, out oGraphMetrics) )
        {
            // The user cancelled.

            return (false);
        }

        // Transfer the graph metrics to an array of GraphMetricValue objects.

        List<GraphMetricValueWithID> oGraphMetricValues =
            new List<GraphMetricValueWithID>();

        foreach (IVertex oVertex in graph.Vertices)
        {
            // Try to get the row ID stored in the worksheet.

            Int32 iRowID;

            if ( TryGetRowID(oVertex, out iRowID) )
            {
                oGraphMetricValues.Add( new GraphMetricValueWithID(
                    iRowID, oGraphMetrics[oVertex.ID] ) );
            }
        }

        graphMetricColumns = new GraphMetricColumn [] {
            new GraphMetricColumnWithID( WorksheetNames.Vertices,
                TableNames.Vertices, columnName, columnWidthChars,
                NumericFormat, style, oGraphMetricValues.ToArray()
                ) };

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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Number format for the column.

    protected const String NumericFormat = "0.000";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
