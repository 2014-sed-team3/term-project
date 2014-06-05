using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class MetricDouble : Dictionary<int, double>, VertexMetricBase
    {
        private string metricname;

        public LinkedList<string> MetricNameList {
            get {
                LinkedList<string> namelist = new LinkedList<string>();
                namelist.AddFirst(metricname);
                return namelist;
            }
        }
        public DataTable[] ToDataTable {
            get {
                DataTable dtb = new DataTable();
                dtb.Columns.Add("Vertex_ID", typeof(int));
                dtb.Columns.Add(metricname, typeof(double));
                foreach (KeyValuePair<int, double> p in this) {
                    dtb.Rows.Add(p.Key, p.Value);
                }
                return new DataTable[]{dtb};
            }
        }


        public MetricDouble(int count, string name)
            : base(count)
        {
            metricname = name;
        }
    }

}
