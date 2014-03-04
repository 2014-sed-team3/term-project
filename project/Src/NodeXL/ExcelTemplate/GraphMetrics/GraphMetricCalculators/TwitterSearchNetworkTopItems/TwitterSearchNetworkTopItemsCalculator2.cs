
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;
using Smrf.SocialNetworkLib.Twitter;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TwitterSearchNetworkTopItemsCalculator2
//
/// <summary>
/// Calculates the top items within a Twitter search network.
/// </summary>
///
/// <remarks>
/// This class calculates various top-item metrics for a Twitter search
/// network.  If the workbook doesn't contain a Twitter search network, this
/// class does nothing.
///
/// <para>
/// This graph metric calculator differs from most other calculators in that it
/// reads tweet-related columns that were written to the Excel workbook by the
/// TwitterSearchNetworkImporter class.  The other calculators look only at how
/// the graph's vertices are connected to each other.  Therefore, there is no
/// corresponding lower-level TwitterSearchNetworkTopItemsCalculator class in
/// the <see cref="Smrf.NodeXL.Algorithms" /> namespace, and the top items in a
/// Twitter search network cannot be calculated outside of this ExcelTemplate
/// project.
/// </para>
///
/// <para>
/// Here are the metrics calculated by this class.
/// </para>
///
/// <para>
/// For the Twitter Search network top items worksheet:
/// </para>
///
/// <para>
/// Top URLs, domains, hashtags, words, word pairs, replies-to, and mentions in
/// tweets; and top tweeters.  These get calculated for the entire graph, as
/// well as for the graph's top groups, ranked by vertex count.
/// </para>
///
/// <para>
/// For the group worksheet:
/// </para>
///
/// <para>
/// Top URLs, domains, hashtags, words, word pairs, replies-to, and mentions in
/// tweets; and top tweeters.  These get calculated for each group.
/// </para>
///
/// <para>
/// For the vertex worksheet:
/// </para>
///
/// <para>
/// Top URLs, domains, hashtags, words, and word pairs in tweets, by both count
/// and salience.  These get calculated for each vertex.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TwitterSearchNetworkTopItemsCalculator2 : TopItemsCalculatorBase2
{
    //*************************************************************************
    //  Constructor: TwitterSearchNetworkTopItemsCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" /> class.
    /// </summary>
    ///
    /// <param name="graphHistory">
    /// Describes how the graph was created.
    /// </param>
    //*************************************************************************

    public TwitterSearchNetworkTopItemsCalculator2
    (
        GraphHistory graphHistory
    )
    {
        m_oGraphHistory = graphHistory;
        m_oTwitterStatusTextParser = new TwitterStatusTextParser();

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricToCalculate
    //
    /// <summary>
    /// Gets the graph metric that will be calculated.
    /// </summary>
    ///
    /// <value>
    /// The graph metric that will be calculated, as a <see
    /// cref="GraphMetrics" /> flag.
    /// </value>
    //*************************************************************************

    protected override GraphMetrics
    GraphMetricToCalculate
    {
        get
        {
            AssertValid();

            return (GraphMetrics.TwitterSearchNetworkTopItems);
        }
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetricsInternal()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that gets populated by this
    /// method if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected override Boolean
    TryCalculateGraphMetricsInternal
    (
        IGraph oGraph,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        return (
            TryCalculateTopItemAndGroupMetrics(
                oGraph, oCalculateGraphMetricsContext, oGraphMetricColumns)
            &&
            TryCalculateVertexMetrics(
                oGraph, oCalculateGraphMetricsContext, oGraphMetricColumns)
            );
    }

    //*************************************************************************
    //  Method: TryCalculateTopItemAndGroupMetrics()
    //
    /// <summary>
    /// Attempts to calculate metrics for the Twitter search network top items
    /// and group worksheets.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that gets populated by this
    /// method if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateTopItemAndGroupMetrics
    (
        IGraph oGraph,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        if ( !ReportProgressAndCheckCancellationPending(
            oCalculateGraphMetricsContext) )
        {
            return (false);
        }

        // Get information about each of the graph's groups, including a
        // "dummy" group for the entire graph.

        List<GroupEdgeInfo> oAllGroupEdgeInfos =
            GroupEdgeSorter.SortGroupEdges(oGraph, Int32.MaxValue, true,
                false);

        Int32 iTableIndex = 1;

        AddColumnsForStatusContent(ref iTableIndex, oAllGroupEdgeInfos,
            UrlsInTweetColumnName, true,
            GroupTableColumnNames.TopUrlsInTweet, oGraphMetricColumns);

        AddColumnsForStatusContent(ref iTableIndex, oAllGroupEdgeInfos,
            DomainsInTweetColumnName, false,
            GroupTableColumnNames.TopDomainsInTweet, oGraphMetricColumns);

        AddColumnsForStatusContent(ref iTableIndex, oAllGroupEdgeInfos,
            HashtagsInTweetColumnName, false,
            GroupTableColumnNames.TopHashtagsInTweet, oGraphMetricColumns);

        if ( !TryAddColumnsForWordsAndWordPairs(ref iTableIndex, oGraph,
            oAllGroupEdgeInfos, oCalculateGraphMetricsContext,
            oGraphMetricColumns) )
        {
            return (false);
        }

        AddColumnsForRepliesToAndMentions(ref iTableIndex, oAllGroupEdgeInfos,
            oGraphMetricColumns);

        AddColumnsForTweeters(ref iTableIndex, oGraph, oGraphMetricColumns);

        AdjustColumnWidths(oGraphMetricColumns, oAllGroupEdgeInfos);

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateVertexMetrics()
    //
    /// <summary>
    /// Attempts to calculate metrics for the vertex worksheet.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that gets populated by this
    /// method if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateVertexMetrics
    (
        IGraph oGraph,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        if ( !ReportProgressAndCheckCancellationPending(
            oCalculateGraphMetricsContext) )
        {
            return (false);
        }

        // The key is a screen name and the value is a list of zero or more
        // unique edges belonging to that user.

        Dictionary< String, List<IEdge> > oUniqueEdgesByUser =
            TwitterSearchNetworkVertexMetricUtil.GetUniqueEdgesByUser(oGraph);

        List<GraphMetricValueWithID>
            oTopUrlsInTweetByCountGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopUrlsInTweetBySalienceGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopDomainsInTweetByCountGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopDomainsInTweetBySalienceGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopHashtagsInTweetByCountGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopHashtagsInTweetBySalienceGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopWordsInTweetByCountGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopWordsInTweetBySalienceGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopWordPairsInTweetByCountGraphMetricValues =
                new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID>
            oTopWordPairsInTweetBySalienceGraphMetricValues =
                new List<GraphMetricValueWithID>();

        // For efficiency, these counters are created once only.

        WordCounter oWordCounter;
        WordPairCounter oWordPairCounter;

        TwitterSearchNetworkVertexMetricUtil
            .CreateCountersForWordsAndWordPairs(
                GetSearchTerm(), oCalculateGraphMetricsContext,
                out oWordCounter, out oWordPairCounter);

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            Int32 iRowID;
            List<IEdge> oUniqueEdgesForUser;

            if (
                TryGetRowID(oVertex, out iRowID)
                &&
                !String.IsNullOrEmpty(oVertex.Name)
                &&
                oUniqueEdgesByUser.TryGetValue(
                    oVertex.Name, out oUniqueEdgesForUser)
                )
            {
                TwitterSearchNetworkVertexMetricUtil
                    .AddGraphMetricValueForTopStrings(
                        oUniqueEdgesForUser, UrlsInTweetColumnName,
                        MaximumTopItems, iRowID,
                        oTopUrlsInTweetByCountGraphMetricValues,
                        oTopUrlsInTweetBySalienceGraphMetricValues
                        );

                TwitterSearchNetworkVertexMetricUtil
                    .AddGraphMetricValueForTopStrings(
                        oUniqueEdgesForUser, DomainsInTweetColumnName,
                        MaximumTopItems, iRowID,
                        oTopDomainsInTweetByCountGraphMetricValues,
                        oTopDomainsInTweetBySalienceGraphMetricValues
                        );

                TwitterSearchNetworkVertexMetricUtil
                    .AddGraphMetricValueForTopStrings(
                        oUniqueEdgesForUser, HashtagsInTweetColumnName,
                        MaximumTopItems, iRowID,
                        oTopHashtagsInTweetByCountGraphMetricValues,
                        oTopHashtagsInTweetBySalienceGraphMetricValues
                        );

                TwitterSearchNetworkVertexMetricUtil
                    .AddGraphMetricValuesForTopWordsAndWordPairs(
                        oUniqueEdgesForUser, StatusColumnName,
                        MaximumTopItems, oWordCounter, oWordPairCounter,
                        iRowID,
                        oTopWordsInTweetByCountGraphMetricValues,
                        oTopWordsInTweetBySalienceGraphMetricValues,
                        oTopWordPairsInTweetByCountGraphMetricValues,
                        oTopWordPairsInTweetBySalienceGraphMetricValues
                        );
            }
        }

        TwitterSearchNetworkVertexMetricUtil.AddGraphMetricColumns(

            oGraphMetricColumns,

            oTopUrlsInTweetByCountGraphMetricValues,
            VertexTableColumnNames.TopUrlsInTweetByCount,

            oTopUrlsInTweetBySalienceGraphMetricValues,
            VertexTableColumnNames.TopUrlsInTweetBySalience,

            oTopDomainsInTweetByCountGraphMetricValues,
            VertexTableColumnNames.TopDomainsInTweetByCount,

            oTopDomainsInTweetBySalienceGraphMetricValues,
            VertexTableColumnNames.TopDomainsInTweetBySalience,

            oTopHashtagsInTweetByCountGraphMetricValues,
            VertexTableColumnNames.TopHashtagsInTweetByCount,

            oTopHashtagsInTweetBySalienceGraphMetricValues,
            VertexTableColumnNames.TopHashtagsInTweetBySalience,

            oTopWordsInTweetByCountGraphMetricValues,
            VertexTableColumnNames.TopWordsInTweetByCount,

            oTopWordsInTweetBySalienceGraphMetricValues,
            VertexTableColumnNames.TopWordsInTweetBySalience,

            oTopWordPairsInTweetByCountGraphMetricValues,
            VertexTableColumnNames.TopWordPairsInTweetByCount,

            oTopWordPairsInTweetBySalienceGraphMetricValues,
            VertexTableColumnNames.TopWordPairsInTweetBySalience
            );

        return (true);
    }

    //*************************************************************************
    //  Method: AddColumnsForStatusContent()
    //
    /// <summary>
    /// Adds "status content" columns.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the table on the Twitter search network top
    /// items worksheet.  This gets incremented by this method.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="sEdgeColumnName">
    /// Name of the edge column to calculate the metric from.  The column must
    /// contain space-delimited strings.  Sample: "URLs in Tweet", in which
    /// case the column contains space-delimited URLs that this method counts
    /// and ranks.
    /// </param>
    ///
    /// <param name="bEdgeColumnContainsUrls">
    /// true if the content might contains URLs.
    /// </param>
    ///
    /// <param name="sGroupTableColumnName">
    /// Name of the column to add for the group table on the group worksheet.
    /// Sample: "Top URLs in Tweet".
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method adds a set of columns to the <paramref
    /// name="oGraphMetricColumns" /> collection.  The added set corresponds to
    /// one table that needs to be written to the Twitter search network top
    /// items worksheet; for example, the table that shows top URLs in tweets.
    /// The added set contains one pair of columns for each group in <paramref
    /// name="oAllGroupEdgeInfos" />.  A sample pair of columns is "Top URLs in
    /// G7" and "G7 Count", where G7 is the name of a group.
    ///
    /// <para>
    /// It also adds a column for the group worksheet.  Each cell in the column
    /// is a space-delimited list of the top strings in the group's tweets; the
    /// top URLs in the group's tweets, for example.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AddColumnsForStatusContent
    (
        ref Int32 iTableIndex,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        String sEdgeColumnName,
        Boolean bEdgeColumnContainsUrls,
        String sGroupTableColumnName,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert( !String.IsNullOrEmpty(sEdgeColumnName) );
        Debug.Assert(oGraphMetricColumns != null);
        Debug.Assert( !String.IsNullOrEmpty(sGroupTableColumnName) );
        AssertValid();

        List<GraphMetricValueWithID> oTopStringsForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        Int32 iGroups = oAllGroupEdgeInfos.Count;
        String sTableName = GetTableName(ref iTableIndex);

        for (Int32 iGroup = 0; iGroup < iGroups; iGroup++)
        {
            GroupEdgeInfo oGroupEdgeInfo = oAllGroupEdgeInfos[iGroup];

            // The key is a string being counted (an URL in the tweets, for
            // example), and the value is the number of times the string was
            // found in the edges.

            Dictionary<String, Int32> oStringCounts =
                TwitterSearchNetworkStringUtil
                .CountDelimitedStringsInEdgeColumn(oGroupEdgeInfo.Edges,
                    sEdgeColumnName);

            // (The extra "1" is for the dummy group that represents the entire
            // graph.)

            if (iGroup < MaximumTopGroups + 1)
            {
                // Populate two GraphMetricValueOrdered collections with the
                // top strings and their counts.

                List<GraphMetricValueOrdered> oTopStrings, oTopStringCounts;

                TwitterSearchNetworkStringUtil.GetTopGraphMetricValues(
                    oStringCounts, MaximumTopItems, out oTopStrings,
                    out oTopStringCounts);

                String sGraphMetricColumn1Name, sGraphMetricColumn2Name;

                GetGraphMetricColumnNames(sEdgeColumnName,
                    GetGroupName(oGroupEdgeInfo),
                    out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

                // Add a pair of columns for the Twitter search network top
                // items worksheet.

                AddGraphMetricColumnPair(sTableName, sGraphMetricColumn1Name,
                    sGraphMetricColumn2Name, oTopStrings, oTopStringCounts,
                    bEdgeColumnContainsUrls, oGraphMetricColumns);
            }

            if (oGroupEdgeInfo.GroupInfo != null)
            {
                // This is a real group, not the dummy group.  Add a cell for
                // the column on the group worksheet.

                oTopStringsForGroupWorksheet.Add( new GraphMetricValueWithID(
                    GetGroupRowID(oGroupEdgeInfo),

                    TwitterSearchNetworkStringUtil.ConcatenateTopStrings(
                        oStringCounts, MaximumTopItems) ) );
            }
        }

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups,
            sGroupTableColumnName, ExcelTableUtil.AutoColumnWidth, null, null,
            oTopStringsForGroupWorksheet.ToArray() ) );
    }

    //*************************************************************************
    //  Method: AddColumnsForRepliesToAndMentions()
    //
    /// <summary>
    /// Add the columns for the top replies-to and top mentions.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The start index to use for naming the top replies-to and top mentions
    /// tables.  This gets incremented by this method.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method adds two sets of columns to the <paramref
    /// name="oGraphMetricColumns" /> collection.  The first set corresponds to
    /// a top replies-to table that needs to be written to the workbook, and
    /// the second set corresponds to a top mentions table.
    ///
    /// <para>
    /// It also adds columns for the group worksheet.  Each cell in a column is
    /// a space-delimited list of the top replies-to or mentions in the group's
    /// tweets.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AddColumnsForRepliesToAndMentions
    (
        ref Int32 iTableIndex,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        List<GraphMetricValueWithID> oTopRepliesToForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        List<GraphMetricValueWithID> oTopMentionsForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        String sRepliesToTableName = GetTableName(ref iTableIndex);
        String sMentionsTableName = GetTableName(ref iTableIndex);
        Int32 iGroups = oAllGroupEdgeInfos.Count;

        for (Int32 iGroup = 0; iGroup < iGroups; iGroup++)
        {
            GroupEdgeInfo oGroupEdgeInfo = oAllGroupEdgeInfos[iGroup];

            // The key is a screen name and the value is the number of times
            // the screen name was found as a "reply-to" in all the edges.

            Dictionary<String, Int32> oRepliesToCounts =
                new Dictionary<String, Int32>();

            // The key is a screen name and the value is the number of times
            // the screen name was found as a "mentions" in all the edges.

            Dictionary<String, Int32> oMentionsCounts =
                new Dictionary<String, Int32>();

            foreach (IEdge oEdge in oGroupEdgeInfo.Edges)
            {
                String sStatus;

                if ( oEdge.TryGetNonEmptyStringValue(
                    StatusColumnName, out sStatus) )
                {
                    String sReplyToScreenName;
                    String [] asMentionedScreenNames;

                    m_oTwitterStatusTextParser.GetScreenNames(sStatus,
                        out sReplyToScreenName, out asMentionedScreenNames);

                    if (sReplyToScreenName != null)
                    {
                        TwitterSearchNetworkStringUtil.CountString(
                            sReplyToScreenName, oRepliesToCounts);
                    }

                    foreach (String sMentionedScreenName in
                        asMentionedScreenNames)
                    {
                        TwitterSearchNetworkStringUtil.CountString(
                            sMentionedScreenName, oMentionsCounts);
                    }
                }
            }

            // (The extra "1" is for the dummy group that represents the entire
            // graph.)

            if (iGroup < MaximumTopGroups + 1)
            {
                String sGroupName = GetGroupName(oGroupEdgeInfo);

                // Populate GraphMetricValueOrdered collections with the top
                // replies-to and their counts.

                List<GraphMetricValueOrdered> oTopStrings, oTopStringCounts;
                String sGraphMetricColumn1Name, sGraphMetricColumn2Name;

                TwitterSearchNetworkStringUtil.GetTopGraphMetricValues(
                    oRepliesToCounts, MaximumTopItems, out oTopStrings,
                    out oTopStringCounts);

                GetGraphMetricColumnNames("Replied-To", sGroupName,
                    out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

                AddGraphMetricColumnPair(sRepliesToTableName,
                    sGraphMetricColumn1Name, sGraphMetricColumn2Name,
                    oTopStrings, oTopStringCounts, false, oGraphMetricColumns);

                // Repeat for mentions.

                TwitterSearchNetworkStringUtil.GetTopGraphMetricValues(
                    oMentionsCounts, MaximumTopItems, out oTopStrings,
                    out oTopStringCounts);

                GetGraphMetricColumnNames("Mentioned", sGroupName,
                    out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

                AddGraphMetricColumnPair(sMentionsTableName,
                    sGraphMetricColumn1Name, sGraphMetricColumn2Name,
                    oTopStrings, oTopStringCounts, false, oGraphMetricColumns);
            }

            if (oGroupEdgeInfo.GroupInfo != null)
            {
                // This is a real group, not the dummy group.  Add cells for
                // the columns on the group worksheet.

                oTopRepliesToForGroupWorksheet.Add( new GraphMetricValueWithID(
                    GetGroupRowID(oGroupEdgeInfo),

                    TwitterSearchNetworkStringUtil.ConcatenateTopStrings(
                        oRepliesToCounts, MaximumTopItems) ) );

                oTopMentionsForGroupWorksheet.Add( new GraphMetricValueWithID(
                    GetGroupRowID(oGroupEdgeInfo),

                    TwitterSearchNetworkStringUtil.ConcatenateTopStrings(
                        oMentionsCounts, MaximumTopItems) ) );
            }
        }

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.TopRepliesToInTweet,
            ExcelTableUtil.AutoColumnWidth, null, null,
            oTopRepliesToForGroupWorksheet.ToArray() ) );

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.TopMentionsInTweet,
            ExcelTableUtil.AutoColumnWidth, null, null,
            oTopMentionsForGroupWorksheet.ToArray() ) );
    }

    //*************************************************************************
    //  Method: TryAddColumnsForWordsAndWordPairs()
    //
    /// <summary>
    /// Adds the columns for the top words and top word pair tables on the
    /// Twitter search network top items worksheet.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the tables.  This gets incremented by this
    /// method.
    /// </param>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method adds two sets of columns to the <paramref
    /// name="oGraphMetricColumns" /> collection.  The sets correspond to top
    /// words and top word pairs tables that need to be written to the
    /// workbook.
    ///
    /// <para>
    /// It also adds columns for the group worksheet.  Each cell in a column is
    /// a space-delimited list of the top words or word pairs in the group's
    /// tweets.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryAddColumnsForWordsAndWordPairs
    (
        ref Int32 iTableIndex,
        IGraph oGraph,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        CalculateGraphMetricsContext oCalculateGraphMetricsContext,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oGraph != null);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert(oCalculateGraphMetricsContext != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        // Calculate all word metrics for all the graph's groups.

        GraphMetricColumn [] oWordMetricColumns;

        if ( !TwitterSearchNetworkWordMetricUtil.TryCalculateWordMetrics(
            oGraph, oCalculateGraphMetricsContext, StatusColumnName,
            out oWordMetricColumns) )
        {
            return (false);
        }

        // Not all of the word metrics are needed, and they are in the wrong
        // format.  Filter and reformat them.

        FilterAndReformatWordMetrics(ref iTableIndex, oWordMetricColumns,
            oAllGroupEdgeInfos, oGraphMetricColumns);

        return (true);
    }

    //*************************************************************************
    //  Method: AddColumnsForTweeters()
    //
    /// <summary>
    /// Adds the columns for the top tweeters.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the Top Tweeters table.  This gets
    /// incremented by this method.
    /// </param>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method adds one set of columns to the <paramref
    /// name="oGraphMetricColumns" /> collection.  The set corresponds to a Top
    /// Tweeters table that needs to be written to the workbook.
    ///
    /// <para>
    /// It also adds a column for the group worksheet.  Each cell in the column
    /// is a space-delimited list of the top tweeters in the group.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AddColumnsForTweeters
    (
        ref Int32 iTableIndex,
        IGraph oGraph,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oGraph != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        String sTweetersTableName = GetTableName(ref iTableIndex);
        const String TableDescription = "Tweeters";

        // Add a pair of columns for the entire graph.

        String sGraphMetricColumn1Name, sGraphMetricColumn2Name;

        GetGraphMetricColumnNames(TableDescription, null,
            out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

        AddColumnsForRankedVertices(oGraph.Vertices, TweetsColumnName,
            MaximumTopItems, WorksheetNames.TwitterSearchNetworkTopItems,
            sTweetersTableName, sGraphMetricColumn1Name,
            sGraphMetricColumn2Name, oGraphMetricColumns);

        GroupInfo [] aoGroups;

        List<GraphMetricValueWithID> oTopTweetersForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        if ( GroupUtil.TryGetGroups(oGraph, out aoGroups) )
        {
            // Add a pair of columns for each of the graph's top groups.

            Int32 iGroup = 0;

            foreach ( ExcelTemplateGroupInfo oGroupInfo in
                ExcelTemplateGroupUtil.GetTopGroups(aoGroups, Int32.MaxValue) )
            {
                if (iGroup < MaximumTopGroups)
                {
                    GetGraphMetricColumnNames(TableDescription, oGroupInfo.Name,
                        out sGraphMetricColumn1Name,
                        out sGraphMetricColumn2Name);

                    AddColumnsForRankedVertices(oGroupInfo.Vertices,
                        TweetsColumnName, MaximumTopItems,
                        WorksheetNames.TwitterSearchNetworkTopItems,
                        sTweetersTableName, sGraphMetricColumn1Name,
                        sGraphMetricColumn2Name, oGraphMetricColumns);
                }

                // Add a cell to the column for the group worksheet.

                IList<ItemInformation> oItems = RankVertices(
                    oGroupInfo.Vertices, TweetsColumnName);

                Debug.Assert(oGroupInfo.RowID.HasValue);

                oTopTweetersForGroupWorksheet.Add( new GraphMetricValueWithID(
                    oGroupInfo.RowID.Value, 

                    String.Join( " ",

                        TwitterSearchNetworkStringUtil.TakeTopStringsAsArray(

                        (from oItem in oItems
                        select oItem.Name),
                        
                        MaximumTopItems
                        ) )
                    ) );

                iGroup++;
            }
        }

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups,
            GroupTableColumnNames.TopTweeters,
            ExcelTableUtil.AutoColumnWidth, null, null,
            oTopTweetersForGroupWorksheet.ToArray() ) );
    }

    //*************************************************************************
    //  Method: AdjustColumnWidths()
    //
    /// <summary>
    /// Adjusts the width of the columns on the Twitter search network top
    /// items worksheet.
    /// </summary>
    ///
    /// <param name="oGraphMetricColumns">
    /// Complete collection of GraphMetricColumn objects.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <remarks>
    /// This method forces each column in the Twitter search network top items
    /// worksheet to be the width of the longest column name in that column.
    ///
    /// <para>
    /// <see cref="GraphMetricWriter" /> "stacks" the tables created by this
    /// class on top of each other in the Twitter search network top items
    /// worksheet, so that the "Top URLs in Tweet in Entire Graph", "Top
    /// Domains in Tweet in Entire Graph", and "Top Hashtags in Tweet in Entire
    /// Graph" graph metric columns all end up in Excel column A, for example.
    /// The graph metric column widths specified by this class are all
    /// ExcelTableUtil.AutoColumnWidth, which would result in the Excel column
    /// having the width of the widest cell in the column.  Because the cells
    /// can contain long text, like URLs, the Excel column would often end up
    /// being too wide.  This method fixes that problem by changing all the
    /// graph metric column widths from AutoColumnWidth to the longest column
    /// name in the column.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected void
    AdjustColumnWidths
    (
        List<GraphMetricColumn> oGraphMetricColumns,
        List<GroupEdgeInfo> oAllGroupEdgeInfos
    )
    {
        Debug.Assert(oGraphMetricColumns != null);
        Debug.Assert(oAllGroupEdgeInfos != null);
        AssertValid();

        // Sample top group names: "Entire Graph", "G1", "G2", ..., "G10".

        var oTopGroupNames =
            (from oGroupEdgeInfo in oAllGroupEdgeInfos
                select GetGroupName(oGroupEdgeInfo) ?? EntireGraphGroupName
            ).Take(MaximumTopGroups + 1);

        // The columns in oGraphMetricColumns are not assumed to be in any
        // order.
        //
        // The first pass through the following loop selects only those columns
        // that involve the entire graph. For example:
        //
        //  "Top URLs in Tweet in Entire Graph"
        //  "Top Domains in Tweet in Entire Graph"
        //  "Top Hashtags in Tweet in Entire Graph"
        //  ...
        //
        // The second pass selects only those columns that involve the first
        // real group.  For example:
        //
        //  "Top URLs in Tweet in G1"
        //  "Top Domains in Tweet in G1"
        //  "Top Hashtags in Tweet in G1"
        //  ...
        //
        // ...and so on through all the top groups.

        foreach (String sGroupName in oTopGroupNames)
        {
            var oColumnsForGroup =

                from oGraphMetricColumn in oGraphMetricColumns

                where
                (
                    oGraphMetricColumn.TableName.StartsWith(
                        TableNames.TwitterSearchNetworkTopItemsRoot)
                    &&
                    oGraphMetricColumn.ColumnName.StartsWith("Top ")
                    &&
                    oGraphMetricColumn.ColumnName.EndsWith("in " + sGroupName)
                )
                select oGraphMetricColumn
                ;

            if (oColumnsForGroup.Count() > 0)
            {
                Double dMaximumColumnWidthChars = oColumnsForGroup.Max(
                    oGraphMetricColumn => oGraphMetricColumn.ColumnName.Length);

                foreach (GraphMetricColumn oGraphMetricColumn in
                    oColumnsForGroup)
                {
                    oGraphMetricColumn.ColumnWidthChars =
                        dMaximumColumnWidthChars;
                }
            }
        }
    }

    //*************************************************************************
    //  Method: FilterAndReformatWordMetrics()
    //
    /// <summary>
    /// Filters and reformats calculated word metrics.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the tables.  This gets incremented by this
    /// method.
    /// </param>
    ///
    /// <param name="oWordMetricColumns">
    /// Word metric columns calculated by WordMetricCalculator2.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method takes the word metric columns calculated by
    /// WordMetricCalculator2, filters out what it doesn't need, and reformats
    /// the rest into new graph metric columns.
    /// </remarks>
    //*************************************************************************

    protected void
    FilterAndReformatWordMetrics
    (
        ref Int32 iTableIndex,
        GraphMetricColumn [] oWordMetricColumns,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert(oWordMetricColumns != null);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        // This method assumes a certain column order created by
        // WordMetricCalculator2.  This can be changed in the future to search
        // for columns, but that seems wasteful for now.

        Debug.Assert(oWordMetricColumns.Length == 10);
        Debug.Assert(oWordMetricColumns[0] is GraphMetricColumnOrdered);

        Debug.Assert(oWordMetricColumns[0].ColumnName ==
            WordTableColumnNames.Word);

        Debug.Assert(oWordMetricColumns[1].ColumnName ==
            WordTableColumnNames.Count);

        Debug.Assert(oWordMetricColumns[3].ColumnName ==
            WordTableColumnNames.Group);

        Debug.Assert(oWordMetricColumns[4].ColumnName ==
            WordPairTableColumnNames.Word1);

        Debug.Assert(oWordMetricColumns[5].ColumnName ==
            WordPairTableColumnNames.Word2);

        Debug.Assert(oWordMetricColumns[6].ColumnName ==
            WordPairTableColumnNames.Count);

        Debug.Assert(oWordMetricColumns[9].ColumnName ==
            WordPairTableColumnNames.Group);

        // Filter and reformat calculated metrics for words.

        FilterAndReformatWordMetrics(ref iTableIndex, "Words in Tweet",
            (GraphMetricColumnOrdered)oWordMetricColumns[0],
            null,
            (GraphMetricColumnOrdered)oWordMetricColumns[1],
            (GraphMetricColumnOrdered)oWordMetricColumns[3],
            oAllGroupEdgeInfos, GroupTableColumnNames.TopWordsInTweet,
            oGraphMetricColumns
            );

        // Filter and reformat calculated metrics for word pairs.

        FilterAndReformatWordMetrics(ref iTableIndex, "Word Pairs in Tweet",
            (GraphMetricColumnOrdered)oWordMetricColumns[4],
            (GraphMetricColumnOrdered)oWordMetricColumns[5],
            (GraphMetricColumnOrdered)oWordMetricColumns[6],
            (GraphMetricColumnOrdered)oWordMetricColumns[9],
            oAllGroupEdgeInfos, GroupTableColumnNames.TopWordPairsInTweet,
            oGraphMetricColumns
            );
    }

    //*************************************************************************
    //  Method: FilterAndReformatWordMetrics()
    //
    /// <summary>
    /// Filters and reformats calculated metrics for words or word pairs.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index to use for naming the tables.  This gets incremented by this
    /// method.
    /// </param>
    ///
    /// <param name="sTableDescription">
    /// Description of the table the graph metric columns will eventually be
    /// written to.  Sample: "Words in Tweet".
    /// </param>
    ///
    /// <param name="oWord1Column">
    /// Column that was calculated for either the word (for words) or word 1
    /// (for word pairs).
    /// </param>
    ///
    /// <param name="oWord2Column">
    /// Column that was calculated for word 2, or null if this is being called
    /// for words.
    /// </param>
    ///
    /// <param name="oCountColumn">
    /// Column that was calculated for the word or word pair count.
    /// </param>
    ///
    /// <param name="oGroupNameColumn">
    /// Column that was calculated for the group name.
    /// </param>
    ///
    /// <param name="oAllGroupEdgeInfos">
    /// A List of GroupEdgeInfo objects, one for each of the graph's groups,
    /// where the groups are sorted by descending vertex count.  This includes
    /// a "dummy" group for the entire graph.
    /// </param>
    ///
    /// <param name="sGroupTableColumnName">
    /// Name of the column to add for the group table on the group worksheet.
    /// Sample: "Top Words in Tweet".
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    ///
    /// <remarks>
    /// This method takes word metric columns calculated by
    /// WordMetricCalculator2 for either words or word pairs, filters out what
    /// it doesn't need, and reformats the rest into new graph metric columns.
    /// </remarks>
    //*************************************************************************

    protected void
    FilterAndReformatWordMetrics
    (
        ref Int32 iTableIndex,
        String sTableDescription,
        GraphMetricColumnOrdered oWord1Column,
        GraphMetricColumnOrdered oWord2Column,
        GraphMetricColumnOrdered oCountColumn,
        GraphMetricColumnOrdered oGroupNameColumn,
        List<GroupEdgeInfo> oAllGroupEdgeInfos,
        String sGroupTableColumnName,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(iTableIndex >= 1);
        Debug.Assert( !String.IsNullOrEmpty(sTableDescription) );
        Debug.Assert(oWord1Column != null);
        // oWord2Column
        Debug.Assert(oCountColumn != null);
        Debug.Assert(oGroupNameColumn != null);
        Debug.Assert(oAllGroupEdgeInfos != null);
        Debug.Assert( !String.IsNullOrEmpty(sGroupTableColumnName) );
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        Boolean bIsForWords = (oWord2Column == null);

        GraphMetricValueOrdered [] aoWord1Values =
            oWord1Column.GraphMetricValuesOrdered;

        GraphMetricValueOrdered [] aoWord2Values = null;

        if (!bIsForWords)
        {
            aoWord2Values = oWord2Column.GraphMetricValuesOrdered;
        }

        GraphMetricValueOrdered [] aoCountValues =
            oCountColumn.GraphMetricValuesOrdered;

        GraphMetricValueOrdered [] aoGroupNameValues =
            oGroupNameColumn.GraphMetricValuesOrdered;

        List<GraphMetricValueWithID> oTopWordsOrWordPairsForGroupWorksheet =
            new List<GraphMetricValueWithID>();

        // The key is a group name from the word or word pair table created by
        // WordMetricCalculator2 and the value is the index of the first row
        // for that group.

        Dictionary<String, Int32> oGroupNameIndexes =
            TwitterSearchNetworkWordMetricUtil
            .GetGroupNameIndexesFromWordMetrics(aoGroupNameValues);

        Int32 iGroups = oAllGroupEdgeInfos.Count;
        String sTableName = GetTableName(ref iTableIndex);

        for (Int32 iGroup = 0; iGroup < iGroups; iGroup++)
        {
            GroupEdgeInfo oGroupEdgeInfo = oAllGroupEdgeInfos[iGroup];

            // Populate GraphMetricValueOrdered collections with the top words
            // or word pairs and their counts for this group.

            List<GraphMetricValueOrdered> oTopWordsOrWordPairs =
                new List<GraphMetricValueOrdered>();

            List<GraphMetricValueOrdered> oTopCounts =
                new List<GraphMetricValueOrdered>();

            String sGroupName = GetGroupName(oGroupEdgeInfo);

            String sGroupNameOrDummyGroupName =
                sGroupName ?? GroupEdgeSorter.DummyGroupNameForEntireGraph;

            Int32 iFirstRowForGroup;

            if ( oGroupNameIndexes.TryGetValue(sGroupNameOrDummyGroupName,
                out iFirstRowForGroup) )
            {
                TwitterSearchNetworkWordMetricUtil.CopyWordMetricsForGroup(
                    sGroupNameOrDummyGroupName, aoWord1Values, aoWord2Values,
                    aoCountValues, aoGroupNameValues, iFirstRowForGroup,
                    MaximumTopItems, oTopWordsOrWordPairs, oTopCounts);
            }

            // (The extra "1" is for the dummy group that represents the entire
            // graph.)

            if (iGroup < MaximumTopGroups + 1)
            {
                String sGraphMetricColumn1Name, sGraphMetricColumn2Name;

                GetGraphMetricColumnNames(sTableDescription, sGroupName,
                    out sGraphMetricColumn1Name, out sGraphMetricColumn2Name);

                AddGraphMetricColumnPair(sTableName, sGraphMetricColumn1Name,
                    sGraphMetricColumn2Name, oTopWordsOrWordPairs, oTopCounts,
                    false, oGraphMetricColumns);
            }

            if (oGroupEdgeInfo.GroupInfo != null)
            {
                // This is a real group, not the dummy group.  Add a cell for
                // the column on the group worksheet.

                oTopWordsOrWordPairsForGroupWorksheet.Add(
                    new GraphMetricValueWithID(
                        GetGroupRowID(oGroupEdgeInfo),

                        ExcelUtil.ForceCellText(
                            TwitterSearchNetworkWordMetricUtil
                            .ConcatenateTopWordsOrWordPairs(
                                oTopWordsOrWordPairs, bIsForWords,
                                MaximumTopItems) )
                    ) );
            }
        }

        oGraphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Groups, TableNames.Groups, sGroupTableColumnName,
            ExcelTableUtil.AutoColumnWidth, null, null,
            oTopWordsOrWordPairsForGroupWorksheet.ToArray() ) );
    }

    //*************************************************************************
    //  Method: GetTableName()
    //
    /// <summary>
    /// Gets the name to use for a new table on the Twitter search network top
    /// items worksheet.
    /// </summary>
    ///
    /// <param name="iTableIndex">
    /// The index of the table that will be added.  This gets incremented by
    /// this method.
    /// </param>
    ///
    /// <returns>
    /// The name to use for a new table.
    /// </returns>
    //*************************************************************************

    protected String
    GetTableName
    (
        ref Int32 iTableIndex
    )
    {
        Debug.Assert(iTableIndex >= 1);
        AssertValid();

        return (TableNames.TwitterSearchNetworkTopItemsRoot + iTableIndex++);
    }

    //*************************************************************************
    //  Method: GetGroupName()
    //
    /// <summary>
    /// Gets a group name from a GroupEdgeInfo object.
    /// </summary>
    ///
    /// <param name="oGroupEdgeInfo">
    /// The object to get a group name from.
    /// </param>
    ///
    /// <returns>
    /// The group name, or null if <paramref name="oGroupEdgeInfo" /> is the
    /// "dummy" group that represents the entire graph.
    /// </returns>
    //*************************************************************************

    protected String
    GetGroupName
    (
        GroupEdgeInfo oGroupEdgeInfo
    )
    {
        AssertValid();

        return ( (oGroupEdgeInfo.GroupInfo == null) ?
            null : oGroupEdgeInfo.GroupInfo.Name );
    }

    //*************************************************************************
    //  Method: GetGroupRowID()
    //
    /// <summary>
    /// Gets the row ID for a group.
    /// </summary>
    ///
    /// <param name="oGroupEdgeInfo">
    /// Information about the group to get the row ID for.
    /// </param>
    ///
    /// <returns>
    /// The row ID for the specified group.
    /// </returns>
    //*************************************************************************

    protected Int32
    GetGroupRowID
    (
        GroupEdgeInfo oGroupEdgeInfo
    )
    {
        Debug.Assert(oGroupEdgeInfo != null);
        Debug.Assert(oGroupEdgeInfo.GroupInfo.RowID.HasValue);
        AssertValid();

        return (oGroupEdgeInfo.GroupInfo.RowID.Value);
    }

    //*************************************************************************
    //  Method: GetGraphMetricColumnNames()
    //
    /// <summary>
    /// Gets the column names to use for a pair of graph metric columns.
    /// </summary>
    ///
    /// <param name="sTableDescription">
    /// Description of the table the graph metric columns will eventually be
    /// written to.  Sample: "URLs in Tweet".
    /// </param>
    ///
    /// <param name="sGroupName">
    /// Name of the group to which the pair of graph metric columns
    /// corresponds, or null if it's the "dummy" group that represents the
    /// entire graph.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn1Name">
    /// Where the name of the first graph metric column gets stored.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn2Name">
    /// Where the name of the second graph metric column gets stored.
    /// </param>
    //*************************************************************************

    protected void
    GetGraphMetricColumnNames
    (
        String sTableDescription,
        String sGroupName,
        out String sGraphMetricColumn1Name,
        out String sGraphMetricColumn2Name
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTableDescription) );
        AssertValid();

        Boolean bEntireGraph = (sGroupName == null);

        sGraphMetricColumn1Name = String.Format(
            "Top {0} in {1}"
            ,
            sTableDescription,
            bEntireGraph ? EntireGraphGroupName : sGroupName
            );

        sGraphMetricColumn2Name =
            (bEntireGraph ? EntireGraphGroupName : sGroupName) + " Count";
    }

    //*************************************************************************
    //  Method: AddGraphMetricColumnPair()
    //
    /// <summary>
    /// Adds a pair of GraphMetricColumn objects to a collection.
    /// </summary>
    ///
    /// <param name="sTableName">
    /// The name of the table the columns will eventually be written to.  The
    /// table is on the Twitter search network top items worksheet.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn1Name">
    /// The name of the first graph metric column.
    /// </param>
    ///
    /// <param name="sGraphMetricColumn2Name">
    /// The name of the second graph metric column.
    /// </param>
    ///
    /// <param name="oTopStrings">
    /// Collection of top strings.
    /// </param>
    ///
    /// <param name="oTopStringCounts">
    /// Collection of top string counts.
    /// </param>
    ///
    /// <param name="bTopStringsContainUrls">
    /// true if the collection of top strings might contains URLs.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Collection of GraphMetricColumn objects that this method adds to.
    /// </param>
    //*************************************************************************

    protected void
    AddGraphMetricColumnPair
    (
        String sTableName,
        String sGraphMetricColumn1Name,
        String sGraphMetricColumn2Name,
        List<GraphMetricValueOrdered> oTopStrings,
        List<GraphMetricValueOrdered> oTopStringCounts,
        Boolean bTopStringsContainUrls,
        List<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMetricColumn1Name) );
        Debug.Assert( !String.IsNullOrEmpty(sGraphMetricColumn2Name) );
        Debug.Assert(oTopStrings != null);
        Debug.Assert(oTopStringCounts != null);
        Debug.Assert(oTopStrings.Count == oTopStringCounts.Count);
        AssertValid();

        oGraphMetricColumns.Add( new GraphMetricColumnOrdered(
            WorksheetNames.TwitterSearchNetworkTopItems, sTableName,
            sGraphMetricColumn1Name, ExcelTableUtil.AutoColumnWidth, null,
            null, oTopStrings.ToArray()
            ) );

        if (bTopStringsContainUrls)
        {
            oGraphMetricColumns[oGraphMetricColumns.Count - 1]
                .ConvertUrlsToHyperlinks = true;
        }

        oGraphMetricColumns.Add( new GraphMetricColumnOrdered(
            WorksheetNames.TwitterSearchNetworkTopItems, sTableName,
            sGraphMetricColumn2Name, ExcelTableUtil.AutoColumnWidth, null,
            null, oTopStringCounts.ToArray()
            ) );
    }

    //*************************************************************************
    //  Method: GetSearchTerm()
    //
    /// <summary>
    /// Attempts to get the search term that was used to create the network.
    /// </summary>
    ///
    /// <returns>
    /// The search term, or null if not available.
    /// </returns>
    //*************************************************************************

    protected String
    GetSearchTerm()
    {
        AssertValid();

        // If the user opted to add a description of the imported data to the
        // graph history, then the graph history has an import description in
        // this format:
        //
        //   The graph represents a network of {0} Twitter {1} whose recent
        //   tweets contained \"{2}\", or who...
        //
        // This description was created by the TwitterSearchNetworkAnalyzer
        // class.
        //
        // Parse the "{2}" argument.
        //
        // (This is fragile.  A more robust solution would store the raw search
        // term in a separate metadata value.  For now, there is a comment in
        // the TwitterSearchNetworkAnalyzer code warning about this
        // dependency.)

        String sImportDescription;

        if ( m_oGraphHistory.TryGetValue(GraphHistoryKeys.ImportDescription,
            out sImportDescription) )
        {
            const String Pattern =
                "contained \"(?<SearchTerm>.+)\", or who";

            Match oMatch = Regex.Match(sImportDescription, Pattern);

            if (oMatch.Success)
            {
                return (oMatch.Groups["SearchTerm"].Value);
            }
        }

        return (null);
    }

    //*************************************************************************
    //  Method: ReportProgressAndCheckCancellationPending()
    //
    /// <summary>
    /// Reports progress to the calling thread and checks for cancellation if a
    /// <see cref="BackgroundWorker" /> is in use.
    /// </summary>
    ///
    /// <param name="oCalculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <returns>
    /// false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ReportProgressAndCheckCancellationPending
    (
        CalculateGraphMetricsContext oCalculateGraphMetricsContext
    )
    {
        Debug.Assert(oCalculateGraphMetricsContext != null);
        AssertValid();

        BackgroundWorker oBackgroundWorker =
            oCalculateGraphMetricsContext.BackgroundWorker;

        if (oBackgroundWorker != null)
        {
            if (oBackgroundWorker.CancellationPending)
            {
                return (false);
            }

            oBackgroundWorker.ReportProgress(50,
                "Calculating Twitter search network top items."
                );
        }

        return (true);
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

        Debug.Assert(m_oGraphHistory != null);
        Debug.Assert(m_oTwitterStatusTextParser != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// The number of top groups to calculate metrics for, when the groups are
    /// sorted by descending vertex count.

    protected const Int32 MaximumTopGroups = 10;

    /// Maximum number of top items to include.

    protected const Int32 MaximumTopItems = 10;


    /// Column names on the edge table.

    protected const String StatusColumnName = "Tweet";
    ///
    protected const String UrlsInTweetColumnName = "URLs in Tweet";
    ///
    protected const String DomainsInTweetColumnName = "Domains in Tweet";
    ///
    protected const String HashtagsInTweetColumnName = "Hashtags in Tweet";


    /// Column names on the vertex table.

    protected const String TweetsColumnName = "Tweets";

    /// Display text for the dummy group that represents the entire graph.

    protected const String EntireGraphGroupName = "Entire Graph";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Describes how the graph was created.

    protected GraphHistory m_oGraphHistory;

    /// Parses the text of a Twitter tweet.  This class uses only one instance
    /// to avoid making TwitterStatusTextParser recompile all of its regular
    /// expressions.

    protected TwitterStatusTextParser m_oTwitterStatusTextParser;
}

}
