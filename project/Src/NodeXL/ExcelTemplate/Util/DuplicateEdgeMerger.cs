
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DuplicateEdgeMerger
//
/// <summary>
/// Merges duplicate edges in the edge worksheet.
/// </summary>
///
/// <remarks>
/// Use <see cref="MergeDuplicateEdges(Workbook)" /> to merge duplicate edges
/// in the edge worksheet.
///
/// <para>
/// "Merge" means either count duplicates and include the counts in a new edge
/// weight column, or delete all but one edge in each set of duplicate edges,
/// or both.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DuplicateEdgeMerger : Object
{
    //*************************************************************************
    //  Constructor: DuplicateEdgeMerger()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEdgeMerger" />
    /// class.
    /// </summary>
    //*************************************************************************

    public DuplicateEdgeMerger()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: MergeDuplicateEdges()
    //
    /// <overloads>
    /// Merges duplicate edges in the edge worksheet.
    /// </overloads>
    ///
    /// <summary>
    /// Counts and deletes duplicate edges in the edge worksheet.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the edge worksheet.
    /// </param>
    ///
    /// <remarks>
    /// This method adds an edge weight column to the edge worksheet, then
    /// searches for rows that represent the same edge.  For each set of
    /// duplicate rows, all but the first row in the set are deleted, and the
    /// edge weight cell for the first row is set to the number of duplicate
    /// edges.
    ///
    /// <para>
    /// Any AutoFiltering on the edge table is cleared.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    MergeDuplicateEdges
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);
        AssertValid();

        MergeDuplicateEdges(workbook, true, true, null);
    }

    //*************************************************************************
    //  Method: MergeDuplicateEdges()
    //
    /// <summary>
    /// Merges duplicate edges in the edge worksheet, with options for counting
    /// and deleting the duplicates.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the edge worksheet.
    /// </param>
    ///
    /// <param name="countDuplicateEdges">
    /// true to count duplicates and include the counts in a new edge weight
    /// column.
    /// </param>
    ///
    /// <param name="deleteDuplicateEdges">
    /// true to delete all but one edge in each set of duplicate edges.
    /// </param>
    ///
    /// <param name="thirdColumnNameForDuplicateDetection">
    /// If this is null or empty, only the edges' vertices are used to
    /// determine whether the edges are duplicates.  If a column name is
    /// specified, the edges' vertices and the values in the specified column
    /// are used to determine whether the edges are duplicates.
    /// </param>
    ///
    /// <remarks>
    /// Any AutoFiltering on the edge table is cleared.
    /// </remarks>
    //*************************************************************************

    public void
    MergeDuplicateEdges
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        Boolean countDuplicateEdges,
        Boolean deleteDuplicateEdges,
        String thirdColumnNameForDuplicateDetection
    )
    {
        Debug.Assert(workbook != null);
        AssertValid();

        ListObject oEdgeTable;

        if ( TryGetEdgeTable(workbook, out oEdgeTable) )
        {
            ExcelUtil.ActivateWorksheet(oEdgeTable);

            // Clear AutoFiltering, which would make this code much more
            // complicated.

            ExcelTableUtil.ClearTableAutoFilters(oEdgeTable);

            Boolean bGraphIsDirected;
            Range oVertex1NameData, oVertex2NameData;

            Object [,] aoVertex1NameValues, aoVertex2NameValues,
                aoThirdColumnValues;

            if ( TryGetInformationFromEdgeTable(workbook, oEdgeTable,
                thirdColumnNameForDuplicateDetection,
                out bGraphIsDirected,
                out oVertex1NameData, out aoVertex1NameValues,
                out oVertex2NameData, out aoVertex2NameValues,
                out aoThirdColumnValues
                ) )
            {
                if (countDuplicateEdges)
                {
                    CountDuplicateEdges(oEdgeTable, aoVertex1NameValues,
                        aoVertex2NameValues, aoThirdColumnValues,
                        bGraphIsDirected);
                }

                if (deleteDuplicateEdges)
                {
                    DeleteDuplicateEdges(oEdgeTable, aoVertex1NameValues,
                        aoVertex2NameValues, aoThirdColumnValues,
                        bGraphIsDirected);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetEdgeTable()
    //
    /// <summary>
    /// Attempts to get the edge table from the workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the edge worksheet.
    /// </param>
    ///
    /// <param name="oEdgeTable">
    /// Where the edge table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the edge table was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetEdgeTable
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        out ListObject oEdgeTable
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        // The DataBodyRange test catches the odd case where the user deletes
        // the first data row of the table.  It looks like the row is still
        // there, but it's not.  Continuing with a null DataBodyRange can cause
        // a variety of problems.

        return (
            ExcelTableUtil.TryGetTable(oWorkbook, WorksheetNames.Edges,
                TableNames.Edges, out oEdgeTable)
            &&
            oEdgeTable.DataBodyRange != null
            );
    }

    //*************************************************************************
    //  Method: TryGetInformationFromEdgeTable()
    //
    /// <summary>
    /// Attempts to get the information from the edge table that is needed to
    /// merge duplicate edges.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the edge worksheet.
    /// </param>
    ///
    /// <param name="oEdgeTable">
    /// The workbook's edge table.
    /// </param>
    ///
    /// <param name="sThirdColumnName">
    /// Name of the third column used for duplicate detection, or null or
    /// empty.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// Where the graph's directedness gets stored if true is returned.
    /// </param>
    ///
    /// <param name="oVertex1NameData">
    /// Where the vertex 1 column data range gets stored if true is returned.
    /// </param>
    ///
    /// <param name="aoVertex1NameValues">
    /// Where the vertex 1 values get stored if true is returned.
    /// </param>
    ///
    /// <param name="oVertex2NameData">
    /// Where the vertex 2 column data range gets stored if true is returned.
    /// </param>
    ///
    /// <param name="aoVertex2NameValues">
    /// Where the vertex 2 values get stored if true is returned.
    /// </param>
    ///
    /// <param name="aoThirdColumnValues">
    /// Where the third column values get stored if <paramref
    /// name="sThirdColumnName" /> is specified, the column is found, and true
    /// is returned.  It is not an error if the column is specified by isn't
    /// found.  In that case, this parameter gets set to null.
    /// </param>
    ///
    /// <returns>
    /// true if the information was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetInformationFromEdgeTable
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ListObject oEdgeTable,
        String sThirdColumnName,
        out Boolean bGraphIsDirected,
        out Range oVertex1NameData,
        out Object [,] aoVertex1NameValues,
        out Range oVertex2NameData,
        out Object [,] aoVertex2NameValues,
        out Object [,] aoThirdColumnValues
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oEdgeTable != null);
        AssertValid();

        oVertex1NameData = oVertex2NameData = null;
        aoVertex1NameValues = aoVertex2NameValues = aoThirdColumnValues = null;

        bGraphIsDirected =
            (new PerWorkbookSettings(oWorkbook).GraphDirectedness ==
            GraphDirectedness.Directed);

        Range oThirdColumnData;

        if (
            !String.IsNullOrEmpty(sThirdColumnName)
            &&
            !ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
                sThirdColumnName, out oThirdColumnData,
                out aoThirdColumnValues)
            )
        {
            // This is not an error.

            aoThirdColumnValues = null;
        }

        return (
            ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
                EdgeTableColumnNames.Vertex1Name, out oVertex1NameData,
                out aoVertex1NameValues)
            &&
            ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
                EdgeTableColumnNames.Vertex2Name, out oVertex2NameData,
                out aoVertex2NameValues)
            );
    }

    //*************************************************************************
    //  Method: CountDuplicateEdges()
    //
    /// <summary>
    /// Counts the duplicate edges and stores the counts in an edge weight
    /// column.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// Edge table.
    /// </param>
    ///
    /// <param name="aoVertex1NameValues">
    /// The vertex 1 values.
    /// </param>
    ///
    /// <param name="aoVertex2NameValues">
    /// The vertex 2 values.
    /// </param>
    ///
    /// <param name="aoThirdColumnValues">
    /// The third column values, or null if there is no third column.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed, false if it is undirected.
    /// </param>
    ///
    /// <remarks>
    /// This method adds an edge weight column to the edge table if it doesn't
    /// already exist, then fills in the column.
    ///
    /// <para>
    /// Any existing edge weight values are used when the column is filled in.
    /// For example, if there is already an edge weight column, edge A and
    /// edge B are duplicates, edge A has an edge weight of 5 and edge B has an
    /// edge weight of 2, then this method will set the edge weight of both
    /// edges to 7 (5 + 2).
    /// </para>
    ///
    /// <para>
    /// If, on the other hand, there is not already an edge weight column, or
    /// it's empty, then this method will set the edge weight of both edges to
    /// 2 (1 + 1).
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    CountDuplicateEdges
    (
        ListObject oEdgeTable,
        Object [,] aoVertex1NameValues,
        Object [,] aoVertex2NameValues,
        Object [,] aoThirdColumnValues,
        Boolean bGraphIsDirected
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(aoVertex1NameValues != null);
        Debug.Assert(aoVertex2NameValues != null);
        AssertValid();

        ListColumn oEdgeWeightColumn;
        Range oEdgeWeightData;
        Object [,] aoEdgeWeightValues;

        if ( !ExcelTableUtil.TryGetOrAddTableColumn(oEdgeTable,
            EdgeTableColumnNames.EdgeWeight, 13.7F,
            null, out oEdgeWeightColumn, out oEdgeWeightData,
            out aoEdgeWeightValues) )
        {
            throw new InvalidOperationException(
                "Can't add edge weight column.");
        }

        // The key identifies a unique edge and the value is the sum of the
        // edge weight values for all duplicate edges with the key.

        Dictionary<String, Double> oEdgeWeightSums =
            new Dictionary<String, Double>();

        Int32 iRows = GetRowCount(aoVertex1NameValues);

        // Populate the dictionary.

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            String sEdgeKey;

            if ( TryGetEdgeKey(iRowOneBased, aoVertex1NameValues,
                aoVertex2NameValues, aoThirdColumnValues, bGraphIsDirected,
                out sEdgeKey) )
            {
                // Does the row already have an edge weight in the edge weight
                // column?

                Double dEdgeWeightForRow;

                if ( !ExcelUtil.TryGetDoubleFromCell(aoEdgeWeightValues,
                    iRowOneBased, 1, out dEdgeWeightForRow) )
                {
                    // No.

                    dEdgeWeightForRow = 1;
                }

                // Has a row with the same key already been found?

                Double dEdgeWeightSum;

                if ( !oEdgeWeightSums.TryGetValue(sEdgeKey,
                    out dEdgeWeightSum) )
                {
                    // No.

                    dEdgeWeightSum = 0;
                }

                oEdgeWeightSums[sEdgeKey] = dEdgeWeightForRow + dEdgeWeightSum;
            }
        }

        // Now fill in the edge weight cells with the dictionary values.

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            String sEdgeKey;

            if ( TryGetEdgeKey(iRowOneBased, aoVertex1NameValues,
                aoVertex2NameValues, aoThirdColumnValues, bGraphIsDirected,
                out sEdgeKey) )
            {
                aoEdgeWeightValues[iRowOneBased, 1] =
                    oEdgeWeightSums[sEdgeKey];
            }
        }

        oEdgeWeightData.set_Value(Missing.Value, aoEdgeWeightValues);
    }

    //*************************************************************************
    //  Method: DeleteDuplicateEdges()
    //
    /// <summary>
    /// Deletes the duplicate edges.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// The workbook's edge table.
    /// </param>
    ///
    /// <param name="aoVertex1NameValues">
    /// The vertex 1 values.
    /// </param>
    ///
    /// <param name="aoVertex2NameValues">
    /// The vertex 2 values.
    /// </param>
    ///
    /// <param name="aoThirdColumnValues">
    /// The third column values, or null if there is no third column.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed, false if it is undirected.
    /// </param>
    //*************************************************************************

    protected void
    DeleteDuplicateEdges
    (
        ListObject oEdgeTable,
        Object [,] aoVertex1NameValues,
        Object [,] aoVertex2NameValues,
        Object [,] aoThirdColumnValues,
        Boolean bGraphIsDirected
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(aoVertex1NameValues != null);
        Debug.Assert(aoVertex2NameValues != null);
        AssertValid();

        // Deleting rows one by one is way too slow.  Instead, start by adding
        // a temporary column that has an empty cell for each row that should
        // be deleted.

        ListColumn oDeleteIfEmptyColumn;
        Range oDeleteIfEmptyData;
        Object [,] aoDeleteIfEmptyValues;

        MarkRowsForDeletion(oEdgeTable, aoVertex1NameValues,
            aoVertex2NameValues, aoThirdColumnValues, bGraphIsDirected,
            out oDeleteIfEmptyColumn, out oDeleteIfEmptyData,
            out aoDeleteIfEmptyValues);

        // The edge table will be sorted on the "delete if empty" column so
        // that all edges that need to be deleted are contiguous.  But first,
        // add another temporary column and set its values to the worksheet
        // row numbers.  This will be used later to restore the original sort
        // order.

        ListColumn oOriginalSortOrderColumn;

        if (!ExcelTableUtil.TryAddTableColumnWithRowNumbers(oEdgeTable,
            OriginalSortOrderColumnName, ExcelTableUtil.AutoColumnWidth, null,
            out oOriginalSortOrderColumn) )
        {
            throw new InvalidOperationException(
                "Can't add temporary sort column.");
        }

        SortEdgeTable(oEdgeTable, oDeleteIfEmptyData);

        DeleteMarkedRows(oEdgeTable, oDeleteIfEmptyData,
            aoDeleteIfEmptyValues);

        // Restore the original sort order.

        SortEdgeTable(oEdgeTable, oOriginalSortOrderColumn.Range);

        oOriginalSortOrderColumn.Delete();
        oDeleteIfEmptyColumn.Delete();

        // The (deleted) original sort order column is now selected.  Select
        // something more sensible.

        oEdgeTable.HeaderRowRange.Select();
    }

    //*************************************************************************
    //  Method: TryGetEdgeKey()
    //
    /// <summary>
    /// Attempts to get an edge key for an edge row.
    /// </summary>
    ///
    /// <param name="iRowOneBased">
    /// One-based row number.
    /// </param>
    ///
    /// <param name="aoVertex1NameValues">
    /// The vertex 1 values.
    /// </param>
    ///
    /// <param name="aoVertex2NameValues">
    /// The vertex 2 values.
    /// </param>
    ///
    /// <param name="aoThirdColumnValues">
    /// The third column values, or null if there is no third column.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed, false if it is undirected.
    /// </param>
    ///
    /// <param name="sEdgeKey">
    /// Where the edge key gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if the row should be skipped.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetEdgeKey
    (
        Int32 iRowOneBased,
        Object [,] aoVertex1NameValues,
        Object [,] aoVertex2NameValues,
        Object [,] aoThirdColumnValues,
        Boolean bGraphIsDirected,
        out String sEdgeKey
    )
    {
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(aoVertex1NameValues != null);
        Debug.Assert(aoVertex2NameValues != null);
        AssertValid();

        sEdgeKey = null;

        String sVertex1Name, sVertex2Name;

        if (
            !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertex1NameValues,
                iRowOneBased, 1, out sVertex1Name)
            ||
            !ExcelUtil.TryGetNonEmptyStringFromCell(aoVertex2NameValues,
                iRowOneBased, 1, out sVertex2Name)
            )
        {
            return (false);
        }

        sEdgeKey = EdgeUtil.GetVertexNamePair(sVertex1Name, sVertex2Name,
            bGraphIsDirected);

        String sThirdColumnValue;

        if (
            aoThirdColumnValues != null
            &&
            ExcelUtil.TryGetNonEmptyStringFromCell(aoThirdColumnValues,
                iRowOneBased, 1, out sThirdColumnValue)
            )
        {
            sEdgeKey += EdgeUtil.VertexNamePairSeparator;
            sEdgeKey += sThirdColumnValue;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: MarkRowsForDeletion()
    //
    /// <summary>
    /// Marks duplicate edge rows that should be deleted.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// The workbook's edge table.
    /// </param>
    ///
    /// <param name="aoVertex1NameValues">
    /// The vertex 1 values.
    /// </param>
    ///
    /// <param name="aoVertex2NameValues">
    /// The vertex 2 values.
    /// </param>
    ///
    /// <param name="aoThirdColumnValues">
    /// The third column values, or null if there is no third column.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed, false if it is undirected.
    /// </param>
    ///
    /// <param name="oDeleteIfEmptyColumn">
    /// Where the new <see cref="DeleteIfEmptyColumnName" /> column gets
    /// stored.
    /// </param>
    ///
    /// <param name="oDeleteIfEmptyData">
    /// Where the data range for the new <see
    /// cref="DeleteIfEmptyColumnName" /> column gets stored.
    /// </param>
    ///
    /// <param name="aoDeleteIfEmptyValues">
    /// Where the data values for the new <see
    /// cref="DeleteIfEmptyColumnName" /> column gets stored.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a <see cref="DeleteIfEmptyColumnName" /> column to the
    /// edge worksheet.  For each duplicate edge that is not the first
    /// instance of the edge, it leaves the edge's cell in this column empty.
    /// All other cells are set to 1.
    /// </remarks>
    //*************************************************************************

    protected void
    MarkRowsForDeletion
    (
        ListObject oEdgeTable,
        Object [,] aoVertex1NameValues,
        Object [,] aoVertex2NameValues,
        Object [,] aoThirdColumnValues,
        Boolean bGraphIsDirected,
        out ListColumn oDeleteIfEmptyColumn,
        out Range oDeleteIfEmptyData,
        out Object [,] aoDeleteIfEmptyValues
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(aoVertex1NameValues != null);
        Debug.Assert(aoVertex2NameValues != null);
        AssertValid();

        HashSet<String> oUniqueEdgeKeys = new HashSet<String>();

        if ( !ExcelTableUtil.TryGetOrAddTableColumn(oEdgeTable,
            DeleteIfEmptyColumnName, ExcelTableUtil.AutoColumnWidth, null,
            out oDeleteIfEmptyColumn, out oDeleteIfEmptyData,
            out aoDeleteIfEmptyValues) )
        {
            throw new InvalidOperationException(
                "Can't add marked for deletion column.");
        }

        Int32 iRows = GetRowCount(aoVertex1NameValues);

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            String sEdgeKey;
            Object oDeleteIfEmpty = 1;

            if (
                TryGetEdgeKey(iRowOneBased, aoVertex1NameValues,
                    aoVertex2NameValues, aoThirdColumnValues, bGraphIsDirected,
                    out sEdgeKey)
                &&
                !oUniqueEdgeKeys.Add(sEdgeKey)
                )
            {
                // This is a duplicate that is not the first instance.  It
                // should be deleted.

                oDeleteIfEmpty = null;
            }

            aoDeleteIfEmptyValues[iRowOneBased, 1] = oDeleteIfEmpty;
        }

        oDeleteIfEmptyData.set_Value(Missing.Value, aoDeleteIfEmptyValues);
    }

    //*************************************************************************
    //  Method: DeleteMarkedRows()
    //
    /// <summary>
    /// Deletes the edge rows that have been marked for deletion.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// The workbook's edge table.
    /// </param>
    ///
    /// <param name="oDeleteIfEmptyData">
    /// The data range for the <see cref="DeleteIfEmptyColumnName" /> column.
    /// </param>
    ///
    /// <param name="aoDeleteIfEmptyValues">
    /// The data values for the <see cref="DeleteIfEmptyColumnName" /> column.
    /// </param>
    //*************************************************************************

    protected void
    DeleteMarkedRows
    (
        ListObject oEdgeTable,
        Range oDeleteIfEmptyData,
        Object [,] aoDeleteIfEmptyValues
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oDeleteIfEmptyData != null);
        Debug.Assert(aoDeleteIfEmptyValues != null);
        AssertValid();

        Range oMarkedRows = null;

        if (oDeleteIfEmptyData.Rows.Count != 1)
        {
            try
            {
                oMarkedRows = oDeleteIfEmptyData.SpecialCells(
                    XlCellType.xlCellTypeBlanks, Missing.Value);
            }
            catch (COMException)
            {
                // There are no such rows.

                oMarkedRows = null;
            }
        }
        else
        {
            // Range.SpecialCells() can't be used in the one-cell case, for
            // which it behaves in a bizarre manner.  See this posting:
            //
            // http://ewbi.blogs.com/develops/2006/03/determine_if_a_.html
            //
            // ...of which this is an excerpt:
            //
            // "SpecialCells ignores any source Range consisting of only one
            // cell. When executing SpecialCells on a Range having only one
            // cell, it will instead consider all of the cells falling within
            // the boundary marked by the bottom right cell of the source Range
            // sheet's UsedRange."
            //
            // Instead, just check the single row.

            if (aoDeleteIfEmptyValues[1, 1] == null)
            {
                oMarkedRows = oDeleteIfEmptyData.EntireRow;
            }
        }

        if (oMarkedRows != null)
        {
            // Delete the marked rows, which are now contiguous.

            Debug.Assert(oMarkedRows.Areas.Count == 1);

            oMarkedRows.EntireRow.Delete(XlDeleteShiftDirection.xlShiftUp);
        }
    }

    //*************************************************************************
    //  Method: SortEdgeTable()
    //
    /// <summary>
    /// Sorts the edge table on a specified range.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// The workbook's edge table.
    /// </param>
    ///
    /// <param name="oRangeToSortOn">
    /// The range to sort the table on.
    /// </param>
    //*************************************************************************

    protected void
    SortEdgeTable
    (
        ListObject oEdgeTable,
        Range oRangeToSortOn
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oRangeToSortOn != null);
        AssertValid();

        Sort oSort = oEdgeTable.Sort;
        SortFields oSortFields = oSort.SortFields;
        oSortFields.Clear();

        oSortFields.Add(oRangeToSortOn, XlSortOn.xlSortOnValues,
            XlSortOrder.xlAscending, Missing.Value,
            XlSortDataOption.xlSortNormal);

        oSort.Apply();
        oSortFields.Clear();
    }

    //*************************************************************************
    //  Method: GetRowCount()
    //
    /// <summary>
    /// Gets the number of rows in a column given the data obtained from the
    /// column.
    /// </summary>
    ///
    /// <param name="aoColumnValues">
    /// The data obtaines from the column.
    /// </param>
    ///
    /// <returns>
    /// The number of rows in the column.
    /// </returns>
    //*************************************************************************

    protected Int32
    GetRowCount
    (
        Object [,] aoColumnValues
    )
    {
        Debug.Assert(aoColumnValues != null);
        AssertValid();

        return ( aoColumnValues.GetUpperBound(0) -
            aoColumnValues.GetLowerBound(0) + 1);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the temporary column used to mark duplicates that aren't the
    /// first instance.

    protected const String DeleteIfEmptyColumnName =
        "Delete if Empty";

    /// Name of the temporary column used to save the sort order.

    protected const String OriginalSortOrderColumnName =
        "Original Sort Order";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
