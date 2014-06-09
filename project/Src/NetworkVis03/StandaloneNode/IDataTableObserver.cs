using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandaloneNode
{
    public interface IDataTableObserver
    {
        void refreshwith( DataTableObservableBase source);
    }
}
