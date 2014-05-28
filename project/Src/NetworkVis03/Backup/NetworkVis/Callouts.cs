using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace NetworkVis
{
    class Callouts
    {
        public double xpos;
        public double ypos;
        public string type;
        public string calloutText;
        public Rectangle r;
        public Label rText;

        public void Create(double x, double y, string typ, string text)
        {
            xpos = x;
            ypos = y;
            type = typ;
            calloutText = text;
        }

        public void Render(Canvas _parent)
        {
            rText = new Label();
            rText.Background = new SolidColorBrush(Colors.LightYellow);
            if (type == "Link")
            { rText.Foreground = new SolidColorBrush(Colors.Blue); 
            }
            else
            {
                rText.Foreground = new SolidColorBrush(Colors.Black);
            }
            rText.Content = calloutText;
            rText.FontSize = 10;
            rText.Width = 30;
            rText.MouseEnter +=new System.Windows.Input.MouseEventHandler(rText_MouseEnter);
            rText.MouseLeave += new System.Windows.Input.MouseEventHandler(rText_MouseLeave);
            rText.MouseDown +=new MouseButtonEventHandler(rText_MouseDown);
            _parent.Children.Add(rText);
            Canvas.SetLeft(rText, xpos);
            Canvas.SetTop(rText, ypos);

            Rectangle r1 = new Rectangle();
            r1.Fill = new SolidColorBrush(Colors.LightYellow);
            r1.Width = 8;
            r1.Height = 8;
            _parent.Children.Add(r1);
            Canvas.SetLeft(r1, xpos+5);
            Canvas.SetTop(r1, ypos+15);

            RotateTransform rt = new RotateTransform();
            rt.Angle = 45;
            r1.LayoutTransform = rt;
            

        }

        private void rText_MouseEnter(object sender, MouseEventArgs e)
        {
            rText.Width = 130;
 
        }

        private void rText_MouseLeave(object sender, MouseEventArgs e)
        {
            rText.Width = 30;

        }

        private void rText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (type == "Link")
            {
                MessageBox.Show("Browser");
            }
 
        }
    }
}
