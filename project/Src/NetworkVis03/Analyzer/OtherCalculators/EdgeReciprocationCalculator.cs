﻿using Smrf.AppLib;
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
    public class EdgeReciprocationCalculator : AnalyzerBase
    {
        private BackgroundWorker m_obackgroundWorker;

        public EdgeReciprocationCalculator()
        {
        }

        public override bool tryAnalyze(IGraph graph, BackgroundWorker bgw, out AnalyzeResultBase results)
        {
            Dictionary<Int32, Boolean> graphMetrics;
            EdgeReciprocation oEdgeReciprocation;
            bool rv = TryCalculateGraphMetrics(graph, m_obackgroundWorker, out graphMetrics);
            if (rv == true)
            {
                oEdgeReciprocation = new EdgeReciprocation(graphMetrics.Count);
                foreach (KeyValuePair<Int32, Boolean> p in graphMetrics)
                    oEdgeReciprocation.Add(p.Key, p.Value);
            }
            else oEdgeReciprocation = new EdgeReciprocation(1);
            results = oEdgeReciprocation;
            return rv;
        }


        public override string AnalyzerDescription
        {
            get { return "Calculating EdgeReciprocation"; }
        }

        

        public Dictionary<Int32, Boolean> CalculateGraphMetrics(IGraph graph)
        {
            Debug.Assert(graph != null);
            Dictionary<Int32, Boolean> oGraphMetrics;
            TryCalculateGraphMetrics(graph, null, out oGraphMetrics);
            return (oGraphMetrics);
        }


        public Boolean TryCalculateGraphMetrics
            (IGraph graph, BackgroundWorker backgroundWorker, out Dictionary<Int32, Boolean> graphMetrics)
        {
            Debug.Assert(graph != null);

            IEdgeCollection oEdges = graph.Edges;

            // The key is an IEdge.ID and the value is a Boolean that indicates
            // whether the edge is reciprocated.

            Dictionary<Int32, Boolean> oReciprocationFlags =
                new Dictionary<Int32, Boolean>(oEdges.Count);

            graphMetrics = oReciprocationFlags;

            // The key is the combined IDs of the edge's vertices.

            HashSet<Int64> oVertexIDPairs = new HashSet<Int64>();

            if (graph.Directedness == GraphDirectedness.Directed)
            {
                foreach (IEdge oEdge in oEdges)
                {
                    oVertexIDPairs.Add(GetDictionaryKey(
                        oEdge.Vertex1, oEdge.Vertex2));
                }

                if (!ReportProgressAndCheckCancellationPending(
                    1, 2, backgroundWorker))
                {
                    return (false);
                }

                foreach (IEdge oEdge in oEdges)
                {
                    Boolean bEdgeIsReciprocated = false;

                    if (!oEdge.IsSelfLoop)
                    {
                        Int64 i64ReversedVerticesKey = GetDictionaryKey(
                            oEdge.Vertex2, oEdge.Vertex1);

                        bEdgeIsReciprocated =
                            oVertexIDPairs.Contains(i64ReversedVerticesKey);
                    }

                    oReciprocationFlags.Add(oEdge.ID, bEdgeIsReciprocated);
                }
            }

            return (true);
        }

        protected Int64 GetDictionaryKey(IVertex oVertex1, IVertex oVertex2)
        {
            Debug.Assert(oVertex1 != null);
            Debug.Assert(oVertex2 != null);

            return (CollectionUtil.GetDictionaryKey(oVertex1.ID, oVertex2.ID,
                true));
        }


        
    }

}
