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

namespace SpongeBot
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            //img.Source = EquiDistantArchimedeanSpiral.getImage();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logbox.Height = Double.NaN;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            logbox.Height = Double.NaN;
        }
    }
}
