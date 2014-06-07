using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Smrf.NodeXL.Adapters;

namespace TestLayoutControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateAndDrawGraph();
        }

        protected void PopulateAndDrawGraph()
        {
            IGraphAdapter oGraphAdapter = new SimpleGraphAdapter();
            layoutControl1.Graph = oGraphAdapter.LoadGraphFromFile("..\\..\\SampleGraph.txt");
        }
    }
}
