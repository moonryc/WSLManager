using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using WSLManager.Logger.Core;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ImportExportRemoveDistro
{
    public class RemoveDistroCommand:ICommand
    {
        private RemoveDistroViewModel _viewModel;
        
        public RemoveDistroCommand(RemoveDistroViewModel viewModel)
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
            if (_viewModel.SelectedDistro == null || _viewModel.SelectedDistro.DistroName.Contains("Select Distro"))
            {
                _viewModel.MessageText = "Please Select a valid Distro.";
                return false;
            }
            if (_viewModel.SelectedDistro.IsRunning)
            {
                _viewModel.MessageText = "Cannot remove a distro while it is running, please close the distro before attempting to remove.";
                return false;
            }

            _viewModel.MessageText = "Valid distro selected, press \"Remove Distro\" below to remove this distro.";
            return true;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            _viewModel.MainWindowViewModel.CanGoBack = false;
            Thread removeDistroThread = new Thread(() =>
            {
                string distroName = _viewModel.SelectedDistro.DistroName;
                Process removeProcess = new Process();
                removeProcess.StartInfo.FileName = "cmd.exe";
                removeProcess.StartInfo.CreateNoWindow = true;
                removeProcess.StartInfo.Arguments = $"/c wsl --unregister {distroName}";
                removeProcess.Start();
                IoC.Base.IoC.baseFactory.Log("Removing Distro", LogLevel.Critical);
                

                while (!removeProcess.HasExited)
                {
                    if (_viewModel.IsIndeterminate != true)
                    {
                        _viewModel.MessageText = "Removing distro please wait";
                        _viewModel.IsIndeterminate = true;   
                    }
                }
                _viewModel.MainWindowViewModel.CanGoBack = true;
                _viewModel.MainWindowViewModel.DistroBank.DistroDictionary.Remove(distroName);
                _viewModel.IsIndeterminate = !removeProcess.HasExited;
                _viewModel.MessageText = "Distro has been removed";
            });

            removeDistroThread.Name = "Remove Distro Thread";
            removeDistroThread.Start();
        }

        #endregion
        
    }
}