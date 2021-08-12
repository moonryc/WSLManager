using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace WSLManager.Core.ViewModels.Installer
{
    public class InstallPage4ViewModel:MvxViewModel
    {
        
        #region Navigation 
        
        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavHomeCommand => new MvxAsyncCommand(NavHome);

        public InstallPage4ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavHome(){
            await _navigationService.Navigate<WSLManagerHomeViewModel>();
        }
        
        #endregion
        
    }
}