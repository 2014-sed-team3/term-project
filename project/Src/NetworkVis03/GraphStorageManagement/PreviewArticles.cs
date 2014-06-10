using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
namespace GraphStorageManagement
{
    class PreviewArticles : PreviewData
    {
        private DataTable prefetch = new DataTable();
        private ListViewItem[] list_set;
        private String prev_selected;
        public PreviewArticles(DB_Manager dbm, String _prev_selected)
        {
            prev_selected = _prev_selected;
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
        public void change_prev(String p){
            prev_selected = p;
        }
        public String getFirst()
        {
            bool result = prev_selected.Equals(list_set[0].Text);
            return (result) ? list_set[1].Text : list_set[0].Text;
        }
        public ListViewItem[] show_fields()
        {
            ListViewItem[] selected_list = new ListViewItem[list_set.Length - 1];
            int k = 0;
            for (int i = 0; i < list_set.Length; i++)
            {
                String field = list_set[i].Text;
                if (field.Equals(prev_selected)) continue;
                selected_list[k++] = list_set[i];
            }
            return selected_list;
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
