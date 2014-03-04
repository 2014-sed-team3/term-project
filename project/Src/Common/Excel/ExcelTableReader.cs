﻿
using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: ExcelTableReader
//
/// <summary>
/// Reads rows from an Excel table.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetRows" /> to iterate the rows of an Excel table.  Filtered
/// rows are automatically skipped, and the rows are buffered internally to
/// eliminate having to make a slow COM interop call to get each row.
///
/// <para>
/// A collection of the column names in the table is available via the <see
/// cref="ColumnNames" /> property.
/// </para>
///
/// <para>
/// Note that although this class is optimized for reading a table, the <see
/// cref="ExcelTableRow.GetRangeForCell" /> property allows you to write to the
/// cells in a row as well.  However, each such write operation incurs the
/// overhead of a COM call, so this class should not be used when there are
/// hundreds of cells that need to be written to.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ExcelTableReader : Object
{
    //*************************************************************************
    //  Constructor: ExcelTableReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelTableReader" />
    /// class.
    /// </summary>
    ///
    /// <param name="table">
    /// The table to read.  The table must have no hidden columns.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="table" /> has hidden columns, an exception is
    /// thrown.  Use the <see cref="ExcelColumnHider" /> class to temporarily
    /// show all hidden columns if necessary.
    /// </remarks>
    //*************************************************************************

    public ExcelTableReader
    (
        ListObject table
    )
    {
        Debug.Assert(table != null);

        m_oTable = table;

        m_oColumnIndexesOneBased = new Dictionary<String, Int32>();
        ListColumns oColumns = table.ListColumns;
        Int32 iColumns = oColumns.Count;

        for (Int32 i = 1; i <= iColumns; i++)
        {
            String sColumnName = oColumns[i].Name;

            if ( !String.IsNullOrEmpty(sColumnName) )
            {
                m_oColumnIndexesOneBased.Add(sColumnName, i);
            }
        }

        m_oCurrentSubrange = null;
        m_aoCurrentSubrangeValues = null;
        m_iCurrentRowOneBased = Int32.MinValue;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ColumnNames
    //
    /// <summary>
    /// Gets the names of all the columns in the table.
    /// </summary>
    ///
    /// <value>
    /// The names of all the columns in the table.
    /// </value>
    //*************************************************************************

    public ICollection<String>
    ColumnNames
    {
        get
        {
            AssertValid();

            return (m_oColumnIndexesOneBased.Keys);
        }
    }

    //*************************************************************************
    //  Method: GetRows()
    //
    /// <summary>
    /// Enumerates the rows in the table.
    /// </summary>
    ///
    /// <returns>
    /// An enumerable that enumerates the rows in the table.
    /// </returns>
    ///
    /// <remarks>
    /// Filtered rows are automatically skipped.
    /// </remarks>
    //*************************************************************************

    public IEnumerable<ExcelTableRow>
    GetRows()
    {
        AssertValid();

        // Get the visible range.  If the table is filtered, the range may
        // contain multiple areas.

        Range oVisibleTableRange;

        if (!ExcelTableUtil.TryGetVisibleTableRange(m_oTable,
            out oVisibleTableRange) )
        {
            yield break;
        }

        // Loop through the areas, and split each area into subranges if the
        // area contains too many rows.

        ExcelTableRow oExcelTableRow = new ExcelTableRow(this);

        foreach ( Range oSubrange in
            ExcelRangeSplitter.SplitRange(oVisibleTableRange) )
        {
            m_oCurrentSubrange = oSubrange;

            m_aoCurrentSubrangeValues = ExcelUtil.GetRangeValues(
                m_oCurrentSubrange);

            Int32 iRows = m_oCurrentSubrange.Rows.Count;

            for (m_iCurrentRowOneBased = 1; m_iCurrentRowOneBased <= iRows;
                m_iCurrentRowOneBased++)
            {
                // Note that the same ExcelTableRow object is always returned,
                // and the object doesn't know anything about the current row.
                // The current row information is maintained by this class, not
                // by ExcelTableRow, and ExcelTableRow forwards all its method
                // calls to this class.

                yield return (oExcelTableRow);
            }
        }

        m_oCurrentSubrange = null;
        m_aoCurrentSubrangeValues = null;
        m_iCurrentRowOneBased = Int32.MinValue;
    }

    //*************************************************************************
    //  Method: TryGetNonEmptyStringFromCellInCurrentRow()
    //
    /// <summary>
    /// Attempts to get a non-empty string from one of the current row's cells.
    /// </summary>
    ///
    /// <param name="sColumnName">
    /// Name of the cell's column.  The column does not have to exist.
    /// </param>
    ///
    /// <param name="sNonEmptyString">
    /// Where a string gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified column exists and the cell in the current row and the
    /// specified column contains anything besides spaces, the cell value is
    /// trimmed and stored at <paramref name="sNonEmptyString" /> and true is
    /// returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetNonEmptyStringFromCellInCurrentRow
    (
        String sColumnName,
        out String sNonEmptyString
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(m_aoCurrentSubrangeValues != null);

        sNonEmptyString = null;
        Int32 iColumnOneBased;

        if ( m_oColumnIndexesOneBased.TryGetValue(sColumnName,
            out iColumnOneBased) )
        {
            return ( ExcelUtil.TryGetNonEmptyStringFromCell(
                m_aoCurrentSubrangeValues, m_iCurrentRowOneBased,
                iColumnOneBased, out sNonEmptyString) );
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TryGetDoubleFromCellInCurrentRow()
    //
    /// <summary>
    /// Attempts to get a Double from one of the current row's cells.
    /// </summary>
    ///
    /// <param name="sColumnName">
    /// Name of the cell's column.  The column does not have to exist.
    /// </param>
    ///
    /// <param name="dDouble">
    /// Where a Double gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified column exists and the cell in the current row and the
    /// specified column contains a valid Double, the Double is stored at
    /// <paramref name="dDouble" /> and true is returned.  false is returned
    /// otherwise.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetDoubleFromCellInCurrentRow
    (
        String sColumnName,
        out Double dDouble
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );

        dDouble = Double.MinValue;
        Int32 iColumnOneBased;

        if ( m_oColumnIndexesOneBased.TryGetValue(sColumnName,
            out iColumnOneBased) )
        {
            return ( ExcelUtil.TryGetDoubleFromCell(
                m_aoCurrentSubrangeValues, m_iCurrentRowOneBased,
                iColumnOneBased, out dDouble) );
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TryGetInt32FromCellInCurrentRow()
    //
    /// <summary>
    /// Attempts to get an Int32 from one of the current row's cells.
    /// </summary>
    ///
    /// <param name="sColumnName">
    /// Name of the cell's column.  The column does not have to exist.
    /// </param>
    ///
    /// <param name="iInt32">
    /// Where an Int32 gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified column exists and the cell in the current row and the
    /// specified column contains a valid Int32, the Int32 is stored at
    /// <paramref name="iInt32" /> and true is returned.  false is returned
    /// otherwise.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetInt32FromCellInCurrentRow
    (
        String sColumnName,
        out Int32 iInt32
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );

        iInt32 = Int32.MinValue;
        Int32 iColumnOneBased;

        if ( m_oColumnIndexesOneBased.TryGetValue(sColumnName,
            out iColumnOneBased) )
        {
            return ( ExcelUtil.TryGetInt32FromCell(
                m_aoCurrentSubrangeValues, m_iCurrentRowOneBased,
                iColumnOneBased, out iInt32) );
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetRangeForCellInCurrentRow()
    //
    /// <summary>
    /// Gets the one-cell range for a specified column in the current row.
    /// </summary>
    ///
    /// <param name="sColumnName">
    /// Name of the cell's column.  The column must exist.
    /// </param>
    ///
    /// <returns>
    /// The one-cell range for a specified column in the current row.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified column doesn't exist, an exception is thrown.  Use
    /// <see cref="ExcelTableReader.ColumnNames" /> to determine whether a
    /// column exists.
    /// </remarks>
    //*************************************************************************

    protected Range
    GetRangeForCellInCurrentRow
    (
        String sColumnName
    )
    {
        AssertValid();

        Int32 iColumnOneBased;

        if ( !m_oColumnIndexesOneBased.TryGetValue(sColumnName,
            out iColumnOneBased) )
        {
            Debug.Assert(false);

            throw new InvalidOperationException(
                "ExcelTableReader.GetRangeForCellInCurrentRow: No such"
                + " column.");
        }

        return ( (Range)m_oCurrentSubrange.Cells[
            m_iCurrentRowOneBased, iColumnOneBased] );
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
        Debug.Assert(m_oTable != null);
        Debug.Assert(m_oColumnIndexesOneBased != null);

        if (m_oCurrentSubrange == null)
        {
            Debug.Assert(m_aoCurrentSubrangeValues == null);
            Debug.Assert(m_iCurrentRowOneBased == Int32.MinValue);
        }
        else
        {
            Debug.Assert(m_aoCurrentSubrangeValues != null);
            Debug.Assert(m_iCurrentRowOneBased >= 1);
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The table to read.

    protected ListObject m_oTable;

    /// The key is a column name and the value is the one-based index of that
    /// column.  There is one key/value pair for each named column in the
    /// table.

    protected Dictionary<String, Int32> m_oColumnIndexesOneBased;

    /// Subrange most recently read, or null if a GetRows() call is not in
    /// progress.

    protected Range m_oCurrentSubrange;

    /// 2-dimensional array of values in m_oCurrentSubrange, or null if a 
    /// GetRows() call is not in progress.

    protected Object [,] m_aoCurrentSubrangeValues;

    /// One-based index of the current row in m_oCurrentSubrange, or
    /// Int32.MinValue if a GetRows() call is not in progress.

    protected Int32 m_iCurrentRowOneBased;


    //*************************************************************************
    //  Embedded class: ExcelTableRow
    //
    /// <summary>
    /// Represents one row in an Excel table.
    /// </summary>
    ///
    /// <remarks>
    /// Use <see cref="TryGetNonEmptyStringFromCell" /> to get one cell value.
    /// Use <see cref="GetRangeForCell" /> to get the one-cell Range object
    /// for a cell.
    /// </remarks>
    //*************************************************************************

    public class ExcelTableRow : Object
    {
        //*********************************************************************
        //  Constructor: ExcelTableRow()
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelTableRow" />
        /// class.
        /// </summary>
        ///
        /// <param name="excelTableReader">
        /// The table reader object that owns this object.
        /// </param>
        //*********************************************************************

        public ExcelTableRow
        (
            ExcelTableReader excelTableReader
        )
        {
            m_oExcelTableReader = excelTableReader;

            AssertValid();
        }

        //*********************************************************************
        //  Method: TryGetNonEmptyStringFromCell()
        //
        /// <summary>
        /// Attempts to get a non-empty string from one of the row's cells.
        /// </summary>
        ///
        /// <param name="columnName">
        /// Name of the cell's column.  The column does not have to exist.
        /// </param>
        ///
        /// <param name="nonEmptyString">
        /// Where a string gets stored if true is returned.
        /// </param>
        ///
        /// <returns>
        /// true if successful.
        /// </returns>
        ///
        /// <remarks>
        /// If the specified column exists and the cell in the specified column
        /// contains anything besides spaces, the cell value is trimmed and
        /// stored at <paramref name="nonEmptyString" /> and true is returned.
        /// false is returned otherwise.
        /// </remarks>
        //*********************************************************************

        public Boolean
        TryGetNonEmptyStringFromCell
        (
            String columnName,
            out String nonEmptyString
        )
        {
            Debug.Assert( !String.IsNullOrEmpty(columnName) );

            return (
                m_oExcelTableReader.TryGetNonEmptyStringFromCellInCurrentRow(
                    columnName, out nonEmptyString) );
        }

        //*********************************************************************
        //  Method: TryGetDoubleFromCell()
        //
        /// <summary>
        /// Attempts to get a Double from one of the row's cells.
        /// </summary>
        ///
        /// <param name="columnName">
        /// Name of the cell's column.  The column does not have to exist.
        /// </param>
        ///
        /// <param name="theDouble">
        /// Where a Double gets stored if true is returned.  ("double" is a
        /// keyword.)
        /// </param>
        ///
        /// <returns>
        /// true if successful.
        /// </returns>
        ///
        /// <remarks>
        /// If the specified column exists and the cell in the specified column
        /// contains a valid Double, the Double is stored at <paramref
        /// name="theDouble" /> and true is returned.  false is returned
        /// otherwise.
        /// </remarks>
        //*********************************************************************

        public Boolean
        TryGetDoubleFromCell
        (
            String columnName,
            out Double theDouble
        )
        {
            Debug.Assert( !String.IsNullOrEmpty(columnName) );

            return ( m_oExcelTableReader.TryGetDoubleFromCellInCurrentRow(
                columnName, out theDouble) );
        }

        //*********************************************************************
        //  Method: TryGetSingleFromCell()
        //
        /// <summary>
        /// Attempts to get a Single from one of the row's cells.
        /// </summary>
        ///
        /// <param name="columnName">
        /// Name of the cell's column.  The column does not have to exist.
        /// </param>
        ///
        /// <param name="theSingle">
        /// Where a Single gets stored if true is returned.  ("single" is a
        /// keyword.)
        /// </param>
        ///
        /// <returns>
        /// true if successful.
        /// </returns>
        ///
        /// <remarks>
        /// If the specified column exists and the cell in the specified column
        /// contains a valid Single, the Single is stored at <paramref
        /// name="theSingle" /> and true is returned.  false is returned
        /// otherwise.
        /// </remarks>
        //*********************************************************************

        public Boolean
        TryGetSingleFromCell
        (
            String columnName,
            out Single theSingle
        )
        {
            Debug.Assert( !String.IsNullOrEmpty(columnName) );

            theSingle = Single.MinValue;
            Double dDouble;

            if (!TryGetDoubleFromCell(columnName, out dDouble) ||
                dDouble < Single.MinValue || dDouble > Single.MaxValue)
            {
                return (false);
            }

            theSingle = (Single)dDouble;
            return (true);
        }

        //*********************************************************************
        //  Method: TryGetInt32FromCell()
        //
        /// <summary>
        /// Attempts to get an Int32 from one of the row's cells.
        /// </summary>
        ///
        /// <param name="columnName">
        /// Name of the cell's column.  The column does not have to exist.
        /// </param>
        ///
        /// <param name="int32">
        /// Where an Int32 gets stored if true is returned.
        /// </param>
        ///
        /// <returns>
        /// true if successful.
        /// </returns>
        ///
        /// <remarks>
        /// If the specified column exists and the cell in the specified column
        /// contains a valid Int32, the Int32 is stored at <paramref
        /// name="int32" /> and true is returned.  false is returned otherwise.
        /// </remarks>
        //*********************************************************************

        public Boolean
        TryGetInt32FromCell
        (
            String columnName,
            out Int32 int32
        )
        {
            Debug.Assert( !String.IsNullOrEmpty(columnName) );

            return ( m_oExcelTableReader.TryGetInt32FromCellInCurrentRow(
                columnName, out int32) );
        }

        //*********************************************************************
        //  Method: GetRangeForCell()
        //
        /// <summary>
        /// Gets the one-cell range for a specified column in this row.
        /// </summary>
        ///
        /// <param name="columnName">
        /// Name of the cell's column.  The column must exist.
        /// </param>
        ///
        /// <returns>
        /// The one-cell range for a specified column in this row.
        /// </returns>
        ///
        /// <remarks>
        /// If the specified column doesn't exist, an exception is thrown.  Use
        /// <see cref="ExcelTableReader.ColumnNames" /> to determine whether a
        /// column exists.
        /// </remarks>
        //*********************************************************************

        public Range
        GetRangeForCell
        (
            String columnName
        )
        {
            AssertValid();

            return ( m_oExcelTableReader.GetRangeForCellInCurrentRow(
                columnName) );
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
            Debug.Assert(m_oExcelTableReader != null);
        }


        //*********************************************************************
        //  Protected fields
        //*********************************************************************

        /// The table reader object that owns this object.

        protected ExcelTableReader m_oExcelTableReader;
    }
}

}
