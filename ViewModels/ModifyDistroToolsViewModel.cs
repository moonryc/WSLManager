using System.Windows.Input;
using WSLManager.Commands;


namespace WSLManager.ViewModels
{
    public class ModifyDistroToolsViewModel : BaseViewModel
    {
        private MainWindowViewModel _parent;
        
        public ICommand UpdateViewCommand { get; set; }
        
        public ModifyDistroToolsViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent);
        }

        
    }
}