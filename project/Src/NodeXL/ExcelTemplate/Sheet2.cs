

using System;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet2
//
/// <summary>
/// Represents the vertex worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet2
{
    //*************************************************************************
    //  Property: Sheets1And2Helper
    //
    /// <summary>
    /// Gets the object that does most of the work for this class.
    /// </summary>
    ///
    /// <value>
    /// A Sheets1And2Helper object.
    /// </value>
    //*************************************************************************

    public Sheets1And2Helper
    Sheets1And2Helper
    {
        get
        {
            AssertValid();

            return (m_oSheets1And2Helper);
        }
    }

    //*************************************************************************
    //  Method: GetSelectedVertexNames()
    //
    /// <summary>
    /// Gets a collection of vertex names for all rows in the table that have
    /// at least one cell selected.
    /// </summary>
    ///
    /// <returns>
    /// A collection of vertex names.
    /// </returns>
    ///
    /// <remarks>
    /// This method activates the worksheet if it isn't activated already.
    /// </remarks>
    //*************************************************************************

    public ICollection<String>
    GetSelectedVertexNames()
    {
        return ( m_oSheets1And2Helper.GetSelectedStringColumnValues(
            VertexTableColumnNames.VertexName) );
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
            !m_oSheets1And2Helper.TryGetSelectedRange(out oSelectedRange) )
        {
            return;
        }

        // See if the specified attribute is set by the helper class.

        m_oSheets1And2Helper.SetVisualAttribute(e, oSelectedRange,
            VertexTableColumnNames.Color, CommonTableColumnNames.Alpha);

        if (e.VisualAttributeSet)
        {
            return;
        }

        if (e.VisualAttribute == VisualAttributes.VertexShape)
        {
            Debug.Assert(e.AttributeValue is VertexShape);

            ExcelTableUtil.SetVisibleSelectedTableColumnData(
                this.Vertices.InnerObject, oSelectedRange,
                VertexTableColumnNames.Shape,

                ( new VertexShapeConverter() ).GraphToWorkbook(
                    (VertexShape)e.AttributeValue)
                );

            e.VisualAttributeSet = true;
        }
        else if (e.VisualAttribute == VisualAttributes.VertexRadius)
        {
            VertexRadiusDialog oVertexRadiusDialog = new VertexRadiusDialog();

            if (oVertexRadiusDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelTableUtil.SetVisibleSelectedTableColumnData(
                    this.Vertices.InnerObject, oSelectedRange,
                    VertexTableColumnNames.Radius,
                    oVertexRadiusDialog.VertexRadius);

                e.VisualAttributeSet = true;
            }
        }
        else if (e.VisualAttribute == VisualAttributes.VertexVisibility)
        {
            Debug.Assert(e.AttributeValue is VertexWorksheetReader.Visibility);

            ExcelTableUtil.SetVisibleSelectedTableColumnData(
                this.Vertices.InnerObject, oSelectedRange,
                CommonTableColumnNames.Visibility,

                ( new VertexVisibilityConverter() ).GraphToWorkbook(
                    (VertexWorksheetReader.Visibility)e.AttributeValue)
                );

            e.VisualAttributeSet = true;
        }
    }

    //*************************************************************************
    //  Method: TryGetMarkedColumnData()
    //
    /// <summary>
    /// Attempts to add a Marked column to the vertex table if it doesn't
    /// already exist.
    /// </summary>
    ///
    /// <param name="oMarkedColumnData">
    /// Where the column data gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the column was added.
    /// </returns>
    //*************************************************************************

    private Boolean
    TryGetMarkedColumnData
    (
        out Microsoft.Office.Interop.Excel.Range oMarkedColumnData
    )
    {
        AssertValid();

        oMarkedColumnData = null;
        Microsoft.Office.Interop.Excel.ListColumn oMarkedColumn;

        return (
            ExcelTableUtil.TryGetOrAddTableColumn(Vertices.InnerObject,
                VertexTableColumnNames.IsMarked,
                ExcelTableUtil.AutoColumnWidth, null, out oMarkedColumn)
            &&
            ExcelTableUtil.TryGetTableColumnData(oMarkedColumn,
                out oMarkedColumnData)
            );
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
    /// vertex corresponding to that row.
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
        KeyValuePair<Int32, IIdentityProvider> oLocationInfo,
        out Int32 iRowID,
        out PointF oLocationToSet
    )
    {
        AssertValid();

        iRowID = oLocationInfo.Key;

        Debug.Assert(oLocationInfo.Value is IVertex);
        oLocationToSet = ( (IVertex)oLocationInfo.Value ).Location;

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetRowIDAndLocation()
    //
    /// <summary>
    /// Gets a row ID and a location for a row whose location needs to be set.
    /// </summary>
    ///
    /// <param name="oLocationInfo">
    /// A VertexAndRowID object.
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
        VertexAndRowID oLocationInfo,
        out Int32 iRowID,
        out PointF oLocationToSet
    )
    {
        AssertValid();

        iRowID = oLocationInfo.VertexRowID;
        oLocationToSet = oLocationInfo.Vertex.Location;

        return (true);
    }

    //*************************************************************************
    //  Method: Sheet2_Startup()
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
    Sheet2_Startup
    (
        object sender,
        System.EventArgs e
    )
    {
        // Create the object that does most of the work for this class.

        m_oSheets1And2Helper = new Sheets1And2Helper(this, this.Vertices);

        ThisWorkbook oThisWorkbook = Globals.ThisWorkbook;

        oThisWorkbook.GraphLaidOut +=
            new EventHandler<GraphLaidOutEventArgs>(
                this.ThisWorkbook_GraphLaidOut);

        oThisWorkbook.VerticesMoved +=
            new EventHandler<VerticesMovedEventArgs2>(
                this.ThisWorkbook_VerticesMoved);

        oThisWorkbook.AttributesEditedInGraph +=
            new EventHandler<AttributesEditedEventArgs>(
                this.ThisWorkbook_AttributesEditedInGraph);

        CommandDispatcher.CommandSent +=
            new RunCommandEventHandler(this.CommandDispatcher_CommandSent);

        AssertValid();
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

        if (m_oSheets1And2Helper.TableExists)
        {
            m_oSheets1And2Helper.SetLocations<
                KeyValuePair<Int32, IIdentityProvider> >(
                e.VertexIDDictionary, e.GraphRectangle,
                VertexTableColumnNames.X,
                VertexTableColumnNames.Y,
                VertexTableColumnNames.Locked,

                new TryGetRowIDAndLocation<
                    KeyValuePair<Int32, IIdentityProvider> >(
                    this.TryGetRowIDAndLocation)
                );
        }
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

        if (m_oSheets1And2Helper.TableExists)
        {
            // Note that the lockedColumnName argument is null, even though
            // the vertex table has a locked column.  This causes the locked
            // column to be ignored, which is the correct behavior when
            // vertices are manually moved in the graph pane.

            m_oSheets1And2Helper.SetLocations<VertexAndRowID>(
                e.VerticesAndRowIDs, e.GraphRectangle,
                VertexTableColumnNames.X,
                VertexTableColumnNames.Y,
                null,

                new TryGetRowIDAndLocation<VertexAndRowID>(
                    this.TryGetRowIDAndLocation)
                );
        }
    }

    //*************************************************************************
    //  Method: ThisWorkbook_AttributesEditedInGraph()
    //
    /// <summary>
    /// Handles the AttributesEditedInGraph event on ThisWorkbook.
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
    ThisWorkbook_AttributesEditedInGraph
    (
        Object sender,
        AttributesEditedEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        // The key is the row ID stored in the table's ID column and the value
        // is the one-based row number relative to the worksheet.

        Dictionary<Int32, Int32> oRowIDDictionary;

        if (
            e.EditedVertexAttributes == null
            ||
            !m_oSheets1And2Helper.TableExists
            ||
            !m_oSheets1And2Helper.TryGetAllRowIDs(out oRowIDDictionary)
            )
        {
            return;
        }

        Microsoft.Office.Interop.Excel.ListObject oVertexTable =
            Vertices.InnerObject;

        Globals.ThisWorkbook.ShowWaitCursor = true;

        // Get the columns that might need to be updated.  These columns are
        // not required.

        Microsoft.Office.Interop.Excel.Range oColorColumnData,
            oShapeColumnData, oRadiusColumnData, oAlphaColumnData,
            oVisibilityColumnData, oLabelColumnData, oLabelFillColorColumnData,
            oLabelPositionColumnData, oToolTipColumnData, oLockedColumnData,
            oMarkedColumnData;

        Object [,] aoColorValues = null;
        Object [,] aoShapeValues = null;
        Object [,] aoRadiusValues = null;
        Object [,] aoAlphaValues = null;
        Object [,] aoVisibilityValues = null;
        Object [,] aoLabelValues = null;
        Object [,] aoLabelFillColorValues = null;
        Object [,] aoLabelPositionValues = null;
        Object [,] aoToolTipValues = null;
        Object [,] aoLockedValues = null;
        Object [,] aoMarkedValues = null;

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            VertexTableColumnNames.Color, out oColorColumnData,
            out aoColorValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            VertexTableColumnNames.Shape, out oShapeColumnData,
            out aoShapeValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            VertexTableColumnNames.Radius, out oRadiusColumnData,
            out aoRadiusValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            CommonTableColumnNames.Alpha, out oAlphaColumnData,
            out aoAlphaValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            CommonTableColumnNames.Visibility, out oVisibilityColumnData,
            out aoVisibilityValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            VertexTableColumnNames.Label, out oLabelColumnData,
            out aoLabelValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            VertexTableColumnNames.LabelFillColor,
            out oLabelFillColorColumnData, out aoLabelFillColorValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            VertexTableColumnNames.LabelPosition, out oLabelPositionColumnData,
            out aoLabelPositionValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            VertexTableColumnNames.ToolTip, out oToolTipColumnData,
            out aoToolTipValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oVertexTable,
            VertexTableColumnNames.Locked, out oLockedColumnData,
            out aoLockedValues);

        if ( TryGetMarkedColumnData(out oMarkedColumnData) )
        {
            aoMarkedValues = ExcelUtil.GetRangeValues(oMarkedColumnData);
        }

        ColorConverter2 oColorConverter2 = new ColorConverter2();

        VertexShapeConverter oVertexShapeConverter =
            new VertexShapeConverter();

        VertexVisibilityConverter oVertexVisibilityConverter =
            new VertexVisibilityConverter();

        VertexLabelPositionConverter oVertexLabelPositionConverter =
            new VertexLabelPositionConverter();

        BooleanConverter oBooleanConverter = new BooleanConverter();

        // Loop through the IDs of the vertices whose attributes were edited
        // in the graph.

        EditedVertexAttributes oEditedVertexAttributes =
            e.EditedVertexAttributes;

        foreach (Int32 iID in e.VertexIDs)
        {
            // Look for the row that contains the ID.

            Int32 iRowOneBased;

            if ( !oRowIDDictionary.TryGetValue(iID, out iRowOneBased) )
            {
                continue;
            }

            iRowOneBased -= oVertexTable.Range.Row;

            if (oEditedVertexAttributes.Color.HasValue &&
                aoColorValues != null)
            {
                aoColorValues[iRowOneBased, 1] =
                    oColorConverter2.GraphToWorkbook(
                        oEditedVertexAttributes.Color.Value);
            }

            if (oEditedVertexAttributes.Shape.HasValue &&
                aoShapeValues != null)
            {
                aoShapeValues[iRowOneBased, 1] =
                    oVertexShapeConverter.GraphToWorkbook(
                        oEditedVertexAttributes.Shape.Value);
            }

            if (oEditedVertexAttributes.Radius.HasValue &&
                aoRadiusValues != null)
            {
                aoRadiusValues[iRowOneBased, 1] =
                    oEditedVertexAttributes.Radius.Value.ToString();
            }

            if (oEditedVertexAttributes.Alpha.HasValue &&
                aoAlphaValues != null)
            {
                aoAlphaValues[iRowOneBased, 1] =
                    oEditedVertexAttributes.Alpha.Value.ToString();
            }

            if (oEditedVertexAttributes.Visibility.HasValue &&
                aoVisibilityValues != null)
            {
                aoVisibilityValues[iRowOneBased, 1] =
                    oVertexVisibilityConverter.GraphToWorkbook(
                        oEditedVertexAttributes.Visibility.Value);
            }

            if ( !String.IsNullOrEmpty(oEditedVertexAttributes.Label) )
            {
                aoLabelValues[iRowOneBased, 1] = oEditedVertexAttributes.Label;
            }

            if (oEditedVertexAttributes.LabelFillColor.HasValue &&
                aoLabelFillColorValues != null)
            {
                aoLabelFillColorValues[iRowOneBased, 1] =
                    oColorConverter2.GraphToWorkbook(
                        oEditedVertexAttributes.LabelFillColor.Value);
            }

            if (oEditedVertexAttributes.LabelPosition.HasValue &&
                aoLabelPositionValues != null)
            {
                aoLabelPositionValues[iRowOneBased, 1] =
                    oVertexLabelPositionConverter.GraphToWorkbook(
                        oEditedVertexAttributes.LabelPosition.Value);
            }

            if ( !String.IsNullOrEmpty(oEditedVertexAttributes.ToolTip) )
            {
                aoToolTipValues[iRowOneBased, 1] =
                    oEditedVertexAttributes.ToolTip;
            }

            if (oEditedVertexAttributes.Locked.HasValue &&
                aoLockedValues != null)
            {
                aoLockedValues[iRowOneBased, 1] =
                    oBooleanConverter.GraphToWorkbook(
                        oEditedVertexAttributes.Locked.Value);
            }

            if (oEditedVertexAttributes.Marked.HasValue &&
                aoMarkedValues != null)
            {
                aoMarkedValues[iRowOneBased, 1] =
                    oBooleanConverter.GraphToWorkbook(
                        oEditedVertexAttributes.Marked.Value);
            }
        }

        // Activate this worksheet first, because writing to an inactive
        // worksheet causes problems with the selection in Excel.

        ExcelActiveWorksheetRestorer oExcelActiveWorksheetRestorer =
            m_oSheets1And2Helper.GetExcelActiveWorksheetRestorer();

        ExcelActiveWorksheetState oExcelActiveWorksheetState =
            oExcelActiveWorksheetRestorer.ActivateWorksheet(this.InnerObject);

        try
        {
            m_oSheets1And2Helper.SetColumnDataValues(
                oColorColumnData, aoColorValues,
                oShapeColumnData, aoShapeValues,
                oRadiusColumnData, aoRadiusValues,
                oAlphaColumnData, aoAlphaValues,
                oVisibilityColumnData, aoVisibilityValues,
                oLabelColumnData, aoLabelValues,
                oLabelFillColorColumnData, aoLabelFillColorValues,
                oLabelPositionColumnData, aoLabelPositionValues,
                oToolTipColumnData, aoToolTipValues,
                oLockedColumnData, aoLockedValues,
                oMarkedColumnData, aoMarkedValues
                );
        }
        finally
        {
            oExcelActiveWorksheetRestorer.Restore(oExcelActiveWorksheetState);
        }

        Globals.ThisWorkbook.ShowWaitCursor = false;
    }

    //*************************************************************************
    //  Method: Sheet2_Shutdown()
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
    Sheet2_Shutdown
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
        this.Startup += new System.EventHandler(Sheet2_Startup);
        this.Shutdown += new System.EventHandler(Sheet2_Shutdown);
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
        Debug.Assert(m_oSheets1And2Helper != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object that does most of the work for this class.

    private Sheets1And2Helper m_oSheets1And2Helper;
}

}
