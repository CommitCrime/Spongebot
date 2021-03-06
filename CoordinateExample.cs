﻿using System.Windows;
using SpongeBot.CoordinateProvider;
using System.Windows.Media.Imaging;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SpongeBot
{
    internal class CoordinateExample
    {
        private ACoordinateProvider coordProvider;
        private Rect rect;

        public CoordinateExample(ACoordinateProvider coordProvider)
        {
            this.coordProvider = coordProvider;
            this.rect = coordProvider.Area;
        }

        public string getGifAsFile()
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".gif";

            using (FileStream fs = new FileStream(fileName, System.IO.FileMode.Create))
            {
                getGif().Save(fs);
            }

            return fileName;
        }

        public GifBitmapEncoder getGif()
        {
            coordProvider.Reset();

            GifBitmapEncoder gEnc = new GifBitmapEncoder();

            Bitmap target = new Bitmap((int)rect.Width, (int)rect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.FillRectangle(Brushes.Black, 0, 0, 800, 450);
            }

            while (coordProvider.MoveNext())
            {
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawRectangle(Pens.Turquoise, new Rectangle((int)(coordProvider.Current.X), (int)coordProvider.Current.Y, 1, 1));
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


            return gEnc;
        }

        public string getBitmapAsFile()
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".bmp";
            getBitmap().Save(fileName);

            return fileName;
        }

        public BitmapSource getWpfImage()
        {
            getGif();
            Image img = getBitmap();
            BitmapSource imgStream = Utility.NativeMethods.GetImageStream(img);
            return imgStream;
        }

        public Bitmap getBitmap()
        {
            coordProvider.Reset();

            Bitmap target = new Bitmap((int)rect.Width, (int)rect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.FillRectangle(Brushes.Black, 0, 0, 800, 450);

                while (coordProvider.MoveNext())
                {
                    g.DrawRectangle(Pens.Turquoise, new Rectangle((int)(coordProvider.Current.X), (int)coordProvider.Current.Y, 1, 1));
                }
            }

            return target;
        }
    }
}