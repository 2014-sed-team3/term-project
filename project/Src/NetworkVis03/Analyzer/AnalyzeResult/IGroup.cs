using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public interface IGroup
    {
        IVertex[] getVertexWithin { get; }
        string getDescription { get; }
    }
}
