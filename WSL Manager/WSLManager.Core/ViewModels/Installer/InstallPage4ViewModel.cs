using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WSLManager.Core.Models;

namespace WSLManager.Core.ViewModels.Installer
{
    public class InstallPage4ViewModel:MvxViewModel<DistroInstanceBankNavigationArgs>
    {
        private Dictionary<string,DistroInstance> _bank;
        public override void Prepare(DistroInstanceBankNavigationArgs parameter)
        {
            _bank = parameter.DistroDictionaryBank;
        }
        
        #region Navigation 
        
        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavHomeCommand => new MvxAsyncCommand(NavHome);

        public InstallPage4ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavHome(){
            await _navigationService.Navigate<WSLManagerHomeViewModel, DistroInstanceBankNavigationArgs>( new DistroInstanceBankNavigationArgs(_bank));
        }
        
        #endregion
        
    }
}