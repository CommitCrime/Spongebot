using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SpongeBot
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(typeof(App));


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            log.Info("App started.");
        }


        public static object Execute(Func<object> someFunction)
        {
            return Application.Current.Dispatcher.Invoke(someFunction);
        }

        public static void ExecuteAsync(Action someFunction)
        {
            Application.Current.Dispatcher.BeginInvoke(someFunction);
        }
    }
}
