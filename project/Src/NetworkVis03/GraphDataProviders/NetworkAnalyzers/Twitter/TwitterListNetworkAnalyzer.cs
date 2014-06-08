
using System;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.XmlLib;
using Smrf.SocialNetworkLib;
using Smrf.SocialNetworkLib.Twitter;
using Smrf.NodeXL.GraphMLLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterListNetworkAnalyzer
//
/// <summary>
/// Gets a network that shows the connections between a set of Twitter users
/// specified by either a single Twitter List name or by a set of Twitter
/// screen names.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync" /> to asynchronously get the network.
/// </remarks>
//*****************************************************************************

public class TwitterListNetworkAnalyzer : TwitterNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterListNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterListNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterListNetworkAnalyzer()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Enum: WhatToInclude
    //
    /// <summary>
    /// Flags that specify what should be included in a network requested from
    /// this class.
    /// </summary>
    ///
    /// <remarks>
    /// The flags can be ORed together.
    /// </remarks>
    //*************************************************************************

    [System.FlagsAttribute]

    public enum
    WhatToInclude
    {
        /// <summary>
        /// Include nothing.
        /// </summary>

        None = 0,

        /// <summary>
        /// Include each person's latest status.
        /// </summary>

        LatestStatuses = 1,

        /// <summary>
        /// Expand the URLs contained within each person's latest status.  Used
        /// only if <see cref="LatestStatuses" /> is specified.
        /// </summary>

        ExpandedLatestStatusUrls = 2,

        /// <summary>
        /// Include each person's statistics.
        /// </summary>

        Statistics = 4,

        /// <summary>
        /// Include an edge for each followed relationship.
        /// </summary>

        FollowedEdges = 8,

        /// <summary>
        /// Include an edge from person A to person B if person A's tweet is a
        /// reply to person B.
        /// </summary>

        RepliesToEdges = 16,

        /// <summary>
        /// Include an edge from person A to person B if person A's tweet
        /// mentions person B.
        /// </summary>

        MentionsEdges = 32,
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets a network that shows the connections between a set
    /// of Twitter users specified by either a single Twitter List name or by a
    /// set of Twitter screen names.
    /// </summary>
    ///
    /// <param name="useListName">
    /// If true, <paramref name="listName" /> must be specified and <paramref
    /// name="screenNames" /> is ignored.  If false, <paramref
    /// name="screenNames" /> must be specified and <paramref
    /// name="listName" /> is ignored.
    /// </param>
    ///
    /// <param name="listName">
    /// Twitter List name if <paramref name="useListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="screenNames">
    /// Zero or more Twitter screen names if <paramref name="useListName" /> is
    /// false; unused otherwise.  The screen names can be in any case, but they
    /// get converted to lower case before getting the network.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <remarks>
    /// When the analysis completes, the <see
    /// cref="HttpNetworkAnalyzerBase.AnalysisCompleted" /> event fires.  The
    /// <see cref="RunWorkerCompletedEventArgs.Result" /> property will return
    /// an XmlDocument containing the network as GraphML.
    ///
    /// <para>
    /// To cancel the analysis, call <see
    /// cref="HttpNetworkAnalyzerBase.CancelAsync" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    GetNetworkAsync
    (
        Boolean useListName,
        String listName,
        ICollection<String> screenNames,
        WhatToInclude whatToInclude
    )
    {
        Debug.Assert( !useListName || !String.IsNullOrEmpty(listName) );
        Debug.Assert(useListName || screenNames != null);
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.UseListName = useListName;
        oGetNetworkAsyncArgs.ListName = listName;
        oGetNetworkAsyncArgs.ScreenNames = screenNames;
        oGetNetworkAsyncArgs.WhatToInclude = whatToInclude;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: GetNetwork()
    //
    /// <summary>
    /// Synchronously gets a network that shows the connections between a set
    /// of Twitter users specified by either a single Twitter List name or by a
    /// set of Twitter screen names.
    /// </summary>
    ///
    /// <param name="useListName">
    /// If true, <paramref name="listName" /> must be specified and <paramref
    /// name="screenNames" /> is ignored.  If false, <paramref
    /// name="screenNames" /> must be specified and <paramref
    /// name="listName" /> is ignored.
    /// </param>
    ///
    /// <param name="listName">
    /// Twitter List name if <paramref name="useListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="screenNames">
    /// Zero or more Twitter screen names if <paramref name="useListName" /> is
    /// false; unused otherwise.  The screen names can be in any case, but they
    /// get converted to lower case before getting the network.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    public XmlDocument
    GetNetwork
    (
        Boolean useListName,
        String listName,
        ICollection<String> screenNames,
        WhatToInclude whatToInclude
    )
    {
        Debug.Assert( !useListName || !String.IsNullOrEmpty(listName) );
        Debug.Assert(useListName || screenNames != null);
        AssertValid();

        return ( GetNetworkInternal(useListName, listName, screenNames,
            whatToInclude) );
    }

    //*************************************************************************
    //  Method: TryParseListName()
    //
    /// <summary>
    /// Attempts to parse a Twitter list name into a slug and owner screen
    /// name.
    /// </summary>
    ///
    /// <param name="listName">
    /// List name in the format "bob/twitterit".  Can't be null.
    /// </param>
    ///
    /// <param name="slug">
    /// Where the slug gets stored if true is returned.  Sample: "twitterit".
    /// </param>
    ///
    /// <param name="ownerScreenName">
    /// Where the owner screen name gets stored if true is returned.  Sample:
    /// "bob".
    /// </param>
    ///
    /// <returns>
    /// true if the list name was successfully parsed, false if the list name
    /// is not valid.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryParseListName
    (
        String listName,
        out String slug,
        out String ownerScreenName
    )
    {
        Debug.Assert(listName != null);

        // Twitter screen names: alphanumeric or underscore, 15 characters
        // maximum.
        //
        // Twitter list names: alphanumeric, hyphen or underscore, 25
        // characters maximum.
        //
        // Note: "\w" means "alphanumeric or underscore."

        Regex oRegex = new Regex(@"^(?<OwnerScreenName>\w+)/(?<Slug>(\w|-)+)$");

        Match oMatch = oRegex.Match(listName);

        if (oMatch.Success)
        {
            slug = oMatch.Groups["Slug"].Value;
            ownerScreenName = oMatch.Groups["OwnerScreenName"].Value;

            return (true);
        }

        slug = ownerScreenName = null;
        return (false);
    }

    //*************************************************************************
    //  Method: GetNetworkInternal()
    //
    /// <overloads>
    /// Gets the requested network.
    /// </overloads>
    ///
    /// <summary>
    /// Gets the requested network.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetNetworkInternal
    (
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        WhatToInclude eWhatToInclude
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        AssertValid();

        BeforeGetNetwork();

        Boolean bIncludeStatistics = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.Statistics);

        Boolean bIncludeLatestStatuses = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.LatestStatuses);

        GraphMLXmlDocument oGraphMLXmlDocument = CreateGraphMLXmlDocument(
            bIncludeStatistics, bIncludeLatestStatuses);

        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            GetNetworkInternal(bUseListName, sListName, oScreenNames,
                eWhatToInclude, oRequestStatistics, oGraphMLXmlDocument);
        }
        catch (Exception oException)
        {
            OnUnexpectedException(oException, oGraphMLXmlDocument,
                oRequestStatistics);
        }

        OnNetworkObtained(oGraphMLXmlDocument, oRequestStatistics, 

            GetNetworkDescription(bUseListName, sListName, oScreenNames,
                eWhatToInclude),

            "Twitter List " + (bUseListName ? sListName : "Usernames")
            );

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of Twitter users.
    /// </summary>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include each user's statistics.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatuses">
    /// true to include each user's latest status.
    /// </param>
    ///
    /// <returns>
    /// A GraphMLXmlDocument representing a network of Twitter users.  The
    /// document includes GraphML-attribute definitions but no vertices or
    /// edges.
    /// </returns>
    //*************************************************************************

    protected GraphMLXmlDocument
    CreateGraphMLXmlDocument
    (
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatuses
    )
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        if (bIncludeStatistics)
        {
            TwitterGraphMLUtil.DefineVertexStatisticsGraphMLAttributes(
                oGraphMLXmlDocument);
        }

        if (bIncludeLatestStatuses)
        {
            TwitterGraphMLUtil.DefineVertexLatestStatusGraphMLAttributes(
                oGraphMLXmlDocument);
        }

        TwitterGraphMLUtil.DefineCommonGraphMLAttributes(oGraphMLXmlDocument);

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: GetNetworkInternal()
    //
    /// <summary>
    /// Gets the requested network, given a GraphMLXmlDocument.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument to populate with the requested network.
    /// </param>
    //*************************************************************************

    protected void
    GetNetworkInternal
    (
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        WhatToInclude eWhatToInclude,
        RequestStatistics oRequestStatistics,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        // The key is the Twitter user ID and the value is the corresponding
        // TwitterUser.

        Dictionary<String, TwitterUser> oUserIDDictionary =
            new Dictionary<String, TwitterUser>();

        AppendVertexXmlNodes(bUseListName, sListName, oScreenNames,
            eWhatToInclude, oGraphMLXmlDocument, oUserIDDictionary,
            oRequestStatistics);

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowedEdges) )
        {
            AppendFollowedOrFollowingEdgeXmlNodes(oUserIDDictionary, true,
                Int32.MaxValue, oGraphMLXmlDocument, oRequestStatistics);
        }

        // Add edges nodes for replies-to and mentions relationships.

        AppendRepliesToAndMentionsEdgeXmlNodes(oGraphMLXmlDocument,
            oUserIDDictionary.Values,

            TwitterGraphMLUtil.TwitterUsersToUniqueScreenNames(
                oUserIDDictionary.Values),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.RepliesToEdges),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.MentionsEdges),

            false, false
            );
    }

    //*************************************************************************
    //  Method: AppendVertexXmlNodes()
    //
    /// <summary>
    /// Appends a vertex XML node for each person in the network.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertexXmlNodes
    (
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        WhatToInclude eWhatToInclude,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        Boolean bIncludeStatistics = WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.Statistics);

        Boolean bIncludeLatestStatuses = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.LatestStatuses);

        Boolean bExpandLatestStatusUrls = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.ExpandedLatestStatusUrls);

        String sUserID;

        if (bUseListName)
        {
            String sSlug, sOwnerScreenName;

            if ( !TryParseListName(sListName, out sSlug,
                out sOwnerScreenName) )
            {
                return;
            }

            String sUrl = String.Format(

                "{0}lists/members.json?slug={1}&owner_screen_name={2}&{3}"
                ,
                TwitterApiUrls.Rest,
                TwitterUtil.EncodeUrlParameter(sSlug),
                TwitterUtil.EncodeUrlParameter(sOwnerScreenName),
                TwitterApiUrlParameters.IncludeEntities
                );

            // The JSON contains a "users" array for the users in the Twitter
            // list.

            foreach ( Object oResult in EnumerateJsonValues(sUrl, "users",
                Int32.MaxValue, false, oRequestStatistics) )
            {
                String sScreenName;

                Dictionary<String, Object> oUserValueDictionary =
                    ( Dictionary<String, Object> )oResult;

                if (
                    TryGetScreenNameFromDictionary(oUserValueDictionary,
                        out sScreenName)
                    &&
                    TryGetUserIDFromDictionary(oUserValueDictionary,
                        out sUserID)
                    )
                {
                    AppendVertexXmlNode(sScreenName, sUserID,
                        oGraphMLXmlDocument, oUserIDDictionary,
                        oUserValueDictionary, bIncludeStatistics,
                        bIncludeLatestStatuses, bExpandLatestStatusUrls);
                }
            }
        }
        else
        {
            // Eliminate duplicate names.

            HashSet<String> oUniqueScreenNames = new HashSet<String>();

            foreach (String sScreenName in oScreenNames)
            {
                oUniqueScreenNames.Add( sScreenName.ToLower() );
            }

            foreach (String sScreenName in oUniqueScreenNames)
            {
                Dictionary<String, Object> oUserValueDictionary;

                if (
                    TryGetUserValueDictionary(sScreenName, oRequestStatistics,
                        true, out oUserValueDictionary)
                    &&
                    TryGetUserIDFromDictionary(oUserValueDictionary,
                        out sUserID)
                    )
                {
                    AppendVertexXmlNode(sScreenName, sUserID,
                        oGraphMLXmlDocument, oUserIDDictionary,
                        oUserValueDictionary, bIncludeStatistics,
                        bIncludeLatestStatuses, bExpandLatestStatusUrls);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: AppendVertexXmlNode()
    //
    /// <summary>
    /// Appends a vertex XML node for one person in the network.
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The user's screen name.
    /// </param>
    ///
    /// <param name="sUserID">
    /// The user's ID.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oUserValueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.  Contains
    /// information about a user.
    /// </param>
    ///
    /// <param name="bIncludeStatistics">
    /// true to include the user's statistics as GraphML-Attribute values.
    /// </param>
    ///
    /// <param name="bIncludeLatestStatus">
    /// true to include the user's latest status as a GraphML-Attribute value.
    /// </param>
    ///
    /// <param name="bExpandLatestStatusUrls">
    /// true to expand all URLs in the latest status that might be shortened
    /// URLs.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertexXmlNode
    (
        String sScreenName,
        String sUserID,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        Dictionary<String, Object> oUserValueDictionary,
        Boolean bIncludeStatistics,
        Boolean bIncludeLatestStatus,
        Boolean bExpandLatestStatusUrls
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert( !String.IsNullOrEmpty(sUserID) );
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oUserValueDictionary != null);
        AssertValid();

        TwitterUser oTwitterUser;

        TwitterGraphMLUtil.TryAppendVertexXmlNode(sScreenName, sUserID,
            oGraphMLXmlDocument, oUserIDDictionary, out oTwitterUser);

        AppendUserInformationFromValueDictionary(oUserValueDictionary,
            oGraphMLXmlDocument, oTwitterUser, bIncludeStatistics,
            bIncludeLatestStatus, bExpandLatestStatusUrls);
    }

    //*************************************************************************
    //  Method: WhatToIncludeFlagIsSet()
    //
    /// <summary>
    /// Checks whether a flag is set in an ORed combination of WhatToInclude
    /// flags.
    /// </summary>
    ///
    /// <param name="eORedEnumFlags">
    /// Zero or more ORed Enum flags.
    /// </param>
    ///
    /// <param name="eORedEnumFlagsToCheck">
    /// One or more Enum flags to check.
    /// </param>
    ///
    /// <returns>
    /// true if any of the <paramref name="eORedEnumFlagsToCheck" /> flags are
    /// set in <paramref name="eORedEnumFlags" />.
    /// </returns>
    //*************************************************************************

    protected Boolean
    WhatToIncludeFlagIsSet
    (
        WhatToInclude eORedEnumFlags,
        WhatToInclude eORedEnumFlagsToCheck
    )
    {
        return ( (eORedEnumFlags & eORedEnumFlagsToCheck) != 0 );
    }

    //*************************************************************************
    //  Method: GetNetworkDescription()
    //
    /// <summary>
    /// Gets a description of the network.
    /// </summary>
    ///
    /// <param name="bUseListName">
    /// If true, <paramref name="sListName" /> must be specified and <paramref
    /// name="oScreenNames" /> is ignored.  If false, <paramref
    /// name="oScreenNames" /> must be specified and <paramref
    /// name="sListName" /> is ignored.
    /// </param>
    ///
    /// <param name="sListName">
    /// Twitter List name if <paramref name="bUseListName" /> is true; unused
    /// otherwise.  Sample List name: "bob/twitterit".
    /// </param>
    ///
    /// <param name="oScreenNames">
    /// Zero or more Twitter screen names if <paramref name="bUseListName" />
    /// is false; unused otherwise.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <returns>
    /// A description of the network.
    /// </returns>
    //*************************************************************************

    protected String
    GetNetworkDescription
    (
        Boolean bUseListName,
        String sListName,
        ICollection<String> oScreenNames,
        WhatToInclude eWhatToInclude
    )
    {
        Debug.Assert( !bUseListName || !String.IsNullOrEmpty(sListName) );
        Debug.Assert(bUseListName || oScreenNames != null);
        AssertValid();

        NetworkDescriber oNetworkDescriber = new NetworkDescriber();

        if (bUseListName)
        {
            oNetworkDescriber.AddSentence(

                "The graph represents the network of Twitter users in the"
                + " Twitter List \"{0}\"."
                ,
                sListName
                );
        }
        else
        {
            oNetworkDescriber.AddSentence(

                "The graph represents a network of {0} specified Twitter"
                + " users."
                ,
                oScreenNames.Count
                );
        }

        oNetworkDescriber.AddNetworkTime(NetworkSource);

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,

            WhatToInclude.FollowedEdges
            |
            WhatToInclude.RepliesToEdges
            |
            WhatToInclude.MentionsEdges
            ) )
        {
            oNetworkDescriber.StartNewParagraph();
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowedEdges) )
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each follows relationship."
                );
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.RepliesToEdges) )
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each \"replies-to\" relationship in the"
                + " user's latest tweet."
                );
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.MentionsEdges) )
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each \"mentions\" relationship in the"
                + " user's latest tweet."
                );
        }

        return ( oNetworkDescriber.ConcatenateSentences() );
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

    protected override void
    BackgroundWorker_DoWork
    (
        object sender,
        DoWorkEventArgs e
    )
    {
        Debug.Assert(e.Argument is GetNetworkAsyncArgs);

        GetNetworkAsyncArgs oGetNetworkAsyncArgs =
            (GetNetworkAsyncArgs)e.Argument;

        try
        {
            e.Result = GetNetworkInternal(
                oGetNetworkAsyncArgs.UseListName,
                oGetNetworkAsyncArgs.ListName,
                oGetNetworkAsyncArgs.ScreenNames,
                oGetNetworkAsyncArgs.WhatToInclude);
        }
        catch (CancellationPendingException)
        {
            e.Cancel = true;
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArgs()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get the network.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgs : Object
    {
        ///
        public Boolean UseListName;
        ///
        public String ListName;
        ///
        public ICollection<String> ScreenNames;
        ///
        public WhatToInclude WhatToInclude;
    };
}

}
