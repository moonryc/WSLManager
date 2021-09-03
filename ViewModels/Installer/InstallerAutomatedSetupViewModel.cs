using System.Collections.ObjectModel;
using System.Windows.Input;
using WSLManager.Commands;
using WSLManager.Commands.InstallerCommands;
using WSLManager.Models;

namespace WSLManager.ViewModels.Installer
{
    public class InstallerAutomatedSetupViewModel:BaseViewModel
    {
        private MainWindowViewModel _parent;
        private DistroModel _distroModel;
        private bool _isKali;

        /// <summary>
        /// Gets/Sets the selected Distro Model
        /// </summary>
        public DistroModel DistroModel
        {
            get => _distroModel;
            set
            {
                _distroModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DistroModel> InstallerDistroCollection { get=>_parent.DistroCollection;  }
        
        /// <summary>
        /// Gets/Sets wether or not its a kali distro 
        /// </summary>
        public bool IsKali
        {
            get => _isKali;
            set
            {
                _isKali = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sets the selected view model
        /// </summary>
        public BaseViewModel SelectedViewModel
        {
            set
            {
                _parent.SelectedViewModel = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        public InstallerAutomatedSetupViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            InstallGuiToolsCommand = new InstallGuiToolsCommand(this);
            UpdateViewCommand = new UpdateViewCommand(_parent);
        }

        //Binded commands
        public ICommand InstallGuiToolsCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }


    }
}