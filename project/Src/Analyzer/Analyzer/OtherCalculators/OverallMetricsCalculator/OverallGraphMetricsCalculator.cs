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
    public class OverallMetricCalculator : AnalyzerBase
    {
        public OverallMetricCalculator()
        {
            // (Do nothing.)

        }

        
        public override bool tryAnalyze(IGraph graph, BackgroundWorker bgw, out AnalyzeResultBase results)
        {
            OverallMetrics oOverallMetrics;
            bool rv = TryCalculateGraphMetrics(graph, bgw, out oOverallMetrics);
            results = oOverallMetrics;
            return rv;
        }

        public override string AnalyzerDescription
        {
            get { return "Calculating OverallMetrics"; }
        }
        

        public OverallMetrics CalculateGraphMetrics (IGraph graph)
        {
            Debug.Assert(graph != null);

            OverallMetrics oGraphMetrics;

            TryCalculateGraphMetrics(graph, null, out oGraphMetrics);

            return (oGraphMetrics);
        }


        public Boolean
        TryCalculateGraphMetrics
        (
            IGraph graph,
            BackgroundWorker backgroundWorker,
            out OverallMetrics graphMetrics
        )
        {
            Debug.Assert(graph != null);


            graphMetrics = null;

            if (!ReportProgressAndCheckCancellationPending(
                1, 3, backgroundWorker))
            {
                return (false);
            }

            DuplicateEdgeDetector oDuplicateEdgeDetector =
                new DuplicateEdgeDetector(graph);

            Int32 iVertices = graph.Vertices.Count;
            Int32 iEdges = graph.Edges.Count;
            Int32 iSelfLoops = CountSelfLoops(graph);

            Int32 iConnectedComponents, iSingleVertexConnectedComponents,
                iMaximumConnectedComponentVertices,
                iMaximumConnectedComponentEdges;

            CalculateConnectedComponentMetrics(graph, out iConnectedComponents,
                out iSingleVertexConnectedComponents,
                out iMaximumConnectedComponentVertices,
                out iMaximumConnectedComponentEdges);

            Nullable<Int32> iMaximumGeodesicDistance;
            Nullable<Double> dAverageGeodesicDistance;
            Nullable<Double> dModularity;

            if (!ReportProgressAndCheckCancellationPending(
                2, 3, backgroundWorker))
            {
                return (false);
            }

           // CalculateSnapOverallMetrics(graph, out iMaximumGeodesicDistance, out dAverageGeodesicDistance, out dModularity);

            Nullable<Double> dReciprocatedVertexPairRatio, dReciprocatedEdgeRatio;

            if (!(new OverallReciprocationCalculator())
                .TryCalculateGraphMetrics(graph, backgroundWorker,
                    out dReciprocatedVertexPairRatio, out dReciprocatedEdgeRatio))
            {
                return (false);
            }

            OverallMetrics oOverallMetrics = new OverallMetrics(
                graph.Directedness,
                oDuplicateEdgeDetector.UniqueEdges,
                oDuplicateEdgeDetector.EdgesWithDuplicates,
                iSelfLoops,
                iVertices,

                CalculateGraphDensity(graph, iVertices,
                    oDuplicateEdgeDetector.
                        TotalEdgesAfterMergingDuplicatesNoSelfLoops),

                0,                            //dModularity
                iConnectedComponents,
                iSingleVertexConnectedComponents,
                iMaximumConnectedComponentVertices,
                iMaximumConnectedComponentEdges,
                0,               //iMaximumGeodesicDistance
                0,               //dAverageGeodesicDistance
                dReciprocatedVertexPairRatio,
                dReciprocatedEdgeRatio
                );

            graphMetrics = oOverallMetrics;

            return (true);
        }


        protected Int32 CountSelfLoops(IGraph oGraph)
        {
            Debug.Assert(oGraph != null);

            Int32 iSelfLoops = 0;

            foreach (IEdge oEdge in oGraph.Edges)
            {
                if (oEdge.IsSelfLoop)
                {
                    iSelfLoops++;
                }
            }

            return (iSelfLoops);
        }


        protected Nullable<Double>
        CalculateGraphDensity
        (
            IGraph oGraph,
            Int32 iVertices,
            Int32 iTotalEdgesAfterMergingDuplicatesNoSelfLoops
        )
        {
            Debug.Assert(oGraph != null);
            Debug.Assert(iVertices >= 0);
            Debug.Assert(iTotalEdgesAfterMergingDuplicatesNoSelfLoops >= 0);


            

            Nullable<Double> dGraphDensity = null;

            if (iVertices > 1)
            {
                Double dVertices = (Double)iVertices;

                dGraphDensity =
                    (2 * (Double)iTotalEdgesAfterMergingDuplicatesNoSelfLoops) /
                    (dVertices * (dVertices - 1));

                if (oGraph.Directedness == GraphDirectedness.Directed)
                {
                    dGraphDensity /= 2.0;
                }

                // Don't allow rounding errors to create a very small negative
                // number.

                dGraphDensity = Math.Max(0, dGraphDensity.Value);
            }

            return (dGraphDensity);
        }

       

        protected void CalculateConnectedComponentMetrics
        (
            IGraph oGraph,
            out Int32 iConnectedComponents,
            out Int32 iSingleVertexConnectedComponents,
            out Int32 iMaximumConnectedComponentVertices,
            out Int32 iMaximumConnectedComponentEdges
        )
        {
            Debug.Assert(oGraph != null);


            ConnectedComponentCalculator oConnectedComponentCalculator =
                new ConnectedComponentCalculator();

            IList<LinkedList<IVertex>> oConnectedComponents =
                oConnectedComponentCalculator.CalculateStronglyConnectedComponents(
                    oGraph, true);

            iConnectedComponents = oConnectedComponents.Count;
            iSingleVertexConnectedComponents = 0;
            iMaximumConnectedComponentVertices = 0;
            iMaximumConnectedComponentEdges = 0;

            foreach (LinkedList<IVertex> oConnectedComponent in
                oConnectedComponents)
            {
                Int32 iVertices = oConnectedComponent.Count;

                if (iVertices == 1)
                {
                    iSingleVertexConnectedComponents++;
                }

                iMaximumConnectedComponentVertices = Math.Max(
                    iMaximumConnectedComponentVertices, iVertices);

                iMaximumConnectedComponentEdges = Math.Max(
                    iMaximumConnectedComponentEdges,
                    CountUniqueEdges(oConnectedComponent));
            }
        }


        protected Int32 CountUniqueEdges (LinkedList<IVertex> oConnectedComponent)
        {
            Debug.Assert(oConnectedComponent != null);
        

            HashSet<Int32> oUniqueEdgeIDs = new HashSet<Int32>();

            foreach (IVertex oVertex in oConnectedComponent)
            {
                foreach (IEdge oEdge in oVertex.IncidentEdges)
                {
                    oUniqueEdgeIDs.Add(oEdge.ID);
                }
            }

            return (oUniqueEdgeIDs.Count);
        }


        
    }
}
