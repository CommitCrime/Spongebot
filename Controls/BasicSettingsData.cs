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

namespace SpongeBot.Controls
{

    class BasicSettingsData : INotifyPropertyChanged
    {
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

        private string _processName = "Wow-64.exe";
        public String ProcessName
        {
            get { return _processName; }
            set
            {
                _processName = value;
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
            procChecker = new Timer(checkProcess, null, -1, -1); // init timer, but don`t start
            processTextfield.IsVisibleChanged += DependentUIElement_IsVisibleChanged;
            processTextfield.KeyUp += (a, b) => { procChecker.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1)); };
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

            Console.WriteLine($"Find process: {actualProcessName}.");
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
            IsBotRunning = false;
            IsActive = false;
            IsLoggedIn = false;

            WinSize = new Size();
            WinLoc = new Point();
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
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
