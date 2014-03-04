
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorkbookImporter
//
/// <summary>
/// Imports an edge list from an open workbook into an <see cref="IGraph" />
/// object.
/// </summary>
///
/// <remarks>
/// Call <see cref="ImportWorkbook" /> to import an edge list from an open
/// workbook.
/// </remarks>
//*****************************************************************************

public class WorkbookImporter : WorkbookImporterBase
{
    //*************************************************************************
    //  Constructor: WorkbookImporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkbookImporter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public WorkbookImporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ImportWorkbook()
    //
    /// <summary>
    /// Imports an edge list from an open workbook into an <see
    /// cref="IGraph" /> object.
    /// </summary>
    ///
    /// <param name="application">
    /// Excel Application object.
    /// </param>
    ///
    /// <param name="sourceWorkbookName">
    /// Workbook.Name of the open workbook that contains the edge list to
    /// import.  The workbook's active worksheet can't be empty.
    /// </param>
    ///
    /// <param name="columnNumberToUseForVertex1OneBased">
    /// One-based column number to use for vertex 1.
    /// </param>
    ///
    /// <param name="columnNumberToUseForVertex2OneBased">
    /// One-based column number to use for vertex 2.
    /// </param>
    ///
    /// <param name="edgeColumnNumbersToImportOneBased">
    /// Collection of one-based edge column numbers to import from the source
    /// workbook, not including <paramref
    /// name="columnNumberToUseForVertex1OneBased" /> or <paramref
    /// name="columnNumberToUseForVertex2OneBased" />.  Can be empty.
    /// </param>
    ///
    /// <param name="vertex1ColumnNumbersToImportOneBased">
    /// Collection of one-based vertex 1 column numbers to import from the
    /// source workbook.  Can be empty.
    /// </param>
    ///
    /// <param name="vertex2ColumnNumbersToImportOneBased">
    /// Collection of one-based vertex 2 column numbers to import from the
    /// source workbook.  Can be empty.
    /// </param>
    ///
    /// <param name="sourceColumnsHaveHeaders">
    /// true if the columns have headers that should be ignored.
    /// </param>
    ///
    /// <returns>
    /// A new <see cref="IGraph" /> object.
    /// </returns>
    ///
    /// <remarks>
    /// This method imports the specified columns from the active worksheet of
    /// <paramref name="sourceWorkbookName" /> to a new <see cref="IGraph" />
    /// object.
    ///
    /// <para>
    /// The names of all the workbook's edge and vertex columns are stored as
    /// metadata on the returned <see cref="IGraph" /> object using the <see
    /// cref="ReservedMetadataKeys.AllEdgeMetadataKeys" /> and
    /// <see cref="ReservedMetadataKeys.AllVertexMetadataKeys" /> keys.
    /// </para>
    ///
    /// <para>
    /// If the columns can't be imported, an <see
    /// cref="ImportWorkbookException" /> is thrown.
    /// </para>
    ///
    /// <para>
    /// The columns are imported starting from either the first or second
    /// non-empty row of the source worksheet, depending on the value of
    /// <paramref name="sourceColumnsHaveHeaders" />, and ending at row N,
    /// where N is the last non-empty row of the source worksheet.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public IGraph
    ImportWorkbook
    (
        Microsoft.Office.Interop.Excel.Application application,
        String sourceWorkbookName,
        Int32 columnNumberToUseForVertex1OneBased,
        Int32 columnNumberToUseForVertex2OneBased,
        ICollection<Int32> edgeColumnNumbersToImportOneBased,
        ICollection<Int32> vertex1ColumnNumbersToImportOneBased,
        ICollection<Int32> vertex2ColumnNumbersToImportOneBased,
        Boolean sourceColumnsHaveHeaders
    )
    {
        Debug.Assert(application != null);
        Debug.Assert( !String.IsNullOrEmpty(sourceWorkbookName) );
        Debug.Assert(columnNumberToUseForVertex1OneBased >= 1);
        Debug.Assert(columnNumberToUseForVertex2OneBased >= 1);
        Debug.Assert(edgeColumnNumbersToImportOneBased != null);

        Debug.Assert( !edgeColumnNumbersToImportOneBased.Contains(
            columnNumberToUseForVertex1OneBased) );

        Debug.Assert( !edgeColumnNumbersToImportOneBased.Contains(
            columnNumberToUseForVertex2OneBased) );

        Debug.Assert(vertex1ColumnNumbersToImportOneBased != null);
        Debug.Assert(vertex2ColumnNumbersToImportOneBased != null);
        AssertValid();

        Worksheet oSourceWorksheet = GetActiveSourceWorksheet(application,
            sourceWorkbookName);

        return ( ImportWorkbookIntoGraph(oSourceWorksheet,
            GetNonEmptySourceRange(oSourceWorksheet),
            columnNumberToUseForVertex1OneBased,
            columnNumberToUseForVertex2OneBased,
            edgeColumnNumbersToImportOneBased,
            vertex1ColumnNumbersToImportOneBased,
            vertex2ColumnNumbersToImportOneBased, sourceColumnsHaveHeaders) );
    }

