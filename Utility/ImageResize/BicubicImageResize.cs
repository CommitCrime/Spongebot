using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeBot.Utility.ImageResize
{
    class BicubicImageResize : IImageResize
    {
        /// <summary>
        /// https://www.codeproject.com/Articles/191424/Resizing-an-Image-On-The-Fly-using-NET
        /// </summary>
        /// <param name="originalImg"></param>
        /// <param name="newSize"></param>
        /// <param name="preserveAspectRatio"></param>
        /// <returns></returns>
        public Image ResizeImage(Image originalImg, Size newSize, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = originalImg.Width;
                int originalHeight = originalImg.Height;
                float percentWidth = (float)newSize.Width / (float)originalWidth;
                float percentHeight = (float)newSize.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth; // use smaller one to fit into given newSize
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = newSize.Width;
                newHeight = newSize.Height;
            }
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(originalImg, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
    }
}
