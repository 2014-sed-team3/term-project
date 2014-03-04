﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Common;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="Vertex" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexTest : Object
{
    //*************************************************************************
    //  Constructor: VertexTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexTest()
    {
        m_oVertex = null;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        m_oVertex = new Vertex();
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        m_oVertex = null;
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
        Assert.IsNotNull(m_oVertex.AdjacentVertices);
        Assert.AreEqual(0, m_oVertex.AdjacentVertices.Count);

        Assert.AreEqual(0, m_oVertex.Degree);

        Assert.IsNotNull(m_oVertex.IncidentEdges);
        Assert.AreEqual(0, m_oVertex.IncidentEdges.Count);

        Assert.IsNotNull(m_oVertex.IncomingEdges);
        Assert.AreEqual(0, m_oVertex.IncomingEdges.Count);

        Assert.AreEqual(PointF.Empty, m_oVertex.Location);

        Assert.IsNull(m_oVertex.Name);

        Assert.IsNotNull(m_oVertex.OutgoingEdges);
        Assert.AreEqual(0, m_oVertex.OutgoingEdges.Count);

        Assert.IsNull(m_oVertex.ParentGraph);

        Assert.IsNotNull(m_oVertex.PredecessorVertices);
        Assert.AreEqual(0, m_oVertex.PredecessorVertices.Count);

        Assert.IsNotNull(m_oVertex.SuccessorVertices);
        Assert.AreEqual(0, m_oVertex.SuccessorVertices.Count);

        Assert.IsNull(m_oVertex.Tag);
    }

    //*************************************************************************
    //  Method: TestName()
    //
    /// <summary>
    /// Tests the Name property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestName()
    {
        const String Name = null;

        m_oVertex.Name = Name;

        Assert.AreEqual(Name, m_oVertex.Name);
    }

    //*************************************************************************
    //  Method: TestName2()
    //
    /// <summary>
    /// Tests the Name property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestName2()
    {
        String Name = String.Empty;

        m_oVertex.Name = Name;

        Assert.AreEqual(Name, m_oVertex.Name);
    }

    //*************************************************************************
    //  Method: TestName3()
    //
    /// <summary>
    /// Tests the Name property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestName3()
    {
        const String Name = " jfkd jkreui2 rfdjk*";

        m_oVertex.Name = Name;

        Assert.AreEqual(Name, m_oVertex.Name);
    }

    //*************************************************************************
    //  Method: TestLocation()
    //
    /// <summary>
    /// Tests the Location property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLocation()
    {
        Single fX = 1234.23F;
        Single fY = 9876.77F;

        m_oVertex.Location = new PointF(fX, fY);

        Assert.AreEqual(fX, m_oVertex.Location.X);
        Assert.AreEqual(fY, m_oVertex.Location.Y);
    }

    //*************************************************************************
    //  Method: TestLocation2()
    //
    /// <summary>
    /// Tests the Location property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLocation2()
    {
        Single fX = -1234.123F;
        Single fY = -9876.009F;

        m_oVertex.Location = new PointF(fX, fY);

        Assert.AreEqual(fX, m_oVertex.Location.X);
        Assert.AreEqual(fY, m_oVertex.Location.Y);
    }

    //*************************************************************************
    //  Method: TestParentGraph()
    //
    /// <summary>
    /// Tests the ParentGraph property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParentGraph()
    {
        // Add the vertex to a graph.

        IGraph oGraph = new Graph();

        oGraph.Vertices.Add(m_oVertex);

        Assert.AreEqual(oGraph, m_oVertex.ParentGraph);
    }

    //*************************************************************************
    //  Method: TestParentGraph2()
    //
    /// <summary>
    /// Tests the ParentGraph property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestParentGraph2()
    {
        // Create a graph, populate the graph with vertices and edges, remove
        // the vertex from the graph.

        const Int32 Vertices = 100;

        // Create a graph with each vertex connected to all others.

        IGraph oGraph = new Graph();

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, Vertices);

        TestGraphUtil.MakeGraphComplete(oGraph, aoVertices, false);

        // Pick one of the vertices to test.

        IVertex oVertex = aoVertices[0];

        Assert.AreEqual(oGraph, oVertex.ParentGraph);

        // Remove the vertex.

        oGraph.Vertices.Remove(oVertex);

        Assert.IsNull(oVertex.ParentGraph);
    }

    //*************************************************************************
    //  Method: TestIncomingEdges()
    //
    /// <summary>
    /// Tests the IncomingEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncomingEdges()
    {
        TestIncomingOrOutgoingEdges(true);
    }

    //*************************************************************************
    //  Method: TestIncomingEdges2()
    //
    /// <summary>
    /// Tests the IncomingEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncomingEdges2()
    {
        // Verify that a self-loop is handled correctly.

        TestLoopEdges(true, false, false);
    }

    //*************************************************************************
    //  Method: TestOutgoingEdges()
    //
    /// <summary>
    /// Tests the OutgoingEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOutgoingEdges()
    {
        TestIncomingOrOutgoingEdges(false);
    }

    //*************************************************************************
    //  Method: TestOutgoingEdges2()
    //
    /// <summary>
    /// Tests the OutgoingEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOutgoingEdges2()
    {
        // Verify that a self-loop is handled correctly.

        TestLoopEdges(false, true, false);
    }

    //*************************************************************************
    //  Method: TestIncidentEdges()
    //
    /// <summary>
    /// Tests the IncidentEdges and Degree properties.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncidentEdges()
    {
        // Create a graph and populate it with a random mix of incoming and
        // outgoing edges.

        IGraph oGraph = CreateAndPopulateGraph(100);

        // Loop through the graph's vertices.

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            // Loop through the vertex's incident edges.

            ICollection<IEdge> oIncidentEdges = oVertex.IncidentEdges;

            Assert.IsNotNull(oIncidentEdges);

            Assert.AreEqual(oIncidentEdges.Count, oVertex.Degree);

            HashSet<IEdge> oIncidentEdgesHashSet = new HashSet<IEdge>(
                oIncidentEdges);

            foreach (IEdge oEdge in oIncidentEdges)
            {
                // Verify that the edge is incident to the vertex.

                IVertex [] aoVertices = oEdge.Vertices;

                Assert.IsTrue(
                    aoVertices[0] == oVertex || aoVertices[1] == oVertex);

                // Verify that the edge is in the graph.

                Assert.IsTrue( oGraph.Edges.Contains(oEdge) );
            }

            // Loop through the graph's edges.

            foreach (IEdge oEdge in oGraph.Edges)
            {
                // If the edge is incident to the vertex, verify that it's
                // included in the vertex's incident edges.

                IVertex [] aoVertices = oEdge.Vertices;

                if (aoVertices[0] == oVertex || aoVertices[1] == oVertex)
                {
                    Assert.IsTrue( oIncidentEdgesHashSet.Contains(oEdge) );
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TestIncidentEdges2()
    //
    /// <summary>
    /// Tests the IncidentEdges and Degree properties.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncidentEdges2()
    {
        // Verify that the incoming edges are the union of the incoming and
        // outgoing edges.

        // Create a graph and populate it with a random mix of incoming and
        // outgoing edges.

        IGraph oGraph = CreateAndPopulateGraph(987);

        // Loop through the graph's vertices.

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            // Get the vertex's incoming, outgoing, and incident edges.

            ICollection<IEdge> oIncomingEdges = oVertex.IncomingEdges;
            ICollection<IEdge> oOutgoingEdges = oVertex.OutgoingEdges;
            ICollection<IEdge> oIncidentEdges = oVertex.IncidentEdges;

            Assert.IsTrue(oIncomingEdges.Count +
                oOutgoingEdges.Count >= oIncidentEdges.Count);

            HashSet<IEdge> oIncidentEdgesHashSet = new HashSet<IEdge>(
                oIncidentEdges);

            // Verify that each incoming edge is an incident edge.

            foreach (IEdge oEdge in oIncomingEdges)
            {
                Assert.IsTrue( oIncidentEdgesHashSet.Contains(oEdge) );
            }

            // Verify that each outgoing edge is an incident edge.

            foreach (IEdge oEdge in oOutgoingEdges)
            {
                Assert.IsTrue( oIncidentEdgesHashSet.Contains(oEdge) );
            }

            // Verify that each incident edge is either an incoming edge or an
            // outgoing edge.

            HashSet<IEdge> oIncomingEdgesHashSet = new HashSet<IEdge>(
                oIncomingEdges);

            HashSet<IEdge> oOutgoingEdgesHashSet = new HashSet<IEdge>(
                oOutgoingEdges);

            foreach (IEdge oEdge in oIncidentEdges)
            {
                Assert.IsTrue(
                    oIncomingEdgesHashSet.Contains(oEdge)
                    ||
                    oOutgoingEdgesHashSet.Contains(oEdge)
                    );
            }

            Assert.AreEqual(oIncidentEdges.Count, oVertex.Degree);
        }
    }

    //*************************************************************************
    //  Method: TestIncidentEdges3()
    //
    /// <summary>
    /// Tests the IncidentEdges property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIncidentEdges3()
    {
        // Verify that a self-loop is handled correctly.

        TestLoopEdges(false, false, true);
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdges()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetConnectingEdges()
    {
        // Create a graph and populate it with a random mix of incoming and
        // outgoing edges.

        IGraph oGraph = CreateAndPopulateGraph(25);

        // Loop through the graph's vertices.

        foreach (IVertex oVertex1 in oGraph.Vertices)
        {
            // Loop through all vertices except oVertex1.

            foreach (IVertex oVertex2 in oGraph.Vertices)
            {
                if (oVertex2 == oVertex1)
                {
                    continue;
                }

                // Get oVertex1's connecting edges.

                ICollection<IEdge> oConnectingEdges =
                    oVertex1.GetConnectingEdges(oVertex2);

                Assert.IsNotNull(oConnectingEdges);

                foreach (IEdge oConnectingEdge in oConnectingEdges)
                {
                    // Verify that the edge connects the two vertices.

                    IVertex [] aoVertices = oConnectingEdge.Vertices;

                    Assert.IsTrue(
                        (aoVertices[0] == oVertex1 &&
                            aoVertices[1] == oVertex2)
                        ||
                        (aoVertices[1] == oVertex1 &&
                            aoVertices[0] == oVertex2)
                        );

                    // Verify that the edge is in the graph.

                    Assert.IsTrue( oGraph.Edges.Contains(oConnectingEdge) );
                }

                HashSet<IEdge> oConnectingEdgesHashSet = new HashSet<IEdge>(
                    oConnectingEdges);

                // Loop through the graph's edges.

                foreach (IEdge oEdge in oGraph.Edges)
                {
                    // If the edge connects the two vertices, verify that it's
                    // included in aoConnectingEdges.

                    IVertex [] aoVertices = oEdge.Vertices;

                    if (
                        (aoVertices[0] == oVertex1 &&
                            aoVertices[1] == oVertex2)
                        ||
                        (aoVertices[1] == oVertex1 &&
                            aoVertices[0] == oVertex2)
                        )
                    {
                        Assert.IsTrue(
                            oConnectingEdgesHashSet.Contains(oEdge) );
                    }
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdges2()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetConnectingEdges2()
    {
        // Create a graph and populate it with multiple self-loops.

        IGraph oGraph = new Graph();

        // Add two vertices.

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, 2);

        IVertex oVertex = aoVertices[0];

        IEdgeCollection oEdgeCollection = oGraph.Edges;

        // Add multiple self-loops, both undirected and directed.

        IEdge oUndirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, false);
        IEdge oUndirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, false);

        IEdge oDirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, true);
        IEdge oDirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, true);

        // Add a non-self-loop.

        IEdge oNonSelfLoopEdge = oEdgeCollection.Add(
            oVertex, aoVertices[1], false);

        // If GetConnectingEdges() is passed its own vertex, it should return
        // self-loops.

        ICollection<IEdge> oConnectingEdges =
            oVertex.GetConnectingEdges(oVertex);

        Assert.IsNotNull(oConnectingEdges);

        Assert.AreEqual(4, oConnectingEdges.Count);

        // If GetConnectingEdges() is not passed its own vertex, it should not
        // return self-loops.

        oConnectingEdges = oVertex.GetConnectingEdges( aoVertices[1] );

        Assert.IsNotNull(oConnectingEdges);

        Assert.AreEqual(1, oConnectingEdges.Count);

        Assert.AreEqual( oNonSelfLoopEdge, oConnectingEdges.First() );
    }

    //*************************************************************************
    //  Method: TestGetConnectingEdgesBad()
    //
    /// <summary>
    /// Tests the GetConnectingEdges() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestGetConnectingEdgesBad()
    {
        // null otherVertex.

        try
        {
            IGraph oGraph = new Graph();

            IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, 1);

            ICollection<IEdge> oConnectingEdges =
                aoVertices[0].GetConnectingEdges(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Smrf.NodeXL.Core."
                + "Vertex.GetConnectingEdges: otherVertex argument can't be"
                + " null.\r\n"
                + "Parameter name: otherVertex"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestIsIncidentEdge()
    //
    /// <summary>
    /// Tests the IsIncidentEdge(), IsOutgoingEdge(), and IsIncomingEdge()
    /// methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsIncidentEdge()
    {
        IGraph oGraph = new Graph();

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, 4);

        IVertex oVertex0 = aoVertices[0];
        IVertex oVertex1 = aoVertices[1];
        IVertex oVertex2 = aoVertices[2];
        IVertex oVertex3 = aoVertices[3];

        IEdgeCollection oEdges = oGraph.Edges;

        IEdge oUndirectedEdge = oEdges.Add(oVertex0, oVertex1, false);

        Assert.IsTrue( oVertex0.IsIncidentEdge(oUndirectedEdge) );
        Assert.IsTrue( oVertex0.IsOutgoingEdge(oUndirectedEdge) );
        Assert.IsTrue( oVertex0.IsIncomingEdge(oUndirectedEdge) );

        IEdge oIncomingEdge = oEdges.Add(oVertex2, oVertex0, true);

        Assert.IsTrue( oVertex0.IsIncidentEdge(oIncomingEdge) );
        Assert.IsFalse( oVertex0.IsOutgoingEdge(oIncomingEdge) );
        Assert.IsTrue( oVertex0.IsIncomingEdge(oIncomingEdge) );

        IEdge oOutgoingEdge = oEdges.Add(oVertex0, oVertex3, true);

        Assert.IsTrue( oVertex0.IsIncidentEdge(oOutgoingEdge) );
        Assert.IsTrue( oVertex0.IsOutgoingEdge(oOutgoingEdge) );
        Assert.IsFalse( oVertex0.IsIncomingEdge(oOutgoingEdge) );

        IEdge oUndirectedSelfLoop = oEdges.Add(oVertex0, oVertex0, false);

        Assert.IsTrue( oVertex0.IsIncidentEdge(oUndirectedSelfLoop) );
        Assert.IsTrue( oVertex0.IsOutgoingEdge(oUndirectedSelfLoop) );
        Assert.IsTrue( oVertex0.IsIncomingEdge(oUndirectedSelfLoop) );

        IEdge oDirectedSelfLoop = oEdges.Add(oVertex0, oVertex0, true);

        Assert.IsTrue( oVertex0.IsIncidentEdge(oDirectedSelfLoop) );
        Assert.IsTrue( oVertex0.IsOutgoingEdge(oDirectedSelfLoop) );
        Assert.IsTrue( oVertex0.IsIncomingEdge(oDirectedSelfLoop) );

        IEdge oDisconnectedEdge = oEdges.Add(oVertex2, oVertex3, false);

        Assert.IsFalse( oVertex0.IsIncidentEdge(oDisconnectedEdge) );
        Assert.IsFalse( oVertex0.IsOutgoingEdge(oDisconnectedEdge) );
        Assert.IsFalse( oVertex0.IsIncomingEdge(oDisconnectedEdge) );
    }

    //*************************************************************************
    //  Method: TestIsIncidentEdgeBad()
    //
    /// <summary>
    /// Tests the IsIncidentEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestIsIncidentEdgeBad()
    {
        // null edge.

        try
        {
            IGraph oGraph = new Graph();

            IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, 1);

            aoVertices[0].IsIncidentEdge(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Smrf.NodeXL.Core."
                + "Vertex.IsIncidentEdge: edge argument can't be null.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestIsOutgoingEdgeBad()
    //
    /// <summary>
    /// Tests the IsOutgoingEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestIsOutgoingEdgeBad()
    {
        // null edge.

        try
        {
            IGraph oGraph = new Graph();

            IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, 1);

            aoVertices[0].IsOutgoingEdge(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Smrf.NodeXL.Core."
                + "Vertex.IsOutgoingEdge: edge argument can't be null.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestIsIncomingEdgeBad()
    //
    /// <summary>
    /// Tests the IsIncomingEdge() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentNullException) ) ]

    public void
    TestIsIncomingEdgeBad()
    {
        // null edge.

        try
        {
            IGraph oGraph = new Graph();

            IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, 1);

            aoVertices[0].IsIncomingEdge(null);
        }
        catch (ArgumentNullException oArgumentNullException)
        {
            Assert.AreEqual(

                "Smrf.NodeXL.Core."
                + "Vertex.IsIncomingEdge: edge argument can't be null.\r\n"
                + "Parameter name: edge"
                ,
                oArgumentNullException.Message
                );

            throw oArgumentNullException;
        }
    }

    //*************************************************************************
    //  Method: TestPredecessorVertices()
    //
    /// <summary>
    /// Tests the PredecessorVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPredecessorVertices()
    {
        TestPredecessorOrSuccessorVertices(true);
    }

    //*************************************************************************
    //  Method: TestPredecessorVertices2()
    //
    /// <summary>
    /// Tests the PredecessorVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestPredecessorVertices2()
    {
        // Verify that a self-loop is handled correctly.

        TestLoopVertices(true, false, false);
    }

    //*************************************************************************
    //  Method: TestSuccessorVertices()
    //
    /// <summary>
    /// Tests the SuccessorVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSuccessorVertices()
    {
        TestPredecessorOrSuccessorVertices(false);
    }

    //*************************************************************************
    //  Method: TestSuccessorVertices2()
    //
    /// <summary>
    /// Tests the SuccessorVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSuccessorVertices2()
    {
        // Verify that a self-loop is handled correctly.

        TestLoopVertices(false, true, false);
    }

    //*************************************************************************
    //  Method: TestAdjacentVertices()
    //
    /// <summary>
    /// Tests the AdjacentVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdjacentVertices()
    {
        // Create a graph and populate it with a random mix of incoming and
        // outgoing edges.

        IGraph oGraph = CreateAndPopulateGraph(125);

        // Use a dictionary to keep track of the adjacent vertices that are
        // counted below.  The same vertex should not be counted twice.
        //
        // The dictionary key is the vertex ID and the value is the vertex.

        Dictionary<Int32, IVertex> oCountedVertices =
            new Dictionary<Int32, IVertex>();

        // Loop through the graph's vertices.

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            // Get the vertex's adjacent vertices.

            ICollection<IVertex> oAdjacentVertices = oVertex.AdjacentVertices;

            // Loop through the graph's edges.

            foreach (IEdge oEdge in oGraph.Edges)
            {
                IVertex oAdjacentVertex = null;

                // Get the edge's vertices.

                IVertex [] aoVertices = oEdge.Vertices;

                IVertex oVertex1 = aoVertices[0];
                IVertex oVertex2 = aoVertices[1];

                if (oVertex1 != oVertex && oVertex2 != oVertex)
                {
                    // The edge is not incident to oVertex.

                    continue;
                }

                if (oEdge.IsDirected)
                {
                    if (oEdge.Vertex2 == oVertex)
                    {
                        // The edge's back vertex is an adjacent vertex.

                        oAdjacentVertex = oEdge.Vertex1;
                    }

                    if (oEdge.Vertex1 == oVertex)
                    {
                        // The edge's front vertex is an adjacent vertex.

                        oAdjacentVertex = oEdge.Vertex2;
                    }
                }
                else
                {
                    // The edge is undirected.

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
                        Assert.IsTrue(false);
                    }
                }

                // If there is an adjacent vertex and it hasn't been added to
                // the dictionary yet, add it.

                if (
                    oAdjacentVertex != null &&

                    !oCountedVertices.ContainsKey(oAdjacentVertex.ID)
                    )
                {
                    oCountedVertices.Add(oAdjacentVertex.ID, oAdjacentVertex);
                }
            }

            // Verify that each vertex in oCountedVertices is included in
            // oAdjacentVertices.

            HashSet<IVertex> oAdjacentVerticesHashSet = new HashSet<IVertex>(
                oAdjacentVertices);

            foreach (IVertex oVertexA in oCountedVertices.Values)
            {
                Assert.IsTrue(oAdjacentVerticesHashSet.Contains(oVertexA) );
            }

            // Verify that each vertex in aoAdjacentVertices is
            // in oCountedVertices.

            foreach (IVertex oVertexA in oAdjacentVertices)
            {
                Assert.IsTrue( oCountedVertices.ContainsValue(oVertexA) );
            }

            // Verify that the number of vertices in aoAdjacentVertices is the
            // same as the number of vertices that were just counted.

            Assert.AreEqual(oCountedVertices.Count, oAdjacentVertices.Count);

            oCountedVertices.Clear();
        }
    }

    //*************************************************************************
    //  Method: TestAdjacentVertices2()
    //
    /// <summary>
    /// Tests the AdjacentVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdjacentVertices2()
    {
        // Create a graph and populate it with a random mix of incoming and
        // outgoing edges.

        IGraph oGraph = CreateAndPopulateGraph(987);

        // Loop through the graph's vertices.

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            // Get the vertex's predecessor, successor, and adjacent vertices.

            ICollection<IVertex> oPredecessorVertices =
                oVertex.PredecessorVertices;

            ICollection<IVertex> oSuccessorVertices =
                oVertex.SuccessorVertices;

            ICollection<IVertex> oAdjacentVertices = oVertex.AdjacentVertices;

            Assert.IsTrue(oPredecessorVertices.Count +
                oSuccessorVertices.Count >= oAdjacentVertices.Count);

            // The adjacent vertices must be the union of the predecessor and
            // successor vertices.

            // Verify that each predecessor vertex is an adjacent vertex.

            HashSet<IVertex> oAdjacentVerticesHashSet = new HashSet<IVertex>(
                oAdjacentVertices);

            foreach (IVertex oVertexA in oPredecessorVertices)
            {
                Assert.IsTrue( oAdjacentVerticesHashSet.Contains(oVertexA) );
            }

            // Verify that each successor vertex is an adjacent vertex.

            foreach (IVertex oVertexA in oSuccessorVertices)
            {
                Assert.IsTrue( oAdjacentVerticesHashSet.Contains(oVertexA) );
            }

            // Verify that each adjacent vertex is either a predecessor vertex
            // or a successor vertex.

            HashSet<IVertex> oPredecessorVerticesHashSet = new HashSet<IVertex>(
                oPredecessorVertices);

            HashSet<IVertex> oSuccessorVerticesHashSet = new HashSet<IVertex>(
                oSuccessorVertices);

            foreach (IVertex oVertexA in oAdjacentVertices)
            {
                Assert.IsTrue(
                    oPredecessorVerticesHashSet.Contains(oVertexA)
                    ||
                    oSuccessorVerticesHashSet.Contains(oVertexA)
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: TestAdjacentVertices3()
    //
    /// <summary>
    /// Tests the AdjacentVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdjacentVertices3()
    {
        // Verify that a self-loop is handled correctly.

        TestLoopVertices(false, false, true);
    }

    //*************************************************************************
    //  Method: TestClone()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone()
    {
        TestClone(false, false);
    }

    //*************************************************************************
    //  Method: TestClone2()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone2()
    {
        TestClone(false, true);
    }

    //*************************************************************************
    //  Method: TestClone3()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone3()
    {
        TestClone(true, false);
    }

    //*************************************************************************
    //  Method: TestClone4()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClone4()
    {
        TestClone(true, true);
    }

    //*************************************************************************
    //  Method: TestToString()
    //
    /// <summary>
    /// Tests the ToString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToString()
    {
        Assert.AreEqual( "ID=" +
            m_oVertex.ID.ToString(NodeXLBase.Int32Format),
            m_oVertex.ToString() );
    }

    //*************************************************************************
    //  Method: TestIncomingOrOutgoingEdges()
    //
    /// <summary>
    /// Tests the IncomingEdges or OutgoingEdges property.
    /// </summary>
    ///
    /// <param name="bIncoming">
    /// true to test the IncomingEdges property, false to test the
    /// OutgoingEdges property.
    /// </param>
    //*************************************************************************

    protected void
    TestIncomingOrOutgoingEdges
    (
        Boolean bIncoming
    )
    {
        // Create a graph and populate it with a random mix of incoming and
        // outgoing edges.

        IGraph oGraph = CreateAndPopulateGraph(143);

        // Loop through the graph's vertices.

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            // Loop through the vertex's incoming or outgoing edges.

            ICollection<IEdge> oIncomingOrOutgoingEdges =
                bIncoming ? oVertex.IncomingEdges : oVertex.OutgoingEdges;

            Assert.IsNotNull(oIncomingOrOutgoingEdges);

            foreach (IEdge oEdge in oIncomingOrOutgoingEdges)
            {
                // An incoming edge is either a directed edge that has this
                // vertex at its front, or an undirected edge connected to this
                // vertex. 
                ///
                // An outgoing edge is either a directed edge that has this
                // vertex at its back, or an undirected edge connected to this
                // vertex. 
                //
                // Verify that the edge is incoming or outgoing.

                if (oEdge.IsDirected)
                {
                    Assert.AreEqual(oVertex,
                        bIncoming ? oEdge.Vertex2 : oEdge.Vertex1);
                }
                else
                {
                    IVertex [] aoVertices = oEdge.Vertices;

                    Assert.IsTrue(
                        aoVertices[0] == oVertex || aoVertices[1] == oVertex);
                }

                // Verify that the edge is in the graph.

                Assert.IsTrue( oGraph.Edges.Contains(oEdge) );
            }

            HashSet<IEdge> oIncomingOrOutgoingEdgesHashSet =
                new HashSet<IEdge>(oIncomingOrOutgoingEdges);

            // Loop through the graph's edges.

            foreach (IEdge oEdge in oGraph.Edges)
            {
                // If the edge is incoming (or outgoing), verify that it's
                /// included in the vertex's incoming (or incoming) edges.

                Boolean bShouldBeInArray = false;

                if (oEdge.IsDirected)
                {
                    if ( oVertex ==
                        (bIncoming ? oEdge.Vertex2 : oEdge.Vertex1) )
                    {
                        bShouldBeInArray = true;
                    }
                }
                else
                {
                    IVertex [] aoVertices = oEdge.Vertices;

                    if (aoVertices[0] == oVertex || aoVertices[1] == oVertex)
                    {
                        bShouldBeInArray = true;
                    }
                }

                if (bShouldBeInArray)
                {
                    Assert.IsTrue( oIncomingOrOutgoingEdgesHashSet.Contains(
                        oEdge) );
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TestPredecessorOrSuccessorVertices()
    //
    /// <summary>
    /// Tests the PredecessorVertices or SuccessorVertices property.
    /// </summary>
    ///
    /// <param name="bPredecessor">
    /// true to test the PredecessorVertices property, false to test the
    /// SuccessorVertices property.
    /// </param>
    //*************************************************************************

    protected void
    TestPredecessorOrSuccessorVertices
    (
        Boolean bPredecessor
    )
    {
        // Create a graph and populate it with a random mix of incoming and
        // outgoing edges.

        IGraph oGraph = CreateAndPopulateGraph(100);

        // Use a dictionary to keep track of the predecessor or successor
        // vertices that are counted below.  The same vertex should not be
        // counted twice.
        //
        // The dictionary key is the vertex ID and the value is the vertex.

        Dictionary<Int32, IVertex> oCountedVertices =
            new Dictionary<Int32, IVertex>();

        // Loop through the graph's vertices.

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            // Get the vertex's predecessor or successor vertices.

            ICollection<IVertex> oPredecessorOrSuccessorVertices =
                bPredecessor ?
                oVertex.PredecessorVertices : oVertex.SuccessorVertices;

            // Loop through the graph's edges.

            foreach (IEdge oEdge in oGraph.Edges)
            {
                IVertex oPredecessorOrSuccessorVertex = null;

                // Get the edge's vertices.

                IVertex [] aoVertices = oEdge.Vertices;

                IVertex oVertex1 = aoVertices[0];
                IVertex oVertex2 = aoVertices[1];

                if (oVertex1 != oVertex && oVertex2 != oVertex)
                {
                    // The edge is not incident to oVertex.

                    continue;
                }

                if (oEdge.IsDirected)
                {
                    if (oEdge.Vertex2 == oVertex)
                    {
                        // The edge is incoming.

                        if (bPredecessor)
                        {
                            // The edge's back vertex is a predecessor vertex.

                            oPredecessorOrSuccessorVertex = oEdge.Vertex1;
                        }
                    }

                    if (oEdge.Vertex1 == oVertex)
                    {
                        // The edge is outgoing.

                        if (!bPredecessor)
                        {
                            // The edge's front vertex is a successor vertex.

                            oPredecessorOrSuccessorVertex = oEdge.Vertex2;
                        }
                    }
                }
                else
                {
                    // The edge is undirected.

                    if (oVertex1 == oVertex)
                    {
                        // oVertex2 is both a predecessor and successor vertex.

                        oPredecessorOrSuccessorVertex = oVertex2;
                    }
                    else if (oVertex2 == oVertex)
                    {
                        // oVertex1 is both a predecessor and successor vertex.

                        oPredecessorOrSuccessorVertex = oVertex1;
                    }
                    else
                    {
                        Assert.IsTrue(false);
                    }
                }

                // If there is a predecessor or successor vertex and it hasn't
                // been added to the dictionary yet, add it.

                if (
                    oPredecessorOrSuccessorVertex != null &&

                    !oCountedVertices.ContainsKey(
                        oPredecessorOrSuccessorVertex.ID)
                    )
                {
                    oCountedVertices.Add(oPredecessorOrSuccessorVertex.ID,
                        oPredecessorOrSuccessorVertex);
                }
            }

            // Verify that each vertex in oCountedVertices is included in
            // oPredecessorOrSuccessorVertices.

            HashSet<IVertex> oPredecessorOrSuccessorVerticesHashSet =
                new HashSet<IVertex>(oPredecessorOrSuccessorVertices);

            foreach (IVertex oVertexA in oCountedVertices.Values)
            {
                Assert.IsTrue(
                    oPredecessorOrSuccessorVerticesHashSet.Contains(oVertexA)
                    );
            }

            // Verify that each vertex in aoPredecessorOrSuccessorVertices is
            // in oCountedVertices.

            foreach (IVertex oVertexA in oPredecessorOrSuccessorVertices)
            {
                Assert.IsTrue( oCountedVertices.ContainsValue(oVertexA) );
            }

            // Verify that the number of vertices in
            // aoPredecessorOrSuccessorVertices is the same as the number of
            // vertices that were just counted.

            Assert.AreEqual(oCountedVertices.Count,
                oPredecessorOrSuccessorVertices.Count);

            oCountedVertices.Clear();
        }
    }

    //*************************************************************************
    //  Method: CreateAndPopulateGraph()
    //
    /// <summary>
    /// Creates a graph and populates it with a random mix of incoming and
    /// outgoing edges.
    /// </summary>
    ///
    /// <param name="iVertices">
    /// Number of vertices to add to the graph.
    /// </param>
    ///
    /// <returns>
    /// The new graph.
    /// </returns>
    //*************************************************************************

    protected IGraph
    CreateAndPopulateGraph
    (
        Int32 iVertices
    )
    {
        Debug.Assert(iVertices >= 0);

        Random oRandom = new Random(1);

        IGraph oGraph = new Graph();

        // Add the vertices.

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, iVertices);

        IEdgeCollection oEdgeCollection = oGraph.Edges;

        // Add random directed and undirected edges.

        Int32 iRandomEdges = 100 * iVertices;

        for (Int32 i = 0; i < iRandomEdges; i++)
        {
            Int32 iVertex1Index = oRandom.Next(iVertices);
            Int32 iVertex2Index = oRandom.Next(iVertices);

            Boolean bIsDirected = (oRandom.Next(2) == 0);

            oEdgeCollection.Add(
                aoVertices[iVertex2Index], aoVertices[iVertex1Index],
                bIsDirected);
        }

        return (oGraph);
    }

    //*************************************************************************
    //  Method: TestLoopEdges()
    //
    /// <summary>
    /// Tests the IncomingEdges, OutgoingEdges, or IncidentEdges property when
    /// an edge is a self-loop (an edge that connects a vertex to itself).
    /// </summary>
    ///
    /// <param name="bTestIncomingEdges">
    /// true to test the IncomingEdges property.
    /// </param>
    ///
    /// <param name="bTestOutgoingEdges">
    /// true to test the OutgoingEdges property.
    /// </param>
    ///
    /// <param name="bTestIncidentEdges">
    /// true to test the IncidentEdges property.
    /// </param>
    //*************************************************************************

    protected void
    TestLoopEdges
    (
        Boolean bTestIncomingEdges,
        Boolean bTestOutgoingEdges,
        Boolean bTestIncidentEdges
    )
    {
        IGraph oGraph = new Graph();

        // Add one vertex.

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, 1);

        IVertex oVertex = aoVertices[0];

        IEdgeCollection oEdgeCollection = oGraph.Edges;

        // Add multiple self-loops, both undirected and directed.

        IEdge oUndirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, false);
        IEdge oUndirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, false);

        IEdge oDirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, true);
        IEdge oDirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, true);

        ICollection<IEdge> oEdges = null;

        if (bTestIncomingEdges)
        {
            oEdges = oVertex.IncomingEdges;
        }
        else if (bTestOutgoingEdges)
        {
            oEdges = oVertex.OutgoingEdges;
        }
        else if (bTestIncidentEdges)
        {
            oEdges = oVertex.IncidentEdges;

            Assert.AreEqual(oEdges.Count, oVertex.Degree);
        }
        else
        {
            Debug.Assert(false);
        }

        Assert.AreEqual(4, oEdges.Count);

        HashSet<IEdge> oEdgesHashSet = new HashSet<IEdge>(oEdges);

        Assert.IsTrue( oEdgesHashSet.Contains(oUndirectedEdge1) );
        Assert.IsTrue( oEdgesHashSet.Contains(oUndirectedEdge2) );

        Assert.IsTrue( oEdgesHashSet.Contains(oDirectedEdge1) );
        Assert.IsTrue( oEdgesHashSet.Contains(oDirectedEdge2) );
    }

    //*************************************************************************
    //  Method: TestLoopVertices()
    //
    /// <summary>
    /// Tests the PredecessorVertices, SuccessorVertices, or AdjacentVertices
    /// property when an edge is a self-loop (an edge that connects a vertex to
    /// itself).
    /// </summary>
    ///
    /// <param name="bTestPredecessorVertices">
    /// true to test the PredecessorVertices property.
    /// </param>
    ///
    /// <param name="bTestSuccessorVertices">
    /// true to test the SuccessorVertices property.
    /// </param>
    ///
    /// <param name="bTestAdjacentVertices">
    /// true to test the AdjacentVertices property.
    /// </param>
    //*************************************************************************

    protected void
    TestLoopVertices
    (
        Boolean bTestPredecessorVertices,
        Boolean bTestSuccessorVertices,
        Boolean bTestAdjacentVertices
    )
    {
        IGraph oGraph = new Graph();

        // Add one vertex.

        IVertex [] aoVertices = TestGraphUtil.AddVertices(oGraph, 1);

        IVertex oVertex = aoVertices[0];

        IEdgeCollection oEdgeCollection = oGraph.Edges;

        // Add multiple self-loops, both undirected and directed.

        IEdge oUndirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, false);
        IEdge oUndirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, false);

        IEdge oDirectedEdge1 = oEdgeCollection.Add(oVertex, oVertex, true);
        IEdge oDirectedEdge2 = oEdgeCollection.Add(oVertex, oVertex, true);

        ICollection<IVertex> oVerticesA = null;

        if (bTestPredecessorVertices)
        {
            oVerticesA = oVertex.PredecessorVertices;
        }
        else if (bTestSuccessorVertices)
        {
            oVerticesA = oVertex.SuccessorVertices;
        }
        else if (bTestAdjacentVertices)
        {
            oVerticesA = oVertex.AdjacentVertices;
        }
        else
        {
            Debug.Assert(false);
        }

        Assert.AreEqual(1, oVerticesA.Count);

        HashSet<IVertex> oVerticesAHashSet = new HashSet<IVertex>(oVerticesA);

        Assert.IsTrue( oVerticesAHashSet.Contains(oVertex) );
    }

    //*************************************************************************
    //  Method: TestClone()
    //
    /// <summary>
    /// Tests the Clone() method.
    /// </summary>
    ///
    /// <param name="bCopyMetadataValues">
    /// true to copy metadata values while cloning.
    /// </param>
    ///
    /// <param name="bCopyTag">
    /// true to copy tag values while cloning.
    /// </param>
    //*************************************************************************

    protected void
    TestClone
    (
        Boolean bCopyMetadataValues,
        Boolean bCopyTag
    )
    {
        // Create N objects, set random metadata and Tag on each object, clone
        // each object, check new object.

        const Int32 Vertices = 1000;

        Vertex [] aoVertices = new Vertex[Vertices];

        // Set random values on each object.

        for (Int32 i = 0; i < Vertices; i++)
        {
            Vertex oVertex = aoVertices[i] = new Vertex();

            MetadataUtil.SetRandomMetadata(oVertex, true, true, i);

            oVertex.Name = oVertex.ID.ToString();
        }

        for (Int32 i = 0; i < Vertices; i++)
        {
            // Clone the object.

            Vertex oVertex = aoVertices[i];

            Vertex oNewVertex = (Vertex)oVertex.Clone(
                bCopyMetadataValues, bCopyTag);

            // Check the metadata on the new object.

            MetadataUtil.CheckRandomMetadata(
                oNewVertex, bCopyMetadataValues, bCopyTag, i);

            // Check the name and ID on the new object.

            Assert.AreEqual(oVertex.Name, oNewVertex.Name);

            Assert.AreNotEqual(oVertex.ID, oNewVertex.ID);
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected Vertex m_oVertex;
}

}
