
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TwitterSearchNetworkWordMetricUtil
//
/// <summary>
/// Provides utility methods for calculating word metrics within a Twitter
/// search network.
/// </summary>
//*****************************************************************************

public static class TwitterSearchNetworkWordMetricUtil : Object
{
    //*************************************************************************
    //  Method: TryCalculateWordMetrics()
    //
    /// <summary>
    /// Attempts to calculate word metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="calculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="statusColumnName">
    /// Name of the status column on the edge table.
    /// </param>
    ///
    /// <param name="wordMetricColumns">
    /// Where the calculated word metric columns get stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the word metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryCalculateWordMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        String statusColumnName,
        out GraphMetricColumn [] wordMetricColumns
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(calculateGraphMetricsContext != null);
        Debug.Assert( !String.IsNullOrEmpty(statusColumnName) );

        // Use the WordMetricCalculator2() class to calculate word metrics for
        // all groups.
        //
        // This is somewhat wasteful, because we don't actually need all the
        // word metrics for all groups.  A future version might refactor common
        // code out of WordMetricCalculator2() that can be called by that class
        // and this one.

        GraphMetricUserSettings oGraphMetricUserSettings =
            calculateGraphMetricsContext.GraphMetricUserSettings;

        WordMetricUserSettings oWordMetricUserSettings =
            oGraphMetricUserSettings.WordMetricUserSettings;

        GraphMetrics eOriginalGraphMetricsToCalculate =
            oGraphMetricUserSettings.GraphMetricsToCalculate;

        Boolean bOriginalTextColumnIsOnEdgeWorksheet =
            oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet;

        String sOriginalTextColumnName =
            oWordMetricUserSettings.TextColumnName;

        Boolean bOriginalCountByGroup = oWordMetricUserSettings.CountByGroup;

        oGraphMetricUserSettings.GraphMetricsToCalculate = GraphMetrics.Words;
        oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet = true;
        oWordMetricUserSettings.TextColumnName = statusColumnName;
        oWordMetricUserSettings.CountByGroup = true;

