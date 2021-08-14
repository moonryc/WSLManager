using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WSLManager.Core.Commands;
using WSLManager.Core.Models;

namespace WSLManager.Core.ViewModels.Installer
{
    public class InstallPage3NavigationArgs
    {
        public string distro { get; set; }
        public bool isKali { get; set; }
        public DistroInstanceBankNavigationArgs Bank { get; set; }
    }
    
    public class InstallPage2ViewModel:MvxViewModel<DistroInstanceBankNavigationArgs>
    {
        private Dictionary<string,DistroInstance> _bank;
        public override void Prepare(DistroInstanceBankNavigationArgs parameter)
        {
            _bank = parameter.DistroDictionaryBank;
        }
        
        #region Properties
        
        private DistroModel _selectedDistro;
        private ObservableCollection<DistroModel> _distroCollection = new ObservableCollection<DistroModel>();
        private bool _isKali;
        private bool _page3IsEnabledButton;
     
        
        public ObservableCollection<DistroModel> DistroCollection 
        { 
            get => _distroCollection;             
            set=> SetProperty(ref _distroCollection, value); 
        }
        public DistroModel SelectedDistro
        {
            get => _selectedDistro;
            set {
                _selectedDistro = value; 
                RaisePropertyChanged(()=> SelectedDistro);
                if (_selectedDistro == null || _selectedDistro.DistroStatus == null)
                {
                    Page3IsEnabledButton= false;
                }
                else
                {
                    Page3IsEnabledButton= true;
                }
            }
        }
        
        public bool IsKali
        {
            get => _isKali;
            set
            {
                SetProperty(ref _isKali, value);
                
            }
        }
        
        
        #endregion

        #region ButtonsEnableDisable

        public bool Page3IsEnabledButton
        {
            get => _page3IsEnabledButton;
            set=> SetProperty(ref _page3IsEnabledButton, value);
        }

        #endregion
        
        #region Methods
        
        public void UpdateDistroListOnLoad()
        {
            List<string[]> distroListInfo = DataCommands.GenerateListOfDistros();
            DistroCollection.Add(new DistroModel{DistroName = "Selected Distro",DistroStatus = null,DistroVersion = null});
            foreach (var distro in distroListInfo)
            {
                //Only displays wsl2
                if (int.Parse(distro[2]) == 2)
                {
                    DistroCollection.Add(new DistroModel{DistroName = distro[0], DistroStatus = distro[1],DistroVersion = distro[2]});    
                }
            }
            RaisePropertyChanged(()=>DistroCollection);

        }
        
        #endregion
        
        public override void Prepare()
        {
            // first callback. Initialize parameter-agnostic stuff here
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            UpdateDistroListOnLoad();            
        }
        

        
        #region Navigation

        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavPage1Command => new MvxAsyncCommand(NavPage1);
        public IMvxAsyncCommand NavPage3Command => new MvxAsyncCommand(NavPage3);

        public InstallPage2ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavPage1(){
            await _navigationService.Navigate<InstallPage1ViewModel, DistroInstanceBankNavigationArgs>(new DistroInstanceBankNavigationArgs(_bank));
        }
        private async Task NavPage3(){
            
            await _navigationService.Navigate<InstallPage3ViewModel, InstallPage3NavigationArgs>(
                new InstallPage3NavigationArgs {distro = SelectedDistro.DistroName, isKali = IsKali, Bank = new DistroInstanceBankNavigationArgs(_bank)});
        }
        
    
        #endregion
        
    }
}