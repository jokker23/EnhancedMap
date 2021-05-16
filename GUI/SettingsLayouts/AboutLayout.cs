using System.Diagnostics;
using System.Windows.Forms;

namespace EnhancedMap.GUI.SettingsLayouts
{
    public partial class AboutLayout : UserControl
    {
        public AboutLayout()
        {
            InitializeComponent();

            labelVersion.Text = "Version: " + MainCore.MapVersion;
        }
    }
}