using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpongeBot.Input.Mouse
{
    class Cursor
    {
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms648381(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINTAPI ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll")]
        static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

        const Int32 CURSOR_SHOWING = 0x00000001;

        public static Bitmap GetCursorImg()
        {
            CURSORINFO pci;
            pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CURSORINFO));

            Bitmap target = new Bitmap(32 * (int)Utility.Screen.ScreenHelper.getWidthScalingFactor(), 32 * (int) Utility.Screen.ScreenHelper.getHeightScalingFactor());

            using (Graphics g = Graphics.FromImage(target))
            {
                //g.FillRectangle(Brushes.White, 0, 0, target.Width, target.Height);
                if (GetCursorInfo(out pci))
                {
                    if (pci.flags == CURSOR_SHOWING)
                    {
                        DrawIcon(g.GetHdc(), 0, 0, pci.hCursor);
                        g.ReleaseHdc();
                    }
                }
            }

            target.Save(@"D:\hook.png");

            return target;
        }
    }
}
