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
    public class FanMotifDetector : GroupDetectorBase
    {
        public FanMotifDetector() { }

        public override bool tryPartition(IGraph graph, BackgroundWorker bgw, out Groups results)
        {
            ICollection<Motif> oMotifs;
            Groups oGroups;
            int i = 1;
            bool rv = TryCalculateFanMotif(graph, bgw, out oMotifs);
            if (rv == true)
            {
                oGroups = new Groups(oMotifs.Count);
                foreach (Motif m in oMotifs)
                {
                    oGroups.Add(i, (FanMotif)m);
                    i++;
                }
            }
            else oGroups = new Groups(1);
            results = oGroups;
            return rv;
        }

        public override string getPartitionerDescription()
        {
            return "Detecting FanMotifs";
        }

        public Boolean TryCalculateFanMotif 
            (IGraph oGraph, BackgroundWorker oBackgroundWorker, out ICollection<Motif> oMotifs)
        {
            Debug.Assert(oGraph != null);

            oMotifs = null;

            LinkedList<Motif> oFanMotifs = new LinkedList<Motif>();
            LinkedList<IVertex> oLeaves = new LinkedList<IVertex>();

            IVertexCollection oVertices = oGraph.Vertices;
            Int32 iVertices = oVertices.Count;
            Int32 iCalculationsSoFar = 0;

            foreach (IVertex oVertex in oVertices)
            {
                if ((iCalculationsSoFar % 100 == 0) &&
                    !ReportProgressAndCheckCancellationPending(iCalculationsSoFar, iVertices, oBackgroundWorker)
                    )
                {
                    return (false);
                }

                ICollection<IVertex> oAdjacentVertices = oVertex.AdjacentVertices;

                if (oAdjacentVertices.Count >= 2)
                {
                    foreach (IVertex oAdjacentVertex in oAdjacentVertices)
                    {
                        if (oAdjacentVertex.AdjacentVertices.Count == 1)
                        {
                            oLeaves.AddLast(oAdjacentVertex);
                        }
                    }

                    if (oLeaves.Count >= 2)
                    {
                        oFanMotifs.AddLast(
                            new FanMotif(oVertex, oLeaves.ToArray()));
                    }

                    oLeaves.Clear();
                }

                iCalculationsSoFar++;
            }

            // Set the ArcScale property on each FanMotif object.

            SetFanMotifArcScale(oFanMotifs);

            oMotifs = oFanMotifs;

            return (true);
        }

        protected void SetFanMotifArcScale(ICollection<Motif> oFanMotifs)
        {
            Debug.Assert(oFanMotifs != null);

            // The ArcScale property is the FanMotif's leaf count scaled between 0
            // and 1.0, based on the minimum and maximum leaf counts among all
            // FanMotifs.

            Int32 iMinimumLeafCount = 0;
            Int32 iMaximumLeafCount = 0;

            if (oFanMotifs.Count > 0)
            {
                iMinimumLeafCount = oFanMotifs.Min(
                    oMotif => ((FanMotif)oMotif).LeafVertices.Length);

                iMaximumLeafCount = oFanMotifs.Max(
                    oMotif => ((FanMotif)oMotif).LeafVertices.Length);
            }

            foreach (FanMotif oFanMotif in oFanMotifs)
            {
                Single fArcScale;

                if (iMinimumLeafCount == iMaximumLeafCount)
                {
                    // All the leaf counts are the same.  Arbitrarily set the
                    // ArcScale property to the center of the range.

                    fArcScale = 0.5F;
                }
                else
                {
                    fArcScale = MathUtil.TransformValueToRange(
                        oFanMotif.LeafVertices.Length,
                        iMinimumLeafCount, iMaximumLeafCount,
                        0F, 1.0F
                        );
                }

                oFanMotif.ArcScale = fArcScale;
            }


        }
        



        
    }
}
