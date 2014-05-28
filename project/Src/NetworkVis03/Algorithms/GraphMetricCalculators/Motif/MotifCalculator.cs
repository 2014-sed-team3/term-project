
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: MotifCalculator
//
/// <summary>
/// Calculates motifs for a specified graph.
/// </summary>
//*****************************************************************************

public class MotifCalculator : GraphMetricCalculatorBase
{
    //*************************************************************************
    //  Property: GraphMetricDescription
    //
    /// <summary>
    /// Gets a description of the graph metrics calculated by the
    /// implementation.
    /// </summary>
    ///
    /// <value>
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </value>
    //*************************************************************************

    public override String
    GraphMetricDescription
    {
        get
        {
            AssertValid();

            return ("motifs");
        }
    }

    //*************************************************************************
    //  Method: TryCalculateMotifs()
    //
    /// <summary>
    /// Attempts to partition the graph into motifs while optionally running on
    /// a background thread.
    /// </summary>
    ///
    /// <param name="graph">
    /// Graph to calculate the motifs for.
    /// </param>
    ///
    /// <param name="motifsToCalculate">
    /// An ORed combination of motifs to calculate.
    /// </param>
    ///
    /// <param name="dMinimum">
    /// The minimum number of anchor vertices (dimension) of D-connector motif to find.
    /// </param>
    /// 
    /// <param name="dMaximum">
    /// The maximum number of anchor vertices (dimension) of D-connector motif to find.
    /// </param>
    /// 
    /// <param name="nMinimum">
    /// The minimum number of member vertices of clique motif to find.
    /// </param>
    /// 
    /// <param name="nMaximum">
    /// The maximum number of member vertices of clique motif to find.
    /// </param>
    /// 
    /// <param name="backgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="motifs">
    /// Where a collection of zero or more <see cref="Motif" /> objects gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the motifs were calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryCalculateMotifs
    (
        IGraph graph,
        Motifs motifsToCalculate,
        Int32 dMinimum,
        Int32 dMaximum,
        Int32 nMinimum,
        Int32 nMaximum,
        BackgroundWorker backgroundWorker,
        out ICollection<Motif> motifs
    )
    {
        Debug.Assert(graph != null);

        List<Motif> oMotifs = new List<Motif>();
        motifs = oMotifs;

        if ((motifsToCalculate & Motifs.Fan) != 0)
        {
            ICollection<Motif> oFanMotifs;

            if (!TryCalculateFanMotifs(graph, backgroundWorker,
                out oFanMotifs))
            {
                return (false);
            }

            oMotifs.AddRange(oFanMotifs);
        }

        if ((motifsToCalculate & Motifs.DConnector) != 0)
        {
            ICollection<Motif> oDConnectorMotifs;

            if (!TryCalculateDConnectorMotifs(graph, dMinimum, dMaximum, backgroundWorker,
                out oDConnectorMotifs))
            {
                return (false);
            }

            oMotifs.AddRange(oDConnectorMotifs);
        }

        if ((motifsToCalculate & Motifs.Clique) != 0)
        {
            ICollection<Motif> oCliqueMotifs;

            if (!TryCalculateCliqueMotifs(graph, nMinimum, nMaximum, backgroundWorker,
                oMotifs, out oCliqueMotifs))
            {
                return (false);
            }

            oMotifs.AddRange(oCliqueMotifs);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateFanMotifs()
    //
    /// <summary>
    /// Attempts to calculate a set of fan motifs.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to calculate the motifs for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="oMotifs">
    /// Where a collection of zero or more <see cref="FanMotif" /> objects gets
    /// stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the motifs were calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateFanMotifs
    (
        IGraph oGraph,
        BackgroundWorker oBackgroundWorker,
        out ICollection<Motif> oMotifs
    )
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
            if ( !ReportProgressIfNecessary(iCalculationsSoFar, iVertices,
                oBackgroundWorker) )
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
                        new FanMotif( oVertex, oLeaves.ToArray() ) );
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

    //*************************************************************************
    //  Method: SetFanMotifArcScale()
    //
    /// <summary>
    /// Sets the <see cref="FanMotif.ArcScale" /> property on each fan motif.
    /// </summary>
    ///
    /// <param name="oFanMotifs">
    /// A collection of zero or more <see cref="FanMotif" /> objects.
    /// </param>
    //*************************************************************************

    protected void
    SetFanMotifArcScale
    (
        ICollection<Motif> oFanMotifs
    )
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
                oMotif => ( (FanMotif)oMotif ).LeafVertices.Length);

