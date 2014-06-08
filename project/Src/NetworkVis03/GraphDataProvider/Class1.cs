
using System;
using System.Diagnostics;

namespace GraphDataProvider
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
