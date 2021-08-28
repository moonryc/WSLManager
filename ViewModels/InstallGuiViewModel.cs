namespace WSLManager.ViewModels
{
    public class InstallGuiViewModel:BaseViewModel
    {
        private MainWindowViewModel _parent;
        
        public InstallGuiViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
        }
    }
}