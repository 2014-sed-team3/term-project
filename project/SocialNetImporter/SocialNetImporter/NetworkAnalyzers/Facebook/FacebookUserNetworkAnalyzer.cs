
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.SocialNetworkLib;
using Smrf.AppLib;
using Smrf.XmlLib;
//using Microsoft.Research.CommunityTechnologies.DateTimeLib;
using System.IO;
using System.Text;
using Facebook;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;


namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    //*****************************************************************************
    //  Class: sdcsd
    //
    /// <summary>
    /// Gets a network of Facebook friends.
    /// </summary>
    ///
    /// <remarks>
    /// Use <see cref="GetNetworkAsync" /> to asynchronously get a directed network
    /// of Facebook freinds.
    /// </remarks>
    //*****************************************************************************

    public class FacebookUserNetworkAnalyzer : FacebookNetworkAnalyzerBase
    {
        //*************************************************************************
        //  Constructor: sdcsd()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="sdcsd" /> class.
        /// </summary>
        //*************************************************************************

        public FacebookUserNetworkAnalyzer()
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
            /// Include a vertex for each of the user's contacts.
            /// </summary>

            ContactVertices = 1,

            /// <summary>
            /// Include a vertex for each user who has commented on the user's
            /// photos.
            /// </summary>

            CommenterVertices = 2,

            /// <summary>
            /// Include information about each user in the network.
            /// </summary>

            UserInformation = 4,
        }

        private int NrOfSteps = 4;
        private int CurrentStep = 0;

        //*************************************************************************
        //  Method: GetNetworkAsync()
        //
        /// <summary>
        /// Asynchronously gets a directed network of Facebook friends.
        /// </summary>
        ///
        /// <param name="s_accessToken">
        /// The access_token needed for the authentication in Facebook API.
        /// </param>
        ///
        /// <param name="includeMe">
        /// Specifies whether the ego should be included in the network.
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
            String s_accessToken,            
            List<NetworkType> oEdgeType,
            bool bDownloadFromPostToPost,
            bool bDownloadBetweenDates,
            bool bEgoTimeline,
            bool bFriendsTimeline,
            int iFromPost,
            int iToPost,
            DateTime oStartDate,
            DateTime oEndDate,
            bool bLimitCommentsLikes,
            int iNrLimit,
            bool bGetTooltips,
            bool bIncludeMe,
            AttributesDictionary<bool> attributes
        )
        {            
            Debug.Assert(!String.IsNullOrEmpty(s_accessToken));
            AssertValid();
            
            const String MethodName = "GetNetworkAsync";
            CheckIsBusy(MethodName);

            GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

            oGetNetworkAsyncArgs.AccessToken = s_accessToken;            
            oGetNetworkAsyncArgs.attributes = attributes;            
            oGetNetworkAsyncArgs.EdgeType = oEdgeType;
            oGetNetworkAsyncArgs.DownloadFromPostToPost = bDownloadFromPostToPost;
            oGetNetworkAsyncArgs.DownloadBetweenDates = bDownloadBetweenDates;
            oGetNetworkAsyncArgs.EgoTimeline = bEgoTimeline;
            oGetNetworkAsyncArgs.FriendsTimeline = bFriendsTimeline;
            oGetNetworkAsyncArgs.FromPost = iFromPost;
            oGetNetworkAsyncArgs.ToPost = iToPost;
            oGetNetworkAsyncArgs.StartDate = oStartDate;
            oGetNetworkAsyncArgs.EndDate = oEndDate;
            oGetNetworkAsyncArgs.LimitCommentsLikes = bLimitCommentsLikes;
            oGetNetworkAsyncArgs.Limit = iNrLimit;
            oGetNetworkAsyncArgs.GetTooltips = bGetTooltips;
            oGetNetworkAsyncArgs.IncludeMe = bIncludeMe;
            
            m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
        }

        //*************************************************************************
        //  Method: GetNetwork()
        //
        /// <summary>
        /// Synchronously gets a directed network of Facebook friends.
        /// </summary>
        ///
        /// <param name="s_accessToken">
        /// The access_token needed for the authentication in Facebook API.
        /// </param>
        ///
        /// <param name="includeMe">
        /// Specifies whether the ego should be included in the network.
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

        public XmlDocument
        GetNetwork
        (
            String s_accessToken,            
            List<NetworkType> oEdgeType,
            bool bDownloadFromPostToPost,
            bool bDownloadBetweenDates,
            bool bEgoTimeline,
            bool bFriendsTimeline,
            int iFromPost,
            int iToPost,
            DateTime oStartDate,
            DateTime oEndDate,
            bool bLimitCommentsLikes,
            int iNrLimit,
            bool bGetTooltips,
            bool bIncludeMe,
            AttributesDictionary<bool> attributes
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(s_accessToken));
            AssertValid();


            return (GetFriendsNetworkInternal(s_accessToken, oEdgeType, bDownloadFromPostToPost,
                                                bDownloadBetweenDates, bEgoTimeline, bFriendsTimeline,
                                                iFromPost, iToPost, oStartDate,
                                                oEndDate,bLimitCommentsLikes, iNrLimit,
                                                bGetTooltips, bIncludeMe, attributes));
        }

        //*************************************************************************
        //  Method: GetFriendsNetworkInternal()
        //
        /// <summary>
        /// Gets the friends network from Facebook.
        /// </summary>
        ///
        /// <param name="sAccessToken">
        /// The access_token needed to execute queries in Facebook.
        /// </param>
        ///
        /// <returns>
        /// An XmlDocument containing the network as GraphML.
        /// </returns>
        //*************************************************************************

        protected XmlDocument
        GetFriendsNetworkInternal
        (
            string sAccessToken,            
            List<NetworkType> oEdgeType,
            bool bDownloadFromPostToPost,
            bool bDownloadBetweenDates,
            bool bEgoTimeline,
            bool bFriendsTimeline,
            int iFromPost,
            int iToPost,
            DateTime oStartDate,
            DateTime oEndDate,
            bool bLimitCommentsLikes,
            int iNrLimit,
            bool bGetTooltips,
            bool bIncludeMe,
            AttributesDictionary<bool> attributes
        )

        {
            Debug.Assert(!String.IsNullOrEmpty(sAccessToken));            
            AssertValid();           

            //Set the total nr of steps
            if (bGetTooltips) NrOfSteps++;            

            oTimer.Elapsed += new System.Timers.ElapsedEventHandler(oTimer_Elapsed);

            oGraphMLXmlDocument = CreateGraphMLXmlDocument(attributes);
            RequestStatistics oRequestStatistics = new RequestStatistics();


            var fb = new FacebookAPI(sAccessToken);

            m_oFb = fb;
            
            Dictionary<string, string> friends = new Dictionary<string, string>();
            List<string> friendsUIDs = new List<string>();            
            XmlNode oVertexXmlNode;
            string attributeValue = "";
            Dictionary<String, List<Dictionary<string, object>>> statusUpdates = new Dictionary<string, List<Dictionary<string, object>>>();
            Dictionary<String, List<Dictionary<string, object>>> wallPosts = new Dictionary<string,List<Dictionary<string,object>>>();
            
            string currentStatusUpdate="";
            string currentWallPost = "";
            string currentWallTags = "";
            string currentStatusTags = "";
            List<Dictionary<string,object>>.Enumerator en;            
            bool bGetUsersTagged = oEdgeType.Contains(NetworkType.TimelineUserTagged);
            bool bGetCommenters = oEdgeType.Contains(NetworkType.TimelineUserComments);
            bool bGetLikers = oEdgeType.Contains(NetworkType.TimelineUserLikes);
            bool bGetPostAuthors = oEdgeType.Contains(NetworkType.TimelinePostAuthors);
            bool bGetPosts = oEdgeType.Count > 0;

            DownloadFriends();

            if (bEgoTimeline || bIncludeMe)
            {
                GetEgo();
            }

            oVerticesToQuery = VerticesToQuery(bEgoTimeline, bFriendsTimeline);

            if (bGetPosts)
            {
                DownloadPosts(bDownloadFromPostToPost, bDownloadBetweenDates, iFromPost,
                                iToPost, oStartDate, oEndDate, bGetUsersTagged ,
                                bGetCommenters, bGetLikers);                
            }

            DownloadVertices(bGetUsersTagged, bGetCommenters, bGetLikers, bGetPosts, bLimitCommentsLikes, iNrLimit);

            DownloadAttributes(attributes);

            if(bGetTooltips)
            {
                GetTooltips();
            }           

            CreateEdges(bGetUsersTagged, bGetCommenters, bGetLikers, bGetPostAuthors, bIncludeMe);

            AddVertices();

            ReportProgress(String.Format("Completed downloading {0} friends data", friends.Count));            

            AddEdges();          
            

           

            ReportProgress("Importing downloaded network into NodeXL");

            //After successfull download of the network
            //get the network description
            OnNetworkObtainedWithoutTerminatingException(oGraphMLXmlDocument, oRequestStatistics,
                GetNetworkDescription(oEdgeType, bDownloadFromPostToPost, bDownloadBetweenDates,
                                        iFromPost, iToPost, oStartDate,
                                        oEndDate, bLimitCommentsLikes, iNrLimit,
                                        oGraphMLXmlDocument));

            return oGraphMLXmlDocument;
            
        }

        private void DownloadPosts
        (
            bool bDownloadFromPostToPost,
            bool bDownloadBetweenDates,
            int iFromPost,
            int iToPost,
            DateTime oStartDate,
            DateTime oEndDate,
            bool bGetUsersTagged,
            bool bGetCommenters,            
            bool bGetLikers
        )
        {
            int iNrOfQueriesPerMultiquery = 10;
            int iNrOfMultiqueries = (int)Math.Ceiling((double)oVerticesToQuery.Count / iNrOfQueriesPerMultiquery);
            string sUsersTagged = bGetUsersTagged ? ", description_tags, message_tags, tagged_ids, with_tags" : "";
            string sGetCommenters = bGetCommenters ? ", comment_info" : "";
            string sGetLikers = bGetLikers ? ", like_info" : "";
            string sFirstPart = "SELECT actor_id, message, source_id, post_id, created_time" + sUsersTagged + sGetCommenters + sGetLikers +
                                    " FROM stream WHERE source_id ={0} ";
            string sLimitFirstPosts = " LIMIT 0,{0}"; //String.Format(" LIMIT 0,{0}", iNrOfFirstPosts);
            string sLimitByDate = String.Format("AND created_time >= {0} AND created_time <={1}", DateUtil.ConvertToTimestamp(oStartDate),
                                                DateUtil.ConvertToTimestamp(oEndDate));

            List<JSONObject> oResults = new List<JSONObject>();
            if (bDownloadFromPostToPost)
            {
                //sFirstPart += sLimitFirstPosts;
                oResults = DownloadFromPostToPost(iFromPost, iToPost, sFirstPart);
            }
            else if (bDownloadBetweenDates)
            {
                sFirstPart += sLimitByDate;
            
                string sQuery;
                int iIndex = 0;
                Dictionary<string, string> oQueries = new Dictionary<string, string>();
                //List<JSONObject> oResults = new List<JSONObject>();

                for (int i = 1; i <= iNrOfMultiqueries; i++)
                {
                    oQueries.Clear();
                    for (int j = 0; j < iNrOfQueriesPerMultiquery; j++)
                    {                        
                        if (iIndex >= oVerticesToQuery.Count)
                            break;
                        sQuery = String.Format(sFirstPart, oVerticesToQuery[iIndex].ID);
                        oQueries.Add("query" + j.ToString(), sQuery);
                        iIndex++;
                    }

                    //ReportProgress(String.Format("Downloading Posts (Batch {0}/{1})", i, iNrOfMultiqueries));

                    string sProgress = String.Format("Downloading Posts (Batch {0}/{1})", i, iNrOfMultiqueries);
                    JSONObject oResult = ExecuteFQLMultiqueryWithRetry(oQueries, sProgress); //m_oFb.ExecuteFQLMultiquery(oQueries);
                    if (oResult.IsArray)
                    {
                        foreach (JSONObject oMultiquery in oResult.Array)
                        {
                            oResults.AddRange(oMultiquery.Dictionary["fql_result_set"].Array);
                        }
                    }
                }
            }

            oPosts = (from r in oResults
                      group r by r.Dictionary["source_id"].String into RGroup
                      select new
                      {
                          Key = RGroup.Key,
                          Value = RGroup.ToList()
                      }).ToDictionary(x => x.Key, x => x.Value);
        }

        private void DownloadPostsBetweenDates
        (
            DateTime oStartDate,
            DateTime oEndDate,
            string sFirsPartQuery,
            bool bEgoTimeline,
            bool bFriendsTimeline
        )
        {
        }

        private List<JSONObject> DownloadFromPostToPost
        (
            int iFromPost,
            int iToPost,
            string sFirstPartQuery
        )
        {
            int iNrOfPosts = iToPost - (iFromPost - 1);
            int iNrOfQueriesPerMultiquery = 10;
            int iNrOfFirstPostsPerQuery = 20;                      
            int iNrOfQueriesPerVertex = (int) Math.Ceiling((double)iNrOfPosts / iNrOfFirstPostsPerQuery);
            int iNrOfMultiqueries = (int)Math.Ceiling((double)oVerticesToQuery.Count * iNrOfQueriesPerVertex / iNrOfQueriesPerMultiquery);
            int iNrOfIterationsPerQuery = (int)Math.Ceiling((double)iNrOfQueriesPerMultiquery / iNrOfQueriesPerVertex);
            Dictionary<string, string> oQueries = new Dictionary<string, string>();
            List<JSONObject> oResults = new List<JSONObject>();
            string sQuery, sLimit;
            int iIndex=0;
            int iStartLimit, iLimitCount;
            bool bCanExceedLimit = iNrOfMultiqueries * iNrOfQueriesPerMultiquery > 600;

            for (int i = 1; i <= iNrOfMultiqueries; i++)
            {
                oQueries.Clear();
                for (int j = 0; j < iNrOfIterationsPerQuery; j++)
                {
                    if (iIndex >= oVerticesToQuery.Count)
                        break;
                    for (int k = 0; k < iNrOfQueriesPerVertex; k++)
                    {
                        iStartLimit = k * iNrOfFirstPostsPerQuery+(iFromPost-1);
                        iLimitCount = iToPost < (k + 1) * iNrOfFirstPostsPerQuery ? iToPost - iStartLimit : iNrOfFirstPostsPerQuery;
                        sLimit = String.Format(" LIMIT {0},{1}", iStartLimit, iLimitCount);
                        sQuery = String.Format(sFirstPartQuery + sLimit, oVerticesToQuery[iIndex].ID);
                        oQueries.Add("query" + ((j * iNrOfQueriesPerVertex) + k).ToString(), sQuery);
                    }
                    iIndex++;
                }                

                string sProgress = String.Format("Downloading Posts (Batch {0}/{1})", i, iNrOfMultiqueries);
                JSONObject oResult = ExecuteFQLMultiqueryWithRetry(oQueries, sProgress); //m_oFb.ExecuteFQLMultiquery(oQueries);
                if (bCanExceedLimit)
                {
                    //Thread.Sleep(iNrOfQueriesPerMultiquery*1000);
                }
                if (oResult!=null && oResult.IsArray)
                {
                    foreach (JSONObject oMultiquery in oResult.Array)
                    {
                        oResults.AddRange(oMultiquery.Dictionary["fql_result_set"].Array);
                    }
                }
            }

            return oResults;
        }

        private void DownloadVertices
        (
            bool bGetUsersTagged,
            bool bGetCommenters,
            bool bGetLikers,
            bool bGetPosts,
            bool bLimitCommentsLikes,
            int iNrLimit
        )
        {

            if (bGetPosts || bGetCommenters || bGetLikers)
            {
                GetPostAuthors();
            }

            if (bGetUsersTagged)
            {
                GetUsersTagged();
            }

            if (bGetCommenters)
            {
                GetCommenters(bLimitCommentsLikes, iNrLimit);
            }

            if (bGetLikers)
            {
                GetLikers(bLimitCommentsLikes, iNrLimit);
            }           

        }

        private void DownloadFriends
        (
        )
        {
            CurrentStep++;
            ReportProgress(String.Format("Step {0}/{1}: Downloading Friend Data", CurrentStep, NrOfSteps));
            
            JSONObject vertices = m_oFb.ExecuteFQL("SELECT uid,name FROM user WHERE uid IN " +
                                               "(SELECT uid2 FROM friend WHERE uid1=me()) ORDER BY uid");

            foreach(JSONObject vertex in vertices.Array)
            {
                oVertices.Add(new Vertex(vertex.Dictionary["uid"].String, 
                                        ManageDuplicateNames(vertex.Dictionary["name"].String), "Friend"));
            }
        }

        private JSONObject DownloadEgo
        (
        )
        {
            JSONObject vertices = m_oFb.ExecuteFQL("SELECT uid,name FROM user WHERE uid = me()");

            if (vertices.IsArray & vertices.Array.Length > 0)
            {
                return vertices.Array[0];
            }

            return null;
            
        }

        private void GetEgo
        (
        )
        {
            JSONObject oEgoVertex = DownloadEgo();

            if (oEgoVertex != null)
            {
                oVertices.Add(new Vertex(oEgoVertex.Dictionary["uid"].String,
                                        ManageDuplicateNames(oEgoVertex.Dictionary["name"].String), "Ego"));
            }
            
        }

        private void GetPostAuthors
        (            
        )
        {
            int iIndex = 1;
            int iCount = oPosts.Count;
            foreach (KeyValuePair<string, List<JSONObject>> kvp in oPosts)
            {
                ReportProgress(String.Format("Downloading Post Authors (Batch {0}/{1})", iIndex, iCount));
                foreach (JSONObject oPost in kvp.Value)
                {
                    oVertices.Add(new Vertex(oPost.Dictionary["actor_id"].String, "", "Author"));   
                }
                iIndex++;
            }      
        }

        private void GetUsersTagged
        (            
        )
        {
            int iIndex = 1;
            int iCount = oPosts.Count;
            foreach (KeyValuePair<string, List<JSONObject>> kvp in oPosts)
            {
                ReportProgress(String.Format("Downloading Users Tagged (Batch {0}/{1})", iIndex, iCount));
                foreach (JSONObject oPost in kvp.Value)
                {
                    Vertex oAuthor = oVertices[oPost.Dictionary["actor_id"].String];
                    foreach(JSONObject oTaggedId in oPost.Dictionary["tagged_ids"].Array)
                    {
                        Vertex oUserTagged = new Vertex(oTaggedId.String, "", "User Tagged");
                        oVertices.Add(oUserTagged);                        
                    }                    
                }
                iIndex++;
            }            
        }

        private void GetCommenters
        (        
            bool bLimitCommentsLikes,
            int iNrLimit
        )
        {
            int iIndex = 1;
            int iCount = oPosts.Count;

            List<JSONObject> oTmpCommenters = new List<JSONObject>();
            foreach (KeyValuePair<string, List<JSONObject>> kvp in oPosts)
            {
                ReportProgress(String.Format("Downloading Commenters (Batch {0}/{1})", iIndex, iCount));
                foreach (JSONObject oPost in kvp.Value)
                {
                    int iNrOfComments = (int)oPost.Dictionary["comment_info"].Dictionary["comment_count"].Integer;
                    if(bLimitCommentsLikes)
                    {
                        oTmpCommenters = GetInfo("comment", oPost.Dictionary["post_id"].String, iNrLimit, iNrOfComments, 0);
                    }
                    else
                    {
                        oTmpCommenters = GetInfo("comment", oPost.Dictionary["post_id"].String, -1, iNrOfComments, 0);
                    }
                    oCommenters.AddRange(oTmpCommenters);
                    foreach (JSONObject oComment in oTmpCommenters)
                    {
                        Vertex oCommenter = new Vertex(oComment.Dictionary["fromid"].String, "", "Commenter");
                        oVertices.Add(oCommenter);
                    }
                }
                iIndex++;
            }
        }

        private void GetLikers
        (        
            bool bLimitCommentsLikes,
            int iNrLimit
        )
        {
            int iIndex = 1;
            int iCount = oPosts.Count;

            List<JSONObject> oTmpLikers = new List<JSONObject>();
            foreach (KeyValuePair<string, List<JSONObject>> kvp in oPosts)
            {
                ReportProgress(String.Format("Downloading Likers (Batch {0}/{1})", iIndex, iCount));
                foreach (JSONObject oPost in kvp.Value)
                {
                    int iNrOfComments = (int)oPost.Dictionary["like_info"].Dictionary["like_count"].Integer;
                    if(bLimitCommentsLikes)
                    {
                        oTmpLikers = GetInfo("like", oPost.Dictionary["post_id"].String, iNrLimit, iNrOfComments, 0);
                    }
                    else
                    {
                        oTmpLikers = GetInfo("like", oPost.Dictionary["post_id"].String, -1, iNrOfComments, 0);
                    }
                    oLikers.AddRange(oTmpLikers);
                    foreach (JSONObject oLike in oTmpLikers)
                    {
                        Vertex oLiker = new Vertex(oLike.Dictionary["user_id"].String, "", "Liker");
                        oVertices.Add(oLiker);
                    }
                }
                iIndex++;
            }            
        }

        private List<JSONObject> GetInfo
        (
            string sTableName,
            string sPostId,
            int iLimit,
            int iNrOfLikesComments,
            int iCallNr
        )
        {
            int totalNrOfLikes = iLimit<1?(int)iNrOfLikesComments:iLimit;
            //We can get a maximum of 10000 results for multiqueries, so we have to make multiple multiqueries
            //We have to get information from two different tables and we are limited to 5000 results each
            //for a total of 10000 results for the whole multiquery
            int nrOfMultiqueries = (int)Math.Ceiling(totalNrOfLikes / 5000.0);
            int nrOfQueries = 10;
            JSONObject result;
            string sFromId = (sTableName == "comment") ? "fromid, text, time" : "user_id";

            List<JSONObject> oInfo = new List<JSONObject>();

            for (int j = 0; j < nrOfMultiqueries; j++)
            {
                Dictionary<string, string> queries = new Dictionary<string, string>();
                if ((j == nrOfMultiqueries - 1) && (nrOfMultiqueries % 10000 > 0))
                {
                    nrOfQueries = (int)Math.Ceiling((nrOfMultiqueries % 10000) / 1000.0);
                }
                //Divide the 10000 results in 10 different queries with 1000 results each
                //For each comment we get the commenter's information in another query.
                for (int k = 0; k < nrOfQueries; k++)
                {
                    int nrRes;
                    if (iLimit < 1)
                    {
                        nrRes = 1000;
                    }
                    else if (iLimit < 1000)
                    {
                        nrRes = iLimit;
                    }
                    else
                    {
                        nrRes = (k + 1) * 1000 > iLimit ? iLimit % (k * 1000) : 1000;
                    }
                    queries.Add("query" + (k + 1).ToString(), "SELECT "+sFromId+", post_id FROM " + sTableName +
                                                                " WHERE post_id='" + sPostId +
                                                                "' LIMIT " + (k * 1000).ToString() + ", " + nrRes);
                }

                result = m_oFb.ExecuteFQLMultiquery(queries);

                if (result != null && result.IsArray)
                {
                    for (int k = 0; k < result.Array.Length; k++)
                    {
                        oInfo.AddRange(result.Array[k].Dictionary["fql_result_set"].Array);
                    }

                }

                //ExecuteFQLMultiqueryWithRetryRelogin(queries, String.Format("Step {0}/{1}: Downloading likes - Post {2}/{3}(Batch {4}/{5})",
                //                                CurrentStep, NrOfSteps, iCallNr, iNrOfLikesComments, j + 1, nrOfMultiqueries), false);
            }

            return oInfo;
                           
        }       

        private void CreateEdges
        (
            bool bGetUsersTagged,
            bool bGetCommenters,
            bool bGetLikers,            
            bool bGetAuthorPosts,
            bool bIncludeMe
        )
        {
            DownloadFriendConnections();

            if (bGetUsersTagged)
            {
                CreateUserTaggedEdges();
            }

            if (bGetAuthorPosts)
            {
                CreateAuthorEdges();
            }

            if (bGetCommenters)
            {
                CreateCommenterEdges();
            }

            if (bGetLikers)
            {
                CreateLikerEdges();
            }

            if (bIncludeMe)
            {
                CreateIncludeMeEdges();
            }
        }

        private void DownloadFriendConnections
        (
        )
        {
            /*
             * Facebook FQL has a limit of 5000 results.
             * If one has 100 friends and all of his friends are connected to each-other,
             * so they form a clique, the nr of results returned by the FQL query will be: 4950.
             * This is calculated using the formula n(n-1)/2, where n is the number of nodes.
             * The average nr of friends for a Facebook user is 130 (according to 
             * http://www.facebook.com/press/info.php?statistics), so the probability that the nr
             * of results of a FQL call is more than 5000 is high. In these cases Facebook will
             * return only 5000 results. The LIMIT keyword won't work to get the other results
             * since it is applied after the query is executed.
             * To surpass such a limitation, we divide the friends list into smaller chunks.
             * In order to get all the possible combinations between one's friends and at the
             * same time get less than 5000 combinations, we use two for loops. The first loop, 
             * called the outter loop, can check for 100 users in one iteration and the second
             * loop, called the inner loop, can check for 500 users in one iteration. In this way
             * we will get at most 5000 results in one FQL call.
             * 
             * Thanks to Bernie Hogan for helping on this.
             */

            //Get only "friend" vertices
            List<Vertex> oFriends = oVertices.Where(x => x.Type == "Friend").ToList();
            int maxNrOfUsersInOutterLoop = 100;
            int maxNrOfUsersInInnerLoop = 49;
            int outterLoopIter = (int)Math.Ceiling(oFriends.Count / (double)maxNrOfUsersInOutterLoop);
            int innerLoopIter = (int)Math.Ceiling(oFriends.Count / (double)maxNrOfUsersInInnerLoop);
            int startIndexOutterLoop = 0;
            int endIndexOutterLoop = 0;
            int startIndexInnerLoop = 0;
            int endIndexInnerLoop = 0;
            int nrOfQueries = 1;
            List<JSONObject> edges = new List<JSONObject>();
            List<Dictionary<string, JSONObject>> newEdgesWithNoDuplicate = new List<Dictionary<string, JSONObject>>();
            HashSet<Edge> uniqueEdges = new HashSet<Edge>();
            Dictionary<string, string> queries = new Dictionary<string, string>();

            CurrentStep++;
            //Outter loop
            for (int i = 0; i < outterLoopIter; i++)
            {
                //Calculate the start and end index
                startIndexOutterLoop = i * maxNrOfUsersInOutterLoop;
                endIndexOutterLoop = (i + 1) * maxNrOfUsersInOutterLoop;
                endIndexOutterLoop = endIndexOutterLoop >= oFriends.Count ? oFriends.Count - 1 : endIndexOutterLoop;

                //Inner loop
                for (int j = 0; j < innerLoopIter; j++)
                {
                    //Calculate the start and end index
                    startIndexInnerLoop = j * maxNrOfUsersInInnerLoop;
                    endIndexInnerLoop = (j + 1) * maxNrOfUsersInInnerLoop;
                    endIndexInnerLoop = endIndexInnerLoop >= oFriends.Count ? oFriends.Count - 1 : endIndexInnerLoop;

                    //Add the queries to a dictionary
                    queries.Add("query" + nrOfQueries.ToString(),
                                                "SELECT uid1, uid2 FROM friend WHERE uid1 IN" +
                                                "(SELECT uid2 FROM friend WHERE uid1=me()" +
                                                    " AND uid2>=" + oFriends[startIndexOutterLoop].ID +
                                                    " AND uid2<=" + oFriends[endIndexOutterLoop].ID + ")" +
                                                " AND uid2 IN" +
                                                " (SELECT uid1 FROM friend WHERE uid2=me()" +
                                                    " AND uid1>=" + oFriends[startIndexInnerLoop].ID +
                                                    " AND uid1<=" + oFriends[endIndexInnerLoop].ID + ")");


                    nrOfQueries++;

                }

            }

            List<JSONObject> tmp = FQLBatchRequest(m_oFb, queries.Values.ToList());

            foreach (JSONObject obj in tmp)
            {
                for (int k = 0; k < obj.Array.Length; k++)
                {
                    JSONObject oEdegResults = JSONObject.CreateFromString(obj.Array[k].Dictionary["body"].String);
                    foreach (JSONObject oEdgeResult in oEdegResults.Array)
                    {
                        try
                        {
                            oEdges.Add(new Edge(oVertices[oEdgeResult.Dictionary["uid1"].String], oVertices[oEdgeResult.Dictionary["uid2"].String],
                                            "", "Friend", "", 1));
                        }
                        catch (Exception e)
                        {
                            //Do nothing
                        }
                    }
                }
            }            
        }        

        private VertexCollection VerticesToQuery
        (
            bool bEgoTimeline,
            bool bFriendsTimeline
        )
        {
            if (bEgoTimeline && bFriendsTimeline)
            {
                return oVertices;
            }
            else if (bEgoTimeline)
            {
                return new VertexCollection(oVertices.Where(x => x.Type == "Ego"));
            }
            else if (bFriendsTimeline)
            {
                return new VertexCollection(oVertices.Where(x => x.Type == "Friend"));
            }
            else
            {
                return new VertexCollection();
            }
        }

        private void CreateUserTaggedEdges
        (            
        )
        {
            foreach (KeyValuePair<string, List<JSONObject>> kvp in oPosts)
            {
                foreach (JSONObject oPost in kvp.Value)
                {
                    Vertex oAuthor = oVertices[oPost.Dictionary["actor_id"].String];                    
                    foreach (JSONObject oTaggedId in oPost.Dictionary["tagged_ids"].Array)
                    {
                        Vertex oUserTagged = oVertices[oTaggedId.String];
                        if (oUserTagged != null)
                        {
                            //Create also the edge
                            Edge oEdge = new Edge(oUserTagged, oAuthor, "", "User Tagged", "", 1,int.Parse(oPost.Dictionary["created_time"].String), oVertices[kvp.Key].Name, EdgeDirection.Directed);
                            if (!oEdges.Add(oEdge))
                            {
                                oEdges[oUserTagged, oAuthor].Weight++;
                            }
                        }
                    }
                }
            }       
            
        }

        private void CreateAuthorEdges
        (
        )
        {
            foreach (KeyValuePair<string, List<JSONObject>> kvp in oPosts)
            {
                foreach (JSONObject oPost in kvp.Value)
                {
                    
                    Vertex oAuthor = oVertices[oPost.Dictionary["actor_id"].String];
                    Vertex oOwner = oVertices[oPost.Dictionary["source_id"].String];
                    Edge oEdge = new Edge(oAuthor, oOwner, "", "Post Author", oPost.Dictionary["message"].String, 1, int.Parse(oPost.Dictionary["created_time"].String), oOwner.Name, EdgeDirection.Directed);
                    if (oAuthor !=null && oOwner != null &&
                        !oEdges.Add(oEdge))
                    {
                        oEdges[oAuthor, oOwner].Weight++;
                    }
                    
                }
            }       
        }

        private void CreateCommenterEdges
        (
        )
        {
            foreach (JSONObject oCommenter in oCommenters)
            {
                JSONObject oPost = oPosts.SelectMany(x => x.Value).FirstOrDefault(x => x.Dictionary["post_id"].String == oCommenter.Dictionary["post_id"].String);
                Vertex oAuthor = oVertices[oPost.Dictionary["actor_id"].String];
                Vertex oCommenterV = oVertices[oCommenter.Dictionary["fromid"].String];
                if (oAuthor !=null && oCommenterV !=null &&
                    !oEdges.Add(new Edge(oCommenterV, oAuthor, "", "Commenter", oCommenter.Dictionary["text"].String , 1, int.Parse(oCommenter.Dictionary["time"].String),
                        oVertices[oPost.Dictionary["source_id"].String].Name, EdgeDirection.Directed)))
                {
                    oEdges[oCommenterV, oAuthor].Weight++;
                }
                
                
            }
        }

        private void CreateLikerEdges
        (
        )
        {
            foreach (JSONObject oLiker in oLikers)
            {
                JSONObject oPost = oPosts.SelectMany(x => x.Value).FirstOrDefault(x => x.Dictionary["post_id"].String == oLiker.Dictionary["post_id"].String);
                Vertex oAuthor = oVertices[oPost.Dictionary["actor_id"].String];
                //Vertex oAuthor = oVertices[GetPostAuthorForLikerCommenter(oLiker)];
                Vertex oCommenterV = oVertices[oLiker.Dictionary["user_id"].String];                
                if (oCommenterV!=null && oAuthor !=null &&
                    !oEdges.Add(new Edge(oCommenterV, oAuthor, "", "Liker", "", 1, 0, oVertices[oPost.Dictionary["source_id"].String].Name, EdgeDirection.Directed)))
                {
                    oEdges[oCommenterV, oAuthor].Weight++;
                }
            }
        }

        private void CreateIncludeMeEdges
        (
        )
        {
            List<Vertex> oFriends = oVertices.Where(x => x.Type == "Friend").ToList();
            Vertex oEgo = oVertices.FirstOrDefault(x => x.Type == "Ego");

            if (oEgo != null)
            {
                foreach (Vertex oFriend in oFriends)
                {
                    oEdges.Add(new Edge(oEgo, oFriend, "", "Friend", "", 1));
                }
            }
        }

        //private string GetPostAuthorForLikerCommenter(JSONObject oLikerCommenter)
        //{
        //    JSONObject oFirstResult = null;
        //    foreach (KeyValuePair<string, List<JSONObject>> kvp in oPosts)
        //    {
        //        oFirstResult = kvp.Value.FirstOrDefault(x => x.Dictionary["post_id"].String == oLikerCommenter.Dictionary["post_id"].String);
        //        if (oFirstResult!=null)
        //        {
        //            return oFirstResult.Dictionary["actor_id"].String;
        //        }
        //    }

        //    return null;
        //}

        private List<JSONObject> FQLBatchRequest(FacebookAPI fb, List<string> oQueries)
        {
            Dictionary<string, string> oBatchArguments = new Dictionary<string, string>();
            string oBatch;
            int nrOfQueries = oQueries.Count;
            int nrOfCalls = (int)Math.Ceiling((double)nrOfQueries / 49);
            int startIndex, endIndex;
            List<JSONObject> results = new List<JSONObject>();

            for (int i = 0; i < nrOfCalls; i++)
            {
                ReportProgress(String.Format(
                        "Step {0}/{1}: Downloading Friends of Friends data (batch {2}/{3})",
                        CurrentStep, NrOfSteps, i+1, nrOfCalls));
                startIndex = i * 49;
                endIndex = nrOfQueries - startIndex <= 49 ? nrOfQueries - startIndex : 49;
                oBatch = String.Empty;
                oBatch += "[";
                foreach (string oQuery in oQueries.Skip(startIndex).Take(endIndex))
                {

                    oBatch += "{\"method\":\"POST\",\"relative_url\":\"method/fql.query?query=" + oQuery + "\",},";

                }
                oBatch += "]";
                oBatchArguments.Clear();
                oBatchArguments.Add("batch", oBatch);
                results.Add(fb.Post("", oBatchArguments));
            }

            return results;
        }
        

        //*************************************************************************
        //  Method: AppendVertexTooltipXmlNodes()
        //
        /// <summary>
        /// Appends a vertex tooltip XML node for each person in the network.
        /// </summary>
        ///
        /// <param name="oGraphMLXmlDocument">
        /// The GraphMLXmlDocument being populated.
        /// </param>
        ///
        /// <param name="oVertexXmlNode">
        /// The XmlNode representing the vertex.
        /// </param>
        /// 
        /// <param name="sVertex">
        /// The screening name of the vertex. 
        /// </param>
        /// 
        /// <param name="sDisplayString">
        /// The string to be attached after the screening name.
        /// </param>
        //*************************************************************************

        private void
        GetTooltips
        (            
        )
        {            
            AssertValid();

            int totalNrOfqueries = 20;
            int nrOfMultiqueries = (int)(Math.Ceiling((double)oVertices.Count / (totalNrOfqueries)));
            int iIndex = 0;
            
            JSONObject returnedStatusUpdates = null;
            Dictionary<string, string> queries = new Dictionary<string, string>();
            Dictionary<string, string> statusUpdates = new Dictionary<string, string>();
            

            CurrentStep++;

            string firstPartQuery = "SELECT actor_id, message FROM stream WHERE actor_id = {0} AND source_id = {0} AND message<>'' LIMIT 0,1";
                        
            for (int k = 0; k < nrOfMultiqueries; k++)
            {
                queries.Clear();                
                //Build the queries and add them to a dictionary
                for (int i = 0; i < totalNrOfqueries; i++)
                {                    
                    if (iIndex >= oVertices.Count)
                    {
                        break;
                    }
                    queries.Add("query" + (i + 1).ToString(), String.Format(firstPartQuery, oVertices[iIndex].ID));
                    iIndex++;                  
                }



                returnedStatusUpdates = ExecuteFQLMultiqueryWithRetry(queries, String.Format("Step {0}/{1}: Getting Tooltips (Batch {2}/{3})",
                                                CurrentStep, NrOfSteps, (k + 1), nrOfMultiqueries));

                if (returnedStatusUpdates.IsArray)
                {
                    //Add ToolTip data to each vertex
                    foreach (JSONObject oQuery in returnedStatusUpdates.Array)
                    {
                        foreach (JSONObject oQueryResult in oQuery.Dictionary["fql_result_set"].Array)
                        {
                            oVertices[oQueryResult.Dictionary["actor_id"].String].ToolTip = oQueryResult.Dictionary["message"].String;
                        }
                    }

                }
            }
        }

        //*************************************************************************
        //  Method: getAttributes()
        //
        /// <summary>
        /// Gets the selected attributes for all the friends
        /// </summary>
        ///
        /// <param name="attributes">
        /// The dictionary that holds the attributes and states
        /// for each of them if they are included
        /// </param>
        /// 
        /// <param name="friendUIDs">
        /// A list with friend's UIDs
        /// </param> 
        ///
        /// <param name="fb">
        /// A FacebookAPI instance to make calls to Facebook
        /// </param>
        ///    
        /// <returns>
        /// A dictionary with the attributes' values
        /// </returns>
        //*************************************************************************       

        private void DownloadAttributes
        (
            AttributesDictionary<bool> attributes       
        )
        {            
            int nrOfFriendsPerQuery = 20;
            int totalNrOfqueries = 10;
            int nrOfMultiqueries = (int)(Math.Ceiling((double)oVertices.Count / (nrOfFriendsPerQuery*totalNrOfqueries)));
            int startIndex, endIndex, computedEndIndex;
            Dictionary<string, string> queries = new Dictionary<string, string>();
            JSONObject returnedAttributes;
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues = new Dictionary<string, Dictionary<string, JSONObject>>();
            CurrentStep++;                       

            //Build the first part of the queries
            string firstPartQuery = "SELECT uid,";
            foreach (KeyValuePair<AttributeUtils.Attribute, bool> kvp in attributes)
            {
                if (kvp.Value)
                {
                    firstPartQuery += kvp.Key.value + ",";
                }
            }
            firstPartQuery = firstPartQuery.Remove(firstPartQuery.Length - 1);
            firstPartQuery += " FROM user WHERE uid IN ";
            string sIDs; ;

            for (int k = 0; k < nrOfMultiqueries; k++)
            {
                queries.Clear();
                //Build the queries and add them to a dictionary
                for (int i = 0; i < totalNrOfqueries; i++)
                {
                    startIndex = i * nrOfFriendsPerQuery + k*totalNrOfqueries*nrOfFriendsPerQuery;
                    computedEndIndex = (i + 1) * nrOfFriendsPerQuery + k * totalNrOfqueries * nrOfFriendsPerQuery;
                    endIndex = computedEndIndex >= oVertices.Count ? oVertices.Count - 1 : computedEndIndex;
                    //Build IDs string
                    sIDs = "";
                    for (int j = startIndex; j <= endIndex; j++)
                    {
                        sIDs += oVertices[j].ID+",";
                    }
                    sIDs = sIDs.Remove(sIDs.Length - 1);
                    queries.Add("query" + (i + 1).ToString(), firstPartQuery + "(" + sIDs + ")");
                    if (endIndex != computedEndIndex)
                    {
                        break;
                    }
                }

                //Execute the queries and report progress
                ReportProgress(String.Format("Step {0}/{1}: Getting Attributes (Batch {2}/{3})",
                                            CurrentStep,NrOfSteps,(k+1),nrOfMultiqueries));
                returnedAttributes = m_oFb.ExecuteFQLMultiquery(queries);

                //Extract attributes from the JSONObject
                //and insert them to a dictionary.
                //Loop through the queries
                for (int i = 0; i < returnedAttributes.Array.Length; i++)
                {
                    //Loop through the results of the queries
                    for (int j = 0; j < returnedAttributes.Array[i].Dictionary["fql_result_set"].Array.Length; j++)
                    {
                        string sUID = returnedAttributes.Array[i].Dictionary["fql_result_set"].Array[j].Dictionary["uid"].String;

                        foreach (KeyValuePair<string,JSONObject> kvp in returnedAttributes.Array[i].Dictionary["fql_result_set"].Array[j].Dictionary)
                        {
                            try
                            {                               
                                if (kvp.Key == "name" &&
                                    String.IsNullOrEmpty(oVertices[sUID].Name))
                                {
                                    oVertices[sUID].Name = ManageDuplicateNames(kvp.Value.String);
                                }
                                if (kvp.Key == "profile_update_time")
                                {
                                    int iTimeStamp;
                                    if (int.TryParse(kvp.Value.String, out iTimeStamp))
                                    {
                                        oVertices[sUID].Attributes[kvp.Key] = JSONObject.CreateFromString("\""+DateUtil.ConvertToDateTime(iTimeStamp).ToString()+"\"");
                                    }

                                }
                                else
                                {
                                    oVertices[sUID].Attributes[kvp.Key] = kvp.Value;
                                }
                                
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
            }

            //Some commenters, likers or users tagged are pages
            //and we can not get attributes for them,
            //so they are removed from the vertices list
            oVertices.RemoveWhere(x => String.IsNullOrEmpty(x.Name));
        }

        

                     

        private void AddVertices
        (

        )
        {
            XmlNode oVertexXmlNode;

            foreach (Vertex oVertex in oVertices)
            {
                oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(oVertex.Name);                
                AddVertexAttributes(oVertexXmlNode, oVertex);
            }
        }

        private void AddVertexAttributes
        (
            XmlNode oVertexXmlNode,
            Vertex oVertex
        )
        {
            string sAttribtueValue;
            foreach (KeyValuePair<AttributeUtils.Attribute, JSONObject> kvp in oVertex.Attributes)
            {

                if (kvp.Value == null || (kvp.Value.String == null && !kvp.Value.IsDictionary))
                {
                    sAttribtueValue = "";
                }
                else if (kvp.Key.value.Equals("hometown_location"))
                {

                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "hometown", kvp.Value.Dictionary.ContainsKey("name") ? kvp.Value.Dictionary["name"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "hometown_city", kvp.Value.Dictionary.ContainsKey("city") ? kvp.Value.Dictionary["city"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "hometown_state", kvp.Value.Dictionary.ContainsKey("state") ? kvp.Value.Dictionary["state"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "hometown_country", kvp.Value.Dictionary.ContainsKey("country") ? kvp.Value.Dictionary["country"].String : "");
                }
                else if (kvp.Key.value.Equals("current_location"))
                {
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "location", kvp.Value.Dictionary.ContainsKey("name") ? kvp.Value.Dictionary["name"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "location_city", kvp.Value.Dictionary.ContainsKey("city") ? kvp.Value.Dictionary["city"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "location_state", kvp.Value.Dictionary.ContainsKey("state") ? kvp.Value.Dictionary["state"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "location_country", kvp.Value.Dictionary.ContainsKey("country") ? kvp.Value.Dictionary["country"].String : "");
                }
                else
                {
                    if (kvp.Value.String.Length > 8000)
                    {
                        sAttribtueValue = kvp.Value.String.Remove(8000);
                    }
                    else
                    {
                        sAttribtueValue = kvp.Value.String;
                    }

                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, kvp.Key.value, sAttribtueValue);
                }


            }

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "type", oVertex.Type);

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuTextID,
                "Open Facebook Page for This User");
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuActionID,
                FacebookURL + oVertex.ID);

            AppendVertexTooltipXmlNodes(oGraphMLXmlDocument, oVertexXmlNode, oVertex.Name, oVertex.ToolTip==null?"":oVertex.ToolTip);

            if (oVertex.Attributes.ContainsKey("pic_small") &&
                oVertex.Attributes["pic_small"] != null)
            {
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image", oVertex.Attributes["pic_small"].String);
            }
            
        }

        private void AddEdgeAttributes
        (
            XmlNode oEdgeXmlNode,
            Edge oEdge
        )
        {
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_type", oEdge.Type);
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_origin", oEdge.FeedOfOrigin);
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, RelationshipID, oEdge.Relationship);            
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_comment", oEdge.Comment);
            oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_timestamp", oEdge.Timestamp==DateTime.MinValue?"":oEdge.Timestamp.ToString());
            
        }

        private void AddEdges
        (
        )
        {
            XmlNode oEdgeXmlNode;
            foreach (Edge oEdge in oEdges)
            {
                try
                {
                    oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(oEdge.Vertex1.Name,
                            oEdge.Vertex2.Name);
                    AddEdgeAttributes(oEdgeXmlNode, oEdge);
                }
                catch (KeyNotFoundException ex)
                {
                    //Do Nothing.
                }
            }
        }

        private string ManageDuplicateNames
        (
            string sName
        )
        {
            int iSequenceNumber = 0;
            string sNewName = sName;
            while(oVertices.Any(x=>x.Name==sNewName))
            {
                sNewName = sName+iSequenceNumber.ToString();
                iSequenceNumber++;
            }

            return sNewName;
        }

        private JSONObject
        ExecuteFQLMultiqueryWithRetry
        (
            Dictionary<string, string> oQueries,
            string sProgress
        )
        {
            JSONObject result = null;
            bool retry = true;
            int nrOfRetries = 0;
            string retrying = "";
            //Retry until a max number of retries is reached,
            //in case we get an internal server error
            while (retry && nrOfRetries < 10)
            {
                nrOfRetries++;
                ReportProgress(sProgress + retrying);
                try
                {
                    result = m_oFb.ExecuteFQLMultiquery(oQueries);
                    retry = false;
                    retrying = "";
                }
                catch (FacebookAPIException e)
                {
                    retrying = " - Retrying";                    
                    if (!(e.Message.IndexOf("The remote server returned an error: (500) Internal Server Error.", StringComparison.OrdinalIgnoreCase) >= 0)&&
                        !(e.Message.IndexOf("Service temporarily unavailable", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        //TODO: If we catch a call limit exception we should wait 600s Calls to stream have exceeded the rate of 600 calls per 600 seconds.
                        if (e.Message.IndexOf("Calls to stream have exceeded the rate of 600 calls per 600 seconds.", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            iSecondsToWait = 600;
                            oTimer.Interval = 1000;
                            oTimer.Enabled = true;
                            sTimerProgress = sProgress;                            
                            oTimer.Start();
                            Thread.Sleep(600 * 1000);
                            oTimer.Stop();
                        }
                        else
                        {
                            retry = false;
                            retrying = "";
                            throw e;
                        }
                    }
                }
            }

            return result;
        }

        void oTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {            
            iSecondsToWait--;
            string sProgress = sTimerProgress;
            if (iSecondsToWait < 0)
            {                
                //oTimer.Enabled = false;
                oTimer.Stop();
            }

            ReportProgress(sProgress + String.Format(" - Resuming in {0} seconds", iSecondsToWait));
        }

        void oTimer_Tick(object sender, EventArgs e)
        {
            
            
        }

        //*************************************************************************
        //  Method: GetNetworkDescription()
        //
        /// <summary>
        /// Gets a description of the network.
        /// </summary>
        ///
        /// <param name="includeMe">
        /// A boolean value that states if the logged in user
        /// is included in the network.
        /// </param>
        /// 
        /// <param name="getWallPosts">
        /// Specifies if wall posts are downloaded.
        /// </param>
        /// 
        /// <param name="getStatusUpdates">
        /// Specifies if status updates are downloaded.
        /// </param>
        ///
        /// <param name="oGraphMLXmlDocument">
        /// The GraphMLXmlDocument that contains the network.
        /// </param>
        ///
        /// <returns>
        /// A description of the network.
        /// </returns>
        //*************************************************************************

        protected String
        GetNetworkDescription
        (
            List<NetworkType> oEdgeType,
            bool bDownloadFromPostToPost,
            bool bDownloadBetweenDates,
            int iFromPost,
            int iToPost,
            DateTime oStartDate,
            DateTime oEndDate,
            bool bLimitCommentsLikes,
            int iNrLimit,
            GraphMLXmlDocument oGraphMLXmlDocument
        )
        {            
            Debug.Assert(oGraphMLXmlDocument != null);
            AssertValid();

            NetworkDescriber oNetworkDescriber = new NetworkDescriber();

            

            oNetworkDescriber.AddSentence(
                "The graph represents the network of "+ GetLoggedInUsername()+"\'s friends."
                );

            oNetworkDescriber.AddNetworkTime();
            
            oNetworkDescriber.AddSentence(
                    "The network has "+oVertices.Count+" vertices and "+
                    oEdges.Count+" edges."
                    );

            if (oEdgeType.Contains(NetworkType.TimelinePostAuthors))
            {
                oNetworkDescriber.AddSentence(
                "There exists an edge for each post author."
                );
            }
            if (oEdgeType.Contains(NetworkType.TimelineUserComments))
            {
                oNetworkDescriber.AddSentence(
                "There exists an edge for each user that has commented on a post."
                );
            }
            if (oEdgeType.Contains(NetworkType.TimelineUserLikes))
            {
                oNetworkDescriber.AddSentence(
                "There exists an edge for each user that has liked a post."
                );
            }
            if (oEdgeType.Contains(NetworkType.TimelineUserTagged))
            {
                oNetworkDescriber.AddSentence(
                "There exists an edge for each user tagged in a post."
                );
            }

            oNetworkDescriber.AddSentence(
                "The network is built "
                );

            if (bDownloadFromPostToPost)
            {
                oNetworkDescriber.AddSentence(
                "from post number "+iFromPost+" to post number "+iToPost+" of each friend timeline."
                );
            }
            else if (bDownloadBetweenDates)
            {
                oNetworkDescriber.AddSentence(
                "upon posts between " + oStartDate.ToString() + " and " + oEndDate.ToString() + " of each friend timeline."
                );
            }

            if(bLimitCommentsLikes)
            {
                oNetworkDescriber.AddNetworkLimit(iNrLimit, "comments/likes");
            }

            return (oNetworkDescriber.ConcatenateSentences());
        }

        private string GetLoggedInUsername
        (
        )
        {
            Vertex oEgoVertex = oVertices.FirstOrDefault(x => x.Type == "Ego");
            if (oEgoVertex != null)
            {
                return oEgoVertex.Name;
            }
            else
            {
                JSONObject oEgoVertexJSON = DownloadEgo();

                return oEgoVertexJSON.Dictionary["name"].String;
            }
        }
        
        //*************************************************************************
        //  Method: IncludeMe()
        //
        /// <summary>
        /// Includes also the user and its connections in the graph
        /// </summary>
        ///
        /// <param name="fb">
        /// The initialized FacebookAPI object that will be used to make Facebook calls
        /// </param> 
        ///
        /// <param name="friends">
        /// The list of friends gathered from Facebook
        /// </param>
        /// 
        /// <param name="oGraphMLXmlDocument">
        /// The GraphML document that holds the edges
        /// </param> 
        //*************************************************************************
       

        //*************************************************************************
        //  Method: GetURLs()
        //
        /// <summary>
        /// Returns the URLs found in a string
        /// </summary>
        ///
        /// <param name="txt">
        /// The text to search for URLs
        /// </param> 
        ///
        /// <param name="concatenator">
        /// The concatenator of the URLs
        /// </param>
        /// 
        /// <returns>
        /// A string with found URLs concatenated with the specified concatenator
        /// </returns>
        //*************************************************************************
        private string GetURLs(string txt, char concatenator)
        {
            Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);

            txt=" ";
            foreach (Match match in mactches)
            {
                txt += match.Value + concatenator;

            }

            return txt.Remove(txt.Length - 1);
        }

        //*************************************************************************
        //  Method: GetHashtags()
        //
        /// <summary>
        /// Returns the hashtags found in a string
        /// </summary>
        ///
        /// <param name="txt">
        /// The text to search for hashtags
        /// </param> 
        ///
        /// <param name="concatenator">
        /// The concatenator of the hashtags
        /// </param>
        /// 
        /// <returns>
        /// A string with found hashtags concatenated with the specified concatenator
        /// </returns>
        //*************************************************************************
        private string GetHashtags(string txt, char concatenator)
        {            
            //(#)((?:[A-Za-z0-9-_]*))
            Regex regx = new Regex("(?:(?<=\\s)|^)#(\\w*[A-Za-z_]+\\w*)", RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);

            txt = " ";
            foreach (Match match in mactches)
            {
                txt += match.Value + concatenator;

            }

            return txt.Remove(txt.Length - 1);
        }

        //*************************************************************************
        //  Method: CreateGraphMLXmlDocument()
        //
        /// <summary>
        /// Creates a GraphMLXmlDocument representing a network of friends in Facebook.
        /// </summary>
        ///        
        /// <returns>
        /// A GraphMLXmlDocument representing a network of Facebook friends.  The
        /// document includes GraphML-attribute definitions but no vertices or
        /// edges.
        /// </returns>
        //*************************************************************************

        protected GraphMLXmlDocument
        CreateGraphMLXmlDocument
        (
            AttributesDictionary<bool> attributes
        )
        {
            AssertValid();

            GraphMLXmlDocument oGraphMLXmlDocument = new GraphMLXmlDocument(false);

            DefineImageFileGraphMLAttribute(oGraphMLXmlDocument);
            DefineCustomMenuGraphMLAttributes(oGraphMLXmlDocument);
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, TooltipID,
            "Tooltip", "string", null);
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, "type", "Type", "string", null);

            oGraphMLXmlDocument.DefineGraphMLAttribute(true, "e_type", "Edge Type", "string", null);
            oGraphMLXmlDocument.DefineGraphMLAttribute(true, "e_comment", "Tweet", "string", null);
            oGraphMLXmlDocument.DefineGraphMLAttribute(true, "e_origin", "Feed of Origin", "string", null);
            oGraphMLXmlDocument.DefineGraphMLAttribute(true, "e_timestamp", "Timestamp", "string", null);
            DefineRelationshipGraphMLAttribute(oGraphMLXmlDocument);

            foreach (KeyValuePair<AttributeUtils.Attribute, bool> kvp in attributes)
            {
                if (kvp.Value)
                {
                    if (kvp.Key.value.Equals("hometown_location"))
                    {
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, "hometown",
                        "Hometown", "string", null);
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, "hometown_city",
                        "Hometown City", "string", null);
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, "hometown_state",
                        "Hometown State", "string", null);
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, "hometown_country",
                        "Hometown Country", "string", null);
                    }
                    else if (kvp.Key.value.Equals("current_location"))
                    {
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, "location",
                        "Current Location", "string", null);
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, "location_city",
                        "Current Location City", "string", null);
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, "location_state",
                        "Current Location State", "string", null);
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, "location_country",
                        "Current Location Country", "string", null);
                    }
                    else
                    {
                        oGraphMLXmlDocument.DefineGraphMLAttribute(false, kvp.Key.value,
                        kvp.Key.name, "string", null);
                    }
                }
            }            

            return (oGraphMLXmlDocument);
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
                e.Result = GetFriendsNetworkInternal
                    (
                    oGetNetworkAsyncArgs.AccessToken,                    
                    oGetNetworkAsyncArgs.EdgeType,
                    oGetNetworkAsyncArgs.DownloadFromPostToPost,
                    oGetNetworkAsyncArgs.DownloadBetweenDates,
                    oGetNetworkAsyncArgs.EgoTimeline,
                    oGetNetworkAsyncArgs.FriendsTimeline,
                    oGetNetworkAsyncArgs.FromPost,
                    oGetNetworkAsyncArgs.ToPost,
                    oGetNetworkAsyncArgs.StartDate,
                    oGetNetworkAsyncArgs.EndDate,
                    oGetNetworkAsyncArgs.LimitCommentsLikes,
                    oGetNetworkAsyncArgs.Limit,
                    oGetNetworkAsyncArgs.GetTooltips,
                    oGetNetworkAsyncArgs.IncludeMe,
                    oGetNetworkAsyncArgs.attributes
                    );
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

        public String txt = "";

        //*************************************************************************
        //  Protected fields
        //*************************************************************************        

        /// GraphML-attribute IDs.

        protected const String UserID = "uid";
        ///
        protected const String UserNameID = "UserName";  
        ///
        private FacebookAPI m_oFb;
        ///
        private VertexCollection oVertices = new VertexCollection();
        ///
        private VertexCollection oVerticesToQuery;
        ///
        private EdgeCollection oEdges = new EdgeCollection();
        ///
        private Dictionary<string, List<JSONObject>> oPosts = new Dictionary<string, List<JSONObject>>();
        ///
        private List<JSONObject> oCommenters = new List<JSONObject>();
        ///         
        private List<JSONObject> oLikers = new List<JSONObject>();
        ///
        private System.Timers.Timer oTimer = new System.Timers.Timer();        
        ///
        private int iSecondsToWait = 600;
        ///
        private string sTimerProgress;
        ///
        private GraphMLXmlDocument oGraphMLXmlDocument;
        


        //*************************************************************************
        //  Embedded class: GetNetworkAsyncArgs()
        //
        /// <summary>
        /// Contains the arguments needed to asynchronously get a network of Flickr
        /// users.
        /// </summary>
        //*************************************************************************

        protected class GetNetworkAsyncArgs : GetNetworkAsyncArgsBase
        {               
            ///
            public AttributesDictionary<bool> attributes;
            ///           
            public List<NetworkType> EdgeType;
            ///
            public bool DownloadFromPostToPost;
            ///
            public bool DownloadBetweenDates;
            ///
            public bool EgoTimeline;
            ///
            public bool FriendsTimeline;
            ///
            public int FromPost;
            ///
            public int ToPost;
            ///
            public DateTime StartDate;
            ///
            public DateTime EndDate;
            ///
            public bool LimitCommentsLikes;
            ///
            public int Limit;
            ///
            public bool GetTooltips;
            ///
            public bool IncludeMe;
        };
        
    }

}
