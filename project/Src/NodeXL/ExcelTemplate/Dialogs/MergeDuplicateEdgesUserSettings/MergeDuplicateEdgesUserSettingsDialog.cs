
using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MergeDuplicateEdgesUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="MergeDuplicateEdgesUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="MergeDuplicateEdgesUserSettings" /> object to the
/// constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class MergeDuplicateEdgesUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: MergeDuplicateEdgesUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="MergeDuplicateEdgesUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the dialog is being used.
    /// </param>
    ///
    /// <param name="mergeDuplicateEdgesUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public MergeDuplicateEdgesUserSettingsDialog
    (
        DialogMode mode,
        MergeDuplicateEdgesUserSettings mergeDuplicateEdgesUserSettings,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(mergeDuplicateEdgesUserSettings != null);
        mergeDuplicateEdgesUserSettings.AssertValid();
        Debug.Assert(workbook != null);

        m_oMergeDuplicateEdgesUserSettings = mergeDuplicateEdgesUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oMergeDuplicateEdgesUserSettingsDialogUserSettings =
            new MergeDuplicateEdgesUserSettingsDialogUserSettings(this);

        InitializeComponent();

        if (mode == DialogMode.EditOnly)
        {
            this.Text += " Options";
        }

        ListObject oEdgeTable;

        if ( ExcelTableUtil.TryGetTable(workbook, WorksheetNames.Edges,
            TableNames.Edges, out oEdgeTable) )
        {
            cbxThirdColumnNameForDuplicateDetection
                .PopulateWithTableColumnNames(oEdgeTable);
        }

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Enum: DialogMode
    //
    /// <summary>
    /// Indicates the mode in which the dialog is being used.
    /// </summary>
    //*************************************************************************

    public enum
    DialogMode
    {
        /// The caller will merge duplicate edges after the user edits the
        /// user settings with this dialog.

        Normal,

        /// The user can edit the user settings but the caller will not merge
        /// duplicate edges afterward.

        EditOnly,
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
            Boolean bCountDuplicates = chkCountDuplicates.Checked;
            Boolean bDeleteDuplicates = chkDeleteDuplicates.Checked;

            if (!bCountDuplicates && !bDeleteDuplicates)
            {
                return ( OnInvalidControl(chkCountDuplicates,
                    "Specify whether you want to count or merge duplicate"
                    + " edges."
                    ) );
            }

            String sThirdColumnNameForDuplicateDetection = null;

            if (
                radVerticesAndColumn.Checked
                &&
                !ValidateRequiredComboBox(
                    cbxThirdColumnNameForDuplicateDetection,

                    "Enter or select the column to use for determining"
                        + " whether edges are duplicates.",

                    out sThirdColumnNameForDuplicateDetection) )
            {
                return (false);
            }

            m_oMergeDuplicateEdgesUserSettings.CountDuplicates =
                bCountDuplicates;

            m_oMergeDuplicateEdgesUserSettings.DeleteDuplicates =
                bDeleteDuplicates;

            m_oMergeDuplicateEdgesUserSettings
                .ThirdColumnNameForDuplicateDetection
                = sThirdColumnNameForDuplicateDetection;
        }
        else
        {
            chkCountDuplicates.Checked =
                m_oMergeDuplicateEdgesUserSettings.CountDuplicates;

            chkDeleteDuplicates.Checked =
                m_oMergeDuplicateEdgesUserSettings.DeleteDuplicates;

            String sThirdColumnNameForDuplicateDetection =
                m_oMergeDuplicateEdgesUserSettings
                .ThirdColumnNameForDuplicateDetection;

            if ( String.IsNullOrEmpty(sThirdColumnNameForDuplicateDetection) )
            {
                radVerticesOnly.Checked = true;
            }
            else
            {
                radVerticesAndColumn.Checked = true;

                cbxThirdColumnNameForDuplicateDetection.Text =
                    sThirdColumnNameForDuplicateDetection;
            }

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected void
    EnableControls()
    {
        AssertValid();

        cbxThirdColumnNameForDuplicateDetection.Enabled =
            radVerticesAndColumn.Checked;
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
    /// Handles any event that should changed the enabled state of the dialog's
    /// controls.
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
    OnEventThatRequiresControlEnabling
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
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
        if ( DoDataExchange(true) )
        {
            DialogResult = DialogResult.OK;
            this.Close();
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

    public  override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oMergeDuplicateEdgesUserSettings != null);

        Debug.Assert(m_oMergeDuplicateEdgesUserSettingsDialogUserSettings !=
            null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected MergeDuplicateEdgesUserSettings
        m_oMergeDuplicateEdgesUserSettings;

    /// User settings for this dialog.

    protected MergeDuplicateEdgesUserSettingsDialogUserSettings
        m_oMergeDuplicateEdgesUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: MergeDuplicateEdgesUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="MergeDuplicateEdgesUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("MergeDuplicateEdgesUserSettingsDialog") ]

public class MergeDuplicateEdgesUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: MergeDuplicateEdgesUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="MergeDuplicateEdgesUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public MergeDuplicateEdgesUserSettingsDialogUserSettings
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
