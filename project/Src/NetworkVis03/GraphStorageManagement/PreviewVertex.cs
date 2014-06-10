using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace GraphStorageManagement
{
    class PreviewVertex : PreviewData
    {
        private DataTable prefetch = new DataTable();
        private ListViewItem[] list_set;
        public PreviewVertex(DB_Manager dbm)
        {
            String[] fields = dbm.list_post_schema();
            list_set = new ListViewItem[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                list_set[i] = new ListViewItem(fields[i], 0);
                append(dbm.mysql_query(String.Format("select {0} from networkvis.allinone WHERE ({1} is not null and {2} != '') limit 10", fields[i], fields[i], fields[i])));
            }
            list_set[0].Selected = true;
        }

        private void append(DataTable new_one)
        {
            if (new_one.Columns.Count <= 0) return;
            if (prefetch.Columns.Count <= 0)
            {
                prefetch = new_one;
                return;
            }
            DataColumn col = new_one.Columns[0];
            String name = col.ColumnName;
            prefetch.Columns.Add(name, col.DataType);
            int i = 0;
            foreach (DataRow row in prefetch.Rows)
            {
                if (i >= new_one.Rows.Count) continue;
                row[name] = new_one.Rows[i++][name];
            }
        }
        public String getFirst(){
            return list_set[0].Text;
        }
        public ListViewItem[] show_fields()
        {
            return list_set;
        }
        public ListViewItem[] show_preview(String selected_field)
        {
            int i = 0;
            ListViewItem[] set = new ListViewItem[prefetch.Rows.Count];
            foreach (DataRow row in prefetch.Rows)
            {
                set[i++] = new ListViewItem(row[selected_field].ToString(), 0);
            }
            return set;
        }
    }
}
