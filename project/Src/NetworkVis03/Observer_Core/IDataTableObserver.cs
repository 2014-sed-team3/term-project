using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Core
{
    public interface IDataTableObserver
    {
        void refreshwith(DataTableObservableBase source);
    }
}
