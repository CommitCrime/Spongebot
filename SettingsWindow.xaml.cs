using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using SpongeBot.Utility;

namespace SpongeBot
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        LogWatcher logWatcher;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.SizeToContent = SizeToContent.Manual;
            logbox.Height = Double.NaN;
            // Create a LogFileWatcher to display the log and bind the log textbox to it
            logWatcher = new LogWatcher();
            logWatcher.Updated += logWatcher_Updated;
        }

        public void logWatcher_Updated(object sender, EventArgs e)
        {
            UpdateLogTextbox(logWatcher.LogContent);
        }

        public void UpdateLogTextbox(string value)
        {
            // Check whether invoke is required and then invoke as necessary
            if (!Dispatcher.CheckAccess())
            {
                UI.ExecuteAsync(() =>
                {
                    UpdateLogTextbox(value);
                });
                return;
            }

            // Set the textbox value
            logbox.Text = value;
            logbox.ScrollToEnd();
        }
    }
}
