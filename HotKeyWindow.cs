using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace SpongeBot
{
    public abstract class HotKeyWindow : Window
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HotKeyWindow()
        {
            //Register Hotkey:
            log.Debug("Create HotKeyWindow and reserve window handle.");
            var helper = new WindowInteropHelper(this);
            helper.EnsureHandle();  //get a handle witthout the need to show the window
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
        }


        internal abstract void OnHotKeyPressed();

        #region native hotkey stuff
        internal void RegisterHotKey(Utility.Hotkey.Modifier mod, Utility.Hotkey.KeyCode key)
        {
            log.Debug("Registering Hotkey.");
            var helper = new WindowInteropHelper(this);
            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, (uint)mod, (uint)key))
            {
                // handle error
                log.Error("Could not register hotkey!");
            }
        }

        internal void UnregisterHotKey()
        {
            log.Debug("Unregister Hotkey");
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }


        protected override void OnClosed(EventArgs e)
        {
            log.Debug("Closing.");
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            base.OnClosed(e);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:

                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource _source;
        private const int HOTKEY_ID = 9000;
        #endregion
    }
}
