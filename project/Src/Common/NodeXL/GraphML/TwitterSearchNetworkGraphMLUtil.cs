
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.SocialNetworkLib.Twitter;
using Smrf.AppLib;
using Smrf.XmlLib;

namespace Smrf.NodeXL.GraphMLLib
{
//*****************************************************************************
//  Class: TwitterSearchNetworkGraphMLUtil
//
/// <summary>
/// Utility methods for creating Twitter search network GraphML XML documents
/// for use with the NodeXL Excel Template.
/// </summary>
//*****************************************************************************

public static class TwitterSearchNetworkGraphMLUtil : Object
{
    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of Twitter users.
    /// </summary>
    ///
    /// <param name="includeStatistics">
    /// true to include each user's statistics on the vertex worksheet.
    /// </param>
    ///
    /// <param name="includeStatuses">
    /// true to include each status on the edge worksheet.
    /// </param>
    ///
    /// <returns>
    /// A GraphMLXmlDocument containing a Twitter search network.  The document
    /// includes GraphML-attribute definitions but no vertices or edges.
    /// </returns>
    //*************************************************************************

    public static GraphMLXmlDocument
    CreateGraphMLXmlDocument
    (
        Boolean includeStatistics,
        Boolean includeStatuses
    )
    {
        GraphMLXmlDocument graphMLXmlDocument = new GraphMLXmlDocument(true);

        if (includeStatistics)
        {
            TwitterGraphMLUtil.DefineVertexStatisticsGraphMLAttributes(
                graphMLXmlDocument);
        }

        TwitterGraphMLUtil.DefineCommonGraphMLAttributes(graphMLXmlDocument);

        graphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            VertexTweetedSearchTermID, "Tweeted Search Term?");

        graphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            VertexToolTipID, "Tooltip");

        if (includeStatuses)
        {
            TwitterGraphMLUtil.DefineEdgeStatusGraphMLAttributes(
                graphMLXmlDocument);
        }

