using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;
using Smrf.NodeXL.Core;

namespace AnalyzerUnitTest
{
    /// <summary>
    /// Summary description for PageRankCalculatorTest
    /// </summary>
    [TestClass]
    public class PageRankCalculatorTest
    {
        public PageRankCalculatorTest()
        {
            m_oPageRankCalculator = null;
            m_oGraph = null;
            m_oVertices = null;
            m_oEdges = null;
        }

        [TestMethod]
        public void TestMethod1()
        {
            Boolean bIsDirected = false;
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();
            IVertex oVertexC = m_oVertices.Add();
            IVertex oVertexD = m_oVertices.Add();
            IVertex oVertexE = m_oVertices.Add();
            

            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexD, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexE, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexC, bIsDirected);

            MetricDouble oMetricDouble;
            bool rv = m_oPageRankCalculator.TryCalculateGraphMetrics(m_oGraph, null, out oMetricDouble);

            //Assert.AreEqual(rv, true);
            Assert.AreEqual(oMetricDouble.Count, 5);
            System.Console.WriteLine("{0}, {1}, {2}", oMetricDouble[oVertexA.ID], oMetricDouble[oVertexB.ID], oMetricDouble[oVertexC.ID]);
            Assert.AreEqual(true, Math.Abs(oMetricDouble[oVertexA.ID] - 0.3839) * 500 < oMetricDouble[oVertexA.ID]);
            Assert.AreEqual(true, Math.Abs(oMetricDouble[oVertexB.ID] - 0.1937) * 500 < oMetricDouble[oVertexB.ID]);
            Assert.AreEqual(true, Math.Abs(oMetricDouble[oVertexC.ID] - 0.1937) * 500 < oMetricDouble[oVertexC.ID]);
            Assert.AreEqual(true, Math.Abs(oMetricDouble[oVertexD.ID] - 0.1115) * 500 < oMetricDouble[oVertexD.ID]);
            Assert.AreEqual(true, Math.Abs(oMetricDouble[oVertexE.ID] - 0.1115) * 500 < oMetricDouble[oVertexE.ID]);
           
        }

        [TestMethod]
        public void TestMethod2()
        {
            Boolean bIsDirected = false;
            CreateGraph(bIsDirected);

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();
            IVertex oVertexC = m_oVertices.Add();
            

            m_oEdges.Add(oVertexA, oVertexB, bIsDirected);
            m_oEdges.Add(oVertexB, oVertexC, bIsDirected);
            m_oEdges.Add(oVertexA, oVertexC, bIsDirected);
            

            MetricDouble oMetricDouble;
            bool rv = m_oPageRankCalculator.TryCalculateGraphMetrics(m_oGraph, null, out oMetricDouble);

            Assert.AreEqual(rv, true);
            System.Console.WriteLine("{0}, {1}, {2}", oMetricDouble[oVertexA.ID], oMetricDouble[oVertexB.ID], oMetricDouble[oVertexC.ID]);
            Assert.AreEqual(oMetricDouble.Count, 3);
            Assert.AreEqual(true, (oMetricDouble[oVertexA.ID] - 0.333) < 0.001);
            Assert.AreEqual(true, (oMetricDouble[oVertexB.ID] - 0.333) < 0.001);
            Assert.AreEqual(true, (oMetricDouble[oVertexC.ID] - 0.333) < 0.001);
            
        }

        protected void
        CreateGraph
        (
            Boolean bIsDirected
        )
        {
            m_oPageRankCalculator =
                new PageRankCalculator();

            m_oGraph = new Graph(bIsDirected ?
                GraphDirectedness.Directed : GraphDirectedness.Undirected);

            m_oVertices = m_oGraph.Vertices;
            m_oEdges = m_oGraph.Edges;
        }

        protected PageRankCalculator
            m_oPageRankCalculator;

        /// The graph being tested.

        protected IGraph m_oGraph;

        /// The graph's vertices;

        protected IVertexCollection m_oVertices;

        /// The graph's Edges;

        protected IEdgeCollection m_oEdges;

    }
}
