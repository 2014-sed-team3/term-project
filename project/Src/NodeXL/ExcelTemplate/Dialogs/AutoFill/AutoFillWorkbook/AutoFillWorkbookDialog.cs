

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillWorkbookDialog
//
/// <summary>
/// Edits an <see cref="AutoFillUserSettings" /> object and autofills the
/// workbook using those user settings.
/// </summary>
///
/// <remarks>
/// This dialog can run in either of two modes.  If the <see
/// cref="DialogMode" /> argument passed to the constructor is <see
/// cref="DialogMode.Normal" />, it should be opened with <see
/// cref="Form.Show(IWin32Window)" /> to make it modeless.  If the argument is
/// <see cref="DialogMode.EditOnly" />, it should be opened with <see
/// cref="Form.ShowDialog()" /> to make it modal.
///
/// <para>
/// The AutoFill feature automatically fills edge and vertex attribute columns
/// using values from user-specified source columns.  This dialog lets the user
/// specify AutoFill settings, then uses a <see cref="WorkbookAutoFiller" /> to
/// do most of the work.  A <see cref="WorkbookAutoFilled" /> event is fired
/// when the workbook is autofilled.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class AutoFillWorkbookDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AutoFillWorkbookDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoFillWorkbookDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the dialog is being used.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public AutoFillWorkbookDialog
    (
        DialogMode mode,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(workbook != null);

        InitializeComponent();

        m_eMode = mode;
        m_oWorkbook = workbook;

        m_oAutoFillUserSettings = new AutoFillUserSettings();

        if (m_eMode == DialogMode.EditOnly)
        {
            this.Text += " Options";
            btnAutoFill.Text = "OK";
            btnClose.Text = "Cancel";

            // The column header text "When Autofill is clicked..." makes no
            // sense when the "Autofill" button text has been changed to "OK".

            lblDestinationColumnHeader1.Text =
                lblDestinationColumnHeader2.Text =
                lblDestinationColumnHeader3.Text =
                lblDestinationColumnHeader1.Text.Replace("clicked", "run");
        }

        // Instantiate an object that retrieves and saves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oAutoFillWorkbookDialogUserSettings =
            new AutoFillWorkbookDialogUserSettings(this);

        // Initialize the ComboBoxes used to specify the data sources for the
        // table columns.

        InitializeVertexComboBoxes(m_oWorkbook);
        InitializeEdgeComboBoxes(m_oWorkbook);
        InitializeGroupComboBoxes(m_oWorkbook);

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
        /// The user can edit the autofill settings and then autofill the
        /// workbook.  The dialog should be opened as modeless.

        Normal,

        /// The user can edit the autofill settings but cannot autofill the
        /// workbook.  The dialog should be opened as modal.

        EditOnly,
    }

    //*************************************************************************
    //  Event: WorkbookAutoFilled
    //
    /// <summary>
    /// Occurs when the workbook is autofilled.
    /// </summary>
    //*************************************************************************

    public event EventHandler WorkbookAutoFilled;


    //*************************************************************************
    //  Method: InitializeEdgeComboBoxes()
    //
    /// <summary>
    /// Initializes the ComboBoxes used to specify the data sources for the
    /// edge table columns.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    protected void
    InitializeEdgeComboBoxes
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        m_aoEdgeSourceColumnNameComboBoxes =
            new EdgeColumnComboBox[]
            {
                cbxEdgeColorSourceColumnName,
                cbxEdgeWidthSourceColumnName,
                cbxEdgeStyleSourceColumnName,
                cbxEdgeAlphaSourceColumnName,
                cbxEdgeVisibilitySourceColumnName,
                cbxEdgeLabelSourceColumnName,
            };

        ListObject oEdgeTable;

        if ( ExcelTableUtil.TryGetTable(oWorkbook, WorksheetNames.Edges,
            TableNames.Edges, out oEdgeTable) )
        {
            // Populate the edge table column ComboBoxes with the source column
            // names.

            foreach (EdgeColumnComboBox oComboBox in
                m_aoEdgeSourceColumnNameComboBoxes)
            {
                oComboBox.PopulateWithTableColumnNames(oEdgeTable);
            }
        }

        // Store the name of the column corresponding to the ComboBox in each
        // ComboBox's Tag.  This gets used for error checking by
        // DoDataExchange().

        cbxEdgeColorSourceColumnName.Tag = EdgeTableColumnNames.Color;
        cbxEdgeWidthSourceColumnName.Tag = EdgeTableColumnNames.Width;
        cbxEdgeStyleSourceColumnName.Tag = EdgeTableColumnNames.Style;
        cbxEdgeAlphaSourceColumnName.Tag = CommonTableColumnNames.Alpha;

        cbxEdgeVisibilitySourceColumnName.Tag =
            CommonTableColumnNames.Visibility;

        cbxEdgeLabelSourceColumnName.Tag = EdgeTableColumnNames.Label;
    }

    //*************************************************************************
    //  Method: InitializeVertexComboBoxes()
    //
    /// <summary>
    /// Initializes the ComboBoxes used to specify the data sources for the
    /// vertex table columns.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    protected void
    InitializeVertexComboBoxes
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        m_aoVertexSourceColumnNameComboBoxes =
            new VertexColumnComboBox[]
            {
                cbxVertexColorSourceColumnName,
                cbxVertexShapeSourceColumnName,
                cbxVertexRadiusSourceColumnName,
                cbxVertexAlphaSourceColumnName,
                cbxVertexVisibilitySourceColumnName,
                cbxVertexLabelSourceColumnName,
                cbxVertexLabelFillColorSourceColumnName,
                cbxVertexLabelPositionSourceColumnName,
                cbxVertexToolTipSourceColumnName,
                cbxVertexLayoutOrderSourceColumnName,
                cbxVertexXSourceColumnName,
                cbxVertexYSourceColumnName,
                cbxVertexPolarRSourceColumnName,
                cbxVertexPolarAngleSourceColumnName,
            };

        ListObject oVertexTable;

        if ( ExcelTableUtil.TryGetTable(oWorkbook, WorksheetNames.Vertices,
            TableNames.Vertices, out oVertexTable) )
        {
            // Populate the vertex table column ComboBoxes with the source
            // column names.

            foreach (VertexColumnComboBox oComboBox in
                m_aoVertexSourceColumnNameComboBoxes)
            {
                oComboBox.PopulateWithTableColumnNames(oVertexTable);
            }

            // Insert a few special items.

            foreach (ComboBox oComboBox in new ComboBox [] {
                cbxVertexLabelSourceColumnName,
                cbxVertexToolTipSourceColumnName
                } )
            {
                oComboBox.Items.Insert(0, VertexTableColumnNames.VertexName);
            }
        }

        // Store the name of the column corresponding to the ComboBox in each
        // ComboBox's Tag.  This gets used for error checking by
        // DoDataExchange().

        cbxVertexColorSourceColumnName.Tag = VertexTableColumnNames.Color;
        cbxVertexShapeSourceColumnName.Tag = VertexTableColumnNames.Shape;
        cbxVertexRadiusSourceColumnName.Tag = VertexTableColumnNames.Radius;
        cbxVertexAlphaSourceColumnName.Tag = CommonTableColumnNames.Alpha;

        cbxVertexVisibilitySourceColumnName.Tag =
            CommonTableColumnNames.Visibility;

        cbxVertexLabelSourceColumnName.Tag = VertexTableColumnNames.Label;

        cbxVertexLabelFillColorSourceColumnName.Tag =
            VertexTableColumnNames.LabelFillColor;

        cbxVertexLabelPositionSourceColumnName.Tag =
            VertexTableColumnNames.LabelPosition;

        cbxVertexToolTipSourceColumnName.Tag = VertexTableColumnNames.ToolTip;

        cbxVertexLayoutOrderSourceColumnName.Tag =
            VertexTableColumnNames.LayoutOrder;

        cbxVertexXSourceColumnName.Tag = VertexTableColumnNames.X;
        cbxVertexYSourceColumnName.Tag = VertexTableColumnNames.Y;
        cbxVertexPolarRSourceColumnName.Tag = VertexTableColumnNames.PolarR;

        cbxVertexPolarAngleSourceColumnName.Tag =
            VertexTableColumnNames.PolarAngle;
    }

    //*************************************************************************
    //  Method: InitializeGroupComboBoxes()
    //
    /// <summary>
    /// Initializes the ComboBoxes used to specify the data sources for the
    /// group table columns.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    protected void
    InitializeGroupComboBoxes
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        m_aoGroupSourceColumnNameComboBoxes =
            new GroupColumnComboBox[]
            {
                cbxGroupCollapsedSourceColumnName,
                cbxGroupLabelSourceColumnName,
            };

        ListObject oGroupTable;

        if ( ExcelTableUtil.TryGetTable(oWorkbook, WorksheetNames.Groups,
            TableNames.Groups, out oGroupTable) )
        {
            // Populate the group table column ComboBoxes with the source
            // column names.

            foreach (GroupColumnComboBox oComboBox in
                m_aoGroupSourceColumnNameComboBoxes)
            {
                oComboBox.PopulateWithTableColumnNames(oGroupTable);
            }
        }

        // Store the name of the column corresponding to the ComboBox in each
        // ComboBox's Tag.  This gets used for error checking by
        // DoDataExchange().

        cbxGroupCollapsedSourceColumnName.Tag =
            GroupTableColumnNames.Collapsed;

        cbxGroupLabelSourceColumnName.Tag =
            GroupTableColumnNames.Label;
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
            if (
                !ValidateSourceColumnNameComboBoxes(
                    m_aoEdgeSourceColumnNameComboBoxes)
                ||
                !ValidateSourceColumnNameComboBoxes(
                    m_aoVertexSourceColumnNameComboBoxes)
                ||
                !ValidateSourceColumnNameComboBoxes(
                    m_aoGroupSourceColumnNameComboBoxes)
                )
            {
                return (false);
            }

            String sVertexXSourceColumnName = GetSourceColumnNameFromComboBox(
                cbxVertexXSourceColumnName);

            String sVertexYSourceColumnName = GetSourceColumnNameFromComboBox(
                cbxVertexYSourceColumnName);

            if (
                (sVertexXSourceColumnName.Length == 0 &&
                    sVertexYSourceColumnName.Length > 0)
                ||
                (sVertexYSourceColumnName.Length == 0 &&
                    sVertexXSourceColumnName.Length > 0)
                )
            {
                return ( this.OnInvalidComboBox(cbxVertexXSourceColumnName,
                    "If you autofill one of the Vertex X or Vertex Y columns,"
                    + " you must autofill both of them."
                    ) );
            }

            m_oAutoFillWorkbookDialogUserSettings.TabControlSelectedIndex =
                tcTabControl.SelectedIndex;

            m_oAutoFillUserSettings.EdgeColorSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxEdgeColorSourceColumnName);

            m_oAutoFillUserSettings.EdgeWidthSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxEdgeWidthSourceColumnName);

            m_oAutoFillUserSettings.EdgeStyleSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxEdgeStyleSourceColumnName);

            m_oAutoFillUserSettings.EdgeAlphaSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxEdgeAlphaSourceColumnName);

            m_oAutoFillUserSettings.EdgeVisibilitySourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxEdgeVisibilitySourceColumnName);

            m_oAutoFillUserSettings.EdgeLabelSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxEdgeLabelSourceColumnName);

            m_oAutoFillUserSettings.VertexColorSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexColorSourceColumnName);

            m_oAutoFillUserSettings.VertexShapeSourceColumnName =
                GetSourceColumnNameFromComboBox(cbxVertexShapeSourceColumnName);

            m_oAutoFillUserSettings.VertexRadiusSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexRadiusSourceColumnName);

            m_oAutoFillUserSettings.VertexAlphaSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexAlphaSourceColumnName);

            m_oAutoFillUserSettings.VertexLabelSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexLabelSourceColumnName);

            m_oAutoFillUserSettings.VertexLabelFillColorSourceColumnName
                = GetSourceColumnNameFromComboBox(
                    cbxVertexLabelFillColorSourceColumnName);

            m_oAutoFillUserSettings.VertexLabelPositionSourceColumnName
                = GetSourceColumnNameFromComboBox(
                    cbxVertexLabelPositionSourceColumnName);

            m_oAutoFillUserSettings.VertexToolTipSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexToolTipSourceColumnName);

            m_oAutoFillUserSettings.VertexVisibilitySourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexVisibilitySourceColumnName);

            m_oAutoFillUserSettings.VertexLayoutOrderSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexLayoutOrderSourceColumnName);

            m_oAutoFillUserSettings.VertexXSourceColumnName =
                sVertexXSourceColumnName;

            m_oAutoFillUserSettings.VertexYSourceColumnName =
                sVertexYSourceColumnName;

            m_oAutoFillUserSettings.VertexPolarRSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexPolarRSourceColumnName);

            m_oAutoFillUserSettings.VertexPolarAngleSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxVertexPolarAngleSourceColumnName);

            m_oAutoFillUserSettings.GroupCollapsedSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxGroupCollapsedSourceColumnName);

            m_oAutoFillUserSettings.GroupLabelSourceColumnName =
                GetSourceColumnNameFromComboBox(
                    cbxGroupLabelSourceColumnName);
        }
        else
        {
            tcTabControl.SelectedIndex =
                m_oAutoFillWorkbookDialogUserSettings.TabControlSelectedIndex;

            cbxEdgeColorSourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeColorSourceColumnName;

            cbxEdgeWidthSourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeWidthSourceColumnName;

            cbxEdgeStyleSourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeStyleSourceColumnName;

            cbxEdgeAlphaSourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeAlphaSourceColumnName;

            cbxEdgeVisibilitySourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeVisibilitySourceColumnName;

            cbxEdgeLabelSourceColumnName.Text =
                m_oAutoFillUserSettings.EdgeLabelSourceColumnName;

            cbxVertexColorSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexColorSourceColumnName;

            cbxVertexShapeSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexShapeSourceColumnName;

            cbxVertexRadiusSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexRadiusSourceColumnName;

            cbxVertexAlphaSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexAlphaSourceColumnName;

            cbxVertexLabelSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexLabelSourceColumnName;

            cbxVertexLabelFillColorSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexLabelFillColorSourceColumnName;

            cbxVertexLabelPositionSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexLabelPositionSourceColumnName;

            cbxVertexToolTipSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexToolTipSourceColumnName;

            cbxVertexVisibilitySourceColumnName.Text =
                m_oAutoFillUserSettings.VertexVisibilitySourceColumnName;

            cbxVertexLayoutOrderSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexLayoutOrderSourceColumnName;

            cbxVertexXSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexXSourceColumnName;

            cbxVertexYSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexYSourceColumnName;

            cbxVertexPolarRSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexPolarRSourceColumnName;

            cbxVertexPolarAngleSourceColumnName.Text =
                m_oAutoFillUserSettings.VertexPolarAngleSourceColumnName;

            cbxGroupCollapsedSourceColumnName.Text =
                m_oAutoFillUserSettings.GroupCollapsedSourceColumnName;

            cbxGroupLabelSourceColumnName.Text =
                m_oAutoFillUserSettings.GroupLabelSourceColumnName;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ValidateSourceColumnNameComboBoxes()
    //
    /// <summary>
    /// Validates the text in an array of ComboBoxes that may contain source
    /// column names.
    /// </summary>
    ///
    /// <param name="aoSourceColumnNameComboBoxes">
    /// Array of ComboBoxes that may contain source column names.
    /// </param>
    ///
    /// <returns>
    /// true if the ComboBoxes all contain valid text.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateSourceColumnNameComboBoxes
    (
        ComboBox [] aoSourceColumnNameComboBoxes
    )
    {
        Debug.Assert(aoSourceColumnNameComboBoxes != null);

        foreach (ComboBox oComboBox in aoSourceColumnNameComboBoxes)
        {
            // Prevent the user from trying to autofill a column with itself.

            if ( GetSourceColumnNameFromComboBox(oComboBox).ToLower() ==
                GetDestinationColumnNameFromComboBox(oComboBox).ToLower() )
            {
                this.OnInvalidComboBox(oComboBox,
                    "You can't autofill a column with itself."
                    );

                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetSourceColumnNameFromComboBox()
    //
    /// <summary>
    /// Gets a source column name from a ComboBox.
    /// </summary>
    ///
    /// <param name="oComboBox">
    /// ComboBox to get a source column name from.
    /// </param>
    ///
    /// <returns>
    /// The text in <paramref name="oComboBox" />, or String.Empty if the
    /// ComboBox contains nothing but spaces.
    /// </returns>
    //*************************************************************************

    protected String
    GetSourceColumnNameFromComboBox
    (
        ComboBox oComboBox
    )
    {
        Debug.Assert(oComboBox != null);
        AssertValid();

        String sSourceColumnName = oComboBox.Text;

        if (sSourceColumnName.Trim().Length == 0)
        {
            sSourceColumnName = String.Empty;
        }

        return (sSourceColumnName);
    }

    //*************************************************************************
    //  Method: GetDestinationColumnNameFromComboBox()
    //
    /// <summary>
    /// Gets a destination column name from a ComboBox.
    /// </summary>
    ///
    /// <param name="oComboBox">
    /// ComboBox to get a destination column name from.
    /// </param>
    ///
    /// <returns>
    /// The destination column corresponding to the ComboBox.
    /// </returns>
    //*************************************************************************

    protected String
    GetDestinationColumnNameFromComboBox
    (
        ComboBox oComboBox
    )
    {
        Debug.Assert(oComboBox != null);
        AssertValid();

        // The destination column name was stored in the ComboBox's Tag.

        Debug.Assert(oComboBox.Tag is String);

        return ( (String)oComboBox.Tag );
    }

    //*************************************************************************
    //  Method: ClearColumn()
    //
    /// <summary>
    /// Clears a worksheet column and its associated source column name
    /// ComboBox.
    /// </summary>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet containing the column.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table containing the column.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column.
    /// </param>
    ///
    /// <param name="oSourceColumnNameComboBox">
    /// ComboBox containing the source column name.
    /// </param>
    //*************************************************************************

    protected void
    ClearColumn
    (
        String sWorksheetName,
        String sTableName,
        String sColumnName,
        ComboBox oSourceColumnNameComboBox
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) ); 
        Debug.Assert( !String.IsNullOrEmpty(sTableName) ); 
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) ); 
        Debug.Assert(oSourceColumnNameComboBox != null);
        AssertValid();

        ListObject oTable;

        if (
            ExcelTableUtil.TryGetTable(m_oWorkbook, sWorksheetName, sTableName,
                out oTable)
            &&
            ExcelTableUtil.TryClearTableColumnDataContents(oTable, sColumnName)
            )
        {
            oSourceColumnNameComboBox.Text = String.Empty;
        }
    }

    //*************************************************************************
    //  Method: AskWarningQuestion()
    //
    /// <summary>
    /// Asks the user a question, as a warning.
    /// </summary>
    ///
    /// <param name="sWarningQuestion">
    /// The question to ask.
    /// </param>
    ///
    /// <returns>
    /// true if the user answered Yes, false if No.
    /// </returns>
    //*************************************************************************

    protected Boolean
    AskWarningQuestion
    (
        String sWarningQuestion
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWarningQuestion) );
        AssertValid();

        return (MessageBox.Show(this, sWarningQuestion,
            ApplicationUtil.ApplicationName, MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            == DialogResult.Yes);
    }

    //*************************************************************************
    //  Method: OnDetailsClick()
    //
    /// <summary>
    /// Handles the Click event on all detail buttons.
    /// </summary>
    ///
    /// <param name="oDetailsButton">
    /// The details button that was clicked.
    /// </param>
    ///
    /// <param name="oSourceColumnNameComboBox">
    /// ComboBox containing the source column name associated with the details
    /// button.
    /// </param>
    ///
    /// <param name="sColumnDescription">
    /// Column description to use in menu text.
    /// </param>
    ///
    /// <param name="sWorksheetName">
    /// Name of the worksheet associated with the details button.
    /// </param>
    ///
    /// <param name="sTableName">
    /// Name of the table associated with the details button.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the worksheet column associated with the details button.
    /// </param>
    ///
    /// <param name="oUserSettingsDialog">
    /// Dialog for editing the details, or null if there are no details to
    /// edit.
    /// </param>
    //*************************************************************************

    protected void
    OnDetailsClick
    (
        System.Windows.Forms.Button oDetailsButton,
        ComboBox oSourceColumnNameComboBox,
        String sColumnDescription,
        String sWorksheetName,
        String sTableName,
        String sColumnName,
        Form oUserSettingsDialog
    )
    {
        Debug.Assert(oDetailsButton != null);
        Debug.Assert(oSourceColumnNameComboBox != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnDescription) ); 
        Debug.Assert( !String.IsNullOrEmpty(sWorksheetName) ); 
        Debug.Assert( !String.IsNullOrEmpty(sTableName) ); 
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) ); 
        AssertValid();

        // Note that information needed by the individual menu item handlers
        // gets stored in the menu item Tags.


        // Sample: "Edge Color Options..."

        this.tsmWorksheetColumnDetails.Text =
            sColumnDescription + " &Options...";

        this.tsmWorksheetColumnDetails.Tag = oUserSettingsDialog;

        this.tsmWorksheetColumnDetails.Visible =
            (oUserSettingsDialog != null);


        // Sample: "Clear Edge Color Worksheet Column Now"

        this.tsmClearColumn.Text =
            "Clear " + sColumnDescription + " &Worksheet Column Now";

        // Is it worth creating a typesafe structure to store the multiple
        // pieces of information needed by the handler for this menu item?
        // Given that the information is retrieved at just one point in this
        // class, probably not.

        this.tsmClearColumn.Tag = new Object [] {
            sWorksheetName, sTableName, sColumnName, oSourceColumnNameComboBox
            };


        this.cmsWorksheetColumn.Show(oDetailsButton, oDetailsButton.Width, 0);
    }

    //*************************************************************************
    //  Method: AutoFillWorkbookDialog_FormClosing()
    //
    /// <summary>
    /// Handles the FormClosing event.
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
    AutoFillWorkbookDialog_FormClosing
    (
        object sender,
        FormClosingEventArgs e
    )
    {
        if (m_eMode == DialogMode.EditOnly &&
            this.DialogResult == DialogResult.Cancel)
        {
            // The user clicked Cancel while editing the user settings.  Don't
            // save the edited settings.

            return;
        }

        if ( DoDataExchange(true) )
        {
            m_oAutoFillUserSettings.Save();
        }
        else
        {
            e.Cancel = true;
        }
    }

    //*************************************************************************
    //  Method: btnEdgeColorDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeColorDetails button.
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
    btnEdgeColorDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnEdgeColorDetails,
            this.cbxEdgeColorSourceColumnName, "Edge Color",
            WorksheetNames.Edges, TableNames.Edges, EdgeTableColumnNames.Color,

            new ColorColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeColorDetails,
                "Edge Color Options")
            );
    }

    //*************************************************************************
    //  Method: btnEdgeWidthDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeWidthDetails button.
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
    btnEdgeWidthDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnEdgeWidthDetails,
            this.cbxEdgeWidthSourceColumnName, "Edge Width",
            WorksheetNames.Edges, TableNames.Edges, EdgeTableColumnNames.Width,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeWidthDetails,
                "Edge Width Options", "edge width", "Widths",
                EdgeWidthConverter.MinimumWidthWorkbook,
                EdgeWidthConverter.MaximumWidthWorkbook
                )
            );
    }

    //*************************************************************************
    //  Method: btnEdgeStyleDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeStyleDetails button.
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
    btnEdgeStyleDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        const String ColumnDescription = "Edge Style";

        OnDetailsClick(this.btnEdgeStyleDetails,
            this.cbxEdgeStyleSourceColumnName, ColumnDescription,
            WorksheetNames.Edges, TableNames.Edges,
            EdgeTableColumnNames.Style,

            new NumericComparisonColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeStyleDetails,

                (ComboBoxPlus comboBoxPlus) =>
                    ( new EdgeStyleConverter() ).PopulateComboBox(
                        comboBoxPlus, false),

                ColumnDescription
                )
            );
    }

    //*************************************************************************
    //  Method: btnEdgeAlphaDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeAlphaDetails button.
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
    btnEdgeAlphaDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnEdgeAlphaDetails,
            this.cbxEdgeAlphaSourceColumnName, "Edge Opacity",
            WorksheetNames.Edges, TableNames.Edges,
            CommonTableColumnNames.Alpha,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeAlphaDetails,
                "Edge Opacity Options", "edge opacity", "Opacities",
                AlphaConverter.MinimumAlphaWorkbook,
                AlphaConverter.MaximumAlphaWorkbook
                )
            );
    }

    //*************************************************************************
    //  Method: btnEdgeVisibilityDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeVisibilityDetails button.
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
    btnEdgeVisibilityDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        const String ColumnDescription = "Edge Visibility";

        OnDetailsClick(this.btnEdgeVisibilityDetails,
            this.cbxEdgeVisibilitySourceColumnName, ColumnDescription,
            WorksheetNames.Edges, TableNames.Edges,
            CommonTableColumnNames.Visibility,

            new NumericComparisonColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.EdgeVisibilityDetails,

                (ComboBoxPlus comboBoxPlus) =>
                    ( new EdgeVisibilityConverter() ).PopulateComboBox(
                        comboBoxPlus, false),

                ColumnDescription
                )
            );
    }

    //*************************************************************************
    //  Method: btnEdgeLabelDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEdgeLabelDetails button.
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
    btnEdgeLabelDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnEdgeLabelDetails,
            this.cbxEdgeLabelSourceColumnName, "Edge Label",
            WorksheetNames.Edges, TableNames.Edges,
            EdgeTableColumnNames.Label, null);
    }

    //*************************************************************************
    //  Method: btnVertexColorDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexColorDetails button.
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
    btnVertexColorDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexColorDetails,
            this.cbxVertexColorSourceColumnName, "Vertex Color",
            WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.Color,

            new ColorColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexColorDetails,
                "Vertex Color Options")
            );
    }

    //*************************************************************************
    //  Method: btnVertexShapeDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexShapeDetails button.
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
    btnVertexShapeDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        const String ColumnDescription = "Vertex Shape";

        OnDetailsClick(this.btnVertexShapeDetails,
            this.cbxVertexShapeSourceColumnName, ColumnDescription,
            WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.Shape,

            new NumericComparisonColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexShapeDetails,

                (ComboBoxPlus comboBoxPlus) =>
                    ( new VertexShapeConverter() ).PopulateComboBox(
                        comboBoxPlus, false),

                ColumnDescription
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexRadiusDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexRadiusDetails button.
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
    btnVertexRadiusDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexRadiusDetails,
            this.cbxVertexRadiusSourceColumnName, "Vertex Size",
            WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.Radius,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexRadiusDetails,
                "Vertex Size Options", "vertex size", "Sizes",
                VertexRadiusConverter.MinimumRadiusWorkbook,
                VertexRadiusConverter.MaximumRadiusWorkbook
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexAlphaDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexAlphaDetails button.
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
    btnVertexAlphaDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexAlphaDetails,
            this.cbxVertexAlphaSourceColumnName, "Vertex Opacity",
            WorksheetNames.Vertices, TableNames.Vertices,
            CommonTableColumnNames.Alpha,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexAlphaDetails,
                "Vertex Opacity Options", "vertex opacity", "Opacities",
                AlphaConverter.MinimumAlphaWorkbook,
                AlphaConverter.MaximumAlphaWorkbook
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexVisibilityDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexVisibilityDetails button.
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
    btnVertexVisibilityDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        const String ColumnDescription = "Vertex Visibility";

        OnDetailsClick(this.btnVertexVisibilityDetails,
            this.cbxVertexVisibilitySourceColumnName, ColumnDescription,
            WorksheetNames.Vertices, TableNames.Vertices,
            CommonTableColumnNames.Visibility,

            new NumericComparisonColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexVisibilityDetails,

                (ComboBoxPlus comboBoxPlus) =>
                    ( new VertexVisibilityConverter() ).PopulateComboBox(
                        comboBoxPlus, false),

                ColumnDescription
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexLabelDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexLabelDetails button.
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
    btnVertexLabelDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexLabelDetails,
            this.cbxVertexLabelSourceColumnName, "Vertex Label",
            WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.Label, null);
    }

    //*************************************************************************
    //  Method: btnVertexLabelFillColorDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexLabelFillColorDetails button.
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
    btnVertexLabelFillColorDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexLabelFillColorDetails,
            this.cbxVertexLabelFillColorSourceColumnName,
            "Vertex Label Fill Color", WorksheetNames.Vertices,
            TableNames.Vertices, VertexTableColumnNames.LabelFillColor,

            new ColorColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexLabelFillColorDetails,
                "Vertex Label Fill Color Options"
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexLabelPositionDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexLabelPositionDetails button.
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
    btnVertexLabelPositionDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        const String ColumnDescription = "Vertex Label Position";

        OnDetailsClick(this.btnVertexLabelPositionDetails,
            this.cbxVertexLabelPositionSourceColumnName, ColumnDescription,
            WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.LabelPosition,

            new NumericComparisonColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexLabelPositionDetails,

                (ComboBoxPlus comboBoxPlus) =>
                    ( new VertexLabelPositionConverter() ).PopulateComboBox(
                        comboBoxPlus, false),

                ColumnDescription
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexToolTipDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexToolTipDetails button.
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
    btnVertexToolTipDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexToolTipDetails,
            this.cbxVertexToolTipSourceColumnName, "Vertex Tooltip",
            WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.ToolTip, null);
    }

    //*************************************************************************
    //  Method: btnVertexLayoutOrderDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexLayoutOrderDetails button.
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
    btnVertexLayoutOrderDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexLayoutOrderDetails,
            this.cbxVertexLayoutOrderSourceColumnName, "Vertex Layout Order",
            WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.LayoutOrder,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexLayoutOrderDetails,
                "Vertex Layout Order Options", "vertex layout order", "Orders",
                1, 99999
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexXDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexXDetails button.
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
    btnVertexXDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexXDetails, this.cbxVertexXSourceColumnName,
            "Vertex X", WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.X,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexXDetails,
                "Vertex X Options", "vertex x-coordinate",
                CoordinateColumnNamePlural,
                VertexLocationConverter.MinimumXYWorkbook,
                VertexLocationConverter.MaximumXYWorkbook
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexYDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexYDetails button.
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
    btnVertexYDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexYDetails, this.cbxVertexYSourceColumnName,
            "Vertex Y", WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.Y,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexYDetails,
                "Vertex Y Options", "vertex y-coordinate",
                CoordinateColumnNamePlural,
                VertexLocationConverter.MinimumXYWorkbook,
                VertexLocationConverter.MaximumXYWorkbook
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexPolarRDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexPolarRDetails button.
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
    btnVertexPolarRDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexPolarRDetails,
            this.cbxVertexPolarRSourceColumnName,
            "Vertex Polar R", WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.PolarR,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexPolarRDetails,
                "Vertex Polar R Options", "vertex polar R coordinate",
                CoordinateColumnNamePlural,
                0, 1
                )
            );
    }

    //*************************************************************************
    //  Method: btnVertexPolarAngleDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnVertexPolarAngleDetails button.
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
    btnVertexPolarAngleDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnVertexPolarAngleDetails,
            this.cbxVertexPolarAngleSourceColumnName,
            "Vertex Polar Angle", WorksheetNames.Vertices, TableNames.Vertices,
            VertexTableColumnNames.PolarAngle,

            new NumericRangeColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.VertexPolarAngleDetails,
                "Vertex Polar Angle Options", "vertex polar angle coordinate",
                CoordinateColumnNamePlural,
                -99999,
                99999
                )
            );
    }

    //*************************************************************************
    //  Method: btnGroupCollapsedDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnGroupCollapsedDetails button.
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
    btnGroupCollapsedDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        const String ColumnDescription = "Group Collapsed";

        OnDetailsClick(this.btnGroupCollapsedDetails,
            this.cbxGroupCollapsedSourceColumnName, ColumnDescription,
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.Collapsed,

            new NumericComparisonColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.GroupCollapsedDetails,

                (ComboBoxPlus comboBoxPlus) =>
                    ( new BooleanConverter() ).PopulateComboBox(
                        comboBoxPlus, false),

                ColumnDescription
                )
            );
    }

    //*************************************************************************
    //  Method: btnGroupLabelDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnGroupLabelDetails button.
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
    btnGroupLabelDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        OnDetailsClick(this.btnGroupLabelDetails,
            this.cbxGroupLabelSourceColumnName, "Group Label",
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.Label, 

            new GroupLabelColumnAutoFillUserSettingsDialog(
                m_oAutoFillUserSettings.GroupLabelDetails)
            );
    }

    //*************************************************************************
    //  Method: tsmWorksheetColumnDetails_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsmWorksheetColumnDetails
    /// ToolStripMenuItem.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The tsmWorksheetColumnDetails ToolStripMenuItem is part of the
    /// cmsWorksheetColumn ContextMenuStrip, which is programatically shown by
    /// OnDetailsClick().
    /// </remarks>
    //*************************************************************************

    private void
    tsmWorksheetColumnDetails_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // OnDetailsClick() stored the Form to show in the menu item's Tag.

        Debug.Assert(this.tsmWorksheetColumnDetails.Tag is Form);

        ( (Form)this.tsmWorksheetColumnDetails.Tag ).ShowDialog();
    }

    //*************************************************************************
    //  Method: tsmClearColumn_Click()
    //
    /// <summary>
    /// Handles the Click event on the tsmClearColumn ToolStripMenuItem.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// The tsmClearColumn ToolStripMenuItem is part of the cmsWorksheetColumn
    /// ContextMenuStrip, which is programatically shown by OnDetailsClick().
    /// </remarks>
    //*************************************************************************

    private void
    tsmClearColumn_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // OnDetailsClick() stored the worksheet name, table name, column name
        // and source column name ComboBox in the menu item's Tag.

        Debug.Assert( this.tsmClearColumn.Tag is Object[] );
        Object [] aoColumnInformation = ( Object[] )this.tsmClearColumn.Tag;
        Debug.Assert(aoColumnInformation.Length == 4);
        Debug.Assert(aoColumnInformation[0] is String);
        Debug.Assert(aoColumnInformation[1] is String);
        Debug.Assert(aoColumnInformation[2] is String);
        Debug.Assert(aoColumnInformation[3] is ComboBox);

        ClearColumn(
            (String)aoColumnInformation[0],
            (String)aoColumnInformation[1],
            (String)aoColumnInformation[2],
            (ComboBox)aoColumnInformation[3] 
            );
    }

    //*************************************************************************
    //  Method: btnClearAllColumns_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnClearAllColumns button.
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
    btnClearAllColumns_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if ( !AskWarningQuestion(
            "This will immediately clear all \"autofillable\" worksheet"
            + " columns, regardless of whether they have been autofilled or"
            + " filled in manually.  (You can clear a single worksheet column"
            + " by clicking an arrow button in the Options column of the"
            + " Autofill Columns dialog box.)"
            + " \r\n\r\n"
            + "Do you want to clear all worksheet columns now?"
            ) )
        {
            return;
        }

        foreach (ComboBox oComboBox in m_aoEdgeSourceColumnNameComboBoxes)
        {
            ClearColumn(WorksheetNames.Edges, TableNames.Edges,
                GetDestinationColumnNameFromComboBox(oComboBox), oComboBox);
        }

        foreach (ComboBox oComboBox in m_aoVertexSourceColumnNameComboBoxes)
        {
            ClearColumn(WorksheetNames.Vertices, TableNames.Vertices,
                GetDestinationColumnNameFromComboBox(oComboBox), oComboBox);
        }

        foreach (ComboBox oComboBox in m_aoGroupSourceColumnNameComboBoxes)
        {
            ClearColumn(WorksheetNames.Groups, TableNames.Groups,
                GetDestinationColumnNameFromComboBox(oComboBox), oComboBox);
        }
    }

    //*************************************************************************
    //  Method: btnResetAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnResetAll button.
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
    btnResetAll_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if ( !AskWarningQuestion(
            "This will clear all the source column names and reset any options"
            + " you have changed.  It will not clear any worksheet columns."
            + "\r\n\r\n"
            + "Do you want to reset all autofill settings?"
            ) )
        {
            return;
        }

        m_oAutoFillUserSettings.Reset();
        DoDataExchange(false);
    }

    //*************************************************************************
    //  Method: btnAutoFill_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAutoFill button.
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
    btnAutoFill_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        if ( DoDataExchange(true) )
        {
            if (m_eMode == DialogMode.EditOnly)
            {
                // The dialog is modal, and btnAutoFill is an "OK" button that
                // should close the dialog.

                DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            try
            {
                WorkbookAutoFiller.AutoFillWorkbook(
                    m_oWorkbook, m_oAutoFillUserSettings);

                EventUtil.FireEvent(this, this.WorkbookAutoFilled);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
            }
        }
    }

    //*************************************************************************
    //  Method: btnClose_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnClose button.
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
    btnClose_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

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

        // m_eMode
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oAutoFillUserSettings != null);
        Debug.Assert(m_oAutoFillWorkbookDialogUserSettings != null);
        Debug.Assert(m_aoEdgeSourceColumnNameComboBoxes != null);
        Debug.Assert(m_aoVertexSourceColumnNameComboBoxes != null);
        Debug.Assert(m_aoGroupSourceColumnNameComboBoxes != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Message to display when the user asks for an explanation of how
    /// outliers are log scales work.

    protected internal const String OutliersAndLogsMessage =

        "If you map from the smallest and largest numbers in the source"
        + " column, the results may be skewed by a few unusually small or"
        + " large numbers, or \"outliers.\"  You can prevent this by checking"
        + " the \"Ignore outliers\" checkbox, which causes all source column"
        + " numbers that fall outside one standard deviation of the average"
        + " number to be ignored when the numbers are mapped."
        + "\r\n\r\n"
        + "A linear mapping is used by default, but you can use a logarithmic"
        + " mapping instead.  This can be useful when the numbers in the"
        + " source column span a very large range even when outliers are"
        + " ignored."
        + "\r\n\r\n"
        + "When using a logarithmic mapping, any negative or zero values in"
        + " the source column will not get mapped."
        + "\r\n\r\n"
        + "You can ignore outliers and use a logarithmic mapping in any"
        + " combination."
        ;

    /// Message to display when the user tries to use logs with a source range
    /// that is not entirely positive.

    protected internal const String NegativeSourceRangeMessage =
        "When using a logarithmic mapping, the source column numbers must be"
        + " positive.";

    /// destinationColumnNamePlural argument to the
    /// NumericRangeColumnAutoFillUserSettingsDialog constructor for coordinate
    /// columns.

    protected const String CoordinateColumnNamePlural = "Coordinates";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates the mode in which the dialog is being used.

    protected DialogMode m_eMode;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// AutoFill user settings edited by this dialog.

    protected AutoFillUserSettings m_oAutoFillUserSettings;

    /// User settings for this dialog.

    protected AutoFillWorkbookDialogUserSettings
        m_oAutoFillWorkbookDialogUserSettings;

    /// Array of ComboBoxes for the edge source column names.

    protected EdgeColumnComboBox [] m_aoEdgeSourceColumnNameComboBoxes;

    /// Array of ComboBoxes for the vertex source column names.

    protected VertexColumnComboBox [] m_aoVertexSourceColumnNameComboBoxes;

    /// Array of ComboBoxes for the group source column names.

    protected GroupColumnComboBox [] m_aoGroupSourceColumnNameComboBoxes;
}


//*****************************************************************************
//  Class: AutoFillWorkbookDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="AutoFillWorkbookDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutoFillWorkbookDialog5") ]

public class AutoFillWorkbookDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AutoFillWorkbookDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillWorkbookDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AutoFillWorkbookDialogUserSettings
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
    //  Property: TabControlSelectedIndex
    //
    /// <summary>
    /// Gets or sets the selected index of the dialog's TabControl.
    /// </summary>
    ///
    /// <value>
    /// The selected index.  The default value is 0.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("0") ]

    public Int32
    TabControlSelectedIndex
    {
        get
        {
            AssertValid();

            return ( (Int32)this[TabControlSelectedIndexKey] );
        }

        set
        {
            this[TabControlSelectedIndexKey] = value;

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
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the TabControlSelectedIndex property.

    protected const String TabControlSelectedIndexKey =
        "TabControlSelectedIndex";
}
}
