using System;
using System.Windows.Input;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ImportExportDistro
{
    public class SelectDirectoryImportCommand:ICommand
    {


        private ImportDistroViewModel _viewModel;
        private System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog;
        public SelectDirectoryImportCommand(ImportDistroViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        
        #region ICommand

        //Occurs when changes occur that affect whether or not the command should execute.
        event EventHandler ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        //Defines the method that determines whether the command can be executed in its current state
        public bool CanExecute(object parameter)
        {
            return true;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            _folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            _folderBrowserDialog.ShowNewFolderButton = true;
            
            _folderBrowserDialog.ShowDialog();

            _viewModel.InstallFilePath = _folderBrowserDialog.SelectedPath;

        }

        #endregion
    }
}