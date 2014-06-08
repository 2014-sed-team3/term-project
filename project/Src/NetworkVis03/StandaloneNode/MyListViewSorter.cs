using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StandaloneNode
{
    public class MyListViewSorter : IComparer
    {
        private int _columnIndex;
        bool orderByDesc;
        bool number;
        public MyListViewSorter(int columnIndex, bool orderByDesc, bool number)
        {
            _columnIndex = columnIndex;
            this.orderByDesc = orderByDesc;
            this.number = number;
        }

        #region IComparer Members

        public int Compare(object x, object y)
        {
            ListViewItem lvi1;
            ListViewItem lvi2;
            if (orderByDesc)
            {
                lvi1 = x as ListViewItem;
                lvi2 = y as ListViewItem;
            }
            else
            {
                lvi2 = x as ListViewItem;
                lvi1 = y as ListViewItem;
            }
            if (number)
            {
                return Int32.Parse(lvi1.SubItems[_columnIndex].Text).CompareTo(Int32.Parse(lvi2.SubItems[_columnIndex].Text));
            }
            else
            {
                return lvi1.SubItems[_columnIndex].Text.CompareTo(lvi2.SubItems[_columnIndex].Text);
            }
        }

        #endregion
    }
}
