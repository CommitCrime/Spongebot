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

        public void logWatcher_Updated(object sender, LogEventArgs e)
        {
            UpdateLogTextbox(e.LogEvent);
        }

        public void UpdateLogTextbox(log4net.Core.LoggingEvent ev)
        {
            // Check whether invoke is required and then invoke as necessary
            if (!Dispatcher.CheckAccess())
            {
                UI.ExecuteAsync(() =>
                {
                    UpdateLogTextbox(ev);
                });
                return;
            }

            // Construct the line we want to log
            string line = ev.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss,fff") + " [" + ev.ThreadName + "] " + ev.Level + " " + ev.LoggerName + ": " + ev.RenderedMessage;
            var paragraph = new Paragraph();
            Run logMessage = new Run(line);

            if (ev.Level == log4net.Core.Level.Trace)
                return;
            else if (ev.Level == log4net.Core.Level.Debug)
                logMessage.Foreground = System.Windows.Media.Brushes.Green;
            else if (ev.Level == log4net.Core.Level.Info)
                logMessage.Foreground = System.Windows.Media.Brushes.DarkGoldenrod;
            else if (ev.Level == log4net.Core.Level.Warn)
                logMessage.Foreground = System.Windows.Media.Brushes.OrangeRed;
            else if (ev.Level == log4net.Core.Level.Error)
                logMessage.Foreground = System.Windows.Media.Brushes.Red;


            paragraph.Inlines.Add(logMessage);
            logbox.Document.Blocks.Add(paragraph);

            logbox.ScrollToEnd();
        }
    }
}
