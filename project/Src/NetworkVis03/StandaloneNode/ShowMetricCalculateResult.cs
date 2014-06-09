using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StandaloneNode
{
    public partial class ShowMetricCalculateResult : Form
    {
        public ShowMetricCalculateResult(DataTable Vertex, DataTable Edge, DataTable Group)
        {
            InitializeComponent();
            this.dataGridView1.DataSource = Vertex;
            dataGridView1.Update();
            this.dataGridView2.DataSource = Edge;
            dataGridView2.Update();
            this.dataGridView3.DataSource = Group;
            dataGridView3.Update();
        }
    }
}
