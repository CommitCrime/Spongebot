using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Botsy.Utility
{
    class Screenshot : IScreenshot
    {
        public Image GetScreenshot()
        {
            int screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            int screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;


            Bitmap target = new Bitmap(screenWidth, screenHeight);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));
            }
            return target;
        }
    }
}