    //*************************************************************************
    //  Method: GetNonEmptySourceRange()
    //
    /// <summary>
    /// Gets the non-empty range in the source worksheet.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Worksheet to get the non-empty range from.
    /// </param>
    ///
    /// <returns>
    /// Non-empty range of <paramref name="oSourceWorksheet" />.
    /// </returns>
    //*************************************************************************

    protected Range
    GetNonEmptySourceRange
    (
        Worksheet oSourceWorksheet
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        AssertValid();

        Range oNonEmptySourceRange;

        if ( !ExcelUtil.TryGetNonEmptyRangeInWorksheet(oSourceWorksheet,
            out oNonEmptySourceRange) )
        {
            OnInvalidSourceWorkbook(
                "The active worksheet in the other workbook is empty.",
                oSourceWorksheet, 1, 1
                );
        }

        return (oNonEmptySourceRange);
    }

    //*************************************************************************
    //  Method: ImportWorkbookIntoGraph()
    //
    /// <summary>
    /// Imports an edge list from an open workbook into an <see
    /// cref="IGraph" /> object.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Source worksheet.
    /// </param>
    ///
    /// <param name="oNonEmptySourceRange">
    /// The non-empty range in the source worksheet.
    /// </param>
    ///
    /// <param name="iColumnNumberToUseForVertex1OneBased">
    /// One-based column number to use for vertex 1.
    /// </param>
    ///
    /// <param name="iColumnNumberToUseForVertex2OneBased">
    /// One-based column number to use for vertex 2.
    /// </param>
    ///
    /// <param name="oEdgeColumnNumbersToImportOneBased">
    /// Collection of one-based edge column numbers to import from the source
    /// workbook.  Can be empty.
    /// </param>
    ///
    /// <param name="oVertex1ColumnNumbersToImportOneBased">
    /// Collection of one-based vertex 1 column numbers to import from the
    /// source workbook.  Can be empty.
    /// </param>
    ///
    /// <param name="oVertex2ColumnNumbersToImportOneBased">
    /// Collection of one-based vertex 2 column numbers to import from the
    /// source workbook.  Can be empty.
    /// </param>
    ///
    /// <param name="bSourceColumnsHaveHeaders">
    /// true if the columns have headers that should be ignored.
    /// </param>
    ///
    /// <returns>
    /// A new <see cref="IGraph" /> object.
    /// </returns>
    //*************************************************************************

