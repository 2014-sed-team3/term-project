using Smrf.AppLib;
using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public class OverallReciprocationCalculator
    {
        

        public OverallReciprocationCalculator()
        {
            
        }
        

        public void
        CalculateGraphMetrics
        (
            IGraph graph,
            out Nullable<Double> reciprocatedVertexPairRatio,
            out Nullable<Double> reciprocatedEdgeRatio
        )
        {
            Debug.Assert(graph != null);
           
            TryCalculateGraphMetrics(graph, null, out reciprocatedVertexPairRatio,
                out reciprocatedEdgeRatio);
        }

        public Boolean
        TryCalculateGraphMetrics
        (
            IGraph graph,
            BackgroundWorker backgroundWorker,
            out Nullable<Double> reciprocatedVertexPairRatio,
            out Nullable<Double> reciprocatedEdgeRatio
        )
        {
            Debug.Assert(graph != null);
            
            if (graph.Directedness != GraphDirectedness.Directed)
            {
                reciprocatedVertexPairRatio = reciprocatedEdgeRatio = null;
            }
            else
            {
                HashSet<Int64> oVertexIDPairsOrdered, oVertexIDPairsUnordered;

                CountEdges(graph, out oVertexIDPairsOrdered,
                    out oVertexIDPairsUnordered);

                Int32 iVertexPairsWithBothDirectedEdges =
                    CountVertexPairsWithBothDirectedEdges(
                        graph, oVertexIDPairsOrdered);

                reciprocatedVertexPairRatio = CalculateReciprocatedVertexPairRatio(
                    graph, iVertexPairsWithBothDirectedEdges,
                    oVertexIDPairsUnordered);

                reciprocatedEdgeRatio = CalculateReciprocatedEdgeRatio(
                    graph, iVertexPairsWithBothDirectedEdges);
            }

            return (true);
        }


        protected void
        CountEdges
        (
            IGraph oGraph,
            out HashSet<Int64> oVertexIDPairsOrdered,
            out HashSet<Int64> oVertexIDPairsUnordered
        )
        {
            Debug.Assert(oGraph != null);
            Debug.Assert(oGraph.Directedness == GraphDirectedness.Directed);
           

            oVertexIDPairsOrdered = new HashSet<Int64>();
            oVertexIDPairsUnordered = new HashSet<Int64>();

            foreach (IEdge oEdge in oGraph.Edges)
            {
                if (!oEdge.IsSelfLoop)
                {
                    Int32 iVertex1ID = oEdge.Vertex1.ID;
                    Int32 iVertex2ID = oEdge.Vertex2.ID;

                    oVertexIDPairsOrdered.Add(CollectionUtil.GetDictionaryKey(
                        iVertex1ID, iVertex2ID, true));

                    oVertexIDPairsUnordered.Add(CollectionUtil.GetDictionaryKey(
                        iVertex1ID, iVertex2ID, false));
                }
            }
        }

        
        protected Int32
        CountVertexPairsWithBothDirectedEdges
        (
            IGraph oGraph,
            HashSet<Int64> oVertexIDPairsOrdered
        )
        {
            Debug.Assert(oGraph != null);
            Debug.Assert(oGraph.Directedness == GraphDirectedness.Directed);
            Debug.Assert(oVertexIDPairsOrdered != null);
            
            // Note that each vertex pair connected with both directed edges will
            // get counted twice here.

            Int32 iVertexPairsWithBothDirectedEdges = 0;

            foreach (Int64 oVertexIDPairOrdered in oVertexIDPairsOrdered)
            {
                // Check whether the HashSet contains a vertex pair with the order
                // of the vertex IDs reversed.

                Int32 iVertex1ID, iVertex2ID;

                CollectionUtil.ParseDictionaryKey(oVertexIDPairOrdered,
                    out iVertex1ID, out iVertex2ID);

                if (oVertexIDPairsOrdered.Contains(
                    CollectionUtil.GetDictionaryKey(
                        iVertex2ID, iVertex1ID, true)))
                {
                    iVertexPairsWithBothDirectedEdges++;
                }
            }

            Debug.Assert(iVertexPairsWithBothDirectedEdges % 2 == 0);

            return (iVertexPairsWithBothDirectedEdges / 2);
        }


        protected Nullable<Double>
        CalculateReciprocatedVertexPairRatio
        (
            IGraph oGraph,
            Int32 iVertexPairsWithBothDirectedEdges,
            HashSet<Int64> oVertexIDPairsUnordered
        )
        {
            Debug.Assert(oGraph != null);
            Debug.Assert(oGraph.Directedness == GraphDirectedness.Directed);
            Debug.Assert(iVertexPairsWithBothDirectedEdges >= 0);
            Debug.Assert(oVertexIDPairsUnordered != null);
           

            Int32 iVertexPairsWithAtLeastOneDirectedEdge =
                oVertexIDPairsUnordered.Count;

            if (iVertexPairsWithAtLeastOneDirectedEdge == 0)
            {
                return (null);
            }

            return ((Double)iVertexPairsWithBothDirectedEdges /
                (Double)iVertexPairsWithAtLeastOneDirectedEdge);
        }

        

        protected Nullable<Double>
        CalculateReciprocatedEdgeRatio
        (
            IGraph oGraph,
            Int32 iVertexPairsWithBothDirectedEdges
        )
        {
            Debug.Assert(oGraph != null);
            Debug.Assert(oGraph.Directedness == GraphDirectedness.Directed);
            Debug.Assert(iVertexPairsWithBothDirectedEdges >= 0);
            
            Int32 iTotalEdgesAfterMergingDupcliatesNoSelfLoops =
                new DuplicateEdgeDetector(oGraph)
                .TotalEdgesAfterMergingDuplicatesNoSelfLoops;

            if (iTotalEdgesAfterMergingDupcliatesNoSelfLoops == 0)
            {
                return (null);
            }

            return ((Double)iVertexPairsWithBothDirectedEdges * 2.0 /
                (Double)iTotalEdgesAfterMergingDupcliatesNoSelfLoops);
        }


    }
}
