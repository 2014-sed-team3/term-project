
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: GraphMetrics
//
/// <summary>
/// Specifies one or more graph metrics.
/// </summary>
///
/// <remarks>
/// The flags can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
GraphMetrics
{
    /// <summary>
    /// No graph metrics.
    /// </summary>

    None = 0,

    /// <summary>
    /// In-degree.
    /// </summary>

    InDegree = 1,

    /// <summary>
    /// Out-degree.
    /// </summary>

    OutDegree = 2,

    /// <summary>
    /// Degree.
    /// </summary>

    Degree = 4,

    /// <summary>
    /// Clustering coefficient.
    /// </summary>

    ClusteringCoefficient = 8,

    /// <summary>
    /// Brandes fast centralities.
    /// </summary>

    BrandesFastCentralities = 16,

    /// <summary>
    /// Eigenvector centrality.
    /// </summary>

    EigenvectorCentrality = 32,

    /// <summary>
    /// PageRank.
    /// </summary>

    PageRank = 64,

    /// <summary>
    /// Overall metrics.
    /// </summary>

    OverallMetrics = 128,

    /// <summary>
    /// Group metrics.
    /// </summary>

    GroupMetrics = 256,

    /// <summary>
    /// Edge reciprocation.
    /// </summary>

    EdgeReciprocation = 512,

    /// <summary>
    /// The top N values in one column ranked by values in another column.
    /// </summary>

    TopNBy = 1024,

    /// <summary>
    /// The top items in the tweets within a Twitter search network.
    /// </summary>

    TwitterSearchNetworkTopItems = 2048,

    /// <summary>
    /// The metrics for words in a specified text column.
    /// </summary>

    Words = 4096,

    /// <summary>
    /// Per-vertex reciprocated vertex pair ratio.
    /// </summary>

    ReciprocatedVertexPairRatio = 8192,

    // Important Note:
    //
    // If you add a new graph metric, you must add an array entry to
    // GraphMetricInformation.GetAllGraphMetricInformation().  That will
    // automatically add a checkbox for the new metric to the
    // GraphMetricsDialog.  If the new graph metric does not have user
    // settings, you do not need to modify the GraphMetricsDialog.
    //
    // If the new graph metric DOES have user settings, you have to modify the
    // GraphMetricsDialog to allow the user to edit the user settings.
}


//*****************************************************************************
//  Class: GraphMetricInformation
//
/// <summary>
/// Stores information about one graph metric.
/// </summary>
///
/// <remarks>
/// Call <see cref="GetAllGraphMetricInformation"/> to get information about
/// each graph metric in the <see cref="GraphMetrics" /> enumeration.
/// </remarks>
//*****************************************************************************

