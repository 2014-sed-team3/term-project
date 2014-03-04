
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CollapsedGroupAttributeAdder
//
/// <summary>
/// Contains utility methods for adding collapsed group attributes to a graph.
/// </summary>
///
/// <remarks>
/// Call <see cref="AddCollapsedGroupAttributes" /> to add collapsed group
/// attributes to a graph after the workbook is read.
/// </remarks>
//*****************************************************************************

public static class CollapsedGroupAttributeAdder
{
    //*************************************************************************
    //  Method: AddCollapsedGroupAttributes()
    //
    /// <summary>
    /// Adds collapsed group attributes to a graph after the workbook is read.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="readWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="graph">
    /// The graph read from the workbook.
    /// </param>
    ///
    /// <remarks>
    /// Each of the graph's groups, which are stored on the graph as <see
    /// cref="GroupInfo" /> objects, has a <see
    /// cref="GroupInfo.CollapsedAttributes" /> property that can contain
    /// attributes describing what the group should look like when it is
    /// collapsed.  Most of these attributes are set by the code that creates
    /// the groups; see <see cref="MotifCalculator2" />, for example.  Some
    /// attributes, however, can't be set until after the workbook is read, and
    /// those are the attributes that are added by this method.
    ///
    /// <para>
    /// For example, the color of the vertex that represents a collapsed motif
    /// depends on whether the color column on the vertex worksheet has been
    /// autofilled.  The user can autofill the workbook AFTER motifs are
    /// calculated, so <see cref="MotifCalculator2" /> can't reliably determine
    /// which color to use while it is calculating motifs.  It's up to this
    /// class to do that, after the workbook is read.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    AddCollapsedGroupAttributes
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ReadWorkbookContext readWorkbookContext,
        IGraph graph
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(readWorkbookContext != null);
        Debug.Assert(graph != null);

        GroupInfo[] aoGroups;
        ListObject oEdgeTable,oVertexTable;

