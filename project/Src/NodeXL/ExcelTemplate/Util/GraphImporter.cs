
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphImporter
//
/// <summary>
/// Imports edges and vertices from a graph to the edge and vertex worksheets.
/// </summary>
///
/// <remarks>
/// This class is typically used when an external file is imported into a
/// NodeXL workbook.  The caller should first read the file into a NodeXL graph
/// using one of the graph adapter classes, then call the <see
/// cref="ImportGraph" /> method in this class to import the edges and vertices
/// from the graph into a NodeXL workbook.
/// </remarks>
//*****************************************************************************

public static class GraphImporter : Object
{
    //*************************************************************************
    //  Method: ImportGraph()
    //
    /// <summary>
    /// Imports edges and vertices from a graph to the edge and vertex
    /// worksheets of a NodeXL workbook.
    /// </summary>
    ///
    /// <param name="sourceGraph">
    /// Graph to import the edges and vertices from.
    /// </param>
    ///
    /// <param name="edgeAttributes">
    /// Array of edge attribute names that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="vertexAttributes">
    /// Array of vertex attribute names that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="clearTablesFirst">
    /// true if the NodeXL tables in <paramref
    /// name="destinationNodeXLWorkbook" /> should be cleared first.
    /// </param>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the edges and vertices will be imported into.
    /// </param>
    ///
    /// <remarks>
    /// This method creates a row in the edge worksheet for each edge in
    /// <paramref name="sourceGraph" />, and a row in the vertex worksheet for
    /// each edge.
    ///
    /// <para>
    /// For each attribute name in <paramref name="edgeAttributes" /> (if
    /// <paramref name="edgeAttributes" /> is not null), a column is added to
    /// the edge worksheet if the column doesn't already exist, and the
    /// corresponding attribute values stored on the edges are use to fill the
    /// column.  The same is done for <paramref name="vertexAttributes" /> and
    /// the vertex worksheet.
    /// </para>
    ///
    /// <para>
    /// If the graph is large and Excel's text wrapping is turned on, the
    /// import can be very slow.  Use <see cref="GraphImportTextWrapManager" />
    /// to turn text wrapping off when necessary.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    ImportGraph
    (
        IGraph sourceGraph,
        String [] edgeAttributes,
        String [] vertexAttributes,
        Boolean clearTablesFirst,
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook
    )
    {
        Debug.Assert(sourceGraph != null);
        Debug.Assert(destinationNodeXLWorkbook != null);

        if (clearTablesFirst)
        {
            NodeXLWorkbookUtil.ClearAllNodeXLTables(destinationNodeXLWorkbook);
        }

        // Get the required table that contains edge data.  GetEdgeTable()
        // throws an exception if the table is missing.

        EdgeWorksheetReader oEdgeWorksheetReader = new EdgeWorksheetReader();

        ListObject oEdgeTable =
            oEdgeWorksheetReader.GetEdgeTable(destinationNodeXLWorkbook);

        // Get the required columns.

        Range oVertex1NameColumnData = null;
        Range oVertex2NameColumnData = null;

        if (
            !ExcelTableUtil.TryGetTableColumnData(oEdgeTable,
                EdgeTableColumnNames.Vertex1Name, out oVertex1NameColumnData)
            ||
            !ExcelTableUtil.TryGetTableColumnData(oEdgeTable,
                EdgeTableColumnNames.Vertex2Name, out oVertex2NameColumnData)
            )
        {
            ErrorUtil.OnMissingColumn();
        }

        // Import the edges and their attributes into the workbook.

        ImportEdges(sourceGraph, edgeAttributes, oEdgeTable,
            oVertex1NameColumnData, oVertex2NameColumnData, !clearTablesFirst);

        // Populate the vertex worksheet with the name of each unique vertex in
        // the edge worksheet.

        ( new VertexWorksheetPopulator() ).PopulateVertexWorksheet(
            destinationNodeXLWorkbook, false);

        // Get the table that contains vertex data.

        ListObject oVertexTable;
        Range oVertexNameColumnData = null;
        Range oVisibilityColumnData = null;

        if (
            !ExcelTableUtil.TryGetTable(destinationNodeXLWorkbook,
                WorksheetNames.Vertices, TableNames.Vertices, out oVertexTable)
            ||
            !ExcelTableUtil.TryGetTableColumnData(oVertexTable,
                VertexTableColumnNames.VertexName, out oVertexNameColumnData)
            ||
            !ExcelTableUtil.TryGetTableColumnData(oVertexTable,
                CommonTableColumnNames.Visibility, out oVisibilityColumnData)
            )
        {
            ErrorUtil.OnMissingColumn();
        }

        // Import isolated vertices and the attributes for all the graph's
        // vertices.

        ImportVertices(sourceGraph, vertexAttributes, oVertexTable,
            oVertexNameColumnData, oVisibilityColumnData);
    }

