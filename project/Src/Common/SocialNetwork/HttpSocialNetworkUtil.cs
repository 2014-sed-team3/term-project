

// Define WriteRequestsToDebug to write web request details to Debug.Write().
//
// #define WriteRequestsToDebug

using System;
using System.Net;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.SocialNetworkLib
{
//*****************************************************************************
//  Delegate: ReportProgressHandler
//
/// <summary>
/// Represents a method that will be called while getting a network.
/// </summary>
///
/// <param name="progress">
/// A progress message suitable for display in the UI.
/// </param>
//*****************************************************************************

public delegate void
ReportProgressHandler
(
    String progress
);


//*****************************************************************************
//  Delegate: CheckCancellationPendingHandler
//
/// <summary>
/// Represents a method that will be called while getting a network.
/// </summary>
//*****************************************************************************

public delegate void
CheckCancellationPendingHandler();


//*****************************************************************************
//  Class: HttpSocialNetworkUtil
//
/// <summary>
/// Utility methods for getting social networks via HTTP requests.
/// </summary>
//*****************************************************************************

public static class HttpSocialNetworkUtil
{
    //*************************************************************************
    //  Method: CreateHttpWebRequest()
    //
    /// <summary>
    /// Gets an HttpWebRequest object to use.
    /// </summary>
    ///
    /// <param name="url">
    /// URL to use.
    /// </param>
    ///
    /// <param name="userAgent">
    /// The user agent string to use.
    /// </param>
    ///
    /// <param name="timeoutMs">
    /// Timeout to use, in milliseconds.
    /// </param>
    ///
    /// <returns>
    /// The HttpWebRequest object.
    /// </returns>
    ///
    /// <remarks>
    /// This method handles Web proxies that requre authentication.
    /// </remarks>
    //*************************************************************************

    public static HttpWebRequest
    CreateHttpWebRequest
    (
        String url,
        String userAgent,
        Int32 timeoutMs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(url) );
        Debug.Assert( !String.IsNullOrEmpty(userAgent) );
        Debug.Assert(timeoutMs > 0);

        HttpWebRequest oHttpWebRequest =
            (HttpWebRequest)WebRequest.Create(url);

        oHttpWebRequest.UserAgent = userAgent;
        oHttpWebRequest.Timeout = timeoutMs;

        // Get the request to work if there is a Web proxy that requires
        // authentication.  More information:
        //
        // http://dangarner.co.uk/2008/03/18/webrequest-proxy-authentication/

        // Although Credentials is a static property that needs to be set once
        // only, setting it here guarantees that no Web request is ever made
        // before the credentials are set.

        WebRequest.DefaultWebProxy.Credentials =
            CredentialCache.DefaultCredentials;

        return (oHttpWebRequest);
    }

    //*************************************************************************
    //  Method: GetHttpWebResponseStreamWithRetries()
    //
    /// <summary>
    /// Gets a response stream given an URL.  Retries after an error.
    /// </summary>
    ///
    /// <param name="url">
    /// URL to use.
    /// </param>
    ///
    /// <param name="httpStatusCodesToFailImmediately">
    /// An array of status codes that should be failed immediately, or null to
    /// retry all failures.  An example is HttpStatusCode.Unauthorized (401),
    /// which Twitter returns when information about a user who has "protected"
    /// status is requested.  This should not be retried, because the retries
    /// would produce exactly the same error response.
    /// </param>
    ///
    /// <param name="requestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="userAgent">
    /// The user agent string to use.
    /// </param>
    ///
    /// <param name="timeoutMs">
    /// Timeout to use, in milliseconds.
    /// </param>
    ///
    /// <param name="reportProgressHandler">
    /// Method that will be called to report progress.  Can be null.
    /// </param>
    ///
    /// <param name="checkCancellationPendingHandler">
    /// Method that will be called to check for cancellation.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// The response stream.  The caller MUST close the stream when it is done.
    /// </returns>
    ///
    /// <remarks>
    /// If the request fails and the HTTP status code is not one of the codes
    /// specified in <paramref name="httpStatusCodesToFailImmediately" />, the
    /// request is retried.  If the retries also fail, an exception is thrown.
    ///
    /// <para>
    /// If the request fails with one of the HTTP status code contained in
    /// <paramref name="httpStatusCodesToFailImmediately" />, an exception is
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

    public static Stream
    GetHttpWebResponseStreamWithRetries
    (
        String url,
        HttpStatusCode [] httpStatusCodesToFailImmediately,
        RequestStatistics requestStatistics,
        String userAgent,
        Int32 timeoutMs,
        ReportProgressHandler reportProgressHandler,
        CheckCancellationPendingHandler checkCancellationPendingHandler
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(url) );
        Debug.Assert(requestStatistics != null);
        Debug.Assert( !String.IsNullOrEmpty(userAgent) );
        Debug.Assert(timeoutMs > 0);

        Int32 iMaximumRetries = HttpRetryDelaysSec.Length;
        Int32 iRetriesSoFar = 0;

        while (true)
        {
            if (reportProgressHandler != null && iRetriesSoFar > 0)
            {
                reportProgressHandler("Retrying request.");
            }

            // Important Note: You cannot use the same HttpWebRequest object
            // for the retries.  The object must be recreated each time.

            HttpWebRequest oHttpWebRequest = CreateHttpWebRequest(
                url, userAgent, timeoutMs);

            Stream oStream = null;

            try
            {
                if (checkCancellationPendingHandler != null)
                {
                    checkCancellationPendingHandler();
                }

                oStream = GetHttpWebResponseStreamNoRetries(oHttpWebRequest);

                if (reportProgressHandler != null && iRetriesSoFar > 0)
                {
                    reportProgressHandler("Retry succeeded, continuing...");
                }

                requestStatistics.OnSuccessfulRequest();

                return (oStream);
            }
            catch (Exception oException)
            {
                #if WriteRequestsToDebug

                Debug.WriteLine("Exception: " + oException.Message);

                #endif

                if (oStream != null)
                {
                    oStream.Close();
                }

                // The IOException "Unable to read data from the transport"
                // connection: The connection was closed." can occur when
                // requesting data from Twitter, for example.

                if ( !(
                    oException is WebException
                    ||
                    oException is IOException
                    ) )
                {
                    throw oException;
                }

                if (iRetriesSoFar == iMaximumRetries)
                {
                    requestStatistics.OnUnexpectedException(oException);

                    throw (oException);
                }

                // If the status code is one of the ones specified in
                // httpStatusCodesToFailImmediately, rethrow the exception
                // without retrying the request.

                if (httpStatusCodesToFailImmediately != null &&
                    oException is WebException)
                {
                    if ( WebExceptionHasHttpStatusCode(
                            (WebException)oException,
                            httpStatusCodesToFailImmediately) )
                    {
                        throw (oException);
                    }
                }

                Int32 iSeconds = HttpRetryDelaysSec[iRetriesSoFar];

                if (reportProgressHandler != null)
                {
                    reportProgressHandler( String.Format(

                        "Request failed, pausing {0} {1} before retrying..."
                        ,
                        iSeconds,
                        StringUtil.MakePlural("second", iSeconds)
                        ) );
                }

                System.Threading.Thread.Sleep(1000 * iSeconds);
                iRetriesSoFar++;
            }
        }
    }

    //*************************************************************************
    //  Method: WebExceptionHasHttpStatusCode()
    //
    /// <summary>
    /// Determines whether a WebException has an HttpWebResponse with one of a
    /// specified set of HttpStatusCodes.
    /// </summary>
    ///
    /// <param name="webException">
    /// The WebException to check.
    /// </param>
    ///
    /// <param name="httpStatusCodes">
    /// One or more HttpStatus codes to look for.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="webException" /> has an HttpWebResponse with
    /// an HttpStatusCode contained within <paramref name="httpStatusCodes" />.
    /// </returns>
    //*************************************************************************

    public static Boolean
    WebExceptionHasHttpStatusCode
    (
        WebException webException,
        params HttpStatusCode [] httpStatusCodes
    )
    {
        Debug.Assert(webException != null);
        Debug.Assert(httpStatusCodes != null);

        if ( !(webException.Response is HttpWebResponse) )
        {
            return (false);
        }

        HttpWebResponse oHttpWebResponse =
            (HttpWebResponse)webException.Response;

        return (Array.IndexOf<HttpStatusCode>(
            httpStatusCodes, oHttpWebResponse.StatusCode) >= 0);
    }

    //*************************************************************************
    //  Method: ExceptionIsWebOrXml()
    //
    /// <summary>
    /// Determines whether an exception is a WebException or XmlException.
    /// </summary>
    ///
    /// <param name="exception">
    /// The exception to test.
    /// </param>
    ///
    /// <returns>
    /// true if the exception is a WebException or XmlException.
    /// </returns>
    //*************************************************************************

    public static Boolean
    ExceptionIsWebOrXml
    (
        Exception exception
    )
    {
        Debug.Assert(exception != null);

        return (exception is WebException || exception is XmlException);
    }

    //*************************************************************************
    //  Method: ExceptionIsWebOrJson()
    //
    /// <summary>
    /// Determines whether an exception is a WebException or an exception
    /// thrown while parsing JSON.
    /// </summary>
    ///
    /// <param name="exception">
    /// The exception to test.
    /// </param>
    ///
    /// <returns>
    /// true if the exception is a WebException or an exception thrown by the
    /// JavaScriptSerializer class.
    /// </returns>
    //*************************************************************************

    public static Boolean
    ExceptionIsWebOrJson
    (
        Exception exception
    )
    {
        Debug.Assert(exception != null);

        return (
            exception is WebException ||
            exception is ArgumentException ||
            exception is InvalidOperationException ||
            exception is InvalidCastException
            );
    }

    //*************************************************************************
    //  Method: GetHttpWebResponseStreamNoRetries()
    //
    /// <summary>
    /// Gets a response stream given an HttpWebRequest object.  Does not retry
    /// after an error.
    /// </summary>
    ///
    /// <param name="oHttpWebRequest">
    /// HttpWebRequest object to use.
    /// </param>
    ///
    /// <returns>
    /// The response stream.  The caller MUST close the stream when it is done.
    /// </returns>
    ///
    /// <remarks>
    /// This method sets several properties on <paramref
    /// name="oHttpWebRequest" /> before it is used.
    /// </remarks>
    //*************************************************************************

    private static Stream
    GetHttpWebResponseStreamNoRetries
    (
        HttpWebRequest oHttpWebRequest
    )
    {
        Debug.Assert(oHttpWebRequest != null);

        // This is to prevent "The request was aborted: The request was
        // canceled" WebExceptions that arose for Twitter on at least one
        // user's machine, at the expense of performance.  This is not a good
        // solution, but see this posting:
        //
        // http://arnosoftwaredev.blogspot.com/2006/09/
        // net-20-httpwebrequestkeepalive-and.html

        oHttpWebRequest.KeepAlive = false;

        #if WriteRequestsToDebug

        Debug.WriteLine("\r\n\r\nURL: "
            + oHttpWebRequest.RequestUri.AbsoluteUri);

        #endif

        HttpWebResponse oHttpWebResponse =
            (HttpWebResponse)oHttpWebRequest.GetResponse();

        #if WriteRequestsToDebug

        foreach (String sKey in oHttpWebResponse.Headers.Keys)
        {
            Debug.WriteLine( String.Format(
                "Response header: {0} = {1}"
                ,
                sKey,
                oHttpWebResponse.Headers[sKey]
                ) );
        }
        #endif

        return ( oHttpWebResponse.GetResponseStream() );
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Time to wait between retries to the HTTP Web service, in seconds.  The
    /// length of the array determines the number of retries: three array
    /// elements means there will be up to three retries, or four attempts
    /// total.

    private static Int32 [] HttpRetryDelaysSec =
        new Int32 [] {1, 1, 5,};
}

}
