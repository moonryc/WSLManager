using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using WSLManager.Models;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ImportExportDistro
{
    public class ImportDistroCommand:ICommand
    {
        FileInfo fileInfo;
        private string _desiredDistroName = "";

        private ImportDistroViewModel _viewModel;

        public ImportDistroCommand(ImportDistroViewModel viewModel)
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
            bool test1 = String.IsNullOrEmpty(_viewModel.FullFilePath);
            bool test2 = String.IsNullOrWhiteSpace(_viewModel.FullFilePath);
            bool test3 = String.IsNullOrEmpty(_viewModel.InstallFilePath);
            bool test4 = String.IsNullOrWhiteSpace(_viewModel.InstallFilePath);
            if (test1 || test2 ||test3 || test4)
            {
                return false;
            }
            
            fileInfo = new FileInfo(_viewModel.FullFilePath);
            if (!fileInfo.Exists)
            {
                return false;
            }
            
            _desiredDistroName = _viewModel.DistroName;
            if (_desiredDistroName.Contains(' ') || String.IsNullOrEmpty(_desiredDistroName) || String.IsNullOrWhiteSpace(_desiredDistroName))
            {
                return false;
            }
            
            foreach (DistroModel distroModel in _viewModel.MainWindowViewModel.DistroCollection)
            {
                if (distroModel.DistroName.ToUpper() == _desiredDistroName.ToUpper())
                {
                    return false;
                }
            }
            
            return true;
            
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            
            string fileDirectory = _viewModel.FullFilePath;
            string installFilePath  = _viewModel.InstallFilePath;

            string command = string.Format(@"/c wsl --import {0} {1} {2}",_desiredDistroName,installFilePath,fileDirectory);
            string[] commandSplit = command.Split("\\");
            command = Path.Combine(commandSplit);
            
            
            Process importProcess = new Process();
            importProcess.StartInfo.FileName = "cmd.exe";
            importProcess.StartInfo.Arguments = command;
            importProcess.StartInfo.CreateNoWindow = true;
            importProcess.Start();
            
            
        }

        #endregion
    }
}