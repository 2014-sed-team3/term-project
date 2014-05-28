using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.OleDb;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Layouts;

namespace NetworkVis
{
    /// <summary>
    /// Interaction logic for Networkmanager.xaml
    /// </summary>
    public partial class Networkmanager : Window
    {
        private Repository localengine = new Repository();
        private OleDbCommand aCommandEdge;
        private OleDbDataReader aReaderEdge;
        public string SelectedNetID = "NONE";

        public Networkmanager()
        {
            InitializeComponent();
            localengine.Open_Repository();
            Load_Networks();
        }
        private void Load_Networks()
        {
            string SQL;
            string NetID, Netname;

            SQL = "select NetworkID, NetName from Networks";
            OleDbCommand aCommand = new OleDbCommand(SQL, localengine.RepositoryConnection);
            try
            {
                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Iterate throuth the database
                while (aReader.Read())
                {
                    NetID = aReader.GetString(0);
                    Netname = aReader.GetString(1);

                    ListBoxItem li = new ListBoxItem();
                    li.Content = Netname;
                    li.Tag = NetID;
                    ContainNetworks.Items.Add(li);
                }
                aReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void KillDialog(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            Window1.SelectedNet = SelectedNetID;
            Close();
        }

        private void ShowDesc(object sender, SelectionChangedEventArgs e)
        {
            string NetID;
            
            NetID = ((System.Windows.FrameworkElement)(((System.Windows.Controls.Primitives.Selector)(sender)).SelectedItem)).Tag.ToString();
            
            string SQL;
            string desc="(No Description)";

            SQL = "select Description from Networks where NetworkID='" + NetID.ToString() + "'";
            OleDbCommand aCommand = new OleDbCommand(SQL, localengine.RepositoryConnection);
            try
            {
                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Iterate throuth the database
                while (aReader.Read())
                {
                    desc = aReader.GetString(0);
                }
                aReader.Close();      
            }
            catch (Exception edesc)
            {
                MessageBox.Show(edesc.Message);
                desc = "Unable to show Description";
            }
                 
            // Hit the database and get the description...
            DescribeNetwork.Content = desc;
            SelectedNetID = NetID;

        }

        private void ShowProperties(object sender, RoutedEventArgs e)
        {
            NetProperty n = new NetProperty();
            n.Show();

        }

        private void BuildNetwork(object sender, RoutedEventArgs e)
        {
            string cs = "";
            string inserts = "";
            string networkkey = "PAYR";

            Repository localenginecreate = new Repository();
            localenginecreate.Open_Repository();

            // Go grab everything we need, based on the net id
            string DataBaseRoot = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()).Replace(@"file:\", "") + "\\Friends.db";

            cs = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Firefly\\List of customers for firefly poc\\List of customers for firefly poc.mdb";
            
            // Node SQL
            string nodeSQL = "select cust_skey, cust_name from cust where cust_type_MDM in ('PLAN', 'PAYR', 'CORP','PHAR', 'UNSP')";
            int nodenameMap;
            int nodeidMap;
            string nodename;
            double nodeid;
            

            


            // Edge SQL
            string edgeSQL = "select from_cust_skey, to_cust_skey from cust_affl WHERE affl_type in ('PLAN_to_PAYR', 'PHAR_to_CORP', 'PAYR_to_CORP', 'CORP_to_CORP')";
            
            int EdgeFromidMap = 0;
            int EdgeToidMap = 1;
            double from;
            double to;
            double nodecount = 0;
            try
            {
                // Set up a network object
                Graph oGraph = new Graph(GraphDirectedness.Directed);
                IVertexCollection oVertices = oGraph.Vertices;
                IEdgeCollection oEdges = oGraph.Edges;

                // connection
                OleDbConnection TestConn = new OleDbConnection(cs);
                TestConn.Open();
                
                OleDbCommand aCommand = new OleDbCommand(nodeSQL, TestConn);

                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                // Get the Nodes!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                nodenameMap = 1;
                nodeidMap = 0;
                while (aReader.Read())
                {
                    nodename = aReader.GetString(nodenameMap);
                    nodeid = aReader.GetDouble(nodeidMap);
                    
                    // Add a node...
                    IVertex oVertexA = oVertices.Add();
                    oVertexA.Name = nodename;
                    oVertexA.Tag = nodeid;

                    nodecount++;
                }
                aReader.Close();

                // Get the Edges!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                OleDbCommand aEdgeCommand = new OleDbCommand(edgeSQL, TestConn);

                //create the datareader object to connect to table
                OleDbDataReader aEdgeReader = aEdgeCommand.ExecuteReader();

                EdgeFromidMap = 0;
                EdgeToidMap = 1;

                while (aEdgeReader.Read())
                {
                    from = aEdgeReader.GetDouble(EdgeFromidMap);
                    to = aEdgeReader.GetDouble(EdgeToidMap);
                    
                    // Add an edge
                    IVertex oFrom = null;
                    IVertex oTo = null;

                    foreach (IVertex oVertex in oVertices)
                    {
                        if (oVertex.Tag.ToString() == from.ToString())
                        {
                            oFrom = oVertex;
                        }

                        if (oVertex.Tag.ToString() == to.ToString())
                        {
                            oTo = oVertex;

                        }


                    }
                    IEdge oEdge1 = oEdges.Add(oFrom, oTo, true);

                }
                aEdgeReader.Close();

                // Perform a layout
                // Apply Layout 
                // ==================================================================

                double xdim;
                double ydim;

                xdim = 5000;
                ydim = xdim;
                string layoutmethod = "Fruchterman/Reingold Layout";
                switch (layoutmethod)
                {
                    case "Circular Layout":
                        ILayout oLayout_cir = new CircleLayout();
                        LayoutContext oLayoutContext_cir = new LayoutContext(new System.Drawing.Rectangle(0, 0, (int)xdim, (int)ydim));
                        oLayout_cir.LayOutGraph(oGraph, oLayoutContext_cir);
                        break;
                    case "Random Layout":
                        ILayout oLayout_rand = new RandomLayout();
                        LayoutContext oLayoutContext_rand = new LayoutContext(new System.Drawing.Rectangle(0, 0, (int)xdim, (int)ydim));
                        oLayout_rand.LayOutGraph(oGraph, oLayoutContext_rand);
                        break;
                    case "Sugiyama Layout":
                        ILayout oLayout_Sugi = new SugiyamaLayout();
                        LayoutContext oLayoutContext_Sugi = new LayoutContext(new System.Drawing.Rectangle(0, 0, (int)xdim, (int)ydim));
                        oLayout_Sugi.LayOutGraph(oGraph, oLayoutContext_Sugi);
                        break;
                    case "Grid Layout":
                        ILayout oLayout_grid = new GridLayout();
                        LayoutContext oLayoutContext_grid = new LayoutContext(new System.Drawing.Rectangle(0, 0, (int)xdim, (int)ydim));
                        oLayout_grid.LayOutGraph(oGraph, oLayoutContext_grid);
                        break;
                    case "Spiral Layout":
                        ILayout oLayout_spiral = new SpiralLayout();
                        LayoutContext oLayoutContext_spiral = new LayoutContext(new System.Drawing.Rectangle(0, 0, (int)xdim, (int)ydim));
                        oLayout_spiral.LayOutGraph(oGraph, oLayoutContext_spiral);
                        break;
                    case "Fruchterman/Reingold Layout":
                        ILayout oLayout_Fruch = new FruchtermanReingoldLayout();
                        LayoutContext oLayoutContext_Fruch = new LayoutContext(new System.Drawing.Rectangle(0, 0, (int)xdim, (int)ydim));
                        oLayout_Fruch.LayOutGraph(oGraph, oLayoutContext_Fruch);
                        break;
                    case "Sinusoid H Layout":
                        ILayout oLayout_SinH = new SinusoidHorizontalLayout();
                        LayoutContext oLayoutContext_SinH = new LayoutContext(new System.Drawing.Rectangle(0, 0, (int)xdim, (int)ydim));
                        oLayout_SinH.LayOutGraph(oGraph, oLayoutContext_SinH);
                        break;
                    case "Sinusoid V Layout":
                        ILayout oLayout_SinV = new SinusoidVerticalLayout();
                        LayoutContext oLayoutContext_SinV = new LayoutContext(new System.Drawing.Rectangle(0, 0, (int)xdim, (int)ydim));
                        oLayout_SinV.LayOutGraph(oGraph, oLayoutContext_SinV);
                        break;

                }


                // Save the Nodes back out to the Firefly database
                // List the results.
                int xoffset = 0;
                int yoffset = 0;
                int size=10;
                double xx2, yy2,xx,yy;
                
                foreach (IVertex oVertex in oVertices)
                {
                    
                    UniversePadPoint p2 = new UniversePadPoint();

                    p2.PadName = oVertex.Name;
                    xx2 = oVertex.Location.X;
                    yy2 = oVertex.Location.Y;
                    xx2 = xx2 + xoffset;
                    yy2 = yy2 + yoffset;
                 
                    size = 10;
                                       
                    inserts = "insert into Nodes(id, nodename, networkid,x,y,z) values (" + oVertex.Tag.ToString() + ", '" + oVertex.Name.ToString() + "', '" + networkkey + "'," + xx2.ToString() + "," + yy2.ToString() + ",0)";
                    OleDbCommand postnode = new OleDbCommand(inserts, localenginecreate.RepositoryConnection);
                    OleDbDataReader Poster = postnode.ExecuteReader();
                    Poster.Close();

                }

                foreach (IEdge e2 in oEdges)
                {
                    IVertex f1 = e2.BackVertex;
                    IVertex t2 = e2.FrontVertex;
                    xx = f1.Location.X;
                    yy = f1.Location.Y;
                    xx2 = t2.Location.X;
                    yy2 = t2.Location.Y;

                    xx = xx + xoffset;
                    yy = yy + yoffset;
                    xx2 = xx2 + xoffset;
                    yy2 = yy2 + yoffset;

                    inserts = "insert into Edges(fromid, toid, networkid) values (" + f1.Tag.ToString() + ", " + t2.Tag.ToString() + ", '" + networkkey + "')";

                    OleDbCommand postedge = new OleDbCommand(inserts, localenginecreate.RepositoryConnection);
                    OleDbDataReader Poster = postedge.ExecuteReader();
                    Poster.Close();

                }

            }

            //Some usual exception handling
            catch (OleDbException eWriteNodes)
            {
                MessageBox.Show(eWriteNodes.Message);
                
            }
            localenginecreate.Close_Repository();
            MessageBox.Show("Done creating network.");

        }

        private void OpenNet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
