using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smrf.NodeXL.Core;
using System.ComponentModel;
using System.Diagnostics;

namespace Analyzer
{
    public class PageRankCalculator : VertexMetricCalculatorBase
    {
        private double damping_factor;

        public PageRankCalculator()
        {
            damping_factor = 0.85;
        }

        public override VertexMetricBase Calculate(IGraph graph)
        {
            VertexMetricBase graphMetrics;
            TryCalculateGraphMetrics(graph, this.getBackgroundWorker(), out graphMetrics);
            return graphMetrics;
        }

        public Boolean TryCalculateGraphMetrics
        (
            IGraph graph,
            BackgroundWorker backgroundWorker,
            out VertexMetricBase graphMetrics
        )
        {
            Debug.Assert(graph != null);
            

            IVertexCollection oVertices = graph.Vertices;

            /** initialize PR per vertex **/
            Dictionary<Int32, Double> oldPageRanks = new Dictionary<Int32, Double>(oVertices.Count);
            Dictionary<Int32, Double> newPageRanks = new Dictionary<Int32, Double>(oVertices.Count);
            MetricDouble oMetricDouble = new MetricDouble(oVertices.Count, "PageRank");
            graphMetrics = oMetricDouble;

            foreach(IVertex oVertex in oVertices){
                oldPageRanks.Add(oVertex.ID, 1/oVertices.Count);
                newPageRanks.Add(oVertex.ID, 0);
            }

            while (!isConverged(oldPageRanks, newPageRanks)){
                calculateVertexPageRank(oVertices, oldPageRanks, out newPageRanks);
            }
            
            foreach (KeyValuePair<Int32, Double> p in newPageRanks) {
                oMetricDouble.Add(p.Key, p.Value);
            }
            /*
            graphMetrics = oPageRanks;

            Boolean bGraphIsDirected = (graph.Directedness == GraphDirectedness.Directed);

            Int32 iCalculations = 0;

            foreach (IVertex oVertex in oVertices)
            {
                if (
                    iCalculations % VerticesPerProgressReport == 0
                    &&
                    !ReportProgressAndCheckCancellationPending(
                        iCalculations, iVertices, backgroundWorker)
                    )
                {
                    return (false);
                }

                oPageRanks.Add(oVertex.ID,
                    CalculateClusteringCoefficient(oVertex, bGraphIsDirected) );

                iCalculations++;
            }
             */
          

            return (true);
        }

        private bool isConverged
            (Dictionary<Int32, Double> oldPageRanks, Dictionary<Int32, Double> newPageRanks)
        {
            foreach (KeyValuePair<Int32, Double> vpr in oldPageRanks)
            {
                double newPR;
                newPageRanks.TryGetValue(vpr.Key, out newPR);
                if (newPR - vpr.Value > 0.001) return false;
            }
            return true;
        }

        private void calculateVertexPageRank(IVertexCollection vertices, Dictionary<Int32, Double> oldPageRanks, out Dictionary<Int32, Double> newPageRanks)
        {
            newPageRanks = new Dictionary<Int32, Double>(vertices.Count);
            foreach(IVertex node in vertices){
                ICollection<IVertex> adjacentNodes = node.AdjacentVertices;
                double newRank = (1 - damping_factor) / vertices.Count;
                foreach (IVertex adjNode in adjacentNodes) {
                    double adjNodeRank;
                    oldPageRanks.TryGetValue(adjNode.ID, out adjNodeRank);
                    newRank += adjNodeRank / adjNode.AdjacentVertices.Count;
                }
                newPageRanks.Add(node.ID, newRank);
            }
        }

        
    }
}
