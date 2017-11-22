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
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool animated = false;
        MemoryStream memoryStream = new MemoryStream();

        public PreviewWindow(System.Drawing.Bitmap image, double scale = 1)
        {
            init(new Size(image.Width, image.Height), scale);
            image.Save(this.memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            this.img.Source = Utility.NativeMethods.GetImageStream(image);
        }

        public PreviewWindow(CoordinateProvider.ACoordinateProvider coordProvider, bool animate = true, double scale =  1)
        {
            this.animated = animate;
            init(coordProvider.Area.Size, scale);

            MemoryStream memoryStream = new MemoryStream();
            if(animate)
                new CoordinateExample(coordProvider).getGif().Save(memoryStream);
            else
                new CoordinateExample(coordProvider).getBitmap().Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

            memoryStream.CopyTo(this.memoryStream);

            XamlAnimatedGif.AnimationBehavior.SetSourceStream(img, memoryStream);
        }

        private void init(Size size, double scale = 1)
        {
            InitializeComponent();
            log.Debug($"Preview imag. Size {size}.");
            this.img.Width = size.Width * scale;
            this.img.Height = size.Height * scale;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.Trace($"Size changed from {e.PreviousSize} to {e.NewSize}.");
            fitImg();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // we will reset the image's size, which would resize the window, so we disable sizetocontent as the window is already rendered
            this.SizeToContent = SizeToContent.Manual;
            fitImg();
        }

        /// <summary>
        /// The img should get bigger/smaller when the window is resized.
        /// Normal img resize method is very greedy. it would push the button out of the visible area so it can display its content.
        /// </summary>
        private void fitImg()
        {
            // reset the width to Auto:
            this.img.Width = Double.NaN;
            // to make img adapt to window size the height is set manually
            // otherwise the image would push the btn out of visible area
            this.img.Height = Math.Max(0, this.panel.ActualHeight - this.btnSave.MinHeight);
        }

        private void Save_Img(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
            if(animated)
                saveDialog.Filter = "Image (*.gif)|*.gif|All files (*.*)|*.*";
            else
                saveDialog.Filter = "Image (*.bmp)|*.bmp|All files (*.*)|*.*";

            saveDialog.FilterIndex = 1;
            saveDialog.RestoreDirectory = true;

            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                memoryStream.WriteTo(saveDialog.OpenFile());
            }
        }
    }
}
