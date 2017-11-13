using System;
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
using System.Windows.Shapes;

namespace SpongeBot
{
    /// <summary>
    /// Interaktionslogik für PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        public PreviewWindow(BitmapSource imgSource)
        {
            InitializeComponent();
            this.img.Source = imgSource;
            this.img.Width = imgSource.Width;
            this.img.Height = imgSource.Height;
        }
    }
}
