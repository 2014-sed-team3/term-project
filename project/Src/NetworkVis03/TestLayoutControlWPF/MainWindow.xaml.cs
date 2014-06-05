using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LayoutControls;
using Smrf.NodeXL.Adapters;
using Smrf.NodeXL.Core;

namespace TestLayoutControlWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LayoutControl layoutControl1 ;
        public MainWindow()
        {
            InitializeComponent();
            layoutControl1 = new LayoutControl();
            WfHost.Child = layoutControl1;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
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
