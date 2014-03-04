

using System;
using System.Configuration;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;
using Smrf.NodeXL.Core;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ImportFromWorkbookDialog
//
/// <summary>
/// Imports edge and vertex columns from another open workbook to an <see
/// cref="IGraph" /> object.
/// </summary>
///
/// <remarks>
/// If <see cref="Form.ShowDialog()" /> returns <see cref="DialogResult.OK" />,
/// the imported <see cref="IGraph" /> object can be obtained from the <see
/// cref="ImportFromWorkbookDialog.Graph" /> property.
/// </remarks>
//*****************************************************************************

public partial class ImportFromWorkbookDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ImportFromWorkbookDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromWorkbookDialog" /> class.
    /// </summary>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// Workbook into which the edges and vertices will be imported.  This is
    /// used only to avoid including the workbook in the list of workbooks
    /// that can be imported; the workbook itself is not modified by this
    /// class.
    /// </param>
    //*************************************************************************

    public ImportFromWorkbookDialog
    (
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook
    )
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oImportFromWorkbookDialogUserSettings =
            new ImportFromWorkbookDialogUserSettings(this);

        m_oGraph = null;
        m_oDestinationNodeXLWorkbook = destinationNodeXLWorkbook;

        lbxSourceWorkbook.PopulateWithOtherWorkbookNames(
            m_oDestinationNodeXLWorkbook);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: Graph
    //
    /// <summary>
    /// Gets the <see cref="IGraph" /> object imported from the workbook.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="IGraph" /> object imported from the workbook.
    /// </value>
    ///
    /// <remarks>
    /// This should be read only if <see cref="Form.ShowDialog()" /> returns
    /// <see cref="DialogResult.OK" />.
    ///
    /// <para>
    /// The names of all the workbook's edge and vertex columns are stored as
    /// metadata on the returned IGraph object using the <see
    /// cref="ReservedMetadataKeys.AllEdgeMetadataKeys" /> and
    /// <see cref="ReservedMetadataKeys.AllVertexMetadataKeys" /> keys.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public IGraph
    Graph
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            Debug.Assert(m_oGraph != null);
            AssertValid();

            return (m_oGraph);
        }
    }

    //*************************************************************************
    //  Property: SourceWorkbookName
    //
    /// <summary>
    /// Gets the name of the matrix workbook that was imported from.
    /// </summary>
    ///
    /// <value>
    /// The name of the matrix workbook that was imported from.
    /// </value>
    ///
    /// <remarks>
    /// This can be used by the caller after the dialog exits to determine
    /// which matrix workbook was imported from.
    /// </remarks>
    //*************************************************************************

    public String
    SourceWorkbookName
    {
        get
        {
            AssertValid();

            return ( (String)lbxSourceWorkbook.SelectedItem );
        }
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
            // Validate the controls.

            if (cbxVertex1.SelectedIndex >= 0 && cbxVertex2.SelectedIndex >= 0
                && cbxVertex1.SelectedItem == cbxVertex2.SelectedItem)
            {
                OnInvalidComboBox(cbxVertex1,
                    "The same imported column can't be used for both Vertex 1"
                    + " and Vertex 2."
                    );

                return (false);
            }

            m_oImportFromWorkbookDialogUserSettings.SourceColumnsHaveHeaders =
                cbxSourceColumnsHaveHeaders.Checked;
        }
        else
        {
            cbxSourceColumnsHaveHeaders.Checked =
                m_oImportFromWorkbookDialogUserSettings.
                SourceColumnsHaveHeaders;
        }

        EnableControls();

        return (true);
    }

    //*************************************************************************
    //  Method: PopulateSourceColumnsDataGridView()
    //
    /// <summary>
    /// Populates the dgvSourceColumns DataGridView.
    /// </summary>
    //*************************************************************************

    protected void
    PopulateSourceColumnsDataGridView()
    {
        AssertValid();

        DataGridViewRowCollection oDataGridRows = dgvSourceColumns.Rows;
        oDataGridRows.Clear();

        // Attempt to get the non-empty range of the active worksheet of the
        // selected source workbook.

        Range oNonEmptyRange;

        if ( !TryGetSourceWorkbookNonEmptyRange(out oNonEmptyRange) )
        {
            return;
        }

        Boolean bSourceColumnsHaveHeaders =
            cbxSourceColumnsHaveHeaders.Checked;

        // Get the first row and column of the non-empty range.

        Range oFirstRow = oNonEmptyRange.get_Resize(1, Missing.Value);
        Range oColumn = oNonEmptyRange.get_Resize(Missing.Value, 1);

        Object [,] oFirstRowValues = ExcelUtil.GetRangeValues(oFirstRow);
        Int32 iNonEmptyColumns = oNonEmptyRange.Columns.Count;
        Int32 iColumnOneBased = oColumn.Column;

        for (Int32 i = 1; i <= iNonEmptyColumns; i++, iColumnOneBased++)
        {
            String sColumnLetter = ExcelUtil.GetColumnLetter(
                ExcelUtil.GetRangeAddress( (Range)oColumn.Cells[1, 1] ) );

            // Get the value of the column's first cell, if there is one.

            String sFirstCellValue;

            if ( !ExcelUtil.TryGetNonEmptyStringFromCell(oFirstRowValues, 1,
                i, out sFirstCellValue) )
            {
                sFirstCellValue = null;
            }

            String sItemText = GetSourceColumnItemText(sFirstCellValue,
                sColumnLetter, bSourceColumnsHaveHeaders);

            oDataGridRows.Add(new ObjectWithText(iColumnOneBased, sItemText),
                false, false, false);

            // Move to the next column.

            oColumn = oColumn.get_Offset(0, 1);
        }
    }

    //*************************************************************************
    //  Method: PopulateVertexComboBoxes()
    //
    /// <summary>
    /// Populates the vertex 1 and vertex 2 ComboBox with the rows of the
    /// dgvSourceColumns DataGridView that have "Is Edge Column" checked.
    /// </summary>
    //*************************************************************************

    protected void
    PopulateVertexComboBoxes()
    {
        AssertValid();

        PopulateVertexComboBox(cbxVertex1);
        PopulateVertexComboBox(cbxVertex2);
    }

    //*************************************************************************
    //  Method: PopulateVertexComboBox()
    //
    /// <summary>
    /// Populates the vertex 1 or vertex 2 ComboBox with the rows of the
    /// dgvSourceColumns DataGridView that have "Is Edge Column" checked.
    /// </summary>
    ///
    /// <param name="cbxVertex">
    /// The vertex 1 or vertex 2 ComboBox.
    /// </param>
    //*************************************************************************

    protected void
    PopulateVertexComboBox
    (
        ComboBox cbxVertex
    )
    {
        Debug.Assert(cbxVertex != null);
        AssertValid();

        // Save the ObjectWithText that is selected in the vertex ComboBox, if
        // there is one.

        ObjectWithText oOldSelectedObjectWithText =
            (ObjectWithText)cbxVertex.SelectedItem;

        ComboBox.ObjectCollection oItems = cbxVertex.Items;
        oItems.Clear();

        foreach (DataGridViewRow oDataGridViewRow in dgvSourceColumns.Rows)
        {
            if ( (Boolean)oDataGridViewRow.Cells[IsEdgeColumnIndex].Value )
            {
                oItems.Add( (ObjectWithText)oDataGridViewRow.Cells[
                    ColumnNameIndex].Value );
            }
        }

        cbxVertex.SelectedItem = oOldSelectedObjectWithText;
    }

    //*************************************************************************
    //  Method: TryGetSourceWorkbookNonEmptyRange()
    //
    /// <summary>
    /// Attempts to get the non-empty range of the active worksheet of the
    /// selected source workbook.
    /// </summary>
    ///
    /// <param name="oNonEmptyRange">
    /// Where the non-empty range gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetSourceWorkbookNonEmptyRange
    (
        out Range oNonEmptyRange
    )
    {
        AssertValid();

        oNonEmptyRange = null;

        Debug.Assert(lbxSourceWorkbook.Items.Count > 0);

        String sSourceWorkbookName = (String)lbxSourceWorkbook.SelectedItem;

        Object oSourceWorksheetAsObject;

        try
        {
            oSourceWorksheetAsObject =
                m_oDestinationNodeXLWorkbook.Application.Workbooks[
                    sSourceWorkbookName].ActiveSheet;
        }
        catch (COMException)
        {
            // This occurred once.

            oSourceWorksheetAsObject = null;
        }

        if ( oSourceWorksheetAsObject == null ||
            !(oSourceWorksheetAsObject is Worksheet) )
        {
            this.ShowWarning( String.Format(

                WorkbookImporterBase.SourceWorkbookSheetIsNotWorksheetMessage
                ,
                sSourceWorkbookName
                ) );

            return (false);
        }

        Worksheet oSourceWorksheet = (Worksheet)oSourceWorksheetAsObject;

        if ( !ExcelUtil.TryGetNonEmptyRangeInWorksheet(oSourceWorksheet,
            out oNonEmptyRange) )
        {
            this.ShowWarning( String.Format(

                "The selected worksheet in {0} is empty.  It has no columns"
                + " that can be imported."
                ,
                sSourceWorkbookName
                ) );

            return (false);
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

        pnlVertices.Enabled = (cbxVertex1.Items.Count > 0);

        this.btnCheckAllIsEdgeColumnCheckBoxes.Enabled =
            this.btnUncheckAllIsEdgeColumnCheckBoxes.Enabled =
            (dgvSourceColumns.Rows.Count > 0);

        btnOK.Enabled = pnlVertices.Enabled && cbxVertex1.SelectedIndex >= 0
            && cbxVertex2.SelectedIndex >= 0;
    }

    //*************************************************************************
    //  Method: CheckAllIsEdgeColumnCheckBoxes()
    //
    /// <summary>
    /// Checks or unchecks checkboxes in the dgvSourceColumns DataGridView.
    /// </summary>
    ///
    /// <param name="bCheck">
    /// true to check the Is Edge Column checkboxes and uncheck all others,
    /// false to uncheck all checkboxes.
    /// </param>
    //*************************************************************************

    protected void
    CheckAllIsEdgeColumnCheckBoxes
    (
        Boolean bCheck
    )
    {
        AssertValid();

        foreach (DataGridViewRow oDataGridViewRow in dgvSourceColumns.Rows)
        {
            oDataGridViewRow.Cells[IsEdgeColumnIndex].Value = bCheck;

            oDataGridViewRow.Cells[IsVertex1PropertyColumnIndex].Value =
                false;

            oDataGridViewRow.Cells[IsVertex2PropertyColumnIndex].Value =
                false;
        }

        PopulateVertexComboBoxes();
        EnableControls();
    }

    //*************************************************************************
    //  Method: GetOneBasedColumnNumbersToImport()
    //
    /// <summary>
    /// Gets a collection of the one-based column numbers that are checked in
    /// one column of the dgvSourceColumns DataGridView.
    /// </summary>
    ///
    /// <param name="iDataGridViewColumnIndex">
    /// Zero-based index of the column in the DataGridView that contains the
    /// checkboxes to read.
    /// </param>
    //*************************************************************************

    protected ICollection<Int32>
    GetOneBasedColumnNumbersToImport
    (
        Int32 iDataGridViewColumnIndex
    )
    {
        Debug.Assert(iDataGridViewColumnIndex > 0);
        AssertValid();

        LinkedList<Int32> oOneBasedColumnNumbers = new LinkedList<Int32>();

        foreach (DataGridViewRow oDataGridViewRow in dgvSourceColumns.Rows)
        {
            if ( (Boolean)oDataGridViewRow.Cells[
                iDataGridViewColumnIndex].Value )
            {
                oOneBasedColumnNumbers.AddLast(
                    ObjectWithTextToColumnNumberOneBased(
                    (ObjectWithText)oDataGridViewRow.Cells[
                        ColumnNameIndex].Value ) );
            }
        }

        return (oOneBasedColumnNumbers);
    }

    //*************************************************************************
    //  Method: GetSourceColumnItemText()
    //
    /// <summary>
    /// Gets the text to use for a row in the dgvSourceColumns DataGridView.
    /// </summary>
    ///
    /// <param name="sFirstSourceCellValue">
    /// String value from the first cell in the source column, or null if the
    /// first cell doesn't contain a string.
    /// </param>
    ///
    /// <param name="sColumnLetter">
    /// Excel's letter for the source column.
    /// </param>
    ///
    /// <param name="bSourceColumnsHaveHeaders">
    /// true if the source columns have headers.
    /// </param>
    ///
    /// <returns>
    /// The text to use.
    /// </returns>
    //*************************************************************************

    protected String
    GetSourceColumnItemText
    (
        String sFirstSourceCellValue,
        String sColumnLetter,
        Boolean bSourceColumnsHaveHeaders
    )
    {
        AssertValid();

        if (sFirstSourceCellValue == null)
        {
            // Just use the column letter.

            return ("Column " + sColumnLetter);
        }

        // Truncate the first cell if necessary.

        const Int32 MaxItemTextLength = 30;

        sFirstSourceCellValue = StringUtil.TruncateWithEllipses(
            sFirstSourceCellValue, MaxItemTextLength);

        if (bSourceColumnsHaveHeaders)
        {
            // The first cell is a header.

            return ( String.Format(

                "\"{0}\"",

                sFirstSourceCellValue
                ) );
        }

        // The first cell isn't a header.  Precede the cell value with the
        // column letter.

        return ( String.Format(

            "Column {0}: \"{1}\""
            ,
            sColumnLetter,
            sFirstSourceCellValue
            ) );
    }

    //*************************************************************************
    //  Method: ObjectWithTextToColumnNumberOneBased()
    //
    /// <summary>
    /// Retrieves the one-based column number from an ObjectWithText.
    /// </summary>
    ///
    /// <param name="oObjectWithText">
    /// The ObjectWithText that has a one-based column number in its Object
    /// property.
    /// </param>
    //*************************************************************************

    protected Int32
    ObjectWithTextToColumnNumberOneBased
    (
        ObjectWithText oObjectWithText
    )
    {
        Debug.Assert(oObjectWithText != null);
        AssertValid();

        Debug.Assert(oObjectWithText.Object is Int32);

        return ( (Int32)oObjectWithText.Object );
    }

    //*************************************************************************
    //  Method: ImportFromEdgeWorkbook()
    //
    /// <summary>
    /// Imports edges from a source workbook to the edge worksheet of the
    /// NodeXL workbook.
    /// </summary>
    ///
    /// <remarks>
    /// This should be called only after DoDataExchange(true) returns true.
    /// </remarks>
    //*************************************************************************

    protected void
    ImportFromEdgeWorkbook()
    {
        AssertValid();

        String sSourceWorkbookName = this.SourceWorkbookName;

        Debug.Assert(cbxVertex1.SelectedItem != null);

        Int32 iColumnToUseForVertex1OneBased =
            ObjectWithTextToColumnNumberOneBased(
                (ObjectWithText)cbxVertex1.SelectedItem);

        Debug.Assert(cbxVertex2.SelectedItem != null);

        Int32 iColumnToUseForVertex2OneBased =
            ObjectWithTextToColumnNumberOneBased(
                (ObjectWithText)cbxVertex2.SelectedItem);

        // Get the collection of edge columns to import, not including the
        // vertex 1 or vertex 2 columns.

        ICollection<Int32> oOneBasedEdgeColumnNumbersToImport =
            GetOneBasedColumnNumbersToImport(IsEdgeColumnIndex);

        oOneBasedEdgeColumnNumbersToImport.Remove(
            iColumnToUseForVertex1OneBased);

        oOneBasedEdgeColumnNumbersToImport.Remove(
            iColumnToUseForVertex2OneBased);

        m_oGraph = ( new WorkbookImporter() ).ImportWorkbook(
            m_oDestinationNodeXLWorkbook.Application,
            sSourceWorkbookName,
            iColumnToUseForVertex1OneBased, iColumnToUseForVertex2OneBased,
            oOneBasedEdgeColumnNumbersToImport,
            GetOneBasedColumnNumbersToImport(IsVertex1PropertyColumnIndex),
            GetOneBasedColumnNumbersToImport(IsVertex2PropertyColumnIndex),

            m_oImportFromWorkbookDialogUserSettings.
                SourceColumnsHaveHeaders
            );
    }

    //*************************************************************************
    //  Method: OnLoad()
    //
    /// <summary>
    /// Handles the Load event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnLoad
    (
        EventArgs e
    )
    {
        AssertValid();

        base.OnLoad(e);

        // Known DataGridView bug: Setting the AutoSizeMode of a column to
        // Fill in the designer can cause this exception:
        //
        // System.InvalidOperationException: This operation cannot be performed
        // while an auto-filled column is being resized.
        //
        // See the post "AutoResizeColumns gives operation exception" at 
        // http://social.msdn.microsoft.com/forums/en-US/winformsdatacontrols/
        // thread/b66520b7-94e4-4b13-ab82-64f3aeee23e4/.
        //
        // To work around this bug, set the AutoSizeMode property during the
        // Load event.

        this.colColumnName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        switch (lbxSourceWorkbook.Items.Count)
        {
            case 0:

                this.ShowWarning(ExcelWorkbookListBox.NoOtherWorkbooks);
                this.Close();
                break;

            case 1:

                lbxSourceWorkbook.SelectedIndex = 0;
                break;

            default:

                // (Do nothing.)

                break;
        }
    }

    //*************************************************************************
    //  Method: OnSourceWorkbookChanged()
    //
    /// <summary>
    /// Performs tasks necessary when the user selects a source workbook.
    /// </summary>
    //*************************************************************************

    protected void
    OnSourceWorkbookChanged()
    {
        AssertValid();

        PopulateSourceColumnsDataGridView();
        Boolean bAtLeastTwoRows = (dgvSourceColumns.Rows.Count >= 2);

        if (bAtLeastTwoRows)
        {
            for (Int32 iRow = 0; iRow < 2; iRow++)
            {
                dgvSourceColumns.Rows[iRow].Cells[IsEdgeColumnIndex].Value =
                    true;
            }
        }

        PopulateVertexComboBoxes();

        if (bAtLeastTwoRows)
        {
            Debug.Assert(cbxVertex1.Items.Count >= 2);
            Debug.Assert(cbxVertex2.Items.Count >= 2);

            cbxVertex1.SelectedIndex = 0;
            cbxVertex2.SelectedIndex = 1;
        }

        EnableControls();
    }

    //*************************************************************************
    //  Method: lbxSourceWorkbook_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the lbxSourceWorkbook
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
    lbxSourceWorkbook_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnSourceWorkbookChanged();
    }

    //*************************************************************************
    //  Method: cbxSourceColumnsHaveHeaders_CheckedChanged()
    //
    /// <summary>
    /// Handles the CheckedChanged event on the cbxSourceColumnsHaveHeaders
    /// CheckBox.
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
    cbxSourceColumnsHaveHeaders_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (lbxSourceWorkbook.SelectedItem != null)
        {
            OnSourceWorkbookChanged();
        }
    }

    //*************************************************************************
    //  Method: dgvSourceColumns_CellContentClick()
    //
    /// <summary>
    /// Handles the CellContentClick event on the dgvSourceColumns
    /// DataGridView.
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
    dgvSourceColumns_CellContentClick
    (
        object sender,
        DataGridViewCellEventArgs e
    )
    {
        AssertValid();

        dgvSourceColumns.EndEdit();
        DataGridViewRow oDataGridViewRow = dgvSourceColumns.Rows[e.RowIndex];

        if (
            e.ColumnIndex > ColumnNameIndex
            &&
            (Boolean)oDataGridViewRow.Cells[e.ColumnIndex].Value
            )
        {
            // A checkbox was checked.  Uncheck the row's other checkboxes.

            foreach (Int32 iColumnIndex in new Int32 [] {
                IsEdgeColumnIndex,
                IsVertex1PropertyColumnIndex,
                IsVertex2PropertyColumnIndex
                } )
            {
                if (iColumnIndex != e.ColumnIndex)
                {
                    oDataGridViewRow.Cells[iColumnIndex].Value = false;
                }
            }
        }

        PopulateVertexComboBoxes();
        EnableControls();
    }

    //*************************************************************************
    //  Method: btnCheckAllIsEdgeColumnCheckBoxes_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCheckAllIsEdgeColumnCheckBoxes button.
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
    btnCheckAllIsEdgeColumnCheckBoxes_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        CheckAllIsEdgeColumnCheckBoxes(true);
    }

    //*************************************************************************
    //  Method: btnUncheckAllIsEdgeColumnCheckBoxes_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnUncheckAllIsEdgeColumnCheckBoxes
    /// button.
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
    btnUncheckAllIsEdgeColumnCheckBoxes_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        CheckAllIsEdgeColumnCheckBoxes(false);
    }

    //*************************************************************************
    //  Method: cbxVertex_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the cbxVertex1 and cbxVertex2
    /// ComboBox.
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
    cbxVertex_SelectedIndexChanged
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
        if ( !DoDataExchange(true) )
        {
            return;
        }

        try
        {
            ImportFromEdgeWorkbook();
        }
        catch (ImportWorkbookException oImportWorkbookException)
        {
            this.ShowWarning(oImportWorkbookException.Message);
            return;
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }

        DialogResult = DialogResult.OK;
        this.Close();
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

        Debug.Assert(m_oImportFromWorkbookDialogUserSettings != null);
        // m_oGraph
        Debug.Assert(m_oDestinationNodeXLWorkbook != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// <summary>
    /// Index of the column name column in the dgvSourceColumn DataGridView.
    /// ColumnType: DataGridViewTextBoxColumn.
    /// </summary>

    protected const Int32 ColumnNameIndex = 0;

    /// <summary>
    /// Index of the "is edge column" column in the dgvSourceColumn
    /// DataGridView.  ColumnType: DataGridViewCheckBoxColumn.
    /// </summary>

    protected const Int32 IsEdgeColumnIndex = 1;

    /// <summary>
    /// Index of the "is vertex 1 proprty column" column in the dgvSourceColumn
    /// DataGridView.  ColumnType: DataGridViewCheckBoxColumn.
    /// </summary>

    protected const Int32 IsVertex1PropertyColumnIndex = 2;

    /// <summary>
    /// Index of the "is vertex 2 proprty column" column in the dgvSourceColumn
    /// DataGridView.  ColumnType: DataGridViewCheckBoxColumn.
    /// </summary>

    protected const Int32 IsVertex2PropertyColumnIndex = 3;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected ImportFromWorkbookDialogUserSettings
        m_oImportFromWorkbookDialogUserSettings;

    /// The imported IGraph object, or null.

    protected IGraph m_oGraph;

    /// Workbook to which the edge workbook will be imported.

    protected Microsoft.Office.Interop.Excel.Workbook
        m_oDestinationNodeXLWorkbook;
}


//*****************************************************************************
//  Class: ImportFromWorkbookDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="ImportFromWorkbookDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ImportFromWorkbookDialog") ]

public class ImportFromWorkbookDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ImportFromWorkbookDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromWorkbookDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ImportFromWorkbookDialogUserSettings
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
    //  Property: SourceColumnsHaveHeaders
    //
    /// <summary>
    /// Gets or sets a flag that indicating whether the source columns have
    /// headers.
    /// 
    /// </summary>
    ///
    /// <value>
    /// true if the source columns have headers.  The default is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    SourceColumnsHaveHeaders
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SourceColumnsHaveHeadersKey] );
        }

        set
        {
            this[SourceColumnsHaveHeadersKey] = value;

            AssertValid();
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the SourceColumnsHaveHeaders property.

    protected const String SourceColumnsHaveHeadersKey =
        "SourceColumnsHaveHeaders";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
