using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public class DuplicateEdgeDetector
    {
       

        public DuplicateEdgeDetector
        (
            IGraph graph
        )
        {
            m_oGraph = graph;

            m_bEdgesCounted = false;
            m_iUniqueEdges = Int32.MinValue;
            m_iEdgesWithDuplicates = Int32.MinValue;
            m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops = Int32.MinValue;

           
        }

        

        public Boolean
        GraphContainsDuplicateEdges
        {
            get
            {
                CountEdges();

                return (m_iEdgesWithDuplicates > 0);
            }
        }

        

        public Int32
        EdgesWithDuplicates
        {
            get
            {
               
                CountEdges();

                return (m_iEdgesWithDuplicates);
            }
        }

        

        public Int32
        UniqueEdges
        {
            get
            {
              

                CountEdges();

                return (m_iUniqueEdges);
            }
        }

       

        public Int32
        TotalEdgesAfterMergingDuplicatesNoSelfLoops
        {
            get
            {
               
                CountEdges();

                return (m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops);
            }
        }

        

        protected void
        CountEdges()
        {
            
            if (m_bEdgesCounted)
            {
                return;
            }

            m_iUniqueEdges = 0;
            m_iEdgesWithDuplicates = 0;

            IEdgeCollection oEdges = m_oGraph.Edges;

            Boolean bGraphIsDirected =
                (m_oGraph.Directedness == GraphDirectedness.Directed);

            // Create a dictionary of vertex ID pairs.  The key is the vertex ID
            // pair and the value is true if the edge has duplicates or false if it
            // doesn't.

            Dictionary<Int64, Boolean> oVertexIDPairs =
                new Dictionary<Int64, Boolean>(oEdges.Count);

            foreach (IEdge oEdge in oEdges)
            {
                Int64 i64VertexIDPair = EdgeUtil.GetVertexIDPair(oEdge);
                Boolean bEdgeHasDuplicate;

                if (oVertexIDPairs.TryGetValue(i64VertexIDPair,
                    out bEdgeHasDuplicate))
                {
                    if (!bEdgeHasDuplicate)
                    {
                        // This is the edge's first duplicate.

                        m_iUniqueEdges--;
                        m_iEdgesWithDuplicates++;

                        oVertexIDPairs[i64VertexIDPair] = true;
                    }

                    m_iEdgesWithDuplicates++;
                }
                else
                {
                    m_iUniqueEdges++;

                    oVertexIDPairs.Add(i64VertexIDPair, false);
                }
            }

            m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops = 0;

            foreach (Int64 i64VertexIDPair in oVertexIDPairs.Keys)
            {
                Int32 iVertexID1 = (Int32)(i64VertexIDPair >> 32);
                Int32 iVertexID2 = (Int32)i64VertexIDPair;

                if (iVertexID1 != iVertexID2)
                {
                    m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops++;
                }
            }

            m_bEdgesCounted = true;

        }


       


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// Graph to check.

        protected IGraph m_oGraph;

        /// true if the edges have already been counted.

        protected Boolean m_bEdgesCounted;

        /// If m_bEdgesCounted is true, this is the number of unique edges in
        /// m_oGraph.

        protected Int32 m_iUniqueEdges;

        /// If m_bEdgesCounted is true, this is the number of edges in m_oGraph
        /// that have duplicates.

        protected Int32 m_iEdgesWithDuplicates;

        /// If m_bEdgesCounted is true, this is the number of edges that would be
        /// in m_oGraph if its duplicate edges were merged and all self-loops were
        /// removed.

        protected Int32 m_iTotalEdgesAfterMergingDuplicatesNoSelfLoops;
    }
}
