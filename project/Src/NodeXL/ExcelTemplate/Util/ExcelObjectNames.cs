
using System;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WorksheetNames
//
/// <summary>
/// Provides the names of the Excel worksheets used by the template.
/// </summary>
///
/// <remarks>
/// All worksheet names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class WorksheetNames
{
    /// <summary>
    /// Name of the required worksheet that contains edge data.
    /// </summary>

    public const String Edges = "Edges";

    /// <summary>
    /// Name of the optional worksheet that contains vertex data.
    /// </summary>

    public const String Vertices = "Vertices";

    /// <summary>
    /// Name of the optional worksheet that contains group data.
    /// </summary>

    public const String Groups = "Groups";

    /// <summary>
    /// Name of the optional worksheet that contains group-vertex data.
    /// </summary>

    public const String GroupVertices = "Group Vertices";

    /// <summary>
    /// Name of the optional worksheet that contains group-edge metrics.
    /// </summary>

    public const String GroupEdgeMetrics = "Group Edges";

    /// <summary>
    /// Name of the optional worksheet that contains overall graph metrics.
    /// </summary>

    public const String OverallMetrics = "Overall Metrics";

    /// <summary>
    /// Name of the optional worksheet that contains top-N-by metrics.
    /// </summary>

    public const String TopNByMetrics = "Top Items";

    /// <summary>
    /// Name of the optional worksheet that contains Twitter search network top
    /// items.  Note that this is abbreviated, because worksheet names can't be
    /// longer than 32 characters.
    /// </summary>

    public const String TwitterSearchNetworkTopItems =
        "Twitter Search Ntwrk Top Items";

    /// <summary>
    /// Name of the optional worksheet that contains word counts.
    /// </summary>

    public const String WordCounts = "Words";

    /// <summary>
    /// Name of the optional worksheet that contains word pair counts.
    /// </summary>

    public const String WordPairCounts = "Word Pairs";

    /// <summary>
    /// Name of the optional worksheet that contains miscellaneous information.
    /// </summary>

    public const String Miscellaneous = "Misc";
}


//*****************************************************************************
//  Class: TableNames
//
/// <summary>
/// Provides the names of the Excel tables (ListObjects) used by the template.
/// </summary>
///
/// <remarks>
/// All table names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class TableNames
{
    /// <summary>
    /// Name of the required table that contains edge data.
    /// </summary>

    public const String Edges = "Edges";

    /// <summary>
    /// Name of the optional table that contains vertex data.
    /// </summary>

    public const String Vertices = "Vertices";

    /// <summary>
    /// Name of the optional table that contains group data.
    /// </summary>

    public const String Groups = "Groups";

    /// <summary>
    /// Name of the optional table that contains group-vertex data.
    /// </summary>

    public const String GroupVertices = "GroupVertices";

    /// <summary>
    /// Name of the optional table that contains group-edge metrics.
    /// </summary>

    public const String GroupEdgeMetrics = "GroupEdges";

    /// <summary>
    /// Name of the optional table that contains overall graph metrics.
    /// </summary>

    public const String OverallMetrics = "OverallMetrics";

    /// <summary>
    /// Root of the names of the optional tables that contain top-N-by metrics.
    /// The tables are numbered consecutively: "TopItems_1", "TopItems_2", and
    /// so on.
    /// </summary>

    public const String TopNByMetricsRoot = "TopItems_";

    /// <summary>
    /// Root of the names of the optional tables that contain Twitter search
    /// network top items.  The tables are numbered consecutively:
    /// "TwitterSearchNetworkTopItems_1", "TwitterSearchNetworkTopItems_2", and
    /// so on.
    /// </summary>

    public const String TwitterSearchNetworkTopItemsRoot =
        "TwitterSearchNetworkTopItems_";

    /// <summary>
    /// Name of the optional table that contains word counts.
    /// </summary>

    public const String WordCounts = "Words";

    /// <summary>
    /// Name of the optional table that contains word pair counts.
    /// </summary>

    public const String WordPairCounts = "WordPairs";

    /// <summary>
    /// Name of the optional table that contains overall readability metrics.
    /// </summary>

    public const String OverallReadabilityMetrics =
        "OverallReadabilityMetrics";

    /// <summary>
    /// Name of the optional table that contains per-workbook settings.
    /// </summary>

    public const String PerWorkbookSettings = "PerWorkbookSettings";

    /// <summary>
    /// Name of the optional table that contains dynamic filter settings.
    /// </summary>

    public const String DynamicFilterSettings = "DynamicFilterSettings";
}


