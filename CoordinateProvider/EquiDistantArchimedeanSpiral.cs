using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace SpongeBot.CoordinateProvider
{
    class EquiDistantArchimedeanSpiral
    {

        internal static BitmapSource getImage()
        {
            System.Drawing.Image img = draw();
            BitmapSource imgStream = GetImageStream(img);
            return imgStream;
        }


        private static Bitmap draw()
        {
            Bitmap target = new Bitmap(800, 450);
            Point spiralCenter = new Point(400, 225);
            double coilGap = 13; //gap bewteen coils 

            //find distance to nearest boundry:
            double maxRadius = new[] { spiralCenter.X, spiralCenter.Y, target.Width - spiralCenter.X, target.Height - spiralCenter.Y }.Min();
            double numberOfCoils = Math.Ceiling(maxRadius / coilGap);

            double rotation = 2 * Math.PI;
            double thetaMax = numberOfCoils * rotation;
            double awayStep = maxRadius / thetaMax;
            double equiPointDistance = 5;

            using (Graphics g = Graphics.FromImage(target))
            {
                g.FillRectangle(Brushes.Black, 0, 0, 800, 450);
                g.DrawRectangle(Pens.White, new Rectangle(spiralCenter.X, spiralCenter.Y, 1, 1));

                for (double theta = equiPointDistance / awayStep; theta <= thetaMax;)
                {
                    double away = awayStep * theta;
                    double around = theta + rotation;

                    double x = spiralCenter.X + Math.Cos(around) * away;
                    double biasedX = spiralCenter.X + Math.Cos(around) * away * 16/9;
                    double y = spiralCenter.Y + Math.Sin(around) * away;

                    g.DrawRectangle(Pens.Turquoise, new Rectangle((int)(biasedX), (int)y, 1, 1));
                    //g.DrawRectangle(Pens.Gray, new Rectangle((int)x, (int)y, 1, 1));

                    theta += equiPointDistance / away;
                }
            }

            return target;
        }



        static PointF archimedeanPoint(int theta)
        {
            double away = 5 * theta;
            double around = theta + 2 * Math.PI; ;

            double x = 250f + Math.Cos(around) * away;
            double y = 250f + Math.Sin(around) * away;

            return new PointF((float)x, (float)y);
        }

        

        /// <summary>
        /// https://stackoverflow.com/a/10077805
        /// </summary>
        /// <param name="myImage"></param>
        /// <returns></returns>
        public static BitmapSource GetImageStream(System.Drawing.Image myImage)
        {
            var bitmap = new System.Drawing.Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   System.Windows.Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);
    }
}
