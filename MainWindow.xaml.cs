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
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //https://github.com/gmamaladze/globalmousekeyhook
        private IKeyboardMouseEvents m_GlobalHook;

        Timer fiveSeconds, botTimer;

        bool timerEnabled = false;



        public MainWindow() : base()
        {
            log.Debug("Initialize MainWindow");
            InitializeComponent();
            botTimer = new Timer(doFish, null, -1, -1);

            log.Debug("Initialize Hotkey");
            base.RegisterHotKey(Utility.Hotkey.Modifier.CTRL, Utility.Hotkey.KeyCode.KEY_A);

            log.Debug("Register global hook lib.");
            m_GlobalHook = Hook.GlobalEvents();
        }


        internal override void OnHotKeyPressed()
        {
            if(!timerEnabled)
            {
                log.Info("Enable Bot");
                timerEnabled = true;
                botTimer.Change(500, 20*1000);
                m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            }
            else
            {
                log.Info("Disable Bot");
                timerEnabled = false;
                botTimer.Change(-1, -1);
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            }
        }

        private void doFish(Object state)
        {
            try
            {

                // 1 Cast fishing
                log.Debug("Cast fishing");
                Input.Keyboard.ISendString send = new Input.Keyboard.User32_SendInput_VirtualKeycode();
                send.SendString("1");

                // 2 Find Bobber on screen
                Thread.Sleep(100);
                log.Debug("Try to find bobber.");
                Point hookPos = getHookPos();

                log.Info($"Found bobber at {hookPos.ToString()}");

                // 3 Listen for catch noise
                log.Debug("Start listening to audio output");
                log.Debug("Get audio device");
                // http://gigi.nullneuron.net/gigilabs/displaying-a-volume-meter-using-naudio/
                MMDeviceEnumerator SndDevEnum = new MMDeviceEnumerator();
                MMDevice SndDevice = SndDevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

                while (timerEnabled)
                {
                    double volume = SndDevice.AudioMeterInformation.MasterPeakValue * 100;
                    log.Verbose("Current volume is:" + volume);
                    if (volume > 20)
                    {
                        log.Info("Catch detected!");
                        break;
                    }
                    Thread.Sleep(100);
                }

                // 4 Click on Bobber
                log.Info($"Click bobber at {hookPos.ToString()}");
                new Input.Mouse.User32_MouseClick(new Input.Mouse.User32_MousePosition()).Left(hookPos);
            }
            catch (Exception e)
            {
                log.Error("Catching a fish failed", e);
            }
        }


        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            log.Debug("Registered mouse click");
            if (timerEnabled)
            {
                log.Debug("Restart fishing");
                botTimer.Change(1500, 20 * 1000);
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            log.Debug("Window is closing. Dispose global Input hook.");
            
            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }


        private void Screenshot_in5(object sender, RoutedEventArgs e)
        {
            log.Debug("Will take a screenshot in 5s");

            if (fiveSeconds != null)
                fiveSeconds.Dispose();

            fiveSeconds = new Timer((state) =>
            {
                UI.ExecuteAsync(() => { Screenshot_Now(sender, e); });
            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
        }

        private Point getHookPos()
        {
            //Point F_initialMousePos = MouseInteropHelper.getCursorPos();
            Input.Mouse.User32_MousePosition mousePos = new Input.Mouse.User32_MousePosition();

            double screenWidth = UI.getActualPrimaryScreenWidth();
            double screenHeight = UI.getActualPrimaryScreenHeight();

            Point rectLocation = mousePos.GetCursorPos(); //get MousePos --> center of new rect
            log.Debug($"Start searching for hook at {rectLocation.ToString()}.");
            rectLocation.Offset(-screenWidth / 4, -screenWidth / 4); //move point to use it as rect location

            Rect halfScreenWidthSquare = new Rect(rectLocation, new Size(screenWidth / 2, screenWidth / 2));
            log.Debug($"Search for hook in area: {halfScreenWidthSquare.ToString()}");

            IEnumerator<Point> coords = new CoordinateProvider.RectSpiral(halfScreenWidthSquare);

            while(coords.MoveNext() && timerEnabled)
            {
                // 1 Move Cursor
                Point p = coords.Current;
                log.Trace($"Set mouse pos at {p.ToString()}.");
                mousePos.SetCursorPos(p);

                Thread.Sleep(10);

                // 2 Compare Cursor
                if (Equals(Input.Mouse.Cursor.GetCursorImg(), SpongeBot.Properties.Resources.HookCursor))
                {
                    log.Info($"Found bobber at {p.ToString()}). Wait and recheck.");
                    //p = new Point(p.X + 30, p.Y + 30); // works well @ 1080p & first person view & line by line 
                    //mousePos.SetCursorPos(p);
                    Thread.Sleep(250); //cursor might change due to bobber movement when mouse is at the very corner
                    if (Equals(Input.Mouse.Cursor.GetCursorImg(), SpongeBot.Properties.Resources.HookCursor))
                    {
                        log.Notice($"Found bobber at {p.ToString()}).");
                        return p;
                    }
                }
            }

            throw new Exception("No Bobber found in area.");
        }

        private void Screenshot_Now(object sender, RoutedEventArgs e)
        {
            log.Debug("Taking a screenshot " + (this.includeCursor.IsChecked == true ? "with" : "without") + " Cursor.");
            System.Drawing.Image screenshotImg;
            if (this.includeCursor.IsChecked == true)
                screenshotImg = new Input.Mouse.CursorScreenshot().GetScreenshot();
            else
                screenshotImg = new Utility.Screenshot().GetScreenshot();

            //screenshotImg = new Utility.BicubicImageResize().ResizeImage(screenshotImg, new System.Drawing.Size(256, 144));
            BitmapSource screenStream = SpongeBot.Utility.NativeMethods.GetImageStream(screenshotImg);
            this.screenshot.Source = screenStream;

            System.Drawing.Image cursorImg = Input.Mouse.Cursor.GetCursorImg();
            BitmapSource cursorStream = SpongeBot.Utility.NativeMethods.GetImageStream(cursorImg);
            this.cursor.Source = cursorStream;
        }

        private void Draw_Spiral_Click(object sender, RoutedEventArgs e)
        {
            Rect area = new Rect(new Size(this.screenshot.Width, this.screenshot.Height));
            
            new PreviewWindow(new CoordinateExample(new CoordinateProvider.ArchimedeanSpiral(area), area).getGif()).Show();
        }

        private bool Equals(System.Drawing.Bitmap bmp1, System.Drawing.Bitmap bmp2)
        {
            if (!bmp1.Size.Equals(bmp2.Size))
            {
                log.Debug("Bitmaps are not equal, as their size does not match.");
                return false;
            }
            for (int x = 0; x < bmp1.Width; ++x)
            {
                for (int y = 0; y < bmp1.Height; ++y)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                    {
                        log.Trace($"Bitmaps are not equal. Pixel ({x},{y}) does not match.");
                        return false;
                    }
                }
            }

            log.Debug("Bitmaps are equal");
            return true;
        }
    }
}