public class
GraphMetricInformation
{
    //*************************************************************************
    //  Property: GraphMetric
    //
    /// <summary>
    /// Gets the graph metric.
    /// </summary>
    ///
    /// <value>
    /// The graph metric's flag in the <see cref="GraphMetrics" /> enumeration.
    /// </value>
    //*************************************************************************

    public GraphMetrics
    GraphMetric
    {
        get
        {
            AssertValid();

            return (m_eGraphMetric);
        }
    }

    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the graph metric's friendly name.
    /// </summary>
    ///
    /// <value>
    /// The graph metric's friendly name, suitable for display in a dialog box.
    /// </value>
    //*************************************************************************

    public String
    Name
    {
        get
        {
            AssertValid();

            return (m_sName);
        }
    }

    //*************************************************************************
    //  Property: DescriptionAsHtml
    //
    /// <summary>
    /// Gets the graph metric's detailed description.
    /// </summary>
    ///
    /// <value>
    /// The graph metric's detailed description as HTML, without a body.
    /// </value>
    //*************************************************************************

    public String
    DescriptionAsHtml
    {
        get
        {
            AssertValid();

            // The description (in HTML) is stored in a resource file to make
            // it easier to enter a long, multiline description.
            //
            // The name of the string resource that contains a description of
            // the GraphMetrics.OverallMetrics metric, for example, is
            // "OverallMetrics".

            String sDescriptionResourceName = m_eGraphMetric.ToString();

            return ( GraphMetricDescriptions.ResourceManager.GetString(
                sDescriptionResourceName, GraphMetricDescriptions.Culture) );
        }
    }

    //*************************************************************************
    //  Property: HasUserSettings
    //
    /// <summary>
    /// Gets a flag indicating whether the graph metric has user settings.
    /// </summary>
    ///
    /// <value>
    /// true if graph metric has user settings.
    /// </value>
    //*************************************************************************

    public Boolean
    HasUserSettings
    {
        get
        {
            AssertValid();

            return (m_bHasUserSettings);
        }
    }

    //*************************************************************************
    //  Method: GetAllGraphMetricInformation()
    //
    /// <summary>
    /// Gets an array of <see cref="GraphMetricInformation" /> objects.
    /// </summary>
    ///
    /// <returns>
    /// An array of <see cref="GraphMetricInformation" /> objects, one for
    /// each graph metric.
    /// </returns>
    //*************************************************************************

    public static GraphMetricInformation []
    GetAllGraphMetricInformation()
    {
        // AssertValid();

        // Important Note:
        //
        // If you add a new graph metric to this array, you must also add a
        // string resource to GraphMetricDescriptions.resx that contains a
        // description of the graph metric in HTML format, without a body.  The
        // name of the string resource must be the same as the graph metric's
        // value in the GraphMetrics enumeration.  For example, the name of the
        // string resource that contains an HTML description of the
        // GraphMetrics.OverallMetrics metric is "OverallMetrics".

        return ( new GraphMetricInformation[] {

            new GraphMetricInformation(
                GraphMetrics.OverallMetrics,
                "Overall graph metrics",
                false),

            new GraphMetricInformation(
                GraphMetrics.Degree,
                "Vertex degree (undirected graphs only)",
                false),

            new GraphMetricInformation(
                GraphMetrics.InDegree,
                "Vertex in-degree (directed graphs only)",
                false),

            new GraphMetricInformation(
                GraphMetrics.OutDegree,
                "Vertex out-degree (directed graphs only)",
                false),

            new GraphMetricInformation(
                GraphMetrics.BrandesFastCentralities,
                "Vertex betweenness and closeness centralities",
                false),

            new GraphMetricInformation(
                GraphMetrics.EigenvectorCentrality,
                "Vertex eigenvector centrality",
                false),

            new GraphMetricInformation(
                GraphMetrics.PageRank,
                "Vertex PageRank",
                false),

            new GraphMetricInformation(
                GraphMetrics.ClusteringCoefficient,
                "Vertex clustering coefficient",
                false),

            new GraphMetricInformation(
                GraphMetrics.ReciprocatedVertexPairRatio,
                "Vertex reciprocated vertex pair ratio (directed graphs only)",
                false),

            new GraphMetricInformation(
                GraphMetrics.EdgeReciprocation,
                "Edge reciprocation (directed graphs only)",
                false),

            new GraphMetricInformation(
                GraphMetrics.GroupMetrics,
                "Group metrics",
                false),

            new GraphMetricInformation(
                GraphMetrics.Words,
                "Words and word pairs",
                true),

            new GraphMetricInformation(
                GraphMetrics.TopNBy,
                "Top items",
                true),

            new GraphMetricInformation(
                GraphMetrics.TwitterSearchNetworkTopItems,
                "Twitter search network top items",
                false),
            } );
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Formats the value of the current instance using the default format. 
    /// </summary>
    ///
    /// <returns>
    /// The formatted string.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        return (m_sName);
    }

    //*************************************************************************
    //  Constructor: GraphMetricInformation()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricInformation" />
    /// class.
    /// </summary>
    ///
    /// <param name="graphMetric">
    /// The graph metric's flag in the GraphMetrics enumeration.
    /// </param>
    ///
    /// <param name="name">
    /// The graph metric's friendly name, suitable for display in a dialog box.
    /// </param>
    ///
    /// <param name="hasUserSettings">
    /// true if graph metric has user settings.
    /// </param>
    //*************************************************************************

    private GraphMetricInformation
    (
        GraphMetrics graphMetric,
        String name,
        Boolean hasUserSettings
    )
    {
        m_eGraphMetric = graphMetric;
        m_sName = name;
        m_bHasUserSettings = hasUserSettings;

        AssertValid();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // m_eGraphMetric
        Debug.Assert( !String.IsNullOrEmpty(m_sName) );
        // m_bHasUserSettings
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The graph metric's flag in the GraphMetrics enumeration.

    protected GraphMetrics m_eGraphMetric;

    /// The graph metric's friendly name.

    protected String m_sName;

    /// true if graph metric has user settings.

    protected Boolean m_bHasUserSettings;
}

}
