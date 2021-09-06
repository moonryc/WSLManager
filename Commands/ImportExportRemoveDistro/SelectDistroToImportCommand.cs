using System;
using System.Windows.Input;
using Microsoft.Win32;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ImportExportRemoveDistro
{
    public class SelectDistroToImportCommand:ICommand
    {

        private ImportDistroViewModel _viewModel;
        private OpenFileDialog _openFileDialog;

        public SelectDistroToImportCommand(ImportDistroViewModel viewModel)
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
            if (_viewModel.IsIndeterminate)
            {
                return false;
            }
            return true;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.Multiselect = false;
            _openFileDialog.AddExtension = true;

            Nullable<bool> result = _openFileDialog.ShowDialog(); 
            
            if (result == true)
            {
                string fullFilePath = _openFileDialog.FileName;
                _viewModel.FullFilePath = fullFilePath;
            }
        }

        #endregion
    }
}