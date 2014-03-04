
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Diagnostics;

namespace Smrf.SocialNetworkLib.Twitter
{
//*****************************************************************************
//  Class: TwitterUtil
//
/// <summary>
/// Provides utility methods for getting social networks from Twitter.
/// </summary>
//*****************************************************************************

public class TwitterUtil
{
    //*************************************************************************
    //  Constructor: TwitterUtil()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterUtil" /> class.
    /// </summary>
    ///
    /// <param name="twitterAccessToken">
    /// The Twitter access token to use.
    /// </param>
    ///
    /// <param name="twitterAccessTokenSecret">
    /// The Twitter access token secret to use.
    /// </param>
    ///
    /// <param name="userAgent">
    /// The user agent string to use for web requests.
    /// </param>
    ///
    /// <param name="timeoutMs">
    /// Timeout to use for web requests, in milliseconds.
    /// </param>
    //*************************************************************************

    public TwitterUtil
    (
        String twitterAccessToken,
        String twitterAccessTokenSecret,
        String userAgent,
        Int32 timeoutMs
    )
    {
        m_TwitterAccessToken = twitterAccessToken;
        m_TwitterAccessTokenSecret = twitterAccessTokenSecret;
        m_UserAgent = userAgent;
        m_TimeoutMs = timeoutMs;

        AssertValid();
    }

    //*************************************************************************
    //  Method: EnumerateSearchStatuses()
    //
    /// <summary>
    /// Enumerates through the statuses that include a specified search term.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="maximumStatuses">
    /// Maximum number of tweets to request.  Can't be Int32.MaxValue.
    /// </param>
    ///
    /// <param name="requestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
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
    /// A Dictionary for each status, returned one by one.  The dictionary keys
    /// are names and the dictionary values are the named values.
    /// </returns>
    //*************************************************************************

    public IEnumerable< Dictionary<String, Object> >
    EnumerateSearchStatuses
    (
        String searchTerm,
        Int32 maximumStatuses,
        RequestStatistics requestStatistics,
        ReportProgressHandler reportProgressHandler,
        CheckCancellationPendingHandler checkCancellationPendingHandler
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumStatuses > 0);
        Debug.Assert(maximumStatuses != Int32.MaxValue);
        Debug.Assert(requestStatistics != null);
        AssertValid();

        Int32 iPage = 1;
        Int32 iStatusesEnumerated = 0;
        String sQueryParametersForNextPage = null;

        while (true)
        {
            if (reportProgressHandler != null && iPage > 1)
            {
                reportProgressHandler("Getting page " + iPage + ".");
            }

            Dictionary<String, Object> oResponseDictionary = null;
            Object [] aoStatusesThisPage;

            String sUrl = GetSearchUrl(searchTerm,
                sQueryParametersForNextPage);

            try
            {
                Object oDeserializedTwitterResponse = 
                    ( new JavaScriptSerializer() ).DeserializeObject(
                        GetTwitterResponseAsString(
                            sUrl, requestStatistics, reportProgressHandler,
                            checkCancellationPendingHandler) );

                // The top level of the Json response contains a set of
                // name/value pairs.  The value for the "statuses" name is the
                // array of objects this method will enumerate.

                oResponseDictionary = ( Dictionary<String, Object> )
                    oDeserializedTwitterResponse;

                aoStatusesThisPage =
                    ( Object [] )oResponseDictionary["statuses"];
            }
            catch (Exception oException)
            {
                // Rethrow the exception if appropriate.

                OnExceptionWhileEnumeratingJsonValues(oException, iPage,
                    false);

                // Otherwise, just halt the enumeration.

                yield break;
            }

            Int32 iObjectsThisPage = aoStatusesThisPage.Length;

            if (iObjectsThisPage == 0)
            {
                yield break;
            }

            for (Int32 i = 0; i < iObjectsThisPage; i++)
            {
                yield return (
                    ( Dictionary<String, Object> )aoStatusesThisPage[i] );

                iStatusesEnumerated++;

                if (iStatusesEnumerated == maximumStatuses)
                {
                    yield break;
                }
            }

            iPage++;

            if ( !TryGetQueryParametersForNextSearchPage(oResponseDictionary,
                out sQueryParametersForNextPage) )
            {
                yield break;
            }

            // Get the next page...
        }
    }

