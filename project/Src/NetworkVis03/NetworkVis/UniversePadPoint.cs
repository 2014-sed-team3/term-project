using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows;
//using System.Data.OleDb;
using MySql.Data.MySqlClient;

namespace NetworkVis
{
    class UniversePadPoint
    {
        public Canvas PadParent;
        public Canvas ChildCanvas;
        public Rectangle Padback;
        public long PadID;
        public string PadName;
        public double xpos, ypos;
        public RotateTransform rt;
        public Line WhiskX;
        public Line WhiskY;
        public Label nodelabel = new Label();
        public Label DrillLabel;
        public ListBox DrillList;
        //public OleDbConnection local;
        //20140525
        public MySqlConnection local;
        //20140525

        //public void SetDrillFeature(Label l, ListBox l2, OleDbConnection l3)
        public void SetDrillFeature(Label l, ListBox l2, MySqlConnection l3)
        {
            DrillLabel = l;
            DrillList = l2;
            local = l3;
        }

        public void Render(Canvas _parent, Canvas _childCanvas, double xx, double yy, double padsize, int tagid, string name)
        {
            PadParent = _parent;
            ChildCanvas = _childCanvas;
            PadName = name;
            xpos = xx;
            ypos = yy;
            Padback = new Rectangle();
            ContextMenu cm = new ContextMenu();
            MenuItem mi = new MenuItem();
            mi.Header = "Add Comment...";
            mi.Click += new RoutedEventHandler(AddComment);
            cm.Items.Add(mi);
            MenuItem mi2 = new MenuItem();
            mi2.Header = "Add Link...";
            mi2.Click += new RoutedEventHandler(AddLink);
            cm.Items.Add(mi2);
            Padback.ContextMenu = cm;
            Padback.ToolTip = name;
            Padback.MouseEnter += new MouseEventHandler(Padback_Grow);
            Padback.MouseLeave += new MouseEventHandler(Padback_Shrink);
            Padback.MouseDown += new MouseButtonEventHandler(Padback_Selection);
            Padback.Fill = new SolidColorBrush(Colors.Blue);
            Padback.Width = padsize;
            Padback.Height = padsize;
            Padback.RadiusX = 2.5;
            Padback.RadiusY = 2.5;
            Padback.Stroke = new SolidColorBrush(Colors.Gray);
            Padback.StrokeThickness = 1;
            Padback.Tag = tagid.ToString();
            _parent.Children.Add(Padback);
            Canvas.SetLeft(Padback, xx);
            Canvas.SetTop(Padback, yy);
            Canvas.SetZIndex(Padback, 100);

            nodelabel.Content = name;
            nodelabel.Tag = "LABEL";
            nodelabel.Foreground = new SolidColorBrush(Colors.White);
            _parent.Children.Add(nodelabel);
            Canvas.SetLeft(nodelabel, xx-6);
            Canvas.SetTop(nodelabel, yy + 8);
            Canvas.SetZIndex(nodelabel, 100);


        }

        private void AddComment(object sender, RoutedEventArgs e)
        {
            double x, y;
            string textmsg = "Add a Comment here";

            x = 2000;
            y = 2000;
            Callouts c = new Callouts();
            c.type = "Comment";
            c.xpos = x;
            c.ypos = y;
            c.calloutText = textmsg;
            c.Render(PadParent);

        }

        private void AddLink(object sender, RoutedEventArgs e)
        {
            double x, y;
            string textmsg = "http://www.astrazeneca.com";

            x = 2000;
            y = 2000;
            Callouts c = new Callouts();
            c.type = "Link";
            c.xpos = x;
            c.ypos = y;
            c.calloutText = textmsg;
            c.Render(PadParent);

        }

        private void OnAnimationComplete(object sender, EventArgs e)
        {
            AnimationTimeline at = sender as AnimationTimeline;
            if (at != null)
                at.Completed -= OnAnimationComplete;

        }

        private void ReloadNetwork(object sender, EventArgs e)
        {
            LocalNet l = new LocalNet();
            l._FieldBackground = PadParent;
            l.Clear_Canvas();
            l.Draw_Local_Network(1);
        }

        private bool bTestNode(System.Windows.Point Rubberpoint, UIElement childVisual)
        {
            bool result = false;

            if ((Canvas.GetTop(childVisual)-12 <= Rubberpoint.Y) && (Canvas.GetTop(childVisual)+12 >= Rubberpoint.Y))
            {
                if ((Canvas.GetLeft(childVisual)-12 <= Rubberpoint.X) && (Canvas.GetLeft(childVisual) + 12 >= Rubberpoint.X))
                {
                    result = true;
                    
                }
            }
            return(result);
        }

