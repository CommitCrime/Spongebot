using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace SpongeBot.Input.Keyboard
{ 


    internal enum Modifier : int
    {
        NONE = 0x000,
        ALT = 0x0001,
        CTRL = 0x0002,
        SHIFT = 0x0004,
        WIN = 0x0008
    }
}
