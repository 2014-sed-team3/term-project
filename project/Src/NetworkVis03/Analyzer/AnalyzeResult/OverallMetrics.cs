using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class OverallMetrics : AnalyzeResultBase
    {

        public OverallMetrics
        (
            GraphDirectedness directedness,
            Int32 uniqueEdges,
            Int32 edgesWithDuplicates,
            Int32 selfLoops,
            Int32 vertices,
            Nullable<Double> graphDensity,
            Nullable<Double> modularity,
            Int32 connectedComponents,
            Int32 singleVertexConnectedComponents,
            Int32 maximumConnectedComponentVertices,
            Int32 maximumConnectedComponentEdges,
            Nullable<Int32> maximumGeodesicDistance,
            Nullable<Double> averageGeodesicDistance,
            Nullable<Double> reciprocatedVertexPairRatio,
            Nullable<Double> reciprocatedEdgeRatio
        )
        {
            m_eDirectedness = directedness;
            m_iUniqueEdges = uniqueEdges;
            m_iEdgesWithDuplicates = edgesWithDuplicates;
            m_iSelfLoops = selfLoops;
            m_iVertices = vertices;
            m_dGraphDensity = graphDensity;
            m_dModularity = modularity;
            m_iConnectedComponents = connectedComponents;
            m_iSingleVertexConnectedComponents = singleVertexConnectedComponents;

            m_iMaximumConnectedComponentVertices =
                maximumConnectedComponentVertices;

            m_iMaximumConnectedComponentEdges = maximumConnectedComponentEdges;
            m_iMaximumGeodesicDistance = maximumGeodesicDistance;
            m_dAverageGeodesicDistance = averageGeodesicDistance;
            m_dReciprocatedVertexPairRatio = reciprocatedVertexPairRatio;
            m_dReciprocatedEdgeRatio = reciprocatedEdgeRatio;

            
        }

        public DataTable[] ToDataTable
        {
            get {
                DataTable dtb = new DataTable();
                dtb.Columns.Add("Group_ID", typeof(int));
                dtb.Columns.Add("UniqueEdges", typeof(int));
                dtb.Columns.Add("EdgesWithDuplicates", typeof(int));
                dtb.Columns.Add("Vertices", typeof(int));
                dtb.Columns.Add("TotalEdges", typeof(int));
                dtb.Columns.Add("SelfLoops", typeof(int));
                dtb.Columns.Add("ReciprocatedVertexPairRatio", typeof(double));
                dtb.Columns.Add("Modularity", typeof(double));
                dtb.Columns.Add("ConnectedComponents", typeof(int));
                dtb.Columns.Add("SingleVertexConnectedComponents", typeof(int));
                dtb.Columns.Add("MaximumConnectedComponentVertices", typeof(int));
                dtb.Columns.Add("MaximumConnectedComponentEdges", typeof(int));
                dtb.Columns.Add("MaximumGeodesicDistance", typeof(int));
                dtb.Columns.Add("AverageGeodesicDistance", typeof(double));
                dtb.Columns.Add("GraphDensity", typeof(double));

                int iUniqueEdges = UniqueEdges;
                int iEdgesWithDuplicates = EdgesWithDuplicates;
                int iVertices = Vertices;
                int iTotalEdges = TotalEdges;
                int iSelfLoops = SelfLoops;
                double dReciprocatedVertexPairRatio = (!ReciprocatedVertexPairRatio.HasValue) ? 0 : ReciprocatedVertexPairRatio.Value;
                double dModularity = (!Modularity.HasValue) ? 0 : Modularity.Value;
                int iConnectedComponents = ConnectedComponents;
                int iSingleVertexConnectedComponents = SingleVertexConnectedComponents;
                int iMaximumConnectedComponentVertices = MaximumConnectedComponentVertices;
                int iMaximumConnectedComponentEdges = MaximumConnectedComponentEdges;
                int iMaximumGeodesicDistance = (!MaximumGeodesicDistance.HasValue) ? 0 : MaximumGeodesicDistance.Value;
                double dAverageGeodesicDistance = (!AverageGeodesicDistance.HasValue) ? 0 : AverageGeodesicDistance.Value;
                double dGraphDensity = (!GraphDensity.HasValue) ? 0 : GraphDensity.Value;

                dtb.Rows.Add(
                        0,
                        iUniqueEdges,
                        iEdgesWithDuplicates,
                        iVertices,
                        iTotalEdges,
                        iSelfLoops,
                        dReciprocatedVertexPairRatio,
                        dModularity,
                        iConnectedComponents,
                        iSingleVertexConnectedComponents,
                        iMaximumConnectedComponentVertices,
                        iMaximumConnectedComponentEdges,
                        iMaximumGeodesicDistance,
                        dAverageGeodesicDistance,
                        dGraphDensity
                        );

                return new DataTable[] { dtb };
            
            }
        }

        public GraphDirectedness Directedness{
            get{
                return (m_eDirectedness);
            }
        }


        public Int32 UniqueEdges{
            get
            {
               
                return (m_iUniqueEdges);
            }
        }

     

        public Int32
        EdgesWithDuplicates
        {
            get
            {
                return (m_iEdgesWithDuplicates);
            }
        }

        

        public Int32
        TotalEdges
        {
            get
            {
                return (m_iUniqueEdges + m_iEdgesWithDuplicates);
            }
        }

        

        public Int32
        SelfLoops
        {
            get
            {
                return (m_iSelfLoops);
            }
        }

        

        public Int32
        Vertices
        {
            get
            {
                return (m_iVertices);
            }
        }

        public Nullable<Double>
        GraphDensity
        {
            get
            {
                return (m_dGraphDensity);
            }
        }


        public Nullable<Double>
        Modularity
        {
            get
            {
                return (m_dModularity);
            }
        }


        public int
        ConnectedComponents
        {
            get
            {
                return (m_iConnectedComponents);
            }
        }


        public Int32 SingleVertexConnectedComponents
        {
            get
            {
                
                return (m_iSingleVertexConnectedComponents);
            }
        }

        public Int32
        MaximumConnectedComponentVertices
        {
            get
            {
                return (m_iMaximumConnectedComponentVertices);
            }
        }

       

        public Int32
        MaximumConnectedComponentEdges
        {
            get
            {
               
                return (m_iMaximumConnectedComponentEdges);
            }
        }

        

        public Nullable<Int32>
        MaximumGeodesicDistance
        {
            get
            {
                return (m_iMaximumGeodesicDistance);
            }
        }

        

        public Nullable<Double>
        AverageGeodesicDistance
        {
            get
            {
               
                return (m_dAverageGeodesicDistance);
            }
        }

        

        public Nullable<Double>
        ReciprocatedVertexPairRatio
        {
            get
            {
               
                return (m_dReciprocatedVertexPairRatio);
            }
        }

        

        public Nullable<Double>
        ReciprocatedEdgeRatio
        {
            get
            {
                return (m_dReciprocatedEdgeRatio);
            }
        }

        

        public String
        ConvertToSummaryString()
        {


            StringBuilder oStringBuilder = new StringBuilder();

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.Vertices, m_iVertices);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.UniqueEdges, m_iUniqueEdges);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.EdgesWithDuplicates, m_iEdgesWithDuplicates);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.TotalEdges, this.TotalEdges);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.SelfLoops, m_iSelfLoops);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.ReciprocatedVertexPairRatio,

                NullableToOverallMetricValue<Double>(
                    m_dReciprocatedVertexPairRatio)
                );

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.ReciprocatedEdgeRatio,
                NullableToOverallMetricValue<Double>(m_dReciprocatedEdgeRatio)
                );

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.ConnectedComponents, m_iConnectedComponents);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.SingleVertexConnectedComponents,
                m_iSingleVertexConnectedComponents);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.MaximumConnectedComponentVertices,
                m_iMaximumConnectedComponentVertices);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.MaximumConnectedComponentEdges,
                m_iMaximumConnectedComponentEdges);

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.MaximumGeodesicDistance,
                NullableToOverallMetricValue<Int32>(m_iMaximumGeodesicDistance)
                );

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.AverageGeodesicDistance,
                NullableToOverallMetricValue<Double>(m_dAverageGeodesicDistance)
                );

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.GraphDensity,
                NullableToOverallMetricValue<Double>(m_dGraphDensity)
                );

            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.Modularity,
                NullableToOverallMetricValue<Double>(m_dModularity)
                );
            /*
            AppendOverallMetricToSummaryString(oStringBuilder,
                OverallMetricNames.NodeXLVersion,
                AssemblyUtil2.GetFileVersion()
                );
             */

            return (oStringBuilder.ToString());
        }

        

        protected void
        AppendOverallMetricToSummaryString
        (
            StringBuilder oStringBuilder,
            String sOverallMetricName,
            Object oOverallMetricValue
        )
        {
            Debug.Assert(oStringBuilder != null);
            Debug.Assert(!String.IsNullOrEmpty(sOverallMetricName));
            Debug.Assert(oOverallMetricValue != null);

            if (oStringBuilder.Length > 0)
            {
                oStringBuilder.AppendLine();
            }

            oStringBuilder.AppendFormat(
                "{0}: {1}"
                ,
                sOverallMetricName,
                oOverallMetricValue
                );
        }

       

        protected String
        NullableToOverallMetricValue<T>
        (
            Nullable<T> oNullable
        )
        where T : struct
        {
            if (oNullable.HasValue)
            {
                return (oNullable.Value.ToString());
            }

            return ("Not Applicable");
        }


       

        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// The graph's directedness.

        protected GraphDirectedness m_eDirectedness;

        /// The number of unique edges.

        protected Int32 m_iUniqueEdges;

        /// The number of edges that have duplicates.

        protected Int32 m_iEdgesWithDuplicates;

        /// The number of self-loops.

        protected Int32 m_iSelfLoops;

        /// The number of vertices.

        protected Int32 m_iVertices;

        /// The graph's density, or null.

        protected Nullable<Double> m_dGraphDensity;

        /// The graph's modularity, or null.

        protected Nullable<Double> m_dModularity;

        /// The number of connected components in the graph.

        protected Int32 m_iConnectedComponents;

        /// The number of connected components in the graph that have one vertex.

        protected Int32 m_iSingleVertexConnectedComponents;

        /// The maximum number of vertices in a connected component.

        protected Int32 m_iMaximumConnectedComponentVertices;

        /// The maximum number of edges in a connected component.

        protected Int32 m_iMaximumConnectedComponentEdges;

        /// The maximum geodesic distance in the graph, or null.

        protected Nullable<Int32> m_iMaximumGeodesicDistance;

        /// The average geodesic distance in the graph, or null.

        protected Nullable<Double> m_dAverageGeodesicDistance;

        /// The ratio of the number of vertex pairs that are connected with
        /// directed edges in both directions to the number of vertex pairs that
        /// are connected with any directed edge, or null.

        protected Nullable<Double> m_dReciprocatedVertexPairRatio;

        /// The ratio of directed edges that have a reciprocated edge to the total
        /// number of directed edges, or null.

        protected Nullable<Double> m_dReciprocatedEdgeRatio;

        
    }


    //*****************************************************************************
    //  Class: OverallMetricNames
    //
    /// <summary>
    /// Provides names to use for overall metrics.
    /// </summary>
    ///
    /// <remarks>
    /// All overall metric names are available as public constants.
    /// </remarks>
    //*****************************************************************************

    public static class OverallMetricNames
    {
        ///
        public const String Directedness = "Graph Type";
        ///
        public const String UniqueEdges = "Unique Edges";
        ///
        public const String EdgesWithDuplicates = "Edges With Duplicates";
        ///
        public const String SelfLoops = "Self-Loops";
        ///
        public const String TotalEdges = "Total Edges";
        ///
        public const String Vertices = "Vertices";
        ///
        public const String GraphDensity = "Graph Density";
        ///
        public const String Modularity = "Modularity";
        ///
        public const String ConnectedComponents = "Connected Components";
        ///
        public const String SingleVertexConnectedComponents =
            "Single-Vertex Connected Components";
        ///
        public const String MaximumConnectedComponentVertices =
            "Maximum Vertices in a Connected Component";
        ///
        public const String MaximumConnectedComponentEdges =
            "Maximum Edges in a Connected Component";
        ///
        public const String MaximumGeodesicDistance =
            "Maximum Geodesic Distance (Diameter)";
        ///
        public const String AverageGeodesicDistance =
            "Average Geodesic Distance";
        ///
        public const String ReciprocatedVertexPairRatio =
            "Reciprocated Vertex Pair Ratio";
        ///
        public const String ReciprocatedEdgeRatio =
            "Reciprocated Edge Ratio";
        ///
        public const String NodeXLVersion =
            "NodeXL Version";
    }
}
