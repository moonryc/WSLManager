using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace WSLManager.Core.ViewModels
{
    public class ImportDistroViewModel:MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavHomeCommand => new MvxAsyncCommand(NavHome);

        public ImportDistroViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavHome(){
            await _navigationService.Navigate<WSLManagerHomeViewModel>();
        }
    }
}