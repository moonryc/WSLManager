using System.Windows.Input;
using WSLManager.Commands;

namespace WSLManager.ViewModels.Installer
{
    public class InstallerWindowViewModel:MainWindowViewModel
    {
        private BaseViewModel _selectedViewModel;
        
        
        public InstallerWindowViewModel()
        {
            SelectedViewModel = new InstallerIntroViewModel(this);
            UpdateViewCommand = new UpdateViewCommand(this);
        }

        // public BaseViewModel SelectedViewModel
        // {
        //     get => _selectedViewModel;
        //     set
        //     {
        //         _selectedViewModel = value;
        //         OnPropertyChanged();
        //     }
        // }

        public ICommand UpdateViewCommand { get; set; }
        
    }
}