using Smrf.AppLib;
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
    public class ReciprocatedVertexPairRatioCalculator : VertexMetricCalculatorBase
    {
        
        public ReciprocatedVertexPairRatioCalculator()
        {
        }

        public override VertexMetricBase Calculate(IGraph graph)
        {
            MetricDouble graphMetrics;
            TryCalculateGraphMetrics(graph, this.getBackgroundWorker(), out graphMetrics);
            return graphMetrics;
        }



        public bool TryCalculateGraphMetrics
            (IGraph graph, BackgroundWorker backgroundWorker, out MetricDouble graphMetrics)
        {
            Debug.Assert(graph != null);

            IVertexCollection oVertices = graph.Vertices;
            Int32 iVertices = oVertices.Count;
            Int32 iCalculations = 0;

            // The key is an IVertex.ID and the value is the vertex's reciprocated
            // vertex pair ratio, or null.

            MetricDouble oReciprocatedVertexPairRatios = new MetricDouble(oVertices.Count, "ReciprocatedVertexPairRatio");
            graphMetrics = oReciprocatedVertexPairRatios;
            if (graph.Directedness == GraphDirectedness.Directed)
            {
                // Contains a key for each of the graph's unique edges.  The key is
                // the edge's ordered vertex ID pair.

                HashSet<Int64> oVertexIDPairs = GetVertexIDPairs(graph);

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

                    oReciprocatedVertexPairRatios.Add(oVertex.ID,
                        CalculateReciprocatedVertexPairRatio(
                            oVertex, oVertexIDPairs) );

                    iCalculations++;
                }
            }

            return (true);
        }

        protected HashSet<Int64> GetVertexIDPairs(IGraph oGraph)
        {
            Debug.Assert(oGraph != null);

            HashSet<Int64> oVertexIDPairs = new HashSet<Int64>();

            foreach (IEdge oEdge in oGraph.Edges)
            {
                if (!oEdge.IsSelfLoop)
                {
                    oVertexIDPairs.Add( CollectionUtil.GetDictionaryKey(
                        oEdge.Vertex1.ID, oEdge.Vertex2.ID, true) );
                }
            }

            return (oVertexIDPairs);
        }

        protected Double CalculateReciprocatedVertexPairRatio
            (IVertex oVertex,HashSet<Int64> oVertexIDPairs)
        {
            Debug.Assert(oVertex != null);
            Debug.Assert(oVertexIDPairs != null);

            Int32 iAdjacentVerticesWithBothEdges = 0;
            Int32 iAdjacentVertices = 0;

            foreach (IVertex oAdjacentVertex in oVertex.AdjacentVertices)
            {
                // Skip self-loops.

                if (oAdjacentVertex != oVertex)
                {
                    iAdjacentVertices++;

                    // Check for edges in both directions.

                    if (
                        oVertexIDPairs.Contains( CollectionUtil.GetDictionaryKey(
                            oVertex.ID, oAdjacentVertex.ID, true) )
                        &&
                        oVertexIDPairs.Contains( CollectionUtil.GetDictionaryKey(
                            oAdjacentVertex.ID, oVertex.ID, true) )
                        )
                    {
                        iAdjacentVerticesWithBothEdges++;
                    }
                }
            }

            if (iAdjacentVertices == 0)
            {
                return (0);
            }

            return ( (Double)iAdjacentVerticesWithBothEdges /
                (Double)iAdjacentVertices );
        }

        protected const Int32 VerticesPerProgressReport = 100;




        
    }
}
