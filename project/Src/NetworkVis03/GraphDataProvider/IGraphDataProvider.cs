
using System;
using System.Diagnostics;

namespace InterfaceGraphDataProvider
{
    public interface IGraphDataProvider
    {
 
        String Name
        {
            get;
        }

        String  Description
        {
            get;
        }

        Boolean TryGetGraphData
        (
            out String graphDataAsGraphML
        );
    }
}
