
using System;
using System.Xml;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.XmlLib;
using Smrf.SocialNetworkLib;
using Smrf.NodeXL.GraphMLLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterUserNetworkAnalyzer
//
/// <summary>
/// Gets a network of Twitter users related to a specified user.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync" /> to asynchronously get the network, or
/// <see cref="GetNetwork" /> to get it synchronously.
/// </remarks>
//*****************************************************************************

public class TwitterUserNetworkAnalyzer : TwitterNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: TwitterUserNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterUserNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterUserNetworkAnalyzer()
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
        /// Include a vertex for each person followed by the user.
        /// </summary>

        FollowedVertices = 1,

        /// <summary>
        /// Include a vertex for each person following the user.
        /// </summary>

        FollowerVertices = 2,

        /// <summary>
        /// Include each person's latest status.
        /// </summary>

        LatestStatuses = 4,

        /// <summary>
        /// Expand the URLs contained within each person's latest status.  Used
        /// only if <see cref="LatestStatuses" /> is specified.
        /// </summary>

        ExpandedLatestStatusUrls = 8,

        /// <summary>
        /// Include an edge for each followed relationship if FollowedVertices
        /// is specified, and include an edge for each follower relationship if
        /// FollowerVertices is specified,
        /// </summary>

        FollowedFollowerEdges = 16,

        /// <summary>
        /// Include an edge from person A to person B if person A's latest
        /// tweet is a reply to person B.
        /// </summary>

        RepliesToEdges = 32,

        /// <summary>
        /// Include an edge from person A to person B if person A's latest
        /// tweet mentions person B.
        /// </summary>

        MentionsEdges = 64,
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets a directed network of Twitter users.
    /// </summary>
    ///
    /// <param name="screenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// Can be in any case, but it gets converted to lower case before getting
    /// the network.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="networkLevel">
    /// Network level to include.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
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
        String screenNameToAnalyze,
        WhatToInclude whatToInclude,
        NetworkLevel networkLevel,
        Int32 maximumPeoplePerRequest
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(screenNameToAnalyze) );

        Debug.Assert(networkLevel == NetworkLevel.One ||
            networkLevel == NetworkLevel.OnePointFive ||
            networkLevel == NetworkLevel.Two);

        Debug.Assert(maximumPeoplePerRequest > 0);
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.ScreenNameToAnalyze = screenNameToAnalyze;
        oGetNetworkAsyncArgs.WhatToInclude = whatToInclude;
        oGetNetworkAsyncArgs.NetworkLevel = networkLevel;
        oGetNetworkAsyncArgs.MaximumPeoplePerRequest = maximumPeoplePerRequest;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: GetNetwork()
    //
    /// <summary>
    /// Synchronously gets a directed network of Twitter users.
    /// </summary>
    ///
    /// <param name="screenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// Can be in any case, but it gets converted to lower case before getting
    /// the network.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="networkLevel">
    /// Network level to include.
    /// </param>
    ///
    /// <param name="maximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    public XmlDocument
    GetNetwork
    (
        String screenNameToAnalyze,
        WhatToInclude whatToInclude,
        NetworkLevel networkLevel,
        Int32 maximumPeoplePerRequest
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(screenNameToAnalyze) );

        Debug.Assert(networkLevel == NetworkLevel.One ||
            networkLevel == NetworkLevel.OnePointFive ||
            networkLevel == NetworkLevel.Two);

        Debug.Assert(maximumPeoplePerRequest > 0);
        AssertValid();

        return ( GetNetworkInternal(screenNameToAnalyze, whatToInclude,
            networkLevel, maximumPeoplePerRequest) );
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
    /// <param name="sScreenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetNetworkInternal
    (
        String sScreenNameToAnalyze,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPeoplePerRequest
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameToAnalyze) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPeoplePerRequest > 0);
        AssertValid();

        BeforeGetNetwork();

        Boolean bIncludeLatestStatus = WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.LatestStatuses);

        GraphMLXmlDocument oGraphMLXmlDocument =
            CreateGraphMLXmlDocument(bIncludeLatestStatus);

        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            GetNetworkInternal(sScreenNameToAnalyze, eWhatToInclude,
                eNetworkLevel, iMaximumPeoplePerRequest, oRequestStatistics,
                oGraphMLXmlDocument);
        }
        catch (Exception oException)
        {
            OnUnexpectedException(oException, oGraphMLXmlDocument,
                oRequestStatistics);
        }

        OnNetworkObtained(oGraphMLXmlDocument, oRequestStatistics,

            GetNetworkDescription(sScreenNameToAnalyze, eWhatToInclude,
                eNetworkLevel, iMaximumPeoplePerRequest),

            "Twitter User " + sScreenNameToAnalyze
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
        Boolean bIncludeLatestStatuses
    )
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(true);

        TwitterGraphMLUtil.DefineVertexStatisticsGraphMLAttributes(
            oGraphMLXmlDocument);

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
    /// <param name="sScreenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
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
        String sScreenNameToAnalyze,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPeoplePerRequest,
        RequestStatistics oRequestStatistics,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameToAnalyze) );

        Debug.Assert(eNetworkLevel == NetworkLevel.One ||
            eNetworkLevel == NetworkLevel.OnePointFive ||
            eNetworkLevel == NetworkLevel.Two);

        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        sScreenNameToAnalyze = sScreenNameToAnalyze.ToLower();

        // The unique screen names of the people in the network.

        HashSet<String> oUniqueScreenNames = new HashSet<String>();

        // The key is the Twitter user ID and the value is the corresponding
        // TwitterUser.  This is used by AppendOnePointFiveDegreeNetwork() to
        // determine which Twitter users are in the 1-degree network.

        Dictionary<String, TwitterUser> oUserIDDictionary =
            new Dictionary<String, TwitterUser>();

        // The screen names of people followed by (or following) sScreenName.

        HashSet<String> oOneDegreeOtherScreenNames = new HashSet<String>();

        // Include followed, followers, both, or neither.

        Boolean [] abFollowedFollowerFlags = new Boolean[] {

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.FollowedVertices),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.FollowerVertices),
            };

        for (Int32 i = 0; i < 2; i++)
        {
            if ( abFollowedFollowerFlags[i] )
            {
                // Always start with a 1-degree network.

                AppendOneDegreeNetwork(sScreenNameToAnalyze, eWhatToInclude,
                    (i == 0), iMaximumPeoplePerRequest, oGraphMLXmlDocument,
                    oUniqueScreenNames, oUserIDDictionary,
                    oOneDegreeOtherScreenNames, oRequestStatistics);
            }
        }

        for (Int32 i = 0; i < 2; i++)
        {
            if ( abFollowedFollowerFlags[i] )
            {
                if (eNetworkLevel == NetworkLevel.OnePointFive)
                {
                    // Append edges to make it a 1.5-degree network.

                    AppendOnePointFiveDegreeNetwork(sScreenNameToAnalyze,
                        eWhatToInclude, (i == 0), iMaximumPeoplePerRequest,
                        oGraphMLXmlDocument, oUserIDDictionary,
                        oOneDegreeOtherScreenNames, oRequestStatistics);
                }
                else if (eNetworkLevel == NetworkLevel.Two)
                {
                    // Append vertices and edges to make it a 2-degree network.

                    AppendTwoDegreeNetwork(sScreenNameToAnalyze,
                        eWhatToInclude, (i == 0), iMaximumPeoplePerRequest,
                        oGraphMLXmlDocument, oUniqueScreenNames,
                        oUserIDDictionary, oOneDegreeOtherScreenNames,
                        oRequestStatistics);
                }
            }
        }

        AppendRepliesToAndMentionsEdgeXmlNodes(oGraphMLXmlDocument,
            oUserIDDictionary.Values, oUniqueScreenNames,

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.RepliesToEdges),

            WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.MentionsEdges),

            false, false
            );
    }

    //*************************************************************************
    //  Method: AppendOneDegreeNetwork()
    //
    /// <summary>
    /// Appends the vertices and edges for a 1-degree network to a GraphML XML
    /// document .
    /// </summary>
    ///
    /// <param name="sScreenName">
    /// The screen name to use.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="bIncludeFollowedThisCall">
    /// true to include the people followed by the user, false to include the
    /// people following the user.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUniqueScreenNames">
    /// The unique screen names of the people in the network.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oOneDegreeOtherScreenNames">
    /// The screen names of people followed by (or following) <paramref
    /// name="sScreenName" />.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    void
    AppendOneDegreeNetwork
    (
        String sScreenName,
        WhatToInclude eWhatToInclude,
        Boolean bIncludeFollowedThisCall,
        Int32 iMaximumPeoplePerRequest,
        GraphMLXmlDocument oGraphMLXmlDocument,
        HashSet<String> oUniqueScreenNames,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        HashSet<String> oOneDegreeOtherScreenNames,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenName) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUniqueScreenNames != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oOneDegreeOtherScreenNames != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        Boolean bIncludeLatestStatuses = WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.LatestStatuses);

        Boolean bExpandLatestStatusUrls = WhatToIncludeFlagIsSet(
            eWhatToInclude, WhatToInclude.ExpandedLatestStatusUrls);

        if ( !oUniqueScreenNames.Contains(sScreenName) )
        {
            // Append a vertex node for sScreenName.
            //
            // This block executes once only, at the start of the get-network
            // process.  In that case, sScreenName is the screen name of the
            // user whose network is being analyzed.
            //
            // Because this is the first network request, the
            // bIgnoreWebAndXmlExceptions parameter to TryGetUserXmlNode() is
            // set to false, so that all exceptions stop the process.

            Dictionary<String, Object> oUserValueDictionary;

            TryGetUserValueDictionary(sScreenName, oRequestStatistics, false,
                out oUserValueDictionary);

            String sUserID;

            if ( !TryGetUserIDFromDictionary(oUserValueDictionary,
                out sUserID) )
            {
                throw new ArgumentException("Missing ID.");
            }

            TwitterUser oTwitterUser;

            TwitterGraphMLUtil.TryAppendVertexXmlNode(sScreenName, sUserID,
                oGraphMLXmlDocument, oUserIDDictionary, out oTwitterUser);

            oUniqueScreenNames.Add(sScreenName);

            AppendUserInformationFromValueDictionary(oUserValueDictionary,
                oGraphMLXmlDocument, oTwitterUser, true,
                bIncludeLatestStatuses, bExpandLatestStatusUrls);
        }

        ReportProgressForFollowedOrFollowing(sScreenName,
            bIncludeFollowedThisCall);

        // Get the IDs of the people followed by or following sScreenName.

        List<String> oOtherUserIDs = new List<String>();

        foreach ( String sOtherUserID in EnumerateFriendOrFollowerIDs(
            sScreenName, bIncludeFollowedThisCall, iMaximumPeoplePerRequest,
            oRequestStatistics) )
        {
            oOtherUserIDs.Add(sOtherUserID);
        }

        String [] asOtherUserIDs = oOtherUserIDs.ToArray();
        oOtherUserIDs = null;

        // Get information about each of those people.

        foreach (Dictionary<String, Object> oUserValueDictionary in
            EnumerateUserValueDictionaries(asOtherUserIDs, true,
                oRequestStatistics) )
        {
            String sOtherScreenName, sOtherUserID;

            if (
                !TryGetScreenNameFromDictionary(oUserValueDictionary,
                    out sOtherScreenName)
                ||
                !TryGetUserIDFromDictionary(oUserValueDictionary,
                    out sOtherUserID)
                )
            {
                // Nothing can be done with this other user without having a
                // screen name and ID, but this error is not fatal.  Just move
                // on to the next other user.

                continue;
            }

            TwitterUser oTwitterUser;

            TwitterGraphMLUtil.TryAppendVertexXmlNode(sOtherScreenName,
                sOtherUserID, oGraphMLXmlDocument, oUserIDDictionary,
                out oTwitterUser);

            oUniqueScreenNames.Add(sOtherScreenName);

            AppendUserInformationFromValueDictionary(oUserValueDictionary,
                oGraphMLXmlDocument, oTwitterUser, true,
                bIncludeLatestStatuses, bExpandLatestStatusUrls);

            if ( WhatToIncludeFlagIsSet(eWhatToInclude,
                WhatToInclude.FollowedFollowerEdges) )
            {
                AppendFollowedOrFollowingEdgeXmlNode(sScreenName,
                    sOtherScreenName, bIncludeFollowedThisCall,
                    oGraphMLXmlDocument, oRequestStatistics);
            }

            oOneDegreeOtherScreenNames.Add(sOtherScreenName);
        }
    }

    //*************************************************************************
    //  Method: AppendOnePointFiveDegreeNetwork()
    //
    /// <summary>
    /// Appends edges to a GraphML XML document containing a 1-degree network
    /// to make it a 1.5-degree network.
    /// </summary>
    ///
    /// <param name="sScreenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="bIncludeFollowedThisCall">
    /// true to include the people followed by the user, false to include the
    /// people following the user.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oOneDegreeOtherScreenNames">
    /// The screen names of people followed by (or following) <paramref
    /// name="sScreenNameToAnalyze" />.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <remarks>
    /// <see cref="AppendOneDegreeNetwork" /> must be called before this method
    /// is called.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendOnePointFiveDegreeNetwork
    (
        String sScreenNameToAnalyze,
        WhatToInclude eWhatToInclude,
        Boolean bIncludeFollowedThisCall,
        Int32 iMaximumPeoplePerRequest,
        GraphMLXmlDocument oGraphMLXmlDocument,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        HashSet<String> oOneDegreeOtherScreenNames,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameToAnalyze) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oOneDegreeOtherScreenNames != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowedFollowerEdges) )
        {
            AppendFollowedOrFollowingEdgeXmlNodes(oOneDegreeOtherScreenNames,
                oUserIDDictionary, bIncludeFollowedThisCall,
                iMaximumPeoplePerRequest, oGraphMLXmlDocument,
                oRequestStatistics);
        }
    }

    //*************************************************************************
    //  Method: AppendTwoDegreeNetwork()
    //
    /// <summary>
    /// Appends vertices and edges to a GraphML XML document containing a
    /// 1-degree network to make it a 2-degree network.
    /// </summary>
    ///
    /// <param name="sScreenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="bIncludeFollowedThisCall">
    /// true to include the people followed by the user, false to include the
    /// people following the user.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oUniqueScreenNames">
    /// The unique screen names of the people in the network.
    /// </param>
    ///
    /// <param name="oUserIDDictionary">
    /// The key is the Twitter user ID and the value is the corresponding
    /// TwitterUser.
    /// </param>
    ///
    /// <param name="oOneDegreeOtherScreenNames">
    /// The screen names of people followed by (or following) <paramref
    /// name="sScreenNameToAnalyze" />.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    ///
    /// <remarks>
    /// <see cref="AppendOneDegreeNetwork" /> must be called before this method
    /// is called.
    /// </remarks>
    //*************************************************************************

    protected void
    AppendTwoDegreeNetwork
    (
        String sScreenNameToAnalyze,
        WhatToInclude eWhatToInclude,
        Boolean bIncludeFollowedThisCall,
        Int32 iMaximumPeoplePerRequest,
        GraphMLXmlDocument oGraphMLXmlDocument,
        HashSet<String> oUniqueScreenNames,
        Dictionary<String, TwitterUser> oUserIDDictionary,
        HashSet<String> oOneDegreeOtherScreenNames,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameToAnalyze) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oUniqueScreenNames != null);
        Debug.Assert(oUserIDDictionary != null);
        Debug.Assert(oOneDegreeOtherScreenNames != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        foreach (String sScreenName in oOneDegreeOtherScreenNames)
        {
            Debug.Assert(sScreenName != sScreenNameToAnalyze);

            // We need to find out who are the followed (or followers) of
            // sScreenName, and add them to the GraphML XML document if they
            // are not already there.
            //
            // Use a new HashSet and discard it when done.  We don't want
            // oOneDegreeOtherScreenNames to be modified by this method.

            AppendOneDegreeNetwork(sScreenName, eWhatToInclude,
                bIncludeFollowedThisCall, iMaximumPeoplePerRequest,
                oGraphMLXmlDocument, oUniqueScreenNames, oUserIDDictionary,
                new HashSet<String>(), oRequestStatistics);
        }
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
    /// <param name="sScreenNameToAnalyze">
    /// The screen name of the Twitter user whose network should be analyzed.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="eNetworkLevel">
    /// Network level to include.  Must be NetworkLevel.One, OnePointFive, or
    /// Two.
    /// </param>
    ///
    /// <param name="iMaximumPeoplePerRequest">
    /// Maximum number of people to request for each query, or Int32.MaxValue
    /// for no limit.
    /// </param>
    ///
    /// <returns>
    /// A description of the network.
    /// </returns>
    //*************************************************************************

    protected String
    GetNetworkDescription
    (
        String sScreenNameToAnalyze,
        WhatToInclude eWhatToInclude,
        NetworkLevel eNetworkLevel,
        Int32 iMaximumPeoplePerRequest
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sScreenNameToAnalyze) );
        Debug.Assert(iMaximumPeoplePerRequest > 0);
        AssertValid();

        NetworkDescriber oNetworkDescriber = new NetworkDescriber();

        oNetworkDescriber.AddSentence(

            "The graph represents the {0} Twitter network of the user with the"
            + " username \"{1}\"."
            ,
            NetworkLevelToString(eNetworkLevel),
            sScreenNameToAnalyze
            );

        oNetworkDescriber.AddNetworkTime(NetworkSource);

        oNetworkDescriber.StartNewParagraph();

        Boolean bFollowedVertices = WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowedVertices);

        Boolean bFollowerVertices = WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowerVertices);

        if (bFollowedVertices)
        {
            oNetworkDescriber.AddSentence(
                "There is a vertex for each person followed by the user."
                );
        }

        if (bFollowerVertices)
        {
            oNetworkDescriber.AddSentence(
                "There is a vertex for each person following the user."
                );
        }

        oNetworkDescriber.AddNetworkLimit(iMaximumPeoplePerRequest, "people");

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,

            WhatToInclude.FollowedFollowerEdges
            |
            WhatToInclude.RepliesToEdges
            |
            WhatToInclude.MentionsEdges
            ) )
        {
            oNetworkDescriber.StartNewParagraph();
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.FollowedFollowerEdges) )
        {
            if (bFollowedVertices)
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge for each followed relationship."
                    );
            }

            if (bFollowerVertices)
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge for each following relationship."
                    );
            }
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
                oGetNetworkAsyncArgs.ScreenNameToAnalyze,
                oGetNetworkAsyncArgs.WhatToInclude,
                oGetNetworkAsyncArgs.NetworkLevel,
                oGetNetworkAsyncArgs.MaximumPeoplePerRequest);
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

    protected class GetNetworkAsyncArgs : GetNetworkAsyncArgsBase
    {
        ///
        public String ScreenNameToAnalyze;
        ///
        public WhatToInclude WhatToInclude;
        ///
        public NetworkLevel NetworkLevel;
    };
}

}
