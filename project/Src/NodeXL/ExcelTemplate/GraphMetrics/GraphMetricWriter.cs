

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricWriter
//
/// <summary>
/// Writes GraphMetricColumn objects to the workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="WriteGraphMetricColumnsToWorkbook" /> to write one or more
/// <see cref="GraphMetricColumn" /> objects to a workbook, then call <see
/// cref="ActivateRelevantWorksheet" /> to let the user know that graph metrics
/// have been calculated.
/// </remarks>
//*****************************************************************************

public partial class GraphMetricWriter : Object
{
    //*************************************************************************
    //  Constructor: GraphMetricWriter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricWriter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphMetricWriter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: WriteGraphMetricColumnsToWorkbook()
    //
    /// <summary>
    /// Writes an array of GraphMetricColumn objects to the workbook.
    /// </summary>
    ///
    /// <param name="graphMetricColumns">
    /// An array of GraphMetricColumn objects, one for each column of metrics
    /// that should be written to the workbook.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public void
    WriteGraphMetricColumnsToWorkbook
    (
        GraphMetricColumn [] graphMetricColumns,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(graphMetricColumns != null);
        Debug.Assert(workbook != null);
        AssertValid();

        // Clear any worksheets that must be cleared before anything else is
        // done.

        ClearWorksheets(graphMetricColumns, workbook);

        // (Note: Don't sort grapMetricColumns by worksheet name/table name in
        // an effort to minimize worksheet switches in the code below.  That
        // would interfere with the column order specified by the
        // IGraphMetricCalculator2 implementations.

        // Create a dictionary of tables that have been written to.  The key is
        // the worksheet name + table name, and the value is a WrittenTableInfo
        // object that contains information about the table.

        Dictionary<String, WrittenTableInfo> oWrittenTables =
            new Dictionary<String, WrittenTableInfo>();

        // Loop through the columns.

        String sCurrentWorksheetPlusTable = String.Empty;
        ListObject oTable = null;

        foreach (GraphMetricColumn oGraphMetricColumn in graphMetricColumns)
        {
            String sThisWorksheetPlusTable = oGraphMetricColumn.WorksheetName
                + oGraphMetricColumn.TableName;

            if (sThisWorksheetPlusTable != sCurrentWorksheetPlusTable)
            {
                // This is a different table.  Get its ListObject.

                if ( !TryGetTable(workbook, oGraphMetricColumn, out oTable) )
                {
                    continue;
                }

                sCurrentWorksheetPlusTable = sThisWorksheetPlusTable;
            }

            WrittenTableInfo oWrittenTableInfo;

            if ( !oWrittenTables.TryGetValue(sThisWorksheetPlusTable,
                out oWrittenTableInfo) )
            {
                // Show all the table's columns.  If a graph metric column
                // isn't visible, it can't be written to.

                ExcelHiddenColumns oExcelHiddenColumns =
                    ExcelColumnHider.ShowHiddenColumns(oTable);

                oWrittenTableInfo = new WrittenTableInfo();
                oWrittenTableInfo.Table = oTable;
                oWrittenTableInfo.HiddenColumns = oExcelHiddenColumns;
                oWrittenTableInfo.Cleared = false;

                oWrittenTables.Add(sThisWorksheetPlusTable, oWrittenTableInfo);
            }

            // Apparent Excel bug: Adding a column when the header row is not
            // the default row height increases the header row height.  Work
            // around this by saving the height and restoring it below.

            Double dHeaderRowHeight = (Double)oTable.HeaderRowRange.RowHeight;

            // Write the column.

            Debug.Assert(oTable != null);

            if (oGraphMetricColumn is GraphMetricColumnWithID)
            {
                WriteGraphMetricColumnWithIDToWorkbook(
                    (GraphMetricColumnWithID)oGraphMetricColumn, oTable);
            }
            else if (oGraphMetricColumn is GraphMetricColumnOrdered)
            {
                if (!oWrittenTableInfo.Cleared)
                {
                    // GraphMetricColumnOrdered columns require that the table
                    // be cleared before any graph metric values are written to
                    // it.

                    ExcelTableUtil.ClearTable(oTable);
                    oWrittenTableInfo.Cleared = true;

                    // Clear AutoFiltering, which interferes with writing an
                    // ordered list to the column.

                    ExcelTableUtil.ClearTableAutoFilters(oTable);
                }

                WriteGraphMetricColumnOrderedToWorkbook(
                    (GraphMetricColumnOrdered)oGraphMetricColumn, oTable);
            }
            else
            {
                Debug.Assert(false);
            }

            oTable.HeaderRowRange.RowHeight = dHeaderRowHeight;
        }

        // Restore any hidden columns in the tables that were written to.

        foreach (KeyValuePair<String, WrittenTableInfo> oKeyValuePair in
            oWrittenTables)
        {
            WrittenTableInfo oWrittenTableInfo = oKeyValuePair.Value;

            ExcelColumnHider.RestoreHiddenColumns(oWrittenTableInfo.Table,
                oWrittenTableInfo.HiddenColumns);
        }
    }

