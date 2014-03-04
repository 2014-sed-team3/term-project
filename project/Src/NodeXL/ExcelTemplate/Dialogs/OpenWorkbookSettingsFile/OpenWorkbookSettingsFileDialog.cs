
using System;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OpenWorkbookSettingsFileDialog
//
/// <summary>
/// Represents a dialog box for opening a workbook settings file.
/// </summary>
///
/// <remarks>
/// Call <see cref="ShowDialogAndOpenWorkbookSettingsFile" /> to allow the user
/// to open a workbook settings file from a location of his choice.
/// </remarks>
//*****************************************************************************

public class OpenWorkbookSettingsFileDialog : OpenFileDialog2
{
    //*************************************************************************
    //  Constructor: OpenWorkbookSettingsFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OpenWorkbookSettingsFileDialog" /> class.
    /// </summary>
    //*************************************************************************

    public OpenWorkbookSettingsFileDialog()
    :
    base
    (
        String.Empty,
        String.Empty
    )
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Method: ShowDialogAndOpenWorkbookSettingsFile()
    //
    /// <summary>
    /// Opens a workbook settings file.
    /// </summary>
    ///
    /// <param name="workbookSettings">
    /// Where the contents of the workbook settings file get stored if
    /// DialogResults.OK is returned.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.OK if the user selected a file name and workbook settings
    /// were successfully read from the file.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select a workbook settings file name.
    /// It then opens the file and reads the contents.
    /// </remarks>
    //*************************************************************************

    public DialogResult
    ShowDialogAndOpenWorkbookSettingsFile
    (
        out String workbookSettings
    )
    {
        AssertValid();

        // Let the base class do most of the work.  ShowDialogAndOpenObject()
        // calls OpenObject(), which will open the file and read it.

        Object oObject;

        DialogResult oDialogResult = ShowDialogAndOpenObject(out oObject);

        Debug.Assert(oObject == null || oObject is String);
        workbookSettings = (String)oObject;

        return (oDialogResult);
    }

    //*************************************************************************
    //  Method: GetDialogTitle()
    //
    /// <summary>
    /// Gets the title to use for the dialog.
    /// </summary>
    //*************************************************************************

    protected override String
    GetDialogTitle()
    {
        AssertValid();

        return (DialogTitle);
    }

    //*************************************************************************
    //  Method: GetFilter()
    //
    /// <summary>
    /// Gets the filter to use for the dialog.
    /// </summary>
    //*************************************************************************

    protected override String
    GetFilter()
    {
        AssertValid();

        return (Filter);
    }

    //*************************************************************************
    //  Method: OpenObject()
    //
    /// <summary>
    /// Opens a workbook settings file and reads it.
    /// </summary>
    ///
    /// <param name="sFileName">
    /// File name to open, including a full path.
    /// </param>
    ///
    /// <param name="oObject">
    /// Where the contents of the workbook settings file get stored.
    /// </param>
    ///
    /// <remarks>
    /// This is called by the base-class ShowDialogAndOpenObject() method.
    /// </remarks>
    //*************************************************************************

    protected override void
    OpenObject
    (
        String sFileName,
        out Object oObject
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sFileName) );
        Debug.Assert( File.Exists(sFileName) );
        AssertValid();

        oObject = null;
        String sWorkbookSettings = FileUtil.ReadTextFile(sFileName);

        // Test for valid XML.

        XmlDocument oXmlDocument = new XmlDocument();

        try
        {
            oXmlDocument.LoadXml(sWorkbookSettings);
        }
        catch (XmlException)
        {
            throw new XmlException(
                "This does not appear to be a NodeXL options file."
                );
        }

        oObject = sWorkbookSettings;
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
    //  Public constants
    //*************************************************************************

    /// Filter to use for this dialog and SaveWorkbookSettingsFileDialog.

    public const String Filter =
        "NodeXL Options Files (*.NodeXLOptions)|*.NodeXLOptions";


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Title to use for this dialog.

    protected const String DialogTitle =
        "Import Options";
}

}
