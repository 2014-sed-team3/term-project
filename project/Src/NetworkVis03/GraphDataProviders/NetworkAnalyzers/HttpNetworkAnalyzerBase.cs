

// Define WriteRequestsToDebug to write web request details to Debug.Write().
//
// #define WriteRequestsToDebug

using System;
using System.Xml;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.SocialNetworkLib;
using Smrf.AppLib;
using Smrf.XmlLib;
using Smrf.DateTimeLib;
using Smrf.NodeXL.GraphMLLib;

namespace Smrf.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: HttpNetworkAnalyzerBase
//
/// <summary>
/// Abstract base class for classes that analyze network information obtained
/// via HTTP Web requests.
/// </summary>
///
/// <remarks>
/// This base class implements properties related to HTTP Web requests, a
/// BackgroundWorker instance, and properties, methods, and events related to
/// the BackgroundWorker.  The derived class must implement a method to start
/// an analysis and implement the <see cref="BackgroundWorker_DoWork" />
/// method.
/// </remarks>
//*****************************************************************************

public abstract class HttpNetworkAnalyzerBase : Object
{
    //*************************************************************************
    //  Constructor: HttpNetworkAnalyzerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="HttpNetworkAnalyzerBase" /> class.
    /// </summary>
    //*************************************************************************

