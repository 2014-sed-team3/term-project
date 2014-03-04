
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.SocialNetworkLib.Twitter;

namespace Smrf.NodeXL.GraphMLLib
{
//*****************************************************************************
//  Class: TwitterStatus
//
/// <summary>
/// Stores information about a Twitter status.
/// </summary>
///
/// <remarks>
/// This is meant for use while creating Twitter GraphML XML documents for use
/// with the NodeXL Excel Template.
/// </remarks>
//*****************************************************************************

public class TwitterStatus : Object
{
    //*************************************************************************
    //  Method: TryFromStatusValueDictionary()
    //
    /// <summary>
    /// Attempts to create a new instance of the <see cref="TwitterStatus" />
    /// class from a Twitter JSON response.
    /// </summary>
    ///
    /// <param name="statusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a status.
    /// </param>
    ///
    /// <param name="expandStatusUrls">
    /// true to expand all URLs that might be shortened URLs.
    /// </param>
    ///
    /// <param name="twitterStatus">
    /// Where a new <see cref="TwitterStatus" /> object gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryFromStatusValueDictionary
    (
        Dictionary<String, Object> statusValueDictionary,
        Boolean expandStatusUrls,
        out TwitterStatus twitterStatus
    )
    {
        Debug.Assert(statusValueDictionary != null);

        twitterStatus = null;

        // Get the status information.

        String statusID, statusText;

        if (
            !TwitterJsonUtil.TryGetJsonValueFromDictionary(
                statusValueDictionary, "id_str", out statusID)
            ||
            !TwitterJsonUtil.TryGetJsonValueFromDictionary(
                statusValueDictionary, "text", out statusText)
            )
        {
            return (false);
        }

        String statusDateUtc;

        if ( TwitterJsonUtil.TryGetJsonValueFromDictionary(
            statusValueDictionary, "created_at", out statusDateUtc) )
        {
            statusDateUtc = TwitterDateParser.ParseTwitterDate(statusDateUtc);
        }

        String latitude, longitude;

        TwitterGraphMLUtil.GetLatitudeAndLongitudeFromStatusValueDictionary(
            statusValueDictionary, out latitude, out longitude);

        String statusUrls, statusHashtags;

        TwitterGraphMLUtil.GetUrlsAndHashtagsFromStatusValueDictionary(
            statusValueDictionary, expandStatusUrls, out statusUrls,
            out statusHashtags);

        String inReplyToStatusID;

        TwitterJsonUtil.TryGetJsonValueFromDictionary(
            statusValueDictionary, "in_reply_to_status_id_str",
            out inReplyToStatusID);

        // Note that null date, coordinates, URLs hashtags, and in-reply-to-ID
        // are acceptable here.

        twitterStatus = new TwitterStatus(
            statusID, statusText, statusDateUtc, latitude, longitude,
            statusUrls, statusHashtags, inReplyToStatusID);

        return (true);
    }

    //*************************************************************************
    //  Constructor: TwitterStatus()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterStatus" /> class.
    /// </summary>
    ///
    /// <param name="ID">
    /// The status ID.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="text">
    /// The status text.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="parsedDateUtc">
    /// The parsed status date as a culture-invariant UTC string, or null if
    /// not available.  See <see
    /// cref="Smrf.SocialNetworkLib.Twitter.TwitterDateParser.ParseTwitterDate"
    /// />.  Can be null.
    /// </param>
    ///
    /// <param name="latitude">
    /// The status's latitude, or null if not available.
    /// </param>
    ///
    /// <param name="longitude">
    /// The status's longitude, or null if not available.
    /// </param>
    ///
    /// <param name="urls">
    /// The status's space-delimited URLs, or null if not available.
    /// </param>
    ///
    /// <param name="hashtags">
    /// The status's space-delimited hashtags, or null if not available.
    /// </param>
    ///
    /// <param name="inReplyToStatusID">
    /// The in-reply-to status ID, or null if not available.
    /// </param>
    //*************************************************************************

