using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace WSLManager.Core.ViewModels
{
    public class InstallPage1ViewModel:MvxViewModel
    {
        
        public override async Task Initialize()
        {
            await base.Initialize();
        }

        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavPage2Command => new MvxAsyncCommand(NavPage2);

        public InstallPage1ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavPage2(){
            await _navigationService.Navigate<InstallPage2ViewModel>();
        }
        
        
    }
}