            iMaximumLeafCount = oFanMotifs.Max(
                oMotif => ( (FanMotif)oMotif ).LeafVertices.Length);
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

    //*************************************************************************
    //  Method: TryCalculateDConnectorMotifs()
    //
    /// <summary>
    /// Attempts to calculate a set of D-connector motifs.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to calculate the motifs for.
    /// </param>
    ///
    /// <param name="iDMinimum">
    /// The minimum number of anchor vertices (dimension) of D-connector motif to find.
    /// </param>
    /// 
    /// <param name="iDMaximum">
    /// The maximum number of anchor vertices (dimension) of D-connector motif to find.
    /// </param>
    /// 
    /// <param name="oBackgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="oMotifs">
    /// Where a collection of zero or more <see cref="DConnectorMotif" />
    /// objects gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the motifs were calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateDConnectorMotifs
    (
        IGraph oGraph,
        Int32 iDMinimum,
        Int32 iDMaximum,
        BackgroundWorker oBackgroundWorker,
        out ICollection<Motif> oMotifs
    )
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
            if ( !ReportProgressIfNecessary(iCalculationsSoFar, iVertices,
                oBackgroundWorker) )
            {
                return (false);
            }

            // Get only the non-self-loop adjacent vertices
            ICollection<IVertex> oPotentialAnchorVertices =
                oPotentialSpanVertex.AdjacentVertices
                .Where<IVertex>(adjVertex => adjVertex != oPotentialSpanVertex).ToList<IVertex>();

            if ( DVerticesMightBeAnchors(oPotentialAnchorVertices, iDMinimum, iDMaximum) )
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

    //*************************************************************************
    //  Method: DVerticesMightBeAnchors()
    //
    /// <summary>
    /// Determines whether a collection of vertices might be anchor vertices in
    /// a D-connector motif.
    /// </summary>
    ///
    /// <param name="oVertices">
    /// The collection to test.
    /// </param>
    ///
    /// <param name="iDMinimum">
    /// The minimum number of anchor vertices (dimension) of D-connector motif to find.
    /// </param>
    /// 
    /// <param name="iDMaximum">
    /// The maximum number of anchor vertices (dimension) of D-connector motif to find.
    /// </param>
    ///
    /// <returns>
    /// true if the vertices might be anchor vertices, false if they cannot be.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DVerticesMightBeAnchors
    (
        ICollection<IVertex> oVertices,
        Int32 iDMinimum,
        Int32 iDMaximum
    )
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

    //*************************************************************************
    //  Method: AddSpanVertexToPotentialDConnectorMotifs()
    //
    /// <summary>
    /// Adds a potential span vertex to a dictionary of potential D-connector
    /// motifs.
    /// </summary>
    ///
    /// <param name="oPotentialSpanVertex">
    /// Vertex that might be a span vertex.
    /// </param>
    ///
    /// <param name="oDPotentialAnchorVertices">
    /// Collection of D vertices that might be anchor vertices for <paramref
    /// name="oPotentialSpanVertex" />.
    /// </param>
    ///
    /// <param name="oPotentialDConnectorMotifs">
    /// The key is an ordered combination of the vertex IDs of the potential
    /// motif's D anchor vertices, and the value is the corresponding
    /// potential DConnectorMotif object.
    /// </param>
    //*************************************************************************

    protected void
    AddSpanVertexToPotentialDConnectorMotifs
    (
        IVertex oPotentialSpanVertex,
        ICollection<IVertex> oDPotentialAnchorVertices,
        Dictionary<string, DConnectorMotif> oPotentialDConnectorMotifs
    )
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


    //*************************************************************************
    //  Method: FilterDConnectorMotifs()
    //
    /// <summary>
    /// Filters a collection of potential D-connector motifs and adds the real
    /// or more favorable overlapping ones to a new collection.
    /// </summary>
    ///
    /// <param name="oPotentialDConnectorMotifs">
    /// The key is an ordered combination of the vertex IDs of the potential
    /// motif's D anchor vertices, and the value is the corresponding
    /// potential DConnectorMotif object.
    /// </param>
    ///
    /// <returns>
    /// A new collection of zero or more <see cref="DConnectorMotif" />
    /// objects.
    /// </returns>
    //*************************************************************************

    protected ICollection<Motif>
    FilterDConnectorMotifs
    (
        Dictionary<string, DConnectorMotif> oPotentialDConnectorMotifs
    )
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

