using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpongeBot.Input.Keyboard
{
    //Does nto work
    class User32_SendInput_Unicode : User32_SendInput, ISendString
    {
        public void SendString(string toType)
        {
            INPUT Input = new INPUT();

            foreach(char c in toType)
            {
                INPUT[] Inputs = new INPUT[2];
                Input.type = 1; // 1 = Keyboard Input
                Input.U.ki.wScan = (short)c;
                Input.U.ki.dwFlags = KEYEVENTF.UNICODE;
                Inputs[0] = Input;

                Input.type = 1; // 1 = Keyboard Input
                Input.U.ki.wScan = (short)c;
                Input.U.ki.dwFlags = KEYEVENTF.UNICODE | KEYEVENTF.KEYUP;
                Inputs[1] = Input;
                SendInput((uint)Inputs.Length, Inputs, INPUT.Size);

                Thread.Sleep(new Random().Next(1, 20));
            }
        }

       
    }
}
