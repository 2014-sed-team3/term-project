
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: OverallReciprocationCalculatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="OverallReciprocationCalculator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class OverallReciprocationCalculatorTest : Object
{
    //*************************************************************************
    //  Constructor: OverallReciprocationCalculatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="OverallReciprocationCalculatorTest" /> class.
    /// </summary>
    //*************************************************************************

    public OverallReciprocationCalculatorTest()
    {
        m_oOverallReciprocationCalculator = null;
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
        m_oOverallReciprocationCalculator = null;
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
        // Empty directed graph.

        Boolean bIsDirected = true;
        CreateGraph(bIsDirected);

        Nullable<Double> dReciprocatedVertexPairRatio, dReciprocatedEdgeRatio;

        m_oOverallReciprocationCalculator.CalculateGraphMetrics(m_oGraph,
            out dReciprocatedVertexPairRatio, out dReciprocatedEdgeRatio);

        Assert.IsFalse(dReciprocatedVertexPairRatio.HasValue);
        Assert.IsFalse(dReciprocatedEdgeRatio.HasValue);
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

        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);

        Nullable<Double> dReciprocatedVertexPairRatio, dReciprocatedEdgeRatio;

        m_oOverallReciprocationCalculator.CalculateGraphMetrics(m_oGraph,
            out dReciprocatedVertexPairRatio, out dReciprocatedEdgeRatio);

        Assert.IsFalse(dReciprocatedVertexPairRatio.HasValue);
        Assert.IsFalse(dReciprocatedEdgeRatio.HasValue);
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
        // Directed graph, sample from http://
        // faculty.ucr.edu/~hanneman/nettext/C8_Embedding.html#reciprocity

        Boolean bIsDirected = true;
        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexC, bIsDirected);

        Nullable<Double> dReciprocatedVertexPairRatio, dReciprocatedEdgeRatio;

        m_oOverallReciprocationCalculator.CalculateGraphMetrics(m_oGraph,
            out dReciprocatedVertexPairRatio, out dReciprocatedEdgeRatio);

        Assert.AreEqual(0.5, dReciprocatedVertexPairRatio.Value);
        Assert.AreEqual(0.667, dReciprocatedEdgeRatio.Value, 0.001);
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
        // Directed graph, sample from http://
        // faculty.ucr.edu/~hanneman/nettext/C8_Embedding.html#reciprocity
        //
        // ...but with random duplicate edges.  The duplicate edges should not
        // affect the results.

        Boolean bIsDirected = true;
        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();

        Random oRandom = new Random();

        for (Int32 i = 0; i < 10; i++)
        {
            for (Int32 j = 0; j < oRandom.Next(1, 100); j++)
            {
                m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            }

            for (Int32 j = 0; j < oRandom.Next(1, 100); j++)
            {
                m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
            }

            for (Int32 j = 0; j < oRandom.Next(1, 100); j++)
            {
                m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
            }

            Nullable<Double> dReciprocatedVertexPairRatio,
                dReciprocatedEdgeRatio;

            m_oOverallReciprocationCalculator.CalculateGraphMetrics(m_oGraph,
                out dReciprocatedVertexPairRatio, out dReciprocatedEdgeRatio);

            Assert.AreEqual(0.5, dReciprocatedVertexPairRatio.Value);
            Assert.AreEqual(0.667, dReciprocatedEdgeRatio.Value, 0.001);
        }
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics5()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics5()
    {
        // Directed graph, sample from http://
        // faculty.ucr.edu/~hanneman/nettext/C8_Embedding.html#reciprocity
        //
        // ...but with self-loops.  The self-loops should not affect the
        // results.

        Boolean bIsDirected = true;
        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexC, bIsDirected);

        m_oEdges.Add(oVertexA, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexC, oVertexC, bIsDirected);

        Nullable<Double> dReciprocatedVertexPairRatio, dReciprocatedEdgeRatio;

        m_oOverallReciprocationCalculator.CalculateGraphMetrics(m_oGraph,
            out dReciprocatedVertexPairRatio, out dReciprocatedEdgeRatio);

        Assert.AreEqual(0.5, dReciprocatedVertexPairRatio.Value);
        Assert.AreEqual(0.667, dReciprocatedEdgeRatio.Value, 0.001);
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics6()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics6()
    {
        // More complicated example.

        Boolean bIsDirected = true;
        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();
        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();
        IVertex oVertexE = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexC, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexE, bIsDirected);
        m_oEdges.Add(oVertexE, oVertexD, bIsDirected);

        Nullable<Double> dReciprocatedVertexPairRatio, dReciprocatedEdgeRatio;

        m_oOverallReciprocationCalculator.CalculateGraphMetrics(m_oGraph,
            out dReciprocatedVertexPairRatio, out dReciprocatedEdgeRatio);

        Assert.AreEqual(2.0 / 5.0, dReciprocatedVertexPairRatio.Value);
        Assert.AreEqual(4.0 / 7.0, dReciprocatedEdgeRatio.Value, 0.001);
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
        m_oOverallReciprocationCalculator =
            new OverallReciprocationCalculator();

        m_oGraph = new Graph(bIsDirected ?
            GraphDirectedness.Directed : GraphDirectedness.Undirected);

        m_oVertices = m_oGraph.Vertices;
        m_oEdges = m_oGraph.Edges;
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object being tested.

    protected OverallReciprocationCalculator m_oOverallReciprocationCalculator;

    /// The graph being tested.

    protected IGraph m_oGraph;

    /// The graph's vertices;

    protected IVertexCollection m_oVertices;

    /// The graph's Edges;

    protected IEdgeCollection m_oEdges;
}

}
