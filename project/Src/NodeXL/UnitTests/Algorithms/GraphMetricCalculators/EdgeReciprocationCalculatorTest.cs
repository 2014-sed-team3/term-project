
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: EdgeReciprocationCalculatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="EdgeReciprocationCalculator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class EdgeReciprocationCalculatorTest : Object
{
    //*************************************************************************
    //  Constructor: EdgeReciprocationCalculatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EdgeReciprocationCalculatorTest" /> class.
    /// </summary>
    //*************************************************************************

    public EdgeReciprocationCalculatorTest()
    {
        m_oEdgeReciprocationCalculator = null;
        m_oGraph = null;
        m_oVertices = null;
        m_oEdges = null;
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
        m_oEdgeReciprocationCalculator = null;
        m_oGraph = null;
        m_oVertices = null;
        m_oEdges = null;
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics()
    {
        // Empty graph.

        CreateGraph(true);

        IDictionary<Int32, Boolean> oReciprocationFlags =
            m_oEdgeReciprocationCalculator.CalculateGraphMetrics(m_oGraph);

        Assert.AreEqual(0, oReciprocationFlags.Count);
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics2()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics2()
    {
        // Undirected graph.

        Boolean bIsDirected = false;
        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexC, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexB, bIsDirected);

        IDictionary<Int32, Boolean> oReciprocationFlags =
            m_oEdgeReciprocationCalculator.CalculateGraphMetrics(m_oGraph);

        Assert.AreEqual(0, oReciprocationFlags.Count);
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics3()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics3()
    {
        // Directed graph.

        Boolean bIsDirected = true;
        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();

        IEdge oEdge1 = m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        IEdge oEdge2 = m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
        IEdge oEdge3 = m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
        IEdge oEdge4 = m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        IEdge oEdge5 = m_oEdges.Add(oVertexC, oVertexA, bIsDirected);
        IEdge oEdge6 = m_oEdges.Add(oVertexC, oVertexB, bIsDirected);

        IDictionary<Int32, Boolean> oReciprocationFlags =
            m_oEdgeReciprocationCalculator.CalculateGraphMetrics(m_oGraph);

        Assert.AreEqual(6, oReciprocationFlags.Count);

        Assert.IsTrue( oReciprocationFlags[oEdge1.ID] );
        Assert.IsTrue( oReciprocationFlags[oEdge2.ID] );
        Assert.IsTrue( oReciprocationFlags[oEdge3.ID] );
        Assert.IsTrue( oReciprocationFlags[oEdge4.ID] );
        Assert.IsTrue( oReciprocationFlags[oEdge5.ID] );
        Assert.IsTrue( oReciprocationFlags[oEdge6.ID] );
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics4()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics4()
    {
        // Directed graph, more complex.

        Boolean bIsDirected = true;
        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();

        IEdge oEdge1  = m_oEdges.Add(oVertexA, oVertexA, bIsDirected);
        IEdge oEdge2  = m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        IEdge oEdge3  = m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        IEdge oEdge4  = m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        IEdge oEdge5  = m_oEdges.Add(oVertexB, oVertexD, bIsDirected);
        IEdge oEdge6  = m_oEdges.Add(oVertexB, oVertexD, bIsDirected);
        IEdge oEdge7  = m_oEdges.Add(oVertexD, oVertexD, bIsDirected);
        IEdge oEdge8  = m_oEdges.Add(oVertexD, oVertexC, bIsDirected);
        IEdge oEdge9  = m_oEdges.Add(oVertexD, oVertexC, bIsDirected);
        IEdge oEdge10 = m_oEdges.Add(oVertexC, oVertexD, bIsDirected);
        IEdge oEdge11 = m_oEdges.Add(oVertexC, oVertexD, bIsDirected);
        IEdge oEdge12 = m_oEdges.Add(oVertexD, oVertexE, bIsDirected);

        IDictionary<Int32, Boolean> oReciprocationFlags =
            m_oEdgeReciprocationCalculator.CalculateGraphMetrics(m_oGraph);

        Assert.AreEqual(12, oReciprocationFlags.Count);

        Assert.IsFalse( oReciprocationFlags[oEdge1.ID] );
        Assert.IsTrue ( oReciprocationFlags[oEdge2.ID] );
        Assert.IsTrue ( oReciprocationFlags[oEdge3.ID] );
        Assert.IsTrue ( oReciprocationFlags[oEdge4.ID] );
        Assert.IsFalse( oReciprocationFlags[oEdge5.ID] );
        Assert.IsFalse( oReciprocationFlags[oEdge6.ID] );
        Assert.IsFalse( oReciprocationFlags[oEdge7.ID] );
        Assert.IsTrue ( oReciprocationFlags[oEdge8.ID] );
        Assert.IsTrue ( oReciprocationFlags[oEdge9.ID] );
        Assert.IsTrue ( oReciprocationFlags[oEdge10.ID] );
        Assert.IsTrue ( oReciprocationFlags[oEdge11.ID] );
        Assert.IsFalse( oReciprocationFlags[oEdge12.ID] );
    }

    //*************************************************************************
    //  Method: CreateGraph()
    //
    /// <summary>
    /// Creates the graph.
    /// </summary>
    ///
    /// <param name="bIsDirected">
    /// true if the graph should be directed.
    /// </param>
    //*************************************************************************

    protected void
    CreateGraph
    (
        Boolean bIsDirected
    )
    {
        m_oEdgeReciprocationCalculator = new EdgeReciprocationCalculator();

        m_oGraph = new Graph(bIsDirected ?
            GraphDirectedness.Directed : GraphDirectedness.Undirected);

        m_oVertices = m_oGraph.Vertices;
        m_oEdges = m_oGraph.Edges;
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object being tested.

    protected EdgeReciprocationCalculator m_oEdgeReciprocationCalculator;

    /// The graph being tested.

    protected IGraph m_oGraph;

    /// The graph's vertices;

    protected IVertexCollection m_oVertices;

    /// The graph's Edges;

    protected IEdgeCollection m_oEdges;
}

}
