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
    class ArchimedeanSpiral
    {

        internal static BitmapSource getImage()
        {
            System.Drawing.Image img = draw();
            BitmapSource imgStream = GetImageStream(img);
            return imgStream;
        }


        private static Bitmap draw()
        {
            Bitmap target = new Bitmap(500, 500);
            using (Graphics g = Graphics.FromImage(target))
            {
                PointF p0 = new PointF(500 / 2, 500 / 2);
                g.DrawRectangle(Pens.Red, new Rectangle((int)p0.X, (int)p0.Y, 1, 1));

                int i = 0;
                do
                {
                    PointF p1 = archimedeanPoint(i);
                    //sg.DrawLine(Pens.Red, p0, p1);
                    g.DrawRectangle(Pens.Blue, new Rectangle((int)p1.X, (int)p1.Y, 1,1));
                    p0 = p1;
                    i += 10;
                } while (p0.X > 0 && p0.Y > 0);
            }
            return target;
        }



        static PointF archimedeanPoint(int degrees)
        {
            const double a = 15; //initial distance from center
            const double b = 3; //gap between spiral lines
            double t = degrees * Math.PI / 180;
            double r = a + b * t;
            return new PointF { X = (float)(500 / 2 + r * Math.Cos(t)), Y = (float)(500 / 2 + r * Math.Sin(t)) };
        }



        private static IList<Point> getSpiralCoords(Point center)
        {
            int stepSize = 11;

            IList<Point> points = new List<Point>();
            points.Add(center);

            for (int i = stepSize; Math.Min(center.X, center.Y) - i > 0; i += stepSize)
            {
                System.Windows.Rect bounds = new System.Windows.Rect(new System.Windows.Point(center.X - i, center.Y - i), new System.Windows.Point(center.X + i, center.Y + i));

                Point topLeft = new Point(center.X - i, center.Y - i);
                Point p = topLeft;
                do
                {
                    points.Add(p); // Point is a struct, and struct is not a reference type -> no need to create new Point
                    if (p.Y <= bounds.Top && p.X < bounds.Right)
                    {
                        p.X += stepSize; //top left to top right
                    }
                    else if (p.X >= bounds.Right && p.Y < bounds.Bottom)
                    {
                        p.Y += stepSize; //top right to bottom right
                    }
                    else if (p.Y >= bounds.Bottom && p.X > bounds.Left)
                    {
                        p.X -= stepSize; //bottom right to bottom left
                    }
                    else if (p.X <= bounds.Left && p.Y > bounds.Top)
                    {
                        p.Y -= stepSize;
                    }
                    else throw new Exception("This position should not be possible");


                } while (p != topLeft);

            }

            return points;
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
