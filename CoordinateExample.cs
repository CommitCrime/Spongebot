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

        public BitmapSource getImage()
        {
            System.Drawing.Image img = getLegacyImage();
            BitmapSource imgStream = Utility.NativeMethods.GetImageStream(img);
            return imgStream;
        }

        private Image getLegacyImage()
        {
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