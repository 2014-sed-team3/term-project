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
        private GraphMetricCalculatorManager m_ocontroller;
        public MetricSettingDialog()
        {
            InitializeComponent();
        }

        public MetricSettingDialog(GraphMetricCalculatorManager ctlr) : base(){
            this.setController(ctlr);
            m_ocontroller.setCheckedListBox(checkedListBox1);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MetricSettingDialog_Load(object sender, EventArgs e)
        {

        }

        public void setController(GraphMetricCalculatorManager ctlr) {
            m_ocontroller = ctlr;
            this.btnCalculate.Click += new System.EventHandler(m_ocontroller.calculateMetrics);
            
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

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

    }
}
