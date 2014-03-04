

using System;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CreateSubgraphImagesDialog
//
/// <summary>
/// Dialog that creates a subgraph image for each of a graph's vertices.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to run the dialog.  All image
/// creation is performed within the dialog.
///
/// <para>
/// A <see cref="SubgraphImageCreator" /> object does most of the work.  The
/// image creation is done asynchronously, so it doesn't hang the UI and can be
/// cancelled by the user.  However, the optional insertion of thumbnail images
/// into the vertex worksheet is done synchronously, because you can't update
/// the UI in Windows from a background thread.  During thumbnail insertion,
/// all dialog controls are disabled and the UI doesn't respond to user input.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class CreateSubgraphImagesDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: CreateSubgraphImagesDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CreateSubgraphImagesDialog" /> class.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the dialog is being used.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.  If <paramref name="mode" /> is
    /// <see cref="DialogMode.EditOnly" />, the workbook isn't used and this
    /// parameter must be null.
    /// </param>
    ///
    /// <param name="selectedVertexNames">
    /// Collection of zero or more vertex names corresponding to the selected
    /// rows in the vertex worksheet.  If <paramref name="mode" /> is <see
    /// cref="DialogMode.EditOnly" />, the collection isn't used and this
    /// parameter must be null.
    /// </param>
    //*************************************************************************

    public CreateSubgraphImagesDialog
    (
        DialogMode mode,
        Microsoft.Office.Interop.Excel.Workbook workbook,
        ICollection<String> selectedVertexNames
    )
    {
        InitializeComponent();

        m_eMode = mode;
        m_oWorkbook = workbook;
        m_oSelectedVertexNames = selectedVertexNames;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oCreateSubgraphImagesDialogUserSettings =
            new CreateSubgraphImagesDialogUserSettings(this);

        m_oSubgraphImageCreator = new SubgraphImageCreator();

        m_oSubgraphImageCreator.ImageCreationProgressChanged +=
            new ProgressChangedEventHandler(
                SubgraphImageCreator_ImageCreationProgressChanged);

        m_oSubgraphImageCreator.ImageCreationCompleted +=
            new RunWorkerCompletedEventHandler(
                SubgraphImageCreator_ImageCreationCompleted);

        m_eState = DialogState.Idle;

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
        /// The user can edit the dialog settings and then create subgraph
        /// images.

        Normal,

        /// The user can edit the dialog settings but cannot create subgraph
        /// images.

        EditOnly,

        /// Subgraph images are created as soon as the dialog opens, and the
        /// dialog closes after the images are created.

        Automate,
    }

    //*************************************************************************
    //  Enum: DialogState
    //
    /// <summary>
    /// Indicates the state of the dialog.
    /// </summary>
    //*************************************************************************

    protected enum
    DialogState
    {
        /// Idle, waiting for user action.

        Idle,

        /// Creating subgraph images.

        CreatingSubgraphImages,

        /// Populating the subgraph image column on the vertex worksheet.

        PopulatingImageColumn,
    }

    //*************************************************************************
    //  Property: State
    //
    /// <summary>
    /// Gets or sets the state of the dialog.
    /// </summary>
    ///
    /// <value>
    /// The state of the dialog.
    /// </value>
    //*************************************************************************

    protected DialogState
    State
    {
        get
        {
            AssertValid();

            return (m_eState);
        }

        set
        {
            m_eState = value;

            EnableControls();

            AssertValid();
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
            Boolean bSaveToFolder = chkSaveToFolder.Checked;

            if (bSaveToFolder)
            {
                if (
                    !usrFolder.Validate()
                    ||
                    !usrImageFormat.Validate()
                    )
                {
                    return (false);
                }
            }

            Boolean bInsertThumbnails = chkInsertThumbnails.Checked;
            Int32 iThumbnailWidthPx = Int32.MinValue;
            Int32 iThumbnailHeightPx = Int32.MinValue;

            if (bInsertThumbnails)
            {
                if (
                    !ValidateNumericUpDown(nudThumbnailWidthPx,
                        "thumbnail width", out iThumbnailWidthPx)
                    ||
                    !ValidateNumericUpDown(nudThumbnailHeightPx,
                        "thumbnail height", out iThumbnailHeightPx)
                    )
                {
                    return (false);
                }
            }

            // All data is now valid.

            if (bSaveToFolder)
            {
                m_oCreateSubgraphImagesDialogUserSettings.Folder =
                    usrFolder.FolderPath;

                m_oCreateSubgraphImagesDialogUserSettings.ImageSizePx =
                    usrImageFormat.ImageSizePx;

                m_oCreateSubgraphImagesDialogUserSettings.ImageFormat =
                    usrImageFormat.ImageFormat;
            }

            if (bInsertThumbnails)
            {
                m_oCreateSubgraphImagesDialogUserSettings.ThumbnailSizePx =
                    new Size(iThumbnailWidthPx, iThumbnailHeightPx);
            }

            m_oCreateSubgraphImagesDialogUserSettings.Levels =
                usrSubgraphLevels.Levels;

            m_oCreateSubgraphImagesDialogUserSettings.SaveToFolder =
                bSaveToFolder;

            m_oCreateSubgraphImagesDialogUserSettings.InsertThumbnails =
                bInsertThumbnails;

            m_oCreateSubgraphImagesDialogUserSettings.SelectedVerticesOnly =
                chkSelectedVerticesOnly.Checked;

            m_oCreateSubgraphImagesDialogUserSettings.SelectVertex =
                chkSelectVertex.Checked;

            m_oCreateSubgraphImagesDialogUserSettings.SelectIncidentEdges =
                chkSelectIncidentEdges.Checked;
        }
        else
        {
            usrSubgraphLevels.Levels =
                m_oCreateSubgraphImagesDialogUserSettings.Levels;

            chkSaveToFolder.Checked =
                m_oCreateSubgraphImagesDialogUserSettings.SaveToFolder;

            usrFolder.FolderPath =
                m_oCreateSubgraphImagesDialogUserSettings.Folder;

            usrImageFormat.ImageSizePx = 
                m_oCreateSubgraphImagesDialogUserSettings.ImageSizePx;

            usrImageFormat.ImageFormat = 
                m_oCreateSubgraphImagesDialogUserSettings.ImageFormat;

            chkInsertThumbnails.Checked =
                m_oCreateSubgraphImagesDialogUserSettings.InsertThumbnails;

            Size oThumbnailSizePx =
                m_oCreateSubgraphImagesDialogUserSettings.ThumbnailSizePx;

            nudThumbnailWidthPx.Value = oThumbnailSizePx.Width;
            nudThumbnailHeightPx.Value = oThumbnailSizePx.Height;

            chkSelectedVerticesOnly.Checked =
                m_oCreateSubgraphImagesDialogUserSettings.SelectedVerticesOnly;

            chkSelectVertex.Checked =
                m_oCreateSubgraphImagesDialogUserSettings.SelectVertex;

            chkSelectIncidentEdges.Checked =
                m_oCreateSubgraphImagesDialogUserSettings.SelectIncidentEdges;

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

        const String StopText = "Stop";

        switch (this.State)
        {
            case DialogState.Idle:

                pnlDisableWhileCreating.Enabled = true;
                btnCreate.Enabled = true;

                btnCreate.Text = (m_eMode == DialogMode.EditOnly) ?
                    "OK" : "Create";

                btnClose.Enabled = true;
                this.UseWaitCursor = false;

                pnlSaveToFolder.Enabled = chkSaveToFolder.Checked;
                grpThumbnailSize.Enabled = chkInsertThumbnails.Checked;

                if (m_oSelectedVertexNames != null &&
                    m_oSelectedVertexNames.Count > 0)
                {
                    chkSelectedVerticesOnly.Enabled = true;
                }
                else
                {
                    chkSelectedVerticesOnly.Checked = false;
                    chkSelectedVerticesOnly.Enabled = false;
                }

                break;

            case DialogState.CreatingSubgraphImages:

                pnlDisableWhileCreating.Enabled = false;
                btnCreate.Enabled = true;
                btnCreate.Text = StopText;
                btnClose.Enabled = true;
                this.UseWaitCursor = true;

                break;

            case DialogState.PopulatingImageColumn:

                pnlDisableWhileCreating.Enabled = false;
                btnCreate.Enabled = false;
                btnCreate.Text = StopText;
                btnClose.Enabled = false;
                this.UseWaitCursor = true;

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: StartImageCreation()
    //
    /// <summary>
    /// Starts the creation of subgraph images.
    /// </summary>
    ///
    /// <remarks>
    /// It's assumed that m_oCreateSubgraphImagesDialogUserSettings contains
    /// valid settings.
    /// </remarks>
    //*************************************************************************

    protected void
    StartImageCreation()
    {
        AssertValid();
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oSelectedVertexNames != null);

        // Read the workbook into a new IGraph.

        IGraph oGraph;

        try
        {
            oGraph = ReadWorkbook(m_oWorkbook);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            this.State = DialogState.Idle;

            return;
        }

        lblStatus.Text = "Creating subgraph images.";

        ICollection<IVertex> oSelectedVertices = new IVertex[0];

        if (m_oCreateSubgraphImagesDialogUserSettings.SelectedVerticesOnly)
        {
            // Get the vertices corresponding to the selected rows in the
            // vertex worksheet.

            oSelectedVertices = GetSelectedVertices(
                oGraph, m_oSelectedVertexNames);
        }

        m_oSubgraphImageCreator.CreateSubgraphImagesAsync(
            oGraph,
            oSelectedVertices,
            m_oCreateSubgraphImagesDialogUserSettings.Levels,
            m_oCreateSubgraphImagesDialogUserSettings.SaveToFolder,
            m_oCreateSubgraphImagesDialogUserSettings.Folder,
            m_oCreateSubgraphImagesDialogUserSettings.ImageSizePx,
            m_oCreateSubgraphImagesDialogUserSettings.ImageFormat,
            m_oCreateSubgraphImagesDialogUserSettings.InsertThumbnails,
            m_oCreateSubgraphImagesDialogUserSettings.ThumbnailSizePx,
            m_oCreateSubgraphImagesDialogUserSettings.SelectedVerticesOnly,
            m_oCreateSubgraphImagesDialogUserSettings.SelectVertex,
            m_oCreateSubgraphImagesDialogUserSettings.SelectIncidentEdges,
            new GeneralUserSettings(),
            new LayoutUserSettings()
            );
    }

    //*************************************************************************
    //  Method: ReadWorkbook()
    //
    /// <summary>
    /// Reads a workbook into a new graph.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The workbook to read.
    /// </param>
    ///
    /// <returns>
    /// The new graph.
    /// </returns>
    //*************************************************************************

    protected IGraph
    ReadWorkbook
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();

        WorkbookReader oWorkbookReader = new WorkbookReader();

        // Convert the workbook contents to a Graph object.

        return ( oWorkbookReader.ReadWorkbook(
            oWorkbook, oReadWorkbookContext) );
    }

    //*************************************************************************
    //  Method: GetSelectedVertices()
    //
    /// <summary>
    /// Gets the vertices corresponding to the selected rows in the vertex
    /// worksheet.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph created from the workbook.
    /// </param>
    ///
    /// <param name="oSelectedVertexNames">
    /// Collection of zero or more vertex names corresponding to the selected
    /// rows in the vertex worksheet.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// Collection of vertices in <paramref name="oGraph" /> corresponding to
    /// the vertex names in <paramref name="oSelectedVertexNames" />.
    /// </returns>
    //*************************************************************************

    protected ICollection<IVertex>
    GetSelectedVertices
    (
        IGraph oGraph,
        ICollection<String> oSelectedVertexNames
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oSelectedVertexNames != null);
        AssertValid();

        List<IVertex> oSelectedVertices = new List<IVertex>();

        // Store the selected vertex names in a HashSet for quick lookup.  The
        // key is the vertex name.

        HashSet<String> oSelectedVertexNameHashSet =
            new HashSet<String>(oSelectedVertexNames);

        // Loop through the graph's vertices, looking for vertex names that are
        // in the dictionary.

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            Debug.Assert( !String.IsNullOrEmpty(oVertex.Name) );

            if (oSelectedVertexNames.Contains(oVertex.Name) )
            {
                oSelectedVertices.Add(oVertex);
            }
        }

        return (oSelectedVertices);
    }

    //*************************************************************************
    //  Method: OnLoad()
    //
    /// <summary>
    /// Handles the OnLoad event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event arguments.
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

        if (m_eMode == DialogMode.Automate)
        {
            // Automatically start image creation.

            btnCreate.PerformClick();
        }
        else if (m_eMode == DialogMode.EditOnly)
        {
            this.Text += " Options";
            btnClose.Text = "Cancel";
        }
    }

    //*************************************************************************
    //  Method: OnImageCreationCompleted()
    //
    /// <summary>
    /// Handles the ImageCreationCompleted event on the SubgraphImageCreator
    /// object.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    OnImageCreationCompleted
    (
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oSelectedVertexNames != null);

        if (e.Cancelled)
        {
            this.State = DialogState.Idle;

            lblStatus.Text = "Image creation stopped.";
        }
        else if (e.Error != null)
        {
            this.State = DialogState.Idle;

            Exception oException = e.Error;

            if (oException is System.IO.IOException)
            {
                lblStatus.Text = "Image creation error.";

                this.ShowWarning(oException.Message);
            }
            else
            {
                ErrorUtil.OnException(oException);
            }
        }
        else
        {
            // Success.  Were temporary images created that need to be inserted
            // into the vertex worksheet?

            Debug.Assert(e.Result is TemporaryImages);

            TemporaryImages oTemporaryImages = (TemporaryImages)e.Result;

            if (oTemporaryImages.Folder != null)
            {
                // Yes.  Insert them, then delete the temporary images.

                this.State = DialogState.PopulatingImageColumn;

                String sLastStatusFromSubgraphImageCreator = lblStatus.Text;

                lblStatus.Text =
                    "Inserting subgraph thumbnails into the worksheet.  Please"
                    + " wait...";

                TableImagePopulator.PopulateColumnWithImages(m_oWorkbook,
                    WorksheetNames.Vertices, TableNames.Vertices,
                    VertexTableColumnNames.SubgraphImage,
                    VertexTableColumnNames.VertexName, oTemporaryImages
                    );

                lblStatus.Text = sLastStatusFromSubgraphImageCreator;
            }

            this.State = DialogState.Idle;

            if (m_eMode == DialogMode.Automate)
            {
                this.Close();
            }
        }
    }

    //*************************************************************************
    //  Method: OnClosed()
    //
    /// <summary>
    /// Handles the Closed event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnClosed
    (
        EventArgs e
    )
    {
        AssertValid();

        if (m_oSubgraphImageCreator.IsBusy)
        {
            // Let the background thread cancel its task, but don't try to
            // notify this dialog.

            m_oSubgraphImageCreator.ImageCreationProgressChanged -=
                new ProgressChangedEventHandler(
                    SubgraphImageCreator_ImageCreationProgressChanged);

            m_oSubgraphImageCreator.ImageCreationCompleted -=
                new RunWorkerCompletedEventHandler(
                    SubgraphImageCreator_ImageCreationCompleted);

            m_oSubgraphImageCreator.CancelAsync();
        }
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
    //  Method: btnCreate_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCreate button.
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
    btnCreate_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        switch (this.State)
        {
            case DialogState.Idle:

                if (!m_oSubgraphImageCreator.IsBusy)
                {
                    if ( DoDataExchange(true) )
                    {
                        if (m_eMode == DialogMode.EditOnly)
                        {
                            this.Close();
                            return;
                        }
                        else
                        {
                            this.State = DialogState.CreatingSubgraphImages;
                            StartImageCreation();
                        }
                    }
                }

                break;

            case DialogState.CreatingSubgraphImages:

                if (m_oSubgraphImageCreator.IsBusy)
                {
                    // Request to cancel image creation.  When the request is
                    // completed, SubgraphImageCreator_ImageCreationCompleted()
                    // will be called.

                    m_oSubgraphImageCreator.CancelAsync();
                }

                break;

            case DialogState.PopulatingImageColumn:

                // (Do nothing.)

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: SubgraphImageCreator_ImageCreationProgressChanged()
    //
    /// <summary>
    /// Handles the ImageCreationProgressChanged event on the
    /// SubgraphImageCreator object.
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
    SubgraphImageCreator_ImageCreationProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        Debug.Assert(e.UserState is String);
        AssertValid();

        lblStatus.Text = (String)e.UserState;
    }

    //*************************************************************************
    //  Method: SubgraphImageCreator_ImageCreationCompleted()
    //
    /// <summary>
    /// Handles the ImageCreationCompleted event on the SubgraphImageCreator
    /// object.
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
    SubgraphImageCreator_ImageCreationCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        OnImageCreationCompleted(e);
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

        Debug.Assert(m_eMode == DialogMode.EditOnly || m_oWorkbook != null);

        Debug.Assert(m_eMode == DialogMode.EditOnly ||
            m_oSelectedVertexNames != null);

        Debug.Assert(m_oCreateSubgraphImagesDialogUserSettings != null);
        Debug.Assert(m_oSubgraphImageCreator != null);
        // m_eState
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates the mode in which the dialog is being used.

    protected DialogMode m_eMode;

    /// Workbook containing the graph data, or null if m_eMode == EditOnly.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Collection of zero or more vertex names corresponding to the selected
    /// rows in the vertex worksheet, or null if m_eMode == EditOnly.

    protected ICollection<String> m_oSelectedVertexNames;

    /// User settings for this dialog.

    protected CreateSubgraphImagesDialogUserSettings
        m_oCreateSubgraphImagesDialogUserSettings;

    /// Object that does most of the work.

    protected SubgraphImageCreator m_oSubgraphImageCreator;

    /// Indicates the state of the dialog.

    protected DialogState m_eState;
}

}
