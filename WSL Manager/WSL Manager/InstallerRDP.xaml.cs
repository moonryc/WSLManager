using System.Windows;

namespace WSL_Manager
{
    /// <summary>
    ///     Interaction logic for PopUpWindow.xaml
    /// </summary>
    public partial class InstallerRDP : Window
    {
        public InstallerRDP()
        {
            InitializeComponent();
            OnWindowLoad();
        }

        private void OnWindowLoad()
        {
            //Display First Frame
            InstallerFrame.NavigationService.Navigate(new InstallPage2());
        }
    }
}