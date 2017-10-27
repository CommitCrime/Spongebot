using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeBot.Input.Mouse
{
    interface IClick
    {
        void Left(System.Windows.Point p);
        void Left(int x, int y);
    }
}
