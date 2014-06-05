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
    public class OverallMetricCalculator : IAnalyzer
    {
        private BackgroundWorker m_obackgroundWorker;

        public OverallMetricCalculator()
        {
            // (Do nothing.)

        }

        public string GraphMetricDescription
        {
            get { return "~~~"; }
        }

        public void setBackgroundWorker(BackgroundWorker b)
        {
            m_obackgroundWorker = b;
        }


        public AnalyzeResultBase analyze(IGraph graph){
            throw new NotImplementedException();
        }
        

        //*************************************************************************
        //  Method: CalculateGraphMetrics()
        //
        /// <summary>
        /// Calculate the graph metrics.
        /// </summary>
        ///
        /// <param name="graph">
        /// The graph to calculate metrics for.  The graph may contain duplicate
        /// edges and self-loops.
        /// </param>
        ///
        /// <returns>
        /// The graph metrics.
        /// </returns>
        //*************************************************************************

        public OverallMetrics CalculateGraphMetrics
            (IGraph graph)
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

        //*************************************************************************
        //  Method: CountSelfLoops()
        //
        /// <summary>
        /// Counts the number of self-loops in the graph.
        /// </summary>
        ///
        /// <param name="oGraph">
        /// The graph to calculate metrics for.
        /// </param>
        ///
        /// <returns>
        /// The number of self-loops in the graph.
        /// </returns>
        //*************************************************************************

        protected Int32
        CountSelfLoops
        (
            IGraph oGraph
        )
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

        //*************************************************************************
        //  Method: CalculateGraphDensity()
        //
        /// <summary>
        /// Calculates the graph's density.
        /// </summary>
        ///
        /// <param name="oGraph">
        /// The graph to calculate metrics for.
        /// </param>
        ///
        /// <param name="iVertices">
        /// The number of vertices in the graph.
        /// </param>
        ///
        /// <param name="iTotalEdgesAfterMergingDuplicatesNoSelfLoops">
        /// The total number of edges the graph would have if its duplicate edges
        /// were merged and all self-loops were removed.
        /// </param>
        ///
        /// <returns>
        /// The graph density, or null if it couldn't be calculated.
        /// </returns>
        //*************************************************************************

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

        //*************************************************************************
        //  Method: CalculateConnectedComponentMetrics()
        //
        /// <summary>
        /// Calculates the graph's connected component metrics.
        /// </summary>
        ///
        /// <param name="oGraph">
        /// The graph to calculate metrics for.
        /// </param>
        ///
        /// <param name="iConnectedComponents">
        /// Where the number of connected components in the graph gets stored.
        /// </param>
        ///
        /// <param name="iSingleVertexConnectedComponents">
        /// Where the number of connected components in the graph that have one
        /// vertex gets stored.
        /// </param>
        ///
        /// <param name="iMaximumConnectedComponentVertices">
        /// Where the maximum number of vertices in a connected component gets
        /// stored.
        /// </param>
        ///
        /// <param name="iMaximumConnectedComponentEdges">
        /// Where the maximum number of edges in a connected component gets stored.
        /// </param>
        //*************************************************************************

        protected void
        CalculateConnectedComponentMetrics
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


        protected Int32
        CountUniqueEdges
        (
            LinkedList<IVertex> oConnectedComponent
        )
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
        protected Boolean ReportProgressAndCheckCancellationPending
             (Int32 iCalculationsSoFar, Int32 iTotalCalculations, BackgroundWorker oBackgroundWorker)
        {
            Debug.Assert(iCalculationsSoFar >= 0);
            Debug.Assert(iTotalCalculations >= 0);
            Debug.Assert(iCalculationsSoFar <= iTotalCalculations);


            if (oBackgroundWorker != null)
            {
                if (oBackgroundWorker.CancellationPending)
                {
                    return (false);
                }

                ReportProgress(iCalculationsSoFar, iTotalCalculations,
                    oBackgroundWorker);
            }

            return (true);
        }

        protected void ReportProgress
            (Int32 iCalculationsSoFar, Int32 iTotalCalculations, BackgroundWorker oBackgroundWorker)
        {
            Debug.Assert(iCalculationsSoFar >= 0);
            Debug.Assert(iTotalCalculations >= 0);
            Debug.Assert(iCalculationsSoFar <= iTotalCalculations);


            if (oBackgroundWorker != null)
            {
                String sProgress = String.Format(

                    "Calculating {0}."
                    ,
                    this.GraphMetricDescription
                    );

                oBackgroundWorker.ReportProgress(

                    CalculateProgressInPercent(iCalculationsSoFar,
                        iTotalCalculations),

                    sProgress);
            }
        }

        protected Boolean ReportProgressIfNecessary
            (Int32 iCalculationsSoFar, Int32 iTotalCalculations, BackgroundWorker oBackgroundWorker)
        {
            Debug.Assert(iCalculationsSoFar >= 0);
            Debug.Assert(iTotalCalculations >= 0);

            return (
                (iCalculationsSoFar % 100 != 0)
                ||
                ReportProgressAndCheckCancellationPending(
                    iCalculationsSoFar, iTotalCalculations, oBackgroundWorker)
                );
        }

        public static Int32 CalculateProgressInPercent
            (Int32 calculationsCompleted, Int32 totalCalculations)
        {
            Debug.Assert(calculationsCompleted >= 0);
            Debug.Assert(totalCalculations >= 0);
            Debug.Assert(calculationsCompleted <= totalCalculations);

            Int32 iPercentProgress = 0;

            if (totalCalculations > 0)
            {
                iPercentProgress = (Int32)(100F *
                    (Single)calculationsCompleted / (Single)totalCalculations);
            }

            return (iPercentProgress);
        }
    }
}
