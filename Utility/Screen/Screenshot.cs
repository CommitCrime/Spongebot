﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeBot.Utility.Screen
{
    class Screenshot : IScreenshot
    {
        public Bitmap GetScreenshot()
        {
            int screenWidth = (int)ScreenHelper.getActualPrimaryScreenWidth();
            int screenHeight = (int)ScreenHelper.getActualPrimaryScreenHeight();

            Bitmap target = new Bitmap(screenWidth, screenHeight);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));

                //for(int i=1; i<16; i++)
                //{
                //    g.DrawLine(Pens.Red, (screenWidth / 16) * i, 0, (screenWidth / 16) * i, screenHeight);
                //}


                //for (int i = 1; i < 9; i++)
                //{
                //    g.DrawLine(Pens.Red, 0, screenHeight/9*i, screenWidth, screenHeight/9*i);
                //}
            }

            return target;
        }
    }
}
