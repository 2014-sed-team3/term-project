

using System;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Sheet1
//
/// <summary>
/// Represents the edge worksheet.
/// </summary>
//*****************************************************************************

public partial class Sheet1
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
            EdgeTableColumnNames.Color, CommonTableColumnNames.Alpha);

        if (e.VisualAttributeSet)
        {
            return;
        }

        if (e.VisualAttribute == VisualAttributes.EdgeWidth)
        {
            EdgeWidthDialog oEdgeWidthDialog = new EdgeWidthDialog();

            if (oEdgeWidthDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelTableUtil.SetVisibleSelectedTableColumnData(
                    this.Edges.InnerObject, oSelectedRange,
                    EdgeTableColumnNames.Width, oEdgeWidthDialog.EdgeWidth);

                e.VisualAttributeSet = true;
            }
        }
        else if (e.VisualAttribute == VisualAttributes.EdgeVisibility)
        {
            Debug.Assert(e.AttributeValue is EdgeWorksheetReader.Visibility);

            ExcelTableUtil.SetVisibleSelectedTableColumnData(
                this.Edges.InnerObject, oSelectedRange,
                CommonTableColumnNames.Visibility,

                ( new EdgeVisibilityConverter() ).GraphToWorkbook(
                    (EdgeWorksheetReader.Visibility)e.AttributeValue)
                );

            e.VisualAttributeSet = true;
        }
    }

    //*************************************************************************
    //  Method: Sheet1_Startup()
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
    Sheet1_Startup
    (
        object sender,
        System.EventArgs e
    )
    {
        // Create the object that does most of the work for this class.

        m_oSheets1And2Helper = new Sheets1And2Helper(this, this.Edges);

        Globals.ThisWorkbook.AttributesEditedInGraph +=
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
            e.EditedEdgeAttributes == null
            ||
            !m_oSheets1And2Helper.TableExists
            ||
            !m_oSheets1And2Helper.TryGetAllRowIDs(out oRowIDDictionary)
            )
        {
            return;
        }

        Microsoft.Office.Interop.Excel.ListObject oEdgeTable =
            Edges.InnerObject;

        Globals.ThisWorkbook.ShowWaitCursor = true;

        // Get the columns that might need to be updated.  These columns are
        // not required.

        Microsoft.Office.Interop.Excel.Range oColorColumnData,
            oWidthColumnData, oStyleColumnData, oAlphaColumnData,
            oVisibilityColumnData, oLabelColumnData,
            oLabelTextColorColumnData, oLabelFontSizeColumnData;

        Object [,] aoColorValues = null;
        Object [,] aoWidthValues = null;
        Object [,] aoStyleValues = null;
        Object [,] aoAlphaValues = null;
        Object [,] aoVisibilityValues = null;
        Object [,] aoLabelValues = null;
        Object [,] aoLabelTextColorValues = null;
        Object [,] aoLabelFontSizeValues = null;

        ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
            EdgeTableColumnNames.Color, out oColorColumnData,
            out aoColorValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
            EdgeTableColumnNames.Width, out oWidthColumnData,
            out aoWidthValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
            EdgeTableColumnNames.Style, out oStyleColumnData,
            out aoStyleValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
            CommonTableColumnNames.Alpha, out oAlphaColumnData,
            out aoAlphaValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
            CommonTableColumnNames.Visibility, out oVisibilityColumnData,
            out aoVisibilityValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
            EdgeTableColumnNames.Label, out oLabelColumnData,
            out aoLabelValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
            EdgeTableColumnNames.LabelTextColor, out oLabelTextColorColumnData,
            out aoLabelTextColorValues);

        ExcelTableUtil.TryGetTableColumnDataAndValues(oEdgeTable,
            EdgeTableColumnNames.LabelFontSize, out oLabelFontSizeColumnData,
            out aoLabelFontSizeValues);

        ColorConverter2 oColorConverter2 = new ColorConverter2();
        EdgeStyleConverter oEdgeStyleConverter = new EdgeStyleConverter();

        EdgeVisibilityConverter oEdgeVisibilityConverter =
            new EdgeVisibilityConverter();

        // Loop through the IDs of the edges whose attributes were edited
        // in the graph.

        EditedEdgeAttributes oEditedEdgeAttributes = e.EditedEdgeAttributes;

        foreach (Int32 iID in e.EdgeIDs)
        {
            // Look for the row that contains the ID.

            Int32 iRowOneBased;

            if ( !oRowIDDictionary.TryGetValue(iID, out iRowOneBased) )
            {
                continue;
            }

            iRowOneBased -= oEdgeTable.Range.Row;

            if (oEditedEdgeAttributes.Color.HasValue && aoColorValues != null)
            {
                aoColorValues[iRowOneBased, 1] =
                    oColorConverter2.GraphToWorkbook(
                        oEditedEdgeAttributes.Color.Value);
            }

            if (oEditedEdgeAttributes.Width.HasValue && aoWidthValues != null)
            {
                aoWidthValues[iRowOneBased, 1] =
                    oEditedEdgeAttributes.Width.Value.ToString();
            }

            if (oEditedEdgeAttributes.Style.HasValue && aoStyleValues != null)
            {
                aoStyleValues[iRowOneBased, 1] =
                    oEdgeStyleConverter.GraphToWorkbook(
                        oEditedEdgeAttributes.Style.Value);
            }

            if (oEditedEdgeAttributes.Alpha.HasValue && aoAlphaValues != null)
            {
                aoAlphaValues[iRowOneBased, 1] =
                    oEditedEdgeAttributes.Alpha.Value.ToString();
            }

            if (oEditedEdgeAttributes.Visibility.HasValue &&
                aoVisibilityValues != null)
            {
                aoVisibilityValues[iRowOneBased, 1] =
                    oEdgeVisibilityConverter.GraphToWorkbook(
                        oEditedEdgeAttributes.Visibility.Value);
            }

            if ( !String.IsNullOrEmpty(oEditedEdgeAttributes.Label) )
            {
                aoLabelValues[iRowOneBased, 1] = oEditedEdgeAttributes.Label;
            }

            if (oEditedEdgeAttributes.LabelTextColor.HasValue &&
                aoLabelTextColorValues != null)
            {
                aoLabelTextColorValues[iRowOneBased, 1] =
                    oColorConverter2.GraphToWorkbook(
                        oEditedEdgeAttributes.LabelTextColor.Value);
            }

            if (oEditedEdgeAttributes.LabelFontSize.HasValue &&
                aoLabelFontSizeValues != null)
            {
                aoLabelFontSizeValues[iRowOneBased, 1] =
                    oEditedEdgeAttributes.LabelFontSize.Value.ToString();
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
                oWidthColumnData, aoWidthValues,
                oStyleColumnData, aoStyleValues,
                oAlphaColumnData, aoAlphaValues,
                oVisibilityColumnData, aoVisibilityValues,
                oLabelColumnData, aoLabelValues,
                oLabelTextColorColumnData, aoLabelTextColorValues,
                oLabelFontSizeColumnData, aoLabelFontSizeValues
                );
        }
        finally
        {
            oExcelActiveWorksheetRestorer.Restore(oExcelActiveWorksheetState);
        }

        Globals.ThisWorkbook.ShowWaitCursor = false;
    }

    //*************************************************************************
    //  Method: Sheet1_Shutdown()
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
    Sheet1_Shutdown
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
        this.Startup += new System.EventHandler(Sheet1_Startup);
        this.Shutdown += new System.EventHandler(Sheet1_Shutdown);
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
