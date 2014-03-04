
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TopItemsCalculatorBase2
//
/// <summary>
/// Base class for several graph metric calculators that calculate the top
/// items in the graph.
/// </summary>
///
/// <remarks>
/// The meaning of "top items" and the worksheet to which they are written in
/// the workbook are determined by the derived class.
/// </remarks>
//*****************************************************************************

public abstract class TopItemsCalculatorBase2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: TopItemsCalculatorBase2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TopItemsCalculatorBase2" /> class.
    /// </summary>
    //*************************************************************************

    public TopItemsCalculatorBase2()
    {
        // (Do nothing.)

        // AssertValid();
    }

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

    public override Boolean
    HandlesDuplicateEdges
    {
        get
        {
            AssertValid();

            // We don't want GraphMetricCalculationManager to remove duplicate
            // edges for this calculator.

            return (true);
        }
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

    public override Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out GraphMetricColumn [] graphMetricColumns
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(calculateGraphMetricsContext != null);
        AssertValid();

        List<GraphMetricColumn> oGraphMetricColumns =
            new List<GraphMetricColumn>();

        Boolean bCancelled = false;

        if ( calculateGraphMetricsContext.ShouldCalculateGraphMetrics(
            this.GraphMetricToCalculate) )
        {
            bCancelled = !TryCalculateGraphMetricsInternal(graph,
                calculateGraphMetricsContext, oGraphMetricColumns);
        }

        graphMetricColumns = oGraphMetricColumns.ToArray();

        return (!bCancelled);
    }

    //*************************************************************************
    //  Property: GraphMetricToCalculate
    //
    /// <summary>
    /// Gets the graph metric that will be calculated.
    /// </summary>
    ///
    /// <value>
    /// The graph metric that will be calculated, as a <see
    /// cref="GraphMetrics" /> flag.
    /// </value>
    //*************************************************************************

    protected abstract GraphMetrics
    GraphMetricToCalculate
    {
        get;
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetricsInternal()
    //
    /// <summary>
    /// Attempts to calculate metrics for the entire List of
    /// TopNByMetricUserSettings objects.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that gets populated by this
    /// method if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected abstract Boolean
    TryCalculateGraphMetricsInternal
    (
        IGraph oGraph,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricColumn> oGraphMetricColumns
    );

    //*************************************************************************
    //  Method: AddColumnsForRankedVertices()
    //
    /// <summary>
    /// Adds a pair of GraphMetricColumn objects that rank vertices based on
    /// the numbers in a specified column.
    /// </summary>
    ///
    /// <param name="oVertices">
    /// The vertices to rank.
    /// </param>
    ///
    /// <param name="sRankedColumnName">
    /// The name of the vertex column that contains numbers to rank the
    /// vertices by.
    /// </param>
    ///
    /// <param name="iMaximumVertices">
    /// The maximum number of ranked vertices to get.
    /// </param>
    ///
    /// <param name="sGraphMetricWorksheetName">
    /// The name of the worksheet where the graph metric columns will
    /// eventually be written.
    /// </param>
    ///
    /// <param name="sGraphMetricTableName">
    /// The name of the table where the graph metric columns will eventually be
    /// written.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn1Name">
    /// The name of the first graph metric column.  This is where the vertex
    /// names will eventually be written.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn2Name">
    /// The name of the second graph metric column.  This is where the numbers
    /// from <paramref name="sRankedColumnName" /> will eventually be written.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds a pair
    /// of columns to.
    /// </param>
    //*************************************************************************

    protected void
    AddColumnsForRankedVertices
    (
        IEnumerable<IVertex> oVertices,
        String sRankedColumnName,
        Int32 iMaximumVertices,
        String sGraphMetricWorksheetName,
        String sGraphMetricTableName,
        String sGraphMetricColumn1Name,
        String sGraphMetricColumn2Name,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(oVertices != null);
        Debug.Assert( !String.IsNullOrEmpty(sRankedColumnName) );
        Debug.Assert(iMaximumVertices >= 1);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMetricWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMetricTableName) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMetricColumn1Name) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMetricColumn2Name) );
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        // Get a collection of information about the specified column on the
        // vertex worksheet, sorted by descending RankedValue.

        IList<ItemInformation> oItems = RankVertices(
            oVertices, sRankedColumnName);

        // Add an ordered column containing the top vertex names, and a
        // corresponding ordered column containing the ranked values.

        iMaximumVertices = Math.Min(iMaximumVertices, oItems.Count);

        GraphMetricValueOrdered[] aoVertexNames =
            new GraphMetricValueOrdered[iMaximumVertices];

        GraphMetricValueOrdered[] aoRankedValues =
            new GraphMetricValueOrdered[iMaximumVertices];

        for (Int32 i = 0; i < iMaximumVertices; i++)
        {
            ItemInformation oItem = oItems[i];

            aoVertexNames[i] = new GraphMetricValueOrdered(oItem.Name);

            aoRankedValues[i] =
                new GraphMetricValueOrdered(oItem.RankedValue);
        }

        oGraphMetricColumns.Add( new GraphMetricColumnOrdered(
            sGraphMetricWorksheetName,
            sGraphMetricTableName,
            sGraphMetricColumn1Name,
            ExcelTableUtil.AutoColumnWidth,
            null,
            null,
            aoVertexNames
            ) );

        oGraphMetricColumns.Add( new GraphMetricColumnOrdered(
            sGraphMetricWorksheetName,
            sGraphMetricTableName,
            sGraphMetricColumn2Name,
            ExcelTableUtil.AutoColumnWidth,
            null,
            null,
            aoRankedValues
            ) );
    }

    //*************************************************************************
    //  Method: RankVertices()
    //
    /// <summary>
    /// Ranks vertices based on the numbers in a specified column.
    /// </summary>
    ///
    /// <param name="oVertices">
    /// The vertices to rank.
    /// </param>
    ///
    /// <param name="sRankedColumnName">
    /// The name of the vertex column that contains numbers to rank the
    /// vertices by.
    /// </param>
    ///
    /// <returns>
    /// A new collection of ItemInformation objects, with one object for each
    /// vertex.  The objects are ordered by the numbers in <paramref
    /// name="sRankedColumnName" />.
    /// </returns>
    //*************************************************************************

    protected IList<ItemInformation>
    RankVertices
    (
        IEnumerable<IVertex> oVertices,
        String sRankedColumnName
    )
    {
        Debug.Assert(oVertices != null);
        Debug.Assert( !String.IsNullOrEmpty(sRankedColumnName) );
        AssertValid();

        List<ItemInformation> oItems = new List<ItemInformation>();

        // The vertex cell values are stored as metadata on the graph's
        // vertices.  Read the metadata.

        foreach (IVertex oVertex in oVertices)
        {
            String sItemName = oVertex.Name;
            Object oRankedValue;
            Double dRankedValue;

            if (
                !String.IsNullOrEmpty(sItemName)
                &&
                oVertex.TryGetValue(sRankedColumnName, typeof(String),
                    out oRankedValue)
                &&
                Double.TryParse( (String)oRankedValue, out dRankedValue )
                )
            {
                oItems.Add( new ItemInformation(sItemName, dRankedValue) );
            }
        }

        oItems.Sort( (a, b) => -a.RankedValue.CompareTo(b.RankedValue) );

        return (oItems);
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
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded struct: ItemInformation
    //
    /// <summary>
    /// Stores calculated information about one item.
    /// </summary>
    //*************************************************************************

    protected struct
    ItemInformation
    {
        /// <summary>
        /// Constructor.
        /// </summary>

        public ItemInformation(String name, Double rankedValue)
        {
            Name = name;
            RankedValue = rankedValue;
        }

        /// <summary>
        /// The item's name.
        /// </summary>

        public String Name;

        /// <summary>
        /// The value to rank the item by.
        /// </summary>

        public Double RankedValue;
    }
}

}
