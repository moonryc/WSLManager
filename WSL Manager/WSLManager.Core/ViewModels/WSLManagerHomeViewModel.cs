using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WSLManager.Core.Commands;
using WSLManager.Core.Models;
using WSLManager.Core.ViewModels.Installer;

namespace WSLManager.Core.ViewModels
{
    public class WSLManagerHomeViewModel:MvxViewModel
    {

        private ObservableCollection<DistroModel> _distroListComboBox = new ObservableCollection<DistroModel>();
        private ObservableCollection<string> _userListComboBox = new ObservableCollection<string>(){"Select User","Default User","root","User" };
        
        private string _selectedUser;
        
        
        private string _loginUsername;
        private Dictionary<string, DistroInstance> _storeDistroInstances = new Dictionary<string, DistroInstance>();
        
            
        public ObservableCollection<DistroModel> DistroListComboBox { get=>_distroListComboBox; set=>SetProperty(ref _distroListComboBox,value); }
        public ObservableCollection<string> UserListComboBox { get=>_userListComboBox; set=>SetProperty(ref _userListComboBox,value); }
        private DistroModel _selectedDistro;
        
        #region Button On/Off props/getters
        
        private bool _isEnabledSelectUser;
        private bool _isEnabledUserName;
        
        private bool _isEnabledStartSelectedDistro;
        private bool _isEnabledCloseSelectedDistro;
        private bool _isEnabledCloseAllDistro;
        
