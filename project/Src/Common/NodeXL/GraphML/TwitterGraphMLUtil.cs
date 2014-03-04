
using System;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Smrf.SocialNetworkLib.Twitter;
using Smrf.AppLib;
using Smrf.XmlLib;

namespace Smrf.NodeXL.GraphMLLib
{
//*****************************************************************************
//  Class: TwitterGraphMLUtil
//
/// <summary>
/// Utility methods for creating Twitter GraphML XML documents for use with the
/// NodeXL Excel Template.
/// </summary>
//*****************************************************************************

public static class TwitterGraphMLUtil : Object
{
    //*************************************************************************
    //  Method: DefineVertexStatisticsGraphMLAttributes()
    //
    /// <summary>
    /// Defines GraphML-Attributes for Twitter user statistics.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <remarks>
    /// The statistic attribute values appear on the vertex worksheet.
    /// </remarks>
    //*************************************************************************

    public static void
    DefineVertexStatisticsGraphMLAttributes
    (
        GraphMLXmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineGraphMLAttributes(false, "int",
            VertexFollowedID, "Followed",
            VertexFollowersID, "Followers",
            VertexStatusesID, "Tweets",
            VertexFavoritesID, "Favorites",
            VertexUtcOffsetID, "Time Zone UTC Offset (Seconds)"
            );

        graphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            VertexDescriptionID, "Description",
            VertexLocationID, "Location",
            VertexUrlID, "Web",
            VertexTimeZoneID, "Time Zone",
            VertexJoinedDateUtcID, "Joined Twitter Date (UTC)"
            );
    }

    //*************************************************************************
    //  Method: DefineVertexLatestStatusGraphMLAttributes()
    //
    /// <summary>
    /// Defines GraphML-Attributes for Twitter users' latest statuses.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <remarks>
    /// The latest status attribute values appear on the vertex worksheet.
    /// </remarks>
    //*************************************************************************

    public static void
    DefineVertexLatestStatusGraphMLAttributes
    (
        GraphMLXmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            VertexLatestStatusID, "Latest Tweet",
            VertexLatestStatusUrlsID, "URLs in Latest Tweet",
            VertexLatestStatusDomainsID, "Domains in Latest Tweet",
            VertexLatestStatusHashtagsID, "Hashtags in Latest Tweet",
            VertexLatestStatusDateUtcID, "Latest Tweet Date (UTC)"
            );

