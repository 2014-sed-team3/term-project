

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutomateTasksDialog
//
/// <summary>
/// Edits a <see cref="AutomateTasksUserSettings" /> object and optionally runs
/// the tasks specified in the object.
/// </summary>
///
/// <remarks>
/// Unlike other dialogs in this application, you do not pass a user settings
/// object to the constructor.  That's because when automating a folder, this
/// class must update the user settings before opening other workbooks, so
/// those other workbooks will have access to the updated settings.
///
/// <para>
/// The <see cref="Form.ShowDialog()" /> return value does indicate whether the
/// settings were updated: it returns DialogResult.OK if they were edited, or
/// DialogResult.Cancel if they were not.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class AutomateTasksDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AutomateTasksDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="AutomateTasksDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the dialog is being used.
    /// </param>
    ///
    /// <param name="thisWorkbook">
    /// Workbook containing the graph contents.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// The NodeXLControl object.  This can be null if <paramref name="mode" />
    /// is <see cref="DialogMode.EditOnly" />.
    /// </param>
    //*************************************************************************

    public AutomateTasksDialog
    (
        DialogMode mode,
        ThisWorkbook thisWorkbook,
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(thisWorkbook != null);
        Debug.Assert(nodeXLControl != null || mode == DialogMode.EditOnly);

        m_eMode = mode;
        m_oAutomateTasksUserSettings = new AutomateTasksUserSettings();
        m_oThisWorkbook = thisWorkbook;
        m_oNodeXLControl = nodeXLControl;
        m_bIgnoreItemCheckEvents = false;

        InitializeComponent();

        if (m_eMode == DialogMode.EditOnly)
        {
            this.Text += " Options";
            btnOK.Text = "OK";
        }

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oAutomateTasksDialogUserSettings =
            new AutomateTasksDialogUserSettings(this);

        PopulateTasksToRun();

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
        /// The user can edit the automate tasks user settings and then run the
        /// tasks.

        Normal,

        /// The user can edit the automate tasks user settings but cannot run
        /// the tasks.

        EditOnly,
    }

    //*************************************************************************
    //  Method: PopulateTasksToRun()
    //
    /// <summary>
    /// Populates the clbTasksToRun CheckedListBox.
    /// </summary>
    //*************************************************************************

    protected void
    PopulateTasksToRun()
    {
        // AssertValid();

        clbTasksToRun.PopulateWithObjectsAndText(

            AutomationTasks.MergeDuplicateEdges, 
            "Count and merge duplicate edges",

            AutomationTasks.CalculateClusters, 
            "Group by cluster",

            AutomationTasks.CalculateGraphMetrics, 
            "Graph metrics",

            AutomationTasks.AutoFillWorkbook, 
            "Autofill columns",

            AutomationTasks.CreateSubgraphImages, 
            "Subgraph images",

            AutomationTasks.ReadWorkbook, 
            "Show graph",

            AutomationTasks.SaveWorkbookIfNeverSaved, 
            "Save workbook to a new file if it has never been saved",

            AutomationTasks.SaveGraphImageFile, 
            "Save image to file",

            AutomationTasks.ExportToNodeXLGraphGallery, 
            "Export graph to NodeXL Graph Gallery",

            AutomationTasks.ExportToEmail, 
            "Export graph to email"
            );
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
        AssertValid();

        if (bFromControls)
        {
            AutomationTasks eTasksToRun = GetTasksToRun();

            if (eTasksToRun == AutomationTasks.None)
            {
                this.ShowWarning("No tasks have been selected.");
                return (false);
            }

            Boolean bAutomateThisWorkbookOnly =
                radAutomateThisWorkbookOnly.Checked;

            if (
                !bAutomateThisWorkbookOnly
                &&
                !usrFolderToAutomate.Validate()
                )
            {
                return (false);
            }

            m_oAutomateTasksUserSettings.TasksToRun = eTasksToRun;

            m_oAutomateTasksUserSettings.AutomateThisWorkbookOnly =
                bAutomateThisWorkbookOnly;

            m_oAutomateTasksUserSettings.FolderToAutomate =
                usrFolderToAutomate.FolderPath;
        }
        else
        {
            SetTasksToRun(m_oAutomateTasksUserSettings.TasksToRun);

            if (m_oAutomateTasksUserSettings.AutomateThisWorkbookOnly)
            {
                radAutomateThisWorkbookOnly.Checked = true;
            }
            else
            {
                radAutomateFolder.Checked = true;
            }

            usrFolderToAutomate.FolderPath =
                m_oAutomateTasksUserSettings.FolderToAutomate;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SetTasksToRun()
    //
    /// <summary>
    /// Checks the tasks to run in the clbTasksToRun CheckListBox.
    /// </summary>
    ///
    /// <param name="eTasksToRun">
    /// The tasks to run.
    /// </param>
    //*************************************************************************

    protected void
    SetTasksToRun
    (
        AutomationTasks eTasksToRun
    )
    {
        AssertValid();

        CheckedListBox.ObjectCollection oItems = clbTasksToRun.Items;
        Int32 iItems = oItems.Count;
        m_bIgnoreItemCheckEvents = true;

        for (Int32 i = 0; i < iItems; i++)
        {
            clbTasksToRun.SetItemChecked(i, 
                ( eTasksToRun & ItemToAutomationTask( oItems[i] ) ) != 0);
        }

        m_bIgnoreItemCheckEvents = false;
    }

    //*************************************************************************
    //  Method: GetTasksToRun()
    //
    /// <summary>
    /// Gets the checked tasks in the clbTasksToRun CheckListBox.
    /// </summary>
    ///
    /// <returns>
    /// The checked tasks to run.
    /// </returns>
    //*************************************************************************

    protected AutomationTasks
    GetTasksToRun()
    {
        AssertValid();

        AutomationTasks eTasksToRun = AutomationTasks.None;

        foreach (Object oCheckedItem in clbTasksToRun.CheckedItems)
        {
            eTasksToRun |= ItemToAutomationTask(oCheckedItem);
        }

        return (eTasksToRun);
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

        EnableControls(radAutomateFolder.Checked,
            usrFolderToAutomate, lblNote);
    }

    //*************************************************************************
    //  Method: ItemToAutomationTask()
    //
    /// <summary>
    /// Casts an item from the clbTasksToRun CheckedListBox to a value in the
    /// <see cref="AutomationTasks" /> enumeration.
    /// </summary>
    ///
    /// <param name="oCheckedListBoxItem">
    /// An item from the clbTasksToRun CheckedListBox.
    /// </param>
    ///
    /// <returns>
    /// The <see cref="AutomationTasks" /> value represented by <paramref
    /// name="oCheckedListBoxItem" />.
    /// </returns>
    //*************************************************************************

    protected AutomationTasks
    ItemToAutomationTask
    (
        Object oCheckedListBoxItem
    )
    {
        Debug.Assert(oCheckedListBoxItem != null);
        AssertValid();

        Debug.Assert(oCheckedListBoxItem is ObjectWithText);

        Debug.Assert( ( (ObjectWithText)oCheckedListBoxItem ).Object
            is AutomationTasks );

        return ( (AutomationTasks)
            ( (ObjectWithText)oCheckedListBoxItem ).Object );
    }

    //*************************************************************************
    //  Method: EditFolderToSaveWorkbookTo()
    //
    /// <summary>
    /// Lets the user edit the folder to save the workbook to.
    /// </summary>
    //*************************************************************************

    protected void
    EditFolderToSaveWorkbookTo()
    {
        AssertValid();

        FolderBrowserDialog oFolderBrowserDialog = new FolderBrowserDialog();

        oFolderBrowserDialog.Description =
            "Select the folder in which the new workbook file will be saved.";

        String sFolderToSaveWorkbookTo =
            m_oAutomateTasksUserSettings.FolderToSaveWorkbookTo;

        if ( String.IsNullOrEmpty(sFolderToSaveWorkbookTo) )
        {
            // If the folder is null or empty, the user's MyDocuments folder is
            // used.

            sFolderToSaveWorkbookTo = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
        }

        oFolderBrowserDialog.SelectedPath = sFolderToSaveWorkbookTo;

        if (oFolderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            m_oAutomateTasksUserSettings.FolderToSaveWorkbookTo =
                oFolderBrowserDialog.SelectedPath;
        }
    }

    //*************************************************************************
    //  Method: OnCheckOrUncheckAll()
    //
    /// <summary>
    /// Handles the Click event on the btnCheckAll and btnUncheckAll buttons.
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
    OnCheckOrUncheckAll
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        m_bIgnoreItemCheckEvents = true;
        clbTasksToRun.SetAllItemsChecked(sender == btnCheckAll);
        m_bIgnoreItemCheckEvents = false;
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
    //  Method: clbTasksToRun_ItemCheck()
    //
    /// <summary>
    /// Handles the ItemCheck event on the clbTasksToRun CheckedListBox.
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
    clbTasksToRun_ItemCheck
    (
        object sender,
        ItemCheckEventArgs e
    )
    {
        // AssertValid();

        if (!m_bIgnoreItemCheckEvents)
        {
            // Some tasks are dependent on other tasks.  Automatically check or
            // uncheck tasks as appropriate.

            AutomationTasks eTasksToRun = GetTasksToRun();
            Boolean bItemChecked = (e.NewValue == CheckState.Checked);

            switch ( ItemToAutomationTask( clbTasksToRun.Items[e.Index] ) )
            {
                case AutomationTasks.SaveGraphImageFile:
                case AutomationTasks.ExportToNodeXLGraphGallery:
                case AutomationTasks.ExportToEmail:

                    if (bItemChecked)
                    {
                        eTasksToRun |= (
                            AutomationTasks.ReadWorkbook
                            |
                            AutomationTasks.SaveWorkbookIfNeverSaved
                            );
                    }

                    break;

                case AutomationTasks.ReadWorkbook:
                case AutomationTasks.SaveWorkbookIfNeverSaved:

                    if (!bItemChecked)
                    {
                        eTasksToRun &= ~(
                            AutomationTasks.SaveGraphImageFile
                            |
                            AutomationTasks.ExportToNodeXLGraphGallery
                            |
                            AutomationTasks.ExportToEmail
                            );
                    }

                    break;

                default:

                    break;
            }

            SetTasksToRun(eTasksToRun);
        }
    }

    //*************************************************************************
    //  Method: btnOptions_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOptions button.
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
    btnOptions_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // These are shared by several cases below.

        Form oUserSettingsDialog = null;
        NodeXLApplicationSettingsBase oUserSettings = null;

        switch ( (AutomationTasks)clbTasksToRun.SelectedValue )
        {
            case AutomationTasks.MergeDuplicateEdges:

                oUserSettings = new MergeDuplicateEdgesUserSettings();

                oUserSettingsDialog =
                    new MergeDuplicateEdgesUserSettingsDialog(
                    MergeDuplicateEdgesUserSettingsDialog.DialogMode.EditOnly,
                    (MergeDuplicateEdgesUserSettings)oUserSettings,
                    m_oThisWorkbook.InnerObject);

                break;

            case AutomationTasks.CalculateClusters:

                oUserSettings = new ClusterUserSettings();

                oUserSettingsDialog = new ClusterUserSettingsDialog(
                    ClusterUserSettingsDialog.DialogMode.EditOnly,
                    (ClusterUserSettings)oUserSettings);

                break;

            case AutomationTasks.CalculateGraphMetrics:

                ( new GraphMetricsDialog(
                    GraphMetricsDialog.DialogMode.EditOnly,
                    m_oThisWorkbook.InnerObject) ).ShowDialog();

                break;

            case AutomationTasks.AutoFillWorkbook:

                // ThisWorkbook manages the modeless AutoFillWorkbookDialog, so
                // tell ThisWorkbook to open the dialog.

                m_oThisWorkbook.AutoFillWorkbook(
                    AutoFillWorkbookDialog.DialogMode.EditOnly);

                break;

            case AutomationTasks.CreateSubgraphImages:

                ( new CreateSubgraphImagesDialog(
                    CreateSubgraphImagesDialog.DialogMode.EditOnly, null,
                    null) ).ShowDialog();

                break;

            case AutomationTasks.ReadWorkbook:

                oUserSettings = new GeneralUserSettings();

                oUserSettingsDialog = new GeneralUserSettingsDialog(
                    (GeneralUserSettings)oUserSettings,
                    m_oThisWorkbook.InnerObject);

                break;

            case AutomationTasks.SaveWorkbookIfNeverSaved:

                EditFolderToSaveWorkbookTo();

                break;

            case AutomationTasks.SaveGraphImageFile:

                oUserSettings = new AutomatedGraphImageUserSettings();

                oUserSettingsDialog =
                    new AutomatedGraphImageUserSettingsDialog(
                    (AutomatedGraphImageUserSettings)oUserSettings);

                break;

            case AutomationTasks.ExportToNodeXLGraphGallery:

                ( new ExportToNodeXLGraphGalleryDialog(
                    ExportToNodeXLGraphGalleryDialog.DialogMode.EditOnly,
                    m_oThisWorkbook.InnerObject, null) ).ShowDialog();

                break;

            case AutomationTasks.ExportToEmail:

                ( new ExportToEmailDialog(
                    ExportToEmailDialog.DialogMode.EditOnly,
                    m_oThisWorkbook.InnerObject, null) ).ShowDialog();

                break;

            default:

                Debug.Assert(false);
                break;
        }

        if (oUserSettingsDialog != null)
        {
            Debug.Assert(oUserSettings != null);

            if (oUserSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                oUserSettings.Save();
            }
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
        System.EventArgs e
    )
    {
        if ( !DoDataExchange(true) )
        {
            return;
        }

        m_oAutomateTasksUserSettings.Save();

        try
        {
            if (m_eMode == DialogMode.EditOnly)
            {
                // (Just close the dialog.)
            }
            else if (m_oAutomateTasksUserSettings.AutomateThisWorkbookOnly)
            {
                Debug.Assert(m_oNodeXLControl != null);

                TaskAutomator.AutomateOneWorkbook(m_oThisWorkbook,
                    m_oNodeXLControl, m_oAutomateTasksUserSettings.TasksToRun,
                    m_oAutomateTasksUserSettings.FolderToSaveWorkbookTo);
            }
            else
            {
                // The user settings for this workbook will be used for and
                // stored in each workbook in the specified folder.

                CommandDispatcher.SendCommand( this,
                    new RunNoParamCommandEventArgs(
                        NoParamCommand.SaveUserSettings) );

                String sWorkbookSettings = ( new PerWorkbookSettings(
                    m_oThisWorkbook.InnerObject) ).WorkbookSettings;

                TaskAutomator.AutomateFolder(
                    m_oAutomateTasksUserSettings.FolderToAutomate,
                    sWorkbookSettings);
            }
        }
        catch (UnauthorizedAccessException oUnauthorizedAccessException)
        {
            // This occurs when a workbook is read-only.

            this.ShowWarning(
                "A problem occurred while running tasks.  Details:"
                + "\r\n\r\n"
                + oUnauthorizedAccessException.Message
                );

            return;
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }

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
        Debug.Assert(m_oAutomateTasksUserSettings != null);
        Debug.Assert(m_oThisWorkbook != null);

        Debug.Assert(m_oNodeXLControl != null ||
            m_eMode == DialogMode.EditOnly);

        // m_bIgnoreItemCheckEvents
        Debug.Assert(m_oAutomateTasksDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates the mode in which the dialog is being used.

    protected DialogMode m_eMode;

    /// Object whose properties are being edited.

    protected AutomateTasksUserSettings m_oAutomateTasksUserSettings;

    /// Workbook containing the graph contents.

    protected ThisWorkbook m_oThisWorkbook;

    /// The NodeXLControl object.  Can be null if m_eMode is EditOnly.

    protected NodeXLControl m_oNodeXLControl;

    /// true to ignore clbTasksToRun_ItemCheck events.

    protected Boolean m_bIgnoreItemCheckEvents;

    /// User settings for this dialog.

    protected AutomateTasksDialogUserSettings
        m_oAutomateTasksDialogUserSettings;
}


//*****************************************************************************
//  Class: AutomateTasksDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="AutomateTasksDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AutomateTasksDialog2") ]

public class AutomateTasksDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AutomateTasksDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutomateTasksDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AutomateTasksDialogUserSettings
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
