
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: ColumnGroups
//
/// <summary>
/// Specifies a group of related columns in a single table.
/// </summary>
///
/// <remarks>
/// The values can be ORed together to specify multiple column groups.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
ColumnGroups
{
    //*************************************************************************
    //  Edge table column groups
    //
    //  Important Notes:
    //
    //    1. Column groups in the edge table must start with "Edge".
    //
    //    2. Column names that include "DoNotHide" or "InternalUse" are never
    //       collapsed or expanded.
    //*************************************************************************

    /// <summary>
    /// The columns in the edge table that must always be visible.
    /// </summary>

    EdgeDoNotHide = 1,

    /// <summary>
    /// The visual attribute columns in the edge table.
    /// </summary>

    EdgeVisualAttributes = 2,

    /// <summary>
    /// The label columns in the edge table.
    /// </summary>

    EdgeLabels = 4,

    /// <summary>
    /// The graph metric columns in the edge table.
    /// </summary>

    EdgeGraphMetrics = 8,

    /// <summary>
    /// The columns in the edge table that are used only by NodeXL and should
    /// not be edited by the user.
    /// </summary>

    EdgeInternalUse = 16,

    /// <summary>
    /// All columns in the edge table that are not included in one of the
    /// previous groups.
    /// </summary>

    EdgeOtherColumns = 32,

    // Important Note:
    //
    // When a column group is added, the methods in the ColumnGroupManager
    // class must be updated.


    //*************************************************************************
    //  Vertex table column groups
    //
    //  Important Notes:
    //
    //    1. Column groups in the edge table must start with "Vertex".
    //
    //    2. Column names that include "DoNotHide" or "InternalUse" are never
    //       collapsed or expanded.
    //*************************************************************************

    /// <summary>
    /// The columns in the vertex table that must always be visible.
    /// </summary>

    VertexDoNotHide = 64,

    /// <summary>
    /// The visual attribute columns in the vertex table.
    /// </summary>

    VertexVisualAttributes = 128,

    /// <summary>
    /// The graph metric columns in the vertex table.
    /// </summary>

    VertexGraphMetrics = 256,

    /// <summary>
    /// The label columns in the vertex table.
    /// </summary>

    VertexLabels = 512,

    /// <summary>
    /// The layout columns in the vertex table.
    /// </summary>

    VertexLayout = 1024,

    /// <summary>
    /// The columns in the vertex table that are used only by NodeXL and should
    /// not be edited by the user.
    /// </summary>

    VertexInternalUse = 2048,

    /// <summary>
    /// All columns in the vertex table that are not included in one of the
    /// previous groups.
    /// </summary>

    VertexOtherColumns = 4096,

    // Important Note:
    //
    // When a column group is added, the methods in the ColumnGroupManager
    // class must be updated.


    //*************************************************************************
    //  Group table column groups
    //
    //  Important Notes:
    //
    //    1. Column groups in the edge table must start with "Group".
    //
    //    2. Column names that include "DoNotHide" or "InternalUse" are never
    //       collapsed or expanded.
    //*************************************************************************

    /// <summary>
    /// The columns in the group table that must always be visible.
    /// </summary>

    GroupDoNotHide = 8192,

    /// <summary>
    /// The visual attribute columns in the group table.
    /// </summary>

    GroupVisualAttributes = 16384,

    /// <summary>
    /// The label columns in the group table.
    /// </summary>

    GroupLabels = 32768,

    /// <summary>
    /// The layout columns in the group table.
    /// </summary>

    GroupLayout = 65536,

    /// <summary>
    /// The graph metric columns in the group table.
    /// </summary>

    GroupGraphMetrics = 131072,

    /// <summary>
    /// The columns in the group table that are used only by NodeXL and should
    /// not be edited by the user.
    /// </summary>

    GroupInternalUse = 262144,

    /// <summary>
    /// All columns in the group table that are not included in one of the
    /// previous groups.
    /// </summary>

    GroupOtherColumns = 524288,


    //*************************************************************************
    //  Group-edge table column groups
    //
    //  Important Notes:
    //
    //    1. Column groups in the group-edge table must start with "GroupEdge".
    //
    //    2. Column names that include "DoNotHide" or "InternalUse" are never
    //       collapsed or expanded.
    //*************************************************************************

    /// <summary>
    /// The columns in the group-edge table that must always be visible.
    /// </summary>

    GroupEdgeDoNotHide = 1048576,

    /// <summary>
    /// The graph metric columns in the group-edge table.
    /// </summary>

    GroupEdgeGraphMetrics = 2097152,


    //*************************************************************************
    //  All column groups
    //*************************************************************************

    /// <summary>
    /// All column groups.
    /// </summary>

    All =
        EdgeDoNotHide |
        EdgeVisualAttributes |
        EdgeLabels |
        EdgeGraphMetrics |
        EdgeInternalUse |
        EdgeOtherColumns |
        VertexDoNotHide |
        VertexVisualAttributes |
        VertexGraphMetrics |
        VertexLabels |
        VertexLayout |
        VertexInternalUse |
        VertexOtherColumns |
        GroupEdgeDoNotHide |
        GroupVisualAttributes |
        GroupLabels |
        GroupLayout |
        GroupGraphMetrics |
        GroupInternalUse |
        GroupOtherColumns |
        GroupEdgeDoNotHide |
        GroupEdgeGraphMetrics
        ,

    // Important Note:
    //
    // When a column group is added, the methods in the ColumnGroupManager
    // class must be updated.
}


