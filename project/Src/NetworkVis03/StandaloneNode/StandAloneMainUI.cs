using System;
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
using Observer_Core;

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
        public DataTableObservableBase m_oResultDataTableObservableBase;

        protected IGraph m_oGraph;
        protected IVertexCollection m_oVertices;
        protected IEdgeCollection m_oEdges;
        private ShowMetricCalculateResult m_oShowMetricCalculateResult;

        public StandAloneMainUI()
        {
            InitializeComponent();
        }

        public void SetAndShowGraph(object sender, IgraphGenerateEvent e)
        {
            layoutControl1.SetAndShowGraph(e.getGraph());
            m_oShowMetricCalculateResult = new ShowMetricCalculateResult();     
        }

        private void facebookCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FacebookCrawler FBCrawl = new FacebookCrawler();
            FBCrawl.Show(this);
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SimpleGraphAdapter test, load
            cdbOpenModel.Filter = "Text Files (*.txt)|*.txt";
            DialogResult d = cdbOpenModel.ShowDialog();
            
            if (d != DialogResult.Cancel)
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                try
                {
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
            cdbOpenGraphML.Filter = "XML Files (*.xml)|*.xml";
            DialogResult d = cdbOpenGraphML.ShowDialog();
            if (d != DialogResult.Cancel)
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                try
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(cdbOpenGraphML.FileName);
                    string line = file.ReadToEnd();
                    file.Close();
                    oGraphMLGraphAdapter = new GraphMLGraphAdapter();
                    layoutControl1.SetAndShowGraph(oGraphMLGraphAdapter.LoadGraphFromString(line));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Cursor = Cursors.Default;
            }
        }

        private void youTuBeUserCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.Delete(TempXmlFileName);

            GetGraphData(new YouTubeUserNetworkGraphDataProvider());
            if (File.Exists(TempXmlFileName))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(TempXmlFileName);
                string line = file.ReadToEnd();
                file.Close();
                GraphMLGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();
                layoutControl1.SetAndShowGraph(oGraphMLGraphAdapter.LoadGraphFromString(line));
            }
            else
            {
                MessageBox.Show("Error: 請重新選取youTuBe User!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_oGraph = new Graph();
                layoutControl1.SetAndShowGraph(m_oGraph);
            }
        }

        private void youTuBeVideoCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.Delete(TempXmlFileName);

            GetGraphData(new YouTubeVideoNetworkGraphDataProvider());
            if (File.Exists(TempXmlFileName))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(TempXmlFileName);
                string line = file.ReadToEnd();
                file.Close();
                GraphMLGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();
                layoutControl1.SetAndShowGraph(oGraphMLGraphAdapter.LoadGraphFromString(line));
            }
            else
            {
                MessageBox.Show("Error: 請重新選取youTuBe Video!" , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_oGraph = new Graph();
                layoutControl1.SetAndShowGraph(m_oGraph);
            }
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
            if(m_oShowMetricCalculateResult == null)
                m_oShowMetricCalculateResult = new ShowMetricCalculateResult();
            if (layoutControl1.Graph.Vertices.Count >0)
            {
                GroupItemSelecter GIS = new GroupItemSelecter(layoutControl1.Graph, m_oShowMetricCalculateResult);
                if (GIS.ShowDialog() == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                    m_oResultDataTableObservableBase = m_oShowMetricCalculateResult.m_oDataTableObservableBase;
                }                
            }
            else
            {
                MessageBox.Show("Error: 請先選好讀入的Graph", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadFromDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventHandler<IgraphGenerateEvent> handler = new System.EventHandler<IgraphGenerateEvent>(this.SetAndShowGraph);
            DB_Manager dbm = new DB_Manager("networkvis");
            NetworkID nid = new NetworkID(dbm, handler);
            nid.Show(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (m_oShowMetricCalculateResult == null)
                m_oShowMetricCalculateResult = new ShowMetricCalculateResult();
            if (layoutControl1.Graph.Vertices.Count > 0)
            {
                GroupSettingDialog oGroupSettingDialog = new GroupSettingDialog(layoutControl1.Graph, m_oShowMetricCalculateResult);
                if (oGroupSettingDialog.ShowDialog() == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                    m_oResultDataTableObservableBase = m_oShowMetricCalculateResult.m_oDataTableObservableBase;
                }
            }
            else
            {
                MessageBox.Show("Error: 請先選好讀入的Graph", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void generateFromDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EventHandler<IgraphGenerateEvent> handler = new System.EventHandler<IgraphGenerateEvent>(this.SetAndShowGraph);
            DB_Manager dbm = new DB_Manager("networkvis");
            GenerateGraph genGraph = new GenerateGraph(dbm, handler);
            genGraph.Show(this);
        }
        
    }
}
