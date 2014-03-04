
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: IntergroupEdgeCalculator2
//
/// <summary>
/// Given a collection of vertex groups, this class counts the number of edges
/// between each pair of groups and the number of edges within each group.
/// </summary>
//*****************************************************************************

public class IntergroupEdgeCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: IntergroupEdgeCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="IntergroupEdgeCalculator2" />
    /// class.
    /// </summary>
    //*************************************************************************

    public IntergroupEdgeCalculator2()
    {
        // (Do nothing.)

        AssertValid();
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

        graphMetricColumns = new GraphMetricColumn[0];

        // Attempt to retrieve the group information the WorkbookReader object
        // may have stored as metadata on the graph.

        GroupInfo [] aoGroups;

        if (
            !calculateGraphMetricsContext.ShouldCalculateGraphMetrics(
                GraphMetrics.GroupMetrics)
            ||
            !GroupUtil.TryGetGroups(graph, out aoGroups)
            )
        {
            return (true);
        }

        // Count the edges using the IntergroupEdgeCalculator class in the
        // Algorithms namespace, which knows nothing about Excel.

        IList<IntergroupEdgeInfo> oIntergroupEdges;

        if ( !(new Algorithms.IntergroupEdgeCalculator() ).
            TryCalculateGraphMetrics(graph,
                calculateGraphMetricsContext.BackgroundWorker, aoGroups, true,
                out oIntergroupEdges) )
        {
            // The user cancelled.

            return (false);
        }

        // Add a row to the group-edge table for each pair of groups that has
        // edges.

        graphMetricColumns = CreateGraphMetricColumns(
            AddRows(aoGroups, oIntergroupEdges) );

        return (true);
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

            // Duplicate edges get counted by this class, so they should be
            // included in the graph.

            return (true);
        }
    }

    //*************************************************************************
    //  Method: AddRows()
    //
    /// <summary>
    /// Adds a row to the group-edge table for each pair of groups that has
    /// edges.
    /// </summary>
    ///
    /// <param name="oGroups">
    /// The graph's groups.
    /// </param>
    ///
    /// <param name="oIntergroupEdges">
    /// There is one IntergroupEdgeInfo object in the collection for each pair
    /// of groups in that have edges between them, and one object for each
    /// group that has edges within it.  Pairs of groups that do not have edges
    /// between them are not included in the collection, nor are groups that do
    /// not have edges within them.
    /// </param>
    ///
    /// <returns>
    /// The row data for the group-edge table.
    /// </returns>
    //*************************************************************************

    protected GroupEdgeRows
    AddRows
    (
        GroupInfo [] oGroups,
        IList<IntergroupEdgeInfo> oIntergroupEdges
    )
    {
        Debug.Assert(oGroups != null);
        Debug.Assert(oIntergroupEdges != null);
        AssertValid();

        GroupEdgeRows oGroupEdgeRows = new GroupEdgeRows();

        foreach (IntergroupEdgeInfo oIntergroupEdge in oIntergroupEdges)
        {
            AddRow(oGroups[oIntergroupEdge.Group1Index].Name,
                oGroups[oIntergroupEdge.Group2Index].Name,
                oIntergroupEdge.Edges, oGroupEdgeRows);
        }

        return (oGroupEdgeRows);
    }

    //*************************************************************************
    //  Method: AddRow()
    //
    /// <summary>
    /// Adds a row to the group-edge table.
    /// </summary>
    ///
    /// <param name="sGroup1Name">
    /// Name of the first group.
    /// </param>
    ///
    /// <param name="sGroup2Name">
    /// Name of the second group.
    /// </param>
    ///
    /// <param name="iEdges">
    /// Number of edges between the groups.
    /// </param>
    ///
    /// <param name="oGroupEdgeRows">
    /// Contains the row data for the group-edge table.
    /// </param>
    //*************************************************************************

    protected void
    AddRow
    (
        String sGroup1Name,
        String sGroup2Name,
        Int32 iEdges,
        GroupEdgeRows oGroupEdgeRows
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sGroup1Name) );
        Debug.Assert( !String.IsNullOrEmpty(sGroup2Name) );
        Debug.Assert(iEdges >= 0);
        Debug.Assert(oGroupEdgeRows != null);
        AssertValid();

        oGroupEdgeRows.Group1Names.Add( new GraphMetricValueOrdered(
            sGroup1Name) );

        oGroupEdgeRows.Group2Names.Add( new GraphMetricValueOrdered(
            sGroup2Name) );

        oGroupEdgeRows.Edges.Add( new GraphMetricValueOrdered(iEdges) );
    }

    //*************************************************************************
    //  Method: CreateGraphMetricColumns()
    //
    /// <summary>
    /// Creates an array of GraphMetricColumnOrdered objects for the columns in
    /// the group edge table.
    /// </summary>
    ///
    /// <param name="oGroupEdgeRows">
    /// Contains the row data for the group-edge table.
    /// </param>
    ///
    /// <returns>
    /// An array of GraphMetricColumnOrdered objects.
    /// </returns>
    //*************************************************************************

    protected GraphMetricColumn[]
    CreateGraphMetricColumns
    (
        GroupEdgeRows oGroupEdgeRows
    )
    {
        Debug.Assert(oGroupEdgeRows != null);
        AssertValid();

        return ( new GraphMetricColumn[] {

            CreateGraphMetricColumnOrdered(
                GroupEdgeTableColumnNames.Group1Name, CellStyleNames.Required,
                oGroupEdgeRows.Group1Names),

            CreateGraphMetricColumnOrdered(
                GroupEdgeTableColumnNames.Group2Name, CellStyleNames.Required,
                oGroupEdgeRows.Group2Names),

            CreateGraphMetricColumnOrdered(
                GroupEdgeTableColumnNames.Edges, CellStyleNames.GraphMetricGood,
                oGroupEdgeRows.Edges),
            } );
    }

    //*************************************************************************
    //  Method: CreateGraphMetricColumnOrdered()
    //
    /// <summary>
    /// Creates a GraphMetricColumnOrdered object for one of the columns in the
    /// group edge table.
    /// </summary>
    ///
    /// <param name="sColumnName">
    /// Name of the column.
    /// </param>
    ///
    /// <param name="sStyle">
    /// Style of the column, or null to not apply a style.  Sample: "Bad".
    /// </param>
    ///
    /// <param name="oValues">
    /// Column cell values.
    /// </param>
    ///
    /// <returns>
    /// A GraphMetricColumnOrdered object for the specified column.
    /// </returns>
    //*************************************************************************

    protected GraphMetricColumnOrdered
    CreateGraphMetricColumnOrdered
    (
        String sColumnName,
        String sStyle,
        List<GraphMetricValueOrdered> oValues
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oValues != null);
        AssertValid();

        return ( new GraphMetricColumnOrdered(WorksheetNames.GroupEdgeMetrics,
            TableNames.GroupEdgeMetrics, sColumnName,
            ExcelTableUtil.AutoColumnWidth, null, sStyle,
            oValues.ToArray() ) );
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
    //  Embedded class: GroupEdgeRows
    //
    /// <summary>
    /// Contains the row data for the group-edge table.
    /// </summary>
    //*************************************************************************

    public class GroupEdgeRows : Object
    {
        //*********************************************************************
        //  Constructor: GroupEdgeRows()
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupEdgeRows" />
        /// class.
        /// </summary>
        //*********************************************************************

        public GroupEdgeRows()
        {
            m_oRowInformation = new List<GraphMetricValueOrdered> [3] {
                new List<GraphMetricValueOrdered>(),
                new List<GraphMetricValueOrdered>(),
                new List<GraphMetricValueOrdered>(),
                };

            AssertValid();
        }

        //*********************************************************************
        //  Property: Group1Names
        //
        /// <summary>
        /// Gets the List of group 1 names.
        /// </summary>
        ///
        /// <value>
        /// The List of group 1 names.
        /// </value>
        //*********************************************************************

        public List<GraphMetricValueOrdered>
        Group1Names
        {
            get
            {
                AssertValid();

                return ( m_oRowInformation[0] );
            }
        }

        //*********************************************************************
        //  Property: Group2Names
        //
        /// <summary>
        /// Gets the List of group 2 names.
        /// </summary>
        ///
        /// <value>
        /// The List of group 2 names.
        /// </value>
        //*********************************************************************

        public List<GraphMetricValueOrdered>
        Group2Names
        {
            get
            {
                AssertValid();

                return ( m_oRowInformation[1] );
            }
        }

        //*********************************************************************
        //  Property: Edges
        //
        /// <summary>
        /// Gets the List of edge counts.
        /// </summary>
        ///
        /// <value>
        /// The List of edge counts.
        /// </value>
        //*********************************************************************

        public List<GraphMetricValueOrdered>
        Edges
        {
            get
            {
                AssertValid();

                return ( m_oRowInformation[2] );
            }
        }


        //*********************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*********************************************************************

        [Conditional("DEBUG")]

        public void
        AssertValid()
        {
            Debug.Assert(m_oRowInformation != null);
            Debug.Assert(m_oRowInformation.Length == 3);
        }


        //*********************************************************************
        //  Protected fields
        //*********************************************************************

        /// 3 Lists, one for each column in the group-edge table.

        protected List<GraphMetricValueOrdered> [] m_oRowInformation;
    }

}

}