//*****************************************************************************
//  Class: ColumnGroupManager
//
/// <summary>
/// Hides and shows groups of related columns.
/// </summary>
///
/// <remarks>
/// Call <see cref="ShowOrHideColumnGroups" /> to show or hide one or more
/// column groups.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class ColumnGroupManager
{
    //*************************************************************************
    //  Method: ShowOrHideColumnGroups()
    //
    /// <summary>
    /// Shows or hides the specified groups of related columns.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="columnGroups">
    /// The column groups to show or hide, as an ORed combination of <see
    /// cref="ColumnGroups" /> flags.  Column groups that aren't specified are
    /// not modified.
    /// </param>
    ///
    /// <param name="show">
    /// true to show the column groups, false to hide them.
    /// </param>
    ///
    /// <param name="updateUserSettings">
    /// true to update the persisted column group user settings.
    /// </param>
    ///
    /// <remarks>
    /// Columns that don't exist are ignored.
    /// </remarks>
    //*************************************************************************

    public static void
    ShowOrHideColumnGroups
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ColumnGroups columnGroups,
        Boolean show,
        Boolean updateUserSettings
    )
    {
        Debug.Assert(workbook != null);

        foreach ( ColumnGroups eColumnGroup in
            Enum.GetValues(typeof(ColumnGroups) ) )
        {
            String sColumnGroup = eColumnGroup.ToString();

            if (eColumnGroup != ColumnGroups.All &&
                (columnGroups & eColumnGroup) != 0 &&
                sColumnGroup.IndexOf("DoNotHide") == -1 &&
                sColumnGroup.IndexOf("InternalUse") == -1
                )
            {
                ShowOrHideColumnGroup(workbook, eColumnGroup, show);
            }
        }

        if (updateUserSettings)
        {
            ColumnGroupUserSettings oColumnGroupUserSettings =
                new ColumnGroupUserSettings();

            ColumnGroups eColumnGroupsToShow =
                oColumnGroupUserSettings.ColumnGroupsToShow;

            if (show)
            {
                eColumnGroupsToShow |= columnGroups;
            }
            else
            {
                eColumnGroupsToShow &= ~columnGroups;
            }

            oColumnGroupUserSettings.ColumnGroupsToShow = eColumnGroupsToShow;
            oColumnGroupUserSettings.Save();
        }
    }

    //*************************************************************************
    //  Method: ShowOrHideColumnGroup()
    //
    /// <summary>
    /// Shows or hides one group of related columns.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="eColumnGroup">
    /// The group of columns to show or hide.  This must be a single value in
    /// the <see cref="ColumnGroups" /> enumeration, not an ORed combination.
    /// </param>
    ///
    /// <param name="bShow">
    /// true to show the column group, false to hide it.
    /// </param>
    ///
    /// <remarks>
    /// Columns that don't exist are ignored.
    /// </remarks>
    //*************************************************************************

    private static void
    ShowOrHideColumnGroup
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ColumnGroups eColumnGroup,
        Boolean bShow
    )
    {
        Debug.Assert(oWorkbook != null);

        #if DEBUG

        // Verify that a single value in the ColumnGroup enumeration has been
        // specified.

        Double dLogColumnGroup = Math.Log( (Double)eColumnGroup, 2 );
        Debug.Assert( (Int32)dLogColumnGroup == dLogColumnGroup );

        #endif

        ListObject oTable;

        if ( TryGetColumnGroupTable(oWorkbook, eColumnGroup, out oTable) )
        {
            ExcelColumnHider.ShowOrHideColumns(oTable,
                GetColumnNames(oWorkbook, eColumnGroup), bShow);

            if (eColumnGroup == ColumnGroups.VertexOtherColumns)
            {
                // Hiding the subgraph image column doesn't hide the images in
                // the column.

                TableImagePopulator.ShowOrHideImagesInColumn(oWorkbook,
                    WorksheetNames.Vertices,
                    VertexTableColumnNames.SubgraphImage, bShow);
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetColumnGroupTable()
    //
    /// <summary>
    /// Attempts to get the table that contains a specified column group.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="eColumnGroup">
    /// The group of columns to get the table for.
    /// </param>
    ///
    /// <param name="oTable">
    /// Where the table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetColumnGroupTable
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ColumnGroups eColumnGroup,
        out ListObject oTable
    )
    {
        Debug.Assert(oWorkbook != null);

        oTable = null;
        String sColumnGroup = eColumnGroup.ToString();

        if ( sColumnGroup.StartsWith("Edge") )
        {
            return ( ExcelTableUtil.TryGetTable(oWorkbook,
                WorksheetNames.Edges, TableNames.Edges, out oTable) );
        }

        if ( sColumnGroup.StartsWith("Vertex") )
        {
            return ( ExcelTableUtil.TryGetTable(oWorkbook,
                WorksheetNames.Vertices, TableNames.Vertices, out oTable) );
        }

        if ( sColumnGroup.StartsWith("GroupEdge") )
        {
            return ( ExcelTableUtil.TryGetTable(oWorkbook,
                WorksheetNames.GroupEdgeMetrics, TableNames.GroupEdgeMetrics,
                out oTable) );
        }

        if ( sColumnGroup.StartsWith("Group") )
        {
            return ( ExcelTableUtil.TryGetTable(oWorkbook,
                WorksheetNames.Groups, TableNames.Groups, out oTable) );
        }

        Debug.Assert(false);
        return (false);
    }

    //*************************************************************************
    //  Method: GetColumnNames()
    //
    /// <summary>
    /// Gets the names of the columns in a specified column group.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="eColumnGroup">
    /// The group to get the column names for.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more column names.
    /// </returns>
    //*************************************************************************

    private static String []
    GetColumnNames
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ColumnGroups eColumnGroup
    )
    {
        Debug.Assert(oWorkbook != null);

        String [] asColumnNames = null;

        switch (eColumnGroup)
        {
            case ColumnGroups.EdgeDoNotHide:

                asColumnNames = new String [] {
                    EdgeTableColumnNames.Vertex1Name,
                    EdgeTableColumnNames.Vertex2Name,
                    };

                break;

            case ColumnGroups.EdgeVisualAttributes:

                asColumnNames = new String [] {
                    EdgeTableColumnNames.Color,
                    EdgeTableColumnNames.Width,
                    EdgeTableColumnNames.Style,
                    CommonTableColumnNames.Alpha,
                    CommonTableColumnNames.Visibility,
                    };

                break;

            case ColumnGroups.EdgeLabels:

                asColumnNames = new String [] {
                    EdgeTableColumnNames.Label,
                    EdgeTableColumnNames.LabelTextColor,
                    EdgeTableColumnNames.LabelFontSize,
                    };

                break;

            case ColumnGroups.EdgeGraphMetrics:

                asColumnNames = new String [] {
                    EdgeTableColumnNames.IsReciprocated,
                    };

                break;

            case ColumnGroups.VertexDoNotHide:

                asColumnNames = new String [] {
                    VertexTableColumnNames.VertexName,
                    };

                break;

            case ColumnGroups.VertexVisualAttributes:

                asColumnNames = new String [] {
                    VertexTableColumnNames.Color,
                    VertexTableColumnNames.Shape,
                    VertexTableColumnNames.Radius,
                    CommonTableColumnNames.Alpha,
                    VertexTableColumnNames.ImageUri,
                    CommonTableColumnNames.Visibility,
                    };

                break;

            case ColumnGroups.VertexGraphMetrics:

                asColumnNames = new String [] {
                    VertexTableColumnNames.Degree,
                    VertexTableColumnNames.InDegree,
                    VertexTableColumnNames.OutDegree,
                    VertexTableColumnNames.BetweennessCentrality,
                    VertexTableColumnNames.ClosenessCentrality,
                    VertexTableColumnNames.EigenvectorCentrality,
                    VertexTableColumnNames.PageRank,
                    VertexTableColumnNames.ClusteringCoefficient,
                    VertexTableColumnNames.ReciprocatedVertexPairRatio,
                    };

                break;

            case ColumnGroups.VertexLabels:

                asColumnNames = new String [] {
                    VertexTableColumnNames.Label,
                    VertexTableColumnNames.LabelFillColor,
                    VertexTableColumnNames.LabelPosition,
                    VertexTableColumnNames.ToolTip,
                    };

                break;

            case ColumnGroups.VertexLayout:

                asColumnNames = new String [] {
                    VertexTableColumnNames.LayoutOrder,
                    VertexTableColumnNames.X,
                    VertexTableColumnNames.Y,
                    VertexTableColumnNames.Locked,
                    VertexTableColumnNames.PolarR,
                    VertexTableColumnNames.PolarAngle,
                    };

                break;

            case ColumnGroups.EdgeInternalUse:
            case ColumnGroups.VertexInternalUse:

                asColumnNames = new String [] {
                    CommonTableColumnNames.ID,
                    CommonTableColumnNames.DynamicFilter,
                    };

                break;

            case ColumnGroups.EdgeOtherColumns:
            case ColumnGroups.VertexOtherColumns:
            case ColumnGroups.GroupOtherColumns:

                asColumnNames = GetOtherColumnNames(oWorkbook, eColumnGroup);
                break;

            case ColumnGroups.GroupDoNotHide:

                asColumnNames = new String [] {
                    GroupTableColumnNames.Name,
                    };

                break;

            case ColumnGroups.GroupVisualAttributes:

                asColumnNames = new String [] {
                    GroupTableColumnNames.VertexColor,
                    GroupTableColumnNames.VertexShape,
                    CommonTableColumnNames.Visibility,
                    GroupTableColumnNames.Collapsed,
                    };

                break;

            case ColumnGroups.GroupLabels:

                asColumnNames = new String [] {
                    GroupTableColumnNames.Label,
                    };

                break;

            case ColumnGroups.GroupLayout:

                asColumnNames = new String [] {
                    GroupTableColumnNames.CollapsedX,
                    GroupTableColumnNames.CollapsedY,
                    };

                break;

            case ColumnGroups.GroupGraphMetrics:

                asColumnNames = new String [] {
                    GroupTableColumnNames.Vertices,
                    GroupTableColumnNames.UniqueEdges,
                    GroupTableColumnNames.EdgesWithDuplicates,
                    GroupTableColumnNames.TotalEdges,
                    GroupTableColumnNames.SelfLoops,
                    GroupTableColumnNames.ReciprocatedVertexPairRatio,
                    GroupTableColumnNames.ReciprocatedEdgeRatio,
                    GroupTableColumnNames.ConnectedComponents,
                    GroupTableColumnNames.SingleVertexConnectedComponents,
                    GroupTableColumnNames.MaximumConnectedComponentVertices,
                    GroupTableColumnNames.MaximumConnectedComponentEdges,
                    GroupTableColumnNames.MaximumGeodesicDistance,
                    GroupTableColumnNames.AverageGeodesicDistance,
                    GroupTableColumnNames.GraphDensity,
                    };

                break;

            case ColumnGroups.GroupInternalUse:

                asColumnNames = new String [] {
                    CommonTableColumnNames.ID,
                    GroupTableColumnNames.CollapsedAttributes,
                    };

                break;

            case ColumnGroups.GroupEdgeDoNotHide:

                asColumnNames = new String [] {
                    GroupEdgeTableColumnNames.Group1Name,
                    GroupEdgeTableColumnNames.Group2Name,
                    };

                break;

            case ColumnGroups.GroupEdgeGraphMetrics:

                asColumnNames = new String [] {
                    GroupEdgeTableColumnNames.Edges,
                    };

                break;

            default:

                Debug.Assert(false);
                break;
        }
        
        Debug.Assert(asColumnNames != null);

        return (asColumnNames);
    }

    //*************************************************************************
    //  Method: GetOtherColumnNames()
    //
    /// <summary>
    /// Gets the names of the columns in the ColumnGroup.EdgeOtherColumns,
    /// VertexOtherColumns, or GroupOtherColumns column group.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the column group.
    /// </param>
    ///
    /// <param name="eOtherColumnGroup">
    /// The group to get the column names for.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more column names.
    /// </returns>
    //*************************************************************************

    private static String []
    GetOtherColumnNames
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        ColumnGroups eOtherColumnGroup
    )
    {
        Debug.Assert(oWorkbook != null);

        List<String> oOtherColumnNames = new List<String>();
        ListObject oTable;

        if ( TryGetColumnGroupTable(oWorkbook, eOtherColumnGroup, out oTable) )
        {
            // Create a HashSet of the column names that are included in all
            // the non-other column groups in the table.

            HashSet<String> oColumnNamesInNonOtherGroups =
                new HashSet<String>();

            // All column groups in the edge table start with "Edge", all
            // column groups in the vertex table start with "Vertex", and all
            // column groups in the group table start with "Group".

            String sStartsWith = null;

            switch (eOtherColumnGroup)
            {
                case ColumnGroups.EdgeOtherColumns:

                    sStartsWith = "Edge";
                    break;

                case ColumnGroups.VertexOtherColumns:

                    sStartsWith = "Vertex";
                    break;

                case ColumnGroups.GroupOtherColumns:

                    sStartsWith = "Group";
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            foreach ( ColumnGroups eColumnGroup in
                Enum.GetValues(typeof(ColumnGroups) ) )
            {
                if (
                    eColumnGroup != eOtherColumnGroup
                    &&
                    eColumnGroup.ToString().StartsWith(sStartsWith)
                    )
                {
                    foreach ( String sColumnNameInNonOtherGroup in 
                        GetColumnNames(oWorkbook, eColumnGroup) )
                    {
                        oColumnNamesInNonOtherGroups.Add(
                            sColumnNameInNonOtherGroup);
                    }
                }
            }

            // Any column not included in one of the non-other column groups is
            // an "other" column.

            foreach (ListColumn oColumn in oTable.ListColumns)
            {
                String sColumnName = oColumn.Name;

                if ( !oColumnNamesInNonOtherGroups.Contains(sColumnName) )
                {
                    oOtherColumnNames.Add(sColumnName);
                }
            }
        }
        
        return ( oOtherColumnNames.ToArray() );
    }
}
}
