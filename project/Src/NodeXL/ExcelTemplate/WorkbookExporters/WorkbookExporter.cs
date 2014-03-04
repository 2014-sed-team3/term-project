
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookExporter
//
/// <summary>
/// Exports a workbook's contents to another workbook.
/// </summary>
///
/// <remarks>
/// Use <see cref="ExportSelectionToNewNodeXLWorkbook" /> to export the
/// selected rows of the edge and vertex tables to a new NodeXL workbook.
/// Use <see cref="ExportToNewMatrixWorkbook" /> to export the edge table to a
/// new workbook as an adjacency matrix.
/// </remarks>
//*****************************************************************************

public class WorkbookExporter
{
    //*************************************************************************
    //  Constructor: WorkbookExporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkbookExporter" />
    /// class.
    /// </summary>
    ///
    /// <param name="workbookToExport">
    /// The NodeXL workbook to export.
    /// </param>
    //*************************************************************************

    public WorkbookExporter
    (
        Microsoft.Office.Interop.Excel.Workbook workbookToExport
    )
    {
        m_oWorkbookToExport = workbookToExport;

        AssertValid();
    }

    //*************************************************************************
    //  Method: ExportSelectionToNewNodeXLWorkbook()
    //
    /// <summary>
    /// Exports the selected rows of the edge and vertex tables to a new NodeXL
    /// workbook.
    /// </summary>
    ///
    /// <returns>
    /// The new workbook.
    /// </returns>
    ///
    /// <remarks>
    /// The new workbook is created from the NodeXL template.
    ///
    /// <para>
    /// The active worksheet is changed several times.  The caller should turn
    /// off screen updating and save and restore the active worksheet.
    /// </para>
    ///
    /// <para>
    /// If there are no selected rows in the edge or vertex tables, or if
    /// an error occurs while exporting the selected rows, an <see
    /// cref="ExportWorkbookException" /> exception is thrown.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Microsoft.Office.Interop.Excel.Workbook
    ExportSelectionToNewNodeXLWorkbook()
    {
        AssertValid();

        // Get the path to the application's template.

        String sTemplatePath;

        if ( !ApplicationUtil.TryGetTemplatePath(out sTemplatePath) )
        {
            throw new ExportWorkbookException(
                ApplicationUtil.GetMissingTemplateMessage() );
        }

        Workbook oNewNodeXLWorkbook = null;

        CopyTableToNewNodeXLWorkbook(WorksheetNames.Edges,
            TableNames.Edges, sTemplatePath, ref oNewNodeXLWorkbook);

        CopyTableToNewNodeXLWorkbook(WorksheetNames.Vertices,
            TableNames.Vertices, sTemplatePath, ref oNewNodeXLWorkbook);

        if (oNewNodeXLWorkbook == null)
        {
            throw new ExportWorkbookException(
                "There are no selected edges or vertices to export to a new"
                + " workbook."
                );
        }

        return (oNewNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: ExportToNewMatrixWorkbook()
    //
    /// <summary>
    /// Exports the edge table to a new workbook as an adjacency matrix.
    /// </summary>
    ///
    /// <returns>
    /// The new workbook.
    /// </returns>
    ///
    /// <remarks>
    /// If there are no rows in the edge table, or if an error occurs while
    /// exporting the rows, an <see cref="ExportWorkbookException" /> exception
    /// is thrown.
    /// </remarks>
    //*************************************************************************

    public Microsoft.Office.Interop.Excel.Workbook
    ExportToNewMatrixWorkbook()
    {
        AssertValid();

        // Merge duplicate edges and add an edge weight column.

        ( new DuplicateEdgeMerger() ).MergeDuplicateEdges(m_oWorkbookToExport);

        // Read the workbook, including the edge weight column.

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadEdgeWeights = true;

        IGraph oGraph = ( new WorkbookReader() ).ReadWorkbook(
            m_oWorkbookToExport, oReadWorkbookContext);

        IVertexCollection oVertices = oGraph.Vertices;
        Int32 iVertices = oVertices.Count;

        if (iVertices == 0)
        {
            throw new ExportWorkbookException(
                "There are no vertices to export."
                );
        }

        Workbook oNewWorkbook =
            m_oWorkbookToExport.Application.Workbooks.Add(Missing.Value);

        Worksheet oNewWorksheet = (Worksheet)oNewWorkbook.ActiveSheet;

        // Fill in row 1 and column A with the vertex names, starting at B1 and
        // A2, respectively.

        String [,] asVertexNamesForRow1 = ExcelUtil.GetSingleRow2DStringArray(
            iVertices);

        String [,] asVertexNamesForColumnA =
            ExcelUtil.GetSingleColumn2DStringArray(iVertices);

        Int32 i = 0;

        foreach (IVertex oVertexI in oVertices)
        {
            asVertexNamesForRow1[1, i + 1] = asVertexNamesForColumnA[i + 1, 1]
                = oVertexI.Name;

            i++;
        }

        ExcelUtil.SetRangeValues( (Range)oNewWorksheet.Cells[1, 2],
            asVertexNamesForRow1 );

        ExcelUtil.SetRangeValues( (Range)oNewWorksheet.Cells[2, 1],
            asVertexNamesForColumnA );

        asVertexNamesForRow1 = asVertexNamesForColumnA = null;

        // Now fill in the edge weights, row by row.

        Range oFirstColumnCell = (Range)oNewWorksheet.Cells[2, 2];

        foreach (IVertex oVertexI in oVertices)
        {
            Object [,] aoEdgeWeights =
                ExcelUtil.GetSingleRow2DArray(iVertices);

            Int32 j = 0;

            foreach (IVertex oVertexJ in oVertices)
            {
                aoEdgeWeights[1, j + 1] =
                    EdgeUtil.GetEdgeWeightSum(oVertexI, oVertexJ);

                j++;
            }

            ExcelUtil.SetRangeValues(oFirstColumnCell, aoEdgeWeights);
            oFirstColumnCell = oFirstColumnCell.get_Offset(1, 0);
        }

        return (oNewWorkbook);
    }

    //*************************************************************************
    //  Method: CopyTableToNewNodeXLWorkbook()
    //
    /// <summary>
    /// Copies the selected rows of a specified table to a new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the table.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table.
    /// </param>
    ///
    /// <param name="sTemplatePath">
    /// Path to the NodeXL template.
    /// </param>
    ///
    /// <param name="oNewNodeXLWorkbook">
    /// If this isn't already set to a new workbook, this method creates the
    /// workbook if necessary and sets this parameter to it.  Note that it may
    /// still be null when this method returns.
    /// </param>
    //*************************************************************************

    protected void
    CopyTableToNewNodeXLWorkbook
    (
        String sWorksheetName,
        String sTableName,
        String sTemplatePath,
        ref Workbook oNewNodeXLWorkbook
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert( !String.IsNullOrEmpty(sTemplatePath) );
        AssertValid();

        ListObject oTable;
        Range oSelectedTableRange;

        if (
            !ExcelTableUtil.TryGetSelectedTableRange(m_oWorkbookToExport,
                sWorksheetName, sTableName, out oTable,
                out oSelectedTableRange)
            ||
            ExcelTableUtil.VisibleTableRangeIsEmpty(oTable)
            )
        {
            return;
        }

        Range oVisibleSelectedTableRange;

        // CopyRowsToNewNodeXLWorkbook() can handle hidden rows, but not hidden
        // columns.  Temporarily show all hidden columns in the table.

        ExcelHiddenColumns oHiddenColumns =
            ExcelColumnHider.ShowHiddenColumns(oTable);

        try
        {
            if ( ExcelUtil.TryGetVisibleRange(oSelectedTableRange,
                out oVisibleSelectedTableRange) )
            {
                // Create the new workbook if necessary and copy the table's
                // selected rows to it.

                if (oNewNodeXLWorkbook == null)
                {
                    oNewNodeXLWorkbook =
                        m_oWorkbookToExport.Application.Workbooks.Add(
                            sTemplatePath);
                }

                CopyRowsToNewNodeXLWorkbook(oTable, oVisibleSelectedTableRange,
                    oNewNodeXLWorkbook);
            }
        }
        finally
        {
            ExcelColumnHider.RestoreHiddenColumns(oTable, oHiddenColumns);
        }
    }

    //*************************************************************************
    //  Method: CopyRowsToNewNodeXLWorkbook()
    //
    /// <summary>
    /// Copies specified rows in a table to a new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="oSourceTable">
    /// Table to copy from.
    /// </param>
    ///
    /// <param name="oSourceTableRangeToCopy">
    /// For each row in <paramref name="oSourceTable" /> that intersects this
    /// range, the row is copied to the new workbook.  This will contain
    /// multiple areas if the table has filtered rows.  It must not contain
    /// filtered columns.
    /// </param>
    ///
    /// <param name="oNewNodeXLWorkbook">
    /// The new NodeXL workbook to copy the rows to.
    /// </param>
    //*************************************************************************

    protected void
    CopyRowsToNewNodeXLWorkbook
    (
        ListObject oSourceTable,
        Range oSourceTableRangeToCopy,
        Workbook oNewNodeXLWorkbook
    )
    {
        Debug.Assert(oSourceTable != null);
        Debug.Assert(oSourceTableRangeToCopy != null);
        Debug.Assert(oNewNodeXLWorkbook != null);
        AssertValid();

        String sTableName = oSourceTable.Name;

        Debug.Assert(oSourceTable.Parent is Worksheet);

        String sWorksheetName = ( (Worksheet)oSourceTable.Parent ).Name;

        ListObject oNewTable;

        if (!ExcelTableUtil.TryGetTable(oNewNodeXLWorkbook, sWorksheetName,
            sTableName, out oNewTable) )
        {
            throw new ExportWorkbookException(
                "A table is missing in the new workbook."
                );
        }

        foreach (ListColumn oSourceColumn in oSourceTable.ListColumns)
        {
            // Add the column to the new table if it doesn't already exist.

            Range oNewColumnData =
                GetOrAddTableColumn(oSourceColumn, oNewTable);

            Range oSourceColumnData = oSourceColumn.DataBodyRange;

            Debug.Assert(oSourceColumnData != null);
            Debug.Assert(oNewColumnData != null);

            // Read the source column.

            Object [,] aoSourceValues =
                ExcelUtil.GetRangeValues(oSourceColumnData);

            // Create a collection to hold the specified column cells to be
            // copied.

            LinkedList<Object> oNewValues = new LinkedList<Object>();

            // Get the cells to be copied.

            foreach (Range oSourceTableRangeArea in
                oSourceTableRangeToCopy.Areas)
            {
                Int32 iFirstRowOneBased =
                    oSourceTableRangeArea.Row - oSourceColumnData.Row + 1;

                Int32 iLastRowOneBased =
                    iFirstRowOneBased + oSourceTableRangeArea.Rows.Count - 1;

                for (Int32 iRowOneBased = iFirstRowOneBased;
                    iRowOneBased <= iLastRowOneBased; iRowOneBased++)
                {
                    oNewValues.AddLast( aoSourceValues[iRowOneBased, 1] );
                }
            }

            // Copy the cells to the new workbook.

            Int32 iValuesToCopy = oNewValues.Count;

            if (iValuesToCopy > 0)
            {
                Object [,] aoNewValues =
                    ExcelUtil.GetSingleColumn2DArray(iValuesToCopy);

                Int32 iNewValueIndexOneBased = 1;

                foreach (Object oNewValue in oNewValues)
                {
                    aoNewValues[iNewValueIndexOneBased, 1] = oNewValue;
                    iNewValueIndexOneBased++;
                }

                // Note that a cell may contain a string that starts with an
                // equal sign, which would cause Excel to throw an exception.
                // Remove any equal signs before copying the cells.

                ExcelUtil.SetRangeValues(oNewColumnData, aoNewValues, true);
            }
        }
    }

    //*************************************************************************
    //  Method: GetOrAddTableColumn()
    //
    /// <summary>
    /// Adds a column to a new NodeXL table if the column doesn't already
    /// exist.
    /// </summary>
    ///
    /// <param name="oSourceColumn">
    /// Column in the source table.
    /// </param>
    ///
    /// <param name="oNewTable">
    /// Table to add a column to if necessary.
    /// </param>
    ///
    /// <returns>
    /// The data body range of the column in the new table.
    /// </returns>
    //*************************************************************************

    protected Range
    GetOrAddTableColumn
    (
        ListColumn oSourceColumn,
        ListObject oNewTable
    )
    {
        Debug.Assert(oSourceColumn != null);
        Debug.Assert(oNewTable != null);
        AssertValid();

        String sColumnName = oSourceColumn.Name;

        Range oSourceColumnData = oSourceColumn.DataBodyRange;
        Debug.Assert(oSourceColumnData != null);

        ListColumn oNewColumn;

        if (!ExcelTableUtil.TryGetOrAddTableColumn(oNewTable, sColumnName,
                (Double)oSourceColumnData.ColumnWidth, null,
                out oNewColumn) )
        {
            throw new ExportWorkbookException(
                "A column couldn't be added to the new workbook."
                );
        }

        Range oNewColumnData = oNewColumn.DataBodyRange;

        Debug.Assert(oNewColumnData != null);

        oNewColumnData.NumberFormat = oSourceColumnData.NumberFormat;

        return (oNewColumnData);
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
        Debug.Assert(m_oWorkbookToExport != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// NodeXL workbook to export.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbookToExport;
}

}