    protected IGraph
    ImportWorkbookIntoGraph
    (
        Worksheet oSourceWorksheet,
        Range oNonEmptySourceRange,
        Int32 iColumnNumberToUseForVertex1OneBased,
        Int32 iColumnNumberToUseForVertex2OneBased,
        ICollection<Int32> oEdgeColumnNumbersToImportOneBased,
        ICollection<Int32> oVertex1ColumnNumbersToImportOneBased,
        ICollection<Int32> oVertex2ColumnNumbersToImportOneBased,
        Boolean bSourceColumnsHaveHeaders
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        Debug.Assert(oNonEmptySourceRange != null);
        Debug.Assert(iColumnNumberToUseForVertex1OneBased >= 1);
        Debug.Assert(iColumnNumberToUseForVertex2OneBased >= 1);
        Debug.Assert(oEdgeColumnNumbersToImportOneBased != null);
        Debug.Assert(oVertex1ColumnNumbersToImportOneBased != null);
        Debug.Assert(oVertex2ColumnNumbersToImportOneBased != null);
        AssertValid();

        String [] asWorkbookColumnNames = GetWorkbookColumnNames(
            oSourceWorksheet, oNonEmptySourceRange, bSourceColumnsHaveHeaders);

        Int32 iColumns = oNonEmptySourceRange.Columns.Count;

        Int32 iFirstNonEmptyColumnOneBased =
            oNonEmptySourceRange.Columns.Column;

        if (bSourceColumnsHaveHeaders)
        {
            // Skip the header row.

            if (oNonEmptySourceRange.Rows.Count < 2)
            {
                OnInvalidSourceWorkbook(
                    "If the columns in the other workbook have headers, then"
                    + " there must be at least two rows.",

                    oSourceWorksheet, 1, 1
                    );
            }

            ExcelUtil.OffsetRange(ref oNonEmptySourceRange, 1, 0);

            ExcelUtil.ResizeRange(ref oNonEmptySourceRange,
                oNonEmptySourceRange.Rows.Count - 1, iColumns);
        }

        IGraph oGraph = new Graph(GraphDirectedness.Undirected);
        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;

        // The key is a vertex name and the value is the corresponding IVertex
        // object.

        Dictionary<String, IVertex> oVertexNameDictionary =
            new Dictionary<String, IVertex>();

        foreach ( Range oSubrange in
            ExcelRangeSplitter.SplitRange(oNonEmptySourceRange, 500) )
        {
            Object [,] oSubrangeValues = ExcelUtil.GetRangeValues(oSubrange);
            Int32 iSubrangeRows = oSubrangeValues.GetUpperBound(0);

            for (Int32 iRowOneBased = 1; iRowOneBased <= iSubrangeRows;
                iRowOneBased++)
            {
                String sVertex1Name, sVertex2Name;

                if (
                    !ExcelUtil.TryGetNonEmptyStringFromCell(oSubrangeValues,
                        iRowOneBased,
                        iColumnNumberToUseForVertex1OneBased -
                            iFirstNonEmptyColumnOneBased + 1,
                        out sVertex1Name)
                    ||
                    !ExcelUtil.TryGetNonEmptyStringFromCell(oSubrangeValues,
                        iRowOneBased,
                        iColumnNumberToUseForVertex2OneBased -
                            iFirstNonEmptyColumnOneBased + 1,
                        out sVertex2Name)
                    )
                {
                    continue;
                }

                IVertex oVertex1 = WorksheetReaderBase.VertexNameToVertex(
                    sVertex1Name, oVertices, oVertexNameDictionary);

                IVertex oVertex2 = WorksheetReaderBase.VertexNameToVertex(
                    sVertex2Name, oVertices, oVertexNameDictionary);

                IEdge oEdge = oEdges.Add(oVertex1, oVertex2);

                // Add the edge and vertex attributes.

                AddAttributeValuesToEdgeOrVertex(asWorkbookColumnNames,
                    oSubrangeValues, iRowOneBased,
                    iFirstNonEmptyColumnOneBased,
                    oEdgeColumnNumbersToImportOneBased, oEdge);

                AddAttributeValuesToEdgeOrVertex(asWorkbookColumnNames,
                    oSubrangeValues, iRowOneBased,
                    iFirstNonEmptyColumnOneBased,
                    oVertex1ColumnNumbersToImportOneBased, oVertex1);

                AddAttributeValuesToEdgeOrVertex(asWorkbookColumnNames,
                    oSubrangeValues, iRowOneBased,
                    iFirstNonEmptyColumnOneBased,
                    oVertex2ColumnNumbersToImportOneBased, oVertex2);
            }
        }

        // Store metadata on the graph indicating the sets of keys that may be
        // present on the graph's edges and vertices.

        oGraph.SetValue(ReservedMetadataKeys.AllEdgeMetadataKeys,

            GetColumnNamesToImport(oNonEmptySourceRange,
                asWorkbookColumnNames, oEdgeColumnNumbersToImportOneBased)
            );

        List<Int32> oVertexColumnNumbersToImportOneBased = new List<Int32>();

        oVertexColumnNumbersToImportOneBased.AddRange(
            oVertex1ColumnNumbersToImportOneBased);

        oVertexColumnNumbersToImportOneBased.AddRange(
            oVertex2ColumnNumbersToImportOneBased);

        oGraph.SetValue( ReservedMetadataKeys.AllVertexMetadataKeys,

            GetColumnNamesToImport(oNonEmptySourceRange, asWorkbookColumnNames,
                oVertexColumnNumbersToImportOneBased)
            );

        return (oGraph);
    }