    //*************************************************************************
    //  Method: EnumerateUserValueDictionaries()
    //
    /// <summary>
    /// Enumerates through a collection of dictionaries of values for a
    /// collection of specified users.
    /// </summary>
    ///
    /// <param name="userIDsOrScreenNames">
    /// Array of user IDs or screen names to get user value dictionaries for.
    /// </param>
    ///
    /// <param name="userIDsSpecified">
    /// true if <paramref name="userIDsOrScreenNames" /> contains IDs, false if
    /// it contains screen names.
    /// </param>
    ///
    /// <param name="requestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
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
    /// The enumerated values are returned one-by-one, as a dictionary.
    /// </returns>
    ///
    /// <remarks>
    /// For each user ID or screen name in <paramref
    /// name="userIDsOrScreenNames" />, this method gets information about
    /// the user from Twitter and returns it as a dictionary of values.  The
    /// order of the returned values is not necessarily the same as the order
    /// of the user IDs or screen names.
    /// </remarks>
    //*************************************************************************

    public IEnumerable< Dictionary<String, Object> >
    EnumerateUserValueDictionaries
    (
        String [] userIDsOrScreenNames,
        Boolean userIDsSpecified,
        RequestStatistics requestStatistics,
        ReportProgressHandler reportProgressHandler,
        CheckCancellationPendingHandler checkCancellationPendingHandler
    )
    {
        Debug.Assert(userIDsOrScreenNames != null);
        Debug.Assert(requestStatistics != null);
        AssertValid();

        // We'll use Twitter's users/lookup API, which gets extended
        // information for up to 100 users in one call.

        Int32 iUsers = userIDsOrScreenNames.Length;
        Int32 iUsersProcessed = 0;
        Int32 iCalls = 0;

        while (iUsersProcessed < iUsers)
        {
            // For each call, ask for information about as many users as
            // possible until either 100 is reached or the URL reaches an
            // arbitrary maximum length.  Twitter recommends using a POST here
            // (without specifying why), but it would require revising the
            // base-class HTTP calls and isn't worth the trouble.

            Int32 iUsersProcessedThisCall = 0;

            StringBuilder oUrl = new StringBuilder();

            oUrl.AppendFormat(
                "{0}users/lookup.json?{1}&{2}="
                ,
                TwitterApiUrls.Rest,
                TwitterApiUrlParameters.IncludeEntities,
                userIDsSpecified ? "user_id" : "screen_name"
                );

            const Int32 MaxUsersPerCall = 100;
            const Int32 MaxUrlLength = 2000;

            // Construct the URL for this call.

            while (
                iUsersProcessed < iUsers
                &&
                iUsersProcessedThisCall < MaxUsersPerCall
                &&
                oUrl.Length < MaxUrlLength
                )
            {
                if (iUsersProcessedThisCall > 0)
                {
                    // Append an encoded comma.  Using an unencoded comma 
                    // causes Twitter to return a 401 "unauthorized" error.
                    //
                    // See this post for an explanation:
                    //
                    // https://dev.twitter.com/discussions/11399

                    oUrl.Append("%2C");
                }

                oUrl.Append( userIDsOrScreenNames[iUsersProcessed] );
                iUsersProcessed++;
                iUsersProcessedThisCall++;
            }

            iCalls++;

            if (iCalls > 1 && reportProgressHandler != null)
            {
                reportProgressHandler("Getting page " + iCalls + ".");
            }

            foreach ( Object oResult in EnumerateJsonValues(oUrl.ToString(),
                null, Int32.MaxValue, true, requestStatistics,
                reportProgressHandler, checkCancellationPendingHandler) )
            {
                yield return ( ( Dictionary<String, Object> )oResult );
            }
        }
    }

    //*************************************************************************
    //  Method: EnumerateJsonValues()
    //
    /// <summary>
    /// Gets a JSON response from a Twitter URL, then enumerates a specified
    /// set of values in the response.
    /// </summary>
    ///
    /// <param name="url">
    /// The URL to get the JSON from.  Can include URL parameters.
    /// </param>
    ///
    /// <param name="jsonName">
    /// If the top level of the JSON response contains a set of name/value
    /// pairs, this parameter should be the name whose value is the array of
    /// objects this method will enumerate.  If the top level of the JSON
    /// response contains an array of objects this method will enumerate, this
    /// parameter should be null. 
    /// </param>
    ///
    /// <param name="maximumValues">
    /// Maximum number of values to return, or Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="skipMostPage1Errors">
    /// If true, most page-1 errors are skipped over.  If false, they are
    /// rethrown.  (Errors that occur on page 2 and above are always skipped,
    /// unless they are due to rate limiting.)
    /// </param>
    ///
    /// <param name="requestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
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
    /// The enumerated values are returned one-by-one, as an Object.
    /// </returns>
    //*************************************************************************

