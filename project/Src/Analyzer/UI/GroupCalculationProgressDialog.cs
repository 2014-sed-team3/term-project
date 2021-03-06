﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class GroupCalculationProgressDialog : Form
    {
        private GroupsCheckedList m_oGroupsCheckedList;
        private GroupCalculatorManager m_oGroupCalculatorManager;
        public GroupCalculationProgressDialog(GroupsCheckedList chkList)
        {
            InitializeComponent();
            m_oGroupsCheckedList = chkList;
            m_oGroupCalculatorManager = new GroupCalculatorManager();

            m_oGroupCalculatorManager.CalculationProgressChanged += new ProgressChangedEventHandler(Manager_ProgressChanged);
            m_oGroupCalculatorManager.CalculationCompleted += new RunWorkerCompletedEventHandler(Manager_WorksCompleted);

        }

        private void WorkProgressDialog_Load(object sender, EventArgs e)
        {
            m_oGroupCalculatorManager.calculateMetricsAsync(m_oGroupsCheckedList);

        }

        /* This is invoked when manager's calculation progress is changed.
         * Then this dialog can refresh its display according to the msg given by manager
         * */
        private void Manager_ProgressChanged
            (object sender, ProgressChangedEventArgs e)
        {
            Debug.Assert(e.UserState is ProgressState);

            ProgressState oProgressState = (ProgressState)e.UserState;

            /* TO DO
             * refresh dialog according to oProgressState 
             */

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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_oGroupCalculatorManager.CancelAsyncCalculate();
        }
    }
}