//*****************************************************************************
//  Class: EdgeTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the edge table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class EdgeTableColumnNames
{
    /// <summary>
    /// Name of the required table column containing the edge's first vertex.
    /// </summary>

    public const String Vertex1Name = "Vertex 1";

    /// <summary>
    /// Name of the required table column containing the edge's second vertex.
    /// </summary>

    public const String Vertex2Name = "Vertex 2";

    /// <summary>
    /// Name of the optional table column containing the edge's color.
    /// </summary>

    public const String Color = "Color";

    /// <summary>
    /// Name of the optional table column containing the edge's width.
    /// </summary>

    public const String Width = "Width";

    /// <summary>
    /// Name of the optional table column containing the edge's style.
    /// </summary>

    public const String Style = "Style";

    /// <summary>
    /// Name of the optional table column containing the edge's label.
    /// </summary>

    public const String Label = "Label";

    /// <summary>
    /// Name of the optional table column containing the edge's label text
    /// color.
    /// </summary>

    public const String LabelTextColor = "Label Text Color";

    /// <summary>
    /// Name of the optional table column containing the edge's label font
    /// size.
    /// </summary>

    public const String LabelFontSize = "Label Font Size";

    /// <summary>
    /// Name of the optional table column containing the "this edge is
    /// reciprocated" flag.  This gets added to the table on demand by <see
    /// cref="EdgeReciprocationCalculator2" />.
    /// </summary>

    public const String IsReciprocated = "Reciprocated?";

    /// <summary>
    /// Name of the optional table column containing the edge weight.  This
    /// gets added to the table on demand by various classes.
    /// </summary>

    public const String EdgeWeight = "Edge Weight";

    /// <summary>
    /// Name of the optional table column containing the edge's edge crossing
    /// readability metric.  This gets added to the table on demand by <see
    /// cref="ReadabilityMetricCalculator2" />.
    /// </summary>

    public const String EdgeCrossings = "Edge Crossing Readability";

    // IMPORTANT NOTES:
    //
    // 1. If a new column name is added, EdgeColumnComboBox may need to be
    //    modified to exclude the new column name.
    //
    // 2. If the new column is part of a column group, ColumnGroupManager must
    //    be modified.
}