    private TwitterStatus
    (
        String ID,
        String text,
        String parsedDateUtc,
        String latitude,
        String longitude,
        String urls,
        String hashtags,
        String inReplyToStatusID
    )
    {
        m_ID = ID;
        m_Text = text;
        m_ParsedDateUtc = parsedDateUtc;
        m_Latitude = latitude;
        m_Longitude = longitude;
        m_Urls = urls;
        m_Hashtags = hashtags;
        m_InReplyToStatusID = inReplyToStatusID;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ID
    //
    /// <summary>
    /// Gets the status ID.
    /// </summary>
    ///
    /// <value>
    /// The status ID.  Can't be null or empty.
    /// </value>
    //*************************************************************************

    public String
    ID
    {
        get
        {
            AssertValid();

            return (m_ID);
        }
    }

    //*************************************************************************
    //  Property: Text
    //
    /// <summary>
    /// Gets the status text.
    /// </summary>
    ///
    /// <value>
    /// The status text.  Can't be null or empty.
    /// </value>
    //*************************************************************************

    public String
    Text
    {
        get
        {
            AssertValid();

            return (m_Text);
        }
    }

    //*************************************************************************
    //  Property: ParsedDateUtc
    //
    /// <summary>
    /// Gets the parsed status date.
    /// </summary>
    ///
    /// <value>
    /// The parsed status date as a culture-invariant UTC string, or null if
    /// not available.  See <see
    /// cref="Smrf.SocialNetworkLib.Twitter.TwitterDateParser.ParseTwitterDate"
    /// />.
    /// </value>
    //*************************************************************************

    public String
    ParsedDateUtc
    {
        get
        {
            AssertValid();

            return (m_ParsedDateUtc);
        }
    }

    //*************************************************************************
    //  Property: Latitude
    //
    /// <summary>
    /// Gets the status's latitude.
    /// </summary>
    ///
    /// <value>
    /// The status's latitude, or null if not available.
    /// </value>
    //*************************************************************************

    public String
    Latitude
    {
        get
        {
            AssertValid();

            return (m_Latitude);
        }
    }

    //*************************************************************************
    //  Property: Longitude
    //
    /// <summary>
    /// Gets the status's longitude.
    /// </summary>
    ///
    /// <value>
    /// The status's longitude, or null if not available.
    /// </value>
    //*************************************************************************

    public String
    Longitude
    {
        get
        {
            AssertValid();

            return (m_Longitude);
        }
    }

    //*************************************************************************
    //  Property: Urls
    //
    /// <summary>
    /// Gets or sets the status's space-delimited URLs.
    /// </summary>
    ///
    /// <value>
    /// The status's space-delimited URLs, or null if not available.
    /// </value>
    //*************************************************************************

    public String
    Urls
    {
        get
        {
            AssertValid();

            return (m_Urls);
        }

        set
        {
            m_Urls = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Hashtags
    //
    /// <summary>
    /// Gets the status's space-delimited hashtags.
    /// </summary>
    ///
    /// <value>
    /// The status's space-delimited hashtags, or null if not available.
    /// </value>
    //*************************************************************************

    public String
    Hashtags
    {
        get
        {
            AssertValid();

            return (m_Hashtags);
        }
    }

    //*************************************************************************
    //  Property: InReplyToStatusID
    //
    /// <summary>
    /// Gets the ID of the status that this status is a reply to.
    /// </summary>
    ///
    /// <value>
    /// The in-reply-to status ID, or null if not available.
    /// </value>
    //*************************************************************************

    public String
    InReplyToStatusID
    {
        get
        {
            AssertValid();

            return (m_InReplyToStatusID);
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

    public void
    AssertValid()
    {
        Debug.Assert( !String.IsNullOrEmpty(m_ID) );
        Debug.Assert( !String.IsNullOrEmpty(m_Text) );
        // m_ParsedDateUtc
        // m_Latitude
        // m_Longitude
        // m_Urls
        // m_Hashtags
        // m_InReplyToStatusID
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The status ID.

    protected String m_ID;

    /// The status text.

    protected String m_Text;

    /// The parsed status date as a culture-invariant UTC string, or null.

    protected String m_ParsedDateUtc;

    /// The status's latitude and longitude, or null.

    protected String m_Latitude;
    ///
    protected String m_Longitude;

    /// The status's space-delimited URLs, or null.

    protected String m_Urls;

    /// The status's space-delimited hashtags, or null.

    protected String m_Hashtags;

    /// The in-reply-to status ID, or null.

    protected String m_InReplyToStatusID;
}

}
