using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    class GroupsMetric : Dictionary<int, OverallMetrics>, AnalyzeResultBase
    {
       

        public GroupsMetric(int count) : base(count){}

        public DataTable[] ToDataTable
        {
            get {
                DataTable dtb = new DataTable();
                dtb.Columns.Add("Group_ID", typeof(int));
                dtb.Columns.Add("UniqueEdges", typeof(int));
                dtb.Columns.Add("EdgesWithDuplicates", typeof(int));
                dtb.Columns.Add("TotalEdges", typeof(int));
                dtb.Columns.Add("SelfLoops", typeof(int));
                dtb.Columns.Add("ReciprocatedVertexPairRatio", typeof(double));
                dtb.Columns.Add("ConnectedComponents", typeof(int));
                dtb.Columns.Add("SingleVertexConnectedComponents", typeof(int));
                dtb.Columns.Add("MaximumConnectedComponentVertices", typeof(int));
                dtb.Columns.Add("MaximumConnectedComponentEdges", typeof(int));
                dtb.Columns.Add("MaximumGeodesicDistance", typeof(int));
                dtb.Columns.Add("AverageGeodesicDistance", typeof(double));
                dtb.Columns.Add("GraphDensity", typeof(double));


                foreach (KeyValuePair<int, OverallMetrics> p in this) {

                    int iUniqueEdges = p.Value.UniqueEdges;
                    int iEdgesWithDuplicates = p.Value.EdgesWithDuplicates;
                    int iTotalEdges = p.Value.TotalEdges;
                    int iSelfLoops = p.Value.SelfLoops;
                    double dReciprocatedVertexPairRatio = (!p.Value.ReciprocatedVertexPairRatio.HasValue) ? 0 : p.Value.ReciprocatedVertexPairRatio.Value;
                    int iConnectedComponents = p.Value.ConnectedComponents;
                    int iSingleVertexConnectedComponents = p.Value.SingleVertexConnectedComponents;
                    int iMaximumConnectedComponentVertices = p.Value.MaximumConnectedComponentVertices;
                    int iMaximumConnectedComponentEdges = p.Value.MaximumConnectedComponentEdges;
                    int iMaximumGeodesicDistance = (!p.Value.MaximumGeodesicDistance.HasValue) ? 0 : p.Value.MaximumGeodesicDistance.Value;
                    double dAverageGeodesicDistance = (!p.Value.AverageGeodesicDistance.HasValue) ? 0 : p.Value.AverageGeodesicDistance.Value;
                    double dGraphDensity = (!p.Value.GraphDensity.HasValue) ? 0 : p.Value.GraphDensity.Value;

                    dtb.Rows.Add(
                        p.Key,
                        iUniqueEdges,
                        iEdgesWithDuplicates,
                        iTotalEdges,
                        iSelfLoops,
                        dReciprocatedVertexPairRatio,
                        iConnectedComponents,
                        iSingleVertexConnectedComponents,
                        iMaximumConnectedComponentVertices,
                        iMaximumConnectedComponentEdges,
                        iMaximumGeodesicDistance,
                        dAverageGeodesicDistance,
                        dGraphDensity
                        );
                }

                return new DataTable[]{dtb};

                
            }
        }
    }
}
