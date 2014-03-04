
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TwitterSearchNetworkVertexMetricUtil
//
/// <summary>
/// Provides utility methods for calculating vertex metrics within a Twitter
/// search network.
/// </summary>
//*****************************************************************************

public static class TwitterSearchNetworkVertexMetricUtil : Object
{
    //*************************************************************************
    //  Method: AddGraphMetricValueForTopStrings()
    //
    /// <summary>
    /// Adds a graph metric value that is a concatenation of the top strings by
    /// count and salience in a set of edges.
    /// </summary>
    ///
    /// <param name="edges">
    /// The edges to use.
    /// </param>
    ///
    /// <param name="edgeColumnName">
    /// Name of the edge column to read.  The column must contain
    /// space-delimited strings.  Sample: "URLs in Tweet", in which case the
    /// column contains space-delimited URLs that this method counts, ranks,
    /// concatenates, and adds as graph metric values.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <param name="vertexRowID">
    /// The ID of the vertex worksheet row containing the user the edges belong
    /// to.
    /// </param>
    ///
    /// <param name="topStringsByCountGraphMetricValues">
    /// The top strings by count collection to add to.
    /// </param>
    ///
    /// <param name="topStringsBySalienceGraphMetricValues">
    /// The top strings by salience collection to add to.
    /// </param>
    //*************************************************************************

    public static void
    AddGraphMetricValueForTopStrings
    (
        IEnumerable<IEdge> edges,
        String edgeColumnName,
        Int32 maximumTopStrings,
        Int32 vertexRowID,
        List<GraphMetricValueWithID> topStringsByCountGraphMetricValues,
        List<GraphMetricValueWithID> topStringsBySalienceGraphMetricValues
    )
    {
        Debug.Assert(edges != null);
        Debug.Assert( !String.IsNullOrEmpty(edgeColumnName) );
        Debug.Assert(maximumTopStrings > 0);
        Debug.Assert(topStringsByCountGraphMetricValues != null);
        Debug.Assert(topStringsBySalienceGraphMetricValues != null);

        AddGraphMetricValueForTopStringsByCount(edges, edgeColumnName,
            maximumTopStrings, vertexRowID,
            topStringsByCountGraphMetricValues);

        AddGraphMetricValueForTopStringsBySalience(edges, edgeColumnName,
            maximumTopStrings, vertexRowID,
            topStringsBySalienceGraphMetricValues);
    }

    //*************************************************************************
    //  Method: AddGraphMetricValuesForTopWordsAndWordPairs()
    //
    /// <summary>
    /// Adds graph metric values that are concatenations of the top words and
    /// word pairs by count and salience in a set of edges.
    /// </summary>
    ///
    /// <param name="edges">
    /// The edges to use.
    /// </param>
    ///
    /// <param name="statusEdgeColumnName">
    /// Name of the Tweet edge column.
    /// </param>
    ///
    /// <param name="maximumTopTerms">
    /// Maximum number of top words and word pairs to include.
    /// </param>
    ///
    /// <param name="wordCounter">
    /// Counter created by <see cref="CreateCountersForWordsAndWordPairs" />.
    /// </param>
    ///
    /// <param name="wordPairCounter">
    /// Counter created by <see cref="CreateCountersForWordsAndWordPairs" />.
    /// </param>
    ///
    /// <param name="vertexRowID">
    /// The ID of the vertex worksheet row containing the user the edges belong
    /// to.
    /// </param>
    ///
    /// <param name="topWordsInTweetByCountGraphMetricValues">
    /// The top words by count collection to add to.
    /// </param>
    ///
    /// <param name="topWordsInTweetBySalienceGraphMetricValues">
    /// The top words by salience collection to add to.
    /// </param>
    ///
    /// <param name="topWordPairsInTweetByCountGraphMetricValues">
    /// The top word pairs by count collection to add to.
    /// </param>
    ///
    /// <param name="topWordPairsInTweetBySalienceGraphMetricValues">
    /// The top word pairs by salience collection to add to.
    /// </param>
    //*************************************************************************

