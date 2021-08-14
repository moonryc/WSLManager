using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WSLManager.Core.Commands;
using WSLManager.Core.Models;
using WSLManager.Core.ViewModels.Installer;

namespace WSLManager.Core.ViewModels
{

    public class WSLManagerHomeViewModel:MvxViewModel<DistroInstanceBankNavigationArgs>
    {
        private bool updateDistroList = true;
        private DistroModel _selectedDistro;
        private ObservableCollection<DistroModel> _distroListComboBox = new ObservableCollection<DistroModel>();
        private string _selectedUser;
        private ObservableCollection<string> _userListComboBox = new ObservableCollection<string>(){"Select User","Default User","root","User" };
        private int _selectedDistroIndex = 0;
        public int SelectedDistroIndex { get=>_selectedDistroIndex; set=> SetProperty(ref _selectedDistroIndex,value); }
        private object _lockObject = new object();
        
        private string _loginUsername;
        private Dictionary<string, DistroInstance> _storeDistroInstances;
        
            
        public ObservableCollection<DistroModel> DistroListComboBox { get=>_distroListComboBox; set=>SetProperty(ref _distroListComboBox,value); }
        public ObservableCollection<string> UserListComboBox { get=>_userListComboBox; set=>SetProperty(ref _userListComboBox,value); }

        #region Buttons

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
                if (_selectedDistro == null || _selectedDistro.DistroListItem.Contains("Select"))
                {
                    IsEnabledSelectUser = false;
                }
                else
                {
                    IsEnabledSelectUser = true;
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
            if (SelectedDistro == null || SelectedDistro.DistroListItem.Contains("Select"))
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
            if(SelectedUser == null)
            {
                IsEnabledUserName = false;
            }
            else
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
            
        }
        private void OnOffStartDistro()
        {
            if (SelectedDistro == null || SelectedDistro.DistroStatus == null || SelectedDistro.DistroStatus.Equals("Running"))
            {
                IsEnabledStartSelectedDistro = false;
            }
            else
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
        }
        private void OnOffCloseSelectedDistro()
        {
            if (SelectedDistro == null || SelectedDistro.DistroStatus == null  || !SelectedDistro.DistroStatus.Equals("Running"))
            {
                IsEnabledCloseSelectedDistro = false;
            }
            else
            {
                IsEnabledCloseSelectedDistro = true;
            }
        }
        private void OnOffCloseAllDistros()
        {
            bool temp = false;
            foreach (DistroModel distro in _distroListComboBox)
            {
                if (distro.DistroStatus !=null && distro.DistroStatus.Equals("Running"))
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
        }        

        private async Task CloseSelectedDistro()
        {
            _storeDistroInstances[SelectedDistro.DistroName].StopDistroInstance();
        }
        
        private async Task CloseAllDistro()
        {
            foreach (DistroModel distro in _distroListComboBox)
            {
                if (distro.DistroVersion!=null && distro.DistroStatus.Equals("Running"))
                {
                    _storeDistroInstances[distro.DistroName].StopDistroInstance(); 
                } 
            }
        }
        
        #endregion

        #endregion
        
        #region Prepare/Initialize/View Callbacks
        
        public override void Prepare(DistroInstanceBankNavigationArgs parameter)
        {
            if (parameter.DistroDictionaryBank == null)
            {
                _storeDistroInstances = new Dictionary<string, DistroInstance>();
            }
            else
            {
                _storeDistroInstances = parameter.DistroDictionaryBank;    
            }
        }
        
        public override async Task Initialize()
        {
            await base.Initialize();
            
            
            BindingOperations.EnableCollectionSynchronization(_distroListComboBox, _lockObject);
            if (_storeDistroInstances == null)
            {
                _storeDistroInstances = new Dictionary<string, DistroInstance>();
            }
            
            OnLoadUpdateDistroComboBox();
            
            Thread worker = new Thread(() =>
            {
                while (updateDistroList)
                {
                    UpdateDistroComboBox();
                    
                    Thread.Sleep(5000);
                    if (!updateDistroList)
                    {
                        break;
                    }
                }
            });
            worker.Name = "List updater";
            worker.Start();
            
        }
        
        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            updateDistroList = false;
        }
        #endregion


