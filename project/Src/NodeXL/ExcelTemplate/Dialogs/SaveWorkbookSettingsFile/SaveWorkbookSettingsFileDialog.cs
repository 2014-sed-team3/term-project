
using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SaveWorkbookSettingsFileDialog
//
/// <summary>
/// Save's the workbook settings to an external file.
/// </summary>
///
/// <remarks>
/// Call <see cref="ShowDialogAndSaveWorkbookSettings" /> to allow the
/// user to save the workbook settings to a location of his choice.
/// </remarks>
//*****************************************************************************

public class SaveWorkbookSettingsFileDialog : SaveFileDialog2
{
    //*************************************************************************
    //  Constructor: SaveWorkbookSettingsFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SaveWorkbookSettingsFileDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="initialDirectory">
    /// Initial directory the dialog will display.  Use an empty string to let
    /// the dialog select an initial directory.
    /// </param>
    ///
    /// <param name="initialFileName">
    /// Initial file name.  Can be a complete path, a path without an
    /// extension, a file name, or a file name without an extension.
    /// </param>
    //*************************************************************************

    public SaveWorkbookSettingsFileDialog
    (
        String initialDirectory,
        String initialFileName

    ) : base(initialDirectory, initialFileName)
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Method: ShowDialogAndSaveWorkbookSettings()
    //
    /// <summary>
    /// Shows the file save dialog and saves the workbook settings to the
    /// selected file.
    /// </summary>
    ///
    /// <param name="workbookSettings">
    /// Workbook settings to save.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.OK if the user selected a file name and the workbook
    /// settings were successfully saved.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select a file name.  It then saves the
    /// workbook settings to the file.
    /// </remarks>
    //*************************************************************************

    public DialogResult
    ShowDialogAndSaveWorkbookSettings
    (
        String workbookSettings
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(workbookSettings) );
        AssertValid();

        // Let the base class do most of the work.  The actual saving will be
        // done by SaveObject().

        return ( ShowDialogAndSaveObject(workbookSettings) );
    }

    //*************************************************************************
    //  Method: GetDialogTitle()
    //
    /// <summary>
    /// Returns the title to use for the dialog.
    /// </summary>
    ///
    /// <param name="oObjectBeingSaved">
    /// Object being saved.
    /// </param>
    ///
    /// <returns>
    /// Title to use for the dialog.
    /// </returns>
    //*************************************************************************

    protected override String
    GetDialogTitle
    (
        Object oObjectBeingSaved
    )
    {
        return (DialogTitle);
    }

    //*************************************************************************
    //  Method: GetFilter()
    //
    /// <summary>
    /// Returns the filter to use for the dialog.
    /// </summary>
    ///
    /// <returns>
    /// Filter to use for the dialog.
    /// </returns>
    //*************************************************************************

    protected override String
    GetFilter()
    {
        return (OpenWorkbookSettingsFileDialog.Filter);
    }

    //*************************************************************************
    //  Method: SaveObject()
    //
    /// <summary>
    /// Saves the object to the specified file.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to save.
    /// </param>
    ///
    /// <param name="sFileName">
    /// File name to save the object to.
    /// </param>
    ///
    /// <remarks>
    /// This is called by the base-class ShowDialogAndSaveObject() method.
    /// </remarks>
    //*************************************************************************

    protected override void
    SaveObject
    (
        Object oObject,
        String sFileName
    )
    {
        Debug.Assert(oObject is String);

        using ( StreamWriter oStreamWriter = new StreamWriter(sFileName) )
        {
            oStreamWriter.Write( (String)oObject );
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
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Title to use for this dialog.

    protected const String DialogTitle =
        "Export Options";
}

}
