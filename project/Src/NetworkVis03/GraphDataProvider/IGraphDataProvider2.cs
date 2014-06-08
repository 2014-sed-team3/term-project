using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphDataProvider
{
    public interface IGraphDataProvider2
    {
  
        String  Name
        {
            get;
        }

        String   Description
        {
            get;
        }

        Boolean  TryGetGraphDataAsTemporaryFile
        (
            out String pathToTemporaryFile
        );
    }
}
