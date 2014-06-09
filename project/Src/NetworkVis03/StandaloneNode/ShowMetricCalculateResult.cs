using Observer_Core;
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
        public DataTableObservableBase m_oDataTableObservableBase;

        private DataTable m_oVertexTable;
        private DataTable m_oEdgeTable;
        private DataTable m_oGroupTable;

        public ShowMetricCalculateResult() {
            InitializeComponent();
            /*
            m_oVertexTable = new DataTable("Vertex");
            m_oVertexTable.Columns.Add("Vertex_ID", typeof(int));
            m_oVertexTable.PrimaryKey = new DataColumn[] { m_oVertexTable.Columns["Vertex_ID"] };
            m_oEdgeTable = new DataTable("Edge");
            m_oEdgeTable.Columns.Add("Edge_ID", typeof(int));
            m_oEdgeTable.PrimaryKey = new DataColumn[] { m_oEdgeTable.Columns["Edge_ID"] };
            m_oGroupTable = new DataTable("Group");
            m_oGroupTable.Columns.Add("Group_ID", typeof(int));
            m_oGroupTable.PrimaryKey = new DataColumn[] { m_oGroupTable.Columns["Group_ID"] };
             * */
            m_oVertexTable = null;
            m_oEdgeTable = null;
            m_oGroupTable = null;
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
            m_oDataTableObservableBase = source;
            List<DataTable> tbs = source.getDataTables;

            foreach (DataTable tb in tbs) {
                if (tb.TableName == "Vertex") {
                    if (m_oVertexTable == null)
                    {
                        m_oVertexTable = tb;
                        continue;
                    }
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
                    if (m_oEdgeTable == null)
                    {
                        m_oEdgeTable = tb;
                        continue;
                    }
                    foreach (DataRow dr in tb.Rows)
                    {
                        Debug.Assert(m_oEdgeTable.Rows.Contains(dr["Edge_ID"]));

                    }
                    foreach (DataColumn dc in tb.Columns)
                    {
                        if (!m_oEdgeTable.Columns.Contains(dc.ColumnName))
                            m_oEdgeTable.Columns.Add(dc.ColumnName, dc.DataType);
                        foreach (DataRow dr in tb.Rows)
                        {
                            if (dc.ColumnName != "Edge_ID")
                                m_oEdgeTable.Rows.Find(dr["Edge_ID"])[dc.ColumnName] = dr[dc.ColumnName];
                        }
                    }
                }
                else if (tb.TableName == "Group")
                {
                    m_oGroupTable = tb;
                    /*
                    if (m_oGroupTable == null){
                        m_oGroupTable = tb;
                        continue;
                    }
                    foreach (DataRow dr in tb.Rows)
                    {
                        Debug.Assert(m_oGroupTable.Rows.Contains(dr["Group_ID"]));

                    }
                    foreach (DataColumn dc in tb.Columns)
                    {
                        if (!m_oGroupTable.Columns.Contains(dc.ColumnName))
                            m_oGroupTable.Columns.Add(dc.ColumnName, dc.DataType);
                        foreach (DataRow dr in tb.Rows)
                        {
                            if (dc.ColumnName != "Group_ID")
                            {
                                Debug.Assert(m_oGroupTable != null);
                                Debug.Assert(dr[dc.ColumnName] != null);
                                System.Console.WriteLine("!!!!!!!!!!!!!!!");
                                m_oGroupTable.Rows.Find(dr["Group_ID"])[dc.ColumnName] = dr[dc.ColumnName];
                            }
                        }
                    }*/
                }
            
            }
            refreshDisplay(m_oVertexTable, m_oEdgeTable, m_oGroupTable);
            if (!this.Visible)
                this.Show();
        }
        private void MyFormClosing(object sender, FormClosingEventArgs e) 
        {
            this.Hide();
            e.Cancel = true;
        }


        
    }
}
