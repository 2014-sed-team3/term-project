﻿
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.Core
{
//*****************************************************************************
//  Interface: IEdge
//
/// <summary>
/// Represents an edge.
/// </summary>
///
/// <remarks>
/// An edge is a connection between two vertices in the same graph.  An edge
/// can be directed or undirected.  A directed edge has a front and a back.
///
/// <para>
/// An edge always connects two vertices.  Although an edge can be created
/// before it is added to a graph, it can't be created without specifying the
/// vertices it connects.  An edge can be added only to the graph that contains
/// the edge's vertices.
/// </para>
///
/// <para>
/// An edge can be created via the edge constructor and then added to a graph
/// via IGraph.Edges.<see cref="EdgeCollection.Add(IEdge)" />, or created and
/// added to a graph at the same time via IGraph.Edges.<see
/// cref="IEdgeCollection.Add(IVertex, IVertex, Boolean)" />.
/// </para>
///
/// <para>
/// An edge is immutable, meaning that its vertices can't be removed.  An edge
/// can be removed from a graph and disposed of, however.
/// </para>
///
/// </remarks>
///
/// <seealso cref="Edge" />
//*****************************************************************************

public interface IEdge : IIdentityProvider, IMetadataProvider
{
    //*************************************************************************
    //  Property: ParentGraph
    //
    /// <summary>
    /// Gets the graph that owns the edge.
    /// </summary>
    ///
    /// <value>
    /// The graph that owns the edge, as an <see cref="IGraph" />.
    /// </value>
    ///
    /// <remarks>
    /// This property is never null.  If the edge hasn't yet been added to a
    /// graph, the parent graph is obtained from the edge's vertices.  Because
    /// an edge can only be added to the graph that contains the edge's
    /// vertices, the vertices' parent graph is always the same as the edge's
    /// parent graph.
    /// </remarks>
    //*************************************************************************

    IGraph
    ParentGraph
    {
        get;
    }

    //*************************************************************************
    //  Property: IsDirected
    //
    /// <summary>
    /// Gets a value indicating whether the edge is directed.
    /// </summary>
    ///
    /// <value>
    /// true if the edge is directed, false if not.
    /// </value>
    ///
    /// <remarks>
    /// A directed edge has a front and a back.
    ///
    /// <para>
    /// This property is set when the edge is created by the edge constructor.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    IsDirected
    {
        get;
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets the vertices to which the edge is connected.
    /// </summary>
    ///
    /// <value>
    /// An array of two vertices.
    /// </value>
    ///
    /// <remarks>
    /// The order of the returned vertices is the same order that was specified
    /// when the edge was created by the edge constructor.
    ///
    /// <para>
    /// The vertices are also available via the <see cref="Vertex1" /> and <see
    /// cref="Vertex2" /> properties.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IsDirected" />
    /// <seealso cref="Vertex1" />
    /// <seealso cref="Vertex2" />
    //*************************************************************************

    IVertex []
    Vertices
    {
        get;
    }

    //*************************************************************************
    //  Property: Vertex1
    //
    /// <summary>
    /// Gets the first vertex to which the edge is connected.
    /// </summary>
    ///
    /// <value>
    /// The edge's first vertex, as an <see cref="IVertex" />.
    /// </value>
    ///
    /// <remarks>
    /// The first vertex is also available as the first vertex in the array
    /// returned by <see cref="Vertices" />.
    ///
    /// <para>
    /// The edge's vertices are set when the edge is created.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IsDirected" />
    /// <seealso cref="Vertices" />
    /// <seealso cref="Vertex2" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    IVertex
    Vertex1
    {
        get;
    }

    //*************************************************************************
    //  Property: Vertex2
    //
    /// <summary>
    /// Gets the second vertex to which the edge is connected.
    /// </summary>
    ///
    /// <value>
    /// The edge's second vertex, as an <see cref="IVertex" />.
    /// </value>
    ///
    /// <remarks>
    /// The second vertex is also available as the second vertex in the array
    /// returned by <see cref="Vertices" />.
    ///
    /// <para>
    /// The edge's vertices are set when the edge is created.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IsDirected" />
    /// <seealso cref="Vertices" />
    /// <seealso cref="Vertex1" />
    //*************************************************************************

    [ DebuggerBrowsable(DebuggerBrowsableState.Never) ]

    IVertex
    Vertex2
    {
        get;
    }

    //*************************************************************************
    //  Property: IsSelfLoop
    //
    /// <summary>
    /// Gets a value indicating whether the edge connects a vertex to itself.
    /// </summary>
    ///
    /// <value>
    /// true if the edge connects a vertex to itself, false if not.
    /// </value>
    //*************************************************************************

    Boolean
    IsSelfLoop
    {
        get;
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <overloads>
    /// Creates a copy of the edge.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a copy of the edge.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
    /// If true, the key/value pairs that were set with <see
    /// cref="IMetadataProvider.SetValue" /> are copied to the new edge.  (This
    /// is a shallow copy.  The objects pointed to by the original values are
    /// NOT cloned.)  If false, the key/value pairs are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
    /// If true, the <see cref="IMetadataProvider.Tag" /> property on the new
    /// edge is set to the same value as in the original edge.  (This is a
    /// shallow copy.  The object pointed to by the original <see
    /// cref="IMetadataProvider.Tag" /> is NOT cloned.)  If false, the <see
    /// cref="IMetadataProvider.Tag "/> property on the new edge is set to
    /// null.
    /// </param>
    ///
    /// <returns>
    /// The copy of the edge, as an <see cref="IEdge" />.
    /// </returns>
    ///
    /// <remarks>
    /// The new edge is connected to the same vertices as the original edge.
    /// Its <see cref="IIdentityProvider.Name" /> is set to the same value as
    /// the original's, but it is assigned a new <see
    /// cref="IIdentityProvider.ID" />.
    ///
    /// <para>
    /// The new edge can be added only to the same graph.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    IEdge
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag
    );

    //*************************************************************************
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a copy of the edge using specified vertices.
    /// </summary>
    ///
    /// <param name="copyMetadataValues">
    /// If true, the key/value pairs that were set with <see
    /// cref="IMetadataProvider.SetValue" /> are copied to the new edge.  (This
    /// is a shallow copy.  The objects pointed to by the original values are
    /// NOT cloned.)  If false, the key/value pairs are not copied.
    /// </param>
    ///
    /// <param name="copyTag">
    /// If true, the <see cref="IMetadataProvider.Tag" /> property on the new
    /// edge is set to the same value as in the original edge.  (This is a
    /// shallow copy.  The object pointed to by the original <see
    /// cref="IMetadataProvider.Tag" /> is NOT cloned.)  If false, the <see
    /// cref="IMetadataProvider.Tag "/> property on the new edge is set to
    /// null.
    /// </param>
    ///
    /// <param name="vertex1">
    /// The new edge's first vertex.  The vertex must be contained in the graph
    /// to which the new edge will be added.
    /// </param>
    ///
    /// <param name="vertex2">
    /// The new edge's second vertex.  The vertex must be contained in the
    /// graph to which the new edge will be added.
    /// </param>
    ///
    /// <param name="isDirected">
    /// If true, <paramref name="vertex1" /> is the new edge's back vertex and
    /// <paramref name="vertex2" /> is the new edge's front vertex.  If false,
    /// the new edge is undirected.
    /// </param>
    ///
    /// <returns>
    /// The copy of the edge, as an <see cref="IEdge" />.
    /// </returns>
    ///
    /// <remarks>
    /// A new edge is created and then connected to <paramref
    /// name="vertex1" /> and <paramref name="vertex2" />, which can be in the
    /// same graph as the original edge or in a different graph.  Its <see
    /// cref="IIdentityProvider.Name" /> is set to the same value as the
    /// original's, but it is assigned a new <see
    /// cref="IIdentityProvider.ID" />.
    ///
    /// <para>
    /// The new edge can be added only to the graph that owns <paramref
    /// name="vertex1" /> and <paramref name="vertex2" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    IEdge
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag,
        IVertex vertex1,
        IVertex vertex2,
        Boolean isDirected
    );

    //*************************************************************************
    //  Method: IsParallelTo()
    //
    /// <summary>
    /// Gets a value indicating whether the edge is parallel to a specified
    /// edge.
    /// </summary>
    ///
    /// <param name="otherEdge">
    /// Edge to test.
    /// </param>
    ///
    /// <returns>
    /// true if the edge is parallel to <paramref name="otherEdge" />, false if
    /// not.
    /// </returns>
    ///
    /// <remarks>
    /// If the edges do not connect the same vertices, they are not parallel or
    /// anti-parallel.  If they do connect the same vertices, they are parallel
    /// or anti-parallel, depending on the directedness of the graph and the
    /// edges.  This is shown in the following table.
    ///
    /// <list type="table">
    ///
    /// <listheader>
    /// <term>Graph Directedness</term>
    /// <term>Edge 1</term>
    /// <term>Edge 2</term>
    /// <term>Edge 1 Parallel to Edge 2?</term>
    /// <term>Edge 1 Anti-Parallel to Edge 2?</term>
    /// </listheader>
    ///
    /// <item>
    /// <term>Directed</term>
    /// <term>(A->B)</term>
    /// <term>(A->B)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// <item>
    /// <term>Directed</term>
    /// <term>(A->B)</term>
    /// <term>(B->A)</term>
    /// <term>No</term>
    /// <term>Yes</term>
    /// </item>
    ///
    /// <item>
    /// <term>Undirected</term>
    /// <term>(A,B)</term>
    /// <term>(A,B)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(A,B)</term>
    /// <term>(A,B)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(A,B)</term>
    /// <term>(A->B)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(A,B)</term>
    /// <term>(B->A)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(A->B)</term>
    /// <term>(A,B)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(A->B)</term>
    /// <term>(A->B)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(A->B)</term>
    /// <term>(B->A)</term>
    /// <term>No</term>
    /// <term>Yes</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(B->A)</term>
    /// <term>(A,B)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(B->A)</term>
    /// <term>(A->B)</term>
    /// <term>No</term>
    /// <term>Yes</term>
    /// </item>
    ///
    /// <item>
    /// <term>Mixed</term>
    /// <term>(B->A)</term>
    /// <term>(B->A)</term>
    /// <term>Yes</term>
    /// <term>No</term>
    /// </item>
    ///
    /// </list>
    ///
    /// </remarks>
    ///
    /// <seealso cref="IsAntiparallelTo" />
    ///
    /// TODO: "A simple graph is one that contains no loops or parallel edges,
    /// where more than one edge connects two given vertices,"
    //*************************************************************************

    Boolean
    IsParallelTo
    (
        IEdge otherEdge
    );

    //*************************************************************************
    //  Method: IsAntiparallelTo()
    //
    /// <summary>
    /// Gets a value indicating whether the edge is antiparallel to a specified
    /// edge.
    /// </summary>
    ///
    /// <param name="otherEdge">
    /// Edge to test.
    /// </param>
    ///
    /// <returns>
    /// true if the edge is antiparallel to <paramref name="otherEdge" />,
    /// false if not.
    /// </returns>
    ///
    /// <remarks>
    /// See <see cref="IsParallelTo" /> for details on the returned value.
    /// </remarks>
    ///
    /// <seealso cref="IsParallelTo" />
    //*************************************************************************

    Boolean
    IsAntiparallelTo
    (
        IEdge otherEdge
    );

    //*************************************************************************
    //  Method: GetAdjacentVertex()
    //
    /// <summary>
    /// Given one of the edge's vertices, returns the other vertex.
    /// </summary>
    ///
    /// <param name="vertex">
    /// One of the edge's vertices.
    /// </param>
    ///
    /// <returns>
    /// The edge's other vertex.
    /// </returns>
    ///
    /// <remarks>
    /// An ArgumentException is thrown if <paramref name="vertex" /> is not one
    /// of the edge's vertices.
    /// </remarks>
    //*************************************************************************

    IVertex
    GetAdjacentVertex
    (
        IVertex vertex
    );
}

}
