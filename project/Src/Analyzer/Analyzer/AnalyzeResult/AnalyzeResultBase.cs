using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public interface AnalyzeResultBase
    {
         DataTable[] ToDataTable { get; }
    }
}
