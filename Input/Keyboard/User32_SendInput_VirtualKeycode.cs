using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace SpongeBot.Input.Keyboard
{
    class User32_SendInput_VirtualKeycode : User32_SendInput, ISendString
    {
        public void SendString(string toType)
        {
            foreach(char c in toType)
            {
                new VirtualKey(c).Send();
                Thread.Sleep(new Random().Next(1, 20));
            }
        }
    }
}
