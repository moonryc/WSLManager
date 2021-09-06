using System;
using System.Windows.Input;
using WSLManager.Commands;
using WSLManager.Commands.ImportExportRemoveDistro;
using WSLManager.Models;

namespace WSLManager.ViewModels.ModifyDistroTools
{
    public class ExportDistroViewModel:BaseViewModel
    {
        private MainWindowViewModel _parent;
        private string _fullFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private DistroModel _selectedDistro;
        private bool _isIndeterminate = false;
        private string _messageOutput = "";

        public MainWindowViewModel MainWindowViewModel { get=>_parent;}
        
        /// <summary>
        /// Gets/Sets the Full File Path/ destination to export to
        /// </summary>
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
        /// Gets/Sets the selected distro
        /// </summary>
        public DistroModel SelectedDistro
        {
            get => _selectedDistro;
            set
            {
                _selectedDistro = value;
                OnPropertyChanged();
            }
        }

        public bool IsIndetermiante
        {
            get => _isIndeterminate;
            set
            {
                _isIndeterminate = value;
                OnPropertyChanged();
            }
        }

        public string MessageOutput
        {
            get => _messageOutput;
            set
            {
                _messageOutput = value;
                OnPropertyChanged();
            }
        }

        public ExportDistroViewModel(MainWindowViewModel parent)
        {
            _parent = parent;
            UpdateViewCommand = new UpdateViewCommand(_parent);
            ExportDistroDestinationCommand = new ExportDistroDestinationCommand(this);
            ExportDistroCommand = new ExportDistroCommand(this);
        }

        public ICommand UpdateViewCommand { get; set; }
        public ICommand ExportDistroDestinationCommand { get; set; }
        public ICommand ExportDistroCommand { get; set; }
    }
}