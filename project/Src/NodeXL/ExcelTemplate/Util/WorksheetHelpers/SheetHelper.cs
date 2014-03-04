
using System;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SheetHelper
//
/// <summary>
/// Helper class used by several sheet classes.
/// </summary>
///
/// <remarks>
/// Several sheet classes (such as Sheet1 and Sheet2) implement similar
/// functionality.  To avoid duplicate code, the ideal solution would be to
/// have the sheet classes derive from a base class that implements the common
/// code.  The Visual Studio designer makes this difficult, if not impossible,
/// however, because the base class for each sheet
/// (Microsoft.Office.Tools.Excel.Worksheet) is specified in the
/// SheetX.Designer.cs file, and that file is dynamically generated.
/// 
/// <para>
/// As a workaround, the common code is implemented in this helper class, an
/// instance of which is owned by each sheet.  The sheets delegate some of
/// their method calls to the methods in this class.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SheetHelper : Object
{
    //*************************************************************************
    //  Constructor: SheetHelper()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SheetHelper" /> class.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// The worksheet that owns this object.
    /// </param>
    ///
    /// <param name="table">
    /// The NodeXL table in the worksheet.
    /// </param>
    //*************************************************************************

    public SheetHelper
    (
        Microsoft.Office.Tools.Excel.WorksheetBase worksheet,
        Microsoft.Office.Tools.Excel.ListObject table
    )
    {
        m_oWorksheet = worksheet;
        m_oTable = table;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: TableExists
    //
    /// <summary>
    /// Gets a flag indicating whether the NodeXL table still exists.
    /// </summary>
    ///
    /// <value>
    /// true if the NodeXL table still exists.
    /// </value>
    ///
    /// <remarks>
    /// The user can delete the worksheet after the workbook has been read.  If
    /// he has done so, this property returns false.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TableExists
    {
        get
        {
            AssertValid();

            try
            {
                // Most of the ListObject properties and methods throw an
                // exception once the ListObject has been deleted.
                // ListObject.Name is one of them.

                String sName = m_oTable.Name;

                return (true);
            }
            catch (COMException)
            {
                return (false);
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetAllRowIDs()
    //
    /// <summary>
    /// Attempts to get a dictionary containing the row ID for each row in the
    /// table.
    /// </summary>
    ///
    /// <param name="rowIDDictionary">
    /// Where a dictionary gets stored if true is returned.  There is one
    /// dictionary entry for each row in the table that has an ID.  The key is
    /// the row ID stored in the table's ID column and the value is the
    /// one-based row number relative to the worksheet.
    /// </param>
    ///
    /// <returns>
    /// true if the dictionary was obtained, false if there is no ID column.
    /// </returns>
    ///
    /// <remarks>
    /// The returned dictionary includes the IDs of hidden rows.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetAllRowIDs
    (
        out Dictionary<Int32, Int32> rowIDDictionary
    )
    {
        AssertValid();

        return ( TryGetValuesInAllRows<Int32>(CommonTableColumnNames.ID,
            ExcelUtil.TryGetInt32FromCell, out rowIDDictionary) );
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

    public void
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

        if (e.VisualAttribute == VisualAttributes.Color &&
            colorColumnName != null)
        {
            String sColor;

            // Get a color from the user.

            if ( NodeXLWorkbookUtil.TryGetColor(out sColor) )
            {
                ExcelTableUtil.SetVisibleSelectedTableColumnData(
                    m_oTable.InnerObject, selectedRange, colorColumnName,
                    sColor);

                e.VisualAttributeSet = true;
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetSelectedRange()
    //
    /// <summary>
    /// Gets the selected range in the worksheet.
    /// </summary>
    ///
    /// <param name="selectedRange">
    /// Where the selected range in the worksheet gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the worksheet is active and the selected range was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If the worksheet is active and it contains a selected range, that range
    /// gets stored at <paramref name="selectedRange" /> and true is returned.
    /// false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetSelectedRange
    (
        out Range selectedRange
    )
    {
        AssertValid();

        selectedRange = null;

        if (!ExcelUtil.WorksheetIsActive(m_oWorksheet.InnerObject) ||
            !TableExists)
        {
            return (false);
        }

        Object oSelection = m_oWorksheet.Application.Selection;

        if ( oSelection == null || !(oSelection is Range) )
        {
            return (false);
        }

        selectedRange = (Range)oSelection;

        return (true);
    }

    //*************************************************************************
    //  Method: GetSelectedStringColumnValues()
    //
    /// <summary>
    /// Gets a collection of unique string values from one column for all rows
    /// in the table that have at least one selected cell.
    /// </summary>
    ///
    /// <param name="columnName">
    /// Name of the column to read the values from.
    /// </param>
    ///
    /// <returns>
    /// A collection of zero or more string cell values.  The collection can be
    /// empty but is never null.  The collection values are the cell values,
    /// which are guaranteed to be non-null, non-empty, and unique.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// Then, for each row in the table that has at least one selected cell, it
    /// reads the string value from the row's <paramref name="columnName" />
    /// cell and stores it in the returned collection.
    /// </remarks>
    //*************************************************************************

    public ICollection<String>
    GetSelectedStringColumnValues
    (
        String columnName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        AssertValid();

        return ( GetSelectedColumnValues<String>(columnName,
            ExcelUtil.TryGetNonEmptyStringFromCell) );
    }

    //*************************************************************************
    //  Method: GetSelectedColumnValues()
    //
    /// <summary>
    /// Gets a collection of unique values from one column for all rows in the
    /// table that have at least one selected cell.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// Type of the column values to read.
    /// </typeparam>
    ///
    /// <param name="columnName">
    /// Name of the column to read the values from.
    /// </param>
    ///
    /// <param name="tryGetValueFromCell">
    /// Method that attempts to get a value of a specified type from a
    /// worksheet cell given an array of cell values read from the worksheet.
    /// </param>
    ///
    /// <returns>
    /// A collection of zero or more cell values.  The collection can be empty
    /// but is never null.  The collection values are the cell values, which
    /// are guaranteed to be non-null, non-empty, and unique.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// Then, for each row in the table that has at least one selected cell, it
    /// reads the value from the row's <paramref name="columnName" /> cell and
    /// stores it in the returned collection.
    /// </remarks>
    //*************************************************************************

    public ICollection<TValue>
    GetSelectedColumnValues<TValue>
    (
        String columnName,
        ExcelUtil.TryGetValueFromCell<TValue> tryGetValueFromCell
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(tryGetValueFromCell != null);
        AssertValid();

        // Create a HashSet for the selected values.  The HashSet key is the
        // cell value.

        HashSet<TValue> oSelectedValues = new HashSet<TValue>();

        if (!this.TableExists)
        {
            goto Done;
        }

        // The selected range can extend outside the table.  Get the
        // intersection of the table with the selection.  Note that
        // ExcelUtil.TryGetSelectedTableRange() activates the worksheet.

        ListObject oTable;
        Range oSelectedTableRange;

        if (!ExcelTableUtil.TryGetSelectedTableRange(
            (Workbook)m_oWorksheet.Parent, m_oWorksheet.Name,
            m_oTable.Name, out oTable, out oSelectedTableRange) )
        {
            goto Done;
        }

        Range oDataBodyRange = m_oTable.DataBodyRange;

        Debug.Assert(oDataBodyRange != null);

        // Get data for the specified column.  This includes hidden rows but
        // excludes the header row.

        Range oColumnData;
        Object [,] aoColumnValues;

        if (!ExcelTableUtil.TryGetTableColumnDataAndValues(
            m_oTable.InnerObject, columnName, out oColumnData,
            out aoColumnValues) )
        {
            goto Done;
        }

        // Read the column.

        foreach (Range oSelectedTableRangeArea in oSelectedTableRange.Areas)
        {
            Int32 iFirstRowOneBased =
                oSelectedTableRangeArea.Row - oDataBodyRange.Row + 1;

            Int32 iLastRowOneBased =
                iFirstRowOneBased + oSelectedTableRangeArea.Rows.Count - 1;

            for (Int32 iRowOneBased = iFirstRowOneBased;
                iRowOneBased <= iLastRowOneBased; iRowOneBased++)
            {
                TValue tValue;

                if ( tryGetValueFromCell(aoColumnValues, iRowOneBased, 1,
                    out tValue) )
                {
                    oSelectedValues.Add(tValue);
                }
            }
        }

        Done:

        return (oSelectedValues);
    }

    //*************************************************************************
    //  Method: SelectTableRowsByColumnValues()
    //
    /// <summary>
    /// Selects the rows in the table that contain one of a collection of
    /// values in a specified column.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// Type of the values in the specified column.
    /// </typeparam>
    ///
    /// <param name="columnName">
    /// Name of the column to read.
    /// </param>
    ///
    /// <param name="valuesToSelect">
    /// Collection of values to look for.
    /// </param>
    ///
    /// <param name="tryGetValueFromCell">
    /// Method that attempts to get a value of a specified type from a
    /// worksheet cell given an array of cell values read from the worksheet.
    /// </param>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated, then
    /// selects each row that contains one of the values in <paramref
    /// name="valuesToSelect" /> in the column named <paramref
    /// name="columnName" />.  The values are of type TValue.
    ///
    /// <para>
    /// Any row that doesn't contain one of the specified values gets
    /// deselected.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    SelectTableRowsByColumnValues<TValue>
    (
        String columnName,
        ICollection<TValue> valuesToSelect,
        ExcelUtil.TryGetValueFromCell<TValue> tryGetValueFromCell
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(valuesToSelect != null);
        Debug.Assert(tryGetValueFromCell != null);
        AssertValid();

        if (!TableExists)
        {
            return;
        }

        ExcelUtil.ActivateWorksheet(m_oWorksheet.InnerObject);

        Int32 iValuesToSelect = valuesToSelect.Count;

        if (iValuesToSelect == 0)
        {
            // Unselect any currently selected rows.

            m_oTable.HeaderRowRange.Select();

            return;
        }

        // Get a dictionary containing the value of each row in the table.  The
        // key is the value from the specified column and the value is the
        // one-based row number relative to the worksheet.

        Dictionary<TValue, Int32> oValueDictionary;

        if ( !TryGetValuesInAllRows<TValue>(columnName, tryGetValueFromCell,
            out oValueDictionary) )
        {
            return;
        }

        // Build a range address string that is the union of the rows to
        // select.  Sample: "3:3,6:6,12:12".  Excel allows this for an address
        // up to MaximumBuiltRangeAddressLength characters.  (This was
        // determined experimentally.)  Building a union via a string address
        // is much more efficient than creating one range per row and using
        // Application.Union() on all of them.

        StringBuilder oBuiltRangeAddress = new StringBuilder();

        const Int32 MaximumBuiltRangeAddressLength = 250;

        Range oAccumulatedRange = null;

        // The ExcelLocale1033(true) attribute in AssemblyInfo.cs is supposed
        // to make the Excel object model act the same in all locales, so a
        // hard-coded comma should always work as the list separator for a
        // union range address.  That's not the case, though; Excel uses the
        // locale-specified list separator instead.  Is this a bug in the Excel
        // PIAs?  Here is a posting from someone else who found the same
        // problem:
        //
        // http://social.msdn.microsoft.com/Forums/en-US/vsto/thread/
        // 0e4bd7dc-37d3-42ea-9ce4-53b9e5a53719/

        String sListSeparator =
            CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        Int32 i = 0;

        foreach (TValue tValueToSelect in valuesToSelect)
        {
            Int32 iRow;

            if ( oValueDictionary.TryGetValue(tValueToSelect, out iRow) )
            {
                if (oBuiltRangeAddress.Length != 0)
                {
                    oBuiltRangeAddress.Append(sListSeparator);
                }

                oBuiltRangeAddress.Append(String.Format(
                    "{0}:{0}",
                    iRow
                    ) );
            }

            // In the following test, assume that the next appended address
            // would be ",1048576:1048576".

            Int32 iBuiltRangeAddressLength = oBuiltRangeAddress.Length;

            if (
                iBuiltRangeAddressLength + 1 + 7 + 1 + 7 >
                    MaximumBuiltRangeAddressLength
                ||
                (i == iValuesToSelect - 1 && iBuiltRangeAddressLength > 0)
                )
            {
                // Get the range specified by the StringBuilder.

                Range oBuiltRange = m_oWorksheet.InnerObject.get_Range(
                    oBuiltRangeAddress.ToString(), Missing.Value);

                Debug.Assert(oBuiltRange != null);

                // Add it to the total.

                if ( !ExcelUtil.TryUnionRanges(
                    oAccumulatedRange, oBuiltRange, out oAccumulatedRange) )
                {
                    Debug.Assert(false);
                }

                oBuiltRangeAddress.Length = 0;
            }

        i++;
        }

        // Intersect the accumulated range with the table and select the
        // intersection.

        Range oRangeToSelect;

        if ( ExcelUtil.TryIntersectRanges(oAccumulatedRange,
            m_oTable.DataBodyRange, out oRangeToSelect) )
        {
            oRangeToSelect.Select();
        }
    }

    //*************************************************************************
    //  Method: SetLocations()
    //
    /// <summary>
    /// Sets the location in the table for each row in a specified collection.
    /// </summary>
    ///
    /// <typeparam name="TLocationInfo">
    /// Type that stores a row ID and the location to write to the row.
    /// </typeparam>
    ///
    /// <param name="locationInfo">
    /// Collection of <typeparamref name="TLocationInfo" /> objects, one for
    /// each row in the table whose location needs to be set.
    /// </param>
    ///
    /// <param name="graphRectangle">
    /// The rectangle the graph is drawn within.
    /// </param>
    ///
    /// <param name="xColumnName">
    /// The name of the column that contains the x-coordinates.
    /// </param>
    ///
    /// <param name="yColumnName">
    /// The name of the column that contains the y-coordinates.
    /// </param>
    ///
    /// <param name="lockedColumnName">
    /// The name of the column that contains the "locked" values, or null if
    /// the table has no such column or the column should be ignored.
    /// </param>
    ///
    /// <param name="tryGetRowIDAndLocation">
    /// Represents a method that gets a row ID and a location for a row whose
    /// location needs to be set.
    /// </param>
    ///
    /// <remarks>
    /// For each row in the <paramref name="locationInfo" /> collection, this
    /// method writes the specified location to the row.  The row ID and
    /// location to write to the row are obtained by calling the <paramref
    /// name="tryGetRowIDAndLocation" /> method.
    /// </remarks>
    //*************************************************************************

    public void
    SetLocations<TLocationInfo>
    (
        ICollection<TLocationInfo> locationInfo,
        System.Drawing.Rectangle graphRectangle,
        String xColumnName,
        String yColumnName,
        String lockedColumnName,
        TryGetRowIDAndLocation<TLocationInfo> tryGetRowIDAndLocation
    )
    {
        Debug.Assert(locationInfo != null);
        Debug.Assert( !String.IsNullOrEmpty(xColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(yColumnName) );
        Debug.Assert(tryGetRowIDAndLocation != null);
        AssertValid();

        if (locationInfo.Count == 0)
        {
            return;
        }

        // Gather some required information.

        ListObject oTable = m_oTable.InnerObject;
        Range oXColumnData, oYColumnData, oLockedColumnData;
        Object [,] aoLockedColumnValues = null;

        // The key is a row ID and the value is the row's one-based row number
        // relative to the worksheet.

        Dictionary<Int32, Int32> oRowIDDictionary;

        if (
            TryGetAllRowIDs(out oRowIDDictionary)
            &&
            ExcelTableUtil.TryGetTableColumnData(oTable, xColumnName,
                out oXColumnData)
            &&
            ExcelTableUtil.TryGetTableColumnData(oTable, yColumnName,
                out oYColumnData)
            &&
                (
                lockedColumnName == null
                ||
                ExcelTableUtil.TryGetTableColumnDataAndValues(oTable,
                    lockedColumnName, out oLockedColumnData,
                    out aoLockedColumnValues)
                )
            )
        {
            // Activate this worksheet, because writing to an inactive
            // worksheet causes problems with the selection in Excel.

            ExcelActiveWorksheetRestorer oExcelActiveWorksheetRestorer =
                GetExcelActiveWorksheetRestorer();

            ExcelActiveWorksheetState oExcelActiveWorksheetState =
                oExcelActiveWorksheetRestorer.ActivateWorksheet(
                    m_oWorksheet.InnerObject);

            try
            {
                SetLocations<TLocationInfo>(locationInfo, graphRectangle,
                    tryGetRowIDAndLocation, oRowIDDictionary, oXColumnData,
                    oYColumnData, aoLockedColumnValues);
            }
            finally
            {
                oExcelActiveWorksheetRestorer.Restore(
                    oExcelActiveWorksheetState);
            }
        }
    }

    //*************************************************************************
    //  Method: GetExcelActiveWorksheetRestorer()
    //
    /// <summary>
    /// Creates an ExcelActiveWorksheetRestorer object.
    /// </summary>
    ///
    /// <remarks>
    /// The returned object can be used to activate the worksheet and later
    /// restore the previously active worksheet.
    /// </remarks>
    //*************************************************************************

    public ExcelActiveWorksheetRestorer
    GetExcelActiveWorksheetRestorer()
    {
        AssertValid();

        Debug.Assert(m_oWorksheet.InnerObject.Parent is
            Microsoft.Office.Interop.Excel.Workbook);

        return ( new ExcelActiveWorksheetRestorer(
            (Microsoft.Office.Interop.Excel.Workbook)
            m_oWorksheet.InnerObject.Parent)
            );
    }

    //*************************************************************************
    //  Method: TryGetValuesInAllRows()
    //
    /// <summary>
    /// Attempts to get a dictionary containing a column value from each row in
    /// the table.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// Type of the column values to read.
    /// </typeparam>
    ///
    /// <param name="sColumnName">
    /// Name of the column to read the values from.
    /// </param>
    ///
    /// <param name="oTryGetValueFromCell">
    /// Method that attempts to get a value of a specified type from a
    /// worksheet cell given an array of cell values read from the worksheet.
    /// </param>
    ///
    /// <param name="oValueDictionary">
    /// Where a dictionary gets stored if true is returned.  There is one
    /// dictionary entry for each row in the table that has a value in the
    /// specified column.  The dictionary key is the cell value and the
    /// dictionary value is the one-based row number relative to the worksheet.
    /// </param>
    ///
    /// <returns>
    /// true if the dictionary was obtained, false if there is no such column.
    /// </returns>
    ///
    /// <remarks>
    /// The returned dictionary includes the values in hidden rows.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryGetValuesInAllRows<TValue>
    (
        String sColumnName,
        ExcelUtil.TryGetValueFromCell<TValue> oTryGetValueFromCell,
        out Dictionary<TValue, Int32> oValueDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oTryGetValueFromCell != null);
        AssertValid();

        oValueDictionary = null;

        if (!TableExists)
        {
            return (false);
        }

        Range oDataBodyRange = m_oTable.DataBodyRange;

        if (oDataBodyRange == null)
        {
            return (false);
        }

        Range oColumnData;
        Object [,] aoColumnValues;

        // Get the values in the column.  This includes hidden rows but
        // excludes the header row.

        if (!ExcelTableUtil.TryGetTableColumnDataAndValues(
            m_oTable.InnerObject, sColumnName, out oColumnData,
            out aoColumnValues) )
        {
            return (false);
        }

        oValueDictionary = new Dictionary<TValue, Int32>();

        Int32 iDataBodyRangeRow = oDataBodyRange.Row;
        Int32 iRows = oColumnData.Rows.Count;

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            TValue tValue;

            if ( !oTryGetValueFromCell(aoColumnValues, iRowOneBased, 1,
                out tValue) )
            {
                continue;
            }

            oValueDictionary[tValue] = iRowOneBased + iDataBodyRangeRow - 1;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SetLocations()
    //
    /// <summary>
    /// Sets the location in the table for each row in a specified collection.
    /// </summary>
    ///
    /// <typeparam name="TLocationInfo">
    /// Type that stores a row ID and the location to write to the row.
    /// </typeparam>
    ///
    /// <param name="oLocationInfo">
    /// Collection of <typeparamref name="TLocationInfo" /> objects, one for
    /// each row in the table whose location needs to be set.
    /// </param>
    ///
    /// <param name="oGraphRectangle">
    /// The rectangle the graph is drawn within.
    /// </param>
    ///
    /// <param name="oTryGetRowIDAndLocation">
    /// Represents a method that gets a row ID and a location for a row whose
    /// location needs to be set.
    /// </param>
    ///
    /// <param name="oRowIDDictionary">
    /// The key is a row ID and the value is the row's one-based row number
    /// relative to the worksheet.
    /// </param>
    ///
    /// <param name="oXColumnData">
    /// Data range for the x column.
    /// </param>
    ///
    /// <param name="oYColumnData">
    /// Data range for the y column.
    /// </param>
    ///
    /// <param name="aoLockedColumnValues">
    /// Values for the locked column, or null if the table has no such column
    /// or the column should be ignored.
    /// </param>
    //*************************************************************************

    protected void
    SetLocations<TLocationInfo>
    (
        IEnumerable<TLocationInfo> oLocationInfo,
        System.Drawing.Rectangle oGraphRectangle,
        TryGetRowIDAndLocation<TLocationInfo> oTryGetRowIDAndLocation,
        Dictionary<Int32, Int32> oRowIDDictionary,
        Range oXColumnData,
        Range oYColumnData,
        Object [,] aoLockedColumnValues
    )
    {
        Debug.Assert(oLocationInfo != null);
        Debug.Assert(oTryGetRowIDAndLocation != null);
        Debug.Assert(oRowIDDictionary != null);
        Debug.Assert(oXColumnData != null);
        Debug.Assert(oYColumnData != null);
        AssertValid();

        Int32 iRowNumberOneBased;

        Object [,] aoXValues = ExcelUtil.GetRangeValues(oXColumnData);
        Object [,] aoYValues = ExcelUtil.GetRangeValues(oYColumnData);

        // This is the row number of the table's first data row.

        Int32 iDataBodyRangeRowOneBased = oXColumnData.Row;

        // Create an object that converts a location between coordinates used
        // in the NodeXL graph and coordinates used in the worksheet.

        VertexLocationConverter oVertexLocationConverter =
            new VertexLocationConverter(oGraphRectangle);

        foreach (TLocationInfo oOneLocationInfo in oLocationInfo)
        {
            Int32 iRowID;
            PointF oLocationToSet;

            if (
                !oTryGetRowIDAndLocation(oOneLocationInfo, out iRowID,
                    out oLocationToSet)
                ||
                !oRowIDDictionary.TryGetValue(iRowID, out iRowNumberOneBased)
                )
            {
                continue;
            }

            Int32 iRowToWriteOneBased =
                iRowNumberOneBased - iDataBodyRangeRowOneBased + 1;

            if (
                aoLockedColumnValues != null
                &&
                VertexIsLocked(aoLockedColumnValues, iRowToWriteOneBased)
                )
            {
                continue;
            }

            // Convert the location to workbook coordinates before writing it
            // to the X and Y column cells.

            Single fWorkbookX, fWorkbookY;

            oVertexLocationConverter.GraphToWorkbook(oLocationToSet,
                out fWorkbookX, out fWorkbookY);

            aoXValues[iRowToWriteOneBased, 1] = fWorkbookX;
            aoYValues[iRowToWriteOneBased, 1] = fWorkbookY;
        }

        // Write the X and Y columns.

        oXColumnData.set_Value(Missing.Value, aoXValues);
        oYColumnData.set_Value(Missing.Value, aoYValues);
    }

    //*************************************************************************
    //  Method: VertexIsLocked()
    //
    /// <summary>
    /// Returns a flag indicating whether a vertex is locked.
    /// </summary>
    ///
    /// <param name="aoLockedValues">
    /// Values read from the vertex table's lock column.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row index to check.
    /// </param>
    ///
    /// <returns>
    /// true if the vertex on row <paramref name="iRowOneBased" /> is locked.
    /// </returns>
    //*************************************************************************

    private Boolean
    VertexIsLocked
    (
        Object [,] aoLockedValues,
        Int32 iRowOneBased
    )
    {
        Debug.Assert(aoLockedValues != null);
        Debug.Assert(iRowOneBased >= 1);
        AssertValid();

        String sLock;
        Boolean bLock;

        return (
            ExcelUtil.TryGetNonEmptyStringFromCell(aoLockedValues,
                iRowOneBased, 1, out sLock)
            &&
            ( new BooleanConverter() ).TryWorkbookToGraph(sLock, out bLock)
            &&
            bLock
            );
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
        Debug.Assert(m_oWorksheet != null);
        Debug.Assert(m_oTable != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The worksheet that owns this object.

    protected Microsoft.Office.Tools.Excel.WorksheetBase m_oWorksheet;

    /// The NodeXL table in the worksheet.

    protected Microsoft.Office.Tools.Excel.ListObject m_oTable;
}


//*****************************************************************************
//  Delegate: TryGetRowIDAndLocation()
//
/// <summary>
/// Represents a method that gets a row ID and a location for a row whose
/// location needs to be set.
/// </summary>
///
/// <typeparam name="TLocationInfo">
/// Type that stores a row ID and the location to write to the row.
/// </typeparam>
///
/// <param name="locationInfo">
/// A <typeparamref name="TLocationInfo" /> object that stores a row ID and
/// the location to write to the row.
/// </param>
///
/// <param name="rowID">
/// Where the ID of the row whose location needs to be set gets stored if true
/// is returned.
/// </param>
///
/// <param name="locationToSet">
/// Where the location that should be written to the row gets stored if true is
/// returned, in graph coordinates.
/// </param>
///
/// <returns>
/// true if the row ID and location were obtained.
/// </returns>
//*****************************************************************************

public delegate Boolean
TryGetRowIDAndLocation<TLocationInfo>
(
    TLocationInfo locationInfo,
    out Int32 rowID,
    out PointF locationToSet
);

}
