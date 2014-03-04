

using System;
using System.Configuration;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="GroupUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="GroupUserSettings" /> object to the constructor.  If the
/// user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GroupUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GroupUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="groupUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public GroupUserSettingsDialog
    (
        GroupUserSettings groupUserSettings
    )
    {
        Debug.Assert(groupUserSettings != null);

        m_oGroupUserSettings = groupUserSettings;

        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oGroupUserSettingsDialogUserSettings =
            new GroupUserSettingsDialogUserSettings(this);

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
            m_oGroupUserSettings.ReadGroups = !chkDoNotReadGroups.Checked;

            m_oGroupUserSettings.ReadVertexColorFromGroups =
                radReadVertexColorFromGroups.Checked;

            m_oGroupUserSettings.ReadVertexShapeFromGroups =
                radReadVertexShapeFromGroups.Checked;
        }
        else
        {
            chkDoNotReadGroups.Checked = !m_oGroupUserSettings.ReadGroups;

            radReadVertexColorFromGroups.Checked =
                m_oGroupUserSettings.ReadVertexColorFromGroups;

            radReadVertexColorFromVertices.Checked =
                !radReadVertexColorFromGroups.Checked;

            radReadVertexShapeFromGroups.Checked =
                m_oGroupUserSettings.ReadVertexShapeFromGroups;

            radReadVertexShapeFromVertices.Checked =
                !radReadVertexShapeFromGroups.Checked;

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

        EnableControls(!chkDoNotReadGroups.Checked, grpVertexColor,
            grpVertexShape, lblLayout);
    }

    //*************************************************************************
    //  Method: chkDoNotReadGroups_CheckedChanged()
    //
    /// <summary>
    /// Handles the CheckedChanged event on the chkDoNotReadGroups button.
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
    chkDoNotReadGroups_CheckedChanged
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

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oGroupUserSettings != null);
        Debug.Assert(m_oGroupUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected GroupUserSettings m_oGroupUserSettings;

    /// User settings for this dialog.

    protected GroupUserSettingsDialogUserSettings
        m_oGroupUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GroupUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="GroupUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GroupUserSettingsDialog") ]

public class GroupUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GroupUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GroupUserSettingsDialogUserSettings
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
