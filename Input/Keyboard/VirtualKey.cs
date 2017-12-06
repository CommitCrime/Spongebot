using System.Collections.Generic;
using System.Windows.Forms;
using static SpongeBot.Input.Keyboard.User32_SendInput;

namespace SpongeBot.Input.Keyboard
{

    /// <summary>
    /// List of Virtual Key Codes
    /// http://kbdedit.com/manual/low_level_vk_list.html
    /// http://cherrytree.at/misc/vk.htm
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx?f=255&MSPPError=-2147217396
    /// <seealso cref="System.Windows.Forms.Keys"/>
    /// </summary>
    class VirtualKey
    {
        public ushort VKey { get; }
        #region properties
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

        List<VirtualKey> _modifiers = new List<VirtualKey>();
        public List<VirtualKey> Modifiers
        {
            get
            {
                return _modifiers;
            }
        }
        #endregion

        public VirtualKey(Keys vKeyCode) : this((ushort)vKeyCode) { }
        public VirtualKey(char c) : this(Utility.NativeMethods.VkKeyScan(c)) { }
        public VirtualKey(ushort vKeyCode)
        {
            this.VKey = vKeyCode;

            if (ShiftNeeded)
                _modifiers.Add(new VirtualKey(Keys.ShiftKey));  //there are additional codes for left and right modifier keys
            if (CtrlNeeded)
                _modifiers.Add(new VirtualKey(Keys.ControlKey));
            if (AltNeeded)
                _modifiers.Add(new VirtualKey(Keys.Menu)); //Menu == alt, alt is something
        }

        public void Send()
        {
            pressModifiers(); //Press e.g. shift

            INPUT Input = new INPUT();
            INPUT[] Inputs = new INPUT[2];

            //Set up the INPUT structure
            Input.type = 1;
            Input.U.ki.time = 0;
            Input.U.ki.wVk = VKey; // Set a Virtual Keycode 
            Input.U.ki.wScan = 0;  // We're doing virtual keycodes instead
            Input.U.ki.dwFlags = KEYEVENTF.VIRTUALKEY; //key down event
            Inputs[0] = Input;

            Input.U.ki.dwFlags = KEYEVENTF.KEYUP; // key up event
            Inputs[1] = Input;

            SendInput((uint)Inputs.Length, Inputs, INPUT.Size);

            releaseModifiers();
        }

        private void pressModifiers()
        {
            foreach (VirtualKey modifier in Modifiers)
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

        private void releaseModifiers()
        {
            foreach (VirtualKey modifier in Modifiers)
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
}