    public HttpNetworkAnalyzerBase()
    {
        m_oBackgroundWorker = new BackgroundWorker();
        m_oBackgroundWorker.WorkerSupportsCancellation = true;
        m_oBackgroundWorker.WorkerReportsProgress = true;

        m_oBackgroundWorker.DoWork += new DoWorkEventHandler(
            BackgroundWorker_DoWork);

        m_oBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(
            BackgroundWorker_ProgressChanged);

        m_oBackgroundWorker.RunWorkerCompleted +=
            new RunWorkerCompletedEventHandler(
                BackgroundWorker_RunWorkerCompleted);

        // AssertValid();
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
            return (m_oBackgroundWorker.IsBusy);
        }
    }

    //*************************************************************************
    //  Method: CancelAsync()
    //
    /// <summary>
    /// Cancels the analysis started by an async method.
    /// </summary>
    ///
    /// <remarks>
    /// When the analysis cancels, the <see cref="AnalysisCompleted" /> event
    /// fires.  The <see cref="AsyncCompletedEventArgs.Cancelled" /> property
    /// will be true.
    ///
    /// <para>
    /// Important note: If the background thread started by an async method
    /// is running a Web request when <see cref="CancelAsync" /> is called, the
    /// cancel won't occur until the request completes.
    /// </para>
    ///
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
    //  Event: ProgressChanged
    //
    /// <summary>
    /// Occurs when progress is reported.
    /// </summary>
    //*************************************************************************

    public event ProgressChangedEventHandler ProgressChanged;


    //*************************************************************************
    //  Event: AnalysisCompleted
    //
    /// <summary>
    /// Occurs when the analysis started by an async method completes, is
    /// cancelled, or encounters an error.
    /// </summary>
    //*************************************************************************

    public event RunWorkerCompletedEventHandler AnalysisCompleted;


    //*************************************************************************
    //  Method: ExceptionToMessage()
    //
    /// <summary>
    /// Converts an exception to an error message appropriate for a user
    /// interface.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that occurred.
    /// </param>
    ///
    /// <returns>
    /// An error message appropriate for a user interface.
    /// </returns>
    //*************************************************************************

    public abstract String
    ExceptionToMessage
    (
        Exception oException
    );


    //*************************************************************************
    //  Property: ClassName
    //
    /// <summary>
    /// Gets the full name of this class.
    /// </summary>
    ///
    /// <value>
    /// The full name of this class, suitable for use in error messages.
    /// </value>
    //*************************************************************************

    protected String
    ClassName
    {
        get
        {
            return (this.GetType().FullName);
        }
    }

    //*************************************************************************
    //  Method: CheckIsBusy()
    //
    /// <summary>
    /// Throws an exception if an asynchronous operation is in progress.
    /// </summary>
    ///
    /// <param name="sMethodName">
    /// Name of the calling method.
    /// </param>
    //*************************************************************************

    protected void
    CheckIsBusy
    (
        String sMethodName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMethodName) );

        if (this.IsBusy)
        {
            throw new InvalidOperationException( String.Format(

                "{0}:{1}: An asynchronous operation is already in progress."
                ,
                this.ClassName,
                sMethodName
                ) );
        }
    }

    //*************************************************************************
    //  Method: GetXmlDocumentWithRetries()
    //
    /// <summary>
    /// Gets an XML document given an URL.  Retries after an error.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// URL to use.
    /// </param>
    ///
    /// <param name="aeHttpStatusCodesToFailImmediately">
    /// An array of status codes that should be failed immediately, or null to
    /// retry all failures.  An example is HttpStatusCode.Unauthorized (401),
    /// which Twitter returns when information about a user who has "protected"
    /// status is requested.  This should not be retried, because the retries
    /// would produce exactly the same error response.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <returns>
    /// The XmlDocument.
    /// </returns>
    ///
    /// <remarks>
    /// If the request fails and the HTTP status code is not one of the codes
    /// specified in <paramref name="aeHttpStatusCodesToFailImmediately" />,
    /// the request is retried.  If the retries also fail, an exception is
    /// thrown.
    ///
    /// <para>
    /// If the request fails with one of the HTTP status code contained in
    /// <paramref name="aeHttpStatusCodesToFailImmediately" />, an exception is
    /// thrown immediately.
    /// </para>
    ///
    /// <para>
    /// In either case, it is always up to the caller to handle the exceptions.
    /// This method never ignores an exception; it either retries it and throws
    /// it if all retries fail, or throws it immediately.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected XmlDocument
    GetXmlDocumentWithRetries
    (
        String sUrl,
        HttpStatusCode [] aeHttpStatusCodesToFailImmediately,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        Debug.Assert(oRequestStatistics != null);

        AssertValid();

        Stream oStream = null;

        try
        {
            oStream =
                HttpSocialNetworkUtil.GetHttpWebResponseStreamWithRetries(
                    sUrl, aeHttpStatusCodesToFailImmediately,
                    oRequestStatistics, UserAgent, HttpWebRequestTimeoutMs,
                    new ReportProgressHandler(this.ReportProgress),

                    new CheckCancellationPendingHandler(
                        this.CheckCancellationPending)
                    );

            XmlDocument oXmlDocument = new XmlDocument();
            oXmlDocument.Load(oStream);

            return (oXmlDocument);
        }
        finally
        {
            if (oStream != null)
            {
                oStream.Close();
            }
        }
    }

    //*************************************************************************
    //  Method: AppendStringGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a String GraphML-Attribute value to an edge or vertex XML node. 
    /// </summary>
    ///
    /// <param name="oXmlNodeToSelectFrom">
    /// Node to select from.
    /// </param>
    /// 
    /// <param name="sXPath">
    /// XPath expression to a String descendant of <paramref
    /// name="oXmlNodeToSelectFrom" />.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML ID of the attribute.
    /// </param>
    ///
    /// <returns>
    /// true if the GraphML-Attribute was appended.
    /// </returns>
    ///
    /// <remarks>
    /// This method selects from <paramref name="oXmlNodeToSelectFrom" /> using
    /// the <paramref name="sXPath" /> expression.  If the selection is
    /// successful, the specified String value gets stored on <paramref
    /// name="oEdgeOrVertexXmlNode" /> as a Graph-ML Attribute.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    AppendStringGraphMLAttributeValue
    (
        XmlNode oXmlNodeToSelectFrom,
        String sXPath,
        XmlNamespaceManager oXmlNamespaceManager,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeOrVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oXmlNodeToSelectFrom != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        String sAttributeValue;

        if ( XmlUtil2.TrySelectSingleNodeAsString(oXmlNodeToSelectFrom, sXPath,
            oXmlNamespaceManager, out sAttributeValue) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, sGraphMLAttributeID, sAttributeValue);

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: AppendInt32GraphMLAttributeValue()
    //
    /// <summary>
    /// Appends an Int32 GraphML-Attribute value to an edge or vertex XML node. 
    /// </summary>
    ///
    /// <param name="oXmlNodeToSelectFrom">
    /// Node to select from.
    /// </param>
    /// 
    /// <param name="sXPath">
    /// XPath expression to an Int32 descendant of <paramref
    /// name="oXmlNodeToSelectFrom" />.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML ID of the attribute.
    /// </param>
    ///
    /// <returns>
    /// true if the GraphML-Attribute was appended.
    /// </returns>
    ///
    /// <remarks>
    /// This method selects from <paramref name="oXmlNodeToSelectFrom" /> using
    /// the <paramref name="sXPath" /> expression.  If the selection is
    /// successful, the specified Int32 value gets stored on <paramref
    /// name="oEdgeOrVertexXmlNode" /> as a Graph-ML Attribute.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    AppendInt32GraphMLAttributeValue
    (
        XmlNode oXmlNodeToSelectFrom,
        String sXPath,
        XmlNamespaceManager oXmlNamespaceManager,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeOrVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oXmlNodeToSelectFrom != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        Int32 iAttributeValue;

        if ( XmlUtil2.TrySelectSingleNodeAsInt32(oXmlNodeToSelectFrom, sXPath,
            oXmlNamespaceManager, out iAttributeValue) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, sGraphMLAttributeID, iAttributeValue);

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: AppendDoubleGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a Double GraphML-Attribute value to an edge or vertex XML node. 
    /// </summary>
    ///
    /// <param name="oXmlNodeToSelectFrom">
    /// Node to select from.
    /// </param>
    /// 
    /// <param name="sXPath">
    /// XPath expression to a Double descendant of <paramref
    /// name="oXmlNodeToSelectFrom" />.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oEdgeOrVertexXmlNode">
    /// The edge or vertex XML node from <paramref
    /// name="oGraphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="sGraphMLAttributeID">
    /// GraphML ID of the attribute.
    /// </param>
    ///
    /// <returns>
    /// true if the GraphML-Attribute was appended.
    /// </returns>
    ///
    /// <remarks>
    /// This method selects from <paramref name="oXmlNodeToSelectFrom" /> using
    /// the <paramref name="sXPath" /> expression.  If the selection is
    /// successful, the specified Double value gets stored on <paramref
    /// name="oEdgeOrVertexXmlNode" /> as a Graph-ML Attribute.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    AppendDoubleGraphMLAttributeValue
    (
        XmlNode oXmlNodeToSelectFrom,
        String sXPath,
        XmlNamespaceManager oXmlNamespaceManager,
        GraphMLXmlDocument oGraphMLXmlDocument,
        XmlNode oEdgeOrVertexXmlNode,
        String sGraphMLAttributeID
    )
    {
        Debug.Assert(oXmlNodeToSelectFrom != null);
        Debug.Assert( !String.IsNullOrEmpty(sXPath) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oEdgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLAttributeID) );
        AssertValid();

        Double dAttributeValue;

        if ( XmlUtil2.TrySelectSingleNodeAsDouble(oXmlNodeToSelectFrom, sXPath,
            oXmlNamespaceManager, out dAttributeValue) )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oEdgeOrVertexXmlNode, sGraphMLAttributeID, dAttributeValue);

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: NetworkLevelToString()
    //
    /// <summary>
    /// Converts a <see cref="NetworkLevel" /> value to a string suitable for
    /// use in a network description.
    /// </summary>
    ///
    /// <param name="eNetworkLevel">
    /// The <see cref="NetworkLevel" /> value to convert to a string.  Sample:
    /// NetworkLevel.OnePointFive.
    /// </param>
    ///
    /// <returns>
    /// A string suitable for use in a network description.  Sample:
    /// "1.5-level".
    /// </returns>
    //*************************************************************************

    protected String
    NetworkLevelToString
    (
        NetworkLevel eNetworkLevel
    )
    {
        AssertValid();

        String sNetworkLevel = String.Empty;

        switch (eNetworkLevel)
        {
            case NetworkLevel.One:

                sNetworkLevel = "1";
                break;

            case NetworkLevel.OnePointFive:

                sNetworkLevel = "1.5";
                break;

            case NetworkLevel.Two:

                sNetworkLevel = "2";
                break;

            case NetworkLevel.TwoPointFive:

                sNetworkLevel = "2.5";
                break;

            case NetworkLevel.Three:

                sNetworkLevel = "3";
                break;

            case NetworkLevel.ThreePointFive:

                sNetworkLevel = "3.5";
                break;

            case NetworkLevel.Four:

                sNetworkLevel = "4";
                break;

            case NetworkLevel.FourPointFive:

                sNetworkLevel = "4.5";
                break;

            default:

                Debug.Assert(false);
                break;
        }

        return (sNetworkLevel + "-level");
    }

    //*************************************************************************
    //  Method: ReportProgress()
    //
    /// <summary>
    /// Reports progress.
    /// </summary>
    ///
    /// <param name="sProgressMessage">
    /// Progress message.  Can be empty but not null.
    /// </param>
    //*************************************************************************

    protected void
    ReportProgress
    (
        String sProgressMessage
    )
    {
        Debug.Assert(sProgressMessage != null);

        // This method is meant to be called when the derived class wants to
        // report progress.  It results in the
        // BackgroundWorker_ProgressChanged() method being called on the main
        // thread, which in turn fires the ProgressChanged event.

        m_oBackgroundWorker.ReportProgress(0, sProgressMessage);
    }

    //*************************************************************************
    //  Method: GetNeedToRecurse()
    //
    /// <summary>
    /// Determines whether a method getting a recursive network needs to
    /// recurse.
    /// </summary>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iRecursionLevel">
    /// Recursion level for the current call.  Must be 1 or 2.
    /// </param>
    ///
    /// <returns>
    /// true if the caller needs to recurse.
    /// </returns>
    ///
    /// <remarks>
    /// This is meant for network analyzers that analyze a recursive network.
    /// Call this from the method that uses recursion to get the different
    /// network levels, and use the return value to determine whether to
    /// recurse.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    GetNeedToRecurse
    (
        NetworkLevel eNetworkLevel,
        Int32 iRecursionLevel
    )
    {
        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        AssertValid();

        return (
            iRecursionLevel == 1
            &&
            (eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two)
            );
    }

    //*************************************************************************
    //  Method: GetNeedToAppendVertices()
    //
    /// <summary>
    /// Determines whether a method getting a recursive network needs to
    /// add vertices for a specified network and recursion level.
    /// </summary>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iRecursionLevel">
    /// Recursion level for the current call.  Must be 1 or 2.
    /// </param>
    ///
    /// <returns>
    /// true if the caller needs to add vertices for the specified network and
    /// recursion levels.
    /// </returns>
    ///
    /// <remarks>
    /// This is meant for network analyzers that analyze a recursive network.
    /// Call this from the method that uses recursion to get the different
    /// network levels, and use the return value to determine whether to add
    /// vertices for the current network and recursion levels.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    GetNeedToAppendVertices
    (
        NetworkLevel eNetworkLevel,
        Int32 iRecursionLevel
    )
    {
        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iRecursionLevel == 1 || iRecursionLevel == 2);
        AssertValid();

        return (
            (eNetworkLevel != NetworkLevel.OnePointFive ||
            iRecursionLevel == 1)
            );
    }

    //*************************************************************************
    //  Method: CheckCancellationPending()
    //
    /// <summary>
    /// Checks whether a cancellation is pending.
    /// </summary>
    ///
    /// <remarks>
    /// If an asynchronous operation is in progress and a cancellation is
    /// pending, this method throws a <see
    /// cref="CancellationPendingException" />.  When the asynchronous method
    /// catches this exception, it should set the DoWorkEventArgs.Cancel
    /// property to true and then return.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckCancellationPending()
    {
        if (m_oBackgroundWorker.IsBusy &&
            m_oBackgroundWorker.CancellationPending)
        {
            throw new CancellationPendingException();
        }
    }

    //*************************************************************************
    //  Method: FireProgressChanged()
    //
    /// <summary>
    /// Fires the ProgressChanged event if appropriate.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    FireProgressChanged
    (
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        ProgressChangedEventHandler oProgressChanged = this.ProgressChanged;

        if (oProgressChanged != null)
        {
            oProgressChanged(this, e);
        }
    }

    //*************************************************************************
    //  Method: OnNetworkObtained()
    //
    /// <summary>
    /// Call this after part or all of the network has been obtained.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="sNetworkDescription">
    /// A description of the network.
    /// </param>
    ///
    /// <param name="sNetworkTitle">
    /// A title for the network.  This becomes part of the suggested file name
    /// for the network.
    /// </param>
    ///
    /// <remarks>
    /// This method adds <paramref name="sNetworkDescription" /> to the
    /// <paramref name="oGraphMLXmlDocument" />.  Then, if the entire network
    /// has been obtained, it simply returns.  Otherwise, it throws a 
    /// PartialNetworkException.
    /// </remarks>
    //*************************************************************************

    protected void
    OnNetworkObtained
    (
        XmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics,
        String sNetworkDescription,
        String sNetworkTitle
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert( !String.IsNullOrEmpty(sNetworkDescription) );
        Debug.Assert( !String.IsNullOrEmpty(sNetworkTitle) );
        AssertValid();

        XmlNode oGraphXmlNode = XmlUtil2.SelectRequiredSingleNode(
            oGraphMLXmlDocument, "g:graphml/g:graph",

            GraphMLXmlDocument.CreateXmlNamespaceManager(
                oGraphMLXmlDocument, "g")
            );

        XmlUtil2.SetAttributes(oGraphXmlNode,
            "description", sNetworkDescription);

        String sSuggestedFileNameNoExtension = String.Format(
            "{0} NodeXL {1}"
            ,
            DateTimeUtil2.ToCultureInvariantFileName(DateTime.Now),
            sNetworkTitle
            );

        XmlUtil2.SetAttributes(oGraphXmlNode,
            "suggestedFileNameNoExtension", sSuggestedFileNameNoExtension);

        if (oRequestStatistics.UnexpectedExceptions > 0)
        {
            // The network is partial.

            throw new PartialNetworkException(oGraphMLXmlDocument,
                oRequestStatistics);
        }
    }

    //*************************************************************************
    //  Method: OnUnexpectedException()
    //
    /// <summary>
    /// Handles an exception that unexpectedly terminated the process of
    /// getting the network.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that occurred.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <remarks>
    /// This should be called only when an unexpected exception occurs,
    /// retrying the request doesn't fix it, and the process must be
    /// terminated.
    ///
    /// <para>
    /// If the user cancelled or none of the network was obtained, this method
    /// just rethrows the exception.  Otherwise, it adds the exception to
    /// <paramref name="oRequestStatistics" />, and the caller should call
    /// <see cref="OnNetworkObtained" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    OnUnexpectedException
    (
        Exception oException,
        XmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oException != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        if (
            oException is CancellationPendingException
            ||
            !GraphMLXmlDocument.GetHasVertexXmlNode(oGraphMLXmlDocument)
            )
        {
            throw (oException);
        }

        oRequestStatistics.OnUnexpectedException(oException);
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
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected abstract void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    );

    //*************************************************************************
    //  Method: BackgroundWorker_ProgressChanged()
    //
    /// <summary>
    /// Handles the ProgressChanged event on the BackgroundWorker.
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

    protected void
    BackgroundWorker_ProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        AssertValid();

        FireProgressChanged(e);
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
    /// Standard event arguments.
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

        FireProgressChanged( new ProgressChangedEventArgs(0, String.Empty) );

        // Forward the event.

        RunWorkerCompletedEventHandler oAnalysisCompleted =
            this.AnalysisCompleted;

        if (oAnalysisCompleted != null)
        {
            oAnalysisCompleted(this, e);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        Debug.Assert(m_oBackgroundWorker != null);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// User agent to use for all Web requests.

    public const String UserAgent = "Microsoft NodeXL";

    /// The timeout to use for HTTP Web requests, in milliseconds.

    public const Int32 HttpWebRequestTimeoutMs = 30000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Used for asynchronous analysis.

    protected BackgroundWorker m_oBackgroundWorker;
}

}
