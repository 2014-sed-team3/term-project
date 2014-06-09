using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StandaloneNode
{
    public partial class ShowMetricCalculateResult : Form, IDataTableObserver
    {
        public ShowMetricCalculateResult(DataTable Vertex, DataTable Edge, DataTable Group)
        {
            InitializeComponent();
            this.dataGridView1.DataSource = Vertex;
            dataGridView1.Update();
            this.dataGridView2.DataSource = Edge;
            dataGridView2.Update();
            this.dataGridView3.DataSource = Group;
            dataGridView3.Update();
        }

        public void refreshwith(DataTableObservableBase source)
        {
            List<DataTable> tbs = source.getDataTables;

            foreach (DataTable tb in tbs) {
                if (tb.TableName == "Vertex") {
                    foreach (DataRow dr in tb.Rows)
                    {
                        Debug.Assert(m_oVertexTable.Rows.Contains(dr["VertexID"]));
                        
                    }
                    foreach (DataColumn dc in tb.Columns)
                    {
                        if (!m_oVertexTable.Columns.Contains(dc.ColumnName))
                            m_oVertexTable.Columns.Add(dc.ColumnName, dc.DataType);
                        foreach (DataRow dr in tb.Rows) {
                            if (dc.ColumnName != "VertexID")
                                m_oVertexTable.Rows.Find(dr["VertexID"])[dc.ColumnName] = dr[dc.ColumnName]; 
                        }
                    } 
                }
                else if (tb.TableName == "Edge")
                {
                    foreach (DataRow dr in tb.Rows)
                    {
                        Debug.Assert(m_oVertexTable.Rows.Contains(dr["VertexID"]));

                    }
                    foreach (DataColumn dc in tb.Columns)
                    {
                        if (!m_oVertexTable.Columns.Contains(dc.ColumnName))
                            m_oVertexTable.Columns.Add(dc.ColumnName, dc.DataType);
                        foreach (DataRow dr in tb.Rows)
                        {
                            if (dc.ColumnName != "EdgeID")
                                m_oVertexTable.Rows.Find(dr["EdgeID"])[dc.ColumnName] = dr[dc.ColumnName];
                        }
                    }
                }
                else if (tb.TableName == "Group")
                {
                    foreach (DataRow dr in tb.Rows)
                    {
                        Debug.Assert(m_oVertexTable.Rows.Contains(dr["GroupID"]));

                    }
                    foreach (DataColumn dc in tb.Columns)
                    {
                        if (!m_oVertexTable.Columns.Contains(dc.ColumnName))
                            m_oVertexTable.Columns.Add(dc.ColumnName, dc.DataType);
                        foreach (DataRow dr in tb.Rows)
                        {
                            if (dc.ColumnName != "GroupID")
                                m_oVertexTable.Rows.Find(dr["GroupID"])[dc.ColumnName] = dr[dc.ColumnName];
                        }
                    }
                }
            
            }
            throw new NotImplementedException();
        }

        private DataTable m_oVertexTable;
        private DataTable m_oEdgeTable;
        private DataTable m_oGroupTable;
    }
}
