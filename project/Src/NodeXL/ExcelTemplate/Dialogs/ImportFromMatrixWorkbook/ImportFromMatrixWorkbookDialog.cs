

using System;
using System.Configuration;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ImportFromMatrixWorkbookDialog
//
/// <summary>
/// Imports a graph from an open matrix workbook to an <see cref="IGraph" />
/// object.
/// </summary>
///
/// <remarks>
/// If <see cref="Form.ShowDialog()" /> returns <see cref="DialogResult.OK" />,
/// the imported <see cref="IGraph" /> object can be obtained from the <see
/// cref="ImportFromWorkbookDialog.Graph" /> property.
///
/// <para>
/// See <see cref="MatrixWorkbookImporter" /> for details on the expected
/// format of the matrix workbook.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class ImportFromMatrixWorkbookDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ImportFromMatrixWorkbookDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromMatrixWorkbookDialog" /> class.
    /// </summary>
    ///
    /// <param name="destinationNodeXLWorkbook">
    /// Workbook to which the matrix workbook will eventually be imported.
    /// This class does NOT import the graph into the destination workbook, but
    /// it needs to know the destination to prevent the user from selecting
    /// it as a source workbook.
    /// </param>
    //*************************************************************************

    public ImportFromMatrixWorkbookDialog
    (
        Microsoft.Office.Interop.Excel.Workbook destinationNodeXLWorkbook
    )
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oImportFromMatrixWorkbookDialogUserSettings =
            new ImportFromMatrixWorkbookDialogUserSettings(this);

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
    /// The edges all contain the <see
    /// cref="ReservedMetadataKeys.EdgeWeight" /> key.
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
            m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookHasVertexNames =
                radSourceWorkbookHasVertexNames.Checked;

            m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookDirectedness = radDirected.Checked ?
                GraphDirectedness.Directed : GraphDirectedness.Undirected;
        }
        else
        {
            radAssignVertexNames.Checked =
                !(radSourceWorkbookHasVertexNames.Checked =
                m_oImportFromMatrixWorkbookDialogUserSettings.
                    SourceWorkbookHasVertexNames);

            radDirected.Checked =
                (m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookDirectedness == GraphDirectedness.Directed);

            radUndirected.Checked = !radDirected.Checked;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ImportFromMatrixWorkbook()
    //
    /// <summary>
    /// Imports a graph from a source matrix workbook.
    /// </summary>
    ///
    /// <remarks>
    /// This should be called only after DoDataExchange(true) returns true.
    /// </remarks>
    //*************************************************************************

    protected void
    ImportFromMatrixWorkbook()
    {
        AssertValid();

        MatrixWorkbookImporter oMatrixWorkbookImporter =
            new MatrixWorkbookImporter();

        m_oGraph = oMatrixWorkbookImporter.ImportMatrixWorkbook(
            m_oDestinationNodeXLWorkbook.Application,
            this.SourceWorkbookName,

            m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookHasVertexNames,

            m_oImportFromMatrixWorkbookDialogUserSettings.
                SourceWorkbookDirectedness
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

        if (lbxSourceWorkbook.Items.Count == 0)
        {
            this.ShowWarning(ExcelWorkbookListBox.NoOtherWorkbooks);
            this.Close();
        }
        else
        {
            lbxSourceWorkbook.SelectedIndex = 0;
        }
    }

    //*************************************************************************
    //  Method: lnkMatrixWorkbookSamples_Clicked()
    //
    /// <summary>
    /// Handles the Clicked event on the lnkMatrixWorkbookSamples LinkLabel.
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
    lnkMatrixWorkbookSamples_LinkClicked
    (
        object sender,
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        MatrixWorkbookSamplesDialog oMatrixWorkbookSamplesDialog =
            new MatrixWorkbookSamplesDialog();

        oMatrixWorkbookSamplesDialog.ShowDialog();
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

        this.Cursor = Cursors.WaitCursor;

        try
        {
            ImportFromMatrixWorkbook();
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
        finally
        {
            this.Cursor = Cursors.Default;
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

        Debug.Assert(m_oImportFromMatrixWorkbookDialogUserSettings != null);
        // m_oGraph
        Debug.Assert(m_oDestinationNodeXLWorkbook != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected ImportFromMatrixWorkbookDialogUserSettings
        m_oImportFromMatrixWorkbookDialogUserSettings;

    /// The imported IGraph object, or null.

    protected IGraph m_oGraph;

    /// Workbook to which the matrix workbook will be imported.

    protected Microsoft.Office.Interop.Excel.Workbook
        m_oDestinationNodeXLWorkbook;
}


//*****************************************************************************
//  Class: ImportFromMatrixWorkbookDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ImportFromMatrixWorkbookDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ImportFromMatrixWorkbookDialog") ]

public class ImportFromMatrixWorkbookDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ImportFromMatrixWorkbookDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromMatrixWorkbookDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ImportFromMatrixWorkbookDialogUserSettings
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
    //  Property: SourceWorkbookHasVertexNames
    //
    /// <summary>
    /// Gets or sets a flag that indicating whether the source workbook has
    /// vertex names in row 1 and column A.
    /// 
    /// </summary>
    ///
    /// <value>
    /// true if the source workbook has vertex names.  The default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    SourceWorkbookHasVertexNames
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SourceWorkbookHasVertexNamesKey] );
        }

        set
        {
            this[SourceWorkbookHasVertexNamesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SourceWorkbookDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness of the source workbook.
    /// </summary>
    ///
    /// <value>
    /// The graph directedness of the source workbook, as a GraphDirectedness.
    /// The default value is GraphDirectedness.Directed.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Directed") ]

    public GraphDirectedness
    SourceWorkbookDirectedness
    {
        get
        {
            AssertValid();

            return ( (GraphDirectedness)
                this[SourceWorkbookDirectednessKey] );
        }

        set
        {
            this[SourceWorkbookDirectednessKey] = value;

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

    /// Name of the settings key for the SourceWorkbookHasVertexNames property.

    protected const String SourceWorkbookHasVertexNamesKey =
        "SourceWorkbookHasVertexNames";

    /// Name of the settings key for the SourceWorkbookDirectedness property.

    protected const String SourceWorkbookDirectednessKey =
        "SourceWorkbookDirectedness";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
