using System.Windows;
using System.Windows.Controls;
using WSLManager.Windows;

namespace WSLManager.Views.MainWindow
{
    public partial class ModifyDistroToolsView : UserControl
    {
        public ModifyDistroToolsView()
        {
            InitializeComponent();
        }

        private void GuIButton_OnClick(object sender, RoutedEventArgs e)
        {
            InstallerWindow newInstallerWindow = new InstallerWindow();
            newInstallerWindow.Show();
        }
    }
}