        public DistroModel SelectedDistro
        {
            get => _selectedDistro;
            set
            {
                SetProperty(ref _selectedDistro, value);
                if (!_selectedDistro.DistroListItem.Contains("Select"))
                {
                    IsEnabledSelectUser = true;
                }
                else
                {
                    IsEnabledSelectUser = false;
                }
                ButtonUpdates();
            }
        }
        public string SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
                ButtonUpdates();
            }
        }
        public string LoginUsername
        {
            get=>_loginUsername;
            set
            {
                SetProperty(ref _loginUsername, value);
                ButtonUpdates();
            }
        }

        
        public bool IsEnabledSelectUser { get=> _isEnabledSelectUser; set=> SetProperty(ref _isEnabledSelectUser, value); }
        public bool IsEnabledUserName { get=> _isEnabledUserName; set=> SetProperty(ref _isEnabledUserName, value); }
        public bool IsEnabledStartSelectedDistro { get=>_isEnabledStartSelectedDistro; set=>SetProperty(ref _isEnabledStartSelectedDistro,value); }
        public bool IsEnabledCloseSelectedDistro { get=>_isEnabledCloseSelectedDistro; set=>SetProperty(ref _isEnabledCloseSelectedDistro,value); }
        public bool IsEnabledCloseAllDistro { get=>_isEnabledCloseAllDistro; set=>SetProperty(ref _isEnabledCloseAllDistro,value); }


        #region Buttons On/Off

        private void ButtonUpdates()
        {
            OnOffUsername();
            OnOffSelectUser();
            OnOffStartDistro();
            OnOffCloseSelectedDistro();
            OnOffCloseAllDistros();
        }
        
        private void OnOffSelectUser()
        {
            if (SelectedDistro.DistroListItem.Contains("Select"))
            {
                IsEnabledSelectUser = false;
                IsEnabledUserName = false;
                IsEnabledStartSelectedDistro = false;
                IsEnabledCloseSelectedDistro = false;
            }
            else
            {
                IsEnabledSelectUser = true;
            }
        }
        private void OnOffUsername()
        {
            if (SelectedUser.Equals("User"))
            {
                IsEnabledUserName = true;
            }
            else
            {
                IsEnabledUserName = false;
            }
        }
        private void OnOffStartDistro()
        {
            switch (SelectedUser)
            {
                case "Select User":
                    IsEnabledStartSelectedDistro = false;
                    break;
                case "Default User":
                    if (SelectedDistro.DistroListItem.Contains("Select"))
                    {
                        IsEnabledStartSelectedDistro = false;
                    }
                    else
                    {
                        IsEnabledStartSelectedDistro = true;
                    }
                    break;
                case "root":
                    if (SelectedDistro.DistroListItem.Contains("Select"))
                    {
                        IsEnabledStartSelectedDistro = false;
                    }
                    else
                    {
                        IsEnabledStartSelectedDistro = true;
                    }
                    break;
                case "User":
                    if (string.IsNullOrEmpty(LoginUsername) || string.IsNullOrWhiteSpace(LoginUsername) || SelectedDistro.DistroListItem.Contains("Select"))
                    {
                        IsEnabledStartSelectedDistro = false;    
                    }
                    else
                    {
                        IsEnabledStartSelectedDistro = true;
                    }
                    break;
            }
        }
        private void OnOffCloseSelectedDistro()
        {
            if (SelectedDistro.DistroStatus.Equals("Running"))
            {
                IsEnabledCloseSelectedDistro = true;
            }
            else
            {
                IsEnabledCloseSelectedDistro = false;
            }
        }
        private void OnOffCloseAllDistros()
        {
            bool temp = false;
            foreach (DistroModel distro in _distroListComboBox)
            {
                if (distro.DistroStatus.Equals("Running"))
                {
                    temp = true;
                    break;
                }
            }
            IsEnabledCloseAllDistro = temp;
        }
        
        #endregion
        
        
        #endregion
        
        #region Button Command Getters
        
        //Starting and ending distros
        public IMvxAsyncCommand StartSelectedDistroCommand => new MvxAsyncCommand(StartSelectedDistro);
        public IMvxAsyncCommand CloseSelectedDistroCommand => new MvxAsyncCommand(CloseSelectedDistro);
        public IMvxAsyncCommand CloseAllDistroCommand => new MvxAsyncCommand(CloseAllDistro);
        
        //Advanced tools
        public IMvxAsyncCommand AdvanceToolsHelpCommand=> new MvxAsyncCommand(AdvanceToolsHelp);
        public IMvxAsyncCommand ChangeWslVersionCommand=> new MvxAsyncCommand(ChangeWslVersion);
        public IMvxAsyncCommand InstallGuiToolsCommand => new MvxAsyncCommand(InstallerPage1);
        public IMvxAsyncCommand OpenDistroGuiCommand => new MvxAsyncCommand(OpenDistroGui);
        public IMvxAsyncCommand ImportDistroCommand => new MvxAsyncCommand(ImportDistro);
        public IMvxAsyncCommand UnregisterDistroCommand => new MvxAsyncCommand(UnregisterDistro);
        
        #endregion
        
        #region Button Left SideCommands

        private async Task StartSelectedDistro()
        {
            switch (SelectedUser)
            {
                case "Default User":
                    _storeDistroInstances[SelectedDistro.DistroName].StartDistroInstance();
                    break;
                case "root":
                    _storeDistroInstances[SelectedDistro.DistroName].StartDistroInstanceUser("root");
                    break;
                case "User":
                    _storeDistroInstances[SelectedDistro.DistroName].StartDistroInstanceUser(LoginUsername);
                    break;
            }
            //IsEnabledCloseSelectedDistro = true;
        }        

        private async Task CloseSelectedDistro()
        {
            _storeDistroInstances[SelectedDistro.DistroName].StopDistroInstance();
            //Todo MAKE THIS WORK
        }
        
        private async Task CloseAllDistro()
        {
            //TODO MAKE CLOSE ALL DISTROS WORK
        }
        
        #endregion
        
        
        
        
        public override async Task Initialize()
        {
            await base.Initialize();

            //Thread UpdateDistroComboBoxThread = new Thread();
            
            UpdateDistroComboBox();
            
            //UpdateDistroComboBoxThread.Start();
            //await Task.Run(() => { UpdateDistroComboBox(); });
        }

        

        #region ComboBox Distro Methods
        
        private string RunningOrStoppedConverted(bool isRunning)
        {
            if(isRunning){
                return "Running";
            }else
            {
                return "Stopped";
            }
        }

        private void UpdateDistroComboBox()
        {
            _distroListComboBox.Add(new DistroModel(){DistroName = "Select Distro", DistroStatus = null, DistroVersion = null});
            
                List<string[]> distroList = DataCommands.GenerateListOfDistros();
                foreach (string[] distro in distroList)
                {
                    string name = distro[0];
                    bool status = distro[1].Equals("Running");
                    int version = int.Parse(distro[2]);
                    
                    //if it is an old distro
                    if (_storeDistroInstances.ContainsKey(name))
                    {
                        //
                        if (_storeDistroInstances[name].IsRunning != status || _storeDistroInstances[name].Version != version)
                        {
                            _distroListComboBox.Remove(new DistroModel() {
                                DistroName = name, 
                                DistroStatus = RunningOrStoppedConverted(_storeDistroInstances[name].IsRunning),
                                DistroVersion = $"{_storeDistroInstances[name].Version}"});
                            _storeDistroInstances[name].IsRunning = status;
                            _storeDistroInstances[name].Version = version;
                            _distroListComboBox.Add(new DistroModel() {DistroName = name,DistroStatus = distro[1],DistroVersion = distro[2]});
                        }
                        
                    }
                    else
                    {
                        //if its a new distro
                        _storeDistroInstances.Add(name, new DistroInstance(name, status,version));
                        _distroListComboBox.Add(new DistroModel(){DistroName = name,DistroStatus = distro[1],DistroVersion = distro[2]});
                    }
                }
        }

        #endregion
        
        

        #region Advance Tools

        private async Task AdvanceToolsHelp()
        {
            await _navigationService.Navigate<AdvanceMoreInfoViewModel>();
        }
        
        private async Task ChangeWslVersion()
        {
            await _navigationService.Navigate<ChangeWslVersionViewModel>();
        }
        
        private async Task InstallerPage1(){
            await _navigationService.Navigate<InstallPage1ViewModel>();
        }
        
        private async Task OpenDistroGui()
        {
            //TODO MAKE OPEN DISTRO GUI
        }
        
        private async Task ImportDistro()
        {
            await _navigationService.Navigate<ImportDistroViewModel>();
        }
        
        private async Task UnregisterDistro()
        {
            await _navigationService.Navigate<UnRegisterADistroViewModel>();
        }

        #endregion

        #region Navigation
        
        private readonly IMvxNavigationService _navigationService;
        
        public WSLManagerHomeViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        #endregion
    }
}