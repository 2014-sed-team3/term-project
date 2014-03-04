
using System;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ImportDataUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="ImportDataUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass <see cref="ImportDataUserSettings" /> and <see
/// cref="PlugInUserSettings" /> objects to the constructor.  If the user edits
/// the objects, <see cref="Form.ShowDialog()" /> returns DialogResult.OK.
/// Otherwise, the objects are not modified and
/// <see cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class ImportDataUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ImportDataUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportDataUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="importDataUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="plugInUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="thisWorkbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public ImportDataUserSettingsDialog
    (
        ImportDataUserSettings importDataUserSettings,
        PlugInUserSettings plugInUserSettings,
        ThisWorkbook thisWorkbook
    )
    {
        Debug.Assert(importDataUserSettings != null);
        importDataUserSettings.AssertValid();
        Debug.Assert(thisWorkbook != null);

        m_oImportDataUserSettings = importDataUserSettings;
		m_oPlugInUserSettings = plugInUserSettings;
        m_oThisWorkbook = thisWorkbook;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oImportDataUserSettingsDialogUserSettings =
            new ImportDataUserSettingsDialogUserSettings(this);

        InitializeComponent();
        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            if ( !usrPlugInFolderPath.Validate() )
            {
                return (false);
            }

            m_oImportDataUserSettings.ClearTablesBeforeImport =
                this.chkClearTablesBeforeImport.Checked;

            m_oImportDataUserSettings.SaveImportDescription =
                this.chkSaveImportDescription.Checked;

            m_oImportDataUserSettings.AutomateAfterImport =
                this.chkAutomateAfterImport.Checked;

            m_oPlugInUserSettings.PlugInFolderPath =
                this.usrPlugInFolderPath.FolderPath;
        }
        else
        {
            this.chkClearTablesBeforeImport.Checked =
                m_oImportDataUserSettings.ClearTablesBeforeImport;

            this.chkSaveImportDescription.Checked =
                m_oImportDataUserSettings.SaveImportDescription;

            this.chkAutomateAfterImport.Checked =
                m_oImportDataUserSettings.AutomateAfterImport;

            this.usrPlugInFolderPath.FolderPath =
                m_oPlugInUserSettings.PlugInFolderPath;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
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
    btnOK_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        if ( DoDataExchange(true) )
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    //*************************************************************************
    //  Method: btnAutomateTasksUserSettings_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAutomateTasksUserSettings button.
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
    btnAutomateTasksUserSettings_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        ( new AutomateTasksDialog(AutomateTasksDialog.DialogMode.EditOnly,
            m_oThisWorkbook, null) ).ShowDialog();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")] 

    public  override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oImportDataUserSettings != null);
		Debug.Assert(m_oPlugInUserSettings != null);
        Debug.Assert(m_oThisWorkbook != null);
        Debug.Assert(m_oImportDataUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected ImportDataUserSettings m_oImportDataUserSettings;

	/// The user's plug-in settings.

	protected PlugInUserSettings m_oPlugInUserSettings;

    /// Workbook containing the graph contents.

    protected ThisWorkbook m_oThisWorkbook;

    /// User settings for this dialog.

    protected ImportDataUserSettingsDialogUserSettings
        m_oImportDataUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: ImportDataUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ImportDataUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ImportDataUserSettingsDialog") ]

public class ImportDataUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ImportDataUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportDataUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ImportDataUserSettingsDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
    {
        Debug.Assert(oForm != null);

        // (Do nothing.)

        AssertValid();
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
