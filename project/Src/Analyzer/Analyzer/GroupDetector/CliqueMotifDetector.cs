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
    public class CliqueMotifDetector
    {
        /*
        private int m_iNMinimum;
        private int m_iNMaximum;
        
        public CliqueMotifDetector(int iNMinimum, int iNMaximum) {
            m_iNMinimum = iNMinimum;
            m_iNMaximum = iNMaximum;
        }
        public override Groups partition(IGraph graph)
        {
            throw new NotImplementedException();
        }

        protected Boolean TryCalculateCliqueMotifs
            (IGraph oGraph, Int32 iNMinimum, Int32 iNMaximum, BackgroundWorker oBackgroundWorker, ICollection<Motif> oExistingMotifs, out ICollection<Motif> oMotifs)
        {
            Debug.Assert(oGraph != null);

            oMotifs = null;

            WakitaTsurumiClusterDetector clusterCalculator = new WakitaTsurumiClusterDetector();
            ICollection<Community> communities;

            if (clusterCalculator.TryCalculateClustersWakitaTsurumi(oGraph, oBackgroundWorker,
                out communities))
            {


                Int32 iTotalOperations = communities.Count;
                Int32 iCalculationsSoFar = 0;

                HashSet<Motif> currentCliqueMotifs = new HashSet<Motif>();

                Dictionary<IVertex, Motif> verticesAlreadyInMotifs =
                    new Dictionary<IVertex, Motif>();

                // Don't consider any vertices used by other motifs
                if (oExistingMotifs != null)
                {
                    iTotalOperations += oExistingMotifs.Count;

                    foreach (Motif existingMotif in oExistingMotifs)
                    {
                        if ((iCalculationsSoFar % 100 != 0)||
                            ReportProgressAndCheckCancellationPending(iCalculationsSoFar, iTotalOperations, oBackgroundWorker)
                            )
                        
                        {
                            return (false);
                        }

                        // We don't need to consider fan motifs because they cannot overlap
                        if (!(existingMotif is FanMotif))
                        {
                            foreach (IVertex existingVertex in existingMotif.VerticesInMotif)
                            {
                                verticesAlreadyInMotifs.Add(existingVertex, existingMotif);
                            }
                        }
                    }
                }

                // Sort the found cliques by the number of vertices
                IOrderedEnumerable<Community> sortedCommunities =
                    communities.OrderByDescending(c => c.Vertices.Count);

                // Select the cliques in the order of their original size
                foreach (Community community in sortedCommunities)
                {
                    if (!ReportProgressIfNecessary(iCalculationsSoFar, iTotalOperations,
                        oBackgroundWorker))
                    {
                        return (false);
                    }

                    // Remove any overlapping vertices before considering the clique
                    List<IVertex> availableVertices = community.Vertices.Where(
                        v => !verticesAlreadyInMotifs.ContainsKey(v)).ToList();
                    // Ensure the clique passes our criteria
                    if (availableVertices.Count >= iNMinimum &&
                        availableVertices.Count <= iNMaximum)
                    {
                        CliqueMotif trimmedCliqueMotif = new CliqueMotif(availableVertices);
                        currentCliqueMotifs.Add(trimmedCliqueMotif);

                        foreach (IVertex cliqueVertex in trimmedCliqueMotif.VerticesInMotif)
                        {
                            verticesAlreadyInMotifs.Add(cliqueVertex, trimmedCliqueMotif);
                        }
                    }
                }

                SetCliqueMotifScale(currentCliqueMotifs);

                oMotifs = currentCliqueMotifs;
            }

            return (true);
        }

        protected void SetCliqueMotifScale (ICollection<Motif> oCliqueMotifs)
        {
            Debug.Assert(oCliqueMotifs != null);

            // The ArcScale property is the CliqueMotif's member count scaled between 0
            // and 1.0, based on the minimum and maximum CliqueMotif counts among all
            // CliqueMotifs.

            Int32 iMinimumMemberCount = 0;
            Int32 iMaximumMemberCount = 0;

            if (oCliqueMotifs.Count > 0)
            {
                iMinimumMemberCount = oCliqueMotifs.Min(
                    oMotif => ((CliqueMotif)oMotif).MemberVertices.Count);

                iMaximumMemberCount = oCliqueMotifs.Max(
                    oMotif => ((CliqueMotif)oMotif).MemberVertices.Count);
            }

            foreach (CliqueMotif oCliqueMotif in oCliqueMotifs)
            {
                Single fCliqueScale;

                if (iMinimumMemberCount == iMaximumMemberCount)
                {
                    // All the member counts are the same.  Arbitrarily set the
                    // CliqueScale property to the center of the range.

                    fCliqueScale = 0.5F;
                }
                else
                {
                    fCliqueScale = MathUtil.TransformValueToRange(
                        oCliqueMotif.MemberVertices.Count,
                        iMinimumMemberCount, iMaximumMemberCount,
                        0F, 1.0F
                        );
                }

                oCliqueMotif.CliqueScale = fCliqueScale;
            }
        }

        protected Boolean ReportProgressIfNecessary
            (Int32 iCalculationsSoFar,Int32 iTotalCalculations,BackgroundWorker oBackgroundWorker)
        {
            Debug.Assert(iCalculationsSoFar >= 0);
            Debug.Assert(iTotalCalculations >= 0);

            return (
                (iCalculationsSoFar % VerticesPerProgressReport != 0)
                ||
                ReportProgressAndCheckCancellationPending(
                    iCalculationsSoFar, iTotalCalculations, oBackgroundWorker)
                );
        }

        protected const Int32 VerticesPerProgressReport = 100;
         * */

    }
}
