
using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using Smrf.WpfGraphicsLib;
using Smrf.DateTimeLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TaskAutomator
//
/// <summary>
/// Runs multiple tasks on one workbook or a folder full of workbooks.
/// </summary>
///
/// <remarks>
/// Call <see cref="AutomateOneWorkbook" /> or <see
/// cref="AutomateOneWorkbookIndirect" /> to run a specified set of tasks on
/// one NodeXL workbook, or <see cref="AutomateFolder" /> to run them on every
/// unopened NodeXL workbook in a folder.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class TaskAutomator : Object
{
    //*************************************************************************
    //  Method: AutomateOneWorkbook()
    //
    /// <summary>
    /// Immediately runs a specified set of tasks on one NodeXL workbook, given
    /// a <see cref="ThisWorkbook" /> object.
    /// </summary>
    ///
    /// <param name="thisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl containing the graph.  The control's ActualWidth and
    /// ActualHeight properties must be at least <see
    /// cref="GraphExporterUtil.MinimumNodeXLControlWidth" /> and <see 
    /// cref="GraphExporterUtil.MinimumNodeXLControlHeight" />, respectively.
    /// </param>
    ///
    /// <param name="tasksToRun">
    /// The tasks to run, as an ORed combination of <see
    /// cref="AutomationTasks" /> flags.
    /// </param>
    ///
    /// <param name="folderToSaveWorkbookTo">
    /// If the <see cref="AutomationTasks.SaveWorkbookIfNeverSaved" /> flag is
    /// specified and the workbook has never been saved, the workbook is saved
    /// to a new file in this folder.  If this argument is null or empty, the
    /// workbook is saved to the Environment.SpecialFolder.MyDocuments folder.
    /// </param>
    ///
    /// <remarks>
    /// If a <see cref="ThisWorkbook" /> object isn't available, use <see
    /// cref="AutomateOneWorkbookIndirect" /> instead.
    /// </remarks>
    ///
    /// <exception cref="System.ArgumentException">
    /// Occurs when <paramref name="tasksToRun" /> includes <see
    /// cref="AutomationTasks.SaveGraphImageFile" /> without also including
    /// both <see cref="AutomationTasks.ReadWorkbook" /> and <see
    /// cref="AutomationTasks.SaveWorkbookIfNeverSaved" />.
    /// </exception>
    //*************************************************************************

    public static void
    AutomateOneWorkbook
    (
        ThisWorkbook thisWorkbook,
        NodeXLControl nodeXLControl,
        AutomationTasks tasksToRun,
        String folderToSaveWorkbookTo
    )
    {
        Debug.Assert(thisWorkbook != null);
        Debug.Assert(nodeXLControl != null);

        CheckTasksToRunArgument(ref tasksToRun);

        Microsoft.Office.Interop.Excel.Workbook oWorkbook =
            thisWorkbook.InnerObject;

        if
        (
            (
                ShouldRunTask(tasksToRun, AutomationTasks.MergeDuplicateEdges)
                &&
                !TryMergeDuplicateEdges(thisWorkbook)
            )
            ||
            (
                ShouldRunTask(tasksToRun, AutomationTasks.CalculateClusters)
                &&
                !TryCalculateClusters(thisWorkbook)
            )
            ||
            (
                ShouldRunTask(tasksToRun,
                    AutomationTasks.CalculateGraphMetrics)
                &&
                !TryCalculateGraphMetrics(oWorkbook)
            )
            ||
            (
                ShouldRunTask(tasksToRun, AutomationTasks.AutoFillWorkbook)
                &&
                !TryAutoFillWorkbook(thisWorkbook)
            )
            ||
            (
                ShouldRunTask(tasksToRun, AutomationTasks.CreateSubgraphImages)
                &&
                !TryCreateSubgraphImages(thisWorkbook)
            )
        )
        {
            return;
        }

        RunReadWorkbookTasks(thisWorkbook, nodeXLControl, tasksToRun,
            folderToSaveWorkbookTo);
    }

    //*************************************************************************
    //  Method: AutomateFolder()
    //
    /// <summary>
    /// Runs a specified set of tasks on every unopened NodeXL workbook in a
    /// folder.
    /// </summary>
    ///
    /// <param name="folderToAutomate">
    /// Path to the folder to automate.
    /// </param>
    ///
    /// <param name="workbookSettings">
    /// The workbook settings to use for each NodeXL workbook.  These should
    /// come from <see cref="PerWorkbookSettings.WorkbookSettings" />.
    /// </param>
    ///
    /// <remarks>
    /// The workbook settings specified by <paramref name="workbookSettings" />
    /// get stored in each NodeXL workbook.
    /// </remarks>
    //*************************************************************************

    public static void
    AutomateFolder
    (
        String folderToAutomate,
        String workbookSettings
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(folderToAutomate) );
        Debug.Assert( !String.IsNullOrEmpty(workbookSettings) );

        foreach ( String sFileName in Directory.GetFiles(folderToAutomate,
            "*.xlsx") )
        {
            String sFilePath = Path.Combine(folderToAutomate, sFileName);

            try
            {
                if (!NodeXLWorkbookUtil.FileIsNodeXLWorkbook(sFilePath))
                {
                    continue;
                }
            }
            catch (IOException)
            {
                // Skip any workbooks that are already open, or that have any
                // other problems that prevent them from being opened.

                continue;
            }

            AutomateOneWorkbookIndirect(sFilePath, workbookSettings);
        }
    }

    //*************************************************************************
    //  Method: AutomateOneWorkbookIndirect()
    //
    /// <summary>
    /// Indirectly runs a specified set of tasks on one NodeXL workbook, given
    /// a path to the workbook.
    /// </summary>
    ///
    /// <param name="nodeXLWorkbookFilePath">
    /// Path to the NodeXL workbook to automate.
    /// </param>
    ///
    /// <param name="workbookSettings">
    /// The workbook settings to use for each NodeXL workbook.  These should
    /// come from <see cref="PerWorkbookSettings.WorkbookSettings" />.
    /// </param>
    ///
    /// <remarks>
    /// This method stores an "automate tasks on open" flag in the workbook,
    /// indicating that task automation should be run on it the next time it's
    /// opened, then opens it.
    ///
    /// <para>
    /// The specified workbook settings also get stored in the workbook.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    AutomateOneWorkbookIndirect
    (
        String nodeXLWorkbookFilePath,
        String workbookSettings
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(nodeXLWorkbookFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(workbookSettings) );

        // Ideally, the Excel API would be used here to open the workbook
        // and run the AutomateOneWorkbook() method on it.  Two things
        // make that impossible:
        //
        //   1. When you open a workbook using
        //      Application.Workbooks.Open(), you get only a native Excel
        //      workbook, not an "extended" ThisWorkbook object.
        //
        //      Although a GetVstoObject() extension method is available to
        //      convert a native Excel workbook to an extended workbook,
        //      that method doesn't work on a native workbook opened via
        //      the Excel API -- it always returns null.
        //
        //      It might be possible to refactor AutomateOneWorkbook() to
        //      require only a native workbook.  However, problem 2 would
        //      still make things impossible...
        //
        //   2. If this method is being run from a modal dialog, which it
        //      is (see AutomateTasksDialog), then code in the workbook
        //      that needs to be automated doesn't run until the modal
        //      dialog closes.
        //      
        // The following code works around these problems.

        Microsoft.Office.Interop.Excel.Application oExcelApplication =
            null;

        ExcelApplicationKiller oExcelApplicationKiller = null;

        try
        {
            // Use a new Application object for each workbook.  If the same
            // Application object is reused, the memory used by each
            // workbook is never released and the machine will eventually
            // run out of memory.

            oExcelApplication =
                new Microsoft.Office.Interop.Excel.Application();

            if (oExcelApplication == null)
            {
                throw new Exception("Excel couldn't be opened.");
            }

            // ExcelApplicationKiller requires that the application be
            // visible.

            oExcelApplication.Visible = true;

            oExcelApplicationKiller = new ExcelApplicationKiller(
                oExcelApplication);

            // Store an "automate tasks on open" flag in the workbook,
            // indicating that task automation should be run on it the next
            // time it's opened.  This can be done via the Excel API.

            Microsoft.Office.Interop.Excel.Workbook oWorkbookToAutomate =
                ExcelUtil.OpenWorkbook(nodeXLWorkbookFilePath,
                oExcelApplication);

            PerWorkbookSettings oPerWorkbookSettings =
                new PerWorkbookSettings(oWorkbookToAutomate);

            oPerWorkbookSettings.WorkbookSettings = workbookSettings;
            oPerWorkbookSettings.AutomateTasksOnOpen = true;
            oWorkbookToAutomate.Save();
            oWorkbookToAutomate.Close(false, Missing.Value, Missing.Value);
            oExcelApplication.Quit();
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }
        finally
        {
            // Quitting the Excel application does not remove it from
            // memory.  Kill its process.

            oExcelApplicationKiller.KillExcelApplication();
            oExcelApplication = null;
            oExcelApplicationKiller = null;
        }

        try
        {
            // Now open the workbook in another instance of Excel, which
            // bypasses problem 2.  Code in the workbook's Ribbon will
            // detect the flag's presence, run task automation on it, close
            // the workbook, and close the other instance of Excel.

            OpenWorkbookToAutomate(nodeXLWorkbookFilePath, 60 * 60);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }
    }

    //*************************************************************************
    //  Method: OpenWorkbookToAutomate()
    //
    /// <summary>
    /// Opens a workbook that has its AutomateTasksOnOpen flag set to true,
    /// then waits for the workbook to close.
    /// </summary>
    ///
    /// <param name="workbookPath">
    /// Path to the workbook to open.
    /// </param>
    ///
    /// <param name="timeoutSeconds">
    /// Maximum time to wait for automation to complete, in seconds.
    /// </param>
    ///
    /// <returns>
    /// true if the workbook closed by itself, false if the Excel process had
    /// to be killed because the workbook was taking too long to automate.
    /// </returns>
    //*************************************************************************

    public static Boolean
    OpenWorkbookToAutomate
    (
        String workbookPath,
        Int32 timeoutSeconds
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(workbookPath) );

        Boolean bWorkbookClosedByItself = true;

        Process oProcess = Process.Start("Excel.exe",
            "\"" + workbookPath + "\"");

        if ( !oProcess.WaitForExit(timeoutSeconds * 1000) )
        {
            bWorkbookClosedByItself = false;

            try
            {
                oProcess.Kill();
                oProcess.WaitForExit();
            }
            catch
            {
                // It's possible that the process exited after WaitForExit()
                // returned, in which case the Kill() call will fail.  Ignore
                // the Kill() failure.
            }
        }

        oProcess.Close();

        return (bWorkbookClosedByItself);
    }

    //*************************************************************************
    //  Method: CheckTasksToRunArgument
    //
    /// <summary>
    /// Checks a "tasks to run" argument for validity.
    /// </summary>
    ///
    /// <param name="eTasksToRun">
    /// The tasks to check.
    /// </param>
    ///
    /// <remarks>
    /// <paramref name="eTasksToRun" /> gets modified if necessary to guarantee
    /// backward compatibility.
    /// </remarks>
    //*************************************************************************

    private static void
    CheckTasksToRunArgument
    (
        ref AutomationTasks eTasksToRun
    )
    {
        // Here are the various AutomationTasks flag dependencies:
        //
        // SaveGraphImageFile requires ReadWorkbook, because you can't save an
        // image of a graph that hasn't been shown yet.
        //
        // SaveGraphImageFile requires SaveWorkbookIfNeverSaved, because the
        // image is saved to a folder and the folder isn't determined until the
        // workbook is saved.
        //
        // ExportToNodeXLGraphGallery and ExportToEmail require ReadWorkbook,
        // because you can't export an image of a graph that hasn't been shown
        // yet.
        //
        // ExportToNodeXLGraphGallery and ExportToEmail require
        // SaveWorkbookIfNeverSaved, because the title or subject of the
        // exported graph is the workbook's file name.

        if ( ShouldRunTask(eTasksToRun,

            AutomationTasks.SaveGraphImageFile
            |
            AutomationTasks.ExportToNodeXLGraphGallery
            |
            AutomationTasks.ExportToEmail
            ) )
        {
            // The SaveWorkbookIfNeverSaved flag was introduced after the
            // SaveGraphImageFile flag, so it's possible to have an older
            // workbook that has SaveGraphImageFile set without the necessary
            // SaveWorkbookIfNeverSaved flag.  Fix this.
            //
            // The ExportToNodeXLGraphGallery and ExportToEmail flags were
            // introduced after all the others, so the dialogs that use this
            // class should ensure that ReadWorkbook and
            // SaveWorkbookIfNeverSaved are specified if
            // ExportToNodeXLGraphGallery is set.  Set it anyway, in case there
            // are any unforseen circumstances where this isn't done.

            eTasksToRun |=
                (
                AutomationTasks.ReadWorkbook
                |
                AutomationTasks.SaveWorkbookIfNeverSaved
                );
        }
    }

    //*************************************************************************
    //  Method: ShouldRunTask
    //
    /// <summary>
    /// Gets a flag specifying whether a task should be run.
    /// </summary>
    ///
    /// <param name="eTasksToRun">
    /// The tasks to run, as an ORed combination of <see
    /// cref="AutomationTasks" /> flags.
    /// </param>
    ///
    /// <param name="eTaskToTest">
    /// The task to be tested.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="eTaskToTest" /> should be run.
    /// </returns>
    //*************************************************************************

    private static Boolean
    ShouldRunTask
    (
        AutomationTasks eTasksToRun,
        AutomationTasks eTaskToTest
    )
    {
        return ( (eTasksToRun & eTaskToTest) != 0 );
    }

    //*************************************************************************
    //  Method: TryMergeDuplicateEdges()
    //
    /// <summary>
    /// Attempts to merge duplicate edges.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryMergeDuplicateEdges
    (
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oThisWorkbook != null);

        return ( RunEditableCommand(
            new RunMergeDuplicateEdgesCommandEventArgs(false),
            oThisWorkbook) );
    }

    //*************************************************************************
    //  Method: TryCalculateClusters()
    //
    /// <summary>
    /// Attempts to calculate clusters.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryCalculateClusters
    (
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oThisWorkbook != null);

        return ( RunEditableCommand(
            new RunGroupByClusterCommandEventArgs(false), oThisWorkbook) );
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate graph metrics.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryCalculateGraphMetrics
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog(null, oWorkbook);

        if (oCalculateGraphMetricsDialog.ShowDialog() != DialogResult.OK)
        {
            return (false);
        }

        if ( ( new GraphMetricUserSettings() ).ShouldCalculateGraphMetrics(
            GraphMetrics.TopNBy) )
        {
            // See the comments in GraphMetricsDialog for details on how
            // top-N-by metrics must be calculated after the other metrics
            // are calculated.

            TopNByMetricCalculator2 oTopNByMetricCalculator2 =
                new TopNByMetricCalculator2();

            oCalculateGraphMetricsDialog = new CalculateGraphMetricsDialog(
                null, oWorkbook,
                new IGraphMetricCalculator2 [] {oTopNByMetricCalculator2},
                null, true);

            if (oCalculateGraphMetricsDialog.ShowDialog() != DialogResult.OK)
            {
                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryAutoFillWorkbook()
    //
    /// <summary>
    /// Attempts to autofill the workbook.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryAutoFillWorkbook
    (
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oThisWorkbook != null);

        try
        {
            WorkbookAutoFiller.AutoFillWorkbook(
                oThisWorkbook.InnerObject, new AutoFillUserSettings() );

            oThisWorkbook.OnWorkbookAutoFilled(false);

            return (true);
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return (false);
        }
    }

    //*************************************************************************
    //  Method: TryCreateSubgraphImages()
    //
    /// <summary>
    /// Attempts to create subgraph images.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryCreateSubgraphImages
    (
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oThisWorkbook != null);

        oThisWorkbook.CreateSubgraphImages(
            CreateSubgraphImagesDialog.DialogMode.Automate);

        return (true);
    }

    //*************************************************************************
    //  Method: RunReadWorkbookTasks()
    //
    /// <summary>
    /// Runs tasks related to reading the workbook.
    /// </summary>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <param name="oNodeXLControl">
    /// NodeXLControl containing the graph.  The control's ActualWidth and
    /// ActualHeight properties must be at least <see
    /// cref="GraphExporterUtil.MinimumNodeXLControlWidth" /> and <see 
    /// cref="GraphExporterUtil.MinimumNodeXLControlHeight" />, respectively.
    /// </param>
    ///
    /// <param name="eTasksToRun">
    /// The tasks to run, as an ORed combination of <see
    /// cref="AutomationTasks" /> flags.
    /// </param>
    ///
    /// <param name="sFolderToSaveWorkbookTo">
    /// If the <see cref="AutomationTasks.SaveWorkbookIfNeverSaved" /> flag is
    /// specified and the workbook has never been saved, the workbook is saved
    /// to a new file in this folder.  If this argument is null or empty, the
    /// workbook is saved to the Environment.SpecialFolder.MyDocuments folder.
    /// </param>
    //*************************************************************************

    private static void
    RunReadWorkbookTasks
    (
        ThisWorkbook oThisWorkbook,
        NodeXLControl oNodeXLControl,
        AutomationTasks eTasksToRun,
        String sFolderToSaveWorkbookTo
    )
    {
        Debug.Assert(oThisWorkbook != null);
        Debug.Assert(oNodeXLControl != null);

        Boolean bReadWorkbook = ShouldRunTask(
            eTasksToRun, AutomationTasks.ReadWorkbook);

        Boolean bSaveWorkbookIfNeverSaved = ShouldRunTask(
            eTasksToRun, AutomationTasks.SaveWorkbookIfNeverSaved);

        Boolean bSaveGraphImageFile = ShouldRunTask(
            eTasksToRun, AutomationTasks.SaveGraphImageFile);

        Boolean bExportToNodeXLGraphGallery = ShouldRunTask(
            eTasksToRun, AutomationTasks.ExportToNodeXLGraphGallery);

        Boolean bExportToEmail = ShouldRunTask(
            eTasksToRun, AutomationTasks.ExportToEmail);

        Microsoft.Office.Interop.Excel.Workbook oWorkbook =
            oThisWorkbook.InnerObject;

        if (bReadWorkbook)
        {
            // If the vertex X and Y columns were autofilled, the layout type
            // was set to LayoutType.Null.  This will cause
            // TaskPane.ReadWorkbook() to display a warning.  Temporarily turn
            // the warning off.

            Boolean bLayoutTypeIsNullNotificationsWereEnabled =
                EnableLayoutTypeIsNullNotifications(false);

            if (
                bSaveWorkbookIfNeverSaved
                ||
                bSaveGraphImageFile
                ||
                bExportToNodeXLGraphGallery
                ||
                bExportToEmail
                )
            {
                // These tasks need to wait until the workbook is read and the
                // graph is laid out.

                EventHandler<GraphLaidOutEventArgs> oGraphLaidOutEventHandler =
                    null;

                oGraphLaidOutEventHandler =
                    delegate(Object sender, GraphLaidOutEventArgs e)
                {
                    // This delegate remains forever, even when the dialog
                    // class is destroyed.  Prevent it from being called again.

                    oThisWorkbook.GraphLaidOut -= oGraphLaidOutEventHandler;

                    if (bSaveWorkbookIfNeverSaved)
                    {
                        if ( !TrySaveWorkbookIfNeverSaved(oWorkbook,
                            sFolderToSaveWorkbookTo) )
                        {
                            return;
                        }
                    }

                    if (bSaveGraphImageFile)
                    {
                        Debug.Assert( !String.IsNullOrEmpty(
                            oThisWorkbook.Path) );

                        SaveGraphImageFile(e.NodeXLControl, e.LegendControls,
                            oThisWorkbook.FullName);
                    }

                    if (bExportToNodeXLGraphGallery)
                    {
                        if ( !TryExportToNodeXLGraphGallery(
                            oThisWorkbook.InnerObject, oNodeXLControl) )
                        {
                            return;
                        }
                    }

                    if (bExportToEmail)
                    {
                        if ( !TryExportToEmail(
                            oThisWorkbook.InnerObject, oNodeXLControl) )
                        {
                            return;
                        }
                    }
                };

                oThisWorkbook.GraphLaidOut += oGraphLaidOutEventHandler;
            }

            // Read the workbook and lay out the graph.

            CommandDispatcher.SendNoParamCommand(oThisWorkbook,
                NoParamCommand.ShowGraphAndReadWorkbook);

            EnableLayoutTypeIsNullNotifications(
                bLayoutTypeIsNullNotificationsWereEnabled);
        }
        else
        {
            if (bSaveWorkbookIfNeverSaved)
            {
                if ( !TrySaveWorkbookIfNeverSaved(oWorkbook,
                    sFolderToSaveWorkbookTo) )
                {
                    return;
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TrySaveWorkbookIfNeverSaved()
    //
    /// <summary>
    /// Attempts to save the workbook if it has never been saved.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// The NodeXL workbook to save.
    /// </param>
    ///
    /// <param name="sFolderToSaveWorkbookTo">
    /// If the workbook has never been saved, the workbook is saved to a new
    /// file in this folder.  If this argument is null or empty, the
    /// workbook is saved to the Environment.SpecialFolder.MyDocuments folder.
    /// </param>
    ///
    /// <returns>
    /// true if the workbook was saved or doesn't need to be saved, false if
    /// there was an error.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TrySaveWorkbookIfNeverSaved
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        String sFolderToSaveWorkbookTo
    )
    {
        Debug.Assert(oWorkbook != null);

        // The Workbook.Path is an empty string until the workbook is saved.

        if ( String.IsNullOrEmpty(oWorkbook.Path) )
        {
            if ( String.IsNullOrEmpty(sFolderToSaveWorkbookTo) )
            {
                sFolderToSaveWorkbookTo = Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments);
            }

            String sFileNameNoExtension;
            
            // Use a suggested file name if available; otherwise use a file
            // name based on the current time.

            if ( ( new PerWorkbookSettings(oWorkbook) ).GraphHistory
                .TryGetValue(
                    GraphHistoryKeys.ImportSuggestedFileNameNoExtension,
                    out sFileNameNoExtension) )
            {
                if (sFileNameNoExtension.Length >
                    MaximumImportSuggestedFileNameNoExtension)
                {
                    sFileNameNoExtension = sFileNameNoExtension.Substring(
                        0, MaximumImportSuggestedFileNameNoExtension);
                }

                sFileNameNoExtension = FileUtil.ReplaceIllegalFileNameChars(
                    sFileNameNoExtension, " ");
            }
            else
            {
                sFileNameNoExtension =
                    DateTimeUtil2.ToCultureInvariantFileName(DateTime.Now)
                    + " NodeXL";
            }

            try
            {
                ExcelUtil.SaveWorkbookAs(oWorkbook,
                    Path.Combine(sFolderToSaveWorkbookTo,
                        sFileNameNoExtension) );
            }
            catch (COMException)
            {
                FormUtil.ShowWarning(
                    "The workbook can't be saved, probably because the folder"
                    + " where the workbook should be saved does not exist.  To"
                    + " fix this, go to NodeXL, Graph, Automate and change the"
                    + " Options for \"Save workbook to a new file if it has"
                    + " never been saved\"."
                    );

                return (false);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SaveGraphImageFile()
    //
    /// <summary>
    /// Save an image of the graph to a file.
    /// </summary>
    ///
    /// <param name="oNodeXLControl">
    /// The control that displays the graph.
    /// </param>
    ///
    /// <param name="oLegendControls">
    /// Zero or more legend controls associated with <paramref
    /// name="oNodeXLControl" />.
    /// </param>
    ///
    /// <param name="sWorkbookFilePath">
    /// Path to the workbook file.  Sample: "C:\Workbooks\TheWorkbook.xlsx".
    /// </param>
    ///
    /// <remarks>
    /// The settings stored in the AutomatedGraphImageUserSettings class are
    /// used to save the image.
    /// </remarks>
    //*************************************************************************

    private static void
    SaveGraphImageFile
    (
        NodeXLControl oNodeXLControl,
        IEnumerable<LegendControlBase> oLegendControls,
        String sWorkbookFilePath
    )
    {
        Debug.Assert(oNodeXLControl != null);
        Debug.Assert(oLegendControls != null);
        Debug.Assert( !String.IsNullOrEmpty(sWorkbookFilePath) );

        AutomatedGraphImageUserSettings oAutomatedGraphImageUserSettings =
            new AutomatedGraphImageUserSettings();

        System.Drawing.Size oImageSizePx =
            oAutomatedGraphImageUserSettings.ImageSizePx;

        Int32 iWidth = oImageSizePx.Width;
        Int32 iHeight = oImageSizePx.Height;

        Boolean bIncludeHeader = oAutomatedGraphImageUserSettings.IncludeHeader;
        Boolean bIncludeFooter = oAutomatedGraphImageUserSettings.IncludeFooter;

        Debug.Assert(!bIncludeHeader ||
            oAutomatedGraphImageUserSettings.HeaderText != null);

        Debug.Assert(!bIncludeFooter ||
            oAutomatedGraphImageUserSettings.FooterText != null);

        GraphImageCompositor oGraphImageCompositor =
            new GraphImageCompositor(oNodeXLControl);

        UIElement oCompositeElement = oGraphImageCompositor.Composite(
            iWidth, iHeight,
            bIncludeHeader ? oAutomatedGraphImageUserSettings.HeaderText : null,
            bIncludeFooter ? oAutomatedGraphImageUserSettings.FooterText : null,
            oAutomatedGraphImageUserSettings.HeaderFooterFont, oLegendControls
            );

        System.Drawing.Bitmap oBitmap = WpfGraphicsUtil.VisualToBitmap(
            oCompositeElement, iWidth, iHeight);

        ImageFormat eImageFormat =
            oAutomatedGraphImageUserSettings.ImageFormat;

        String sImageFilePath = Path.ChangeExtension( sWorkbookFilePath,
            SaveableImageFormats.GetFileExtension(eImageFormat) );

        try
        {
            oBitmap.Save(sImageFilePath, eImageFormat);
        }
        catch (System.Runtime.InteropServices.ExternalException)
        {
            // When an image file already exists and is read-only, an
            // ExternalException is thrown.
            //
            // Note that this method is called from the
            // ThisWorkbook.GraphLaidOut event handler, so this exception can't
            // be handled by a TaskAutomator.AutomateOneWorkbook() exception
            // handler.

            FormUtil.ShowWarning( String.Format(
                "The image file \"{0}\" couldn't be saved.  Does a read-only"
                + " file with the same name already exist?"
                ,
                sImageFilePath
                ) );
        }
        finally
        {
            oBitmap.Dispose();

            oGraphImageCompositor.RestoreNodeXLControl();
        }
    }

    //*************************************************************************
    //  Method: TryExportToNodeXLGraphGallery()
    //
    /// <summary>
    /// Attempts to export the graph to the NodeXL Graph Gallery.
    /// </summary>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryExportToNodeXLGraphGallery
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        NodeXLControl oNodeXLControl
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oNodeXLControl != null);

        ExportToNodeXLGraphGalleryUserSettings
            oExportToNodeXLGraphGalleryUserSettings =
            new ExportToNodeXLGraphGalleryUserSettings();

        String sAuthor, sPassword;

        GetGraphGalleryAuthorAndPassword(
            oExportToNodeXLGraphGalleryUserSettings, out sAuthor,
            out sPassword);

        // Note that the workbook's name is used for the title, and a graph
        // summary is used for the description.

        Debug.Assert( !String.IsNullOrEmpty(oWorkbook.Name) );

        try
        {
            ( new NodeXLGraphGalleryExporter() ).ExportToNodeXLGraphGallery(
                oWorkbook,
                oNodeXLControl,
                oWorkbook.Name,
                GraphSummarizer.SummarizeGraph(oWorkbook),
                oExportToNodeXLGraphGalleryUserSettings.SpaceDelimitedTags,
                sAuthor,
                sPassword,

                oExportToNodeXLGraphGalleryUserSettings
                    .ExportWorkbookAndSettings,

                oExportToNodeXLGraphGalleryUserSettings.ExportGraphML,
                oExportToNodeXLGraphGalleryUserSettings.UseFixedAspectRatio
            );

            return (true);
        }
        catch (Exception oException)
        {
            String sMessage;

            if ( NodeXLGraphGalleryExceptionHandler
                .TryGetMessageForRecognizedException(
                    oException, out sMessage) )
            {
                FormUtil.ShowWarning(sMessage);
            }
            else
            {
                ErrorUtil.OnException(oException);
            }

            return (false);
        }
    }

    //*************************************************************************
    //  Method: TryExportToEmail()
    //
    /// <summary>
    /// Attempts to export the graph to email.
    /// </summary>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryExportToEmail
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        NodeXLControl oNodeXLControl
    )
    {
        Debug.Assert(oWorkbook != null);
        Debug.Assert(oNodeXLControl != null);

        ExportToEmailUserSettings oExportToEmailUserSettings =
            new ExportToEmailUserSettings();

        String sSmtpPassword = ( new PasswordUserSettings() ).SmtpPassword;

        if ( !ExportToEmailUserSettingsAreComplete(
            oExportToEmailUserSettings, sSmtpPassword) )
        {
            FormUtil.ShowWarning(
                "The graph can't be exported to email because all required"
                + " email options haven't been specified yet.  Go to NodeXL,"
                + " Graph, Automate to fix this."
                );

            return (false);
        }

        // Note that the workbook's name is used for the subject.

        Debug.Assert( !String.IsNullOrEmpty(oWorkbook.Name) );

        try
        {
            ( new EmailExporter() ).ExportToEmail(
                oWorkbook,
                oNodeXLControl,
                oExportToEmailUserSettings.SpaceDelimitedToAddresses.Split(' '),
                oExportToEmailUserSettings.FromAddress,
                oWorkbook.Name,
                oExportToEmailUserSettings.MessageBody,
                oExportToEmailUserSettings.SmtpHost,
                oExportToEmailUserSettings.SmtpPort,
                oExportToEmailUserSettings.UseSslForSmtp,
                oExportToEmailUserSettings.SmtpUserName,
                sSmtpPassword,
                oExportToEmailUserSettings.ExportWorkbookAndSettings,
                oExportToEmailUserSettings.ExportGraphML,
                oExportToEmailUserSettings.UseFixedAspectRatio
                );

            return (true);
        }
        catch (Exception oException)
        {
            String sMessage;

            if ( EmailExceptionHandler.TryGetMessageForRecognizedException(
                oException, out sMessage) )
            {
                FormUtil.ShowWarning(sMessage);
            }
            else
            {
                ErrorUtil.OnException(oException);
            }

            return (false);
        }
    }

    //*************************************************************************
    //  Method: GetGraphGalleryAuthorAndPassword()
    //
    /// <summary>
    /// Gets the author and password to use when exporting the graph to the
    /// NodeXL Graph Gallery.
    /// </summary>
    ///
    /// <param name="oExportToNodeXLGraphGalleryUserSettings">
    /// Stores the user's settings for exporting the graph to the NodeXL Graph
    /// Gallery.
    /// </param>
    ///
    /// <param name="sAuthor">
    /// Where the author gets stored.
    /// </param>
    ///
    /// <param name="sPassword">
    /// Where the password gets stored.
    /// </param>
    //*************************************************************************

    private static void
    GetGraphGalleryAuthorAndPassword
    (
        ExportToNodeXLGraphGalleryUserSettings
            oExportToNodeXLGraphGalleryUserSettings,

        out String sAuthor,
        out String sPassword
    )
    {
        Debug.Assert(oExportToNodeXLGraphGalleryUserSettings != null);

        // The user may have automated the graph without ever opening the
        // ExportToNodeXLGraphGalleryDialog to specify an author and password.
        // This can be worked around.
        //
        // The NodeXLGraphGalleryExporter class requires one of the following:
        //
        // 1. An author that is the name of a Graph Gallery account, along with
        //    the password for that account.
        //
        // 2. A guest author name, with a null password.

        sAuthor = oExportToNodeXLGraphGalleryUserSettings.Author;

        if ( String.IsNullOrEmpty(sAuthor) )
        {
            // The user hasn't specified an author or password yet.  Export the
            // graph as a guest.
            //
            // Note that there is nothing special about the word "Guest": there
            // is no such Graph Gallery account with that name.  Any name could
            // be used here.

            sAuthor = "Guest";
            sPassword = null;
        }
        else
        {
            // The user specified either the name of a Graph Gallery account
            // along with a password, or a guest name.  In the first case, the
            // saved password is non-empty; in the second case, it's empty.

            sPassword =
                ( new PasswordUserSettings() ).NodeXLGraphGalleryPassword;

            if (sPassword.Length == 0)
            {
                sPassword = null;
            }
        }
    }

    //*************************************************************************
    //  Method: ExportToEmailUserSettingsAreComplete()
    //
    /// <summary>
    /// Determines whether the user's settings for exporting the graph to
    /// email are complete.
    /// </summary>
    ///
    /// <param name="oExportToEmailUserSettings">
    /// Stores the user's settings for exporting the graph to email.
    /// </param>
    ///
    /// <param name="sSmtpPassword">
    /// The user's password for the SMTP server he uses to export the graph to
    /// email.  Can be empty or null.
    /// </param>
    ///
    /// <returns>
    /// true if the user settings are complete.
    /// </returns>
    //*************************************************************************

    private static Boolean
    ExportToEmailUserSettingsAreComplete
    (
        ExportToEmailUserSettings oExportToEmailUserSettings,
        String sSmtpPassword
    )
    {
        return (
            !String.IsNullOrEmpty(
                oExportToEmailUserSettings.SpaceDelimitedToAddresses)
            &&
            !String.IsNullOrEmpty(oExportToEmailUserSettings.FromAddress)
            &&
            !String.IsNullOrEmpty(oExportToEmailUserSettings.SmtpHost)
            &&
            !String.IsNullOrEmpty(oExportToEmailUserSettings.SmtpUserName)
            &&
            !String.IsNullOrEmpty(sSmtpPassword)
            );
    }

    //*************************************************************************
    //  Method: RunEditableCommand()
    //
    /// <summary>
    /// Runs a command whose event arguments are derived from <see
    /// cref="RunEditableCommandEventArgs" />.
    /// </summary>
    ///
    /// <param name="oRunEditableCommandEventArgs">
    /// Provides information for the command.
    /// </param>
    ///
    /// <param name="oThisWorkbook">
    /// The NodeXL workbook to run the tasks on.
    /// </param>
    ///
    /// <returns>
    /// true if the command was successfully run.
    /// </returns>
    //*************************************************************************

    private static Boolean
    RunEditableCommand
    (
        RunEditableCommandEventArgs oRunEditableCommandEventArgs,
        ThisWorkbook oThisWorkbook
    )
    {
        Debug.Assert(oRunEditableCommandEventArgs != null);
        Debug.Assert(oThisWorkbook != null);

        // Arbitrarily use the workbook as the sender here.  TaskAutomator
        // can't be used, because it's static.

        CommandDispatcher.SendCommand(oThisWorkbook,
            oRunEditableCommandEventArgs);

        return (oRunEditableCommandEventArgs.CommandSuccessfullyRun);
    }

    //*************************************************************************
    //  Method: EnableLayoutTypeIsNullNotifications()
    //
    /// <summary>
    /// Enables or disables notifications for "layout type is null."
    /// </summary>
    ///
    /// <param name="bNewValue">
    /// true to enable the notifications, false to disable them.
    /// </param>
    ///
    /// <returns>
    /// true if the notifications were enabled before this method was called.
    /// </returns>
    //*************************************************************************

    private static Boolean
    EnableLayoutTypeIsNullNotifications
    (
        Boolean bNewValue
    )
    {
        NotificationUserSettings oNotificationUserSettings =
            new NotificationUserSettings();

        Boolean bOldValue = oNotificationUserSettings.LayoutTypeIsNull;

        if (bNewValue != bOldValue)
        {
            oNotificationUserSettings.LayoutTypeIsNull = bNewValue;
            oNotificationUserSettings.Save();
        }

        return (bOldValue);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    // Maximum length of the file name suggested when a graph is imported.

    private const Int32 MaximumImportSuggestedFileNameNoExtension = 80;
}

}
