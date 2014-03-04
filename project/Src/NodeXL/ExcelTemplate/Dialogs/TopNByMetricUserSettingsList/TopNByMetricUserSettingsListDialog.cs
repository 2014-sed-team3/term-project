

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TopNByMetricUserSettingsListDialog
//
/// <summary>
/// Edits a collection of <see cref="TopNByMetricUserSettings" /> objects.
/// </summary>
///
/// <remarks>
/// Pass a List of <see cref="TopNByMetricUserSettings" /> objects to the
/// constructor.  If the user edits the List, <see cref="Form.ShowDialog()" />
/// returns DialogResult.OK.  Otherwise, <see cref="Form.ShowDialog()" />
/// returns DialogResult.Cancel and the List isn't modified.
/// </remarks>
//*****************************************************************************

public partial class TopNByMetricUserSettingsListDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: TopNByMetricUserSettingsListDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TopNByMetricUserSettingsListDialog" /> class.
    /// </summary>
    ///
    /// <param name="topNByMetricUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public TopNByMetricUserSettingsListDialog
    (
        List<TopNByMetricUserSettings> topNByMetricUserSettings,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(topNByMetricUserSettings != null);
        Debug.Assert(workbook != null);

        m_oTopNByMetricUserSettings = topNByMetricUserSettings;

        // This dialog edits a clone of the List, then copies the clone back to
        // the original List when OK is pressed.

        m_oTopNByMetricUserSettingsClone = new List<TopNByMetricUserSettings>(
            m_oTopNByMetricUserSettings.Count);

        foreach (TopNByMetricUserSettings oTopNByMetricUserSettings in
            m_oTopNByMetricUserSettings)
        {
            m_oTopNByMetricUserSettingsClone.Add(
                oTopNByMetricUserSettings.Clone() );
        }

        m_oWorkbook = workbook;

        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oTopNByMetricUserSettingsListDialogUserSettings =
            new TopNByMetricUserSettingsListDialogUserSettings(this);

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
            // Do nothing.  The handlers for the add, edit, and delete buttons
            // update the m_oTopNByMetricUserSettingsClone List.
        }
        else
        {
            PopulateTopNByMetricUserSettingsListBox();
            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: PopulateTopNByMetricUserSettingsListBox()
    //
    /// <summary>
    /// Populates the list of TopNByMetricUserSettings objects.
    /// </summary>
    //*************************************************************************

    protected void
    PopulateTopNByMetricUserSettingsListBox()
    {
        AssertValid();

        ListBox.ObjectCollection oItems =
            this.lbxTopNByMetricUserSettings.Items;

        oItems.Clear();

        foreach (TopNByMetricUserSettings oTopNByMetricUserSettings in
            m_oTopNByMetricUserSettingsClone)
        {
            // Note that the TopNByMetricUserSettings.ToString() method
            // provides a description of the object that the
            // lbxTopNByMetricUserSettings ListBox will automatically display.

            oItems.Add(oTopNByMetricUserSettings);
        }
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

        btnEdit.Enabled = btnDelete.Enabled =
            (lbxTopNByMetricUserSettings.SelectedIndex >= 0);
    }

    //*************************************************************************
    //  Method: TryEditTopNByMetricUserSettings()
    //
    /// <summary>
    /// Lets the user edit a new TopNByMetricUserSettings object.
    /// </summary>
    ///
    /// <param name="oTopNByMetricUserSettings">
    /// Object to edit.
    /// </param>
    ///
    /// <returns>
    /// true if the user edited the object.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryEditTopNByMetricUserSettings
    (
        TopNByMetricUserSettings oTopNByMetricUserSettings
    )
    {
        Debug.Assert(oTopNByMetricUserSettings != null);
        AssertValid();

        TopNByMetricUserSettingsDialog oTopNByMetricUserSettingsDialog =
            new TopNByMetricUserSettingsDialog(
                oTopNByMetricUserSettings, m_oWorkbook);

        return (oTopNByMetricUserSettingsDialog.ShowDialog() ==
            DialogResult.OK);
    }

    //*************************************************************************
    //  Method: lbxTopNByMetricUserSettings_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the
    /// lbxTopNByMetricUserSettings ListBox.
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
    lbxTopNByMetricUserSettings_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
    }

    //*************************************************************************
    //  Method: btnAdd_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAdd button.
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
    btnAdd_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // Let the user edit a new TopNByMetricUserSettings object.

        TopNByMetricUserSettings oTopNByMetricUserSettings =
            new TopNByMetricUserSettings();

        if ( TryEditTopNByMetricUserSettings(oTopNByMetricUserSettings) )
        {
            // Add the new object to the List and the ListBox, then select the
            // object in the ListBox.

            m_oTopNByMetricUserSettingsClone.Add(oTopNByMetricUserSettings);
            DoDataExchange(false);

            lbxTopNByMetricUserSettings.SelectedIndex =
                lbxTopNByMetricUserSettings.Items.Count - 1;
        }
    }

    //*************************************************************************
    //  Method: btnEdit_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdit button.
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
    btnEdit_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // Let the user edit the selected TopNByMetricUserSettings object.

        Int32 iSelectedIndex = lbxTopNByMetricUserSettings.SelectedIndex;

        Debug.Assert(iSelectedIndex >= 0);

        Debug.Assert(lbxTopNByMetricUserSettings.SelectedItem is
            TopNByMetricUserSettings);

        TopNByMetricUserSettings oTopNByMetricUserSettings =
            (TopNByMetricUserSettings)lbxTopNByMetricUserSettings.SelectedItem;

        if ( TryEditTopNByMetricUserSettings(oTopNByMetricUserSettings) )
        {
            // Replace the object in the ListBox, then select the object in the
            // ListBox.

            DoDataExchange(false);
            lbxTopNByMetricUserSettings.SelectedIndex = iSelectedIndex;
        }
    }

    //*************************************************************************
    //  Method: btnDelete_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnDelete button.
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
    btnDelete_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        Int32 iSelectedIndex = lbxTopNByMetricUserSettings.SelectedIndex;

        Debug.Assert(iSelectedIndex >= 0);

        // Remove the selected object from the List and the ListBox, then
        // select the preceeding object in the ListBox.

        m_oTopNByMetricUserSettingsClone.RemoveAt(iSelectedIndex);
        DoDataExchange(false);

        Int32 iItems = lbxTopNByMetricUserSettings.Items.Count;

        if (iItems > 0)
        {
            lbxTopNByMetricUserSettings.SelectedIndex =
                Math.Min(iSelectedIndex, iItems - 1);
        }
    }

    //*************************************************************************
    //  Method: lbxTopNByMetricUserSettings_MouseDoubleClick()
    //
    /// <summary>
    /// Handles the MouseDoubleClick event on the lbxTopNByMetricUserSettings
    /// ListBox.
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
    lbxTopNByMetricUserSettings_MouseDoubleClick
    (
        object sender,
        MouseEventArgs e
    )
    {
        AssertValid();

        if (lbxTopNByMetricUserSettings.SelectedIndex >= 0)
        {
            // Double-clicking an item is the same as selecting an item and
            // clicking the Edit button.

            btnEdit_Click(sender, e);
        }
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
            m_oTopNByMetricUserSettings.Clear();

            m_oTopNByMetricUserSettings.AddRange(
                m_oTopNByMetricUserSettingsClone);

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

        Debug.Assert(m_oTopNByMetricUserSettings != null);
        Debug.Assert(m_oTopNByMetricUserSettingsClone != null);
        Debug.Assert(m_oWorkbook != null);

        Debug.Assert(m_oTopNByMetricUserSettingsListDialogUserSettings !=
            null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object being edited.

    protected List<TopNByMetricUserSettings> m_oTopNByMetricUserSettings;

    /// Clone of the object being edited.

    protected List<TopNByMetricUserSettings> m_oTopNByMetricUserSettingsClone;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// User settings for this dialog.

    protected TopNByMetricUserSettingsListDialogUserSettings
        m_oTopNByMetricUserSettingsListDialogUserSettings;
}


//*****************************************************************************
//  Class: TopNByMetricUserSettingsListDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="TopNByMetricUserSettingsListDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("TopNByMetricUserSettingsListDialog") ]

public class TopNByMetricUserSettingsListDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: TopNByMetricUserSettingsListDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TopNByMetricUserSettingsListDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public TopNByMetricUserSettingsListDialogUserSettings
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
