

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;
using System.Net.Mail;
using Smrf.AppLib;
using Smrf.NodeXL.Visualization.Wpf;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportToEmailDialog
//
/// <summary>
/// Exports the graph to one or more email addresses.
/// </summary>
///
/// <remarks>
/// This dialog lets the user specify export settings.  It saves the settings
/// in an <see cref="ExportToEmailUserSettings" /> object.  If the <see
/// cref="DialogMode" /> argument to the constructor is <see
/// cref="DialogMode.Normal" />, it also exports the graph.
///
/// <para>
/// If the user edits and saves the settings, <see cref="Form.ShowDialog()" />
/// returns DialogResult.OK.  Otherwise, it returns DialogResult.Cancel.
/// </para>
///
/// <para>
/// Most of the work is done by an internal <see cref="EmailExporter" />
/// object.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class ExportToEmailDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ExportToEmailDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExportToEmailDialog" />
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
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl containing the graph.  This can be null if <paramref
    /// name="mode" /> is <see cref="DialogMode.EditOnly" />.
    /// </param>
    //*************************************************************************

    public ExportToEmailDialog
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
        m_oExportToEmailUserSettings = new ExportToEmailUserSettings();
        m_oPasswordUserSettings = new PasswordUserSettings();

        InitializeComponent();

        if (m_eMode == DialogMode.EditOnly)
        {
            InitializeForEditOnly();
        }

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oExportToEmailDialogUserSettings =
            new ExportToEmailDialogUserSettings(this);

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

        EnableControls(false, lblDialogDescription, lblSubject, txbSubject);
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
            String sToAddresses, sFromAddress, sSubject, sSmtpHost,
                sSmtpUserName, sSmtpPassword;

            Int32 iSmtpPort;

            if (
                !ValidateRequiredTextBox(txbToAddresses,

                    "Enter one or more \"to\" email addresses, separated with"
                    + " spaces, commas or returns.",

                    out sToAddresses)
                ||
                !ValidateRequiredTextBox(txbFromAddress,
                    "Enter a \"from\" email address.",
                    out sFromAddress)
                ||
                !ValidateRequiredTextBox(txbSubject,
                    "Enter a subject.",
                    out sSubject)
                ||
                !ValidateRequiredTextBox(txbSmtpHost,

                    "Enter the name of an SMTP server, such as"
                        + " smtp.gmail.com.",

                    out sSmtpHost)
                ||
                !ValidateInt32TextBox(txbSmtpPort, 1, 65535,
                    "Enter the SMTP port number.",
                    out iSmtpPort)
                ||
                !ValidateRequiredTextBox(txbSmtpUserName,
                    "Enter the user name for an account on the SMTP server.",
                    out sSmtpUserName)
                ||
                !ValidateRequiredTextBox(txbSmtpPassword,
                    "Enter the password for an account on the SMTP server.",
                    out sSmtpPassword)
                ||
                !usrExportedFiles.Validate()
                )
            {
                return (false);
            }

            m_oExportToEmailUserSettings.SpaceDelimitedToAddresses =
                String.Join( " ", StringUtil.SplitOnCommonDelimiters(
                    sToAddresses) );

            m_oExportToEmailUserSettings.FromAddress = sFromAddress;

            if (m_eMode == DialogMode.Normal)
            {
                m_oExportToEmailUserSettings.Subject = txbSubject.Text;
            }

            m_oExportToEmailUserSettings.MessageBody = txbMessageBody.Text;
            m_oExportToEmailUserSettings.SmtpHost = sSmtpHost;
            m_oExportToEmailUserSettings.SmtpPort = iSmtpPort;

            m_oExportToEmailUserSettings.UseSslForSmtp =
                chkUseSslForSmtp.Checked;

            m_oExportToEmailUserSettings.SmtpUserName = sSmtpUserName;
            m_oPasswordUserSettings.SmtpPassword = sSmtpPassword;

            m_oExportToEmailUserSettings.ExportWorkbookAndSettings =
                usrExportedFiles.ExportWorkbookAndSettings;

            m_oExportToEmailUserSettings.ExportGraphML =
                usrExportedFiles.ExportGraphML;

            m_oExportToEmailUserSettings.UseFixedAspectRatio =
                usrExportedFiles.UseFixedAspectRatio;
        }
        else
        {
            txbToAddresses.Text =
                m_oExportToEmailUserSettings.SpaceDelimitedToAddresses;

            txbFromAddress.Text = m_oExportToEmailUserSettings.FromAddress;

            txbSubject.Text = (m_eMode == DialogMode.Normal) ?
                m_oExportToEmailUserSettings.Subject
                :
                "[The file name will be used as the subject.]"
                ;

            txbMessageBody.Text = m_oExportToEmailUserSettings.MessageBody;
            txbSmtpHost.Text = m_oExportToEmailUserSettings.SmtpHost;

            txbSmtpPort.Text =
                m_oExportToEmailUserSettings.SmtpPort.ToString(
                    CultureInfo.InvariantCulture);

            chkUseSslForSmtp.Checked =
                m_oExportToEmailUserSettings.UseSslForSmtp;

            txbSmtpUserName.Text =
                m_oExportToEmailUserSettings.SmtpUserName;

            txbSmtpPassword.Text = m_oPasswordUserSettings.SmtpPassword;

            usrExportedFiles.ExportWorkbookAndSettings =
                m_oExportToEmailUserSettings.ExportWorkbookAndSettings;

            usrExportedFiles.ExportGraphML =
                m_oExportToEmailUserSettings.ExportGraphML;

            usrExportedFiles.UseFixedAspectRatio = 
                m_oExportToEmailUserSettings.UseFixedAspectRatio;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: btnInsertSampleMessageBody_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnInsertSampleMessageBody button.
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
    btnInsertSampleMessageBody_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        txbMessageBody.SelectedText = SampleMessageBody;
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
            this.Cursor = Cursors.WaitCursor;

            try
            {
                ( new EmailExporter() ).ExportToEmail(
                    m_oWorkbook,
                    m_oNodeXLControl,

                    m_oExportToEmailUserSettings
                        .SpaceDelimitedToAddresses.Split(' '),

                    m_oExportToEmailUserSettings.FromAddress,
                    m_oExportToEmailUserSettings.Subject,
                    m_oExportToEmailUserSettings.MessageBody,
                    m_oExportToEmailUserSettings.SmtpHost,
                    m_oExportToEmailUserSettings.SmtpPort,
                    m_oExportToEmailUserSettings.UseSslForSmtp,
                    m_oExportToEmailUserSettings.SmtpUserName,
                    m_oPasswordUserSettings.SmtpPassword,
                    m_oExportToEmailUserSettings.ExportWorkbookAndSettings,
                    m_oExportToEmailUserSettings.ExportGraphML,
                    m_oExportToEmailUserSettings.UseFixedAspectRatio
                    );
            }
            catch (Exception oException)
            {
                String sMessage;

                if ( EmailExceptionHandler
                    .TryGetMessageForRecognizedException(oException,
                        out sMessage) )
                {
                    if (oException is EmailAddressFormatException)
                    {
                        switch ( ( (EmailAddressFormatException)oException )
                            .EmailAddressType)
                        {
                            case EmailAddressType.To:

                                OnInvalidTextBox(txbToAddresses, sMessage);
                                break;

                            case EmailAddressType.From:

                                OnInvalidTextBox(txbFromAddress, sMessage);
                                break;

                            default:

                                Debug.Assert(false);
                                break;
                        }
                    }
                    else
                    {
                        this.ShowWarning(sMessage);
                    }
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

        m_oExportToEmailUserSettings.Save();
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

        Debug.Assert(m_oExportToEmailUserSettings != null);
        Debug.Assert(m_oPasswordUserSettings != null);
        Debug.Assert(m_oExportToEmailDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Sample message body.

    protected const String SampleMessageBody =

        "<img src=\"http://www.nodexlgraphgallery.org/Images/SiteLogo.png\" />"
        + "\r\n\r\n"
        + "This graph was brought to you by NodeXL."
        + "\r\n\r\n"
        + "{Graph Image}"
        + "\r\n\r\n"
        + "{Graph Summary}"
        + "\r\n\r\n"
        + "For more information, go to <a href=\"http://nodexl.codeplex.com/\">"
        + "NodeXL on CodePlex</a>."
        ;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates the mode in which the dialog is being used.

    protected DialogMode m_eMode;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// NodeXLControl containing the graph.

    protected NodeXLControl m_oNodeXLControl;

    /// User settings for exporting the graph to email.

    protected ExportToEmailUserSettings m_oExportToEmailUserSettings;

    /// Password user settings.

    protected PasswordUserSettings m_oPasswordUserSettings;

    /// User settings for this dialog.

    protected ExportToEmailDialogUserSettings
        m_oExportToEmailDialogUserSettings;
}


//*****************************************************************************
//  Class: ExportToEmailDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="ExportToEmailDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ExportToEmailDialog") ]

public class ExportToEmailDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ExportToEmailDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportToEmailDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ExportToEmailDialogUserSettings
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
