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

        public ShowMetricCalculateResult() {
            InitializeComponent();
            m_oVertexTable = new DataTable("Vertex");
            m_oVertexTable.Columns.Add("Vertex_ID", typeof(int));
            m_oVertexTable.PrimaryKey = new DataColumn[] { m_oVertexTable.Columns["Vertex_ID"] };
            m_oEdgeTable = new DataTable("Edge");
            m_oEdgeTable.Columns.Add("Edge_ID", typeof(int));
            m_oEdgeTable.PrimaryKey = new DataColumn[] { m_oEdgeTable.Columns["Edge_ID"] };
            m_oGroupTable = new DataTable("Group");
            m_oGroupTable.Columns.Add("Group_ID", typeof(int));
            m_oGroupTable.PrimaryKey = new DataColumn[] { m_oGroupTable.Columns["Group_ID"] };
        }
        public void refreshDisplay(DataTable Vertex, DataTable Edge, DataTable Group)
        {   
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
                        Debug.Assert(m_oVertexTable.Rows.Contains(dr["Vertex_ID"]));                      
                    }
                    foreach (DataColumn dc in tb.Columns)
                    {
                        if (!m_oVertexTable.Columns.Contains(dc.ColumnName))
                            m_oVertexTable.Columns.Add(dc.ColumnName, dc.DataType);
                        foreach (DataRow dr in tb.Rows) {
                            if (dc.ColumnName != "Vertex_ID")
                                m_oVertexTable.Rows.Find(dr["Vertex_ID"])[dc.ColumnName] = dr[dc.ColumnName]; 
                        }
                    } 
                }
                else if (tb.TableName == "Edge")
                {
                    foreach (DataRow dr in tb.Rows)
                    {
                        Debug.Assert(m_oVertexTable.Rows.Contains(dr["Edge_ID"]));

                    }
                    foreach (DataColumn dc in tb.Columns)
                    {
                        if (!m_oVertexTable.Columns.Contains(dc.ColumnName))
                            m_oVertexTable.Columns.Add(dc.ColumnName, dc.DataType);
                        foreach (DataRow dr in tb.Rows)
                        {
                            if (dc.ColumnName != "Edge_ID")
                                m_oVertexTable.Rows.Find(dr["Edge_ID"])[dc.ColumnName] = dr[dc.ColumnName];
                        }
                    }
                }
                else if (tb.TableName == "Group")
                {
                    foreach (DataRow dr in tb.Rows)
                    {
                        Debug.Assert(m_oVertexTable.Rows.Contains(dr["Group_ID"]));

                    }
                    foreach (DataColumn dc in tb.Columns)
                    {
                        if (!m_oVertexTable.Columns.Contains(dc.ColumnName))
                            m_oVertexTable.Columns.Add(dc.ColumnName, dc.DataType);
                        foreach (DataRow dr in tb.Rows)
                        {
                            if (dc.ColumnName != "Group_ID")
                            {
                                Debug.Assert(m_oVertexTable.Rows.Contains(dr["Group_ID"]));
                                m_oVertexTable.Rows.Find(dr["Group_ID"])[dc.ColumnName] = dr[dc.ColumnName];
                            }
                        }
                    }
                }
            
            }
            refreshDisplay(m_oVertexTable, m_oEdgeTable, m_oGroupTable);
            if (!this.Visible)
                this.Show();
        }

        private DataTable m_oVertexTable;
        private DataTable m_oEdgeTable;
        private DataTable m_oGroupTable;
    }
}
