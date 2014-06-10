using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphStorageManagement
{
    public interface PreviewData
    {
        ListViewItem[] show_fields();
        ListViewItem[] show_preview(String selected_field);
    }
}
