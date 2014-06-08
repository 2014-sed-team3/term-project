using Analyzer;
using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandaloneNode
{
    public class MetricsCalculatorManager
    {
        private BackgroundWorker m_oBackgroundWorker;
        private MetricSettingDialog m_oMetricSettingDialog;
        private MetricsCalculationProgressDialog m_oWorkProgressDialog;

        public MetricsCalculatorManager()
        {
            m_oBackgroundWorker = null;
        }

        /* Create analyzers and graph according to a "setting" object provided by outter View component; then pass to a BackgroundWorker.
         * In this project, this is invoked by a Dialog onload event handler.
         * */
        public void calculateMetricsAsync(MetricsCheckedList checkedlist)
        {

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

            CalculateGraphMetricsAsyncArgs args = new CalculateGraphMetricsAsyncArgs(); // add analyzer to this object and pass to BackgroundWorker

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

        protected void BackgroundWorker_DoWork
            (object sender, DoWorkEventArgs e)
        {

            Debug.Assert(e.Argument is CalculateGraphMetricsAsyncArgs);

            CalculateGraphMetricsAsyncArgs tmpArgs = (CalculateGraphMetricsAsyncArgs)e.Argument;
            AnalyzerBase[] analyzers = tmpArgs.Analyzers;
            IGraph graph = tmpArgs.Graph;
            LinkedList<AnalyzerBase> results = new LinkedList<AnalyzerBase>();
            foreach (AnalyzerBase analyzer in analyzers)
            {
                AnalyzeResultBase result;
                if (!analyzer.tryAnalyze(graph, m_oBackgroundWorker, out result))
                {
                    // The user cancelled.

                    e.Cancel = true;

                    m_oBackgroundWorker.ReportProgress(0, new GraphMetricProgress("Cancelled.", false));
                    return;
                }

                results.AddLast(result);
            }
            e.Result = results;

        }

        /* This is invoked when a progress changed event is raised
         * Outter View component will be notified by this method to do corresponding acts.
         * */
        protected void BackgroundWorker_ProgressChanged
            (object sender, ProgressChangedEventArgs e)
        {
            AssertValid();

            // Forward the event.

            ProgressChangedEventHandler oGraphMetricCalculationProgressChanged = this.GraphMetricCalculationProgressChanged;

            if (oGraphMetricCalculationProgressChanged != null)
            {
                // There are two sources of this event: the graph metric
                // calculators in the Algorithms namespace, which set e.UserState
                // to a simple string ("Calculating vertex degrees," for example);
                // and this GraphMetricCalculationManager class, which sets
                // e.UserState to a GraphMetricProgress object.  In the first case,
                // wrap the simple string in a new GraphMetricProgress object.

                if (e.UserState is String)
                {
                    String sProgressMessage = (String)e.UserStat;
                    e = new ProgressChangedEventArgs(e.ProgressPercentage, sProgressMessage);
                }

                oGraphMetricCalculationProgressChanged(this, e);
            }
        }

        /* This is invoked when the BackgroundWorker completes, is cancelled or issues error.
         * This method will forward such event to View, i.e Dialog in this project, so that View can do corresponding acts.
         * */
        protected void BackgroundWorker_RunWorkerCompleted
            (object sender, RunWorkerCompletedEventArgs e)
        {
            RunWorkerCompletedEventHandler oGraphMetricCalculationCompleted = this.CalculationCompleted;

            if (oGraphMetricCalculationCompleted != null)
            {
                Debug.Assert(e.Cancelled || e.Error != null ||
                    e.Result is LinkedList<AnalyzerBase>);

                oGraphMetricCalculationCompleted(this, e);  //forward event
                if (!e.Cancelled) //normally complete
                {
                    Debug.Assert(e.Result is LinkedList<AnalyzerBase>);
                    /* write e.Result to data base */
                }
            }

            oGraphMetricCalculationCompleted(this, e);
        }

        /* Allow outter View component to give a "Cancell" command to this manager
         * */
        public void CancelAsyncCalculate()
        {
            if (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy)
                m_oBackgroundWorker.CancelAsync();
        }

        protected class CalculateGraphMetricsAsyncArgs
        {
            public IGraph Graph;
            public AnalyzerBase[] Analyzers;
        };


        public event ProgressChangedEventHandler CalculationProgressChanged; // invoked when BackgroundWorker progress changed
        public event RunWorkerCompletedEventHandler CalculationCompleted; // invoked when BackgroundWorker complete works
    }

    public class MetricsCheckedList
    {
        public bool overall_graph_metrics;
        public bool vertex_degree;
        public bool vertex_reciprocated_pair_ratio;
        public bool vertex_clustering_coefficient;
        public bool vertex_pagerank;
        public bool vertex_eigenvector_centrality;
        public bool group_metrics;

        public MetricsCheckedList()
        {
            overall_graph_metrics = false;
            vertex_degree = false;
            vertex_reciprocated_pair_ratio = false;
            vertex_clustering_coefficient = false;
            vertex_pagerank = false;
            vertex_eigenvector_centrality = false;
            group_metrics = false;
        }

    }
}
