﻿using Analyzer;
using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{

    class GroupCalculatorManager
    {

        private BackgroundWorker m_oBackgroundWorker;

        private bool wbFlag;

        public GroupCalculatorManager() {
            m_oBackgroundWorker = null;
            wbFlag = false;
        }

        /* Create calculator accoring to the input checkedlist and pass to BackgroundWorker */
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

            CalculateGroupsAsyncArgs args = new CalculateGroupsAsyncArgs(); // add analyzer to this object and pass to BackgroundWorker

            if (checkedlist.Fan_Motif == true) { }
            if (checkedlist.Dconnector_Motif == true && checkedlist.dMininum > 0 && checkedlist.dMaximum > 0) { }
            if (checkedlist.WakitaTsurumi_Cluster == true) { }
            

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

            Debug.Assert(e.Argument is CalculateGroupsAsyncArgs);

            CalculateGroupsAsyncArgs tmpArgs = (CalculateGroupsAsyncArgs)e.Argument;
            AnalyzerBase[] analyzers = tmpArgs.Analyzers;
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
        protected void BackgroundWorker_ProgressChanged
            (object sender, ProgressChangedEventArgs e)
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
        protected void BackgroundWorker_RunWorkerCompleted
            (object sender, RunWorkerCompletedEventArgs e)
        {

            wbFlag = true;

            RunWorkerCompletedEventHandler oCalculationCompleted = this.CalculationCompleted;

            if (oCalculationCompleted != null)
            {
                Debug.Assert(e.Cancelled || e.Error != null ||
                    e.Result is LinkedList<AnalyzerBase>);

                oCalculationCompleted(this, e);  //forward event
                if (!e.Cancelled) //normally complete
                {
                    Debug.Assert(e.Result is LinkedList<AnalyzerBase>);
                    /* TO DO:
                     * write e.Result to data base 
                     */
                }
            }

            oCalculationCompleted(this, e);
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
