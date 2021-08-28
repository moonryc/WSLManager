using System.Windows.Input;
using WSLManager.Commands;
using WSLManager.Commands.DistroLaunchCloseCommands;
using WSLManager.Models;

namespace WSLManager.ViewModels
{
    public class DistroLaunchCloseViewModel : BaseViewModel
    {
        private int _userLoginType = (int)LoginOptions.SelectLoginMethod;
        private string _userName = "User Name";
        private int _selectedDistroIndex = 0;
        private DistroModel _selectedDistroModel;
        private MainWindowViewModel _parent;
        private bool _userNameFieldCommand = false;

        public MainWindowViewModel Parent
        {
            get => _parent;
        }

        public int SelectedDistroIndex
        {
            get => _selectedDistroIndex;
            set
            {
                _selectedDistroIndex = value;
                if ((LoginOptions)_selectedDistroIndex == LoginOptions.SpecificUser)
                {
                    UserNameFieldCommand = true;
                }
                else
                {
                    UserNameFieldCommand = false;
                }
                OnPropertyChanged();
            }
        }

        //TODO: FIX THIS
        public bool UserNameFieldCommand
        {
            get => _userNameFieldCommand;
            set
            {
                _userNameFieldCommand = value;
                OnPropertyChanged();
            }
        }
        public DistroModel SelectedDistroModel
        {
            get => _selectedDistroModel;
            set
            {
                _selectedDistroModel = value;
                OnPropertyChanged();
            }
        }
        public int UserLoginType
        {
            get => _userLoginType;
            set
            {
                _userLoginType = value;
                OnPropertyChanged();
            }
        }
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public DistroLaunchCloseViewModel(MainWindowViewModel parent)
        {
            StartDistroCommand = new StartDistroCommand(this);
            StopSelectedDistroCommand = new StopSelectedDistroCommand(this);
            StopAllDistrosCommand = new StopAllDistrosCommand(this);
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent);
        }


        public ICommand StartDistroCommand { get; set; }
        public ICommand StopSelectedDistroCommand { get; set; }
        public ICommand StopAllDistrosCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }
    }
}