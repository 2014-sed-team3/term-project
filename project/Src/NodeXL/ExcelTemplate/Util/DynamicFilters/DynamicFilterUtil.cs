﻿
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterUtil
//
/// <summary>
/// Utility methods for working with dynamic filters.
/// </summary>
///
/// <remarks>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.
///
/// <para>
/// Call the <see cref="GetDynamicFilterParameters" /> method to get filter
/// parameters for all filterable columns in a specified table.
/// </para>
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class DynamicFilterUtil : Object
{
    //*************************************************************************
    //  Method: GetDynamicFilterParameters()
    //
    /// <summary>
    /// Gets a collection of all dynamic filter parameters for one table.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="tableName">
    /// Name of the table to get filter parameters for.
    /// </param>
    ///
    /// <returns>
    /// One <see cref="DynamicFilterParameters" /> object for each filterable
    /// column in the specified table.
    /// </returns>
    ///
    /// <remarks>
    /// The returned collection may be empty, but it is never null.
    /// </remarks>
    //*************************************************************************

    public static ICollection<DynamicFilterParameters>
    GetDynamicFilterParameters
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        String worksheetName,
        String tableName
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert( !String.IsNullOrEmpty(worksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(tableName) );

        #if false  // For testing.
        return ( GetRandomDynamicFilterParameters(tableName) );
        #endif

        LinkedList<DynamicFilterParameters> oDynamicFilterParameters = 
            new LinkedList<DynamicFilterParameters>();

        // Get the specified table and loop through its columns.

        ListObject oTable;

        if (ExcelTableUtil.TryGetTable(workbook, worksheetName, tableName,
            out oTable) )
        {
            Application oApplication = workbook.Application;

            foreach (ListColumn oColumn in oTable.ListColumns)
            {
                if ( ColumnShouldBeExcluded(oColumn) )
                {
                    continue;
                }

                ExcelColumnFormat eColumnFormat =
                    ExcelTableUtil.GetTableColumnFormat(oColumn);

                switch (eColumnFormat)
                {
                    case ExcelColumnFormat.Number:
                    case ExcelColumnFormat.Date:
                    case ExcelColumnFormat.Time:
                    case ExcelColumnFormat.DateAndTime:

                        // Get the range of values in the column.

                        Double dMinimumCellValue, dMaximumCellValue;

                        if ( TryGetNumericRange(worksheetName, oColumn,
                            out dMinimumCellValue, out dMaximumCellValue) )
                        {
                            if (eColumnFormat == ExcelColumnFormat.Number)
                            {
                                oDynamicFilterParameters.AddLast(
                                    new NumericFilterParameters(oColumn.Name,
                                        dMinimumCellValue, dMaximumCellValue,

                                        ExcelTableUtil.GetTableColumnDecimalPlaces(
                                            oColumn) )
                                        );
                            }
                            else
                            {
                                oDynamicFilterParameters.AddLast(
                                    new DateTimeFilterParameters(oColumn.Name,
                                        dMinimumCellValue, dMaximumCellValue,
                                        eColumnFormat) );
                            }
                        }

                        break;

                    case ExcelColumnFormat.Other:

                        // Skip the column.

                        break;

                    default:

                        Debug.Assert(false);
                        break;
                }
            }
        }

        return (oDynamicFilterParameters);
    }

    //*************************************************************************
    //  Method: ColumnShouldBeExcluded()
    //
    /// <summary>
    /// Determines whether a table column should be excluded.
    /// </summary>
    ///
    /// <param name="oColumn">
    /// The table column to check.
    /// </param>
    ///
    /// <returns>
    /// true if the column should be excluded.
    /// </returns>
    //*************************************************************************

    private static Boolean
    ColumnShouldBeExcluded
    (
        ListColumn oColumn
    )
    {
        Debug.Assert(oColumn != null);

        switch (oColumn.Name)
        {
            case CommonTableColumnNames.ID:

                // It makes no sense to filter on the NodeXL-generated ID
                // column.

                return (true);

            default:

                break;
        }

        Range oColumnData = oColumn.DataBodyRange;

        // Exclude columns with no data or with an empty first data cell.

        if (
            oColumnData == null
            ||
            oColumnData.Rows.Count < 1
            ||
            !(oColumnData.Cells[1, 1] is Range)
            ||
            ( (Range)oColumnData.Cells[1, 1] ).get_Value(Missing.Value) == null
            )
        {
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TryGetNumericRange()
    //
    /// <summary>
    /// Attempts to get the range of values in a numeric column.
    /// </summary>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="oColumn">
    /// The numeric column.
    /// </param>
    ///
    /// <param name="dMinimumCellValue">
    /// Where the minimum value in the column gets stored if true is returned.
    /// </param>
    ///
    /// <param name="dMaximumCellValue">
    /// Where the maximum value in the column gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the column contains a range of numeric values.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetNumericRange
    (
        String sWorksheetName,
        ListColumn oColumn,
        out Double dMinimumCellValue,
        out Double dMaximumCellValue
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert(oColumn != null);
        Debug.Assert(oColumn.DataBodyRange != null);

        dMinimumCellValue = dMaximumCellValue = Double.MinValue;

        Application oApplication = oColumn.Application;

        String sFunctionCall = String.Format(

            "=MIN({0}!{1})"
            ,
            sWorksheetName,
            ExcelUtil.GetRangeAddress(oColumn.DataBodyRange)
            );

        if ( !ExcelUtil.TryEvaluateDoubleFunction(oApplication, sFunctionCall,
            out dMinimumCellValue) )
        {
            return (false);
        }

        sFunctionCall = sFunctionCall.Replace("MIN", "MAX");

        if ( !ExcelUtil.TryEvaluateDoubleFunction(oApplication, sFunctionCall,
            out dMaximumCellValue) )
        {
            return (false);
        }

        return (dMaximumCellValue > dMinimumCellValue);
    }


    #if false  // For testing.

    //*************************************************************************
    //  Method: GetRandomDynamicFilterParameters()
    //
    /// <summary>
    /// Gets a random collection of dynamic filter parameters for testing.
    /// </summary>
    ///
    /// <param name="sTableName">
    /// Name of the table to get filter parameters for.
    /// </param>
    ///
    /// <returns>
    /// A collection of random <see cref="DynamicFilterParameters" /> objects
    /// for testing.
    /// </returns>
    //*************************************************************************

    private static ICollection<DynamicFilterParameters>
    GetRandomDynamicFilterParameters
    (
        String sTableName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );

        LinkedList<DynamicFilterParameters> oDynamicFilterParameters = 
            new LinkedList<DynamicFilterParameters>();

        Random oRandom = new Random();

        Int32 iDynamicFilters = oRandom.Next(40);

        for (Int32 i = 0; i < iDynamicFilters; i++)
        {
            String sColumnName = sTableName + " "
                + new String('A', oRandom.Next(100) );

            Double dMinimumCellValue = -1000 + oRandom.NextDouble() * (20000);

            Double dMaximumCellValue =
                dMinimumCellValue + oRandom.NextDouble() * (20000);

            oDynamicFilterParameters.AddLast(
                new NumericFilterParameters(sColumnName, dMinimumCellValue,
                    dMaximumCellValue) );
        }

        return (oDynamicFilterParameters);
    }

    #endif
}

}
