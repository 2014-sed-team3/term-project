using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
//using System.Data.OleDb;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

using MySql.Data.MySqlClient;

namespace NetworkVis
{
    class LocalNet
    {
        public Canvas _FieldBackground;
        private int StartID;

        private void CreateNew(Canvas back)
        {
            _FieldBackground = back;
        }

        public void Clear_Canvas()
        {
            int loop;
            loop = _FieldBackground.Children.Count;
            while (loop > 0)
            {
                UIElement childVisual = (UIElement)VisualTreeHelper.GetChild(_FieldBackground, 0);
                Visual myvisual = (Visual)VisualTreeHelper.GetChild(_FieldBackground, 0);
                
                _FieldBackground.Children.Remove(childVisual);
                loop = _FieldBackground.Children.Count;
            }

        }

        public void Draw_Local_Network(int StartID)
        {
            string SQL, SQLOuter;
            string thisnodename;
            int node_id;
            //OleDbCommand aLocal;
            //OleDbCommand aLocalOuter;
            //OleDbDataReader aReaderLocal;
            //OleDbDataReader aReaderOuter;
            MySqlCommand aLocal;
            MySqlCommand aLocalOuter;
            MySqlDataReader aReaderLocal;
            MySqlDataReader aReaderOuter;

            int rowcount = 0;

            double looper = 0;
            double xx2, yy2;
            double cx, cy;

            double size;
            double r;
            cx = 725;
            cy = 475;
            r = 33 * 10;
            double inc;

            double fromx, tox, fromy, toy;
            int duration = 1000;

            Repository localengine = new Repository();

            localengine.Open_Repository();

            // Draw the Central Point...
            PadPoint p = new PadPoint();
            p.Render(_FieldBackground, 725, 475, 20, StartID, "CenterName");

            Label nodecenter = new Label();
            nodecenter.Content = "Center";
            nodecenter.Foreground = new SolidColorBrush(Colors.White);
            nodecenter.FontSize = 12;
            _FieldBackground.Children.Add(nodecenter);
            Canvas.SetLeft(nodecenter, 735);
            Canvas.SetTop(nodecenter, 510);

            // How many records...
            SQL = "select n.ID, n.NodeName from edges e, nodes n where e.FromID = " + StartID.ToString() + " and e.ToID = n.ID";
            //aLocal = new OleDbCommand(SQL, localengine.RepositoryConnection);
            ////create the datareader object to connect to table
            //aReaderLocal = aLocal.ExecuteReader();
            //20140525, modify to mysql
            aLocal = new MySqlCommand(SQL, localengine.RepositoryConnection);
            aLocal.CommandTimeout = 120000;
            aReaderLocal = aLocal.ExecuteReader();

            //Iterate throuth the database
            while (aReaderLocal.Read())
            {
                rowcount++;
            }
            aReaderLocal.Close();
   

            // Now, look backward the other way
            SQLOuter = "select n.ID, n.NodeName from edges e, nodes n where e.ToID = " + StartID.ToString() + " and e.FromID = n.ID";
            //aLocalOuter = new OleDbCommand(SQLOuter, localengine.RepositoryConnection);
            //aReaderOuter = aLocalOuter.ExecuteReader();
            //201405025
            aLocalOuter = new MySqlCommand(SQLOuter, localengine.RepositoryConnection);
            aLocalOuter.CommandTimeout =  120000;
            aReaderOuter = aLocalOuter.ExecuteReader();

            while (aReaderOuter.Read())
            {
                rowcount++;
            }
            aReaderOuter.Close();

            // Base the spacing on the circle with how many relationships we found...
            inc = (6.4 / (rowcount));

            try
            {
                //create the datareader object to connect to table
                aReaderLocal = aLocal.ExecuteReader();


                //Iterate throuth the database
                while (aReaderLocal.Read())
                {
                    node_id = aReaderLocal.GetInt32(0);
                    thisnodename = aReaderLocal.GetString(1);

                    PadPoint p2 = new PadPoint();
                    xx2 = r * Math.Cos(looper);
                    yy2 = r * Math.Sin(looper);
                    xx2 = xx2 + cx;
                    yy2 = yy2 + cy;
                    size = 10;
                    p2.Render(_FieldBackground, 725, 460, size, node_id, thisnodename);

                    fromx = 725;
                    tox = xx2;
                    fromy = 460;
                    toy = yy2;
                    Animator.AnimatePenner(p2.Padback, Canvas.TopProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                    Animator.AnimatePenner(p2.Padback, Canvas.LeftProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);

                    Label nodel = new Label();
                    nodel.Content = thisnodename;
                    nodel.Foreground = new SolidColorBrush(Colors.White);
                    nodel.FontSize = 12;
                    _FieldBackground.Children.Add(nodel);
                    fromx = 725;
                    tox = xx2 + (size / 2); ;
                    fromy = 460;
                    toy = yy2 + size +10;
                    Animator.AnimatePenner(nodel, Canvas.LeftProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);
                    Animator.AnimatePenner(nodel, Canvas.TopProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);


                    Line l = new Line();
                    l.X1 = 725 + 10;
                    l.Y1 = 475 + 10;
                    l.X2 = xx2 + (size / 2);
                    l.Y2 = yy2 + (size / 2);
                    l.Stroke = new SolidColorBrush(Colors.Wheat);
                    l.Opacity = 0.4;
                    l.Tag = "EDGE";
                    _FieldBackground.Children.Add(l);

                    fromx = 725;
                    tox = xx2 + (size / 2); ;
                    fromy = 460;
                    toy = yy2 + (size / 2); ;
                    Animator.AnimatePenner(l, Line.X2Property, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);
                    Animator.AnimatePenner(l, Line.Y2Property, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);

                    looper = looper + inc;

                }
                aReaderLocal.Close();
            }
            catch (Exception elocalOutside)
            {
                MessageBox.Show(elocalOutside.Message);

            }

            try
            {
                //create the datareader object to connect to table
                aReaderOuter = aLocalOuter.ExecuteReader();


                //Iterate throuth the database
                while (aReaderOuter.Read())
                {
                    node_id = aReaderOuter.GetInt32(0);
                    thisnodename = aReaderOuter.GetString(1);

                    PadPoint p2 = new PadPoint();
                    xx2 = r * Math.Cos(looper);
                    yy2 = r * Math.Sin(looper);
                    xx2 = xx2 + cx;
                    yy2 = yy2 + cy;
                    size = 10;
                    p2.Render(_FieldBackground, 725, 460, size, node_id, thisnodename);

                    fromx = 725;
                    tox = xx2;
                    fromy = 460;
                    toy = yy2;
                    Animator.AnimatePenner(p2.Padback, Canvas.TopProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
                    Animator.AnimatePenner(p2.Padback, Canvas.LeftProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);

                    Line l = new Line();
                    l.X1 = 725 + 10;
                    l.Y1 = 475 + 10;
                    l.X2 = xx2 + (size / 2);
                    l.Y2 = yy2 + (size / 2);
                    l.Stroke = new SolidColorBrush(Colors.Wheat);
                    l.Opacity = 0.4;
                    l.Tag = "EDGE";
                    _FieldBackground.Children.Add(l);
                    looper = looper + inc;

                }
                aReaderOuter.Close();
            }
            catch (Exception elocalOuter)
            {
                MessageBox.Show(elocalOuter.Message);

            }
        }

        private void OnAnimationComplete(object sender, EventArgs e)
        {
            AnimationTimeline at = sender as AnimationTimeline;
            if (at != null)
            {
                
                at.Completed -= OnAnimationComplete;

            }
           

        }
    }
}
