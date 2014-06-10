using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
namespace GraphStorageManagement
{
    class PreviewComments : PreviewData
    {
        public DataTable preview()
        {
            return new DataTable();
        }
        public ListViewItem[] show_fields()
        {
            return new ListViewItem[1];
        }
        public ListViewItem[] show_preview(String text)
        {
            return new ListViewItem[1];
        }
    }
}
