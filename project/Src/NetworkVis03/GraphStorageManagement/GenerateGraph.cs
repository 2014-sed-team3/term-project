using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphStorageManagement
{
    public partial class GenerateGraph : Form
    {
        public event EventHandler<IgraphGenerateEvent> GraphGenerated;
        private PreviewVertex preview_vertex;
        private PreviewArticles preview_articles;
      
        public void loadVertexFields(ListView view){
            preview_vertex = new PreviewVertex(dbm);
            view.Items.AddRange(preview_vertex.show_fields());
            view.EndUpdate();
        }
        public void loadEdgeFields(ListView view, String selected){
            preview_articles = new PreviewArticles(dbm, selected);
            view.Items.Clear();
            view.Items.AddRange(preview_articles.show_fields());
            view.EndUpdate();
        }
        public GenerateGraph(DB_Manager _dbm, EventHandler<IgraphGenerateEvent> _e)
        {
            dbm = _dbm;
            GraphGenerated = _e;
            InitializeComponent();            
            loadVertexFields(listView1);
            loadEdgeFields(listView2, preview_vertex.getFirst());
            comboBox1.SelectedIndex = 0;
        }

        private void GenerateGraph_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String first = dbm.setting.vertexCol;
            String second = dbm.setting.edgeCol;
            if (first == null)
            {
                first = listView1.Items[0].Text;
            }
            if (second == null)
            {
                second = listView2.Items[0].Text;
            }
            Console.WriteLine(first);
            Console.WriteLine(second);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public bool isSelected;
        DB_Manager dbm;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                String field = item.Text;
                
                preview_articles.change_prev(field);
                loadEdgeFields(listView2, field);
                String first = preview_articles.getFirst();
        
                ListViewItem[] list = preview_articles.show_preview(first);
                list[0].Selected = true;
                listView3.Items.Clear();
                listView3.Items.AddRange(list);

                listView3.EndUpdate();
                listView8.Items.Clear();
                listView8.Items.AddRange(preview_vertex.show_preview(field));
                listView8.EndUpdate();

                dbm.setting.vertexCol = field;
                dbm.setting.edgeCol = null;
            }
        }

        private void listView8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ListViewItem item = listView2.SelectedItems[0];
                String field = item.Text;
                listView3.Items.Clear();
                listView3.Items.AddRange(preview_articles.show_preview(field));
                listView3.EndUpdate();

                dbm.setting.edgeCol = field;
            }
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
