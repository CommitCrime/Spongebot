using System.Windows;
using SpongeBot.CoordinateProvider;
using System.Windows.Media.Imaging;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace SpongeBot
{
    internal class CoordinateExample
    {
        private IEnumerator<System.Windows.Point> coordEnum;
        private Rect rect;

        public CoordinateExample(IEnumerator<System.Windows.Point> someCoordEnum, Rect rect)
        {
            this.coordEnum = someCoordEnum;
            this.rect = rect;
        }

        public GifBitmapEncoder getGif()
        {
            coordEnum.Reset();
            System.Windows.Media.Imaging.GifBitmapEncoder gEnc = new GifBitmapEncoder();

            Bitmap target = new Bitmap((int)rect.Width, (int)rect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.FillRectangle(Brushes.Black, 0, 0, 800, 450);
            }

            while (coordEnum.MoveNext())
            {
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawRectangle(Pens.Turquoise, new Rectangle((int)(coordEnum.Current.X), (int)coordEnum.Current.Y, 1, 1));
                }

                var bmp = target.GetHbitmap();
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                gEnc.Frames.Add(BitmapFrame.Create(src));
                Utility.NativeMethods.DeleteObject(bmp); // recommended, handle memory leak
            }


            using (System.IO.FileStream fs = new System.IO.FileStream(@"D:\preview.gif", System.IO.FileMode.Create))
            {
                gEnc.Save(fs);
            }


            return gEnc;
        }

        public BitmapSource getImage()
        {
            getGif();
            System.Drawing.Image img = getLegacyImage();
            BitmapSource imgStream = Utility.NativeMethods.GetImageStream(img);
            return imgStream;
        }

        private Image getLegacyImage()
        {
            coordEnum.Reset();

            Bitmap target = new Bitmap((int)rect.Width, (int)rect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.FillRectangle(Brushes.Black, 0, 0, 800, 450);

                while (coordEnum.MoveNext())
                {
                    g.DrawRectangle(Pens.Turquoise, new Rectangle((int)(coordEnum.Current.X), (int)coordEnum.Current.Y, 1, 1));
                }
            }

            return target;
        }
    }
}