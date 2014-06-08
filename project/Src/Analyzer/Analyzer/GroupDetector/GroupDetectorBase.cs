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
    public abstract class GroupDetectorBase : AnalyzerBase
    {
        private BackgroundWorker m_obackgroundWorker;

        public abstract bool tryPartition(IGraph graph, BackgroundWorker bgw, out Groups results);

         public override bool tryAnalyze(IGraph graph, BackgroundWorker bgw, out AnalyzeResultBase results)
         {
             Groups oGroups;
             bool rv = tryPartition(graph, bgw, out oGroups);
             results = oGroups;
             return rv;
         }

         public override string AnalyzerDescription
         {
             get { return getPartitionerDescription(); }
         }

         public abstract string getPartitionerDescription(); 

         
    }
}
