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
    /// <summary>
    /// List of Virtual Key Codes
    /// http://kbdedit.com/manual/low_level_vk_list.html
    /// http://cherrytree.at/misc/vk.htm
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx?f=255&MSPPError=-2147217396
    /// </summary>
    class User32_SendInput_VirtualKeycode : User32_SendInput, ISendString
    {
        public void SendString(string toType)
        {
            foreach(char c in toType)
            {
                VirtualKey  vKey = new VirtualKey(c);

                vKey.Send();
                Thread.Sleep(new Random().Next(1, 20));
            }
        }

        private void send(Keys key)
        {
            INPUT Input = new INPUT();
            INPUT[] Inputs = new INPUT[2];

            //Set up the INPUT structure
            Input.type = 1;
            Input.U.ki.time = 0;
            Input.U.ki.wVk = (ushort)key; // Set a Virtual Keycode 

            Input.U.ki.wScan = 0;  // We're doing virtual keycodes instead

            //This let's you do a hardware scan instead of a virtual keypress
            Input.U.ki.dwFlags = KEYEVENTF.VIRTUALKEY;

            Inputs[0] = Input;

            //Prepare a keyup event
            Input.U.ki.dwFlags = KEYEVENTF.KEYUP;
            Inputs[1] = Input;

            SendInput((uint)Inputs.Length, Inputs, INPUT.Size);
        }

       
    }
}
