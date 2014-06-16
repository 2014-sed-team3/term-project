
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeUtilTest
//
/// <summary>
/// This is Visual Studio test fixture for the <see cref="EdgeUtil" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeUtilTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeUtilTest" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeUtilTest()
    {
        // (Do nothing.)
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
        // (Do nothing.)
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestEdgeToVertices()
    //
    /// <summary>
    /// Tests the EdgeToVertices() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestEdgeToVertices()
    {
        // Valid vertices.

        MockVertex oVertex1 = new MockVertex();
        MockVertex oVertex2 = new MockVertex();

        IGraph oGraph = new Graph();

        oVertex1.ParentGraph = oGraph;
        oVertex2.ParentGraph = oGraph;

        IEdge oEdge = new MockEdge(oVertex1, oVertex2, false, false, 2);

        IVertex oVertexA, oVertexB;

        EdgeUtil.EdgeToVertices(oEdge, ClassName, MethodOrPropertyName,
            out oVertexA, out oVertexB);

        Assert.AreEqual(oVertex1, oVertexA);
        Assert.AreEqual(oVertex2, oVertexB);
    }

   

    //*************************************************************************
    //  Method: TestGetVertexNamePair()
    //
    /// <summary>
    /// Tests the GetVertexNamePair() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexNamePair()
    {
        Assert.AreEqual( "A\vB", EdgeUtil.GetVertexNamePair("A", "B", true) );
    }

    //*************************************************************************
    //  Method: TestGetVertexNamePair2()
    //
    /// <summary>
    /// Tests the GetVertexNamePair() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexNamePair2()
    {
        Assert.AreEqual( "B\vA", EdgeUtil.GetVertexNamePair("B", "A", true) );
    }

    //*************************************************************************
    //  Method: TestGetVertexNamePair3()
    //
    /// <summary>
    /// Tests the GetVertexNamePair() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexNamePair3()
    {
        Assert.AreEqual( "A\vB", EdgeUtil.GetVertexNamePair("A", "B", false) );
    }

    //*************************************************************************
    //  Method: TestGetVertexNamePair4()
    //
    /// <summary>
    /// Tests the GetVertexNamePair() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexNamePair4()
    {
        Assert.AreEqual( "A\vB", EdgeUtil.GetVertexNamePair("B", "A", false) );
    }

    //*************************************************************************
    //  Method: TestGetVertexIDPair()
    //
    /// <summary>
    /// Tests the GetVertexIDPair(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexIDPair()
    {
        // Directed graph.

        IGraph oDirectedGraph = new Graph(GraphDirectedness.Directed);
        IVertexCollection oDirectedVertices = oDirectedGraph.Vertices;

        IVertex oDirectedVertexA = oDirectedVertices.Add();
        IVertex oDirectedVertexB = oDirectedVertices.Add();
        IVertex oDirectedVertexC = oDirectedVertices.Add();
        IVertex oDirectedVertexD = oDirectedVertices.Add();

        IEdgeCollection oEdges = oDirectedGraph.Edges;

        IEdge oEdge1 = oEdges.Add(oDirectedVertexA, oDirectedVertexB, true);
        IEdge oEdge2 = oEdges.Add(oDirectedVertexA, oDirectedVertexB, true);
        IEdge oEdge3 = oEdges.Add(oDirectedVertexB, oDirectedVertexA, true);
        IEdge oEdge4 = oEdges.Add(oDirectedVertexA, oDirectedVertexC, true);

        Int64 i64VertexIDPair1 = EdgeUtil.GetVertexIDPair(oEdge1);
        Int64 i64VertexIDPair2 = EdgeUtil.GetVertexIDPair(oEdge2);
        Int64 i64VertexIDPair3 = EdgeUtil.GetVertexIDPair(oEdge3);
        Int64 i64VertexIDPair4 = EdgeUtil.GetVertexIDPair(oEdge4);

        // Make sure that the left-shift worked.

        Assert.IsTrue(i64VertexIDPair1 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair2 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair3 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair4 > Int32.MaxValue);

        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair2);
        Assert.AreNotEqual(i64VertexIDPair1, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair1, i64VertexIDPair4);

        Assert.AreNotEqual(i64VertexIDPair2, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair2, i64VertexIDPair4);

        Assert.AreNotEqual(i64VertexIDPair3, i64VertexIDPair4);
    }

    //*************************************************************************
    //  Method: TestGetVertexIDPair2()
    //
    /// <summary>
    /// Tests the GetVertexIDPair(IEdge) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexIDPair2()
    {
        // Undirected graph.

        IGraph oUndirectedGraph = new Graph(GraphDirectedness.Undirected);
        IVertexCollection oUndirectedVertices = oUndirectedGraph.Vertices;

        IVertex oUndirectedVertexA = oUndirectedVertices.Add();
        IVertex oUndirectedVertexB = oUndirectedVertices.Add();
        IVertex oUndirectedVertexC = oUndirectedVertices.Add();
        IVertex oUndirectedVertexD = oUndirectedVertices.Add();

        IEdgeCollection oEdges = oUndirectedGraph.Edges;

        IEdge oEdge1 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexB,
            false);

        IEdge oEdge2 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexB,
            false);

        IEdge oEdge3 = oEdges.Add(oUndirectedVertexB, oUndirectedVertexA,
            false);

        IEdge oEdge4 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexC,
            false);

        Int64 i64VertexIDPair1 = EdgeUtil.GetVertexIDPair(oEdge1);
        Int64 i64VertexIDPair2 = EdgeUtil.GetVertexIDPair(oEdge2);
        Int64 i64VertexIDPair3 = EdgeUtil.GetVertexIDPair(oEdge3);
        Int64 i64VertexIDPair4 = EdgeUtil.GetVertexIDPair(oEdge4);

        // Make sure that the left-shift worked.

        Assert.IsTrue(i64VertexIDPair1 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair2 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair3 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair4 > Int32.MaxValue);

        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair2);
        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair1, i64VertexIDPair4);

        Assert.AreEqual(i64VertexIDPair2, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair2, i64VertexIDPair4);

        Assert.AreNotEqual(i64VertexIDPair3, i64VertexIDPair4);
    }

    //*************************************************************************
    //  Method: TestGetVertexIDPair3()
    //
    /// <summary>
    /// Tests the GetVertexIDPair(IEdge, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexIDPair3()
    {
        // Directed graph, useDirectedness = false.

        IGraph oDirectedGraph = new Graph(GraphDirectedness.Directed);
        IVertexCollection oDirectedVertices = oDirectedGraph.Vertices;

        IVertex oDirectedVertexA = oDirectedVertices.Add();
        IVertex oDirectedVertexB = oDirectedVertices.Add();
        IVertex oDirectedVertexC = oDirectedVertices.Add();
        IVertex oDirectedVertexD = oDirectedVertices.Add();

        IEdgeCollection oEdges = oDirectedGraph.Edges;

        IEdge oEdge1 = oEdges.Add(oDirectedVertexA, oDirectedVertexB, true);
        IEdge oEdge2 = oEdges.Add(oDirectedVertexA, oDirectedVertexB, true);
        IEdge oEdge3 = oEdges.Add(oDirectedVertexB, oDirectedVertexA, true);
        IEdge oEdge4 = oEdges.Add(oDirectedVertexA, oDirectedVertexC, true);

        Int64 i64VertexIDPair1 = EdgeUtil.GetVertexIDPair(oEdge1, false);
        Int64 i64VertexIDPair2 = EdgeUtil.GetVertexIDPair(oEdge2, false);
        Int64 i64VertexIDPair3 = EdgeUtil.GetVertexIDPair(oEdge3, false);
        Int64 i64VertexIDPair4 = EdgeUtil.GetVertexIDPair(oEdge4, false);

        // Make sure that the left-shift worked.

        Assert.IsTrue(i64VertexIDPair1 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair2 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair3 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair4 > Int32.MaxValue);

        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair2);
        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair1, i64VertexIDPair4);

        Assert.AreEqual(i64VertexIDPair2, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair2, i64VertexIDPair4);

        Assert.AreNotEqual(i64VertexIDPair3, i64VertexIDPair4);
    }

    //*************************************************************************
    //  Method: TestGetVertexIDPair4()
    //
    /// <summary>
    /// Tests the GetVertexIDPair(IEdge, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexIDPair4()
    {
        // Directed graph, useDirectedness = true.

        IGraph oDirectedGraph = new Graph(GraphDirectedness.Directed);
        IVertexCollection oDirectedVertices = oDirectedGraph.Vertices;

        IVertex oDirectedVertexA = oDirectedVertices.Add();
        IVertex oDirectedVertexB = oDirectedVertices.Add();
        IVertex oDirectedVertexC = oDirectedVertices.Add();
        IVertex oDirectedVertexD = oDirectedVertices.Add();

        IEdgeCollection oEdges = oDirectedGraph.Edges;

        IEdge oEdge1 = oEdges.Add(oDirectedVertexA, oDirectedVertexB, true);
        IEdge oEdge2 = oEdges.Add(oDirectedVertexA, oDirectedVertexB, true);
        IEdge oEdge3 = oEdges.Add(oDirectedVertexB, oDirectedVertexA, true);
        IEdge oEdge4 = oEdges.Add(oDirectedVertexA, oDirectedVertexC, true);

        Int64 i64VertexIDPair1 = EdgeUtil.GetVertexIDPair(oEdge1, true);
        Int64 i64VertexIDPair2 = EdgeUtil.GetVertexIDPair(oEdge2, true);
        Int64 i64VertexIDPair3 = EdgeUtil.GetVertexIDPair(oEdge3, true);
        Int64 i64VertexIDPair4 = EdgeUtil.GetVertexIDPair(oEdge4, true);

        // Make sure that the left-shift worked.

        Assert.IsTrue(i64VertexIDPair1 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair2 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair3 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair4 > Int32.MaxValue);

        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair2);
        Assert.AreNotEqual(i64VertexIDPair1, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair1, i64VertexIDPair4);

        Assert.AreNotEqual(i64VertexIDPair2, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair2, i64VertexIDPair4);

        Assert.AreNotEqual(i64VertexIDPair3, i64VertexIDPair4);
    }

    //*************************************************************************
    //  Method: TestGetVertexIDPair5()
    //
    /// <summary>
    /// Tests the GetVertexIDPair(IEdge, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexIDPair5()
    {
        // Undirected graph, useDirectedness = true.

        IGraph oUndirectedGraph = new Graph(GraphDirectedness.Undirected);
        IVertexCollection oUndirectedVertices = oUndirectedGraph.Vertices;

        IVertex oUndirectedVertexA = oUndirectedVertices.Add();
        IVertex oUndirectedVertexB = oUndirectedVertices.Add();
        IVertex oUndirectedVertexC = oUndirectedVertices.Add();
        IVertex oUndirectedVertexD = oUndirectedVertices.Add();

        IEdgeCollection oEdges = oUndirectedGraph.Edges;

        IEdge oEdge1 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexB,
            false);

        IEdge oEdge2 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexB,
            false);

        IEdge oEdge3 = oEdges.Add(oUndirectedVertexB, oUndirectedVertexA,
            false);

        IEdge oEdge4 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexC,
            false);

        Int64 i64VertexIDPair1 = EdgeUtil.GetVertexIDPair(oEdge1, true);
        Int64 i64VertexIDPair2 = EdgeUtil.GetVertexIDPair(oEdge2, true);
        Int64 i64VertexIDPair3 = EdgeUtil.GetVertexIDPair(oEdge3, true);
        Int64 i64VertexIDPair4 = EdgeUtil.GetVertexIDPair(oEdge4, true);

        // Make sure that the left-shift worked.

        Assert.IsTrue(i64VertexIDPair1 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair2 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair3 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair4 > Int32.MaxValue);

        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair2);
        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair1, i64VertexIDPair4);

        Assert.AreEqual(i64VertexIDPair2, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair2, i64VertexIDPair4);

        Assert.AreNotEqual(i64VertexIDPair3, i64VertexIDPair4);
    }

    //*************************************************************************
    //  Method: TestGetVertexIDPair6()
    //
    /// <summary>
    /// Tests the GetVertexIDPair(IEdge, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetVertexIDPair6()
    {
        // Undirected graph, useDirectedness = false.

        IGraph oUndirectedGraph = new Graph(GraphDirectedness.Undirected);
        IVertexCollection oUndirectedVertices = oUndirectedGraph.Vertices;

        IVertex oUndirectedVertexA = oUndirectedVertices.Add();
        IVertex oUndirectedVertexB = oUndirectedVertices.Add();
        IVertex oUndirectedVertexC = oUndirectedVertices.Add();
        IVertex oUndirectedVertexD = oUndirectedVertices.Add();

        IEdgeCollection oEdges = oUndirectedGraph.Edges;

        IEdge oEdge1 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexB,
            false);

        IEdge oEdge2 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexB,
            false);

        IEdge oEdge3 = oEdges.Add(oUndirectedVertexB, oUndirectedVertexA,
            false);

        IEdge oEdge4 = oEdges.Add(oUndirectedVertexA, oUndirectedVertexC,
            false);

        Int64 i64VertexIDPair1 = EdgeUtil.GetVertexIDPair(oEdge1, false);
        Int64 i64VertexIDPair2 = EdgeUtil.GetVertexIDPair(oEdge2, false);
        Int64 i64VertexIDPair3 = EdgeUtil.GetVertexIDPair(oEdge3, false);
        Int64 i64VertexIDPair4 = EdgeUtil.GetVertexIDPair(oEdge4, false);

        // Make sure that the left-shift worked.

        Assert.IsTrue(i64VertexIDPair1 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair2 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair3 > Int32.MaxValue);
        Assert.IsTrue(i64VertexIDPair4 > Int32.MaxValue);

        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair2);
        Assert.AreEqual(i64VertexIDPair1, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair1, i64VertexIDPair4);

        Assert.AreEqual(i64VertexIDPair2, i64VertexIDPair3);
        Assert.AreNotEqual(i64VertexIDPair2, i64VertexIDPair4);

        Assert.AreNotEqual(i64VertexIDPair3, i64VertexIDPair4);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Arguments to pass to EdgeUtil methods.

    protected const String ClassName = "TheClass";
    ///
    protected const String MethodOrPropertyName = "TheMethodOrProperty";
}

}
