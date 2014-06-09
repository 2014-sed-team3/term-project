using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class MetricBoolean : Dictionary<int, bool>, VertexMetricBase
    {
        private string metricname;

        public LinkedList<string> MetricNameList
        {
            get
            {
                LinkedList<string> namelist = new LinkedList<string>();
                namelist.AddFirst(metricname);
                return namelist;
            }
        }
        public DataTable[] ToDataTable
        {
            get
            {
                DataTable dtb = new DataTable("Vertex");

                dtb.Columns.Add("Vertex_ID", typeof(int));
                dtb.Columns.Add(metricname, typeof(bool));
                dtb.PrimaryKey = new DataColumn[] { dtb.Columns["Vertex_ID"] };
                foreach (KeyValuePair<int, bool> p in this)
                {
                    dtb.Rows.Add(p.Key, p.Value);
                }
                return new DataTable[]{dtb};
            }
        }


        public MetricBoolean(int count, string name)
            : base(count)
        {
            metricname = name;
        }

        
    }

}
