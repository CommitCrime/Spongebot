using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpongeBot.Input.Mouse
{
    class User32_MousePosition : IPosition
    {
        public Point GetCursorPos()
        {
            MousePoint F_struct;
            if (GetCursorPos(out F_struct))
            {
                return new Point(F_struct.X, F_struct.Y);
            }

            return new Point(Double.NaN, Double.NaN);
        }

        public void SetCursorPos(Point P_point)
        {
            SetCursorPos((int)P_point.X, (int)P_point.Y);
        }


        /* ------------ Native Part ------------- */
        [StructLayout(LayoutKind.Sequential)]
        private struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpPoint);


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
    }
}
