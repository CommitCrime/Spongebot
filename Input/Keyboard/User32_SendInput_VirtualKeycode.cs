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
                VirtualKey  vKey = new VirtualKey(c);
                pressModifiers(vKey); //Press e.g. shift

                INPUT[] Inputs = new INPUT[2];


                //Set up the INPUT structure
                Input.type = 1;
                Input.U.ki.time = 0;
                Input.U.ki.wVk = vKey.VKey; // Set a Virtual Keycode 

                Input.U.ki.wScan = 0;  // We're doing virtual keycodes instead

                //This let's you do a hardware scan instead of a virtual keypress
                Input.U.ki.dwFlags = KEYEVENTF.VIRTUALKEY;

                Inputs[0] = Input;

                //Prepare a keyup event
                Input.U.ki.dwFlags = KEYEVENTF.KEYUP;
                Inputs[1] = Input;

                SendInput((uint)Inputs.Length, Inputs, INPUT.Size);

                releaseModifiers(vKey);
                Thread.Sleep(new Random().Next(1, 20));
            }
        }

        private void pressModifiers(VirtualKey vKey)
        {
            foreach (VirtualKey modifier in vKey.Modifiers)
            {
                INPUT Input = new INPUT();
                INPUT[] Inputs = new INPUT[1];

                //Set up the INPUT structure
                Input.type = 1;
                Input.U.ki.time = 0;
                Input.U.ki.wVk = modifier.VKey;
                Input.U.ki.wScan = 0;  //would ne the unicode charcter

                //This let's you do a hardware scan instead of a virtual keypress
                Input.U.ki.dwFlags = KEYEVENTF.VIRTUALKEY;

                Inputs[0] = Input;

                SendInput((uint)Inputs.Length, Inputs, INPUT.Size);
            }
        }

        private void releaseModifiers(VirtualKey vKey)
        {
            foreach(VirtualKey modifier in vKey.Modifiers)
            {
                INPUT Input = new INPUT();

                INPUT[] Inputs = new INPUT[1];

                //Set up the INPUT structure
                Input.type = 1;
                Input.U.ki.time = 0;
                Input.U.ki.wVk = modifier.VKey; //there are additional codes for left and right shift
                Input.U.ki.wScan = 0;  //would ne the unicode charcter

                //This let's you do a hardware scan instead of a virtual keypress

                Input.U.ki.dwFlags = KEYEVENTF.KEYUP;
                Inputs[0] = Input;

                SendInput((uint)Inputs.Length, Inputs, INPUT.Size);
            }
        }
    }

    class VirtualKey
    {
        public ushort VKey { get; }
        public bool ShiftNeeded {
            get
            {
                return ((VKey >> 8 & 1) != 0);
            }
        }
        public bool CtrlNeeded
        {
            get
            {
                return ((VKey >> 8 & 2) != 0);
            }
        }
        public bool AltNeeded
        {
            get
            {
                return ((VKey >> 8 & 4) != 0);
            }
        }

        public VirtualKey(Keys vKeyCode)
        {
            this.VKey = (ushort)vKeyCode;
        }


        public VirtualKey(char c)
        {
            this.VKey = VkKeyScan(c);
        }

        public List<VirtualKey> Modifiers
        {
            get
            {
                List<VirtualKey> modifiers = new List<VirtualKey>();
                if (ShiftNeeded)
                    modifiers.Add(new VirtualKey(Keys.ShiftKey));  //there are additional codes for left and right modifier keys
                if (CtrlNeeded)
                    modifiers.Add(new VirtualKey(Keys.ControlKey));
                if (AltNeeded)
                    modifiers.Add(new VirtualKey(Keys.Menu)); //Menu == alt, alt is something

                return modifiers;
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern ushort VkKeyScan(char ch);
    }
}
