

using System;
using System.Configuration;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupLabelColumnAutoFillUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="GroupLabelColumnAutoFillUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="GroupLabelColumnAutoFillUserSettings" /> object to the
/// constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GroupLabelColumnAutoFillUserSettingsDialog
    : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GroupLabelColumnAutoFillUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupLabelColumnAutoFillUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="groupLabelColumnAutoFillUserSettings">
    /// Object to edit.
    /// </param>
    //*************************************************************************

    public GroupLabelColumnAutoFillUserSettingsDialog
    (
        GroupLabelColumnAutoFillUserSettings
            groupLabelColumnAutoFillUserSettings
    )
    {
        Debug.Assert(groupLabelColumnAutoFillUserSettings != null);

        m_oGroupLabelColumnAutoFillUserSettings =
            groupLabelColumnAutoFillUserSettings;

        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oGroupLabelColumnAutoFillUserSettingsDialogUserSettings =
            new GroupLabelColumnAutoFillUserSettingsDialogUserSettings(this);

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
            m_oGroupLabelColumnAutoFillUserSettings.PrependWithGroupName =
                chkPrependWithGroupName.Checked;
        }
        else
        {
            chkPrependWithGroupName.Checked =
                m_oGroupLabelColumnAutoFillUserSettings.PrependWithGroupName;
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

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oGroupLabelColumnAutoFillUserSettings != null);

        Debug.Assert(
            m_oGroupLabelColumnAutoFillUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object being edited.

    protected GroupLabelColumnAutoFillUserSettings
        m_oGroupLabelColumnAutoFillUserSettings;

    /// User settings for this dialog.

    protected GroupLabelColumnAutoFillUserSettingsDialogUserSettings
        m_oGroupLabelColumnAutoFillUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GroupLabelColumnAutoFillUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GroupLabelColumnAutoFillUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GroupLabelColumnAutoFillUserSettingsDialog") ]

public class GroupLabelColumnAutoFillUserSettingsDialogUserSettings
    : FormSettings
{
    //*************************************************************************
    //  Constructor: GroupLabelColumnAutoFillUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupLabelColumnAutoFillUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GroupLabelColumnAutoFillUserSettingsDialogUserSettings
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
