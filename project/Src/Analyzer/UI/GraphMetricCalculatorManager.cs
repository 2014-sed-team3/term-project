using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    class GraphMetricCalculatorManager
    {
        private System.Windows.Forms.CheckedListBox checkedListBox; // manager need to reference this object to know what kind of metrics to be calculated
        private MetricSettingDialog m_oMetricSettingDialog;

        public GraphMetricCalculatorManager() {
            m_oMetricSettingDialog = new MetricSettingDialog(this);
        }

        public void calculateMetrics(object sender, EventArgs e) {

            if (checkedListBox.CheckedItems.Contains("overall graph metrics")) { } // calculate overall metrics
            if (checkedListBox.CheckedItems.Contains("vertex degree")) { } //calculate vertex degree
            if (checkedListBox.CheckedItems.Contains("vertex reciprocated pair ratio")) { } //calculate reciprocated pair ratio
            if(checkedListBox.CheckedItems.Contains("vertex clustering coefficient")){}
            if(checkedListBox.CheckedItems.Contains("vertex pagerank")){}
            if(checkedListBox.CheckedItems.Contains("vertex eigen vector centrality")){}
            if (checkedListBox.CheckedIndices.Contains("group metrics")) { }

            closeDialog();

        }

        public void closeDialog() { } // notify MetricSettingDialog to close

        public void setCheckedListBox(System.Windows.Forms.CheckedListBox cb){
            checkedListBox = cb;
        }
    }
}