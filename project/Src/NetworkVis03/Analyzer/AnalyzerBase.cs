using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public abstract class AnalyzerBase
    {
        

        public abstract bool tryAnalyze(IGraph graph, BackgroundWorker bgw, out AnalyzeResultBase results);


        public abstract string AnalyzerDescription { get; }

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
                    this.AnalyzerDescription
                    );

                oBackgroundWorker.ReportProgress(
                    CalculateProgressInPercent(iCalculationsSoFar,iTotalCalculations),
                    sProgress);
            }
        }


        public static Int32 CalculateProgressInPercent
            (Int32 calculationsCompleted, Int32 totalCalculations)
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