//*****************************************************************************
//  Class: VertexTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the vertex table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class VertexTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the vertex name.
    /// </summary>

    public const String VertexName = "Vertex";

    /// <summary>
    /// Name of the optional table column containing the vertex's color.
    /// </summary>

    public const String Color = "Color";

    /// <summary>
    /// Name of the optional table column containing the vertex's shape.
    /// </summary>

    public const String Shape = "Shape";

    /// <summary>
    /// Name of the optional table column containing the vertex's radius.  Note
    /// that "radius" would more accurately be called "size" and is called that
    /// in the UI, but that changing "radius" to "size" throughout the source
    /// code is too difficult.  Thus, the source code uses "radius" but the UI
    /// uses "size".
    /// </summary>

    public const String Radius = "Size";

    /// <summary>
    /// Name of the optional table column containing the URI of the image file.
    /// </summary>

    public const String ImageUri = "Image File";

    /// <summary>
    /// Name of the optional table column containing the vertex's label.
    /// </summary>

    public const String Label = "Label";

    /// <summary>
    /// Name of the optional table column containing the vertex's label fill
    /// color.
    /// </summary>

    public const String LabelFillColor = "Label Fill Color";

    /// <summary>
    /// Name of the optional table column containing the vertex's label
    /// position.
    /// </summary>

    public const String LabelPosition = "Label Position";

    /// <summary>
    /// Name of the optional table column containing the vertex's tooltip.
    /// </summary>

    public const String ToolTip = "Tooltip";

    /// <summary>
    /// Name of the optional table column containing the vertex's layout order.
    /// </summary>

    public const String LayoutOrder = "Layout Order";

    /// <summary>
    /// Name of the optional table column containing a boolean flag indicating
    /// whether the vertex should be locked at its current location.
    /// </summary>

    public const String Locked = "Locked?";

    /// <summary>
    /// Name of the optional table column containing the vertex's x-coordinate.
    /// </summary>

    public const String X = "X";

    /// <summary>
    /// Name of the optional table column containing the vertex's y-coordinate.
    /// </summary>

    public const String Y = "Y";

    /// <summary>
    /// Name of the optional table column containing the vertex's polar R
    /// coordinate.
    /// </summary>

    public const String PolarR = "Polar R";

    /// <summary>
    /// Name of the optional table column containing the vertex's polar angle
    /// coordinate.
    /// </summary>

    public const String PolarAngle = "Polar Angle";

    /// <summary>
    /// Name of the optional table column containing the "this row is marked"
    /// flag.  This gets added to the table on demand when the user marks one
    /// or more vertices in the graph.
    /// </summary>

    public const String IsMarked = "Marked?";

    /// <summary>
    /// Name of the optional table column containing an image of the vertex's
    /// subgraph.  This gets added to the table on demand by <see
    /// cref="TableImagePopulator" />.
    /// </summary>

    public const String SubgraphImage = "Subgraph";

    /// <summary>
    /// Name of the optional table column containing the text of a custom menu
    /// item to add to the vertex context menu in the graph.  "Base" means that
    /// there can be more than one such column.  Additional columns have "2",
    /// "3", and so on appended to the base column name.
    /// </summary>

    public const String CustomMenuItemTextBase =
        "Custom Menu Item Text";

    /// <summary>
    /// Name of the optional table column containing the action to take when
    /// the menu item corresponding to <see cref="CustomMenuItemTextBase" /> is
    /// selected.  "Base" means that there can be more than one such column.
    /// Additional columns have "2", "3", and so on appended to the base
    /// column name.
    /// </summary>

    public const String CustomMenuItemActionBase =
        "Custom Menu Item Action";

    /// <summary>
    /// Name of the optional table column containing the vertex's in-degree.
    /// This gets added to the table on demand by <see
    /// cref="VertexDegreeCalculator2" />.
    /// </summary>

    public const String InDegree = "In-Degree";

    /// <summary>
    /// Name of the optional table column containing the vertex's out-degree.
    /// This gets added to the table on demand by <see
    /// cref="VertexDegreeCalculator2" />.
    /// </summary>

    public const String OutDegree = "Out-Degree";

    /// <summary>
    /// Name of the optional table column containing the vertex's degree.  This
    /// gets added to the table on demand by <see
    /// cref="VertexDegreeCalculator2" />.
    /// </summary>

    public const String Degree = "Degree";

    /// <summary>
    /// Name of the optional table column containing the vertex's betweenness
    /// centrality.  This gets added to the table on demand by <see
    /// cref="BrandesFastCentralityCalculator2" />.
    /// </summary>

    public const String BetweennessCentrality =
        "Betweenness Centrality";

    /// <summary>
    /// Name of the optional table column containing the vertex's closeness
    /// centrality.  This gets added to the table on demand by <see
    /// cref="BrandesFastCentralityCalculator2" />.
    /// </summary>

    public const String ClosenessCentrality =
        "Closeness Centrality";

    /// <summary>
    /// Name of the optional table column containing the vertex's eigenvector
    /// centrality.  This gets added to the table on demand by <see
    /// cref="EigenvectorCentralityCalculator2" />.
    /// </summary>

    public const String EigenvectorCentrality =
        "Eigenvector Centrality";

    /// <summary>
    /// Name of the optional table column containing the vertex's PageRank.
    /// This gets added to the table on demand by <see
    /// cref="PageRankCalculator2" />.
    /// </summary>

    public const String PageRank = "PageRank";

    /// <summary>
    /// Name of the optional table column containing the vertex's clustering
    /// coefficient.  This gets added to the table on demand by <see
    /// cref="ClusteringCoefficientCalculator2" />.
    /// </summary>

    public const String ClusteringCoefficient =
        "Clustering Coefficient";

    /// <summary>
    /// Name of the optional table column containing the vertex's reciprocated
    /// pair ratio.  .  This gets added to the table on demand by <see
    /// cref="ReciprocatedVertexPairRatioCalculator2" />.
    /// </summary>

    public const String ReciprocatedVertexPairRatio =
        "Reciprocated Vertex Pair Ratio";

    /// <summary>
    /// Name of the optional table column containing the vertex's vertex
    /// overlap readability metric.  This gets added to the table on demand by
    /// <see cref="ReadabilityMetricCalculator2" />.
    /// </summary>

    public const String VertexOverlap = "Vertex Overlap Readability";

    /// <summary>
    /// Name of the optional table column containing the vertex's edge crossing
    /// readability metric.  This gets added to the table on demand by <see
    /// cref="ReadabilityMetricCalculator2" />.
    /// </summary>

    public const String EdgeCrossings = "Edge Crossing Readability";

    /// <summary>
    /// Name of the optional table column containing the vertex's top URLs in
    /// tweets, by count.  This gets added to the table on demand by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopUrlsInTweetByCount =
        "Top URLs in Tweet by Count";

    /// <summary>
    /// Name of the optional table column containing the vertex's top URLs in
    /// tweets, by salience.  This gets added to the table on demand by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopUrlsInTweetBySalience =
        "Top URLs in Tweet by Salience";

    /// <summary>
    /// Name of the optional table column containing the vertex's top domains
    /// in tweets, by count.  This gets added to the table on demand by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopDomainsInTweetByCount =
        "Top Domains in Tweet by Count";

    /// <summary>
    /// Name of the optional table column containing the vertex's top domains
    /// in tweets, by salience.  This gets added to the table on demand by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopDomainsInTweetBySalience =
        "Top Domains in Tweet by Salience";

    /// <summary>
    /// Name of the optional table column containing the vertex's top hashtags
    /// in tweets, by count.  This gets added to the table on demand by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopHashtagsInTweetByCount =
        "Top Hashtags in Tweet by Count";

    /// <summary>
    /// Name of the optional table column containing the vertex's top hashtags
    /// in tweets, by salience.  This gets added to the table on demand by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopHashtagsInTweetBySalience =
        "Top Hashtags in Tweet by Salience";

    /// <summary>
    /// Name of the optional table column containing the vertex's top words
    /// in tweets, by count.  This gets added to the table on demand by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopWordsInTweetByCount =
        "Top Words in Tweet by Count";

    /// <summary>
    /// Name of the optional table column containing the vertex's top words
    /// in tweets, by salience.  This gets added to the table on demand by <see
    /// cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopWordsInTweetBySalience =
        "Top Words in Tweet by Salience";

    /// <summary>
    /// Name of the optional table column containing the vertex's top word
    /// pairs in tweets, by count.  This gets added to the table on demand by
    /// <see cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopWordPairsInTweetByCount =
        "Top Word Pairs in Tweet by Count";

    /// <summary>
    /// Name of the optional table column containing the vertex's top word
    /// pairs in tweets, by salience.  This gets added to the table on demand
    /// by <see cref="TwitterSearchNetworkTopItemsCalculator2" />.
    /// </summary>

    public const String TopWordPairsInTweetBySalience =
        "Top Word Pairs in Tweet by Salience";


    // IMPORTANT NOTES:
    //
    // 1. If a new column name is added, VertexColumnComboBox may need to be
    //    modified to exclude the new column name.
    //
    // 2. If the new column is part of a column group, ColumnGroupManager must
    //    be modified.
}


