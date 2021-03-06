﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer_Core
{
    public abstract class DataTableObservableBase
    {
        private LinkedList<IDataTableObserver> m_oDataTableObservers;
        public abstract List<DataTable> getDataTables { get; }
        public void notify()
        {
            foreach (IDataTableObserver ob in m_oDataTableObservers)
                ob.refreshwith(this);
        }
        public void attach(IDataTableObserver observer)
        {
            if (m_oDataTableObservers == null) m_oDataTableObservers = new LinkedList<IDataTableObserver>();
            m_oDataTableObservers.AddLast(observer);
        }
        public bool deattach(IDataTableObserver observer) {
            return m_oDataTableObservers.Remove(observer);
        }
    }
}
