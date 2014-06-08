using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;
using Smrf.NodeXL.Core;

namespace AnalyzerUnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DConnectorMotifDetectorTest
    {
        public DConnectorMotifDetectorTest()
        {
            m_oDConnectorMotifDetector = null;
            m_oGraph = null;
            m_oVertices = null;
            m_oEdges = null;
        }

        [TestInitializeAttribute]

        public void
        SetUp()
        {
            m_oDConnectorMotifDetector = new DConnectorMotifDetector();
            m_oGraph = new Graph();
            m_oVertices = m_oGraph.Vertices;
            m_oEdges = m_oGraph.Edges;
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethodAttribute]

        public void
        TestCalculateDconnectorMotifs4()
        {
            // Simple two-connector motifs.

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
            IVertex oVertexK = m_oVertices.Add();
            IVertex oVertexL = m_oVertices.Add();

            // Not a two-connector.

            m_oEdges.Add(oVertexA, oVertexB);
            m_oEdges.Add(oVertexA, oVertexC);

            // Two-connector.

            m_oEdges.Add(oVertexD, oVertexF);
            m_oEdges.Add(oVertexD, oVertexG);
            m_oEdges.Add(oVertexE, oVertexF);
            m_oEdges.Add(oVertexE, oVertexG);

            // Two-connector.

            m_oEdges.Add(oVertexH, oVertexK);
            m_oEdges.Add(oVertexH, oVertexL);
            m_oEdges.Add(oVertexI, oVertexK);
            m_oEdges.Add(oVertexI, oVertexL);
            m_oEdges.Add(oVertexJ, oVertexK);
            m_oEdges.Add(oVertexJ, oVertexL);

            ICollection<Motif> oMotifs;


            Assert.IsTrue(m_oDConnectorMotifDetector.TryCalculateDConnectorMotifs(
                m_oGraph, 2, 2, null, out oMotifs));

            Assert.AreEqual(2, oMotifs.Count);

            foreach (Motif oMotif in oMotifs)
            {
                Assert.IsTrue(oMotif is DConnectorMotif);
            }

            VerifyDConnectorMotif(oMotifs, new List<IVertex>() { oVertexF, oVertexG },
                0.0, oVertexD, oVertexE);

            VerifyDConnectorMotif(oMotifs, new List<IVertex>() { oVertexK, oVertexL },
                1.0, oVertexH, oVertexI, oVertexJ);
        }

        protected void VerifyDConnectorMotif
            ( ICollection<Motif> oMotifs, List<IVertex> oExpectedAnchorVertices, Double dExpectedSpanScale,params IVertex[] aoExpectedSpanVertices)
        {
            DConnectorMotif oDConnectorMotif = (DConnectorMotif)oMotifs.Single(
                oMotif =>
                oMotif is DConnectorMotif
                &&
                DConnectorMotifHasExpectedAnchors((DConnectorMotif)oMotif,
                    oExpectedAnchorVertices)
                );

            Assert.AreEqual(dExpectedSpanScale, oDConnectorMotif.SpanScale);

            Assert.IsTrue(UnsortedEquality<IVertex>(aoExpectedSpanVertices, oDConnectorMotif.SpanVertices));
        }

        protected Boolean DConnectorMotifHasExpectedAnchors(
            DConnectorMotif oDConnectorMotif,
            List<IVertex> oExpectedAnchorVertices
            )
        {
            return UnsortedEquality<IVertex>(oExpectedAnchorVertices, oDConnectorMotif.AnchorVertices);
        }

        protected Boolean UnsortedEquality<T>
            (IEnumerable<T> source,IEnumerable<T> destination)
        {
            if (source.Count() != destination.Count())
            {
                return false;
            }

            Dictionary<T, int> dictionary = new Dictionary<T, int>();

            foreach (T value in source)
            {
                if (!dictionary.ContainsKey(value))
                {
                    dictionary[value] = 1;
                }
                else
                {
                    dictionary[value]++;
                }
            }

            foreach (T member in destination)
            {
                if (!dictionary.ContainsKey(member))
                {
                    return false;
                }

                dictionary[member]--;
            }

            foreach (KeyValuePair<T, int> kvp in dictionary)
            {
                if (kvp.Value != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// The object being tested.

        protected DConnectorMotifDetector m_oDConnectorMotifDetector;


        /// The graph being tested.

        protected IGraph m_oGraph;

        /// The graph's vertices;

        protected IVertexCollection m_oVertices;

        /// The graph's Edges;

        protected IEdgeCollection m_oEdges;
    }
}