//*****************************************************************************
//  Class: GroupTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the group table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class GroupTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the group name.
    /// </summary>

    public const String Name = "Group";

    /// <summary>
    /// Name of the optional table column containing the color of the vertices
    /// in the group.
    /// </summary>

    public const String VertexColor = "Vertex Color";

    /// <summary>
    /// Name of the optional table column containing the shape of the vertices
    /// in the group.
    /// </summary>

    public const String VertexShape = "Vertex Shape";

    /// <summary>
    /// Name of the optional table column containing a flag indicating whether
    /// the group is collapsed.
    /// </summary>

    public const String Collapsed = "Collapsed?";

    /// <summary>
    /// Name of the optional table column containing the collapsed group's
    /// x-coordinate.
    /// </summary>

    public const String CollapsedX = "Collapsed X";

    /// <summary>
    /// Name of the optional table column containing the collapsed group's
    /// y-coordinate.
    /// </summary>

    public const String CollapsedY = "Collapsed Y";

    /// <summary>
    /// Name of the optional table column that describes how the group should
    /// be drawn when it is collapsed.
    /// </summary>

    public const String CollapsedAttributes = "Collapsed Properties";

    /// <summary>
    /// Name of the optional table column containing the group's label.
    /// </summary>

    public const String Label = "Label";

    /// <summary>
    /// Name of the optional table column containing the number of vertices in
    /// the group.
    /// </summary>

    public const String Vertices = OverallMetricNames.Vertices;

    /// <summary>
    /// Name of the optional table column containing the number of unique edges
    /// in the group.
    /// </summary>

    public const String UniqueEdges = OverallMetricNames.UniqueEdges;

    /// <summary>
    /// Name of the optional table column containing the number of edges in the
    /// group that have duplicates.
    /// </summary>

    public const String EdgesWithDuplicates =
        OverallMetricNames.EdgesWithDuplicates;

    /// <summary>
    /// Name of the optional table column containing the total number of edges
    /// in the group.
    /// </summary>

    public const String TotalEdges = OverallMetricNames.TotalEdges;

    /// <summary>
    /// Name of the optional table column containing the number of self-loops
    /// in the group.
    /// </summary>

    public const String SelfLoops = OverallMetricNames.SelfLoops;

    /// <summary>
    /// Name of the optional table column containing the reciprocated vertex
    /// pair ratio.
    /// </summary>

    public const String ReciprocatedVertexPairRatio =
        OverallMetricNames.ReciprocatedVertexPairRatio;

    /// <summary>
    /// Name of the optional table column containing the reciprocated edge
    /// ratio.
    /// </summary>

    public const String ReciprocatedEdgeRatio =
        OverallMetricNames.ReciprocatedEdgeRatio;

    /// <summary>
    /// Name of the optional table column containing the number of connected
    /// components in the group.
    /// </summary>

    public const String ConnectedComponents =
        OverallMetricNames.ConnectedComponents;

    /// <summary>
    /// Name of the optional table column containing the number of connected
    /// components in the group that have one vertex.
    /// </summary>

    public const String SingleVertexConnectedComponents =
        OverallMetricNames.SingleVertexConnectedComponents;

    /// <summary>
    /// Name of the optional table column containing the maximum number of
    /// vertices in the group's connected components.
    /// </summary>

    public const String MaximumConnectedComponentVertices =
        OverallMetricNames.MaximumConnectedComponentVertices;

    /// <summary>
    /// Name of the optional table column containing the maximum number of
    /// edges in the group's connected components.
    /// </summary>

    public const String MaximumConnectedComponentEdges =
        OverallMetricNames.MaximumConnectedComponentEdges;

    /// <summary>
    /// Name of the optional table column containing the maximum geodesic
    /// distance in the group.
    /// </summary>

    public const String MaximumGeodesicDistance =
        OverallMetricNames.MaximumGeodesicDistance;

    /// <summary>
    /// Name of the optional table column containing the average geodesic
    /// distance in the group.
    /// </summary>

    public const String AverageGeodesicDistance =
        OverallMetricNames.AverageGeodesicDistance;

    /// <summary>
    /// Name of the optional table column containing the group's graph density.
    /// </summary>

    public const String GraphDensity = OverallMetricNames.GraphDensity;

    /// <summary>
    /// Name of the optional table column containing the group's modularity.
    /// </summary>

    public const String Modularity = OverallMetricNames.Modularity;

    /// <summary>
    /// Name of the optional table column containing the group's top
    /// replies-to.
    /// </summary>

    public const String TopRepliesToInTweet = "Top Replied-To in Tweet";

    /// <summary>
    /// Name of the optional table column containing the group's top mentions.
    /// </summary>

    public const String TopMentionsInTweet = "Top Mentioned in Tweet";

    /// <summary>
    /// Name of the optional table column containing the group's top URLs.
    /// </summary>

    public const String TopUrlsInTweet = "Top URLs in Tweet";

    /// <summary>
    /// Name of the optional table column containing the group's top domains.
    /// </summary>

    public const String TopDomainsInTweet = "Top Domains in Tweet";

    /// <summary>
    /// Name of the optional table column containing the group's top hashtags.
    /// </summary>

    public const String TopHashtagsInTweet = "Top Hashtags in Tweet";

    /// <summary>
    /// Name of the optional table column containing the group's top words.
    /// </summary>

    public const String TopWordsInTweet = "Top Words in Tweet";

    /// <summary>
    /// Name of the optional table column containing the group's top word
    /// pairs.
    /// </summary>

    public const String TopWordPairsInTweet = "Top Word Pairs in Tweet";

    /// <summary>
    /// Name of the optional table column containing the group's top tweeters.
    /// </summary>

    public const String TopTweeters = "Top Tweeters";

    // IMPORTANT NOTES:
    //
    // 1. If a new column name is added, GroupColumnComboBox may need to be
    //    modified to exclude the new column name.
    //
    // 2. If a new column is part of a column group, ColumnGroupManager must be
    //    modified.
}


