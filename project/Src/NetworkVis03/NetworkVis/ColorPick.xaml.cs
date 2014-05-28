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

namespace NetworkVis
{
    /// <summary>
    /// Interaction logic for ColorPick.xaml
    /// </summary>
    public partial class ColorPick : Window
    {
        private byte[] pixels;
        public SolidColorBrush returnedcolor = new SolidColorBrush();
        public ColorPick()
        {
            InitializeComponent();
            wheel.MouseMove += new MouseEventHandler(wheel_MouseMove);

        }

        void wheel_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                CroppedBitmap cb = new CroppedBitmap(wheel.Source as BitmapSource, new Int32Rect((int)Mouse.GetPosition(this).X, (int)Mouse.GetPosition(this).Y, 1, 1));
                pixels = new byte[4];
                try
                {
                    cb.CopyPixels(pixels, 4, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Console.WriteLine(pixels[0] + ":" + pixels[1] + ":" + pixels[2] + ":" + pixels[3]);
                rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(pixels[2], pixels[1], pixels[0]));
            }
            catch (Exception exc)
            {
            }
        }
    }
}
