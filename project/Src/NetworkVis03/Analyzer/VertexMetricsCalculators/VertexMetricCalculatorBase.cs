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
    public abstract class VertexMetricCalculatorBase : AnalyzerBase
    {
        public abstract bool tryCalculate(IGraph graph, BackgroundWorker bgw, out VertexMetricBase metrics);

        public override string AnalyzerDescription
        {
            get { return CalculatorDescription(); }
        }

        public override bool tryAnalyze(IGraph graph, BackgroundWorker bgw, out AnalyzeResultBase results)
        {
            VertexMetricBase oVertexMetricBase;
            bool rv = tryCalculate(graph, bgw, out oVertexMetricBase);
            results = oVertexMetricBase;
            return rv;
        }
        
        public abstract string CalculatorDescription();





        
    }
}
