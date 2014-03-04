
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WordMetricCalculator2
//
/// <summary>
/// Counts words and word pairs in a specified column.
/// </summary>
///
/// <remarks>
/// This graph metric calculator differs from most other calculators in that it
/// reads an arbitrary column in the Excel workbook.  The other calculators
/// look only at how the graph's vertices are connected to each other.
/// Therefore, there is no corresponding lower-level WordMetricCalculator class
/// in the <see cref="Smrf.NodeXL.Algorithms" /> namespace, and the word
/// metrics cannot be calculated outside of this ExcelTemplate project.
///
/// <para>
/// This class uses "term" to describe either a word or a word pair.  It uses
/// the <see cref="WordCounter" /> class to count words and the <see
/// cref="WordPairCounter" /> class to count word pairs in a specified column.
/// It specifies that the words be stored on one worksheet and the word pairs
/// in another.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class WordMetricCalculator2 : GraphMetricCalculatorBase2
{
    //*************************************************************************
    //  Constructor: WordMetricCalculator2()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WordMetricCalculator2" />
    /// class.
    /// </summary>
    //*************************************************************************

    public WordMetricCalculator2()
    {
        // (Do nothing.)

        AssertValid();
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

            // Duplicate edges do not cause a problem.

            return (true);
        }
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

        WordMetricUserSettings oWordMetricUserSettings =
            calculateGraphMetricsContext.GraphMetricUserSettings
            .WordMetricUserSettings;

        if (
            !calculateGraphMetricsContext.ShouldCalculateGraphMetrics(
                GraphMetrics.Words)
            ||
            String.IsNullOrEmpty(oWordMetricUserSettings.TextColumnName)
            )
        {
            return (true);
        }

        String [] asWordsToSkip = StringUtil.SplitOnCommonDelimiters(
            oWordMetricUserSettings.WordsToSkip);

        WordCounter oWordCounter = new WordCounter(asWordsToSkip);
        WordPairCounter oWordPairCounter = new WordPairCounter(asWordsToSkip);

        // The edges or vertices may have unique imported IDs.  If so, this
        // becomes a collection of the IDs.

        HashSet<String> oUniqueImportedIDs = 
            EdgesOrVerticesHaveImportedIDs(graph,
                oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet) ?
            new HashSet<String>() : null;

        if (oWordMetricUserSettings.CountByGroup)
        {
            if (oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet)
            {
                return ( TryCountEdgeTermsByGroup(graph,
                    oWordMetricUserSettings, oWordCounter, oWordPairCounter,
                    oUniqueImportedIDs, out graphMetricColumns) );
            }
            else
            {
                return ( TryCountVertexTermsByGroup(graph,
                    oWordMetricUserSettings, oWordCounter, oWordPairCounter,
                    oUniqueImportedIDs, out graphMetricColumns) );
            }
        }
        else
        {
            return ( TryCountTermsNoGroups(graph,
                oWordMetricUserSettings, oWordCounter, oWordPairCounter,
                oUniqueImportedIDs, out graphMetricColumns) );
        }
    }

    //*************************************************************************
    //  Method: TryCountEdgeTermsByGroup()
    //
    /// <summary>
    /// Attempts to count edge terms using the graph's groups.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oWordMetricUserSettings">
    /// Stores the user's settings for calculating word metrics.
    /// </param>
    ///
    /// <param name="oWordCounter">
    /// Counts words in one or more strings.
    /// </param>
    ///
    /// <param name="oWordPairCounter">
    /// Counts pairs of words in one or more strings.
    /// </param>
    ///
    /// <param name="oUniqueImportedIDs">
    /// The edges or vertices may have unique imported IDs.  If so, this is a
    /// collection of the IDs, which is empty when this method is called.
    /// Otherwise, it's null.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCountEdgeTermsByGroup
    (
        IGraph oGraph,
        WordMetricUserSettings oWordMetricUserSettings,
        WordCounter oWordCounter,
        WordPairCounter oWordPairCounter,
        HashSet<String> oUniqueImportedIDs,
        out GraphMetricColumn [] oGraphMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oWordMetricUserSettings != null);
        Debug.Assert(oWordCounter != null);
        Debug.Assert(oWordPairCounter != null);
        AssertValid();

        List<GraphMetricValueOrdered> oWordWordValues, oWordCountValues,
            oWordSalienceValues;

        List<GraphMetricValueOrdered> oWordPairWord1Values,
            oWordPairWord2Values, oWordPairCountValues,
            oWordPairSalienceValues, oWordPairMutualInformationValues;

        CreateGraphMetricValueLists(
            out oWordWordValues, out oWordCountValues, out oWordSalienceValues,
            
            out oWordPairWord1Values, out oWordPairWord2Values,
            out oWordPairCountValues, out oWordPairSalienceValues,
            out oWordPairMutualInformationValues
            );

        List<GraphMetricValueOrdered> oWordGroupNameValues =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oWordPairGroupNameValues =
            new List<GraphMetricValueOrdered>();

        // Get the edges in each of the graph's groups.  Include a "dummy"
        // group that contains the edges that aren't contained in any real
        // groups.

        foreach ( GroupEdgeInfo oGroupEdgeInfo in
            GroupEdgeSorter.SortGroupEdges(oGraph, Int32.MaxValue,
                true, true) )
        {
            // Count the terms in this group.

            oWordCounter.Clear();
            oWordPairCounter.Clear();

            foreach ( IEdge oEdge in EnumerateEdgesOrVertices(
                oGroupEdgeInfo.Edges, true, oGraph, oUniqueImportedIDs) )
            {
                CountTermsInEdgeOrVertex(oEdge,
                    oWordMetricUserSettings.TextColumnName, oWordCounter,
                    oWordPairCounter);
            }

            oWordCounter.CalculateSalienceOfCountedTerms();
            oWordPairCounter.CalculateSalienceOfCountedTerms();
            oWordPairCounter.CalculateMutualInformationOfCountedTerms();

            // Transfer the words and word pairs to the graph metric value
            // lists.

            String sGroupName = oGroupEdgeInfo.GroupName;

            AddCountedWordsToValueLists( oWordCounter.CountedTerms,
                oWordMetricUserSettings, sGroupName, oWordWordValues,
                oWordCountValues, oWordSalienceValues, oWordGroupNameValues);

            AddCountedWordPairsToValueLists( oWordPairCounter.CountedTerms,
                oWordMetricUserSettings, sGroupName, oWordPairWord1Values,
                oWordPairWord2Values, oWordPairCountValues,
                oWordPairSalienceValues, oWordPairMutualInformationValues,
                oWordPairGroupNameValues);

            if (
                sGroupName == GroupEdgeSorter.DummyGroupNameForEntireGraph
                &&
                oUniqueImportedIDs != null
                )
            {
                // This is the dummy group that stores all the edges in the
                // graph.  Note that SortGroupEdges() guarantees that this is
                // the first group, so the imported IDs need to be cleared only
                // once within this loop.

                oUniqueImportedIDs.Clear();
            }
        }

        oGraphMetricColumns = CreateGraphMetricColumns(
            oWordWordValues, oWordCountValues, oWordSalienceValues,
            oWordGroupNameValues,
        
            oWordPairWord1Values, oWordPairWord2Values, oWordPairCountValues,
            oWordPairSalienceValues, oWordPairMutualInformationValues,
            oWordPairGroupNameValues
            );

        return (true);
    }

    //*************************************************************************
    //  Method: TryCountVertexTermsByGroup()
    //
    /// <summary>
    /// Attempts to count vertex terms using the graph's groups.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oWordMetricUserSettings">
    /// Stores the user's settings for calculating word metrics.
    /// </param>
    ///
    /// <param name="oWordCounter">
    /// Counts words in one or more strings.
    /// </param>
    ///
    /// <param name="oWordPairCounter">
    /// Counts pairs of words in one or more strings.
    /// </param>
    ///
    /// <param name="oUniqueImportedIDs">
    /// The edges or vertices may have unique imported IDs.  If so, this is a
    /// collection of the IDs, which is empty when this method is called.
    /// Otherwise, it's null.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCountVertexTermsByGroup
    (
        IGraph oGraph,
        WordMetricUserSettings oWordMetricUserSettings,
        WordCounter oWordCounter,
        WordPairCounter oWordPairCounter,
        HashSet<String> oUniqueImportedIDs,
        out GraphMetricColumn [] oGraphMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oWordMetricUserSettings != null);
        Debug.Assert(oWordCounter != null);
        Debug.Assert(oWordPairCounter != null);
        AssertValid();

        List<GraphMetricValueOrdered> oWordWordValues, oWordCountValues,
            oWordSalienceValues;

        List<GraphMetricValueOrdered> oWordPairWord1Values,
            oWordPairWord2Values, oWordPairCountValues,
            oWordPairSalienceValues, oWordPairMutualInformationValues;

        CreateGraphMetricValueLists(
            out oWordWordValues, out oWordCountValues, out oWordSalienceValues,
            
            out oWordPairWord1Values, out oWordPairWord2Values,
            out oWordPairCountValues, out oWordPairSalienceValues,
            out oWordPairMutualInformationValues
            );

        List<GraphMetricValueOrdered> oWordGroupNameValues =
            new List<GraphMetricValueOrdered>();

        List<GraphMetricValueOrdered> oWordPairGroupNameValues =
            new List<GraphMetricValueOrdered>();

        // Get a list of the graph's groups, adding a dummy group for the
        // entire graph and another to contain any non-grouped vertices.

        foreach ( GroupInfo oGroup in
            EnumerateGroupsForCountingVertexTerms(oGraph) )
        {
            // Count the terms in this group.

            oWordCounter.Clear();
            oWordPairCounter.Clear();

            foreach ( IVertex oVertex in EnumerateEdgesOrVertices(
                oGroup.Vertices, false, oGraph, oUniqueImportedIDs) )
            {
                CountTermsInEdgeOrVertex(oVertex,
                    oWordMetricUserSettings.TextColumnName, oWordCounter,
                    oWordPairCounter);
            }

            oWordCounter.CalculateSalienceOfCountedTerms();
            oWordPairCounter.CalculateSalienceOfCountedTerms();
            oWordPairCounter.CalculateMutualInformationOfCountedTerms();

            // Transfer the words and word pairs to the graph metric value
            // lists.

            AddCountedWordsToValueLists(oWordCounter.CountedTerms,
                oWordMetricUserSettings, oGroup.Name, oWordWordValues,
                oWordCountValues, oWordSalienceValues, oWordGroupNameValues);

            AddCountedWordPairsToValueLists(oWordPairCounter.CountedTerms,
                oWordMetricUserSettings, oGroup.Name, oWordPairWord1Values,
                oWordPairWord2Values, oWordPairCountValues,
                oWordPairSalienceValues, oWordPairMutualInformationValues,
                oWordPairGroupNameValues);
        }

        oGraphMetricColumns = CreateGraphMetricColumns(
            oWordWordValues, oWordCountValues, oWordSalienceValues,
            oWordGroupNameValues,
        
            oWordPairWord1Values, oWordPairWord2Values, oWordPairCountValues,
            oWordPairSalienceValues, oWordPairMutualInformationValues,
            oWordPairGroupNameValues
            );

        return (true);
    }

    //*************************************************************************
    //  Method: TryCountTermsNoGroups()
    //
    /// <summary>
    /// Attempts to count terms without paying attention to the graph's groups.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="oWordMetricUserSettings">
    /// Stores the user's settings for calculating word metrics.
    /// </param>
    ///
    /// <param name="oWordCounter">
    /// Counts words in one or more strings.
    /// </param>
    ///
    /// <param name="oWordPairCounter">
    /// Counts pairs of words in one or more strings.
    /// </param>
    ///
    /// <param name="oUniqueImportedIDs">
    /// The edges or vertices may have unique imported IDs.  If so, this is a
    /// collection of the IDs, which is empty when this method is called.
    /// Otherwise, it's null.
    /// </param>
    ///
    /// <param name="oGraphMetricColumns">
    /// Where an array of GraphMetricColumn objects gets stored if true is
    /// returned, one for each related metric calculated by this method.
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were calculated, false if the user wants to
    /// cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCountTermsNoGroups
    (
        IGraph oGraph,
        WordMetricUserSettings oWordMetricUserSettings,
        WordCounter oWordCounter,
        WordPairCounter oWordPairCounter,
        HashSet<String> oUniqueImportedIDs,
        out GraphMetricColumn [] oGraphMetricColumns
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert(oWordMetricUserSettings != null);
        Debug.Assert(oWordCounter != null);
        Debug.Assert(oWordPairCounter != null);
        AssertValid();

        Boolean bTextColumnIsOnEdgeWorksheet =
            oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet;

        System.Collections.IEnumerable oEdgesOrVertices =
            bTextColumnIsOnEdgeWorksheet ?
            (System.Collections.IEnumerable)oGraph.Edges :
            (System.Collections.IEnumerable)oGraph.Vertices;

        // Count the terms in each of the column's cells.

        foreach ( IMetadataProvider oEdgeOrVertex in EnumerateEdgesOrVertices(
            oEdgesOrVertices, bTextColumnIsOnEdgeWorksheet, oGraph,
            oUniqueImportedIDs) )
        {
            CountTermsInEdgeOrVertex(oEdgeOrVertex,
                oWordMetricUserSettings.TextColumnName, oWordCounter,
                oWordPairCounter);
        }

        oWordCounter.CalculateSalienceOfCountedTerms();
        oWordPairCounter.CalculateSalienceOfCountedTerms();
        oWordPairCounter.CalculateMutualInformationOfCountedTerms();

        // Transfer the words and word pairs to graph metric value lists.

        List<GraphMetricValueOrdered> oWordWordValues, oWordCountValues,
            oWordSalienceValues;

        List<GraphMetricValueOrdered> oWordPairWord1Values,
            oWordPairWord2Values, oWordPairCountValues,
            oWordPairSalienceValues, oWordPairMutualInformationValues;

        CreateGraphMetricValueLists(
            out oWordWordValues, out oWordCountValues, out oWordSalienceValues,
            
            out oWordPairWord1Values, out oWordPairWord2Values,
            out oWordPairCountValues, out oWordPairSalienceValues,
            out oWordPairMutualInformationValues
            );

        foreach (CountedWord oCountedWord in oWordCounter.CountedTerms)
        {
            AddCountedWordToValueLists(oCountedWord, oWordMetricUserSettings,
                oWordWordValues, oWordCountValues, oWordSalienceValues);
        }

        foreach (CountedWordPair oCountedWordPair in
            oWordPairCounter.CountedTerms)
        {
            AddCountedWordPairToValueLists(oCountedWordPair,
                oWordMetricUserSettings, oWordPairWord1Values,
                oWordPairWord2Values, oWordPairCountValues,
                oWordPairSalienceValues, oWordPairMutualInformationValues);
        }

        oGraphMetricColumns = CreateGraphMetricColumns(
            oWordWordValues, oWordCountValues, oWordSalienceValues, null,

            oWordPairWord1Values, oWordPairWord2Values, oWordPairCountValues,
            oWordPairSalienceValues, oWordPairMutualInformationValues, null
            );

        return (true);
    }

    //*************************************************************************
    //  Method: EdgesOrVerticesHaveImportedIDs()
    //
    /// <summary>
    /// Determines whether a graph's edges or vertices contain imported IDs.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="bCheckEdges">
    /// true to check the graph's edges, false to check its vertices.
    /// </param>
    ///
    /// <returns>
    /// true if the graph's edges or vertices contain imported IDs.
    /// </returns>
    //*************************************************************************

    protected Boolean
    EdgesOrVerticesHaveImportedIDs
    (
        IGraph oGraph,
        Boolean bCheckEdges
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        // When the workbook was read, arrays of all the edge and vertex
        // metadata key names were stored on the graph.  Get the relevant
        // array.

        Object oAllMetadataKeysAsObject;

        if ( oGraph.TryGetValue(

            bCheckEdges ? ReservedMetadataKeys.AllEdgeMetadataKeys :
                ReservedMetadataKeys.AllVertexMetadataKeys,

            typeof ( String[] ), out oAllMetadataKeysAsObject) )
        {
            if ( ( (String[] )oAllMetadataKeysAsObject ).Contains(
                CommonTableColumnNames.ImportedID) )
            {
                return (true);
            }
        }

        return (false);
    }

    //*************************************************************************
    //  Method: EnumerateGroupsForCountingVertexTerms()
    //
    /// <summary>
    /// Enumerates groups when counting vertex terms using the graph's groups.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <returns>
    /// The groups to use when counting terms using the graph's groups,
    /// returned one-by-one as a GroupInfo object.
    /// </returns>
    //*************************************************************************

    protected IEnumerable<GroupInfo>
    EnumerateGroupsForCountingVertexTerms
    (
        IGraph oGraph
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        // Start with a dummy group that contains all the graph's vertices.
        // For consistency, use the same group name used by the GroupEdgeSorter
        // for such a group.

        GroupInfo oDummyGroupForEntireGraph = new GroupInfo();

        oDummyGroupForEntireGraph.Name =
            GroupEdgeSorter.DummyGroupNameForEntireGraph;

        foreach (IVertex oVertex in oGraph.Vertices)
        {
            oDummyGroupForEntireGraph.Vertices.AddLast(oVertex);
        }

        yield return oDummyGroupForEntireGraph;

        oDummyGroupForEntireGraph = null;

        // Get a list of the graph's groups, adding a dummy group if necessary
        // to contain any non-grouped vertices.

        foreach ( GroupInfo oGroup in GroupUtil.GetGroupsWithAllVertices(
            oGraph, false) )
        {
            if (oGroup.Name == null)
            {
                // This is the dummy group that contains non-grouped vertices.

                oGroup.Name = GroupEdgeSorter.DummyGroupNameForUngroupedEdges;
            }

            yield return oGroup;
        }
    }

    //*************************************************************************
    //  Method: EnumerateEdgesOrVertices()
    //
    /// <summary>
    /// Enumerates an edge or vertex collection.
    /// </summary>
    ///
    /// <param name="oEdgesOrVerticesToEnumerate">
    /// The collection to enumerate.
    /// </param>
    ///
    /// <param name="bAreEdges">
    /// true if the collection contains edges, false if it contains vertices.
    /// </param>
    ///
    /// <param name="oGraph">
    /// The graph the collection came from.
    /// </param>
    ///
    /// <param name="oUniqueImportedIDs">
    /// The edges or vertices may have unique imported IDs.  If so, this is a
    /// collection of the IDs, which is empty when this method is called.
    /// Otherwise, it's null.
    /// </param>
    ///
    /// <returns>
    /// The edges or vertices in <paramref
    /// name="oEdgesOrVerticesToEnumerate" />, returned one-by-one as an
    /// IMetadataProvider.
    /// </returns>
    ///
    /// <remarks>
    /// If the edges or vertices have imported IDs, then this method filters
    /// out any duplicate edges or vertices.
    /// </remarks>
    //*************************************************************************

    protected IEnumerable<IMetadataProvider>
    EnumerateEdgesOrVertices
    (
        System.Collections.IEnumerable oEdgesOrVerticesToEnumerate,
        Boolean bAreEdges,
        IGraph oGraph,
        HashSet<String> oUniqueImportedIDs
    )
    {
        Debug.Assert(oEdgesOrVerticesToEnumerate != null);
        Debug.Assert(oGraph != null);
        AssertValid();

        // (Note that oEdgesOrVerticesToEnumerate is not declared as a
        // type-safe IEnumerable<IMetadataProvider> because .NET 3.5 does not
        // support covariance.)

        foreach (IMetadataProvider oEdgeOrVertex in
            oEdgesOrVerticesToEnumerate)
        {
            if ( ShouldEnumerateEdgeOrVertex(
                oEdgeOrVertex, oUniqueImportedIDs) )
            {
                yield return (oEdgeOrVertex);
            }
        }
    }

    //*************************************************************************
    //  Method: ShouldEnumerateEdgeOrVertex()
    //
    /// <summary>
    /// Determine whether an edge or vertex should be enumerated.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex to check.
    /// </param>
    ///
    /// <param name="oUniqueImportedIDs">
    /// Unique imported IDs that have been found on the graph's edges or
    /// vertices, or null if the edges or vertices don't have imported IDs.
    /// </param>
    ///
    /// <returns>
    /// true if the edge or vertex should be enumerated, false if not.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ShouldEnumerateEdgeOrVertex
    (
        IMetadataProvider oEdgeOrVertex,
        HashSet<String> oUniqueImportedIDs
    )
    {
        Debug.Assert(oEdgeOrVertex != null);
        AssertValid();

        Object oImportedIDAsObject;

        if (
            oUniqueImportedIDs == null
            ||
            !oEdgeOrVertex.TryGetValue(CommonTableColumnNames.ImportedID,
                typeof(String), out oImportedIDAsObject)
            ||
            oImportedIDAsObject == null
            )
        {
            return (true);
        }

        // The edge or vertex has an imported ID.  Check whether it is unique.

        return ( oUniqueImportedIDs.Add( (String)oImportedIDAsObject) );
    }

    //*************************************************************************
    //  Method: CountTermsInEdgeOrVertex()
    //
    /// <summary>
    /// Counts the terms in an edge's or vertex's text cell.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex.
    /// </param>
    ///
    /// <param name="sTextColumnName">
    /// The name of the column containing the text.
    /// </param>
    ///
    /// <param name="oWordCounter">
    /// Counts words in one or more strings.
    /// </param>
    ///
    /// <param name="oWordPairCounter">
    /// Counts pairs of words in one or more strings.
    /// </param>
    //*************************************************************************

    protected void
    CountTermsInEdgeOrVertex
    (
        IMetadataProvider oEdgeOrVertex,
        String sTextColumnName,
        WordCounter oWordCounter,
        WordPairCounter oWordPairCounter
    )
    {
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert( !String.IsNullOrEmpty(sTextColumnName) );
        Debug.Assert(oWordCounter != null);
        Debug.Assert(oWordPairCounter != null);
        AssertValid();

        Object oTextAsObject;

        if ( oEdgeOrVertex.TryGetValue(sTextColumnName, typeof(String),
            out oTextAsObject ) )
        {
            String sText = (String)oTextAsObject;

            if ( !String.IsNullOrEmpty(sText) )
            {
                oWordCounter.CountTermsInDocument(sText);
                oWordPairCounter.CountTermsInDocument(sText);
            }
        }
    }

    //*************************************************************************
    //  Method: CreateGraphMetricValueLists()
    //
    /// <summary>
    /// Creates lists of graph metric values.
    /// </summary>
    ///
    /// <param name="oWordWordValues">
    /// Where the list for word values gets stored.
    /// </param>
    ///
    /// <param name="oWordCountValues">
    /// Where the list for word count values gets stored.
    /// </param>
    ///
    /// <param name="oWordSalienceValues">
    /// Where the list for word salience values gets stored.
    /// </param>
    ///
    /// <param name="oWordPairWord1Values">
    /// Where the list for word pair word 1 values gets stored.
    /// </param>
    ///
    /// <param name="oWordPairWord2Values">
    /// Where the list for word pair word 2 values gets stored.
    /// </param>
    ///
    /// <param name="oWordPairCountValues">
    /// Where the list for word pair count values gets stored.
    /// </param>
    ///
    /// <param name="oWordPairSalienceValues">
    /// Where the list for word pair salience values gets stored.
    /// </param>
    ///
    /// <param name="oWordPairMutualInformationValues">
    /// Where the list for word pair mutual information values gets stored.
    /// </param>
    //*************************************************************************

    protected void
    CreateGraphMetricValueLists
    (
        out List<GraphMetricValueOrdered> oWordWordValues,
        out List<GraphMetricValueOrdered> oWordCountValues,
        out List<GraphMetricValueOrdered> oWordSalienceValues,
        out List<GraphMetricValueOrdered> oWordPairWord1Values,
        out List<GraphMetricValueOrdered> oWordPairWord2Values,
        out List<GraphMetricValueOrdered> oWordPairCountValues,
        out List<GraphMetricValueOrdered> oWordPairSalienceValues,
        out List<GraphMetricValueOrdered> oWordPairMutualInformationValues
    )
    {
        AssertValid();

        oWordWordValues = new List<GraphMetricValueOrdered>();
        oWordCountValues = new List<GraphMetricValueOrdered>();
        oWordSalienceValues = new List<GraphMetricValueOrdered>();

        oWordPairWord1Values = new List<GraphMetricValueOrdered>();
        oWordPairWord2Values = new List<GraphMetricValueOrdered>();
        oWordPairCountValues = new List<GraphMetricValueOrdered>();
        oWordPairSalienceValues = new List<GraphMetricValueOrdered>();
        oWordPairMutualInformationValues = new List<GraphMetricValueOrdered>();
    }

    //*************************************************************************
    //  Method: AddCountedWordsToValueLists()
    //
    /// <summary>
    /// Adds all counted words to the lists of graph metric values.
    /// </summary>
    ///
    /// <param name="oCountedWords">
    /// The words to add.
    /// </param>
    ///
    /// <param name="oWordMetricUserSettings">
    /// Stores the user's settings for calculating word metrics.
    /// </param>
    ///
    /// <param name="sGroupName">
    /// The name of the group the words belong to.
    /// </param>
    ///
    /// <param name="oWordWordValues">
    /// The list for word values.
    /// </param>
    ///
    /// <param name="oWordCountValues">
    /// The list for word count values.
    /// </param>
    ///
    /// <param name="oWordSalienceValues">
    /// The list for word salience values.
    /// </param>
    ///
    /// <param name="oWordGroupNameValues">
    /// The list for word group name values.
    /// </param>
    //*************************************************************************

    protected void
    AddCountedWordsToValueLists
    (
        IEnumerable<CountedWord> oCountedWords,
        WordMetricUserSettings oWordMetricUserSettings,
        String sGroupName,
        List<GraphMetricValueOrdered> oWordWordValues,
        List<GraphMetricValueOrdered> oWordCountValues,
        List<GraphMetricValueOrdered> oWordSalienceValues,
        List<GraphMetricValueOrdered> oWordGroupNameValues
    )
    {
        Debug.Assert(oCountedWords != null);
        Debug.Assert(oWordMetricUserSettings != null);
        Debug.Assert( !String.IsNullOrEmpty(sGroupName) );
        Debug.Assert(oWordWordValues != null);
        Debug.Assert(oWordCountValues != null);
        Debug.Assert(oWordSalienceValues != null);
        Debug.Assert(oWordGroupNameValues != null);

        foreach (CountedWord oCountedWord in oCountedWords)
        {
            if ( AddCountedWordToValueLists(oCountedWord,
                oWordMetricUserSettings, oWordWordValues, oWordCountValues,
                oWordSalienceValues) )
            {
                oWordGroupNameValues.Add(
                    new GraphMetricValueOrdered(sGroupName) );
            }
        }
    }

    //*************************************************************************
    //  Method: AddCountedWordPairsToValueLists()
    //
    /// <summary>
    /// Adds all counted word pairs to the lists of graph metric values.
    /// </summary>
    ///
    /// <param name="oCountedWordPairs">
    /// The word pairs to add.
    /// </param>
    ///
    /// <param name="oWordMetricUserSettings">
    /// Stores the user's settings for calculating word metrics.
    /// </param>
    ///
    /// <param name="sGroupName">
    /// The name of the group the word pairs belong to.
    /// </param>
    ///
    /// <param name="oWordPairWord1Values">
    /// The list for word pair word 1 values.
    /// </param>
    ///
    /// <param name="oWordPairWord2Values">
    /// The list for word pair word 2 values.
    /// </param>
    ///
    /// <param name="oWordPairCountValues">
    /// The list for word pair count values.
    /// </param>
    ///
    /// <param name="oWordPairSalienceValues">
    /// The list for word pair salience values.
    /// </param>
    ///
    /// <param name="oWordPairMutualInformationValues">
    /// The list for word pair mutual information values.
    /// </param>
    ///
    /// <param name="oWordPairGroupNameValues">
    /// The list for word pair group name values.
    /// </param>
    //*************************************************************************

    protected void
    AddCountedWordPairsToValueLists
    (
        IEnumerable<CountedWordPair> oCountedWordPairs,
        WordMetricUserSettings oWordMetricUserSettings,
        String sGroupName,
        List<GraphMetricValueOrdered> oWordPairWord1Values,
        List<GraphMetricValueOrdered> oWordPairWord2Values,
        List<GraphMetricValueOrdered> oWordPairCountValues,
        List<GraphMetricValueOrdered> oWordPairSalienceValues,
        List<GraphMetricValueOrdered> oWordPairMutualInformationValues,
        List<GraphMetricValueOrdered> oWordPairGroupNameValues
    )
    {
        Debug.Assert(oCountedWordPairs != null);
        Debug.Assert(oWordMetricUserSettings != null);
        Debug.Assert( !String.IsNullOrEmpty(sGroupName) );
        Debug.Assert(oWordPairWord1Values != null);
        Debug.Assert(oWordPairWord2Values != null);
        Debug.Assert(oWordPairCountValues != null);
        Debug.Assert(oWordPairSalienceValues != null);
        Debug.Assert(oWordPairMutualInformationValues != null);
        Debug.Assert(oWordPairGroupNameValues != null);

        foreach (CountedWordPair oCountedWordPair in oCountedWordPairs)
        {
            if ( AddCountedWordPairToValueLists(oCountedWordPair,
                oWordMetricUserSettings, oWordPairWord1Values,
                oWordPairWord2Values, oWordPairCountValues,
                oWordPairSalienceValues, oWordPairMutualInformationValues) )
            {
                oWordPairGroupNameValues.Add(
                    new GraphMetricValueOrdered(sGroupName) );
            }
        }
    }

    //*************************************************************************
    //  Method: AddCountedWordToValueLists()
    //
    /// <summary>
    /// Adds a counted word to the lists of graph metric values.
    /// </summary>
    ///
    /// <param name="oCountedWord">
    /// The word to add.
    /// </param>
    ///
    /// <param name="oWordMetricUserSettings">
    /// Stores the user's settings for calculating word metrics.
    /// </param>
    ///
    /// <param name="oWordWordValues">
    /// The list for word values.
    /// </param>
    ///
    /// <param name="oWordCountValues">
    /// The list for word pair count values.
    /// </param>
    ///
    /// <param name="oWordSalienceValues">
    /// The list for word salience values.
    /// </param>
    ///
    /// <returns>
    /// true if the word was added, false if it was a single word and was
    /// skipped.
    /// </returns>
    //*************************************************************************

    protected Boolean
    AddCountedWordToValueLists
    (
        CountedWord oCountedWord,
        WordMetricUserSettings oWordMetricUserSettings,
        List<GraphMetricValueOrdered> oWordWordValues,
        List<GraphMetricValueOrdered> oWordCountValues,
        List<GraphMetricValueOrdered> oWordSalienceValues
    )
    {
        Debug.Assert(oCountedWord != null);
        Debug.Assert(oWordMetricUserSettings != null);
        Debug.Assert(oWordWordValues != null);
        Debug.Assert(oWordCountValues != null);
        Debug.Assert(oWordSalienceValues != null);
        AssertValid();

        if (
            oCountedWord.Count == 1
            &&
            oWordMetricUserSettings.SkipSingleTerms
            )
        {
            return (false);
        }

        oWordWordValues.Add( new GraphMetricValueOrdered(
            ExcelUtil.ForceCellText(oCountedWord.Word) ) );

        oWordCountValues.Add(
            new GraphMetricValueOrdered(oCountedWord.Count) );

        oWordSalienceValues.Add(
            new GraphMetricValueOrdered(oCountedWord.Salience) );

        return (true);
    }

    //*************************************************************************
    //  Method: AddCountedWordPairToValueLists()
    //
    /// <summary>
    /// Adds a counted word pair to the lists of graph metric values.
    /// </summary>
    ///
    /// <param name="oCountedWordPair">
    /// The word pair to add.
    /// </param>
    ///
    /// <param name="oWordMetricUserSettings">
    /// Stores the user's settings for calculating word metrics.
    /// </param>
    ///
    /// <param name="oWordPairWord1Values">
    /// The list for word pair word 1 values.
    /// </param>
    ///
    /// <param name="oWordPairWord2Values">
    /// The list for word pair word 2 values.
    /// </param>
    ///
    /// <param name="oWordPairCountValues">
    /// The list for word pair count values.
    /// </param>
    ///
    /// <param name="oWordPairSalienceValues">
    /// The list for word pair salience values.
    /// </param>
    ///
    /// <param name="oWordPairMutualInformationValues">
    /// The list for word pair mutual information values.
    /// </param>
    ///
    /// <returns>
    /// true if the word pair was added, false if it was a single pair and was
    /// skipped.
    /// </returns>
    //*************************************************************************

    protected Boolean
    AddCountedWordPairToValueLists
    (
        CountedWordPair oCountedWordPair,
        WordMetricUserSettings oWordMetricUserSettings,
        List<GraphMetricValueOrdered> oWordPairWord1Values,
        List<GraphMetricValueOrdered> oWordPairWord2Values,
        List<GraphMetricValueOrdered> oWordPairCountValues,
        List<GraphMetricValueOrdered> oWordPairSalienceValues,
        List<GraphMetricValueOrdered> oWordPairMutualInformationValues
    )
    {
        Debug.Assert(oCountedWordPair != null);
        Debug.Assert(oWordMetricUserSettings != null);
        Debug.Assert(oWordPairWord1Values != null);
        Debug.Assert(oWordPairWord2Values != null);
        Debug.Assert(oWordPairCountValues != null);
        Debug.Assert(oWordPairSalienceValues != null);
        Debug.Assert(oWordPairMutualInformationValues != null);
        AssertValid();

        if (
            oCountedWordPair.Count == 1
            &&
            oWordMetricUserSettings.SkipSingleTerms
            )
        {
            return (false);
        }

        oWordPairWord1Values.Add( new GraphMetricValueOrdered(
            ExcelUtil.ForceCellText(oCountedWordPair.Word1) ) );

        oWordPairWord2Values.Add( new GraphMetricValueOrdered(
            ExcelUtil.ForceCellText(oCountedWordPair.Word2) ) );

        oWordPairCountValues.Add(
            new GraphMetricValueOrdered(oCountedWordPair.Count) );

        oWordPairSalienceValues.Add(
            new GraphMetricValueOrdered(oCountedWordPair.Salience) );

        oWordPairMutualInformationValues.Add(
            new GraphMetricValueOrdered(oCountedWordPair.MutualInformation) );

        return (true);
    }

    //*************************************************************************
    //  Method: CreateGraphMetricColumns()
    //
    /// <summary>
    /// Creates all <see cref="GraphMetricColumnOrdered" /> objects for the
    /// word count table.
    /// </summary>
    ///
    /// <param name="oWordWordValues">
    /// The list for word values.
    /// </param>
    ///
    /// <param name="oWordCountValues">
    /// The list for word count values.
    /// </param>
    ///
    /// <param name="oWordSalienceValues">
    /// The list for word salience values.
    /// </param>
    ///
    /// <param name="oWordGroupNameValues">
    /// The list for word group name values, or null if not counting words by
    /// group.
    /// </param>
    ///
    /// <param name="oWordPairWord1Values">
    /// The list for word pair word 1 values.
    /// </param>
    ///
    /// <param name="oWordPairWord2Values">
    /// The list for word pair word 2 values.
    /// </param>
    ///
    /// <param name="oWordPairCountValues">
    /// The list for word pair count values.
    /// </param>
    ///
    /// <param name="oWordPairSalienceValues">
    /// The list for word pair salience values.
    /// </param>
    ///
    /// <param name="oWordPairMutualInformationValues">
    /// The list for word pair mutual information values.
    /// </param>
    ///
    /// <param name="oWordPairGroupNameValues">
    /// The list for word pair group name values, or null if not counting word
    /// pairs by group.
    /// </param>
    ///
    /// <returns>
    /// An array of GraphMetricColumn objects.
    /// </returns>
    //*************************************************************************

    protected GraphMetricColumn []
    CreateGraphMetricColumns
    (
        List<GraphMetricValueOrdered> oWordWordValues,
        List<GraphMetricValueOrdered> oWordCountValues,
        List<GraphMetricValueOrdered> oWordSalienceValues,
        List<GraphMetricValueOrdered> oWordGroupNameValues,

        List<GraphMetricValueOrdered> oWordPairWord1Values,
        List<GraphMetricValueOrdered> oWordPairWord2Values,
        List<GraphMetricValueOrdered> oWordPairCountValues,
        List<GraphMetricValueOrdered> oWordPairSalienceValues,
        List<GraphMetricValueOrdered> oWordPairMutualInformationValues,
        List<GraphMetricValueOrdered> oWordPairGroupNameValues
    )
    {
        Debug.Assert(oWordWordValues != null);
        Debug.Assert(oWordCountValues != null);
        Debug.Assert(oWordSalienceValues != null);

        Debug.Assert(oWordPairWord1Values != null);
        Debug.Assert(oWordPairWord2Values != null);
        Debug.Assert(oWordPairCountValues != null);
        Debug.Assert(oWordPairSalienceValues != null);
        Debug.Assert(oWordPairMutualInformationValues != null);
        AssertValid();

        List<GraphMetricColumn> oGraphMetricColumns =
            new List<GraphMetricColumn>();

        oGraphMetricColumns.Add( CreateGraphMetricColumn( true,
            WordTableColumnNames.Word, false, oWordWordValues) );

        oGraphMetricColumns.Add( CreateGraphMetricColumn( true,
            WordTableColumnNames.Count, false, oWordCountValues) );

        oGraphMetricColumns.Add( CreateGraphMetricColumn( true,
            WordTableColumnNames.Salience, true, oWordSalienceValues) );

        if (oWordGroupNameValues != null)
        {
            oGraphMetricColumns.Add( CreateGraphMetricColumn( true,
                WordTableColumnNames.Group, false, oWordGroupNameValues) );
        }

        oGraphMetricColumns.Add( CreateGraphMetricColumn( false,
            WordPairTableColumnNames.Word1, false, oWordPairWord1Values) );

        oGraphMetricColumns.Add( CreateGraphMetricColumn( false,
            WordPairTableColumnNames.Word2, false, oWordPairWord2Values) );

        oGraphMetricColumns.Add( CreateGraphMetricColumn( false,
            WordPairTableColumnNames.Count, false, oWordPairCountValues) );

        oGraphMetricColumns.Add( CreateGraphMetricColumn( false,
            WordPairTableColumnNames.Salience, true,
            oWordPairSalienceValues) );

        oGraphMetricColumns.Add( CreateGraphMetricColumn( false,
            WordPairTableColumnNames.MutualInformation, true,
            oWordPairMutualInformationValues) );

        if (oWordPairGroupNameValues != null)
        {
            oGraphMetricColumns.Add( CreateGraphMetricColumn( false,
                WordPairTableColumnNames.Group, false,
                oWordPairGroupNameValues) );
        }

        return ( oGraphMetricColumns.ToArray() );
    }

    //*************************************************************************
    //  Method: CreateGraphMetricColumn()
    //
    /// <summary>
    /// Creates a <see cref="GraphMetricColumnOrdered" /> object for the word
    /// or word pair table.
    /// </summary>
    ///
    /// <param name="bColumnIsInWordTable">
    /// true if the column is in the word table, false if it is in the word
    /// pair table.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// The column's name.
    /// </param>
    ///
    /// <param name="bValuesAreDouble">
    /// true if the column's values are Doubles, false if they are something
    /// else.
    /// </param>
    ///
    /// <param name="oValues">
    /// The column's values.
    /// </param>
    ///
    /// <returns>
    /// A new <see cref="GraphMetricColumnOrdered" /> object.
    /// </returns>
    //*************************************************************************

    protected GraphMetricColumnOrdered
    CreateGraphMetricColumn
    (
        Boolean bColumnIsInWordTable,
        String sColumnName,
        Boolean bValuesAreDouble,
        List<GraphMetricValueOrdered> oValues
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert(oValues != null);
        AssertValid();

        return ( new GraphMetricColumnOrdered(

            bColumnIsInWordTable ?
                WorksheetNames.WordCounts : WorksheetNames.WordPairCounts,

            bColumnIsInWordTable ?
                TableNames.WordCounts : TableNames.WordPairCounts,

            sColumnName, ExcelTableUtil.AutoColumnWidth,
            bValuesAreDouble ? "0.000" : null,
            null, oValues.ToArray()
            ) );
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
