
using System;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.ExcelTemplate;

namespace Smrf.NodeXL.GraphMLFileProcessor
{
//*****************************************************************************
//  Class: GraphMLFileProcessor
//
/// <summary>
/// Asynchronously processes GraphML files.
/// </summary>
///
/// <remarks>
/// Processing continues in an endless loop.  It completes only when you cancel
/// it or an exception is thrown.
///
/// <para>
/// Call <see cref="ProcessGraphMLFilesAsync" /> to process GraphML files.
/// Call <see cref="CancelAsync" /> to stop the processing.  Handle the <see
/// cref="GraphMLFileProcessingProgressChanged" /> and <see
/// cref="GraphMLFileProcessingCompleted" /> events to monitor the processing
/// and cancellation.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphMLFileProcessor : Object
{
    //*************************************************************************
    //  Constructor: GraphMLFileProcessor()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMLFileProcessor" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphMLFileProcessor()
    {
        m_BackgroundWorker = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: IsBusy
    //
    /// <summary>
    /// Gets a flag indicating whether an asynchronous operation is in
    /// progress.
    /// </summary>
    ///
    /// <value>
    /// true if an asynchronous operation is in progress.
    /// </value>
    //*************************************************************************

    public Boolean
    IsBusy
    {
        get
        {
            return (m_BackgroundWorker != null && m_BackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: ProcessGraphMLFilesAsync()
    //
    /// <summary>
    /// Asynchronously processes GraphML files.
    /// </summary>
    ///
    /// <param name="graphMLFolderPath">
    /// Full path to the folder to recursively search for GraphML files.
    /// </param>
    ///
    /// <remarks>
    /// Processing continues in an endless loop.  It completes only when you
    /// cancel it or an exception is thrown.
    ///
    /// <para>
    /// To cancel the processing, call <see cref="CancelAsync" />.
    /// </para>
    ///
    /// <para>
    /// When processing completes, the <see
    /// cref="GraphMLFileProcessingCompleted" /> event fires.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ProcessGraphMLFilesAsync
    (
        String graphMLFolderPath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFolderPath) );
        AssertValid();

        if (this.IsBusy)
        {
            throw new InvalidOperationException(
                "An asynchronous operation is already in progress."
                );
        }

        ProcessGraphMLFilesAsyncArgs processGraphMLFilesAsyncArgs =
            new ProcessGraphMLFilesAsyncArgs();

        processGraphMLFilesAsyncArgs.GraphMLFolderPath = graphMLFolderPath;

        m_BackgroundWorker = CreateBackgroundWorker();
        m_BackgroundWorker.RunWorkerAsync(processGraphMLFilesAsyncArgs);
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the processing started by <see
    /// cref="ProcessGraphMLFilesAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// When the processing cancels, the <see
    /// cref="GraphMLFileProcessingCompleted" /> event fires.  The <see
    /// cref="AsyncCompletedEventArgs.Cancelled" /> property will be true.
    /// </remarks>
    //*************************************************************************

    public void
    CancelAsync()
    {
        AssertValid();

        if (this.IsBusy)
        {
            m_BackgroundWorker.CancelAsync();
        }
    }

    //*************************************************************************
    //  Event: GraphMLFileProcessingProgressChanged
    //
    /// <summary>
    /// Occurs when progress is made during the processing started by <see
    /// cref="ProcessGraphMLFilesAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="ProgressChangedEventArgs.UserState" /> argument is a
    /// String describing the progress.  The String is suitable for display to
    /// the user.
    /// </remarks>
    //*************************************************************************

    public event ProgressChangedEventHandler
        GraphMLFileProcessingProgressChanged;


    //*************************************************************************
    //  Event: GraphMLFileProcessingCompleted
    //
    /// <summary>
    /// Occurs when the processing started by <see
    /// cref="ProcessGraphMLFilesAsync" /> completes, is cancelled, or
    /// encounters an error.
    /// </summary>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler GraphMLFileProcessingCompleted;


    //*************************************************************************
    //  Method: CreateBackgroundWorker()
    //
    /// <summary>
    /// Creates a BackgroundWorker object and hooks up its events.
    /// </summary>
    ///
    /// <returns>
    /// A new BackgroundWorker object.
    /// </returns>
    //*************************************************************************

    private BackgroundWorker
    CreateBackgroundWorker()
    {
        AssertValid();

        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = true;
        backgroundWorker.WorkerSupportsCancellation = true;

        backgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        backgroundWorker.ProgressChanged +=
            new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);

        backgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        return (backgroundWorker);
    }

    //*************************************************************************
    //  Method: ProcessGraphMLFilesInternal()
    //
    /// <summary>
    /// Processes GraphML files.
    /// </summary>
    ///
    /// <param name="processGraphMLFilesAsyncArgs">
    /// Contains the arguments needed to asynchronously process GraphML files.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="doWorkEventArgs">
    /// A DoWorkEventArgs object.
    /// </param>
    //*************************************************************************

    private void
    ProcessGraphMLFilesInternal
    (
        ProcessGraphMLFilesAsyncArgs processGraphMLFilesAsyncArgs,
        BackgroundWorker backgroundWorker,
        DoWorkEventArgs doWorkEventArgs
    )
    {
        Debug.Assert(processGraphMLFilesAsyncArgs != null);
        Debug.Assert(backgroundWorker != null);
        Debug.Assert(doWorkEventArgs != null);
        AssertValid();

        while (true)
        {
            ProcessGraphMLFilesRecursive(
                processGraphMLFilesAsyncArgs.GraphMLFolderPath,
                backgroundWorker);

            Int32 sleepPeriodSeconds =
                Properties.Settings.Default.SleepPeriodSeconds;

            ReportProgress(backgroundWorker,

                String.Format(
                    "{0} {1} seconds."
                    ,
                    PausingMessagePrefix,
                    sleepPeriodSeconds
                    ) );

            while (sleepPeriodSeconds > 0)
            {
                if (backgroundWorker.CancellationPending)
                {
                    doWorkEventArgs.Cancel = true;
                    return;
                }

                Thread.Sleep(1000);
                sleepPeriodSeconds--;
            }
        }
    }

    //*************************************************************************
    //  Method: ProcessGraphMLFilesRecursive()
    //
    /// <summary>
    /// Processes GraphML files is a specified folder and subfolder.
    /// </summary>
    ///
    /// <param name="graphMLFolderPath">
    /// Full path to the folder to recursively search for GraphML files.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    //*************************************************************************

    private void
    ProcessGraphMLFilesRecursive
    (
        String graphMLFolderPath,
        BackgroundWorker backgroundWorker
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFolderPath) );
        Debug.Assert(backgroundWorker != null);
        AssertValid();

        String [] graphMLFilePaths;
        HashSet<String> workbookFilePaths;

        if ( TryGetFilePaths(graphMLFolderPath, backgroundWorker,
            out graphMLFilePaths, out workbookFilePaths) )
        {
            foreach (String graphMLFilePath in graphMLFilePaths)
            {
                if ( GraphMLFileIsNew(graphMLFilePath, workbookFilePaths) )
                {
                    ProcessGraphMLFile(graphMLFilePath, backgroundWorker);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetFilePaths()
    //
    /// <summary>
    /// Attempts to get the paths to the GraphML and workbook files found in a
    /// specified folder and its subfolders.
    /// </summary>
    ///
    /// <param name="graphMLFolderPath">
    /// Full path to the folder to recursively search for GraphML files.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="graphMLFilePaths">
    /// Where the full paths to the GraphML files get stored if true is
    /// returned.
    /// </param>
    ///
    /// <param name="workbookFilePaths">
    /// Where the full paths to the workbook files get stored, in lower case,
    /// if true is returned.
    /// </param>
    //*************************************************************************

    private Boolean
    TryGetFilePaths
    (
        String graphMLFolderPath,
        BackgroundWorker backgroundWorker,
        out String [] graphMLFilePaths,
        out HashSet<String> workbookFilePaths
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFolderPath) );
        Debug.Assert(backgroundWorker != null);
        AssertValid();

        graphMLFilePaths = null;
        workbookFilePaths = null;

        ReportProgress(backgroundWorker, LookingForGraphMLFilesMessage);

        // According to the following post, Directory.GetFiles() throws an
        // exception if it's called while a file is being written to the
        // folder.  That's exactly what the NetworkServer program may be doing,
        // so be prepared for this error.
        //
        // http://social.msdn.microsoft.com/Forums/en/csharpgeneral/thread/d41e8b32-181b-49b1-97a7-e65493f6d648

        try
        {
            graphMLFilePaths = Directory.GetFiles(graphMLFolderPath,
                "*.graphml", SearchOption.AllDirectories);

            workbookFilePaths = new HashSet<String>(
        
                from workbookFilePath in

                Directory.GetFiles(
                    graphMLFolderPath, "*.xlsx", SearchOption.AllDirectories)

                select workbookFilePath.ToLower()
                );

            return (true);
        }
        catch (Exception exception)
        {
            ReportException(backgroundWorker,

                "Couldn't get a list of GraphML files, will try again after"
                + " pausing.",

                exception
                );

            return (false);
        }
    }

    //*************************************************************************
    //  Method: GraphMLFileIsNew()
    //
    /// <summary>
    /// Determines whether a GraphML file is a new file that hasn't been't
    /// processed yet.
    /// </summary>
    ///
    /// <param name="graphMLFilePath">
    /// Full path to the GraphML file to check.
    /// </param>
    ///
    /// <param name="workbookFilePaths">
    /// Full paths to all workbook files, in lower case.
    /// </param>
    //*************************************************************************

    private Boolean
    GraphMLFileIsNew
    (
        String graphMLFilePath,
        HashSet<String> workbookFilePaths
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFilePath) );
        Debug.Assert(workbookFilePaths != null);
        AssertValid();

        // The GraphML file is new if there is no corresponding workbook file.

        String workbookFilePath =
            GetCorrespondingWorkbookFilePath(graphMLFilePath);

        return ( !workbookFilePaths.Contains( workbookFilePath.ToLower() ) );
    }

    //*************************************************************************
    //  Method: GetCorrespondingWorkbookFilePath()
    //
    /// <summary>
    /// Gets the path of a NodeXL workbook that corresponds to a GraphML file.
    /// </summary>
    ///
    /// <param name="graphMLFilePath">
    /// Full path to the GraphML file.
    /// </param>
    ///
    /// <returns>
    /// The path of the corresponding NodeXL workbook.  The workbook file may
    /// or may not exist; this method just determines the path.
    /// </returns>
    //*************************************************************************

    private String
    GetCorrespondingWorkbookFilePath
    (
        String graphMLFilePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFilePath) );
        AssertValid();