    public static void
    AddGraphMetricValuesForTopWordsAndWordPairs
    (
        IEnumerable<IEdge> edges,
        String statusEdgeColumnName,
        Int32 maximumTopTerms,
        WordCounter wordCounter,
        WordPairCounter wordPairCounter,
        Int32 vertexRowID,

        List<GraphMetricValueWithID>
            topWordsInTweetByCountGraphMetricValues,

        List<GraphMetricValueWithID>
            topWordsInTweetBySalienceGraphMetricValues,

        List<GraphMetricValueWithID>
            topWordPairsInTweetByCountGraphMetricValues,

        List<GraphMetricValueWithID>
            topWordPairsInTweetBySalienceGraphMetricValues
    )
    {
        Debug.Assert(edges != null);
        Debug.Assert( !String.IsNullOrEmpty(statusEdgeColumnName) );
        Debug.Assert(maximumTopTerms > 0);
        Debug.Assert(wordCounter != null);
        Debug.Assert(wordPairCounter != null);
        Debug.Assert(topWordsInTweetByCountGraphMetricValues != null);
        Debug.Assert(topWordsInTweetBySalienceGraphMetricValues != null);
        Debug.Assert(topWordPairsInTweetByCountGraphMetricValues != null);
        Debug.Assert(topWordPairsInTweetBySalienceGraphMetricValues != null);

        String sTopWordsInTweetByCount, sTopWordsInTweetBySalience,
            sTopWordPairsInTweetByCount, sTopWordPairsInTweetBySalience;

        ConcatenateTopWordsAndWordPairs(edges, statusEdgeColumnName,
            maximumTopTerms, wordCounter, wordPairCounter,
            out sTopWordsInTweetByCount, out sTopWordsInTweetBySalience,
            out sTopWordPairsInTweetByCount,
            out sTopWordPairsInTweetBySalience);

        topWordsInTweetByCountGraphMetricValues.Add(
            new GraphMetricValueWithID( vertexRowID,
                ExcelUtil.ForceCellText(sTopWordsInTweetByCount) ) );

        topWordsInTweetBySalienceGraphMetricValues.Add(
            new GraphMetricValueWithID( vertexRowID,
                ExcelUtil.ForceCellText(sTopWordsInTweetBySalience) ) );

        topWordPairsInTweetByCountGraphMetricValues.Add(
            new GraphMetricValueWithID( vertexRowID,
                ExcelUtil.ForceCellText(sTopWordPairsInTweetByCount) ) );

        topWordPairsInTweetBySalienceGraphMetricValues.Add(
            new GraphMetricValueWithID( vertexRowID,
                ExcelUtil.ForceCellText(sTopWordPairsInTweetBySalience) ) );
    }

    //*************************************************************************
    //  Method: CreateCountersForWordsAndWordPairs()
    //
    /// <summary>
    /// Create objects for counting words and word pairs in a set of statuses.
    /// </summary>
    ///
    /// <param name="searchTerm">
    /// The Twitter search term that was used to create the network, or null
    /// if not available.
    /// </param>
    ///
    /// <param name="calculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="wordCounter">
    /// Where the new WordCounter gets stored.
    /// </param>
    ///
    /// <param name="wordPairCounter">
    /// Where the new WordPairCounter gets stored.
    /// </param>
    //*************************************************************************

    public static void
    CreateCountersForWordsAndWordPairs
    (
        String searchTerm,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out WordCounter wordCounter,
        out WordPairCounter wordPairCounter
    )
    {
        Debug.Assert(calculateGraphMetricsContext != null);

        // When counting word pairs, skip words in the user-supplied list.

        String [] asWordsForWordCounterToSkip = StringUtil.SplitOnSpaces(
            calculateGraphMetricsContext.GraphMetricUserSettings
            .WordMetricUserSettings.WordsToSkip);

        wordPairCounter = new WordPairCounter(asWordsForWordCounterToSkip);

        // When counting words, skip words in the user-supplied list, AND the
        // search term, AND "rt" ("reply to").

        List<String> oWordsForWordPairCounterToSkip = new List<String>(
            asWordsForWordCounterToSkip);

        if ( !String.IsNullOrEmpty(searchTerm) )
        {
            oWordsForWordPairCounterToSkip.AddRange(
                StringUtil.SplitOnSpaces(searchTerm) );
        }

        oWordsForWordPairCounterToSkip.Add("rt");

        wordCounter = new WordCounter(
            oWordsForWordPairCounterToSkip.ToArray() );
    }

