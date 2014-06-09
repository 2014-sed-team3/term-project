using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.NodeXL.Core;

namespace GraphStorageManagement
{
    public class IgraphGenerateEvent : EventArgs
    {
        private IGraph myGraph;
        public IgraphGenerateEvent(IGraph oGraph)
        {
            myGraph = oGraph;
        }
        public IGraph getGraph()
        {
            return myGraph;
        }
    }
}