        #region ComboBox Distro Methods
        
        private void OnLoadUpdateDistroComboBox()
        {
            List<string[]> distroList = DataCommands.GenerateListOfDistros();

            DistroListComboBox.Clear();
            _distroListComboBox.Add(new DistroModel()
                {DistroName = "Select Distro", DistroStatus = null, DistroVersion = null});

            foreach (string[] distro in distroList) 
            {
                string name = distro[0];

                //if it is an new distro update the dictionary and the combobox
                if (!_storeDistroInstances.ContainsKey(name))
                {
                    _storeDistroInstances.Add(name, new DistroInstance(name));
                }

                DistroListComboBox.Insert(1,
                        new DistroModel() {DistroName = name, DistroStatus = distro[1], DistroVersion = distro[2]});
            }
        }

        private void UpdateDistroComboBox()
        {
            int tempIndexSelection = SelectedDistroIndex;
            List<string[]> distroList = DataCommands.GenerateListOfDistros();

            ObservableCollection<DistroModel> temp = new ObservableCollection<DistroModel>();
            temp.Add(new DistroModel()
                {DistroName = "Select Distro", DistroStatus = null, DistroVersion = null});

            foreach (string[] distro in distroList)
            {
                
                string name = distro[0];
                
                //if it is an new distro update the dictionary of distro instances
                if (!_storeDistroInstances.ContainsKey(name))
                {
                    _storeDistroInstances.Add(name, new DistroInstance(name));
                }

                temp.Insert(1, new DistroModel() {DistroName = name, DistroStatus = distro[1], DistroVersion = distro[2]});
            }

            for (int index = 0; index < temp.Count; index++)
            {
                if (temp[index].DistroStatus != DistroListComboBox[index].DistroStatus || temp[index].DistroVersion != DistroListComboBox[index].DistroVersion )
                {
                    //DistroListComboBox = temp;
                    _distroListComboBox[index] = new DistroModel()
                    {
                        DistroName = temp[index].DistroName,
                        DistroStatus = temp[index].DistroStatus,
                        DistroVersion = temp[index].DistroVersion
                    };
                    
                    
                    RaisePropertyChanged((() => DistroListComboBox));
                    RaisePropertyChanged(() => SelectedDistro);
                }
            }
            SelectedDistroIndex = tempIndexSelection;
            ButtonUpdates();
        }
        
        
        #endregion

        #region Navigation
        
        private readonly IMvxNavigationService _navigationService;
        
        public WSLManagerHomeViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        
        private async Task AdvanceToolsHelp()
        {
            await _navigationService.Navigate<AdvanceMoreInfoViewModel, DistroInstanceBankNavigationArgs>(new DistroInstanceBankNavigationArgs(_storeDistroInstances));
        }
        
        private async Task ChangeWslVersion()
        {
            await _navigationService.Navigate<ChangeWslVersionViewModel, DistroInstanceBankNavigationArgs>(new DistroInstanceBankNavigationArgs(_storeDistroInstances));
        }
        
        private async Task InstallerPage1(){
            await _navigationService.Navigate<InstallPage1ViewModel, DistroInstanceBankNavigationArgs>(new DistroInstanceBankNavigationArgs(_storeDistroInstances));
        }
        
        private async Task OpenDistroGui()
        {
            //TODO MAKE OPEN DISTRO GUI
        }
        
        private async Task ImportDistro()
        {
            await _navigationService.Navigate<ImportDistroViewModel, DistroInstanceBankNavigationArgs>(new DistroInstanceBankNavigationArgs(_storeDistroInstances));
        }
        
        private async Task UnregisterDistro()
        {
            await _navigationService.Navigate<UnRegisterADistroViewModel, DistroInstanceBankNavigationArgs>(new DistroInstanceBankNavigationArgs(_storeDistroInstances));
        }
        
        #endregion
    }
}