//*****************************************************************************
//  Class: GroupVertexTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the group-vertex table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class GroupVertexTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the group name.
    /// </summary>

    public const String GroupName = "Group";

    /// <summary>
    /// Name of the optional table column containing the vertex name.
    /// </summary>

    public const String VertexName = "Vertex";

    /// <summary>
    /// Name of the optional table column containing the vertex ID from the
    /// vertex table.
    /// </summary>

    public const String VertexID = "Vertex ID";
}


//*****************************************************************************
//  Class: GroupEdgeTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the group-edge table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class GroupEdgeTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the name of the first
    /// group.
    /// </summary>

    public const String Group1Name = "Group 1";

    /// <summary>
    /// Name of the optional table column containing the name of the second
    /// group.
    /// </summary>

    public const String Group2Name = "Group 2";

    /// <summary>
    /// Name of the optional table column containing the number of edges
    /// connecting the groups.
    /// </summary>

    public const String Edges = "Edges";
}


//*****************************************************************************
//  Class: WordTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the word count table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class WordTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the word.
    /// </summary>

    public const String Word = "Word";

    /// <summary>
    /// Name of the optional table column containing the number of times the
    /// word was found.
    /// </summary>

    public const String Count = "Count";

    /// <summary>
    /// Name of the optional table column containing the salience of the word.
    /// </summary>

    public const String Salience = "Salience";

    /// <summary>
    /// Name of the optional table column containing the group the word was
    /// found in.
    /// </summary>

    public const String Group = "Group";
}

