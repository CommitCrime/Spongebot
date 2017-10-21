using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Botsy
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// https://pacoup.com/2011/06/12/list-of-true-169-resolutions/
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {

        Timer five;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// https://stackoverflow.com/a/10077805
        /// </summary>
        /// <param name="myImage"></param>
        /// <returns></returns>
        public static BitmapSource GetImageStream(System.Drawing.Image myImage)
        {
            var bitmap = new System.Drawing.Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);

        private void Screenshot_Now(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image screenshotImg = new Utility.CursorScreenshot().GetScreenshot();
            screenshotImg = new Utility.BicubicImageResize().ResizeImage(screenshotImg, new System.Drawing.Size(640, 360));
            BitmapSource screenStream = GetImageStream(screenshotImg);
            this.screenshot.Source = screenStream;

            System.Drawing.Image cursorImg = Utility.Cursor.GetCursorImg();
            BitmapSource cursorStream = GetImageStream(cursorImg);
            this.cursor.Source = cursorStream;
        }

        private void Screenshot_in5(object sender, RoutedEventArgs e)
        {
            if (five != null)
                five.Dispose();

            five = new Timer((state) =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => Screenshot_Now(sender, e)));
            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
        }
    }
}