        if (
            GroupUtil.TryGetGroups(graph, out aoGroups)
            &&
            ExcelTableUtil.TryGetTable(workbook, WorksheetNames.Edges,
                TableNames.Edges, out oEdgeTable)
            &&
            ExcelTableUtil.TryGetTable(workbook, WorksheetNames.Vertices,
                TableNames.Vertices, out oVertexTable)
            )
        {
            AddCollapsedGroupAttributesInternal(workbook, readWorkbookContext,
                oEdgeTable, oVertexTable, aoGroups);
        }
    }

    //*************************************************************************
    //  Method: AddCollapsedGroupAttributesInternal()
    //
    /// <summary>
    /// Adds collapsed group attributes to a graph after the workbook is read.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oEdgeTable">
    /// The workbook's edge table.
    /// </param>
    ///
    /// <param name="oVertexTable">
    /// The workbook's vertex table.
    /// </param>
    ///
    /// <param name="aoGroups">
    /// A non-empty array of ExcelTemplateGroupInfo objects, one for each group
    /// in the graph.
    /// </param>
    ///
    /// <remarks>
    /// This method adds <see
    /// cref="CollapsedGroupAttributeKeys.VertexColor" />, <see
    /// cref="CollapsedGroupAttributeKeys.GetAnchorVertexEdgeColorKey" /> and
    /// <see cref="CollapsedGroupAttributeKeys.GetAnchorVertexEdgeWidthKey" />
    /// attributes to the graph's motif groups where necessary.
    /// </remarks>
    //*************************************************************************

    private static void
    AddCollapsedGroupAttributesInternal
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ReadWorkbookContext oReadWorkbookContext,
        ListObject oEdgeTable,
        ListObject oVertexTable,
        GroupInfo[] aoGroups
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oEdgeTable != null);
        Debug.Assert(oVertexTable != null);
        Debug.Assert(aoGroups != null);
        Debug.Assert(aoGroups.Length > 0);

        // Check whether relevant columns have been autofilled using numerical
        // source columns.

        PerWorkbookSettings oPerWorkbookSettings =
            new PerWorkbookSettings(oWorkbook);

        AutoFillColorColumnResults oVertexColorResults =
            oPerWorkbookSettings.AutoFillWorkbookResults.VertexColorResults;

        Boolean bVertexColorColumnAutoFilled =
            oVertexColorResults.ColumnAutoFilled &&
            !oVertexColorResults.ColumnAutoFilledWithCategories;

        AutoFillColorColumnResults oEdgeColorResults =
            oPerWorkbookSettings.AutoFillWorkbookResults.EdgeColorResults;

        Boolean bEdgeColorColumnAutoFilled =
            oEdgeColorResults.ColumnAutoFilled &&
            !oEdgeColorResults.ColumnAutoFilledWithCategories;

        AutoFillNumericRangeColumnResults oEdgeWidthResults =
            oPerWorkbookSettings.AutoFillWorkbookResults.EdgeWidthResults;

        Boolean bEdgeWidthColumnAutoFilled =
            oEdgeWidthResults.ColumnAutoFilled;

        // Some user settings for autofill may be needed.
        //
        // Note: This is a design bug.  The user's current settings should not
        // be required; everything needed here should come from the autofill
        // results.  The long-term fix is to add a UseLogs property to the
        // AutoFillColorColumnResults class.

        AutoFillUserSettings oAutoFillUserSettings =
            new AutoFillUserSettings();

        ColorColumnAutoFillUserSettings oVertexColorDetails =
            oAutoFillUserSettings.VertexColorDetails;

        ColorColumnAutoFillUserSettings oEdgeColorDetails =
            oAutoFillUserSettings.EdgeColorDetails;

        NumericRangeColumnAutoFillUserSettings oEdgeWidthDetails =
            oAutoFillUserSettings.EdgeWidthDetails;

        // The key is the row ID for each visible row in the vertex table and
        // the value is value of the source column cell in that row that was
        // used to autofill the vertex color column, if it was autofilled.

        Dictionary<Int32, Object> oVertexColorSourceDictionary =
            bVertexColorColumnAutoFilled ?

            GetRowIDDictionary(oVertexTable,
                oVertexColorResults.SourceColumnName)
            :
            null;

        // Ditto for edge colors and edge widths.

        Dictionary<Int32, Object> oEdgeColorSourceDictionary =
            bEdgeColorColumnAutoFilled ?

            GetRowIDDictionary(oEdgeTable, oEdgeColorResults.SourceColumnName)
            :
            null;

        Dictionary<Int32, Object> oEdgeWidthSourceDictionary =
            bEdgeWidthColumnAutoFilled ?

            GetRowIDDictionary(oEdgeTable, oEdgeWidthResults.SourceColumnName)
            :
            null;

        // Only motifs need to have attributes added to them.

        foreach ( ExcelTemplateGroupInfo oGroup in aoGroups.Where(
            oGroup => oGroup.CollapsedAttributes != null) )
        {
            CollapsedGroupAttributes oCollapsedGroupAttributes =
                CollapsedGroupAttributes.FromString(
                    oGroup.CollapsedAttributes);

            String sType = oCollapsedGroupAttributes.GetGroupType();

            if (
                sType == CollapsedGroupAttributeValues.FanMotifType
                ||
                sType == CollapsedGroupAttributeValues.DConnectorMotifType
                ||
                sType == CollapsedGroupAttributeValues.CliqueMotifType
                )
            {
                AddVertexColorAttributeToMotif(oGroup, sType,
                    bVertexColorColumnAutoFilled, oVertexColorResults,
                    oVertexColorDetails, oVertexColorSourceDictionary,
                    oReadWorkbookContext, oCollapsedGroupAttributes);
            }

            if (sType == CollapsedGroupAttributeValues.DConnectorMotifType)
            {
                Int32 iAnchorVertices;

                if ( oCollapsedGroupAttributes.TryGetValue(
                    CollapsedGroupAttributeKeys.AnchorVertices,
                    out iAnchorVertices) )
                {
                    AddEdgeColorAttributesToDConnectorMotif(oGroup, 
                        bEdgeColorColumnAutoFilled, oEdgeColorResults,
                        oEdgeColorDetails, oEdgeColorSourceDictionary,
                        oReadWorkbookContext, oCollapsedGroupAttributes,
                        iAnchorVertices);

                    AddEdgeWidthAttributesToDConnectorMotif(oGroup,
                        bEdgeWidthColumnAutoFilled, oEdgeWidthResults,
                        oEdgeWidthDetails, oEdgeWidthSourceDictionary,
                        oReadWorkbookContext, oCollapsedGroupAttributes,
                        iAnchorVertices);
                }
            }

            oGroup.CollapsedAttributes = oCollapsedGroupAttributes.ToString();
        }
    }

    //*************************************************************************
    //  Method: AddVertexColorAttributeToMotif()
    //
    /// <summary>
    /// Adds a collapsed group attribute for vertex color to one fan,
    /// D-connector, or clique motif.
    /// </summary>
    ///
    /// <param name="oGroup">
    /// The group to add the attribute to.
    /// </param>
    ///
    /// <param name="sType">
    /// The type of the group.
    /// </param>
    ///
    /// <param name="bVertexColorColumnAutoFilled">
    /// true if the vertex color column was autofilled.
    /// </param>
    ///
    /// <param name="oVertexColorResults">
    /// Stores the results for autofilling the vertex color column.
    /// </param>
    ///
    /// <param name="oVertexColorDetails">
    /// User settings for autofilling the vertex color column, or null if the
    /// vertex color column was not autofilled.
    /// </param>
    ///
    /// <param name="oVertexColorSourceDictionary">
    /// The key is the row ID for each visible row in the vertex table and the
    /// value is the value of the source column cell in that row that was used
    /// to autofill the vertex color column.  This is null if the vertex color
    /// column wasn't autofilled.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oCollapsedGroupAttributes">
    /// Stores attributes that describe how a collapsed group should be
    /// displayed.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a <see
    /// cref="CollapsedGroupAttributeKeys.VertexColor" /> attribute to one
    /// group.
    /// </remarks>
    //*************************************************************************

    private static void
    AddVertexColorAttributeToMotif
    (
        ExcelTemplateGroupInfo oGroup,
        String sType,
        Boolean bVertexColorColumnAutoFilled,
        AutoFillColorColumnResults oVertexColorResults,
        ColorColumnAutoFillUserSettings oVertexColorDetails,
        Dictionary<Int32, Object> oVertexColorSourceDictionary,
        ReadWorkbookContext oReadWorkbookContext,
        CollapsedGroupAttributes oCollapsedGroupAttributes
    )
    {
        Debug.Assert(oGroup != null);
        Debug.Assert( !String.IsNullOrEmpty(sType) );
        Debug.Assert(oVertexColorResults != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oCollapsedGroupAttributes != null);

        Color oColor;

        // If the vertex color column was autofilled, get the average color
        // for the vertices in the motif.

        if (
            !bVertexColorColumnAutoFilled
            ||
            !TableColumnMapper.TryMapAverageColor(

                GetRowIDsToAverageForVertexColor(oGroup,
                    oCollapsedGroupAttributes, sType),

                oVertexColorSourceDictionary,
                oVertexColorResults.SourceCalculationNumber1,
                oVertexColorResults.SourceCalculationNumber2,
                oVertexColorResults.DestinationColor1,
                oVertexColorResults.DestinationColor2,
                oVertexColorDetails.UseLogs,
                out oColor)
            )
        {
            // Default to the color that was assigned to the group.

            oColor = oGroup.VertexColor;
        }

        oCollapsedGroupAttributes.Add(
            CollapsedGroupAttributeKeys.VertexColor,
            oReadWorkbookContext.ColorConverter2.GraphToWorkbook(oColor)
            );
    }

    //*************************************************************************
    //  Method: AddEdgeColorAttributesToDConnectorMotif()
    //
    /// <summary>
    /// Adds collapsed group attributes for edge colors to one D-connector
    /// motif.
    /// </summary>
    ///
    /// <param name="oGroup">
    /// The group to add the attributes to.
    /// </param>
    ///
    /// <param name="bEdgeColorColumnAutoFilled">
    /// true if the edge color column was autofilled.
    /// </param>
    ///
    /// <param name="oEdgeColorResults">
    /// Stores the results for autofilling the edge color column.
    /// </param>
    ///
    /// <param name="oEdgeColorDetails">
    /// User settings for autofilling the edge color column, or null if the
    /// edge color column was not autofilled.
    /// </param>
    ///
    /// <param name="oEdgeColorSourceDictionary">
    /// The key is the row ID for each visible row in the edge table and the
    /// value is the value of the source column cell in that row that was used
    /// to autofill the edge color column.  This is null if the edge color
    /// column wasn't autofilled.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oCollapsedGroupAttributes">
    /// Stores attributes that describe how a collapsed group should be
    /// displayed.
    /// </param>
    ///
    /// <param name="iAnchorVertices">
    /// Number of anchor vertices in the motif.
    /// </param>
    ///
    /// <remarks>
    /// This method adds <see
    /// cref="CollapsedGroupAttributeKeys.GetAnchorVertexEdgeColorKey" /> and
    /// <see cref="CollapsedGroupAttributeKeys.GetAnchorVertexEdgeColorKey" />
    /// attributes to one D-connector motif group.
    /// </remarks>
    //*************************************************************************

    private static void
    AddEdgeColorAttributesToDConnectorMotif
    (
        ExcelTemplateGroupInfo oGroup,
        Boolean bEdgeColorColumnAutoFilled,
        AutoFillColorColumnResults oEdgeColorResults,
        ColorColumnAutoFillUserSettings oEdgeColorDetails,
        Dictionary<Int32, Object> oEdgeColorSourceDictionary,
        ReadWorkbookContext oReadWorkbookContext,
        CollapsedGroupAttributes oCollapsedGroupAttributes,
        Int32 iAnchorVertices
    )
    {
        Debug.Assert(oGroup != null);
        Debug.Assert(oEdgeColorResults != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oCollapsedGroupAttributes != null);
        Debug.Assert(iAnchorVertices >= 0);

        // If the edge color column was autofilled, get the average color for
        // the edges incident to the D-connector motif's first anchor, then its
        // second anchor, and so on.  Otherwise, don't do anything.

        if (!bEdgeColorColumnAutoFilled)
        {
            return;
        }

        for (Int32 iAnchorVertexIndex = 0;
            iAnchorVertexIndex < iAnchorVertices;
            iAnchorVertexIndex++)
        {
            Color oAverageColor;

            if ( TableColumnMapper.TryMapAverageColor(

                    GetRowIDsToAverageForEdges(oGroup,
                        oCollapsedGroupAttributes, iAnchorVertexIndex),

                    oEdgeColorSourceDictionary,
                    oEdgeColorResults.SourceCalculationNumber1,
                    oEdgeColorResults.SourceCalculationNumber2,
                    oEdgeColorResults.DestinationColor1,
                    oEdgeColorResults.DestinationColor2,
                    oEdgeColorDetails.UseLogs,
                    out oAverageColor)
                )
            {
                oCollapsedGroupAttributes.Add(

                    CollapsedGroupAttributeKeys.GetAnchorVertexEdgeColorKey(
                        iAnchorVertexIndex),

                    oReadWorkbookContext.ColorConverter2.GraphToWorkbook(
                        oAverageColor)
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: AddEdgeWidthAttributesToDConnectorMotif()
    //
    /// <summary>
    /// Adds collapsed group attributes for edge widths to one D-connector
    /// motif.
    /// </summary>
    ///
    /// <param name="oGroup">
    /// The group to add the attributes to.
    /// </param>
    ///
    /// <param name="bEdgeWidthColumnAutoFilled">
    /// true if the edge width column was autofilled.
    /// </param>
    ///
    /// <param name="oEdgeWidthResults">
    /// Stores the results for autofilling the edge width column.
    /// </param>
    ///
    /// <param name="oEdgeWidthDetails">
    /// User settings for autofilling the edge width column, or null if the
    /// edge width column was not autofilled.
    /// </param>
    ///
    /// <param name="oEdgeWidthSourceDictionary">
    /// The key is the row ID for each visible row in the edge table and the
    /// value is the value of the source column cell in that row that was used
    /// to autofill the edge width column.  This is null if the edge width
    /// column wasn't autofilled.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oCollapsedGroupAttributes">
    /// Stores attributes that describe how a collapsed group should be
    /// displayed.
    /// </param>
    ///
    /// <param name="iAnchorVertices">
    /// Number of anchor vertices in the motif.
    /// </param>
    ///
    /// <remarks>
    /// This method adds <see
    /// cref="CollapsedGroupAttributeKeys.GetAnchorVertexEdgeWidthKey" /> and
    /// <see cref="CollapsedGroupAttributeKeys.GetAnchorVertexEdgeWidthKey" />
    /// attributes to one D-connector motif group.
    /// </remarks>
    //*************************************************************************

    private static void
    AddEdgeWidthAttributesToDConnectorMotif
    (
        ExcelTemplateGroupInfo oGroup,
        Boolean bEdgeWidthColumnAutoFilled,
        AutoFillNumericRangeColumnResults oEdgeWidthResults,
        NumericRangeColumnAutoFillUserSettings oEdgeWidthDetails,
        Dictionary<Int32, Object> oEdgeWidthSourceDictionary,
        ReadWorkbookContext oReadWorkbookContext,
        CollapsedGroupAttributes oCollapsedGroupAttributes,
        Int32 iAnchorVertices
    )
    {
        Debug.Assert(oGroup != null);
        Debug.Assert(oEdgeWidthResults != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oCollapsedGroupAttributes != null);
        Debug.Assert(iAnchorVertices >= 0);

        // If the edge width column was autofilled, get the average width for
        // the edges incident to the two-connector motif's first anchor, then
        // its second anchor.  Otherwise, don't do anything.

        if (!bEdgeWidthColumnAutoFilled)
        {
            return;
        }

        for (Int32 iAnchorVertexIndex = 0;
            iAnchorVertexIndex < iAnchorVertices;
            iAnchorVertexIndex++)
        {
            Double dAverageEdgeWidth;

            if ( TableColumnMapper.TryMapAverageNumber(

                    GetRowIDsToAverageForEdges(oGroup,
                        oCollapsedGroupAttributes, iAnchorVertexIndex),

                    oEdgeWidthSourceDictionary,
                    oEdgeWidthResults.SourceCalculationNumber1,
                    oEdgeWidthResults.SourceCalculationNumber2,
                    oEdgeWidthResults.DestinationNumber1,
                    oEdgeWidthResults.DestinationNumber2,
                    oEdgeWidthDetails.UseLogs,
                    out dAverageEdgeWidth
                    )
                )
            {
                oCollapsedGroupAttributes.Add(

                    CollapsedGroupAttributeKeys.GetAnchorVertexEdgeWidthKey(
                        iAnchorVertexIndex),

                    dAverageEdgeWidth
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: GetRowIDDictionary()
    //
    /// <summary>
    /// Gets a dictionary that maps table row IDs to numerical cell values for
    /// a specified column.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table to use.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the numerical table column to get cell values for.
    /// </param>
    ///
    /// <returns>
    /// The key is the row ID for each visible row in the table and the value
    /// is the numerical value of the cell in the specified column.
    /// </returns>
    ///
    /// <remarks>
    /// The values are Objects rather than Doubles to retain compatibility with
    /// the TableColumnMapper class, which deals with arbitrary cell values.
    /// </remarks>
    //*************************************************************************

    private static Dictionary<Int32, Object>
    GetRowIDDictionary
    (
        ListObject oTable,
        String sColumnName
    )
    {
        Debug.Assert(oTable != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );

        Dictionary<Int32, Object> oRowIDDictionary =
            new Dictionary<Int32, Object>();

        // The code that reads the table can handle hidden rows, but not hidden
        // columns.  Temporarily show all hidden columns in the table.

        ExcelHiddenColumns oHiddenColumns =
            ExcelColumnHider.ShowHiddenColumns(oTable);

        try
        {
            ExcelTableReader oExcelTableReader = new ExcelTableReader(oTable);

            foreach ( ExcelTableReader.ExcelTableRow oRow in
                oExcelTableReader.GetRows() )
            {
                Int32 iRowID;
                Double dCellValue;

                if (
                    oRow.TryGetInt32FromCell(CommonTableColumnNames.ID,
                        out iRowID)
                    &&
                    oRow.TryGetDoubleFromCell(sColumnName, out dCellValue)
                    )
                {
                    oRowIDDictionary[iRowID] = dCellValue;
                }
            }
        }
        finally
        {
            ExcelColumnHider.RestoreHiddenColumns(oTable, oHiddenColumns);
        }

        return (oRowIDDictionary);
    }

    //*************************************************************************
    //  Method: GetRowIDsToAverageForVertexColor()
    //
    /// <summary>
    /// Gets a collection of vertex row IDs for the vertices in a motif.
    /// </summary>
    ///
    /// <param name="oGroup">
    /// Group that represents the motif.
    /// </param>
    ///
    /// <param name="oCollapsedGroupAttributes">
    /// Stores attributes that describe how a collapsed group should be
    /// displayed.
    /// </param>
    ///
    /// <param name="sType">
    /// The type of the motif.
    /// </param>
    ///
    /// <returns>
    /// A collection of row IDs, one for each vertex in a motif whose numerical
    /// source column cell should be included in the average that is used to
    /// calculate the color of the collapsed group.
    /// </returns>
    //*************************************************************************

    private static IEnumerable<Int32>
    GetRowIDsToAverageForVertexColor
    (
        GroupInfo oGroup,
        CollapsedGroupAttributes oCollapsedGroupAttributes,
        String sType
    )
    {
        Debug.Assert(oGroup != null);
        Debug.Assert(oCollapsedGroupAttributes != null);
        Debug.Assert( !String.IsNullOrEmpty(sType) );

        List<Int32> oRowIDsOfRowsToAverage = new List<Int32>();
        IEnumerable<IVertex> oVerticesToAverage = null;

        switch (sType)
        {
            // Fan, Connector, and Clique fall through as each includes the proper vertices
            case CollapsedGroupAttributeValues.FanMotifType:

            case CollapsedGroupAttributeValues.DConnectorMotifType:

            case CollapsedGroupAttributeValues.CliqueMotifType:

                // All of the motif's vertices should be included.

                oVerticesToAverage = oGroup.Vertices;
                break;

            default:

                break;
        }

        if (oVerticesToAverage != null)
        {
            foreach (IVertex oVertex in oVerticesToAverage)
            {
                // The row ID is stored in the vertex's Tag, as long as the
                // vertex row isn't hidden.

                if (oVertex.Tag is Int32)
                {
                    oRowIDsOfRowsToAverage.Add( (Int32)oVertex.Tag );
                }
            }
        }

        return (oRowIDsOfRowsToAverage);
    }

    //*************************************************************************
    //  Method: GetRowIDsToAverageForEdges()
    //
    /// <summary>
    /// Gets a collection of edge row IDs for edges in a D-connector motif.
    /// </summary>
    ///
    /// <param name="oGroup">
    /// Group that represents the D-connector motif.
    /// </param>
    ///
    /// <param name="oCollapsedGroupAttributes">
    /// Stores attributes that describe how a collapsed group should be
    /// displayed.
    /// </param>
    ///
    /// <param name="iAnchorVertexIndex">
    /// Index of the anchor vertex to get the edge row IDs for.
    /// </param>
    ///
    /// <returns>
    /// A collection of row IDs, one for each of the edges incident to the
    /// specified anchor vertex of a D-connector motif.
    /// </returns>
    //*************************************************************************

    private static IEnumerable<Int32>
    GetRowIDsToAverageForEdges
    (
        GroupInfo oGroup,
        CollapsedGroupAttributes oCollapsedGroupAttributes,
        Int32 iAnchorVertexIndex
    )
    {
        Debug.Assert(oGroup != null);
        Debug.Assert(oCollapsedGroupAttributes != null);
        Debug.Assert(iAnchorVertexIndex >= 0);

        List<Int32> oRowIDsOfRowsToAverage = new List<Int32>();

        String sAnchorVertexName;

        if ( oCollapsedGroupAttributes.TryGetValue(

                CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(
                    iAnchorVertexIndex),

                out sAnchorVertexName) )
        {
            // The group's vertices are the span vertices.  Loop through them.

            foreach (IVertex oVertex in oGroup.Vertices)
            {
                // We need to find the edge that is incident to the specified
                // anchor vertex.

                foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
                {
                    IVertex oAdjacentVertex =
                        oIncidentEdge.GetAdjacentVertex(oVertex);

                    if (oAdjacentVertex.Name == sAnchorVertexName)
                    {
                        // The row ID is stored in the edge's Tag, as long as
                        // the edge row isn't hidden.

                        if (oIncidentEdge.Tag is Int32)
                        {
                            oRowIDsOfRowsToAverage.Add(
                                (Int32)oIncidentEdge.Tag);
                        }

                        break;
                    }
                }
            }
        }

        return (oRowIDsOfRowsToAverage);
    }
}

}
