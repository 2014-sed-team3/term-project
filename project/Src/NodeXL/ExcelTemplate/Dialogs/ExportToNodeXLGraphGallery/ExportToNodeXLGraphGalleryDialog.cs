

using System;
using System.Configuration;
using System.Windows.Forms;
using System.ServiceModel;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.Algorithms;
using Smrf.NodeXL.Common;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportToNodeXLGraphGalleryDialog
//
/// <summary>
/// Exports the graph to the NodeXL Graph Gallery.
/// </summary>
///
/// <remarks>
/// This dialog lets the user specify export settings.  It saves the settings
/// in an <see cref="ExportToNodeXLGraphGalleryUserSettings" /> object.  If
/// the <see cref="DialogMode" /> argument to the constructor is <see
/// cref="DialogMode.Normal" />, it also exports the graph.
///
/// <para>
/// If the user edits and saves the settings, <see cref="Form.ShowDialog()" />
/// returns DialogResult.OK.  Otherwise, it returns DialogResult.Cancel.
/// </para>
///
/// <para>
/// Most of the work is done by an internal <see
/// cref="NodeXLGraphGalleryExporter" /> object.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class ExportToNodeXLGraphGalleryDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ExportToNodeXLGraphGalleryDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportToNodeXLGraphGalleryDialog" /> class.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the dialog is being used.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl containing the graph.  This can be null if <paramref
    /// name="mode" /> is <see cref="DialogMode.EditOnly" />.
    /// </param>
    //*************************************************************************

    public ExportToNodeXLGraphGalleryDialog
    (
        DialogMode mode,
        Microsoft.Office.Interop.Excel.Workbook workbook,
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(nodeXLControl != null || mode == DialogMode.EditOnly);

        m_eMode = mode;
        m_oWorkbook = workbook;
        m_oNodeXLControl = nodeXLControl;

        m_oExportToNodeXLGraphGalleryUserSettings =
            new ExportToNodeXLGraphGalleryUserSettings();

        m_oPasswordUserSettings = new PasswordUserSettings();

        InitializeComponent();

        if (m_eMode == DialogMode.EditOnly)
        {
            InitializeForEditOnly();
        }

        lnkNodeXLGraphGallery.Tag = ProjectInformation.NodeXLGraphGalleryUrl;

        usrExportedFilesDescription.Workbook = workbook;

        lnkCreateAccount.Tag =
            ProjectInformation.NodeXLGraphGalleryCreateAccountUrl;

        // Instantiate an object that saves and retrieves the position of
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oExportToNodeXLGraphGalleryDialogUserSettings =
            new ExportToNodeXLGraphGalleryDialogUserSettings(this);

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
        /// The user can edit the dialog settings and then export the graph.

        Normal,

        /// The user can edit the dialog settings but cannot export the graph.

        EditOnly,
    }

    //*************************************************************************
    //  Method: InitializeForEditOnly()
    //
    /// <summary>
    /// Initializes controls when the dialog mode is <see
    /// cref="DialogMode.EditOnly" />.
    /// </summary>
    //*************************************************************************

    protected void
    InitializeForEditOnly()
    {
        this.Text += " Options";
        lnkNodeXLGraphGallery.Enabled = false;

        usrExportedFilesDescription.Title =
            "[The file name will be used as the title.]";

        usrExportedFilesDescription.Description =
            "[The graph summary will be used as the description.]";

        usrExportedFilesDescription.Enabled = false;
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
            if ( !usrExportedFilesDescription.Validate() )
            {
                return (false);
            }

            m_oExportToNodeXLGraphGalleryUserSettings.UseCredentials =
                radUseCredentials.Checked;

            String sAuthor = null;
            String sPassword = null;

            if (m_oExportToNodeXLGraphGalleryUserSettings.UseCredentials)
            {
                if (
                    !ValidateRequiredTextBox(txbAuthor,
                        "Enter a user name.", out sAuthor)
                    ||
                    !ValidateRequiredTextBox(txbPassword,
                        "Enter a password.", out sPassword)
                    )
                {
                    return (false);
                }
            }
            else
            {
                if (
                    !ValidateRequiredTextBox(txbGuestName,
                        "Enter a guest name.", out sAuthor)
                    )
                {
                    return (false);
                }
            }

            if ( !usrExportedFiles.Validate() )
            {
                return (false);
            }

            if (m_eMode == DialogMode.Normal)
            {
                m_oExportToNodeXLGraphGalleryUserSettings.Title =
                    usrExportedFilesDescription.Title;

                m_oExportToNodeXLGraphGalleryUserSettings.Description =
                    usrExportedFilesDescription.Description;
            }

            m_oExportToNodeXLGraphGalleryUserSettings.SpaceDelimitedTags =
                txbSpaceDelimitedTags.Text.Trim();

            m_oExportToNodeXLGraphGalleryUserSettings.
                ExportWorkbookAndSettings =
                usrExportedFiles.ExportWorkbookAndSettings;

            m_oExportToNodeXLGraphGalleryUserSettings.ExportGraphML =
                usrExportedFiles.ExportGraphML;

            m_oExportToNodeXLGraphGalleryUserSettings
                .UseFixedAspectRatio = usrExportedFiles.UseFixedAspectRatio;

            m_oExportToNodeXLGraphGalleryUserSettings.Author = sAuthor;

            m_oPasswordUserSettings.NodeXLGraphGalleryPassword =
                sPassword;
        }
        else
        {
            if (m_eMode == DialogMode.Normal)
            {
                usrExportedFilesDescription.Title =
                    m_oExportToNodeXLGraphGalleryUserSettings.Title;

                usrExportedFilesDescription.Description =
                    m_oExportToNodeXLGraphGalleryUserSettings.Description;
            }

            txbSpaceDelimitedTags.Text =
                m_oExportToNodeXLGraphGalleryUserSettings.
                SpaceDelimitedTags;

            usrExportedFiles.ExportWorkbookAndSettings =
                m_oExportToNodeXLGraphGalleryUserSettings.
                ExportWorkbookAndSettings;

            usrExportedFiles.ExportGraphML =
                m_oExportToNodeXLGraphGalleryUserSettings.ExportGraphML;

            usrExportedFiles.UseFixedAspectRatio = 
                m_oExportToNodeXLGraphGalleryUserSettings
                .UseFixedAspectRatio;

            if (m_oExportToNodeXLGraphGalleryUserSettings.UseCredentials)
            {
                radUseCredentials.Checked = true;

                txbAuthor.Text =
                    m_oExportToNodeXLGraphGalleryUserSettings.Author;

                txbPassword.Text =
                    m_oPasswordUserSettings.NodeXLGraphGalleryPassword;
            }
            else
            {
                radAsGuest.Checked = true;

                txbGuestName.Text =
                    m_oExportToNodeXLGraphGalleryUserSettings.Author;
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

        EnableControls(radAsGuest.Checked,
            lblGuestName, txbGuestName);

        EnableControls(radUseCredentials.Checked,
            lblAuthor, txbAuthor, lblPassword, txbPassword);
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
        EventArgs e
    )
    {
        AssertValid();

        if ( !DoDataExchange(true) )
        {
            return;
        }

        if (m_eMode == DialogMode.Normal)
        {
            NodeXLGraphGalleryExporter oNodeXLGraphGalleryExporter =
                new NodeXLGraphGalleryExporter();

            this.Cursor = Cursors.WaitCursor;

            try
            {
                Debug.Assert(m_oNodeXLControl != null);

                oNodeXLGraphGalleryExporter.ExportToNodeXLGraphGallery(

                    m_oWorkbook, m_oNodeXLControl,
                    m_oExportToNodeXLGraphGalleryUserSettings.Title,
                    m_oExportToNodeXLGraphGalleryUserSettings.Description,

                    m_oExportToNodeXLGraphGalleryUserSettings.
                        SpaceDelimitedTags,

                    m_oExportToNodeXLGraphGalleryUserSettings.Author,

                    m_oExportToNodeXLGraphGalleryUserSettings.UseCredentials
                        ? m_oPasswordUserSettings.NodeXLGraphGalleryPassword
                        : null,

                    m_oExportToNodeXLGraphGalleryUserSettings.
                        ExportWorkbookAndSettings,

                    m_oExportToNodeXLGraphGalleryUserSettings.ExportGraphML,

                    m_oExportToNodeXLGraphGalleryUserSettings
                        .UseFixedAspectRatio
                    );
            }
            catch (Exception oException)
            {
                String sMessage;

                if (NodeXLGraphGalleryExceptionHandler
                    .TryGetMessageForRecognizedException(oException,
                        out sMessage)
                    )
                {
                    this.ShowWarning(sMessage);
                }
                else
                {
                    ErrorUtil.OnException(oException);
                }

                return;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        m_oExportToNodeXLGraphGalleryUserSettings.Save();
        m_oPasswordUserSettings.Save();

        this.DialogResult = DialogResult.OK;
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

        Debug.Assert(m_oNodeXLControl != null
            || m_eMode == DialogMode.EditOnly);

        Debug.Assert(m_oExportToNodeXLGraphGalleryUserSettings != null);
        Debug.Assert(m_oExportToNodeXLGraphGalleryDialogUserSettings != null);
        Debug.Assert(m_oPasswordUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates the mode in which the dialog is being used.

    protected DialogMode m_eMode;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// NodeXLControl containing the graph, or null.

    protected NodeXLControl m_oNodeXLControl;

    /// User settings for exporting the graph to the NodeXL Graph Gallery.

    protected ExportToNodeXLGraphGalleryUserSettings
        m_oExportToNodeXLGraphGalleryUserSettings;

    /// Password user settings.

    protected PasswordUserSettings m_oPasswordUserSettings;

    /// User settings for this dialog.

    protected ExportToNodeXLGraphGalleryDialogUserSettings
        m_oExportToNodeXLGraphGalleryDialogUserSettings;
}


//*****************************************************************************
//  Class: ExportToNodeXLGraphGalleryDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ExportToNodeXLGraphGalleryDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ExportToNodeXLGraphGalleryDialog") ]

public class ExportToNodeXLGraphGalleryDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ExportToNodeXLGraphGalleryDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportToNodeXLGraphGalleryDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ExportToNodeXLGraphGalleryDialogUserSettings
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
}
}
