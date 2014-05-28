using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Input;
using System.Data.OleDb;

namespace NetworkVis
{
    class PadPoint
    {
        public Canvas PadParent;
        public Canvas PadBacking;
        public Rectangle Padback;
        public long PadID;
        public string PadName;
        public double xpos, ypos;
        public RotateTransform rt;

        public void Render(Canvas _parent, double xx, double yy, double padsize, int tagid, string name)
        {
            PadParent = _parent;
            PadName = name;
            PadID = tagid;
            xpos = xx;
            ypos = yy;
            Padback = new Rectangle();
            Padback.ToolTip = name;
            Padback.MouseEnter +=new MouseEventHandler(Padback_Grow);
            Padback.MouseLeave += new MouseEventHandler(Padback_Shrink);
            Padback.MouseDown +=new MouseButtonEventHandler(Padback_MoveToCenter);
            Padback.Fill = new SolidColorBrush(Colors.Blue);
            Padback.Width = padsize;
            Padback.Height = padsize;
            Padback.RadiusX = 5;
            Padback.RadiusY = 5;
            Padback.Stroke = new SolidColorBrush(Colors.Gray);
            Padback.StrokeThickness = 1;
            Padback.Tag = tagid.ToString();
            _parent.Children.Add(Padback);
            Canvas.SetLeft(Padback, xx);
            Canvas.SetTop(Padback, yy);
            Canvas.SetZIndex(Padback, 100);


        }

       

        private void OnAnimationComplete(object sender, EventArgs e)
        {
            AnimationTimeline at = sender as AnimationTimeline;
            if (at != null)
                at.Completed -= OnAnimationComplete;

        }

        private void ReloadNetwork(object sender, EventArgs e)
        {
            long GetID;
            GetID = PadID;
            LocalNet l = new LocalNet();
            l._FieldBackground = PadParent;
            l.Clear_Canvas();

            l.Draw_Local_Network((int)GetID);
        }

        public void Padback_MoveToCenter(object sender, MouseButtonEventArgs e)
        {
            double fromx, fromy;
            double tox, toy;
            int duration = 1000;

            TranslateTransform tt = new TranslateTransform();
            Padback.LayoutTransform = tt;
            fromx = Canvas.GetLeft(Padback);
            tox = 725;
            fromy = Canvas.GetTop(Padback);
            toy = 460;
            Animator.AnimatePenner(Padback, Canvas.TopProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, ReloadNetwork, ReloadNetwork);
            Animator.AnimatePenner(Padback, Canvas.LeftProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, ReloadNetwork, ReloadNetwork);

        }

        public void Padback_Grow(object sender, MouseEventArgs e)
        {
            double fromx, fromy;
            double tox, toy;
            int duration = 300;

            ScaleTransform st = new ScaleTransform();
            st.CenterX = 0;
            st.CenterY = 0;
            st.ScaleX = 1;
            st.ScaleY = 1;
            Padback.LayoutTransform = st;
            fromx = 1;
            tox = 1.5;
            fromy = 1;
            toy = 1.5;

            Animator.AnimatePenner(st, ScaleTransform.ScaleXProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
            Animator.AnimatePenner(st, ScaleTransform.ScaleYProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);

        }

        public void Padback_Shrink(object sender, MouseEventArgs e)
        {
            double fromx, fromy;
            double tox, toy;
            int duration = 500;

            ScaleTransform st = new ScaleTransform();
            st.CenterX = 0;
            st.CenterY = 0;
            st.ScaleX = 1;
            st.ScaleY = 1;
            Padback.LayoutTransform = st;
            fromx = 1.5;
            tox = 1.0;
            fromy = 1.5;
            toy = 1.0;

            Animator.AnimatePenner(st, ScaleTransform.ScaleXProperty, Tween.Equations.CubicEaseIn, fromy, toy, duration, OnAnimationComplete, OnAnimationComplete);
            Animator.AnimatePenner(st, ScaleTransform.ScaleYProperty, Tween.Equations.CubicEaseIn, fromx, tox, duration, OnAnimationComplete, OnAnimationComplete);

        }

        public void Apply_Transform(RotateTransform rt)
        {
            Padback.LayoutTransform = rt;
        }
    }
}
