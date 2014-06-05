using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    class EdgeReciprocation : Dictionary<int, bool>, AnalyzeResultBase
    {
        public EdgeReciprocation(int count) : base(count) { }
        public System.Data.DataTable[] ToDataTable
        {
            get
            {
                DataTable dtb = new DataTable();
                dtb.Columns.Add("Edge_ID", typeof(int));
                dtb.Columns.Add("Reciprocated", typeof(bool));
                foreach (KeyValuePair<int, bool> p in this)
                {
                    dtb.Rows.Add(p.Key, p.Value);
                }
                return new DataTable[] { dtb };
            }
        }
    }
}
