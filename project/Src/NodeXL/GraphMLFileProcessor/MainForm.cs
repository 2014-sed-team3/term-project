
using System;
using System.Windows.Forms;
using System.Configuration;
using System.ComponentModel;
using System.Text;
using System.Resources;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphMLFileProcessor
{
//*****************************************************************************
//  Class: MainForm
//
/// <summary>
/// The application's main form.
/// </summary>
//*****************************************************************************

public partial class MainForm : Form
{
    //*************************************************************************
    //  Constructor: MainForm()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm" /> class.
    /// </summary>
    //*************************************************************************

    public MainForm()
    {
        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_MainFormUserSettings = new MainFormUserSettings(this);

        m_GeneralUserSettings = new GeneralUserSettings();

        m_GraphMLFileProcessor = CreateGraphMLFileProcessor();

        InitializeComponent();

        DoDataExchange(false);
    }

    //*************************************************************************
    //  Method: CreateGraphMLFileProcessor()
    //
    /// <summary>
    /// Creates a GraphMLFileProcessor object and hooks up its events.
    /// </summary>
    ///
    /// <returns>
    /// A new GraphMLFileProcessor object.
    /// </returns>
    //*************************************************************************

    private GraphMLFileProcessor
    CreateGraphMLFileProcessor()
    {
        // AssertValid();

        GraphMLFileProcessor graphMLFileProcessor = new GraphMLFileProcessor();

        graphMLFileProcessor.GraphMLFileProcessingProgressChanged +=
            new ProgressChangedEventHandler(
                GraphMLFileProcessor_GraphMLFileProcessingProgressChanged);

        graphMLFileProcessor.GraphMLFileProcessingCompleted +=
            new RunWorkerCompletedEventHandler(
                GraphMLFileProcessor_GraphMLFileProcessingCompleted);

        return (graphMLFileProcessor);
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

    private Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            if ( !usrGraphMLFolderPath.Validate() )
            {
                return (false);
            }

            m_GeneralUserSettings.GraphMLFolderPath =
                usrGraphMLFolderPath.FolderPath;
        }
        else
        {
            usrGraphMLFolderPath.FolderPath =
                m_GeneralUserSettings.GraphMLFolderPath;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: AppendStatus()
    //
    /// <summary>
    /// Appends a status line to the status TextBox and to the Trace system.
    /// </summary>
    ///
    /// <param name="status">
    /// The status to append.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a CR-LF.
    /// </remarks>
    //*************************************************************************

    private void
    AppendStatus
    (
        String status
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(status) );

        status = ProcessingUtil.AppendTimestamp(status);

        // Don't trace all the "Looking for..." and "Pausing for..." messages.

        if (
            !status.StartsWith(
                GraphMLFileProcessor.LookingForGraphMLFilesMessage)
            &&
            !status.StartsWith(
                GraphMLFileProcessor.PausingMessagePrefix)
            )
        {
            Trace.WriteLine(status);
        }

        StringBuilder newStatus = new StringBuilder();
        newStatus.Append( GetStatusTextBoxText() );
        newStatus.AppendLine(status);

        txbStatus.Text = newStatus.ToString();
        txbStatus.Select(newStatus.Length - 1, 0);
        txbStatus.ScrollToCaret();
    }

    //*************************************************************************
    //  Method: GetStatusTextBoxText()
    //
    /// <summary>
    /// Gets the text from the status TextBox and removes the first lines if
    /// the text is too long.
    /// </summary>
    ///
    /// <returns>
    /// Text text in the txbStatus TextBox, with the first lines removed if
    /// necessary.
    /// </returns>
    //*************************************************************************

    private String
    GetStatusTextBoxText()
    {
        String status = txbStatus.Text;

        String [] statusLines =
            status.Split(new String[] {Environment.NewLine},
                StringSplitOptions.None);

        Int32 statusLineCount = statusLines.Length;

        // Note that due to the way String.Split() works, there is always an
        // empty string in the resulting array.

        if (statusLineCount > MaximumStatusLineCount)
        {
            // Remove the first status line.

            status = String.Join(Environment.NewLine, statusLines,
                statusLineCount - MaximumStatusLineCount,
                MaximumStatusLineCount);
        }

        return (status);
    }

    //*************************************************************************
    //  Method: ShowMessage()
    //
    /// <summary>
    /// Shows a message in a message box.
    /// </summary>
    ///
    /// <param name="message">
    /// Message to show.
    /// </param>
    //*************************************************************************

    private void
    ShowMessage
    (
        String message
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(message) );
        AssertValid();

        MessageBox.Show(message, "GraphML File Processor",
            MessageBoxButtons.OK, MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1);
    }


    //*************************************************************************
    //  Method: GraphMLFileProcessor_GraphMLFileProcessingProgressChanged()
    //
    /// <summary>
    /// Handles the GraphMLFileProcessingProgressChanged event on the
    /// GraphMLFileProcessor object.
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
    GraphMLFileProcessor_GraphMLFileProcessingProgressChanged
    (
        Object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        Debug.Assert(e.UserState is String);

        AppendStatus( (String)e.UserState );
    }

    //*************************************************************************
    //  Method: GraphMLFileProcessor_GraphMLFileProcessingCompleted()
    //
    /// <summary>
    /// Handles the GraphMLFileProcessingCompleted event on the
    /// GraphMLFileProcessor object.
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
    GraphMLFileProcessor_GraphMLFileProcessingCompleted
    (
        Object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        AppendStatus("Stopped.");

        btnStartStop.Text = "Start";
        btnStartStop.Enabled = true;
        usrGraphMLFolderPath.Enabled = true;
    }

    //*************************************************************************
    //  Method: btnStartStop_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnStartStop button.
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
    btnStartStop_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (m_GraphMLFileProcessor.IsBusy)
        {
            btnStartStop.Enabled = false;
            AppendStatus("Stopping, please wait...");

            m_GraphMLFileProcessor.CancelAsync();
        }
        else if ( DoDataExchange(true) )
        {
            btnStartStop.Text = "Stop";
            usrGraphMLFolderPath.Enabled = false;

            m_GraphMLFileProcessor.ProcessGraphMLFilesAsync(
                m_GeneralUserSettings.GraphMLFolderPath);
        }
    }

    //*************************************************************************
    //  Method: btnHelp_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnHelp button.
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
    btnHelp_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        ShowMessage(Properties.Resources.HelpMessage);
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
    //  Method: MainForm_FormClosing()
    //
    /// <summary>
    /// Handles the FormClosing event on the MainForm.
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
    MainForm_FormClosing
    (
        object sender,
        FormClosingEventArgs e
    )
    {
        AssertValid();

        if (m_GraphMLFileProcessor.IsBusy)
        {
            ShowMessage(
                "If a GraphML file is being processed, the processing might"
                + " continue after this program closes."
                );
        }

        m_GeneralUserSettings.Save();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")] 

    public void
    AssertValid()
    {
        Debug.Assert(m_MainFormUserSettings != null);
        Debug.Assert(m_GeneralUserSettings != null);
        Debug.Assert(m_GraphMLFileProcessor != null);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Maximum number of lines to allow in the status TextBox.

    private Int32 MaximumStatusLineCount = 1000;


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// User settings for this dialog.

    private MainFormUserSettings m_MainFormUserSettings;

    /// The user's general settings.

    private GeneralUserSettings m_GeneralUserSettings;

    /// Object that does all of the work.

    private GraphMLFileProcessor m_GraphMLFileProcessor;
}


//*****************************************************************************
//  Class: MainFormUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="MainForm" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("MainForm") ]

public class MainFormUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: MainFormUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="MainFormUserSettings" />
    /// class.
    /// </summary>
    ///
    /// <param name="form">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public MainFormUserSettings
    (
        Form form
    )
    : base (form, false)
    {
        Debug.Assert(form != null);

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
