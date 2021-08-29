using System.Windows.Input;
using WSLManager.Commands;

namespace WSLManager.ViewModels.ModifyDistroTools
{
    public class GuiDistroLauncherViewModel:BaseViewModel
    {
        private MainWindowViewModel _parent;

        public GuiDistroLauncherViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent);
        }

        public ICommand UpdateViewCommand { get; set; }
        
    }
}