    //*************************************************************************
    //  Method: GetWorkbookColumnNames()
    //
    /// <summary>
    /// Gets the column names from the open workbook.
    /// </summary>
    ///
    /// <param name="oSourceWorksheet">
    /// Source worksheet.
    /// </param>
    ///
    /// <param name="oNonEmptySourceRange">
    /// The non-empty range in the source worksheet.
    /// </param>
    ///
    /// <param name="bSourceColumnsHaveHeaders">
    /// true if the columns have headers that should be ignored.
    /// </param>
    ///
    /// <returns>
    /// An array of workbook column names.
    /// </returns>
    //*************************************************************************

    protected String[]
    GetWorkbookColumnNames
    (
        Worksheet oSourceWorksheet,
        Range oNonEmptySourceRange,
        Boolean bSourceColumnsHaveHeaders
    )
    {
        Debug.Assert(oSourceWorksheet != null);
        Debug.Assert(oNonEmptySourceRange != null);
        AssertValid();

        Int32 iOneBasedFirstRow = oNonEmptySourceRange.Row;
        Int32 iOneBasedFirstColumn = oNonEmptySourceRange.Column;
        Int32 iColumns = oNonEmptySourceRange.Columns.Count;
        String [] asWorkbookColumnNames = new String[iColumns];

        if (bSourceColumnsHaveHeaders)
        {
            Object [,] oHeaderValues = ExcelUtil.GetValuesInRow(
                oSourceWorksheet, iOneBasedFirstRow, iOneBasedFirstColumn,
                iColumns);

            for (Int32 i = 0; i < iColumns; i++)
            {
                String sColumnName;

                if ( !ExcelUtil.TryGetNonEmptyStringFromCell(oHeaderValues,
                    1, i + 1, out sColumnName) )
                {
                    OnInvalidSourceWorkbook(
                        "The header row in the other workbook has an empty"
                            + " cell.",

                        oSourceWorksheet, iOneBasedFirstRow,
                        iOneBasedFirstColumn + i
                        );
                }

                asWorkbookColumnNames[i] = sColumnName;
            }
        }
        else
        {
            for (Int32 i = 0; i < iColumns; i++)
            {
                asWorkbookColumnNames[i] = "Imported Column " +
                    (i + 1).ToString();
            }
        }

        return (asWorkbookColumnNames);
    }