        DefineLatitudeAndLongitudeGraphMLAttributes(graphMLXmlDocument, false);
        DefineInReplyToStatusIDGraphMLAttribute(graphMLXmlDocument, false);
    }

    //*************************************************************************
    //  Method: DefineEdgeStatusGraphMLAttributes()
    //
    /// <summary>
    /// Defines GraphML-Attributes for statuses.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    ///
    /// <remarks>
    /// The status attribute values appear on the edge worksheet.
    /// </remarks>
    //*************************************************************************

    public static void
    DefineEdgeStatusGraphMLAttributes
    (
        GraphMLXmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineEdgeStringGraphMLAttributes(
            EdgeStatusID, "Tweet",
            EdgeStatusUrlsID, "URLs in Tweet",
            EdgeStatusDomainsID, "Domains in Tweet",
            EdgeStatusHashtagsID, "Hashtags in Tweet",
            EdgeStatusDateUtcID, "Tweet Date (UTC)",
            EdgeStatusWebPageUrlID, "Twitter Page for Tweet"
            );

        DefineLatitudeAndLongitudeGraphMLAttributes(graphMLXmlDocument, true);

        NodeXLGraphMLUtil.DefineImportedIDGraphMLAttribute(
            graphMLXmlDocument, true);

        DefineInReplyToStatusIDGraphMLAttribute(graphMLXmlDocument, true);
    }

    //*************************************************************************
    //  Method: DefineCommonGraphMLAttributes()
    //
    /// <summary>
    /// Defines GraphML-Attributes that are common to all Twitter networks.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <remarks>
    /// The common attribute values appear on the edge and vertex worksheets.
    /// </remarks>
    //*************************************************************************

    public static void
    DefineCommonGraphMLAttributes
    (
        GraphMLXmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        NodeXLGraphMLUtil.DefineVertexImageFileGraphMLAttribute(
            graphMLXmlDocument);

        NodeXLGraphMLUtil.DefineVertexCustomMenuGraphMLAttributes(
            graphMLXmlDocument);

        NodeXLGraphMLUtil.DefineEdgeRelationshipGraphMLAttribute(
            graphMLXmlDocument);

        graphMLXmlDocument.DefineEdgeStringGraphMLAttributes(
            EdgeRelationshipDateUtcID, "Relationship Date (UTC)");
    }

    //*************************************************************************
    //  Method: DefineInReplyToStatusIDGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute for in-reply-to status ID.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="forEdges">
    /// true if the attribute is for edges, false if it is for vertices.
    /// </param>
    //*************************************************************************

    public static void
    DefineInReplyToStatusIDGraphMLAttribute
    (
        GraphMLXmlDocument graphMLXmlDocument,
        Boolean forEdges
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineStringGraphMLAttributes(forEdges,
            InReplyToStatusIDID, "In-Reply-To Tweet ID"
            );
    }

    //*************************************************************************
    //  Method: TryAppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node to the GraphML document for a user if such a
    /// node doesn't already exist.
    /// </summary>
    ///
    /// <param name="screenName">
    /// Screen name to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="userID">
    /// Twitter user ID to add a vertex XML node for.
    /// </param>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="userIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="twitterUser">
    /// Where the TwitterUser that represents the user gets stored.  This gets
    /// set regardless of whether the node already exists.
    /// </param>
    ///
    /// <returns>
    /// true if a vertex XML node was added, false if a vertex XML node already
    /// exists.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryAppendVertexXmlNode
    (
        String screenName,
        String userID,
        GraphMLXmlDocument graphMLXmlDocument,
        Dictionary<String, TwitterUser> userIDDictionary,
        out TwitterUser twitterUser
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(screenName) );
        Debug.Assert( !String.IsNullOrEmpty(userID) );
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(userIDDictionary != null);

        twitterUser = null;

        if ( userIDDictionary.TryGetValue(userID, out twitterUser) )
        {
            // A vertex XML node already exists.

            return (false);
        }

        XmlNode vertexXmlNode = graphMLXmlDocument.AppendVertexXmlNode(
            screenName);

        twitterUser = new TwitterUser(screenName, vertexXmlNode);
        userIDDictionary.Add(userID, twitterUser);

        graphMLXmlDocument.AppendGraphMLAttributeValue(vertexXmlNode,
            NodeXLGraphMLUtil.VertexMenuTextID,
            "Open Twitter Page for This Person");

        graphMLXmlDocument.AppendGraphMLAttributeValue(
            vertexXmlNode,
            NodeXLGraphMLUtil.VertexMenuActionID,
            String.Format(TwitterApiUrls.UserWebPageUrlPattern, screenName)
            );

        return (true);
    }

    //*************************************************************************
    //  Method: AppendCommonUserInformationFromValueDictionary()
    //
    /// <summary>
    /// Appends common GraphML-Attribute values from a user value dictionary
    /// returned by Twitter to a vertex XML node.
    /// </summary>
    ///
    /// <param name="userValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about the user.
    /// </param>
    /// 
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="twitterUser">
    /// Contains the vertex XML node from <paramref
    /// name="graphMLXmlDocument" /> to add the GraphML attribute values to.
    /// </param>
    ///
    /// <remarks>
    /// This method reads information from a value dictionary returned by
    /// Twitter and appends the information to a vertex XML node in the GraphML
    /// document.
    ///
    /// <para>
    /// "Common" means that the information is included in all Twitter
    /// networks.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static void
    AppendCommonUserInformationFromValueDictionary
    (
        Dictionary<String, Object> userValueDictionary,
        GraphMLXmlDocument graphMLXmlDocument,
        TwitterUser twitterUser
    )
    {
        Debug.Assert(userValueDictionary != null);
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(twitterUser != null);

        XmlNode vertexXmlNode = twitterUser.VertexXmlNode;

        // Always include an image file.

        AppendValueFromValueDictionary(userValueDictionary,
            "profile_image_url", graphMLXmlDocument, vertexXmlNode,
            NodeXLGraphMLUtil.VertexImageFileID);
    }

    //*************************************************************************
    //  Method: AppendUserStatisticsFromValueDictionary()
    //
    /// <summary>
    /// Appends GraphML-Attribute statistic values from a user value
    /// dictionary returned by Twitter to a vertex XML node.
    /// </summary>
    ///
    /// <param name="userValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about the user.
    /// </param>
    /// 
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="twitterUser">
    /// Contains the vertex XML node from <paramref
    /// name="graphMLXmlDocument" /> to add the GraphML attribute values to.
    /// </param>
    ///
    /// <remarks>
    /// This method reads statistics information from a value dictionary
    /// returned by Twitter and appends the information to a vertex XML node in
    /// the GraphML document.
    /// </remarks>
    //*************************************************************************

    public static void
    AppendUserStatisticsFromValueDictionary
    (
        Dictionary<String, Object> userValueDictionary,
        GraphMLXmlDocument graphMLXmlDocument,
        TwitterUser twitterUser
    )
    {
        Debug.Assert(userValueDictionary != null);
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(twitterUser != null);

        XmlNode vertexXmlNode = twitterUser.VertexXmlNode;

        AppendValueFromValueDictionary(userValueDictionary,
            "friends_count", graphMLXmlDocument, vertexXmlNode,
            VertexFollowedID);

        AppendValueFromValueDictionary(userValueDictionary,
            "followers_count", graphMLXmlDocument, vertexXmlNode,
            VertexFollowersID);

        AppendValueFromValueDictionary(userValueDictionary,
            "statuses_count", graphMLXmlDocument, vertexXmlNode,
            VertexStatusesID);

        AppendValueFromValueDictionary(userValueDictionary,
            "favourites_count", graphMLXmlDocument, vertexXmlNode,
            VertexFavoritesID);

        AppendValueFromValueDictionary(userValueDictionary,
            "description", graphMLXmlDocument, vertexXmlNode,
            VertexDescriptionID);

        AppendValueFromValueDictionary(userValueDictionary,
            "location", graphMLXmlDocument, vertexXmlNode,
            VertexLocationID);

        AppendValueFromValueDictionary(userValueDictionary,
            "url", graphMLXmlDocument, vertexXmlNode,
            VertexUrlID);

        AppendValueFromValueDictionary(userValueDictionary,
            "time_zone", graphMLXmlDocument, vertexXmlNode,
            VertexTimeZoneID);

        AppendValueFromValueDictionary(userValueDictionary,
            "utc_offset", graphMLXmlDocument, vertexXmlNode,
            VertexUtcOffsetID);

        String joinedDateUtc;

        if ( TwitterJsonUtil.TryGetJsonValueFromDictionary(userValueDictionary,
            "created_at", out joinedDateUtc) )
        {
            graphMLXmlDocument.AppendGraphMLAttributeValue(
                vertexXmlNode, VertexJoinedDateUtcID,
                TwitterDateParser.ParseTwitterDate(joinedDateUtc) );
        }
    }

    //*************************************************************************
    //  Method: AppendValueFromValueDictionary()
    //
    /// <summary>
    /// Appends a GraphML-Attribute value from a value dictionary returned by
    /// Twitter to an edge or vertex XML node.
    /// </summary>
    ///
    /// <param name="valueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.
    /// </param>
    /// 
    /// <param name="name">
    /// The name of the value to get from <paramref name="valueDictionary" />.
    /// </param>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="edgeOrVertexXmlNode">
    /// The edge or vertex XML node from <paramref
    /// name="graphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="graphMLAttributeID">
    /// GraphML ID of the GraphML-Attribute.
    /// </param>
    ///
    /// <returns>
    /// true if the GraphML-Attribute was appended.
    /// </returns>
    ///
    /// <remarks>
    /// This method looks for a value named <paramref name="name" /> in
    /// <paramref name="valueDictionary" />.  If the value is found, it gets
    /// stored on <paramref name="edgeOrVertexXmlNode" /> as a
    /// GraphML-Attribute.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    AppendValueFromValueDictionary
    (
        Dictionary<String, Object> valueDictionary,
        String name,
        GraphMLXmlDocument graphMLXmlDocument,
        XmlNode edgeOrVertexXmlNode,
        String graphMLAttributeID
    )
    {
        Debug.Assert(valueDictionary != null);
        Debug.Assert( !String.IsNullOrEmpty(name) );
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(edgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(graphMLAttributeID) );

        String value;

        if ( TwitterJsonUtil.TryGetJsonValueFromDictionary(
            valueDictionary, name, out value) )
        {
            graphMLXmlDocument.AppendGraphMLAttributeValue(
                edgeOrVertexXmlNode, graphMLAttributeID, value);

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetLatitudeAndLongitudeFromStatusValueDictionary()
    //
    /// <summary>
    /// Attempts to get a latitude and longitude from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="statusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a status.
    /// </param>
    ///
    /// <param name="latitude">
    /// Where the latitude gets stored if it is available.  If it is not
    /// available, this gets set to null.
    /// </param>
    ///
    /// <param name="longitude">
    /// Where the longitude gets stored if it is available.  If it is not
    /// available, this gets set to null.
    /// </param>
    //*************************************************************************

    public static void
    GetLatitudeAndLongitudeFromStatusValueDictionary
    (
        Dictionary<String, Object> statusValueDictionary,
        out String latitude,
        out String longitude
    )
    {
        Debug.Assert(statusValueDictionary != null);

        Object geoAsObject;

        if (
            statusValueDictionary.TryGetValue("geo", out geoAsObject)
            &&
            geoAsObject is Dictionary<String, Object>
            )
        {
            Dictionary<String, Object> geoValueDictionary =
                ( Dictionary<String, Object> )geoAsObject;

            Object coordinatesAsObject;

            if (
                geoValueDictionary.TryGetValue("coordinates",
                    out coordinatesAsObject)
                &&
                coordinatesAsObject is Object[]
                )
            {
                Object [] coordinates = ( Object[] )coordinatesAsObject;

                if (coordinates.Length == 2)
                {
                    TwitterJsonUtil.TryConvertJsonValueToString(
                        coordinates[0], out latitude);

                    TwitterJsonUtil.TryConvertJsonValueToString(
                        coordinates[1], out longitude);

                    return;
                }
            }
        }

        latitude = longitude = null;
    }

    //*************************************************************************
    //  Method: GetUrlsAndHashtagsFromStatusValueDictionary()
    //
    /// <summary>
    /// Attempts to get URLs and hashtags from the entities in a JSON value
    /// dictionary.
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
    /// <param name="urls">
    /// Where the space-delimited URLs get stored if they are available.  If
    /// they are not available, this gets set to null.
    /// </param>
    ///
    /// <param name="hashtags">
    /// Where the space-delimited hashtags get stored if they are available.
    /// If they are not available, this gets set to null.  The hashtags are all
    /// in lower case and do not include a leading pound sign.
    /// </param>
    //*************************************************************************

    public static void
    GetUrlsAndHashtagsFromStatusValueDictionary
    (
        Dictionary<String, Object> statusValueDictionary,
        Boolean expandUrls,
        out String urls,
        out String hashtags
    )
    {
        Debug.Assert(statusValueDictionary != null);

        urls = hashtags = null;

        String [] urlArray =
            TwitterStatusParser.GetUrlsFromStatusValueDictionary(
                statusValueDictionary, expandUrls);

        if (urlArray.Length > 0)
        {
            urls = String.Join(" ", urlArray);
        }

        String [] hashtagArray =
            TwitterStatusParser.GetHashtagsFromStatusValueDictionary(
                statusValueDictionary);

        if (hashtagArray.Length > 0)
        {
            hashtags = String.Join(" ", hashtagArray);
        }
    }

    //*************************************************************************
    //  Method: AppendRepliesToAndMentionsEdgeXmlNodes()
    //
    /// <overloads>
    /// Appends edge XML nodes for replies-to and mentions relationships.
    /// </overloads>
    ///
    /// <summary>
    /// Appends edge XML nodes for replies-to and mentions relationships for
    /// all statuses.
    /// </summary>
    ///
    /// <param name="graphmlXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="twitterUsers">
    /// Collection of the TwitterUsers in the network.
    /// </param>
    ///
    /// <param name="uniqueScreenNames">
    /// Collection of the unique screen names in the network.
    /// </param>
    ///
    /// <param name="includeRepliesToEdges">
    /// true to append edges for replies-to relationships.
    /// </param>
    ///
    /// <param name="includeMentionsEdges">
    /// true to append edges for mentions relationships.
    /// </param>
    ///
    /// <param name="includeNonRepliesToNonMentionsEdges">
    /// true to append edges for tweets that don't reply to or mention anyone.
    /// </param>
    ///
    /// <param name="includeStatuses">
    /// true to include the status in the edge XML nodes.
    /// </param>
    //*************************************************************************

    public static void
    AppendRepliesToAndMentionsEdgeXmlNodes
    (
        GraphMLXmlDocument graphmlXmlDocument,
        IEnumerable<TwitterUser> twitterUsers,
        HashSet<String> uniqueScreenNames,
        Boolean includeRepliesToEdges,
        Boolean includeMentionsEdges,
        Boolean includeNonRepliesToNonMentionsEdges,
        Boolean includeStatuses
    )
    {
        Debug.Assert(graphmlXmlDocument != null);
        Debug.Assert(twitterUsers != null);
        Debug.Assert(uniqueScreenNames != null);

        if (!includeRepliesToEdges && !includeMentionsEdges &&
            !includeNonRepliesToNonMentionsEdges)
        {
            return;
        }

        // This method uses only one instance of TwitterStatusTextParser to
        // avoid making it repeatedly recompile all of its regular expressions.

        TwitterStatusTextParser twitterStatusTextParser =
            new TwitterStatusTextParser();

        foreach (TwitterUser twitterUser in twitterUsers)
        {
            foreach (TwitterStatus twitterStatus in twitterUser.Statuses)
            {
                AppendRepliesToAndMentionsEdgeXmlNodes(
                    graphmlXmlDocument, twitterStatusTextParser,
                    uniqueScreenNames, includeRepliesToEdges,
                    includeMentionsEdges, includeNonRepliesToNonMentionsEdges,
                    twitterUser.ScreenName, twitterStatus, includeStatuses);
            }
        }
    }

    //*************************************************************************
    //  Method: AppendLatitudeAndLongitudeGraphMLAttributeValues()
    //
    /// <summary>
    /// Appends GraphML attribute values for latitude and longitude to an edge
    /// or vertex XML node.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="edgeOrVertexXmlNode">
    /// The edge or vertex XML node to add the Graph-ML attribute values to.
    /// </param>
    ///
    /// <param name="latitude">
    /// The latitude.  Can be null or empty.
    /// </param>
    ///
    /// <param name="longitude">
    /// The longitude.  Can be null or empty.
    /// </param>
    //*************************************************************************

    public static void
    AppendLatitudeAndLongitudeGraphMLAttributeValues
    (
        GraphMLXmlDocument graphMLXmlDocument,
        XmlNode edgeOrVertexXmlNode,
        String latitude,
        String longitude
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(edgeOrVertexXmlNode != null);

        if ( !String.IsNullOrEmpty(latitude) )
        {
            graphMLXmlDocument.AppendGraphMLAttributeValue(
                edgeOrVertexXmlNode, LatitudeID, latitude);
        }

        if ( !String.IsNullOrEmpty(longitude) )
        {
            graphMLXmlDocument.AppendGraphMLAttributeValue(
                edgeOrVertexXmlNode, LongitudeID, longitude);
        }
    }

    //*************************************************************************
    //  Method: AppendInReplyToStatusIDGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a GraphML attribute value for an in-reply-to status ID.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="edgeOrVertexXmlNode">
    /// The edge or vertex XML node to add the Graph-ML attribute value to.
    /// </param>
    ///
    /// <param name="inReplyToStatusID">
    /// The in-reply-to status ID.  Can be null or empty.
    /// </param>
    //*************************************************************************

    public static void
    AppendInReplyToStatusIDGraphMLAttributeValue
    (
        GraphMLXmlDocument graphMLXmlDocument,
        XmlNode edgeOrVertexXmlNode,
        String inReplyToStatusID
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(edgeOrVertexXmlNode != null);

        if ( !String.IsNullOrEmpty(inReplyToStatusID) )
        {
            // Precede the ID with a single quote to force Excel to treat the
            // ID as text.  Otherwise, it formats the ID, which is a large
            // number, in scientific notation.

            graphMLXmlDocument.AppendGraphMLAttributeValue(
                edgeOrVertexXmlNode, InReplyToStatusIDID,
                "'" + inReplyToStatusID);
        }
    }

    //*************************************************************************
    //  Method: UrlsToDomains()
    //
    /// <summary>
    /// Extracts the domains from a string of space-delimited URLs.
    /// </summary>
    ///
    /// <param name="spaceDelimitedUrls">
    /// URLs delimited by spaces.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// Space-delimited domains, one per space-delimited URL.
    /// </returns>
    //*************************************************************************

    public static String
    UrlsToDomains
    (
        String spaceDelimitedUrls
    )
    {
        Debug.Assert(spaceDelimitedUrls != null);

        StringBuilder domains = new StringBuilder();

        foreach ( String url in spaceDelimitedUrls.Split(new Char[]{' '},
            StringSplitOptions.RemoveEmptyEntries) )
        {
            String domain;

            if ( UrlUtil.TryGetDomainFromUrl(url, out domain) )
            {
                domains.AppendFormat(
                    "{0}{1}",
                    domains.Length > 0 ? " " : String.Empty,
                    domain
                    );
            }
        }

        return ( domains.ToString() );
    }

    //*************************************************************************
    //  Method: TwitterUsersToUniqueScreenNames()
    //
    /// <summary>
    /// Creates a collection of unique screen names from a collection of
    /// TwitterUser objects.
    /// </summary>
    ///
    /// <param name="twitterUsers">
    /// Collection of the TwitterUsers in the network.
    /// </param>
    ///
    /// <returns>
    /// A collection of unique screen names. 
    /// </returns>
    //*************************************************************************

    public static HashSet<String>
    TwitterUsersToUniqueScreenNames
    (
        IEnumerable<TwitterUser> twitterUsers
    )
    {
        Debug.Assert(twitterUsers != null);

        HashSet<String> uniqueScreenNames = new HashSet<String>();

        foreach (TwitterUser twitterUser in twitterUsers)
        {
            uniqueScreenNames.Add(twitterUser.ScreenName);
        }

        return (uniqueScreenNames);
    }

    //*************************************************************************
    //  Method: DefineLatitudeAndLongitudeGraphMLAttributes()
    //
    /// <summary>
    /// Defines GraphML-Attributes for latitude and longitude.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="forEdges">
    /// true if the attributes are for edges, false if they are for vertices.
    /// </param>
    //*************************************************************************

    private static void
    DefineLatitudeAndLongitudeGraphMLAttributes
    (
        GraphMLXmlDocument graphMLXmlDocument,
        Boolean forEdges
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineStringGraphMLAttributes(forEdges,
            LatitudeID, "Latitude",
            LongitudeID, "Longitude"
            );
    }

    //*************************************************************************
    //  Method: AppendRepliesToAndMentionsEdgeXmlNodes()
    //
    /// <summary>
    /// Appends edge XML nodes for replies-to and mentions relationships for
    /// one status.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="twitterStatusTextParser">
    /// Parses the text of a Twitter status.
    /// </param>
    ///
    /// <param name="uniqueScreenNames">
    /// Collection of the unique screen names in the network.
    /// </param>
    ///
    /// <param name="includeRepliesToEdges">
    /// true to append edges for replies-to relationships.
    /// </param>
    ///
    /// <param name="includeMentionsEdges">
    /// true to append edges for mentions relationships.
    /// </param>
    ///
    /// <param name="includeNonRepliesToNonMentionsEdges">
    /// true to append edges for tweets that don't reply to or mention anyone.
    /// </param>
    ///
    /// <param name="screenName">
    /// The user's screen name.
    /// </param>
    ///
    /// <param name="twitterStatus">
    /// One of the user's statuses.
    /// </param>
    ///
    /// <param name="includeStatus">
    /// true to include the status in the edge XML nodes.
    /// </param>
    //*************************************************************************

    private static void
    AppendRepliesToAndMentionsEdgeXmlNodes
    (
        GraphMLXmlDocument graphMLXmlDocument,
        TwitterStatusTextParser twitterStatusTextParser,
        HashSet<String> uniqueScreenNames,
        Boolean includeRepliesToEdges,
        Boolean includeMentionsEdges,
        Boolean includeNonRepliesToNonMentionsEdges,
        String screenName,
        TwitterStatus twitterStatus,
        Boolean includeStatus
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(twitterStatusTextParser != null);
        Debug.Assert(uniqueScreenNames != null);
        Debug.Assert( !String.IsNullOrEmpty(screenName) );
        Debug.Assert(twitterStatus != null);

        String statusText = twitterStatus.Text;
        Boolean isReplyTo = false;
        Boolean isMentions = false;

        Debug.Assert( !String.IsNullOrEmpty(statusText) );

        String repliedToScreenName;
        String [] uniqueMentionedScreenNames;

        twitterStatusTextParser.GetScreenNames(statusText,
            out repliedToScreenName, out uniqueMentionedScreenNames);

        if (repliedToScreenName != null)
        {
            if (
                repliedToScreenName != screenName
                &&
                uniqueScreenNames.Contains(repliedToScreenName)
                )
            {
                isReplyTo = true;

                if (includeRepliesToEdges)
                {
                    AppendRepliesToAndMentionsEdgeXmlNode(
                        graphMLXmlDocument, screenName, repliedToScreenName,
                        RepliesToRelationship, twitterStatus, includeStatus);
                }
            }
        }

        foreach (String mentionedScreenName in uniqueMentionedScreenNames)
        {
            if (
                mentionedScreenName != screenName
                &&
                uniqueScreenNames.Contains(mentionedScreenName)
                )
            {
                isMentions = true;

                if (includeMentionsEdges)
                {
                    AppendRepliesToAndMentionsEdgeXmlNode(
                        graphMLXmlDocument, screenName, mentionedScreenName,
                        MentionsRelationship, twitterStatus, includeStatus);
                }
            }
        }

        if (includeNonRepliesToNonMentionsEdges && !isReplyTo && !isMentions)
        {
            // Append a self-loop edge to represent the tweet.

            AppendRepliesToAndMentionsEdgeXmlNode(
                graphMLXmlDocument, screenName, screenName,
                NonRepliesToNonMentionsRelationship, twitterStatus,
                includeStatus);
        }
    }

    //*************************************************************************
    //  Method: AppendRepliesToAndMentionsEdgeXmlNode()
    //
    /// <summary>
    /// Appends an edge XML node for a replies-to, mentions, or non-replies-to-
    /// non-mentions relationship for one status.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="screenName1">
    /// The edge's first screen name.
    /// </param>
    ///
    /// <param name="screenName2">
    /// The edge's second screen name.
    /// </param>
    ///
    /// <param name="relationship">
    /// A description of the relationship represented by the edge.
    /// </param>
    ///
    /// <param name="twitterStatus">
    /// A twitter status.
    /// </param>
    ///
    /// <param name="includeStatus">
    /// true to include the status in the edge XML node.
    /// </param>
    //*************************************************************************

    private static void
    AppendRepliesToAndMentionsEdgeXmlNode
    (
        GraphMLXmlDocument graphMLXmlDocument,
        String screenName1,
        String screenName2,
        String relationship,
        TwitterStatus twitterStatus,
        Boolean includeStatus
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(screenName1) );
        Debug.Assert( !String.IsNullOrEmpty(screenName2) );
        Debug.Assert( !String.IsNullOrEmpty(relationship) );
        Debug.Assert(twitterStatus != null);

        XmlNode edgeXmlNode = NodeXLGraphMLUtil.AppendEdgeXmlNode(
            graphMLXmlDocument, screenName1, screenName2, relationship);

        String statusDateUtc = twitterStatus.ParsedDateUtc;
        Boolean hasStatusDateUtc = !String.IsNullOrEmpty(statusDateUtc);

        if (hasStatusDateUtc)
        {
            // The status's date is the relationship date.

            graphMLXmlDocument.AppendGraphMLAttributeValue(edgeXmlNode,
                EdgeRelationshipDateUtcID, statusDateUtc);
        }

        if (includeStatus)
        {
            String statusText = twitterStatus.Text;

            graphMLXmlDocument.AppendGraphMLAttributeValue(edgeXmlNode,
                EdgeStatusID, statusText);

            String urls = twitterStatus.Urls;

            if ( !String.IsNullOrEmpty(urls) )
            {
                graphMLXmlDocument.AppendGraphMLAttributeValue(edgeXmlNode,
                    EdgeStatusUrlsID, urls);

                graphMLXmlDocument.AppendGraphMLAttributeValue( edgeXmlNode,
                    EdgeStatusDomainsID, UrlsToDomains(urls) );
            }

            if ( !String.IsNullOrEmpty(twitterStatus.Hashtags) )
            {
                graphMLXmlDocument.AppendGraphMLAttributeValue(edgeXmlNode,
                    EdgeStatusHashtagsID, twitterStatus.Hashtags);
            }

            if (hasStatusDateUtc)
            {
                graphMLXmlDocument.AppendGraphMLAttributeValue(edgeXmlNode,
                    EdgeStatusDateUtcID, statusDateUtc);
            }

            graphMLXmlDocument.AppendGraphMLAttributeValue(edgeXmlNode,
                EdgeStatusWebPageUrlID, 

                FormatStatusWebPageUrl(screenName1, twitterStatus)
                );

            AppendLatitudeAndLongitudeGraphMLAttributeValues(
                graphMLXmlDocument, edgeXmlNode, twitterStatus.Latitude,
                twitterStatus.Longitude);

            // Precede the ID with a single quote to force Excel to treat the
            // ID as text.  Otherwise, it formats the ID, which is a large
            // number, in scientific notation.

            graphMLXmlDocument.AppendGraphMLAttributeValue(edgeXmlNode,
                NodeXLGraphMLUtil.ImportedIDID, "'" + twitterStatus.ID);

            AppendInReplyToStatusIDGraphMLAttributeValue(graphMLXmlDocument,
                edgeXmlNode, twitterStatus.InReplyToStatusID);
        }
    }

    //*************************************************************************
    //  Method: FormatStatusWebPageUrl()
    //
    /// <summary>
    /// Formats a string to use for a status Web page URL.
    /// </summary>
    ///
    /// <param name="screenName">
    /// The status's author.
    /// </param>
    ///
    /// <param name="twitterStatus">
    /// The twitter status.
    /// </param>
    //*************************************************************************

    private static String
    FormatStatusWebPageUrl
    (
        String screenName,
        TwitterStatus twitterStatus
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(screenName) );
        Debug.Assert(twitterStatus != null);

        return ( String.Format(
            TwitterApiUrls.StatusWebPageUrlPattern
            ,
            screenName,
            twitterStatus.ID
            ) );
    }


    //*************************************************************************
    //  Public GraphML-attribute IDs for edges and vertices
    //*************************************************************************

    ///
    public const String EdgeRelationshipDateUtcID = "RelationshipDateUtc";
    ///
    public const String VertexLatestStatusID = "LatestStatus";
    ///
    public const String VertexLatestStatusUrlsID = "LatestStatusUrls";
    ///
    public const String VertexLatestStatusDomainsID = "LatestStatusDomains";
    ///
    public const String VertexLatestStatusHashtagsID = "LatestStatusHashtags";
    ///
    public const String VertexLatestStatusDateUtcID = "LatestStatusDateUtc";


    //*************************************************************************
    //  Public GraphML-attribute values for relationship descriptions
    //*************************************************************************

    ///
    public const String RepliesToRelationship = "Replies to";
    ///
    public const String MentionsRelationship = "Mentions";
    ///
    public const String NonRepliesToNonMentionsRelationship = "Tweet";


    //*************************************************************************
    //  Private GraphML-attribute IDs for edges and vertices
    //*************************************************************************

    ///
    private const String EdgeStatusID = "Status";
    ///
    private const String EdgeStatusDateUtcID = "StatusDateUtc";
    ///
    private const String EdgeStatusUrlsID = "StatusUrls";
    ///
    private const String EdgeStatusDomainsID = "StatusDomains";
    ///
    private const String EdgeStatusHashtagsID = "StatusHashtags";
    ///
    private const String EdgeStatusWebPageUrlID = "StatusWebPageUrl";
    ///
    private const String VertexFollowedID = "Followed";
    ///
    private const String VertexFollowersID = "Followers";
    ///
    private const String VertexStatusesID = "Statuses";
    ///
    private const String VertexFavoritesID = "Favorites";
    ///
    private const String VertexUtcOffsetID = "UtcOffset";
    ///
    private const String VertexDescriptionID = "Description";
    ///
    private const String VertexLocationID = "Location";
    ///
    private const String VertexUrlID = "Url";
    ///
    private const String VertexTimeZoneID = "TimeZone";
    ///
    private const String VertexJoinedDateUtcID = "JoinedDateUtc";


    //*************************************************************************
    //  Private GraphML-attribute IDs for edges or vertices
    //*************************************************************************

    ///
    private const String LatitudeID = "Latitude";
    ///
    private const String LongitudeID = "Longitude";
    ///
    private const String InReplyToStatusIDID = "InReplyToStatusID";
}

}
