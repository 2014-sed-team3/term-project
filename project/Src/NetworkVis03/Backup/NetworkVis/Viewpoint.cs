using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkVis
{
    class Viewpoint
    {
        public double x, y;
        public double x_extent, y_extent;
        public double scale_x;
        public double scale_y;
        public string viewname;

        public void Render(double x1, double y1, double x2, double y2, double sx, double sy, string nm, Canvas _parent, Canvas mainscene)
        {
            double fixx;
            double fixy;

            x = x1;
            y = y1;
            x_extent = x2;
            y_extent = y2;
            scale_x = sx;
            scale_y = sy;
            viewname = nm;

            Rectangle r = new Rectangle();
            r.Width = x2;
            r.Height = y2;
            _parent.Children.Add(r);
            Canvas.SetLeft(r, x);
            Canvas.SetTop(r, y);
            r.Fill = new SolidColorBrush(Colors.Cyan);
            r.Opacity = 0.15;
            r.Stroke = new SolidColorBrush(Colors.WhiteSmoke);
            r.StrokeThickness = 0.25;

            fixx = (2000/190)*x1;
            fixy = (1500/120)*y1;
            
            Rectangle r2 = new Rectangle();
            r2.Width = 150;
            r2.Height = 150;
            mainscene.Children.Add(r2);
            Canvas.SetLeft(r2, fixx);
            Canvas.SetTop(r2, fixy);
            r2.Opacity = 0.5;
            r2.Stroke = new SolidColorBrush(Colors.WhiteSmoke);
            r2.StrokeThickness = 2.5;

        }
    }
}
