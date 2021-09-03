using System.Collections.ObjectModel;
using System.Windows.Input;
using WSLManager.Commands;
using WSLManager.Commands.DistroLaunchCloseCommands;
using WSLManager.Models;

namespace WSLManager.ViewModels
{
    public class DistroLaunchCloseViewModel : BaseViewModel
    {
        private int _userLoginType = 0;
        private string _userName = "User Name";
        private MainWindowViewModel _parent;
        private bool _userNameFieldCommand = false;

        private ObservableCollection<DistroModel> _launchCloseDistroCollection;
        
        /// <summary>
        /// Gets/Sets wether of not the username field should be accessable
        /// </summary>
        public bool UserNameFieldCommand
        {
            get => _userNameFieldCommand;
            set
            {
                _userNameFieldCommand = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        ///  Gets/sets the UserLogin type
        /// </summary>
        public int UserLoginType
        {
            get => _userLoginType;
            set
            {
                _userLoginType = value;
                if ((LoginOptions)_userLoginType == LoginOptions.SpecificUser)
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
        
        /// <summary>
        /// Gets/sets The Username used to login to the distro
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent"></param>
        public DistroLaunchCloseViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            StartDistroCommand = new StartDistroCommand(this, _parent);
            StopSelectedDistroCommand = new StopSelectedDistroCommand(this,_parent);
            StopAllDistrosCommand = new StopAllDistrosCommand(this, _parent);
            
            UpdateViewCommand = new UpdateViewCommand(_parent);
        }


        //Commands binded to buttons
        public ICommand StartDistroCommand { get; set; }
        public ICommand StopSelectedDistroCommand { get; set; }
        public ICommand StopAllDistrosCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }
    }
}