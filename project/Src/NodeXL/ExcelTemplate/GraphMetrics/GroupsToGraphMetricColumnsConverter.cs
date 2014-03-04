
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using Smrf.GraphicsLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupsToGraphMetricColumnsConverter
//
/// <summary>
/// Converts a collection of vertex groups to a collection of <see
/// cref="GraphMetricColumn" /> objects.
/// </summary>
///
/// <remarks>
/// This is used by graph metric calculator classes that partition the graph
/// into a collection of groups.  They use the Convert() method to convert the
/// collection of groups, the type of which is determined by the graph metric
/// calculator class, into a collection of <see cref="GraphMetricColumn" />
/// objects suitable for populating the workbook.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class GroupsToGraphMetricColumnsConverter
{
    //*************************************************************************
    //  Delegate: GroupToString()
    //
    /// <summary>
    /// Gets a string associated with a group.
    /// </summary>
    ///
    /// <typeparam name="TGroup">
    /// The type of the group.
    /// </typeparam>
    ///
    /// <param name="group">
    /// The group to get the string for.
    /// </param>
    ///
    /// <returns>
    /// The string associated with the group.
    /// </returns>
    //*************************************************************************

    public delegate String GroupToString<TGroup>(TGroup group);


    //*************************************************************************
    //  Delegate: GroupToGroupVertices()
    //
    /// <summary>
    /// Gets the collection of vertices in a group.
    /// </summary>
    ///
    /// <typeparam name="TGroup">
    /// The type of the group.
    /// </typeparam>
    ///
    /// <param name="group">
    /// The group to get the vertices for.
    /// </param>
    ///
    /// <returns>
    /// The collection of vertices in the group.
    /// </returns>
    //*************************************************************************

    public delegate ICollection<IVertex>
        GroupToGroupVertices<TGroup>(TGroup group);


    //*************************************************************************
    //  Method: Convert()
    //
    /// <overloads>
    /// Converts a collection of groups to an array of <see
    /// cref="GraphMetricColumn" /> objects.
    /// </overloads>
    ///
    /// <summary>
    /// Converts a collection of groups to an array of <see
    /// cref="GraphMetricColumn" /> objects using a default set of group names.
    /// </summary>
    ///
    /// <typeparam name="TGroup">
    /// The type of the group.
    /// </typeparam>
    ///
    /// <param name="groups">
    /// Collection of groups, each of which is of type TGroup.
    /// </param>
    ///
    /// <param name="groupToGroupVertices">
    /// Method to get the collection of vertices in a group.
    /// </param>
    ///
    /// <returns>
    /// An array of <see cref="GraphMetricColumn" /> objects.
    /// </returns>
    //*************************************************************************

    public static GraphMetricColumn []
    Convert<TGroup>
    (
        ICollection<TGroup> groups,
        GroupToGroupVertices<TGroup> groupToGroupVertices
    )
    {
        Debug.Assert(groups != null);
        Debug.Assert(groupToGroupVertices != null);

        return ( Convert<TGroup>(groups, groupToGroupVertices, null, false,
            null) );
    }

    //*************************************************************************
    //  Method: Convert()
    //
    /// <summary>
    /// Converts a collection of groups to an array of <see
    /// cref="GraphMetricColumn" /> objects using specified group names and
    /// additional group information.
    /// </summary>
    ///
    /// <typeparam name="TGroup">
    /// The type of the group.
    /// </typeparam>
    ///
    /// <param name="groups">
    /// Collection of groups, each of which is of type TGroup.
    /// </param>
    ///
    /// <param name="groupToGroupVertices">
    /// Method to get the collection of vertices in a group.
    /// </param>
    ///
    /// <param name="groupToGroupName">
    /// Method to get the group's name, or null to use a default set of group
    /// names.
    /// </param>
    ///
    /// <param name="collapseGroups">
    /// true if the groups should all be collapsed initially, false if they
    /// should all be expanded.
    /// </param>
    ///
    /// <param name="groupToCollapsedGroupAttributes">
    /// Method to get a string that contains the attributes describing what the
    /// group should look like when it is collapsed, or null to give the
    /// collapsed group a default appearance.
    /// </param>
    ///
    /// <returns>
    /// An array of <see cref="GraphMetricColumn" /> objects.
    /// </returns>
    //*************************************************************************

    public static GraphMetricColumn []
    Convert<TGroup>
    (
        ICollection<TGroup> groups,
        GroupToGroupVertices<TGroup> groupToGroupVertices,
        GroupToString<TGroup> groupToGroupName,
        Boolean collapseGroups,
        GroupToString<TGroup> groupToCollapsedGroupAttributes
    )
    {
        Debug.Assert(groups != null);
        Debug.Assert(groupToGroupVertices != null);

        // These columns are needed:
        //
        // * Group name on the group worksheet.
        //
        // * Vertex color on the group worksheet.
        //
        // * Vertex shape on the group worksheet.
        //
        // * Group name on the group-vertex worksheet.
        //
        // * Vertex name on the group-vertex worksheet.
        //
        // * Vertex ID on the group-vertex worksheet.  This gets copied from
        //   the hidden ID column on the Vertex worksheet via an Excel
        //   formula.

        // These columns might be needed:
        //
        // * Collapsed flag on the group worksheet.
        //
        // * Collapsed group attributes on the group worksheet.


        List<GraphMetricValueOrdered> oGroupNamesForGroupWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexColorsForGroupWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexShapesForGroupWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oCollapsedFlagsForGroupWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oCollapsedAttributesForGroupWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oGroupNamesForGroupVertexWorksheet =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oVertexNamesForGroupVertexWorksheet =
            new List<GraphMetricValueOrdered>();

        // This column contains a constant value, which is a formula.

        GraphMetricValueOrdered [] aoVertexIDsForGroupVertexWorksheet = {
            new GraphMetricValueOrdered(
                GroupManager.GetExcelFormulaForVertexID() )
            };

        Int32 iGroups = groups.Count;
        Int32 iGroup = 0;

        ColorConverter2 oColorConverter2 = new ColorConverter2();

        VertexShapeConverter oVertexShapeConverter =
            new VertexShapeConverter();

        GraphMetricValueOrdered oTrueGraphMetricValueOrdered =
            new GraphMetricValueOrdered(
                ( new BooleanConverter() ).GraphToWorkbook(true) );

        foreach (TGroup oGroup in 
            from oGroup in groups
            orderby groupToGroupVertices(oGroup).Count descending
            select oGroup)
        {
            String sGroupName;

            if (groupToGroupName == null)
            {
                sGroupName = 'G' + (iGroup + 1).ToString(
                    CultureInfo.InvariantCulture);
            }
            else
            {
                sGroupName = groupToGroupName(oGroup);
            }

            Color oColor;
            VertexShape eShape;

            GroupManager.GetVertexAttributes(iGroup, iGroups, out oColor,
                out eShape);

            GraphMetricValueOrdered oGroupNameGraphMetricValue =
                new GraphMetricValueOrdered(sGroupName);

            oGroupNamesForGroupWorksheet.Add(oGroupNameGraphMetricValue);

            // Write the color in a format that is understood by
            // ColorConverter2.WorkbookToGraph(), which is what
            // WorksheetReaderBase uses.

            oVertexColorsForGroupWorksheet.Add(
                new GraphMetricValueOrdered(
                    oColorConverter2.GraphToWorkbook(oColor) ) );

            oVertexShapesForGroupWorksheet.Add(
                new GraphMetricValueOrdered(
                    oVertexShapeConverter.GraphToWorkbook(eShape) ) );

            if (collapseGroups)
            {
                oCollapsedFlagsForGroupWorksheet.Add(
                    oTrueGraphMetricValueOrdered);
            }

            if (groupToCollapsedGroupAttributes != null)
            {
                oCollapsedAttributesForGroupWorksheet.Add(
                    new GraphMetricValueOrdered(
                        groupToCollapsedGroupAttributes(oGroup) ) );
            }

            Int32 iVertices = 0;

            foreach ( IVertex oVertex in groupToGroupVertices(oGroup) )
            {
                oGroupNamesForGroupVertexWorksheet.Add(
                    oGroupNameGraphMetricValue);

                oVertexNamesForGroupVertexWorksheet.Add(
                    new GraphMetricValueOrdered(
                        ExcelUtil.ForceCellText(oVertex.Name) ) );

                iVertices++;
            }

            iGroup++;
        }

        List<GraphMetricColumn> oGraphMetricColumns =
            new List<GraphMetricColumn>();

        oGraphMetricColumns.AddRange(

            new GraphMetricColumn [] {

            new GraphMetricColumnOrdered( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.Name,
                ExcelTableUtil.AutoColumnWidth, null,
                CellStyleNames.Required,
                oGroupNamesForGroupWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.VertexColor,
                ExcelTableUtil.AutoColumnWidth, null,
                CellStyleNames.VisualProperty,
                oVertexColorsForGroupWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.Groups,
                TableNames.Groups,
                GroupTableColumnNames.VertexShape,
                ExcelTableUtil.AutoColumnWidth, null,
                CellStyleNames.VisualProperty,
                oVertexShapesForGroupWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.GroupVertices,
                TableNames.GroupVertices,
                GroupVertexTableColumnNames.GroupName,
                ExcelTableUtil.AutoColumnWidth, null, null,
                oGroupNamesForGroupVertexWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.GroupVertices,
                TableNames.GroupVertices,
                GroupVertexTableColumnNames.VertexName,
                ExcelTableUtil.AutoColumnWidth, null, null,
                oVertexNamesForGroupVertexWorksheet.ToArray()
                ),

            new GraphMetricColumnOrdered( WorksheetNames.GroupVertices,
                TableNames.GroupVertices,
                GroupVertexTableColumnNames.VertexID,
                ExcelTableUtil.AutoColumnWidth, null, null,
                aoVertexIDsForGroupVertexWorksheet
                ),
                } );

            if (collapseGroups)
            {
                oGraphMetricColumns.Add(
                    new GraphMetricColumnOrdered( WorksheetNames.Groups,
                        TableNames.Groups,
                        GroupTableColumnNames.Collapsed,
                        ExcelTableUtil.AutoColumnWidth, null,
                        CellStyleNames.VisualProperty,
                        oCollapsedFlagsForGroupWorksheet.ToArray()
                    ) );
            }

            if (groupToCollapsedGroupAttributes != null)
            {
                oGraphMetricColumns.Add(
                    new GraphMetricColumnOrdered( WorksheetNames.Groups,
                        TableNames.Groups,
                        GroupTableColumnNames.CollapsedAttributes,
                        ExcelTableUtil.AutoColumnWidth, null,
                        CellStyleNames.DoNotEdit,
                        oCollapsedAttributesForGroupWorksheet.ToArray()
                    ) );
            }

            return ( oGraphMetricColumns.ToArray() );
    }
}

}
