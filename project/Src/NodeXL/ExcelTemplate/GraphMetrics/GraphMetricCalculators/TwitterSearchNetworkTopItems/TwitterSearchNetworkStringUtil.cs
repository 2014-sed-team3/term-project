
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TwitterSearchNetworkStringUtil
//
/// <summary>
/// Provides utility methods for working with strings while calculating the top
/// items within a Twitter search network.
/// </summary>
//*****************************************************************************

public static class TwitterSearchNetworkStringUtil : Object
{
    //*************************************************************************
    //  Method: GetTopGraphMetricValues()
    //
    /// <summary>
    /// Gets a pair of GraphMetricValueOrdered collections populated with top
    /// strings and their counts.
    /// </summary>
    ///
    /// <param name="stringCounts">
    /// The key is a string being counted and the value is the number of times
    /// the string was found.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <param name="topStrings">
    /// Where this method stores the top strings in <paramref
    /// name="stringCounts" />.
    /// </param>
    ///
    /// <param name="topStringCounts">
    /// Where this method stores the counts for the top strings in <paramref
    /// name="stringCounts" />.
    /// </param>
    //*************************************************************************

    public static void
    GetTopGraphMetricValues
    (
        Dictionary<String, Int32> stringCounts,
        Int32 maximumTopStrings,
        out List<GraphMetricValueOrdered> topStrings,
        out List<GraphMetricValueOrdered> topStringCounts
    )
    {
        Debug.Assert(stringCounts != null);
        Debug.Assert(maximumTopStrings > 0);

        topStrings = new List<GraphMetricValueOrdered>();
        topStringCounts = new List<GraphMetricValueOrdered>();

        // Sort the Dictionary by descending string counts.

        foreach ( String sKey in GetTopStrings(
            stringCounts, maximumTopStrings) )
        {
            topStrings.Add( new GraphMetricValueOrdered(sKey) );

            topStringCounts.Add(
                new GraphMetricValueOrdered( stringCounts[sKey] ) );
        }
    }

    //*************************************************************************
    //  Method: ConcatenateTopStrings()
    //
    /// <summary>
    /// Concatenates the top strings from a dictionary of string counts.
    /// </summary>
    ///
    /// <param name="stringCounts">
    /// The key is a string and the value is the number of times the string was
    /// found.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <remarks>
    /// The top strings in <paramref name="stringCounts" />, separated by
    /// spaces.
    /// </remarks>
    //*************************************************************************

    public static String
    ConcatenateTopStrings
    (
        Dictionary<String, Int32> stringCounts,
        Int32 maximumTopStrings
    )
    {
        Debug.Assert(stringCounts != null);
        Debug.Assert(maximumTopStrings > 0);

        return ( String.Join( " ",
            GetTopStrings(stringCounts, maximumTopStrings).ToArray() ) );
    }

    //*************************************************************************
    //  Method: GetTopStrings()
    //
    /// <summary>
    /// Gets the top string counts in a dictionary of strings.
    /// </summary>
    ///
    /// <param name="stringCounts">
    /// The key is a string being counted (an URL in the tweets, for example),
    /// and the value is the number of times the string was found in the edge
    /// column.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to include.
    /// </param>
    ///
    /// <returns>
    /// The top strings being counted, sorted by the number of times the string
    /// was found in the edge column.
    /// </returns>
    //*************************************************************************

    public static IEnumerable<String>
    GetTopStrings
    (
        Dictionary<String, Int32> stringCounts,
        Int32 maximumTopStrings
    )
    {
        Debug.Assert(stringCounts != null);
        Debug.Assert(maximumTopStrings > 0);

        // Sort the Dictionary by descending string counts.

        return (
            (from sKey in stringCounts.Keys
            orderby stringCounts[sKey] descending
            select sKey).Take(maximumTopStrings)
            );
    }

    //*************************************************************************
    //  Method: TakeTopStringsAsArray()
    //
    /// <summary>
    /// Takes the top strings from a string enumeration and returns them as an
    /// array.
    /// </summary>
    ///
    /// <param name="strings">
    /// The strings to take from.
    /// </param>
    ///
    /// <param name="maximumTopStrings">
    /// Maximum number of top strings to take.
    /// </param>
    ///
    /// <returns>
    /// An array of the top strings taken from <paramref name="strings" />.
    /// </returns>
    //*************************************************************************

    public static String[]
    TakeTopStringsAsArray
    (
        IEnumerable<string> strings,
        Int32 maximumTopStrings
    )
    {
        Debug.Assert(strings != null);
        Debug.Assert(maximumTopStrings > 0);

        return ( strings.Take(maximumTopStrings).ToArray() );
    }

    //*************************************************************************
    //  Method: CountString()
    //
    /// <summary>
    /// Adds 1 to the count for a string in a dictionary of string counts.
    /// </summary>
    ///
    /// <param name="stringToCount">
    /// The string to count.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="stringCounts">
    /// The key is a string being counted and the value is the number of times
    /// the string was found.
    /// </param>
    //*************************************************************************

    public static void
    CountString
    (
        String stringToCount,
        Dictionary<String, Int32> stringCounts
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(stringToCount) );
        Debug.Assert(stringCounts != null);

        Int32 iStringCount;

        if ( stringCounts.TryGetValue(stringToCount, out iStringCount) )
        {
            iStringCount++;
        }
        else
        {
            iStringCount = 1;
        }

        stringCounts[stringToCount] = iStringCount;
    }

    //*************************************************************************
    //  Method: CountDelimitedStringsInEdgeColumn()
    //
    /// <summary>
    /// Counts the number of times each string was found in a specified edge
    /// column.
    /// </summary>
    ///
    /// <param name="edges">
    /// The edges to calculate the metrics from.
    /// </param>
    ///
    /// <param name="edgeColumnName">
    /// Name of the edge column to calculate the metric from.  The column must
    /// contain space-delimited strings.  Sample: "URLs in Tweet", in which
    /// case the column contains space-delimited URLs that this method counts
    /// and ranks.
    /// </param>
    ///
    /// <returns>
    /// A Dictionary in which the key is a string being counted (an URL in the
    /// tweets, for example), and the value is the number of times the string
    /// was found in the edge column.
    /// </returns>
    //*************************************************************************

    public static Dictionary<String, Int32>
    CountDelimitedStringsInEdgeColumn
    (
        IEnumerable<IEdge> edges,
        String edgeColumnName
    )
    {
        Debug.Assert(edges != null);
        Debug.Assert( !String.IsNullOrEmpty(edgeColumnName) );

        // The key is a string being counted (an URL in a Tweet, for example),
        // and the value is the number of times the string was found in the
        // edge column.

        Dictionary<String, Int32> oStringCounts =
            new Dictionary<String, Int32>();

        foreach (IEdge oEdge in edges)
        {
            String sSpaceDelimitedCellValue;

            if ( oEdge.TryGetNonEmptyStringValue(edgeColumnName,
                out sSpaceDelimitedCellValue) )
            {
                foreach ( String sString in sSpaceDelimitedCellValue.Split(
                    new Char[] {' '}, StringSplitOptions.RemoveEmptyEntries) )
                {
                    CountString(sString, oStringCounts);
                }
            }
        }

        return (oStringCounts);
    }
}

}
