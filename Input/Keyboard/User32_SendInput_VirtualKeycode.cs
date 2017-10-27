using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace SpongeBot.Input.Keyboard
{
    //Does nto work
    class User32_SendInput_VirtualKeycode : User32_SendInput, ISendString
    {
        public void SendString(string toType)
        {
            INPUT Input = new INPUT();

            foreach(char c in toType)
            {
                INPUT[] Inputs = new INPUT[2];


                //Set up the INPUT structure
                Input.type = 1;
                Input.U.ki.time = 0;
                Input.U.ki.wVk = (ushort)ConvertCharToVirtualKey(c); //We're doing scan codes instead

                Input.U.ki.wScan = 0;  //Set a unicode character to use (A)

                //This let's you do a hardware scan instead of a virtual keypress
                Input.U.ki.dwFlags = KEYEVENTF.VIRTUALKEY;

                Inputs[0] = Input;

                //Prepare a keyup event
                Input.U.ki.dwFlags = KEYEVENTF.KEYUP;
                Inputs[1] = Input;

                SendInput((uint)Inputs.Length, Inputs, INPUT.Size);

                Thread.Sleep(new Random().Next(1, 20));
            }
        }

        public static Keys ConvertCharToVirtualKey(char ch)
        {
            short vkey = VkKeyScan(ch);
            Keys retval = (Keys)(vkey & 0xff);
            int modifiers = vkey >> 8;
            if ((modifiers & 1) != 0) retval |= Keys.Shift;
            if ((modifiers & 2) != 0) retval |= Keys.Control;
            if ((modifiers & 4) != 0) retval |= Keys.Alt;
            return retval;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);


    }
}