    public IEnumerable<Object>
    EnumerateJsonValues
    (
        String url,
        String jsonName,
        Int32 maximumValues,
        Boolean skipMostPage1Errors,
        RequestStatistics requestStatistics,
        ReportProgressHandler reportProgressHandler,
        CheckCancellationPendingHandler checkCancellationPendingHandler
    )
    {
        // Note:
        //
        // The logic in this method is similar to the logic in
        // EnumerateSearchStatuses().  In fact, at one time all enumeration was
        // done through this EnumerateJsonValues() method.
        // EnumerateSearchStatuses() was created only when version 1.1 of the
        // Twitter API introduced yet another paging scheme, one that differs
        // from the cursor scheme that this method handles.
        //
        // A possible work item is to recombine the two methods into one,
        // possibly by using a delegate to handle the different paging schemes.

        Debug.Assert( !String.IsNullOrEmpty(url) );
        Debug.Assert(maximumValues > 0);
        Debug.Assert(requestStatistics != null);
        AssertValid();

        Int32 iPage = 1;
        String sCursor = null;
        Int32 iObjectsEnumerated = 0;

        while (true)
        {
            if (iPage > 1 && reportProgressHandler != null)
            {
                reportProgressHandler("Getting page " + iPage + ".");
            }

            String sUrlWithCursor = AppendCursorToUrl(url, sCursor);

            Dictionary<String, Object> oValueDictionary = null;
            Object [] aoObjectsThisPage;

            try
            {
                Object oDeserializedTwitterResponse = 
                    ( new JavaScriptSerializer() ).DeserializeObject(
                        GetTwitterResponseAsString(sUrlWithCursor,
                        requestStatistics, reportProgressHandler,
                        checkCancellationPendingHandler) );

                Object oObjectsThisPageAsObject;

                if (jsonName == null)
                {
                    // The top level of the Json response contains an array of
                    // objects this method will enumerate.

                    oObjectsThisPageAsObject = oDeserializedTwitterResponse;
                }
                else
                {
                    // The top level of the Json response contains a set of
                    // name/value pairs.  The value for the specified name is
                    // the array of objects this method will enumerate.

                    oValueDictionary = ( Dictionary<String, Object> )
                        oDeserializedTwitterResponse;

                    oObjectsThisPageAsObject = oValueDictionary[jsonName];
                }

                aoObjectsThisPage = ( Object [] )oObjectsThisPageAsObject;
            }
            catch (Exception oException)
            {
                // Rethrow the exception if appropriate.

                TwitterUtil.OnExceptionWhileEnumeratingJsonValues(
                    oException, iPage, skipMostPage1Errors);

                // Otherwise, just halt the enumeration.

                yield break;
            }

            Int32 iObjectsThisPage = aoObjectsThisPage.Length;

            if (iObjectsThisPage == 0)
            {
                yield break;
            }

            for (Int32 i = 0; i < iObjectsThisPage; i++)
            {
                yield return ( aoObjectsThisPage[i] );

                iObjectsEnumerated++;

                if (iObjectsEnumerated == maximumValues)
                {
                    yield break;
                }
            }

            iPage++;

            // When the top level of the Json response contains a set of
            // name/value pairs, a next_cursor_str value of "0" means "end of
            // data."

            if (
                oValueDictionary == null
                ||
                !TwitterJsonUtil.TryGetJsonValueFromDictionary(
                    oValueDictionary, "next_cursor_str", out sCursor)
                ||
                sCursor == "0"
                )
            {
                yield break;
            }

            // Get the next page...
        }
    }

    //*************************************************************************
    //  Method: EncodeUrlParameter()
    //
    /// <summary>
    /// Encodes an URL parameter using a method appropriate for Twitter and
    /// OAuth.
    /// </summary>
    ///
    /// <param name="urlParameter">
    /// The URL parameter to be encoded.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// The encoded parameter.
    /// </returns>
    //*************************************************************************

    public static String
    EncodeUrlParameter
    (
        String urlParameter
    )
    {
        Debug.Assert(urlParameter != null);

        // The OAuth code provides a method for this.  That method is based on
        // RFC 3986, Section 2.1, which is documented in "Percent encoding
        // parameters" at
        // https://dev.twitter.com/docs/auth/percent-encoding-parameters.

        return ( OAuthBase.UrlEncode(urlParameter) );
    }

