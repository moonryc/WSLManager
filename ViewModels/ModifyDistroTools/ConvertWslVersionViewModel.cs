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

        /// <summary>
        /// Gets/Sets the selected distro model
        /// </summary>
        public DistroModel SelectedDistroModel
        {
            get=>_selectedDistroModel;
            set
            {
                _selectedDistroModel = value;
                OnPropertyChanged();
            }
        }
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        public ConvertWslVersionViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(parent);
            ConvertDistroCommand = new ConvertDistroCommand(this);
        }

        /// <summary>
        /// Gets/Sets the message output
        /// </summary>
        public string MessageOutput
        {
            get => _messageOutput;
            set
            {
                _messageOutput = value;
                OnPropertyChanged();
            }
        }

        //binded commands
        public ICommand UpdateViewCommand { get; set; }
        public ICommand ConvertDistroCommand { get; set; }
    }
}