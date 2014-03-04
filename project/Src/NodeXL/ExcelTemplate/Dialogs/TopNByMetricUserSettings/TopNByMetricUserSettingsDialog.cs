

using System;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TopNByMetricUserSettingsDialog
//
/// <summary>
/// Edits one <see cref="TopNByMetricUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="TopNByMetricUserSettings" /> object to the constructor.
/// If the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel and the object isn't modified.
/// </remarks>
//*****************************************************************************

public partial class TopNByMetricUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: TopNByMetricUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TopNByMetricUserSettingsDialog" /> class.
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

    public TopNByMetricUserSettingsDialog
    (
        TopNByMetricUserSettings topNByMetricUserSettings,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(topNByMetricUserSettings != null);
        Debug.Assert(workbook != null);

        m_oTopNByMetricUserSettings = topNByMetricUserSettings;
        m_oWorkbook = workbook;

        InitializeComponent();

        nudN.Minimum = TopNByMetricUserSettings.MinimumN;
        nudN.Maximum = TopNByMetricUserSettings.MaximumN;

        // This dialog is hard-coded for now to get only the top vertices from
        // the vertex worksheet.  It can be updated later to get the top items
        // from any worksheet later, if necessary.  If that is done,
        // TopNByMetrics.ToString() must also be updated.

        cbxWorksheetName.Items.Add(WorksheetNames.Vertices);
        cbxItemNameColumnName.Items.Add(VertexTableColumnNames.VertexName);

        ListObject oVertexTable;

        if ( ExcelTableUtil.TryGetTable(m_oWorkbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oVertexTable) )
        {
            cbxRankedColumnName.PopulateWithTableColumnNames(oVertexTable);
        }

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oTopNByMetricUserSettingsDialogUserSettings =
            new TopNByMetricUserSettingsDialogUserSettings(this);

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
            String sRankedColumnName;
            Int32 iN;

            if (
                !ValidateRequiredComboBox(cbxRankedColumnName,

                    "Enter or select the column containing the numbers to rank"
                        + " the items by.",

                    out sRankedColumnName)
                ||
                !ValidateNumericUpDown(nudN, "the number of top items to get,",
                    out iN)
                )
            {
                return (false);
            }

            m_oTopNByMetricUserSettings.WorksheetName =
                (String)cbxWorksheetName.SelectedItem;

            // See the notes in the constructor about how only the vertex
            // worksheet is handled now.

            m_oTopNByMetricUserSettings.TableName = TableNames.Vertices;

            m_oTopNByMetricUserSettings.ItemNameColumnName =
                (String)cbxItemNameColumnName.SelectedItem;

            m_oTopNByMetricUserSettings.RankedColumnName = sRankedColumnName;
            m_oTopNByMetricUserSettings.N = iN;
        }
        else
        {
            cbxWorksheetName.SelectedItem =
                m_oTopNByMetricUserSettings.WorksheetName;

            cbxItemNameColumnName.SelectedItem =
                m_oTopNByMetricUserSettings.ItemNameColumnName;

            cbxRankedColumnName.Text =
                m_oTopNByMetricUserSettings.RankedColumnName;

            nudN.Value = m_oTopNByMetricUserSettings.N;
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

        Debug.Assert(m_oTopNByMetricUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oTopNByMetricUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object being edited.

    protected TopNByMetricUserSettings m_oTopNByMetricUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// User settings for this dialog.

    protected TopNByMetricUserSettingsDialogUserSettings
        m_oTopNByMetricUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: TopNByMetricUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="TopNByMetricUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("TopNByMetricUserSettingsDialog") ]

public class TopNByMetricUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: TopNByMetricUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TopNByMetricUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public TopNByMetricUserSettingsDialogUserSettings
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
