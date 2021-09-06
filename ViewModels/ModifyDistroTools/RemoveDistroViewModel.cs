using System.Windows.Input;
using WSLManager.Commands;
using WSLManager.Commands.ImportExportRemoveDistro;
using WSLManager.Models;

namespace WSLManager.ViewModels.ModifyDistroTools
{
    public class RemoveDistroViewModel:BaseViewModel
    {
        private MainWindowViewModel _parent;
        private DistroModel _selectedDistro;
        private string _messageText="";
        private bool _isIndetermiante = false;

        public MainWindowViewModel MainWindowViewModel { get=>_parent; }
        
        /// <summary>
        /// Gets/sets the Selected distro
        /// </summary>
        public DistroModel SelectedDistro
        {
            get => _selectedDistro;
            set
            {
                _selectedDistro = value;
                OnPropertyChanged();
            }
        }

        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged();
            }
        }

        public bool IsIndeterminate
        {
            get => _isIndetermiante;
            set
            {
                _isIndetermiante = value;
                OnPropertyChanged();
            }
        }

        public RemoveDistroViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent);
            RemoveDistroCommand = new RemoveDistroCommand(this);
        }

        public ICommand UpdateViewCommand { get; set; }
        public ICommand RemoveDistroCommand { get; set; }
    }
}