        return (graphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: TryAppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node to the GraphML document for a person if such
    /// a node doesn't already exist.
    /// </summary>
    ///
    /// <param name="userValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a user.
    /// </param>
    ///
    /// <param name="includeStatistics">
    /// true to include the user's statistics as GraphML-Attribute values.
    /// </param>
    ///
    /// <param name="userTweetedSearchTerm">
    /// true if the user tweeted the search term.
    /// </param>
    ///
    /// <param name="graphMLXmlDocument">
    /// The GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="userIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="twitterUser">
    /// If true is returned, this is where the TwitterUser that represents the
    /// user gets stored.  This gets set regardless of whether the node already
    /// existed.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="userValueDictionary" /> contained a valid user.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryAppendVertexXmlNode
    (
        Dictionary<String, Object> userValueDictionary,
        Boolean includeStatistics,
        Boolean userTweetedSearchTerm,
        GraphMLXmlDocument graphMLXmlDocument,
        Dictionary<String, TwitterUser> userIDDictionary,
        out TwitterUser twitterUser
    )
    {
        Debug.Assert(userValueDictionary != null);
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(userIDDictionary != null);

        twitterUser = null;

        String screenName, userID;

        if (
            !TwitterJsonUtil.TryGetJsonValueFromDictionary(userValueDictionary,
                "screen_name", out screenName)
            ||
            !TwitterJsonUtil.TryGetJsonValueFromDictionary(userValueDictionary,
                "id_str", out userID)
            )
        {
            return (false);
        }

        screenName = screenName.ToLower();

        Boolean isFirstTweetForAuthor =
            TwitterGraphMLUtil.TryAppendVertexXmlNode(
                screenName, userID, graphMLXmlDocument, userIDDictionary,
                out twitterUser);

        if (isFirstTweetForAuthor)
        {
            TwitterGraphMLUtil.AppendCommonUserInformationFromValueDictionary(
                userValueDictionary, graphMLXmlDocument, twitterUser);

            if (includeStatistics)
            {
                TwitterGraphMLUtil.AppendUserStatisticsFromValueDictionary(
                    userValueDictionary, graphMLXmlDocument, twitterUser);
            }

            AppendTweetedSearchTermGraphMLAttributeValue(
                graphMLXmlDocument, twitterUser, userTweetedSearchTerm);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: AppendVertexTooltipXmlNodes()
    //
    /// <summary>
    /// Appends a vertex tooltip XML node for each person in the network.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// The GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="userIDDictionary">
    /// The key is the user ID and the value is the corresponding TwitterUser.
    /// </param>
    //*************************************************************************

    public static void
    AppendVertexTooltipXmlNodes
    (
        GraphMLXmlDocument graphMLXmlDocument,
        Dictionary<String, TwitterUser> userIDDictionary
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(userIDDictionary != null);

        foreach (TwitterUser twitterUser in userIDDictionary.Values)
        {
            // The tooltip is the user's screen name followed by the first
            // tweet returned by Twitter.

            ICollection<TwitterStatus> statuses = twitterUser.Statuses;

            String firstStatus = statuses.Count > 0 ?
                statuses.First().Text : String.Empty;

            // The Excel template doesn't wrap long tooltip text.  Break the
            // status into lines so the entire tooltip will show in the graph
            // pane.

            firstStatus = StringUtil.BreakIntoLines(firstStatus, 30);

            String toolTip = String.Format(
                "{0}\n\n{1}"
                ,
                twitterUser.ScreenName,
                firstStatus
                );

            graphMLXmlDocument.AppendGraphMLAttributeValue(
                twitterUser.VertexXmlNode, VertexToolTipID, toolTip);
        }
    }

    //*************************************************************************
    //  Method: TryAddStatusToUser()
    //
    /// <summary>
    /// Attempts to parse a status and add it to a user's status collection.
    /// </summary>
    ///
    /// <param name="statusValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a status.
    /// </param>
    ///
    /// <param name="twitterUser">
    /// The user to add the status to.
    /// </param>
    ///
    /// <param name="expandStatusUrls">
    /// true to expand all URLs that might be shortened URLs.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryAddStatusToUser
    (
        Dictionary<String, Object> statusValueDictionary,
        TwitterUser twitterUser,
        Boolean expandStatusUrls
    )
    {
        Debug.Assert(statusValueDictionary != null);
        Debug.Assert(twitterUser != null);

		TwitterStatus twitterStatus;

		if ( !TwitterStatus.TryFromStatusValueDictionary(statusValueDictionary,
			expandStatusUrls, out twitterStatus) )
        {
            return (false);
        }

        twitterUser.Statuses.Add(twitterStatus);

        return (true);
    }

    //*************************************************************************
    //  Method: GetMentionsAndRepliesToScreenNames()
    //
    /// <summary>
    /// Gets the screen names that were mentioned or replied to by the people
    /// who tweeted the search term.
    /// </summary>
    ///
    /// <param name="userIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <returns>
    /// An array of screen names.  The names are unique.
    /// </returns>
    //*************************************************************************

    public static String[]
    GetMentionsAndRepliesToScreenNames
    (
        Dictionary<String, TwitterUser> userIDDictionary
    )
    {
        Debug.Assert(userIDDictionary != null);

        HashSet<String> uniqueScreenNamesWhoTweetedSearchTerm =
            TwitterGraphMLUtil.TwitterUsersToUniqueScreenNames(
                userIDDictionary.Values);

        HashSet<String> uniqueMentionsAndRepliesToScreenNames =
            new HashSet<String>();

        TwitterStatusTextParser twitterStatusTextParser =
            new TwitterStatusTextParser();

        foreach (TwitterUser twitterUser in userIDDictionary.Values)
        {
            foreach (TwitterStatus twitterStatus in twitterUser.Statuses)
            {
                String repliesToScreenName;
                String [] uniqueMentionsScreenNames;

                twitterStatusTextParser.GetScreenNames(twitterStatus.Text,
                    out repliesToScreenName,
                    out uniqueMentionsScreenNames);

                if (
                    repliesToScreenName != null
                    &&
                    !uniqueScreenNamesWhoTweetedSearchTerm.Contains(
                        repliesToScreenName) )
                {
                    uniqueMentionsAndRepliesToScreenNames.Add(
                        repliesToScreenName);
                }

                foreach (String uniqueMentionsScreenName in
                    uniqueMentionsScreenNames)
                {
                    if ( !uniqueScreenNamesWhoTweetedSearchTerm.Contains(
                        uniqueMentionsScreenName) )
                    {
                        uniqueMentionsAndRepliesToScreenNames.Add(
                            uniqueMentionsScreenName);
                    }
                }
            }
        }

        return ( uniqueMentionsAndRepliesToScreenNames.ToArray() );
    }

    //*************************************************************************
    //  Method: AppendTweetedSearchTermGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a GraphML attribute value to a vertex XML node that indicates
    /// whether the user tweeted the search term.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="twitterUser">
    /// Contains the vertex XML node from <paramref
    /// name="graphMLXmlDocument" /> to add the GraphML attribute value to.
    /// </param>
    ///
    /// <param name="userTweetedSearchTerm">
    /// true if the user tweeted the search term.
    /// </param>
    //*************************************************************************

    private static void
    AppendTweetedSearchTermGraphMLAttributeValue
    (
        GraphMLXmlDocument graphMLXmlDocument,
        TwitterUser twitterUser,
        Boolean userTweetedSearchTerm
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(twitterUser != null);

        graphMLXmlDocument.AppendGraphMLAttributeValue(
            twitterUser.VertexXmlNode,
            VertexTweetedSearchTermID,
            userTweetedSearchTerm ? "Yes" : "No");
    }


    //*************************************************************************
    //  Public GraphML-attribute IDs for vertices
    //*************************************************************************

    ///
    public const String VertexToolTipID = "Tooltip";
    ///
    public const String VertexTweetedSearchTermID = "TweetedSearchTerm";
}

}
