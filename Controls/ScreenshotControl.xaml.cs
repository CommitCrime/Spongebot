using SpongeBot.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SpongeBot.Controls
{
    /// <summary>
    /// Interaktionslogik für ScreenshotControl.xaml
    /// </summary>
    public partial class ScreenshotControl : UserControl
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Timer delay;

        public ScreenshotControl()
        {
            InitializeComponent();
        }

        private void Screenshot(object sender, RoutedEventArgs e)
        {
            log.Debug($"Will take a screenshot in {this.slDelay.Value}s");
            executeDelayed(() =>
            {
                new PreviewWindow(new Utility.Screen.Screenshot().GetScreenshot(), 0.5).Show();
            });
        }

        private void CursorScreenshot(object sender, RoutedEventArgs e)
        {
            log.Debug($"Taking a screenshot with cursor in {this.slDelay.Value}s");
            executeDelayed(() =>
            {
                new PreviewWindow(new Input.Mouse.CursorScreenshot().GetScreenshot(), 0.5).Show();
            });
        }

        private void executeDelayed(Action exec)
        {
            if (delay != null)
                delay.Dispose();

            delay = new Timer((state) =>
            {
                App.ExecuteAsync(exec);
            }, null, TimeSpan.FromSeconds(this.slDelay.Value), TimeSpan.FromMilliseconds(-1));
        }

        private void Cursorshot(object sender, RoutedEventArgs e)
        {
            log.Debug($"Taking a image of the cursor {this.slDelay.Value}s.");
            executeDelayed(() =>
            {
                new PreviewWindow(Input.Mouse.Cursor.GetCursorImg()).Show();
            });
        }
    }
}
