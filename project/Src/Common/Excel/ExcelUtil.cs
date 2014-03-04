
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: ExcelUtil
//
/// <summary>
/// Static utility methods for working with Excel.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class ExcelUtil
{
    //*************************************************************************
    //  Property: SpecialCellsBeingCalled
    //
    /// <summary>
    /// Gets a flag indicating whether a call to Range.SpecialCells() is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if a call to Range.SpecialCells() is in progress.
    /// </value>
    ///
    /// <remarks>
    /// There is a bug in Excel's Range.SpecialCells() method that causes the
    /// Microsoft.Office.Tools.Excel.ListObject.SelectionChange event to fire
    /// if the range has hidden cells when the method is called.  ExcelUtil
    /// sets this property to true during all such calls.  An application can
    /// work around the Excel bug by checking this property during the
    /// SelectionChange event and ignoring the event if the property is true.
    ///
    /// <para>
    /// WARNING: This is not thread-safe and is not a good solution.  The
    /// correct way to do this would be to turn ExcelUtil into a non-static
    /// class and make m_bSpecialCellsBeingCalled an instance member instead of
    /// a static member.
    /// </para>
    ///
    /// <para>
    /// In the ExcelTemplate VSTO project for which this class was originally
    /// written, each workbook gets its own AppDomain, each of which gets its
    /// own copy of ExcelUtil.  Therefore this static implementation works in
    /// that project, but it won't work in the general case.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    SpecialCellsBeingCalled
    {
        get
        {
            return (m_bSpecialCellsBeingCalled);
        }
    }

    //*************************************************************************
    //  Method: ExcelApplicationIsReady()
    //
    /// <summary>
    /// Determines whether the Excel application is ready to accept method
    /// calls.
    /// </summary>
    ///
    /// <param name="application">
    /// Excel Application object.
    /// </param>
    ///
    /// <returns>
    /// true if the Excel application is ready to accept method calls.
    /// </returns>
    //*************************************************************************

    public static Boolean
    ExcelApplicationIsReady
    (
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        Debug.Assert(application != null);

        // Calling into Excel while a cell is in edit mode or while Excel is
        // displaying a modal dialog (see notes below) can raise an exception.
        // There is an Application.Ready property that is supposed to determine
        // whether the application is ready to accept calls, but it always
        // returns true while a cell is in edit mode.  The following workaround
        // was suggested in several online postings.  It checks whether the
        // File,Open command is enabled.  Excel disables this command while a
        // cell is in edit mode.

        return (application.CommandBars.FindControl(
            Missing.Value, 23, Missing.Value, Missing.Value).Enabled);


        // Repro case for the problem with modal dialogs:
        //
        //   1. Open NodeXL workbook Test2.xlsx and read the workbook into
        //      the graph.
        //
        //   2. Open NodeXL workbook Test1.xlsx and read the workbook into
        //      the graph.
        //
        //   3. Close Excel.  Excel asks if you want to save the changes to
        //      Test1.xlsx, which is on top.  Say no.
        //
        //   4. Closing Test1.xlsx uncovers Test2.xlsx, which then redraws
        //      its graph.  This causes TaskPane_GraphDrawn() to run, which
        //      makes various calls into Excel.
        //
        //   5. Excel is busy with "do you want to save..." dialogs, and
        //      at least one Excel call ( Application.Intersection() )
        //      throws a COMException with error code 0x800AC472.
    }

    //*************************************************************************
    //  Method: OpenWorkbook()
    //
    /// <summary>
    /// Opens a workbook using default arguments.
    /// </summary>
    ///
    /// <param name="filePath">
    /// Path to the workbook to open.
    /// </param>
    ///
    /// <param name="application">
    /// Excel Application object.
    /// </param>
    ///
    /// <returns>
    /// A Workbook object for the opened workbook.
    /// </returns>
    //*************************************************************************

    public static Microsoft.Office.Interop.Excel.Workbook
    OpenWorkbook
    (
        String filePath,
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(filePath) );
        Debug.Assert(application != null);

        return ( application.Workbooks.Open(filePath, 1, false, Missing.Value,
            Missing.Value, Missing.Value, false, Missing.Value, Missing.Value,
            false, Missing.Value, Missing.Value, false, true, Missing.Value) );
    }

    //*************************************************************************
    //  Method: SaveWorkbookAs()
    //
    /// <summary>
    /// Saves a workbook to a new file.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to save.
    /// </param>
    ///
    /// <param name="workbookPath">
    /// Full path to the new file.
    /// </param>
    //*************************************************************************

    public static void
    SaveWorkbookAs
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String workbookPath
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(workbookPath) );

        workbook.SaveAs(workbookPath, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value,
            XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value);
    }

    //*************************************************************************
    //  Method: TryGetWorksheet()
    //
    /// <summary>
    /// Attempts to get a worksheet by name.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the worksheet from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet to get.
    /// </param>
    ///
    /// <param name="worksheet">
    /// Where the requested worksheet gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="workbook" /> contains a worksheet named <paramref
    /// name="worksheetName" />, the worksheet is stored at <paramref
    /// name="worksheet" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        out Worksheet worksheet
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );

        worksheet = null;

        Object oSheet;

        try
        {
            oSheet = workbook.Sheets[worksheetName];
        }
        catch (COMException)
        {
            return (false);
        }

        if ( !(oSheet is Worksheet) )
        {
            return (false);
        }

        worksheet = (Worksheet)oSheet;

        return (true);
    }

    //*************************************************************************
    //  Method: TryAddWorksheet()
    //
    /// <summary>
    /// Attempts to add a worksheet to the end of the workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to add a worksheet to.
    /// </param>
    ///
    /// <param name="name">
    /// Name of the new worksheet.
    /// </param>
    ///
    /// <param name="worksheet">
    /// Where the new worksheet gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// The worksheet is added after the workbook's last worksheet.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryAddWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String name,
        out Microsoft.Office.Interop.Excel.Worksheet worksheet
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(name) );

        worksheet = null;

        try
        {
            Sheets oSheets = workbook.Sheets;

            worksheet = (Worksheet)oSheets.Add(Missing.Value,
                oSheets[oSheets.Count], 1, XlSheetType.xlWorksheet);

            worksheet.Name = name;
        }
        catch (COMException)
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetOrAddWorksheet()
    //
    /// <summary>
    /// Attempts to get a worksheet by name or add the worksheet if it doesn't
    /// exist.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the worksheet from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet to get or add.
    /// </param>
    ///
    /// <param name="worksheet">
    /// Where the requested worksheet gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="workbook" /> contains a worksheet named <paramref
    /// name="worksheetName" />, or it doesn't and the worksheet was
    /// successfully added, the worksheet is stored at <paramref
    /// name="worksheet" /> and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetOrAddWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        out Worksheet worksheet
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );

        return (
            TryGetWorksheet(workbook, worksheetName, out worksheet)
            ||
            TryAddWorksheet(workbook, worksheetName, out worksheet)
            );
    }

    //*************************************************************************
    //  Method: TryClearWorksheet()
    //
    /// <summary>
    /// Attempts to clear a worksheet by name.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook to get the worksheet from.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet to clear.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="workbook" /> contains a worksheet named <paramref
    /// name="worksheetName" />, the worksheet is cleared of all content and
    /// formatting and true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryClearWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );

        Microsoft.Office.Interop.Excel.Worksheet oWorksheet;

        if ( TryGetWorksheet(workbook, worksheetName, out oWorksheet) )
        {
            oWorksheet.Cells.Clear();
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: ActivateWorkbook()
    //
    /// <overloads>
    /// Activates a workbook.
    /// </overloads>
    ///
    /// <summary>
    /// Activates a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook to activate.
    /// </param>
    //*************************************************************************

    public static void
    ActivateWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        // See this posting for an explanation of the strange cast:
        //
        // http://blogs.officezealot.com/maarten/archive/2006/01/02/8918.aspx

        ( (Microsoft.Office.Interop.Excel._Workbook)workbook ).Activate();
    }

    //*************************************************************************
    //  Method: ActivateWorksheet()
    //
    /// <overloads>
    /// Activates a worksheet.
    /// </overloads>
    ///
    /// <summary>
    /// Activates a worksheet.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// The worksheet to activate.
    /// </param>
    //*************************************************************************

    public static void
    ActivateWorksheet
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet
    )
    {
        Debug.Assert(worksheet != null);

        // See this posting for an explanation of the strange cast:
        //
        // http://blogs.officezealot.com/maarten/archive/2006/01/02/8918.aspx

        ( (Microsoft.Office.Interop.Excel._Worksheet)worksheet ).Activate();
    }

    //*************************************************************************
    //  Method: ActivateWorksheet()
    //
    /// <summary>
    /// Activates a worksheet that contains a table (ListObject).
    /// </summary>
    ///
    /// <param name="table">
    /// The table whose parent worksheet should be activated.
    /// </param>
    //*************************************************************************

    public static void
    ActivateWorksheet
    (
        ListObject table
    )
    {
        Debug.Assert(table != null);

        Debug.Assert(table.Parent is Worksheet);

        ActivateWorksheet( (Worksheet)table.Parent );
    }

    //*************************************************************************
    //  Method: WorksheetIsActive()
    //
    /// <summary>
    /// Determines whether a worksheet is the active worksheet.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet to test.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="worksheet" /> is the active worksheet in its
    /// workbook.
    /// </returns>
    //*************************************************************************

    public static Boolean
    WorksheetIsActive
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(worksheet.Parent is Workbook);

        return ( ( (Workbook)worksheet.Parent ).ActiveSheet == worksheet );
    }

    //*************************************************************************
    //  Method: GetCellAddress()
    //
    /// <summary>
    /// Gets a cell's address in A1 style.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet containing the cell.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number of the cell.  Sample: 3.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number of the cell.  Sample: 2.
    /// </param>
    ///
    /// <returns>
    /// The A1-style address of the cell.  Sample: "B3".
    /// </returns>
    //*************************************************************************

    public static String
    GetCellAddress
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        Int32 rowOneBased,
        Int32 columnOneBased
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        return ( GetRangeAddress(
            (Range)worksheet.Cells[rowOneBased, columnOneBased] ) );
    }

    //*************************************************************************
    //  Method: SetCellStringValue()
    //
    /// <summary>
    /// Sets a string value on a single worksheet cell.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet containing the cell.
    /// </param>
    ///
    /// <param name="cellAddress">
    /// Address of the cell.  Sample: "C1".
    /// </param>
    ///
    /// <param name="value">
    /// Value to set the cell to.  Can be empty or null.
    /// </param>
    ///
    /// <returns>
    /// The one-cell range.
    /// </returns>
    //*************************************************************************

    public static Range
    SetCellStringValue
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        String cellAddress,
        String value
    )
    {
        Debug.Assert(worksheet != null);

        Range oRange = ExcelUtil.GetRange(worksheet, cellAddress);
        oRange.set_Value(Missing.Value, value);

        return (oRange);
    }

    //*************************************************************************
    //  Method: GetRangeAddress()
    //
    /// <summary>
    /// Gets a Range's address in A1 style.
    /// </summary>
    ///
    /// <param name="range">
    /// Range to get the address of.
    /// </param>
    ///
    /// <returns>
    /// The A1-style address of <paramref name="range" />.  Sample: "A1:B2".
    /// </returns>
    //*************************************************************************

    public static String
    GetRangeAddress
    (
        Range range
    )
    {
        Debug.Assert(range != null);

        return ( range.get_Address(
            false, false, XlReferenceStyle.xlA1, Missing.Value, Missing.Value
            ) );
    }

    //*************************************************************************
    //  Method: GetRangeAddressAbsolute()
    //
    /// <summary>
    /// Gets a Range's address in absolute $A1 style.
    /// </summary>
    ///
    /// <param name="range">
    /// Range to get the absolute address of.
    /// </param>
    ///
    /// <returns>
    /// The absolute $A1-style address of <paramref name="range" />.  Sample:
    /// "$A1:$B2".
    /// </returns>
    //*************************************************************************

    public static String
    GetRangeAddressAbsolute
    (
        Range range
    )
    {
        Debug.Assert(range != null);

        return ( range.get_Address(
            true, true, XlReferenceStyle.xlA1, Missing.Value, Missing.Value
            ) );
    }

    //*************************************************************************
    //  Method: TryGetRange()
    //
    /// <summary>
    /// Attempts to get a range from a worksheet given a range address.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet to get the range from.
    /// </param>
    ///
    /// <param name="rangeAddress">
    /// Address of the range to get.  Can also be a name.  Samples: "C1",
    /// "MyRange".
    /// </param>
    ///
    /// <param name="range">
    /// Where the range gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// If the specified range exists in <paramref name="worksheet" />, the
    /// range is stored at <paramref name="range" /> and true is returned.
    /// false is returned otherwise.
    /// </remarks>
    ///
    /// <seealso cref="GetRange" />
    //*************************************************************************

    public static Boolean
    TryGetRange
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        String rangeAddress,
        out Range range
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert( !String.IsNullOrEmpty(rangeAddress) );

        range = null;

        try
        {
            range = GetRange(worksheet, rangeAddress);
            return (true);
        }
        catch (System.Runtime.InteropServices.COMException)
        {
            return (false);
        }
    }

    //*************************************************************************
    //  Method: GetRange()
    //
    /// <summary>
    /// Gets a range from a worksheet given a range address.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Workbook to get the range from.
    /// </param>
    ///
    /// <param name="rangeAddress">
    /// Address of the range to get.  Can also be a name.  Samples: "C1",
    /// "MyRange".
    /// </param>
    ///
    /// <returns>
    /// The specified range.
    /// </returns>
    ///
    /// <remarks>
    /// This method does not catch any exceptions.
    /// </remarks>
    ///
    /// <seealso cref="TryGetRange" />
    //*************************************************************************

    public static Range
    GetRange
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        String rangeAddress
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert( !String.IsNullOrEmpty(rangeAddress) );

        return ( worksheet.get_Range(rangeAddress, Missing.Value) );
    }

    //*************************************************************************
    //  Method: TryGetVisibleRange()
    //
    /// <summary>
    /// Attempts to reduce a range to visible cells only.
    /// </summary>
    ///
    /// <param name="range">
    /// The range to reduce.
    /// </param>
    ///
    /// <param name="visibleRange">
    /// Where the visible range gets stored if true is returned.  The range
    /// may consist of multiple areas.
    /// </param>
    ///
    /// <returns>
    /// true if the visible range was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="range" /> contains at least one cell that is
    /// visible, the range of visible cells is stored at <paramref
    /// name="visibleRange" /> and true is returned.  false is returned
    /// otherwise.
    ///
    /// <para>
    /// WARNING: Due to an apparent bug in Excel, this method can cause the
    /// Microsoft.Office.Tools.Excel.ListObject.SelectionChange event to fire.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetVisibleRange
    (
        Range range,
        out Range visibleRange
    )
    {
        Debug.Assert(range != null);

        visibleRange = null;

        // WARNING: If the range contains hidden cells, range.SpecialCells()
        // causes the Microsoft.Office.Tools.Excel.ListObject.SelectionChange
        // event to fire.  It shouldn't, but it does.  Allow the application to
        // work around this Excel bug by checking the SpeciealCellsBeingCalled
        // property from within the application's event handler.

        m_bSpecialCellsBeingCalled = true;

        try
        {
            if (range.Rows.Count == 1 && range.Columns.Count == 1)
            {
                // The Range.SpecialCells() call below does not work for single
                // cells.  For example, if range is "B3", Range.SpecialCells()
                // returns "A:Y,AB:XFD".

                if ( (Boolean)range.EntireRow.Hidden ||
                    (Boolean)range.EntireColumn.Hidden)
                {
                    return (false);
                }

                visibleRange = range;
                return (true);
            }

            visibleRange = range.SpecialCells(
                XlCellType.xlCellTypeVisible, Missing.Value);
        }
        catch (COMException)
        {
            // This can definitely occur.

            return (false);
        }
        finally
        {
            m_bSpecialCellsBeingCalled = false;
        }

        // Can a null visibleRange occur as well?  The documentation doesn't
        // say.

        return (visibleRange != null);
    }

    //*************************************************************************
    //  Method: TryGetNonEmptyRangeInWorksheet()
    //
    /// <summary>
    /// Attempts to get the range within a worksheet that is actually used.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet to use.
    /// </param>
    ///
    /// <param name="usedRange">
    /// Where the used range within <paramref name="worksheet" /> is stored if
    /// true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="worksheet" /> contains at least one used cell.
    /// </returns>
    ///
    /// <remarks>
    /// This differs from Range.UsedRange, which includes cells that were once
    /// used but are now empty.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetNonEmptyRangeInWorksheet
    (
        Worksheet worksheet,
        out Range usedRange
    )
    {
        Debug.Assert(worksheet != null);

        usedRange = null;

        Range oEntireWorksheet = (Range)worksheet.get_Range(

            (Range)worksheet.Cells[1, 1],

            (Range)worksheet.Cells[
                worksheet.Rows.Count, worksheet.Columns.Count]
            );

        return ( TryGetNonEmptyRangeInVisibleArea(oEntireWorksheet,
            out usedRange) );
    }

    //*************************************************************************
    //  Method: TryGetNonEmptyRangeInVisibleArea()
    //
    /// <summary>
    /// Attempts to get the range within a single-area visible range that is
    /// actually used.
    /// </summary>
    ///
    /// <param name="visibleArea">
    /// Single-area visible range to use.
    /// </param>
    ///
    /// <param name="usedRange">
    /// Where the used range within <paramref name="visibleArea" /> is stored
    /// if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="visibleArea" /> contains at least one used
    /// cell.
    /// </returns>
    ///
    /// <remarks>
    /// This differs from Range.UsedRange, which includes cells that were once
    /// used but are now empty.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetNonEmptyRangeInVisibleArea
    (
        Range visibleArea,
        out Range usedRange
    )
    {
        Debug.Assert(visibleArea != null);
        Debug.Assert(visibleArea.Areas.Count == 1);

        usedRange = null;

        if (visibleArea.Rows.Count == 1 && visibleArea.Columns.Count == 1)
        {
            // The code below fails for the single-cell case -- iFirstColumn
            // ends up being greater than iLastColumn.  Check the cell
            // manually.

            if (visibleArea.get_Value(Missing.Value) == null)
            {
                return (false);
            }

            usedRange = visibleArea;

            return (true);
        }

        const String WildCard = "*";

        Range oFirstRow = visibleArea.Find(WildCard,
            visibleArea.Cells[visibleArea.Rows.Count, 1],
            XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByRows,
            XlSearchDirection.xlNext, false, false, Missing.Value);

        if (oFirstRow == null)
        {
            return (false);
        }

        Range oFirstColumn = visibleArea.Find(WildCard,
            visibleArea.Cells[1, visibleArea.Columns.Count],
            XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByColumns,
            XlSearchDirection.xlNext, false, false, Missing.Value);

        Range oLastRow = visibleArea.Find(WildCard,
            visibleArea.Cells[1, 1], XlFindLookIn.xlValues,
            XlLookAt.xlPart, XlSearchOrder.xlByRows,
            XlSearchDirection.xlPrevious, false, false, Missing.Value);

        Range oLastColumn = visibleArea.Find(WildCard,
            visibleArea.Cells[1, 1], XlFindLookIn.xlValues,
            XlLookAt.xlPart, XlSearchOrder.xlByColumns,
            XlSearchDirection.xlPrevious, false, false, Missing.Value);

        Debug.Assert(oFirstColumn != null);
        Debug.Assert(oLastRow != null);
        Debug.Assert(oLastColumn != null);

        Int32 iFirstRow = oFirstRow.Row;
        Int32 iFirstColumn = oFirstColumn.Column;
        Int32 iLastRow = oLastRow.Row;
        Int32 iLastColumn = oLastColumn.Column;

        Worksheet oWorksheet = visibleArea.Worksheet;

        usedRange = (Range)oWorksheet.get_Range(
            (Range)oWorksheet.Cells[iFirstRow, iFirstColumn],
            (Range)oWorksheet.Cells[iLastRow, iLastColumn]
            );

        return (true);
    }

    //*************************************************************************
    //  Method: OffsetRange()
    //
    /// <summary>
    /// Offsets a Range by a specified number of rows and columns.
    /// </summary>
    ///
    /// <param name="range">
    /// The range to offset.
    /// </param>
    ///
    /// <param name="rowOffset">
    /// Number of rows to offset the range.  Can be negative, zero, or
    /// positive.
    /// </param>
    ///
    /// <param name="columnOffset">
    /// Number of columns to offset the range.  Can be negative, zero, or
    /// positive.
    /// </param>
    //*************************************************************************

    public static void
    OffsetRange
    (
        ref Range range,
        Int32 rowOffset,
        Int32 columnOffset
    )
    {
        Debug.Assert(range != null);

        range = range.get_Offset(rowOffset, columnOffset);
    }

    //*************************************************************************
    //  Method: ResizeRange()
    //
    /// <summary>
    /// Resizes a Range to a specified number of rows and columns.
    /// </summary>
    ///
    /// <param name="range">
    /// The range to resize.
    /// </param>
    ///
    /// <param name="rows">
    /// Number of rows to resize the range to.  Must be positive.
    /// </param>
    ///
    /// <param name="columns">
    /// Number of columns to resize the range to.  Must be positive.
    /// </param>
    //*************************************************************************

    public static void
    ResizeRange
    (
        ref Range range,
        Int32 rows,
        Int32 columns
    )
    {
        Debug.Assert(range != null);
        Debug.Assert(rows > 0);
        Debug.Assert(columns > 0);

        range = range.get_Resize(rows, columns);
    }

    //*************************************************************************
    //  Method: RemoveFormulaFromValue()
    //
    /// <summary>
    /// Removes any formula from a value that will be written to a cell.
    /// </summary>
    ///
    /// <param name="value">
    /// The value to remove the formula from.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// The value with any formula removed.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="value" /> is a String that starts with "=",
    /// which Excel would interpret as a formula, this method prepends the "="
    /// with a single quote.  This is to avoid a COMException ("Exception from
    /// HRESULT: 0x800A03EC") when the value starts with "=" but isn't actually
    /// a valid formula.
    /// </remarks>
    //*************************************************************************

    public static Object
    RemoveFormulaFromValue
    (
        Object value
    )
    {
        if (value is String)
        {
            String sValue = (String)value;

            if ( sValue.StartsWith("=") )
            {
                value = "'" + sValue;
            }
        }

        return (value);
    }

    //*************************************************************************
    //  Method: LimitStringValueLength()
    //
    /// <summary>
    /// Truncates a String value to the maximum length that can be written to
    /// an Excel cell.
    /// </summary>
    ///
    /// <param name="value">
    /// The value to truncate.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// The truncated value.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="value" /> is a string that exceeds the maximum
    /// length that can be written to an Excel cell, this method truncates the
    /// string and adds ellipses.
    /// </remarks>
    //*************************************************************************

    public static Object
    LimitStringValueLength
    (
        Object value
    )
    {
        // If you write a string longer than about 8,200 character to an Excel
        // cell via Range.set_Value(), a COMException occurs ("Exception from
        // HRESULT: 0x800A03EC").

        if (value is String)
        {
            value = StringUtil.TruncateWithEllipses( (String)value,
                MaximumStringValueLength);
        }

        return (value);
    }

    //*************************************************************************
    //  Method: PasteValues()
    //
    /// <summary>
    /// Pastes the values and number formats stored in the clipboard into a
    /// range.
    /// </summary>
    ///
    /// <param name="range">
    /// Range to paste into.
    /// </param>
    //*************************************************************************

    public static void
    PasteValues
    (
        Range range
    )
    {
        Debug.Assert(range!= null);

        range.PasteSpecial(XlPasteType.xlPasteValuesAndNumberFormats,
            XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
    }

    //*************************************************************************
    //  Method: GetValuesInRow()
    //
    /// <summary>
    /// Gets a specified number of cell values in a row starting at a specified
    /// cell.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet that contains the values to get.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number.
    /// </param>
    ///
    /// <param name="firstColumnOneBased">
    /// One-based column number of the first cell to get.
    /// </param>
    ///
    /// <param name="columns">
    /// Number of cells to get.  Must be greater than zero.
    /// </param>
    ///
    /// <returns>
    /// The cell values.  A value can be null.
    /// </returns>
    //*************************************************************************

    public static Object [,]
    GetValuesInRow
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        Int32 rowOneBased,
        Int32 firstColumnOneBased,
        Int32 columns
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(firstColumnOneBased >= 1);
        Debug.Assert(columns >= 1);

        Range oRange = (Range)worksheet.get_Range(

            (Range)worksheet.Cells[rowOneBased, firstColumnOneBased],

            (Range)worksheet.Cells[rowOneBased,
                firstColumnOneBased + columns - 1]
            );

        return ( ExcelUtil.GetRangeValues(oRange) );
    }

    //*************************************************************************
    //  Method: GetValuesInColumn()
    //
    /// <summary>
    /// Gets a specified number of cell values in a column starting at a
    /// specified cell.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet that contains the values to get.
    /// </param>
    ///
    /// <param name="firstRowOneBased">
    /// One-based row number of the first cell to get.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number.
    /// </param>
    ///
    /// <param name="rows">
    /// Number of cells to get.  Must be greater than zero.
    /// </param>
    ///
    /// <returns>
    /// The cell values.  A value can be null.
    /// </returns>
    //*************************************************************************

    public static Object [,]
    GetValuesInColumn
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        Int32 firstRowOneBased,
        Int32 columnOneBased,
        Int32 rows
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(firstRowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);
        Debug.Assert(rows >= 1);

        Range oRange = (Range)worksheet.get_Range(

            (Range)worksheet.Cells[firstRowOneBased, columnOneBased],

            (Range)worksheet.Cells[firstRowOneBased + rows - 1,
                columnOneBased]
            );

        return ( ExcelUtil.GetRangeValues(oRange) );
    }

    //*************************************************************************
    //  Method: TryGetContiguousValuesInRow()
    //
    /// <summary>
    /// Attempts to get the contiguous values in a row starting at a specified
    /// cell.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet that contains the values to get.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number.
    /// </param>
    ///
    /// <param name="firstColumnOneBased">
    /// One-based column number of the first cell to get.
    /// </param>
    ///
    /// <param name="values">
    /// Where the contiguous cell values get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the cell at [rowOneBased, firstColumnOneBased] is not empty.
    /// </returns>
    ///
    /// <remarks>
    /// If the [rowOneBased, firstColumnOneBased] cell is not empty, the values
    /// from that cell and all contiguous cells to the right get stored at
    /// <paramref name="values" /> and true is returned.  false is returned
    /// otherwise.
    ///
    /// <para>
    /// Note that any of the contiguous cells can contain strings consisting of
    /// nothing but spaces.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetContiguousValuesInRow
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        Int32 rowOneBased,
        Int32 firstColumnOneBased,
        out Object [,] values
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(firstColumnOneBased >= 1);

        return ( TryGetContiguousValuesInRowOrColumn(worksheet, rowOneBased,
            firstColumnOneBased, true, out values) );
    }

    //*************************************************************************
    //  Method: TryGetContiguousValuesInColumn()
    //
    /// <summary>
    /// Attempts to get the contiguous values in a column starting at a
    /// specified cell.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet that contains the values to get.
    /// </param>
    ///
    /// <param name="firstRowOneBased">
    /// One-based row number of the first cell to get.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number.
    /// </param>
    ///
    /// <param name="values">
    /// Where the contiguous cell values get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the cell at [firstRowOneBased, columnOneBased] is not empty.
    /// </returns>
    ///
    /// <remarks>
    /// If the [firstRowOneBased, columnOneBased] cell is not empty, the values
    /// from that cell and all contiguous cells below get stored at <paramref
    /// name="values" /> and true is returned.  false is returned otherwise.
    ///
    /// <para>
    /// Note that any of the contiguous cells can contain strings consisting of
    /// nothing but spaces.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetContiguousValuesInColumn
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        Int32 firstRowOneBased,
        Int32 columnOneBased,
        out Object [,] values
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(firstRowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        return ( TryGetContiguousValuesInRowOrColumn(worksheet,
            firstRowOneBased, columnOneBased, false, out values) );
    }

    //*************************************************************************
    //  Method: TryGetLastNonEmptyRow()
    //
    /// <summary>
    /// Attempts to get the one-based row number of the last row in a range
    /// that has a non-empty cell.
    /// </summary>
    ///
    /// <param name="range">
    /// The range to use.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// Where the one-based row number gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the row number was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches <paramref name="range" /> from the bottom up for
    /// the first non-empty cell.  If it finds one, it stores the cell's
    /// one-based row number at <paramref name="rowOneBased" /> and returns
    /// true.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetLastNonEmptyRow
    (
        Range range,
        out Int32 rowOneBased
    )
    {
        Debug.Assert(range != null);

        rowOneBased = Int32.MinValue;

        #if false

        This should really be done with the following code:

        {
        Range oCell = range.Find("*", MissingValue, XlFindLookIn.xlFormulas,
            XlLookAt.xlWhole, XlSearchOrder.xlByRows,
            XlSearchDirection.xlPrevious, true, true, Missing.Value
            );

        if (oCell == null)
        {
            return (false);
        }

        rowOneBased = oCell.Row;
        return (true);
        }

        However, Range.Find() has problems finding things when cells are
        hidden.  If the third parameter is XlFindLookIn.xlValues, it doesn't
        look in hidden cells at all.  If the third parameter is xlFormulas,
        it sometimes, but not always, looks in hidden cells.

        To work around these oddities, use a brute-force technique of reading
        all the cells and searching manually.  This is wasteful and slow, but
        it works.

        #endif

        Object [,] aoValues = GetRangeValues(range);

        Int32 iRows = aoValues.GetUpperBound(0);
        Int32 iColumns = aoValues.GetUpperBound(1);

        for (Int32 iRowOneBased = iRows; iRowOneBased >= 1; iRowOneBased--)
        {
            for (Int32 iColumnOneBased = 1; iColumnOneBased <= iColumns;
                iColumnOneBased++)
            {
                if (aoValues[iRowOneBased, iColumnOneBased] != null)
                {
                    rowOneBased = range.Row + iRowOneBased - 1;

                    return (true);
                }
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TryGetChart()
    //
    /// <summary>
    /// Attempts to get a chart by name.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// The worksheet to get the chart from.
    /// </param>
    ///
    /// <param name="chartName">
    /// Name of the chart embedded in the worksheet.
    /// </param>
    ///
    /// <param name="chart">
    /// Where the chart gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetChart
    (
        Worksheet worksheet,
        String chartName,
        out Chart chart
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert( !String.IsNullOrEmpty(chartName) );

        chart = null;

        ChartObjects oChartObjects =
            (ChartObjects)worksheet.ChartObjects(Type.Missing);

        ChartObject oChartObject;

        try
        {
            oChartObject = (Microsoft.Office.Interop.Excel.ChartObject)
                oChartObjects.Item(chartName);
        }
        catch (ArgumentException)
        {
            return (false);
        }

        chart = oChartObject.Chart;
        return (true);
    }

    //*************************************************************************
    //  Method: GetRangeValues()
    //
    /// <summary>
    /// Gets a range's values as a two-dimensional, one-based array.
    /// </summary>
    ///
    /// <param name="range">
    /// Range to get the values for.
    /// </param>
    ///
    /// <returns>
    /// A two-dimensional array of Objects.  Each dimension is one-based.
    /// </returns>
    ///
    /// <remarks>
    /// This is an alternative to Range.get_value(), which doesn't return a
    /// consistent type.  When a range contains two or more cells,
    /// Range.get_value() returns a two-dimensional array of Objects.  When a
    /// range contains just one cell, however, Range.get_value() returns an
    /// Object.
    ///
    /// <para>
    /// In contrast, this method always returns a two-dimensional array of
    /// Objects.  In the one-cell case, the array contains a single Object.
    /// </para>
    ///
    /// <para>
    /// The returned array has one-based dimensions, so the Object
    /// corresponding to the first cell in the range is at [1,1].
    /// </para>
    /// 
    /// </remarks>
    //*************************************************************************

    public static Object [,]
    GetRangeValues
    (
        Range range
    )
    {
        Debug.Assert(range != null);

        if (range.Rows.Count > 1 || range.Columns.Count > 1)
        {
            return ( (Object[,] )range.get_Value(Missing.Value) );
        }

        Object [,] aoCellValues = GetSingleElement2DArray();

        aoCellValues[1, 1] = range.get_Value(Missing.Value);

        return (aoCellValues);
    }

    //*************************************************************************
    //  Method: SetRangeValues()
    //
    /// <overloads>
    /// Sets the values on a range.
    /// </overloads>
    ///
    /// <summary>
    /// Sets the values on a range without removing any formulas from the
    /// values.
    /// </summary>
    ///
    /// <param name="upperLeftCornerMarker">
    /// The values are set on the parent worksheet starting at this range's
    /// upper-left corner.  Only those cells that correspond to <paramref
    /// name="values" /> are set, so the only requirement for this range is
    /// that it's upper-left corner be in the desired location.  Its size is
    /// unimportant.
    /// </param>
    ///
    /// <param name="values">
    /// The values to set.  The array indexes can be either zero-based or
    /// one-based.
    /// </param>
    ///
    /// <returns>
    /// The Range that was actually set.
    /// </returns>
    ///
    /// <remarks>
    /// This method copies <paramref name="values" /> to the parent worksheet
    /// of the <paramref name="upperLeftCornerMarker" /> range, starting at the 
    /// range's upper-left corner.  If the upper-left corner is B2 and
    /// <paramref name="values" /> is 3 rows by 2 columns, for example, then
    /// the values are copied to B2:C4.
    /// </remarks>
    //*************************************************************************

    public static Range
    SetRangeValues
    (
        Range upperLeftCornerMarker,
        Object [,] values
    )
    {
        Debug.Assert(upperLeftCornerMarker != null);
        Debug.Assert(values != null);

        return ( SetRangeValues(upperLeftCornerMarker, values, false) );
    }

    //*************************************************************************
    //  Method: SetRangeValues()
    //
    /// <summary>
    /// Sets the values on a range after optionally removing any formulas from
    /// the values.
    /// </summary>
    ///
    /// <param name="upperLeftCornerMarker">
    /// The values are set on the parent worksheet starting at this range's
    /// upper-left corner.  Only those cells that correspond to <paramref
    /// name="values" /> are set, so the only requirement for this range is
    /// that it's upper-left corner be in the desired location.  Its size is
    /// unimportant.
    /// </param>
    ///
    /// <param name="values">
    /// The values to set.  The array indexes can be either zero-based or
    /// one-based.
    /// </param>
    ///
    /// <param name="removeFormulasFromValues">
    /// true to remove any formulas from <paramref name="values" /> before
    /// copying the values to the worksheet.
    /// </param>
    ///
    /// <returns>
    /// The Range that was actually set.
    /// </returns>
    ///
    /// <remarks>
    /// This method copies <paramref name="values" /> to the parent worksheet
    /// of the <paramref name="upperLeftCornerMarker" /> range, starting at the 
    /// range's upper-left corner.  If the upper-left corner is B2 and
    /// <paramref name="values" /> is 3 rows by 2 columns, for example, then
    /// the values are copied to B2:C4.
    /// </remarks>
    //*************************************************************************

    public static Range
    SetRangeValues
    (
        Range upperLeftCornerMarker,
        Object [,] values,
        Boolean removeFormulasFromValues
    )
    {
        Debug.Assert(upperLeftCornerMarker != null);
        Debug.Assert(values != null);

        Int32 iStartRow = values.GetLowerBound(0);
        Int32 iEndRow = values.GetUpperBound(0);
        Int32 iStartColumn = values.GetLowerBound(1);
        Int32 iEndColumn = values.GetUpperBound(1);

        if (removeFormulasFromValues)
        {
            for (Int32 i = iStartRow; i <= iEndRow; i++)
            {
                for (Int32 j = iStartColumn; j <= iEndColumn; j++)
                {
                    values[i, j] = RemoveFormulaFromValue( values[i, j] );
                }
            }
        }

        Debug.Assert(upperLeftCornerMarker.Parent is Worksheet);
        Worksheet oWorksheet = (Worksheet)upperLeftCornerMarker.Parent;

        Int32 iRow = upperLeftCornerMarker.Row;
        Int32 iColumn = upperLeftCornerMarker.Column;

        Range oRangeToSet = oWorksheet.get_Range(

            (Range)oWorksheet.Cells[iRow, iColumn],

            (Range)oWorksheet.Cells[
                iRow + iEndRow - iStartRow,
                iColumn + iEndColumn - iStartColumn]
            );

        oRangeToSet.set_Value(Missing.Value, values);

        return (oRangeToSet);
    }

    //*************************************************************************
    //  Method: SetVisibleRangeValue()
    //
    /// <summary>
    /// Sets the values in the visible cells of a range to a specified value.
    /// </summary>
    ///
    /// <param name="range">
    /// Range whose visible cells should be set to a value.
    /// </param>
    ///
    /// <param name="value">
    /// The value to set.
    /// </param>
    ///
    /// <remarks>
    /// This method sets the value of every cell in <paramref name="range" />
    /// to <paramref name="value" />.
    /// </remarks>
    //*************************************************************************

    public static void
    SetVisibleRangeValue
    (
        Range range,
        Object value
    )
    {
        Debug.Assert(range != null);

        Range oVisibleRange;

        if ( !ExcelUtil.TryGetVisibleRange(range, out oVisibleRange) )
        {
            return;
        }

        foreach (Range oVisibleArea in oVisibleRange.Areas)
        {
            oVisibleArea.set_Value(Missing.Value, value);
        }
    }

    //*************************************************************************
    //  Method: SetRangeStyle()
    //
    /// <summary>
    /// Sets the style on a range.
    /// </summary>
    ///
    /// <param name="range">
    /// Range to set the style on.
    /// </param>
    ///
    /// <param name="style">
    /// Style to set, or null to apply Excel's normal style.  Sample: "Bad".
    /// </param>
    ///
    /// <remarks>
    /// This method should be called instead of Range.Style.  If the Style
    /// property is set on a range in an inactive worksheet, Excel messes up
    /// the selection in the range's worksheet, and sometimes other worksheets
    /// as well.  This method works around that bug so that the selection isn't
    /// modified at all.
    /// </remarks>
    //*************************************************************************

    public static void
    SetRangeStyle
    (
        Range range,
        String style
    )
    {
        Debug.Assert(range != null);

        Application oApplication = range.Application;
        Boolean bOldScreenUpdating = false;
        Boolean bSwitchWorksheets = false;

        // Check whether the specified range is in the active worksheet.

        Object oOldActiveSheet = oApplication.ActiveSheet;
        Worksheet oOldActiveWorksheet = null;

        if (oOldActiveSheet != null && oOldActiveSheet is Worksheet)
        {
            oOldActiveWorksheet = (Worksheet)oOldActiveSheet;
            Worksheet oRangeWorksheet = range.Worksheet;

            if (oOldActiveWorksheet != oRangeWorksheet)
            {
                bSwitchWorksheets = true;
                bOldScreenUpdating = oApplication.ScreenUpdating;

                oApplication.ScreenUpdating = false;
                ActivateWorksheet(oRangeWorksheet);
            }
        }

        if (style == null)
        {
            style = "Normal";
        }

        try
        {
            range.Style = style;
        }
        catch (COMException)
        {
            // This can occur if the specified style is missing.
        }

        if (bSwitchWorksheets)
        {
            // Restore the original active worksheet.

            ActivateWorksheet(oOldActiveWorksheet);

            oApplication.ScreenUpdating = bOldScreenUpdating;
        }
    }

    //*************************************************************************
    //  Method: ConvertUrlsToHyperlinks()
    //
    /// <summary>
    /// Converts the URLs found in a range to Excel hyperlinks.
    /// </summary>
    ///
    /// <param name="range">
    /// The range whose URLs should be converted to hyperlinks.
    /// </param>
    ///
    /// <remarks>
    /// For each cell in <paramref name="range" />, if the cell contains a
    /// single URL, the URL is converted to an Excel hyperlink.
    /// </remarks>
    //*************************************************************************

    public static void
    ConvertUrlsToHyperlinks
    (
        Range range
    )
    {
        Debug.Assert(range != null);

        if ( !(range.Parent is Worksheet) )
        {
            return;
        }

        Hyperlinks oHyperlinks = ( (Worksheet)range.Parent ).Hyperlinks;
        Object [,] aoValues = GetRangeValues(range);
        Int32 iRows = range.Rows.Count;
        Int32 iColumns = range.Columns.Count;

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            for (Int32 iColumnOneBased = 1; iColumnOneBased <= iColumns;
                iColumnOneBased++)
            {
                Object oValue = aoValues[iRowOneBased, iColumnOneBased];

                if ( ValueIsHyperlink(oValue) )
                {
                    String sHyperlink = (String)oValue;

                    try
                    {
                        oHyperlinks.Add(
                            range.Cells[iRowOneBased, iColumnOneBased],
                            sHyperlink, Missing.Value, Missing.Value,
                            sHyperlink);
                    }
                    catch (Exception)
                    {
                        // Skip anything Excel says isn't a legitimate
                        // hyperlink: "http://a.com\r\nhttp://b.com", for
                        // example.  It's easier to just catch and ignore the
                        // exception than to try to detect every possible bad
                        // hyperlink.
                        //
                        // COMException and ArgumentException are two of the
                        // exceptions that Excel can throw here, but there may
                        // be others.  This certainly isn't documented, so just
                        // catch everything.
                    }
                }
            }
        }
    }

    //*************************************************************************
    //  Method: ForceCellText()
    //
    /// <summary>
    /// Forces a string to always appear as text when inserted into a cell.
    /// </summary>
    ///
    /// <param name="cellText">
    /// String that should always appear as text.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="cellText" /> with a prepended apostrophe.
    /// </returns>
    ///
    /// <remarks>
    /// When the string "5/1", for example, is programmatically inserted into
    /// an Excel cell, Excel will convert it to a date even if the cell is
    /// formatted as Text.  If the string is prepended with an apostrophe,
    /// Excel will always treat it as text, regardless of the cell format.
    /// </remarks>
    //*************************************************************************

    public static String
    ForceCellText
    (
        String cellText
    )
    {
        Debug.Assert(cellText != null);

        return ("'" + cellText);
    }

    //*************************************************************************
    //  Method: UnforceCellText()
    //
    /// <summary>
    /// Reveses the effect of <see cref="ForceCellText" />.
    /// </summary>
    ///
    /// <param name="cellText">
    /// String that may include a prepended apostrophe.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="cellText" /> with any prepended apostrophe removed.
    /// </returns>
    //*************************************************************************

    public static String
    UnforceCellText
    (
        String cellText
    )
    {
        Debug.Assert(cellText != null);

        if ( cellText.StartsWith("'") )
        {
            cellText = cellText.Substring(1);
        }

        return (cellText);
    }

    //*************************************************************************
    //  Method: GetRangeFormulas()
    //
    /// <summary>
    /// Gets a range's formulas as a two-dimensional, one-based array.
    /// </summary>
    ///
    /// <param name="range">
    /// Range to get the formulas for.
    /// </param>
    ///
    /// <returns>
    /// A two-dimensional array of Objects.  Each dimension is one-based.
    /// </returns>
    ///
    /// <remarks>
    /// This is an alternative to Range.Formula, which doesn't return a
    /// consistent type.  When a range contains two or more cells,
    /// Range.Formula returns a two-dimensional array of Objects.  When a
    /// range contains just one cell, however, Range.Formula() returns an
    /// Object.
    ///
    /// <para>
    /// In contrast, this method always returns a two-dimensional array of
    /// Objects.  In the one-cell case, the array contains a single Object.
    /// </para>
    ///
    /// <para>
    /// The returned array has one-based dimensions, so the Object
    /// corresponding to the first cell in the range is at [1,1].
    /// </para>
    /// 
    /// </remarks>
    //*************************************************************************

    public static Object [,]
    GetRangeFormulas
    (
        Range range
    )
    {
        Debug.Assert(range != null);

        if (range.Rows.Count > 1 || range.Columns.Count > 1)
        {
            return ( (Object[,] )range.Formula );
        }

        Object [,] aoCellFormulas = GetSingleElement2DArray();

        aoCellFormulas[1, 1] = range.Formula;

        return (aoCellFormulas);
    }

    //*************************************************************************
    //  Method: SelectRange()
    //
    /// <summary>
    /// Selects a range.
    /// </summary>
    ///
    /// <param name="range">
    /// The range to select.
    /// </param>
    ///
    /// <remarks>
    /// This method activates the range's worksheet before selecting the
    /// range.
    /// </remarks>
    //*************************************************************************

    public static void
    SelectRange
    (
        Range range
    )
    {
        Debug.Assert(range != null);

        range.Application.Goto(range, false);
    }

    //*************************************************************************
    //  Delegate: TryGetValueFromCell()
    //
    /// <summary>
    /// Represents a method that attempts to get a value of a specified type
    /// from a worksheet cell given an array of cell values read from the
    /// worksheet.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// The type of the value to get.
    /// </typeparam>
    ///
    /// <param name="cellValues">
    /// Two-dimensional array of values read from the worksheet.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to read.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number to read.
    /// </param>
    ///
    /// <param name="cellValue">
    /// Where the value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    public delegate Boolean
    TryGetValueFromCell<TValue>
    (
        Object [,] cellValues,
        Int32 rowOneBased,
        Int32 columnOneBased,
        out TValue cellValue
    );

    //*************************************************************************
    //  Method: TryGetNonEmptyStringFromCell()
    //
    /// <overloads>
    /// Attempts to get a non-empty string from a worksheet cell.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get a non-empty string from a worksheet cell given the
    /// worksheet.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// The worksheet to get the non-empty string from.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to read.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number to read.
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
    /// If the specified cell value contains anything besides spaces, the cell
    /// value is trimmed and stored at <paramref name="nonEmptyString" />, and
    /// true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetNonEmptyStringFromCell
    (
        Worksheet worksheet,
        Int32 rowOneBased,
        Int32 columnOneBased,
        out String nonEmptyString
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        return ( TryGetNonEmptyStringFromCell(
            (Range)worksheet.Cells[rowOneBased, columnOneBased],
            out nonEmptyString)
            );
    }

    //*************************************************************************
    //  Method: TryGetNonEmptyStringFromCell()
    //
    /// <summary>
    /// Attempts to get a non-empty string from a worksheet cell given a
    /// one-cell range from the worksheet.
    /// </summary>
    ///
    /// <param name="oneCellRange">
    /// A range containing one cell.
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
    /// If the specified cell value contains anything besides spaces, the cell
    /// value is trimmed and stored at <paramref name="nonEmptyString" />, and
    /// true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetNonEmptyStringFromCell
    (
        Range oneCellRange,
        out String nonEmptyString
    )
    {
        Debug.Assert(oneCellRange != null);
        Debug.Assert(oneCellRange.Rows.Count == 1);
        Debug.Assert(oneCellRange.Columns.Count == 1);

        nonEmptyString = null;

        return ( TryGetNonEmptyStringFromCell(GetRangeValues(oneCellRange),
            1, 1, out nonEmptyString) );
    }

    //*************************************************************************
    //  Method: TryGetNonEmptyStringFromCell()
    //
    /// <summary>
    /// Attempts to get a non-empty string from a worksheet cell given an array
    /// of cell values read from the worksheet.
    /// </summary>
    ///
    /// <param name="cellValues">
    /// Two-dimensional array of values read from the worksheet.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to read.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number to read.
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
    /// If the specified cell value contains anything besides spaces, the cell
    /// value is trimmed and stored at <paramref name="nonEmptyString" />, and
    /// true is returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetNonEmptyStringFromCell
    (
        Object [,] cellValues,
        Int32 rowOneBased,
        Int32 columnOneBased,
        out String nonEmptyString
    )
    {
        Debug.Assert(cellValues != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        return ( TryGetNonEmptyStringFromCell(cellValues, rowOneBased,
            columnOneBased, true, out nonEmptyString) );
    }

    //*************************************************************************
    //  Method: TryGetNonEmptyStringFromCell()
    //
    /// <summary>
    /// Attempts to get a non-empty string from a worksheet cell given an array
    /// of cell values read from the worksheet, with optional trimming.
    /// </summary>
    ///
    /// <param name="cellValues">
    /// Two-dimensional array of values read from the worksheet.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to read.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number to read.
    /// </param>
    ///
    /// <param name="trimCell">
    /// true to trim leading and trailing spaces from the string before
    /// returning it.
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
    /// If the specified cell value contains anything besides spaces, the cell
    /// value is optionally trimmed and stored at <paramref
    /// name="nonEmptyString" />, and true is returned.  false is returned
    /// otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetNonEmptyStringFromCell
    (
        Object [,] cellValues,
        Int32 rowOneBased,
        Int32 columnOneBased,
        Boolean trimCell,
        out String nonEmptyString
    )
    {
        Debug.Assert(cellValues != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        nonEmptyString = null;

        Object oObject = cellValues[rowOneBased, columnOneBased];

        if (oObject == null)
        {
            return (false);
        }

        String sString = oObject.ToString();
        String sTrimmedString = oObject.ToString().Trim();

        if (sTrimmedString.Length == 0)
        {
            return (false);
        }

        nonEmptyString = trimCell ? sTrimmedString : sString;

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetInt32FromCell()
    //
    /// <summary>
    /// Attempts to get an Int32 from a worksheet cell.
    /// </summary>
    ///
    /// <param name="cellValues">
    /// Two-dimensional array of values read from the worksheet.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to read.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number to read.
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
    /// If the specified cell value contains a valid Double that can be cast to
    /// an Int32, the Int32 is stored at <paramref name="int32" /> and true is
    /// returned.  false is returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetInt32FromCell
    (
        Object [,] cellValues,
        Int32 rowOneBased,
        Int32 columnOneBased,
        out Int32 int32
    )
    {
        Debug.Assert(cellValues != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        int32 = Int32.MinValue;

        Double dDouble;

        if (
            TryGetDoubleFromCell(cellValues, rowOneBased, columnOneBased,
                out dDouble)
            &&
            dDouble <= Int32.MaxValue
            &&
            dDouble >= Int32.MinValue
            )
        {
            Double dInt32 = Math.Truncate(dDouble);

            if (dInt32 == dDouble)
            {
                int32 = (Int32)dInt32;
                return (true);
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TryGetDoubleFromCell()
    //
    /// <summary>
    /// Attempts to get a Double from a worksheet cell.
    /// </summary>
    ///
    /// <param name="cellValues">
    /// Two-dimensional array of values read from the worksheet.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to read.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number to read.
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
    /// If the specified cell value contains a valid Double, the Double is
    /// stored at <paramref name="theDouble" /> and true is returned.  false is
    /// returned otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetDoubleFromCell
    (
        Object [,] cellValues,
        Int32 rowOneBased,
        Int32 columnOneBased,
        out Double theDouble
    )
    {
        Debug.Assert(cellValues != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        theDouble = Double.MinValue;

        Object oObject = cellValues[rowOneBased, columnOneBased];

        if (oObject is Double)
        {
            theDouble = (Double)oObject;
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: CellContainsFormula()
    //
    /// <summary>
    /// Determines whether a cell contains a formula.
    /// </summary>
    ///
    /// <param name="cellValues">
    /// A two-dimensional array of Objects.  Each dimension is one-based.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number to check.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number to check.
    /// </param>
    ///
    /// <returns>
    /// true if the cell at the specified location in <paramref
    /// name="cellValues" /> contains a formula.
    /// </returns>
    //*************************************************************************

    public static Boolean
    CellContainsFormula
    (
        Object [,] cellValues,
        Int32 rowOneBased,
        Int32 columnOneBased
    )
    {
        Debug.Assert(cellValues != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        String sNonEmptyString;

        return (
            TryGetNonEmptyStringFromCell(cellValues, rowOneBased,
                columnOneBased, out sNonEmptyString)
            &&
            sNonEmptyString.IndexOf('=') >= 0
            );

    }

    //*************************************************************************
    //  Method: TryIntersectRanges()
    //
    /// <summary>
    /// Attempts to get the intersection of two ranges.
    /// </summary>
    ///
    /// <param name="range1">
    /// First range.  Can be null.
    /// </param>
    ///
    /// <param name="range2">
    /// Range to intersect with <paramref name="range1" />.  Can be null.
    /// </param>
    ///
    /// <param name="intersection">
    /// Where the intersection of <paramref name="range1" /> and <paramref
    /// name="range2" /> gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the intersection is not null.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryIntersectRanges
    (
        Range range1,
        Range range2,
        out Range intersection
    )
    {
        intersection = null;

        if (range1 != null && range2 != null)
        {
            intersection = range1.Application.Intersect(range1, range2,

                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value
                );
        }

        return (intersection != null);
    }

    //*************************************************************************
    //  Method: TryUnionRanges()
    //
    /// <summary>
    /// Attempts to get the union of two ranges.
    /// </summary>
    ///
    /// <param name="range1">
    /// First range.  Can be null.
    /// </param>
    ///
    /// <param name="range2">
    /// Range to union with <paramref name="range1" />.  Can be null.
    /// </param>
    ///
    /// <param name="union">
    /// Where the union of <paramref name="range1" /> and <paramref
    /// name="range2" /> gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the union is not null.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryUnionRanges
    (
        Range range1,
        Range range2,
        out Range union
    )
    {
        union = null;

        if (range1 != null && range2 != null)
        {
            union = range1.Application.Union(range1, range2,

                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value
                );
        }
        else if (range1 != null)
        {
            union = range1;
        }
        else if (range2 != null)
        {
            union = range2;
        }

        return (union != null);
    }

    //*************************************************************************
    //  Method: GetOneBasedRowNumber()
    //
    /// <summary>
    /// Gets the one-based row number from an A1-style cell address.
    /// </summary>
    ///
    /// <param name="cellAddressA1Style">
    /// Cell address from which to get the one-based row number.  Sample cell
    /// address: "B16".
    /// </param>
    ///
    /// <returns>
    /// The one-based row number for <paramref name="cellAddressA1Style" />.
    /// Sample return value: 16.
    /// </returns>
    ///
    /// <seealso cref="GetColumnLetter" />
    /// <seealso cref="ParseCellAddress" />
    //*************************************************************************

    public static Int32
    GetOneBasedRowNumber
    (
        String cellAddressA1Style
    )
    {
        Int32 iOneBasedRowNumber;
        String sColumnLetter;

        ParseCellAddress(cellAddressA1Style, out iOneBasedRowNumber,
            out sColumnLetter);

        return (iOneBasedRowNumber);
    }

    //*************************************************************************
    //  Method: GetColumnLetter()
    //
    /// <summary>
    /// Gets the column letter (or letters) from an A1-style cell address.
    /// </summary>
    ///
    /// <param name="cellAddressA1Style">
    /// Cell address from which to get the column letter.  Sample cell
    /// address: "B16".
    /// </param>
    ///
    /// <returns>
    /// The column letter (or letters) for <paramref
    /// name="cellAddressA1Style" />.  Sample return value: "B".
    /// </returns>
    ///
    /// <seealso cref="GetOneBasedRowNumber" />
    /// <seealso cref="ParseCellAddress" />
    //*************************************************************************

    public static String
    GetColumnLetter
    (
        String cellAddressA1Style
    )
    {
        Int32 iOneBasedRowNumber;
        String sColumnLetter;

        ParseCellAddress(cellAddressA1Style, out iOneBasedRowNumber,
            out sColumnLetter);

        return (sColumnLetter);
    }

    //*************************************************************************
    //  Method: CellContainsTime()
    //
    /// <summary>
    /// Determines whether a cell contains a time.
    /// </summary>
    ///
    /// <param name="cell">
    /// The cell to check.
    /// </param>
    ///
    /// <returns>
    /// true if the cell contains a time.
    /// </returns>
    //*************************************************************************

    public static Boolean
    CellContainsTime
    (
        Range cell
    )
    {
        Debug.Assert(cell != null);

        // The easiest (but not perfect) test is to check the number format for
        // hours.

        Object oNumberFormat = cell.NumberFormat;

        Debug.Assert(oNumberFormat is String);

        return ( ( (String)cell.NumberFormat ).Contains("h") );
    }

    //*************************************************************************
    //  Method: ParseCellAddress()
    //
    /// <summary>
    /// Gets the one-based row number and column letter from an A1-style cell
    /// address.
    /// </summary>
    ///
    /// <param name="cellAddressA1Style">
    /// Cell address to parse.  Sample cell address: "B16".
    /// </param>
    ///
    /// <param name="oneBasedRowNumber">
    /// Where the one-based row number for <paramref
    /// name="cellAddressA1Style" /> gets stored.  Sample value: 16.
    /// </param>
    ///
    /// <param name="columnLetter">
    /// Where the column letter (or letters) for <paramref
    /// name="cellAddressA1Style" /> gets stored.  Sample value: "B".
    /// </param>
    ///
    /// <seealso cref="GetOneBasedRowNumber" />
    /// <seealso cref="GetColumnLetter" />
    //*************************************************************************

    public static void
    ParseCellAddress
    (
        String cellAddressA1Style,
        out Int32 oneBasedRowNumber,
        out String columnLetter
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(cellAddressA1Style) );
        Debug.Assert(cellAddressA1Style.IndexOf(':') == -1);

        oneBasedRowNumber = Int32.MinValue;
        columnLetter = null;

        // Don't use regular expressions here.  They are too slow.

        Int32 iLength = cellAddressA1Style.Length;

        for (Int32 i = 0; i < iLength; i++)
        {
            if ( Char.IsDigit( cellAddressA1Style[i] ) )
            {
                oneBasedRowNumber =
                    Int32.Parse( cellAddressA1Style.Substring(i) );

                columnLetter = cellAddressA1Style.Substring(0, i);

                return;
            }
        }
    }

    //*************************************************************************
    //  Method: TryEvaluateDoubleFunction()
    //
    /// <summary>
    /// Attempts to evaluate an Excel function that should return a Double.
    /// </summary>
    ///
    /// <param name="application">
    /// Excel Application object.
    /// </param>
    ///
    /// <param name="functionCall">
    /// Function call text.  Sample: "=SUM(A:A)".
    /// </param>
    ///
    /// <param name="functionReturn">
    /// Where the Double returned by the function gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the returned a Double, false if it failed and returned an
    /// Int32 error code.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryEvaluateDoubleFunction
    (
        Application application,
        String functionCall,
        out Double functionReturn
    )
    {
        Debug.Assert(application != null);
        Debug.Assert( !String.IsNullOrEmpty(functionCall) );

        functionReturn = Double.MinValue;

        Object oFunctionReturn = application.Evaluate(functionCall);

        // If the function failed, it returned an Int32 error code.  See this
        // post for details on how .NET deals with CVErr values:
        //
        // Dealing with CVErr Values in .NET - Part I: The Problem
        //
        // http://xldennis.wordpress.com/2006/11/22/dealing-with-cverr-values-
        // in-net-%E2%80%93-part-i-the-problem/

        if (oFunctionReturn is Double)
        {
            functionReturn = (Double)oFunctionReturn;
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetSingleElement2DArray()
    //
    /// <summary>
    /// Gets a two-dimensional Object array with one row and one column.
    /// </summary>
    ///
    /// <returns>
    /// A two-dimensional Object array with one row and one column.  Each
    /// dimension is one-based.  The single element in the array is initialized
    /// to null.
    /// </returns>
    //*************************************************************************

    public static Object [,]
    GetSingleElement2DArray()
    {
        return ( GetSingleColumn2DArray(1) );
    }

    //*************************************************************************
    //  Method: GetSingleColumn2DArray()
    //
    /// <summary>
    /// Gets a two-dimensional Object array with N rows and one column.
    /// </summary>
    ///
    /// <param name="rows">
    /// Number of rows to include in the array.  Must be greater than or equal
    /// to zero.
    /// </param>
    ///
    /// <returns>
    /// A two-dimensional Object array with N rows and one column.  Each
    /// dimension is one-based.  The elements in the array are initialized to
    /// null.
    /// </returns>
    //*************************************************************************

    public static Object [,]
    GetSingleColumn2DArray
    (
        Int32 rows
    )
    {
        Debug.Assert(rows >= 0);

        return ( Get2DArray<Object>(rows, 1) );
    }

    //*************************************************************************
    //  Method: GetSingleRow2DArray()
    //
    /// <summary>
    /// Gets a two-dimensional Object array with 1 row and N columns.
    /// </summary>
    ///
    /// <param name="columns">
    /// Number of columns to include in the array.  Must be greater than or
    /// equal to zero.
    /// </param>
    ///
    /// <returns>
    /// A two-dimensional Object array with one row and N columns.  Each
    /// dimension is one-based.  The elements in the array are initialized to
    /// null.
    /// </returns>
    //*************************************************************************

    public static Object [,]
    GetSingleRow2DArray
    (
        Int32 columns
    )
    {
        Debug.Assert(columns >= 0);

        return ( Get2DArray<Object>(1, columns) );
    }

    //*************************************************************************
    //  Method: GetSingleColumn2DStringArray()
    //
    /// <summary>
    /// Gets a two-dimensional String array with N rows and one column.
    /// </summary>
    ///
    /// <param name="rows">
    /// Number of rows to include in the array.  Must be greater than or equal
    /// to zero.
    /// </param>
    ///
    /// <returns>
    /// A two-dimensional String array with N rows and one column.  Each
    /// dimension is one-based.  The elements in the array are initialized to
    /// null.
    /// </returns>
    //*************************************************************************

    public static String [,]
    GetSingleColumn2DStringArray
    (
        Int32 rows
    )
    {
        Debug.Assert(rows >= 0);

        return ( Get2DArray<String>(rows, 1) );
    }

    //*************************************************************************
    //  Method: GetSingleRow2DStringArray()
    //
    /// <summary>
    /// Gets a two-dimensional String array with 1 row and N columns.
    /// </summary>
    ///
    /// <param name="columns">
    /// Number of columns to include in the array.  Must be greater than or
    /// equal to zero.
    /// </param>
    ///
    /// <returns>
    /// A two-dimensional String array with one row and N columns.  Each
    /// dimension is one-based.  The elements in the array are initialized to
    /// null.
    /// </returns>
    //*************************************************************************

    public static String [,]
    GetSingleRow2DStringArray
    (
        Int32 columns
    )
    {
        Debug.Assert(columns >= 0);

        return ( Get2DArray<String>(1, columns) );
    }

    //*************************************************************************
    //  Method: Get2DArray()
    //
    /// <summary>
    /// Gets a two-dimensional array.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// The type of the array values.
    /// </typeparam>
    ///
    /// <param name="rows">
    /// Number of rows to include in the array.  Must be greater than or equal
    /// to zero.
    /// </param>
    ///
    /// <param name="columns">
    /// Number of columns to include in the array.  Must be greater than or
    /// equal to zero.
    /// </param>
    ///
    /// <returns>
    /// A two-dimensional array.  Each dimension is one-based.
    /// </returns>
    //*************************************************************************

    public static TValue [,]
    Get2DArray<TValue>
    (
        Int32 rows,
        Int32 columns
    )
    {
        Debug.Assert(rows >= 0);
        Debug.Assert(columns >= 0);

        return ( ( TValue [,] )Array.CreateInstance(
            typeof(TValue), new Int32[] {rows, columns}, new Int32[] {1,1} ) );
    }

    //*************************************************************************
    //  Method: TryGetContiguousValuesInRowOrColumn()
    //
    /// <summary>
    /// Attempts to get the contiguous values in a row or column starting at a
    /// specified cell.
    /// </summary>
    ///
    /// <param name="worksheet">
    /// Worksheet that contains the values to get.
    /// </param>
    ///
    /// <param name="rowOneBased">
    /// One-based row number of the first cell to get.
    /// </param>
    ///
    /// <param name="columnOneBased">
    /// One-based column number of the first cell to get.
    /// </param>
    ///
    /// <param name="inRow">
    /// true to get the contiguous values in the row, false to get the
    /// contiguous values in the column.
    /// </param>
    ///
    /// <param name="values">
    /// Where the contiguous cell values get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the cell at [rowOneBased, columnOneBased] is not empty.
    /// </returns>
    ///
    /// <remarks>
    /// If the [rowOneBased, columnOneBased] cell is not empty, the values
    /// from that cell and all contiguous cells to the right or below get
    /// stored at <paramref name="values" /> and true is returned.  false is
    /// returned otherwise.
    ///
    /// <para>
    /// Note that any of the contiguous cells can contain strings consisting of
    /// nothing but spaces.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    private static Boolean
    TryGetContiguousValuesInRowOrColumn
    (
        Microsoft.Office.Interop.Excel.Worksheet worksheet,
        Int32 rowOneBased,
        Int32 columnOneBased,
        Boolean inRow,
        out Object [,] values
    )
    {
        Debug.Assert(worksheet != null);
        Debug.Assert(rowOneBased >= 1);
        Debug.Assert(columnOneBased >= 1);

        values = null;

        Range oFirstCell = (Range)worksheet.Cells[rowOneBased, columnOneBased];

        String sTemp;

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(oFirstCell, out sTemp) )
        {
            return (false);
        }

        Range oNextCell = (Range)worksheet.Cells[
            rowOneBased + (inRow ? 0 : 1),
            columnOneBased + (inRow ? 1 : 0)
            ];

        Range oLastCell;

        // If the next cell is empty, Range.get_End() can't be used because it
        // jumps beyond the empty cell, possibly to the end of the worksheet.

        if ( !ExcelUtil.TryGetNonEmptyStringFromCell(oNextCell, out sTemp) )
        {
            oLastCell = oFirstCell;
        }
        else
        {
            oLastCell = oFirstCell.get_End(
                inRow ? XlDirection.xlToRight : XlDirection.xlDown);
        }

        values = ExcelUtil.GetRangeValues(
            worksheet.get_Range(oFirstCell, oLastCell) );

        return (true);
    }

    //*************************************************************************
    //  Method: ValueIsHyperlink()
    //
    /// <summary>
    /// Determines whether a cell value appears to be a hyperlink.
    /// </summary>
    ///
    /// <param name="oValue">
    /// The value to check.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="oValue" /> appears to be a hyperlink.
    /// </returns>
    //*************************************************************************

    private static Boolean
    ValueIsHyperlink
    (
        Object oValue
    )
    {
        if (oValue is String)
        {
            String sValue = (String)oValue;
            Int32 iLength = sValue.Length;

            // Exclude "http://" by checking the length.  This was found in a
            // Twitter GraphML file.  Worksheet.Hyperlinks.Add() doesn't raise
            // an exception for it, but Excel refuses to save the resulting
            // workbook, saying the workbook appears to be corrupted.
            //
            // Exclude "https://" also, for good measure.

            if (
                (sValue.StartsWith("http://") && iLength > 7)
                ||
                (sValue.StartsWith("https://") && iLength > 8)
                )
            {
                if (sValue.IndexOf(" ") == -1)
                {
                    return (true);
                }
            }
        }

        return (false);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Maximum length of a string value that can be written to an Excel cell.
    /// This is approximate (conservatively) and was determined experimentally.

    private static readonly Int32 MaximumStringValueLength = 8200;


    //*************************************************************************
    //  Private members
    //*************************************************************************

    /// true if a call to Range.SpecialCells() is in progress.

    private static Boolean m_bSpecialCellsBeingCalled = false;
}

}
