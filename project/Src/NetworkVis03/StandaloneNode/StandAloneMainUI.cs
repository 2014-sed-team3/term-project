using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;

namespace StandaloneNode
{
    public partial class StandAloneMainUI: Form
    {
        IGraphAdapter oGraphAdapter;
        GraphMLGraphAdapter oGraphMLGraphAdapter;

        public StandAloneMainUI()
        {
            InitializeComponent();
        }

        private void facebookCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FacebookCrawler FBCrawl = new FacebookCrawler();
            FBCrawl.Show();
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SimpleGraphAdapter test, load
            DialogResult d = cdbOpenModel.ShowDialog();
            if (d != DialogResult.Cancel)
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                try
                {
                    // Serializer.Deserialize<Corpus>(cdbOpenModel.FileName, ref cp);
                    oGraphAdapter = new SimpleGraphAdapter();
                    layoutControl1.Graph = oGraphAdapter.LoadGraphFromFile(cdbOpenModel.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Cursor = Cursors.Default;
                //Application.DoEvents();
            }
        }

        private void loadGraphMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ////SimpleGraphAdapter test, load
            //DialogResult d = cdbOpenGraphML.ShowDialog();
            //if (d != DialogResult.Cancel)
            //{
            //    this.Cursor = Cursors.WaitCursor;
            //    Application.DoEvents();

            //    try
            //    {
            //        oGraphMLGraphAdapter = new GraphMLGraphAdapter();
            //        //oObject = oGraphMLGraphAdapter.LoadGraphFromFile(sFileName);
            //        IGraph graph = oGraphMLGraphAdapter.LoadGraphFromFile(cdbOpenModel.FileName);
            //        //layoutControl1.Graph = oGraphAdapter.LoadGraphFromFile(cdbOpenModel.FileName);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }

            //    this.Cursor = Cursors.Default;
            //    //Application.DoEvents();
            //}
        }
    }
}