    //*************************************************************************
    //  Method: AddDConnectorMotif()
    //
    /// <summary>
    /// Adds a new DConnectorMotif to the collections and creates mappings from
    /// its vertices to it
    /// </summary>
    /// 
    /// <param name="currentDConnectorMotifs">
    /// The current DConnectorMotif collection to add to
    /// </param>
    /// 
    /// <param name="verticesAlreadyInDConnectorMotifs">
    /// The mapping between seen vertices and their associated DConnectorMotifs
    /// </param>
    /// 
    /// <param name="connectorMotifToAdd">
    /// The DConnectorMotif to add to the collections
    /// </param>
    //*************************************************************************

    private static void
    AddDConnectorMotif
    (
        HashSet<Motif> currentDConnectorMotifs,
        Dictionary<IVertex, DConnectorMotif> verticesAlreadyInDConnectorMotifs,
        DConnectorMotif connectorMotifToAdd)
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

    //*************************************************************************
    //  Method: SetDConnectorMotifSpanScale()
    //
    /// <summary>
    /// Sets the <see cref="DConnectorMotif.SpanScale" /> property on each
    /// D-connector motif.
    /// </summary>
    ///
    /// <param name="oDConnectorMotifs">
    /// A collection of zero or more <see cref="DConnectorMotif" /> objects.
    /// </param>
    //*************************************************************************

    protected void
    SetDConnectorMotifSpanScale
    (
        ICollection<Motif> oDConnectorMotifs
    )
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

    //*************************************************************************
    //  Method: TryCalculateCliqueMotifs()
    //
    /// <summary>
    /// Attempts to calculate a set of clique motifs.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// Graph to calculate the motifs for.
    /// </param>
    ///
    /// <param name="iNMinimum">
    /// The minimum number of member vertices of clique motif to find.
    /// </param>
    /// 
    /// <param name="iNMaximum">
    /// The maximum number of member vertices of clique motif to find.
    /// </param>
    /// 
    /// <param name="oBackgroundWorker">
    /// The BackgroundWorker whose thread is calling this method, or null if
    /// the method is being called by some other thread.
    /// </param>
    ///
    /// <param name="oExistingMotifs">
    /// Existing motifs to avoid overlap with (null if none exist)
    /// </param>
    /// 
    /// <param name="oMotifs">
    /// Where a collection of zero or more <see cref="CliqueMotif" />
    /// objects gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// True if the motifs were calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateCliqueMotifs
    (
        IGraph oGraph,
        Int32 iNMinimum,
        Int32 iNMaximum,
        BackgroundWorker oBackgroundWorker,
        ICollection<Motif> oExistingMotifs,
        out ICollection<Motif> oMotifs
    )
    {
        Debug.Assert(oGraph != null);

        oMotifs = null;

        ClusterCalculator clusterCalculator = new ClusterCalculator();
        clusterCalculator.Algorithm = ClusterAlgorithm.Clique;
        ICollection<Community> communities;
         
        if ( clusterCalculator.TryCalculateGraphMetrics(oGraph, oBackgroundWorker,
            out communities) )
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
                    if (!ReportProgressIfNecessary(iCalculationsSoFar, iTotalOperations,
                        oBackgroundWorker))
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

    //*************************************************************************
    //  Method: SetCliqueMotifScale()
    //
    /// <summary>
    /// Sets the <see cref="CliqueMotif.CliqueScale" /> property on each clique motif.
    /// </summary>
    ///
    /// <param name="oCliqueMotifs">
    /// A collection of zero or more <see cref="CliqueMotif" /> objects.
    /// </param>
    //*************************************************************************

    protected void
    SetCliqueMotifScale
    (
        ICollection<Motif> oCliqueMotifs
    )
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

    //*************************************************************************
    //  Method: ReportProgressIfNecessary()
    //
    /// <summary>
    /// Periodically checks for cancellation and reports progress.
    /// </summary>
    ///
    /// <param name="iCalculationsSoFar">
    /// Number of calculations that have been performed so far.
    /// </param>
    ///
    /// <param name="iTotalCalculations">
    /// Total number of calculations.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The <see cref="BackgroundWorker" /> object that is performing all graph
    /// metric calculations.
    /// </param>
    ///
    /// <returns>
    /// false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ReportProgressIfNecessary
    (
        Int32 iCalculationsSoFar,
        Int32 iTotalCalculations,
        BackgroundWorker oBackgroundWorker
    )
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


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Number of vertices that are processed before progress is reported and
    /// the cancellation flag is checked.

    protected const Int32 VerticesPerProgressReport = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
