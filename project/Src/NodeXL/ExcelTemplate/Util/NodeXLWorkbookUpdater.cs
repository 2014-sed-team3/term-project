
using System;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLWorkbookUpdater
//
/// <summary>
/// Updates the worksheets, tables, and columns in an older NodeXL workbook.
/// </summary>
///
/// <remarks>
/// Use <see cref="UpdateWorkbook" /> to update a workbook.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class NodeXLWorkbookUpdater : Object
{
    //*************************************************************************
    //  Method: UpdateWorkbook()
    //
    /// <summary>
    /// Updates the worksheets, tables, and columns in an older NodeXL workbook
    /// if neccessary.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The NodeXL workbook to update.
    /// </param>
    ///
    /// <remarks>
    /// Some column, table, and worksheet names have changed in later versions
    /// of NodeXL.  This method looks for the older names and updates them to
    /// the latest names if it finds them.  It also adds columns that were
    /// added in later versions.
    /// </remarks>
    //*************************************************************************

    public static void
    UpdateWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        Worksheet oWorksheet;
        ListObject oTable;
        ListColumn oColumn;

        // Rename the Radius column on the Vertices worksheet to Size.

        if ( ExcelTableUtil.TryGetTable(workbook, "Vertices", "Vertices",
            out oTable)
            &&
            ExcelTableUtil.TryGetTableColumn(oTable, "Radius", out oColumn)
        )
        {
            oColumn.Name = "Size";
        }

        // Rename the Clusters worksheet to Groups.

        if ( ExcelUtil.TryGetWorksheet(workbook, "Clusters", out oWorksheet) )
        {
            oWorksheet.Name = "Groups";

            if (ExcelTableUtil.TryGetTable(oWorksheet, "Clusters", out oTable))
            {
                oTable.Name = "Groups";

                if (ExcelTableUtil.TryGetTableColumn(oTable, "Cluster",
                    out oColumn) )
                {
                    oColumn.Name = "Group";
                }
            }
        }

        // Rename the Cluster Vertices worksheet to Group Vertices.

        if ( ExcelUtil.TryGetWorksheet(workbook, "Cluster Vertices",
            out oWorksheet) )
        {
            oWorksheet.Name = "Group Vertices";

            if (ExcelTableUtil.TryGetTable(oWorksheet, "ClusterVertices",
                out oTable) )
            {
                oTable.Name = "GroupVertices";

                if (ExcelTableUtil.TryGetTableColumn(oTable, "Cluster",
                    out oColumn) )
                {
                    oColumn.Name = "Group";
                }
            }
        }

        // Add a Label column to the Groups worksheet.

        if ( ExcelTableUtil.TryGetTable(workbook,
            WorksheetNames.Groups, TableNames.Groups, out oTable) )
        {
            ExcelTableUtil.TryGetOrAddTableColumn(oTable,
                GroupTableColumnNames.Label, ExcelTableUtil.AutoColumnWidth,
                CellStyleNames.Label, out oColumn);
        }

        // Rename the Metric column on the Overall Metrics worksheet to Graph
        // Metric.

        if ( ExcelUtil.TryGetWorksheet(workbook, "Overall Metrics",
            out oWorksheet) )
        {
            if (ExcelTableUtil.TryGetTable(oWorksheet, "OverallMetrics",
                out oTable) )
            {
                if (ExcelTableUtil.TryGetTableColumn(oTable, "Metric",
                    out oColumn) )
                {
                    oColumn.Name = "Graph Metric";
                }
            }
        }
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    // (None.)
}

}