    //*************************************************************************
    //  Method: ActivateRelevantWorksheet()
    //
    /// <summary>
    /// Activates a worksheet to let the user know that graph metrics have been
    /// calculated and shows the graph metric column groups if appropriate.
    /// </summary>
    ///
    /// <param name="graphMetricColumns">
    /// An array of GraphMetricColumn objects, one for each column of metrics
    /// that were written to the workbook.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public void
    ActivateRelevantWorksheet
    (
        IEnumerable<GraphMetricColumn> graphMetricColumns,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(graphMetricColumns != null);
        Debug.Assert(workbook != null);
        AssertValid();

        Boolean bEdgeMetricCalculated = false;
        Boolean bVertexMetricCalculated = false;
        Boolean bOverallMetricsCalculated = false;
        Boolean bGroupsCalculated = false;
        Boolean bGroupMetricCalculated = false;
        Boolean bWordMetricCalculated = false;
        Boolean bTopNByMetricCalculated = false;

        Boolean bShowGraphMetricColumnGroups = false;

        foreach (GraphMetricColumn oGraphMetricColumn in graphMetricColumns)
        {
            switch (oGraphMetricColumn.WorksheetName)
            {
                case WorksheetNames.Edges:

                    bEdgeMetricCalculated = true;
                    bShowGraphMetricColumnGroups = true;
                    break;

                case WorksheetNames.Vertices:

                    bVertexMetricCalculated = true;
                    bShowGraphMetricColumnGroups = true;
                    break;

                case WorksheetNames.Groups:

                    if (oGraphMetricColumn.ColumnName ==
                        GroupTableColumnNames.Name)
                    {
                        bGroupsCalculated = true;
                    }
                    else if (oGraphMetricColumn.ColumnName ==
                        GroupTableColumnNames.Vertices)
                    {
                        bGroupMetricCalculated = true;
                        bShowGraphMetricColumnGroups = true;
                    }

                    break;

                case WorksheetNames.OverallMetrics:

                    bOverallMetricsCalculated = true;
                    break;

                case WorksheetNames.WordCounts:
                case WorksheetNames.WordPairCounts:

                    bWordMetricCalculated = true;
                    break;

                case WorksheetNames.TopNByMetrics:

                    bTopNByMetricCalculated = true;
                    break;

                default:

                    break;
            }
        }

        if (bShowGraphMetricColumnGroups)
        {
            ColumnGroupManager.ShowOrHideColumnGroups(workbook,

                ColumnGroups.EdgeGraphMetrics |
                ColumnGroups.VertexGraphMetrics |
                ColumnGroups.GroupGraphMetrics |
                ColumnGroups.GroupEdgeGraphMetrics,

                true, true);
        }

        // The worksheet to activate should be the one corresponding to the
        // bottommost checked item in the GraphMetricsDialog.

        String sWorksheetNameToActivate = null;

        if (bTopNByMetricCalculated)
        {
            sWorksheetNameToActivate = WorksheetNames.TopNByMetrics;
        }
        else if (bWordMetricCalculated)
        {
            sWorksheetNameToActivate = WorksheetNames.WordCounts;
        }
        else if (bOverallMetricsCalculated)
        {
            sWorksheetNameToActivate = WorksheetNames.OverallMetrics;
        }
        else if (bGroupsCalculated || bGroupMetricCalculated)
        {
            sWorksheetNameToActivate = WorksheetNames.Groups;
        }
        else if (bEdgeMetricCalculated)
        {
            sWorksheetNameToActivate = WorksheetNames.Edges;
        }
        else if (bVertexMetricCalculated)
        {
            sWorksheetNameToActivate = WorksheetNames.Vertices;
        }

        Worksheet oWorksheetToActivate;

        if (
            sWorksheetNameToActivate != null
            &&
            ExcelUtil.TryGetWorksheet(workbook, sWorksheetNameToActivate,
                out oWorksheetToActivate)
            )
        {
            ExcelUtil.ActivateWorksheet(oWorksheetToActivate);
        }
    }

