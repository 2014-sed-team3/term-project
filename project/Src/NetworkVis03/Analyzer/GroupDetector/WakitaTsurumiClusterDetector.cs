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
    public class WakitaTsurumiClusterDetector : GroupDetectorBase
    {
        public override Groups partition(IGraph graph)
        {
            ICollection<Community> oGraphMetrics;
            Groups oGroups;
            int i = 0;
            TryCalculateClustersWakitaTsurumi(graph, getBackgroundWorker(), out oGraphMetrics);
            oGroups = new Groups(oGraphMetrics.Count);
            foreach (Community m in oGraphMetrics)
            {
                oGroups.Add(i, m);
                i++;
            }
            return oGroups;
        }

        public Boolean TryCalculateClustersWakitaTsurumi
            (IGraph oGraph, BackgroundWorker oBackgroundWorker,out ICollection<Community> oGraphMetrics)
        {
            Debug.Assert(oGraph != null);
            

            IVertexCollection oVertices = oGraph.Vertices;
            Int32 iVertices = oVertices.Count;
            Int32 iEdges = oGraph.Edges.Count;

            IDGenerator oIDGenerator = new IDGenerator(1);

            // Create and populate a community for each of the graph's vertices.

            LinkedList<Community> oCommunities =
                CreateCommunities(oVertices, oIDGenerator);

            oGraphMetrics = oCommunities;

            if (iVertices == 0 || iEdges == 0)
            {
                // There is no point in going any further.

                return (true);
            }

            // This max heap is used to keep track of the maximum delta Q value in
            // each community.  There is an element in the max heap for each
            // community.  The key is the Community and the value is the
            // Community's maximum delta Q.

            DeltaQMaxHeap oDeltaQMaxHeap = new DeltaQMaxHeap(iVertices);

            // Initialize all the delta Q values.

            InitializeDeltaQs(oCommunities, oDeltaQMaxHeap, iEdges);

            // Run the algorithm outlined in the Wakita/Tsurumi paper.

            BinaryHeapItem<Community, Single> oBinaryHeapItemWithMaximumDeltaQ;
            Int32 iMergeCycles = 0;

            // Retrieve the community pair with the largest delta Q.

            while (oDeltaQMaxHeap.TryGetTop(out oBinaryHeapItemWithMaximumDeltaQ))
            {
                // Check for cancellation and report progress every
                // MergeCyclesPerProgressReport calculations.

                if (
                    iMergeCycles % MergeCyclesPerProgressReport == 0
                    &&
                    !ReportProgressAndCheckCancellationPending(
                        iMergeCycles, iVertices, oBackgroundWorker)
                    )
                {
                    return (false);
                }

                Community oCommunityWithMaximumDeltaQ =
                    oBinaryHeapItemWithMaximumDeltaQ.Key;

                Single fMaximumGlobalDeltaQ =
                    oBinaryHeapItemWithMaximumDeltaQ.Value;

                if (fMaximumGlobalDeltaQ < 0)
                {
                    // Merging additional communities would yield worse results, so
                    // quit.

                    break;
                }

                // Merge the communities in the community pair with maximum
                // delta Q, update the maximum delta Q values for all communities,
                // and update the global max heap.

                CommunityPair oCommunityPairWithMaximumDeltaQ =
                    oCommunityWithMaximumDeltaQ.CommunityPairWithMaximumDeltaQ;

                MergeCommunities(oCommunities, oCommunityPairWithMaximumDeltaQ,
                    oDeltaQMaxHeap, iEdges, oIDGenerator);

                iMergeCycles++;
            }

            return (true);
        }

        protected LinkedList<Community> CreateCommunities
            (IVertexCollection oVertices, IDGenerator oIDGenerator)
        {
            Debug.Assert(oVertices != null);
            Debug.Assert(oIDGenerator != null);

            // This is the list of communities.  Initially, there will be one
            // community for each of the graph's vertices.

            LinkedList<Community> oCommunities = new LinkedList<Community>();

            // This temporary dictionary is used to map a vertex ID to a community.
            // The key is the IVertex.ID and the value is the corresponding
            // Community object.

            Dictionary<Int32, Community> oVertexIDDictionary =
                new Dictionary<Int32, Community>(oVertices.Count);

            // First, create a community for each of the graph's vertices.  Each
            // community contains just the vertex.

            foreach (IVertex oVertex in oVertices)
            {
                Community oCommunity = new Community();

                Int32 iID = oIDGenerator.GetNextID();

                oCommunity.ID = iID;
                oCommunity.Vertices.Add(oVertex);

                // TODO: IVertex.AdjacentVertices includes self-loops.  Should
                // self-loops be eliminated everywhere, including here and within
                // the graph's total edge count?  Not sure how self-loops are
                // affecting the algorithm used by this class...

                oCommunity.Degree = oVertex.AdjacentVertices.Count;

                oCommunities.AddLast(oCommunity);
                oVertexIDDictionary.Add(oVertex.ID, oCommunity);
            }

            // Now populate each community's list of community pairs.

            foreach (Community oCommunity1 in oCommunities)
            {
                Debug.Assert(oCommunity1.Vertices.Count == 1);

                IVertex oVertex = oCommunity1.Vertices.First();

                SortedList<Int32, CommunityPair> oCommunityPairs =
                    oCommunity1.CommunityPairs;

                foreach (IVertex oAdjacentVertex in oVertex.AdjacentVertices)
                {
                    if (oAdjacentVertex == oVertex)
                    {
                        // Skip self-loops.

                        continue;
                    }

                    Community oCommunity2 =
                        oVertexIDDictionary[oAdjacentVertex.ID];

                    CommunityPair oCommunityPair = new CommunityPair();
                    oCommunityPair.Community1 = oCommunity1;
                    oCommunityPair.Community2 = oCommunity2;

                    oCommunityPairs.Add(oCommunity2.ID, oCommunityPair);
                }
            }

            return (oCommunities);
        }

        protected void InitializeDeltaQs
            (LinkedList<Community> oCommunities, DeltaQMaxHeap oDeltaQMaxHeap, Int32 iEdgesInGraph)
        {
            Debug.Assert(oCommunities != null);
            Debug.Assert(oDeltaQMaxHeap != null);
            Debug.Assert(iEdgesInGraph > 0);

            foreach (Community oCommunity in oCommunities)
            {
                // Initialize the delta Q values for the community.

                oCommunity.InitializeDeltaQs(oCommunities, iEdgesInGraph);

                Single fMaximumDeltaQ = oCommunity.MaximumDeltaQ;

                if (fMaximumDeltaQ != Community.DeltaQNotSet)
                {
                    // Store the community's maximum delta Q on the max heap.

                    oDeltaQMaxHeap.Add(oCommunity, fMaximumDeltaQ);
                }
            }
        }

        protected void MergeCommunities
            (LinkedList<Community> oCommunities,CommunityPair oCommunityPairToMerge,DeltaQMaxHeap oDeltaQMaxHeap,Int32 iEdgesInGraph,IDGenerator oIDGenerator)
        {
            Debug.Assert(oCommunityPairToMerge != null);
            Debug.Assert(oCommunities != null);
            Debug.Assert(oDeltaQMaxHeap != null);
            Debug.Assert(iEdgesInGraph > 0);
            Debug.Assert(oIDGenerator != null);

            // Merge Community1 and Community2 into a NewCommunity.

            Community oCommunity1 = oCommunityPairToMerge.Community1;
            Community oCommunity2 = oCommunityPairToMerge.Community2;

            Community oNewCommunity = new Community();

            oNewCommunity.ID = oIDGenerator.GetNextID();
            oNewCommunity.Degree = oCommunity1.Degree + oCommunity2.Degree;

            ICollection<IVertex> oNewCommunityVertices = oNewCommunity.Vertices;

            foreach (IVertex oVertex in oCommunity1.Vertices)
            {
                oNewCommunityVertices.Add(oVertex);
            }

            foreach (IVertex oVertex in oCommunity2.Vertices)
            {
                oNewCommunityVertices.Add(oVertex);
            }

            // In the following sorted lists, the sort key is the ID of
            // CommunityPair.Community2 and the value is the CommunityPair.

            SortedList<Int32, CommunityPair> oCommunity1CommunityPairs =
                oCommunity1.CommunityPairs;

            SortedList<Int32, CommunityPair> oCommunity2CommunityPairs =
                oCommunity2.CommunityPairs;

            SortedList<Int32, CommunityPair> oNewCommunityCommunityPairs =
                oNewCommunity.CommunityPairs;

            Int32 iCommunity1CommunityPairs = oCommunity1CommunityPairs.Count;
            Int32 iCommunity2CommunityPairs = oCommunity2CommunityPairs.Count;

            IList<Int32> oCommunity1Keys = oCommunity1CommunityPairs.Keys;

            IList<CommunityPair> oCommunity1Values =
                oCommunity1CommunityPairs.Values;

            IList<Int32> oCommunity2Keys = oCommunity2CommunityPairs.Keys;

            IList<CommunityPair> oCommunity2Values =
                oCommunity2CommunityPairs.Values;

            // Step through the community pairs in oCommunity1 and oCommunity2.

            Int32 iCommunity1Index = 0;
            Int32 iCommunity2Index = 0;

            Single fMaximumDeltaQ = Single.MinValue;
            CommunityPair oCommunityPairWithMaximumDeltaQ = null;
            Single fTwoTimesEdgesInGraph = 2F * iEdgesInGraph;

            while (iCommunity1Index < iCommunity1CommunityPairs ||
                   iCommunity2Index < iCommunity2CommunityPairs)
            {
                Int32 iCommunity1OtherCommunityID =
                    (iCommunity1Index < iCommunity1CommunityPairs) ?
                    oCommunity1Keys[iCommunity1Index] : Int32.MaxValue;

                Int32 iCommunity2OtherCommunityID =
                    (iCommunity2Index < iCommunity2CommunityPairs) ?
                    oCommunity2Keys[iCommunity2Index] : Int32.MaxValue;

                CommunityPair oNewCommunityPair = new CommunityPair();
                oNewCommunityPair.Community1 = oNewCommunity;

                if (iCommunity1OtherCommunityID == oCommunity2.ID)
                {
                    // This is an internal connection eliminated by the merge.
                    // Skip it.

                    iCommunity1Index++;
                    continue;
                }
                else if (iCommunity2OtherCommunityID == oCommunity1.ID)
                {
                    // This is an internal connection eliminated by the merge.
                    // Skip it.

                    iCommunity2Index++;
                    continue;
                }
                else if (iCommunity1OtherCommunityID == iCommunity2OtherCommunityID)
                {
                    // The other community is connected to both commmunity 1 and
                    // community 2.
                    //
                    // This is equation 10a from the paper "Finding Community
                    // Structure in Very Large Networks," by Clauset, Newman, and
                    // Moore.

                    oNewCommunityPair.Community2 =
                        oCommunity1Values[iCommunity1Index].Community2;

                    oNewCommunityPair.DeltaQ =
                        oCommunity1Values[iCommunity1Index].DeltaQ +
                        oCommunity2Values[iCommunity2Index].DeltaQ;

                    iCommunity1Index++;
                    iCommunity2Index++;
                }
                else if (iCommunity1OtherCommunityID < iCommunity2OtherCommunityID)
                {
                    // The other community is connected only to commmunity 1.
                    //
                    // This is equation 10b from the same paper.

                    Community oOtherCommunity =
                        oCommunity1Values[iCommunity1Index].Community2;

                    oNewCommunityPair.Community2 = oOtherCommunity;

                    Single fAj = oCommunity2.Degree / fTwoTimesEdgesInGraph;
                    Single fAk = oOtherCommunity.Degree / fTwoTimesEdgesInGraph;

                    oNewCommunityPair.DeltaQ =
                        oCommunity1Values[iCommunity1Index].DeltaQ -
                        2F * fAj * fAk;

                    iCommunity1Index++;
                }
                else
                {
                    // The other community is connected only to commmunity 2.
                    //
                    // This is equation 10c from the same paper.

                    Community oOtherCommunity =
                        oCommunity2Values[iCommunity2Index].Community2;

                    oNewCommunityPair.Community2 = oOtherCommunity;

                    Single fAi = oCommunity1.Degree / fTwoTimesEdgesInGraph;
                    Single fAk = oOtherCommunity.Degree / fTwoTimesEdgesInGraph;

                    oNewCommunityPair.DeltaQ =
                        oCommunity2Values[iCommunity2Index].DeltaQ -
                        2F * fAi * fAk;

                    iCommunity2Index++;
                }

                oNewCommunityCommunityPairs.Add(oNewCommunityPair.Community2.ID,
                    oNewCommunityPair);

                Single fNewCommunityPairDeltaQ = oNewCommunityPair.DeltaQ;

                if (fNewCommunityPairDeltaQ > fMaximumDeltaQ)
                {
                    fMaximumDeltaQ = oNewCommunityPair.DeltaQ;
                    oCommunityPairWithMaximumDeltaQ = oNewCommunityPair;
                }

                // The other community is connected to one or both of the merged
                // communities.  Update it.

                oNewCommunityPair.Community2.OnMergedCommunities(
                    oCommunity1, oCommunity2, oNewCommunity,
                    fNewCommunityPairDeltaQ, oDeltaQMaxHeap);
            }

            oNewCommunity.CommunityPairWithMaximumDeltaQ =
                oCommunityPairWithMaximumDeltaQ;

            // Update the community list.

            oCommunities.Remove(oCommunity1);
            oCommunities.Remove(oCommunity2);
            oCommunities.AddLast(oNewCommunity);

            // Update the max heap.

            oDeltaQMaxHeap.Remove(oCommunity1);
            oDeltaQMaxHeap.Remove(oCommunity2);
            oDeltaQMaxHeap.Add(oNewCommunity, oNewCommunity.MaximumDeltaQ);
        }

        protected const Int32 MergeCyclesPerProgressReport = 100;
        
    }
}
