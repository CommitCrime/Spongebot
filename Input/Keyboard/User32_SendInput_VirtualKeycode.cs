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
                new VirtualKey(c).Send();
                Thread.Sleep(new Random().Next(1, 20));
            }
        }
    }
}
