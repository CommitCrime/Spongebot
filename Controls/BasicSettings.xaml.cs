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

            initHotkeyCmb(cmbHotkey1, typeof(Utility.Hotkey.Modifier));
            initHotkeyCmb(cmbHotkey2, typeof(Utility.Hotkey.Modifier), includeNone: true);
            initHotkeyCmb(cmbHotkeyKey, typeof(Utility.Hotkey.KeyCode));
        }

        private void initHotkeyCmb(ComboBox cmb, Type enumType, bool includeNone = false)
        {
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();

            var test = Enum.GetValues(enumType);

            if (includeNone)
            {
                cbp.Add(new ComboBoxPairs(null, 0));
            }

            foreach (var i in Enum.GetValues(enumType)) {
                String name = Enum.GetName(enumType, i);
                cbp.Add(new ComboBoxPairs(name, (short)i));
            }

            cmb.DisplayMemberPath = "_Key";
            cmb.SelectedValuePath = "_Value";
            cmb.ItemsSource = cbp;
        }

        private void procName_KeyUp(object sender, KeyEventArgs e)
        {
            // TODO show tooltip when .exe or some path is entered
        }
    }
}
