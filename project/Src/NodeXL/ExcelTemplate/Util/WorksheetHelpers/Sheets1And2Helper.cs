
using System;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheets1And2Helper
//
/// <summary>
/// Helper class used by Sheet1 and Sheet2.
/// </summary>
///
/// <remarks>
/// See the base-class comments for details on how sheet helper classes are
/// used.
/// </remarks>
//*****************************************************************************

public class Sheets1And2Helper : SheetHelper
{
    //*************************************************************************
    //  Constructor: Sheets1And2Helper()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Sheets1And2Helper" />
    /// class.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// The worksheet that owns this object.
    /// </param>
    ///
    /// <param name="table">
    /// The edge or vertex table.
    /// </param>
    //*************************************************************************

    public Sheets1And2Helper
    (
        Microsoft.Office.Tools.Excel.WorksheetBase worksheet,
        Microsoft.Office.Tools.Excel.ListObject table
    )
    : base(worksheet, table)
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: SetVisualAttribute()
    //
    /// <summary>
    /// Sets a visual attribute in the selected rows of the worksheet.
    /// </summary>
    ///
    /// <param name="e">
    /// Event arguments.
    /// </param>
    ///
    /// <param name="selectedRange">
    /// The selected range in the worksheet.
    /// </param>
    ///
    /// <param name="colorColumnName">
    /// Name of the worksheet's color column, or null if the worksheet doesn't
    /// have a color column.
    /// </param>
    ///
    /// <param name="alphaColumnName">
    /// Name of the worksheet's alpha column, or null if the worksheet doesn't
    /// have an alpha column.
    /// </param>
    ///
    /// <remarks>
    /// If the visual attribute specified by <paramref name="e" /> is handled
    /// by this method or its base-class implementation, the visual attribute
    /// is set in the selected rows of the worksheet and e.VisualAttributeSet
    /// is set to true.
    ///
    /// <para>
    /// The worksheet must be active.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public new void
    SetVisualAttribute
    (
        RunSetVisualAttributeCommandEventArgs e,
        Range selectedRange,
        String colorColumnName,
        String alphaColumnName
    )
    {
        Debug.Assert(e != null);
        Debug.Assert(selectedRange != null);
        Debug.Assert( ExcelUtil.WorksheetIsActive(m_oWorksheet.InnerObject) );
        AssertValid();

        base.SetVisualAttribute(e, selectedRange, colorColumnName,
            alphaColumnName);

        if (e.VisualAttributeSet)
        {
            return;
        }

        if (e.VisualAttribute == VisualAttributes.Alpha &&
            alphaColumnName != null)
        {
            AlphaDialog oAlphaDialog = new AlphaDialog();

            if (oAlphaDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelTableUtil.SetVisibleSelectedTableColumnData(
                    m_oTable.InnerObject, selectedRange, alphaColumnName,
                    oAlphaDialog.Alpha);

                e.VisualAttributeSet = true;
            }
        }
    }

    //*************************************************************************
    //  Method: SetColumnDataValues()
    //
    /// <summary>
    /// Sets data in one or more columns.
    /// </summary>
    ///
    /// <param name="aoColumnDataValuePairs">
    /// Pairs of column data/value arrays.  The first object in each pair is
    /// a Range of column data.  The second object is a one-column,
    /// two-dimensional Object array that stores the values to set in the
    /// column, or null if values shouldn't be set in the column.
    /// </param>
    //*************************************************************************

    public void
    SetColumnDataValues
    (
        params Object[] aoColumnDataValuePairs
    )
    {
        Debug.Assert(aoColumnDataValuePairs != null);
        Debug.Assert(aoColumnDataValuePairs.Length % 2 == 0);
        AssertValid();

        Int32 iColumnDataAndValues = aoColumnDataValuePairs.Length;

        for (Int32 i = 0; i < iColumnDataAndValues; i += 2)
        {
            Debug.Assert(aoColumnDataValuePairs[i + 0] is
                Microsoft.Office.Interop.Excel.Range);

            Debug.Assert( aoColumnDataValuePairs[i + 1] is
                Object[,] );

            Microsoft.Office.Interop.Excel.Range oColumnData =
                (Microsoft.Office.Interop.Excel.Range)
                aoColumnDataValuePairs[i + 0];

            Object [,] aoValues = ( Object[,] )aoColumnDataValuePairs[i + 1];

            if (aoValues != null)
            {
                oColumnData.set_Value(Missing.Value, aoValues);
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetRowID()
    //
    /// <summary>
    /// Attempts to get a row ID from an ID column.
    /// </summary>
    ///
    /// <param name="IDValues">
    /// Two-dimensional array of row ID values read from an ID column.  The
    /// array is one column wide.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to read.
    /// </param>
    ///
    /// <param name="rowID">
    /// Where a row ID gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified cell value contains a valid row ID, the row ID is
    /// stored at <paramref name="rowID" />, and true is returned.  false is
    /// returned otherwise.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetRowID
    (
        Object [,] IDValues,
        Int32 rowOneBased,
        out Int32 rowID
    )
    {
        Debug.Assert(IDValues != null);
        Debug.Assert(rowOneBased >= 1);
        AssertValid();

        rowID = Int32.MinValue;

        return ( ExcelUtil.TryGetInt32FromCell(IDValues, rowOneBased, 1,
            out rowID) );
    }

    //*************************************************************************
    //  Method: GetSelectedRowIDs()
    //
    /// <summary>
    /// Gets a collection of row IDs for all rows in the table that have at
    /// least one cell selected.
    /// </summary>
    ///
    /// <returns>
    /// A collection of unique row IDs.  The row IDs are the values stored in
    /// the table's ID column.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// </remarks>
    //*************************************************************************

    public ICollection<Int32>
    GetSelectedRowIDs()
    {
        AssertValid();

        return ( GetSelectedColumnValues<Int32>(CommonTableColumnNames.ID,
            ExcelUtil.TryGetInt32FromCell) );
    }

    //*************************************************************************
    //  Method: SelectTableRowsByRowIDs()
    //
    /// <summary>
    /// Selects the rows in the table that contain one of a collection of row
    /// IDs.
    /// </summary>
    ///
    /// <param name="rowIDsToSelect">
    /// Collection of row IDs to look for.
    /// </param>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated, then
    /// selects each row that contains one of the row IDs in <paramref
    /// name="rowIDsToSelect" />.
    /// </remarks>
    //*************************************************************************

    public void
    SelectTableRowsByRowIDs
    (
        ICollection<Int32> rowIDsToSelect
    )
    {
        Debug.Assert(rowIDsToSelect != null);
        Debug.Assert( ExcelUtil.WorksheetIsActive(m_oWorksheet.InnerObject) );
        AssertValid();

        SelectTableRowsByColumnValues<Int32>(CommonTableColumnNames.ID,
            rowIDsToSelect, ExcelUtil.TryGetInt32FromCell);
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
}

}
