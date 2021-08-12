using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace WSLManager.Core.ViewModels
{
    public class ChangeWslVersionViewModel:MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavHomeCommand => new MvxAsyncCommand(NavHome);

        public ChangeWslVersionViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavHome(){
            await _navigationService.Navigate<WSLManagerHomeViewModel>();
        }
    }
}