        try
        {
            return ( ( new WordMetricCalculator2() ).TryCalculateGraphMetrics(
                graph, calculateGraphMetricsContext, out wordMetricColumns) );
        }
        finally
        {
            oGraphMetricUserSettings.GraphMetricsToCalculate =
                eOriginalGraphMetricsToCalculate;

            oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet =
                bOriginalTextColumnIsOnEdgeWorksheet;

            oWordMetricUserSettings.TextColumnName = sOriginalTextColumnName;
            oWordMetricUserSettings.CountByGroup = bOriginalCountByGroup;
        }
    }

    //*************************************************************************
    //  Method: GetGroupNameIndexesFromWordMetrics()
    //
    /// <summary>
    /// Gets the indexes of the start of each group for words or word pairs.
    /// </summary>
    ///
    /// <param name="groupNameValues">
    /// The group name values for words or word pairs.
    /// </param>
    ///
    /// <returns>
    /// The key is a group name from the word or word pair table created by
    /// WordMetricCalculator2 and the value is the index of the first row for
    /// that group.
    ///
    /// <para>
    /// Note that the group name for the "dummy" group that represents the
    /// entire graph is <see
    /// cref="GroupEdgeSorter.DummyGroupNameForEntireGraph" />.
    /// </para>
    ///
    /// </returns>
    //*************************************************************************

    public static Dictionary<String, Int32>
    GetGroupNameIndexesFromWordMetrics
    (
        GraphMetricValueOrdered [] groupNameValues
    )
    {
        Debug.Assert(groupNameValues != null);

        Dictionary<String, Int32> oGroupNameIndexes =
            new Dictionary<String, Int32>();

        Int32 iRows = groupNameValues.Length;
        String sCurrentGroupName = String.Empty;

        for (Int32 iRow = 0; iRow < iRows; iRow++)
        {
            Object oGroupNameAsObject = groupNameValues[iRow].Value;

            if (oGroupNameAsObject is String)
            {
                String sGroupName = (String)oGroupNameAsObject;

                if (sGroupName != sCurrentGroupName)
                {
                    oGroupNameIndexes.Add(sGroupName, iRow);
                    sCurrentGroupName = sGroupName;
                }
            }
        }

        return (oGroupNameIndexes);
    }

    //*************************************************************************
    //  Method: CopyWordMetricsForGroup()
    //
    /// <summary>
    /// Copies calculated metrics for words or word pairs.
    /// </summary>
    ///
    /// <param name="groupNameOrDummyGroupName">
    /// Name of the group, or the dummy name if the group is the "dummy" group
    /// for the entire graph.
    /// </param>
    ///
    /// <param name="word1Values">
    /// Values that were calculated for either the word (for words) or word 1
    /// (for word pairs).
    /// </param>
    ///
    /// <param name="word2Values">
    /// Values that were calculated for word 2, or null if this is being called
    /// for words.
    /// </param>
    ///
    /// <param name="countValues">
    /// Values that were calculated for the word or word pair count.
    /// </param>
    ///
    /// <param name="groupNameValues">
    /// Values that were calculated for the group name.
    /// </param>
    ///
    /// <param name="firstRowForGroup">
    /// The first row for the group in the calculated values.
    /// </param>
    ///
    /// <param name="maximumTopWordsOrWordPairs">
    /// Maximum number of top words or word pairs to include.
    /// </param>
    ///
    /// <param name="topWordsOrWordPairs">
    /// The collection of words or word pairs that this method fills in.
    /// </param>
    ///
    /// <param name="topCounts">
    /// The collection of counts that this method fills in.
    /// </param>
    ///
    /// <remarks>
    /// This method copies the word or word pair metrics that were calculated
    /// for a group to new collections.
    /// </remarks>
    //*************************************************************************

    public static void
    CopyWordMetricsForGroup
    (
        String groupNameOrDummyGroupName,
        GraphMetricValueOrdered [] word1Values,
        GraphMetricValueOrdered [] word2Values,
        GraphMetricValueOrdered [] countValues,
        GraphMetricValueOrdered [] groupNameValues,
        Int32 firstRowForGroup,
        Int32 maximumTopWordsOrWordPairs,
        List<GraphMetricValueOrdered> topWordsOrWordPairs,
        List<GraphMetricValueOrdered> topCounts
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(groupNameOrDummyGroupName) );
        Debug.Assert(word1Values != null);
        // word2Values
        Debug.Assert(countValues != null);
        Debug.Assert(groupNameValues != null);
        Debug.Assert(firstRowForGroup >= 0);
        Debug.Assert(maximumTopWordsOrWordPairs > 0);
        Debug.Assert(topWordsOrWordPairs != null);
        Debug.Assert(topCounts != null);

        Int32 iRows = groupNameValues.Length;

        for (Int32 iRow = firstRowForGroup, iItems = 0;
            iRow < iRows && iItems < maximumTopWordsOrWordPairs;
            iRow++, iItems++)
        {
            Object oWord1AsObject = word1Values[iRow].Value;
            Object oCountAsObject = countValues[iRow].Value;
            Object oGroupNameAsObject = groupNameValues[iRow].Value;

            if (
                !(oGroupNameAsObject is String)
                ||
                (String)oGroupNameAsObject != groupNameOrDummyGroupName
                ||
                !(oWord1AsObject is String)
                ||
                !(oCountAsObject is Int32)
                )
            {
                break;
            }

            String sWordOrWordPair = ExcelUtil.UnforceCellText(
                (String)oWord1AsObject );

            if (word2Values != null)
            {
                Object oWord2AsObject = word2Values[iRow].Value;

                if ( !(oWord2AsObject is String) )
                {
                    break;
                }

                sWordOrWordPair = FormatWordPair(
                    sWordOrWordPair,
                    ExcelUtil.UnforceCellText( (String)oWord2AsObject ) );
            }

            topWordsOrWordPairs.Add( new GraphMetricValueOrdered(
                ExcelUtil.ForceCellText(sWordOrWordPair) ) );

            topCounts.Add( new GraphMetricValueOrdered(oCountAsObject) );
        }
    }

    //*************************************************************************
    //  Method: ConcatenateTopWordsOrWordPairs()
    //
    /// <summary>
    /// Concatenates a list of top words or word pairs.
    /// </summary>
    ///
    /// <param name="topWordsOrWordPairs">
    /// The top words or word pairs to concatenate.
    /// </param>
    ///
    /// <param name="concatenateTopWords">
    /// true if <paramref name="topWordsOrWordPairs" /> contains words, false
    /// if it contains word pairs.
    /// </param>
    ///
    /// <param name="maximumTopWordsOrWordPairs">
    /// Maximum number of top words or word pairs to include.
    /// </param>
    ///
    /// <returns>
    /// The top words or word pairs concatenated with a space.
    /// </returns>
    //*************************************************************************

    public static String
    ConcatenateTopWordsOrWordPairs
    (
        List<GraphMetricValueOrdered> topWordsOrWordPairs,
        Boolean concatenateTopWords,
        Int32 maximumTopWordsOrWordPairs
    )
    {
        Debug.Assert(topWordsOrWordPairs != null);
        Debug.Assert(maximumTopWordsOrWordPairs > 0);

        return ( String.Join(

            concatenateTopWords ? WordSeparator : WordPairSeparator,

            TwitterSearchNetworkStringUtil.TakeTopStringsAsArray(

                (from oGroupMetricValueWithID in topWordsOrWordPairs

                select ExcelUtil.UnforceCellText(
                    (String)oGroupMetricValueWithID.Value ) )
                ,
                maximumTopWordsOrWordPairs)
            ) );
    }

    //*************************************************************************
    //  Method: FormatWordPair()
    //
    /// <summary>
    /// Formats a word pair.
    /// </summary>
    ///
    /// <param name="wordPair">
    /// The word pair.
    /// </param>
    ///
    /// <returns>
    /// A formatted word pair.
    /// </returns>
    //*************************************************************************

    public static String
    FormatWordPair
    (
        CountedWordPair wordPair
    )
    {
        Debug.Assert(wordPair != null);

        return ( FormatWordPair(wordPair.Word1, wordPair.Word2) );
    }

    //*************************************************************************
    //  Method: FormatWordPair()
    //
    /// <summary>
    /// Formats a word pair.
    /// </summary>
    ///
    /// <param name="word1">
    /// The first word.
    /// </param>
    ///
    /// <param name="word2">
    /// The second word.
    /// </param>
    ///
    /// <returns>
    /// A formatted word pair.
    /// </returns>
    //*************************************************************************

    private static String
    FormatWordPair
    (
        String word1,
        String word2
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(word1) );
        Debug.Assert( !String.IsNullOrEmpty(word2) );

        return (word1 + "," + word2);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Separator string used when concatenating words.

    public const String WordSeparator = " ";

    /// Separator string used when concatenating word pairs.  Note the extra
    /// space, which is added for clarity.

    public const String WordPairSeparator = "  ";
}

}
