
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.Core
{
//*****************************************************************************
//  Class: EdgeUtil
//
/// <summary>
/// Utility methods for dealing with <see cref="IEdge" /> objects.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class EdgeUtil
{
    //*************************************************************************
    //  Method: EdgeToVertices()
    //
    /// <summary>
    /// Obtains an edge's two vertices.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge connecting the two vertices.  Can't be null.
    /// </param>
    ///
    /// <param name="className">
    /// Name of the class calling this method.
    /// </param>
    ///
    /// <param name="methodOrPropertyName">
    /// Name of the method or property calling this method.
    /// </param>
    ///
    /// <param name="vertex1">
    /// Where the edge's first vertex gets stored.
    /// </param>
    ///
    /// <param name="vertex2">
    /// Where the edge's second vertex gets stored.
    /// </param>
    ///
    /// <remarks>
    /// This method obtains an edge's two vertices and stores them at
    /// <paramref name="vertex1" /> and <paramref name="vertex2" />.  An
    /// <see cref="ApplicationException" /> is thrown if the vertices can't be
    /// obtained.
    /// </remarks>
    //*************************************************************************

    public static void
    EdgeToVertices
    (
        IEdge edge,
        String className,
        String methodOrPropertyName,
        out IVertex vertex1,
        out IVertex vertex2
    )
    {
        Debug.Assert(edge != null);
        Debug.Assert( !String.IsNullOrEmpty(className) );
        Debug.Assert( !String.IsNullOrEmpty(methodOrPropertyName) );

        String sErrorMessage = null;

        IVertex [] aoVertices = edge.Vertices;

        if (aoVertices == null)
        {
            sErrorMessage = "The edge's Vertices property is null.";
        }
        else if (aoVertices.Length != 2)
        {
            sErrorMessage = "The edge does not connect two vertices.";
        }
        else if (aoVertices[0] == null)
        {
            sErrorMessage = "The edge's first vertex is null.";
        }
        else if (aoVertices[1] == null)
        {
            sErrorMessage = "The edge's second vertex is null.";
        }
        else if (aoVertices[0].ParentGraph == null)
        {
            sErrorMessage =
                "The edge's first vertex does not belong to a graph.";
        }
        else if (aoVertices[1].ParentGraph == null)
        {
            sErrorMessage =
                "The edge's second vertex does not belong to a graph.";
        }
        else if ( aoVertices[0].ParentGraph != aoVertices[1].ParentGraph )
        {
            sErrorMessage =
                "The edge connects vertices not in the same graph.";
        }

        if (sErrorMessage != null)
        {
            Debug.Assert(false);

            throw new ApplicationException( String.Format(

                "{0}.{1}: {2}"
                ,
                className,
                methodOrPropertyName,
                sErrorMessage
                ) );
        }

        vertex1 = aoVertices[0];
        vertex2 = aoVertices[1];
    }

    //*************************************************************************
    //  Method: GetVertexIDPair()
    //
    /// <overloads>
    /// Combines the IDs of an edge's vertices into a pair suitable for use as
    /// a dictionary key.
    /// </overloads>
    ///
    /// <summary>
    /// Combines the IDs of an edge's vertices into a pair suitable for use as
    /// a dictionary key, taking the edge's directedness into account.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to get the vertex ID pair for.
    /// </param>
    ///
    /// <returns>
    /// A vertex ID pair suitable for use as a dictionary key.
    /// </returns>
    ///
    /// <remarks>
    /// In a directed graph, the two edges (A,B) and (A,B) yield the same
    /// vertex ID pair.  The two edges (A,B) and (B,A) do not.
    ///
    /// <para>
    /// In an undirected graph, the two edges (A,B) and (A,B) yield the same
    /// vertex ID pair.  The two edges (A,B) and (B,A) also yield the same
    /// vertex ID pair.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Int64
    GetVertexIDPair
    (
        IEdge edge
    )
    {
        Debug.Assert(edge != null);

        return ( GetVertexIDPair(edge, true) );
    }

    //*************************************************************************
    //  Method: GetVertexIDPair()
    //
    /// <summary>
    /// Combines the IDs of an edge's vertices into a pair suitable for use as
    /// a dictionary key, optionally taking the edge's directedness into
    /// account.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to get the vertex ID pair for.
    /// </param>
    ///
    /// <param name="useDirectedness">
    /// true to take the edge's directedness into account.
    /// </param>
    ///
    /// <returns>
    /// A vertex ID pair suitable for use as a dictionary key.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="useDirectedness" /> is false, the two edges (A,B)
    /// and (A,B) yield the same vertex ID pair.  The two edges (A,B) and (B,A)
    /// also yield the same vertex ID pair.
    ///
    /// <para>
    /// If <paramref name="useDirectedness" /> is true and the graph is
    /// directed, the two edges (A,B) and (A,B) yield the same vertex ID pair.
    /// The two edges (A,B) and (B,A) do not.
    /// </para>
    ///
    /// <para>
    /// If <paramref name="useDirectedness" /> is true and the graph is
    /// undirected, the two edges (A,B) and (A,B) yield the same vertex ID
    /// pair.  The two edges (A,B) and (B,A) also yield the same vertex ID
    /// pair.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Int64
    GetVertexIDPair
    (
        IEdge edge,
        Boolean useDirectedness
    )
    {
        Debug.Assert(edge != null);

        IVertex [] aoVertices = edge.Vertices;

        return ( CollectionUtil.GetDictionaryKey(
            aoVertices[0].ID, aoVertices[1].ID,
            useDirectedness && edge.IsDirected) );
    }

    //*************************************************************************
    //  Method: GetVertexNamePair()
    //
    /// <summary>
    /// Combines the names of an edge's vertices into a name pair suitable for
    /// use as a dictionary key.
    /// </summary>
    ///
    /// <param name="vertexName1">
    /// Name of the edge's first vertex.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="vertexName2">
    /// Name of the edge's second vertex.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="graphIsDirected">
    /// true if the graph is directed, false if it is undirected.
    /// </param>
    ///
    /// <returns>
    /// A name pair suitable for use as a dictionary key.
    /// </returns>
    ///
    /// <remarks>
    /// In a directed graph, the two edges (A,B) and (A,B) yield the same
    /// vertex name pair.  The two edges (A,B) and (B,A) do not.
    ///
    /// <para>
    /// In an undirected graph, the two edges (A,B) and (A,B) yield the same
    /// vertex name pair.  The two edges (A,B) and (B,A) also yield the same
    /// vertex name pair.
    /// </para>
    ///
    /// <para>
    /// This method differs from <see cref="GetVertexIDPair(IEdge)" /> in that
    /// it uses vertex names rather than IDs.  This allows application code to
    /// test whether two edges might be duplicates before the edges and their
    /// vertices are actually added to a graph.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static String
    GetVertexNamePair
    (
        String vertexName1,
        String vertexName2,
        Boolean graphIsDirected
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(vertexName1) );
        Debug.Assert( !String.IsNullOrEmpty(vertexName2) );

        String sVertexNamePair;

        // In the undirected case, guarantee that (A,B) and (B,A) are
        // considered duplicates by always pairing their names in the same
        // order.

        if (graphIsDirected || vertexName1.CompareTo(vertexName2) < 0)
        {
            sVertexNamePair = vertexName1 + VertexNamePairSeparator
                + vertexName2;
        }
        else
        {
            sVertexNamePair = vertexName2 + VertexNamePairSeparator
                + vertexName1;
        }

        return (sVertexNamePair);
    }

    //*************************************************************************
    //  Method: GetEdgeWeightSum()
    //
    /// <summary>
    /// Gets the sum of the edge weights of the edges that connect two
    /// vertices.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// The first vertex.
    /// </param>
    ///
    /// <param name="vertex2">
    /// The second vertex.
    /// </param>
    ///
    /// <returns>
    /// The sum of the edge weights.
    /// </returns>
    ///
    /// <remarks>
    /// It's assumed that duplicate edges have been merged, and that the
    /// <see cref="ReservedMetadataKeys.EdgeWeight" /> key has been set on
    /// each of the graph's edges.
    /// </remarks>
    //*************************************************************************

    public static Double
    GetEdgeWeightSum
    (
        IVertex vertex1,
        IVertex vertex2
    )
    {
        Debug.Assert(vertex1 != null);
        Debug.Assert(vertex2 != null);

        Double dEdgeWeightSum = 0;

        // Get the edges that connect the two vertices.  This includes all
        // connecting edges and does not take directedness into account.

        ICollection<IEdge> oConnectingEdges =
            vertex1.GetConnectingEdges(vertex2);

        Int32 iConnectingEdges = oConnectingEdges.Count;
        IEdge oConnectingEdgeWithEdgeWeight = null;

        switch (vertex1.ParentGraph.Directedness)
        {
            case GraphDirectedness.Directed:

                // There can be 0, 1, or 2 edges between the vertices.  Only
                // one of them can originate at vertex1.

                Debug.Assert(iConnectingEdges <= 2);

                foreach (IEdge oConnectingEdge in oConnectingEdges)
                {
                    if (oConnectingEdge.Vertex1 == vertex1)
                    {
                        oConnectingEdgeWithEdgeWeight = oConnectingEdge;
                        break;
                    }
                }

                break;

            case GraphDirectedness.Undirected:

                // There can be 0 or 1 edges between the vertices.  There can't
                // be 2 edges, because the duplicate edges (A,B) and (B,A) have
                // been merged.

                Debug.Assert(iConnectingEdges <= 1);

                if (iConnectingEdges == 1)
                {
                    oConnectingEdgeWithEdgeWeight = oConnectingEdges.First();
                }

                break;

            default:

                Debug.Assert(false);
                break;
        }

        if (oConnectingEdgeWithEdgeWeight != null)
        {
            dEdgeWeightSum = (Double)
                oConnectingEdgeWithEdgeWeight.GetRequiredValue(
                    ReservedMetadataKeys.EdgeWeight, typeof(Double) );
        }

        return (dEdgeWeightSum);
    }

    //*************************************************************************
    //  Method: TryGetEdgeWeight()
    //
    /// <summary>
    /// Attempts to get the edge weight that might be stored on an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to check.
    /// </param>
    ///
    /// <param name="edgeWeight">
    /// Where the edge weight gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the edge weight was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If the edge has the <see cref="ReservedMetadataKeys.EdgeWeight" /> key,
    /// this method stores the key's value at <paramref name="edgeWeight" />
    /// and returns true.  Otherwise, false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetEdgeWeight
    (
        IEdge edge,
        out Double edgeWeight
    )
    {
        Debug.Assert(edge != null);

        Object oEdgeWeightAsObject;

        if ( edge.TryGetValue(ReservedMetadataKeys.EdgeWeight,
            typeof(Double), out oEdgeWeightAsObject) )
        {
            edgeWeight = (Double)oEdgeWeightAsObject;
            return (true);
        }

        edgeWeight = Double.MinValue;
        return (false);
    }

    //*************************************************************************
    //  Method: GetPositiveEdgeWeight()
    //
    /// <summary>
    /// Gets an edge's weight, ignoring non-positive values.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to check.
    /// </param>
    ///
    /// <returns>
    /// The edge's weight.  Always greater than zero.
    /// </returns>
    ///
    /// <remarks>
    /// If the edge has the <see cref="ReservedMetadataKeys.EdgeWeight" /> key
    /// and its value is positive, this method returns the value.  If it has
    /// the key and the value is not positive, 1.0 is returned.  If it doesn't
    /// have the key, 1.0 is returned.
    /// </remarks>
    //*************************************************************************

    public static Double
    GetPositiveEdgeWeight
    (
        IEdge edge
    )
    {
        Debug.Assert(edge != null);

        Double dEdgeWeight;

        if ( !EdgeUtil.TryGetEdgeWeight(edge, out dEdgeWeight) ||
            dEdgeWeight <= 0)
        {
            dEdgeWeight = 1.0;
        }

        return (dEdgeWeight);
    }

    //*************************************************************************
    //  Method: TryGetIntermediateCurvePoints
    //
    /// <summary>
    /// Attempts to get the intermediate curve points that might be stored on
    /// an edge.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to check.
    /// </param>
    ///
    /// <param name="intermediateCurvePoints">
    /// Where an array of intermediate curve points gets stored if true is
    /// returned.  The array may be empty, which is not an error.
    /// </param>
    ///
    /// <returns>
    /// true if the intermediate curve points were obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If the edge has the <see
    /// cref="ReservedMetadataKeys.PerEdgeIntermediateCurvePoints" /> key, this
    /// method stores the key's value at <paramref
    /// name="intermediateCurvePoints" /> and returns true.  Otherwise, false
    /// is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetIntermediateCurvePoints
    (
        IEdge edge,
        out System.Drawing.PointF [] intermediateCurvePoints
    )
    {
        Debug.Assert(edge != null);

        Object oIntermediateCurvePointsAsObject;

        if ( edge.TryGetValue(
            ReservedMetadataKeys.PerEdgeIntermediateCurvePoints,
            typeof( System.Drawing.PointF[] ),
            out oIntermediateCurvePointsAsObject) )
        {
            intermediateCurvePoints = ( System.Drawing.PointF[] )
                oIntermediateCurvePointsAsObject;

            return (true);
        }

        intermediateCurvePoints = null;
        return (false);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <remarks>
    /// The separator character used by <see cref="GetVertexNamePair" />.
    ///
    /// <para>
    /// This is a vertical tab, which is highly unlikely to be used in a vertex
    /// name.
    /// </para>
    ///
    /// </remarks>

    public const Char VertexNamePairSeparator = '\v';
}

}
