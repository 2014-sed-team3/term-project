﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public interface VertexMetricBase : AnalyzeResultBase
    {
         LinkedList<String> MetricNameList { get; }
        
        
    }
}
