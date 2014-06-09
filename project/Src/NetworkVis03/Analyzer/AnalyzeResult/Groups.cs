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
                DataTable dtb1 = new DataTable("Vertex"); // (groupid, vertexwithin) graunilarity to vertex

                dtb1.Columns.Add("Vertex_ID", typeof(int));
                dtb1.Columns.Add("Group_ID", typeof(int));
                dtb1.PrimaryKey = new DataColumn[] { dtb1.Columns["Vertex_ID"] };
                
    
                DataTable dtb2 = new DataTable("Group"); // (groupid, groupdescription) granularity to group
                dtb2.Columns.Add("Group_ID", typeof(int));
                dtb2.Columns.Add("GroupDescription", typeof(string));
                dtb2.PrimaryKey = new DataColumn[] { dtb2.Columns["Group_ID"] };
                
                foreach (KeyValuePair<int, IGroup> p in this) {
                    dtb2.Rows.Add(p.Key, p.Value.getDescription);
                    foreach (IVertex v in p.Value.getVertexWithin) {
                        dtb1.Rows.Add(v.ID, p.Key);
                    }
                }

                return new DataTable[] { dtb1, dtb2 };

            }
        }

        
    }
}
