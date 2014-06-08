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
    public class ClusteringCoefficientCalculator : VertexMetricCalculatorBase
    {

        public ClusteringCoefficientCalculator()
        {
        }

        public override bool tryCalculate(IGraph graph, BackgroundWorker bgw, out VertexMetricBase metrics)
        {
            MetricDouble oMetricDouble;
            bool rv = TryCalculateGraphMetrics(graph, bgw, out oMetricDouble);
            if (rv == true)
                metrics = oMetricDouble;
            else
                metrics = new MetricDouble(1);
            return rv;
        }

        public override string CalculatorDescription()
        {
            return "Calculating ClusteringCoefficient";
        }

        public bool TryCalculateGraphMetrics
            (IGraph graph, BackgroundWorker backgroundWorker, out MetricDouble graphMetrics)
        {
            Debug.Assert(graph != null);

            IVertexCollection oVertices = graph.Vertices;
            Int32 iVertices = oVertices.Count;

            MetricDouble oClusteringCoefficients = new MetricDouble(iVertices, "ClusteringCoefficient");
            graphMetrics = oClusteringCoefficients;

            bool bGraphIsDirected = (graph.Directedness == GraphDirectedness.Directed);

            Int32 iCalculations = 0;

            foreach (IVertex oVertex in oVertices)
            {
                if (iCalculations % VerticesPerProgressReport == 0 &&
                    !ReportProgressAndCheckCancellationPending(iCalculations, iVertices, backgroundWorker))
                {
                    return (false);
                }

                oClusteringCoefficients.Add(oVertex.ID, CalculateClusteringCoefficient(oVertex, bGraphIsDirected));

                iCalculations++;
            }

            return (true);
        }

        protected Double CalculateClusteringCoefficient
            (IVertex oVertex, Boolean bGraphIsDirected)
        {
            Debug.Assert(oVertex != null);

            ICollection<IVertex> oAdjacentVertices = oVertex.AdjacentVertices;
            Int32 iAdjacentVertices = 0;
            Int32 iVertexID = oVertex.ID;

            // Create a HashSet of the vertex's adjacent vertices.

            HashSet<Int32> oAdjacentVertexIDs = new HashSet<Int32>();

            foreach (IVertex oAdjacentVertex in oAdjacentVertices)
            {
                Int32 iAdjacentVertexID = oAdjacentVertex.ID;

                // Skip self-loops.

                if (iAdjacentVertexID == iVertexID)
                {
                    continue;
                }

                oAdjacentVertexIDs.Add(iAdjacentVertexID);
                iAdjacentVertices++;
            }

            if (iAdjacentVertices == 0)
            {
                return (0);
            }

            // Create a HashSet of the unique edges in the vertex's neighborhood.
            // These are the edges that connect adjacent vertices to each other but
            // not to the vertex.  The key is the IEdge.ID.

            HashSet<Int32> oEdgesInNeighborhood = new HashSet<Int32>();

            // Loop through the vertex's adjacent vertices.

            foreach (IVertex oAdjacentVertex in oAdjacentVertices)
            {
                // Skip self-loops.

                if (oAdjacentVertex.ID == iVertexID)
                {
                    continue;
                }

                // Loop through the adjacent vertex's incident edges.

                foreach (IEdge oIncidentEdge in oAdjacentVertex.IncidentEdges)
                {
                    if (oIncidentEdge.IsSelfLoop)
                    {
                        continue;
                    }

                    // If this incident edge connects the adjacent vertex to
                    // another adjacent vertex, add it to the HashSet if it isn't
                    // already there.

                    if (oAdjacentVertexIDs.Contains(
                        oIncidentEdge.GetAdjacentVertex(oAdjacentVertex).ID))
                    {
                        oEdgesInNeighborhood.Add(oIncidentEdge.ID);
                    }
                }
            }

            Double dNumerator = (Double)oEdgesInNeighborhood.Count;

            Debug.Assert(iAdjacentVertices > 0);

            Double dDenominator = CalculateEdgesInFullyConnectedNeighborhood(
                iAdjacentVertices, bGraphIsDirected);

            if (dDenominator == 0)
            {
                return (0);
            }

            return (dNumerator / dDenominator);
        }

        protected Int32 CalculateEdgesInFullyConnectedNeighborhood
            (Int32 iAdjacentVertices,Boolean bGraphIsDirected)
        {
            Debug.Assert(iAdjacentVertices >= 0);
          

            return ((iAdjacentVertices * (iAdjacentVertices - 1)) /
                (bGraphIsDirected ? 1 : 2));
        }

        protected const Int32 VerticesPerProgressReport = 100;







        
    }
}