    //*************************************************************************
    //  Method: GetUniqueEdgesByUser()
    //
    /// <summary>
    /// Gets the unique edges belonging to each user in the graph.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to analyze.
    /// </param>
    ///
    /// <returns>
    /// A dictionary.  The key is a screen name and the value is a collection
    /// of zero or more unique edges belonging to that user.
    /// </returns>
    ///
    /// <remarks>
    /// An edge "belongs" to a user if the name of edge's first vertex is the 
    /// user's screen name.  That's because in the Twitter search network, each
    /// edge represents a status and the screen name of the tweeter is always
    /// in the Vertex 1 column.
    /// </remarks>
    //*************************************************************************

    public static Dictionary< String, List<IEdge> >
    GetUniqueEdgesByUser
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        Dictionary< String, List<IEdge> > oUniqueEdgesByUser =
            new Dictionary< String, List<IEdge> >();

        // Skip edges that correspond to the same status.

        foreach ( IEdge oEdge in EdgeFilter.EnumerateEdgesByUniqueImportedID(
            graph.Edges) )
        {
            String sScreenName = oEdge.Vertex1.Name;

            if ( !String.IsNullOrEmpty(sScreenName) )
            {
                List<IEdge> oUniqueEdgesForUser;

                if ( !oUniqueEdgesByUser.TryGetValue(
                    sScreenName, out oUniqueEdgesForUser) )
                {
                    // This is the first edge for the user.

                    oUniqueEdgesForUser = new List<IEdge>();

                    oUniqueEdgesByUser.Add(
                        sScreenName, oUniqueEdgesForUser);
                }

                oUniqueEdgesForUser.Add(oEdge);
            }
        }

