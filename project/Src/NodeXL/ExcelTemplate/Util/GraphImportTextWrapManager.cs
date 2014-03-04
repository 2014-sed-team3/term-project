
using System;
using System.Windows.Forms;
using System.Drawing;
using Smrf.NodeXL.Core;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphImportTextWrapManager
//
/// <summary>
/// Manages text wrapping in a workbook before a graph is imported into the
/// workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="ManageTextWrapBeforeImport" /> before importing a graph
/// into the workbook.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class GraphImportTextWrapManager : Object
{
    //*************************************************************************
    //  Method: ManageTextWrapBeforeImport()
    //
    /// <summary>
    /// If necessary, turns text wrapping off in a workbook before a graph is
    /// imported into it.
    /// </summary>
    ///
    /// <param name="sourceGraph">
    /// Graph that edges and vertices will be imported from.
    /// </param>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// NodeXL workbook the edges and vertices will be imported to.
    /// </param>
    ///
    /// <param name="notifyUser">
    /// true to notify the user if text wrapping is turned off.
    /// </param>
    ///
    /// <returns>
    /// true if the import should continue, false if the user cancelled.
    /// </returns>
    ///
    /// <remarks>
    /// Importing a large number of rows into a workbook is very slow when
    /// Excel's text wrapping feature is turned on in some of the cells.  This
    /// method turns of text wrapping in the edge and vertex tables if the
    /// graph that will be imported into the workbook is large.
    ///
    /// <para>
    /// Note that this method does NOT actually import the graph into the
    /// workbook.  Use the <see cref="GraphImporter" /> class for that after
    /// calling this method.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Boolean
    ManageTextWrapBeforeImport
    (
        IGraph sourceGraph,
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook,
        Boolean notifyUser
    )
    {
        Debug.Assert(sourceGraph != null);
        Debug.Assert(destinationNodeXLWorkbook != null);

        if (sourceGraph.Edges.Count > RowThreshold ||
            sourceGraph.Vertices.Count > RowThreshold)
        {
            if ( notifyUser && !NotifyUser() )
            {
                return (false);
            }

            ExcelTableUtil.SetTableWrapText(destinationNodeXLWorkbook,
                WorksheetNames.Edges, TableNames.Edges, false);

            ExcelTableUtil.SetTableWrapText(destinationNodeXLWorkbook,
                WorksheetNames.Vertices, TableNames.Vertices, false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: NotifyUser()
    //
    /// <summary>
    /// Notifies the user that text wrapping will be turned off.
    /// </summary>
    ///
    /// <returns>
    /// true if the import should continue, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    private static Boolean
    NotifyUser()
    {
        NotificationUserSettings oNotificationUserSettings =
            new NotificationUserSettings();

        if (!oNotificationUserSettings.TextWrapWillBeTurnedOff)
        {
            // The user doesn't want to be notified.

            return (true);
        }

        Boolean bReturn = true;

        const String Message =
            "To speed up the import, text wrapping will be turned off in most"
            + " of the NodeXL workbook.  Do you want to continue with the"
            + " import?"
            ;

        NotificationDialog oNotificationDialog = new NotificationDialog(
            "Text Wrapping", SystemIcons.Warning, Message);

        if (oNotificationDialog.ShowDialog() != DialogResult.Yes)
        {
            bReturn = false;
        }

        if (oNotificationDialog.DisableFutureNotifications)
        {
            oNotificationUserSettings.TextWrapWillBeTurnedOff = false;
            oNotificationUserSettings.Save();
        }

        return (bReturn);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Maximum number of rows to import without turning text wrapping on.

    private const Int32 RowThreshold = 100;
}

}
