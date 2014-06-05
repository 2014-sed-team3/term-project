using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smrf.NodeXL.Core;

namespace Analyzer
{
    public class Groups : Dictionary<int, IGroup>, AnalyzeResultBase
    {
        public Groups(int count) : base(count) { }
        public DataTable[] ToDataTable
        {
            get {
                DataTable dtb1 = new DataTable(); // (groupid, vertexwithin) graunilarity to vertex

                dtb1.Columns.Add("GroupID", typeof(int));
                dtb1.Columns.Add("VertexID", typeof(int));

   
    
                DataTable dtb2 = new DataTable(); // (groupid, groupdescription) granularity to group
                dtb2.Columns.Add("GroupID", typeof(int));
                dtb2.Columns.Add("GroupDescription", typeof(string));
                
                foreach (KeyValuePair<int, IGroup> p in this) {
                    dtb2.Rows.Add(p.Key, p.Value.getDescription);
                    foreach (IVertex v in p.Value.getVertexWithin) {
                        dtb1.Rows.Add(p.Key, v.ID);
                    }
                }

                return new DataTable[] { dtb1, dtb2 };

            }
        }

        
    }
}