    //*************************************************************************
    //  Method: GetColumnNamesToImport()
    //
    /// <summary>
    /// Gets the names of a collection of columns to import.
    /// </summary>
    ///
    /// <param name="oNonEmptySourceRange">
    /// The non-empty range in the source worksheet.
    /// </param>
    ///
    /// <param name="asWorkbookColumnNames">
    /// An array of workbook column names.
    /// </param>
    ///
    /// <param name="oColumnNumbersToImportOneBased">
    /// Collection of one-based edge column numbers to import from the source
    /// workbook.  Can be empty.
    /// </param>
    ///
    /// <returns>
    /// An array of column names to import.
    /// </returns>
    //*************************************************************************

    protected String []
    GetColumnNamesToImport
    (
        Range oNonEmptySourceRange,
        String [] asWorkbookColumnNames,
        ICollection<Int32> oColumnNumbersToImportOneBased
    )
    {
        Debug.Assert(oNonEmptySourceRange != null);
        Debug.Assert(asWorkbookColumnNames != null);
        Debug.Assert(oColumnNumbersToImportOneBased != null);
        AssertValid();

        Int32 iFirstNonEmptyColumnOneBased =
            oNonEmptySourceRange.Columns.Column;

        String [] asColumnNamesToImport = new String[
            oColumnNumbersToImportOneBased.Count];

        Int32 i = 0;

        foreach (Int32 iOneBasedColumnNumberToImport in
            oColumnNumbersToImportOneBased)
        {
            asColumnNamesToImport[i] = asWorkbookColumnNames[
                iOneBasedColumnNumberToImport -
                iFirstNonEmptyColumnOneBased];

            i++;
        }

        return (asColumnNamesToImport);
    }

    //*************************************************************************
    //  Method: AddAttributeValuesToEdgeOrVertex()
    //
    /// <summary>
    /// Reads attribute values from the source workbook and adds them to an
    /// edge or vertex.
    /// </summary>
    ///
    /// <param name="asWorkbookColumnNames">
    /// An array of workbook column names.
    /// </param>
    ///
    /// <param name="oSubrangeValues">
    /// Values read from part of the workbook.
    /// </param>
    ///
    /// <param name="iRowOneBased">
    /// One-based row number to read from <paramref name="oSubrangeValues" />.
    /// </param>
    ///
    /// <param name="iFirstNonEmptyColumnOneBased">
    /// One-based column number of the first non-empty column.
    /// </param>
    ///
    /// <param name="oColumnNumbersToImportOneBased">
    /// Collection of one-based column numbers to import from the source
    /// workbook for the edge or vertex.  Can be empty.
    /// </param>
    ///
    /// <param name="oEdgeOrVertex">
    /// Edge or vertex to store the attribute values on.
    /// </param>
    //*************************************************************************

    protected void
    AddAttributeValuesToEdgeOrVertex
    (
        String [] asWorkbookColumnNames,
        Object [,] oSubrangeValues,
        Int32 iRowOneBased,
        Int32 iFirstNonEmptyColumnOneBased,
        ICollection<Int32> oColumnNumbersToImportOneBased,
        IMetadataProvider oEdgeOrVertex
    )
    {
        Debug.Assert(asWorkbookColumnNames != null);
        Debug.Assert(oSubrangeValues != null);
        Debug.Assert(iRowOneBased >= 1);
        Debug.Assert(iFirstNonEmptyColumnOneBased >= 1);
        Debug.Assert(oColumnNumbersToImportOneBased != null);
        Debug.Assert(oEdgeOrVertex != null);
        AssertValid();

        foreach (Int32 iColumnNumberToImportOneBased in
            oColumnNumbersToImportOneBased)
        {
            String sAttributeValue;

            if ( ExcelUtil.TryGetNonEmptyStringFromCell(oSubrangeValues,
                iRowOneBased,

                iColumnNumberToImportOneBased -
                    iFirstNonEmptyColumnOneBased + 1,

                out sAttributeValue)
               )
            {
                String sColumnName = asWorkbookColumnNames[
                    iColumnNumberToImportOneBased -
                        iFirstNonEmptyColumnOneBased];

                oEdgeOrVertex.SetValue(sColumnName, sAttributeValue);
            }
        }
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
