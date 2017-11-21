using System;
using System.Collections.Generic;
using System.Drawing;
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
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PreviewWindow(System.Drawing.Image image, double scale = 0.5)
        {
            InitializeComponent();
            this.img.Source = Utility.NativeMethods.GetImageStream(image);
            log.Debug($"Preview imag. Size {image.Width}x{image.Height}");
            this.panel.Width = image.Width * scale;
            this.panel.Height = image.Height * scale;
        }

        public PreviewWindow(CoordinateProvider.ACoordinateProvider coordProvider, bool animate = true)
        {
            InitializeComponent();
            this.Width = coordProvider.Area.Width;
            this.Height = coordProvider.Area.Height;


            MemoryStream memoryStream = new MemoryStream();
            if(animate)
                new CoordinateExample(coordProvider).getGif().Save(memoryStream);
            else
                new CoordinateExample(coordProvider).getBitmap().Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            
            XamlAnimatedGif.AnimationBehavior.SetSourceStream(img, memoryStream);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.Trace($"Size changed from {e.PreviousSize} to {e.NewSize}.");

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.SizeToContent = SizeToContent.Manual;
            this.panel.Width = Double.NaN;
            this.panel.Height = Double.NaN;
        }
    }
}
