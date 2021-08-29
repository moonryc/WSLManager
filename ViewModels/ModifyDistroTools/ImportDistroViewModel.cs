using System.Windows.Input;
using WSLManager.Commands;

namespace WSLManager.ViewModels.ModifyDistroTools
{
    public class ImportDistroViewModel:BaseViewModel
    {
        private MainWindowViewModel _parent;

        public ImportDistroViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent);
        }

        public ICommand UpdateViewCommand { get; set; }
    }
}