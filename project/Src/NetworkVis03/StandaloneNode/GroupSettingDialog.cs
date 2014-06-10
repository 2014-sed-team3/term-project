using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Observer_Core;

namespace StandaloneNode
{
    public partial class GroupSettingDialog : Form
    {
        private GroupsCheckedList chklist;
        private IGraph m_oGraph;
        private ShowMetricCalculateResult m_oShowMetricCalculateResult;
        public DataTableObservableBase m_oResultDataTableObservableBase;

        public GroupSettingDialog(IGraph graph, ShowMetricCalculateResult oShowMetricCalculateResult)
        {
            InitializeComponent();
            this.m_oGraph = graph;
            this.m_oShowMetricCalculateResult = oShowMetricCalculateResult;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            /* TO DO:
             * logic to set GroupsCheckedList and pass it to 
             * 
             * */
            chklist = new GroupsCheckedList();
            if (radDConnectorMotif.Checked)
            {
                chklist.Dconnector_Motif = true;
                chklist.dMaximum = Convert.ToInt32(numericUpDown2.Value);
                chklist.dMininum = Convert.ToInt32(numericUpDown1.Value);
            }
            else if (radFanMotif.Checked) chklist.Fan_Motif = true;
            else chklist.WakitaTsurumi_Cluster = true;

            GroupCalculationProgressDialog gp = new GroupCalculationProgressDialog(m_oGraph, chklist, m_oShowMetricCalculateResult);
            if (gp.ShowDialog() == DialogResult.OK) {
                DialogResult = DialogResult.OK;
                m_oResultDataTableObservableBase = m_oShowMetricCalculateResult.m_oDataTableObservableBase;
                this.Close();            
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

    }
}
