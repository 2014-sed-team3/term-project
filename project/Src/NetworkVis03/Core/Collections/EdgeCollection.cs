﻿
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.Core
{
//*****************************************************************************
//  Class: EdgeCollection
//
/// <summary>
/// Represents a collection of edges.
/// </summary>
///
/// <remarks>
/// This is a collection of objects that implement the <see cref="IEdge" />
/// interface.  You can add edges to the collection, remove them, access an
/// edge, and enumerate all edges.
/// </remarks>
//
//  Implementation notes:
//
//  All edges are stored in a single LinkedList.  Edges incident to a given
//  vertex are contiguous in the list.  The group of edges incident to a given
//  vertex is terminated with a LinkedListNode that has a null edge.  This
//  separates the group of incident edges for one vertex from the group for the
//  next vertex.
//
//  Here are the edges in the LinkedList for two vertices A, and B, for
//  example:
//  
//      VertexA-Edge43 VertexA-Edge27 VertexA-null VertexB-Edge96 VertexB-null
//
//  Each of the graph's non-loop edges is stored twice in the LinkedList --
//  once in the group for the edge's first vertex, and again in the group for
//  the edge's second vertex.  A self-loop edge is stored once only.
//
//  Each Vertex instance maintains a FirstIncidentEdgeNode field of type
//  LinkedListNode<IEdge> that points to the first LinkedList node in the
//  Vertex's group of incident edges.  If the field is null, the Vertex has no
//  incident edges.
//*****************************************************************************

public partial class EdgeCollection : NodeXLBase, IEdgeCollection
{
    //*************************************************************************
    //  Constructor: EdgeCollection()
    //
    /// <summary>
    /// Initializes a new instance of the EdgeCollection class.
    /// </summary>
    ///
    /// <param name="graph">
    /// <see cref="IGraph" /> to which the new collection belongs.  Can't be
    /// null.
    /// </param>
    //*************************************************************************

    public EdgeCollection
    (
        IGraph graph
    )
    {
        this.ArgumentChecker.CheckArgumentNotNull(
            "Constructor", "graph", graph);

        m_oParentGraph = graph;

        m_oLinkedList = new LinkedList<IEdge>();

        m_iEdges = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Count
    //
    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    ///
    /// <value>
    /// The number of elements contained in the collection.
    /// </value>
    ///
    /// <remarks>
    /// This method is an O(1) operation.
    /// </remarks>
    //*************************************************************************

    public Int32
    Count
    {
        get
        {
            AssertValid();

            return (m_iEdges);
        }
    }

    //*************************************************************************
    //  Property: IsReadOnly
    //
    /// <summary>
    /// Gets a value indicating whether the collection is read-only.
    /// </summary>
    ///
    /// <value>
    /// true if the collection is read-only.
    /// </value>
    ///
    /// <remarks>
    /// This method is an O(1) operation.
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsReadOnly
    {
        get
        {
            AssertValid();

            return (false);
        }
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <overloads>
    /// Adds an edge to the collection.
    /// </overloads>
    ///
    /// <summary>
    /// Adds an existing edge to the collection.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to add to the collection.
    /// </param>
    ///
    /// <remarks>
    /// An exception is thrown if <paramref name="edge" /> is already in this
    /// collection or in another edge collection.
    ///
    /// <para>
    /// An exception is thrown if the directedness of <paramref name="edge" />
    /// is incompatible with the <see cref="IGraph.Directedness" /> property on
    /// the graph that owns this edge collection.
    /// </para>
    /// 
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    Add
    (
        IEdge edge
    )
    {
        AssertValid();

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;
        const String MethodName = "Add";
        const String ArgumentName = "edge";

        // Get the edge's two Vertex objects.

        Vertex oVertex1, oVertex2;

        EdgeToVertices(
            edge, MethodName, ArgumentName, out oVertex1, out oVertex2);

        // Check whether the vertices are contained in the graph that owns this
        // edge collection.

        CheckVertex(oVertex1, MethodName, ArgumentName);
        CheckVertex(oVertex2, MethodName, ArgumentName);

        // Check whether the directedness of the edge is compatible with this
        // graph.

        CheckDirectednessBeforeAddingEdge(edge.IsDirected, "edge");

        // Add the edge to both vertices' groups of incident edges, unless the
        // edge is a self-loop (an edge that connects a vertex to itself).

        AddToGroup(edge, oVertex1);

        if (!edge.IsSelfLoop)
        {
            AddToGroup(edge, oVertex2);
        }

        OnEdgeAdded(edge);
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Creates an edge and adds it to the collection.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// The edge's first vertex.  The vertex must be contained in the graph
    /// that owns this edge collection.
    /// </param>
    ///
    /// <param name="vertex2">
    /// The edge's second vertex.  The vertex must be contained in the graph
    /// that owns this edge collection.
    /// </param>
    ///
    /// <param name="isDirected">
    /// If true, <paramref name="vertex1" /> is the edge's back vertex and
    /// <paramref name="vertex2" /> is the edge's front vertex.  If false, the
    /// edge is undirected.
    /// </param>
    ///
    /// <returns>
    /// The added edge.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates an edge, connects it to the specified vertices, and
    /// adds the edge to the collection.
    ///
    /// <para>
    /// An exception is thrown if <paramref name="isDirected" /> is
    /// incompatible with the <see cref="IGraph.Directedness" /> property on
    /// the graph that owns this edge collection.
    /// </para>
    ///
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public IEdge
    Add
    (
        IVertex vertex1,
        IVertex vertex2,
        Boolean isDirected
    )
    {
        AssertValid();

        const String MethodName = "Add";

        // Check whether the vertices are contained in the graph that owns this
        // edge collection.  Although this is done again by the Add(IEdge)
        // overload, it should be done here as well.  If not done here and
        // one of the vertices is null or doesn't belong to the parent graph,
        // the Edge constructor will throw an exception and the exact
        // source of the problem won't be obvious.

        CheckVertex(vertex1, MethodName, "vertex1");
        CheckVertex(vertex2, MethodName, "vertex2");

        IEdge oEdge = new Edge(vertex1, vertex2, isDirected);
        this.Add(oEdge);

        return (oEdge);
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Creates an undirected edge and adds it to the collection.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// The edge's first vertex.  The vertex must be contained in the graph
    /// that owns this edge collection.
    /// </param>
    ///
    /// <param name="vertex2">
    /// The edge's second vertex.  The vertex must be contained in the graph
    /// that owns this edge collection.
    /// </param>
    ///
    /// <returns>
    /// The new undirected edge.
    /// </returns>
    ///
    /// <remarks>
    /// This method creates an undirected edge, connects it to the specified
    /// vertices, and adds the edge to the collection.
    ///
    /// <para>
    /// An exception is thrown if the graph that owns this edge collection has
    /// a <see cref="IGraph.Directedness" /> value of <see
    /// cref="GraphDirectedness.Directed" />.
    /// </para>
    ///
    /// <para>
    /// This method is an O(1) operation.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public IEdge
    Add
    (
        IVertex vertex1,
        IVertex vertex2
    )
    {
        AssertValid();

        return ( Add(vertex1, vertex2, false) );
    }

    //*************************************************************************
    //  Method: Clear()
    //
    /// <summary>
    /// Removes all edges from the collection.
    /// </summary>
    ///
    /// <remarks>
    /// Do not call this method if you are using NodeXLControl.  To clear the
    /// graph within NodeXLControl, use NodeXLControl.ClearGraph().
    ///
    /// <para>
    /// This method is an O(n) operation, where n is the number of vertices in
    /// the parent graph.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    Clear()
    {
        AssertValid();

        const String MethodName = "Clear";

        // Remove all edges from the LinkedList.

        m_oLinkedList.Clear();
        m_iEdges = 0;

        // Loop through the parent graph's vertices.

        foreach (IVertex oVertex in m_oParentGraph.Vertices)
        {
            // Tell the Vertex that it no longer has any incident edges.

            Vertex oVertex2 = IVertexToVertex(oVertex, MethodName);

            oVertex2.FirstIncidentEdgeNode = null;
        }
    }

    //*************************************************************************
    //  Method: RemoveDuplicates()
    //
    /// <summary>
    /// Removes duplicate edges from the collection.
    /// </summary>
    ///
    /// <remarks>
    /// If the graph is directed, the edges (A,B) and (A,B) are duplicates, but
    /// the edges (A,B) and (B,A) are not.
    ///
    /// <para>
    /// If the graph is undirected, the edges (A,B) and (A,B) are duplicates,
    /// and so are the edges (A,B) and (B,A).
    /// </para>
    ///
    /// <para>
    /// It is not possible to control or predict which of the duplicate edges
    /// will be removed.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n * e) operation, where n is the number of vertices
    /// in the parent graph and e is the number of edges.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    RemoveDuplicates()
    {
        AssertValid();

        HashSet<Int64> oUniqueVertexIDPairs = new HashSet<Int64>();

        foreach (IVertex oVertex in m_oParentGraph.Vertices)
        {
            oUniqueVertexIDPairs.Clear();

            foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
            {
                if ( !oUniqueVertexIDPairs.Add(
                    EdgeUtil.GetVertexIDPair(oIncidentEdge) ) )
                {
                    Remove(oIncidentEdge);
                }
            }
        }
    }

    //*************************************************************************
    //  Method: Contains()
    //
    /// <overloads>
    /// Determines whether the collection contains a specified edge.
    /// </overloads>
    ///
    /// <summary>
    /// Determines whether the collection contains an edge specified by
    /// reference.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to search for.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains <paramref name="edge" />.
    /// </returns>
    ///
    /// <remarks>
    /// This method is an O(n) operation, where n is the number of edges
    /// incident to <paramref name="edge" />'s first vertex.
    /// </remarks>
    //*************************************************************************

    public Boolean
    Contains
    (
        IEdge edge
    )
    {
        AssertValid();

        const String MethodName = "Contains";
        const String ArgumentName = "edge";

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNull(MethodName, ArgumentName, edge);

        // Get the edge's vertices.

        Vertex oVertex1, oVertex2;

        EdgeToVertices(
            edge, MethodName, ArgumentName, out oVertex1, out oVertex2);

        Debug.Assert(oVertex1.ParentGraph == oVertex2.ParentGraph);

        if (oVertex1.ParentGraph != m_oParentGraph)
        {
            // The edge's vertices don't belong to the parent graph, so it
            // can't belong to this collection.

            return (false);
        }

        // Arbitrarily select the first vertex and search its group of incident
        // edges.  If the group does not contain the edge, the edge is not
        // contained in this collection.

        for (
            LinkedListNode<IEdge> oNode = oVertex1.FirstIncidentEdgeNode;
            oNode != null && oNode.Value != null;
            oNode = oNode.Next
            )
        {
            if (oNode.Value == edge)
            {
                // The edge was found.

                return (true);
            }
        }

        // The edge was not found.

        return (false);
    }

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether the collection contains an edge specified by <see
    /// cref="IIdentityProvider.ID" />
    /// </summary>
    ///
    /// <param name="id">
    /// The ID to search for.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains an edge with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.
    /// </returns>
    ///
    /// <remarks>
    /// IDs are unique among all edges, so there can be only one edge with the
    /// specified ID.
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Contains
    (
        Int32 id
    )
    {
        AssertValid();

        IEdge oEdge;

        return ( Find(id, out oEdge) );
    }

    //*************************************************************************
    //  Method: Contains()
    //
    /// <summary>
    /// Determines whether the collection contains an edge specified by <see
    /// cref="IIdentityProvider.Name" />
    /// </summary>
    ///
    /// <param name="name">
    /// The name to search for.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains an edge with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.
    /// </returns>
    ///
    /// <remarks>
    /// Names do not have to be unique, so there could be more than one edge
    /// with the same name.
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Contains
    (
        String name
    )
    {
        AssertValid();

        const String MethodName = "Contains";
        const String ArgumentName = "name";

        this.ArgumentChecker.CheckArgumentNotEmpty(
            MethodName, ArgumentName, name);

        IEdge oEdge;

        return ( Find(name, out oEdge) );
    }

    //*************************************************************************
    //  Method: CopyTo()
    //
    /// <summary>
    /// Copies the elements of the collection to an array, starting at a
    /// particular array index.
    /// </summary>
    ///
    /// <param name="array">
    /// The one-dimensional array that is the destination of the elements
    /// copied from the collection.  The array must have zero-based indexing. 
    /// </param>
    ///
    /// <param name="index">
    /// The zero-based index in <paramref name="array" /> at which copying
    /// begins. 
    /// </param>
    ///
    /// <remarks>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </remarks>
    //*************************************************************************

    public void
    CopyTo
    (
        IEdge [] array,
        Int32 index
    )
    {
        AssertValid();

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;
        const String MethodName = "CopyTo";
        const String ArrayArgumentName = "array";
        const String IndexArgumentName = "index";

        oArgumentChecker.CheckArgumentNotNull(
            MethodName, ArrayArgumentName, array);

        oArgumentChecker.CheckArgumentNotNegative(
            MethodName, IndexArgumentName, index);

        if (array.Length - index < this.Count)
        {
            oArgumentChecker.ThrowArgumentException(MethodName,
                ArrayArgumentName,

                "The array is not large enough to hold the copied collection."
                );
        }

        foreach (IEdge oEdge in this)
        {
            Debug.Assert(index < array.Length);

            array.SetValue(oEdge, index);

            index++;
        }
    }

    //*************************************************************************
    //  Method: Find()
    //
    /// <overloads>
    /// Searches for a specified edge.
    /// </overloads>
    ///
    /// <summary>
    /// Searches for an edge with the specified <see
    /// cref="IIdentityProvider.ID" />.
    /// </summary>
    ///
    /// <param name="id">
    /// The <see cref="IIdentityProvider.ID" /> of the edge to search for.
    /// </param>
    ///
    /// <param name="edge">
    /// Gets set to the specified <see cref="IEdge" /> if true is returned,
    /// or to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if an edge with the <see cref="IIdentityProvider.ID" /> <paramref
    /// name="id" /> is found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for an edge with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.  If such an
    /// edge is found, it gets stored at <paramref name="edge" /> and true is
    /// returned.  Otherwise, <paramref name="edge" /> gets set to null and
    /// false is returned.
    ///
    /// <para>
    /// IDs are unique among all edges, so there can be only one edge with the
    /// specified ID.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="Contains(Int32)" /> if you want to determine whether
    /// such an edge exists in the collection but you don't need the actual
    /// edge.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Find
    (
        Int32 id,
        out IEdge edge
    )
    {
        AssertValid();

        return ( Find(true, id, null, out edge) );
    }

    //*************************************************************************
    //  Method: Find()
    //
    /// <summary>
    /// Searches for the first edge with the specified <see
    /// cref="IIdentityProvider.Name" />.
    /// </summary>
    ///
    /// <param name="name">
    /// The <see cref="IIdentityProvider.Name" /> of the edge to search for.
    /// Can't be null or empty.
    /// </param>
    ///
    /// <param name="edge">
    /// Gets set to the specified <see cref="IEdge" /> if true is returned, or
    /// to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if an edge with the <see cref="IIdentityProvider.Name" />
    /// <paramref name="name" /> is found, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the first edge with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.  If such
    /// an edge is found, it gets stored at <paramref name="edge" /> and true
    /// is returned.  Otherwise, <paramref name="edge" /> gets set to null and
    /// false is returned.
    ///
    /// <para>
    /// Names do not have to be unique, so there could be more than one edge
    /// with the same name.
    /// </para>
    ///
    /// <para>
    /// Use <see cref="Contains(String)" /> if you want to determine whether
    /// such an edge exists in the collection but you don't need the actual
    /// edge.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Find
    (
        String name,
        out IEdge edge
    )
    {
        AssertValid();

        const String MethodName = "Find";
        const String ArgumentName = "name";

        this.ArgumentChecker.CheckArgumentNotEmpty(
            MethodName, ArgumentName, name);

        return ( Find(false, -1, name, out edge) );
    }

    //*************************************************************************
    //  Method: GetConnectingEdges()
    //
    /// <summary>
    /// Gets a collection of edges that connect two specified vertices.
    /// </summary>
    ///
    /// <param name="vertex1">
    /// First vertex.  Must belong to the parent graph.
    /// </param>
    ///
    /// <param name="vertex2">
    /// Second vertex.  Must belong to the parent graph.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more edges that connect <paramref name="vertex1" />
    /// to <paramref name="vertex2" />, as a collection of <see cref="IEdge" />
    /// objects.
    /// </returns>
    ///
    /// <remarks>
    /// This method returns a collection of all edges that connect <paramref
    /// name="vertex1" /> to <paramref name="vertex2" />.  The directedness of
    /// the edges is not considered.
    ///
    /// <para>
    /// If there are no such edges, the returned array is empty.  The returned
    /// value is never null.
    /// </para>
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is returned in
    /// the array only if <paramref name="vertex1" /> and <paramref
    /// name="vertex2" /> are the same vertex.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is the number of edges
    /// incident to <paramref name="vertex1" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public ICollection<IEdge>
    GetConnectingEdges
    (
        IVertex vertex1,
        IVertex vertex2
    )
    {
        AssertValid();

        const String MethodName = "GetConnectingEdges";

        CheckVertex(vertex1, MethodName, "vertex1");

        CheckVertex(vertex2, MethodName, "vertex2");

        // Cast the IVertex interfaces to Vertex, checking the types in the
        // process.

        Vertex oVertex1 = IVertexToVertex(vertex1, MethodName);
        Vertex oVertex2 = IVertexToVertex(vertex2, MethodName);

        List<IEdge> oConnectingEdges = new List<IEdge>();

        Boolean bVerticesAreTheSame = (oVertex1 == oVertex2);

        // Loop through oVertex1's group of incident edges.

        for (
            LinkedListNode<IEdge> oNode = oVertex1.FirstIncidentEdgeNode;
            oNode != null && oNode.Value != null;
            oNode = oNode.Next
            )
        {
            IEdge oEdge = oNode.Value;

            if (bVerticesAreTheSame)
            {
                if (!oEdge.IsSelfLoop)
                {
                    // A self-loop should be returned only if the caller asked
                    // for it by passing the same vertex twice.

                    continue;
                }
            }

            // Get the edge's vertices.

            IVertex oEdgeVertex1, oEdgeVertex2;

            EdgeUtil.EdgeToVertices(oEdge, this.ClassName, MethodName,
                out oEdgeVertex1, out oEdgeVertex2);

            Debug.Assert(oEdgeVertex1 == oVertex1 || oEdgeVertex2 == oVertex1);

            if (oEdgeVertex1 == oVertex2 || oEdgeVertex2 == oVertex2)
            {
                oConnectingEdges.Add(oEdge);
            }
        }

        return (oConnectingEdges);
    }

    //*************************************************************************
    //  Method: GetEnumerator()
    //
    /// <summary>
    /// Returns an enumerator that iterates through the collection. 
    /// </summary>
    ///
    /// <returns>
    /// An enumerator object that can be used to iterate through the
    /// collection. 
    /// </returns>
    //*************************************************************************

    public IEnumerator<IEdge>
    GetEnumerator()
    {
        AssertValid();

        // Create a HashSet to keep track of the edge IDs that have already
        // been enumerated.  When an edge is added to EdgeCollection, it gets
        // added to the incident edge groups for both of the edge's vertices.
        // If every edge were enumerated, the collection would appear to
        // contain twice as many edges as actually exist.

        HashSet<Int32> oEnumeratedEdgeIDs = new HashSet<Int32>();

        LinkedListNode<IEdge> oCurrentNode = m_oLinkedList.First;

        while (oCurrentNode != null)
        {
            if (oCurrentNode.Value != null)
            {
                // The current node is not the null terminator for a vertex's
                // group of incident edges.  (Such a node must be skipped.)

                // Has the edge already been enumerated?

                Int32 iID = oCurrentNode.Value.ID;

                if ( !oEnumeratedEdgeIDs.Contains(iID) )
                {
                    // No.  To prevent it from being enumerated a second time,
                    // add the ID to the HashSet.

                    oEnumeratedEdgeIDs.Add(iID);

                    yield return oCurrentNode.Value;
                }
            }

            oCurrentNode = oCurrentNode.Next;
        }
    }

    //*************************************************************************
    //  Method: GetEnumerator()
    //
    /// <summary>
    /// Returns an enumerator that iterates through the collection. 
    /// </summary>
    ///
    /// <returns>
    /// An enumerator object that can be used to iterate through the
    /// collection. 
    /// </returns>
    //*************************************************************************

    System.Collections.IEnumerator
    System.Collections.IEnumerable.GetEnumerator()
    {
        AssertValid();

        return ( GetEnumerator() );
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <overloads>
    /// Removes an edge from the collection.
    /// </overloads>
    ///
    /// <summary>
    /// Removes an edge specified by reference from the collection.
    /// </summary>
    ///
    /// <param name="edge">
    /// The edge to remove from the collection.
    /// </param>
    ///
    /// <returns>
    /// true if the edge was removed, false if the edge wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for <paramref name="edge" />.  If
    /// found, it is removed from the collection and true is returned.  false
    /// is returned otherwise.
    ///
    /// <para>
    /// The edge is unusable once it is removed from the collection.
    /// Attempting to access the edge's properties or methods will lead to
    /// unpredictable results.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is the number of edges
    /// incident to the edge's vertices.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Remove
    (
        IEdge edge
    )
    {
        AssertValid();

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;
        const String MethodName = "Remove";
        const String ArgumentName = "edge";

        oArgumentChecker.CheckArgumentNotNull(MethodName, ArgumentName, edge);

        // Get the edge's two Vertex objects.

        Vertex oVertex1, oVertex2;

        EdgeToVertices(
            edge, MethodName, ArgumentName, out oVertex1, out oVertex2);

        Debug.Assert(oVertex1.ParentGraph == oVertex2.ParentGraph);

        if (oVertex1.ParentGraph != m_oParentGraph)
        {
            // The edge's vertices don't belong to this graph, so the edge
            // can't belong to this graph either.

            return (false);
        }

        // Attempt to remove the edge from the first vertex's group of incident
        // edges.

        if ( !RemoveFromGroup(edge, oVertex1) )
        {
            // The edge does not belong to this collection.  (An edge
            // connecting the two vertices was created but wasn't added to the
            // collection.)

            return (false);
        }

        // Remove the edge from the second vertex's group of incident edges.

        if ( !edge.IsSelfLoop && !RemoveFromGroup(edge, oVertex2) )
        {
            // This should never occur.

            Debug.Assert(false);

            throw new ApplicationException( String.Format(

                "{0}.{1}: An edge was found in its first vertex's group of"
                + " incident edges, but not in the second vertex's group."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        OnEdgeRemoved(edge, true);

        return (true);
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes an edge specified by <see cref="IIdentityProvider.ID" /> from
    /// the collection.
    /// </summary>
    ///
    /// <param name="id">
    /// The ID of the edge to remove.
    /// </param>
    ///
    /// <returns>
    /// true if the edge was removed, false if the edge wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for an edge with the <see
    /// cref="IIdentityProvider.ID" /> <paramref name="id" />.  If found, it
    /// is removed from the collection and true is returned.  false is returned
    /// otherwise.
    ///
    /// <para>
    /// The edge is unusable once it is removed from the collection.
    /// Attempting to access the edge's properties or methods will lead to
    /// unpredictable results.
    /// </para>
    ///
    /// <para>
    /// IDs are unique among all edges, so there can be only one vertex with
    /// the specified ID.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Remove
    (
        Int32 id
    )
    {
        AssertValid();

        const String MethodName = "Remove";

        IEdge oEdge;

        if ( !this.Find(id, out oEdge) )
        {
            // The edge is not in the collection.

            return (false);
        }

        if ( !Remove(oEdge) )
        {
            // This should never occur.

            Debug.Assert(false);

            throw new ApplicationException(String.Format(
                "{0}.{1}: An edge was found but couldn't be removed."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: Remove()
    //
    /// <summary>
    /// Removes an edge specified by <see cref="IIdentityProvider.Name" />
    /// from the collection.
    /// </summary>
    ///
    /// <param name="name">
    /// The name of the edge to remove.  Can't be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the edge was removed, false if the edge wasn't found in the
    /// collection.
    /// </returns>
    ///
    /// <remarks>
    /// This method searches the collection for the first edge with the <see
    /// cref="IIdentityProvider.Name" /> <paramref name="name" />.  If
    /// found, it is removed from the collection and true is returned.  false
    /// is returned otherwise.
    ///
    /// <para>
    /// The edge is unusable once it is removed from the collection.
    /// Attempting to access the edge's properties or methods will lead to
    /// unpredictable results.
    /// </para>
    ///
    /// <para>
    /// Names do not have to be unique, so there could be more than one edge
    /// with the same name.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is <see cref="Count" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    Remove
    (
        String name
    )
    {
        AssertValid();

        const String MethodName = "Remove";
        const String ArgumentName = "name";

        this.ArgumentChecker.CheckArgumentNotEmpty(
            MethodName, ArgumentName, name);

        AssertValid();

        IEdge oEdge;

        if ( !this.Find(name, out oEdge) )
        {
            // The edge is not in the collection.

            return (false);
        }

        if ( !Remove(oEdge) )
        {
            // This should never occur.

            Debug.Assert(false);

            throw new ApplicationException(String.Format(
                "{0}.{1}: An edge was found but couldn't be removed."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Formats the value of the current instance.
    /// </summary>
    ///
    /// <returns>
    /// The formatted string.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        Int32 iEdges = this.Count;

        return ( iEdges.ToString(NodeXLBase.Int32Format) + 
            ( (iEdges == 1) ? " edge" : " edges" ) );

    }

    //*************************************************************************
    //  Event: EdgeAdded
    //
    /// <summary>
    /// Occurs when an edge is added to the collection.
    /// </summary>
    //*************************************************************************

    public event EdgeEventHandler
    EdgeAdded;


    //*************************************************************************
    //  Event: EdgeRemoved
    //
    /// <summary>
    /// Occurs when an edge is removed from the collection.
    /// </summary>
    //*************************************************************************

    public event EdgeEventHandler
    EdgeRemoved;


    //*************************************************************************
    //  Method: GetIncomingOrOutgoingEdges()
    //
    /// <summary>
    /// Gets a collection of a vertex's incoming or outgoing edges.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// <see cref="Vertex" /> to get edges for.
    /// </param>
    ///
    /// <param name="bIncludeIncoming">
    /// true to include the vertex's incoming edges.
    /// </param>
    ///
    /// <param name="bIncludeOutgoing">
    /// true to include the vertex's outgoing edges.
    /// </param>
    ///
    /// <returns>
    /// A collection of the vertex's zero or more incoming or outgoing edges
    /// (or both), as a collection of <see cref="IEdge" /> objects.
    /// </returns>
    ///
    /// <remarks>
    /// An incoming edge is either a directed edge that has the vertex at its
    /// front, or an undirected edge connected to the vertex.
    ///
    /// <para>
    /// An outgoing edge is either a directed edge that has the vertex at its
    /// back, or an undirected edge connected to the vertex.
    /// </para>
    ///
    /// <para>
    /// If there are no such edges, the returned collection is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is the number of edges
    /// incident to <paramref name="oVertex" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected internal ICollection<IEdge>
    GetIncomingOrOutgoingEdges
    (
        Vertex oVertex,
        Boolean bIncludeIncoming,
        Boolean bIncludeOutgoing
    )
    {
        AssertValid();
        Debug.Assert(oVertex != null);

        const String MethodName = "GetIncomingOrOutgoingEdges";

        LinkedList<IEdge> oEdges = new LinkedList<IEdge>();

        // Loop through the vertex's group of incident edges.

        for (
            LinkedListNode<IEdge> oNode = oVertex.FirstIncidentEdgeNode;
            oNode != null && oNode.Value != null;
            oNode = oNode.Next
            )
        {
            // Get the edge and its vertices.

            IEdge oEdge = oNode.Value;

            IVertex oVertex1, oVertex2;

            EdgeUtil.EdgeToVertices(oEdge, this.ClassName, MethodName,
                out oVertex1, out oVertex2);

            Debug.Assert(oVertex1 == oVertex as IVertex ||
                oVertex2 == oVertex as IVertex);

            // Determine whether the edge should be included in the returned
            // array.

            Boolean bIncludeEdge = false;

            if (oEdge.IsDirected)
            {
                if (bIncludeIncoming)
                {
                    // oVertex2 is the front vertex.

                    if (oVertex2 == oVertex)
                    {
                        bIncludeEdge = true;
                    }
                }

                if (bIncludeOutgoing)
                {
                    // oVertex1 is the back vertex.

                    if (oVertex1 == oVertex)
                    {
                        bIncludeEdge = true;
                    }
                }
            }
            else
            {
                // The edge is undirected.

                bIncludeEdge = true;
            }

            if (bIncludeEdge)
            {
                oEdges.AddLast(oEdge);
            }
        }

        return (oEdges);
    }

    //*************************************************************************
    //  Method: GetPredecessorOrSuccessorVertices()
    //
    /// <summary>
    /// Gets a collection of a vertex's predecessor or successor vertices, or
    /// both.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// <see cref="Vertex" /> to get predecessor or successor vertices for.
    /// </param>
    ///
    /// <param name="bIncludePredecessor">
    /// true to include the vertex's predecessor vertices.
    /// </param>
    ///
    /// <param name="bIncludeSuccessor">
    /// true to include the vertex's successor vertices.
    /// </param>
    ///
    /// <returns>
    /// A collection of the vertex's zero or more predecessor or successor
    /// vertices, as a collection of <see cref="IVertex" /> objects.
    /// </returns>
    ///
    /// <remarks>
    /// A predecessor vertex is a vertex at the other side of an incoming edge.
    /// (An incoming edge is either a directed edge that has this vertex at its
    /// front, or an undirected edge connected to this vertex.)
    ///
    /// <para>
    /// A successor vertex is a vertex at the other side of an outgoing edge.
    /// (An outgoing edge is either a directed edge that has this vertex at its
    /// back, or an undirected edge connected to this vertex.)
    /// </para>
    ///
    /// <para>
    /// If there are no such vertices, the returned collection is empty.  The
    /// returned value is never null.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is the number of edges
    /// incident to <paramref name="oVertex" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected internal ICollection<IVertex>
    GetPredecessorOrSuccessorVertices
    (
        Vertex oVertex,
        Boolean bIncludePredecessor,
        Boolean bIncludeSuccessor
    )
    {
        AssertValid();
        Debug.Assert(oVertex != null);

        const String MethodName = "GetPredecessorOrSuccessorVertices";

        // If two edges connect this vertex to another vertex, the other vertex
        // should not be counted twice.  To prevent this, use a dictionary to
        // collect the vertices and check the dictionary before adding a vertex
        // to it.
        //
        // The dictionary key is the vertex ID and the value is the vertex.

        Dictionary<Int32, IVertex> oPredecessorOrSuccessorVertices =
            new Dictionary<Int32, IVertex>();

        // Loop through the vertex's group of incident edges.

        for (
            LinkedListNode<IEdge> oNode = oVertex.FirstIncidentEdgeNode;
            oNode != null && oNode.Value != null;
            oNode = oNode.Next
            )
        {
            IEdge oEdge = oNode.Value;

            // Get the edge's vertices.

            IVertex oVertex1, oVertex2;

            EdgeUtil.EdgeToVertices(oEdge, this.ClassName, MethodName,
                out oVertex1, out oVertex2);

            Debug.Assert(oVertex1 == oVertex || oVertex2 == oVertex);

            // Determine whether the adjacent vertex should be included in the
            // returned collection.

            IVertex oAdjacentVertex = null;

            if (oEdge.IsDirected)
            {
                // oVertex1 is the back vertex.  oVertex2 is the front vertex.

                if (bIncludePredecessor)
                {
                    if (oVertex2 == oVertex)
                    {
                        oAdjacentVertex = oVertex1;
                    }
                }

                if (bIncludeSuccessor)
                {
                    // oVertex1 is the back vertex.

                    if (oVertex1 == oVertex)
                    {
                        oAdjacentVertex = oVertex2;
                    }
                }
            }
            else
            {
                // The edge is undirected.

                if (oVertex1 == oVertex)
                {
                    oAdjacentVertex = oVertex2;
                }
                else
                {
                    Debug.Assert(oVertex2 == oVertex);

                    oAdjacentVertex = oVertex1;
                }
            }

            // If there is an adjacent vertex, make sure it hasn't already been
            // added to the dictionary.

            if ( oAdjacentVertex != null &&

                !oPredecessorOrSuccessorVertices.ContainsKey(
                    oAdjacentVertex.ID)
                )
            {
                oPredecessorOrSuccessorVertices.Add(
                    oAdjacentVertex.ID, oAdjacentVertex);
            }
        }

        // Convert the dictionary values to an array.

        Dictionary<Int32, IVertex>.ValueCollection oValueCollection =
            oPredecessorOrSuccessorVertices.Values;

        Int32 iValues = oValueCollection.Count;

        IVertex [] aoPredecessorOrSuccessorVertices = new IVertex[iValues];

        oValueCollection.CopyTo(aoPredecessorOrSuccessorVertices, 0);

        oPredecessorOrSuccessorVertices.Clear();

        return (aoPredecessorOrSuccessorVertices);
    }

    //*************************************************************************
    //  Method: RemoveAllFromGroup()
    //
    /// <summary>
    /// Removes all edges from a vertex's group of incident edges.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// <see cref="Vertex" /> to remove all incident edges from.
    /// </param>
    ///
    /// <remarks>
    /// Each edge incident to the vertex is stored twice, once in the vertex's
    /// group of incident edges and once in the adjacent vertex's group of
    /// incident edges.  This method removes all such edges, including the
    /// duplicated ones.
    /// </remarks>
    //*************************************************************************

    protected internal void
    RemoveAllFromGroup
    (
        Vertex oVertex
    )
    {
        AssertValid();
        Debug.Assert(oVertex != null);

        String sClassName = this.ClassName;
        const String MethodName = "RemoveAllFromGroup";

        // Loop through the vertex's group of incident edges.

        LinkedListNode<IEdge> oNode = oVertex.FirstIncidentEdgeNode;

        while (oNode != null)
        {
            IEdge oEdge = oNode.Value;

            if (oEdge == null)
            {
                // The node is the null terminator.  Remove it.

                m_oLinkedList.Remove(oNode);

                break;
            }

            // Get the edge's adjacent vertices.

            Vertex oVertex1, oVertex2;

            Edge.EdgeToVertices(
                oEdge, sClassName, MethodName, out oVertex1, out oVertex2);

            Debug.Assert(oVertex1 == oVertex || oVertex2 == oVertex);

            // Figure out which of the adjacent vertices is not oVertex.

            Vertex oAdjacentVertex = null;

            if (oVertex1 == oVertex)
            {
                oAdjacentVertex = oVertex2;
            }   
            else if (oVertex2 == oVertex)
            {
                oAdjacentVertex = oVertex1;
            }
            else
            {
                // This should never occur.

                Debug.Assert(false);

                throw new ApplicationException( String.Format(

                    "{0}.{1}: An incident edge does not connect to the"
                    + " specified vertex."
                    ,
                    sClassName,
                    MethodName
                    ) );
            }

            // Remove the edge from the adjacent Vertex's group of incident
            // edges.

            if ( !oEdge.IsSelfLoop &&
                !RemoveFromGroup(oEdge, oAdjacentVertex) )
            {
                // This should never occur.

                Debug.Assert(false);

                throw new ApplicationException( String.Format(

                    "{0}.{1}: An incident edge could not be removed from an"
                    + " adjacent vertex's group of incident edges."
                    ,
                    sClassName,
                    MethodName
                    ) );
            }

            // Remove the edge from oVertex's group of incident edges.

            LinkedListNode<IEdge> oNodeToRemove = oNode;

            oNode = oNode.Next;

            m_oLinkedList.Remove(oNodeToRemove);

            OnEdgeRemoved(oEdge, false);
        }

        // The vertex no longer has any incident edges.

        oVertex.FirstIncidentEdgeNode = null;

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetDegree()
    //
    /// <summary>
    /// Gets a vertex's degree.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// <see cref="Vertex" /> to get the degree for.
    /// </param>
    ///
    /// <returns>
    /// The vertex's degree.
    /// </returns>
    ///
    /// <remarks>
    /// The degree of a vertex is the number of edges that are incident to it.
    /// (An incident edge is an edge that is connected to this vertex.)
    ///
    /// <para>
    /// A self-loop (an edge that connects a vertex to itself) is considered
    /// one incident edge.
    /// </para>
    ///
    /// <para>
    /// This method is an O(n) operation, where n is the number of edges
    /// incident to <paramref name="oVertex" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected internal Int32
    GetDegree
    (
        Vertex oVertex
    )
    {
        AssertValid();
        Debug.Assert(oVertex != null);

        // Loop through the vertex's group of incident edges.

        Int32 iDegree = 0;

        for (
            LinkedListNode<IEdge> oNode = oVertex.FirstIncidentEdgeNode;
            oNode != null && oNode.Value != null;
            oNode = oNode.Next
            )
        {
            iDegree++;
        }

        return (iDegree);
    }

    //*************************************************************************
    //  Method: AddToGroup()
    //
    /// <summary>
    /// Adds an edge to a vertex's group of incident edges.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to add to the group.
    /// </param>
    ///
    /// <param name="oVertex">
    /// The vertex to add the edge to.
    /// </param>
    //*************************************************************************

    protected void
    AddToGroup
    (
        IEdge oEdge,
        Vertex oVertex
    )
    {
        AssertValid();
        Debug.Assert(oEdge != null);
        Debug.Assert(oVertex != null);

        // Get the vertex's first incident edge.

        LinkedListNode<IEdge> oFirstIncidentEdgeNode =
            oVertex.FirstIncidentEdgeNode;

        // Create a node for the new edge.

        LinkedListNode<IEdge> oNewNode = new LinkedListNode<IEdge>(oEdge);

        if (oFirstIncidentEdgeNode != null)
        {
            // Add the new node before the vertex's first node.

            m_oLinkedList.AddBefore(oFirstIncidentEdgeNode, oNewNode);
        }
        else
        {
            // The vertex has no nodes.  Create a group for the vertex.  The
            // group consists of the new node followed by a null terminator.

            m_oLinkedList.AddFirst( new LinkedListNode<IEdge>(null) );

            m_oLinkedList.AddFirst(oNewNode);
        }

        // The new node is now the first node for the vertex.

        oVertex.FirstIncidentEdgeNode = oNewNode;
    }

    //*************************************************************************
    //  Method: RemoveFromGroup()
    //
    /// <summary>
    /// Removes an edge from a vertex's group of incident edges.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to remove from the group.
    /// </param>
    ///
    /// <param name="oVertex">
    /// The vertex whose group the edge should be removed from.
    /// </param>
    ///
    /// <returns>
    /// true if the edge was removed, false if the edge wasn't found in the
    /// vertex's group.
    /// </returns>
    //*************************************************************************

    protected Boolean
    RemoveFromGroup
    (
        IEdge oEdge,
        Vertex oVertex
    )
    {
        AssertValid();
        Debug.Assert(oEdge != null);
        Debug.Assert(oVertex != null);

        LinkedListNode<IEdge> oNodeToRemove = null;

        // Loop through the vertex's group of incident edges.

        LinkedListNode<IEdge> oOldFirstIncidentEdgeNode =
            oVertex.FirstIncidentEdgeNode;

        for (
            LinkedListNode<IEdge> oNode = oOldFirstIncidentEdgeNode;
            oNode != null && oNode.Value != null;
            oNode = oNode.Next
            )
        {
            if (oNode.Value == oEdge)
            {
                oNodeToRemove = oNode;

                break;
            }
        }

        if (oNodeToRemove == null)
        {
            // The edge wasn't found.

            return (false);
        }

        LinkedListNode<IEdge> oPreviousNode = oNodeToRemove.Previous;
        LinkedListNode<IEdge> oNextNode = oNodeToRemove.Next;

        Debug.Assert(oNextNode != null);

        // Remove the node.

        m_oLinkedList.Remove(oNodeToRemove);

        if (oOldFirstIncidentEdgeNode != oNodeToRemove)
        {
            // Nothing more needs to be done.

            return (true);
        }

        // The vertex's first node was removed, so its FirstIncidentEdgeNode
        // property needs to be set with a new first node.

        LinkedListNode<IEdge> oNewFirstIncidentEdgeNode;

        if (oNextNode.Value != null)
        {
            // The next node belongs to the vertex's group of incident edges
            // and is not the group's null terminator.

            oNewFirstIncidentEdgeNode = oNextNode;
        }
        else
        {
            // The next node is the null terminator for the vertex's group of
            // incident edges, which means that the vertex has no more incident
            // edges.  Remove the null terminator.

            m_oLinkedList.Remove(oNextNode);

            oNewFirstIncidentEdgeNode = null;
        }

        oVertex.FirstIncidentEdgeNode = oNewFirstIncidentEdgeNode;

        return (true);
    }

    //*************************************************************************
    //  Method: Find()
    //
    /// <summary>
    /// Searches for an edge with the specified <see
    /// cref="IIdentityProvider.ID" /> or <see
    /// cref="IIdentityProvider.Name" />.
    /// </summary>
    ///
    /// <param name="bByID">
    /// true to search by ID, false to search by name.
    /// </param>
    ///
    /// <param name="iID">
    /// The <see cref="IIdentityProvider.ID" /> of the edge to search for if
    /// <paramref name="bByID" /> is true.
    /// </param>
    ///
    /// <param name="sName">
    /// The <see cref="IIdentityProvider.Name" /> of the edge to search for if
    /// <paramref name="bByID" /> is false.  Can't be null or empty if
    /// <paramref name="bByID" /> is false.
    /// </param>
    ///
    /// <param name="oEdge">
    /// Gets set to the specified <see cref="IEdge" /> if true is returned,
    /// or to null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified edge is found, false if not.
    /// </returns>
    //*************************************************************************

    protected Boolean
    Find
    (
        Boolean bByID,
        Int32 iID,
        String sName,
        out IEdge oEdge
    )
    {
        AssertValid();
        Debug.Assert( bByID || !String.IsNullOrEmpty(sName) );

        oEdge = null;

        foreach (IEdge oEdgeInCollection in this)
        {
            if (bByID)
            {
                if (oEdgeInCollection.ID == iID)
                {
                    oEdge = oEdgeInCollection;

                    return (true);
                }
            }
            else
            {
                if (oEdgeInCollection.Name == sName)
                {
                    oEdge = oEdgeInCollection;

                    return (true);
                }
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: OnEdgeAdded()
    //
    /// <summary>
    /// Gets called when an edge is added to the collection.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The added edge.
    /// </param>
    //*************************************************************************

    protected void
    OnEdgeAdded
    (
        IEdge oEdge
    )
    {
        AssertValid();
        Debug.Assert(oEdge != null);

        const String MethodName = "OnEdgeAdded";

        m_iEdges++;

        if (m_oLinkedList.Count == 0)
        {
            // This should never occur.

            Debug.Assert(false);

            throw new ApplicationException(String.Format(
                "{0}.{1}: The LinkedList is empty."
                ,
                this.ClassName,
                MethodName
                ) );
        }

        // Fire an EdgeAdded event if necessary.

        EdgeEventHandler oEdgeAdded = this.EdgeAdded;

        if (oEdgeAdded != null)
        {
            oEdgeAdded( this, new EdgeEventArgs(oEdge) );
        }
    }

    //*************************************************************************
    //  Method: OnEdgeRemoved()
    //
    /// <summary>
    /// Gets called when an edge is removed from the collection.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The removed edge.
    /// </param>
    ///
    /// <param name="bFireEdgeRemoved">
    /// true to fire the <see cref="EdgeRemoved" /> event.
    /// </param>
    //*************************************************************************

    protected void
    OnEdgeRemoved
    (
        IEdge oEdge,
        Boolean bFireEdgeRemoved
    )
    {
        Debug.Assert(oEdge != null);
        AssertValid();

        m_iEdges--;

        // Fire an EdgeRemoved event if necessary.

        if (bFireEdgeRemoved)
        {
            EdgeEventHandler oEdgeRemoved = this.EdgeRemoved;

            if (oEdgeRemoved != null)
            {
                oEdgeRemoved( this, new EdgeEventArgs(oEdge) );
            }
        }
    }

    //*************************************************************************
    //  Method: EdgeToVertices()
    //
    /// <summary>
    /// Obtains an edge's two <see cref="Vertex" /> objects.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge connecting the two <see cref="Vertex" /> objects.
    /// </param>
    ///
    /// <param name="sMethodName">
    /// Name of the method calling this method.
    /// </param>
    ///
    /// <param name="sArgumentName">
    /// Name of the edge argument.
    /// </param>
    ///
    /// <param name="oVertex1">
    /// Where the edge's first <see cref="Vertex" /> gets stored.
    /// </param>
    ///
    /// <param name="oVertex2">
    /// Where the edge's second <see cref="Vertex" /> gets stored.
    /// </param>
    ///
    /// <remarks>
    /// This method obtains an edge's two <see cref="Vertex" /> objects and
    /// stores them at <paramref name="oVertex1" /> and <paramref
    /// name="oVertex2" />.  An exception is thrown if the edge is null or the
    /// edge's vertices are null or of the incorrect type.
    /// </remarks>
    //*************************************************************************

    protected void
    EdgeToVertices
    (
        IEdge oEdge,
        String sMethodName,
        String sArgumentName,
        out Vertex oVertex1,
        out Vertex oVertex2
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(sMethodName) );
        Debug.Assert( !String.IsNullOrEmpty(sArgumentName) );

        oVertex1 = null;
        oVertex2 = null;

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        // Check for null.

        oArgumentChecker.CheckArgumentNotNull(
            sMethodName, sArgumentName, oEdge);

        try
        {
            // Get the edge's vertices as IVertex interfaces.

            IVertex oVertexA = null, oVertexB = null;

            EdgeUtil.EdgeToVertices(oEdge, this.ClassName, sMethodName,
                out oVertexA, out oVertexB);

            // Cast the IVertex interfaces to Vertex, checking the types in the
            // process.

            oVertex1 = IVertexToVertex(oVertexA, sMethodName);
            oVertex2 = IVertexToVertex(oVertexB, sMethodName);
        }
        catch (ApplicationException oApplicationException)
        {
            // Change to an ArgumentException so the caller knows the source
            // of the problem.

            oArgumentChecker.ThrowArgumentException(sMethodName, sArgumentName,

                "The edge is invalid.",

                oApplicationException
                );
        }
    }

    //*************************************************************************
    //  Method: IVertexToVertex
    //
    /// <summary>
    /// Casts an <see cref="IVertex" /> to a <see cref="Vertex" /> object.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// The <see cref="IVertex" /> to cast to a <see cref="Vertex" /> object.
    /// </param>
    ///
    /// <param name="sMethodOrPropertyName">
    /// Name of the method or property calling this method.
    /// </param>
    ///
    /// <returns>
    /// The <see cref="Vertex" /> object.
    /// </returns>
    ///
    /// <remarks>
    /// An exception is thrown if <paramref name="oVertex" /> is null or not of
    /// type <see cref="Vertex" />.
    /// </remarks>
    //*************************************************************************

    protected Vertex
    IVertexToVertex
    (
        IVertex oVertex,
        String sMethodOrPropertyName
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(sMethodOrPropertyName) );

        return ( Vertex.IVertexToVertex(
            oVertex, this.ClassName, sMethodOrPropertyName) );
    }

    //*************************************************************************
    //  Method: CheckDirectednessBeforeAddingEdge()
    //
    /// <summary>
    /// Checks whether the directedness of an edge is compatible with the graph
    /// that owns this edge collection.
    /// </summary>
    ///
    /// <param name="bEdgeIsDirected">
    /// true if the edge is directed, false if undirected.
    /// </param>
    ///
    /// <param name="sArgumentName">
    /// Name of the argument that determines the directedness of the edge.
    /// </param>
    ///
    /// <remarks>
    /// This is meant to be called from the <see cref="Add(IEdge)" /> methods.
    /// An exception is thrown if <paramref name="bEdgeIsDirected" /> is
    /// incompatible with the <see cref="IGraph.Directedness" /> property on
    /// the graph that owns this edge collection.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckDirectednessBeforeAddingEdge
    (
        Boolean bEdgeIsDirected,
        String sArgumentName
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(sArgumentName) );

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;
        const String MethodName = "Add";

        switch (m_oParentGraph.Directedness)
        {
            case GraphDirectedness.Directed:

                if (!bEdgeIsDirected)
                {
                    oArgumentChecker.ThrowArgumentException(MethodName,
                        sArgumentName,

                        "An undirected edge can't be added to a directed"
                        + " graph."
                        );
                }

                break;

            case GraphDirectedness.Undirected:

                if (bEdgeIsDirected)
                {
                    oArgumentChecker.ThrowArgumentException(MethodName,
                        sArgumentName,

                        "A directed edge can't be added to an undirected graph."
                        );
                }

                break;

            case GraphDirectedness.Mixed:

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: CheckVertex()
    //
    /// <summary>
    /// Checks whether a vertex is contained in the graph that owns this edge
    /// collection.
    /// </summary>
    ///
    /// <param name="oVertex">
    /// Vertex to check.
    /// </param>
    ///
    /// <param name="sMethodName">
    /// Name of the method calling this method.
    /// </param>
    ///
    /// <param name="sArgumentName">
    /// Name of the argument to use in error messages.
    /// </param>
    ///
    /// <remarks>
    /// An exception is thrown if <paramref name="oVertex" /> is null or is not
    /// contained in the graph that owns this edge collection.
    /// </remarks>
    //*************************************************************************

    protected void
    CheckVertex
    (
        IVertex oVertex,
        String sMethodName,
        String sArgumentName
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(sMethodName) );
        Debug.Assert( !String.IsNullOrEmpty(sArgumentName) );

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        // Check for null.

        oArgumentChecker.CheckArgumentNotNull(
            sMethodName, sArgumentName, oVertex);

        // Check whether the vertex belongs to the parent graph.

        if (oVertex.ParentGraph != m_oParentGraph)
        {
            oArgumentChecker.ThrowArgumentException(sMethodName, sArgumentName,

                "One of the vertices is not contained in this graph."
                );
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

        Debug.Assert(m_oParentGraph != null);
        Debug.Assert(m_oLinkedList != null);
        Debug.Assert(m_iEdges >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph that owns this collection.

    protected IGraph m_oParentGraph;

    /// LinkedList of IEdge interfaces.

    protected LinkedList<IEdge> m_oLinkedList;

    /// Number of unique edges in m_oLinkedList.

    protected Int32 m_iEdges;
}
}
