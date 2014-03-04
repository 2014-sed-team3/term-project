
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using Smrf.GraphicsLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupManager
//
/// <summary>
/// Manages vertex groups in a NodeXL workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="GetGroupCommandsToEnable" /> to determine which group
/// commands to enable in the Ribbon.  Call <see cref="TryRunGroupCommand" />
/// to run a command.
/// </remarks>
//*****************************************************************************

public static class GroupManager
{
    //*************************************************************************
    //  Method: GetGroupCommandsToEnable
    //
    /// <summary>
    /// Gets the group commands to enable in the program's group menus.
    /// </summary>
    ///
    /// <param name="workbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <returns>
    /// An ORed combination of <see cref="GroupCommands" /> flags.
    /// </returns>
    //*************************************************************************

    public static GroupCommands
    GetGroupCommandsToEnable
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        // Various worksheets must be activated to read their selection.  Save
        // the active worksheet state.

        ExcelActiveWorksheetRestorer oExcelActiveWorksheetRestorer =
            new ExcelActiveWorksheetRestorer(workbook);

        ExcelActiveWorksheetState oExcelActiveWorksheetState =
            oExcelActiveWorksheetRestorer.GetActiveWorksheetState();

        GroupCommands eGroupCommandsToEnable;

        try
        {
            eGroupCommandsToEnable =
                GetGroupCommandsToEnableInternal(workbook);
        }
        finally
        {
            oExcelActiveWorksheetRestorer.Restore(oExcelActiveWorksheetState);
        }

