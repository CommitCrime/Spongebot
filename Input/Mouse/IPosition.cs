using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeBot.Input.Mouse
{
    interface IPosition
    {
        //void SetCursorPos(int x, int y);
        //void SetCursorPos(System.Drawing.Point drawPoint);
        void SetCursorPos(System.Windows.Point winPoint);


        //System.Drawing.Point GetCursorPos();
        System.Windows.Point GetCursorPos();
    }
}
