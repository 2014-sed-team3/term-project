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

            DataRowCollection node_rows = node_table.Rows;
            DataRowCollection edge_rows = edge_table.Rows;

            add_nodes(node_rows, oVertices);
            add_edges(edge_rows, oVertices, oEdges);

            return oGraph;
        }

        private void add_nodes(DataRowCollection node_rows, IVertexCollection oVertices)
        {
            string nodename;
            double nodeid;
            double nodecount = 0;

            foreach (DataRow row in node_rows)
            {
                //Notice: "nodename" and "nodeid" should be edited
                nodename = row["nodename"].ToString();
                nodeid = (Double)row["nodeid"];

                // Add a node...
                IVertex oVertexA = oVertices.Add();
                oVertexA.Name = nodename;
                oVertexA.Tag = nodeid;

                nodecount++;
            }
        }

        private void add_edges(DataRowCollection edge_rows, IVertexCollection oVertices, IEdgeCollection oEdges)
        {
            double from;
            double to;

            foreach (DataRow row in edge_rows)
            {
                //Notice: "EdgeFromid" and "EdgeToid" should be edited
                from = (Double)row["EdgeFromid"];
                to = (Double)row["EdgeToid"];

                // Add an edge
                IVertex oFrom = null;
                IVertex oTo = null;

                foreach (IVertex oVertex in oVertices)
                {
                    if (oVertex.Tag.ToString() == from.ToString())
                    {
                        oFrom = oVertex;
                    }

                    if (oVertex.Tag.ToString() == to.ToString())
                    {
                        oTo = oVertex;
                    }
                }
                IEdge oEdge1 = oEdges.Add(oFrom, oTo, true);
            }
        }
    }
}
