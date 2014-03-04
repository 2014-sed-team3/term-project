
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeFilter
//
/// <summary>
/// Filters collections of edges.
/// </summary>
//*****************************************************************************

public static class EdgeFilter
{
    //*************************************************************************
    //  Method: FilterEdgesByImportedID()
    //
    /// <summary>
    /// Filters out multiple edges that have the same imported IDs.
    /// </summary>
    ///
    /// <param name="edges">
    /// The collection of edges to filter.
    /// </param>
    ///
    /// <returns>
    /// An array of edges from <paramref name="edges" /> that have unique
    /// imported ID metadata values.
    /// </returns>
    ///
    /// <remarks>
    /// Edges that don't have imported IDs are included in the returned array.
    /// </remarks>
    //*************************************************************************

    public static IEdge []
    FilterEdgesByImportedID
    (
        IEnumerable<IEdge> edges
    )
    {
        Debug.Assert(edges != null);

        List<IEdge> oFilteredEdges = new List<IEdge>();

        foreach ( IEdge oEdge in EnumerateEdgesByUniqueImportedID(edges) )
        {
            oFilteredEdges.Add(oEdge);
        }

        return ( oFilteredEdges.ToArray() );
    }

    //*************************************************************************
    //  Method: EnumerateEdgesByUniqueImportedID()
    //
    /// <summary>
    /// Enumerates edges that have unique imported IDs.
    /// </summary>
    ///
    /// <param name="edges">
    /// The collection of edges to enumerate.
    /// </param>
    ///
    /// <returns>
    /// The enumerated edges with unique imported IDs are returned one-by-one.
    /// </returns>
    ///
    /// <remarks>
    /// Edges that don't have imported IDs are included in the enumeration.
    /// </remarks>
    //*************************************************************************

    public static IEnumerable<IEdge>
    EnumerateEdgesByUniqueImportedID
    (
        IEnumerable<IEdge> edges
    )
    {
        Debug.Assert(edges != null);

        HashSet<String> oUniqueImportedIDs = new HashSet<String>();

        foreach (IEdge oEdge in edges)
        {
            String sImportedID;

            if (
                oEdge.TryGetNonEmptyStringValue(
                    CommonTableColumnNames.ImportedID, out sImportedID)
                &&
                !oUniqueImportedIDs.Add(sImportedID)
                )
            {
                // The edge has an imported ID that has already been found on
                // another edge.  Skip it.

                continue;
            }

            yield return (oEdge);
        }
    }
}

}
