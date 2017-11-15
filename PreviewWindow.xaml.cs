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

        public PreviewWindow(CoordinateProvider.ACoordinateProvider coordProvider, bool animate = true)
        {
            InitializeComponent();
            this.img.Width = coordProvider.Area.Width;
            this.img.Height = coordProvider.Area.Height;


            MemoryStream memoryStream = new MemoryStream();
            if(animate)
                new CoordinateExample(coordProvider).getGif().Save(memoryStream);
            else
                new CoordinateExample(coordProvider).getBitmap().Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            
            XamlAnimatedGif.AnimationBehavior.SetSourceStream(img, memoryStream);
        }
    }
}
