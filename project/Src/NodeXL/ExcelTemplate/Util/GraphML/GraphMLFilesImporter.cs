
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMLFilesImporter
//
/// <summary>
/// Imports a set of GraphML files into a set of new NodeXL workbooks.
/// </summary>
///
/// <remarks>
/// Call <see cref="ImportGraphMLFilesAsync" /> to import the GraphML files.
/// Call <see cref="CancelAsync" /> to stop the import.  Handle the <see
/// cref="ImportationProgressChanged" /> and <see
/// cref="ImportationCompleted" /> events to monitor the progress and
/// completion of the importation.
///
/// <para>
/// If a file does not contain valid GraphML, it is skipped and added to an
/// internal list of invalid files.  After the importation completes, read the
/// <see cref="InvalidGraphMLFileNames" /> property to retrieve the names of
/// any invalid files.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphMLFilesImporter : Object
{
    //*************************************************************************
    //  Constructor: GraphMLFilesImporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMLFilesImporter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphMLFilesImporter()
    {
        m_oBackgroundWorker = null;
        m_oInvalidGraphMLFileNames = new List<String>();

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
            return (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Property: InvalidGraphMLFileNames
    //
    /// <summary>
    /// Gets the names of any files that did not contain valid GraphML, to be
    /// called only after importation completes.
    /// </summary>
    ///
    /// <value>
    /// A collection of zero or more file names, without a path.
    /// </value>
    ///
    /// <remarks>
    /// Do not use this property until after the <see
    /// cref="ImportationCompleted" /> event fires.
    /// </remarks>
    //*************************************************************************

    public ICollection<String>
    InvalidGraphMLFileNames
    {
        get
        {
            AssertValid();
            Debug.Assert(!this.IsBusy);

            return (m_oInvalidGraphMLFileNames);
        }
    }

    //*************************************************************************
    //  Method: ImportGraphMLFilesAsync()
    //
    /// <summary>
    /// Asynchronously imports a set of GraphML files into a set of new NodeXL
    /// workbooks.
    /// </summary>
    ///
    /// <param name="sourceFolderPath">
    /// Folder containing GraphML files.
    /// </param>
    ///
    /// <param name="destinationFolderPath">
    /// Folder where the new NodeXL workbooks will be saved.
    /// </param>
    ///
    /// <remarks>
    /// When importation completes, the <see cref="ImportationCompleted" />
    /// event fires.
    ///
    /// <para>
    /// To cancel the analysis, call <see cref="CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    ImportGraphMLFilesAsync
    (
        String sourceFolderPath,
        String destinationFolderPath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sourceFolderPath) );
        Debug.Assert( Directory.Exists(sourceFolderPath) );
        Debug.Assert( !String.IsNullOrEmpty(destinationFolderPath) );
        Debug.Assert( Directory.Exists(destinationFolderPath) );
        AssertValid();

        if (this.IsBusy)
        {
            throw new InvalidOperationException(
                "GraphMLFilesImporter.ImportGraphMLFilesAsync: An asynchronous"
                + " operation is already in progress."
                );
        }

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        ImportGraphMLFilesAsyncArgs oImportGraphMLFilesAsyncArgs =
            new ImportGraphMLFilesAsyncArgs();

        oImportGraphMLFilesAsyncArgs.SourceFolderPath = sourceFolderPath;

        oImportGraphMLFilesAsyncArgs.DestinationFolderPath =
            destinationFolderPath;

        // Create a BackgroundWorker and handle its events.

        m_oBackgroundWorker = new BackgroundWorker();
        m_oBackgroundWorker.WorkerReportsProgress = true;
        m_oBackgroundWorker.WorkerSupportsCancellation = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.ProgressChanged +=
            new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        m_oBackgroundWorker.RunWorkerAsync(oImportGraphMLFilesAsyncArgs);
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the importation started by <see
    /// cref="ImportGraphMLFilesAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// When the importation cancels, the <see cref="ImportationCompleted" />
    /// event fires.  The <see cref="AsyncCompletedEventArgs.Cancelled" />
    /// property will be true.
    /// </remarks>
    //*************************************************************************

    public void
    CancelAsync()
    {
        AssertValid();

        if (this.IsBusy)
        {
            m_oBackgroundWorker.CancelAsync();
        }
    }

    //*************************************************************************
    //  Event: ImportationProgressChanged
    //
    /// <summary>
    /// Occurs when progress is made during the importation started by <see
    /// cref="ImportGraphMLFilesAsync" />.
    /// </summary>
    ///
    /// <remarks>
    /// The <see cref="ProgressChangedEventArgs.UserState" /> argument is a
    /// String describing the progress.  The String is suitable for display to
    /// the user.
    /// </remarks>
    //*************************************************************************

    public event ProgressChangedEventHandler ImportationProgressChanged;


    //*************************************************************************
    //  Event: ImportationCompleted
    //
    /// <summary>
    /// Occurs when the importation started by <see
    /// cref="ImportGraphMLFilesAsync" /> completes, is cancelled, or
    /// encounters an error.
    /// </summary>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler ImportationCompleted;


    //*************************************************************************
    //  Method: ImportGraphMLFilesInternal()
    //
    /// <summary>
    /// Imports a set of GraphML files into a set of new NodeXL workbooks.
    /// </summary>
    ///
    /// <param name="oImportGraphMLFilesAsyncArgs">
    /// Contains the arguments needed to asynchronously import GraphML files.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// A BackgroundWorker object.
    /// </param>
    ///
    /// <param name="oDoWorkEventArgs">
    /// A DoWorkEventArgs object.
    /// </param>
    //*************************************************************************

    protected void
    ImportGraphMLFilesInternal
    (
        ImportGraphMLFilesAsyncArgs oImportGraphMLFilesAsyncArgs,
        BackgroundWorker oBackgroundWorker,
        DoWorkEventArgs oDoWorkEventArgs
    )
    {
        Debug.Assert(oImportGraphMLFilesAsyncArgs != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oDoWorkEventArgs != null);
        AssertValid();

        Int32 iGraphMLFiles = 0;

        foreach ( String sGraphMLFilePath in Directory.GetFiles(
            oImportGraphMLFilesAsyncArgs.SourceFolderPath, "*.graphml") )
        {
            if (oBackgroundWorker.CancellationPending)
            {
                oDoWorkEventArgs.Cancel = true;
                return;
            }

            String sGraphMLFileName = Path.GetFileName(sGraphMLFilePath);

            oBackgroundWorker.ReportProgress(0,
                String.Format(
                    "Importing \"{0}\"."
                    ,
                    sGraphMLFileName
                ) );

            XmlDocument oGraphMLDocument = new XmlDocument();

            try
            {
                oGraphMLDocument.Load(sGraphMLFilePath);
            }
            catch (XmlException oXmlException)
            {
                OnXmlException(sGraphMLFileName, oXmlException);
                continue;
            }

            String sNodeXLWorkbookPath = Path.Combine(
                oImportGraphMLFilesAsyncArgs.DestinationFolderPath,
                sGraphMLFileName
                );

            sNodeXLWorkbookPath = Path.ChangeExtension(sNodeXLWorkbookPath,
                ".xlsx");

            try
            {
                GraphMLToNodeXLWorkbookConverter.SaveGraphToNodeXLWorkbook(
                    oGraphMLDocument, sGraphMLFilePath, sNodeXLWorkbookPath,
                    null, false);
            }
            catch (XmlException oXmlException)
            {
                OnXmlException(sGraphMLFileName, oXmlException);
                continue;
            }

            iGraphMLFiles++;
        }

        oBackgroundWorker.ReportProgress(0,
            String.Format(
                "Done.  GraphML files imported: {0}."
                ,
                iGraphMLFiles
            ) );
    }

    //*************************************************************************
    //  Method: OnXmlException()
    //
    /// <summary>
    /// Handles an XmlException caught while attempting to import a GraphML
    /// file.
    /// </summary>
    ///
    /// <param name="sGraphMLFileName">
    /// The name of the invalid GraphML file, without a path.
    /// </param>
    ///
    /// <param name="oXmlException">
    /// The XmlException that was caught.
    /// </param>
    //*************************************************************************

    protected void
    OnXmlException
    (
        String sGraphMLFileName,
        XmlException oXmlException
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLFileName) );
        Debug.Assert(oXmlException != null);
        AssertValid();

        m_oInvalidGraphMLFileNames.Add( String.Format(
        
            "{0}: {1}"
            ,
            sGraphMLFileName,
            oXmlException.Message
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

    protected void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(sender is BackgroundWorker);
        AssertValid();

        Debug.Assert(e.Argument is ImportGraphMLFilesAsyncArgs);

        ImportGraphMLFilesAsyncArgs oImportGraphMLFilesAsyncArgs =
            (ImportGraphMLFilesAsyncArgs)e.Argument;

        ImportGraphMLFilesInternal(oImportGraphMLFilesAsyncArgs,
            m_oBackgroundWorker, e);
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

    protected void
    BackgroundWorker_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        ProgressChangedEventHandler oImportationProgressChanged =
            this.ImportationProgressChanged;

        if (oImportationProgressChanged != null)
        {
            oImportationProgressChanged(this, e);
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

    protected void
    BackgroundWorker_RunWorkerCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        // Forward the event.

        RunWorkerCompletedEventHandler oImportationCompleted =
            this.ImportationCompleted;

        if (oImportationCompleted != null)
        {
            oImportationCompleted(this, e);
        }

        m_oBackgroundWorker = null;
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
        // m_oBackgroundWorker
        Debug.Assert(m_oInvalidGraphMLFileNames != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Used for asynchronous importation.  null if an asynchronous importation
    /// is not in progress.

    protected BackgroundWorker m_oBackgroundWorker;

    /// The names of any files that did not contain valid GraphML, without a
    /// path.

    protected List<String> m_oInvalidGraphMLFileNames;


    //*************************************************************************
    //  Embedded class: ImportGraphMLFilesAsyncArguments()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously import GraphML files.
    /// </summary>
    //*************************************************************************

    protected class ImportGraphMLFilesAsyncArgs
    {
        ///
        public String SourceFolderPath;
        ///
        public String DestinationFolderPath;
    };
}

}