        return (oUniqueEdgesByUser);
    }

    //*************************************************************************
    //  Method: AddGraphMetricColumns()
    //
    /// <summary>
    /// Adds graph metric columns for the vertex worksheet.
    /// </summary>
    ///
    /// <param name="graphMetricColumns">
    /// Collection of GraphMetricColumn objects that gets added to.
    /// </param>
    ///
    /// <param name="graphMetricValuesAndColumnNamePairs">
    /// One or more pairs of objects.  The first object in each pair is
    /// List of GraphMetricValueWithID> objects, and the second object in each
    /// pair is the corresponding column name on the vertex worksheet.
    /// </param>
    //*************************************************************************

    public static void
    AddGraphMetricColumns
    (
        List<GraphMetricColumn> graphMetricColumns,
        params Object[] graphMetricValuesAndColumnNamePairs
    )
    {
        Debug.Assert(graphMetricColumns != null);
        Debug.Assert(graphMetricValuesAndColumnNamePairs != null);

        Int32 iObjects = graphMetricValuesAndColumnNamePairs.Length;
        Debug.Assert(iObjects % 2 == 0);

        for (Int32 i = 0; i < iObjects; i+= 2)
        {
            Debug.Assert( graphMetricValuesAndColumnNamePairs[i + 0] is
                List<GraphMetricValueWithID> );

            Debug.Assert(graphMetricValuesAndColumnNamePairs[i + 1] is
                String);

            AddGraphMetricColumn(

                ( List<GraphMetricValueWithID> )
                    graphMetricValuesAndColumnNamePairs[i + 0],

                (String)graphMetricValuesAndColumnNamePairs[i + 1],
                graphMetricColumns
                );
        }
    }

    //*************************************************************************
    //  Method: AddGraphMetricColumn()
    //
    /// <summary>
    /// Adds a graph metric column for the vertex worksheet.
    /// </summary>
    ///
    /// <param name="graphMetricValues">
    /// The values to store in the column.
    /// </param>
    ///
    /// <param name="graphMetricColumnName">
    /// Name of the column on the vertex worksheet.
    /// </param>
    ///
    /// <param name="graphMetricColumns">
    /// Collection of GraphMetricColumn objects that gets added to.
    /// </param>
    //*************************************************************************

    private static void
    AddGraphMetricColumn
    (
        List<GraphMetricValueWithID> graphMetricValues,
        String graphMetricColumnName,
        List<GraphMetricColumn> graphMetricColumns
    )
    {
        Debug.Assert(graphMetricValues != null);
        Debug.Assert( !String.IsNullOrEmpty(graphMetricColumnName) );
        Debug.Assert(graphMetricColumns != null);

        const String NumericFormat = "0";

        graphMetricColumns.Add( new GraphMetricColumnWithID(
            WorksheetNames.Vertices, TableNames.Vertices,
            graphMetricColumnName, ExcelTableUtil.AutoColumnWidth,
            NumericFormat, CellStyleNames.GraphMetricGood,
            graphMetricValues.ToArray() ) );
    }

    //*************************************************************************
    //  Method: AddGraphMetricValueForTopStringsByCount()
    //
    /// <summary>
    /// Adds a graph metric value that is a concatenation of the top strings by
    /// count in a set of edges.
    /// </summary>
    ///
    /// <param name="edges">
    /// The edges to use.
    /// </param>
    ///
    /// <param name="edgeColumnName">
    /// Name of the edge column to read.  The column must contain
    /// space-delimited strings.  Sample: "URLs in Tweet", in which case the
    /// column contains space-delimited URLs that this method counts, ranks,
    /// concatenates, and adds as a graph metric value.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <param name="vertexRowID">
    /// The ID of the vertex worksheet row containing the user the edges belong
    /// to.
    /// </param>
    ///
    /// <param name="graphMetricValues">
    /// The collection to add to.
    /// </param>
    //*************************************************************************

    private static void
    AddGraphMetricValueForTopStringsByCount
    (
        IEnumerable<IEdge> edges,
        String edgeColumnName,
        Int32 maximumTopStrings,
        Int32 vertexRowID,
        List<GraphMetricValueWithID> graphMetricValues
    )
    {
        Debug.Assert(edges != null);
        Debug.Assert( !String.IsNullOrEmpty(edgeColumnName) );
        Debug.Assert(maximumTopStrings > 0);
        Debug.Assert(graphMetricValues != null);

        String sTopStrings = ConcatenateTopStringsByCount(
            edges, edgeColumnName, maximumTopStrings);

        graphMetricValues.Add(
            new GraphMetricValueWithID(vertexRowID, sTopStrings) );
    }

    //*************************************************************************
    //  Method: AddGraphMetricValueForTopStringsBySalience()
    //
    /// <summary>
    /// Adds a graph metric value that is a concatenation of the top strings by
    /// salience in a set of edges.
    /// </summary>
    ///
    /// <param name="edges">
    /// The edges to use.
    /// </param>
    ///
    /// <param name="edgeColumnName">
    /// Name of the edge column to read.  The column must contain
    /// space-delimited strings.  Sample: "URLs in Tweet", in which case the
    /// column contains space-delimited URLs that this method counts, ranks,
    /// concatenates, and adds as a graph metric value.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <param name="vertexRowID">
    /// The ID of the vertex worksheet row containing the user the edges belong
    /// to.
    /// </param>
    ///
    /// <param name="graphMetricValues">
    /// The collection to add to.
    /// </param>
    //*************************************************************************

    private static void
    AddGraphMetricValueForTopStringsBySalience
    (
        IEnumerable<IEdge> edges,
        String edgeColumnName,
        Int32 maximumTopStrings,
        Int32 vertexRowID,
        List<GraphMetricValueWithID> graphMetricValues
    )
    {
        Debug.Assert(edges != null);
        Debug.Assert( !String.IsNullOrEmpty(edgeColumnName) );
        Debug.Assert(maximumTopStrings > 0);
        Debug.Assert(graphMetricValues != null);

        String sTopStrings = ConcatenateTopStringsBySalience(
            edges, edgeColumnName, maximumTopStrings);

        graphMetricValues.Add(
            new GraphMetricValueWithID(vertexRowID, sTopStrings) );
    }

    //*************************************************************************
    //  Method: ConcatenateTopStringsByCount()
    //
    /// <summary>
    /// Gets a space-delimited string of the top strings by count in a set of
    /// edges.
    /// </summary>
    ///
    /// <param name="edges">
    /// The edges to use.
    /// </param>
    ///
    /// <param name="edgeColumnName">
    /// Name of the edge column to read.  The column must contain
    /// space-delimited strings.  Sample: "URLs in Tweet", in which case the
    /// column contains space-delimited URLs that this method counts, ranks,
    /// and concatenates.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <returns>
    /// Sample: "http://Url1 http://Url2"
    /// </returns>
    //*************************************************************************

    private static String
    ConcatenateTopStringsByCount
    (
        IEnumerable<IEdge> edges,
        String edgeColumnName,
        Int32 maximumTopStrings
    )
    {
        Debug.Assert(edges != null);
        Debug.Assert( !String.IsNullOrEmpty(edgeColumnName) );
        Debug.Assert(maximumTopStrings > 0);

        Dictionary<String, Int32> oStringCounts =
            TwitterSearchNetworkStringUtil.CountDelimitedStringsInEdgeColumn(
                edges, edgeColumnName);

        return ( TwitterSearchNetworkStringUtil.ConcatenateTopStrings(
            oStringCounts, maximumTopStrings) );
    }

    //*************************************************************************
    //  Method: ConcatenateTopStringsBySalience()
    //
    /// <summary>
    /// Gets a space-delimited string of the top strings by salience in a set
    /// of edges.
    /// </summary>
    ///
    /// <param name="edges">
    /// The edges to use.
    /// </param>
    ///
    /// <param name="edgeColumnName">
    /// Name of the edge column to read.  The column must contain
    /// space-delimited strings.  Sample: "URLs in Tweet", in which case the
    /// column contains space-delimited URLs that this method counts, ranks,
    /// and concatenates.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <returns>
    /// Sample: "http://Url1 http://Url2"
    /// </returns>
    //*************************************************************************

    private static String
    ConcatenateTopStringsBySalience
    (
        IEnumerable<IEdge> edges,
        String edgeColumnName,
        Int32 maximumTopStrings
    )
    {
        Debug.Assert(edges != null);
        Debug.Assert( !String.IsNullOrEmpty(edgeColumnName) );
        Debug.Assert(maximumTopStrings > 0);

        // Don't convert to lower case (bitly URLs are case-sensitive, for
        // example), and don't skip any words.

        WordCounter oWordCounter = new WordCounter( false, new String[0] );
        oWordCounter.SkipUrlsAndPunctuation = false;

        foreach (IEdge oEdge in edges)
        {
            String sSpaceDelimitedCellValue;

            if ( oEdge.TryGetNonEmptyStringValue(edgeColumnName,
                out sSpaceDelimitedCellValue) )
            {
                oWordCounter.CountTermsInDocument(sSpaceDelimitedCellValue);
            }
        }

        oWordCounter.CalculateSalienceOfCountedTerms();

        return ( String.Join(TwitterSearchNetworkWordMetricUtil.WordSeparator,

            TwitterSearchNetworkStringUtil.TakeTopStringsAsArray(

                (from CountedWord oCountedWord in oWordCounter.CountedTerms
                orderby oCountedWord.Salience descending
                select oCountedWord.Word),
            
                maximumTopStrings
            ) ) );
    }

    //*************************************************************************
    //  Method: ConcatenateTopWordsAndWordPairs()
    //
    /// <summary>
    /// Gets space-delimited strings of the top words and word pairs by count
    /// and salience in a set of edges.
    /// </summary>
    ///
    /// <param name="oEdges">
    /// The edges to use.
    /// </param>
    ///
    /// <param name="sStatusEdgeColumnName">
    /// Name of the Tweet edge column.
    /// </param>
    ///
    /// <param name="iMaximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <param name="oWordCounter">
    /// Counter created by <see cref="CreateCountersForWordsAndWordPairs" />.
    /// </param>
    ///
    /// <param name="oWordPairCounter">
    /// Counter created by <see cref="CreateCountersForWordsAndWordPairs" />.
    /// </param>
    ///
    /// <param name="sTopWordsInTweetByCount">
    /// Where the top words by count get stored.
    /// </param>
    ///
    /// <param name="sTopWordsInTweetBySalience">
    /// Where the top words by salience get stored.
    /// </param>
    ///
    /// <param name="sTopWordPairsInTweetByCount">
    /// Where the top word pairs by count get stored.
    /// </param>
    ///
    /// <param name="sTopWordPairsInTweetBySalience">
    /// Where the top word pairs by salience get stored.
    /// </param>
    //*************************************************************************

    private static void
    ConcatenateTopWordsAndWordPairs
    (
        IEnumerable<IEdge> oEdges,
        String sStatusEdgeColumnName,
        Int32 iMaximumTopStrings,
        WordCounter oWordCounter,
        WordPairCounter oWordPairCounter,
        out String sTopWordsInTweetByCount,
        out String sTopWordsInTweetBySalience,
        out String sTopWordPairsInTweetByCount,
        out String sTopWordPairsInTweetBySalience
    )
    {
        Debug.Assert(oEdges != null);
        Debug.Assert( !String.IsNullOrEmpty(sStatusEdgeColumnName) );
        Debug.Assert(iMaximumTopStrings > 0);
        Debug.Assert(oWordCounter != null);
        Debug.Assert(oWordPairCounter != null);

        oWordCounter.Clear();
        oWordPairCounter.Clear();

        foreach (IEdge oEdge in oEdges)
        {
            String sStatus;

            if ( oEdge.TryGetNonEmptyStringValue(sStatusEdgeColumnName,
                out sStatus) )
            {
                oWordCounter.CountTermsInDocument(sStatus);
                oWordPairCounter.CountTermsInDocument(sStatus);
            }
        }

        oWordCounter.CalculateSalienceOfCountedTerms();
        oWordPairCounter.CalculateSalienceOfCountedTerms();

        sTopWordsInTweetByCount = String.Join(

            TwitterSearchNetworkWordMetricUtil.WordSeparator,

            TwitterSearchNetworkStringUtil.TakeTopStringsAsArray(

                (from CountedWord oCountedWord in oWordCounter.CountedTerms
                orderby oCountedWord.Count descending
                select oCountedWord.Word)
                ,
                iMaximumTopStrings
            ) );

        sTopWordsInTweetBySalience = String.Join(
        
            TwitterSearchNetworkWordMetricUtil.WordSeparator,

            TwitterSearchNetworkStringUtil.TakeTopStringsAsArray(

                (from CountedWord oCountedWord in oWordCounter.CountedTerms
                orderby oCountedWord.Salience descending
                select oCountedWord.Word)
                ,
                iMaximumTopStrings
            ) );

        sTopWordPairsInTweetByCount = String.Join(
        
            TwitterSearchNetworkWordMetricUtil.WordPairSeparator,

            TwitterSearchNetworkStringUtil.TakeTopStringsAsArray(

                (from CountedWordPair oCountedWordPair in
                    oWordPairCounter.CountedTerms

                orderby oCountedWordPair.Count descending

                select TwitterSearchNetworkWordMetricUtil.FormatWordPair(
                    oCountedWordPair) )
                ,
                iMaximumTopStrings
            ) );

        sTopWordPairsInTweetBySalience = String.Join(

            TwitterSearchNetworkWordMetricUtil.WordPairSeparator,

            TwitterSearchNetworkStringUtil.TakeTopStringsAsArray(

                (from CountedWordPair oCountedWordPair in
                    oWordPairCounter.CountedTerms

                orderby oCountedWordPair.Salience descending

                select TwitterSearchNetworkWordMetricUtil.FormatWordPair(
                    oCountedWordPair) )
                ,
                iMaximumTopStrings
            ) );
    }
}

}
