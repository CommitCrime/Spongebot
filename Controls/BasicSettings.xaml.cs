using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaktionslogik für WowInfo.xaml
    /// </summary>
    public partial class BasicSettings : UserControl
    { 
        public BasicSettings()
        {
            InitializeComponent();
            this.DataContext = new BasicSettingsData(procName);
        }

        private void procName_KeyUp(object sender, KeyEventArgs e)
        {
            // TODO chow tooltip when .exe or some path is entered
        }
    }
}
