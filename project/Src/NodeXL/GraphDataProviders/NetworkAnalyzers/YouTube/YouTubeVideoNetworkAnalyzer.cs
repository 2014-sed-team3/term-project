﻿
using System;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.XmlLib;
using Smrf.SocialNetworkLib;
using Smrf.NodeXL.GraphMLLib;

namespace Smrf.NodeXL.GraphDataProviders.YouTube
{
//*****************************************************************************
//  Class: YouTubeVideoNetworkAnalyzer
//
/// <summary>
/// Gets a network of related YouTube videos.
/// </summary>
///
/// <remarks>
/// Use <see cref="GetNetworkAsync" /> to asynchronously get an undirected
/// network of related YouTube videos.
/// </remarks>
//*****************************************************************************

public class YouTubeVideoNetworkAnalyzer : YouTubeNetworkAnalyzerBase
{
    //*************************************************************************
    //  Constructor: YouTubeVideoNetworkAnalyzer()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="YouTubeVideoNetworkAnalyzer" /> class.
    /// </summary>
    //*************************************************************************

    public YouTubeVideoNetworkAnalyzer()
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
        /// Include an edge for each pair of videos commented on by the same
        /// person.
        /// </summary>

        SharedCommenterEdges = 1,

        /// <summary>
        /// Include an edge for each pair of videos that share the same
        /// category.
        /// </summary>

        SharedCategoryEdges = 2,

        /// <summary>
        /// Include an edge for each pair of videos for which the same person
        /// responded with a video.
        /// </summary>

        SharedVideoResponderEdges = 4,
    }