    //*************************************************************************
    //  Method: ClearWorksheets()
    //
    /// <summary>
    /// Clears any worksheets that must be cleared before anything else is
    /// done.
    /// </summary>
    ///
    /// <param name="aoGraphMetricColumns">
    /// An array of GraphMetricColumn objects, one for each column of metrics
    /// that should be written to the workbook.
    /// </param>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    protected void
    ClearWorksheets
    (
        GraphMetricColumn [] aoGraphMetricColumns,
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(aoGraphMetricColumns != null);
        Debug.Assert(oWorkbook != null);
        AssertValid();

        // The top-N-by and top Twitter search network item worksheets can
        // contain multiple tables, one below the other.  If one of the tables
        // has more rows than before, Excel will raise an error when the table
        // is written again, because it would overlap the next table.  Clear
        // the entire worksheet so that all the tables are written from
        // scratch.

        foreach (String sWorksheetName in new String [] {
            WorksheetNames.TopNByMetrics,
            WorksheetNames.TwitterSearchNetworkTopItems,
            } )
        {
            if ( GraphMetricColumnsIncludeWorksheet(aoGraphMetricColumns,
                sWorksheetName) )
            {
                ExcelUtil.TryClearWorksheet(oWorkbook, sWorksheetName);
            }
        }
    }

    //*************************************************************************
    //  Method: GraphMetricColumnsIncludeWorksheet()
    //
    /// <summary>
    /// Determines whether a collection of GraphMetricColumn objects includes
    /// at least one column on a specified worksheet.
    /// </summary>
    ///
    /// <param name="oGraphMetricColumns">
    /// A collection of GraphMetricColumn objects.
    /// </param>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet to look for.
    /// </param>
    ///
    /// <remarks>
    /// true if <paramref name="sWorksheetName" /> is included at least once in
    /// <paramref name="oGraphMetricColumns" />.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    GraphMetricColumnsIncludeWorksheet
    (
        IEnumerable<GraphMetricColumn> oGraphMetricColumns,
        String sWorksheetName
    )
    {
        Debug.Assert(oGraphMetricColumns != null);
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        AssertValid();

        return ( oGraphMetricColumns.Any(oGraphMetricColumn =>
            oGraphMetricColumn.WorksheetName == sWorksheetName) );
    }

