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
        public override bool tryCalculate(IGraph graph, BackgroundWorker bgw, out VertexMetricBase metrics)
        {
            MetricDouble oMetrics;
            bool rv = TryCalculateGraphMetrics(graph, bgw, out oMetrics);
            metrics = oMetrics;
            return rv;
        }

        public override string CalculatorDescription()
        {
            return "Calculating PageRank";
        }

        public Boolean TryCalculateGraphMetrics
        (
            IGraph graph,
            BackgroundWorker backgroundWorker,
            out MetricDouble graphMetrics
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
                System.Console.WriteLine("V{0}", oVertex.ID);
                oldPageRanks.Add(oVertex.ID, 1.0/oVertices.Count);
                newPageRanks.Add(oVertex.ID, 0);
            }
            int ii = 0;
            while (!isConverged(oldPageRanks, newPageRanks)){
                
                oldPageRanks = newPageRanks;
                if (!ReportProgressAndCheckCancellationPending(0, 100, backgroundWorker))
                {
                    return (false);
                }
                calculateVertexPageRank(oVertices, oldPageRanks, out newPageRanks);
                ii++;
            }
            System.Console.WriteLine("ii = {0}", ii);
            
            foreach (KeyValuePair<Int32, Double> p in newPageRanks) {
                oMetricDouble.Add(p.Key, p.Value);
            }
          

            return (true);
        }

        private bool isConverged
            (Dictionary<Int32, Double> oldPageRanks, Dictionary<Int32, Double> newPageRanks)
        {
            foreach (KeyValuePair<Int32, Double> vpr in oldPageRanks)
            {
                
                double newPR;
                newPageRanks.TryGetValue(vpr.Key, out newPR);
                System.Console.WriteLine("{0} || {1} || {2}", vpr.Key, newPR, vpr.Value);
                if (Math.Abs(newPR - vpr.Value) > (newPR + vpr.Value )*0.0005 || newPR == 0) return false;
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
                    newRank += damping_factor * adjNodeRank / adjNode.AdjacentVertices.Count;
                }
                newPageRanks.Add(node.ID, newRank);
            }
        }





        
    }
}
