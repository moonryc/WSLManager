using MvvmCross.IoC;
using MvvmCross.ViewModels;
using WSLManager.Core.ViewModels;

namespace WSLManager.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            
            RegisterAppStart<InstallPage1ViewModel>();
        }
    }
}