    //*************************************************************************
    //  Method: TryGetTable()
    //
    /// <summary>
    /// Attempts to get a table to write to.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="oGraphMetricColumn">
    /// The GraphMetricColumn object that the caller will write to the
    /// workbook.
    /// </param>
    ///
    /// <param name="oTable">
    /// Where the table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the table was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetTable
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        GraphMetricColumn oGraphMetricColumn,
        out ListObject oTable
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oGraphMetricColumn != null);
        AssertValid();

        oTable = null;
        String sTableName = oGraphMetricColumn.TableName;

        if ( !ExcelTableUtil.TryGetTable(oWorkbook,
            oGraphMetricColumn.WorksheetName, sTableName, out oTable) )
        {
            // The table couldn't be found.  It has to be created.

            if (sTableName == TableNames.GroupEdgeMetrics)
            {
                if ( !TryCreateGroupEdgeTable(oWorkbook, out oTable) )
                {
                    return (false);
                }
            }
            else if ( sTableName.StartsWith(TableNames.TopNByMetricsRoot) )
            {
                if ( !TryCreateStackedTable(oWorkbook,
                    WorksheetNames.TopNByMetrics,
                    oGraphMetricColumn, out oTable) )
                {
                    return (false);
                }
            }
            else if ( sTableName.StartsWith(
                TableNames.TwitterSearchNetworkTopItemsRoot) )
            {
                if ( !TryCreateStackedTable(oWorkbook,
                    WorksheetNames.TwitterSearchNetworkTopItems,
                    oGraphMetricColumn, out oTable) )
                {
                    return (false);
                }
            }
            else if (sTableName == TableNames.WordCounts)
            {
                // There is actually just one table on the words worksheet, not
                // a stack of tables, but that's okay.  TryCreateStackedTable()
                // doesn't care whether it will be called again with another
                // table for the worksheet.

                if ( !TryCreateStackedTable(oWorkbook,
                    WorksheetNames.WordCounts, oGraphMetricColumn,
                    out oTable) )
                {
                    return (false);
                }
            }
            else if (sTableName == TableNames.WordPairCounts)
            {
                if ( !TryCreateStackedTable(oWorkbook,
                    WorksheetNames.WordPairCounts, oGraphMetricColumn,
                    out oTable) )
                {
                    return (false);
                }
            }
            else
            {
                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: WriteGraphMetricColumnWithIDToWorkbook()
    //
    /// <summary>
    /// Writes a GraphMetricColumnWithID object to the workbook.
    /// </summary>
    ///
    /// <param name="oGraphMetricColumnWithID">
    /// The GraphMetricColumnWithID object to write to the workbook.
    /// </param>
    ///
    /// <param name="oTable">
    /// The table containing the column.
    /// </param>
    //*************************************************************************

    protected void
    WriteGraphMetricColumnWithIDToWorkbook
    (
        GraphMetricColumnWithID oGraphMetricColumnWithID,
        ListObject oTable
    )
    {
        Debug.Assert(oGraphMetricColumnWithID != null);
        Debug.Assert(oTable != null);
        AssertValid();

        // Get the required column information.

        Range oVisibleColumnData, oIDColumnData;

        if ( !TryGetRequiredColumnWithIDInformation(oGraphMetricColumnWithID,
            oTable, out oVisibleColumnData, out oIDColumnData) )
        {
            return;
        }

        // Store the column's GraphMetricValueWithID objects in a dictionary.
        // The key is the GraphMetricValueWithID.RowID and the value is the
        // GraphMetricValueWithID.

        Dictionary<Int32, GraphMetricValueWithID> oIDDictionary =
            new Dictionary<Int32, GraphMetricValueWithID>();

        foreach (GraphMetricValueWithID oGraphMetricValueWithID in
            oGraphMetricColumnWithID.GraphMetricValuesWithID)
        {
            oIDDictionary.Add(oGraphMetricValueWithID.RowID,
                oGraphMetricValueWithID);
        }

        Debug.Assert(oTable.Parent is Worksheet);
        Worksheet oWorksheet = (Worksheet)oTable.Parent;

        Int32 iIDColumnNumberOneBased = oIDColumnData.Column;

        // Loop through the areas, and split each area into subranges if the
        // area contains too many rows.

        foreach ( Range oColumnSubrange in
            ExcelRangeSplitter.SplitRange(oVisibleColumnData) )
        {
            Int32 iRows = oColumnSubrange.Rows.Count;

            Range oIDColumnSubrange = ExcelRangeSplitter.GetParallelSubrange(
                oColumnSubrange, iIDColumnNumberOneBased);

            Debug.Assert(oIDColumnSubrange.Rows.Count == iRows);

            Object [,] aoColumnValues =
                ExcelUtil.GetSingleColumn2DArray(iRows);

            Object [,] aoIDColumnValues =
                ExcelUtil.GetRangeValues(oIDColumnSubrange);

            // Loop through the rows.

            for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
            {
                String sID;
                Int32 iID;

                // Get the ID stored in the row.

                if (
                    !ExcelUtil.TryGetNonEmptyStringFromCell(
                        aoIDColumnValues, iRowOneBased, 1, out sID)
                    ||
                    !Int32.TryParse(sID, out iID)
                   )
                {
                    continue;
                }

                // Is the ID one of the IDs specified within the
                // GraphMetricColumn object?

                GraphMetricValueWithID oGraphMetricValueWithID;

                if ( !oIDDictionary.TryGetValue(iID,
                    out oGraphMetricValueWithID) )
                {
                    // No.

                    continue;
                }

                // Set the column cell in this row to the specified value.

                aoColumnValues[iRowOneBased, 1] =
                    oGraphMetricValueWithID.Value;
            }

            oColumnSubrange.set_Value(Missing.Value, aoColumnValues);

            if (oGraphMetricColumnWithID.ConvertUrlsToHyperlinks)
            {
                ExcelUtil.ConvertUrlsToHyperlinks(oColumnSubrange);
            }
        }
    }

    //*************************************************************************
    //  Method: WriteGraphMetricColumnOrderedToWorkbook()
    //
    /// <summary>
    /// Writes a GraphMetricColumnOrdered object to the workbook.
    /// </summary>
    ///
    /// <param name="oGraphMetricColumnOrdered">
    /// The GraphMetricColumnOrdered object to write to the workbook.
    /// </param>
    ///
    /// <param name="oTable">
    /// The table containing the column.
    /// </param>
    //*************************************************************************

    protected void
    WriteGraphMetricColumnOrderedToWorkbook
    (
        GraphMetricColumnOrdered oGraphMetricColumnOrdered,
        ListObject oTable
    )
    {
        Debug.Assert(oGraphMetricColumnOrdered != null);
        Debug.Assert(oTable != null);
        AssertValid();

        GraphMetricValueOrdered [] aoGraphMetricValuesOrdered =
            oGraphMetricColumnOrdered.GraphMetricValuesOrdered;

        Int32 iRows = aoGraphMetricValuesOrdered.Length;

        // Make sure the table has enough rows.

        ResizeTable(oTable, iRows);

        Range oVisibleColumnData;

        // Get the specified column.

        if ( !TryGetRequiredColumnInformation(oGraphMetricColumnOrdered,
            oTable, out oVisibleColumnData) )
        {
            return;
        }

        // Copy the graph metric values to an array.

        Object [,] aoValues = ExcelUtil.GetSingleColumn2DArray(iRows);
        Boolean bStyleSpecified = false;

        for (Int32 i = 0; i < iRows; i++)
        {
            GraphMetricValueOrdered oGraphMetricValueOrdered =
                aoGraphMetricValuesOrdered[i];

            aoValues[i + 1, 1] = oGraphMetricValueOrdered.Value;

            if (oGraphMetricValueOrdered.Style != null)
            {
                // A style was specified for this row.  It will need to be
                // applied later.  (It should be possible to apply the style
                // here, but that does not work reliably when values are
                // written to the cells afterwards, as they are after this
                // loop.)

                bStyleSpecified = true;
            }
        }

        // Write the array to the column.

        Range oWrittenRange = ExcelUtil.SetRangeValues(
            oVisibleColumnData, aoValues);

        if (oGraphMetricColumnOrdered.ConvertUrlsToHyperlinks)
        {
            ExcelUtil.ConvertUrlsToHyperlinks(oWrittenRange);
        }

        if (bStyleSpecified)
        {
            for (Int32 i = 0; i < iRows; i++)
            {
                String sStyle = aoGraphMetricValuesOrdered[i].Style;

                if (sStyle != null)
                {
                    // This row has a style.  Apply it.

                    ExcelUtil.SetRangeStyle(
                        (Range)oVisibleColumnData.Cells[i + 1, 1], sStyle);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: ResizeTable()
    //
    /// <summary>
    /// Resizes a table if necessary so it has a minimum number of rows.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table to resize.
    /// </param>
    ///
    /// <param name="iMinimumRows">
    /// Minimum number of data rows the table must have.  Can be zero.
    /// </param>
    //*************************************************************************

    protected void
    ResizeTable
    (
        ListObject oTable,
        Int32 iMinimumRows
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert(iMinimumRows >= 0);
        AssertValid();

        ListRows oRows = oTable.ListRows;

        // Note that ListRows.Count returns 0 even if there is one data row.
        // For example, if the table is empty when this method is called and
        // iMinimumRows=5, ListRows.Count will return 0 but adding 5 rows will
        // result in 6 rows.

        Int32 iRows = Math.Max(oRows.Count, 1);

        while (iRows < iMinimumRows)
        {
            oRows.Add(Missing.Value);
            iRows++;
        }
    }

    //*************************************************************************
    //  Method: TryGetRequiredColumnWithIDInformation()
    //
    /// <summary>
    /// Gets the column information required to write a GraphMetricColumnWithID
    /// object to the workbook.
    /// </summary>
    ///
    /// <param name="oGraphMetricColumnWithID">
    /// The GraphMetricColumnWithID object to write to the workbook.
    /// </param>
    ///
    /// <param name="oTable">
    /// The table containing the column.
    /// </param>
    ///
    /// <param name="oVisibleColumnData">
    /// Where the visible range of the specified column gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <param name="oIDColumnData">
    /// Where the ID column gets stored if true is returned.  The column may
    /// contain hidden rows.
    /// </param>
    ///
    /// <returns>
    /// true if the column information was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetRequiredColumnWithIDInformation
    (
        GraphMetricColumnWithID oGraphMetricColumnWithID,
        ListObject oTable,
        out Range oVisibleColumnData,
        out Range oIDColumnData
    )
    {
        Debug.Assert(oGraphMetricColumnWithID != null);
        Debug.Assert(oTable != null);
        AssertValid();

        oVisibleColumnData = null;
        oIDColumnData = null;

        // Get the specified column.

        if ( !TryGetRequiredColumnInformation(oGraphMetricColumnWithID, oTable,
            out oVisibleColumnData) )
        {
            return (false);
        }

        // Get the ID column.

        if (!ExcelTableUtil.TryGetTableColumnData(oTable,
            CommonTableColumnNames.ID, out oIDColumnData) )
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetRequiredColumnInformation()
    //
    /// <summary>
    /// Gets the column information required to write a GraphMetricColumn
    /// object to the workbook.
    /// </summary>
    ///
    /// <param name="oGraphMetricColumn">
    /// The GraphMetricColumn object to write to the workbook.
    /// </param>
    ///
    /// <param name="oTable">
    /// The table containing the column.
    /// </param>
    ///
    /// <param name="oVisibleColumnData">
    /// Where the visible range of the specified column gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the column information was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetRequiredColumnInformation
    (
        GraphMetricColumn oGraphMetricColumn,
        ListObject oTable,
        out Range oVisibleColumnData
    )
    {
        Debug.Assert(oGraphMetricColumn != null);
        Debug.Assert(oTable != null);
        AssertValid();

        oVisibleColumnData = null;

        // Add the specified column if it's not already present.

        String sColumnName = oGraphMetricColumn.ColumnName;
        String sColumnStyle = oGraphMetricColumn.Style;

        Microsoft.Office.Interop.Excel.ListColumn oColumn;

        if (
            !ExcelTableUtil.TryGetTableColumn(oTable, sColumnName, out oColumn)
            &&
            !ExcelTableUtil.TryAddTableColumn(oTable, sColumnName,
                oGraphMetricColumn.ColumnWidthChars, sColumnStyle, out oColumn)
            )
        {
            // Give up.

            return (false);
        }

        Range oColumnData;

        if (!ExcelTableUtil.TryGetTableColumnData(oTable, sColumnName,
            out oColumnData) )
        {
            return (false);
        }

        ExcelUtil.SetRangeStyle(oColumnData, sColumnStyle);

        String sNumberFormat = oGraphMetricColumn.NumberFormat;

        if (sNumberFormat != null)
        {
            oColumnData.NumberFormat = sNumberFormat;
        }

        // Wrapping text makes Range.set_Value() very slow, so turn it off.

        oColumn.Range.WrapText = false;

        // But wrap the text in the column's header.

        ExcelTableUtil.WrapTableColumnHeader(oColumn);

        // Get the visible range.

        if ( !ExcelUtil.TryGetVisibleRange(oColumnData,
            out oVisibleColumnData) )
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCreateGroupEdgeTable()
    //
    /// <summary>
    /// Creates the group-edge table.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="oGroupEdgeTable">
    /// Where the new table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// The group-edge worksheet and table are created on demand, which permits
    /// group-edge information to be calculated for all NodeXL workbooks,
    /// including older ones that lack the worksheet.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryCreateGroupEdgeTable
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        out ListObject oGroupEdgeTable
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        oGroupEdgeTable = null;
        Worksheet oGroupEdgeWorksheet;

        if ( !ExcelUtil.TryGetOrAddWorksheet(oWorkbook,
            WorksheetNames.GroupEdgeMetrics, out oGroupEdgeWorksheet) )
        {
            return (false);
        }

        Range oRange;
            
        try
        {
            // Create just the first column.  The table will auto-expand when
            // this class adds the table's other columns.

            oRange = ExcelUtil.SetCellStringValue(oGroupEdgeWorksheet, "C1",
                "Graph Metrics");

            ExcelUtil.SetCellStringValue(oGroupEdgeWorksheet, "A2",
                GroupEdgeTableColumnNames.Group1Name);

            ExcelUtil.SetRangeStyle(oRange, CellStyleNames.GraphMetricGood);
            oRange.EntireColumn.AutoFit();

            oGroupEdgeTable = ExcelTableUtil.AddTable(oGroupEdgeWorksheet,
                ExcelUtil.GetRange(oGroupEdgeWorksheet, "A2:A3"),
                TableNames.GroupEdgeMetrics, TableStyleNames.NodeXLTable);
        }
        catch (System.Runtime.InteropServices.COMException)
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCreateStackedTable()
    //
    /// <summary>
    /// Creates a table that can be stacked with other tables.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet that will contain the stacked table.
    /// </param>
    ///
    /// <param name="oGraphMetricColumn">
    /// The GraphMetricColumn object that the caller will write to the table.
    /// This is assumed to correspond to the table's first column.
    /// </param>
    ///
    /// <param name="oStackedTable">
    /// Where the new table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method should be used when creating one table on a worksheet in
    /// which multiple tables can be stacked vertically.  All the worksheet's
    /// tables have their first column in column A, but the width and height of
    /// the tables can vary.
    ///
    /// <para>
    /// This method creates a worksheet if necessary, and then the stacked
    /// table.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryCreateStackedTable
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        String sWorksheetName,
        GraphMetricColumn oGraphMetricColumn,
        out ListObject oStackedTable
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert(oGraphMetricColumn != null);
        AssertValid();

        oStackedTable = null;
        Worksheet oWorksheet;
        Range oColumnARange;

        if (
            !ExcelUtil.TryGetOrAddWorksheet(oWorkbook, sWorksheetName,
                out oWorksheet)
            ||
            !ExcelUtil.TryGetRange(oWorksheet, "A:A", out oColumnARange)
            )
        {
            return (false);
        }

        try
        {
            // The worksheet can contain multiple top-N-by tables, so we need
            // to find where to put the new table.

            Range oColumn1HeaderCell = GetColumn1HeaderCellForStackedTable(
                oWorksheet, oColumnARange);

            // Create just the first column.  The table will auto-expand when
            // this class adds the table's other column.
            //
            // (It's assumed here that oGraphMetricColumn represents the
            // table's first column.)

            oColumn1HeaderCell.set_Value(Missing.Value,
                oGraphMetricColumn.ColumnName);

            Range oTableRange = oColumn1HeaderCell;
            ExcelUtil.ResizeRange(ref oTableRange, 2, 1);

            oStackedTable = ExcelTableUtil.AddTable(oWorksheet, oTableRange,
                oGraphMetricColumn.TableName, TableStyleNames.NodeXLTable);

            ExcelTableUtil.SetColumnWidth(oColumn1HeaderCell,
                oGraphMetricColumn.ColumnWidthChars);
        }
        catch (System.Runtime.InteropServices.COMException)
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetColumn1HeaderCellForStackedTable()
    //
    /// <summary>
    /// Gets the header cell for the first column of a table that can be
    /// stacked with other tables.
    /// </summary>
    ///
    /// <param name="oWorksheet">
    /// Worksheet that will contain the stacked table.
    /// </param>
    ///
    /// <param name="oColumnARange">
    /// The worksheet's column A range.
    /// </param>
    ///
    /// <returns>
    /// The cell where the header of the first column of a new stacked table
    /// should be written.
    /// </returns>
    ///
    /// <remarks>
    /// This method should be used when creating one table on a worksheet in
    /// which multiple tables can be stacked vertically.  All the worksheet's
    /// tables have their first column in column A, but the width and height of
    /// the tables can vary.
    /// </remarks>
    //*************************************************************************

    protected Range
    GetColumn1HeaderCellForStackedTable
    (
        Microsoft.Office.Interop.Excel.Worksheet oWorksheet,
        Range oColumnARange
    )
    {
        Debug.Assert(oWorksheet != null);
        Debug.Assert(oColumnARange != null);
        AssertValid();

        // The worksheet can contain multiple stacked tables, so we need to
        // find where to put the new table.  Find the first empty cell in
        // column A.  If it's not the worksheet's first cell, move three
        // columns down to provide a two-line buffer between tables.

        Range oColumn1HeaderCell = oColumnARange.Find("*", Missing.Value,
            XlFindLookIn.xlValues, XlLookAt.xlWhole, XlSearchOrder.xlByRows,
            XlSearchDirection.xlPrevious, true, true, Missing.Value);

        if (oColumn1HeaderCell == null)
        {
            oColumn1HeaderCell = ExcelUtil.GetRange(oWorksheet, "A1");
        }
        else
        {
            ExcelUtil.OffsetRange(ref oColumn1HeaderCell, 3, 0);
        }

        return (oColumn1HeaderCell);
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
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: WrittenTableInfo
    //
    /// <summary>
    /// Contains information about a table that has been written to.
    /// </summary>
    //*************************************************************************

    private class WrittenTableInfo
    {
        /// The table that was written to.

        public ListObject Table;

        /// Object that remembers which columns were hidden before the table
        /// was written to.

        public ExcelHiddenColumns HiddenColumns;

        /// true if the table has been cleared.

        public Boolean Cleared;
    }
}
}
