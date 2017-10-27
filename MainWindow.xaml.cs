using Gma.System.MouseKeyHook;
using NAudio.CoreAudioApi;
using SpongeBot.Utility;
using System;
using System.Collections;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpongeBot
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HotKeyWindow
    {
        //https://github.com/gmamaladze/globalmousekeyhook
        private IKeyboardMouseEvents m_GlobalHook;

        Timer fiveSeconds, botTimer;

        bool timerEnabled = false;



        public MainWindow() : base()
        {
            InitializeComponent();
            botTimer = new Timer(doFish, null, -1, -1);

            base.RegisterHotKey(Utility.Hotkey.Modifier.CTRL, Utility.Hotkey.KeyCode.KEY_A);

            m_GlobalHook = Hook.GlobalEvents();
        }


        internal override void OnHotKeyPressed()
        {
            if(!timerEnabled)
            {
                Console.WriteLine("+++ Enable Hotkey Code");
                timerEnabled = true;
                botTimer.Change(500, 20*1000);
                m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            }
            else
            {
                Console.WriteLine("--- Disable Hotkey Code");
                timerEnabled = false;
                botTimer.Change(-1, -1);
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            }
        }

        private void doFish(Object state)
        {
            try
            {
                // do stuff
                Input.Keyboard.ISendString send = new Input.Keyboard.User32_SendInput_VirtualKeycode();
                send.SendString("1");

                Thread.Sleep(100);
                Point hookPos = getHookPos();

                Console.WriteLine("yeah ", hookPos.ToString());

                // 3 Listen
                // http://gigi.nullneuron.net/gigilabs/displaying-a-volume-meter-using-naudio/
                MMDeviceEnumerator SndDevEnum = new MMDeviceEnumerator();
                MMDevice SndDevice = SndDevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                while (timerEnabled)
                {
                    double volume = SndDevice.AudioMeterInformation.MasterPeakValue * 100;
                    Console.WriteLine("Volume:" + volume);
                    if (volume > 20)
                        break;
                    Thread.Sleep(100);
                }

                // 4 Click
                new Input.Mouse.User32_MouseClick(new Input.Mouse.User32_MousePosition()).Left(hookPos);
            }
            catch
            {
                Console.WriteLine("Nope, no fish");
            }
        }


        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (timerEnabled)
            {
                botTimer.Change(1500, 20 * 1000);
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Unsubscribe();
            //e.Cancel = true;
            //this.Hide();
        }


        private void Screenshot_in5(object sender, RoutedEventArgs e)
        {
            if (fiveSeconds != null)
                fiveSeconds.Dispose();

            fiveSeconds = new Timer((state) =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => Screenshot_Now(sender, e)));
            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
        }

        private Point getHookPos()
        {
            //Point F_initialMousePos = MouseInteropHelper.getCursorPos();
            Input.Mouse.User32_MousePosition mousePos = new Input.Mouse.User32_MousePosition();

            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            Point rectLocation = mousePos.GetCursorPos(); //get MousePos --> center of new rect
            rectLocation.Offset(-screenWidth / 4, -screenWidth / 4); //move point to use it as rect location

            Rect halfScreenWidthSquare = new Rect(rectLocation, new Size(screenWidth / 2, screenWidth / 2));

            IEnumerator<Point> coords = new SpiralCoordinateProvider(halfScreenWidthSquare);

            while(coords.MoveNext() && timerEnabled)
            {
                // 1 Move Cursor
                Point p = coords.Current;
                mousePos.SetCursorPos(p);

                Thread.Sleep(10);

                // 2 Compare Cursor
                if (Equals(Input.Mouse.Cursor.GetCursorImg(), SpongeBot.Properties.Resources.HookCursor))
                {
                    //p = new Point(p.X + 30, p.Y + 30); // works well @ 1080p & first person view & line by line 
                    //mousePos.SetCursorPos(p);
                    Thread.Sleep(250); //cursor might change due to bobber movement when mouse is at the very corner
                    if (Equals(Input.Mouse.Cursor.GetCursorImg(), SpongeBot.Properties.Resources.HookCursor))
                    {
                        return p;
                    }
                }
            }

            throw new Exception("Not found");
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
            System.Drawing.Image screenshotImg;
            if (this.includeCursor.IsChecked == true)
                screenshotImg = new Input.Mouse.CursorScreenshot().GetScreenshot();
            else
                screenshotImg = new Utility.Screenshot().GetScreenshot();

            screenshotImg = new Utility.BicubicImageResize().ResizeImage(screenshotImg, new System.Drawing.Size(256, 144));
            BitmapSource screenStream = GetImageStream(screenshotImg);
            this.screenshot.Source = screenStream;

            System.Drawing.Image cursorImg = Input.Mouse.Cursor.GetCursorImg();
            BitmapSource cursorStream = GetImageStream(cursorImg);
            this.cursor.Source = cursorStream;
        }

        private static bool Equals(System.Drawing.Bitmap bmp1, System.Drawing.Bitmap bmp2)
        {
            if (!bmp1.Size.Equals(bmp2.Size))
            {
                return false;
            }
            for (int x = 0; x < bmp1.Width; ++x)
            {
                for (int y = 0; y < bmp1.Height; ++y)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public void Unsubscribe()
        {
            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }
    }
}
