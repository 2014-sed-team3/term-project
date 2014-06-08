using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Smrf.NodeXL.Core;
using Analyzer;

namespace AnalyzerUnitTest
{
    [TestClass]
    public class FanMotifDetectorTest
    {
        public FanMotifDetectorTest()
        {
            m_oFanMotifCalculator = null;
            m_oGraph = null;
            m_oVertices = null;
            m_oEdges = null;
        }

        [TestInitializeAttribute]

        public void
        SetUp()
        {
            m_oFanMotifCalculator = new FanMotifDetector();
            m_oGraph = new Graph();
            m_oVertices = m_oGraph.Vertices;
            m_oEdges = m_oGraph.Edges;
        }

        [TestMethodAttribute]
        public void TestCalculateFanMotifs2()
        {
            // Simple fan motifs.

            IVertex oVertexA = m_oVertices.Add();
            IVertex oVertexB = m_oVertices.Add();
            IVertex oVertexC = m_oVertices.Add();
            IVertex oVertexD = m_oVertices.Add();
            IVertex oVertexE = m_oVertices.Add();
            IVertex oVertexF = m_oVertices.Add();
            IVertex oVertexG = m_oVertices.Add();
            IVertex oVertexH = m_oVertices.Add();
            IVertex oVertexI = m_oVertices.Add();
            IVertex oVertexJ = m_oVertices.Add();

            // Not a fan.

            m_oEdges.Add(oVertexA, oVertexB);

            // Fan.

            m_oEdges.Add(oVertexC, oVertexD);
            m_oEdges.Add(oVertexC, oVertexE);
            m_oEdges.Add(oVertexC, oVertexF);
            m_oEdges.Add(oVertexF, oVertexG);

            // Fan.  Note the connector edges in this one.

            m_oEdges.Add(oVertexH, oVertexI);
            m_oEdges.Add(oVertexH, oVertexI);
            m_oEdges.Add(oVertexI, oVertexH);
            m_oEdges.Add(oVertexH, oVertexJ);

            ICollection<Motif> oMotifs;

            Assert.IsTrue(m_oFanMotifCalculator.TryCalculateFanMotif(
                m_oGraph, null, out oMotifs));

            //Assert.AreEqual(2, oMotifs.Count);

            
        /*foreach (Motif oMotif in oMotifs)
            {
                Assert.IsTrue(oMotif is FanMotif);
            }
         * */
        /*
            VerifyFanMotif(oMotifs, oVertexC, 0.5, oVertexD, oVertexE);
            VerifyFanMotif(oMotifs, oVertexH, 0.5, oVertexI, oVertexJ);
         * */
        }

        protected void VerifyFanMotif
            (ICollection<Motif> oMotifs,IVertex oExpectedHeadVertex,Double dExpectedArcScale,params IVertex[] aoExpectedLeafVertices)
        {
            FanMotif oFanMotif = (FanMotif)oMotifs.Single(oMotif =>
                oMotif is FanMotif
                &&
                (((FanMotif)oMotif).HeadVertex == oExpectedHeadVertex));

            Assert.AreEqual(oExpectedHeadVertex, oFanMotif.HeadVertex);

            Assert.AreEqual(aoExpectedLeafVertices.Length,
                oFanMotif.LeafVertices.Length);

            Assert.AreEqual(dExpectedArcScale, oFanMotif.ArcScale);

            foreach (IVertex oExpectedLeafVertex in aoExpectedLeafVertices)
            {
                IVertex oLeafVertex = oFanMotif.LeafVertices.Single(
                    oVertex => (oVertex == oExpectedLeafVertex));
            }
        }

        /// The object being tested.

        protected FanMotifDetector m_oFanMotifCalculator;


        /// The graph being tested.

        protected IGraph m_oGraph;

        /// The graph's vertices;

        protected IVertexCollection m_oVertices;

        /// The graph's Edges;

        protected IEdgeCollection m_oEdges;
    }
}
