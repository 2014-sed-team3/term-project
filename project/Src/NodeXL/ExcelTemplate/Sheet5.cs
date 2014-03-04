
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet5
//
/// <summary>
/// Represents the group worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet5
{
    //*************************************************************************
    //  Property: SheetHelper
    //
    /// <summary>
    /// Gets the object that does most of the work for this class.
    /// </summary>
    ///
    /// <value>
    /// A SheetHelper object.
    /// </value>
    //*************************************************************************

    public SheetHelper
    SheetHelper
    {
        get
        {
            AssertValid();

            return (m_oSheetHelper);
        }
    }

    //*************************************************************************
    //  Method: GetSelectedGroupNames()
    //
    /// <summary>
    /// Gets the group names for the rows in the table that have at least one
    /// cell selected.
    /// </summary>
    ///
    /// <returns>
    /// A collection of group names.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// </remarks>
    //*************************************************************************

    public ICollection<String>
    GetSelectedGroupNames()
    {
        AssertValid();

        return ( m_oSheetHelper.GetSelectedStringColumnValues(
            GroupTableColumnNames.Name ) );
    }

    //*************************************************************************
    //  Method: SelectGroups()
    //
    /// <summary>
    /// Selects a specified collection of groups.
    /// </summary>
    ///
    /// <param name="groupNames">
    /// Names of the groups to select.
    /// </param>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't already activated.
    /// </remarks>
    //*************************************************************************

    public void
    SelectGroups
    (
        ICollection<String> groupNames
    )
    {
        Debug.Assert(groupNames != null);
        AssertValid();

        m_oSheetHelper.SelectTableRowsByColumnValues<String>(
            GroupTableColumnNames.Name, groupNames,
            ExcelUtil.TryGetNonEmptyStringFromCell
            );
    }

    //*************************************************************************
    //  Method: SetVisualAttribute()
    //
    /// <summary>
    /// Sets a visual attribute in the worksheet if appropriate.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    SetVisualAttribute
    (
        RunSetVisualAttributeCommandEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        Microsoft.Office.Interop.Excel.Range oSelectedRange;

        if ( e.VisualAttributeSet ||
            !m_oSheetHelper.TryGetSelectedRange(out oSelectedRange) )
        {
            return;
        }

        // See if the specified attribute is set by the helper class.

        m_oSheetHelper.SetVisualAttribute(e, oSelectedRange,
            GroupTableColumnNames.VertexColor, null);

        if (e.VisualAttributeSet)
        {
            return;
        }

        if (e.VisualAttribute == VisualAttributes.VertexShape)
        {
            Debug.Assert(e.AttributeValue is VertexShape);

            ExcelTableUtil.SetVisibleSelectedTableColumnData(
                this.Groups.InnerObject, oSelectedRange,
                GroupTableColumnNames.VertexShape,

                ( new VertexShapeConverter() ).GraphToWorkbook(
                    (VertexShape)e.AttributeValue)
                );

            e.VisualAttributeSet = true;
        }
    }

    //*************************************************************************
    //  Method: SetLocationsOfCollapsedGroups()
    //
    /// <summary>
    /// For each collapsed group, set the collapsed location cells in the
    /// group's row.
    /// </summary>
    ///
    /// <param name="e">
    /// Event arguments for events that involve a graph rectangle.
    /// </param>
    //*************************************************************************

    private void
    SetLocationsOfCollapsedGroups
    (
        GraphRectangleEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if (m_oSheetHelper.TableExists)
        {
            m_oSheetHelper.SetLocations< KeyValuePair<String, IVertex> >(
                e.NodeXLControl.CollapsedGroups, e.GraphRectangle,
                GroupTableColumnNames.CollapsedX,
                GroupTableColumnNames.CollapsedY,
                null,

                new TryGetRowIDAndLocation<KeyValuePair<String, IVertex>>(
                    this.TryGetRowIDAndLocation)
                );
        }
    }

    //*************************************************************************
    //  Method: TryGetRowIDAndLocation()
    //
    /// <summary>
    /// Gets a row ID and a location for a row whose location needs to be set.
    /// </summary>
    ///
    /// <param name="oLocationInfo">
    /// A KeyValuePair object.  They key is a row ID and the value is the
    /// collapsed group vertex corresponding to that row.
    /// </param>
    ///
    /// <param name="iRowID">
    /// Where the ID of the row whose location needs to be set gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <param name="oLocationToSet">
    /// Where the location that should be written to the row gets stored if
    /// true is returned, in graph coordinates.
    /// </param>
    ///
    /// <returns>
    /// true if the row ID and location were obtained.
    /// </returns>
    //*************************************************************************

    private Boolean
    TryGetRowIDAndLocation
    (
        KeyValuePair<String, IVertex> oLocationInfo,
        out Int32 iRowID,
        out PointF oLocationToSet
    )
    {
        AssertValid();

        iRowID = Int32.MinValue;
        oLocationToSet = PointF.Empty;

        // The IVertex is the collapsed vertex that represents the group.
        // Information about the group is stored in the vertex's metadata.

        IVertex oVertex = oLocationInfo.Value;
        Object oGroupInfoAsObject;

        if ( oVertex.TryGetValue( ReservedMetadataKeys.CollapsedGroupInfo,
            typeof(ExcelTemplateGroupInfo), out oGroupInfoAsObject) )
        {
            ExcelTemplateGroupInfo oGroupInfo =
                (ExcelTemplateGroupInfo)oGroupInfoAsObject;

            if (oGroupInfo.RowID.HasValue)
            {
                iRowID = oGroupInfo.RowID.Value;
                oLocationToSet = oVertex.Location;

                return (true);
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: Sheet5_Startup()
    //
    /// <summary>
    /// Handles the Startup event on the worksheet.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    Sheet5_Startup
    (
        object sender,
        System.EventArgs e
    )
    {
        m_oSheetHelper = new SheetHelper(this, this.Groups);

        ThisWorkbook oThisWorkbook = Globals.ThisWorkbook;

        oThisWorkbook.GraphLaidOut +=
            new EventHandler<GraphLaidOutEventArgs>(
                this.ThisWorkbook_GraphLaidOut);

        oThisWorkbook.VerticesMoved +=
            new EventHandler<VerticesMovedEventArgs2>(
                this.ThisWorkbook_VerticesMoved);

        oThisWorkbook.GroupsCollapsedOrExpanded +=
            new EventHandler<GroupsCollapsedOrExpandedEventArgs>(
                this.ThisWorkbook_GroupsCollapsedOrExpanded);

        CommandDispatcher.CommandSent +=
            new RunCommandEventHandler(this.CommandDispatcher_CommandSent);
    }

    //*************************************************************************
    //  Method: CommandDispatcher_CommandSent()
    //
    /// <summary>
    /// Handles the CommandSent event on the <see cref="CommandDispatcher" />
    /// static class.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// See the comments in <see cref="RunCommandEventArgs" /> for information
    /// on how commands are sent from one UI object to another in NodeXL.
    /// </remarks>
    //*************************************************************************

    private void
    CommandDispatcher_CommandSent
    (
        Object sender,
        RunCommandEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        if ( !Globals.ThisWorkbook.ExcelApplicationIsReady(false) )
        {
            return;
        }

        if (e is RunSetVisualAttributeCommandEventArgs)
        {
            SetVisualAttribute( (RunSetVisualAttributeCommandEventArgs)e );
        }
    }

    //*************************************************************************
    //  Method: ThisWorkbook_GraphLaidOut()
    //
    /// <summary>
    /// Handles the GraphLaidOut event on ThisWorkbook.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// Graph layout occurs asynchronously.  This event fires when the graph
    /// is successfully laid out.
    /// </remarks>
    //*************************************************************************

    private void
    ThisWorkbook_GraphLaidOut
    (
        Object sender,
        GraphLaidOutEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        SetLocationsOfCollapsedGroups(e);
    }

    //*************************************************************************
    //  Method: ThisWorkbook_VerticesMoved()
    //
    /// <summary>
    /// Handles the VerticesMoved event on ThisWorkbook.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    ThisWorkbook_VerticesMoved
    (
        Object sender,
        VerticesMovedEventArgs2 e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        SetLocationsOfCollapsedGroups(e);
    }

    //*************************************************************************
    //  Method: ThisWorkbook_GroupsCollapsedOrExpanded()
    //
    /// <summary>
    /// Handles the GroupsCollapsedOrExpanded event on ThisWorkbook.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    ThisWorkbook_GroupsCollapsedOrExpanded
    (
        Object sender,
        GroupsCollapsedOrExpandedEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        // Set the collapsed group locations only if the groups were collapsed
        // or expanded as a result of a user's collapse or expand command, not
        // as a result of the graph pane being refreshed.  When the graph pane
        // is refreshed, the GraphLaidOut event takes care of setting the
        // collapsed group locations.

        if (e.GroupsRedrawnImmediately)
        {
            SetLocationsOfCollapsedGroups(e);
        }
    }

    //*************************************************************************
    //  Method: Sheet5_Shutdown()
    //
    /// <summary>
    /// Handles the Shutdown event on the worksheet.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    Sheet5_Shutdown
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        // (Do nothing.)
    }

    #region VSTO Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
    {
        this.Startup += new System.EventHandler(Sheet5_Startup);
        this.Shutdown += new System.EventHandler(Sheet5_Shutdown);
    }

    #endregion


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
        Debug.Assert(m_oSheetHelper != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Helper object.

    private SheetHelper m_oSheetHelper;
}
}
