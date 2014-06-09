﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using InterfaceGraphDataProvider;
using Analyzer;

using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Smrf.NodeXL.GraphDataProviders.YouTube;

using GraphStorageManagement;

namespace StandaloneNode
{
    public partial class StandAloneMainUI : Form
    {
        IGraphAdapter oGraphAdapter;
        GraphMLGraphAdapter oGraphMLGraphAdapter;
        ClusteringCoefficientCalculator m_oClusteringCoefficientCalculator = new ClusteringCoefficientCalculator();
        MetricDouble oClusteringCoefficientDouble;
        PageRankCalculator m_oPageRankCalculator = new PageRankCalculator();
        MetricDouble oPageRankMetricDouble;
        protected IGraph m_oGraph;
        protected IVertexCollection m_oVertices;
        protected IEdgeCollection m_oEdges;

        public StandAloneMainUI()
        {
            InitializeComponent();
        }

        public void SetAndShowGraph(IGraph oGraph)
        {
            layoutControl1.SetAndShowGraph(oGraph);
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
                    layoutControl1.SetAndShowGraph(oGraphAdapter.LoadGraphFromFile(cdbOpenModel.FileName));
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
            //GraphMLGraphAdapter test, load
            DialogResult d = cdbOpenGraphML.ShowDialog();
            if (d != DialogResult.Cancel)
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                try
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(cdbOpenGraphML.FileName);
                    string line = file.ReadToEnd();
                    oGraphMLGraphAdapter = new GraphMLGraphAdapter();
                    layoutControl1.SetAndShowGraph(oGraphMLGraphAdapter.LoadGraphFromString(line));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Cursor = Cursors.Default;
                //Application.DoEvents();
            }
        }

        private void youTuBeUserCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetGraphData(new YouTubeUserNetworkGraphDataProvider());
            System.IO.StreamReader file = new System.IO.StreamReader(TempXmlFileName);
            string line = file.ReadToEnd();
            GraphMLGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();
            layoutControl1.SetAndShowGraph(oGraphMLGraphAdapter.LoadGraphFromString(line));
        }

        private void youTuBeVideoCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetGraphData(new YouTubeVideoNetworkGraphDataProvider());
            System.IO.StreamReader file = new System.IO.StreamReader(TempXmlFileName);
            string line = file.ReadToEnd();
            GraphMLGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();
            layoutControl1.SetAndShowGraph(oGraphMLGraphAdapter.LoadGraphFromString(line));
        }
        private void GetGraphData(IGraphDataProvider2 oGraphDataProvider)
        {
            String sPathToTemporaryFile;

            if (!oGraphDataProvider.TryGetGraphDataAsTemporaryFile(out sPathToTemporaryFile))
            {
                return;
            }
            File.Copy(sPathToTemporaryFile, TempXmlFileName, true);
            File.Delete(sPathToTemporaryFile);

        }
        private String TempXmlFileName
        {
            get
            {
                String sAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

                if (sAssemblyPath.StartsWith("file:"))
                {
                    sAssemblyPath = sAssemblyPath.Substring(6);
                }

                return (Path.Combine(sAssemblyPath, "TempGetGraphData.xml"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RTFDebugOut.Clear();
            /*
            foreach (IVertex oVertex in layoutControl1.Graph.Vertices)
            {
                oVertex.SetValue(ReservedMetadataKeys.PerVertexLabel, oVertex.Name);
                RTFDebugOut.AppendText(oVertex.Name.ToString() + "\r\n");
            }
            layoutControl1.ShowGraph(true);
             */
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool rv = m_oPageRankCalculator.TryCalculateGraphMetrics(layoutControl1.Graph, null, out oPageRankMetricDouble);    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool rv = m_oClusteringCoefficientCalculator.TryCalculateGraphMetrics(layoutControl1.Graph, null, out oClusteringCoefficientDouble);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GroupItemSelecter GIS = new GroupItemSelecter(layoutControl1.Graph);
            GIS.Show(this);
        }

        private void loadFromDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB_Manager dbm = new DB_Manager("test");
            NetworkID nid = new NetworkID(dbm);
            nid.Show(this);
        }
    }
}
