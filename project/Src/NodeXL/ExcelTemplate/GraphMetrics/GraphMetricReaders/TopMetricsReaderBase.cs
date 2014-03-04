
using System;
using System.Text;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TopMetricsReaderBase
//
/// <summary>
/// Base class for classes that read top metrics from a worksheet in a NodeXL
/// workbook.
/// </summary>
//*****************************************************************************

public class TopMetricsReaderBase : Object
{
    //*************************************************************************
    //  Constructor: TopMetricsReaderBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TopMetricsReaderBase" />
    /// class.
    /// </summary>
    ///
    /// <param name="worksheetName">
    /// Name of the worksheet containing the top metrics tables.
    /// </param>
    ///
    /// <param name="tableNameRoot">
    /// Root of the name of each top metric table on the worksheet.
    /// </param>
    //*************************************************************************

    public TopMetricsReaderBase
    (
        String worksheetName,
        String tableNameRoot
    )
    {
        m_sWorksheetName = worksheetName;
        m_sTableNameRoot = tableNameRoot;

        AssertValid();
    }

    //*************************************************************************
    //  Method: TryReadMetrics()
    //
    /// <summary>
    /// Attempts to read the top metrics from a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="topMetrics">
    /// Where a summary of the top metrics gets stored if true is returned.
    /// Gets set to an empty string if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method attempts to read the top metrics that have already been
    /// calculated and written to one or more tables in a worksheet in a NodeXL
    /// workbook.  If it successfully reads them, a summary of them gets stored
    /// at <paramref name="topMetrics" /> and true is returned.  false is
    /// returned otherwise.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryReadMetrics
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        out String topMetrics
    )
    {
        Debug.Assert(workbook != null);
        AssertValid();

        Worksheet oWorksheet;
        StringBuilder oTopMetrics = new StringBuilder();

        if ( ExcelUtil.TryGetWorksheet(workbook, m_sWorksheetName,
            out oWorksheet) )
        {
            foreach (ListObject oTopMetricsTable in oWorksheet.ListObjects)
            {
                if ( oTopMetricsTable.Name.StartsWith(m_sTableNameRoot) )
                {
                    ReadTopColumns(oTopMetricsTable, oTopMetrics);
                }
            }
        }

        topMetrics = oTopMetrics.ToString();
        return (topMetrics.Length > 0);
    }

    //*************************************************************************
    //  Method: ReadTopColumns()
    //
    /// <summary>
    /// Reads the "Top" columns in a table.
    /// </summary>
    ///
    /// <param name="oTopMetricsTable">
    /// The table to read the "Top" columns from.
    /// </param>
    ///
    /// <param name="oTopMetrics">
    /// Where the column contents get appended.
    /// </param>
    ///
    /// <remarks>
    /// This method provides a text representation of top items stored in a
    /// table.  It is meant for use with a table that has one or more columns
    /// whose column headers start with "Top " ("Top URLs", for example).  For
    /// each such column, this method appends the column header to <paramref
    /// name="oTopMetrics" />, then appends the contents of each cell in the
    /// column.
    /// </remarks>
    //*************************************************************************

    protected void
    ReadTopColumns
    (
        ListObject oTopMetricsTable,
        StringBuilder oTopMetrics
    )
    {
        Debug.Assert(oTopMetricsTable != null);
        Debug.Assert(oTopMetrics != null);

        foreach (ListColumn oColumn in oTopMetricsTable.ListColumns)
        {
            String sColumnHeader = oColumn.Name;

            if ( sColumnHeader.StartsWith("Top ") )
            {
                ReadTopColumn(oTopMetricsTable, sColumnHeader, oTopMetrics);
            }
        }
    }

    //*************************************************************************
    //  Method: ReadTopColumn()
    //
    /// <summary>
    /// Reads one "Top" column in a table.
    /// </summary>
    ///
    /// <param name="oTopMetricsTable">
    /// The table to read the "Top" column from.
    /// </param>
    ///
    /// <param name="sColumnHeader">
    /// Header text for the column to read.
    /// </param>
    ///
    /// <param name="oTopMetrics">
    /// Where the column contents get appended.
    /// </param>
    ///
    /// <remarks>
    /// This method appends the column header to <paramref
    /// name="oTopMetrics" />, then appends the contents of each cell in the
    /// column.
    /// </remarks>
    //*************************************************************************

    protected void
    ReadTopColumn
    (
        ListObject oTopMetricsTable,
        String sColumnHeader,
        StringBuilder oTopMetrics
    )
    {
        Debug.Assert(oTopMetricsTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnHeader) );
        Debug.Assert(oTopMetrics != null);

        StringBuilder oTopColumn = new StringBuilder();
        Boolean bColumnIsEmpty = true;

        oTopColumn.Append(sColumnHeader);
        oTopColumn.Append(':');

        ExcelTableReader oExcelTableReader =
            new ExcelTableReader(oTopMetricsTable);

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            String sItemName;

            if ( oRow.TryGetNonEmptyStringFromCell(sColumnHeader,
                out sItemName) )
            {
                StringUtil.AppendAfterEmptyLine(oTopColumn, sItemName);
                bColumnIsEmpty = false;
            }
        }

        if (!bColumnIsEmpty)
        {
            StringUtil.AppendSectionSeparator(oTopMetrics);
            oTopMetrics.Append( oTopColumn.ToString() );
        }
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
        Debug.Assert( !String.IsNullOrEmpty(m_sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(m_sTableNameRoot) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Name of the worksheet containing the top metrics tables.

    protected String m_sWorksheetName;

    /// Root of the name of each top metric table on the worksheet.

    protected String m_sTableNameRoot;
}

}
