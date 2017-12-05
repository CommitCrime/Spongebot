using SpongeBot.Utility;
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

            initHotkeyModCmb();
        }

        private void initHotkeyModCmb()
        {
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();

            Type enumType = typeof(SpongeBot.Utility.Hotkey.Modifier);
            var test = Enum.GetValues(enumType);
            foreach (SpongeBot.Utility.Hotkey.Modifier i in Enum.GetValues(enumType))
            {
                String name = Enum.GetName(enumType, i);
                cbp.Add(new ComboBoxPairs(name, (short)i));
            }

            cmbHotkey1.DisplayMemberPath = "_Key";
            cmbHotkey1.SelectedValuePath = "_Value";
            cmbHotkey1.ItemsSource = cbp;

            cmbHotkey2.DisplayMemberPath = "_Key";
            cmbHotkey2.SelectedValuePath = "_Value";
            cmbHotkey2.ItemsSource = cbp;
        }

        private void procName_KeyUp(object sender, KeyEventArgs e)
        {
            // TODO show tooltip when .exe or some path is entered
        }
    }
}
