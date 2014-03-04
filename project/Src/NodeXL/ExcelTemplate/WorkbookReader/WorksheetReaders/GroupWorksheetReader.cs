
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupWorksheetReader
//
/// <summary>
/// Class that knows how to read Excel worksheets containing group data.
/// </summary>
///
/// <remarks>
/// Call <see cref="ReadWorksheet" /> to read the group worksheets.
/// </remarks>
//*****************************************************************************

public class GroupWorksheetReader : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: GroupWorksheetReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupWorksheetReader" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GroupWorksheetReader()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Enum: Visibility
    //
    /// <summary>
    /// Specifies the visibility of a group.
    /// </summary>
    //*************************************************************************

    public enum
    Visibility
    {
        /// <summary>
        /// Read the group, its vertices and its edges into the graph and show
        /// them.  This is the default.
        /// </summary>

        Show,

        /// <summary>
        /// Skip the group, its vertices and its edges.  Do not read them into
        /// the graph.
        /// </summary>

        Skip,

        /// <summary>
        /// Read the group, its vertices and its edges into the graph, but hide
        /// them.
        /// </summary>

        Hide,
    }

    //*************************************************************************
    //  Method: ReadWorksheet()
    //
    /// <summary>
    /// Reads the group worksheets and adds the contents to a graph.
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
    /// Graph to add group data to.
    /// </param>
    ///
    /// <remarks>
    /// If the group worksheets in <paramref name="workbook" /> contain valid
    /// group data, the data is added to <paramref name="graph" />.  Otherwise,
    /// a <see cref="WorkbookFormatException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    public void
    ReadWorksheet
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ReadWorkbookContext readWorkbookContext,
        IGraph graph
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(readWorkbookContext != null);
        Debug.Assert(graph != null);
        AssertValid();

        // Attempt to get the optional tables that contain group data.

        ListObject oGroupTable, oGroupVertexTable;

        if (
            ExcelTableUtil.TryGetTable(workbook, WorksheetNames.Groups,
                TableNames.Groups, out oGroupTable)
            &&
            ExcelTableUtil.TryGetTable(workbook, WorksheetNames.GroupVertices,
                TableNames.GroupVertices, out oGroupVertexTable)
            )
        {
            // The code that reads the tables can handle hidden rows, but not
            // hidden columns.  Temporarily show all hidden columns in the
            // table.

            ExcelHiddenColumns oHiddenGroupColumns =
                ExcelColumnHider.ShowHiddenColumns(oGroupTable);

            ExcelHiddenColumns oHiddenGroupVertexColumns =
                ExcelColumnHider.ShowHiddenColumns(oGroupVertexTable);

            try
            {
                ReadGroupTables(oGroupTable, oGroupVertexTable,
                    readWorkbookContext, graph);
            }
            finally
            {
                ExcelColumnHider.RestoreHiddenColumns(oGroupTable,
                    oHiddenGroupColumns);

                ExcelColumnHider.RestoreHiddenColumns(oGroupVertexTable,
                    oHiddenGroupVertexColumns);
            }
        }
    }

    //*************************************************************************
    //  Method: VertexIsCollapsedGroup()
    //
    /// <summary>
    /// Returns a flag indicating whether a vertex represents a collapsed
    /// group.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex to check.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="vertex" /> represents a collapsed group, false
    /// if it is a normal vertex.
    /// </returns>
    ///
    /// <remarks>
    /// A collapsed group is represented by a vertex that differs in appearance
    /// from a regular vertex.  It corresponds to a row in the group worksheet
    /// instead of a row in the vertex worksheet.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    VertexIsCollapsedGroup
    (
        IVertex vertex
    )
    {
        Debug.Assert(vertex != null);

        // A normal vertex has its Tag set to the vertex's row ID in the vertex
        // worksheet.  A vertex that represents a collapsed group does not have
        // its Tag set.

        return ( !(vertex.Tag is Int32) );
    }

    //*************************************************************************
    //  Method: ReadGroupTables()
    //
    /// <summary>
    /// Reads the group tables and add the contents to a graph.
    /// </summary>
    ///
    /// <param name="oGroupTable">
    /// Table that contains the group data.
    /// </param>
    ///
    /// <param name="oGroupVertexTable">
    /// Table that contains the group vertex data.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add group data to.
    /// </param>
    //*************************************************************************

    protected void
    ReadGroupTables
    (
        ListObject oGroupTable,
        ListObject oGroupVertexTable,
        ReadWorkbookContext oReadWorkbookContext,
        IGraph oGraph
    )
    {
        Debug.Assert(oGroupTable != null);
        Debug.Assert(oGroupVertexTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        // If a required column is missing, do nothing.

        ListColumn oColumn;

        if (
            !ExcelTableUtil.TryGetTableColumn(oGroupTable,
                GroupTableColumnNames.Name, out oColumn)
            ||
            !ExcelTableUtil.TryGetTableColumn(oGroupTable,
                GroupTableColumnNames.VertexColor, out oColumn)
            ||
            !ExcelTableUtil.TryGetTableColumn(oGroupTable,
                GroupTableColumnNames.VertexShape, out oColumn)
            ||
            !ExcelTableUtil.TryGetTableColumn(oGroupVertexTable,
                GroupVertexTableColumnNames.GroupName, out oColumn)
            ||
            !ExcelTableUtil.TryGetTableColumn(oGroupVertexTable,
                GroupVertexTableColumnNames.VertexName, out oColumn)
            )
        {
            return;
        }

        // These are the names of the groups that should be skipped or hidden.

        HashSet<String> oSkippedGroupNames = new HashSet<String>();
        HashSet<String> oHiddenGroupNames = new HashSet<String>();

        // Create a dictionary from the group table.  The key is the group name
        // and the value is an ExcelTemplateGroupInfo object for the group.

        Dictionary<String, ExcelTemplateGroupInfo> oGroupNameDictionary =
            ReadGroupTable(oGroupTable, oReadWorkbookContext,
                oSkippedGroupNames, oHiddenGroupNames);

        // Read the group vertex table and set the color and shape of each
        // group vertex in the graph.

        ReadGroupVertexTable(oGroupVertexTable, oReadWorkbookContext,
            oGroupNameDictionary, oGraph);

        // Now that the groups and the vertices they contain are known, skip
        // and hide those groups that should be skipped or hidden.

        SkipAndHideGroups(oReadWorkbookContext, oGroupNameDictionary,
            oSkippedGroupNames, oHiddenGroupNames, oGraph);

        if (oGroupNameDictionary.Count > 0)
        {
            // Save the group information on the graph.

            Debug.Assert( oGroupNameDictionary.Values is
                ICollection<ExcelTemplateGroupInfo> );

            oGraph.SetValue(ReservedMetadataKeys.GroupInfo,
                oGroupNameDictionary.Values.ToArray() );
        }
    }

    //*************************************************************************
    //  Method: ReadGroupTable()
    //
    /// <summary>
    /// Reads the group table.
    /// </summary>
    ///
    /// <param name="oGroupTable">
    /// The group table.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oSkippedGroupNames">
    /// This gets populated with the names of groups that should be skipped.
    /// </param>
    ///
    /// <param name="oHiddenGroupNames">
    /// This gets populated with the names of groups that should be hidden.
    /// </param>
    ///
    /// <returns>
    /// A dictionary.  The key is the group name and the value is an
    /// ExcelTemplateGroupInfo object for the group.
    /// </returns>
    //*************************************************************************

    protected Dictionary<String, ExcelTemplateGroupInfo>
    ReadGroupTable
    (
        ListObject oGroupTable,
        ReadWorkbookContext oReadWorkbookContext,
        HashSet<String> oSkippedGroupNames,
        HashSet<String> oHiddenGroupNames
    )
    {
        Debug.Assert(oGroupTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oSkippedGroupNames != null);
        Debug.Assert(oHiddenGroupNames != null);
        AssertValid();

        if (oReadWorkbookContext.FillIDColumns)
        {
            FillIDColumn(oGroupTable);
        }

        Dictionary<String, ExcelTemplateGroupInfo> oGroupNameDictionary =
            new Dictionary<String, ExcelTemplateGroupInfo>();

        ColorConverter2 oColorConverter2 =
            oReadWorkbookContext.ColorConverter2;

        GroupVisibilityConverter oGroupVisibilityConverter =
            new GroupVisibilityConverter();

        BooleanConverter oBooleanConverter =
            oReadWorkbookContext.BooleanConverter;

        ExcelTableReader oExcelTableReader = new ExcelTableReader(oGroupTable);

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            // Get the group information.

            String sGroupName;
            Color oVertexColor;
            VertexShape eVertexShape;

            if (
                !oRow.TryGetNonEmptyStringFromCell(GroupTableColumnNames.Name,
                    out sGroupName)
                ||
                !TryGetColor(oRow, GroupTableColumnNames.VertexColor,
                    oColorConverter2, out oVertexColor)
                ||
                !TryGetVertexShape(oRow, GroupTableColumnNames.VertexShape,
                    out eVertexShape)
                )
            {
                continue;
            }

            ReadVisibility(oRow, oGroupVisibilityConverter, sGroupName,
                oSkippedGroupNames, oHiddenGroupNames);

            Boolean bCollapsed = false;
            Boolean bCollapsedCellValue;

            if (
                TryGetBoolean(oRow, GroupTableColumnNames.Collapsed,
                    oBooleanConverter, out bCollapsedCellValue)
                &&
                bCollapsedCellValue
                )
            {
                bCollapsed = true;
            }

            String sCollapsedAttributes;

            if ( !oRow.TryGetNonEmptyStringFromCell(
                GroupTableColumnNames.CollapsedAttributes,
                out sCollapsedAttributes) )
            {
                sCollapsedAttributes = null;
            }

            Int32 iRowIDAsInt32;
            Nullable<Int32> iRowID = null;

            if ( oRow.TryGetInt32FromCell(CommonTableColumnNames.ID,
                out iRowIDAsInt32) )
            {
                iRowID = iRowIDAsInt32;
            }

            ExcelTemplateGroupInfo oExcelTemplateGroupInfo =
                new ExcelTemplateGroupInfo(sGroupName, iRowID, oVertexColor,
                    eVertexShape, bCollapsed, sCollapsedAttributes);

            if (oReadWorkbookContext.ReadGroupLabels)
            {
                String sLabel;

                if ( oRow.TryGetNonEmptyStringFromCell(
                    GroupTableColumnNames.Label, out sLabel) )
                {
                    oExcelTemplateGroupInfo.Label = sLabel;
                }
            }

            if (!oReadWorkbookContext.IgnoreVertexLocations)
            {
                System.Drawing.PointF oCollapsedLocation;

                if ( TryGetLocation(oRow, GroupTableColumnNames.CollapsedX,
                    GroupTableColumnNames.CollapsedY,
                    oReadWorkbookContext.VertexLocationConverter,
                    out oCollapsedLocation) )
                {
                    oExcelTemplateGroupInfo.CollapsedLocation =
                        oCollapsedLocation;
                }
            }

            try
            {
                oGroupNameDictionary.Add(sGroupName, oExcelTemplateGroupInfo);
            }
            catch (ArgumentException)
            {
                Range oInvalidCell = oRow.GetRangeForCell(
                    GroupTableColumnNames.Name);

                OnWorkbookFormatError( String.Format(

                    "The cell {0} contains a duplicate group name.  There"
                    + " can't be two rows with the same group name."
                    ,
                    ExcelUtil.GetRangeAddress(oInvalidCell)
                    ),

                    oInvalidCell
                );
            }
        }

        return (oGroupNameDictionary);
    }

    //*************************************************************************
    //  Method: ReadGroupVertexTable()
    //
    /// <summary>
    /// Reads the group vertex table.
    /// </summary>
    ///
    /// <param name="oGroupVertexTable">
    /// The group vertex table.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oGroupNameDictionary">
    /// The key is the group name and the value is the ExcelTemplateGroupInfo
    /// object for the group.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add group data to.
    /// </param>
    //*************************************************************************

    protected void
    ReadGroupVertexTable
    (
        ListObject oGroupVertexTable,
        ReadWorkbookContext oReadWorkbookContext,
        Dictionary<String, ExcelTemplateGroupInfo> oGroupNameDictionary,
        IGraph oGraph
    )
    {
        Debug.Assert(oGroupVertexTable != null);
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGroupNameDictionary != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        Dictionary<String, IVertex> oVertexNameDictionary =
            oReadWorkbookContext.VertexNameDictionary;

        ExcelTableReader oExcelTableReader =
            new ExcelTableReader(oGroupVertexTable);

        foreach ( ExcelTableReader.ExcelTableRow oRow in
            oExcelTableReader.GetRows() )
        {
            // Get the group vertex information from the row.

            String sGroupName, sVertexName;

            if (
                !oRow.TryGetNonEmptyStringFromCell(
                    GroupVertexTableColumnNames.GroupName, out sGroupName)
                ||
                !oRow.TryGetNonEmptyStringFromCell(
                    GroupVertexTableColumnNames.VertexName, out sVertexName)
                )
            {
                continue;
            }

            // Get the group information for the vertex.

            ExcelTemplateGroupInfo oExcelTemplateGroupInfo;
            IVertex oVertex;

            if (
                !oGroupNameDictionary.TryGetValue(sGroupName,
                    out oExcelTemplateGroupInfo)
                ||
                !oVertexNameDictionary.TryGetValue(sVertexName,
                    out oVertex)
                )
            {
                continue;
            }

            // If the vertex should get its color or shape from the group, set
            // the vertex's color or shape.

            Boolean bReadColorFromGroup, bReadShapeFromGroup;

            GetReadColorAndShapeFlags(oVertex, oExcelTemplateGroupInfo,
                oReadWorkbookContext, out bReadColorFromGroup,
                out bReadShapeFromGroup);

            if (bReadColorFromGroup)
            {
                oVertex.SetValue(ReservedMetadataKeys.PerColor,
                    oExcelTemplateGroupInfo.VertexColor);
            }

            if (bReadShapeFromGroup)
            {
                oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
                    oExcelTemplateGroupInfo.VertexShape);
            }

            oExcelTemplateGroupInfo.Vertices.AddLast(oVertex);
        }
    }

    //*************************************************************************
    //  Method: SkipAndHideGroups()
    //
    /// <summary>
    /// Skips and hides those groups that should be skipped or hidden.
    /// </summary>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oGroupNameDictionary">
    /// The key is the group name and the value is the ExcelTemplateGroupInfo
    /// object for the group.
    /// </param>
    ///
    /// <param name="oSkippedGroupNames">
    /// The names of groups that should be skipped.
    /// </param>
    ///
    /// <param name="oHiddenGroupNames">
    /// The names of groups that should be hidden.
    /// </param>
    ///
    /// <param name="oGraph">
    /// Graph to add group data to.
    /// </param>
    //*************************************************************************

    protected void
    SkipAndHideGroups
    (
        ReadWorkbookContext oReadWorkbookContext,
        Dictionary<String, ExcelTemplateGroupInfo> oGroupNameDictionary,
        HashSet<String> oSkippedGroupNames,
        HashSet<String> oHiddenGroupNames,
        IGraph oGraph
    )
    {
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oGroupNameDictionary != null);
        Debug.Assert(oSkippedGroupNames != null);
        Debug.Assert(oHiddenGroupNames != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        foreach (String sGroupName in oSkippedGroupNames)
        {
            // Remove the group's vertices and their incident edges from the
            // graph and dictionaries.

            foreach (IVertex oVertex in
                oGroupNameDictionary[sGroupName].Vertices)
            {
                RemoveVertex(oVertex, oReadWorkbookContext, oGraph);
            }

            oGroupNameDictionary.Remove(sGroupName);
        }

        foreach (String sGroupName in oHiddenGroupNames)
        {
            foreach (IVertex oVertex in
                oGroupNameDictionary[sGroupName].Vertices)
            {
                // Hide the group's vertices and their incident edges.

                HideVertex(oVertex);
            }
        }
    }

    //*************************************************************************
    //  Method: GetReadColorAndShapeFlags()
    //
    /// <summary>
    /// Determines whether a vertex should get its color and shape from the
    /// group it belongs to.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The vertex to check.
    /// </param>
    ///
    /// <param name="oGroup">
    /// The group the vertex belongs to.
    /// </param>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="bReadColorFromGroup">
    /// Where a flag indicating whether the vertex should get its color from
    /// the group gets stored.
    /// </param>
    ///
    /// <param name="bReadShapeFromGroup">
    /// Where a flag indicating whether the vertex should get its shape from
    /// the group gets stored.
    /// </param>
    //*************************************************************************

    protected void
    GetReadColorAndShapeFlags
    (
        IVertex oVertex,
        GroupInfo oGroup,
        ReadWorkbookContext oReadWorkbookContext,
        out Boolean bReadColorFromGroup,
        out Boolean bReadShapeFromGroup
    )
    {
        Debug.Assert(oVertex != null);
        Debug.Assert(oReadWorkbookContext != null);
        AssertValid();

        // Assume that the settings specified in the context object will be
        // used.

        bReadColorFromGroup = oReadWorkbookContext.ReadVertexColorFromGroups;
        bReadShapeFromGroup = oReadWorkbookContext.ReadVertexShapeFromGroups;

        if (oGroup.CollapsedAttributes != null)
        {
            CollapsedGroupAttributes oCollapsedGroupAttributes =
                CollapsedGroupAttributes.FromString(
                    oGroup.CollapsedAttributes);

            String sHeadVertexName;

            if (
                oCollapsedGroupAttributes.GetGroupType() ==
                    CollapsedGroupAttributeValues.FanMotifType
                &&
                oCollapsedGroupAttributes.TryGetValue(
                    CollapsedGroupAttributeKeys.HeadVertexName,
                        out sHeadVertexName)
                &&
                oVertex.Name == sHeadVertexName
                )
            {
                // The shape of a fan motif's head vertex should never be
                // changed.  Do not get it from the group.

                bReadShapeFromGroup = false;
            }
        }
    }

    //*************************************************************************
    //  Method: ReadVisibility()
    //
    /// <summary>
    /// Reads a group's visibility.
    /// </summary>
    ///
    /// <param name="oRow">
    /// Row containing the group data.
    /// </param>
    ///
    /// <param name="oGroupVisibilityConverter">
    /// Object that converts a group visibility between values used in the
    /// Excel workbook and values used in the NodeXL graph.
    /// </param>
    ///
    /// <param name="sGroupName">
    /// Name of the row's group.
    /// </param>
    ///
    /// <param name="oSkippedGroupNames">
    /// If the group should be skipped, this method adds its name to this
    /// collection.
    /// </param>
    ///
    /// <param name="oHiddenGroupNames">
    /// If the group should be hidden, this method adds its name to this
    /// collection.
    /// </param>
    //*************************************************************************

    protected void
    ReadVisibility
    (
        ExcelTableReader.ExcelTableRow oRow,
        GroupVisibilityConverter oGroupVisibilityConverter,
        String sGroupName,
        HashSet<String> oSkippedGroupNames,
        HashSet<String> oHiddenGroupNames
    )
    {
        Debug.Assert(oRow != null);
        Debug.Assert(oGroupVisibilityConverter != null);
        Debug.Assert( !String.IsNullOrEmpty(sGroupName) );
        Debug.Assert(oSkippedGroupNames != null);
        Debug.Assert(oHiddenGroupNames != null);
        AssertValid();

        // Assume a default visibility.

        Visibility eVisibility = Visibility.Show;

        String sVisibility;

        if (
            oRow.TryGetNonEmptyStringFromCell(
                CommonTableColumnNames.Visibility, out sVisibility)
            &&
            !oGroupVisibilityConverter.TryWorkbookToGraph(
                sVisibility, out eVisibility)
            )
        {
            OnInvalidVisibility(oRow);
        }

        if (eVisibility == Visibility.Skip)
        {
            oSkippedGroupNames.Add(sGroupName);
        }
        else if (eVisibility == Visibility.Hide)
        {
            oHiddenGroupNames.Add(sGroupName);
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
