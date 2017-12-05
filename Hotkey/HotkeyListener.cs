using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace SpongeBot.Hotkey
{
    class HotkeyListener : IDisposable
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HwndSource _source;
        private static int NEXT_HOTKEY_ID = 9000;
        private readonly int HOTKEY_ID = NEXT_HOTKEY_ID++;
        private readonly IntPtr handle;


        public HotkeyListener(IntPtr handle)
        {
            this.handle = handle;

            //Window mainWindow = Application.Current.MainWindow;
            //var helper = new WindowInteropHelper(mainWindow);
            //helper.EnsureHandle();  //get a handle witthout the need to show the window
            //this.handle = helper.Handle;

            //Register Hotkey:
            log.Debug($"Add Hook to handle: {this.handle} with ID: {HOTKEY_ID}.");
            _source = HwndSource.FromHwnd(this.handle);
            _source.AddHook(HwndHook);
        }

        public void Dispose()
        {
            log.Debug("Dispose HotkeyListener.");
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
        }

        public delegate void HotkeyPressed();
        public event HotkeyPressed OnHotKeyPressed;

        #region native hotkey stuff

        internal void RegisterHotKey(int modifier, int key)
        {
            UnregisterHotKey();

            log.Debug("Registering Hotkey.");
            if (!Utility.NativeMethods.RegisterHotKey(this.handle, HOTKEY_ID, (uint)modifier, (uint)key))
            {
                // handle error
                log.Error("Could not register hotkey!");
            }
        }

        internal void UnregisterHotKey()
        {
            log.Debug("Unregister Hotkey");
            Utility.NativeMethods.UnregisterHotKey(this.handle, HOTKEY_ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if(msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                OnHotKeyPressed();
                handled = true;
            }
            return IntPtr.Zero;
        }
        #endregion
    }
}
