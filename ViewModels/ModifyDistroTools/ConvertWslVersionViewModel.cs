using System.Windows.Input;
using WSLManager.Commands;
using WSLManager.Commands.ConvertWslCommands;
using WSLManager.Models;

namespace WSLManager.ViewModels.ModifyDistroTools
{
    public class ConvertWslVersionViewModel: BaseViewModel
    {

        private MainWindowViewModel _parent;
        private DistroModel _selectedDistroModel;
        private string _messageOutput="";
        
        public ICommand UpdateViewCommand { get; set; }
        public ICommand ConvertDistroCommand { get; set; }

        public DistroModel SelectedDistroModel
        {
            get=>_selectedDistroModel;
            set
            {
                _selectedDistroModel = value;
                OnPropertyChanged();
            }
        }


        public ConvertWslVersionViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(parent);
            ConvertDistroCommand = new ConvertDistroCommand(this);
        }

        public string MessageOutput
        {
            get => _messageOutput;
            set
            {
                _messageOutput = value;
                OnPropertyChanged();
            }
        }


    }
}