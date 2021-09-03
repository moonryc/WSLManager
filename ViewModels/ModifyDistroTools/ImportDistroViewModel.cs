using System.Windows.Input;
using WSLManager.Commands;
using WSLManager.Commands.ImportExportDistro;

namespace WSLManager.ViewModels.ModifyDistroTools
{
    public class ImportDistroViewModel:BaseViewModel
    {
        private MainWindowViewModel _parent;
        private string _distroName = "";
        private string _filePath = "";
        private string _installFilePath = "Select Install Location";
        private string _fullFilePath = "Select Distro File";
        
        
        

        // the file path of the distro to be imported
        public string FullFilePath
        {
            get => _fullFilePath;
            set
            {
                _fullFilePath = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// The file path to install the distro
        /// </summary>
        public string InstallFilePath
        {
            get => _installFilePath;
            set
            {
                _installFilePath = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Get Main window viewmodel (for changing views with the update view command)
        /// </summary>
        public MainWindowViewModel MainWindowViewModel { get=>_parent; }
        
        /// <summary>
        /// Gets/sets the custom name of the distro to import
        /// </summary>
        public string DistroName
        {
            get => _distroName;
            set
            {
                _distroName = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="parent"></param>
        public ImportDistroViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent);
            ImportDistroCommand = new ImportDistroCommand(this);
            SelectDistroToImportCommand = new SelectDistroToImportCommand(this);
            SelectDirectoryImportCommand = new SelectDirectoryImportCommand(this);
        }

        // commands binded to viewmodel
        public ICommand UpdateViewCommand { get; set; }
        public ICommand ImportDistroCommand { get; set; }
        public ICommand SelectDistroToImportCommand { get; set; }
        public ICommand SelectDirectoryImportCommand { get; set; }
    }
}