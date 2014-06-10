using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Analyzer;
using Smrf.NodeXL.Core;
using System.Diagnostics;


namespace StandaloneNode
{
    public partial class MetricsCalculationProgressDialog : Form
    {
        private MetricsCheckedList m_oMetricsCheckedList;
        private MetricsCalculatorManager m_oGraphMetricCalculatorManager;
        private ShowMetricCalculateResult m_oShowMetricCalculateResult;
        IGraph m_ograph;

        public MetricsCalculationProgressDialog(IGraph graph, MetricsCheckedList chkList, ShowMetricCalculateResult oShowMetricCalculateResult)
        {
            InitializeComponent();
            m_oMetricsCheckedList = chkList;
            m_oGraphMetricCalculatorManager = new MetricsCalculatorManager();
            m_ograph = graph;

            m_oGraphMetricCalculatorManager.CalculationProgressChanged += new ProgressChangedEventHandler(Manager_ProgressChanged);
            m_oGraphMetricCalculatorManager.CalculationCompleted += new RunWorkerCompletedEventHandler(Manager_WorksCompleted);

            m_oShowMetricCalculateResult = oShowMetricCalculateResult;
            m_oGraphMetricCalculatorManager.attach(m_oShowMetricCalculateResult);

        }

        private void WorkProgressDialog_Load(object sender, EventArgs e)
        {
            m_oGraphMetricCalculatorManager.calculateMetricsAsync(m_ograph, m_oMetricsCheckedList);

        }

        /* This is invoked when manager's calculation progress is changed.
         * Then this dialog can refresh its display according to the msg given by manager
         * */
        private void Manager_ProgressChanged
            (object sender, ProgressChangedEventArgs e)
        {
            Debug.Assert(e.UserState is ProgressState);

            ProgressState oProgressState = (ProgressState)e.UserState;

            progressBar1.Value = e.ProgressPercentage;
            lblProcessState.Text = oProgressState.m_State;
                
            if (oProgressState.m_wbFlag == true)
            {
                // Writing to the workbook will occur shortly within the
                // GraphMetricCalculationCompleted handler.  Writing can't be
                // cancelled, so disable the cancel button.

                btnCancel.Enabled = false;
            }
        }

        /* This is invoked when manager complete its jobs (or being cancelled)
         * In this project, this dialog, as a View component, should just close itself
         * */
        private void Manager_WorksCompleted
            (object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
            m_oGraphMetricCalculatorManager.notify();
            m_oGraphMetricCalculatorManager.deattach(m_oShowMetricCalculateResult);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_oGraphMetricCalculatorManager.CancelAsyncCalculate();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        
    }
}
