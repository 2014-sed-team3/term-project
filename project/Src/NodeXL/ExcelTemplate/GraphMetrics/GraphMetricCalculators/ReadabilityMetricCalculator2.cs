
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using Smrf.GraphicsLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ReadabilityMetricCalculator2
//
/// <summary>
/// Calculates readability metrics for the graph.
/// </summary>
///
/// <remarks>
/// The readability metrics calculated by this class are based on work done by
/// Cody Dunne and Ben Shneiderman at the University of Maryland.
///
/// <para>
/// This graph metric calculator differs from most other calculators in that it
/// uses the vertices' locations and shapes.  The other calculators look only 
/// at how the vertices are connected to each other.  To avoid making the
/// lower-level Algorithms project dependent on higher-level drawing classes
/// (in particular, the <see cref="VertexDrawingHistory" /> class), there
/// is no corresponding lower-level ReadabilityMetricCalculator class in the
/// <see cref="Smrf.NodeXL.Algorithms" /> namespace, and the readability
/// metrics cannot be calculated outside of this ExcelTemplate project.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ReadabilityMetricCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: ReadabilityMetricCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ReadabilityMetricCalculator2" /> class.
    /// </summary>
    ///
    /// <param name="graphRectangleSize">
    /// Size of the rectangle containing the graph, in WPF units.
    /// </param>
    //*************************************************************************

    public ReadabilityMetricCalculator2
    (
        System.Windows.Size graphRectangleSize
    )
    {
        m_oGraphRectangleSize = graphRectangleSize;

        AssertValid();
    }

    //*************************************************************************
    //  Method: TryCalculateGraphMetrics()
    //
    /// <summary>
    /// Attempts to calculate a set of one or more related metrics.
    /// </summary>
    ///
    /// <param name="graph">
    /// The graph to calculate metrics for.  The graph may contain duplicate
    /// edges and self-loops.
    /// </param>
    ///
    /// <param name="calculateGraphMetricsContext">
    /// Provides access to objects needed for calculating graph metrics.
    /// </param>
    ///
    /// <param name="graphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated or don't need to be
    /// calculated, false if the user wants to cancel.
    /// </returns>
    ///
    /// <remarks>
    /// This method periodically checks BackgroundWorker.<see
    /// cref="BackgroundWorker.CancellationPending" />.  If true, the method
    /// immediately returns false.
    ///
    /// <para>
    /// It also periodically reports progress by calling the
    /// BackgroundWorker.<see
    /// cref="BackgroundWorker.ReportProgress(Int32, Object)" /> method.  The
    /// userState argument is a <see cref="GraphMetricProgress" /> object.
    /// </para>
    ///
    /// <para>
    /// Calculated metrics for hidden rows are ignored by the caller, because
    /// Excel misbehaves when values are written to hidden cells.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public override Boolean
    TryCalculateGraphMetrics
    (
        IGraph graph,
        CalculateGraphMetricsContext calculateGraphMetricsContext,
        out GraphMetricColumn [] graphMetricColumns
    )
    {
        Debug.Assert(graph != null);
        Debug.Assert(calculateGraphMetricsContext != null);
        AssertValid();

        graphMetricColumns = new GraphMetricColumn[0];

        BackgroundWorker oBackgroundWorker =
            calculateGraphMetricsContext.BackgroundWorker;

        ReadabilityMetricUserSettings oReadabilityMetricUserSettings =
            new ReadabilityMetricUserSettings();

        IGraph oGraphClone = CloneAndFilterGraph(graph);

        // The key is an IEdge and the value is the number of edges that cross
        // the edge.

        Dictionary<IEdge, Int32> oEdgeInformation;

        // The key is an IVertex and the value is a VertexInformation object.

        Dictionary<IVertex, VertexInformation> oVertexInformations;

        // Future improvement: Calculate edge and vertex information only when
        // necessary, based on the metrics selected by the user.

        LinkedList<GraphMetricColumn> oGraphMetricColumns =
            new LinkedList<GraphMetricColumn>();

        if (
            !TryCalculateEdgeInformation(oGraphClone, oBackgroundWorker,
                out oEdgeInformation)
            ||
            !TryCalculateVertexInformations(oGraphClone, oBackgroundWorker,
                out oVertexInformations)
            ||
            !TryCalculatePerEdgeAndPerVertexMetrics(oGraphClone, graph,
                oReadabilityMetricUserSettings, oBackgroundWorker,
                oEdgeInformation, oVertexInformations, oGraphMetricColumns)
            ||
            !TryCalculateOverallMetrics(oGraphClone,
                oReadabilityMetricUserSettings, oBackgroundWorker,
                oEdgeInformation, oVertexInformations, oGraphMetricColumns)
            )
        {
            // The user cancelled.

            return (false);
        }

        graphMetricColumns = oGraphMetricColumns.ToArray();

        return (true);
    }

    //*************************************************************************
    //  Property: HandlesDuplicateEdges
    //
    /// <summary>
    /// Gets a flag indicating whether duplicate edges are properly handled.
    /// </summary>
    ///
    /// <value>
    /// true if the graph metric calculator handles duplicate edges, false if
    /// duplicate edges should be removed from the graph before the
    /// calculator's <see cref="TryCalculateGraphMetrics" /> method is called.
    /// </value>
    //*************************************************************************

    public override Boolean
    HandlesDuplicateEdges
    {
        get
        {
            AssertValid();

            // We don't want GraphMetricCalculationManager to remove duplicate
            // edges, because the Graph object that was passed to it by
            // TaskPane belongs to the NodeXLControl in the TaskPane and edges
            // in that control should not be removed.

            return (true);
        }
    }

    //*************************************************************************
    //  Method: CloneAndFilterGraph()
    //
    /// <summary>
    /// Clones the graph that was passed to this class, filtering out unwanted
    /// vertices and edges in the process.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <returns>
    /// A clone of <paramref name="oGraph" />, with unwanted vertices and edges
    /// removed.
    /// </returns>
    ///
    /// <remarks>
    /// Hidden vertices and edges are excluded from the clone, as are
    /// overlapping edges.
    /// </remarks>
    //*************************************************************************

    protected IGraph
    CloneAndFilterGraph
    (
        IGraph oGraph
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        IGraph oGraphClone = new Graph(GraphDirectedness.Undirected);

        // The key is the IVertex in oGraph and the value is the corresponding
        // IVertex in oGraphClone.

        Dictionary<IVertex, IVertex> oVertexDictionary =
            new Dictionary<IVertex, IVertex>();

        IVertexCollection oVerticesClone = oGraphClone.Vertices;

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            VertexDrawingHistory oVertexDrawingHistory;

            // (If the vertex has no VertexDrawingHistory, it means that it's
            // hidden and should be ignored.)

            if ( DrawerBase.TryGetVertexDrawingHistory(oVertex,
                out oVertexDrawingHistory) )
            {
                IVertex oVertexClone = oVertex.Clone(true, true);
                oVertexClone.Location = oVertex.Location;
                oVerticesClone.Add(oVertexClone);
                oVertexDictionary.Add(oVertex, oVertexClone);
            }
        }

        // This is used to eliminate overlapping edges, where edges are
        // considered overlapping if they connect the same vertex pairs.  The
        // directedness of the edges is not taken into account.
        //
        // For readability calculations, there should be only one edge
        // connecting a pair of vertexes.

        HashSet<Int64> oVertexIDPairs = new HashSet<Int64>();

        IEdgeCollection oEdgesClone = oGraphClone.Edges;

        foreach (IEdge oEdge in oGraph.Edges)
        {
            EdgeDrawingHistory oEdgeDrawingHistory;
            Int64 i64VertexIDPair = EdgeUtil.GetVertexIDPair(oEdge, false);

            if (
                !oEdge.IsSelfLoop
                &&
                DrawerBase.TryGetEdgeDrawingHistory(oEdge,
                    out oEdgeDrawingHistory)
                &&
                !oVertexIDPairs.Contains(i64VertexIDPair)
                )
            {
                oEdgesClone.Add( oEdge.Clone(true, true,
                    oVertexDictionary[ oEdge.Vertices[0] ],
                    oVertexDictionary[ oEdge.Vertices[1] ], false) );

                oVertexIDPairs.Add(i64VertexIDPair);
            }
        }

        return (oGraphClone);
    }

    //*************************************************************************
    //  Method: TryCalculateEdgeInformation()
    //
    /// <summary>
    /// Calculates information about the graph's edges.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oEdgeInformation">
    /// The key is an IEdge and the value is the number of edges that cross the
    /// edge.
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateEdgeInformation
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        out Dictionary<IEdge, Int32> oEdgeInformation
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        AssertValid();

        Int32 iCalculationsCompleted = 0;
        IEdgeCollection oEdges = oGraphClone.Edges;
        Int32 iEdges = oEdges.Count;

        oEdgeInformation = new Dictionary<IEdge, Int32>(iEdges);

        foreach (IEdge oEdge in oEdges)
        {
            if ( CancellationPending(oBackgroundWorker, iCalculationsCompleted,
                iEdges, "Counting edge crossings.") )
            {
                return (false);
            }

            Int32 iEdgeCrossings = 0;

            foreach (IEdge oOtherEdge in oEdges)
            {
                if ( EdgesIntersect(oEdge, oOtherEdge) )
                {
                    iEdgeCrossings++;
                }
            }

            oEdgeInformation.Add(oEdge, iEdgeCrossings);
            iCalculationsCompleted++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateVertexInformations()
    //
    /// <summary>
    /// Calculates information about the graph's vertices.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if the user cancelled.
    /// </returns>
    ///
    /// <remarks>
    /// If a vertex is hidden, it is not included in the Dictionary.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    TryCalculateVertexInformations
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        out Dictionary<IVertex, VertexInformation> oVertexInformations
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        AssertValid();

        Int32 iCalculationsCompleted = 0;
        IVertexCollection oVertices = oGraphClone.Vertices;
        Int32 iVertices = oVertices.Count;

        oVertexInformations =
            new Dictionary<IVertex, VertexInformation>(iVertices);

        foreach (IVertex oVertex in oVertices)
        {
            if ( CancellationPending(oBackgroundWorker, iCalculationsCompleted,
                iVertices, "Calculating vertex areas.") )
            {
                return (false);
            }

            VertexDrawingHistory oVertexDrawingHistory;

            if ( !DrawerBase.TryGetVertexDrawingHistory(oVertex,
                out oVertexDrawingHistory) )
            {
                Debug.Assert(false);
            }

            Geometry oBounds = oVertexDrawingHistory.GetBounds();
            WpfGraphicsUtil.FreezeIfFreezable(oBounds);

            VertexInformation oVertexInformation = new VertexInformation();
            oVertexInformation.Bounds = oBounds;
            oVertexInformation.Area = GetArea(oBounds);

            oVertexInformations.Add(oVertex, oVertexInformation);

            iCalculationsCompleted++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculatePerEdgeAndPerVertexMetrics()
    //
    /// <summary>
    /// Attempts to calculate the per-edge and per-vertex readability metrics.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oGraph">
    /// The original unfiltered graph.
    /// </param>
    ///
    /// <param name="oReadabilityMetricUserSettings">
    /// Stores the user's settings for calculating readability metrics.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oEdgeInformation">
    /// The key is an IEdge and the value is the number of edges that cross the
    /// edge.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// This method adds objects to this collection, one for each metric it
    /// calculates.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated or don't need to be
    /// calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculatePerEdgeAndPerVertexMetrics
    (
        IGraph oGraphClone,
        IGraph oGraph,
        ReadabilityMetricUserSettings oReadabilityMetricUserSettings,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IEdge, Int32> oEdgeInformation,
        Dictionary<IVertex, VertexInformation> oVertexInformations,
        LinkedList<GraphMetricColumn> oGraphMetricColumns
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oGraph != null);
        Debug.Assert(oReadabilityMetricUserSettings != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oEdgeInformation != null);
        Debug.Assert(oVertexInformations != null);
        Debug.Assert(oGraphMetricColumns != null);
        AssertValid();

        ICollection<GraphMetricValueWithID> oGraphMetricValuesWithID;

        if ( oReadabilityMetricUserSettings.ShouldCalculateReadabilityMetrics(
                ReadabilityMetrics.PerEdgeEdgeCrossings) )
        {
            if ( !TryCalculatePerEdgeEdgeCrossings(oGraphClone, oGraph,
                oBackgroundWorker, oEdgeInformation,
                out oGraphMetricValuesWithID) )
            {
                return (false);
            }

            oGraphMetricColumns.AddLast(
                new GraphMetricColumnWithID( WorksheetNames.Edges,
                    TableNames.Edges, EdgeTableColumnNames.EdgeCrossings,
                    ExcelTableUtil.AutoColumnWidth, NumericFormat,
                    CellStyleNames.GraphMetricGood,
                    oGraphMetricValuesWithID.ToArray() ) );
        }

        if ( oReadabilityMetricUserSettings.ShouldCalculateReadabilityMetrics(
                ReadabilityMetrics.PerVertexEdgeCrossings) )
        {
            if ( !TryCalculatePerVertexEdgeCrossings(oGraphClone,
                oBackgroundWorker, oEdgeInformation, oVertexInformations,
                out oGraphMetricValuesWithID) )
            {
                return (false);
            }

            oGraphMetricColumns.AddLast(
                new GraphMetricColumnWithID( WorksheetNames.Vertices,
                    TableNames.Vertices, VertexTableColumnNames.EdgeCrossings,
                    ExcelTableUtil.AutoColumnWidth, NumericFormat,
                    CellStyleNames.GraphMetricGood,
                    oGraphMetricValuesWithID.ToArray() ) );
        }

        if ( oReadabilityMetricUserSettings.ShouldCalculateReadabilityMetrics(
                ReadabilityMetrics.PerVertexVertexOverlap) )
        {
            if ( !TryCalculatePerVertexVertexOverlap(oGraphClone,
                oBackgroundWorker, oVertexInformations,
                out oGraphMetricValuesWithID) )
            {
                return (false);
            }

            oGraphMetricColumns.AddLast(
                new GraphMetricColumnWithID( WorksheetNames.Vertices,
                    TableNames.Vertices, VertexTableColumnNames.VertexOverlap,
                    ExcelTableUtil.AutoColumnWidth, NumericFormat,
                    CellStyleNames.GraphMetricGood,
                    oGraphMetricValuesWithID.ToArray() ) );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculatePerEdgeEdgeCrossings()
    //
    /// <summary>
    /// Attempts to calculate the per-edge edge crossing metric for each edge.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oGraph">
    /// The original unfiltered graph.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oEdgeInformation">
    /// The key is an IEdge and the value is the number of edges that cross the
    /// edge.
    /// </param>
    ///
    /// <param name="oPerEdgeEdgeCrossings">
    /// Where the metrics gets stored if true is returned.  Never null.
    /// </param>
    ///
    /// <returns>
    /// true if the metric was calculated, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculatePerEdgeEdgeCrossings
    (
        IGraph oGraphClone,
        IGraph oGraph,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IEdge, Int32> oEdgeInformation,
        out ICollection<GraphMetricValueWithID> oPerEdgeEdgeCrossings
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oGraph != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oEdgeInformation != null);
        AssertValid();

        Int32 iEdges = oGraphClone.Edges.Count;

        // The key is a row ID from the edge worksheet and the value is the
        // corresponding GraphMetricValueWithID object.

        Dictionary<Int32, GraphMetricValueWithID> oDictionary =
            new Dictionary<Int32, GraphMetricValueWithID>(iEdges);

        oPerEdgeEdgeCrossings = oDictionary.Values;
        Int32 iCalculationsCompleted = 0;

        // Calculate the number of edges that a single edge could cross.

        Int32 iCAll = iEdges - 1;

        foreach (IEdge oEdge in oGraphClone.Edges)
        {
            if ( CancellationPending(oBackgroundWorker, iCalculationsCompleted,
                iEdges, "Calculating per-edge edge crossings.") )
            {
                return (false);
            }

            Int32 iRowID;

            if ( !TryGetRowID(oEdge, out iRowID) )
            {
                goto SkipCalculation;
            }

            // Calculate the impossible crossings for this edge.

            IVertex [] oVertices = oEdge.Vertices;
            Int32 iCImpossible = oVertices[0].Degree + oVertices[1].Degree - 2;

            // Calculate the upper bound of crossings for this edge.

            Int32 iCMx = iCAll - iCImpossible;

            // Calculate the edge-crossing metric for this edge.

            Int32 iEdgeCrossings = oEdgeInformation[oEdge];

            Double dEdgeCrossingMetric = (iCMx > 0) ?
                1.0 - ( (Double)iEdgeCrossings / (Double)iCMx ) : 1.0;

            oDictionary.Add( iRowID,
                new GraphMetricValueWithID(iRowID, dEdgeCrossingMetric) );

            SkipCalculation:

            iCalculationsCompleted++;
        }

        AddMissingPerEdgeEdgeCrossings(oGraphClone, oGraph, oDictionary);

        return (true);
    }

    //*************************************************************************
    //  Method: AddMissingPerEdgeEdgeCrossings()
    //
    /// <summary>
    /// Adds the per-edge edge crossing metrics that are missing for
    /// overlapping edges.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oGraph">
    /// The original unfiltered graph.
    /// </param>
    ///
    /// <param name="oDictionary">
    /// The key is a row ID from the edge worksheet and the value is the
    /// corresponding GraphMetricValueWithID object.
    /// </param>
    ///
    /// <remarks>
    /// The original unfiltered graph may contain one or more groups of
    /// overlapping edges.  All but one of the overlapping edges in each group
    /// were removed before the per-edge edge crossing metrics were calculated,
    /// which means that no metric was calculated for the other edges in the
    /// group.  This method adds a metric for the other edges in the group by
    /// copying the one value that was calculated.
    /// </remarks>
    //*************************************************************************

    protected void
    AddMissingPerEdgeEdgeCrossings
    (
        IGraph oGraphClone,
        IGraph oGraph,
        Dictionary<Int32, GraphMetricValueWithID> oDictionary
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oGraph != null);
        Debug.Assert(oDictionary != null);
        AssertValid();

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Int32 iRowID;

            if (
                !TryGetRowID(oEdge, out iRowID)
                || 
                oDictionary.ContainsKey(iRowID)
                )
            {
                // A metric has already been calculated for this edge.

                continue;
            }

            // Examine the adjacent vertex with the smallest number of incident
            // edges.

            IVertex oVertex1 = oEdge.Vertices[0];
            IVertex oVertex2 = oEdge.Vertices[1];
            IVertex oVertexToUse, oAdjacentVertex;

            if (oVertex1.Degree < oVertex2.Degree)
            {
                oVertexToUse = oVertex1;
                oAdjacentVertex = oVertex2;
            }
            else
            {
                oVertexToUse = oVertex2;
                oAdjacentVertex = oVertex1;
            }

            // Find the overlapping edge for which a metric was calculated.

            foreach (IEdge oIncidentEdge in oVertexToUse.IncidentEdges)
            {
                Int32 iIncidentEdgeRowID;
                GraphMetricValueWithID oGraphMetricValueWithID;

                if (
                    oIncidentEdge != oEdge
                    &&
                    oIncidentEdge.GetAdjacentVertex(oVertexToUse) == 
                        oAdjacentVertex
                    &&
                    TryGetRowID(oIncidentEdge, out iIncidentEdgeRowID)
                    && 
                    oDictionary.TryGetValue(iIncidentEdgeRowID,
                        out oGraphMetricValueWithID)
                    )
                {
                    oDictionary.Add( iRowID,
                        new GraphMetricValueWithID(iRowID,
                            oGraphMetricValueWithID.Value) );

                    break;
                }
            }
        }
    }

    //*************************************************************************
    //  Method: TryCalculatePerVertexVertexOverlap()
    //
    /// <summary>
    /// Attempts to calculate the per-vertex vertex overlap metric for each
    /// vertex.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <param name="oPerVertexVertexOverlap">
    /// Where the metrics gets stored if true is returned.  Never null.
    /// </param>
    ///
    /// <returns>
    /// true if the metric was calculated, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculatePerVertexVertexOverlap
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IVertex, VertexInformation> oVertexInformations,
        out ICollection<GraphMetricValueWithID> oPerVertexVertexOverlap
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oVertexInformations != null);
        AssertValid();

        LinkedList<GraphMetricValueWithID> oLinkedList =
            new LinkedList<GraphMetricValueWithID>();

        oPerVertexVertexOverlap = oLinkedList;
        Int32 iCalculationsCompleted = 0;

        foreach (IVertex oVertex in oGraphClone.Vertices)
        {
            if ( CancellationPending(oBackgroundWorker, iCalculationsCompleted,
                oGraphClone.Vertices.Count,
                "Calculating per-vertex vertex overlap.") )
            {
                return (false);
            }

            Int32 iRowID;

            if ( !TryGetRowID(oVertex, out iRowID) )
            {
                goto SkipCalculation;
            }

            VertexInformation oVertexInformation = oVertexInformations[oVertex];

            // Note: It was determined experimentally that using a
            // CombinedGeometry here is much faster than using a GeometryGroup.

            CombinedGeometry oUnionOfIntersectionsOfBounds =
                new CombinedGeometry();

            Rect oVertexRectangle = oVertexInformation.Bounds.Bounds;

            foreach (IVertex oOtherVertex in oGraphClone.Vertices)
            {
                if (oOtherVertex == oVertex)
                {
                    continue;
                }

                VertexInformation oOtherVertexInformation =
                    oVertexInformations[oOtherVertex];

                Rect oOtherVertexRectangle =
                    oOtherVertexInformation.Bounds.Bounds;

                if ( !oOtherVertexRectangle.IntersectsWith(oVertexRectangle) )
                {
                    // This is an optimization.  Ruling out vertices whose
                    // rectangular bounds don't intersect is much faster than
                    // intersecting their geometries.

                    continue;
                }

                CombinedGeometry oIntersectionOfBounds = new CombinedGeometry(
                    GeometryCombineMode.Intersect, oVertexInformation.Bounds,
                    oOtherVertexInformation.Bounds);

                oUnionOfIntersectionsOfBounds = new CombinedGeometry(
                    GeometryCombineMode.Union, oUnionOfIntersectionsOfBounds,
                    oIntersectionOfBounds);
            }

            Double dVertexOverlap = 1.0 -
                ( GetArea(oUnionOfIntersectionsOfBounds) /
                    GetArea(oVertexInformation.Bounds) );

            oLinkedList.AddLast( new GraphMetricValueWithID(
                iRowID, dVertexOverlap) );

            SkipCalculation:

            iCalculationsCompleted++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculatePerVertexEdgeCrossings()
    //
    /// <summary>
    /// Attempts to calculate the per-vertex edge crossing metric for each
    /// vertex.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oEdgeInformation">
    /// The key is an IEdge and the value is the number of edges that cross the
    /// edge.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <param name="oPerVertexEdgeCrossings">
    /// Where the metrics gets stored if true is returned.  Never null.
    /// </param>
    ///
    /// <returns>
    /// true if the metric was calculated, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculatePerVertexEdgeCrossings
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IEdge, Int32> oEdgeInformation,
        Dictionary<IVertex, VertexInformation> oVertexInformations,
        out ICollection<GraphMetricValueWithID> oPerVertexEdgeCrossings
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oEdgeInformation != null);
        Debug.Assert(oVertexInformations != null);
        AssertValid();

        LinkedList<GraphMetricValueWithID> oLinkedList =
            new LinkedList<GraphMetricValueWithID>();

        oPerVertexEdgeCrossings = oLinkedList;
        Int32 iCalculationsCompleted = 0;

        foreach (IVertex oVertex in oGraphClone.Vertices)
        {
            if ( CancellationPending(oBackgroundWorker, iCalculationsCompleted,
                oGraphClone.Vertices.Count,
                "Calculating per-vertex edge crossings.") )
            {
                return (false);
            }

            Int32 iRowID;

            if ( !TryGetRowID(oVertex, out iRowID) )
            {
                goto SkipCalculation;
            }

            VertexInformation oVertexInformation =
                oVertexInformations[oVertex];

            // Count 1) the edges that cross the vertex's incident edges; and
            // 2) the sum of the adjacent vertices' degrees.

            Int32 iEdgeCrossings = 0;
            Int32 iAdjacentVertexDegreeSum = 0;

            foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
            {
                iEdgeCrossings += oEdgeInformation[oIncidentEdge];

                iAdjacentVertexDegreeSum +=
                    oIncidentEdge.GetAdjacentVertex(oVertex).Degree;
            }

            // Calculate the upper bound to the number of edges that cross the
            // vertex's incident edges.

            Int32 iDegree = oVertex.Degree;

            Int32 iCMx = ( iDegree * (oGraphClone.Edges.Count + 1 - iDegree) )
                - iAdjacentVertexDegreeSum;

            Double dEdgeCrossingMetric = 
                (iCMx > 0) ? 1.0 - ( (Double)iEdgeCrossings / (Double)iCMx )
                : 1.0;

            oLinkedList.AddLast( new GraphMetricValueWithID(
                iRowID, dEdgeCrossingMetric) );

            SkipCalculation:

            iCalculationsCompleted++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateOverallMetrics()
    //
    /// <summary>
    /// Attempts to calculate the overall readability metrics.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oReadabilityMetricUserSettings">
    /// Stores the user's settings for calculating readability metrics.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oEdgeInformation">
    /// The key is an IEdge and the value is the number of edges that cross the
    /// edge.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// This method adds objects to this collection, one for each metric it
    /// calculates.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated or don't need to be
    /// calculated, false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateOverallMetrics
    (
        IGraph oGraphClone,
        ReadabilityMetricUserSettings oReadabilityMetricUserSettings,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IEdge, Int32> oEdgeInformation,
        Dictionary<IVertex, VertexInformation> oVertexInformations,
        LinkedList<GraphMetricColumn> oGraphMetricColumns
    )
    {
        AssertValid();

        Boolean bCalculateOverallEdgeCrossings =
            oReadabilityMetricUserSettings.ShouldCalculateReadabilityMetrics(
                ReadabilityMetrics.OverallEdgeCrossings);

        Boolean bCalculateOverallVertexOverlap =
            oReadabilityMetricUserSettings.ShouldCalculateReadabilityMetrics(
                ReadabilityMetrics.OverallVertexOverlap);

        Boolean bCalculateGraphRectangleCoverage =
            oReadabilityMetricUserSettings.ShouldCalculateReadabilityMetrics(
                ReadabilityMetrics.GraphRectangleCoverage);

        if (!bCalculateOverallEdgeCrossings && !bCalculateOverallVertexOverlap 
            && !bCalculateGraphRectangleCoverage)
        {
            return (true);
        }

        Nullable<Double> dOverallEdgeCrossings = null;
        Nullable<Double> dOverallVertexOverlap = null;
        Nullable<Double> dGraphRectangleCoverage = null;

        if (
            ( bCalculateOverallEdgeCrossings &&
                !TryCalculateOverallEdgeCrossings(oGraphClone,
                oBackgroundWorker, oEdgeInformation, oVertexInformations,
                out dOverallEdgeCrossings)
            )
            ||
            ( bCalculateOverallVertexOverlap &&
                !TryCalculateOverallVertexOverlap(oGraphClone,
                oBackgroundWorker, oVertexInformations,
                out dOverallVertexOverlap)
            )
            ||
            ( bCalculateGraphRectangleCoverage &&
                !TryCalculateGraphRectangleCoverage(oGraphClone,
                oBackgroundWorker, oVertexInformations,
                out dGraphRectangleCoverage)
            )
            )
        {
            // The user cancelled.

            return (false);
        }

        // Known problem: 
        //
        // The following code will calculate either or both of the two overall
        // readability metrics, depending on the user's settings.  If it
        // doesn't calculate one of them, it writes an empty string to the
        // corresponding value cell in the OverallReadabilityMetrics table
        // instead of leaving any existing value untouched.
        //
        // This differs from what occurs when other readability metrics don't
        // get calculated.  In the other cases, any metrics that have already
        // been calculated do not get overwritten.
        //
        // Fixing this will require modifying the way that
        // GraphMetricColumnOrdered objects get written to the workbook by
        // GraphMetricWriter.WriteGraphMetricColumnOrderedToWorkbook().

        oGraphMetricColumns.AddLast(

            new GraphMetricColumnOrdered( WorksheetNames.OverallMetrics,
                TableNames.OverallReadabilityMetrics,
                OverallReadabilityMetricsTableColumnNames.Name,
                ExcelTableUtil.AutoColumnWidth, null,
                CellStyleNames.GraphMetricGood,

                new GraphMetricValueOrdered [] {

                    new GraphMetricValueOrdered("Edge Crossing Readability"),
                    new GraphMetricValueOrdered("Vertex Overlap Readability"),
                    new GraphMetricValueOrdered("Graph Pane Coverage"),
                } ) );

        oGraphMetricColumns.AddLast(

            new GraphMetricColumnOrdered(WorksheetNames.OverallMetrics,
                TableNames.OverallReadabilityMetrics,
                OverallReadabilityMetricsTableColumnNames.Value,
                ExcelTableUtil.AutoColumnWidth, NumericFormat,
                CellStyleNames.GraphMetricGood,

                new GraphMetricValueOrdered [] {

                    new GraphMetricValueOrdered(
                        bCalculateOverallEdgeCrossings ?
                        NullableToGraphMetricValue<Double>(
                            dOverallEdgeCrossings) : String.Empty),

                    new GraphMetricValueOrdered(
                        bCalculateOverallVertexOverlap ?
                        NullableToGraphMetricValue<Double>(
                            dOverallVertexOverlap) : String.Empty),

                    new GraphMetricValueOrdered(
                        bCalculateGraphRectangleCoverage ?
                        NullableToGraphMetricValue<Double>(
                            dGraphRectangleCoverage) : String.Empty),
                    } ) );

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateOverallEdgeCrossings()
    //
    /// <summary>
    /// Attempts to calculate the overall edge crossings metric.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oEdgeInformation">
    /// The key is an IEdge and the value is the number of edges that cross the
    /// edge.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <param name="dOverallEdgeCrossings">
    /// Where the metric gets stored if true is returned.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// true if the metric was calculated, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateOverallEdgeCrossings
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IEdge, Int32> oEdgeInformation,
        Dictionary<IVertex, VertexInformation> oVertexInformations,
        out Nullable<Double> dOverallEdgeCrossings
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oEdgeInformation != null);
        Debug.Assert(oVertexInformations != null);
        AssertValid();

        dOverallEdgeCrossings = null;

        IEdgeCollection oEdges = oGraphClone.Edges;
        Int32 iEdges = oEdges.Count;

        if (iEdges == 0)
        {
            return (true);
        }

        // Calculate the number of crossings if every pair of edges intersect.

        Double dCAll = ( iEdges * (iEdges - 1) ) / 2.0;

        // Calculate the impossible intersections of edges connected to the
        // same vertex.

        Int32 iDegreeSum = 0;

        foreach (IVertex oVertex in oGraphClone.Vertices)
        {
            Int32 iDegree = oVertex.Degree;
            iDegreeSum += iDegree * (iDegree - 1);
        }

        Double dCImpossible = (Double)iDegreeSum / 2.0;

        // Calculate the upper bound to the number of crossings.

        Double dCMx = dCAll - dCImpossible;

        // Count the number of edge crossings in the graph.

        Int32 iC = 0;

        foreach (Int32 iEdgeCrossings in oEdgeInformation.Values)
        {
            iC += iEdgeCrossings;
        }

        // Each crossing exists twice in the dictionary.  Compensate for this.

        Debug.Assert(iC % 2 == 0);
        iC /= 2;

        dOverallEdgeCrossings = (dCMx > 0) ? 1 - (iC / dCMx) : 1.0;

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateOverallVertexOverlap()
    //
    /// <summary>
    /// Attempts to calculate the overall vertex overlap metric.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <param name="dOverallVertexOverlap">
    /// Where the metric gets stored if true is returned.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// true if the metric was calculated, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateOverallVertexOverlap
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IVertex, VertexInformation> oVertexInformations,
        out Nullable<Double> dOverallVertexOverlap
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oVertexInformations != null);
        AssertValid();

        dOverallVertexOverlap = null;

        // Note: It was determined experimentally that there is not much of a
        // difference between using a GeometryGroup and a CombinedGeometry
        // here.  The GeometryGroup is more convenient.

        GeometryGroup oUnionOfVertexBounds = new GeometryGroup();
        oUnionOfVertexBounds.FillRule = FillRule.Nonzero;
        Double dSumOfVertexAreas;

        if ( !TryCombineVertices(oGraphClone, oBackgroundWorker,
            oVertexInformations, "Calculating overall vertex overlap.",
            oUnionOfVertexBounds, out dSumOfVertexAreas) )
        {
            // The user cancelled.

            return (false);
        }

        if (dSumOfVertexAreas > 0)
        {
            dOverallVertexOverlap =
                GetArea(oUnionOfVertexBounds) / dSumOfVertexAreas;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCalculateGraphRectangleCoverage()
    //
    /// <summary>
    /// Attempts to calculate the ratio of the area used by the graph's
    /// vertices and edges to the area of the graph rectangle.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <param name="dGraphRectangleCoverage">
    /// Where the metric gets stored if true is returned.  Can be null.
    /// </param>
    ///
    /// <returns>
    /// true if the metric was calculated, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCalculateGraphRectangleCoverage
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IVertex, VertexInformation> oVertexInformations,
        out Nullable<Double> dGraphRectangleCoverage
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oVertexInformations != null);
        AssertValid();

        dGraphRectangleCoverage = null;

        // Get the union of the graph's vertices and edges.

        GeometryGroup oUnionOfBounds = new GeometryGroup();
        oUnionOfBounds.FillRule = FillRule.Nonzero;
        const String ProgressMessage = "Calculating graph pane coverage.";
        Double dNotUsed;

        if (
            !TryCombineVertices(oGraphClone, oBackgroundWorker,
                oVertexInformations, ProgressMessage, oUnionOfBounds,
                out dNotUsed)
            ||
            !TryCombineEdges(oGraphClone, oBackgroundWorker, ProgressMessage,
                oUnionOfBounds)
            )
        {
            // The user cancelled.

            return (false);
        }

        Double dGraphRectangleArea =
            m_oGraphRectangleSize.Width * m_oGraphRectangleSize.Height;

        if (dGraphRectangleArea > 0)
        {
            dGraphRectangleCoverage =
                GetArea(oUnionOfBounds) / dGraphRectangleArea;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCombineVertices()
    //
    /// <summary>
    /// Attempts to calculate the union of the vertex bounds and the sum of the
    /// vertex areas.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="oVertexInformations">
    /// The key is an IVertex and the value is a VertexInformation object.
    /// </param>
    ///
    /// <param name="sProgressMessage">
    /// Progress report message.
    /// </param>
    ///
    /// <param name="oUnionOfBounds">
    /// This method adds the bounds of each vertex to this GeometryGroup.  The
    /// object should be discarded if false is returned.
    /// </param>
    ///
    /// <param name="dSumOfVertexAreas">
    /// Where the sum of the areas of the graph's vertices gets stored if true
    /// is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCombineVertices
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        Dictionary<IVertex, VertexInformation> oVertexInformations,
        String sProgressMessage,
        GeometryGroup oUnionOfBounds,
        out Double dSumOfVertexAreas
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(oVertexInformations != null);
        Debug.Assert( !String.IsNullOrEmpty(sProgressMessage) );
        Debug.Assert(oUnionOfBounds != null);
        AssertValid();

        dSumOfVertexAreas = 0;
        GeometryCollection oUnionOfBoundsChildren = oUnionOfBounds.Children;
        Int32 iCalculationsCompleted = 0;

        foreach (IVertex oVertex in oGraphClone.Vertices)
        {
            if ( CancellationPending(oBackgroundWorker, iCalculationsCompleted,
                oGraphClone.Vertices.Count, sProgressMessage) )
            {
                return (false);
            }

            VertexInformation oVertexInformation =
                oVertexInformations[oVertex];

            oUnionOfBoundsChildren.Add(oVertexInformation.Bounds);
            dSumOfVertexAreas += oVertexInformation.Area;
            iCalculationsCompleted++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryCombineEdges()
    //
    /// <summary>
    /// Attempts to calculate the union of the edge bounds.
    /// </summary>
    ///
    /// <param name="oGraphClone">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="sProgressMessage">
    /// Progress report message.
    /// </param>
    ///
    /// <param name="oUnionOfBounds">
    /// This method adds the bounds of each edge to this GeometryGroup.  The
    /// object should be discarded if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCombineEdges
    (
        IGraph oGraphClone,
        BackgroundWorker oBackgroundWorker,
        String sProgressMessage,
        GeometryGroup oUnionOfBounds
    )
    {
        Debug.Assert(oGraphClone != null);
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert( !String.IsNullOrEmpty(sProgressMessage) );
        Debug.Assert(oUnionOfBounds != null);
        AssertValid();

        GeometryCollection oUnionOfBoundsChildren = oUnionOfBounds.Children;
        Int32 iCalculationsCompleted = 0;

        foreach (IEdge oEdge in oGraphClone.Edges)
        {
            if ( CancellationPending(oBackgroundWorker, iCalculationsCompleted,
                oGraphClone.Edges.Count, sProgressMessage) )
            {
                return (false);
            }

            // Note that CloneAndFilterGraph() guaranteed that an
            // EdgeDrawingHistory object is available for the edge, and that
            // the edge isn't a self-loop, which EdgeDrawingHistory can't get
            // the bounds for.

            Debug.Assert(!oEdge.IsSelfLoop);

            EdgeDrawingHistory oEdgeDrawingHistory = (EdgeDrawingHistory)
                oEdge.GetRequiredValue(
                    ReservedMetadataKeys.EdgeDrawingHistory,
                    typeof(EdgeDrawingHistory) );

            oUnionOfBoundsChildren.Add( oEdgeDrawingHistory.GetBounds() );
            iCalculationsCompleted++;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EdgesIntersect()
    //
    /// <summary>
    /// Determines whether two edges intersect.
    /// </summary>
    ///
    /// <param name="oEdge1">
    /// The first edge.
    /// </param>
    ///
    /// <param name="oEdge2">
    /// The second edge.
    /// </param>
    ///
    /// <returns>
    /// true if the edges intersect.
    /// </returns>
    //*************************************************************************

    protected Boolean
    EdgesIntersect
    (
        IEdge oEdge1,
        IEdge oEdge2
    )
    {
        Debug.Assert(oEdge1 != null);
        Debug.Assert(oEdge2 != null);
        AssertValid();

        IVertex [] oEdge1Vertices = oEdge1.Vertices;
        PointF oEdge1Location1 = oEdge1Vertices[0].Location;
        PointF oEdge1Location2 = oEdge1Vertices[1].Location;

        IVertex [] oEdge2Vertices = oEdge2.Vertices;
        PointF oEdge2Location1 = oEdge2Vertices[0].Location;
        PointF oEdge2Location2 = oEdge2Vertices[1].Location;

        return (
            oEdge1 != oEdge2
            &&
            !oEdge1.IsSelfLoop
            &&
            !oEdge2.IsSelfLoop
            &&

            // The next four lines test for edges connected to the same
            // vertex, which technically intersect at that vertex but should
            // not count as an intersection for metric calculations.
            // Sample: (A,B) and (B,C).

            oEdge1Location1 != oEdge2Location1
            &&
            oEdge1Location1 != oEdge2Location2
            &&
            oEdge1Location2 != oEdge2Location1
            &&
            oEdge1Location2 != oEdge2Location2

            &&
            GetEdgeRectangle(oEdge1).IntersectsWith( GetEdgeRectangle(oEdge2) )
            &&
            GraphicsUtil.LineSegmentsIntersect(
                oEdge1Location1, oEdge1Location2,
                oEdge2Location1, oEdge2Location2
                )
            );
    }

    //*************************************************************************
    //  Method: GetEdgeRectangle()
    //
    /// <summary>
    /// Gets a rectangle that bounds an edge's endpoints.
    /// </summary>
    ///
    /// <param name="oEdge">
    /// The edge to get a rectangle for.  Can't be a self-loop.
    /// </param>
    ///
    /// <returns>
    /// The Rect that bounds the edge's endpoints.
    /// </returns>
    //*************************************************************************

    protected Rect
    GetEdgeRectangle
    (
        IEdge oEdge
    )
    {
        Debug.Assert(oEdge != null);
        Debug.Assert(!oEdge.IsSelfLoop);
        AssertValid();

        IVertex [] oVertices = oEdge.Vertices;

        return ( new Rect(
            WpfGraphicsUtil.PointFToWpfPoint(oVertices[0].Location),
            WpfGraphicsUtil.PointFToWpfPoint(oVertices[1].Location)
            ) );
    }

    //*************************************************************************
    //  Method: GetArea()
    //
    /// <summary>
    /// Gets the area of a Geometry.
    /// </summary>
    ///
    /// <param name="oGeometry">
    /// The Geometry to get the area of.
    /// </param>
    ///
    /// <returns>
    /// The area of <paramref name="oGeometry" />.
    /// </returns>
    //*************************************************************************

    protected Double
    GetArea
    (
        Geometry oGeometry
    )
    {
        Debug.Assert(oGeometry != null);
        AssertValid();

        // Using the simeple Geometry.GetArea() overload leads to rounding
        // errors when used on circular vertices.  Use the other overload that
        // allows a tolerance to be specified.

        return ( oGeometry.GetArea(0.000001, ToleranceType.Relative) );
    }

    //*************************************************************************
    //  Method: CancellationPending()
    //
    /// <summary>
    /// Checks for cancellation and reports progress.
    /// </summary>
    ///
    /// <param name="oBackgroundWorker">
    /// BackgroundWorker on which the metrics are being calculated.
    /// </param>
    ///
    /// <param name="iCalculationsCompleted">
    /// Number of calculations that have been performed so far.
    /// </param>
    ///
    /// <param name="iTotalCalculations">
    /// Total number of calculations.  Can be zero.
    /// </param>
    ///
    /// <param name="sProgressMessage">
    /// Progress report message.
    /// </param>
    ///
    /// <returns>
    /// true if the user cancelled.
    /// </returns>
    //*************************************************************************

    protected Boolean
    CancellationPending
    (
        BackgroundWorker oBackgroundWorker,
        Int32 iCalculationsCompleted,
        Int32 iTotalCalculations,
        String sProgressMessage
    )
    {
        Debug.Assert(oBackgroundWorker != null);
        Debug.Assert(iCalculationsCompleted >= 0);
        Debug.Assert(iTotalCalculations >= 0);
        Debug.Assert(iCalculationsCompleted <= iTotalCalculations);
        Debug.Assert( !String.IsNullOrEmpty(sProgressMessage) );
        AssertValid();

        if (iCalculationsCompleted % CalculationsPerProgressReport == 0)
        {
            if (oBackgroundWorker.CancellationPending)
            {
                return (true);
            }

            oBackgroundWorker.ReportProgress(

                GraphMetricCalculatorBase.CalculateProgressInPercent(
                    iCalculationsCompleted, iTotalCalculations),

                sProgressMessage);
        }

        return (false);
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

        Debug.Assert(m_oGraphRectangleSize.Width >= 0);
        Debug.Assert(m_oGraphRectangleSize.Height >= 0);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Number of calculations before progress is reported and the cancellation
    /// flag is checked.

    protected const Int32 CalculationsPerProgressReport = 10;

    /// Number format for all columns.

    protected const String NumericFormat = "0.000";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Size of the rectangle containing the graph, in WPF units.

    protected System.Windows.Size m_oGraphRectangleSize;


    //*************************************************************************
    //  Embedded struct: VertexInformation
    //
    /// <summary>
    /// Stores calculated information about one vertex.
    /// </summary>
    //*************************************************************************

    protected struct
    VertexInformation
    {
        /// <summary>
        /// The vertex's bounds.
        /// </summary>

        public Geometry Bounds;

        /// <summary>
        /// The vertex's area.
        /// </summary>

        public Double Area;
    }
}

}
