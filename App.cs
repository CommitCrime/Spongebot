using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SpongeBot
{
    class App
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(typeof(App));
        [STAThread]
        static void Main(string[] args)
        {
            new App();
        }


        Application app;

        public App()
        {
            log.Info("Launch my app!");

            app = new Application();

            ResourceDictionary myResourceDictionary = new ResourceDictionary();
            myResourceDictionary.Source = new Uri("Themes/MetroDark/MetroDark.MSControls.Core.Implicit.xaml", UriKind.Relative);
            app.Resources.MergedDictionaries.Add(myResourceDictionary);

            //app.ShutdownMode = ShutdownMode.OnExplicitShutdown; //You couldn't launch a window after closing the first otherwise
            app.Startup += appStarted;
            app.Run();
        }

        private void appStarted(object sender, StartupEventArgs e)
        {
            log.Info("App started.");
            new SettingsWindow().Show();
        }
    }
}
