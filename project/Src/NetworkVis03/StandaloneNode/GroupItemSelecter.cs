using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Smrf.NodeXL.Core;

namespace StandaloneNode
{
    public partial class GroupItemSelecter : Form
    {
        private MetricsCalculatorManager m_ocontroller;
        public MetricsCheckedList chklist = new MetricsCheckedList();
        private IGraph m_oGraph;

        public GroupItemSelecter(IGraph graph)
        {
            InitializeComponent();
            m_oGraph = graph;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if(checkedListBox1.Items[e.Index].ToString() == "overall graph metrics")
                {
                    chklist.overall_graph_metrics = true;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex degree")
                {
                    chklist.vertex_degree = true;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex reciprocated pair ratio")
                {
                    chklist.vertex_reciprocated_pair_ratio = true;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex clustering coefficient")
                {
                    chklist.vertex_clustering_coefficient = true;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex pagerank")
                {
                    chklist.vertex_pagerank = true;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "edge reciprocation")
                {
                    chklist.edge_reciprocation = true;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex eigen vector centrality")
                {
                    chklist.vertex_eigenvector_centrality = true;
                }
                else
                {
                    chklist.group_metrics = true;
                }

            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                if (checkedListBox1.Items[e.Index].ToString() == "overall graph metrics")
                {
                    chklist.overall_graph_metrics = false;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex degree")
                {
                    chklist.vertex_degree = false;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex reciprocated pair ratio")
                {
                    chklist.vertex_reciprocated_pair_ratio = false;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex clustering coefficient")
                {
                    chklist.vertex_clustering_coefficient = false;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex pagerank")
                {
                    chklist.vertex_pagerank = false;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "edge reciprocation")
                {
                    chklist.edge_reciprocation = false;
                }
                else if (checkedListBox1.Items[e.Index].ToString() == "vertex eigen vector centrality")
                {
                    chklist.vertex_eigenvector_centrality = false;
                }
                else
                {
                    chklist.group_metrics = false;
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, true);
            }                
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, false);
            }
                
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            //chklist = new MetricsCheckedList();

            //if (checkedListBox1.CheckedItems.Contains("overall graph metrics")) chklist.overall_graph_metrics = true;
            //if (checkedListBox1.CheckedItems.Contains("vertex degree")) chklist.vertex_degree = true;
            //if (checkedListBox1.CheckedItems.Contains("vertex reciprocated pair ratio")) chklist.vertex_reciprocated_pair_ratio = true;
            //if (checkedListBox1.CheckedItems.Contains("vertex clustering coefficient")) chklist.vertex_clustering_coefficient = true;
            //if (checkedListBox1.CheckedItems.Contains("vertex pagerank")) chklist.vertex_pagerank = true;
            //if (checkedListBox1.CheckedItems.Contains("edge reciprocation")) chklist.edge_reciprocation = true;
            //if (checkedListBox1.CheckedItems.Contains("vertex eigen vector centrality")) chklist.vertex_eigenvector_centrality = true;
            //if (checkedListBox1.CheckedItems.Contains("group metrics")) chklist.group_metrics = true;

            MetricsCalculationProgressDialog dg = new MetricsCalculationProgressDialog(m_oGraph, chklist);
            //dg.Show(this);
            if (dg.ShowDialog() == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