        return ( Path.ChangeExtension(graphMLFilePath, ".xlsx") );
    }

    //*************************************************************************
    //  Method: ProcessGraphMLFile()
    //
    /// <summary>
    /// Processes one new GraphML file.
    /// </summary>
    ///
    /// <param name="graphMLFilePath">
    /// Full path to the GraphML file to process.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    //*************************************************************************

    private void
    ProcessGraphMLFile
    (
        String graphMLFilePath,
        BackgroundWorker backgroundWorker
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFilePath) );
        Debug.Assert(backgroundWorker != null);
        AssertValid();

        ReportProgress(backgroundWorker,

            String.Format(
                "\r\nProcessing \"{0}\"."
                ,
                graphMLFilePath
            ) );

        XmlDocument graphMLDocument;
        String nodeXLWorkbookSettingsFilePath;
        
        if (
            !TryGetGraphMLDocument(graphMLFilePath, backgroundWorker,
                out graphMLDocument)
            ||
            !TryGetNodeXLWorkbookSettingsFilePath(graphMLFilePath,
                backgroundWorker, out nodeXLWorkbookSettingsFilePath)
            ||
            !TrySaveGraphMLToNodeXLWorkbook(graphMLFilePath, backgroundWorker,
                graphMLDocument, nodeXLWorkbookSettingsFilePath)
            ||
            !TryAutomateNodeXLWorkbook(graphMLFilePath, backgroundWorker)
            )
        {
            return;
        }
    }

    //*************************************************************************
    //  Method: TryGetGraphMLDocument()
    //
    /// <summary>
    /// Attempts to open a GraphML file.
    /// </summary>
    ///
    /// <param name="graphMLFilePath">
    /// Full path to the GraphML file to file.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="graphMLDocument">
    /// Where the GraphML document gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if an exception was thrown.
    /// </returns>
    //*************************************************************************

    private Boolean
    TryGetGraphMLDocument
    (
        String graphMLFilePath,
        BackgroundWorker backgroundWorker,
        out XmlDocument graphMLDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFilePath) );
        Debug.Assert(backgroundWorker != null);
        AssertValid();

        graphMLDocument = null;

        ReportProgress(backgroundWorker,

            String.Format(
                "Opening \"{0}\"."
                ,
                graphMLFilePath
            ) );

        try
        {
            // When the NetworkServer program creates a GraphML file, it uses
            // the FileShare.None flag.  That prevents this method from
            // erroneously reading the GraphML file while it is being written.

            using ( FileStream fileStream = File.Open(graphMLFilePath,
                FileMode.Open, FileAccess.Read, FileShare.None) )
            {
                graphMLDocument = new XmlDocument();
                graphMLDocument.Load(fileStream);

                return (true);
            }
        }
        catch (Exception exception)
        {
            ReportException(backgroundWorker,

                String.Format(
                    "Couldn't open \"{0}\"."
                    ,
                    graphMLFilePath
                    ),

                exception);

            return (false);
        }
    }

    //*************************************************************************
    //  Method: TryGetNodeXLWorkbookSettingsFilePath()
    //
    /// <summary>
    /// Attempts to get the path to the NodeXL workbook settings file
    /// corresponding to a GraphML file.
    /// </summary>
    ///
    /// <param name="graphMLFilePath">
    /// Full path to the GraphML file to process.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="nodeXLWorkbookSettingsFilePath">
    /// Where the full path to a NodeXL workbook settings file gets stored if
    /// true is returned.  Can be null, which means that there is no such
    /// settings file.
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if an exception was thrown.
    /// </returns>
    //*************************************************************************

    private Boolean
    TryGetNodeXLWorkbookSettingsFilePath
    (
        String graphMLFilePath,
        BackgroundWorker backgroundWorker,
        out String nodeXLWorkbookSettingsFilePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFilePath) );
        Debug.Assert(backgroundWorker != null);
        AssertValid();

        nodeXLWorkbookSettingsFilePath = null;

        String graphMLFolderPath = Path.GetDirectoryName(graphMLFilePath);

        // Look for a single file with a ".NodeXLOptions" extension in the
        // same folder as the GraphML file.

        try
        {
            String [] nodeXLWorkbookSettingsFilePaths = Directory.GetFiles(
                graphMLFolderPath, "*.NodeXLOptions",
                SearchOption.TopDirectoryOnly);

            Int32 settingsFileCount = nodeXLWorkbookSettingsFilePaths.Length;

            if (settingsFileCount == 0)
            {
                // There are no settings files, which is okay.

                return (true);
            }

            if (settingsFileCount == 1)
            {
                // There is one settings file, which is okay.

                nodeXLWorkbookSettingsFilePath =
                    nodeXLWorkbookSettingsFilePaths[0];

                ReportProgress(backgroundWorker,

                    String.Format(
                        "Using the NodeXL options file \"{0}\"."
                        ,
                        nodeXLWorkbookSettingsFilePath
                    ) );


                return (true);
            }

            throw new Exception( String.Format(
                "There are {0} NodeXL options files in the {1} folder.  There"
                + " can't be more than one."
                ,
                settingsFileCount,
                graphMLFolderPath
                ) );
        }
        catch (Exception exception)
        {
            ReportException(backgroundWorker,
                "Couldn't get the NodeXL options file.",
                exception);

            return (false);
        }
    }

    //*************************************************************************
    //  Method: TrySaveGraphMLToNodeXLWorkbook()
    //
    /// <summary>
    /// Attempts to save the GraphML to a new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="graphMLFilePath">
    /// Full path to the GraphML file to process.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="graphMLDocument">
    /// The GraphML document.
    /// </param>
    ///
    /// <param name="nodeXLWorkbookSettingsFilePath">
    /// The full path to a NodeXL workbook settings file, or null if there is
    /// no such settings file.
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if an exception was thrown.
    /// </returns>
    //*************************************************************************

    private Boolean
    TrySaveGraphMLToNodeXLWorkbook
    (
        String graphMLFilePath,
        BackgroundWorker backgroundWorker,
        XmlDocument graphMLDocument,
        String nodeXLWorkbookSettingsFilePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFilePath) );
        Debug.Assert(backgroundWorker != null);
        Debug.Assert(graphMLDocument != null);
        AssertValid();

        ReportProgress(backgroundWorker,

            String.Format(
                "Creating NodeXL workbook from \"{0}\"."
                ,
                graphMLFilePath
            ) );

        String workbookFilePath = GetCorrespondingWorkbookFilePath(
            graphMLFilePath);

        try
        {
            GraphMLToNodeXLWorkbookConverter.SaveGraphToNodeXLWorkbook(
                graphMLDocument, null, workbookFilePath,
                nodeXLWorkbookSettingsFilePath, true);

            return (true);
        }
        catch (Exception exception)
        {
            ReportException(backgroundWorker,

                String.Format(
                    "Couldn't create a NodeXL workbook from \"{0}\"."
                    ,
                    graphMLFilePath
                    ),

                    exception
                );

            return (false);
        }
    }

    //*************************************************************************
    //  Method: TryAutomateNodeXLWorkbook()
    //
    /// <summary>
    /// Attempts to automat the new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="graphMLFilePath">
    /// Full path to the GraphML file to process.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private Boolean
    TryAutomateNodeXLWorkbook
    (
        String graphMLFilePath,
        BackgroundWorker backgroundWorker
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphMLFilePath) );
        Debug.Assert(backgroundWorker != null);
        AssertValid();

        String workbookFilePath = GetCorrespondingWorkbookFilePath(
            graphMLFilePath);

        ReportProgress(backgroundWorker,

            String.Format(
                "Automating \"{0}\"."
                ,
                workbookFilePath
            ) );

        try
        {
            if ( !TaskAutomator.OpenWorkbookToAutomate(workbookFilePath,
                Properties.Settings.Default.AutomationTimeoutSeconds) )
            {
                throw new Exception(
                    "The automation took too long."
                    );
            }

            return (true);
        }
        catch (Exception exception)
        {
            // Remove the workbook, which will cause it to be created and
            // automated again after pausing.

            try
            {
                File.Delete(workbookFilePath);
            }
            catch(Exception)
            {
                // Ignore errors.
            }

            ReportException(backgroundWorker,

                String.Format(
                    "Couldn't automate \"{0}\"."
                    ,
                    workbookFilePath
                    ),
                    
                exception);

            return (false);
        }
    }

    //*************************************************************************
    //  Method: ReportProgress()
    //
    /// <summary>
    /// Reports progress via the <see
    /// cref="GraphMLFileProcessingProgressChanged" /> event.
    /// </summary>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="message">
    /// Progress message.
    /// </param>
    //*************************************************************************

    private void
    ReportProgress
    (
        BackgroundWorker backgroundWorker,
        String message
    )
    {
        Debug.Assert(backgroundWorker != null);
        Debug.Assert( !String.IsNullOrEmpty(message) );
        AssertValid();

        backgroundWorker.ReportProgress(0, message);
    }

    //*************************************************************************
    //  Method: ReportException()
    //
    /// <summary>
    /// Reports an exception that was caught.
    /// </summary>
    ///
    /// <param name="backgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="description">
    /// A description of the problem.
    /// </param>
    ///
    /// <param name="exception">
    /// The exception that was caught.
    /// </param>
    //*************************************************************************

    private void
    ReportException
    (
        BackgroundWorker backgroundWorker,
        String description,
        Exception exception
    )
    {
        Debug.Assert(backgroundWorker != null);
        Debug.Assert( !String.IsNullOrEmpty(description) );
        Debug.Assert(exception != null);
        AssertValid();

        ReportProgress(backgroundWorker,

            String.Format(
                "ERROR: {0}  Details:\r\n{1}"
                ,
                description,
                ExceptionUtil.GetMessageTrace(exception)
            ) );
    }

    //*************************************************************************
    //  Method: BackgroundWorker_DoWork()
    //
    /// <summary>
    /// Handles the DoWork event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    private void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        AssertValid();

        Debug.Assert(e.Argument is ProcessGraphMLFilesAsyncArgs);

        ProcessGraphMLFilesAsyncArgs processGraphMLFilesAsyncArgs =
            (ProcessGraphMLFilesAsyncArgs)e.Argument;

        ProcessGraphMLFilesInternal(processGraphMLFilesAsyncArgs,
            m_BackgroundWorker, e);
    }

    //*************************************************************************
    //  Method: BackgroundWorker_ProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    private void
    BackgroundWorker_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        ProgressChangedEventHandler graphMLFileProcessingProgressChanged =
            this.GraphMLFileProcessingProgressChanged;

        if (graphMLFileProcessingProgressChanged != null)
        {
            graphMLFileProcessingProgressChanged(this, e);
        }
    }

    //*************************************************************************
    //  Method: BackgroundWorker_RunWorkerCompleted()
    //
    /// <summary>
    /// Handles the RunWorkerCompleted event on the BackgroundWorker object.
    /// </summary>
    ///
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    ///
    /// <param name="e">
    /// Standard mouse event arguments.
    /// </param>
    //*************************************************************************

    private void
    BackgroundWorker_RunWorkerCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        RunWorkerCompletedEventHandler graphMLFileProcessingCompleted =
            this.GraphMLFileProcessingCompleted;

        if (graphMLFileProcessingCompleted != null)
        {
            graphMLFileProcessingCompleted(this, e);
        }

        m_BackgroundWorker = null;
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
        // m_BackgroundWorker
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Message saying that GraphML files are being looked for.

    public const String LookingForGraphMLFilesMessage =
        "\r\nLooking for new GraphML files.";

    /// Prefix of the message saying that processing is pausing.

    public const String PausingMessagePrefix = "Pausing for";


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Used for asynchronous analysis.  null if an asynchronous analysis is
    /// not in progress.

    private BackgroundWorker m_BackgroundWorker;


    //*************************************************************************
    //  Embedded class: ProcessGraphMLFilesAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously process GraphML files.
    /// </summary>
    //*************************************************************************

    protected class ProcessGraphMLFilesAsyncArgs
    {
        ///
        public String GraphMLFolderPath;
    };
}

}
