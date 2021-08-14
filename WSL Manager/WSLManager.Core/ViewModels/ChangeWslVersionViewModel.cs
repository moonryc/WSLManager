using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WSLManager.Core.Models;

namespace WSLManager.Core.ViewModels
{
    public class ChangeWslVersionViewModel:MvxViewModel<DistroInstanceBankNavigationArgs>
    {
        
        private Dictionary<string,DistroInstance> _bank;
        public override void Prepare(DistroInstanceBankNavigationArgs parameter)
        {
            _bank = parameter.DistroDictionaryBank;
        }
        
        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavHomeCommand => new MvxAsyncCommand(NavHome);

        public ChangeWslVersionViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavHome(){
            await _navigationService.Navigate<WSLManagerHomeViewModel, DistroInstanceBankNavigationArgs>( new DistroInstanceBankNavigationArgs(_bank));
        }
    }
}