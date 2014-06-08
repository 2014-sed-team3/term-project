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
    public class DConnectorMotifDetector : GroupDetectorBase
    {
        private int m_iDMinimum;
        private int m_iDMaximum;
        public DConnectorMotifDetector(int iDMinimum, int iDMaximum) : this(){
            m_iDMinimum = iDMinimum;
            m_iDMaximum = iDMaximum;
        }
        public DConnectorMotifDetector() { }

        public override bool tryPartition(IGraph graph, BackgroundWorker bgw, out Groups results)
        {
            ICollection<Motif> oMotifs;
            Groups oGroups;
            int i = 0;
            bool rv = TryCalculateDConnectorMotifs(graph, m_iDMinimum, m_iDMaximum, bgw, out oMotifs);
            if (rv == true)
            {
                oGroups = new Groups(oMotifs.Count);
                foreach (Motif m in oMotifs)
                {
                    oGroups.Add(i, (DConnectorMotif)m);
                    i++;
                }
            }
            else oGroups = new Groups(1);
            results = oGroups;
            return rv;
        }

        public override string getPartitionerDescription()
        {
            return "Detecting DConnectorMotifs";
        }

        public Boolean TryCalculateDConnectorMotifs
            (IGraph oGraph, Int32 iDMinimum, Int32 iDMaximum, BackgroundWorker oBackgroundWorker, out ICollection<Motif> oMotifs)
        {
            Debug.Assert(oGraph != null);

            oMotifs = null;

            IVertexCollection oVertices = oGraph.Vertices;
            Int32 iVertices = oVertices.Count;
            Int32 iCalculationsSoFar = 0;

            // The key is an ordered combination of the vertex IDs of the potential
            // motif's D anchor vertices, and the value is the corresponding
            // potential DConnectorMotif object.

            Dictionary<string, DConnectorMotif> oPotentialDConnectorMotifs =
                new Dictionary<string, DConnectorMotif>();

            foreach (IVertex oPotentialSpanVertex in oVertices)
            {
                
                if ((iCalculationsSoFar % 100 == 0)
                 &&
                 !ReportProgressAndCheckCancellationPending(
                     iCalculationsSoFar, iVertices, oBackgroundWorker))
                {
                    return (false);
                }

                // Get only the non-self-loop adjacent vertices
                ICollection<IVertex> oPotentialAnchorVertices =
                    oPotentialSpanVertex.AdjacentVertices
                    .Where<IVertex>(adjVertex => adjVertex != oPotentialSpanVertex).ToList<IVertex>();

                if (DVerticesMightBeAnchors(oPotentialAnchorVertices, iDMinimum, iDMaximum))
                {
                    AddSpanVertexToPotentialDConnectorMotifs(
                        oPotentialSpanVertex, oPotentialAnchorVertices,
                        oPotentialDConnectorMotifs);
                }

                iCalculationsSoFar++;
            }

            // Filter the potential D-connector motifs and add the real ones to
            // the collection of motifs.

            oMotifs = FilterDConnectorMotifs(oPotentialDConnectorMotifs);

            // Set the SpanScale property on each DConnectorMotif object.

            SetDConnectorMotifSpanScale(oMotifs);

            return (true);
        }

        protected Boolean DVerticesMightBeAnchors
            (ICollection<IVertex> oVertices, Int32 iDMinimum, Int32 iDMaximum)
        {
            Debug.Assert(oVertices != null);

            return (

                // There have to be between [iDMinimum, iDMaximum] anchor vertices.

                oVertices.Count >= iDMinimum && oVertices.Count <= iDMaximum
                &&

                // Anchor vertices can't have just one adjacent vertex.

                oVertices.All(oVertex => oVertex.AdjacentVertices.Count != 1)
                );
        }

        protected ICollection<Motif> FilterDConnectorMotifs
            (Dictionary<string, DConnectorMotif> oPotentialDConnectorMotifs)
        {
            Debug.Assert(oPotentialDConnectorMotifs != null);

            HashSet<Motif> currentDConnectorMotifs = new HashSet<Motif>();

            Dictionary<IVertex, DConnectorMotif> verticesAlreadyInDConnectorMotifs =
                new Dictionary<IVertex, DConnectorMotif>();

            // Select only those potential D-connector motifs that have at least
            // two span vertices.

            foreach (DConnectorMotif potentialMotif in
                oPotentialDConnectorMotifs.Values.Where(
                    oPotentialDConnectorMotif =>
                    oPotentialDConnectorMotif.SpanVertices.Count >= 2)
                )
            {
                // If any of the motif's span vertices are included in another
                // D-connector motif we need to pick the motif to keep
                //
                // If this weren't done, for example, the following ring of vertices 
                // would result in two redundant two-connector motifs:
                //
                // A-B-C-D-A

                List<DConnectorMotif> overlappingMotifs =
                    (from spanVertex in potentialMotif.SpanVertices
                     where verticesAlreadyInDConnectorMotifs.ContainsKey(spanVertex)
                     select verticesAlreadyInDConnectorMotifs[spanVertex])
                     .Distinct<DConnectorMotif>().ToList<DConnectorMotif>();

                if (overlappingMotifs.Count > 0)
                {
                    // Our bookkeeping should prevent more than one overlap
                    Debug.Assert(overlappingMotifs.Count == 1);

                    DConnectorMotif existingMotif = overlappingMotifs[0];

                    int potAnchors = potentialMotif.AnchorVertices.Count,
                        potSpanners = potentialMotif.SpanVertices.Count,
                        potTotal = potAnchors + potSpanners;

                    int existAnchors = existingMotif.AnchorVertices.Count,
                        existSpanners = existingMotif.SpanVertices.Count,
                        existTotal = existAnchors + existSpanners;

                    // Potential motif is larger in total size, so we favor it
                    // -- OR --
                    // Potential motif is equal in total size and has more spanners, which we favor over more anchors
                    if (potSpanners > existSpanners ||
                        (
                        potSpanners == existSpanners &&
                        potTotal > existTotal))
                    {

                        // Remove the existing motif from the list of motifs and the dictionary entries for its vertices
                        currentDConnectorMotifs.Remove(existingMotif);

                        foreach (IVertex existingSpanVertex in existingMotif.SpanVertices)
                        {
                            verticesAlreadyInDConnectorMotifs.Remove(existingSpanVertex);
                        }

                        foreach (IVertex existingAnchorVertex in existingMotif.AnchorVertices)
                        {
                            verticesAlreadyInDConnectorMotifs.Remove(existingAnchorVertex);
                        }

                        // Add the potential DConnectorMotif and record its vertices
                        AddDConnectorMotif(currentDConnectorMotifs, verticesAlreadyInDConnectorMotifs, potentialMotif);
                    }
                    else
                    {
                        // Potential motif is smaller than the existing one or is the same size with fewer spanners -- do nothing
                    }

                }
                // If all of the motifs span vertices are not included in others, add the DConnectorMotif and record its vertices
                else
                {
                    AddDConnectorMotif(currentDConnectorMotifs, verticesAlreadyInDConnectorMotifs, potentialMotif);
                }
            }

            return currentDConnectorMotifs;
        }

        private static void AddDConnectorMotif
            (HashSet<Motif> currentDConnectorMotifs, Dictionary<IVertex, DConnectorMotif> verticesAlreadyInDConnectorMotifs, DConnectorMotif connectorMotifToAdd)
        {
            // Assert that there are no shared anchor and span vertices
            Debug.Assert(connectorMotifToAdd.SpanVertices.Intersect<IVertex>(connectorMotifToAdd.AnchorVertices).Count<IVertex>() == 0);

            currentDConnectorMotifs.Add(connectorMotifToAdd);

            foreach (IVertex oVertex in connectorMotifToAdd.SpanVertices)
            {
                // We do not allow overlapping span vertices so we use .Add
                verticesAlreadyInDConnectorMotifs.Add(oVertex, connectorMotifToAdd);
            }

            foreach (IVertex oVertex in connectorMotifToAdd.AnchorVertices)
            {
                // We allow overlapping anchor vertices so we use =
                verticesAlreadyInDConnectorMotifs[oVertex] = connectorMotifToAdd;
            }
        }

        protected void AddSpanVertexToPotentialDConnectorMotifs
            (IVertex oPotentialSpanVertex, ICollection<IVertex> oDPotentialAnchorVertices, Dictionary<string, DConnectorMotif> oPotentialDConnectorMotifs)
        {
            Debug.Assert(oPotentialSpanVertex != null);
            Debug.Assert(oDPotentialAnchorVertices != null);
            Debug.Assert(oDPotentialAnchorVertices.Count >= 2);
            Debug.Assert(oPotentialDConnectorMotifs != null);

            // Is there already a DConnectorMotif object for this set of
            // potential anchor vertices?

            IOrderedEnumerable<IVertex> oOrderedDPotentialAnchorVertices = oDPotentialAnchorVertices.OrderBy(v => v.ID);
            string stringKey = string.Join(",", oOrderedDPotentialAnchorVertices.Select(v => v.ID.ToString()).ToArray());

            DConnectorMotif oPotentialDConnectorMotif;

            if (!oPotentialDConnectorMotifs.TryGetValue(
                stringKey, out oPotentialDConnectorMotif))
            {
                // No.  Create one.

                oPotentialDConnectorMotif = new DConnectorMotif(new List<IVertex>(oDPotentialAnchorVertices));

                oPotentialDConnectorMotifs.Add(stringKey,
                    oPotentialDConnectorMotif);
            }

            oPotentialDConnectorMotif.SpanVertices.Add(oPotentialSpanVertex);
        }

        protected void SetDConnectorMotifSpanScale (ICollection<Motif> oDConnectorMotifs)
        {
            Debug.Assert(oDConnectorMotifs != null);

            // The SpanScale property is the DConnectorMotif's span count scaled
            // between 0 and 1.0, based on the minimum and maximum span counts
            // among all DConnectorMotifs.

            Int32 iMinimumSpanCount = 0;
            Int32 iMaximumSpanCount = 0;

            if (oDConnectorMotifs.Count > 0)
            {
                iMinimumSpanCount = oDConnectorMotifs.Min(
                    oMotif => ((DConnectorMotif)oMotif).SpanVertices.Count);

                iMaximumSpanCount = oDConnectorMotifs.Max(
                    oMotif => ((DConnectorMotif)oMotif).SpanVertices.Count);
            }

            foreach (DConnectorMotif oDConnectorMotif in oDConnectorMotifs)
            {
                Single fSpanScale;

                if (iMinimumSpanCount == iMaximumSpanCount)
                {
                    // All the span counts are the same.  Arbitrarily set the
                    // SpanScale property to the center of the range.

                    fSpanScale = 0.5F;
                }
                else
                {
                    fSpanScale = MathUtil.TransformValueToRange(
                        oDConnectorMotif.SpanVertices.Count,
                        iMinimumSpanCount, iMaximumSpanCount,
                        0F, 1.0F
                        );
                }

                oDConnectorMotif.SpanScale = fSpanScale;
            }
        }

        
    }
}
