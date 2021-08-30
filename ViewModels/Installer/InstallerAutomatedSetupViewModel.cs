using System.Windows.Input;
using WSLManager.Commands.InstallerCommands;
using WSLManager.Models;

namespace WSLManager.ViewModels.Installer
{
    public class InstallerAutomatedSetupViewModel:BaseViewModel
    {
        private MainWindowViewModel _parent;
        private DistroModel _distroModel;
        private bool _isKali;

        public DistroModel DistroModel
        {
            get => _distroModel;
            set
            {
                _distroModel = value;
                OnPropertyChanged();
            }
        }

        public bool IsKali
        {
            get => _isKali;
            set
            {
                _isKali = value;
                OnPropertyChanged();
            }
        }

        public InstallerAutomatedSetupViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            InstallGuiToolsCommand = new InstallGuiToolsCommand(this);
        }

        public BaseViewModel SelectedViewModel
        {
            set
            {
                _parent.SelectedViewModel = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand InstallGuiToolsCommand { get; set; }


    }
}