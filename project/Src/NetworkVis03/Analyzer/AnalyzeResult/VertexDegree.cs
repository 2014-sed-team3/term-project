using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class VertexDegrees : Dictionary<int, Dictionary<string, int>>, VertexMetricBase
    {
        private const int numMetrics = 3;
        public VertexDegrees(int count) : base(count)
        {
        }

        public void Add(int VID, int indegree, int outdegree)
        {
            int totaldegree = indegree + outdegree;
            Dictionary<string, int> degreelist = new Dictionary<string, int>(numMetrics);
            degreelist.Add("InDegree", indegree);
            degreelist.Add("OutDegree", outdegree);
            degreelist.Add("TotalDegree", totaldegree);
            base.Add(VID, degreelist);
        }

        public LinkedList<string> MetricNameList {
            get {
                LinkedList<string> list = new LinkedList<string>();
                list.AddLast("InDegree");
                list.AddLast("OutDegree");
                list.AddLast("TotalDegree");
                return list;
            }
        }
        public DataTable[] ToDataTable
        {
            get
            {
                DataTable dtb = new DataTable("Vertex");
                dtb.Columns.Add("Vertex_ID", typeof(int));
                dtb.Columns.Add("InDegree", typeof(int));
                dtb.Columns.Add("OutDegree", typeof(int));
                dtb.Columns.Add("TotalDegree", typeof(int));
                dtb.PrimaryKey = new DataColumn[] { dtb.Columns["Vertex_ID"] };
                foreach (KeyValuePair<int, Dictionary<string, int>> p in this)
                {
                    dtb.Rows.Add(p.Key, p.Value["InDegree"], p.Value["OutDegree"], p.Value["TotalDegree"]);
                }
                return new DataTable[] {dtb};
            }
        }



        
    }
}
