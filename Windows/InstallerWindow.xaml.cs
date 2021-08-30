using System.Windows;
using WSLManager.ViewModels.Installer;

namespace WSLManager.Windows
{
    public partial class InstallerWindow : Window
    {
        public InstallerWindow()
        {
            InitializeComponent();
            DataContext = new InstallerWindowViewModel();
        }
    }
}