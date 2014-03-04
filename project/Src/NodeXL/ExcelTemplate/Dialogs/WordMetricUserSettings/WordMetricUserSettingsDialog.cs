

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WordMetricUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="WordMetricUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="WordMetricUserSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class WordMetricUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: WordMetricUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="WordMetricUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="wordMetricUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public WordMetricUserSettingsDialog
    (
        WordMetricUserSettings wordMetricUserSettings,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(wordMetricUserSettings != null);
        Debug.Assert(workbook != null);

        m_oWordMetricUserSettings = wordMetricUserSettings;

        InitializeComponent();

        // Populate the column name ComboBoxes with column names from the
        // workbook.

        ListObject oTable;

        if ( ExcelTableUtil.TryGetTable(workbook, WorksheetNames.Edges,
            TableNames.Edges, out oTable) )
        {
            cbxEdgeColumnName.PopulateWithTableColumnNames(oTable);
        }

        if ( ExcelTableUtil.TryGetTable(workbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oTable) )
        {
            cbxVertexColumnName.PopulateWithTableColumnNames(oTable);
        }

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oWordMetricUserSettingsDialogUserSettings =
            new WordMetricUserSettingsDialogUserSettings(this);

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
        String sTextColumnName;

        if (bFromControls)
        {
            Boolean bTextColumnIsOnEdgeWorksheet = radOnEdgeWorksheet.Checked;

            ComboBox oComboBoxToValidate = bTextColumnIsOnEdgeWorksheet ?
                (ComboBox)cbxEdgeColumnName : (ComboBox)cbxVertexColumnName;

            if ( !ValidateRequiredComboBox(oComboBoxToValidate,
                "Enter or select the column containing the text.",
                out sTextColumnName) )
            {
                return (false);
            }

            m_oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet =
                bTextColumnIsOnEdgeWorksheet;

            m_oWordMetricUserSettings.TextColumnName = sTextColumnName;
            m_oWordMetricUserSettings.CountByGroup = chkCountByGroup.Checked;

            m_oWordMetricUserSettings.SkipSingleTerms =
                chkSkipSingleTerms.Checked;

            m_oWordMetricUserSettings.WordsToSkip = txbWordsToSkip.Text.Trim();
        }
        else
        {
            sTextColumnName = m_oWordMetricUserSettings.TextColumnName;

            if (m_oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet)
            {
                radOnEdgeWorksheet.Checked = true;
                cbxEdgeColumnName.Text = sTextColumnName;
            }
            else
            {
                radOnVertexWorksheet.Checked = true;
                cbxVertexColumnName.Text = sTextColumnName;
            }

            chkCountByGroup.Checked = m_oWordMetricUserSettings.CountByGroup;

            chkSkipSingleTerms.Checked =
                m_oWordMetricUserSettings.SkipSingleTerms;

            txbWordsToSkip.Text = m_oWordMetricUserSettings.WordsToSkip;

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

        cbxVertexColumnName.Enabled =
            !(cbxEdgeColumnName.Enabled = radOnEdgeWorksheet.Checked);
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

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oWordMetricUserSettings != null);
        Debug.Assert(m_oWordMetricUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object being edited.

    protected WordMetricUserSettings m_oWordMetricUserSettings;

    /// User settings for this dialog.

    protected WordMetricUserSettingsDialogUserSettings
        m_oWordMetricUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: WordMetricUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="WordMetricUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("WordMetricUserSettingsDialog") ]

public class WordMetricUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: WordMetricUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="WordMetricUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public WordMetricUserSettingsDialogUserSettings
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