    //*************************************************************************
    //  Method: GetNetworkAsync()
    //
    /// <summary>
    /// Asynchronously gets an undirected network of related YouTube videos.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="whatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="maximumVideos">
    /// Maximum number of videos to request, or Int32.MaxValue for no limit.
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
        String searchTerm,
        WhatToInclude whatToInclude,
        Int32 maximumVideos
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(searchTerm) );
        Debug.Assert(maximumVideos > 0);
        AssertValid();

        const String MethodName = "GetNetworkAsync";
        CheckIsBusy(MethodName);

        // Wrap the arguments in an object that can be passed to
        // BackgroundWorker.RunWorkerAsync().

        GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

        oGetNetworkAsyncArgs.SearchTerm = searchTerm;
        oGetNetworkAsyncArgs.WhatToInclude = whatToInclude;
        oGetNetworkAsyncArgs.MaximumVideos = maximumVideos;

        m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
    }

    //*************************************************************************
    //  Method: GetVideoNetworkInternal()
    //
    /// <overloads>
    /// Gets a network of related YouTube videos.
    /// </overloads>
    ///
    /// <summary>
    /// Gets a network of related YouTube videos.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="iMaximumVideos">
    /// Maximum number of videos to request, or Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <returns>
    /// An XmlDocument containing the network as GraphML.
    /// </returns>
    //*************************************************************************

    protected XmlDocument
    GetVideoNetworkInternal
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumVideos
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumVideos > 0);
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument =
            CreateGraphMLXmlDocument(eWhatToInclude);

        RequestStatistics oRequestStatistics = new RequestStatistics();

        try
        {
            GetVideoNetworkInternal(sSearchTerm, eWhatToInclude,
                iMaximumVideos, oRequestStatistics, oGraphMLXmlDocument);
        }
        catch (Exception oException)
        {
            OnUnexpectedException(oException, oGraphMLXmlDocument,
                oRequestStatistics);
        }

        OnNetworkObtained(oGraphMLXmlDocument, oRequestStatistics, 
            GetNetworkDescription(sSearchTerm, eWhatToInclude, iMaximumVideos),
            "YouTube Video " + sSearchTerm
            );

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: GetVideoNetworkInternal()
    //
    /// <summary>
    /// Gets a network of related YouTube videos, given a GraphMLXmlDocument.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="iMaximumVideos">
    /// Maximum number of videos to request, or Int32.MaxValue for no limit.
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
    GetVideoNetworkInternal
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumVideos,
        RequestStatistics oRequestStatistics,
        GraphMLXmlDocument oGraphMLXmlDocument
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumVideos > 0);
        Debug.Assert(oRequestStatistics != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        AssertValid();

        // First, add a vertex for each video matching the search term.

        HashSet<String> oVideoIDs;
        Dictionary< String, LinkedList<String> > oCategoryDictionary;

        AppendVertexXmlNodes(sSearchTerm, eWhatToInclude, iMaximumVideos,
            oGraphMLXmlDocument, oRequestStatistics, out oVideoIDs,
            out oCategoryDictionary);

        // Now add whatever edges were requested.

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedCategoryEdges) )
        {
            Debug.Assert(oCategoryDictionary != null);

            ReportProgress("Adding edges for shared categories.");

            AppendEdgesFromDictionary(oCategoryDictionary, oGraphMLXmlDocument,
                "Shared category", SharedCategoryID);
        }

        oCategoryDictionary = null;

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedCommenterEdges) )
        {
            AppendSharedResponderEdges(oGraphMLXmlDocument, oVideoIDs,
                MaximumCommentsPerVideo,
                "http://gdata.youtube.com/feeds/api/videos/{0}/comments",
                "commenter", SharedCommenterID, oRequestStatistics);
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedVideoResponderEdges) )
        {
            AppendSharedResponderEdges(oGraphMLXmlDocument, oVideoIDs,
                iMaximumVideos,
                "http://gdata.youtube.com/feeds/api/videos/{0}/responses",
                "video responder", SharedVideoResponderID, oRequestStatistics);
        }
    }

    //*************************************************************************
    //  Method: CreateGraphMLXmlDocument()
    //
    /// <summary>
    /// Creates a GraphMLXmlDocument representing a network of YouTube videos.
    /// </summary>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <returns>
    /// A GraphMLXmlDocument representing a network of YouTube videos.  The
    /// document includes GraphML-attribute definitions but no vertices or
    /// edges.
    /// </returns>
    //*************************************************************************

    protected GraphMLXmlDocument
    CreateGraphMLXmlDocument
    (
        WhatToInclude eWhatToInclude
    )
    {
        AssertValid();

        GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(false);

        NodeXLGraphMLUtil.DefineEdgeRelationshipGraphMLAttribute(
            oGraphMLXmlDocument);

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedCategoryEdges) )
        {
            oGraphMLXmlDocument.DefineEdgeStringGraphMLAttributes(
                SharedCategoryID, "Shared Category");
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedCommenterEdges) )
        {
            oGraphMLXmlDocument.DefineEdgeStringGraphMLAttributes(
                SharedCommenterID, "Shared Commenter");
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedVideoResponderEdges) )
        {
            oGraphMLXmlDocument.DefineEdgeStringGraphMLAttributes(
                SharedVideoResponderID, "Shared Video Responder");
        }

        oGraphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            TitleID, "Title",
            AuthorID, "Author",
            CreatedDateUtcID, "Created Date (UTC)"
            );

        oGraphMLXmlDocument.DefineGraphMLAttribute(false, RatingID,
            "Rating", "double", null);

        oGraphMLXmlDocument.DefineGraphMLAttributes(false, "int",
            ViewsID, "Views",
            FavoritedID, "Favorited",
            CommentsID, "Comments"
            );

        NodeXLGraphMLUtil.DefineVertexImageFileGraphMLAttribute(
            oGraphMLXmlDocument);

        NodeXLGraphMLUtil.DefineVertexCustomMenuGraphMLAttributes(
            oGraphMLXmlDocument);

        return (oGraphMLXmlDocument);
    }

    //*************************************************************************
    //  Method: AppendVertexXmlNodes()
    //
    /// <summary>
    /// Appends a vertex XML node for each video that matches a specified
    /// search term.
    /// </summary>
    ///
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="iMaximumVideos">
    /// Maximum number of videos to request, or Int32.MaxValue for no limit.
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
    /// <param name="oVideoIDs">
    /// Where the set of unique video IDs gets stored.
    /// </param>
    ///
    /// <param name="oCategoryDictionary">
    /// If an edge should be included for each pair of videos that share the
    /// same category, this gets set to a Dictionary for which the key is a
    /// lower-case category and the value is a LinkedList of the video IDs that
    /// have the category.  Otherwise, it gets set to null.
    /// </param>
    //*************************************************************************

    protected void
    AppendVertexXmlNodes
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumVideos,
        GraphMLXmlDocument oGraphMLXmlDocument,
        RequestStatistics oRequestStatistics,
        out HashSet<String> oVideoIDs,
        out Dictionary< String, LinkedList<String> > oCategoryDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumVideos> 0);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        ReportProgress("Getting a list of videos.");

        // This is used to skip duplicate videos in the results returned by
        // YouTube.  (I'm not sure why YouTube sometimes returns duplicates,
        // but it does.)

        oVideoIDs = new HashSet<String>();

        // If an edge should be included for each pair of videos that share the
        // same category, the key is a lower-case category and the value is a
        // LinkedList of the video IDs that have the category.

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedCategoryEdges) )
        {
            oCategoryDictionary =
                new Dictionary< String, LinkedList<String> >();
        }
        else
        {
            oCategoryDictionary = null;
        }

        String sUrl = String.Format(

            "http://gdata.youtube.com/feeds/api/videos?q={0}"
            ,
            EncodeUrlParameter(sSearchTerm)
            );

        // The document consists of an "entry" XML node for each video.

        foreach ( XmlNode oEntryXmlNode in EnumerateXmlNodes(sUrl,
            "a:feed/a:entry", iMaximumVideos, false, oRequestStatistics) )
        {
            XmlNamespaceManager oXmlNamespaceManager =
                CreateXmlNamespaceManager(oEntryXmlNode.OwnerDocument);

            // Use the video ID as the GraphML vertex name.  The video title
            // can't be used because it is not unique.

            String sVideoID;

            if (
                !XmlUtil2.TrySelectSingleNodeAsString(oEntryXmlNode,
                    "media:group/yt:videoid/text()", oXmlNamespaceManager,
                    out sVideoID)
                ||
                oVideoIDs.Contains(sVideoID)
                )
            {
                continue;
            }

            oVideoIDs.Add(sVideoID);

            XmlNode oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(
                sVideoID);

            AppendStringGraphMLAttributeValue(oEntryXmlNode,
                "a:title/text()", oXmlNamespaceManager, oGraphMLXmlDocument,
                oVertexXmlNode, TitleID);

            AppendStringGraphMLAttributeValue(oEntryXmlNode,
                "a:author/a:name/text()", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode, AuthorID);

            AppendDoubleGraphMLAttributeValue(oEntryXmlNode,
                "gd:rating/@average", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode, RatingID);

            AppendInt32GraphMLAttributeValue(oEntryXmlNode,
                "yt:statistics/@viewCount", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode, ViewsID);

            AppendInt32GraphMLAttributeValue(oEntryXmlNode,
                "yt:statistics/@favoriteCount", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode, FavoritedID);

            AppendInt32GraphMLAttributeValue(oEntryXmlNode,
                "gd:comments/gd:feedLink/@countHint", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode, CommentsID);

            AppendYouTubeDateGraphMLAttributeValue(oEntryXmlNode,
                "a:published/text()", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode, CreatedDateUtcID);

            AppendStringGraphMLAttributeValue(oEntryXmlNode,
                "media:group/media:thumbnail/@url", oXmlNamespaceManager,
                oGraphMLXmlDocument, oVertexXmlNode,
                NodeXLGraphMLUtil.VertexImageFileID);

            if ( AppendStringGraphMLAttributeValue(oEntryXmlNode,
                    "media:group/media:player/@url", oXmlNamespaceManager,
                    oGraphMLXmlDocument, oVertexXmlNode,
                    NodeXLGraphMLUtil.VertexMenuActionID) )
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode,
                    NodeXLGraphMLUtil.VertexMenuTextID,
                    "Play Video in Browser");
            }

            if (oCategoryDictionary != null)
            {
                CollectCategories(oEntryXmlNode, sVideoID,
                    oXmlNamespaceManager, oCategoryDictionary);
            }
        }
    }

    //*************************************************************************
    //  Method: AppendSharedResponderEdges()
    //
    /// <summary>
    /// Appends an edge XML node for each pair of videos for which the same
    /// person responded.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// The GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oVideoIDs">
    /// The set of unique video IDs.
    /// </param>
    ///
    /// <param name="iMaximumResponses">
    /// Maximum number of responses to request, or Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <param name="sUrlPattern">
    /// URL to get the responses for one video, with "{0}" where the video ID
    /// goes.
    /// </param>
    ///
    /// <param name="sResponderTitle">
    /// Title describing the person who has responded.  Samples: "commenter",
    /// "video responder".
    /// </param>
    ///
    /// <param name="sKeyAttributeID">
    /// The GraphML-attribute ID to use for the dictionary key's value.
    /// </param>
    ///
    /// <param name="oRequestStatistics">
    /// A <see cref="RequestStatistics" /> object that is keeping track of
    /// requests made while getting the network.
    /// </param>
    //*************************************************************************

    protected void
    AppendSharedResponderEdges
    (
        GraphMLXmlDocument oGraphMLXmlDocument,
        HashSet<String> oVideoIDs,
        Int32 iMaximumResponses,
        String sUrlPattern,
        String sResponderTitle,
        String sKeyAttributeID,
        RequestStatistics oRequestStatistics
    )
    {
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert(oVideoIDs != null);
        Debug.Assert(iMaximumResponses > 0);
        Debug.Assert( !String.IsNullOrEmpty(sUrlPattern) );
        Debug.Assert(sUrlPattern.IndexOf("{0}") >= 0);
        Debug.Assert( !String.IsNullOrEmpty(sResponderTitle) );
        Debug.Assert( !String.IsNullOrEmpty(sKeyAttributeID) );
        Debug.Assert(oRequestStatistics != null);
        AssertValid();

        // The key is the name of an author and the value is a LinkedList of
        // the video IDs to which the author has responded.

        Dictionary< String, LinkedList<String> > oAuthorUserNameDictionary =
            new Dictionary< String, LinkedList<String> >();

        foreach (String sVideoID in oVideoIDs)
        {
            ReportProgress(String.Format(
            
                "Getting {0}s for the video with the ID \"{1}\"."
                ,
                sResponderTitle,
                sVideoID
                ) );

            // This is to prevent self-loop edges that would result when the
            // same author responds to the same video more than once.

            HashSet<String> oAuthorUserNames = new HashSet<String>();

            String sUrl = String.Format(sUrlPattern, sVideoID);

            // The document consists of an "entry" XML node for each response.

            foreach ( XmlNode oEntryXmlNode in EnumerateXmlNodes(sUrl,
                "a:feed/a:entry", iMaximumResponses, true,
                oRequestStatistics) )
            {
                XmlNamespaceManager oXmlNamespaceManager =
                    CreateXmlNamespaceManager(oEntryXmlNode.OwnerDocument);

                String sAuthorUserName;

                if (
                    XmlUtil2.TrySelectSingleNodeAsString(oEntryXmlNode,
                        "a:author/a:name/text()", oXmlNamespaceManager,
                        out sAuthorUserName)
                    &&
                    !oAuthorUserNames.Contains(sAuthorUserName)
                    )
                {
                    AddVideoIDToDictionary(sAuthorUserName, sVideoID,
                        oAuthorUserNameDictionary);

                    oAuthorUserNames.Add(sAuthorUserName);
                }
            }
        }

        ReportProgress("Adding edges for shared " + sResponderTitle + "s.");

        AppendEdgesFromDictionary(oAuthorUserNameDictionary,
            oGraphMLXmlDocument, "Shared " + sResponderTitle, sKeyAttributeID);
    }

    //*************************************************************************
    //  Method: CollectCategories()
    //
    /// <summary>
    /// Collects categories for a YouTube video.
    /// </summary>
    ///
    /// <param name="oEntryXmlNode">
    /// Entry XML node from YouTube that describes a video.
    /// </param>
    ///
    /// <param name="sVideoID">
    /// YouTube ID for the video.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// NamespaceManager to use, or null to not use one.
    /// </param>
    ///
    /// <param name="oCategoryDictionary">
    /// The key is a lower-case category and the value is a LinkedList of the
    /// video IDs that have the category.
    /// </param>
    //*************************************************************************

    protected void
    CollectCategories
    (
        XmlNode oEntryXmlNode,
        String sVideoID,
        XmlNamespaceManager oXmlNamespaceManager,
        Dictionary< String, LinkedList<String> > oCategoryDictionary
    )
    {
        Debug.Assert(oEntryXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sVideoID) );
        Debug.Assert(oXmlNamespaceManager != null);
        Debug.Assert(oCategoryDictionary != null);
        AssertValid();

        foreach ( XmlAttribute oXmlAttribute in oEntryXmlNode.SelectNodes(

            "a:category[@scheme='http://gdata.youtube.com/schemas/2007/"
            + "categories.cat']/@term",

            oXmlNamespaceManager) )
        {
            if ( !String.IsNullOrEmpty(oXmlAttribute.Value) )
            {
                AddVideoIDToDictionary(oXmlAttribute.Value.ToLower(), sVideoID,
                    oCategoryDictionary);
            }
        }
    }

    //*************************************************************************
    //  Method: AddVideoIDToDictionary()
    //
    /// <summary>
    /// Adds a video ID to a dictionary.
    /// </summary>
    ///
    /// <param name="oDictionary">
    /// The key is a string and the value is a LinkedList of the video IDs
    /// corresponding to that string.
    /// </param>
    ///
    /// <param name="sKey">
    /// The dictionary key the video belongs to.
    /// </param>
    ///
    /// <param name="sVideoID">
    /// YouTube ID for the video.
    /// </param>
    //*************************************************************************

    protected void
    AddVideoIDToDictionary
    (
        String sKey,
        String sVideoID,
        Dictionary< String, LinkedList<String> > oDictionary
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        Debug.Assert( !String.IsNullOrEmpty(sVideoID) );
        Debug.Assert(oDictionary != null);
        AssertValid();

        LinkedList<String> oVideoIDsForKey;

        if ( oDictionary.TryGetValue(sKey, out oVideoIDsForKey) )
        {
            oVideoIDsForKey.AddLast(sVideoID);
        }
        else
        {
            oVideoIDsForKey = new LinkedList<String>();
            oVideoIDsForKey.AddLast(sVideoID);

            oDictionary.Add(sKey, oVideoIDsForKey);
        }
    }

    //*************************************************************************
    //  Method: AppendEdgesFromDictionary()
    //
    /// <summary>
    /// Appends an edge for each pair of videos that share the same dictionary
    /// key.
    /// </summary>
    ///
    /// <param name="oGraphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="oDictionary">
    /// The key is a string and the value is a LinkedList of the video IDs
    /// corresponding to that key.
    /// </param>
    ///
    /// <param name="sRelationship">
    /// The value of the RelationshipID GraphML-attribute to use.
    /// </param>
    ///
    /// <param name="sKeyAttributeID">
    /// The GraphML-attribute ID to use for the dictionary key's value.
    /// </param>
    //*************************************************************************

    protected void
    AppendEdgesFromDictionary
    (
        Dictionary< String, LinkedList<String> > oDictionary,
        GraphMLXmlDocument oGraphMLXmlDocument,
        String sRelationship,
        String sKeyAttributeID
    )
    {
        Debug.Assert(oDictionary != null);
        Debug.Assert(oGraphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(sRelationship) );
        Debug.Assert( !String.IsNullOrEmpty(sKeyAttributeID) );
        AssertValid();

        // For each key...

        foreach (KeyValuePair< String, LinkedList<String> > oKeyValuePair in
            oDictionary)
        {
            String sKey = oKeyValuePair.Key;

            // For each video ID that has the key...

            LinkedList<String> oVideoIDsWithThisKey = oKeyValuePair.Value;

            for (
                LinkedListNode<String> oVideoIDWithThisKey =
                    oVideoIDsWithThisKey.First;

                oVideoIDWithThisKey != null;

                oVideoIDWithThisKey = oVideoIDWithThisKey.Next
                )
            {
                // For each of the subsequent video IDs in the LinkedList that
                // have the key...

                for (
                    LinkedListNode<String> oSubsequentVideoIDWithThisKey =
                        oVideoIDWithThisKey.Next;

                    oSubsequentVideoIDWithThisKey != null;

                    oSubsequentVideoIDWithThisKey =
                        oSubsequentVideoIDWithThisKey.Next
                    )
                {
                    XmlNode oEdgeXmlNode = NodeXLGraphMLUtil.AppendEdgeXmlNode(
                        oGraphMLXmlDocument, oVideoIDWithThisKey.Value,
                        oSubsequentVideoIDWithThisKey.Value, sRelationship);

                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                        oEdgeXmlNode, sKeyAttributeID, sKey);
                }
            }
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
    /// <param name="sSearchTerm">
    /// The term to search for.
    /// </param>
    ///
    /// <param name="eWhatToInclude">
    /// Specifies what should be included in the network.
    /// </param>
    ///
    /// <param name="iMaximumVideos">
    /// Maximum number of videos to request, or Int32.MaxValue for no limit.
    /// </param>
    ///
    /// <returns>
    /// A description of the network.
    /// </returns>
    //*************************************************************************

    protected String
    GetNetworkDescription
    (
        String sSearchTerm,
        WhatToInclude eWhatToInclude,
        Int32 iMaximumVideos
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSearchTerm) );
        Debug.Assert(iMaximumVideos > 0);
        AssertValid();

        NetworkDescriber oNetworkDescriber = new NetworkDescriber();

        oNetworkDescriber.AddSentence(

            "The graph represents the network of YouTube videos whose title,"
            + " keywords, description, categories, or author\'s username"
            + " contain \"{0}\"."
            ,
            sSearchTerm
            );

        oNetworkDescriber.AddNetworkTime(NetworkSource);

        oNetworkDescriber.StartNewParagraph();
        oNetworkDescriber.AddNetworkLimit(iMaximumVideos, "videos");

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,

            WhatToInclude.SharedCategoryEdges
            |
            WhatToInclude.SharedCommenterEdges
            |
            WhatToInclude.SharedVideoResponderEdges
            ) )
        {
            oNetworkDescriber.StartNewParagraph();
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedCategoryEdges) )
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each pair of videos that have the same"
                + " category."
                );
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedCommenterEdges) )
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each pair of videos commented on by the"
                + " same user."
                );
        }

        if ( WhatToIncludeFlagIsSet(eWhatToInclude,
            WhatToInclude.SharedVideoResponderEdges) )
        {
            oNetworkDescriber.AddSentence(
                "There is an edge for each pair of videos responded to with"
                + " another video by the same user."
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
        Debug.Assert(sender is BackgroundWorker);

        BackgroundWorker oBackgroundWorker = (BackgroundWorker)sender;

        Debug.Assert(e.Argument is GetNetworkAsyncArgs);

        GetNetworkAsyncArgs oGetNetworkAsyncArgs =
            (GetNetworkAsyncArgs)e.Argument;

        try
        {
            e.Result = GetVideoNetworkInternal(oGetNetworkAsyncArgs.SearchTerm,
                oGetNetworkAsyncArgs.WhatToInclude,
                oGetNetworkAsyncArgs.MaximumVideos);
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
    //  Protected constants
    //*************************************************************************

    /// GraphML-attribute IDs for edges.

    protected const String SharedCategoryID = "SharedCategory";
    ///
    protected const String SharedCommenterID = "SharedCommenter";
    ///
    protected const String SharedVideoResponderID = "SharedVideoResponder";

    /// GraphML-attribute IDs for vertices.

    protected const String TitleID = "Title";
    ///
    protected const String AuthorID = "Author";
    ///
    protected const String RatingID = "Rating";
    ///
    protected const String ViewsID = "Views";
    ///
    protected const String FavoritedID = "Favorited";
    ///
    protected const String CommentsID = "Comments";
    ///
    protected const String CreatedDateUtcID = "CreatedDateUtc";


    /// Maximum number of comments to get for each video when
    /// WhatToInclude.SharedCommenterEdges is specified.

    protected const Int32 MaximumCommentsPerVideo = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GetNetworkAsyncArgs()
    //
    /// <summary>
    /// Contains the arguments needed to asynchronously get a network of
    /// related YouTube videos.
    /// </summary>
    //*************************************************************************

    protected class GetNetworkAsyncArgs : Object
    {
        ///
        public String SearchTerm;
        ///
        public WhatToInclude WhatToInclude;
        ///
        public Int32 MaximumVideos;
    };
}

}
