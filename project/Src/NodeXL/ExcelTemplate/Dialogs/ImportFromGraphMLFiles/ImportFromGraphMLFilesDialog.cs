

using System;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ImportFromGraphMLFilesDialog
//
/// <summary>
/// Imports a set of GraphML files into a set of new NodeXL workbooks.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to run the dialog.  All file
/// importation is performed within the dialog.
/// </remarks>
//*****************************************************************************

public partial class ImportFromGraphMLFilesDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ImportFromGraphMLFilesDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromGraphMLFilesDialog" /> class.
    /// </summary>
    //*************************************************************************

    public ImportFromGraphMLFilesDialog()
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oImportFromGraphMLFilesDialogUserSettings =
            new ImportFromGraphMLFilesDialogUserSettings(this);

        m_oGraphMLFilesImporter = new GraphMLFilesImporter();

        m_oGraphMLFilesImporter.ImportationProgressChanged +=
            new ProgressChangedEventHandler(
                GraphMLFilesImporter_ImportationProgressChanged);

        m_oGraphMLFilesImporter.ImportationCompleted +=
            new RunWorkerCompletedEventHandler(
                GraphMLFilesImporter_ImportationCompleted);

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
            if (
                !usrSourceFolder.Validate()
                ||
                !usrDestinationFolder.Validate()
                )
            {
                return (false);
            }

            m_oImportFromGraphMLFilesDialogUserSettings.SourceFolderPath =
                usrSourceFolder.FolderPath;

            m_oImportFromGraphMLFilesDialogUserSettings.DestinationFolderPath =
                usrDestinationFolder.FolderPath;
        }
        else
        {
            usrSourceFolder.FolderPath =
                m_oImportFromGraphMLFilesDialogUserSettings.SourceFolderPath;

            usrDestinationFolder.FolderPath =
                m_oImportFromGraphMLFilesDialogUserSettings.
                    DestinationFolderPath;

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

        pnlEnable.Enabled = btnOK.Enabled = !m_oGraphMLFilesImporter.IsBusy;
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

        if (m_oGraphMLFilesImporter.IsBusy)
        {
            // Let the background thread cancel its task, but don't try to
            // notify this dialog.

            m_oGraphMLFilesImporter.ImportationProgressChanged -=
                new ProgressChangedEventHandler(
                    GraphMLFilesImporter_ImportationProgressChanged);

            m_oGraphMLFilesImporter.ImportationCompleted -=
                new RunWorkerCompletedEventHandler(
                    GraphMLFilesImporter_ImportationCompleted);

            m_oGraphMLFilesImporter.CancelAsync();
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
        EventArgs e
    )
    {
        AssertValid();

        if (!m_oGraphMLFilesImporter.IsBusy)
        {
            if ( DoDataExchange(true) )
            {
                m_oGraphMLFilesImporter.ImportGraphMLFilesAsync(

                    m_oImportFromGraphMLFilesDialogUserSettings.
                        SourceFolderPath,

                    m_oImportFromGraphMLFilesDialogUserSettings.
                        DestinationFolderPath
                    );

                EnableControls();
            }
        }
    }

    //*************************************************************************
    //  Method: GraphMLFilesImporter_ImportationProgressChanged()
    //
    /// <summary>
    /// Handles the ImportationProgressChanged event on the
    /// GraphMLFilesImporter object.
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
    GraphMLFilesImporter_ImportationProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        Debug.Assert(e.UserState is String);
        AssertValid();

        slStatusLabel.Text = (String)e.UserState;
    }

    //*************************************************************************
    //  Method: GraphMLFilesImporter_ImportationCompleted()
    //
    /// <summary>
    /// Handles the ImportationCompleted event on the GraphMLFilesImporter
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
    GraphMLFilesImporter_ImportationCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        EnableControls();

        if (e.Cancelled)
        {
        }
        else if (e.Error != null)
        {
            Exception oException = e.Error;

            if (oException is ConvertGraphMLToNodeXLWorkbookException)
            {
                this.ShowWarning(oException.Message);
            }
            else
            {
                ErrorUtil.OnException(oException);
            }

            slStatusLabel.Text = String.Empty;
        }
        else
        {
            // The final status message is a summary of what was done.  Show it
            // in a message box in addition to the StatusLabel.

            this.ShowInformation(slStatusLabel.Text);

            CheckForInvalidGraphMLFileNames();
            this.Close();
        }
    }

    //*************************************************************************
    //  Method: CheckForInvalidGraphMLFileNames()
    //
    /// <summary>
    /// Checks whether any of the files contained invalid GraphML.
    /// </summary>
    //*************************************************************************

    protected void
    CheckForInvalidGraphMLFileNames()
    {
        AssertValid();

        ICollection<String> oInvalidGraphMLFileNames = 
            m_oGraphMLFilesImporter.InvalidGraphMLFileNames;

        Int32 iInvalidGraphMLFileNames = oInvalidGraphMLFileNames.Count;

        if (iInvalidGraphMLFileNames > 0)
        {
            if (MessageBox.Show(

                    iInvalidGraphMLFileNames.ToString() + " of the files did"
                        + " not contain valid GraphML.  Do you want to copy"
                        + " the names of the invalid files to the Clipboard?",
            
                    ApplicationUtil.ApplicationName,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    )
                == DialogResult.Yes)
            {
                Clipboard.SetText( String.Join("\r\n",
                    oInvalidGraphMLFileNames.ToArray() ) );
            }
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

        Debug.Assert(m_oImportFromGraphMLFilesDialogUserSettings != null);
        Debug.Assert(m_oGraphMLFilesImporter != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected ImportFromGraphMLFilesDialogUserSettings
        m_oImportFromGraphMLFilesDialogUserSettings;

    /// Object that does most of the work.

    protected GraphMLFilesImporter m_oGraphMLFilesImporter;
}


//*****************************************************************************
//  Class: ImportFromGraphMLFilesDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ImportFromGraphMLFilesDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ImportFromGraphMLFilesDialog") ]

public class ImportFromGraphMLFilesDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ImportFromGraphMLFilesDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ImportFromGraphMLFilesDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ImportFromGraphMLFilesDialogUserSettings
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
    //  Property: SourceFolderPath
    //
    /// <summary>
    /// Gets or sets the path of the source folder.
    /// </summary>
    ///
    /// <value>
    /// The path of the source folder.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SourceFolderPath
    {
        get
        {
            AssertValid();

            return ( (String)this[SourceFolderPathKey] );
        }

        set
        {
            this[SourceFolderPathKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DestinationFolderPath
    //
    /// <summary>
    /// Gets or sets the path of the destination folder.
    /// </summary>
    ///
    /// <value>
    /// The path of the destination folder.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    DestinationFolderPath
    {
        get
        {
            AssertValid();

            return ( (String)this[DestinationFolderPathKey] );
        }

        set
        {
            this[DestinationFolderPathKey] = value;

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

    /// Name of the settings key for the SourceFolderPath property.

    protected const String SourceFolderPathKey = "SourceFolderPath";

    /// Name of the settings key for the DestinationFolderPath property.

    protected const String DestinationFolderPathKey = "DestinationFolderPath";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
