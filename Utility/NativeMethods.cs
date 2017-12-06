using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpongeBot.Utility
{
    class NativeMethods
    {

        /// <summary>
        /// https://stackoverflow.com/a/10077805
        /// </summary>
        /// <param name="myImage"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapSource GetImageStream(System.Drawing.Image myImage)
        {
            var bitmap = new System.Drawing.Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            System.Windows.Media.Imaging.BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   System.Windows.Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);


        [DllImport("User32.dll")]
        internal static extern bool RegisterHotKey(
          [In] IntPtr hWnd,
          [In] int id,
          [In] uint fsModifiers,
          [In] uint vk);

        [DllImport("User32.dll")]
        internal static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);




        [DllImport("user32.dll")]
        internal static extern ushort VkKeyScan(char ch);

    }
}
