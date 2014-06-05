using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smrf.NodeXL.Core;
using System.ComponentModel;
using System.Diagnostics;

namespace Analyzer
{
    public abstract class GroupDetectorBase : IAnalyzer
    {
        private BackgroundWorker m_obackgroundWorker;
        
        public abstract Groups partition(IGraph graph);

         public AnalyzeResultBase analyze(IGraph graph)
         {
             return partition(graph);
         }

         public string GraphMetricDescription
         {
             get { return "~~~"; }
         }

         public void setBackgroundWorker(BackgroundWorker b)
         {
             m_obackgroundWorker = b;
         }
         protected BackgroundWorker getBackgroundWorker()
         {
             return m_obackgroundWorker;
         }

         protected Boolean ReportProgressAndCheckCancellationPending
             (Int32 iCalculationsSoFar, Int32 iTotalCalculations, BackgroundWorker oBackgroundWorker)
         {
             Debug.Assert(iCalculationsSoFar >= 0);
             Debug.Assert(iTotalCalculations >= 0);
             Debug.Assert(iCalculationsSoFar <= iTotalCalculations);
            

             if (oBackgroundWorker != null)
             {
                 if (oBackgroundWorker.CancellationPending)
                 {
                     return (false);
                 }

                 ReportProgress(iCalculationsSoFar, iTotalCalculations,
                     oBackgroundWorker);
             }

             return (true);
         }

         protected void ReportProgress
             (Int32 iCalculationsSoFar, Int32 iTotalCalculations, BackgroundWorker oBackgroundWorker)
         {
             Debug.Assert(iCalculationsSoFar >= 0);
             Debug.Assert(iTotalCalculations >= 0);
             Debug.Assert(iCalculationsSoFar <= iTotalCalculations);


             if (oBackgroundWorker != null)
             {
                 String sProgress = String.Format(

                     "Calculating {0}."
                     ,
                     this.GraphMetricDescription
                     );

                 oBackgroundWorker.ReportProgress(

                     CalculateProgressInPercent(iCalculationsSoFar,
                         iTotalCalculations),

                     sProgress);
             }
         }


         public static Int32 CalculateProgressInPercent
             (Int32 calculationsCompleted,Int32 totalCalculations)
         {
             Debug.Assert(calculationsCompleted >= 0);
             Debug.Assert(totalCalculations >= 0);
             Debug.Assert(calculationsCompleted <= totalCalculations);

             Int32 iPercentProgress = 0;

             if (totalCalculations > 0)
             {
                 iPercentProgress = (Int32)(100F *
                     (Single)calculationsCompleted / (Single)totalCalculations);
             }

             return (iPercentProgress);
         }


         
    }
}