//*****************************************************************************
//  Class: WordPairTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the word pair count table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class WordPairTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the first word in the
    /// word pair.
    /// </summary>

    public const String Word1 = "Word 1";

    /// <summary>
    /// Name of the optional table column containing the second word in the
    /// word pair.
    /// </summary>

    public const String Word2 = "Word 2";

    /// <summary>
    /// Name of the optional table column containing the number of times the
    /// word pair was found.
    /// </summary>

    public const String Count = "Count";

    /// <summary>
    /// Name of the optional table column containing the salience of the word
    /// pair.
    /// </summary>

    public const String Salience = "Salience";

    /// <summary>
    /// Name of the optional table column containing the mutual information of
    /// the word pair.
    /// </summary>

    public const String MutualInformation = "Mutual Information";

    /// <summary>
    /// Name of the optional table column containing the group the word pair
    /// was found in.
    /// </summary>

    public const String Group = "Group";
}


//*****************************************************************************
//  Class: OverallMetricsTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the overall graph metrics table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class OverallMetricsTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the metric name.
    /// </summary>

    public const String Name = "Graph Metric";

    /// <summary>
    /// Name of the optional table column containing the metric value.
    /// </summary>

    public const String Value = "Value";
}


//*****************************************************************************
//  Class: OverallReadabilityMetricsTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the overall readability metrics table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class OverallReadabilityMetricsTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the metric name.
    /// </summary>

    public const String Name = "Readability Metric";

    /// <summary>
    /// Name of the optional table column containing the metric value.
    /// </summary>

    public const String Value = "Value";
}


//*****************************************************************************
//  Class: PerWorkbookSettingsTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the per-workbook settings table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class PerWorkbookSettingsTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the setting name.
    /// </summary>

    public const String Name = "Per-Workbook Setting";

    /// <summary>
    /// Name of the optional table column containing the setting value.
    /// </summary>

    public const String Value = "Value";
}


//*****************************************************************************
//  Class: DynamicFilterSettingsTableColumnNames
//
/// <summary>
/// Provides the names of the columns in the dynamic filter settings table.
/// </summary>
///
/// <remarks>
/// All column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class DynamicFilterSettingsTableColumnNames
{
    /// <summary>
    /// Name of the table column containing the name of the table containing
    /// the column being filtered on.
    /// </summary>

    public const String TableName = "Table Name";

    /// <summary>
    /// Name of the table column containing the name of the column being
    /// filtered on.
    /// </summary>

    public const String ColumnName = "Column Name";

    /// <summary>
    /// Name of the table column containing the minimum value to show.
    /// </summary>

    public const String SelectedMinimum = "Selected Minimum";

    /// <summary>
    /// Name of the table column containing the maximum value to show.
    /// </summary>

    public const String SelectedMaximum = "Selected Maximum";
}


