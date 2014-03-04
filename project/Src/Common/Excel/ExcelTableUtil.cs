
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: ExcelTableUtil
//
/// <summary>
/// Static utility methods for working with Excel tables.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class ExcelTableUtil
{
    //*************************************************************************
    //  Method: AddTable()
    //
    /// <summary>
    /// Adds a table to a worksheet.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet to add the table to.
    /// </param>
    ///
    /// <param name="tableRange">
    /// The range the table should include.
    /// </param>
    ///
    /// <param name="tableName">
    /// The name to assign to the table, or null to let Excel name the table.
    /// </param>
    ///
    /// <param name="tableStyle">
    /// The style to assign to the table, or null to not assign a style.
    /// </param>
    ///
    /// <returns>
    /// The new table.
    /// </returns>
    //*************************************************************************

    public static ListObject
    AddTable
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        Range tableRange,
        String tableName,
        String tableStyle
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(tableRange != null);

        ListObject oTable = worksheet.ListObjects.Add(
            XlListObjectSourceType.xlSrcRange, tableRange, Missing.Value,
            XlYesNoGuess.xlYes, Missing.Value);

        if (tableName != null)
        {
            Debug.Assert(tableName.Length > 0);
            oTable.Name = tableName;
        }

        if (tableStyle != null)
        {
            Debug.Assert(tableStyle.Length > 0);
            oTable.TableStyle = tableStyle;
        }

        return (oTable);
    }

    //*************************************************************************
    //  Method: SetVisibleTableColumnData()
    //
    /// <overloads>
    /// Sets the values in the visible data range of a table column to a
    /// specified value.
    /// </overloads>
    ///
    /// <summary>
    /// Sets the values in the visible data range of a table column to a
    /// specified value given a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the column.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the column.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table containing the column.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to set.
    /// </param>
    ///
    /// <param name="value">
    /// The value to set.
    /// </param>
    ///
    /// <remarks>
    /// This method sets the value of every visible data cell in <paramref
    /// name="columnName" /> to <paramref name="value" />.
    /// </remarks>
    //*************************************************************************

    public static void
    SetVisibleTableColumnData
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName,
        String columnName,
        Object value
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        ListObject oTable;

        if ( TryGetTable(workbook, worksheetName, tableName, out oTable) )
        {
            SetVisibleTableColumnData(oTable, columnName, value);
        }
    }

    //*************************************************************************
    //  Method: SetVisibleTableColumnData()
    //
    /// <summary>
    /// Sets the values in the visible data range of a table column to a
    /// specified value given a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table containing the column.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to set.
    /// </param>
    ///
    /// <param name="value">
    /// The value to set.
    /// </param>
    ///
    /// <remarks>
    /// This method sets the value of every visible data cell in <paramref
    /// name="columnName" /> to <paramref name="value" />.
    /// </remarks>
    //*************************************************************************

    public static void
    SetVisibleTableColumnData
    (
        ListObject table,
        String columnName,
        Object value
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Range oColumnData;

        if ( TryGetTableColumnData(table, columnName, out oColumnData) )
        {
            ExcelUtil.SetVisibleRangeValue(oColumnData, value);
        }
    }

    //*************************************************************************
    //  Method: SetVisibleSelectedTableColumnData()
    //
    /// <summary>
    /// Sets the selected, visible data cells of a table column to a specified
    /// value.
    /// </summary>
    ///
    /// <param name="table">
    /// Table containing the column.  The table must be within the active
    /// worksheet.
    /// </param>
    ///
    /// <param name="selectedRange">
    /// The application's current selected range.  The range may contain
    /// multiple areas.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to set.
    /// </param>
    ///
    /// <param name="value">
    /// The value to set.
    /// </param>
    ///
    /// <remarks>
    /// This method sets the value of every selected, visible data cell in
    /// <paramref name="columnName" /> to <paramref name="value" />.
    /// </remarks>
    //*************************************************************************

    public static void
    SetVisibleSelectedTableColumnData
    (
        ListObject table,
        Range selectedRange,
        String columnName,
        Object value
    )
    {
        Debug.Assert(table != null);
        Debug.Assert(table.Parent is Worksheet);
        Debug.Assert(ExcelUtil.WorksheetIsActive( (Worksheet)table.Parent) );
        Debug.Assert(selectedRange != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Range oSelectedTableRange, oVisibleSelectedTableRange;
        ListColumn oColumn;

        if (
            !TryGetSelectedTableRange(table, selectedRange,
                out oSelectedTableRange)
            ||
            !ExcelUtil.TryGetVisibleRange(oSelectedTableRange,
                out oVisibleSelectedTableRange)
            ||
            !TryGetTableColumn(table, columnName, out oColumn)
            )
        {
            return;
        }

        Worksheet oWorksheet = (Worksheet)table.Parent;
        Int32 iColumnOneBased = oColumn.Range.Columns.Column;

        foreach (Range oVisibleSelectedArea in
            oVisibleSelectedTableRange.Areas)
        {
            Range oRows = oVisibleSelectedArea.Rows;
            Int32 iFirstRowOneBased = oRows.Row;
            Int32 iRows = oRows.Count;

            Range oRangeToSet = (Range)oWorksheet.get_Range(

                (Range)oWorksheet.Cells[iFirstRowOneBased, iColumnOneBased],

                (Range)oWorksheet.Cells[iFirstRowOneBased + iRows - 1,
                    iColumnOneBased]
                );

            oRangeToSet.set_Value(Missing.Value, value);
        }
    }

    //*************************************************************************
    //  Method: SetTableWrapText()
    //
    /// <summary>
    /// Turns text wrapping on or off in a table's data range.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the table from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table to get.
    /// </param>
    ///
    /// <param name="wrapText">
    /// true to turn text wrapping on, false to turn it off.
    /// </param>
    //*************************************************************************

    public static void
    SetTableWrapText
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName,
        Boolean wrapText
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );

        ListObject oTable;

        if ( TryGetTable(workbook, worksheetName, tableName, out oTable) )
        {
            Range oDataBodyRange = oTable.DataBodyRange;

            if (oDataBodyRange != null)
            {
                oDataBodyRange.WrapText = wrapText;
            }
        }
    }

    //*************************************************************************
    //  Method: SelectAllTableRows()
    //
    /// <summary>
    /// Selects all the data rows in a table.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the table from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table to select the rows within.
    /// </param>
    ///
    /// <remarks>
    /// This method activates the worksheet before selecting the table rows.
    /// </remarks>
    //*************************************************************************

    public static void
    SelectAllTableRows
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );

        ListObject oTable;

        if ( ExcelTableUtil.TryGetTable(workbook, worksheetName, tableName,
            out oTable) )
        {
            Range oDataBodyRange = oTable.DataBodyRange;

            if (oDataBodyRange != null)
            {
                ExcelUtil.SelectRange(oDataBodyRange);
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetTable()
    //
    /// <overloads>
    /// Attempts to get a table (ListObject) by name.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get a table (ListObject) from a workbook by worksheet name
    /// and table name.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the table from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table to get.
    /// </param>
    ///
    /// <param name="table">
    /// Where the requested table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="workbook" /> contains a worksheet named <paramref
    /// name="worksheetName" /> that has a table (ListObject) named
    /// <paramref name="tableName" />, the ListObject is stored at <paramref
    /// name="table" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTable
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName,
        out ListObject table
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );

        table = null;

        Microsoft.Office.Interop.Excel.Worksheet oWorksheet;

        return (
            ExcelUtil.TryGetWorksheet(workbook, worksheetName, out oWorksheet)
            &&
            TryGetTable(oWorksheet, tableName, out table)
            );
    }

    //*************************************************************************
    //  Method: TryGetTable()
    //
    /// <summary>
    /// Attempts to get a table (ListObject) from a worksheet by table name.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet to get the table from.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table to get.
    /// </param>
    ///
    /// <param name="table">
    /// Where the requested table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="worksheet" /> contains a table (ListObject) named
    /// <paramref name="tableName" />, the ListObject is stored at <paramref
    /// name="table" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTable
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        String tableName,
        out ListObject table
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert( !String.IsNullOrEmpty(tableName) );

        table = null;

        try
        {
            table = worksheet.ListObjects[tableName];

            return (true);
        }
        catch (COMException)
        {
            return (false);
        }
    }

    //*************************************************************************
    //  Method: VisibleTableRangeIsEmpty()
    //
    /// <summary>
    /// Determines whether the visible range of a table is empty.
    /// </summary>
    ///
    /// <param name="table">
    /// The table to test.
    /// </param>
    ///
    /// <returns>
    /// true if the visible part of the data body range of the table is empty.
    /// </returns>
    //*************************************************************************

    public static Boolean
    VisibleTableRangeIsEmpty
    (
        ListObject table
    )
    {
        Debug.Assert(table != null);

        Range oVisibleTableRange;

        if ( TryGetVisibleTableRange(table, out oVisibleTableRange) )
        {
            foreach (Range oArea in oVisibleTableRange.Areas)
            {
                Range oUsedRange;

                if ( ExcelUtil.TryGetNonEmptyRangeInVisibleArea(oArea,
                    out oUsedRange) )
                {
                    return (false);
                }
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetOffsetOfFirstEmptyTableRow()
    //
    /// <summary>
    /// Gets the offset of the first empty row in a table.
    /// </summary>
    ///
    /// <param name="table">
    /// The table to get the offset for.
    /// </param>
    ///
    /// <returns>
    /// The offset of the first empty row in <paramref name="table" />,
    /// measured from the first row in the table's data body range.
    /// </returns>
    ///
    /// <remarks>
    /// If the table is empty, 0 is returned.  If the table has data in its
    /// first N data rows, for example, N is returned.
    /// </remarks>
    //*************************************************************************

    public static Int32
    GetOffsetOfFirstEmptyTableRow
    (
        ListObject table
    )
    {
        Debug.Assert(table != null);

        Int32 iLastNonEmptyRowOneBased;
        Range oDataBodyRange = table.DataBodyRange;

        if (
            oDataBodyRange != null
            &&
            ExcelUtil.TryGetLastNonEmptyRow(oDataBodyRange,
                out iLastNonEmptyRowOneBased)
            )
        {
            return (iLastNonEmptyRowOneBased - oDataBodyRange.Row + 1);
        }

        return (0);
    }

    //*************************************************************************
    //  Method: TryGetVisibleTableRange()
    //
    /// <summary>
    /// Attempts to get a range containing visible data rows from a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get a range from.
    /// </param>
    ///
    /// <param name="visibleTableRange">
    /// Where the range gets stored if true is returned.  The range contains at
    /// least one visible row and may consist of multiple areas.
    /// </param>
    ///
    /// <returns>
    /// true if the range was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="table" /> contains at least one data row that is
    /// visible, the range of visible rows is stored at <paramref
    /// name="visibleTableRange" /> and true is returned.  false is returned
    /// otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetVisibleTableRange
    (
        ListObject table,
        out Range visibleTableRange
    )
    {
        Debug.Assert(table != null);

        visibleTableRange = null;

        // Read the range that contains the table data.  If the table is
        // filtered, the range may contain multiple areas.

        Range oDataBodyRange = table.DataBodyRange;

        // Reduce the range to visible rows.

        return (
            oDataBodyRange != null
            &&
            ExcelUtil.TryGetVisibleRange(oDataBodyRange, out visibleTableRange)
            );
    }

    //*************************************************************************
    //  Method: TryGetSelectedTableRange()
    //
    /// <overloads>
    /// Attempts to get the selected range within a table.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get the selected range within a table after activating the
    /// table's parent worksheet.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the table from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table.
    /// </param>
    ///
    /// <param name="table">
    /// Where the table gets stored if true is returned.
    /// </param>
    ///
    /// <param name="selectedTableRange">
    /// Where the selected range within the table gets stored if true is
    /// returned.  The range may contain multiple areas.
    /// </param>
    ///
    /// <returns>
    /// true if the current selected range intersects the table.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetSelectedTableRange
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName,
        out ListObject table,
        out Range selectedTableRange
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );

        selectedTableRange = null;
        table = null;

        Worksheet oWorksheet;

        if (
            ExcelUtil.TryGetWorksheet(workbook, worksheetName, out oWorksheet)
            &&
            TryGetTable(oWorksheet, tableName, out table)
            )
        {
            ExcelUtil.ActivateWorksheet(oWorksheet);

            Object oSelectedRange = workbook.Application.Selection;

            if (
                oSelectedRange is Range
                &&
                TryGetSelectedTableRange(table, (Range)oSelectedRange,
                    out selectedTableRange )
                )
            {
                return (true);
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TryGetSelectedTableRange()
    //
    /// <summary>
    /// Attempts to get the selected range within a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the selected range within.  The table must be within the
    /// active worksheet.
    /// </param>
    ///
    /// <param name="selectedRange">
    /// The application's current selected range.  The range may contain
    /// multiple areas.
    /// </param>
    ///
    /// <param name="selectedTableRange">
    /// Where the selected range within the table gets stored if true is
    /// returned.  The range may contain multiple areas.
    /// </param>
    ///
    /// <returns>
    /// true if the table isn't empty and the current selected range intersects
    /// the table.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetSelectedTableRange
    (
        ListObject table,
        Range selectedRange,
        out Range selectedTableRange
    )
    {
        Debug.Assert(table != null);
        Debug.Assert(table.Parent is Worksheet);
        Debug.Assert( ExcelUtil.WorksheetIsActive( (Worksheet)table.Parent) );
        Debug.Assert(selectedRange != null);

        selectedTableRange = null;

        if ( VisibleTableRangeIsEmpty(table) )
        {
            return (false);
        }

        Range oDataBodyRange = table.DataBodyRange;

        if (oDataBodyRange == null)
        {
            return (false);
        }

        // The selected range can extend outside the table.  Get the
        // intersection of the table with the selection.

        return ( ExcelUtil.TryIntersectRanges(selectedRange,
            oDataBodyRange, out selectedTableRange) );
    }

    //*************************************************************************
    //  Method: AddTableRow()
    //
    /// <summary>
    /// Adds a row to the end of a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to add a row to.
    /// </param>
    ///
    /// <param name="columnNameValuePairs">
    /// Name/value pairs.  There is one name/value pair for each cell to fill
    /// in in the new row.  The first element of each pair is the column name,
    /// as a String, and the second element of each pair is the cell value, as
    /// an Object.
    /// </param>
    ///
    /// <returns>
    /// The new row's Range.
    /// </returns>
    //*************************************************************************

    public static Range
    AddTableRow
    (
        ListObject table,
        params Object [] columnNameValuePairs
    )
    {
        Debug.Assert(table!= null);
        Debug.Assert(columnNameValuePairs != null);
        Debug.Assert(columnNameValuePairs.Length % 2 == 0);

        ListRows oRows = table.ListRows;
        Int32 iOriginalRows = oRows.Count;
        Range oNewRowRange;

        if ( iOriginalRows == 1 && VisibleTableRangeIsEmpty(table) )
        {
            // The table contains one empty row.  Use it.

            oNewRowRange = oRows[1].Range;
        }
        else
        {
            oNewRowRange = oRows.Add(Missing.Value).Range;

            if (iOriginalRows == 0 && oRows.Count == 2)
            {
                // Excel bug, as of 9/20/2010:
                //
                // Adding a row to an empty table adds an extra row after the
                // desired row.  See this post:
                //
                // http://social.msdn.microsoft.com/Forums/en-US/vsto/thread/
                // 99cedf41-f2ce-47a5-b698-b3c0b37c8dc5

                oRows[2].Delete();
            }
        }

        Int32 iColumns = oNewRowRange.Columns.Count;
        Object [,] aoNewRowValues = ExcelUtil.GetRangeValues(oNewRowRange);
        Int32 iColumnNamesAndValues = columnNameValuePairs.Length;

        for (Int32 i = 0; i < iColumnNamesAndValues; i += 2)
        {
            Debug.Assert(columnNameValuePairs[i + 0] is String);
            String sColumnName = (String)columnNameValuePairs[i + 0];
            Debug.Assert( !String.IsNullOrEmpty(sColumnName) );

            ListColumn oColumn;

            if ( TryGetTableColumn(table, sColumnName, out oColumn) )
            {
                Int32 iColumnIndexOneBased = oColumn.Index;

                if (iColumnIndexOneBased <= iColumns)
                {
                    aoNewRowValues[1, iColumnIndexOneBased] = 
                        columnNameValuePairs[i + 1];
                }
            }
        }

        oNewRowRange.set_Value(Missing.Value, aoNewRowValues);

        return (oNewRowRange);
    }

    //*************************************************************************
    //  Method: TryGetTableColumn()
    //
    /// <summary>
    /// Attempts to get a table column given the column name.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the column from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get.
    /// </param>
    ///
    /// <param name="column">
    /// Where the column named <paramref name="columnName" /> gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="table" /> contains a column named <paramref
    /// name="columnName" />, the column is stored at <paramref
    /// name="column" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTableColumn
    (
        ListObject table,
        String columnName,
        out ListColumn column
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        column = null;

        try
        {
            column = table.ListColumns[columnName];

            return (true);
        }
        catch (COMException)
        {
            return (false);
        }
    }

    //*************************************************************************
    //  Method: TryGetOrAddTableColumn()
    //
    /// <overloads>
    /// Attempts to get a table column or add the column if it doesn't exist.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get a table column or add the column if it doesn't exist,
    /// and provides the ListColumn for the column.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the column from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the added column, in characters, or <see
    /// cref="AutoColumnWidth" /> to set the width automatically.
    /// </param>
    ///
    /// <param name="columnStyle">
    /// Style of the added column, or null to apply Excel's normal style.
    /// Sample: "Bad".
    /// </param>
    ///
    /// <param name="listColumn">
    /// Where the column gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the column was retrieved or added.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetOrAddTableColumn
    (
        ListObject table,
        String columnName,
        Double columnWidthChars,
        String columnStyle,
        out ListColumn listColumn
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Debug.Assert(columnWidthChars == AutoColumnWidth ||
            columnWidthChars >= 0);

        return (
            TryGetTableColumn(table, columnName, out listColumn)
            ||
            TryAddTableColumn(table, columnName, columnWidthChars, columnStyle,
                out listColumn)
            );
    }

    //*************************************************************************
    //  Method: TryGetOrAddTableColumn()
    //
    /// <summary>
    /// Attempts to get a table column or add the column if it doesn't exist,
    /// and provides the ListColumn, data range and values for the column.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the column from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the added column, in characters, or <see
    /// cref="AutoColumnWidth" /> to set the width automatically.
    /// </param>
    ///
    /// <param name="columnStyle">
    /// Style of the added column, or null to apply Excel's normal style.
    /// Sample: "Bad".
    /// </param>
    ///
    /// <param name="listColumn">
    /// Where the column gets stored if true is returned.
    /// </param>
    ///
    /// <param name="tableColumnData">
    /// Where the column data range gets stored if true is returned.  The data
    /// range includes only that part of the column within the table's data
    /// body range.  This excludes any header or totals row.
    /// </param>
    ///
    /// <param name="tableColumnDataValues">
    /// Where the values for <paramref name="tableColumnData" /> get stored if
    /// true is returned.  The array has one-based dimensions, so the Object
    /// corresponding to the first cell in the range is at [1,1].
    /// </param>
    ///
    /// <returns>
    /// true if the column was retrieved or added.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetOrAddTableColumn
    (
        ListObject table,
        String columnName,
        Double columnWidthChars,
        String columnStyle,
        out ListColumn listColumn,
        out Range tableColumnData,
        out Object [,] tableColumnDataValues
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Debug.Assert(columnWidthChars == AutoColumnWidth ||
            columnWidthChars >= 0);

        tableColumnData = null;
        tableColumnDataValues = null;

        return (
            TryGetOrAddTableColumn(table, columnName, columnWidthChars,
                columnStyle, out listColumn)
            &&
            TryGetTableColumnDataAndValues(table, columnName,
                out tableColumnData, out tableColumnDataValues)
            );
    }

    //*************************************************************************
    //  Method: TryGetTableColumnDataAndValues()
    //
    /// <summary>
    /// Attempts to get the data range and values of one column of a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the column data range and values from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get data range and values for.
    /// </param>
    ///
    /// <param name="tableColumnData">
    /// Where the column data range gets stored if true is returned.  The data
    /// range includes only that part of the column within the table's data
    /// body range.  This excludes any header or totals row.
    /// </param>
    ///
    /// <param name="tableColumnDataValues">
    /// Where the values for <paramref name="tableColumnData" /> get stored if
    /// true is returned.  The array has one-based dimensions, so the Object
    /// corresponding to the first cell in the range is at [1,1].
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="table" /> contains a column named <paramref
    /// name="columnName" />, the column's data range is stored at <paramref
    /// name="tableColumnData" />, the values for the data range are stored at
    /// <paramref name="tableColumnDataValues" />, and true is returned.  false
    /// is returned otherwise.
    ///
    /// <para>
    /// This method hasn't been tested with a totals row.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTableColumnDataAndValues
    (
        ListObject table,
        String columnName,
        out Range tableColumnData,
        out Object [,] tableColumnDataValues
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        tableColumnData = null;
        tableColumnDataValues = null;

        if ( !ExcelTableUtil.TryGetTableColumnData(table, columnName,
            out tableColumnData) )
        {
            return (false);
        }

        tableColumnDataValues = ExcelUtil.GetRangeValues(tableColumnData);

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetUniqueTableColumnStringValues()
    //
    /// <overloads>
    /// Attempts to get the unique string values from one column of a table.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get the unique string values from one column of a table,
    /// given table information.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the table from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table to remove rows from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get the unique string values from.
    /// </param>
    ///
    /// <param name="uniqueTableColumnStringValues">
    /// Where the unique, non-empty string values get stored if true is
    /// returned.  The collection can be empty but is never null.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If the table contains a column named <paramref name="columnName" />,
    /// each unique unique string value in the column is stored at <paramref
    /// name="uniqueTableColumnStringValues" /> and true is returned.  false is
    /// returned otherwise.
    ///
    /// <para>
    /// This method hasn't been tested with a totals row.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetUniqueTableColumnStringValues
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName,
        String columnName,
        out ICollection<String> uniqueTableColumnStringValues
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        uniqueTableColumnStringValues = null;

        ListObject oTable;

        return (
            ExcelTableUtil.TryGetTable(workbook, worksheetName, tableName,
                out oTable)
            &&
            ExcelTableUtil.TryGetUniqueTableColumnStringValues(oTable,
                columnName, out uniqueTableColumnStringValues)
            );
    }

    //*************************************************************************
    //  Method: TryGetUniqueTableColumnStringValues()
    //
    /// <summary>
    /// Attempts to get the unique string values from one column of a table,
    /// given the table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the unique string values from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get the unique string values from.
    /// </param>
    ///
    /// <param name="uniqueTableColumnStringValues">
    /// Where the unique, non-empty string values get stored if true is
    /// returned.  The collection can be empty but is never null.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="table" /> contains a column named <paramref
    /// name="columnName" />, each unique unique string value in the column is
    /// stored at <paramref name="uniqueTableColumnStringValues" /> and true is
    /// returned.  false is returned otherwise.
    ///
    /// <para>
    /// This method hasn't been tested with a totals row.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetUniqueTableColumnStringValues
    (
        ListObject table,
        String columnName,
        out ICollection<String> uniqueTableColumnStringValues
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        uniqueTableColumnStringValues = null;
        Range oTableColumnData;
        Object [,] aoTableColumnDataValues;

        if ( !TryGetTableColumnDataAndValues(table, columnName,
            out oTableColumnData, out aoTableColumnDataValues) )
        {
            return (false);
        }

        oTableColumnData = null;

        HashSet<String> oUniqueTableColumnStringValues = new HashSet<String>();
        uniqueTableColumnStringValues = oUniqueTableColumnStringValues;
        Int32 iRows = aoTableColumnDataValues.GetUpperBound(0);

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            String sNonEmptyString;

            if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoTableColumnDataValues,
                iRowOneBased, 1, out sNonEmptyString) )
            {
                oUniqueTableColumnStringValues.Add(sNonEmptyString);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetTableColumnData()
    //
    /// <overloads>
    /// Attempts to get the data range of one column of a table.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get the data range of one column of a table given a table
    /// and column name.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the column data range from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get data range for.
    /// </param>
    ///
    /// <param name="tableColumnData">
    /// Where the column data range gets stored if true is returned.  The data
    /// range includes only that part of the column within the table's data
    /// body range.  This excludes any header or totals row.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="table" /> contains a column named <paramref
    /// name="columnName" />, the column's data range is stored at <paramref
    /// name="tableColumnData" /> and true is returned.  false is returned
    /// otherwise.
    ///
    /// <para>
    /// This method hasn't been tested with a totals row.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTableColumnData
    (
        ListObject table,
        String columnName,
        out Range tableColumnData
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        tableColumnData = null;

        ListColumn oColumn;

        return (
            TryGetTableColumn(table, columnName, out oColumn)
            &&
            TryGetTableColumnData(oColumn, out tableColumnData)
            );
    }

    //*************************************************************************
    //  Method: TryGetTableColumnData()
    //
    /// <summary>
    /// Attempts to get the data range of one column of a table given the
    /// column.
    /// </summary>
    ///
    /// <param name="column">
    /// Column to get the data range from.
    /// </param>
    ///
    /// <param name="tableColumnData">
    /// Where the column data range gets stored if true is returned.  The data
    /// range includes only that part of the column within the table's data
    /// body range.  This excludes any header or totals row.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method hasn't been tested with a totals row.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTableColumnData
    (
        ListColumn column,
        out Range tableColumnData
    )
    {
        Debug.Assert(column != null);

        tableColumnData = null;

        Debug.Assert(column.Parent is ListObject);

        ListObject oTable = (ListObject)column.Parent;

        Range oDataBodyRange = oTable.DataBodyRange;

        if (oDataBodyRange == null)
        {
            // This happens when the user deletes the first data row of a one-
            // row table.  It looks like an empty row is there, but the
            // DataBodyRange is actually null.

            Int32 iRow;

            // Is there a header row?

            Range oRangeToUse = oTable.HeaderRowRange;

            if (oRangeToUse != null)
            {
                // Yes.  Use the row after the header row.

                iRow = oRangeToUse.Row + 1;
            }
            else
            {
                // No.  Use the first row of the table.

                oRangeToUse = oTable.Range;

                iRow = oRangeToUse.Row;
            }

            Debug.Assert(oTable.Parent is Worksheet);

            Worksheet oWorksheet = (Worksheet)oTable.Parent;

            oDataBodyRange = oWorksheet.get_Range(

                (Range)oWorksheet.Cells[iRow, oRangeToUse.Column],

                (Range)oWorksheet.Cells[iRow,
                    oRangeToUse.Column + oRangeToUse.Columns.Count - 1]
                );
        }

        return ( ExcelUtil.TryIntersectRanges(oDataBodyRange, column.Range,
            out tableColumnData) );
    }

    //*************************************************************************
    //  Method: TryGetVisibleTableColumnData()
    //
    /// <summary>
    /// Attempts to get the visible data range of one column of a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the visible column data range from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to get visible data range for.
    /// </param>
    ///
    /// <param name="visibleTableColumnData">
    /// Where the visible column data range gets stored if true is returned.
    /// The data range includes only that part of the column within the table's
    /// data body range.  This excludes any header or totals row.  The range
    /// may consist of multiple areas.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="table" /> contains a column named <paramref
    /// name="columnName" />, the column's visible data range is stored at
    /// <paramref name="visibleTableColumnData" /> and true is returned.  false
    /// is returned otherwise.
    ///
    /// <para>
    /// This method hasn't been tested with a totals row.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetVisibleTableColumnData
    (
        ListObject table,
        String columnName,
        out Range visibleTableColumnData
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        visibleTableColumnData = null;

        Range oTempRange;

        return (
            TryGetTableColumnData(table, columnName, out oTempRange)
            &&
            ExcelUtil.TryGetVisibleRange(oTempRange,
                out visibleTableColumnData)
            );
    }

    //*************************************************************************
    //  Method: TryClearTableColumnDataContents()
    //
    /// <summary>
    /// Attempts to clear the contents of the data range of one column of a
    /// table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table containing the column.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to clear.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method hasn't been tested with a totals row.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryClearTableColumnDataContents
    (
        ListObject table,
        String columnName
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Range oTableColumnData;

        if ( !TryGetTableColumnData(table, columnName, out oTableColumnData) )
        {
            return (false);
        }

        oTableColumnData.ClearContents();
        return (true);
    }

    //*************************************************************************
    //  Method: TryAddTableColumnWithRowNumbers()
    //
    /// <summary>
    /// Attempts to add a row-number column to the end of a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to add a column to.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to add.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the new column, in characters, or <see
    /// cref="AutoColumnWidth" /> to set the width automatically.
    /// </param>
    ///
    /// <param name="columnStyle">
    /// Style of the new column, or null to apply Excel's normal style.
    /// Sample: "Bad".
    /// </param>
    ///
    /// <param name="listColumn">
    /// Where the added column gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the column was added.
    /// </returns>
    ///
    /// <remarks>
    /// This method adds the specified column, then fills it with the worksheet
    /// row number of each table row.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryAddTableColumnWithRowNumbers
    (
        ListObject table,
        String columnName,
        Double columnWidthChars,
        String columnStyle,
        out ListColumn listColumn
    )
    {
        Debug.Assert(table!= null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Debug.Assert(columnWidthChars == AutoColumnWidth ||
            columnWidthChars >= 0);

        listColumn = null;

        if ( !TryAddTableColumn(table, columnName, columnWidthChars,
            columnStyle, out listColumn) )
        {
            return (false);
        }

        Range oDataBodyRange = listColumn.DataBodyRange;

        if (oDataBodyRange != null)
        {
            // Fill the column with a ROW() formulas.

            oDataBodyRange.set_Value(Missing.Value, "=ROW()");

            // Convert the formulas to static values.

            oDataBodyRange.Copy(Missing.Value);

            ExcelUtil.PasteValues(oDataBodyRange);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryAddTableColumn()
    //
    /// <summary>
    /// Attempts to add a column to the end of a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to add a column to.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to add.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the new column, in characters, or <see
    /// cref="AutoColumnWidth" /> to set the width automatically.
    /// </param>
    ///
    /// <param name="columnStyle">
    /// Style of the new column, or null to apply Excel's normal style.
    /// Sample: "Bad".
    /// </param>
    ///
    /// <param name="listColumn">
    /// Where the added column gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the column was added.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryAddTableColumn
    (
        ListObject table,
        String columnName,
        Double columnWidthChars,
        String columnStyle,
        out ListColumn listColumn
    )
    {
        Debug.Assert(table!= null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );

        Debug.Assert(columnWidthChars == AutoColumnWidth ||
            columnWidthChars >= 0);

        return ( TryAddOrInsertTableColumn(table, columnName, -1,
            columnWidthChars, columnStyle, out listColumn) );
    }

    //*************************************************************************
    //  Method: WrapTableColumnHeader()
    //
    /// <summary>
    /// Turns on word wrap for the header of a table column.
    /// </summary>
    ///
    /// <param name="listColumn">
    /// Column whose header should wrap text.
    /// </param>
    //*************************************************************************

    public static void
    WrapTableColumnHeader
    (
        ListColumn listColumn
    )
    {
        Debug.Assert(listColumn != null);

        ( (Range)listColumn.Range.Cells[1, 1] ).WrapText = true;
    }

    //*************************************************************************
    //  Method: TryInsertTableColumn()
    //
    /// <summary>
    /// Attempts to insert a column into a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to insert a column into.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to insert.
    /// </param>
    ///
    /// <param name="oneBasedColumnIndex">
    /// One-based index of the column after it is inserted.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the new column, in characters, or <see
    /// cref="AutoColumnWidth" /> to set the width automatically.
    /// </param>
    ///
    /// <param name="columnStyle">
    /// Style of the new column, or null to apply Excel's normal style.
    /// Sample: "Bad".
    /// </param>
    ///
    /// <param name="listColumn">
    /// Where the inserted column gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the column was inserted.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryInsertTableColumn
    (
        ListObject table,
        String columnName,
        Int32 oneBasedColumnIndex,
        Double columnWidthChars,
        String columnStyle,
        out ListColumn listColumn
    )
    {
        Debug.Assert(table!= null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(oneBasedColumnIndex >= 1);

        Debug.Assert(columnWidthChars == AutoColumnWidth ||
            columnWidthChars >= 0);

        return ( TryAddOrInsertTableColumn(table, columnName,
            oneBasedColumnIndex, columnWidthChars, columnStyle,
            out listColumn) );
    }

    //*************************************************************************
    //  Method: GetTableColumnNames()
    //
    /// <summary>
    /// Gets the names of the columns in a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to get the column names from.
    /// </param>
    ///
    /// <param name="columnNamesToExclude">
    /// An array of zero or more column names to exclude from the returned
    /// array.  The constant <see cref="NoColumnNames" /> can be used instead
    /// of an empty array.
    /// </param>
    ///
    /// <param name="columnNameBasesToExclude">
    /// An array of zero or more column name bases to exclude from the returned
    /// array.  If the array includes "Custom", for example, then a column
    /// named "Custom 1" will be excluded.  The constant <see
    /// cref="NoColumnNames" /> can be used instead of an empty array.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more column names.
    /// </returns>
    ///
    /// <remarks>
    /// This method returns the names of all the columns in <paramref
    /// name="table" />, except for those names contained in <paramref
    /// name="columnNamesToExclude" /> and those names that start with the
    /// bases contained in <paramref name="columnNameBasesToExclude" />.
    /// </remarks>
    //*************************************************************************

    public static String []
    GetTableColumnNames
    (
        ListObject table,
        String [] columnNamesToExclude,
        String [] columnNameBasesToExclude
    )
    {
        Debug.Assert(table != null);
        Debug.Assert(columnNamesToExclude != null);
        Debug.Assert(columnNameBasesToExclude != null);

        List<String> oTableColumnNames = new List<String>();

        // Loop through the table's columns.

        foreach (ListColumn oColumn in table.ListColumns)
        {
            String sColumnName = oColumn.Name;

            if ( String.IsNullOrEmpty(sColumnName) )
            {
                goto Skip;
            }

            if (Array.IndexOf(columnNamesToExclude, sColumnName) >= 0)
            {
                goto Skip;
            }

            foreach (String sColumnNameBaseToExclude in
                columnNameBasesToExclude)
            {
                if ( sColumnName.StartsWith(sColumnNameBaseToExclude) )
                {
                    goto Skip;
                }
            }

            oTableColumnNames.Add(sColumnName);

            Skip:
            ;
        }

        return ( oTableColumnNames.ToArray() );
    }

    //*************************************************************************
    //  Method: GetTableColumnDecimalPlaces()
    //
    /// <summary>
    /// Gets the number of decimal places displayed in a table column of format
    /// ExcelColumnFormat.Number.
    /// </summary>
    ///
    /// <param name="column">
    /// The table column to check.  The column must be of format
    /// ExcelColumnFormat.Number.
    /// </param>
    ///
    /// <returns>
    /// The number of decimal places displayed in <paramref name="column" />.
    /// </returns>
    ///
    /// <remarks>
    /// The number of decimal places is determined by looking at the format of
    /// the first cell in the column's data range.
    ///
    /// <para>
    /// If <paramref name="column" /> is not of format
    /// ExcelColumnFormat.Number (and this method doesn't explicitly check for
    /// that), 0 is returned.  It's up to the caller to verify the column
    /// format.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Int32
    GetTableColumnDecimalPlaces
    (
        ListColumn column
    )
    {
        Debug.Assert(column != null);

        Range oColumnData = column.DataBodyRange;

        Debug.Assert(oColumnData != null);
        Debug.Assert(oColumnData.Rows.Count > 0);

        // It would be nice if there were a Range.DecimalPlaces property, but
        // there isn't.  All that Excel provides is Range.NumberFormat, which
        // is actually a string that needs to be parsed.  Parsing that is
        // guaranteed to be correct is difficult, because NumberFormat can be
        // simple ("0.00") or complicated ("$#,##0.00_);[Red]($#,##0.00)").  As
        // an approximation that will be correct most of the time (for "0.00",
        // for example), count the characters after the last decimal place.
        //
        // Note: Don't use the Text property and count decimal places in that,
        // because Range.Text can be "###" if the column is too narrow.

        Debug.Assert(oColumnData.Cells[1, 1] is Range);

        Range oFirstDataCell = (Range)oColumnData.Cells[1, 1];

        String sFirstDataCellNumberFormat =
            (String)oFirstDataCell.NumberFormat;

        Int32 iIndexOfLastDecimalPoint =
            sFirstDataCellNumberFormat.LastIndexOf('.');

        if (iIndexOfLastDecimalPoint < 0)
        {
            return (0);
        }

        Int32 iDecimalPlaces =
            sFirstDataCellNumberFormat.Length - iIndexOfLastDecimalPoint - 1;

        Debug.Assert(iDecimalPlaces >= 0);

        return (iDecimalPlaces);
    }

    //*************************************************************************
    //  Method: RemoveTableRowsByStringColumnValues()
    //
    /// <overloads>
    /// Removes the rows in the table that contain one of a collection of
    /// string values in a specified column.
    /// </overloads>
    ///
    /// <summary>
    /// Removes the rows in the table that contain one of a collection of
    /// string values in a specified column, given table information.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the table from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table to remove rows from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to read.
    /// </param>
    ///
    /// <param name="valuesToRemove">
    /// Collection of values to look for.
    /// </param>
    ///
    /// <remarks>
    /// This method removes each row that contains one of the values in
    /// <paramref name="valuesToRemove" /> in the column named <paramref
    /// name="columnName" />.  The values are of type String.
    /// </remarks>
    //*************************************************************************

    public static void
    RemoveTableRowsByStringColumnValues
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName,
        String columnName,
        ICollection<String> valuesToRemove
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(valuesToRemove != null);

        ListObject oTable;

        if ( ExcelTableUtil.TryGetTable(workbook, worksheetName, tableName,
            out oTable) )
        {
            ExcelTableUtil.RemoveTableRowsByStringColumnValues(oTable,
                columnName, valuesToRemove);
        }
    }

    //*************************************************************************
    //  Method: RemoveTableRowsByStringColumnValues()
    //
    /// <summary>
    /// Removes the rows in the table that contain one of a collection of
    /// string values in a specified column, given a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to remove rows from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to read.
    /// </param>
    ///
    /// <param name="valuesToRemove">
    /// Collection of values to look for.
    /// </param>
    ///
    /// <remarks>
    /// This method removes each row that contains one of the values in
    /// <paramref name="valuesToRemove" /> in the column named <paramref
    /// name="columnName" />.  The values are of type String.
    /// </remarks>
    //*************************************************************************

    public static void
    RemoveTableRowsByStringColumnValues
    (
        ListObject table,
        String columnName,
        ICollection<String> valuesToRemove
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(valuesToRemove != null);

        RemoveTableRowsByColumnValues<String>(table, columnName,
            valuesToRemove, ExcelUtil.TryGetNonEmptyStringFromCell);
    }

    //*************************************************************************
    //  Method: RemoveTableRowsByColumnValues()
    //
    /// <summary>
    /// Removes the rows in the table that contain one of a collection of
    /// values in a specified column.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// Type of the values in the specified column.
    /// </typeparam>
    ///
    /// <param name="table">
    /// Table to remove rows from.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to read.
    /// </param>
    ///
    /// <param name="valuesToRemove">
    /// Collection of values to look for.
    /// </param>
    ///
    /// <param name="tryGetValueFromCell">
    /// Method that attempts to get a value of a specified type from a
    /// worksheet cell given an array of cell values read from the worksheet.
    /// </param>
    ///
    /// <remarks>
    /// This method removes each row that contains one of the values in
    /// <paramref name="valuesToRemove" /> in the column named <paramref
    /// name="columnName" />.  The values are of type TValue.
    /// </remarks>
    //*************************************************************************

    public static void
    RemoveTableRowsByColumnValues<TValue>
    (
        ListObject table,
        String columnName,
        ICollection<TValue> valuesToRemove,
        ExcelUtil.TryGetValueFromCell<TValue> tryGetValueFromCell
    )
    {
        Debug.Assert(table != null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(valuesToRemove != null);
        Debug.Assert(tryGetValueFromCell != null);

        Range oTableColumnData;
        Object [,] aoTableColumnDataValues;

        if ( !TryGetTableColumnDataAndValues(table, columnName,
            out oTableColumnData, out aoTableColumnDataValues) )
        {
            return;
        }

        oTableColumnData = null;

        // Store the values to remove in a HashSet for quick lookup.

        HashSet<TValue> oValuesToRemove = new HashSet<TValue>(valuesToRemove);

        Int32 iRows = aoTableColumnDataValues.GetUpperBound(0);
        ListRows oRows = table.ListRows;

        // Work from the bottom of the table up.

        for (Int32 iRowOneBased = iRows; iRowOneBased >= 1; iRowOneBased--)
        {
            TValue tValue;

            if (
                tryGetValueFromCell(aoTableColumnDataValues, iRowOneBased, 1,
                    out tValue)
                &&
                oValuesToRemove.Contains(tValue)
                )
            {
                oRows[iRowOneBased].Delete();
            }
        }
    }

    //*************************************************************************
    //  Method: ClearTable()
    //
    /// <summary>
    /// Clears a table specified by worksheet and table names of all contents.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the table from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table to clear.
    /// </param>
    ///
    /// <remarks>
    /// This method reduces a table to its header row and totals row, if
    /// present, and one data body row.  The data body row contains the
    /// original formatting and data validation, but its contents are cleared.
    ///
    /// <para>
    /// If the specified table doesn't exist, this method does nothing.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    ClearTable
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );

        ListObject oTable;

        if ( ExcelTableUtil.TryGetTable(workbook, worksheetName, tableName,
            out oTable) )
        {
            ExcelTableUtil.ClearTable(oTable);
        }
    }

    //*************************************************************************
    //  Method: ClearTable()
    //
    /// <overloads>
    /// Clears a table of all contents.
    /// </overloads>
    ///
    /// <summary>
    /// Clears a table specified by ListObject of all contents.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to clear.
    /// </param>
    ///
    /// <remarks>
    /// This method reduces a table to its header row and totals row, if
    /// present, and one data body row.  The data body row contains the
    /// original formatting and data validation, but its contents are cleared.
    /// </remarks>
    //*************************************************************************

    public static void
    ClearTable
    (
        ListObject table
    )
    {
        Debug.Assert(table!= null);

        // Warning:
        //
        // A previous version of this method, which was shorter and simpler,
        // corrupted a table if it was called twice on the table.  The
        // corruption didn't become apparent until the workbook was saved and
        // then opened, at which point Excel displayed an error message.  I
        // couldn't figure out the exact cause, so I rewrote the method using
        // a different technique.

        // Get the current row and column information.

        Range oTableRange = table.Range;
        Range oDataBodyRange = table.DataBodyRange;

        Int32 iRowsOld = 0;
        Int32 iFirstRowOneBased = -1;

        Int32 iColumns = oTableRange.Columns.Count;
        Int32 iFirstColumnOneBased = oTableRange.Column;
        Int32 iLastColumnOneBased = iFirstColumnOneBased + iColumns - 1;

        if (oDataBodyRange != null)
        {
            iRowsOld = oDataBodyRange.Rows.Count;
            iFirstRowOneBased = oDataBodyRange.Row;
        }

        // Resize the table to one data row.

        Int32 iRowsNew = 1;

        if (table.HeaderRowRange != null)
        {
            iRowsNew++;
        }

        if (table.TotalsRowRange != null)
        {
            iRowsNew++;
        }

        oTableRange = oTableRange.get_Resize(iRowsNew, iColumns);
        table.Resize(oTableRange);

        Debug.Assert(table.Parent is Worksheet);
        Worksheet oWorksheet = (Worksheet)table.Parent;

        if (iRowsOld > 0)
        {
            // Clear the contents of the first row, but retain the formatting
            // and data validation.

            Debug.Assert(iFirstRowOneBased >= 1);

            Range oFirstRow = (Range)oWorksheet.get_Range(

                (Range)oWorksheet.Cells[iFirstRowOneBased,
                    iFirstColumnOneBased],

                (Range)oWorksheet.Cells[iFirstRowOneBased, iLastColumnOneBased]
                );

            oFirstRow.ClearContents();
        }

        if (iRowsOld > 1)
        {
            // Clear everything from the remaining old rows.

            Range oRemainingOldRows = (Range)oWorksheet.get_Range(

                (Range)oWorksheet.Cells[iFirstRowOneBased + 1,
                    iFirstColumnOneBased],

                (Range)oWorksheet.Cells[iFirstRowOneBased + iRowsOld - 1,
                    iLastColumnOneBased]
                );

            oRemainingOldRows.Clear();
        }
    }

    //*************************************************************************
    //  Method: ClearTables()
    //
    /// <summary>
    /// Clears multiple tables specified by worksheet and table names of all
    /// contents.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the tables from.
    /// </param>
    ///
    /// <param name="worksheetNameTableNamePairs">
    /// One or more pairs of names.  The first name in each pair is the
    /// worksheet name and the second name in each pair is the table name.
    /// </param>
    ///
    /// <remarks>
    /// This method reduces one or more tables to their header row and totals
    /// row, if present, and one data body row.  The data body row contains the
    /// original formatting and data validation, but its contents are cleared.
    ///
    /// <para>
    /// If a specified table doesn't exist, this method skips it.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    ClearTables
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        params String [] worksheetNameTableNamePairs
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(worksheetNameTableNamePairs != null);
        Debug.Assert(worksheetNameTableNamePairs.Length % 2 == 0);

        Int32 iNames = worksheetNameTableNamePairs.Length;

        for (Int32 i = 0; i < iNames; i += 2)
        {
            ClearTable( workbook,
                worksheetNameTableNamePairs[i + 0],
                worksheetNameTableNamePairs[i + 1]
                );
        }
    }

    //*************************************************************************
    //  Method: ClearTableAutoFilters()
    //
    /// <summary>
    /// Clears any AutoFiltering from a table.
    /// </summary>
    ///
    /// <param name="table">
    /// The table to clear AutoFiltering on.
    /// </param>
    //*************************************************************************

    public static void
    ClearTableAutoFilters
    (
        ListObject table
    )
    {
        Debug.Assert(table != null);

        AutoFilter oAutoFilter = table.AutoFilter;

        if (oAutoFilter == null)
        {
            return;
        }

        Filters oFilters = oAutoFilter.Filters;
        Int32 iFilters = oFilters.Count;
        Range oTableRange = table.Range;

        for (Int32 i = 1; i <= iFilters; i++)
        {
            if (oFilters[i].On)
            {
                oTableRange.AutoFilter(i, Missing.Value,
                    XlAutoFilterOperator.xlAnd, Missing.Value, Missing.Value);
            }
        }
    }

    //*************************************************************************
    //  Method: GetTableColumnFormat()
    //
    /// <summary>
    /// Determines the format of a table column.
    /// </summary>
    ///
    /// <param name="column">
    /// The table column to check.
    /// </param>
    ///
    /// <returns>
    /// The column format.  If the column is empty, ExcelColumnFormat.Other is
    /// returned.
    /// </returns>
    //*************************************************************************

    public static ExcelColumnFormat
    GetTableColumnFormat
    (
        ListColumn column
    )
    {
        Debug.Assert(column != null);

        Range oColumnData = column.DataBodyRange;

        Debug.Assert(oColumnData != null);
        Debug.Assert(oColumnData.Rows.Count > 0);

        // Look at the type of the value in the first cell.

        Debug.Assert(oColumnData.Cells[1, 1] is Range);

        Range oFirstDataCell = (Range)oColumnData.Cells[1, 1];
        Object oFirstDataCellValue = oFirstDataCell.get_Value(Missing.Value);

        if (oFirstDataCellValue is DateTime)
        {
            if ( ExcelUtil.CellContainsTime(oFirstDataCell) )
            {
                // Sample: 1/1/2008 3:40 pm.

                return (ExcelColumnFormat.DateAndTime);
            }
            else
            {
                // Sample: 1/1/2008.

                return (ExcelColumnFormat.Date);
            }
        }
        else if (oFirstDataCellValue is Double)
        {
            // Cells formatted as a time are returned as Double.  Another test
            // is required to distinguish times from real Doubles.

            if ( ExcelUtil.CellContainsTime(oFirstDataCell) )
            {
                // Sample: 3:40 pm.

                return (ExcelColumnFormat.Time);
            }
            else
            {
                // Sample: 123.

                return (ExcelColumnFormat.Number);
            }
        }
        else
        {
            return (ExcelColumnFormat.Other);
        }
    }

    //*************************************************************************
    //  Method: GetTableContextCommandBar()
    //
    /// <summary>
    /// Gets the table context CommandBar.
    /// </summary>
    ///
    /// <param name="application">
    /// Excel Application object.
    /// </param>
    ///
    /// <remarks>
    /// The context menu that appears when you right-click a table (ListObject)
    /// cell.
    /// </remarks>
    //*************************************************************************

    public static Microsoft.Office.Core.CommandBar
    GetTableContextCommandBar
    (
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        Debug.Assert(application != null);

        return ( application.CommandBars[TableContextCommandBarName] );
    }

    //*************************************************************************
    //  Method: TryAddOrInsertTableColumn()
    //
    /// <summary>
    /// Attempts to add or insert a column into a table.
    /// </summary>
    ///
    /// <param name="table">
    /// Table to add or insert a column into.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column to add or insert.
    /// </param>
    ///
    /// <param name="oneBasedColumnIndex">
    /// One-based index of the column after it is inserted, or -1 to add the
    /// column to the end of the table.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the new column, in characters, or <see
    /// cref="AutoColumnWidth" /> to set the width automatically.
    /// </param>
    ///
    /// <param name="columnStyle">
    /// Style of the new column, or null to apply Excel's normal style.
    /// Sample: "Bad".
    /// </param>
    ///
    /// <param name="listColumn">
    /// Where the new column gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the column was added or inserted.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryAddOrInsertTableColumn
    (
        ListObject table,
        String columnName,
        Int32 oneBasedColumnIndex,
        Double columnWidthChars,
        String columnStyle,
        out ListColumn listColumn
    )
    {
        Debug.Assert(table!= null);
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(oneBasedColumnIndex == -1 || oneBasedColumnIndex >= 1);

        Debug.Assert(columnWidthChars == AutoColumnWidth ||
            columnWidthChars >= 0);

        listColumn = null;

        Object oPosition;
        ListColumns oColumns = table.ListColumns;
        Int32 iColumns = oColumns.Count;
        Double [] adColumnWidthChars = null;

        if (oneBasedColumnIndex == -1)
        {
            oPosition = Missing.Value;
        }
        else
        {
            oPosition = oneBasedColumnIndex;

            // When inserting a column, Excel messes up the widths of the
            // columns after the insertion point.  Save the widths of those
            // columns.

            if (oneBasedColumnIndex <= iColumns)
            {
                adColumnWidthChars =
                    new Double[iColumns - oneBasedColumnIndex + 1];

                for (Int32 iOneBasedIndex = oneBasedColumnIndex;
                    iOneBasedIndex <= iColumns; iOneBasedIndex++)
                {
                    adColumnWidthChars[iOneBasedIndex - oneBasedColumnIndex] =
                        (Double)oColumns[iOneBasedIndex].Range.ColumnWidth;
                }
            }
        }

        try
        {
            listColumn = oColumns.Add(oPosition);
        }
        catch (COMException oCOMException)
        {
            if (oCOMException.ErrorCode == -2146827284)
            {
                // This can happen, for example, if adding a table column
                // would cause a merged cells to unmerge, the user is asked
                // if he wants to allow the unmerge, and he says no.

                return (false);
            }

            throw;
        }

        // Set various properties on the new column.

        listColumn.Name = columnName;

        Range oColumnRange = listColumn.Range;

        SetColumnWidth(oColumnRange, columnWidthChars);

        oColumnRange.Validation.Delete();

        ExcelUtil.SetRangeStyle(oColumnRange, columnStyle);

        if (adColumnWidthChars != null)
        {
            // Restore the widths of the columns after the insertion point.

            for (Int32 iOneBasedIndex = oneBasedColumnIndex;
                iOneBasedIndex <= iColumns; iOneBasedIndex++)
            {
                oColumns[iOneBasedIndex + 1].Range.ColumnWidth =
                    adColumnWidthChars[iOneBasedIndex - oneBasedColumnIndex];
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SetColumnWidth()
    //
    /// <summary>
    /// Sets the width of a column.
    /// </summary>
    ///
    /// <param name="rangeInColumn">
    /// One or more cells in the column.
    /// </param>
    ///
    /// <param name="columnWidthChars">
    /// Width of the new column, in characters, or <see
    /// cref="AutoColumnWidth" /> to set the width automatically.
    /// </param>
    //*************************************************************************

    public static void
    SetColumnWidth
    (
        Range rangeInColumn,
        Double columnWidthChars
    )
    {
        Debug.Assert(rangeInColumn!= null);

        Debug.Assert(columnWidthChars == AutoColumnWidth ||
            columnWidthChars >= 0);

        Range oEntireColumn = rangeInColumn.EntireColumn;

        if (columnWidthChars == AutoColumnWidth)
        {
            oEntireColumn.AutoFit();
        }
        else
        {
            oEntireColumn.ColumnWidth = columnWidthChars;
        }
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Name of the command bar that appears when you right-click a table
    /// (ListObject) cell.
    ///
    /// Note: In some versions of Excel, using this command bar's ID (which is
    /// 72) instead of its name works properly, and an earlier version of this
    /// code used the ID.  The ID does not work in all versions of Excel,
    /// however, so the name is used instead.  (The latest documentation for
    /// Application.CommandBars now specifies that using a name instead of an
    /// ID is required for shortcut menus.)
    /// </summary>

    public static readonly String TableContextCommandBarName =
        "List Range Popup";
    
    /// <summary>
    /// For methods that take a column width parameter, specify this constant
    /// to automatically set the column width.
    /// </summary>

    public static readonly Double AutoColumnWidth = -1;

    /// <summary>
    /// This constant can be used in place of an empty String array for either
    /// of the column name array arguments to <see
    /// cref="GetTableColumnNames" />.
    /// </summary>

    public static readonly String [] NoColumnNames = new String[0];
}

}