        private int GetChildIndexClickedOn(System.Windows.Point clickpt)
        {
            int result = -1;
            int loop;
            for (loop = 0; loop < PadParent.Children.Count-1; loop++)
            {
                 UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(PadParent, loop);
                 Visual myvisual = (Visual)VisualTreeHelper.GetChild(PadParent, loop);
                 if (bTestNode(clickpt, childVisual))
                 {
                     result = loop;
                     break;

                 }
                 
            }
            

            return (result);
        }
        
        public void Padback_Selection(object sender, MouseButtonEventArgs e)
        {
            double fromx, fromy;
            double tox, toy;
            int duration = 500;
            int childindex;
            int travelID;
            string tagvalue;
            System.Windows.Point Rubberpoint = new System.Windows.Point();
            if (e.ClickCount == 2)
            {
                Rubberpoint = e.GetPosition(PadParent);
                childindex = GetChildIndexClickedOn(Rubberpoint);
                if (childindex != -1)
                {
                    UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(PadParent, childindex);
                    Visual myvisual = (Visual)VisualTreeHelper.GetChild(PadParent, childindex);
                    
                    tagvalue = ((System.Windows.FrameworkElement)(sender)).Tag.ToString();
                    DrillLabel.Content = "Values for:" + tagvalue.ToString();
                    ListBoxItem lbi = new ListBoxItem();
                                       
                    lbi.Content = tagvalue;
                    DrillList.Items.Add(lbi);

                    Drill_Into_Pad(tagvalue);
            //   BUG     fromx = WhiskX.X1;
            //        tox = Rubberpoint.X;
            //        fromy = WhiskY.Y1;
            //        toy = Rubberpoint.Y;

//                    Animator.AnimatePenner(WhiskX, Line.X1Property, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);
//                    Animator.AnimatePenner(WhiskX, Line.X2Property, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);
//                    Animator.AnimatePenner(WhiskY, Line.Y1Property, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
//                    Animator.AnimatePenner(WhiskY, Line.Y2Property, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);

                    // Get the Tag of the selection Pt - this is the index from the database for the node...
                    if (((System.Windows.FrameworkElement)(childVisual)).Tag != null)
                    {
                        if (((System.Windows.FrameworkElement)(childVisual)).Tag.ToString() != "METRIC")
                        {
                            travelID = Convert.ToInt32(((System.Windows.FrameworkElement)(childVisual)).Tag);
                            LocalNet l = new LocalNet();
                            l._FieldBackground = ChildCanvas;
                            l.Clear_Canvas();
                            l.Draw_Local_Network(travelID);
                        }
                    }
                    
                }
            }
       
        }

        private void Drill_Into_Pad(string idvalue)
        {
            int fieldcount;
            string temp;
            string SQL = "SELECT * from Nodes where id = " + idvalue.ToString();

            //OleDbCommand aCommand = new OleDbCommand(SQL, local);
            //20140525
            MySqlCommand aCommand = new MySqlCommand(SQL, local);
            aCommand.CommandTimeout = 12000;
            //20140525
            try
            {
                //create the datareader object to connect to table
                //OleDbDataReader aReader = aCommand.ExecuteReader();
                MySqlDataReader aReader = aCommand.ExecuteReader();

                DrillList.Items.Clear();
                //Iterate throuth the database
                while (aReader.Read())
                {
                    for (fieldcount = 0; fieldcount <= aReader.FieldCount - 1; fieldcount++)
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        temp = aReader.GetName(fieldcount);
                        temp = temp + " : " + Convert.ToString(aReader.GetValue(fieldcount));
                        lbi.Content = temp;
                        DrillList.Items.Add(lbi);
                    }
                    
                }
                aReader.Close();
            }
            catch (Exception eDrill)
            {
                MessageBox.Show("Unable to Drill into values for " + idvalue.ToString());
                MessageBox.Show(eDrill.Message);

            }

 
        }

        public void Padback_Grow(object sender, MouseEventArgs e)
        {
            double fromy;
            double toy;
            int duration = 300;

            fromy = 1;
            toy = 3.5;

            Animator.AnimatePenner(Padback, Rectangle.StrokeThicknessProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
            
        }

        public void Padback_Shrink(object sender, MouseEventArgs e)
        {
            double fromy;
            double toy;
            int duration = 300;

            fromy = 3.5;
            toy = 1;

            Animator.AnimatePenner(Padback, Rectangle.StrokeThicknessProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
            
        }

        public void Apply_Transform(RotateTransform rt)
        {
            Padback.LayoutTransform = rt;
        }
    }
}
