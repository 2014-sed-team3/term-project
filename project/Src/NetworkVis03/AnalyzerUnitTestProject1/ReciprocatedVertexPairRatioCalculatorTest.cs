using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;
using Smrf.NodeXL.Core;
using System.Collections.Generic;

namespace AnalyzerUnitTest
{
    [TestClassAttribute]
    public class ReciprocatedVertexPairRatioCalculatorTest : Object
    {
        //*************************************************************************
        //  Constructor: ReciprocatedVertexPairRatioCalculatorTest()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ReciprocatedVertexPairRatioCalculatorTest" /> class.
        /// </summary>
        //*************************************************************************

        public ReciprocatedVertexPairRatioCalculatorTest()
        {
            m_oReciprocatedVertexPairRatioCalculator = null;
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
            m_oReciprocatedVertexPairRatioCalculator = null;
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

            MetricDouble oMetricDouble;
            bool rv = m_oReciprocatedVertexPairRatioCalculator.TryCalculateGraphMetrics(m_oGraph, null, out oMetricDouble);

            Assert.AreEqual(true, rv);
            Assert.AreEqual(0, oMetricDouble.Count);
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

            MetricDouble oMetricDouble;
            bool rv = m_oReciprocatedVertexPairRatioCalculator.TryCalculateGraphMetrics(m_oGraph, null, out oMetricDouble);


            Assert.AreEqual(true, rv);
            Assert.AreEqual(0, oMetricDouble.Count);
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
            // Directed graph, sample from Derek Hansen.

            Boolean bIsDirected = true;
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();
            IVertex oVertexC = m_oVertices.Add();

            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexC, bIsDirected);

            MetricDouble oMetricDouble;
            bool rv = m_oReciprocatedVertexPairRatioCalculator.TryCalculateGraphMetrics(m_oGraph, null, out oMetricDouble);

            Assert.AreEqual(true, rv);
            Assert.AreEqual(3, oMetricDouble.Count);

            Assert.AreEqual(0.5, oMetricDouble[oVertexA.ID]);
            Assert.AreEqual(1.0, oMetricDouble[oVertexB.ID]);
            Assert.AreEqual(0.0, oMetricDouble[oVertexC.ID]);
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
            // Directed graph, sample from Derek Hansen, but with self-loops.  The
            // self-loops should not affect the results.

            Boolean bIsDirected = true;
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();
            IVertex oVertexC = m_oVertices.Add();

            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexC, bIsDirected);

            m_oEdges.Add(oVertexA, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexC, oVertexC, bIsDirected);

            MetricDouble oMetricDouble;
            bool rv = m_oReciprocatedVertexPairRatioCalculator.TryCalculateGraphMetrics(m_oGraph, null, out oMetricDouble);

            Assert.AreEqual(true, rv);
            Assert.AreEqual(3, oMetricDouble.Count);

            Assert.AreEqual(0.5, oMetricDouble[oVertexA.ID]);
            Assert.AreEqual(1.0, oMetricDouble[oVertexB.ID]);
            Assert.AreEqual(0.0, oMetricDouble[oVertexC.ID]);
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
            // More complicated example, including isolate.

            Boolean bIsDirected = true;
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();
            IVertex oVertexC = m_oVertices.Add();
            IVertex oVertexD = m_oVertices.Add();
            IVertex oVertexE = m_oVertices.Add();
            IVertex oVertexF = m_oVertices.Add();

            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexC, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
            m_oEdges.Add(oVertexD, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexD, oVertexE, bIsDirected);
            m_oEdges.Add(oVertexE, oVertexD, bIsDirected);

            MetricDouble oMetricDouble;
            bool rv = m_oReciprocatedVertexPairRatioCalculator.TryCalculateGraphMetrics(m_oGraph, null, out oMetricDouble);

            Assert.AreEqual(6, oMetricDouble.Count);

            Assert.AreEqual(2.0 / 3.0, oMetricDouble[oVertexA.ID]);
            Assert.AreEqual(0.0 / 2.0, oMetricDouble[oVertexB.ID]);
            Assert.AreEqual(1.0 / 2.0, oMetricDouble[oVertexC.ID]);
            Assert.AreEqual(2.0 / 2.0, oMetricDouble[oVertexD.ID]);
            Assert.AreEqual(1.0 / 1.0, oMetricDouble[oVertexE.ID]);
            Assert.AreEqual(0, oMetricDouble[oVertexF.ID]);
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
            // More complicated example, including isolate, but with self-loops.
            // The self-loops should not affect the results.

            Boolean bIsDirected = true;
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();
            IVertex oVertexC = m_oVertices.Add();
            IVertex oVertexD = m_oVertices.Add();
            IVertex oVertexE = m_oVertices.Add();
            IVertex oVertexF = m_oVertices.Add();

            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexC, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
            m_oEdges.Add(oVertexD, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexD, oVertexE, bIsDirected);
            m_oEdges.Add(oVertexE, oVertexD, bIsDirected);

            m_oEdges.Add(oVertexA, oVertexA, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexC, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexD, oVertexD, bIsDirected);
            m_oEdges.Add(oVertexE, oVertexE, bIsDirected);
            m_oEdges.Add(oVertexF, oVertexF, bIsDirected);

            MetricDouble oMetricDouble;
            bool rv = m_oReciprocatedVertexPairRatioCalculator.TryCalculateGraphMetrics(m_oGraph, null, out oMetricDouble);

            Assert.AreEqual(6, oMetricDouble.Count);

            Assert.AreEqual(2.0 / 3.0, oMetricDouble[oVertexA.ID]);
            Assert.AreEqual(0.0 / 2.0, oMetricDouble[oVertexB.ID]);
            Assert.AreEqual(1.0 / 2.0, oMetricDouble[oVertexC.ID]);
            Assert.AreEqual(2.0 / 2.0, oMetricDouble[oVertexD.ID]);
            Assert.AreEqual(1.0 / 1.0, oMetricDouble[oVertexE.ID]);
            Assert.AreEqual(0, oMetricDouble[oVertexF.ID]);
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
            m_oReciprocatedVertexPairRatioCalculator =
                new ReciprocatedVertexPairRatioCalculator();

            m_oGraph = new Graph(bIsDirected ?
                GraphDirectedness.Directed : GraphDirectedness.Undirected);

            m_oVertices = m_oGraph.Vertices;
            m_oEdges = m_oGraph.Edges;
        }


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// The object being tested.

        protected ReciprocatedVertexPairRatioCalculator
            m_oReciprocatedVertexPairRatioCalculator;

        /// The graph being tested.

        protected IGraph m_oGraph;

        /// The graph's vertices;

        protected IVertexCollection m_oVertices;

        /// The graph's Edges;

        protected IEdgeCollection m_oEdges;
    }
}
