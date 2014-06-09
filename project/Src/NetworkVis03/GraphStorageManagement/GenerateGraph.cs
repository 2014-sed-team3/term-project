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
        public GenerateGraph(DB_Manager _dbm, EventHandler<IgraphGenerateEvent> _e)
        {
            InitializeComponent();
            dbm = _dbm;
            GraphGenerated = _e;
        }

        private void GenerateGraph_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            DB_setting setting = dbm.setting;

            int idx = e.Index;
            if (e.CurrentValue == CheckState.Unchecked)
            {                
                listView1.Items[idx].Checked = true;
            }
            else if ((e.CurrentValue == CheckState.Checked))
            {
                setting.vertexIdx = idx;
                Console.WriteLine(idx);
            }
        }
        public bool isSelected;
        DB_Manager dbm;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
