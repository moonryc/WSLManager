using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Input;
using WSLManager.Models;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ImportExportRemoveDistro
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

            if (_viewModel.IsIndeterminate)
            {
                return false;
            }
            
            if (!File.Exists(_viewModel.FullFilePath))
            {
                _viewModel.MessageOutput = "Select a valid Distro file location";
                return false;
            }

            fileInfo = new FileInfo(_viewModel.FullFilePath);
            if (!Directory.Exists(_viewModel.InstallFilePath))
            {
                _viewModel.MessageOutput = "Select a valid Install Location";
                return false;
            }
            
            _desiredDistroName = _viewModel.DistroName;
            if (_desiredDistroName.Contains(' ') || String.IsNullOrEmpty(_desiredDistroName) || String.IsNullOrWhiteSpace(_desiredDistroName))
            {
                _viewModel.MessageOutput = "Distro name cannot contain spaces";
                return false;
            }
            
            foreach (DistroModel distroModel in _viewModel.MainWindowViewModel.DistroCollection)
            {
                if (distroModel.DistroName.ToUpper() == _desiredDistroName.ToUpper())
                {
                    _viewModel.MessageOutput = "Distro name is already registered";
                    return false;
                }
            }
            _viewModel.MessageOutput = "Press \"Import Distro\" to continue";
            return true;
            
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            _viewModel.MainWindowViewModel.CanGoBack = false;
            Thread importThread = new Thread(() =>
            {
                string fileDirectory = _viewModel.FullFilePath;
                string installFilePath = _viewModel.InstallFilePath;

                string command = string.Format(@"/c wsl --import {0} {1} {2}", _desiredDistroName, installFilePath,
                    fileDirectory);
                string[] commandSplit = command.Split("\\");
                command = Path.Combine(commandSplit);


                Process importProcess = new Process();
                importProcess.StartInfo.FileName = "cmd.exe";
                importProcess.StartInfo.Arguments = command;
                importProcess.StartInfo.CreateNoWindow = true;
                importProcess.Start();
                
                while (!importProcess.HasExited)
                {
                    _viewModel.IsIndeterminate = true;
                    _viewModel.MessageOutput = "Importing Distro please wait (this can take some time)";
                }
                _viewModel.MainWindowViewModel.CanGoBack = true;
                _viewModel.MessageOutput = "Distro has been imported";
                _viewModel.IsIndeterminate = !importProcess.HasExited;

            });

            importThread.Name = "Import distro thread";
            importThread.Start();


        }

        #endregion
    }
}