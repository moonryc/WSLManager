using System.Windows.Input;
using WSLManager.Commands;

namespace WSLManager.ViewModels.Installer
{
    public class InstallerIntroViewModel:BaseViewModel
    {
        public InstallerIntroViewModel(MainWindowViewModel parent)
        {
            UpdateViewCommand = new UpdateViewCommand(parent);
        }

        public ICommand UpdateViewCommand { get; set; }
    }
}