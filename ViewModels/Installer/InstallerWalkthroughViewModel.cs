using System.Windows.Input;
using WSLManager.Commands;

namespace WSLManager.ViewModels.Installer
{
    public class InstallerWalkthroughViewModel:BaseViewModel
    {
        public InstallerWalkthroughViewModel(MainWindowViewModel parent)
        {
            
            UpdateViewCommand = new UpdateViewCommand(parent);
        }

        public ICommand UpdateViewCommand { get; set; }
    }
}