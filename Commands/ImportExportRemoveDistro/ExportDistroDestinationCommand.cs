using System;
using System.Windows.Forms;
using System.Windows.Input;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ImportExportRemoveDistro
{
    public class ExportDistroDestinationCommand:ICommand
    {

        private ExportDistroViewModel _viewModel;

        public ExportDistroDestinationCommand(ExportDistroViewModel viewModel)
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

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.ShowDialog();

            _viewModel.FullFilePath = folderBrowserDialog.SelectedPath;
        }

        #endregion
    }
}