using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Smrf.NodeXL.Core;

namespace GraphStorageManagement
{
    class DB_Converter
    {
        
        public Graph convert_to_graph(DataTable node_table, DataTable edge_table)
        {
            Graph oGraph = new Graph(GraphDirectedness.Directed);
            IVertexCollection oVertices = oGraph.Vertices;
            IEdgeCollection oEdges = oGraph.Edges;
            
            add_nodes(node_table, oVertices);
            add_edges(edge_table.Rows, oVertices, oEdges);
            
            return oGraph;
        }

        private void add_nodes(DataTable node_table, IVertexCollection oVertices)
        {
            DataRowCollection node_rows = node_table.Rows;
            DataColumnCollection node_col = node_table.Columns;
            
            foreach (DataRow row in node_rows)
            {
                //Notice: "nodename" and "nodeid" should be edited
                IVertex oVertexA = oVertices.Add();
                oVertexA.Name = row["NodeName"].ToString();
                oVertexA.Tag = row["ID"].ToString();
                int i = 0;
                foreach(DataColumn col in node_col){
                    oVertexA.SetValue(col.ColumnName, row[i++]);
                }
                
            }
        }

        private void add_edges(DataRowCollection edge_rows, IVertexCollection oVertices, IEdgeCollection oEdges)
        {
            String from;
            String to;

            foreach (DataRow row in edge_rows)
            {
                //Notice: "EdgeFromid" and "EdgeToid" should be edited
                from = row["FromID"].ToString();
                to = row["ToID"].ToString();

                // Add an edge
                IVertex oFrom = null;
                IVertex oTo = null;

                foreach (IVertex oVertex in oVertices)
                {
                    if (oVertex.Tag.ToString() == from)
                    {
                        oFrom = oVertex;
                    }

                    if (oVertex.Tag.ToString() == to)
                    {
                        oTo = oVertex;
                    }
                }
                IEdge oEdge1 = oEdges.Add(oFrom, oTo, true);
            }
        }
    }
}
