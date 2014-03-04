
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookAutoFiller
//
/// <summary>
/// Automatically fills one or more edge and vertex attribute columns by
/// mapping values from user-specified source columns.
/// </summary>
///
/// <remarks>
/// Call the <see cref="AutoFillWorkbook" /> method to autofill one or more
/// edge and vertex attribute columns.  The method takes an <see
/// cref="AutoFillUserSettings" /> object that specifies one or more
/// source-to-destination column mappings.  For example, the settings might
/// specify that the vertex color column should be autofilled by mapping the
/// numbers in a numeric source to a specified range of colors, and that the
/// edge width column should be autofilled by mapping the numbers in another
/// numeric source column to a specified range of widths.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class WorkbookAutoFiller : Object
{
    //*************************************************************************
    //  Method: AutoFillWorkbook()
    //
    /// <summary>
    /// Runs the application's autofill feature on a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="autoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    ///
    /// <remarks>
    /// See the class topic for information on the AutoFill feature.
    ///
    /// <para>
    /// In addition to autofilling one or more columns, this method stores the
    /// results of the autofill as an <see cref="AutoFillWorkbookResults" />
    /// using <see cref="PerWorkbookSettings" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    AutoFillWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        AutoFillUserSettings autoFillUserSettings
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(autoFillUserSettings != null);

        Application oApplication = workbook.Application;

        oApplication.ScreenUpdating = false;

        try
        {
            AutoFillWorkbookInternal(workbook, autoFillUserSettings);
        }
        finally
        {
            oApplication.ScreenUpdating = true;
        }
    }

    //*************************************************************************
    //  Method: AutoFillWorkbookInternal()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillWorkbookInternal
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        AutoFillUserSettings oAutoFillUserSettings
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oAutoFillUserSettings != null);

        // Populate the vertex worksheet with the name of each unique vertex in
        // the edge worksheet.

        ( new VertexWorksheetPopulator() ).PopulateVertexWorksheet(
            oWorkbook, false);

        AutoFillWorkbookResults oAutoFillWorkbookResults =
            new AutoFillWorkbookResults();

        AutoFillTable(oWorkbook, oAutoFillUserSettings,
            oAutoFillWorkbookResults, WorksheetNames.Edges,
            TableNames.Edges, AutoFillEdgeTable);

        AutoFillTable(oWorkbook, oAutoFillUserSettings,
            oAutoFillWorkbookResults, WorksheetNames.Vertices,
            TableNames.Vertices, AutoFillVertexTable);

        AutoFillTable(oWorkbook, oAutoFillUserSettings,
            oAutoFillWorkbookResults, WorksheetNames.Groups,
            TableNames.Groups, AutoFillGroupTable);

        // Save the results.

        ( new PerWorkbookSettings(oWorkbook) ).AutoFillWorkbookResults =
            oAutoFillWorkbookResults;
    }

    //*************************************************************************
    //  Delegate: AutoFillTableMethod()
    //
    /// <summary>
    /// Represents a method that autofills a table.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    ///
    /// <param name="oAutoFillWorkbookResults">
    /// Stores the autofill results.
    /// </param>
    //*************************************************************************

    private delegate void
    AutoFillTableMethod
    (
        ListObject oTable,
        AutoFillUserSettings oAutoFillUserSettings,
        AutoFillWorkbookResults oAutoFillWorkbookResults
    );

    //*************************************************************************
    //  Method: AutoFillTable()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on one table.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    ///
    /// <param name="oAutoFillWorkbookResults">
    /// Stores the autofill results.
    /// </param>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillTableMethod">
    /// Method to call to autofill the table.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillTable
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        AutoFillUserSettings oAutoFillUserSettings,
        AutoFillWorkbookResults oAutoFillWorkbookResults,
        String sWorksheetName,
        String sTableName,
        AutoFillTableMethod oAutoFillTableMethod
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oAutoFillUserSettings != null);
        Debug.Assert(oAutoFillWorkbookResults != null);
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert(oAutoFillTableMethod != null);

        ListObject oTable;
        ExcelHiddenColumns oHiddenColumns;

        if (ExcelTableUtil.TryGetTable(oWorkbook, sWorksheetName, sTableName,
            out oTable) )
        {
            // The TableColumnMapper class that does the actual autofilling
            // fills only visible cells.  Temporarily show all hidden columns
            // in the table.

            oHiddenColumns = ExcelColumnHider.ShowHiddenColumns(oTable);

            try
            {
                oAutoFillTableMethod(oTable, oAutoFillUserSettings,
                    oAutoFillWorkbookResults);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oTable, oHiddenColumns);
            }
        }
    }

    //*************************************************************************
    //  Method: AutoFillEdgeTable()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on the edge table.
    /// </summary>
    ///
    /// <param name="oEdgeTable">
    /// The table to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    ///
    /// <param name="oAutoFillWorkbookResults">
    /// Stores the autofill results.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillEdgeTable
    (
        ListObject oEdgeTable,
        AutoFillUserSettings oAutoFillUserSettings,
        AutoFillWorkbookResults oAutoFillWorkbookResults
    )
    {
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oAutoFillUserSettings != null);
        Debug.Assert(oAutoFillWorkbookResults != null);

        Boolean bSourceColumnContainsNumbers;
        Double dSourceCalculationNumber1, dSourceCalculationNumber2;
        Int32 iDecimalPlaces;
        ICollection<String> oCategoryNames;

        if ( TryAutoFillColorColumn(oEdgeTable,
                oAutoFillUserSettings.EdgeColorSourceColumnName,
                EdgeTableColumnNames.Color,
                oAutoFillUserSettings.EdgeColorDetails,
                out bSourceColumnContainsNumbers,
                out dSourceCalculationNumber1, out dSourceCalculationNumber2,
                out iDecimalPlaces, out oCategoryNames
                ) )
        {
            oAutoFillWorkbookResults.EdgeColorResults =
                new AutoFillColorColumnResults(
                    bSourceColumnContainsNumbers,
                    oAutoFillUserSettings.EdgeColorSourceColumnName,
                    dSourceCalculationNumber1, dSourceCalculationNumber2,
                    iDecimalPlaces,
                    oAutoFillUserSettings.EdgeColorDetails.DestinationColor1,
                    oAutoFillUserSettings.EdgeColorDetails.DestinationColor2,
                    oCategoryNames
                    );
        }

        if ( TryAutoFillNumericRangeColumn(oEdgeTable,
                oAutoFillUserSettings.EdgeWidthSourceColumnName,
                EdgeTableColumnNames.Width,
                oAutoFillUserSettings.EdgeWidthDetails,
                out dSourceCalculationNumber1, out dSourceCalculationNumber2,
                out iDecimalPlaces
                ) )
        {
            oAutoFillWorkbookResults.EdgeWidthResults =
                new AutoFillNumericRangeColumnResults(
                    oAutoFillUserSettings.EdgeWidthSourceColumnName,
                    dSourceCalculationNumber1, dSourceCalculationNumber2,
                    iDecimalPlaces,
                    oAutoFillUserSettings.EdgeWidthDetails.DestinationNumber1,
                    oAutoFillUserSettings.EdgeWidthDetails.DestinationNumber2,
                    oAutoFillUserSettings.EdgeWidthDetails.UseLogs
                    );
        }

        AutoFillNumericComparisonColumn(oEdgeTable,
            oAutoFillUserSettings.EdgeStyleSourceColumnName,
            EdgeTableColumnNames.Style, oAutoFillUserSettings.EdgeStyleDetails
            );

        if ( TryAutoFillNumericRangeColumn(oEdgeTable,
                oAutoFillUserSettings.EdgeAlphaSourceColumnName,
                CommonTableColumnNames.Alpha,
                oAutoFillUserSettings.EdgeAlphaDetails,
                out dSourceCalculationNumber1, out dSourceCalculationNumber2,
                out iDecimalPlaces
                ) )
        {
            oAutoFillWorkbookResults.EdgeAlphaResults =
                new AutoFillNumericRangeColumnResults(
                    oAutoFillUserSettings.EdgeAlphaSourceColumnName,
                    dSourceCalculationNumber1, dSourceCalculationNumber2,
                    iDecimalPlaces,
                    oAutoFillUserSettings.EdgeAlphaDetails.DestinationNumber1,
                    oAutoFillUserSettings.EdgeAlphaDetails.DestinationNumber2,
                    oAutoFillUserSettings.EdgeAlphaDetails.UseLogs
                    );
        }

        AutoFillNumericComparisonColumn(oEdgeTable,
            oAutoFillUserSettings.EdgeVisibilitySourceColumnName,
            CommonTableColumnNames.Visibility,
            oAutoFillUserSettings.EdgeVisibilityDetails
            );

        AutoFillColumnViaCopy(oEdgeTable,
            oAutoFillUserSettings.EdgeLabelSourceColumnName,
            EdgeTableColumnNames.Label
            );
    }

    //*************************************************************************
    //  Method: AutoFillVertexTable()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on the vertex table.
    /// </summary>
    ///
    /// <param name="oVertexTable">
    /// The table to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    ///
    /// <param name="oAutoFillWorkbookResults">
    /// Stores the autofill results.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillVertexTable
    (
        ListObject oVertexTable,
        AutoFillUserSettings oAutoFillUserSettings,
        AutoFillWorkbookResults oAutoFillWorkbookResults
    )
    {
        Debug.Assert(oVertexTable != null);
        Debug.Assert(oAutoFillUserSettings != null);
        Debug.Assert(oAutoFillWorkbookResults != null);

        Boolean bSourceColumnContainsNumbers;
        Double dSourceCalculationNumber1, dSourceCalculationNumber2;
        Int32 iDecimalPlaces;
        ICollection<String> oCategoryNames;

        if ( TryAutoFillColorColumn(oVertexTable,
                oAutoFillUserSettings.VertexColorSourceColumnName,
                VertexTableColumnNames.Color,
                oAutoFillUserSettings.VertexColorDetails,
                out bSourceColumnContainsNumbers,
                out dSourceCalculationNumber1,
                out dSourceCalculationNumber2,
                out iDecimalPlaces, out oCategoryNames
                ) )
        {
            oAutoFillWorkbookResults.VertexColorResults =
                new AutoFillColorColumnResults(
                    bSourceColumnContainsNumbers,
                    oAutoFillUserSettings.VertexColorSourceColumnName,
                    dSourceCalculationNumber1, dSourceCalculationNumber2,
                    iDecimalPlaces,
                    oAutoFillUserSettings.VertexColorDetails.DestinationColor1,
                    oAutoFillUserSettings.VertexColorDetails.DestinationColor2,
                    oCategoryNames
                    );
        }

        AutoFillNumericComparisonColumn(oVertexTable,
            oAutoFillUserSettings.VertexShapeSourceColumnName,
            VertexTableColumnNames.Shape,
            oAutoFillUserSettings.VertexShapeDetails
            );

        if ( TryAutoFillNumericRangeColumn(oVertexTable,
                oAutoFillUserSettings.VertexRadiusSourceColumnName,
                VertexTableColumnNames.Radius,
                oAutoFillUserSettings.VertexRadiusDetails,
                out dSourceCalculationNumber1, out dSourceCalculationNumber2,
                out iDecimalPlaces
                ) )
        {
            oAutoFillWorkbookResults.VertexRadiusResults =
                new AutoFillNumericRangeColumnResults(
                    oAutoFillUserSettings.VertexRadiusSourceColumnName,
                    dSourceCalculationNumber1, dSourceCalculationNumber2,
                    iDecimalPlaces,
                    oAutoFillUserSettings.VertexRadiusDetails.
                        DestinationNumber1,
                    oAutoFillUserSettings.VertexRadiusDetails.
                        DestinationNumber2,
                    oAutoFillUserSettings.VertexRadiusDetails.UseLogs
                    );
        }

        if ( TryAutoFillNumericRangeColumn(oVertexTable,
                oAutoFillUserSettings.VertexAlphaSourceColumnName,
                CommonTableColumnNames.Alpha,
                oAutoFillUserSettings.VertexAlphaDetails,
                out dSourceCalculationNumber1, out dSourceCalculationNumber2,
                out iDecimalPlaces
                ) )
        {
            oAutoFillWorkbookResults.VertexAlphaResults =
                new AutoFillNumericRangeColumnResults(
                    oAutoFillUserSettings.VertexAlphaSourceColumnName,
                    dSourceCalculationNumber1, dSourceCalculationNumber2,
                    iDecimalPlaces,
                    oAutoFillUserSettings.VertexAlphaDetails.DestinationNumber1,
                    oAutoFillUserSettings.VertexAlphaDetails.DestinationNumber2,
                    oAutoFillUserSettings.VertexAlphaDetails.UseLogs
                    );
        }

        AutoFillColumnViaCopy(oVertexTable,
            oAutoFillUserSettings.VertexLabelSourceColumnName,
            VertexTableColumnNames.Label
            );

        TryAutoFillColorColumn(oVertexTable,
            oAutoFillUserSettings.VertexLabelFillColorSourceColumnName,
            VertexTableColumnNames.LabelFillColor,
            oAutoFillUserSettings.VertexLabelFillColorDetails,
            out bSourceColumnContainsNumbers,
            out dSourceCalculationNumber1, out dSourceCalculationNumber2,
            out iDecimalPlaces, out oCategoryNames
            );

        AutoFillNumericComparisonColumn(oVertexTable,
            oAutoFillUserSettings.VertexLabelPositionSourceColumnName,
            VertexTableColumnNames.LabelPosition,
            oAutoFillUserSettings.VertexLabelPositionDetails
            );

        AutoFillColumnViaCopy(oVertexTable,
            oAutoFillUserSettings.VertexToolTipSourceColumnName,
            VertexTableColumnNames.ToolTip
            );

        AutoFillNumericComparisonColumn(oVertexTable,
            oAutoFillUserSettings.VertexVisibilitySourceColumnName,
            CommonTableColumnNames.Visibility,
            oAutoFillUserSettings.VertexVisibilityDetails
            );

        TryAutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexLayoutOrderSourceColumnName,
            VertexTableColumnNames.LayoutOrder,
            oAutoFillUserSettings.VertexLayoutOrderDetails,
            out dSourceCalculationNumber1, out dSourceCalculationNumber2,
            out iDecimalPlaces
            );

        Boolean bXAutoFilled = TryAutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexXSourceColumnName,
            VertexTableColumnNames.X,
            oAutoFillUserSettings.VertexXDetails,
            out dSourceCalculationNumber1, out dSourceCalculationNumber2,
            out iDecimalPlaces
            );

        Double dYSourceCalculationNumber1, dYSourceCalculationNumber2;

        Boolean bYAutoFilled = TryAutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexYSourceColumnName,
            VertexTableColumnNames.Y,
            oAutoFillUserSettings.VertexYDetails,
            out dYSourceCalculationNumber1, out dYSourceCalculationNumber2,
            out iDecimalPlaces
            );

        if (bXAutoFilled && bYAutoFilled)
        {
            // (Note that the decimal places for the Y column take precedence
            // here.)

            oAutoFillWorkbookResults.VertexXResults =
                new AutoFillNumericRangeColumnResults(
                    oAutoFillUserSettings.VertexXSourceColumnName,
                    dSourceCalculationNumber1, dSourceCalculationNumber2,
                    iDecimalPlaces,
                    oAutoFillUserSettings.VertexXDetails.DestinationNumber1,
                    oAutoFillUserSettings.VertexXDetails.DestinationNumber2,
                    oAutoFillUserSettings.VertexXDetails.UseLogs
                    );

            oAutoFillWorkbookResults.VertexYResults =
                new AutoFillNumericRangeColumnResults(
                    oAutoFillUserSettings.VertexYSourceColumnName,
                    dYSourceCalculationNumber1, dYSourceCalculationNumber2,
                    iDecimalPlaces,
                    oAutoFillUserSettings.VertexYDetails.DestinationNumber1,
                    oAutoFillUserSettings.VertexYDetails.DestinationNumber2,
                    oAutoFillUserSettings.VertexYDetails.UseLogs
                    );
        }

        TryAutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexPolarRSourceColumnName,
            VertexTableColumnNames.PolarR,
            oAutoFillUserSettings.VertexPolarRDetails,
            out dSourceCalculationNumber1, out dSourceCalculationNumber2,
            out iDecimalPlaces
            );

        TryAutoFillNumericRangeColumn(oVertexTable,
            oAutoFillUserSettings.VertexPolarAngleSourceColumnName,
            VertexTableColumnNames.PolarAngle,
            oAutoFillUserSettings.VertexPolarAngleDetails,
            out dSourceCalculationNumber1, out dSourceCalculationNumber2,
            out iDecimalPlaces
            );
    }

    //*************************************************************************
    //  Method: AutoFillGroupTable()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on the group table.
    /// </summary>
    ///
    /// <param name="oGroupTable">
    /// The table to autofill.
    /// </param>
    ///
    /// <param name="oAutoFillUserSettings">
    /// Specifies one or more source-to-destination column mappings.
    /// </param>
    ///
    /// <param name="oAutoFillWorkbookResults">
    /// Stores the autofill results.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillGroupTable
    (
        ListObject oGroupTable,
        AutoFillUserSettings oAutoFillUserSettings,
        AutoFillWorkbookResults oAutoFillWorkbookResults
    )
    {
        Debug.Assert(oGroupTable != null);
        Debug.Assert(oAutoFillUserSettings != null);
        Debug.Assert(oAutoFillWorkbookResults != null);

        AutoFillNumericComparisonColumn(oGroupTable,
            oAutoFillUserSettings.GroupCollapsedSourceColumnName,
            GroupTableColumnNames.Collapsed,
            oAutoFillUserSettings.GroupCollapsedDetails
            );

        if (
            AutoFillColumnViaCopy(oGroupTable,
                oAutoFillUserSettings.GroupLabelSourceColumnName,
                GroupTableColumnNames.Label)
            &&
            oAutoFillUserSettings.GroupLabelDetails.PrependWithGroupName
            )
        {
            PrependGroupLabelsWithGroupNames(oGroupTable);
        }
    }

    //*************************************************************************
    //  Method: PrependGroupLabelsWithGroupNames()
    //
    /// <summary>
    /// Prepends group labels with group names.
    /// </summary>
    ///
    /// <param name="oGroupTable">
    /// The table to autofill.
    /// </param>
    ///
    /// <remarks>
    /// If a group name cell contains "G1" and the corresponding group label
    /// cell contains "label", for example, then this method replaced the group
    /// label cell with "G1: label".
    /// </remarks>
    //*************************************************************************

    private static void
    PrependGroupLabelsWithGroupNames
    (
        ListObject oGroupTable
    )
    {
        Debug.Assert(oGroupTable != null);

        // Use ExcelTableReader to accomplish the task with minimal code.
        //
        // Note that ExcelTableReader is optimized for reading, and that its
        // use for writing incurs the overhead of a COM call for each written
        // cell.  There typically aren't many groups, though, so this is
        // probably tolerable.
        //
        // If this turns out to be too slow, something similar to the code in
        // TableColumnMapper.MapViaCopy() will need to be implemented.

        ExcelTableReader oExcelTableReader = new ExcelTableReader(oGroupTable);

        if (
            oExcelTableReader.ColumnNames.Contains(GroupTableColumnNames.Name)
            &&
            oExcelTableReader.ColumnNames.Contains(GroupTableColumnNames.Label)
            )
        {
            foreach (ExcelTableReader.ExcelTableRow oRow in
                oExcelTableReader.GetRows() )
            {
                String sName;

                if ( oRow.TryGetNonEmptyStringFromCell(
                    GroupTableColumnNames.Name, out sName) )
                {
                    String sLabel;

                    if ( oRow.TryGetNonEmptyStringFromCell(
                        GroupTableColumnNames.Label, out sLabel) )
                    {
                        sName += ": " + sLabel;
                    }

                    oRow.GetRangeForCell(GroupTableColumnNames.Label)
                        .set_Value(Missing.Value, sName);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TryAutoFillColorColumn()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a destination column that
    /// should contain colors.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the source and destination columns.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// Name of the source column.  If null or empty, this method does nothing.
    /// </param>
    ///
    /// <param name="sDestinationColumnName">
    /// Name of the destination column.
    /// </param>
    ///
    /// <param name="oDetails">
    /// User-specified details for the destination column.
    /// </param>
    ///
    /// <param name="bSourceColumnContainsNumbers">
    /// Where a flag gets stored if true is returned: true if the source column
    /// contains numbers, false if it contains categories.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber1">
    /// Where the actual first source number used in the calculations gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber2">
    /// Where the actual second source number used in the calculations gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <param name="iDecimalPlaces">
    /// Where the number of decimal places displayed in the column gets stored.
    /// </param>
    ///
    /// <param name="oCategoryNames">
    /// Where a collection of category names or null gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the autofill was performed.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryAutoFillColorColumn
    (
        ListObject oTable,
        String sSourceColumnName,
        String sDestinationColumnName,
        ColorColumnAutoFillUserSettings oDetails,
        out Boolean bSourceColumnContainsNumbers,
        out Double dSourceCalculationNumber1,
        out Double dSourceCalculationNumber2,
        out Int32 iDecimalPlaces,
        out ICollection<String> oCategoryNames
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );
        Debug.Assert(oDetails != null);

        bSourceColumnContainsNumbers = oDetails.SourceColumnContainsNumbers;
        dSourceCalculationNumber1 = 0;
        dSourceCalculationNumber2 = 1;
        iDecimalPlaces = 0;
        oCategoryNames = null;

        if ( String.IsNullOrEmpty(sSourceColumnName) )
        {
            return (false);
        }

        if (bSourceColumnContainsNumbers)
        {
            return ( TableColumnMapper.TryMapToColor(oTable,
                sSourceColumnName, sDestinationColumnName,
                oDetails.UseSourceNumber1, oDetails.UseSourceNumber2,
                oDetails.SourceNumber1, oDetails.SourceNumber2,
                oDetails.DestinationColor1, oDetails.DestinationColor2,
                oDetails.IgnoreOutliers, oDetails.UseLogs,
                out dSourceCalculationNumber1, out dSourceCalculationNumber2,
                out iDecimalPlaces
                ) );
        }

        return ( TableColumnMapper.TryMapToColor(oTable, sSourceColumnName,
            sDestinationColumnName, out oCategoryNames) );
    }

    //*************************************************************************
    //  Method: TryAutoFillNumericRangeColumn()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a destination column that
    /// should contain a numeric range.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the source and destination columns.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// Name of the source column.  If null or empty, this method does nothing.
    /// </param>
    ///
    /// <param name="sDestinationColumnName">
    /// Name of the destination column.
    /// </param>
    ///
    /// <param name="oDetails">
    /// User-specified details for the destination column.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber1">
    /// Where the actual first source number used in the calculations gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber2">
    /// Where the actual second source number used in the calculations gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <param name="iDecimalPlaces">
    /// Where the number of decimal places displayed in the column gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the autofill was performed.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryAutoFillNumericRangeColumn
    (
        ListObject oTable,
        String sSourceColumnName,
        String sDestinationColumnName,
        NumericRangeColumnAutoFillUserSettings oDetails,
        out Double dSourceCalculationNumber1,
        out Double dSourceCalculationNumber2,
        out Int32 iDecimalPlaces
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );
        Debug.Assert(oDetails != null);

        dSourceCalculationNumber1 = dSourceCalculationNumber2 =
            Double.MinValue;

        iDecimalPlaces = Int32.MinValue;

        if ( String.IsNullOrEmpty(sSourceColumnName) )
        {
            return (false);
        }

        return (TableColumnMapper.TryMapToNumericRange(
            oTable, sSourceColumnName, sDestinationColumnName,
            oDetails.UseSourceNumber1,
            oDetails.UseSourceNumber2,
            oDetails.SourceNumber1,
            oDetails.SourceNumber2,
            oDetails.DestinationNumber1,
            oDetails.DestinationNumber2,
            oDetails.IgnoreOutliers,
            oDetails.UseLogs,
            out dSourceCalculationNumber1,
            out dSourceCalculationNumber2,
            out iDecimalPlaces
            ) );
    }

    //*************************************************************************
    //  Method: AutoFillNumericComparisonColumn()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a destination column that
    /// should contain one of two strings.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the source and destination columns.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// Name of the source column.  If null or empty, this method does nothing.
    /// </param>
    ///
    /// <param name="sDestinationColumnName">
    /// Name of the destination column.
    /// </param>
    ///
    /// <param name="oDetails">
    /// User-specified details for the destination column.
    /// </param>
    //*************************************************************************

    private static void
    AutoFillNumericComparisonColumn
    (
        ListObject oTable,
        String sSourceColumnName,
        String sDestinationColumnName,
        NumericComparisonColumnAutoFillUserSettings oDetails
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );
        Debug.Assert(oDetails != null);

        if ( String.IsNullOrEmpty(sSourceColumnName) )
        {
            return;
        }

        TableColumnMapper.MapToTwoStrings(
            oTable, sSourceColumnName, sDestinationColumnName,
            oDetails.ComparisonOperator,
            oDetails.SourceNumberToCompareTo,
            oDetails.DestinationString1,
            oDetails.DestinationString2
            );
    }

    //*************************************************************************
    //  Method: AutoFillColumnViaCopy()
    //
    /// <summary>
    /// Runs the application's AutoFill feature on a destination column that
    /// should contain a copy of the source column.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the source and destination columns.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// Name of the source column.  If null or empty, this method does nothing.
    /// </param>
    ///
    /// <param name="sDestinationColumnName">
    /// Name of the destination column.
    /// </param>
    ///
    /// <returns>
    /// true if the column was autofilled.
    /// </returns>
    //*************************************************************************

    private static Boolean
    AutoFillColumnViaCopy
    (
        ListObject oTable,
        String sSourceColumnName,
        String sDestinationColumnName
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sDestinationColumnName) );

        if ( String.IsNullOrEmpty(sSourceColumnName) )
        {
            return (false);
        }

        TableColumnMapper.MapViaCopy(
            oTable, sSourceColumnName, sDestinationColumnName
            );

        return (true);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
