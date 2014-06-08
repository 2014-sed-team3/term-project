using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class MetricSettingDialog : Form
    {
        private MetricsCalculatorManager m_ocontroller;
        public MetricSettingDialog()
        {
            InitializeComponent();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                this.checkedListBox1.SetItemChecked(i, true);
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                this.checkedListBox1.SetItemChecked(i, false);
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            MetricsCheckedList chklist = new MetricsCheckedList();
 
            if(checkedListBox1.CheckedItems.Contains("overall graph metrics")) chklist.overall_graph_metrics = true;
            if(checkedListBox1.CheckedItems.Contains("vertex degree")) chklist.vertex_degree = true;
            if(checkedListBox1.CheckedItems.Contains("vertex reciprocated pair ratio")) chklist.vertex_reciprocated_pair_ratio = true;
            if(checkedListBox1.CheckedItems.Contains("vertex clustering coefficient")) chklist.vertex_clustering_coefficient = true;
            if(checkedListBox1.CheckedItems.Contains("vertex pagerank")) chklist.vertex_pagerank = true;
            if(checkedListBox1.CheckedItems.Contains("edge reciprocation")) chklist.edge_reciprocation = true;
            if(checkedListBox1.CheckedItems.Contains("vertex eigen vector centrality")) chklist.vertex_eigenvector_centrality = true;
            if(checkedListBox1.CheckedItems.Contains("group metrics")) chklist.group_metrics = true;

            MetricsCalculationProgressDialog dg = new MetricsCalculationProgressDialog(chklist);
            if (dg.ShowDialog() == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

    }
}
