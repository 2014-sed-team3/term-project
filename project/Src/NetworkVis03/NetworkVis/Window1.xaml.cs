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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Data.OleDb;
using Microsoft.NodeXL.Core;
using Microsoft.NodeXL.Layouts;

namespace NetworkVis
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Repository localengine = new Repository();
        private OleDbCommand aCommandEdge;
        private OleDbDataReader aReaderEdge;
        private bool NetworksOpen = false;
        private bool AppearanceOpen = false;
        private bool ExploreOpen = false;
        private bool DrillDownOpen = false;
        private bool FilterOpen = false;
        public string OpenNet = "PAYR";
        public string mode = "SELECT"; // MOVE
        private Point mouseDragStartPoint;
        private Point scrollStartOffset;
        public double CanvasX = 0;
        public double CanvasY = 0;
        //
        public static string SelectedNet = "";

        public Window1()
        {
            //Splash s = new Splash();
            //s.Show();
            InitializeComponent();
            localengine.Open_Repository();
            Build_Local_Network(1);
            Draw_Universe(OpenNet);
            // Build_Universe_Network("Fruchterman/Reingold Layout", OpenNet);
            // LayoutMethod.SelectedIndex = 0;
            // UniverseBackground.Loaded += new RoutedEventHandler(Up_and_Loaded);
            UniverseBackground.ClipToBounds = true;
            //Load_Viewpoints();
            DetermineNetworkProfile();
            
        }

        private void Load_Callouts()
        {
            Callouts c = new Callouts();
            c.Create(70, 120, "Text", "Here is my comment");
            c.Render(UniverseBackground);

            Callouts c1 = new Callouts();
            c1.Create(245, 150, "Link", "http://www.microsoft.com");
            c1.Render(UniverseBackground);
        }

        private void DetermineNetworkProfile()
        {
            int max;
            int result = 0;
            int loop;
            int ht = 0;
            double scale = 2;
            double spacing;
            max = GetMaximumEdges();
            spacing = 170 / max;
            MinEdges.Maximum = max;
            MaxEdges.Maximum = max;
            for (loop = 0; loop <= max; loop++)
            {
                ht = GetEdgesNumbers(loop);
                Rectangle r1 = new Rectangle();
                r1.ToolTip = ht.ToString() + " Edges";
                if (ht == 0)
                {
                    r1.Height = scale;
                    r1.Stroke = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    r1.Height = ht * scale;
                    r1.Fill = new SolidColorBrush(Colors.Blue);
                }
                r1.Width = spacing/2;
                r1.VerticalAlignment = VerticalAlignment.Bottom;
                r1.Margin = new Thickness(0, spacing/3, spacing/3, 0);
                NetSignature.Children.Add(r1);
            }
        }

        private int GetEdgesNumbers(int spot)
        {
            int result = 0;
            string SQL = "SELECT fromid, count(*) FROM Edges group by fromid order by 2 desc";
            OleDbCommand aCommand = new OleDbCommand(SQL, localengine.RepositoryConnection);
            try
            {
                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Iterate throuth the database
                while (aReader.Read())
                {
                    if (aReader.GetInt32(1) ==spot)
                    {
                        result++;
                    }
                }
                aReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to count Network Edges for " + spot.ToString());
                MessageBox.Show(e.Message);

            }

            return result;
        }

        private int GetMaximumEdges()
        {
            int result = 0;
            
            string SQL = "SELECT fromid, count(*) FROM Edges group by fromid order by 2 desc";
            OleDbCommand aCommand = new OleDbCommand(SQL, localengine.RepositoryConnection);
            try
            {
                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Iterate throuth the database
                while (aReader.Read())
                {
                    if (aReader.GetInt32(1) > result)
                    {
                        result = aReader.GetInt32(1);
                    }
                }
                aReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to discover Maximum Range of Network Edges!");
                MessageBox.Show(e.Message);
 
            }
            return result;

        }

        private void Load_Viewpoints()
        {
            Viewpoint vp = new Viewpoint();
            vp.Render(20, 20, 30, 30, 1, 1, "Vp", OverviewContainer, UniverseBackground);

        }

        private void Up_and_Loaded(object sender, RoutedEventArgs e)
        {
            Record_Thumb(Overview);
            Load_Views();
            
        }

        private void Load_Views()
        {
 
        }

        private void Record_Thumb(Image i2)
        {
                        
            BitmapSource scrn;
            scrn = CaptureScreen(UniverseBackground);

            ScaleTransform myscale;
            myscale = new ScaleTransform(0.01, 0.01);
            
            i2.Source = scrn;
            //Overview.Opacity = 1;
            
        }

        private void Build_Universe_Network(string layoutmethod, string netid)
        {
            string SQL;
            string NodeName;
            int NodeID, FromID, ToID, EdgeID;
            double xx2, yy2, size, xx,yy;
            double px, py;
            int comments;
            Random rnd = new Random();

            Graph oGraph = new Graph(GraphDirectedness.Directed);
            IVertexCollection oVertices = oGraph.Vertices;
            IEdgeCollection oEdges = oGraph.Edges;

            // Nuke the Display
            UniverseBackground.Children.Clear();
            // Add the Whiskers Programmatically
            Add_Whiskers();
                                    
            SQL = "select id, nodename,x,y from nodes where networkid = '" + netid + "'";
            OleDbCommand aCommand = new OleDbCommand(SQL, localengine.RepositoryConnection);
            try
            {
                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Iterate throuth the database
                while (aReader.Read())
                {
                    NodeID = aReader.GetInt32(0);
                    NodeName = aReader.GetString(1);

                    ListBoxItem li = new ListBoxItem();
                    li.Content = NodeName;
                    li.Tag = NodeID.ToString();
                    NodeList.Items.Add(li); 
                    px = (double)aReader.GetInt32(2);
                    py = (double)aReader.GetInt32(3);
                    
                    

                    IVertex oVertexA = oVertices.Add();
                    oVertexA.Name = NodeName;
                    oVertexA.Tag = NodeID;

                }
                aReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
 
            }

            // Connect the edges
            SQL = "select edgeid, fromid, toid from edges where networkid='" + netid + "'";
            aCommandEdge = new OleDbCommand(SQL, localengine.RepositoryConnection);
            
            try
            {
                //create the datareader object to connect to table
                aReaderEdge = aCommandEdge.ExecuteReader();

                //Iterate throuth the database
                while (aReaderEdge.Read())
                {
                    EdgeID = aReaderEdge.GetInt32(0);
                    FromID = aReaderEdge.GetInt32(1);
                    ToID = aReaderEdge.GetInt32(2);

                    xx = Getx(FromID);
                    yy = Gety(FromID);

                    xx2 = Getx(ToID);
                    yy2 = Gety(ToID);

                    Line l = new Line();
                    l.X1 = xx+5;
                    l.Y1 = yy+5;
                    l.X2 = xx2+5;
                    l.Y2 = yy2+5;
                    l.Tag = "EDGE(" + FromID.ToString() + ":" + ToID.ToString() + ")";
                    l.ToolTip = l.Tag;
                    l.Stroke = new SolidColorBrush(Colors.Gray);
                    UniverseBackground.Children.Add(l);

                    IVertex oFrom=null;
                    IVertex oTo=null;

                    foreach (IVertex oVertex in oVertices)
                    {
                        if (oVertex.Tag.ToString() == FromID.ToString())
                        {
                            oFrom = oVertex;
                        }

                        if (oVertex.Tag.ToString() == ToID.ToString())
                        {
                            oTo = oVertex;

                        }

                         
                    }
                    IEdge oEdge1 = oEdges.Add(oFrom, oTo, true);

                }
                //aReaderEdge.Close();
            }
            catch (Exception eEdge)
            {
                MessageBox.Show(eEdge.Message);

            }

            // Apply Layout 
            // ==================================================================
            
            switch(layoutmethod)
            {
                case "Circular Layout":
                    ILayout oLayout_cir = new CircleLayout();
                    LayoutContext oLayoutContext_cir = new LayoutContext(new System.Drawing.Rectangle(0, 0, 5000, 5000));
                    oLayout_cir.LayOutGraph(oGraph, oLayoutContext_cir);
                break;
                case "Random Layout":
                    ILayout oLayout_rand = new RandomLayout();
                    LayoutContext oLayoutContext_rand = new LayoutContext(new System.Drawing.Rectangle(0, 0, 5000, 5000));
                    oLayout_rand.LayOutGraph(oGraph, oLayoutContext_rand);
                break;
                case "Sugiyama Layout":
                    ILayout oLayout_Sugi = new SugiyamaLayout();
                    LayoutContext oLayoutContext_Sugi = new LayoutContext(new System.Drawing.Rectangle(0, 0, 5000, 5000));
                    oLayout_Sugi.LayOutGraph(oGraph, oLayoutContext_Sugi);
                break;
                case "Grid Layout":
                    ILayout oLayout_grid = new GridLayout();
                    LayoutContext oLayoutContext_grid = new LayoutContext(new System.Drawing.Rectangle(0, 0, 5000, 5000));
                    oLayout_grid.LayOutGraph(oGraph, oLayoutContext_grid);
                break;
                case "Spiral Layout":
                    ILayout oLayout_spiral = new SpiralLayout();
                    LayoutContext oLayoutContext_spiral = new LayoutContext(new System.Drawing.Rectangle(0, 0, 5000, 5000));
                    oLayout_spiral.LayOutGraph(oGraph, oLayoutContext_spiral);
                break;
                case "Fruchterman/Reingold Layout":
                    ILayout oLayout_Fruch = new FruchtermanReingoldLayout();
                    LayoutContext oLayoutContext_Fruch = new LayoutContext(new System.Drawing.Rectangle(0, 0, 5000, 5000));
                    oLayout_Fruch.LayOutGraph(oGraph, oLayoutContext_Fruch);
                break;
                case "Sinusoid H Layout":
                    ILayout oLayout_SinH = new SinusoidHorizontalLayout();
                    LayoutContext oLayoutContext_SinH = new LayoutContext(new System.Drawing.Rectangle(0, 0, 5000, 5000));
                    oLayout_SinH.LayOutGraph(oGraph, oLayoutContext_SinH);
                break;
                case "Sinusoid V Layout":
                    ILayout oLayout_SinV = new SinusoidVerticalLayout();
                    LayoutContext oLayoutContext_SinV = new LayoutContext(new System.Drawing.Rectangle(0, 0, 500, 500));
                    oLayout_SinV.LayOutGraph(oGraph, oLayoutContext_SinV);
                break;

            }
           
            // List the results.
            int xoffset=0;
            int yoffset=0;
            Random rc = new Random();
            Random rx = new Random();
            Random ry = new Random();
            Random coin = new Random();
            foreach (IVertex oVertex in oVertices)
            {
                UniversePadPoint p2 = new UniversePadPoint();

                p2.PadName = oVertex.Name;
                xx2 = oVertex.Location.X;
                yy2 = oVertex.Location.Y;
                xx2 = xx2 + xoffset;
                yy2 = yy2 + yoffset;
            // BUG   p2.WhiskX = XWhisker;
            // BUG    p2.WhiskY = YWhisker;

                size = (double)10;
                p2.Render(UniverseBackground, FieldBackground, xx2, yy2, size, (int)oVertex.Tag, oVertex.Name);

                Ellipse re = new Ellipse();
                re.Width = rnd.NextDouble() * 80;
                re.Height = re.Width;
                re.Opacity = 0.25;
                re.Tag = "METRIC";
                re.Fill = new SolidColorBrush(Colors.Blue);
                UniverseBackground.Children.Add(re);
                Canvas.SetLeft(re, xx2-(re.Width/2)+5);
                Canvas.SetTop(re, yy2 - (re.Width / 2)+5);

                                

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

                Line l = new Line();
                l.X1 = xx + 5;
                l.Y1 = yy + 5;
                l.X2 = xx2 + 5;
                l.Y2 = yy2 + 5;
                l.Tag = "EDGE(" + f1.Tag.ToString() + ":" + t2.Tag.ToString() + ")";
                l.ToolTip = l.Tag;
                l.Stroke = new SolidColorBrush(Colors.Gray);
                UniverseBackground.Children.Add(l);
            }

            

        }

        private void Draw_Universe(string netid)
        {
            string SQL;
            string NodeName;
            int NodeID, FromID, ToID, EdgeID;
            double xx2, yy2, size, xx, yy;
            double px, py;
            int comments;
            Random rnd = new Random();

            Graph oGraph = new Graph(GraphDirectedness.Directed);
            IVertexCollection oVertices = oGraph.Vertices;
            IEdgeCollection oEdges = oGraph.Edges;

            // Nuke the Display
            UniverseBackground.Children.Clear();
            // Add the Whiskers Programmatically
            Add_Whiskers();

            SQL = "select id, nodename,x,y from nodes where networkid = '" + netid + "'";
            OleDbCommand aCommand = new OleDbCommand(SQL, localengine.RepositoryConnection);
            try
            {
                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Iterate throuth the database
                while (aReader.Read())
                {
                    NodeID = aReader.GetInt32(0);
                    NodeName = aReader.GetString(1);

                    ListBoxItem li = new ListBoxItem();
                    li.Content = NodeName;
                    li.Tag = NodeID.ToString();
                    NodeList.Items.Add(li);
                    px = (double)aReader.GetInt32(2);
                    py = (double)aReader.GetInt32(3);

                    UniversePadPoint p2 = new UniversePadPoint();

                    p2.PadName = NodeName;
                    xx2 = px;
                    yy2 = py;
                    xx2 = xx2 + 0;
                    yy2 = yy2 + 0;
                    // BUG   p2.WhiskX = XWhisker;
                    // BUG    p2.WhiskY = YWhisker;

                    size = (double)10;
                    p2.SetDrillFeature(DrillDetailLabel, DrillDetailList, localengine.RepositoryConnection);
                    p2.Render(UniverseBackground, FieldBackground, xx2, yy2, size, (int)NodeID, NodeName);

                    Ellipse re = new Ellipse();
                    re.Width = rnd.NextDouble() * 80;
                    re.Height = re.Width;
                    re.Opacity = 0.25;
                    re.Tag = "METRIC";
                    re.Fill = new SolidColorBrush(Colors.Blue);
                    UniverseBackground.Children.Add(re);
                    Canvas.SetLeft(re, xx2 - (re.Width / 2) + 5);
                    Canvas.SetTop(re, yy2 - (re.Width / 2) + 5);


                }
                aReader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }

            // Connect the edges
            SQL = "SELECT e.edgeid, e.fromid, n1.x, n1.y, e.toid, n2.x, n2.y ";
            SQL = SQL + "FROM Edges e, Nodes n1, Nodes n2 ";
            SQL = SQL + "where e.networkid = '" + netid + "' and n1.networkid = '" + netid + "' and n2.networkid = '" + netid + "' ";
            SQL = SQL + " and n1.id = e.fromid and n2.id = e.toid "; 
            
            aCommandEdge = new OleDbCommand(SQL, localengine.RepositoryConnection);

            try
            {
                //create the datareader object to connect to table
                aReaderEdge = aCommandEdge.ExecuteReader();

                //Iterate throuth the database
                while (aReaderEdge.Read())
                {
                    EdgeID = aReaderEdge.GetInt32(0);
                    FromID = aReaderEdge.GetInt32(1);
                    ToID = aReaderEdge.GetInt32(4);

                    xx = aReaderEdge.GetInt32(2);
                    yy = aReaderEdge.GetInt32(3);

                    xx2 = aReaderEdge.GetInt32(5);
                    yy2 = aReaderEdge.GetInt32(6);

                    Line l = new Line();
                    l.X1 = xx + 5;
                    l.Y1 = yy + 5;
                    l.X2 = xx2 + 5;
                    l.Y2 = yy2 + 5;
                    l.Tag = "EDGE(" + FromID.ToString() + ":" + ToID.ToString() + ")";
                    l.ToolTip = l.Tag;
                    l.Stroke = new SolidColorBrush(Colors.Gray);
                    UniverseBackground.Children.Add(l);
                                       

                }
                //aReaderEdge.Close();
            }
            catch (Exception eEdgedDraw)
            {
                MessageBox.Show(eEdgedDraw.Message);

            }

              
        }

        private void Add_Whiskers()
        {
           
            Line XWhisker = new Line();
            XWhisker.Stroke = new SolidColorBrush(Colors.Red);
            XWhisker.X1 = 5000;
            XWhisker.Y1 = 0;
            XWhisker.X2 = 5000;
            XWhisker.Y2 = 10000;
            UniverseBackground.Children.Add(XWhisker);

            Line YWhisker = new Line();
            YWhisker.Stroke = new SolidColorBrush(Colors.Red);
            XWhisker.X1 = 0;
            XWhisker.Y1 = 5000;
            XWhisker.X2 = 10000;
            XWhisker.Y2 = 5000;
            UniverseBackground.Children.Add(YWhisker);

        }

        private double Getx(int idlookup)
        {
            double result=-1;
            int loop;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                // childvisual
                if ((string)((System.Windows.FrameworkElement)(childVisual)).Tag == (string)idlookup.ToString())
                {
                    result = Canvas.GetLeft(childVisual);
                }
            }
            return (result);
        }

        private double Gety(int idlookup)
        {
            double result = -1;
            int loop;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                // childvisual
                if ((string)((System.Windows.FrameworkElement)(childVisual)).Tag == (string)idlookup.ToString())
                {
                    result = Canvas.GetTop(childVisual);
                }
            }
            return (result);
        }

        public void Build_Local_Network(int StartID)
        {
            LocalNet l = new LocalNet();
            l._FieldBackground = FieldBackground;
            l.Clear_Canvas();
            l.Draw_Local_Network(StartID);
                      
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


        }

        private void Throwaway_Click(object sender, RoutedEventArgs e)
        {
            int loop;
            double fromx, fromy;
            double tox, toy;
            int duration = 2000;
            Random r = new Random();
            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);
                
                // childvisual
                if ((string)((System.Windows.FrameworkElement)(childVisual)).Tag == "EDGE")
                {
                                        
                    fromy = 0.4;
                    toy = 0;
                    Animator.AnimatePenner(childVisual, OpacityProperty, Tween.Equations.Linear, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                    
                }
                else
                {
                    TranslateTransform tt = new TranslateTransform();
                    ((System.Windows.FrameworkElement)(childVisual)).LayoutTransform = tt;

                    fromx = Canvas.GetLeft(childVisual);
                    tox = r.NextDouble() * 1450;
                    fromy = Canvas.GetTop(childVisual);
                    toy = r.NextDouble() * 850;
                    Reconfigure_Edges(null,null);

                    Animator.AnimatePenner(childVisual, Canvas.TopProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, Reconfigure_Edges);
                    Animator.AnimatePenner(childVisual, Canvas.LeftProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, Reconfigure_Edges);
                }
                
            }
            
        }

        private void OnAnimationComplete(object sender, EventArgs e)
        {
            AnimationTimeline at = sender as AnimationTimeline;
            if (at != null)
            {
                Reconfigure_Edges(null,null);
                at.Completed -= OnAnimationComplete;

            }
            else 
            {
               // Reconfigure_Edges(null,null);
            }           
            

        }

        private void Reconfigure_Edges(object sender, EventArgs e)
        {
            int EdgeID, FromID, ToID;
            int colon;
            double xx, yy, xx2, yy2;
            string tag, tagfrom, tagto;
            int loop;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("EDGE"))
                {
                    tag = ((string)((System.Windows.FrameworkElement)(childVisual)).Tag);
                    colon = tag.IndexOf(":");
                    tagfrom = tag.Substring(5, (colon-5));
                    tagto = tag.Substring(colon+1);
                    tagto = tagto.Substring(0, (tagto.Length) - 1);

                    FromID = Convert.ToInt32(tagfrom);
                    ToID = Convert.ToInt32(tagto);

                    xx = Getx(FromID);
                    yy = Gety(FromID);

                    xx2 = Getx(ToID);
                    yy2 = Gety(ToID);

                    ((System.Windows.Shapes.Line)(childVisual)).X1 = xx;
                    ((System.Windows.Shapes.Line)(childVisual)).Y1 = yy;
                    ((System.Windows.Shapes.Line)(childVisual)).X2 = xx2;
                    ((System.Windows.Shapes.Line)(childVisual)).Y2 = yy2;
                

                }


            }

            
            
        }

        private BitmapSource CaptureScreen(Visual target)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(5000, 1000, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();

            try
            {


                using (DrawingContext ctx = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(target);

                    ctx.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
                }
                renderBitmap.Render(dv);
                
            }
            catch (Exception eOverviewFail)
            {

                MessageBox.Show(eOverviewFail.Message);

            }
            
            return renderBitmap;
        }

        private void Layout_Click(object sender, RoutedEventArgs e)
        {
            int loop, innerloop;
            double fromx, fromy, dfromx, dfromy;
            double tox, toy, dtox, dtoy;
            int duration = 100;

            double c1,c2,c3,c4;
            double deltax, deltay;
            double ox,dx,oy,dy;
            double newfx, newfy;
            double newofx, newofy;
            double distancesquared, distance;
            double repulsiveforce;

            // Starting conditions
            c1 = 1;
            c2 = 1;
            c3 = 25;
            c4 = 0.1;

            // Loop through all the pads and adjust positions
                        
            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);
                // Get this pad's positions
                ox = Canvas.GetLeft(childVisual);
                oy = Canvas.GetTop(childVisual);

                for (innerloop = 0; innerloop < UniverseBackground.Children.Count; innerloop++)
                {
                    UIElement dchildVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, innerloop);
                    Visual dmyvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, innerloop);

                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != (string)((System.Windows.FrameworkElement)(dchildVisual)).Tag))
                    {
                    // Get a comparison
                    dx = Canvas.GetLeft(dchildVisual);
                    dy = Canvas.GetTop(dchildVisual);
                    
                    // Figure out the distance between points
                    deltax = ox - dx;
                    deltay = oy - dy;

                    // Get the distance...
                    distancesquared = (deltax * deltax) + (deltay * deltay);
                    // Avoid division be zero or nodes flying off into space
                    if (distancesquared == 0) 
                    {
                        distancesquared = 1;
                    }
                    // get the distance by finding the square
                    distance = Math.Sqrt(distancesquared);

                    // Figure out the replusive force on each node
                    repulsiveforce = c3 / distancesquared;

                    // Inner Forces
                    newfx = dx + repulsiveforce * deltax / distance;
                    newfy = dy + repulsiveforce * deltay / distance;
                    
                    // Outer Forces - push away
                    newofx = ox - repulsiveforce * deltax / distance;
                    newofy = oy - repulsiveforce * deltay / distance;
                    
                    // Handle the outside Transform
                    TranslateTransform ott = new TranslateTransform();
                    ((System.Windows.FrameworkElement)(childVisual)).LayoutTransform = ott;

                    fromx = ox;
                    tox = newfx;
                    fromy = oy;
                    toy = newfy;
                    Animator.AnimatePenner(childVisual, Canvas.TopProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                    Animator.AnimatePenner(childVisual, Canvas.LeftProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);

                    // Handle the inside transform
                    TranslateTransform dtt = new TranslateTransform();
                    ((System.Windows.FrameworkElement)(dchildVisual)).LayoutTransform = dtt;

                    dfromx = dx;
                    dtox = newofx;
                    dfromy = dy;
                    dtoy = newofy;
                    Animator.AnimatePenner(dchildVisual, Canvas.TopProperty, Tween.Equations.CubicEaseIn, dfromy, dtoy, duration, OnAnimationComplete, OnAnimationComplete);
                    Animator.AnimatePenner(dchildVisual, Canvas.LeftProperty, Tween.Equations.CubicEaseIn, dfromx, dtox, duration, OnAnimationComplete, OnAnimationComplete);
                    }
                }
            }

        }

        private void TrackCursor(object sender, MouseEventArgs e)
        {
            if(SelectedNet !="")
            {
                Draw_Universe(SelectedNet);
            }
            double fromx, fromy;

            System.Windows.Point Rubberpoint = new System.Windows.Point();
            Rubberpoint = e.GetPosition(UniverseBackground);
            if (mode == "MOVE")
            {
                if (UniverseBackground.IsMouseCaptured)
                {
                    // Get the new mouse position. 
                    Point mouseDragCurrentPoint = e.GetPosition(this);

                    // Determine the new amount to scroll. 
                    Point delta = new Point(
                        (mouseDragCurrentPoint.X > mouseDragStartPoint.X) ?
                        -(mouseDragCurrentPoint.X - mouseDragStartPoint.X) :
                        (mouseDragStartPoint.X - mouseDragCurrentPoint.X),
                        (mouseDragCurrentPoint.Y > mouseDragStartPoint.Y) ?
                        -(mouseDragCurrentPoint.Y - mouseDragStartPoint.Y) :
                        (mouseDragStartPoint.Y - mouseDragCurrentPoint.Y));

                    // Scroll to the new position. 
                    fromx = CanvasX + delta.X;
                    if (fromx < 0) fromx = 0;
                    if (fromx > 20000) fromx = 20000;
                    fromy = CanvasY + delta.Y;
                    //fromy = scrollStartOffset.Y + delta.Y;
                    if (fromy < 0) fromy = 0;
                    if (fromy > 15000) fromy = 15000;
                    UniverseScrollViewer.ScrollToHorizontalOffset(fromx);
                    UniverseScrollViewer.ScrollToVerticalOffset(fromy);


                }
            }
            
            
            
            
            //    XWhisker.X1 = Rubberpoint.X;
        //    XWhisker.X2 = XWhisker.X1;
        //    YWhisker.Y1 = Rubberpoint.Y;
        //    YWhisker.Y2 = YWhisker.Y1;
        }

        private void TakePicture(object sender, RoutedEventArgs e)
        {
            double fromx, tox, fromy, toy;
            int duration = 500;

            Image i = new Image();
            i.Width = 100;
            i.Height = 100;
            UniverseBackground.Children.Add(i);
            Record_Thumb(i);

            fromx = 0;
            tox = 500;
            fromy = 0;
            toy = 800;

            Animator.AnimatePenner(i, Canvas.LeftProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);
            Animator.AnimatePenner(i, Canvas.TopProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
          


        }

        private void AdjustZoomLevel(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double zoom = (double)((System.Windows.Controls.Primitives.RangeBase)(sender)).Value;
            zoom = zoom * 0.10;
            ScaleTransform st = new ScaleTransform();
            st.ScaleX = zoom;
            st.ScaleY = zoom;
      //      st.CenterX = XWhisker.X1;
      //      st.CenterY = YWhisker.Y1;
            UniverseBackground.RenderTransform = st;

        }

        private void MovetoSelectedNode(object sender, SelectionChangedEventArgs e)
        {
            string selectname;
            string selectid;

            double fromx, tox, fromy, toy;
            int duration = 500;
            int travelID;

            System.Windows.Point Rubberpoint = new System.Windows.Point();

            selectname = ((System.Windows.Controls.ContentControl)(((System.Windows.Controls.Primitives.Selector)(sender)).SelectedItem)).Content.ToString();
            selectid = ((System.Windows.FrameworkElement)(((System.Windows.Controls.Primitives.Selector)(sender)).SelectedItem)).Tag.ToString();
            
            int loop;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("EDGE")) 
                        {
                        }
                    else
                    {
                        if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains(selectid))
                        {

                            Rubberpoint.X = Canvas.GetLeft(childVisual)+5;
                            Rubberpoint.Y = Canvas.GetTop(childVisual)+5;
                            
                          

                     // BUG      fromx = XWhisker.X1;
                     //       tox = Rubberpoint.X;
                     //       fromy = YWhisker.Y1;
                     //       toy = Rubberpoint.Y;

                     //       Animator.AnimatePenner(XWhisker, Line.X1Property, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);
                     //       Animator.AnimatePenner(XWhisker, Line.X2Property, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);
                     //       Animator.AnimatePenner(YWhisker, Line.Y1Property, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                     //       Animator.AnimatePenner(YWhisker, Line.Y2Property, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);

                            // Get the Tag of the selection Pt - this is the index from the database for the node...
                            if (((System.Windows.FrameworkElement)(childVisual)).Tag != null)
                            {
                                travelID = Convert.ToInt32(((System.Windows.FrameworkElement)(childVisual)).Tag);
                                LocalNet l = new LocalNet();
                                l._FieldBackground = FieldBackground;
                                l.Clear_Canvas();
                                l.Draw_Local_Network(travelID);
                            }
                        


                        }
                    }
                }


            }

        }

        private void HandleNodeRender(object sender, RoutedEventArgs e)
        {
            double op;       
            int loop;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("NODE"))
                    {
                    
                        if ((bool)((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked==true)
                        {
                            op = (double)(NodeOpacity.Value);
                            op = op * 0.01;
                            childVisual.Opacity = op;
                            
                        }
                        else
                        {
                            childVisual.Opacity = 0;
                        }
                    }
                }
            }

        }

        private void NodeOpacityChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int loop;
            double op;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("NODE"))
                    {
                                            
                        op = (double)((System.Windows.Controls.Primitives.RangeBase)(sender)).Value;
                        op = op * 0.01;
                        childVisual.Opacity = op;
                    }
                    
                }
            }

        }

        private void ShowColorPicker(object sender, RoutedEventArgs e)
        {
            ColorPick c = new ColorPick();
            c.Show();

        }

        private void HandleEdgeRender(object sender, RoutedEventArgs e)
        {
            double op;
            int loop;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("EDGE"))
                    {
                        if ((bool)((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked == true)
                        {
                            op = (double)(NodeOpacity.Value);
                            op = op * 0.01;
                            childVisual.Opacity = op;

                        }
                        else
                        {
                            childVisual.Opacity = 0;
                        }
                    }
                  
                }
            }

        }

        private void EdgeOpacityChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int loop;
            double op;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("EDGE"))
                    {
                        op = (double)((System.Windows.Controls.Primitives.RangeBase)(sender)).Value;
                        op = op * 0.01;
                        childVisual.Opacity = op;

                    }
                    else
                    {
                       
                    }

                }
            }

        }

        private void HandleLabelRender(object sender, RoutedEventArgs e)
        {
            double op;
            int loop;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("LABEL"))
                    {
                        if ((bool)((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked == true)
                        {
                            op = (double)(NodeOpacity.Value);
                            op = op * 0.01;
                            childVisual.Opacity = op;

                        }
                        else
                        {
                            childVisual.Opacity = 0;
                        }
                    }

                }
            }

        }

        private void LabelOpacityChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int loop;
            double op;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("LABEL"))
                    {
                        op = (double)((System.Windows.Controls.Primitives.RangeBase)(sender)).Value;
                        op = op * 0.01;
                        childVisual.Opacity = op;

                    }
                    else
                    {

                    }

                }
            }

        }

        private void MetricOpacityChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int loop;
            double op;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("METRIC"))
                    {
                        op = (double)((System.Windows.Controls.Primitives.RangeBase)(sender)).Value;
                        op = op * 0.01;
                        childVisual.Opacity = op;

                    }
                    else
                    {

                    }

                }
            }


        }

        private void HandleMetricRender(object sender, RoutedEventArgs e)
        {
            double op;
            int loop;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("METRIC"))
                    {
                        if ((bool)((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked == true)
                        {
                            op = (double)(NodeOpacity.Value);
                            op = op * 0.01;
                            childVisual.Opacity = op;

                        }
                        else
                        {
                            childVisual.Opacity = 0;
                        }
                    }

                }
            }

        }

        private void SetInfluence(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int min, max, loop;
            string tag;
            min = Convert.ToInt32(MinEdges.Value);
            max = Convert.ToInt32(MaxEdges.Value);

            // Look through all the nodes and count the connections - if it falls in the range keep it
            // Otherwise, hide it...
            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag.ToString() != "WHISKER"))
                    {
                        if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("EDGE"))
                        {
                        }
                        else
                        {
                            if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("LABEL"))
                            {
                            }
                            else
                            {
                                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("METRIC"))
                                {
                                }
                                else
                                {
                                    tag = ((string)((System.Windows.FrameworkElement)(childVisual)).Tag);
                                    if ((numberofedges(Convert.ToInt32(tag)) >= min) && (numberofedges(Convert.ToInt32(tag)) <= max))
                                    {
                                        childVisual.Opacity = 1;

                                    }
                                    else
                                    {
                                        childVisual.Opacity = 0.1;
                                    }

                                }

                            }

                        }

                    }
                }

            }
        }

        private int numberofedges(int id)
        {
            int result = 0;
            int loop;
            
            int colon;
            string tag;
            string tagfrom, tagto;
            int FromID;
            int ToID;

            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("EDGE"))
                    {

                        tag = ((string)((System.Windows.FrameworkElement)(childVisual)).Tag);
                        colon = tag.IndexOf(":");
                        tagfrom = tag.Substring(5, (colon - 5));
                        tagto = tag.Substring(colon + 1);
                        tagto = tagto.Substring(0, (tagto.Length) - 1);

                        FromID = Convert.ToInt32(tagfrom);
                        ToID = Convert.ToInt32(tagto);

                        if (id == FromID)
                        {
                            result++;
                        }
                        if (id == ToID)
                        {
                            result++;
                        }

                    }

                }

            } // end of loop
            return (result);
        }

        private void HandleCommentRender(object sender, RoutedEventArgs e)
        {

        }

        private void CommentOpacityChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void CheckLayout(object sender, RoutedEventArgs e)
        {
            // Create a graph.  The graph has no visual representation --
            // it is just a data structure.

            Graph oGraph = new Graph(GraphDirectedness.Directed);
            IVertexCollection oVertices = oGraph.Vertices;
            IEdgeCollection oEdges = oGraph.Edges;

            // Add three vertices.

            IVertex oVertexA = oVertices.Add();
            oVertexA.Name = "Vertex A";
            IVertex oVertexB = oVertices.Add();
            oVertexB.Name = "Vertex B";
            IVertex oVertexC = oVertices.Add();
            oVertexC.Name = "Vertex C";

            // Connect the vertices with directed edges.

            IEdge oEdge1 = oEdges.Add(oVertexA, oVertexB, true);
            IEdge oEdge2 = oEdges.Add(oVertexB, oVertexC, true);
            IEdge oEdge3 = oEdges.Add(oVertexC, oVertexA, true);

            // Lay out the graph within a 100x100 rectangle.  This sets
            // the IVertex.Location property of each vertex.

            ILayout oLayout = new FruchtermanReingoldLayout();
            
            LayoutContext oLayoutContext = new LayoutContext(new System.Drawing.Rectangle(0,0,100,100));

            oLayout.LayOutGraph(oGraph, oLayoutContext);

            // List the results.

            foreach (IVertex oVertex in oVertices)
            {
                MessageBox.Show("The location of " + oVertex.Name + " is " + oVertex.Location.ToString());
                    
            }
        }

        private void ChangeLayout(object sender, SelectionChangedEventArgs e)
        {
            string layoutmethod;
            layoutmethod = ((System.Windows.Controls.ContentControl)(((System.Windows.Controls.Primitives.Selector)(LayoutMethod)).SelectedValue)).Content.ToString();
            Build_Universe_Network(layoutmethod, OpenNet);

        }

        private void ExpandRetractNetworkPanel(object sender, RoutedEventArgs e)
        {
            double fromy, toy;
            int duration = 500;

            if (NetworksOpen == false)
            {
                fromy = 0;
                toy = 500;
                Animator.AnimatePenner(ExpandNetworkPanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                NetworksOpen = true;
            }
            else
            {
                fromy = 500;
                toy = 0;
                Animator.AnimatePenner(ExpandNetworkPanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                NetworksOpen = false;
            }
            
        }

        private void ExpandRetractAppearancePanel(object sender, RoutedEventArgs e)
        {
            double fromy, toy;
            int duration = 500;

            if (AppearanceOpen == false)
            {
                fromy = 0;
                toy = 500;
                Animator.AnimatePenner(ExpandAppearancePanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                AppearanceOpen = true;
            }
            else
            {
                fromy = 500;
                toy = 0;
                Animator.AnimatePenner(ExpandAppearancePanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                AppearanceOpen = false;
            }

        }

        private void ExpandRetractExplorePanel(object sender, RoutedEventArgs e)
        {
            double fromy, toy;
            int duration = 500;

            if (ExploreOpen == false)
            {
                fromy = 0;
                toy = 500;
                Animator.AnimatePenner(ExpandExplorePanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                ExploreOpen = true;
            }
            else
            {
                fromy = 500;
                toy = 0;
                Animator.AnimatePenner(ExpandExplorePanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                ExploreOpen = false;
            }
        }

        private void ExpandRetractDrilLDownPanel(object sender, RoutedEventArgs e)
        {
            double fromy, toy;
            int duration = 500;

            if (DrillDownOpen == false)
            {
                fromy = 0;
                toy = 500;
                Animator.AnimatePenner(ExpandDrillDownPanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                DrillDownOpen = true;
            }
            else
            {
                fromy = 500;
                toy = 0;
                Animator.AnimatePenner(ExpandDrillDownPanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                DrillDownOpen = false;
            }
        }

        private void ExpandRetractFilterPanel(object sender, RoutedEventArgs e)
        {
            double fromy, toy;
            int duration = 500;

            if (FilterOpen == false)
            {
                fromy = 0;
                toy = 500;
                Animator.AnimatePenner(ExpandFilterPanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                FilterOpen = true;
            }
            else
            {
                fromy = 500;
                toy = 0;
                Animator.AnimatePenner(ExpandFilterPanel, StackPanel.HeightProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                FilterOpen = false;
            }

        }

        private void OpenNetworkDialog(object sender, RoutedEventArgs e)
        {
            Networkmanager n = new Networkmanager();
            
            n.Show();
            //MessageBox.Show(n.SelectedNetID);
            //n.Close();
            //20100805

            if (n.SelectedNetID != "NONE")
            {
                Draw_Universe(n.SelectedNetID);
            }
            
            //20100805

        }

        private void HideNonInfluence(object sender, RoutedEventArgs e)
        {
            int min, max, loop;
            string tag;
            min = 0;
           

            // Look through all the nodes and count the connections - if it falls in the range keep it
            // Otherwise, hide it...
            for (loop = 0; loop < UniverseBackground.Children.Count; loop++)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(UniverseBackground, loop);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(UniverseBackground, loop);

                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag != null))
                {
                    if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag.ToString() != "WHISKER"))
                    {
                        if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("EDGE"))
                        {
                        }
                        else
                        {
                            if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("LABEL"))
                            {
                            }
                            else
                            {
                                if (((string)((System.Windows.FrameworkElement)(childVisual)).Tag).Contains("METRIC"))
                                {
                                }
                                else
                                {
                                    tag = ((string)((System.Windows.FrameworkElement)(childVisual)).Tag);
                                    if (numberofedges(Convert.ToInt32(tag)) == min) 
                                    {
                                        childVisual.Opacity = 0;

                                    }
                                    else
                                    {
                                        childVisual.Opacity = 1;
                                    }

                                }

                            }

                        }

                    }
                }

            }
        }

        private void OnLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDragStartPoint = e.GetPosition(UniverseBackground);
            scrollStartOffset.X = CanvasX;
            scrollStartOffset.Y = CanvasY;

            // Update the cursor if scrolling is possible 
            UniverseBackground.Cursor = (UniverseScrollViewer.ExtentWidth > UniverseScrollViewer.ViewportWidth) ||
            (UniverseScrollViewer.ExtentHeight > UniverseScrollViewer.ViewportHeight) ?
             Cursors.ScrollAll : Cursors.Arrow;

            UniverseBackground.CaptureMouse();
            UniverseBackground.Cursor = Cursors.ScrollAll;
        }

        private void OnLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (UniverseBackground.IsMouseCaptured)
            {
                UniverseBackground.Cursor = Cursors.Arrow;
                UniverseBackground.ReleaseMouseCapture();

            }
        }

    }
}
