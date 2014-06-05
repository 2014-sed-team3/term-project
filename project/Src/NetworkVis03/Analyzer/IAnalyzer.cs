using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    interface IAnalyzer
    {
        AnalyzeResultBase analyze(IGraph graph);
        void setBackgroundWorker(BackgroundWorker b);
        string GraphMetricDescription { get; }
    }
}
