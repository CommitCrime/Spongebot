using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeBot.Utility.ImageResize
{
    interface IImageResize
    {
        Image ResizeImage(Image originalImg, Size newSize, bool preserveAspectRatio = true);
    }
}
