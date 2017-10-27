using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpongeBot.Input.Mouse
{

    class User32_MouseClick : IClick
    {
        public IPosition MousePosition { get; }

        public User32_MouseClick(IPosition mPos)
        {
            MousePosition = mPos;
        }

        public void Left(Point P_point)
        {
            Left((int)P_point.X, (int)P_point.Y);
        }

        public void Left(int Fi_xpos, int Fi_ypos)
        {
            MousePosition.SetCursorPos(new Point(Fi_xpos, Fi_ypos));
            mouse_event(MOUSEEVENTF_LEFTDOWN, Fi_xpos, Fi_ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, Fi_xpos, Fi_ypos, 0, 0);
        }

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    }


}
