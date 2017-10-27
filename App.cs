using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpongeBot
{
    class App
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Launch my app!");
            new App();
        }


        Application app;

        public App()
        {
            app = new Application();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown; //You couldn't launch a window after closing the first otherwise
            app.Startup += appStarted;
            app.Run();
        }

        private void appStarted(object sender, StartupEventArgs e)
        {
            new MainWindow().Show();
        }
    }
}
