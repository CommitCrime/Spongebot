using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using SpongeBot.Bot;
using SpongeBot.Hotkey;
using SpongeBot.Utility;

namespace SpongeBot.Controls
{

    class BasicSettingsData : INotifyPropertyChanged
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HotkeyListener hotkeyListener;
        #region fiels and properties
        Timer procChecker;

        bool _isRunning = false;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                NotifyPropertyChanged();
            }
        }

        bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                NotifyPropertyChanged();
            }
        }

        bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                NotifyPropertyChanged();
            }
        }

        bool _isBotRunning = false;
        public bool IsBotRunning
        {
            get { return _isBotRunning; }
            set
            {
                _isBotRunning = value;
                NotifyPropertyChanged();
            }
        }

        private string _processName = Properties.Settings.Default.ProcessName;
        public String ProcessName
        {
            get { return _processName; }
            set
            {
                _processName = value;
                NotifyPropertyChanged();
            }
        }

        private string _hotkeyAction = Properties.Settings.Default.HotkeyAction;
        public String HotkeyAction
        {
            get { return _hotkeyAction; }
            set
            {
                _hotkeyAction = value;
                NotifyPropertyChanged();
            }
        }

        private ComboBoxPairs _hotkeyMod1 = new ComboBoxPairs(Properties.Settings.Default.HotkeyMod1, (short)Enum.Parse(typeof(Utility.Hotkey.Modifier), Properties.Settings.Default.HotkeyMod1));
        public ComboBoxPairs HotkeyMod1
        {
            get { return _hotkeyMod1; }
            set
            {
                _hotkeyMod1 = value;
                NotifyPropertyChanged();
            }
        }

        private ComboBoxPairs _hotkeyMod2 = new ComboBoxPairs(Properties.Settings.Default.HotkeyMod2, String.IsNullOrWhiteSpace(Properties.Settings.Default.HotkeyMod2) ? 0 : (short)Enum.Parse(typeof(Utility.Hotkey.Modifier), Properties.Settings.Default.HotkeyMod2));
        public ComboBoxPairs HotkeyMod2
        {
            get { return _hotkeyMod2; }
            set
            {
                _hotkeyMod2 = value;
                NotifyPropertyChanged();
            }
        }

        private ComboBoxPairs _hotkeyKey = new ComboBoxPairs(Properties.Settings.Default.HotkeyKey, (short)Enum.Parse(typeof(Utility.Hotkey.KeyCode), Properties.Settings.Default.HotkeyKey));
        public ComboBoxPairs HotkeyKey
        {
            get { return _hotkeyKey; }
            set
            {
                _hotkeyKey = value;
                NotifyPropertyChanged();
            }
        }


        Point _winLoc = new Point();
        public Point WinLoc
        {
            get { return _winLoc; }
            set
            {
                _winLoc = value;
                NotifyPropertyChanged();
            }
        }

        Size _winSize = new Size();
        private Bot.Bot bot;

        public Size WinSize
        {
            get { return _winSize; }
            set
            {
                _winSize = value;
                NotifyPropertyChanged();
            }
        }

        #endregion


        public BasicSettingsData(System.Windows.Controls.TextBox processTextfield)
        {
            var helper = new WindowInteropHelper(Application.Current.MainWindow);
            helper.EnsureHandle();  //get a handle witthout the need to show the window
            hotkeyListener = new Hotkey.HotkeyListener(helper.Handle);
            
            
            this.bot = new Bot.Bot(new Input.Keyboard.User32_SendInput_VirtualKeycode());
            hotkeyListener.OnHotKeyPressed += () => {
                if (this.IsBotRunning)
                    bot.Stop();
                else
                    bot.Start(this.HotkeyAction);

                this.IsBotRunning = !this.IsBotRunning;
            };

            Application.Current.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
            procChecker = new Timer(checkProcess, null, -1, -1); // init timer, but don`t start
            processTextfield.IsVisibleChanged += DependentUIElement_IsVisibleChanged;
            processTextfield.KeyUp += (a, b) => { procChecker.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1)); };
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void DependentUIElement_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                //Is now visible -> start timer
                procChecker.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
            }
            else
            {
                //Not visible anymore, stop timer
                procChecker.Change(-1, -1);
                procChecker.Dispose();
            }
        }

        private void checkProcess(object state)
        {
            notFound();

            if (String.IsNullOrWhiteSpace(ProcessName))
            {
                return;
            }

            String actualProcessName = Path.GetFileNameWithoutExtension(ProcessName);

            log.Trace($"Try to find process: {actualProcessName}.");
            Process[] processes = Process.GetProcessesByName(actualProcessName);
            IsRunning = processes?.Length > 0;
            IntPtr? winHandle = processes?.FirstOrDefault()?.MainWindowHandle;

            if (winHandle.HasValue)
            {
                RECT winBounds;
                GetWindowRect(new HandleRef(this, winHandle.Value), out winBounds);
                WinLoc = new Point(winBounds.Left, winBounds.Top);
                WinSize = new Size(winBounds.Right - winBounds.Left, winBounds.Bottom - winBounds.Top);

                IntPtr activeWindow = GetForegroundWindow();
                IsActive = activeWindow == winHandle.Value;
            }

        }

        private void notFound()
        {
            IsRunning = false;
            IsActive = false;
            IsLoggedIn = false;

            WinSize = new Size();
            WinLoc = new Point();
        }



        internal void saveSettings()
        {
            Properties.Settings.Default.ProcessName = ProcessName;
            Properties.Settings.Default.HotkeyAction = HotkeyAction;
            Properties.Settings.Default.HotkeyMod1 = HotkeyMod1._Key;
            Properties.Settings.Default.HotkeyMod2 = HotkeyMod2._Key;
            Properties.Settings.Default.HotkeyKey = HotkeyKey._Key;
            Properties.Settings.Default.Save();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        #region property events
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (propertyName.Contains("Hotkey"))
                hotkeyListener.RegisterHotKey(HotkeyMod1._Value | HotkeyMod2._Value, HotkeyKey._Value);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
