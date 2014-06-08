using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandaloneNode
{
    class GroupCalculatorManager
    {

        private BackgroundWorker m_oBackgroundWorker;
        private MetricSettingDialog m_oMetricSettingDialog;
        private MetricsCalculationProgressDialog m_oWorkProgressDialog;

        public GroupCalculatorManager() {
            m_oBackgroundWorker = null;
        }

        public void calculateMetricsAsync(GroupsCheckedList checkedlist) {  

            if (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy)
            {
                /*
                throw new InvalidOperationException(String.Format(
                    "{0}:{1}: An asynchronous operation is already in progress."
                    ,
                    this.ClassName,
                    MethodName
                    ));
                 * */
            }

            // logic about which calculator should be created 

            CalculateGroupsAsyncArgs args = new CalculateGroupsAsyncArgs(); // add analyzer to this object and pass to BackgroundWorker

            if (checkedListBox.CheckedItems.Contains("overall graph metrics")) { } 
            if (checkedListBox.CheckedItems.Contains("vertex degree")) { }
            if (checkedListBox.CheckedItems.Contains("vertex reciprocated pair ratio")) { }
            if (checkedListBox.CheckedItems.Contains("vertex clustering coefficient")) { }
            if (checkedListBox.CheckedItems.Contains("vertex pagerank")) { }
            if (checkedListBox.CheckedItems.Contains("vertex eigen vector centrality")) { }
            if (checkedListBox.CheckedItems.Contains("group metrics")) { }


            // create a new BackgroundWorker
            m_oBackgroundWorker = new BackgroundWorker();
            m_oBackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            m_oBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            m_oBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

            m_oBackgroundWorker.RunWorkerAsync(args);

        }

        protected class CalculateGroupsAsyncArgs
        {
            public IGraph Graph;
            public AnalyzerBase[] Analyzers;
        };
    }

    public class GroupsCheckedList
    {
        public bool Fan_Motif;
        public bool Dconnector_Motif;
        public bool WakitaTsurumi_Cluster;
        public int dMininum;
        public int dMaximum;
        

        public MetricsCheckedList()
        {
            Fan_Motif = false;
            Dconnector_Motif = false;
            WakitaTsurumi_Cluster = false;
            dMininum = -1;
            dMaximum = -1;
        }

    }
}
