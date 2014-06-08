using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class VertexDegreeCalculator : VertexMetricCalculatorBase
    {
       
        public VertexDegreeCalculator()
        {
        }

        public override bool tryCalculate(IGraph graph, BackgroundWorker bgw, out VertexMetricBase metrics)
        {
            VertexMetricBase oMetrics;
            bool rv = TryCalculateGraphMetrics(graph, bgw, out oMetrics);
            metrics = oMetrics;
            return rv;
        }

        public override string CalculatorDescription()
        {
            return "Calculating VertexDegree";
        }


        public Boolean TryCalculateGraphMetrics
            (IGraph graph, BackgroundWorker backgroundWorker, out VertexMetricBase graphMetrics)
        {
            Debug.Assert(graph != null);

            IVertexCollection oVertices = graph.Vertices;
            Int32 iVertices = oVertices.Count;
            Int32 iCalculations = 0;

            VertexDegrees oVertexIDDictionary = new VertexDegrees(iVertices);

            graphMetrics = oVertexIDDictionary;

            foreach (IVertex oVertex in oVertices)
            {
                // Check for cancellation and report progress every
                // VerticesPerProgressReport calculations.

                if (
                    (iCalculations % VerticesPerProgressReport == 0)
                    &&
                    !ReportProgressAndCheckCancellationPending(
                        iCalculations, iVertices, backgroundWorker)
                    )
                {
                    return (false);
                }

                Int32 iInDegree, iOutDegree;

                CalculateVertexDegrees(oVertex, out iInDegree, out iOutDegree);

                oVertexIDDictionary.Add(oVertex.ID, iInDegree, iOutDegree);

                iCalculations++;
            }

            return (true);
        }

        protected void CalculateVertexDegrees
            (IVertex oVertex, out Int32 iInDegree, out Int32 iOutDegree)
        {
            Debug.Assert(oVertex != null);

            iInDegree = 0;
            iOutDegree = 0;

            foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
            {
                IVertex[] aoVertices = oIncidentEdge.Vertices;

                // Test both of the edge's vertices so that a self-loop is properly
                // handled.

                if (aoVertices[0] == oVertex)
                {
                    iOutDegree++;
                }

                if (aoVertices[1] == oVertex)
                {
                    iInDegree++;
                }
            }
        }

        protected const Int32 VerticesPerProgressReport = 100;


        public override bool tryAnalyze(IGraph graph, BackgroundWorker bgw, out AnalyzeResultBase results)
        {
            throw new NotImplementedException();
        }
    }

    
        /*
        public VertexDegrees(Int32 inDegree, Int32 outDegree)
        {
            m_iInDegree = inDegree;
            m_iOutDegree = outDegree;
        }

        public Int32 InDegree
        {
            get
            {
                return (m_iInDegree);
            }
        }

        public Int32 OutDegree
        {
            get
            {
                return (m_iOutDegree);
            }
        }

        public Int32 Degree
        {
            get
            {
                return (m_iInDegree + m_iOutDegree);
            }
        }

        protected Int32 m_iInDegree;

        protected Int32 m_iOutDegree;
         * */
    
}
