using Analyzer;
using Observer_Core;
using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandaloneNode
{
    public class MetricsCalculatorManager : DataTableObservableBase
    {
        private BackgroundWorker m_oBackgroundWorker;
        private bool wbFlag;
        private LinkedList<AnalyzeResultBase> m_oAnalyzeResults;

        public MetricsCalculatorManager() {
            m_oBackgroundWorker = null;
            wbFlag = false;
        }

        public override List<DataTable> getDataTables {
            get {
                List<DataTable> tables = new List<DataTable>();
                foreach (AnalyzeResultBase ar in m_oAnalyzeResults) {
                    tables.AddRange(ar.ToDataTable);
                }
                return tables;
            }
        }

        /* Create analyzers and graph according to a "setting" object provided by outter View component; then pass to a BackgroundWorker.
         * In this project, this is invoked by a Dialog onload event handler.
         * */
        public void calculateMetricsAsync(IGraph graph, MetricsCheckedList checkedlist) {  

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
            args.Analyzers = new LinkedList<AnalyzerBase>();
            args.Graph = graph;


            if (checkedlist.overall_graph_metrics == true) {args.Analyzers.AddLast(new OverallMetricCalculator()); }
            if (checkedlist.vertex_degree == true) {args.Analyzers.AddLast(new VertexDegreeCalculator()); }
            if (checkedlist.vertex_reciprocated_pair_ratio == true) {args.Analyzers.AddLast(new ReciprocatedVertexPairRatioCalculator()); }
            if (checkedlist.vertex_clustering_coefficient == true) {args.Analyzers.AddLast(new ClusteringCoefficientCalculator()); }
            if (checkedlist.vertex_pagerank == true) { args.Analyzers.AddLast(new PageRankCalculator());}
            if (checkedlist.vertex_eigenvector_centrality == true) {}
            if (checkedlist.group_metrics == true) { args.Analyzers.AddLast(new GroupMetricCalculator());} 
            
            // create a new BackgroundWorker
            m_oBackgroundWorker = new BackgroundWorker();

            m_oBackgroundWorker.WorkerReportsProgress = true;
            m_oBackgroundWorker.WorkerSupportsCancellation = true;

            m_oBackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            m_oBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            m_oBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

            m_oBackgroundWorker.RunWorkerAsync(args);
        }

        protected void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {  
            Debug.Assert(e.Argument is CalculateGraphMetricsAsyncArgs);

            CalculateGraphMetricsAsyncArgs tmpArgs = (CalculateGraphMetricsAsyncArgs)e.Argument;
            AnalyzerBase[] analyzers = tmpArgs.Analyzers.ToArray();
            IGraph graph = tmpArgs.Graph;
            LinkedList<AnalyzeResultBase> results = new LinkedList<AnalyzeResultBase>();
            foreach (AnalyzerBase analyzer in analyzers) {
                AnalyzeResultBase result;
                if (!analyzer.tryAnalyze(graph, m_oBackgroundWorker, out result))
                {
                    // The user cancelled.
                    e.Cancel = true;
                    m_oBackgroundWorker.ReportProgress(0, "Cancelled");
                    return;
                }
                results.AddLast(result);
            }
            e.Result = results;
            m_oBackgroundWorker.ReportProgress(100, new ProgressState(100, "Writing Back..", true));
        }

        /* This is invoked when a progress changed event is raised
         * Outter View component will be notified by this method to do corresponding acts.
         * */
        protected void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            ProgressChangedEventHandler oCalculationProgressChanged = this.CalculationProgressChanged;

            if (oCalculationProgressChanged != null)
            {
                // There are two sources of this event: the graph metric
                // calculators in the Algorithms namespace, which set e.UserState
                // to a simple string ("Calculating vertex degrees," for example);
                // and this GraphMetricCalculationManager class, which sets
                // e.UserState to a GraphMetricProgress object.  In the first case,
                // wrap the simple string in a new GraphMetricProgress object.

                if (e.UserState is String)
                {
                    ProgressState oProgressMessage = new ProgressState(e.ProgressPercentage, (string)e.UserState, false);
                    e = new ProgressChangedEventArgs(e.ProgressPercentage, oProgressMessage);
                }

                oCalculationProgressChanged(this, e); //forward event
            }
        }

        /* This is invoked when the BackgroundWorker completes, is cancelled or issues error.
         * This method will forward such event to View, i.e Dialog in this project, so that View can do corresponding acts.
         * */
        protected void BackgroundWorker_RunWorkerCompleted      
            (object sender, RunWorkerCompletedEventArgs e)
        {

            wbFlag = true;

            RunWorkerCompletedEventHandler oCalculationCompleted = this.CalculationCompleted;

            if (oCalculationCompleted != null)
            {
                
                Debug.Assert(e.Cancelled || e.Error != null ||
                    e.Result is LinkedList<AnalyzeResultBase>);
   
                if (!e.Cancelled) //normally complete
                {
                    Debug.Assert(e.Result is LinkedList<AnalyzeResultBase>);

                    /* TO DO
                     * write e.Result to data base
                     */
                }
                m_oAnalyzeResults = (LinkedList<AnalyzeResultBase>)e.Result;
                oCalculationCompleted(this, e);  //forward event to View component
            }

            
        }

        /* Allow outter View component to give a "Cancell" command to this manager
         * */
        public void CancelAsyncCalculate() {
            if (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy && wbFlag == false)
                m_oBackgroundWorker.CancelAsync();
        }

        protected class CalculateGraphMetricsAsyncArgs
        {
            public IGraph Graph;
            public LinkedList<AnalyzerBase> Analyzers;
        };


        public event ProgressChangedEventHandler CalculationProgressChanged; // invoked when BackgroundWorker progress changed
        public event RunWorkerCompletedEventHandler CalculationCompleted; // invoked when BackgroundWorker complete works

        
    }

    public class MetricsCheckedList {
        public bool overall_graph_metrics;
        public bool vertex_degree;
        public bool vertex_reciprocated_pair_ratio;
        public bool vertex_clustering_coefficient;
        public bool vertex_pagerank;
        public bool vertex_eigenvector_centrality;
        public bool group_metrics;
        public bool edge_reciprocation;
  
        public MetricsCheckedList(){
            overall_graph_metrics = false;
            vertex_degree = false;
            vertex_reciprocated_pair_ratio = false;
            vertex_clustering_coefficient = false;
            vertex_pagerank = false;
            vertex_eigenvector_centrality = false;
            group_metrics = false;
            edge_reciprocation = false;
        }
   
    }
}