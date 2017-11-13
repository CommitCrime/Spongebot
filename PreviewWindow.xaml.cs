using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpongeBot
{
    /// <summary>
    /// Interaktionslogik für PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {

        public PreviewWindow(System.Drawing.Bitmap bmpSource)
        {
            InitializeComponent();
            MemoryStream memoryStream = new MemoryStream();
            bmpSource.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

            XamlAnimatedGif.AnimationBehavior.SetSourceStream(img, memoryStream);
        }

        public PreviewWindow(GifBitmapEncoder gifSource)
        {
            InitializeComponent();
            MemoryStream memoryStream = new MemoryStream();
            gifSource.Save(memoryStream);

            XamlAnimatedGif.AnimationBehavior.SetSourceStream(img, memoryStream);
        }
    }
}