    //*************************************************************************
    //  Method: GetImportedFileDescription()
    //
    /// <summary>
    /// Gets a string that describes how the graph was imported from a file.
    /// </summary>
    ///
    /// <param name="fileType">
    /// Type of the file that was imported.  Sample: "Pajek file".
    /// </param>
    ///
    /// <param name="fileName">
    /// Name of the file that was imported.
    /// </param>
    ///
    /// <returns>
    /// A description of how the graph was imported from a file.
    /// </returns>
    ///
    /// <remarks>
    /// The returned string can be used as the importDescription argument to
    /// <see cref="UpdateGraphHistoryAfterImport" />, for example.
    /// </remarks>
    //*************************************************************************

    public static String
    GetImportedFileDescription
    (
        String fileType,
        String fileName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(fileType) );
        Debug.Assert( !String.IsNullOrEmpty(fileName) );

        return ( String.Format(

            "The graph was imported from the {0} \"{1}\"."
            ,
            fileType,
            fileName
            ) );
    }

    //*************************************************************************
    //  Method: GetImportedGraphMLFileDescription()
    //
    /// <summary>
    /// Gets a string that describes how the graph was imported from a GraphML
    /// file.
    /// </summary>
    ///
    /// <param name="graphMLFileName">
    /// Name of the GraphML file that was imported.
    /// </param>
    ///
    /// <param name="graph">
    /// Graph into which the GraphML file was imported.
    /// </param>
    ///
    /// <returns>
    /// A description of how the graph was imported from a GraphML file.
    /// </returns>
    ///
    /// <remarks>
    /// The returned string can be used as the importDescription argument to
    /// <see cref="UpdateGraphHistoryAfterImport" />, for example.
    /// </remarks>
    //*************************************************************************

    public static String
    GetImportedGraphMLFileDescription
    (
        String graphMLFileName,
        IGraph graph
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFileName) );
        Debug.Assert(graph != null);

        // If the graph already has a description, use it.
        //
        // This can occur, for example, if the GraphML was imported from a file
        // created by the NetworkServer command line program, which stores a
        // description in the GraphML.

        String sImportedGraphMLFileDescription = (String)graph.GetValue(
            ReservedMetadataKeys.GraphDescription, typeof(String) );

        if ( String.IsNullOrEmpty(sImportedGraphMLFileDescription) )
        {
            sImportedGraphMLFileDescription = GetImportedFileDescription(
                "GraphML file", graphMLFileName);
        }

        return (sImportedGraphMLFileDescription);
    }

    //*************************************************************************
    //  Method: UpdateGraphHistoryAfterImport()
    //
    /// <summary>
    /// Updates the graph's history with details about how the graph was
    /// imported into a NodeXL workbook, if permitted by the user.
    /// </summary>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the edges and vertices were imported to.
    /// </param>
    ///
    /// <param name="importDescription">
    /// Description of the technique that was used to import the graph.  Can be
    /// empty or null.
    /// </param>
    ///
    /// <param name="suggestedFileNameNoExtension">
    /// File name suggested for the NodeXL workbook, without a path or
    /// extension.  Can be empty or null.
    /// </param>
    //*************************************************************************

    public static void
    UpdateGraphHistoryAfterImport
    (
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook,
        String importDescription,
        String suggestedFileNameNoExtension
    )
    {
        Debug.Assert(destinationNodeXLWorkbook != null);

        if ( ( new ImportDataUserSettings() ).SaveImportDescription )
        {
            UpdateGraphHistoryAfterImportWithoutPermissionCheck(
                destinationNodeXLWorkbook, importDescription,
                suggestedFileNameNoExtension,
                new PerWorkbookSettings(destinationNodeXLWorkbook) );
        }
    }

    //*************************************************************************
    //  Method: UpdateGraphHistoryAfterImportWithoutPermissionCheck()
    //
    /// <summary>
    /// Updates the graph's history with details about how the graph was
    /// imported into a NodeXL workbook, regardless of the user's permissions.
    /// </summary>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the edges and vertices were imported to.
    /// </param>
    ///
    /// <param name="importDescription">
    /// Description of the technique that was used to import the graph.  Can be
    /// empty or null.
    /// </param>
    ///
    /// <param name="suggestedFileNameNoExtension">
    /// File name suggested for the NodeXL workbook, without a path or
    /// extension.  Can be empty or null.
    /// </param>
    ///
    /// <param name="perWorkbookSettings">
    /// The per-workbook settings that contain the graph history.
    /// </param>
    //*************************************************************************

    public static void
    UpdateGraphHistoryAfterImportWithoutPermissionCheck
    (
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook,
        String importDescription,
        String suggestedFileNameNoExtension,
        PerWorkbookSettings perWorkbookSettings
    )
    {
        Debug.Assert(destinationNodeXLWorkbook != null);
        Debug.Assert(perWorkbookSettings != null);

        perWorkbookSettings.SetGraphHistoryValue(
            GraphHistoryKeys.ImportDescription,
            importDescription ?? String.Empty);

        perWorkbookSettings.SetGraphHistoryValue(
            GraphHistoryKeys.ImportSuggestedFileNameNoExtension,
            suggestedFileNameNoExtension ?? String.Empty);
    }

    //*************************************************************************
    //  Method: ImportEdges()
    //
    /// <summary>
    /// Imports edges and their attributes from a graph into the edge
    /// worksheet.
    /// </summary>
    ///
    /// <param name="oSourceGraph">
    /// Graph to import the edges from.
    /// </param>
    ///
    /// <param name="asEdgeAttributes">
    /// Array of edge attribute names that have been added to the metadata of
    /// the graph's edges.  Can be null.
    /// </param>
    ///
    /// <param name="oEdgeTable">
    /// Edge table the edges will be imported to.
    /// </param>
    ///
    /// <param name="oVertex1NameColumnData">
    /// Data body range of the vertex 1 name column.
    /// </param>
    ///
    /// <param name="oVertex2NameColumnData">
    /// Data body range of the vertex 2 name column.
    /// </param>
    ///
    /// <param name="bAppendToTable">
    /// true to append the edges to any edges already in the edge table, false
    /// to overwrite any edges.
    /// </param>
    //*************************************************************************

    private static void
    ImportEdges
    (
        IGraph oSourceGraph,
        String [] asEdgeAttributes,
        ListObject oEdgeTable,
        Range oVertex1NameColumnData,
        Range oVertex2NameColumnData,
        Boolean bAppendToTable
    )
    {
        Debug.Assert(oSourceGraph != null);
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oVertex1NameColumnData != null);
        Debug.Assert(oVertex2NameColumnData != null);

        Int32 iRowOffsetToWriteTo = 0;

        if (bAppendToTable)
        {
            iRowOffsetToWriteTo =
                ExcelTableUtil.GetOffsetOfFirstEmptyTableRow(oEdgeTable);

            ExcelUtil.OffsetRange(ref oVertex1NameColumnData,
                iRowOffsetToWriteTo, 0);

            ExcelUtil.OffsetRange(ref oVertex2NameColumnData,
                iRowOffsetToWriteTo, 0);
        }

        Range [] aoEdgeAttributeColumnData = null;
        Object [][,] aaoEdgeAttributeValues = null;
        Int32 iEdgeAttributes = 0;
        IEdgeCollection oEdges = oSourceGraph.Edges;
        Int32 iEdges = oEdges.Count;

        // Create vertex name and edge attribute arrays that will be written to
        // the edge table.

        Object [,] aoVertex1NameValues =
            ExcelUtil.GetSingleColumn2DArray(iEdges);

        Object [,] aoVertex2NameValues =
            ExcelUtil.GetSingleColumn2DArray(iEdges);

        if (asEdgeAttributes != null)
        {
            iEdgeAttributes = asEdgeAttributes.Length;
            aoEdgeAttributeColumnData = new Range[iEdgeAttributes];
            aaoEdgeAttributeValues = new Object[iEdgeAttributes][,];
            ListColumn oEdgeAttributeColumn;
            Range oEdgeAttributeColumnData;

            for (Int32 i = 0; i < iEdgeAttributes; i++)
            {
                GetAttributeColumn(oEdgeTable, asEdgeAttributes[i],
                    out oEdgeAttributeColumn, out oEdgeAttributeColumnData);

                if (bAppendToTable)
                {
                    ExcelUtil.OffsetRange(ref oEdgeAttributeColumnData,
                        iRowOffsetToWriteTo, 0);
                }

                aoEdgeAttributeColumnData[i] = oEdgeAttributeColumnData;

                aaoEdgeAttributeValues[i] =
                    ExcelUtil.GetSingleColumn2DArray(iEdges);
            }
        }

        // Fill in the vertex name and edge attribute arrays.

        Int32 iEdge = 1;

        foreach (IEdge oEdge in oEdges)
        {
            IVertex [] aoVertices = oEdge.Vertices;

            aoVertex1NameValues[iEdge, 1] = aoVertices[0].Name;
            aoVertex2NameValues[iEdge, 1] = aoVertices[1].Name;

            Object oEdgeAttribute;

            for (Int32 i = 0; i < iEdgeAttributes; i++)
            {
                if ( oEdge.TryGetValue(asEdgeAttributes[i],
                    out oEdgeAttribute) )
                {
                    aaoEdgeAttributeValues[i][iEdge, 1] =
                        CleanUpAttributeValue(oEdgeAttribute);
                }
            }

            iEdge++;
        }

        // Write the vertex name and edge attribute arrays to the table.

        SetRangeValues( (Range)oVertex1NameColumnData.Cells[1, 1],
            aoVertex1NameValues, false );

        SetRangeValues( (Range)oVertex2NameColumnData.Cells[1, 1],
            aoVertex2NameValues, false );

        for (Int32 i = 0; i < iEdgeAttributes; i++)
        {
            SetRangeValues( (Range)aoEdgeAttributeColumnData[i].Cells[1, 1],
                aaoEdgeAttributeValues[i], true );
        }
    }

    //*************************************************************************
    //  Method: ImportVertices()
    //
    /// <summary>
    /// Imports vertices and their attributes from a graph into the vertex
    /// worksheet.
    /// </summary>
    ///
    /// <param name="oSourceGraph">
    /// Graph to import the edges from.
    /// </param>
    ///
    /// <param name="asVertexAttributes">
    /// Array of vertex attribute names that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// Vertex table the vertices will be imported to.
    /// </param>
    ///
    /// <param name="oVertexNameColumnData">
    /// Data body range of the vertex name column.
    /// </param>
    ///
    /// <param name="oVisibilityColumnData">
    /// Data body range of the vertex visibility column.
    /// </param>
    //*************************************************************************

    private static void
    ImportVertices
    (
        IGraph oSourceGraph,
        String [] asVertexAttributes,
        ListObject oVertexTable,
        Range oVertexNameColumnData,
        Range oVisibilityColumnData
    )
    {
        Debug.Assert(oSourceGraph != null);
        Debug.Assert(oVertexTable != null);
        Debug.Assert(oVertexNameColumnData != null);
        Debug.Assert(oVisibilityColumnData != null);

        // Create a dictionary that maps vertex names to row numbers in the
        // vertex worksheet.

        Dictionary<String, Int32> oVertexDictionary =
            new Dictionary<String, Int32>();

        Object [,] aoVertexNameValues =
            ExcelUtil.GetRangeValues(oVertexNameColumnData);

        Int32 iRows = oVertexNameColumnData.Rows.Count;

        if (iRows == 1 && aoVertexNameValues[1, 1] == null)
        {
            // Range.get_Value() (and therefore ExcelUtil.GetRangeValues())
            // returns a single null cell when the table is empty.  Work around
            // this.

            iRows = 0;
        }

        for (Int32 iRowOneBased = 1; iRowOneBased <= iRows; iRowOneBased++)
        {
            String sVertexName;

            if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoVertexNameValues,
                iRowOneBased, 1, out sVertexName) )
            {
                oVertexDictionary[sVertexName] = iRowOneBased;
            }
        }

        aoVertexNameValues = null;

        // Create a list of vertices not already included in the vertex table. 
        // This can occur when the graph has isolated vertices.

        List<String> oIsolatedVertexNames = new List<String>();

        foreach (IVertex oVertex in oSourceGraph.Vertices)
        {
            String sVertexName = oVertex.Name;

            if ( !oVertexDictionary.ContainsKey(sVertexName) )
            {
                oIsolatedVertexNames.Add(sVertexName);
            }
        }

        Int32 iIsolatedVertices = oIsolatedVertexNames.Count;

        if (iIsolatedVertices > 0)
        {
            // Append the isolated vertices to the table.  The vertex
            // visibilities should be set to Show to force them to be shown
            // even though they are not included in edges.

            String [,] asAddedVertexNameValues =
                new String [iIsolatedVertices, 1];

            String [,] asAddedVisibilityValues =
                new String [iIsolatedVertices, 1];

            String sShow = ( new VertexVisibilityConverter() ).GraphToWorkbook(
                VertexWorksheetReader.Visibility.Show);

            for (Int32 i = 0; i < iIsolatedVertices; i++)
            {
                String sIsolatedVertexName = oIsolatedVertexNames[i];
                asAddedVertexNameValues[i, 0] = sIsolatedVertexName;
                asAddedVisibilityValues[i, 0] = sShow;
                oVertexDictionary[sIsolatedVertexName] = iRows + i + 1;
            }

            SetRangeValues(oVertexNameColumnData.get_Offset(iRows, 0),
                asAddedVertexNameValues, false);

            SetRangeValues(oVisibilityColumnData.get_Offset(iRows, 0),
                asAddedVisibilityValues, false);
        }

        if (asVertexAttributes != null)
        {
            ImportVertexAttributes(oSourceGraph, asVertexAttributes,
                oVertexDictionary, oVertexTable);
        }
    }

    //*************************************************************************
    //  Method: ImportVertexAttributes()
    //
    /// <summary>
    /// Imports attributes from the graph's vertices to the vertex worksheet.
    /// </summary>
    ///
    /// <param name="oSourceGraph">
    /// Graph to import the edges from.
    /// </param>
    ///
    /// <param name="asVertexAttributes">
    /// Array of vertex attribute names that have been added to the metadata of
    /// the graph's vertices.  Can't be null.
    /// </param>
    ///
    /// <param name="oVertexDictionary">
    /// The key is a vertex name and the value is the one-based row offset.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// Vertex table the vertices will be imported to.
    /// </param>
    //*************************************************************************

    private static void
    ImportVertexAttributes
    (
        IGraph oSourceGraph,
        String [] asVertexAttributes,
        Dictionary<String, Int32> oVertexDictionary,
        ListObject oVertexTable
    )
    {
        Debug.Assert(oSourceGraph != null);
        Debug.Assert(asVertexAttributes != null);
        Debug.Assert(oVertexDictionary != null);
        Debug.Assert(oVertexTable != null);

        // Create vertex attribute arrays that will be written to the vertex
        // table.

        Int32 iVertexAttributes = asVertexAttributes.Length;

        Range [] aoVertexAttributeColumnData = new Range[iVertexAttributes];

        Object [][,] aaoVertexAttributeValues =
            new Object[iVertexAttributes][,];

        ListColumn oVertexAttributeColumn;
        Range oVertexAttributeColumnData;

        for (Int32 i = 0; i < iVertexAttributes; i++)
        {
            GetAttributeColumn(oVertexTable, asVertexAttributes[i],
                out oVertexAttributeColumn, out oVertexAttributeColumnData);

            aoVertexAttributeColumnData[i] = oVertexAttributeColumnData;

            aaoVertexAttributeValues[i] =
                ExcelUtil.GetRangeValues(oVertexAttributeColumnData);
        }

        foreach (IVertex oVertex in oSourceGraph.Vertices)
        {
            String sVertexName = oVertex.Name;
            Object oVertexAttribute;

            Int32 iOneBasedRowOffset;

            if ( !oVertexDictionary.TryGetValue(sVertexName,
                out iOneBasedRowOffset) )
            {
                Debug.Assert(false);
            }

            for (Int32 i = 0; i < iVertexAttributes; i++)
            {
                if ( oVertex.TryGetValue(asVertexAttributes[i],
                    out oVertexAttribute) )
                {
                    Debug.Assert(iOneBasedRowOffset >= 1);

                    Debug.Assert( iOneBasedRowOffset <=
                        aaoVertexAttributeValues[i].GetUpperBound(0) );

                    aaoVertexAttributeValues[i][iOneBasedRowOffset, 1] =
                        CleanUpAttributeValue(oVertexAttribute);
                }
            }
        }

        for (Int32 i = 0; i < iVertexAttributes; i++)
        {
            SetRangeValues(aoVertexAttributeColumnData[i],
                aaoVertexAttributeValues[i], true);
        }
    }

    //*************************************************************************
    //  Method: GetAttributeColumn()
    //
    /// <summary>
    /// Gets an attribute column.
    /// </summary>
    ///
    /// <param name="oTable">
    /// Table containing the column.
    /// </param>
    ///
    /// <param name="sAttribute">
    /// Name of the attribute.
    /// </param>
    ///
    /// <param name="oAttributeColumn">
    /// Where the attribute column gets stored.
    /// </param>
    ///
    /// <param name="oAttributeColumnData">
    /// Where the column data range gets stored if true is returned.
    /// </param>
    ///
    /// <remarks>
    /// The column is added if it doesn't already exist.
    /// </remarks>
    //*************************************************************************

    private static void
    GetAttributeColumn
    (
        ListObject oTable,
        String sAttribute,
        out ListColumn oAttributeColumn,
        out Range oAttributeColumnData
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sAttribute) );

        if ( !ExcelTableUtil.TryGetTableColumn(oTable, sAttribute,
            out oAttributeColumn) )
        {
            if ( !ExcelTableUtil.TryAddTableColumn(oTable, sAttribute,
                ExcelTableUtil.AutoColumnWidth, null, out oAttributeColumn) )
            {
                goto CannotGetColumn;
            }

            // Wrap the text in the new column's header.

            ExcelTableUtil.WrapTableColumnHeader(oAttributeColumn);

            // This sometimes wraps a single-word header.  Fix it.

            oAttributeColumn.Range.EntireColumn.AutoFit();
        }

        if ( ExcelTableUtil.TryGetTableColumnData(oAttributeColumn,
            out oAttributeColumnData) )
        {
            // Success.

            return;
        }

        CannotGetColumn:

        throw new WorkbookFormatException(
            "The " + sAttribute + " column couldn't be added."
            );
    }

    //*************************************************************************
    //  Method: CleanUpAttributeValue()
    //
    /// <summary>
    /// Cleans up an attribute value so it doesn't raise an exception when
    /// written to a cell.
    /// </summary>
    ///
    /// <param name="value">
    /// The value to clean up.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// The cleaned-up value.
    /// </returns>
    //*************************************************************************

    private static Object
    CleanUpAttributeValue
    (
        Object value
    )
    {
        return ( ExcelUtil.RemoveFormulaFromValue(
            ExcelUtil.LimitStringValueLength(value) ) );
    }

    //*************************************************************************
    //  Method: SetRangeValues()
    //
    /// <summary>
    /// Sets the values on a range.
    /// </summary>
    ///
    /// <param name="oUpperLeftCornerMarker">
    /// The values are set on the parent worksheet starting at this range's
    /// upper-left corner.  Only those cells that correspond to <paramref
    /// name="aoValues" /> are set, so the only requirement for this range is
    /// that it's upper-left corner be in the desired location.  Its size is
    /// unimportant.
    /// </param>
    ///
    /// <param name="aoValues">
    /// The values to set.  The array indexes can be either zero-based or
    /// one-based.
    /// </param>
    ///
    /// <param name="bConvertUrlsToHyperlinks">
    /// true to convert each URL found in the values to an Excel hyperlink.
    /// </param>
    ///
    /// <remarks>
    /// This method copies <paramref name="aoValues" /> to the parent worksheet
    /// of the <paramref name="oUpperLeftCornerMarker" /> range, starting at
    /// the range's upper-left corner.  If the upper-left corner is B2 and
    /// <paramref name="aoValues" /> is 3 rows by 2 columns, for example, then
    /// the values are copied to B2:C4.
    /// </remarks>
    //*************************************************************************

    private static void
    SetRangeValues
    (
        Range oUpperLeftCornerMarker,
        Object [,] aoValues,
        Boolean bConvertUrlsToHyperlinks
    )
    {
        Debug.Assert(oUpperLeftCornerMarker != null);
        Debug.Assert(aoValues != null);

        Range oActualRange = ExcelUtil.SetRangeValues(oUpperLeftCornerMarker,
            aoValues, true);

        if (bConvertUrlsToHyperlinks)
        {
            ExcelUtil.ConvertUrlsToHyperlinks(oActualRange);
        }

        // If a cell value contains line breaks, Excel increases the row height
        // to make all the lines in the cell visible.  It does this even if the
        // column's formatting is set to not wrap text before the values are
        // written.  Setting WrapText to false after the values are written
        // sets the row back to the default height.
        //
        // Unfortunately, this introduces another bug: the cell borders
        // sometime disappear when wrap text is turned off here.  For now,
        // that's a less annoying bug than the row height problem.

        oActualRange.WrapText = false;
    }
}

}
