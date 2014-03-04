
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: IntergroupEdgeCalculatorTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="IntergroupEdgeCalculator" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class IntergroupEdgeCalculatorTest : Object
{
    //*************************************************************************
    //  Constructor: IntergroupEdgeCalculatorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="IntergroupEdgeCalculatorTest" /> class.
    /// </summary>
    //*************************************************************************

    public IntergroupEdgeCalculatorTest()
    {
        m_oIntergroupEdgeCalculator = null;
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
        m_oIntergroupEdgeCalculator = null;
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

        IList<IntergroupEdgeInfo> oIntergroupEdges =
            m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                new GroupInfo[0], true);

        Assert.AreEqual(0, oIntergroupEdges.Count);
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
        // Self-loops only.

        // Directedness makes no difference in this case.

        foreach (Boolean bIsDirected in TestGraphUtil.AllBoolean)
        {
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();
            IVertex oVertexC = m_oVertices.Add();
            IVertex oVertexD = m_oVertices.Add();

            m_oEdges.Add(oVertexA, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexC, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexD, oVertexD, bIsDirected);

            GroupInfo oGroup1 = new GroupInfo();
            oGroup1.Vertices.AddLast(oVertexA);
            oGroup1.Vertices.AddLast(oVertexB);

            GroupInfo oGroup2 = new GroupInfo();
            oGroup2.Vertices.AddLast(oVertexC);

            GroupInfo oGroup3 = new GroupInfo();
            oGroup3.Vertices.AddLast(oVertexD);

            // Using directedness makes no difference in this case.

            foreach (Boolean bUseDirectedness in TestGraphUtil.AllBoolean)
            {
                IList<IntergroupEdgeInfo> oIntergroupEdges =
                    m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                        new GroupInfo[] {oGroup1, oGroup2, oGroup3},
                        bUseDirectedness);

                Assert.AreEqual(3, oIntergroupEdges.Count);

                Assert.AreEqual( 2, GetEdgeCount(oIntergroupEdges, 0, 0) );
                Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 1, 1) );
                Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 2, 2) );
            }
        }
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
        // Intragroup edges only.

        // Directedness makes no difference in this case.

        foreach (Boolean bIsDirected in TestGraphUtil.AllBoolean)
        {
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();

            IVertex oVertexC = m_oVertices.Add();
            IVertex oVertexD = m_oVertices.Add();

            IVertex oVertexE = m_oVertices.Add();
            IVertex oVertexF = m_oVertices.Add();

            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexC, oVertexD, bIsDirected);
            m_oEdges.Add(oVertexE, oVertexF, bIsDirected);

            GroupInfo oGroup1 = new GroupInfo();
            oGroup1.Vertices.AddLast(oVertexA);
            oGroup1.Vertices.AddLast(oVertexB);

            GroupInfo oGroup2 = new GroupInfo();
            oGroup2.Vertices.AddLast(oVertexC);
            oGroup2.Vertices.AddLast(oVertexD);

            GroupInfo oGroup3 = new GroupInfo();
            oGroup3.Vertices.AddLast(oVertexE);
            oGroup3.Vertices.AddLast(oVertexF);

            // Using directedness makes no difference in this case.

            foreach (Boolean bUseDirectedness in TestGraphUtil.AllBoolean)
            {
                IList<IntergroupEdgeInfo> oIntergroupEdges =
                    m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                        new GroupInfo[] {oGroup1, oGroup2, oGroup3},
                        bUseDirectedness);

                Assert.AreEqual(3, oIntergroupEdges.Count);

                Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 0, 0) );
                Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 1, 1) );
                Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 2, 2) );
            }
        }
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
        // More complex case, undirected.

        Boolean bIsDirected = false;

        // Using directedness makes no difference in this case.

        foreach (Boolean bUseDirectedness in TestGraphUtil.AllBoolean)
        {
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();

            IVertex oVertexC = m_oVertices.Add();
            IVertex oVertexD = m_oVertices.Add();

            IVertex oVertexE = m_oVertices.Add();
            IVertex oVertexF = m_oVertices.Add();

            m_oEdges.Add(oVertexA, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
            m_oEdges.Add(oVertexC, oVertexD, bIsDirected);
            m_oEdges.Add(oVertexE, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexD, oVertexE, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexE, bIsDirected);
            m_oEdges.Add(oVertexF, oVertexD, bIsDirected);

            GroupInfo oGroup1 = new GroupInfo();
            oGroup1.Vertices.AddLast(oVertexA);
            oGroup1.Vertices.AddLast(oVertexB);

            GroupInfo oGroup2 = new GroupInfo();
            oGroup2.Vertices.AddLast(oVertexC);
            oGroup2.Vertices.AddLast(oVertexD);

            GroupInfo oGroup3 = new GroupInfo();
            oGroup3.Vertices.AddLast(oVertexE);
            oGroup3.Vertices.AddLast(oVertexF);

            IList<IntergroupEdgeInfo> oIntergroupEdges =
                m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                    new GroupInfo[] {oGroup1, oGroup2, oGroup3},
                    bUseDirectedness);

            Assert.AreEqual(5, oIntergroupEdges.Count);

            Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 0, 0) );
            Assert.AreEqual( 2, GetEdgeCount(oIntergroupEdges, 0, 1) );
            Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 0, 2) );
            Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 1, 1) );
            Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 1, 2) );
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
        // More complex case, directed, don't use directedness.

        Boolean bIsDirected = true;
        Boolean bUseDirectedness = false;

        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();

        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();

        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexC, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexE, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexE, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexE, bIsDirected);
        m_oEdges.Add(oVertexF, oVertexD, bIsDirected);

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast(oVertexA);
        oGroup1.Vertices.AddLast(oVertexB);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast(oVertexC);
        oGroup2.Vertices.AddLast(oVertexD);

        GroupInfo oGroup3 = new GroupInfo();
        oGroup3.Vertices.AddLast(oVertexE);
        oGroup3.Vertices.AddLast(oVertexF);

        IList<IntergroupEdgeInfo> oIntergroupEdges =
            m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                new GroupInfo[] {oGroup1, oGroup2, oGroup3},
                bUseDirectedness);

        Assert.AreEqual(5, oIntergroupEdges.Count);

        Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 0, 0) );
        Assert.AreEqual( 2, GetEdgeCount(oIntergroupEdges, 0, 1) );
        Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 0, 2) );
        Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 1, 1) );
        Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 1, 2) );
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
        // More complex case, directed, use directedness.

        Boolean bIsDirected = true;
        Boolean bUseDirectedness = true;

        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();

        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();

        IVertex oVertexE = m_oVertices.Add();
        IVertex oVertexF = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexC, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexE, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexE, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexE, bIsDirected);
        m_oEdges.Add(oVertexF, oVertexD, bIsDirected);

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast(oVertexA);
        oGroup1.Vertices.AddLast(oVertexB);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast(oVertexC);
        oGroup2.Vertices.AddLast(oVertexD);

        GroupInfo oGroup3 = new GroupInfo();
        oGroup3.Vertices.AddLast(oVertexE);
        oGroup3.Vertices.AddLast(oVertexF);

        IList<IntergroupEdgeInfo> oIntergroupEdges =
            m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                new GroupInfo[] {oGroup1, oGroup2, oGroup3},
                bUseDirectedness);

        Assert.AreEqual(6, oIntergroupEdges.Count);

        Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 0, 0) );
        Assert.AreEqual( 2, GetEdgeCount(oIntergroupEdges, 0, 1) );
        Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 0, 2) );
        Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 1, 1) );
        Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 1, 2) );
        Assert.AreEqual( 2, GetEdgeCount(oIntergroupEdges, 2, 1) );
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics7()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics7()
    {
        // Many parallel edges, undirected.

        Boolean bIsDirected = false;

        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();

        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);

        m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexC, oVertexB, bIsDirected);

        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);

        m_oEdges.Add(oVertexD, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexC, bIsDirected);

        m_oEdges.Add(oVertexD, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexD, bIsDirected);

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast(oVertexA);
        oGroup1.Vertices.AddLast(oVertexB);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast(oVertexC);
        oGroup2.Vertices.AddLast(oVertexD);

        // Using directedness makes no difference in this case.

        foreach (Boolean bUseDirectedness in TestGraphUtil.AllBoolean)
        {
            IList<IntergroupEdgeInfo> oIntergroupEdges =
                m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                    new GroupInfo[] {oGroup1, oGroup2},
                    bUseDirectedness);

            Assert.AreEqual(3, oIntergroupEdges.Count);

            Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 0, 0) );
            Assert.AreEqual( 6, GetEdgeCount(oIntergroupEdges, 0, 1) );
            Assert.AreEqual( 4, GetEdgeCount(oIntergroupEdges, 1, 1) );
        }
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics8()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics8()
    {
        // Many parallel edges, directed, don't use directedness.

        Boolean bIsDirected = true;
        Boolean bUseDirectedness = false;

        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();

        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);

        m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexC, oVertexB, bIsDirected);

        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);

        m_oEdges.Add(oVertexD, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexC, bIsDirected);

        m_oEdges.Add(oVertexD, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexD, bIsDirected);

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast(oVertexA);
        oGroup1.Vertices.AddLast(oVertexB);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast(oVertexC);
        oGroup2.Vertices.AddLast(oVertexD);

        IList<IntergroupEdgeInfo> oIntergroupEdges =
            m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                new GroupInfo[] {oGroup1, oGroup2},
                bUseDirectedness);

        Assert.AreEqual(3, oIntergroupEdges.Count);

        Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 0, 0) );
        Assert.AreEqual( 6, GetEdgeCount(oIntergroupEdges, 0, 1) );
        Assert.AreEqual( 4, GetEdgeCount(oIntergroupEdges, 1, 1) );
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics9()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics9()
    {
        // Many parallel edges, directed, use directedness.

        Boolean bIsDirected = true;
        Boolean bUseDirectedness = true;

        CreateGraph(bIsDirected);

        IVertex oVertexA = m_oVertices.Add();
        IVertex oVertexB = m_oVertices.Add();

        IVertex oVertexC = m_oVertices.Add();
        IVertex oVertexD = m_oVertices.Add();

        m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
        m_oEdges.Add(oVertexB, oVertexA, bIsDirected);

        m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexC, oVertexB, bIsDirected);

        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexA, oVertexD, bIsDirected);

        m_oEdges.Add(oVertexD, oVertexC, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexC, bIsDirected);

        m_oEdges.Add(oVertexD, oVertexD, bIsDirected);
        m_oEdges.Add(oVertexD, oVertexD, bIsDirected);

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast(oVertexA);
        oGroup1.Vertices.AddLast(oVertexB);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast(oVertexC);
        oGroup2.Vertices.AddLast(oVertexD);

        IList<IntergroupEdgeInfo> oIntergroupEdges =
            m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                new GroupInfo[] {oGroup1, oGroup2},
                bUseDirectedness);

        Assert.AreEqual(4, oIntergroupEdges.Count);

        Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 0, 0) );
        Assert.AreEqual( 5, GetEdgeCount(oIntergroupEdges, 0, 1) );
        Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 1, 0) );
        Assert.AreEqual( 4, GetEdgeCount(oIntergroupEdges, 1, 1) );
    }

    //*************************************************************************
    //  Method: TestCalculateGraphMetrics10()
    //
    /// <summary>
    /// Tests the CalculateGraphMetrics() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCalculateGraphMetrics10()
    {
        // More complex case, undirected, check edge weight sum.

        Boolean bIsDirected = false;

        // Using directedness makes no difference in this case.

        foreach (Boolean bUseDirectedness in TestGraphUtil.AllBoolean)
        {
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();

            IVertex oVertexC = m_oVertices.Add();
            IVertex oVertexD = m_oVertices.Add();

            IVertex oVertexE = m_oVertices.Add();
            IVertex oVertexF = m_oVertices.Add();

            IEdge oEdge;

            m_oEdges.Add(oVertexA, oVertexA, bIsDirected);

            oEdge = m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.11);

            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
            m_oEdges.Add(oVertexC, oVertexD, bIsDirected);

            oEdge = m_oEdges.Add(oVertexE, oVertexC, bIsDirected);
            oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 123.0);

            // An edge weight of 0 is counted as 1.0.

            oEdge = m_oEdges.Add(oVertexD, oVertexE, bIsDirected);
            oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 0.0);

            m_oEdges.Add(oVertexA, oVertexE, bIsDirected);

            // A negative edge weight is counted as 1.0.

            oEdge = m_oEdges.Add(oVertexF, oVertexD, bIsDirected);
            oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, -123.0);

            GroupInfo oGroup1 = new GroupInfo();
            oGroup1.Vertices.AddLast(oVertexA);
            oGroup1.Vertices.AddLast(oVertexB);

            GroupInfo oGroup2 = new GroupInfo();
            oGroup2.Vertices.AddLast(oVertexC);
            oGroup2.Vertices.AddLast(oVertexD);

            GroupInfo oGroup3 = new GroupInfo();
            oGroup3.Vertices.AddLast(oVertexE);
            oGroup3.Vertices.AddLast(oVertexF);

            IList<IntergroupEdgeInfo> oIntergroupEdges =
                m_oIntergroupEdgeCalculator.CalculateGraphMetrics(m_oGraph,
                    new GroupInfo[] {oGroup1, oGroup2, oGroup3},
                    bUseDirectedness);

            Assert.AreEqual(5, oIntergroupEdges.Count);

            Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 0, 0) );
            Assert.AreEqual( 2, GetEdgeCount(oIntergroupEdges, 0, 1) );
            Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 0, 2) );
            Assert.AreEqual( 1, GetEdgeCount(oIntergroupEdges, 1, 1) );
            Assert.AreEqual( 3, GetEdgeCount(oIntergroupEdges, 1, 2) );

            Assert.AreEqual( 3.11,
                Math.Round(GetEdgeWeightSum(oIntergroupEdges, 0, 0), 2 ) );

            Assert.AreEqual( 2.0, GetEdgeWeightSum(oIntergroupEdges, 0, 1) );
            Assert.AreEqual( 1.0, GetEdgeWeightSum(oIntergroupEdges, 0, 2) );
            Assert.AreEqual( 1.0, GetEdgeWeightSum(oIntergroupEdges, 1, 1) );
            Assert.AreEqual( 125.0, GetEdgeWeightSum(oIntergroupEdges, 1, 2) );
        }
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
        m_oIntergroupEdgeCalculator = new IntergroupEdgeCalculator();

        m_oGraph = new Graph(bIsDirected ?
            GraphDirectedness.Directed : GraphDirectedness.Undirected);

        m_oVertices = m_oGraph.Vertices;
        m_oEdges = m_oGraph.Edges;
    }

    //*************************************************************************
    //  Method: GetEdgeCount()
    //
    /// <summary>
    /// Searches for a required IntergroupEdgeInfo object and returns the
    /// number of edges it contains.
    /// </summary>
    ///
    /// <param name="oIntergroupEdges">
    /// Collection of IntergroupEdgeInfo objects to search.
    /// </param>
    ///
    /// <param name="iGroup1Index">
    /// The Group1Index value to search for.
    /// </param>
    ///
    /// <param name="iGroup2Index">
    /// The Group2Index value to search for.
    /// </param>
    ///
    /// <returns>
    /// The number of edges in the IntergroupEdgeInfo object.
    /// </returns>
    //*************************************************************************

    protected Int32
    GetEdgeCount
    (
        IList<IntergroupEdgeInfo> oIntergroupEdges,
        Int32 iGroup1Index,
        Int32 iGroup2Index
    )
    {
        IntergroupEdgeInfo oIntergroupEdgeInfo = GetIntergroupEdgeInfo(
            oIntergroupEdges, iGroup1Index, iGroup2Index);

        return (oIntergroupEdgeInfo.Edges);
    }

    //*************************************************************************
    //  Method: GetEdgeWeightSum()
    //
    /// <summary>
    /// Searches for a required IntergroupEdgeInfo object and returns its edge
    /// weight sum.
    /// </summary>
    ///
    /// <param name="oIntergroupEdges">
    /// Collection of IntergroupEdgeInfo objects to search.
    /// </param>
    ///
    /// <param name="iGroup1Index">
    /// The Group1Index value to search for.
    /// </param>
    ///
    /// <param name="iGroup2Index">
    /// The Group2Index value to search for.
    /// </param>
    ///
    /// <returns>
    /// The edge weight sum in the IntergroupEdgeInfo object.
    /// </returns>
    //*************************************************************************

    protected Double
    GetEdgeWeightSum
    (
        IList<IntergroupEdgeInfo> oIntergroupEdges,
        Int32 iGroup1Index,
        Int32 iGroup2Index
    )
    {
        IntergroupEdgeInfo oIntergroupEdgeInfo = GetIntergroupEdgeInfo(
            oIntergroupEdges, iGroup1Index, iGroup2Index);

        return (oIntergroupEdgeInfo.EdgeWeightSum);
    }

    //*************************************************************************
    //  Method: GetIntergroupEdgeInfo()
    //
    /// <summary>
    /// Returns a required IntergroupEdgeInfo object.
    /// </summary>
    ///
    /// <param name="oIntergroupEdges">
    /// Collection of IntergroupEdgeInfo objects to search.
    /// </param>
    ///
    /// <param name="iGroup1Index">
    /// The Group1Index value to search for.
    /// </param>
    ///
    /// <param name="iGroup2Index">
    /// The Group2Index value to search for.
    /// </param>
    ///
    /// <returns>
    /// The specified IntergroupEdgeInfo object.
    /// </returns>
    //*************************************************************************

    protected IntergroupEdgeInfo
    GetIntergroupEdgeInfo
    (
        IList<IntergroupEdgeInfo> oIntergroupEdges,
        Int32 iGroup1Index,
        Int32 iGroup2Index
    )
    {
        return ( oIntergroupEdges.Single(
            intergroupEdgeInfo =>
            intergroupEdgeInfo.Group1Index == iGroup1Index
            &&
            intergroupEdgeInfo.Group2Index == iGroup2Index
            ) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The object being tested.

    protected IntergroupEdgeCalculator m_oIntergroupEdgeCalculator;

    /// The graph being tested.

    protected IGraph m_oGraph;

    /// The graph's vertices;

    protected IVertexCollection m_oVertices;

    /// The graph's Edges;

    protected IEdgeCollection m_oEdges;
}

}