//*****************************************************************************
//  Class: CommonTableColumnNames
//
/// <summary>
/// Provides the names of the columns that are present in multiple tables.
/// </summary>
///
/// <remarks>
/// All common column names are available as public constants.
/// </remarks>
//*****************************************************************************

public static class CommonTableColumnNames
{
    /// <summary>
    /// Name of the optional table column containing the row's unique ID.
    /// </summary>

    public const String ID = "ID";

    /// <summary>
    /// Name of the optional table column containing a visibility.
    /// </summary>

    public const String Visibility = "Visibility";

    /// <summary>
    /// Name of the optional table column containing an alpha value.
    /// </summary>

    public const String Alpha = "Opacity";

    /// <summary>
    /// Name of the optional table column indicating to users where their own
    /// columns can be added.
    /// </summary>

    public const String AddColumnsHere = "Add Your Own Columns Here";

    /// <summary>
    /// Name of the optional table column used by the dynamic filter feature.
    /// </summary>

    public const String DynamicFilter = "Dynamic Filter";

    /// <summary>
    /// The <see cref="TwitterSearchNetworkTopItemsCalculator2" /> and <see
    /// cref="WordMetricCalculator2" /> classes use this column to skip over
    /// redundant rows.
    /// </summary>

    public const String ImportedID = "Imported ID";
}


//*****************************************************************************
//  Class: NamedRangeNames
//
/// <summary>
/// Names of named ranges.
/// </summary>
//*****************************************************************************

public static class NamedRangeNames
{
    /// <summary>
    /// Name of the cell that contains the column for which a dynamic filter
    /// histogram is needed.  Sample cell value: "Edges[Edge Weight]".
    /// </summary>

    public const String DynamicFilterSourceColumnRange =
        "DynamicFilterSourceColumnRange";

    /// <summary>
    /// Name of the range that must be calculated when a dynamic filter
    /// histogram is needed.
    /// </summary>

    public const String DynamicFilterForceCalculationRange =
        "DynamicFilterForceCalculationRange";
}


//*****************************************************************************
//  Class: CellStyleNames
//
/// <summary>
/// Names of cell styles that get applied programatically.
/// </summary>
///
/// <remarks>
/// This class includes standard Excel styles as well as custom styles defined
/// in the NodeXL template.
/// </remarks>
//*****************************************************************************

public static class CellStyleNames
{
    /// <summary>
    /// Style applied to cells that are required.  This is a custom NodeXL
    /// style.
    /// </summary>

    public const String Required = "NodeXL Required";

    /// <summary>
    /// Style applied to cells that store visual properties of the graph.  This
    /// is a custom NodeXL style.
    /// </summary>

    public const String VisualProperty = "NodeXL Visual Property";

    /// <summary>
    /// Style applied to cells that store label properties of the graph.  This
    /// is a custom NodeXL style.
    /// </summary>

    public const String Label = "NodeXL Label";

    /// <summary>
    /// Style applied to graph metric cells for which graph metric values were
    /// successfully calculated.  This is a custom NodeXL style.
    /// </summary>

    public const String GraphMetricGood = "NodeXL Graph Metric";

    /// <summary>
    /// Style applied to graph metric cells for which graph metric values could
    /// not be calculated.  This is a standard Excel style.
    /// </summary>

    public const String GraphMetricBad = "Bad";

    /// <summary>
    /// Style applied to separator rows in the overall graph metrics table.
    /// </summary>

    public const String GraphMetricSeparatorRow =
        "NodeXL Graph Metric Separator";

    /// <summary>
    /// Style applied to cells that users should not edit.  This is a custom
    /// NodeXL style.
    /// </summary>

    public const String DoNotEdit = "NodeXL Do Not Edit";

}


//*****************************************************************************
//  Class: TableStyleNames
//
/// <summary>
/// Names of table styles that get applied programatically.
/// </summary>
//*****************************************************************************

public static class TableStyleNames
{
    /// <summary>
    /// Style applied to all NodeXL tables.  This is a custom NodeXL style.
    /// </summary>

    public const String NodeXLTable = "NodeXL Table";
}


//*****************************************************************************
//  Class: ChartNames
//
/// <summary>
/// Names of charts.
/// </summary>
//*****************************************************************************

public static class ChartNames
{
    /// <summary>
    /// Name of the chart that displays a histogram for a dynamic filter.
    /// </summary>

    public const String DynamicFilterHistogram = "DynamicFilterHistogram";
}

}