        return (eGroupCommandsToEnable);
    }

    //*************************************************************************
    //  Method: TryRunGroupCommand()
    //
    /// <summary>
    /// Attempts to run a group command.
    /// </summary>
    ///
    /// <param name="groupCommand">
    /// One of the flags in the <see cref="GroupCommands" /> enumeration.
    /// </param>
    ///
    /// <param name="workbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="vertexWorksheet">
    /// The vertex worksheet in the NodeXL workbook.
    /// </param>
    ///
    /// <param name="groupWorksheet">
    /// The group worksheet in the NodeXL workbook.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method may modify the contents of the workbook.  It does not
    /// interact with the TaskPane, which contains the graph created from the
    /// workbook.  It is the responsibility of the caller to communicate
    /// group changes to the TaskPane when necessary.
    ///
    /// <para>
    /// This method activates various worksheets to read their selection and
    /// write their contents.  It is the responsibility of the caller to save
    /// the active worksheet state before calling this method and restore it
    /// afterward.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryRunGroupCommand
    (
        GroupCommands groupCommand,
        Microsoft.Office.Interop.Excel.Workbook workbook,
        Sheet2 vertexWorksheet,
        Sheet5 groupWorksheet
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(vertexWorksheet != null);
        Debug.Assert(groupWorksheet != null);

        switch (groupCommand)
        {
            case GroupCommands.None:

                return (true);

            case GroupCommands.CollapseSelectedGroups:

                return ( TryCollapseOrExpandSelectedGroups(workbook, true) );

            case GroupCommands.ExpandSelectedGroups:

                return ( TryCollapseOrExpandSelectedGroups(workbook, false) );

            case GroupCommands.CollapseAllGroups:

                return ( TryCollapseOrExpandAllGroups(workbook, true) );

            case GroupCommands.ExpandAllGroups:

                return ( TryCollapseOrExpandAllGroups(workbook, false) );

            case GroupCommands.SelectGroupsWithSelectedVertices:

                return ( TrySelectGroupsWithSelectedVertices(workbook,
                    vertexWorksheet, groupWorksheet) );

            case GroupCommands.SelectAllGroups:

                return ( TrySelectAllGroups(workbook) );

            case GroupCommands.AddSelectedVerticesToGroup:

                return ( TryAddSelectedVerticesToGroup(workbook,
                    vertexWorksheet) );

            case GroupCommands.RemoveSelectedVerticesFromGroups:

                ICollection<String> oSelectedVertexNames;

                return ( TryRemoveSelectedVerticesFromGroups(workbook,
                    vertexWorksheet, out oSelectedVertexNames) );

            case GroupCommands.RemoveSelectedGroups:

                return ( TryRemoveSelectedGroups(workbook, groupWorksheet) );

            case GroupCommands.RemoveAllGroups:

                return ( TryRemoveAllGroups(workbook) );

            default:

                Debug.Assert(false);
                return (false);
        }
    }

    //*************************************************************************
    //  Method: GetExcelFormulaForVertexID()
    //
    /// <summary>
    /// Gets the Excel formula that gets the ID of a vertex.
    /// </summary>
    ///
    /// <returns>
    /// An Excel formula.
    /// </returns>
    ///
    /// <remarks>
    /// This formula is meant to be used in the
    /// GroupVertexTableColumnNames.VertexID column in the group-vertex table.
    /// </remarks>
    //*************************************************************************

    public static String
    GetExcelFormulaForVertexID()
    {
        // Given a vertex name in the group-vertex table, this formula
        // retrieves the corresponding vertex ID from the vertex worksheet.
        // When formatted, it looks like this:
        //
        // =VLOOKUP(GroupVertices[[#This Row],[Vertex]], Vertices,
        // MATCH("ID", Vertices[#Headers], 0), FALSE)

        return (String.Format(

            "=VLOOKUP({0}[[#This Row],[{1}]], {2},"
                + " MATCH(\"{3}\", {2}[#Headers], 0), FALSE)"
            ,
            TableNames.GroupVertices,
            GroupVertexTableColumnNames.VertexName,
            TableNames.Vertices,
            CommonTableColumnNames.ID
            ) );
    }

    //*************************************************************************
    //  Method: GetVertexAttributes()
    //
    /// <summary>
    /// Gets the vertex attributes for a specified group.
    /// </summary>
    ///
    /// <param name="groupIndex">
    /// Zero-based group index.
    /// </param>
    ///
    /// <param name="totalGroups">
    /// Total number of groups.
    /// </param>
    ///
    /// <param name="color">
    /// Where the color gets stored.
    /// </param>
    ///
    /// <param name="shape">
    /// Where the shape gets stored.
    /// </param>
    //*************************************************************************

    public static void
    GetVertexAttributes
    (
        Int32 groupIndex,
        Int32 totalGroups,
        out Color color,
        out VertexShape shape
    )
    {
        Debug.Assert(groupIndex >= 0);
        Debug.Assert(totalGroups > 0);
        Debug.Assert(groupIndex < totalGroups);

        // Algorithm:
        //
        // Cycle through a set of hues with one shape, then the same set of
        // hues with the next shape, and so on.  When all shapes have been
        // used, repeat the process with another saturation.

        Int32 iHues = ColorUtil.UniqueColorHues;
        Int32 iShapes = Shapes.Length;

        Int32 iSaturations = (Int32)Math.Ceiling(
            (Single)totalGroups / (Single)(iHues * iShapes)
            );

        Int32 iMaximumUniqueColors = iSaturations * iHues;

        Int32 iColorIndex = (groupIndex % iHues) +
            ( iHues * ( groupIndex / (iHues * iShapes) ) );

        color = ColorUtil.GetUniqueColor(iColorIndex, iMaximumUniqueColors);

        Int32 iShapeIndex = (groupIndex / ColorUtil.UniqueColorHues) % iShapes;

        shape = Shapes[iShapeIndex];
    }

    //*************************************************************************
    //  Method: GroupCommandsIncludeGroupCommand()
    //
    /// <summary>
    /// Determines whether an ORed combination of <see cref="GroupCommands" />
    /// flags includes a single specified command.
    /// </summary>
    ///
    /// <param name="groupCommands">
    /// The ORed combination of <see cref="GroupCommands" /> flags to test.
    /// </param>
    ///
    /// <param name="groupCommand">
    /// The single <see cref="GroupCommands" /> flag to look for.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="groupCommands" /> includes <paramref
    /// name="groupCommand" />.
    /// </returns>
    //*************************************************************************

    public static Boolean
    GroupCommandsIncludeGroupCommand
    (
        GroupCommands groupCommands,
        GroupCommands groupCommand
    )
    {
        return ( (groupCommands & groupCommand) != 0 );
    }

    //*************************************************************************
    //  Method: GetGroupCommandsToEnableInternal
    //
    /// <summary>
    /// Gets the group commands to enable in the Ribbon.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <returns>
    /// An ORed combination of <see cref="GroupCommands" /> flags.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates various worksheets to read their selection.  It
    /// is the responsibility of the caller to save the active worksheet state
    /// before calling this method and restore it afterward.
    /// </remarks>
    //*************************************************************************

    private static GroupCommands
    GetGroupCommandsToEnableInternal
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        GroupCommands eGroupCommandsToEnable = GroupCommands.None;

        ListObject oGroupTable;
        Range oSelectedTableRange;

        // Are any groups selected in the group table?

        if (ExcelTableUtil.TryGetSelectedTableRange(oWorkbook,
                WorksheetNames.Groups, TableNames.Groups, out oGroupTable,
                out oSelectedTableRange) )
        {
            // Yes.

            eGroupCommandsToEnable |= (
                GroupCommands.CollapseSelectedGroups |
                GroupCommands.ExpandSelectedGroups |
                GroupCommands.RemoveSelectedGroups
                );
        }

        // Are any vertices selected in the vertex table?

        ListObject oVertexTable;

        Boolean bVerticesSelected = ExcelTableUtil.TryGetSelectedTableRange(
            oWorkbook, WorksheetNames.Vertices, TableNames.Vertices,
            out oVertexTable, out oSelectedTableRange);

        if (bVerticesSelected)
        {
            eGroupCommandsToEnable |=
                GroupCommands.AddSelectedVerticesToGroup;
        }

        // Are there any groups?

        if (!ExcelTableUtil.VisibleTableRangeIsEmpty(oGroupTable))
        {
            // Yes.

            eGroupCommandsToEnable |= (
                GroupCommands.CollapseAllGroups |
                GroupCommands.ExpandAllGroups |
                GroupCommands.SelectAllGroups |
                GroupCommands.RemoveAllGroups
                );

            if (bVerticesSelected)
            {
                eGroupCommandsToEnable |= (
                    GroupCommands.SelectGroupsWithSelectedVertices |
                    GroupCommands.RemoveSelectedVerticesFromGroups
                    );
            }
        }

        return (eGroupCommandsToEnable);
    }

    //*************************************************************************
    //  Method: TryCollapseOrExpandSelectedGroups()
    //
    /// <summary>
    /// Attempts to collapse or expand the selected groups in the workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="bCollapse">
    /// true to collapse the selected groups, false to expand them.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the group worksheet.
    /// </remarks>
    //*************************************************************************

    private static Boolean
    TryCollapseOrExpandSelectedGroups
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Boolean bCollapse
    )
    {
        Debug.Assert(oWorkbook != null);

        ListObject oGroupTable;
        Range oSelectedTableRange;

        if (ExcelTableUtil.TryGetSelectedTableRange(oWorkbook,
                WorksheetNames.Groups, TableNames.Groups, out oGroupTable,
                out oSelectedTableRange) )
        {
            // Store the new collapsed state in the group table.

            ExcelTableUtil.SetVisibleSelectedTableColumnData(oGroupTable,
                oSelectedTableRange, GroupTableColumnNames.Collapsed,
                ( new BooleanConverter() ).GraphToWorkbook(bCollapse) );

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TryCollapseOrExpandAllGroups()
    //
    /// <summary>
    /// Attempts to collapse or expand all groups in the workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="bCollapse">
    /// true to collapse all groups, false to expand them.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryCollapseOrExpandAllGroups
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Boolean bCollapse
    )
    {
        Debug.Assert(oWorkbook != null);

        // Store the new collapsed state in the group table.

        ListObject oGroupTable;

        if (ExcelTableUtil.TryGetTable(oWorkbook, WorksheetNames.Groups,
            TableNames.Groups, out oGroupTable) )
        {
            ExcelTableUtil.SetVisibleTableColumnData(oGroupTable,
                GroupTableColumnNames.Collapsed,
                ( new BooleanConverter() ).GraphToWorkbook(bCollapse) );

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: TrySelectGroupsWithSelectedVertices()
    //
    /// <summary>
    /// Attempts to select the groups containing the selected vertices in the
    /// workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="oVertexWorksheet">
    /// The vertex worksheet in the NodeXL workbook.
    /// </param>
    ///
    /// <param name="oGroupWorksheet">
    /// The group worksheet in the NodeXL workbook.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the group worksheet.
    /// </remarks>
    //*************************************************************************

    private static Boolean
    TrySelectGroupsWithSelectedVertices
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Sheet2 oVertexWorksheet,
        Sheet5 oGroupWorksheet
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oVertexWorksheet != null);
        Debug.Assert(oGroupWorksheet != null);

        // For each selected vertex, get the vertex's group name from the 
        // group-vertex worksheet and select that group in the group worksheet.

        ICollection<String> oSelectedVertexNames =
            oVertexWorksheet.GetSelectedVertexNames();

        oGroupWorksheet.SelectGroups(
            NodeXLWorkbookUtil.GetGroupNamesByVertexName(oWorkbook,
                oSelectedVertexNames) );

        return (true);
    }

    //*************************************************************************
    //  Method: TrySelectAllGroups()
    //
    /// <summary>
    /// Attempts to select all groups in the workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the group worksheet.
    /// </remarks>
    //*************************************************************************

    private static Boolean
    TrySelectAllGroups
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        ExcelTableUtil.SelectAllTableRows(oWorkbook, WorksheetNames.Groups,
            TableNames.Groups);

        return (true);
    }

    //*************************************************************************
    //  Method: TryAddSelectedVerticesToGroup()
    //
    /// <summary>
    /// Attempts to add the selected vertices to a new or existing group.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="oVertexWorksheet">
    /// The vertex worksheet in the NodeXL workbook.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryAddSelectedVerticesToGroup
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Sheet2 oVertexWorksheet
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oVertexWorksheet != null);

        AddSelectedVerticesToGroupDialog oAddSelectedVerticesToGroupDialog =
            new AddSelectedVerticesToGroupDialog(oWorkbook);

        if (oAddSelectedVerticesToGroupDialog.ShowDialog() !=
            DialogResult.OK)
        {
            return (false);
        }

        // First, remove the selected vertices from any groups they belong to.

        ListObject oGroupTable, oGroupVertexTable;
        ICollection<String> oSelectedVertexNames;

        if (
            !TryRemoveSelectedVerticesFromGroups(oWorkbook, oVertexWorksheet,
                out oSelectedVertexNames)
            ||
            !ExcelTableUtil.TryGetTable(oWorkbook, WorksheetNames.Groups,
                TableNames.Groups, out oGroupTable)
            ||
            !ExcelTableUtil.TryGetTable(oWorkbook, WorksheetNames.GroupVertices,
                TableNames.GroupVertices, out oGroupVertexTable)
            )
        {
            return (false);
        }

        String sGroupName = oAddSelectedVerticesToGroupDialog.GroupName;

        if (oAddSelectedVerticesToGroupDialog.IsNewGroup)
        {
            // Add the new group to the group table.
            //
            // Note that the group table (and the group-vertex table, below)
            // needs to be activated before being written to.  If this isn't
            // done, the formula written to the group-vertex table below also
            // mysteriously appears in the vertex table on the vertex
            // worksheet.
            //
            // It's up to the caller to use the ExcelActiveWorksheetRestorer
            // class to save and restore the active worksheet.

            ExcelUtil.ActivateWorksheet(oGroupTable);

            ExcelTableUtil.AddTableRow(oGroupTable,

                GroupTableColumnNames.Name,
                sGroupName
                );

            SetVertexAttributesForAllGroups(oGroupTable);
        }

        // Add the selected vertices to the group-vertex table.

        ExcelUtil.ActivateWorksheet(oGroupVertexTable);

        foreach (String sSelectedVertexName in oSelectedVertexNames)
        {
            ExcelTableUtil.AddTableRow(oGroupVertexTable,

                GroupVertexTableColumnNames.GroupName,
                sGroupName,

                GroupVertexTableColumnNames.VertexName,
                sSelectedVertexName,

                GroupVertexTableColumnNames.VertexID,
                GetExcelFormulaForVertexID()
                );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryRemoveSelectedVerticesFromGroups()
    //
    /// <summary>
    /// Attempts to remove the selected vertices from their groups in the
    /// workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="oVertexWorksheet">
    /// The vertex worksheet in the NodeXL workbook.
    /// </param>
    ///
    /// <param name="oSelectedVertexNames">
    /// Where a collection of the selected vertex names gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryRemoveSelectedVerticesFromGroups
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Sheet2 oVertexWorksheet,
        out ICollection<String> oSelectedVertexNames
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oVertexWorksheet != null);

        oSelectedVertexNames = oVertexWorksheet.GetSelectedVertexNames();

        ExcelTableUtil.RemoveTableRowsByStringColumnValues(oWorkbook,
            WorksheetNames.GroupVertices, TableNames.GroupVertices,
            GroupVertexTableColumnNames.VertexName, oSelectedVertexNames);

        return (true);
    }

    //*************************************************************************
    //  Method: TryRemoveSelectedGroups()
    //
    /// <summary>
    /// Attempts to remove the selected groups from the workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    ///
    /// <param name="oGroupWorksheet">
    /// The group worksheet in the NodeXL workbook.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryRemoveSelectedGroups
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        Sheet5 oGroupWorksheet
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oGroupWorksheet != null);

        ICollection<String> oSelectedGroupNames =
            oGroupWorksheet.GetSelectedGroupNames();

        ExcelTableUtil.RemoveTableRowsByStringColumnValues(oWorkbook,
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.Name, oSelectedGroupNames);

        ExcelTableUtil.RemoveTableRowsByStringColumnValues(oWorkbook,
            WorksheetNames.GroupVertices, TableNames.GroupVertices,
            GroupVertexTableColumnNames.GroupName, oSelectedGroupNames);

        return (true);
    }

    //*************************************************************************
    //  Method: TryRemoveAllGroups()
    //
    /// <summary>
    /// Attempts to remove all the groups.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// NodeXL workbook.
    /// </param>
    //*************************************************************************

    private static Boolean
    TryRemoveAllGroups
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        NodeXLWorkbookUtil.ClearGroupTables(oWorkbook);

        return (true);
    }

    //*************************************************************************
    //  Method: SetVertexAttributesForAllGroups()
    //
    /// <summary>
    /// Sets the vertex attributes for all the groups in the group table.
    /// </summary>
    ///
    /// <param name="oGroupTable">
    /// The group table.
    /// </param>
    //*************************************************************************

    private static void
    SetVertexAttributesForAllGroups
    (
        ListObject oGroupTable
    )
    {
        Debug.Assert(oGroupTable != null);

        Range oVertexColorRange, oVertexShapeRange;
        Object [,] aoVertexColorValues, aoVertexShapeValues;

        if (
            !ExcelTableUtil.TryGetTableColumnDataAndValues(oGroupTable,
                GroupTableColumnNames.VertexShape, out oVertexShapeRange,
                out aoVertexShapeValues)
            ||
            !ExcelTableUtil.TryGetTableColumnDataAndValues(oGroupTable,
                GroupTableColumnNames.VertexColor, out oVertexColorRange,
                out aoVertexColorValues)
            )
        {
            return;
        }

        Int32 iGroups = aoVertexShapeValues.GetUpperBound(0);
        Debug.Assert(aoVertexColorValues.GetUpperBound(0) == iGroups);

        ColorConverter2 oColorConverter2 = new ColorConverter2();

        VertexShapeConverter oVertexShapeConverter =
            new VertexShapeConverter();

        for (Int32 i = 0; i < iGroups; i++)
        {
            Color oColor;
            VertexShape eShape;

            GetVertexAttributes(i, iGroups, out oColor, out eShape);

            // Write the color in a format that is understood by
            // ColorConverter2.WorkbookToGraph(), which is what
            // WorksheetReaderBase uses.

            aoVertexColorValues[i + 1, 1] =
                oColorConverter2.GraphToWorkbook(oColor);

            aoVertexShapeValues[i + 1, 1] =
                oVertexShapeConverter.GraphToWorkbook(eShape);
        }

        oVertexColorRange.set_Value(Missing.Value, aoVertexColorValues);
        oVertexShapeRange.set_Value(Missing.Value, aoVertexShapeValues);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// The ability to create groups (which used to be called clusters)
    /// was introduced in version 54 of the template.

    public const Int32 MinimumTemplateVersionForGroups = 54;


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Shapes to cycle through.

    private static VertexShape [] Shapes = new VertexShape[] {

        VertexShape.Disk,
        VertexShape.SolidSquare,
        VertexShape.SolidDiamond,
        VertexShape.SolidTriangle,
        VertexShape.Sphere,
        VertexShape.Circle,
        VertexShape.Square,
        VertexShape.Diamond,
        VertexShape.Triangle,
        };
}

}
