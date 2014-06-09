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

    class GroupCalculatorManager : DataTableObservableBase
    {

        private BackgroundWorker m_oBackgroundWorker;
        private LinkedList<AnalyzeResultBase> m_oAnalyzeResults;

        private bool wbFlag;

        public GroupCalculatorManager() {
            m_oBackgroundWorker = null;
            wbFlag = false;
        }

        public override List<DataTable> getDataTables
        {
            get
            {
                List<DataTable> tables = new List<DataTable>();
                foreach (AnalyzeResultBase ar in m_oAnalyzeResults)
                {
                    tables.AddRange(ar.ToDataTable);
                }
                return tables;
            }
        }

        /* Create calculator accoring to the input checkedlist and pass to BackgroundWorker */
        public void calculateMetricsAsync(IGraph graph, GroupsCheckedList checkedlist) {  

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

            CalculateGroupsAsyncArgs args = new CalculateGroupsAsyncArgs(); // add analyzer to this object and pass to BackgroundWorker
            args.Analyzers = new LinkedList<AnalyzerBase>();
            args.Graph = graph;


            if (checkedlist.Fan_Motif == true) { args.Analyzers.AddLast(new FanMotifDetector()); }
            if (checkedlist.Dconnector_Motif == true && checkedlist.dMininum > 4 && checkedlist.dMaximum < 9999 && checkedlist.dMininum < checkedlist.dMaximum) {
                args.Analyzers.AddLast(new DConnectorMotifDetector(checkedlist.dMininum, checkedlist.dMaximum));
            }
            if (checkedlist.WakitaTsurumi_Cluster == true) { args.Analyzers.AddLast(new WakitaTsurumiClusterDetector());}
            

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

            Debug.Assert(e.Argument is CalculateGroupsAsyncArgs);

            CalculateGroupsAsyncArgs tmpArgs = (CalculateGroupsAsyncArgs)e.Argument;
            LinkedList<AnalyzerBase> analyzers = tmpArgs.Analyzers;
            IGraph graph = tmpArgs.Graph;
            LinkedList<AnalyzeResultBase> results = new LinkedList<AnalyzeResultBase>();
            foreach (AnalyzerBase analyzer in analyzers)
            {
                AnalyzeResultBase result;
                if (!analyzer.tryAnalyze(graph, m_oBackgroundWorker, out result)) // The user cancelled.
                {
                    e.Cancel = true;
                    m_oBackgroundWorker.ReportProgress(0, "Cancelled.");
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
            ProgressChangedEventHandler oCalculationProgressChanged =this.CalculationProgressChanged;

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
        protected void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            wbFlag = true;
            RunWorkerCompletedEventHandler oCalculationCompleted = this.CalculationCompleted;

            if (oCalculationCompleted != null)
            {
                Debug.Assert(e.Cancelled || e.Error != null ||
                    e.Result is LinkedList<AnalyzeResultBase>);

                oCalculationCompleted(this, e);  //forward event
                if (!e.Cancelled) //normally complete
                {
                    Debug.Assert(e.Result is LinkedList<AnalyzeResultBase>);
                    /* TO DO:
                     * write e.Result to data base 
                     */
                }
                m_oAnalyzeResults = (LinkedList<AnalyzeResultBase>)e.Result;
                oCalculationCompleted(this, e);
            }
            
        }

        /* Allow outter View component to give a "Cancell" command to this manager
         * */
        public void CancelAsyncCalculate()
        {
            if (m_oBackgroundWorker != null && m_oBackgroundWorker.IsBusy)
                m_oBackgroundWorker.CancelAsync();
        }

        public event ProgressChangedEventHandler CalculationProgressChanged; // invoked when BackgroundWorker progress changed
        public event RunWorkerCompletedEventHandler CalculationCompleted; // invoked when BackgroundWorker complete works

        protected class CalculateGroupsAsyncArgs
        {
            public IGraph Graph;
            public LinkedList<AnalyzerBase> Analyzers;
        };

        
    }

    public class GroupsCheckedList
    {
        public bool Fan_Motif;
        public bool Dconnector_Motif;
        public bool WakitaTsurumi_Cluster;
        public int dMininum;
        public int dMaximum;


        public GroupsCheckedList()
        {
            Fan_Motif = false;
            Dconnector_Motif = false;
            WakitaTsurumi_Cluster = false;
            dMininum = -1;
            dMaximum = -1;
        }

    }
}
