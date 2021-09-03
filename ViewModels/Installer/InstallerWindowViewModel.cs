using System.Windows;
using System.Windows.Input;
using WSLManager.Commands;

namespace WSLManager.ViewModels.Installer
{
    public class InstallerWindowViewModel:MainWindowViewModel
    {
        
        private BaseViewModel _selectedViewModel;
        private bool _canClose;
        
        /// <summary>
        /// Get/Set if the window can be Closed
        /// </summary>
        public bool CanClose
        {
            get => _canClose;
            set
            {
                _canClose = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="window"></param>
        public void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InstallerWindowViewModel()
        {
            SelectedViewModel = new InstallerIntroViewModel(this);
            UpdateViewCommand = new UpdateViewCommand(this);
        }
        
        public ICommand UpdateViewCommand { get; set; }
        
    }
}