using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WSLManager.Core.Models;

namespace WSLManager.Core.ViewModels.Installer
{
    
    
    
    public class InstallPage1ViewModel:MvxViewModel<DistroInstanceBankNavigationArgs>
    {
        private Dictionary<string,DistroInstance> _bank;
        public override void Prepare(DistroInstanceBankNavigationArgs parameter)
        {
            _bank = parameter.DistroDictionaryBank;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
        }

        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavPage2Command => new MvxAsyncCommand(NavPage2);
        public IMvxAsyncCommand NavHomeCommand => new MvxAsyncCommand(NavHome);

        public InstallPage1ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavPage2(){
            await _navigationService.Navigate<InstallPage2ViewModel, DistroInstanceBankNavigationArgs>( new DistroInstanceBankNavigationArgs(_bank));
        }
        
        private async Task NavHome(){
            await _navigationService.Navigate<WSLManagerHomeViewModel, DistroInstanceBankNavigationArgs>( new DistroInstanceBankNavigationArgs(_bank));
        }
        
        
    }
}