    //*************************************************************************
    //  Method: GetTwitterResponseAsString()
    //
    /// <summary>
    /// Gets a response from a Twitter URL as a string.
    /// </summary>
    ///
    /// <param name="url">
    /// The URL to get the string from.
    /// </param>
    ///
    /// <param name="requestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
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
    /// The string returned by the Twitter server.
    /// </returns>
    ///
    /// <remarks>
    /// If an error occurs, an exception is thrown.
    /// </remarks>
    //*************************************************************************

    public String
    GetTwitterResponseAsString
    (
        String url,
        RequestStatistics requestStatistics,
        ReportProgressHandler reportProgressHandler,
        CheckCancellationPendingHandler checkCancellationPendingHandler
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(url) );
        Debug.Assert(requestStatistics != null);
        AssertValid();

        Int32 iRateLimitPauses = 0;

        while (true)
        {
            // Add the required authorization information to the URL.
            //
            // Note: Don't do this outside the while (true) loop.  The
            // authorization information includes a timestamp that will
            // probably expire if the code within the catch block pauses.

            oAuthTwitter oAuthTwitter = new oAuthTwitter(
                m_UserAgent, m_TimeoutMs);

            oAuthTwitter.Token = m_TwitterAccessToken;
            oAuthTwitter.TokenSecret = m_TwitterAccessTokenSecret;

            String sAuthorizedUrl, sAuthorizedPostData;

            oAuthTwitter.ConstructAuthWebRequest(oAuthTwitter.Method.GET,
                url, String.Empty, out sAuthorizedUrl,
                out sAuthorizedPostData);

            url = sAuthorizedUrl;

            Stream oStream = null;

            try
            {
                oStream =
                    HttpSocialNetworkUtil.GetHttpWebResponseStreamWithRetries(
                        url, HttpStatusCodesToFailImmediately,
                        requestStatistics, m_UserAgent, m_TimeoutMs,
                        reportProgressHandler, checkCancellationPendingHandler);

                return ( new StreamReader(oStream).ReadToEnd() );
            }
            catch (WebException oWebException)
            {
                if (
                    !WebExceptionIsDueToRateLimit(oWebException)
                    ||
                    iRateLimitPauses > 0
                    )
                {
                    throw;
                }

                // Twitter rate limits have kicked in.  Pause and try again.

                iRateLimitPauses++;
                Int32 iRateLimitPauseMs = GetRateLimitPauseMs(oWebException);

                DateTime oWakeUpTime = DateTime.Now.AddMilliseconds(
                    iRateLimitPauseMs);

                if (reportProgressHandler != null)
                {
                    reportProgressHandler( String.Format(

                    "Reached Twitter rate limits.  Pausing until {0}."
                    ,
                    oWakeUpTime.ToLongTimeString()
                    ) );
                }

                // Don't pause in one large interval, which would prevent
                // cancellation.

                const Int32 SleepCycleDurationMs = 1000;

                Int32 iSleepCycles = (Int32)Math.Ceiling(
                    (Double)iRateLimitPauseMs / SleepCycleDurationMs) ;

                for (Int32 i = 0; i < iSleepCycles; i++)
                {
                    if (checkCancellationPendingHandler != null)
                    {
                        checkCancellationPendingHandler();
                    }

                    System.Threading.Thread.Sleep(SleepCycleDurationMs);
                }
            }
            finally
            {
                if (oStream != null)
                {
                    oStream.Close();
                }
            }
        }
    }

    //*************************************************************************
    //  Method: OnExceptionWhileEnumeratingJsonValues()
    //
    /// <summary>
    /// Handles an exception throws while enumerating JSON values.
    /// </summary>
    ///
    /// <param name="exception">
    /// The exception that was thrown.
    /// </param>
    ///
    /// <param name="iPage">
    /// The page the exception was thrown from.
    /// </param>
    ///
    /// <param name="bSkipMostPage1Errors">
    /// If true, most page-1 errors are skipped over.  If false, they are
    /// rethrown.  (Errors that occur on page 2 and above are always skipped,
    /// unless they are due to rate limiting.)
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="exception" /> is fatal, this method rethrows the
    /// exception.  Otherwise, this method returns and the caller should stop
    /// its enumeration but not throw an exception.
    /// </remarks>
    //*************************************************************************

    public static void
    OnExceptionWhileEnumeratingJsonValues
    (
        Exception exception,
        Int32 iPage,
        Boolean bSkipMostPage1Errors
    )
    {
        Debug.Assert(exception != null);
        Debug.Assert(iPage > 0);

        if ( !HttpSocialNetworkUtil.ExceptionIsWebOrJson(exception))
        {
            // This is an unknown exception.

            throw (exception);
        }

        if (
            exception is WebException
            &&
            WebExceptionIsDueToRateLimit( (WebException)exception )
            )
        {
            throw (exception);
        }
        else if (iPage == 1)
        {
            if (bSkipMostPage1Errors)
            {
                return;
            }
            else
            {
                throw (exception);
            }
        }
        else
        {
            // Always skip non-rate-limit errors on page 2 and above.

            return;
        }
    }

    //*************************************************************************
    //  Method: WebExceptionIsDueToRateLimit()
    //
    /// <summary>
    /// Determines whether a WebException is due to Twitter rate limits.
    /// </summary>
    ///
    /// <param name="webException">
    /// The WebException to check.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="webException" /> is due to Twitter rate limits
    /// kicking in.
    /// </returns>
    //*************************************************************************

    public static Boolean
    WebExceptionIsDueToRateLimit
    (
        WebException webException
    )
    {
        Debug.Assert(webException != null);

        // Starting with version 1.1 of the Twitter API, a single HTTP status
        // code (429, "rate limit exceeded") is used for all rate-limit
        // responses.

        return ( HttpSocialNetworkUtil.WebExceptionHasHttpStatusCode(
            webException, (HttpStatusCode)429) );
    }

    //*************************************************************************
    //  Method: GetSearchUrl()
    //
    /// <summary>
    /// Gets the URL for getting tweets with a specified search term.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="sQueryParametersForNextPage">
    /// The complete query parameters to use if getting page number 2 or
    /// greater, or null if getting the first page.
    /// </param>
    ///
    /// <returns>
    /// The URL to use.
    /// </returns>
    //*************************************************************************

    private String
    GetSearchUrl
    (
        String sSearchTerm,
        String sQueryParametersForNextPage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        AssertValid();

        if (sQueryParametersForNextPage == null)
        {
            return ( String.Format(

                "{0}?q={1}&count=100&result_type=recent&{2}"
                ,
                TwitterApiUrls.Search,
                EncodeUrlParameter(sSearchTerm),
                TwitterApiUrlParameters.IncludeEntities
                ) );
        }

        return (TwitterApiUrls.Search + sQueryParametersForNextPage);
    }

    //*************************************************************************
    //  Method: TryGetQueryParametersForNextSearchPage()
    //
    /// <summary>
    /// Attempts to get the query parameters to use for the next page of search
    /// results.
    /// </summary>
    ///
    /// <param name="oResponseDictionary">
    /// Response returned by Twitter for the current page.
    /// </param>
    ///
    /// <param name="sQueryParametersForNextPage">
    /// Where the complete query parameters to use get stored if true is
    /// returned.  Includes a leading question mark.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private Boolean
    TryGetQueryParametersForNextSearchPage
    (
        Dictionary<String, Object> oResponseDictionary,
        out String sQueryParametersForNextPage
    )
    {
        Debug.Assert(oResponseDictionary != null);
        AssertValid();

        sQueryParametersForNextPage = null;

        Dictionary<String, Object> oSearchMetadataDictionary =
            ( Dictionary<String, Object> )
            oResponseDictionary["search_metadata"];

        return (
            oSearchMetadataDictionary != null
            &&
            TwitterJsonUtil.TryGetJsonValueFromDictionary(
                oSearchMetadataDictionary, "next_results",
                out sQueryParametersForNextPage)
            );
    }

    //*************************************************************************
    //  Method: AppendCursorToUrl()
    //
    /// <summary>
    /// Appends a cursor to a Twitter URL.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL to append to.  Can include URL parameters.
    /// </param>
    ///
    /// <param name="sCursor">
    /// The cursor to append, or null to not append a cursor.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="sUrl" /> with a cursor appended to it if requested.
    /// </returns>
    //*************************************************************************

    private String
    AppendCursorToUrl
    (
        String sUrl,
        String sCursor
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sUrl) );
        AssertValid();

        if (sCursor == null)
        {
            return (sUrl);
        }

        return ( String.Format(
            
            "{0}{1}cursor={2}"
            ,
            sUrl,
            sUrl.IndexOf('?') == -1 ? '?' : '&',
            sCursor
            ) );
    }

    //*************************************************************************
    //  Method: GetRateLimitPauseMs()
    //
    /// <summary>
    /// Gets the time to pause before retrying a request after Twitter rate
    /// limits kicks in.
    /// </summary>
    ///
    /// <param name="oWebException">
    /// The WebException to check.
    /// </param>
    ///
    /// <returns>
    /// The time to pause before retrying a request after Twitter rate limits
    /// kick in, in milliseconds.
    /// </returns>
    //*************************************************************************

    private Int32
    GetRateLimitPauseMs
    (
        WebException oWebException
    )
    {
        Debug.Assert(oWebException != null);
        AssertValid();

        // The Twitter REST API provides a custom X-Rate-Limit-Reset header in
        // the response headers.  This is the time at which the request should
        // be made again, in seconds since 1/1/1970, in UTC.  If this header is
        // available, use it.  Otherwise, use a default pause time.

        Int32 iRateLimitPauseMs = DefaultRateLimitPauseMs;

        WebResponse oWebResponse = oWebException.Response;

        if (oWebResponse != null)
        {
            String sXRateLimitReset =
                oWebResponse.Headers["X-Rate-Limit-Reset"];

            Int32 iSecondsSince1970;

            // (Note that Int32.TryParse() can handle null, which indicates a
            // missing header.)

            if ( Int32.TryParse(sXRateLimitReset, out iSecondsSince1970) )
            {
                DateTime oResetTimeUtc =
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).
                        AddSeconds(iSecondsSince1970);

                Double dRateLimitPauseMs =
                    (oResetTimeUtc - DateTime.UtcNow).TotalMilliseconds;

                // Don't wait longer than two hours.

                if (dRateLimitPauseMs > 0 &&
                    dRateLimitPauseMs <= 2 * 60 * 60 * 1000)
                {
                    iRateLimitPauseMs = (Int32)dRateLimitPauseMs;
                }
            }
        }

        // The extra time added to the calculated pause time is an attempt to
        // work around a problem where Twitter occasionally returns 429 ("rate
        // limit exceeded") a second time, after GetTwitterResponseAsString()
        // pauses once for the time specified by Twitter.
        //
        // Is this a server-client clock synchronization problem?  A question
        // posted on the Twitter API forums went unanswered:
        //
        //   http://dev.twitter.com/discussions/18999

        return (iRateLimitPauseMs + ExtraRateLimitPauseMs);
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
        Debug.Assert( !String.IsNullOrEmpty(m_TwitterAccessToken) );
        Debug.Assert( !String.IsNullOrEmpty(m_TwitterAccessTokenSecret) );
        Debug.Assert( !String.IsNullOrEmpty(m_UserAgent) );
        Debug.Assert(m_TimeoutMs > 0);
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// HTTP status codes that have special meaning with Twitter.  When they
    /// occur, the requests are not retried.

    private static readonly HttpStatusCode []
        HttpStatusCodesToFailImmediately = new HttpStatusCode[] {

            // Occurs when information about a user who has "protected" status
            // is requested, for example.

            HttpStatusCode.Unauthorized,

            // Occurs when the Twitter search API returns a tweet from a user,
            // but then refuses to provide a list of the people followed by the
            // user, probably because the user has protected her account.
            // (But if she has protected her account, why is the search API
            // returning one of her tweets?)

            HttpStatusCode.NotFound,

            // Starting with version 1.1 of the Twitter API, a single HTTP
            // status code (429, "rate limit exceeded") is used for all
            // rate-limit responses.

            (HttpStatusCode)429,

            // Not sure about what causes this one.

            HttpStatusCode.Forbidden,
        };

    /// Default time to pause before retrying a request after Twitter rate
    /// limits kick in, in milliseconds.

    private const Int32 DefaultRateLimitPauseMs = 15 * 60 * 1000;

    /// Extra time to add to the pause, in milliseconds.

    private const Int32 ExtraRateLimitPauseMs = 15 * 1000;


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// The Twitter access token to use.

    private String m_TwitterAccessToken;

    /// The Twitter access token secret to use.

    private String m_TwitterAccessTokenSecret;

    /// The user agent string to use.

    private String m_UserAgent;

    /// Timeout to use, in milliseconds.

    private Int32 m_TimeoutMs;
}

}
