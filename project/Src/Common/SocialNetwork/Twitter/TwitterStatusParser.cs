
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.SocialNetworkLib.Twitter
{
//*****************************************************************************
//  Class: TwitterStatusParser
//
/// <summary>
/// Parses a Twitter status.
/// </summary>
///
/// <remarks>
/// A tweet is known as a "status" in the Twitter API, so "status" is the term
/// used in this class's methods.
/// </remarks>
//*****************************************************************************

public static class TwitterStatusParser : Object
{
    //*************************************************************************
    //  Method: TryParseStatus()
    //
    /// <summary>
    /// Attempts to parse basic information from a Twitter status.
    /// </summary>
    ///
    /// <param name="statusValueDictionary">
    /// A status value dictionary, as returned by
    /// TwitterUtil.EnumerateSearchStatuses().  The dictionary keys are names
    /// and the dictionary values are the named values.
    /// </param>
    /// 
    /// <param name="statusID">
    /// Where the status ID gets stored if true is returned.
    /// </param>
    ///
    /// <param name="statusDateUtc">
    /// Where the date the user tweeted the status gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <param name="screenName">
    /// Where the screen name of the user who tweeted the status gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <param name="text">
    /// Where the status's text gets stored if true is returned.
    /// </param>
    ///
    /// <param name="rawStatusJson">
    /// Where the complete, raw status returned by Twitter gets stored if true
    /// is returned, in JSON format.  Includes the surrounding braces.
    /// </param>
    /// 
    /// <param name="userValueDictionary">
    /// Where a dictionary of values for the user who tweeted the status gets
    /// stored if true is returned.
    /// </param>
    /// 
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryParseStatus
    (
        Dictionary<String, Object> statusValueDictionary,
        out Int64 statusID,
        out DateTime statusDateUtc,
        out String screenName,
        out String text,
        out String rawStatusJson,
        out Dictionary<String, Object> userValueDictionary
    )
    {
        Debug.Assert(statusValueDictionary != null);

        statusID = Int64.MinValue;
        statusDateUtc = DateTime.MinValue;
        screenName = null;
        text = null;
        rawStatusJson = null;
        userValueDictionary = null;

        String statusDateUtcString;

        if (
            !TwitterJsonUtil.TryGetJsonValueFromDictionary(
                statusValueDictionary, "id", out statusID)
            ||
            !TwitterJsonUtil.TryGetJsonValueFromDictionary(
                statusValueDictionary, "created_at", out statusDateUtcString)
            ||
            !TwitterDateParser.TryParseTwitterDate(statusDateUtcString,
                out statusDateUtc)
            ||
            !TwitterJsonUtil.TryGetJsonValueFromDictionary(
                statusValueDictionary, "text", out text)
            )
        {
            return (false);
        }

        const String UserKeyName = "user";

        if ( !statusValueDictionary.ContainsKey(UserKeyName) )
        {
            // This has actually happened--Twitter occasionally sends a
            // status without user information.

            return (false);
        }

        userValueDictionary =
            ( Dictionary<String, Object> )statusValueDictionary[UserKeyName];

        if ( !TwitterJsonUtil.TryGetJsonValueFromDictionary(
                userValueDictionary, "screen_name", out screenName) )
        {
            return (false);
        }

        rawStatusJson = ValueDictionaryToRawJson(statusValueDictionary);

        return (true);
    }

    //*************************************************************************
    //  Method: UserValueDictionaryToRawJson()
    //
    /// <summary>
    /// Converts a user value dictionary to a JSON string.
    /// </summary>
    ///
    /// <param name="userValueDictionary">
    /// A user value dictionary, as returned by <see cref="TryParseStatus" />.
    /// The dictionary keys are names and the dictionary values are the named
    /// values.
    /// </param>
    /// 
    /// <returns>
    /// The user value dictionary as a JSON string.
    /// </returns>
    //*************************************************************************

    public static String
    UserValueDictionaryToRawJson
    (
        Dictionary<String, Object> userValueDictionary
    )
    {
        Debug.Assert(userValueDictionary != null);

        return ( ValueDictionaryToRawJson(userValueDictionary) );
    }

    //*************************************************************************
    //  Method: GetUrlsFromStatusValueDictionary()
    //
    /// <summary>
    /// Get the URLs from the entities in a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="statusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a status.
    /// </param>
    ///
    /// <param name="expandUrls">
    /// true to expand all URLs that might be shortened URLs.
    /// </param>
    ///
    /// <returns>
    /// An array of URLs.  If the status value dictionary didn't contain URLs,
    /// the array is empty.
    /// </returns>
    //*************************************************************************

    public static String []
    GetUrlsFromStatusValueDictionary
    (
        Dictionary<String, Object> statusValueDictionary,
        Boolean expandUrls
    )
    {
        Debug.Assert(statusValueDictionary != null);

        Dictionary<String, Object> entityValueDictionary;

        if ( !TryGetEntityValueDictionary(statusValueDictionary,
            out entityValueDictionary) )
        {
            return ( new String[0] );
        }

        String [] urls = GetEntities(
            entityValueDictionary, "urls", "expanded_url", false);

        if (expandUrls)
        {
            Int32 urlCount = urls.Length;

            for (Int32 i = 0; i < urlCount; i++)
            {
                // If there is an (illegal) space in the expanded URL, escape
                // it to prevent it from causing problems further down the
                // line.

                urls[i] = UrlUtil.ExpandUrl( urls[i] ).Replace(" ", "%20");
            }
        }

        return (urls);
    }

    //*************************************************************************
    //  Method: GetHashtagsFromStatusValueDictionary()
    //
    /// <summary>
    /// Get the hashtags from the entities in a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="statusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a status.
    /// </param>
    ///
    /// <returns>
    /// An array of hashtags.  The hashtags are all in lower case and do not
    /// include a leading pound sign.  If the status value dictionary didn't
    /// contain hashtags, the array is empty.
    /// </returns>
    //*************************************************************************

    public static String []
    GetHashtagsFromStatusValueDictionary
    (
        Dictionary<String, Object> statusValueDictionary
    )
    {
        Debug.Assert(statusValueDictionary != null);

        Dictionary<String, Object> entityValueDictionary;

        if ( !TryGetEntityValueDictionary(statusValueDictionary,
            out entityValueDictionary) )
        {
            return ( new String[0] );
        }

        return (
            GetEntities(entityValueDictionary, "hashtags", "text", true)
            );
    }

    //*************************************************************************
    //  Method: TryGetEntityValueDictionary()
    //
    /// <summary>
    /// Attempts to an entity value dictionary from a status value dictionary.
    /// </summary>
    ///
    /// <param name="statusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a status.
    /// </param>
    ///
    /// <param name="entityValueDictionary">
    /// Where the entity value dictionary gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetEntityValueDictionary
    (
        Dictionary<String, Object> statusValueDictionary,
        out Dictionary<String, Object> entityValueDictionary
    )
    {
        Debug.Assert(statusValueDictionary != null);

        Object entitiesAsObject;

        if (
            statusValueDictionary.TryGetValue("entities",
                out entitiesAsObject)
            &&
            entitiesAsObject is Dictionary<String, Object>
            )
        {
            entityValueDictionary =
                ( Dictionary<String, Object> )entitiesAsObject;

            return (true);
        }

        entityValueDictionary = null;
        return (false);
    }

    //*************************************************************************
    //  Method: GetEntities()
    //
    /// <summary>
    /// Attempts to get entities from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="entityValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains all
    /// entities for a status.
    /// </param>
    ///
    /// <param name="entityName">
    /// Name of the value in <paramref name="entityValueDictionary" />
    /// containing the entities to get.  Sample: "urls".  The value is assumed
    /// to contain an array of entity objects; an array of URL objects, for
    /// example. 
    /// </param>
    ///
    /// <param name="entityChildName">
    /// Name of the child value in each entity to get.  Sample: "expanded_url".
    /// </param>
    ///
    /// <param name="convertToLowerCase">
    /// True to convert all the returned entities to lower case.
    /// </param>
    ///
    /// <returns>
    /// An array of entities.  The array may be empty.
    /// </returns>
    //*************************************************************************

    private static String []
    GetEntities
    (
        Dictionary<String, Object> entityValueDictionary,
        String entityName,
        String entityChildName,
        Boolean convertToLowerCase
    )
    {
        Debug.Assert(entityValueDictionary != null);
        Debug.Assert( !String.IsNullOrEmpty(entityName) );
        Debug.Assert( !String.IsNullOrEmpty(entityChildName) );

        List<String> entities = new List<String>();
        Object entitiesAsObject;

        if (
            entityValueDictionary.TryGetValue(entityName, out entitiesAsObject)
            &&
            entitiesAsObject is Object[]
            )
        {
            foreach (Object entityAsObject in ( Object [] )entitiesAsObject)
            {
                String childValue;

                if (
                    entityAsObject is Dictionary<String, Object>
                    &&
                    TwitterJsonUtil.TryGetJsonValueFromDictionary(
                        ( Dictionary<String, Object> )entityAsObject,
                        entityChildName, out childValue)
                    )
                {
                    if (convertToLowerCase)
                    {
                        childValue = childValue.ToLower();
                    }

                    entities.Add(childValue);
                }
            }
        }

        return ( entities.ToArray() );
    }

    //*************************************************************************
    //  Method: ValueDictionaryToRawJson()
    //
    /// <summary>
    /// Converts a value dictionary to a JSON string.
    /// </summary>
    ///
    /// <param name="valueDictionary">
    /// A value dictionary obtained by parsing a JSON string.  The dictionary
    /// keys are names and the dictionary values are the named values.
    /// </param>
    /// 
    /// <returns>
    /// The value dictionary as a JSON string.
    /// </returns>
    //*************************************************************************

    private static String
    ValueDictionaryToRawJson
    (
        Dictionary<String, Object> valueDictionary
    )
    {
        Debug.Assert(valueDictionary != null);

        // We don't have access to the original string response, so rebuild the
        // complete JSON string from the value dictionary.

        StringBuilder stringBuilder = new StringBuilder();

        ( new JavaScriptSerializer() ).Serialize(
            valueDictionary, stringBuilder);

        return ( stringBuilder.ToString() );
    }
}

}
