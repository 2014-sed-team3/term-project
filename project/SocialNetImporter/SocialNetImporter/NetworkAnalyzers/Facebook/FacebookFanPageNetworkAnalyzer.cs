using System;
using System.Net;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.SocialNetworkLib;
using Smrf.AppLib;
using Smrf.XmlLib;
using System.IO;
using System.Text;
using Facebook;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
//using System.Threading;


namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    //*****************************************************************************
    //  Class: FacebookFanPageNetworkAnalyzer
    //
    /// <summary>
    /// Gets networks of Facebook fan page.
    /// </summary>
    ///
    /// <remarks>
    /// Use <see cref="GetNetworkAsync" /> to asynchronously get a undirected network
    /// of a Facebook fan page.
    /// </remarks>
    //*****************************************************************************

    public class FacebookFanPageNetworkAnalyzer : FacebookNetworkAnalyzerBase
    {
        //*************************************************************************
        //  Constructor: sdcsd()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="sdcsd" /> class.
        /// </summary>
        //*************************************************************************

        public FacebookFanPageNetworkAnalyzer()
        {
            // (Do nothing.)

            AssertValid();
        }     

        

        private int NrOfSteps = 5;
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
            String fanPageUsernameID,
            List<NetworkType> netTypes,
            int iFromPost,
            int iToPost,
            Dictionary<Attribute,bool> attributes,
            bool getStatusUpdates,
            bool getWallPosts,
            bool includeOthers,
            DateTime startDate,
            DateTime endDate,
            int iLimit
        )
        {            
            Debug.Assert(!String.IsNullOrEmpty(s_accessToken));
            AssertValid();
            
            const String MethodName = "GetNetworkAsync";
            CheckIsBusy(MethodName);

            GetNetworkAsyncArgs oGetNetworkAsyncArgs = new GetNetworkAsyncArgs();

            oGetNetworkAsyncArgs.AccessToken = s_accessToken;
            oGetNetworkAsyncArgs.fanPageUsernameID = fanPageUsernameID;
            oGetNetworkAsyncArgs.netTypes = netTypes;
            oGetNetworkAsyncArgs.FromPost = iFromPost;
            oGetNetworkAsyncArgs.ToPost = iToPost;
            oGetNetworkAsyncArgs.attributes = attributes;
            oGetNetworkAsyncArgs.getStatusUpdates = getStatusUpdates;
            oGetNetworkAsyncArgs.getWallPosts = getWallPosts;
            oGetNetworkAsyncArgs.includeOthers = includeOthers;
            oGetNetworkAsyncArgs.startDate = startDate;
            oGetNetworkAsyncArgs.endDate = endDate;
            oGetNetworkAsyncArgs.limit = iLimit;
            
            m_oBackgroundWorker.RunWorkerAsync(oGetNetworkAsyncArgs);
        }

        //*************************************************************************
        //  Method: GetNetwork()
        //
        /// <summary>
        /// Synchronously gets a directed network of a Facebook Fan Page.
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
            String fanPageUsernameID,
            List<NetworkType> netTypes,
            int iFromPost,
            int iToPost,
            Dictionary<Attribute, bool> attributes,
            bool getStatusUpdates,
            bool getWallPosts,
            bool includeOthers,
            DateTime startDate,
            DateTime endDate,
            int iLimit
        )
        {
            Debug.Assert(!String.IsNullOrEmpty(s_accessToken));
            AssertValid();

            const String MethodName = "GetNetwork";
            CheckIsBusy(MethodName);

            return (GetFanPageNetworkInternal(s_accessToken, fanPageUsernameID,
                                                netTypes, iFromPost, iToPost,
                                                attributes, getStatusUpdates,
                                                getWallPosts, includeOthers,
                                                startDate, endDate,
                                                iLimit));
        }

        //*************************************************************************
        //  Method: GetFanPageNetworkInternal()
        //
        /// <summary>
        /// Gets different networks for a fan page.
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
        GetFanPageNetworkInternal
        (
            string sAccessToken,
            string fanPageUsernameID,            
            List<NetworkType> netTypes,
            int iFromPost,
            int iToPost,
            Dictionary<Attribute,bool> attributes,
            bool getStatusUpdates,
            bool getWallPosts,
            bool includeOthers,
            DateTime startDate,
            DateTime endDate,
            int iLimit
        )

        {
            JSONObject streamPosts;            
            Dictionary<string, Dictionary<string, List<string>>> commentersComments = new Dictionary<string,Dictionary<string,List<string>>>();
            Dictionary<string, List<string>> likersPosts=new Dictionary<string,List<string>>();
            usersDisplayName = new Dictionary<string,string>();

            fbAPI = new FacebookAPI(sAccessToken);

            bGetStatusUpdates = getStatusUpdates;
            bGetWallPosts = getWallPosts;

            if (bGetStatusUpdates) NrOfSteps++;
            if (bGetWallPosts) NrOfSteps++;

            RequestStatistics oRequestStatistics = new RequestStatistics();

            //Download data
            streamPosts = DownloadPosts(fanPageUsernameID, iFromPost, iToPost, includeOthers, startDate, endDate);

            if (netTypes.Contains(NetworkType.UserUserComments) ||
                netTypes.Contains(NetworkType.UserPostComments) ||
                netTypes.Contains(NetworkType.PostPostComments))
            {
                commentersComments = DownloadComments(streamPosts, iLimit);
            }
            if (netTypes.Contains(NetworkType.UserUserLikes) ||
                netTypes.Contains(NetworkType.UserPostLikes) ||
                netTypes.Contains(NetworkType.PostPostLikes))
            {
                likersPosts = DownloadLikes(streamPosts, iLimit);
            }

            //Create the network
            GraphMLXmlDocument oGraphMLXmlDocument = CreateGraphMLXmlDocument(attributes, netTypes);
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues = new Dictionary<string, Dictionary<string, JSONObject>>();

            AddVertices(ref oGraphMLXmlDocument, ref attributeValues, 
                        attributes, netTypes,
                        commentersComments, likersPosts, streamPosts);

            AddEdges(ref oGraphMLXmlDocument, netTypes,
                    commentersComments, likersPosts,
                    streamPosts, attributeValues);

            OnNetworkObtainedWithoutTerminatingException(oGraphMLXmlDocument, oRequestStatistics,
                GetNetworkDescription(streamPosts, fanPageUsernameID, netTypes,
                                      iFromPost, iToPost, startDate, endDate, oGraphMLXmlDocument));

            return oGraphMLXmlDocument;
        }

        private JSONObject
        DownloadPosts
        (
            string fanPageUsernameID,
            int iFromPost,
            int iToPost,
            Boolean includeOthers,
            DateTime startDate,
            DateTime endDate
        )
        {
            Debug.Assert(fbAPI!=null);
            AssertValid();

            JSONObject streamPosts;
            string fanPageID;
            string sFilterKey = " AND filter_key='owner'";


            CurrentStep++;
            ReportProgress(String.Format("Step {0}/{1}: Downloading Fan Page Data", CurrentStep, NrOfSteps));

            //Get fan page details from Facebook
            JSONObject fanPage = ExecuteFQLWithRelogin("SELECT page_id, name, username, description, pic_small FROM page " +
                                                    "WHERE page_id = '" + fanPageUsernameID + "' OR username = '" + fanPageUsernameID + "'", false);

            fanPageID = fanPage.Array[0].Dictionary["page_id"].String;

            CurrentStep++;

            if (includeOthers)
            {
                sFilterKey = "";
            }

            //Download x first posts
            if (iFromPost > 0 && iToPost>0)
            {
                ReportProgress(String.Format("Step {0}/{1}: Downloading From Post {2} To Post {3}", CurrentStep, NrOfSteps, iFromPost, iToPost));
                streamPosts = ExecuteFQLWithRelogin("SELECT message, post_id, comment_info, like_info, created_time, permalink, attachment FROM stream WHERE source_id='" + fanPageID + "'" + sFilterKey + " LIMIT "+(iFromPost-1)+"," + (iToPost-(iFromPost-1)),true);
            }
            else
            {
                ReportProgress(String.Format("Step {0}/{1}: Downloading posts between {2} and {3}", CurrentStep, NrOfSteps, startDate, endDate));
                streamPosts = ExecuteFQLWithRelogin("SELECT message, post_id, comment_info, like_info, created_time, permalink, attachment FROM stream WHERE source_id='"
                                            + fanPageID + "'" + sFilterKey + " AND created_time>=" + DateUtil.ConvertToTimestamp(startDate).ToString() +
                                            " AND created_time<=" + DateUtil.ConvertToTimestamp(endDate).ToString() +" LIMIT 500",true);
            }

            return streamPosts;
        }

        private Dictionary<string, List<string>>
        DownloadLikes
        (
            JSONObject streamPosts,
            int iLimit
        )
        {
            Debug.Assert(fbAPI!=null);
            Debug.Assert(streamPosts.IsArray);
            AssertValid();

            JSONObject result=null;
            List<JSONObject> likes = new List<JSONObject>();

            CurrentStep++;
            ReportProgress(String.Format("Step {0}/{1}: Downloading likers network", CurrentStep, NrOfSteps));
            if (streamPosts.IsArray)
            {
                for (int i = 0; i < streamPosts.Array.Length; i++)
                {
                    //Get the total number of likes for the actual post
                    if (!streamPosts.Array[i].Dictionary["like_info"].Dictionary.ContainsKey("like_count")) continue;
                    int totalNrOfLikes = iLimit<1?(int)streamPosts.Array[i].Dictionary["like_info"].Dictionary["like_count"].Integer:iLimit;
                    //We can get a maximum of 10000 results for multiqueries, so we have to make multiple multiqueries
                    //We have to get information from two different tables and we are limited to 5000 results each
                    //for a total of 10000 results for the whole multiquery
                    int nrOfMultiqueries = (int)Math.Ceiling(totalNrOfLikes / 5000.0);
                    int nrOfQueries = 10;

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
                            queries.Add("query" + (k + 1).ToString(), "SELECT user_id, post_id FROM like WHERE post_id='" + streamPosts.Array[i].Dictionary["post_id"].String + "' LIMIT " + (k * 1000).ToString() + ", "+nrRes);                            
                        }

                        result = ExecuteFQLMultiqueryWithRetryRelogin(queries, String.Format("Step {0}/{1}: Downloading likes - Post {2}/{3}(Batch {4}/{5})",
                                                        CurrentStep, NrOfSteps, i + 1, streamPosts.Array.Length, j + 1, nrOfMultiqueries), false);
                           
                                                

                        //After successfull run of the multiquery extract the data
                        if (result.IsArray)
                        {
                            for (int k = 0; k < result.Array.Length; k++)
                            {
                                likes.AddRange(result.Array[k].Dictionary["fql_result_set"].Array);                                
                            }

                        }
                    }                   
                }
            }

            Dictionary<string, List<string>> likersPosts = (from l in likes
                                                                                       group l by new { FromID = l.Dictionary["user_id"].String } into g
                                                                                       select new
                                                                                       {
                                                                                           Key = g.Key.FromID,
                                                                                           PostID = (from x in g
                                                                                                     group x by x.Dictionary["post_id"].String into g2
                                                                                                     select  g2.Key                                                                                                     
                                                                                              ).ToList()

                                                                                       }).ToDictionary(x => x.Key, x => x.PostID);

            return likersPosts;
        }

        private Dictionary<string, Dictionary<string, List<string>>>
        DownloadComments
        (
            JSONObject streamPosts,
            int iLimit
        )
        {
            Debug.Assert(fbAPI != null);
            Debug.Assert(streamPosts.IsArray);
            AssertValid();

            JSONObject result = null;
            List<JSONObject> comments = new List<JSONObject>();

            CurrentStep++;
            ReportProgress(String.Format("Step {0}/{1}: Downloading commenters network", CurrentStep, NrOfSteps));
            if (streamPosts.IsArray)
            {
                for (int i = 0; i < streamPosts.Array.Length; i++)
                {
                    //Get the total number of likes for the actual post
                    int totalNrOfComments = iLimit<1?(int)streamPosts.Array[i].Dictionary["comment_info"].Dictionary["comment_count"].Integer:iLimit;
                    //We can get a maximum of 10000 results for multiqueries, so we have to make multiple multiqueries
                    //We have to get information from two different tables and we are limited to 5000 results each
                    //for a total of 10000 results for the whole multiquery
                    int nrOfMultiqueries = (int)Math.Ceiling(totalNrOfComments / 5000.0);
                    int nrOfQueries = 10;

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
                            queries.Add("query" + (k + 1).ToString(), "SELECT fromid, text, post_id FROM comment WHERE post_id='" + streamPosts.Array[i].Dictionary["post_id"].String + "' ORDER BY time ASC LIMIT " + (k * 1000).ToString() + ", "+nrRes);
                        }

                        result = ExecuteFQLMultiqueryWithRetryRelogin(queries, String.Format("Step {0}/{1}: Downloading comments - Post {2}/{3}(Batch {4}/{5})",
                                                        CurrentStep, NrOfSteps, i + 1, streamPosts.Array.Length, j + 1, nrOfMultiqueries),false);
                                               

                        //After successfull run of the multiquery extract the data
                        if (result != null && result.IsArray)
                        {
                            for (int k = 0; k < result.Array.Length; k++)
                            {
                                comments.AddRange(result.Array[k].Dictionary["fql_result_set"].Array);
                            }

                        }
                    }
                }                
            }

            Dictionary<string, Dictionary<string, List<string>>> commentersComments = (from c in comments
                                                                                       group c by new { FromID = c.Dictionary["fromid"].String } into g
                                                                                       select new
                                                                                       {
                                                                                           Key = g.Key.FromID,
                                                                                           PostID = (from x in g
                                                                                                     group x by x.Dictionary["post_id"].String into g2
                                                                                                     select new
                                                                                                     {
                                                                                                         PostKey = g2.Key,
                                                                                                         List = g2.Select(c => c.Dictionary["text"].String).ToList()
                                                                                                     }
                                                                                              ).ToDictionary(y => y.PostKey, y => y.List)

                                                                                       }).ToDictionary(x => x.Key, x => x.PostID);

            return commentersComments;
        }

        private JSONObject
        ExecuteFQLMultiqueryWithRetryRelogin
        (
            Dictionary<string,string> oQueries,
            string sProgress,
            bool bForcePrevent
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
                ReportProgress(sProgress+retrying);
                try
                {
                    result = fbAPI.ExecuteFQLMultiquery(oQueries, bForcePrevent);
                    retry = false;
                    retrying = "";
                }
                catch (Exception e)
                {
                    retrying = " - Retrying";
                    if (e.Message.IndexOf("Error validating access token", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Relogin();
                    }                        
                    if (!(e.Message.IndexOf("The remote server returned an error: (500) Internal Server Error.", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        retry = false;
                        retrying = "";
                        throw e;
                    }
                }
            }

            return result;
        }

        private JSONObject
        ExecuteFQLWithRelogin
        (
            string sQuery,
            bool bForcePrevent
        )
        {
            JSONObject result = null;
            try
            {
                result = fbAPI.ExecuteFQL(sQuery,bForcePrevent);                
            }
            catch (Exception e)
            {
                if (e.Message.IndexOf("Error validating access token", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Relogin();
                }
            }

            return result;
        }

        private JSONObject
        CallGraphAPIWithRelogin
        (
            string sRelativePath
        )
        {
            JSONObject result = null;
            try
            {
                result = fbAPI.Get(sRelativePath);
            }
            catch (Exception e)
            {
                if (e.Message.IndexOf("Error validating access token", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Relogin();
                }
            }

            return result;
        }

        private void
        AddVertices
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            ref Dictionary<string, Dictionary<string, JSONObject>> attributeValues,            
            Dictionary<Attribute, bool> attributes,            
            List<NetworkType> netTypes,
            Dictionary<string, Dictionary<string, List<string>>> commentersComments,
            Dictionary<string, List<string>> likersPosts,
            JSONObject streamPosts
        )
        {            
            Dictionary<string, List<Dictionary<string, object>>> statusUpdates = new Dictionary<string, List<Dictionary<string, object>>>();
            Dictionary<string, List<Dictionary<string, object>>> wallPosts = new Dictionary<string, List<Dictionary<string, object>>>();
            List<string> uniqueVertices;            

            
            //In case commenters and likers vertices need to be added
            //we have to produce unique vertices since a liker can be
            //also a commenter and this will result in having duplicate
            //vertices
            if ((netTypes.Contains(NetworkType.UserUserComments) ||                
                netTypes.Contains(NetworkType.UserPostComments)) &&
                netTypes.Contains(NetworkType.UserUserLikes) ||                
                netTypes.Contains(NetworkType.UserPostLikes))
            {
                uniqueVertices = commentersComments.Keys.ToList().Union(likersPosts.Keys.ToList()).Distinct().ToList();
                attributeValues = getAttributes(attributes, uniqueVertices);
                ManageDisplayNames(ref attributeValues);
                if (bGetStatusUpdates)
                {
                    statusUpdates = GetStatusUpdates(uniqueVertices);
                }

                if (bGetWallPosts)
                {
                    wallPosts = GetWallPosts(uniqueVertices);
                }

                //Add all commenters vertices
                AddCommenterVertices(ref oGraphMLXmlDocument,
                                     commentersComments, attributeValues,                                       
                                     statusUpdates, wallPosts);                

                //Add likers vertices by excluding those who are also commenters
                AddLikerVertices(ref oGraphMLXmlDocument, likersPosts.Where(x=>!commentersComments.ContainsKey(x.Key)).ToDictionary(y=>y.Key, y=>y.Value),
                    attributeValues, statusUpdates,wallPosts);
                
                
            }               
            //Only commenters vertices will be added
            else if (netTypes.Contains(NetworkType.UserUserComments) ||
                    netTypes.Contains(NetworkType.UserPostComments))
            {
                attributeValues = getAttributes(attributes, commentersComments.Keys.ToList());
                ManageDisplayNames(ref attributeValues);
                if (bGetStatusUpdates)
                {
                    statusUpdates = GetStatusUpdates(commentersComments.Keys.ToList());
                }

                if (bGetWallPosts)
                {
                    wallPosts = GetWallPosts(commentersComments.Keys.ToList());
                }
                AddCommenterVertices(ref oGraphMLXmlDocument,
                                     commentersComments, attributeValues,                                      
                                     statusUpdates, wallPosts);                
            }
            //Only likers vertices will be added
            else if (netTypes.Contains(NetworkType.UserUserLikes) ||
                    netTypes.Contains(NetworkType.UserPostLikes))
            {
                attributeValues = getAttributes(attributes, likersPosts.Keys.ToList());
                ManageDisplayNames(ref attributeValues);
                if (bGetStatusUpdates)
                {
                    statusUpdates = GetStatusUpdates(likersPosts.Keys.ToList());
                }

                if (bGetWallPosts)
                {
                    wallPosts = GetWallPosts(likersPosts.Keys.ToList());
                }
                AddLikerVertices( ref oGraphMLXmlDocument,
                                likersPosts, attributeValues,                                 
                                statusUpdates, wallPosts);
            }

            //Add post vertices if any related network is selected
            if (netTypes.Contains(NetworkType.UserPostComments) ||
                netTypes.Contains(NetworkType.UserPostLikes) ||
                netTypes.Contains(NetworkType.PostPostComments) ||
                netTypes.Contains(NetworkType.PostPostLikes))
            {
                AddPostVertices(ref oGraphMLXmlDocument, streamPosts);
            }
        }

        private void
        AddCommenterVertices
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            Dictionary<string, Dictionary<string, List<string>>> commentersComments,
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues,            
            Dictionary<string, List<Dictionary<string, object>>> statusUpdates,
            Dictionary<string, List<Dictionary<string, object>>> wallPosts            
        )
        {            
            XmlNode oVertexXmlNode;
            commentersWithNoAttributes = new Dictionary<string, string>();

            //Add nodes (commenters)
            foreach (var item in commentersComments)
            {
                if (!attributeValues.ContainsKey(item.Key))
                {
                    //This means that the commenter is not a user but a page
                    //FacebookAPI fb = new FacebookAPI();
                    JSONObject pageObject = CallGraphAPIWithRelogin("/" + item.Key);
                    if (!pageObject.IsDictionary) continue;
                    string vertexName = ManageDisplayNames(pageObject.Dictionary["name"].String, item.Key);
                    if (String.IsNullOrEmpty(vertexName)) continue;
                    oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(vertexName);

                    string sComments = String.Join("\n\n", (item.Value.SelectMany(x => x.Value).ToArray()));
                    //Add tooltip
                    AppendVertexTooltipXmlNodes(oGraphMLXmlDocument, oVertexXmlNode,
                        pageObject.Dictionary["name"].String, sComments);
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "comment", sComments);
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "type", "1");

                    if (pageObject.Dictionary.ContainsKey("picture"))
                    {
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image", pageObject.Dictionary["picture"].String);
                    }                    

                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "v_weight", item.Value.Sum(x => x.Value.Count));
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuTextID,
                        "Open Facebook Page for This User");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuActionID,
                        FacebookURL + item.Key);

                    //Add this user to the appropriate "no attributes" dictionary
                    //commentersWithNoAttributes.Add(item.Key, pageObject.Dictionary["name"].String);
                }
                else
                {
                    oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(usersDisplayName[item.Key]);

                    string sComments = String.Join("\n\n", (item.Value.SelectMany(x => x.Value).ToArray()));
                    //Add tooltip
                    AppendVertexTooltipXmlNodes(oGraphMLXmlDocument, oVertexXmlNode,
                        usersDisplayName[item.Key], sComments);
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "comment", sComments);
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "type", "1");

                    //Add attributes for the actual user
                    AddAttributes(ref oGraphMLXmlDocument, ref oVertexXmlNode, attributeValues[item.Key]);

                    if (attributeValues[item.Key].ContainsKey("pic_small") && attributeValues[item.Key]["pic_small"] != null)
                    {
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image", attributeValues[item.Key]["pic_small"].String);
                    }

                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "v_weight", item.Value.Sum(x => x.Value.Count));
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuTextID,
                        "Open Facebook Page for This User");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuActionID,
                        FacebookURL + item.Key);                    
                }
                if (bGetStatusUpdates)
                    {
                        AddStatusUpdates(ref oGraphMLXmlDocument,
                                        ref oVertexXmlNode,
                                        item.Key,
                                        statusUpdates
                                        );
                    }

                    if (bGetWallPosts)
                    {
                        AddWallPosts(ref oGraphMLXmlDocument,
                                    ref oVertexXmlNode,
                                    item.Key,
                                    wallPosts
                                    );
                    }
            }
        }

        private void
        AddLikerVertices
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            Dictionary<string, List<string>> likersPost,
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues,            
            Dictionary<string, List<Dictionary<string, object>>> statusUpdates,
            Dictionary<string, List<Dictionary<string, object>>> wallPosts
        )
        {
            XmlNode oVertexXmlNode;
            likersWithNoAttributes = new Dictionary<string, string>();

            //Add nodes
            foreach (var item in likersPost)
            {
                if (!attributeValues.ContainsKey(item.Key))
                {
                    //This means that the liker is not a user but a page
                    //FacebookAPI fb = new FacebookAPI();
                    JSONObject pageObject = CallGraphAPIWithRelogin("/" + item.Key);              
                    if (!pageObject.IsDictionary) continue;
                    string vertexName = ManageDisplayNames(pageObject.Dictionary["name"].String, item.Key);
                    if (String.IsNullOrEmpty(vertexName)) continue;
                    oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(vertexName);

                    //Add tooltip
                    AppendVertexTooltipXmlNodes(oGraphMLXmlDocument, oVertexXmlNode,
                        pageObject.Dictionary["name"].String, "");                    
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "type", "1");

                    if (pageObject.Dictionary.ContainsKey("picture"))
                    {
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image", pageObject.Dictionary["picture"].String);
                    }

                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "v_weight", item.Value.Count);
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuTextID,
                        "Open Facebook Page for This User");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuActionID,
                        FacebookURL + item.Key);

                    //Add this user to the appropriate "no attributes" dictionary
                    //likersWithNoAttributes.Add(item.Key, pageObject.Dictionary["name"].String);
                }
                else
                {
                    oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(usersDisplayName[item.Key]);
                    //Add tooltip
                    AppendVertexTooltipXmlNodes(oGraphMLXmlDocument, oVertexXmlNode,
                        usersDisplayName[item.Key], "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "type", "1");

                    //Add attributes for the actual user
                    AddAttributes(ref oGraphMLXmlDocument,
                                    ref oVertexXmlNode,
                                    attributeValues[item.Key]
                                    );

                    if (attributeValues[item.Key].ContainsKey("pic_small") && attributeValues[item.Key]["pic_small"] != null)
                    {
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image", attributeValues[item.Key]["pic_small"].String);
                    }

                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "v_weight", item.Value.Count);
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuTextID,
                        "Open Facebook Page for This User");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuActionID,
                        FacebookURL + item.Key);
                }

                if (bGetStatusUpdates)
                {
                    AddStatusUpdates(ref oGraphMLXmlDocument,
                                    ref oVertexXmlNode,
                                    item.Key,
                                    statusUpdates
                                    );
                }

                if (bGetWallPosts)
                {
                    AddWallPosts(ref oGraphMLXmlDocument,
                                ref oVertexXmlNode,
                                item.Key,
                                wallPosts
                                );
                }                
            }
        }

        private void
        AddPostVertices
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            JSONObject streamPosts
        )
        {
            XmlNode oVertexXmlNode;

            //Add nodes (posts)
            foreach (JSONObject ob in streamPosts.Array)
            {
                oVertexXmlNode = oGraphMLXmlDocument.AppendVertexXmlNode(ob.Dictionary["post_id"].String);

                //Add tooltip
                AppendVertexTooltipXmlNodes(oGraphMLXmlDocument, oVertexXmlNode,
                    ob.Dictionary["post_id"].String, ob.Dictionary["message"].String);
                //Add attributes
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "type", "2");
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, LabelColumnName, ob.Dictionary["message"].String);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "content", ob.Dictionary["message"].String);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuTextID,
                    "Open Facebook Page for This Post");
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, MenuActionID,
                    ob.Dictionary["permalink"].String);
                //If the post contains a link to a video, photo, document etc
                //and it has a picture, use it as the post image
                if(ob.Dictionary["attachment"].Dictionary.ContainsKey("media") &&
                    ob.Dictionary["attachment"].Dictionary["media"].Array.Length>0)
                {
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "Image",
                        ob.Dictionary["attachment"].Dictionary["media"].Array[0].Dictionary["src"].String);
                }
                
            }
        }

        private void
        AddEdges
        (
            ref GraphMLXmlDocument oGraphMlXmlDocument,
            List<NetworkType> netTypes,
            Dictionary<string, Dictionary<string, List<string>>> commentersComments,
            Dictionary<string, List<string>> likersPosts,
            JSONObject streamPosts,
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues
        )
        {
            if (netTypes.Contains(NetworkType.UserUserComments))
            {
                AddUserUserCommentsEdges(ref oGraphMlXmlDocument, commentersComments,
                                    streamPosts, attributeValues);
            }
            if (netTypes.Contains(NetworkType.UserUserLikes))
            {
                AddUserUserLikesEdges(ref oGraphMlXmlDocument, likersPosts,
                                streamPosts, attributeValues);
            }
            if (netTypes.Contains(NetworkType.PostPostComments))
            {
                AddPostPostCommentsEdges(ref oGraphMlXmlDocument, commentersComments);
            }
            if (netTypes.Contains(NetworkType.PostPostLikes))
            {
                AddPostPostLikesEdges(ref oGraphMlXmlDocument, likersPosts);
            }
            if (netTypes.Contains(NetworkType.UserPostComments))
            {
                AddUserPostCommentsEdges(ref oGraphMlXmlDocument, commentersComments,
                                attributeValues);
            }
            if (netTypes.Contains(NetworkType.UserPostLikes))
            {
                AddUserPostLikesEdges(ref oGraphMlXmlDocument, likersPosts,
                                attributeValues);
            }  
        }

        private void
        AddUserUserCommentsEdges
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            Dictionary<string, Dictionary<string, List<string>>> commentersComments,
            JSONObject streamPosts,
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues
        )
        {
            Dictionary<string, List<string>> postCommenters = new Dictionary<string,List<string>>();
            XmlNode oEdgeXmlNode = null;
            List<string> postsIntersection = new List<string>();
            string posts = "";

            //Create the Post-Commenter relationship from commenterComments
            foreach(JSONObject item in streamPosts.Array)
            {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> kvp in commentersComments)
                {
                    if (kvp.Value.ContainsKey(item.Dictionary["post_id"].String))
                    {
                        try
                        {
                            postCommenters[item.Dictionary["post_id"].String].Add(kvp.Key);
                            
                        }
                        catch (KeyNotFoundException e)
                        {
                            List<string> tmp = new List<string>();
                            tmp.Add(kvp.Key);
                            postCommenters.Add(item.Dictionary["post_id"].String, tmp);
                        }                        
                    }
                }
            }

            //Add Edges
            foreach (KeyValuePair<string, List<string>> kvp in postCommenters)
            {
                for (int i = 0; i < kvp.Value.Count - 1; i++)
                {
                    for (int j = i + 1; j < kvp.Value.Count; j++)
                    {
                        //Sometimes a commenter or liker can be another fan page and this is not a user
                        if (!usersDisplayName.ContainsKey(kvp.Value[i]) ||
                            !usersDisplayName.ContainsKey(kvp.Value[j])) continue;
                        
                        oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(usersDisplayName[kvp.Value[i]],
                                                                            usersDisplayName[kvp.Value[j]]);
                       
                        postsIntersection = commentersComments[kvp.Value[i]].Keys.Intersect(commentersComments[kvp.Value[j]].Keys).ToList();
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_weight", postsIntersection.Count);
                        string[] postsToJoin = (from p in postsIntersection
                                                        select new { Post = streamPosts.Array.Single(x => x.Dictionary["post_id"].String.Equals(p)).Dictionary["message"].String }).Select(x => x.Post).ToArray();
                        posts = String.Join("\n\n", postsToJoin);
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "post", posts);
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "Relationship", "Co-Commenter");                        
                    }
                }
            }                                                           
        }

        private void
        AddUserUserLikesEdges
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            Dictionary<string, List<string>> likersPosts,
            JSONObject streamPosts,
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues
        )
        {
            Dictionary<string, List<string>> postLikers = new Dictionary<string, List<string>>();
            XmlNode oEdgeXmlNode;
            List<string> postsIntersection = new List<string>();
            string posts = "";

            //Create the Post-Liker relationship from likersPosts
            foreach (JSONObject item in streamPosts.Array)
            {
                foreach (KeyValuePair<string, List<string>> kvp in likersPosts)
                {
                    if (kvp.Value.Contains(item.Dictionary["post_id"].String))
                    {
                        try
                        {
                            postLikers[item.Dictionary["post_id"].String].Add(kvp.Key);
                        }
                        catch (KeyNotFoundException e)
                        {
                            List<string> tmp = new List<string>();
                            tmp.Add(kvp.Key);
                            postLikers.Add(item.Dictionary["post_id"].String, tmp);
                        }
                    }
                }
            }

            //Add Edges
            foreach (KeyValuePair<string, List<string>> kvp in postLikers)
            {
                for (int i = 0; i < kvp.Value.Count - 1; i++)
                {
                    for (int j = i + 1; j < kvp.Value.Count; j++)
                    {
                        //Sometimes a commenter or liker can be another fan page and this is not a user
                        if (!usersDisplayName.ContainsKey(kvp.Value[i]) ||
                            !usersDisplayName.ContainsKey(kvp.Value[j])) continue;
                        
                        oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(usersDisplayName[kvp.Value[i]],
                                                                            usersDisplayName[kvp.Value[j]]);
                        
                        //postsIntersection = likersPosts[kvp.Value[i]].Intersect(likersPosts[kvp.Value[j]]).ToList();
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_weight", 1);
                        //posts = String.Join("\n\n", (from p in postsIntersection
                        //                             select new { Post = streamPosts.Array.Single(x => x.Dictionary["post_id"].String.Equals(p)).Dictionary["message"].String }).Select(x => x.Post).ToArray());
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "post", streamPosts.Array.Single(x => x.Dictionary["post_id"].String.Equals(kvp.Key)).Dictionary["message"].String);
                        oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "Relationship", "Co-Liker");
                    }
                }
            }
        }

        private void
        AddUserPostCommentsEdges
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            Dictionary<string, Dictionary<string, List<string>>> commentersComments,
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues
        )
        {
            XmlNode oEdgeXmlNode=null;

            foreach (KeyValuePair<string, Dictionary<string, List<string>>> kvp in commentersComments)
            {                
                foreach (KeyValuePair<string, List<string>> kvp2 in commentersComments[kvp.Key])
                {
                    if (!usersDisplayName.ContainsKey(kvp.Key)) continue;
                    oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(usersDisplayName[kvp.Key],
                                                                             kvp2.Key);
                   
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_weight", kvp2.Value.Count);
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "edge_comment", String.Join("\n\n", kvp2.Value.ToArray()));
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "Relationship", "User-Post Based on Comments");
                }

            }
        }

        private void
        AddUserPostLikesEdges
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            Dictionary<string, List<string>> likersPosts,
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues
        )
        {
            XmlNode oEdgeXmlNode = null;

            foreach (KeyValuePair<string, List<string>> kvp in likersPosts)
            {                
                foreach (string item in likersPosts[kvp.Key])
                {
                    if (!usersDisplayName.ContainsKey(kvp.Key)) continue;
                    oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(usersDisplayName[kvp.Key],
                                                                         item);
                                        
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_weight", 1);                    
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "Relationship", "User-Post Based on Likes");
                }

            }
        }

        private void
        AddPostPostCommentsEdges
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            Dictionary<string, Dictionary<string, List<string>>> commentersComments
        )
        {
            XmlNode oEdgeXmlNode;
            Dictionary<string, int> postCount = new Dictionary<string, int>();

            //Create the post-post network
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> kvp in commentersComments)
            {
                if (kvp.Value.Keys.Count > 1)
                {
                    List<string> post_ids = kvp.Value.Keys.ToList();
                    for (int i = 0; i < post_ids.Count - 1; i++)
                    {
                        for (int j = i + 1; j < post_ids.Count; j++)
                        {
                            try
                            {
                                postCount[post_ids[i]+"-"+post_ids[j]]++;
                            }
                            catch (KeyNotFoundException e)
                            {
                                postCount.Add(post_ids[i] + "-" + post_ids[j], 1);
                            }
                        }
                    }
                }
            }

            //Add edges
            foreach (KeyValuePair<string, int> kvp in postCount)
            {
                string[] postIDs= kvp.Key.Split('-');
                oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(postIDs[0], postIDs[1]);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_weight", kvp.Value);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "Relationship", "Post-Post Based on Comments");
            }
        }

        private void
        AddPostPostLikesEdges
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            Dictionary<string, List<string>> likersPosts
        )
        {
            XmlNode oEdgeXmlNode;
            Dictionary<string, int> postCount = new Dictionary<string, int>();

            //Create the post-post network
            foreach (KeyValuePair<string, List<string>> kvp in likersPosts)
            {
                if (kvp.Value.Count > 1)
                {                    
                    for (int i = 0; i < kvp.Value.Count - 1; i++)
                    {
                        for (int j = i + 1; j < kvp.Value.Count; j++)
                        {
                            try
                            {
                                postCount[kvp.Value[i] + "-" + kvp.Value[j]]++;
                            }
                            catch (KeyNotFoundException e)
                            {
                                postCount.Add(kvp.Value[i] + "-" + kvp.Value[j], 1);
                            }
                        }
                    }
                }
            }

            //Add edges
            foreach (KeyValuePair<string, int> kvp in postCount)
            {
                string[] postIDs = kvp.Key.Split('-');
                oEdgeXmlNode = oGraphMLXmlDocument.AppendEdgeXmlNode(postIDs[0], postIDs[1]);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "e_weight", kvp.Value);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oEdgeXmlNode, "Relationship", "Post-Post Based on Likes");
            }
        }

        private void
        AddAttributes
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            ref XmlNode oVertexXmlNode,
            Dictionary<string, JSONObject> attributeValues
        )
        {
            string attributeValue = "";

            foreach (KeyValuePair<string, JSONObject> kvp in attributeValues)
            {
                //Facebook's TOS doesn't allow the redistribution of UIDs
                //so they are not included in the network. 
                //They are used as keys for the dictionaries.
                if (kvp.Key.Equals("uid") || kvp.Key.Equals("display_name"))
                {
                    continue;
                }

                if (kvp.Value == null || (kvp.Value.String == null && !kvp.Value.IsDictionary))
                {
                    attributeValue = "";
                }
                else if (kvp.Key.Equals("hometown_location"))
                {

                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "hometown", kvp.Value.Dictionary.ContainsKey("name") ? kvp.Value.Dictionary["name"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "hometown_city", kvp.Value.Dictionary.ContainsKey("city") ? kvp.Value.Dictionary["city"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "hometown_state", kvp.Value.Dictionary.ContainsKey("state") ? kvp.Value.Dictionary["state"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "hometown_country", kvp.Value.Dictionary.ContainsKey("country") ? kvp.Value.Dictionary["country"].String : "");
                }
                else if (kvp.Key.Equals("current_location"))
                {
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "location", kvp.Value.Dictionary.ContainsKey("name") ? kvp.Value.Dictionary["name"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "location_city", kvp.Value.Dictionary.ContainsKey("city") ? kvp.Value.Dictionary["city"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "location_state", kvp.Value.Dictionary.ContainsKey("state") ? kvp.Value.Dictionary["state"].String : "");
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "location_country", kvp.Value.Dictionary.ContainsKey("country") ? kvp.Value.Dictionary["country"].String : "");
                }                
                else
                {
                    if (kvp.Key.Equals("profile_update_time"))
                    {
                        attributeValue = DateUtil.ConvertToDateTime(kvp.Value.Integer).ToString();
                    }
                    else if (kvp.Value.String.Length > 8000)
                    {
                        attributeValue = kvp.Value.String.Remove(8000);
                    }
                    else
                    {
                        attributeValue = kvp.Value.String;
                    }
                    oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, kvp.Key, attributeValue);
                }

            }
        }

        private void
        AddStatusUpdates
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            ref XmlNode oVertexXmlNode,
            string userID,
            Dictionary<string, List<Dictionary<string, object>>> statusUpdates
        )
        {
            List<Dictionary<string, object>>.Enumerator en;
            string currentStatusUpdate = "";
            string currentStatusTags = "";           


            //Add status updates
            if (statusUpdates.ContainsKey(userID))
            {
                en = statusUpdates[userID].GetEnumerator();
                currentStatusUpdate = "";
                currentStatusTags = "";
                while (en.MoveNext() && currentStatusUpdate.Length < 8000)
                {
                    if (((JSONObject)en.Current["message"]).String.Equals("")) continue;
                    currentStatusUpdate += ((JSONObject)en.Current["message"]).String + "\n\n";
                    //Add tags
                    if (en.Current["message_tags"] is List<JSONObject>)
                    {
                        foreach (JSONObject ob in (List<JSONObject>)en.Current["message_tags"])
                        {
                            currentStatusTags += ob.String + ",";
                        }
                        currentStatusTags = currentStatusTags.Remove(currentStatusTags.Length - 1);
                        currentStatusTags += "\n\n";
                    }
                }

                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "statuses", currentStatusUpdate);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "statuses_tags", currentStatusTags);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "statuses_urls", GetURLs(currentStatusUpdate, ' '));
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "statuses_hashtags", GetHashtags(currentStatusUpdate, ' '));
            }
        }

        private void
        AddWallPosts
        (
            ref GraphMLXmlDocument oGraphMLXmlDocument,
            ref XmlNode oVertexXmlNode,
            string userID,
            Dictionary<string, List<Dictionary<string, object>>> wallPosts
        )
        {
            List<Dictionary<string, object>>.Enumerator en;            
            string currentWallPost = "";
            string currentWallTags = "";

            //Add wall posts
            if (wallPosts.ContainsKey(userID))
            {
                en = wallPosts[userID].GetEnumerator();
                currentWallPost = "";
                currentWallTags = "";
                while (en.MoveNext() && currentWallPost.Length < 8000)
                {                    
                    if (((JSONObject)en.Current["message"]).String.Equals("")) continue;
                    currentWallPost += (en.Current.ContainsKey("name")?((JSONObject)en.Current["name"]).String:"[No Name]") + ": " + ((JSONObject)en.Current["message"]).String + "\n\n";
                    //Add tags
                    if (en.Current["message_tags"] is List<JSONObject>)
                    {
                        foreach (JSONObject ob in (List<JSONObject>)en.Current["message_tags"])
                        {
                            currentWallTags += ob.String + ",";
                        }
                        currentWallTags = currentWallTags.Remove(currentWallTags.Length - 1);
                        currentWallTags += "\n\n";
                    }
                }

                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "wall_posts", currentWallPost);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "wall_tags", currentWallTags);
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "wall_urls", GetURLs(currentWallPost, ' '));
                oGraphMLXmlDocument.AppendGraphMLAttributeValue(oVertexXmlNode, "wall_hashtags", GetHashtags(currentWallPost, ' '));
            }
        }

        //*************************************************************************
        //  Method: GetNetworkDescription()
        //
        /// <summary>
        /// Gets a description of the network.
        /// </summary>
        ///
        /// <param name="streamPosts">
        /// The downloaded posts.
        /// </param>
        /// 
        /// <param name="fanPageUsernameID">
        /// The specified fan page to analyze.
        /// </param>
        /// 
        /// <param name="netTypes">
        /// The network types to construct fo the fan page.
        /// </param>
        /// 
        /// <param name="nrOfPosts">
        /// Number of first wall posts to analyze.
        /// In case its value is equal to 0, 
        /// the fromDate is taken into consideration.
        /// </param>
        /// 
        /// <param name="fromDate">
        /// Analyze wall posts starting from this date.
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
            JSONObject streamPosts,
            String fanPageUsernameID,
            List<NetworkType> netTypes,
            int iFromPost,
            int iToPost,
            DateTime fromDate,          
            DateTime endDate,
            GraphMLXmlDocument oGraphMLXmlDocument
        )
        {
            Debug.Assert(oGraphMLXmlDocument != null);
            AssertValid();

            NetworkDescriber oNetworkDescriber = new NetworkDescriber();

            oNetworkDescriber.AddSentence(
                "The graph represents the {0} network of the \"{1}\" Facebook fan page.",
                ConcatenateNetworkTypes(netTypes),fanPageUsernameID
                );

            oNetworkDescriber.AddNetworkTime();

            if (iFromPost > 0 && iToPost >0)
            {
                oNetworkDescriber.AddSentence(
                    "Wall post from {0} to {1} of the fan page are analyzed.",
                    iFromPost, iToPost
                    );
            }
            else
            {
                oNetworkDescriber.AddSentence(
                    "Wall posts between {0} and {1} of the fan page are analyzed.",
                    fromDate, endDate
                    );
            }

            if (netTypes.Contains(NetworkType.UserUserComments))
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge between users that have co-commented on the same post."
                    );
            }

            if (netTypes.Contains(NetworkType.UserUserLikes))
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge between users that have co-liked on the same post."                    
                    );
            }

            if (netTypes.Contains(NetworkType.UserPostComments))
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge between the user that has commented "
                    +"and the post in which he has commented."                    
                    );
            }

            if (netTypes.Contains(NetworkType.UserPostLikes))
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge between the user that has liked "
                    + "and the post which he has liked."                    
                    );
            }

            if (netTypes.Contains(NetworkType.PostPostComments))
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge between posts that share the same commenters."
                    );
            }

            if (netTypes.Contains(NetworkType.PostPostLikes))
            {
                oNetworkDescriber.AddSentence(
                    "There is an edge between posts that share the same likers."
                    );
            }

            if (bGetWallPosts)
            {
                oNetworkDescriber.AddSentence(
                    "For each user, 8000 characters of his wall posts are downloaded."
                    );
            }

            if (bGetStatusUpdates)
            {
                oNetworkDescriber.AddSentence(
                    "For each user, 8000 characters of his status updates are downloaded."
                    );
            }

            if (bGetWallPosts || bGetStatusUpdates)
            {
                oNetworkDescriber.AddSentence(
                    "URLs, Hashtags and user tags are extracted from the downloaded"
                    + " wall posts/status updates."
                    );
            }

            AddPostDateRangeToNetworkDescription(streamPosts, oNetworkDescriber);

            return (oNetworkDescriber.ConcatenateSentences());
        }

        private void
        AddPostDateRangeToNetworkDescription
        (
            JSONObject streamPosts,
            NetworkDescriber oNetworkDescriber
        )
        {
            DateTime minPostDate = DateTime.MaxValue;
            DateTime maxPostsDate = DateTime.MinValue;

            if (streamPosts.IsArray)
            {
                for (int i = 0; i < streamPosts.Array.Length; i++)
                {
                    DateTime tmp = DateUtil.ConvertToDateTime(double.Parse(streamPosts.Array[i].Dictionary["created_time"].String));                    

                    if (tmp <= minPostDate)
                    {
                        minPostDate = tmp;
                    }
                    
                    if(tmp >= maxPostsDate)
                    {
                        maxPostsDate = tmp;
                    }
                }
            }

            oNetworkDescriber.AddEventTime(
                "The earliest post in the network was posted ",
                minPostDate
                );

            oNetworkDescriber.AddEventTime(
                "The latest post in the network was posted ",
                maxPostsDate
                );
        }

        private string
        ConcatenateNetworkTypes
        (
            List<NetworkType> netTypes
        )
        {
            String concat = "";
            foreach (NetworkType item in netTypes)
            {
                concat += item + ", ";
            }
            return (concat.Remove(concat.Length - 1));
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

        private Dictionary<string, Dictionary<string, JSONObject>> getAttributes
        (
        Dictionary<Attribute, bool> attributes,
        List<string> users
        )
        {
            int nrOfUsersPerQuery = 20;
            int totalNrOfqueries = 10;
            int nrOfMultiqueries = (int)(Math.Ceiling((double)users.Count / (nrOfUsersPerQuery * totalNrOfqueries)));
            int startIndex, endIndex, computedEndIndex;
            Dictionary<string, string> queries = new Dictionary<string, string>();
            JSONObject returnedAttributes;
            Dictionary<string, Dictionary<string, JSONObject>> attributeValues = new Dictionary<string, Dictionary<string, JSONObject>>();
            CurrentStep++;
            string listOfUsers = "";

            //Build the first part of the queries
            string firstPartQuery = "SELECT uid,";
            foreach (KeyValuePair<Attribute, bool> kvp in attributes)
            {
                if (kvp.Value)
                {
                    firstPartQuery += kvp.Key.value + ",";
                }
            }
            firstPartQuery = firstPartQuery.Remove(firstPartQuery.Length - 1);
            firstPartQuery += " FROM user WHERE uid IN ( ";

            for (int k = 0; k < nrOfMultiqueries; k++)
            {
                queries.Clear();
                //Build the queries and add them to a dictionary
                for (int i = 0; i < totalNrOfqueries; i++)
                {
                    listOfUsers = "";
                    startIndex = i * nrOfUsersPerQuery + k * totalNrOfqueries * nrOfUsersPerQuery;
                    computedEndIndex = (i + 1) * nrOfUsersPerQuery + k * totalNrOfqueries * nrOfUsersPerQuery;
                    endIndex = computedEndIndex >= users.Count ? users.Count : computedEndIndex;
                    for (int j = startIndex; j < endIndex; j++)
                    {
                        listOfUsers += "'"+users[j]+"',";
                    }
                    if (endIndex != computedEndIndex)
                    {
                        break;
                    }
                    listOfUsers = listOfUsers.Remove(listOfUsers.Length - 1);
                    queries.Add("query" + (i + 1).ToString(), firstPartQuery + listOfUsers+")");                    
                }

                //Execute the queries and report progress                
                returnedAttributes = ExecuteFQLMultiqueryWithRetryRelogin(queries, String.Format("Step {0}/{1}: Getting Attributes (Batch {2}/{3})",
                                            CurrentStep, NrOfSteps, (k + 1), nrOfMultiqueries),false);

                
                //Extract attributes from the JSONObject
                //and insert them to a dictionary.
                //Loop through the queries
                if (returnedAttributes.IsArray)
                {
                    for (int i = 0; i < returnedAttributes.Array.Length; i++)
                    {
                        if (returnedAttributes.Array[i].Dictionary.ContainsKey("fql_result_set"))
                        {
                            //Loop through the results of the queries
                            for (int j = 0; j < returnedAttributes.Array[i].Dictionary["fql_result_set"].Array.Length; j++)
                            {                                
                                attributeValues[returnedAttributes.Array[i].Dictionary["fql_result_set"].Array[j].Dictionary["uid"].String] =
                                    returnedAttributes.Array[i].Dictionary["fql_result_set"].Array[j].Dictionary;

                            }
                        }
                    }
                }
            }
            //List<string> diff = users.Except(attributeValues.Keys).ToList();
            return attributeValues;

        }

        private Dictionary<string, List<Dictionary<string, object>>> GetStatusUpdates
        (
        List<string> userUIDs
        )
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            Dictionary<string, List<Dictionary<string, object>>> statusUpdates = new Dictionary<string, List<Dictionary<string, object>>>();
            int nrOfFriends = userUIDs.Count;
            int nrOfQueries = 10;
            int nrOfMultiqueries = (int)(Math.Ceiling((double)nrOfFriends / nrOfQueries));
            JSONObject returnedStatusUpdates = CallGraphAPIWithRelogin("me");
            int index = 0;
            
            CurrentStep++;

            for (int i = 0; i < nrOfMultiqueries; i++)
            {
                queries.Clear();                
                
                for (int j = 0; j < nrOfQueries; j++)
                {
                    index = (i * nrOfQueries + j) < (nrOfFriends - 1) ? (i * nrOfQueries + j) : (nrOfFriends - 1);
                    queries.Add("query" + (j + 1).ToString(), "SELECT source_id, post_id, message_tags, message FROM stream WHERE filter_key = 'owner' AND source_id = '" + userUIDs[index] + "' LIMIT 10");
                }

                returnedStatusUpdates = ExecuteFQLMultiqueryWithRetryRelogin(queries, String.Format("Step {0}/{1}: Getting Status Updates (Batch {2}/{3})",
                                                CurrentStep, NrOfSteps, (i + 1), nrOfMultiqueries),true);
                                

                if (returnedStatusUpdates.IsArray)
                {
                    for (int k = 0; k < returnedStatusUpdates.Array.Length; k++)
                    {
                        if (returnedStatusUpdates.Array[k].Dictionary["fql_result_set"].Array.Length > 0)
                        {
                            statusUpdates[returnedStatusUpdates.Array[k].Dictionary["fql_result_set"].Array[0].Dictionary["source_id"].String] = new List<Dictionary<string, object>>();
                        }
                        //Loop through the results of the queries
                        for (int j = 0; j < returnedStatusUpdates.Array[k].Dictionary["fql_result_set"].Array.Length; j++)
                        {
                            statusUpdates[returnedStatusUpdates.Array[k].Dictionary["fql_result_set"].Array[j].Dictionary["source_id"].String].Add(
                                returnedStatusUpdates.Array[k].Dictionary["fql_result_set"].Array[j].Dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Key.Equals("message_tags") && kvp.Value.IsDictionary ? (object)kvp.Value.Dictionary.Select(kvp1 => kvp1.Value.Array[0].Dictionary["name"]).ToList() : (object)kvp.Value));

                        }
                    }
                }

            }

            return statusUpdates;

        }

        private Dictionary<string, List<Dictionary<string, object>>> GetWallPosts
        (
        List<string> userUIDs
        )
        {
            Dictionary<string, string> queries = new Dictionary<string, string>();
            Dictionary<string, List<Dictionary<string, object>>> wallPosts = new Dictionary<string, List<Dictionary<string, object>>>();
            int nrOfFriends = userUIDs.Count;
            int nrOfQueries = 10;
            int nrOfMultiqueries = (int)(Math.Ceiling((double)nrOfFriends / nrOfQueries));
            JSONObject returnedWallPosts = CallGraphAPIWithRelogin("me");
            int index = 0;
            
            CurrentStep++;

            for (int i = 0; i < nrOfMultiqueries; i++)
            {
                queries.Clear();               
                
                for (int j = 0; j < nrOfQueries; j++)
                {
                    index = (i * nrOfQueries + j) < (nrOfFriends - 1) ? (i * nrOfQueries + j) : (nrOfFriends - 1);
                    queries.Add("query" + (j + 1).ToString(), "SELECT source_id, post_id, message_tags, message, actor_id FROM stream WHERE filter_key = 'others' AND source_id = '" + userUIDs[index] + "' LIMIT 10");
                    queries.Add("actor_info" + (j + 1).ToString(), "SELECT uid, name FROM user WHERE uid IN (SELECT actor_id FROM #query" + (j + 1).ToString() + ")");
                }

                returnedWallPosts = ExecuteFQLMultiqueryWithRetryRelogin(queries, String.Format("Step {0}/{1}: Getting Wall Posts (Batch {2}/{3})",
                                                CurrentStep, NrOfSteps, (i + 1), nrOfMultiqueries), true);


                if (returnedWallPosts.IsArray)
                {
                    //Get only the queries not the actor info
                    for (int k = 0; k < returnedWallPosts.Array.Length / 2; k++)
                    {
                        if (returnedWallPosts.Array[k].Dictionary["fql_result_set"].Array.Length > 0)
                        {
                            wallPosts[returnedWallPosts.Array[k].Dictionary["fql_result_set"].Array[0].Dictionary["source_id"].String] = new List<Dictionary<string, object>>();
                        }
                        //Loop through the results of the queries
                        for (int j = 0; j < returnedWallPosts.Array[k].Dictionary["fql_result_set"].Array.Length; j++)
                        {
                            wallPosts[returnedWallPosts.Array[k].Dictionary["fql_result_set"].Array[j].Dictionary["source_id"].String].Add(
                                returnedWallPosts.Array[k].Dictionary["fql_result_set"].Array[j].Dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Key.Equals("message_tags") && kvp.Value.IsDictionary ? (object)kvp.Value.Dictionary.Select(kvp1 => kvp1.Value.Array[0].Dictionary["name"]).ToList() : (object)kvp.Value));
                            //Add the name of the author
                            try
                            {
                                wallPosts[returnedWallPosts.Array[k].Dictionary["fql_result_set"].Array[j].Dictionary["source_id"].String][j].Add("name", returnedWallPosts.Array[returnedWallPosts.Array.Length / 2 + k].Dictionary["fql_result_set"].Array.Where(x => x.Dictionary["uid"].String.Equals(returnedWallPosts.Array[k].Dictionary["fql_result_set"].Array[j].Dictionary["actor_id"].String)).ToArray()[0].Dictionary["name"]);
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                //Do Nothing
                            }
                        }
                    }
                }

            }

            return wallPosts;

        }        

        

        

        private void ManageDuplicateNames2(ref Dictionary<string, Dictionary<string,JSONObject>> attributes)
        {
            List<string> tmp = new List<string>();            
            foreach (KeyValuePair<string, Dictionary<string,JSONObject>> kvp in attributes)
            {
                try
                {
                    attributes[kvp.Key].Add("display_name", kvp.Value["name"]);
                }
                catch (ArgumentException e)
                {
                    //Do Nothing.
                }
                //If there are more than 1 user with a given name                
                if (attributes.Count(x => x.Value["name"].String.Equals(kvp.Value["name"].String)) > 1)
                {
                    tmp = attributes.Where(x => x.Value["name"].String.Equals(kvp.Value["name"].String)).Select(x => x.Key).ToList();
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        attributes[tmp[i]]["display_name"] = JSONObject.CreateFromString("\""+attributes[tmp[i]]["name"].String+ "_"+(i + 1).ToString()+"\"");
                    }
                }
            }
        }


        private void ManageDisplayNames(ref Dictionary<string, Dictionary<string, JSONObject>> attributes)
        {
            List<string> tmp = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, JSONObject>> kvp in attributes)
            {
                ManageDisplayNames(kvp.Value["name"].String, kvp.Key);
            }
        }

        private string ManageDisplayNames(string usersName, string usersID)
        {
            if (usersDisplayName.ContainsKey(usersID)) return usersDisplayName[usersID];
            if (String.IsNullOrEmpty(usersName))
            {
                usersName = "[No Name]";
            }

            bool succeded = false;
            int nr=1;
            while (!succeded)
            {
                if (usersDisplayName.ContainsValue(usersName))
                {
                    usersName += "_" + nr.ToString();
                    nr++;
                }
                else
                {
                    usersDisplayName.Add(usersID, usersName);
                    succeded = true;
                }
            }
            return usersName;
        }

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

            txt = " ";
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

        private void
        Relogin
        (
        )
        {
            var t = new Thread(() => ReloginMethod());
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void
        ReloginMethod
        (            
        )
        {
            FacebookLoginDialog o_fcbLoginDialog = new FacebookLoginDialog(new FacebookGraphDataProviderDialogBase(), UserAttributes.createRequiredPermissionsString(bGetStatusUpdates, bGetWallPosts,false));
            o_fcbLoginDialog.LogIn();
            fbAPI = new FacebookAPI(o_fcbLoginDialog.LocalAccessToken);
        }
        

        //*************************************************************************
        //  Method: CreateGraphMLXmlDocument()
        //
        /// <summary>
        /// Creates a GraphMLXmlDocument representing a network of friends in Facebook.
        /// </summary>
        ///        
        /// <returns>
        /// A GraphMLXmlDocument representing a network of a Facebook fan page.
        /// The document includes GraphML-attribute definitions but no vertices or
        /// edges.
        /// </returns>
        //*************************************************************************

        protected GraphMLXmlDocument
        CreateGraphMLXmlDocument
        (
            Dictionary<Attribute, bool> attributes,            
            List<NetworkType> netTypes
        )
        {
            AssertValid();

            GraphMLXmlDocument oGraphMLXmlDocument;

            if (netTypes.Count == 1 &&
                (netTypes.Contains(NetworkType.UserPostComments) ||
                netTypes.Contains(NetworkType.UserPostLikes)))
            {
                oGraphMLXmlDocument = new GraphMLXmlDocument(true);
            }
            else
            {
                oGraphMLXmlDocument = new GraphMLXmlDocument(false);
            }
            DefineRelationshipGraphMLAttribute(oGraphMLXmlDocument);
            DefineLabelGraphMLAttribute(oGraphMLXmlDocument);
            DefineCustomMenuGraphMLAttributes(oGraphMLXmlDocument);
            oGraphMLXmlDocument.DefineGraphMLAttribute(true, "e_weight", "Edge Weight", "int", "1");
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, "type", "Type", "string", null);
            oGraphMLXmlDocument.DefineGraphMLAttribute(false, TooltipID,
            "Tooltip", "string", null);

            DefineImageFileGraphMLAttribute(oGraphMLXmlDocument);

            if (netTypes.Contains(NetworkType.UserUserComments) ||
                netTypes.Contains(NetworkType.UserUserLikes) ||
                netTypes.Contains(NetworkType.UserPostComments) ||
                netTypes.Contains(NetworkType.UserPostLikes))
            {                
                oGraphMLXmlDocument.DefineGraphMLAttribute(false, "v_weight", "Vertex Weight", "int", "1");
                foreach (KeyValuePair<Attribute, bool> kvp in attributes)
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

                if (bGetStatusUpdates)
                {
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "statuses", "Status Updates", "string", null);
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "statuses_tags", "Tagged Users(Status Updates)", "string", null);
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "statuses_urls", "URLs(Status Updates)", "string", null);
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "statuses_hashtags", "Hashtags(Status Updates)", "string", null);
                }

                if (bGetWallPosts)
                {
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "wall_posts", "Wall Posts", "string", null);
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "wall_tags", "Tagged Users(Wall Posts)", "string", null);
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "wall_urls", "URLs(Wall Posts)", "string", null);
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "wall_hashtags", "Hashtags(Wall Posts)", "string", null);
                }
                if (netTypes.Contains(NetworkType.UserUserComments) ||
                    netTypes.Contains(NetworkType.UserUserLikes))
                {
                    oGraphMLXmlDocument.DefineGraphMLAttribute(true, "post", "Posts", "string", null);
                }
                if (netTypes.Contains(NetworkType.UserUserComments) ||
                    netTypes.Contains(NetworkType.UserPostComments))
                {
                    oGraphMLXmlDocument.DefineGraphMLAttribute(false, "comment", "Tweet", "string", null);
                    if (netTypes.Contains(NetworkType.UserPostComments))
                    {
                        oGraphMLXmlDocument.DefineGraphMLAttribute(true, "edge_comment", "Tweet", "string", null);
                    }
                }                
            }

            if (netTypes.Contains(NetworkType.PostPostComments) ||
                netTypes.Contains(NetworkType.PostPostLikes)||
                netTypes.Contains(NetworkType.UserPostComments) ||
                netTypes.Contains(NetworkType.UserPostLikes))
            {
                oGraphMLXmlDocument.DefineGraphMLAttribute(false, "content", "Post Content", "string", null);
                //DefineImageFileGraphMLAttribute(oGraphMLXmlDocument);
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
                e.Result = GetFanPageNetworkInternal(oGetNetworkAsyncArgs.AccessToken, 
                                                     oGetNetworkAsyncArgs.fanPageUsernameID,
                                                     oGetNetworkAsyncArgs.netTypes,
                                                     oGetNetworkAsyncArgs.FromPost,
                                                     oGetNetworkAsyncArgs.ToPost,
                                                     oGetNetworkAsyncArgs.attributes,
                                                     oGetNetworkAsyncArgs.getStatusUpdates,
                                                     oGetNetworkAsyncArgs.getWallPosts,
                                                     oGetNetworkAsyncArgs.includeOthers,
                                                     oGetNetworkAsyncArgs.startDate,
                                                     oGetNetworkAsyncArgs.endDate,
                                                     oGetNetworkAsyncArgs.limit);
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
        
        ///Private fields
        
        //Thess dictionary will hold all the users which have no attributes
        //Most of them are fan pages which have commented/liked a post in 
        //another fan page
        private Dictionary<string, string> commentersWithNoAttributes;
        private Dictionary<string, string> likersWithNoAttributes;

        private Dictionary<string,string> usersDisplayName;

        private bool bGetStatusUpdates;
        private bool bGetWallPosts;
        private FacebookAPI fbAPI;


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
            public List<NetworkType> netTypes;
            ///
            public int FromPost;
            ///
            public int ToPost;
            ///
            public String fanPageUsernameID;
            ///
            public Dictionary<Attribute, bool> attributes;
            ///
            public bool getStatusUpdates;            
            ///             
            public bool getWallPosts;
            ///
            public bool includeOthers;
            ///
            public DateTime startDate;
            ///
            public DateTime endDate;
            ///
            public int limit;
        };
        
        
    }

}