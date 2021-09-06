using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using WSLManager.Models;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ConvertWslCommands
{
    public class ConvertDistroCommand : ICommand
    {
        private ConvertWslVersionViewModel _convertWslVersionViewModel;

        public ConvertDistroCommand(ConvertWslVersionViewModel viewModel)
        {
            _convertWslVersionViewModel = viewModel;
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
            if (_convertWslVersionViewModel.IsIndeterminate)
            {
                return false;
            }
            
            DistroModel selectedDistroModel = _convertWslVersionViewModel.SelectedDistroModel;
            if (selectedDistroModel == null || selectedDistroModel.DistroName.Contains("Select Distro"))
            {
                _convertWslVersionViewModel.MessageOutput = "Select a valid Distro";
                return false;
            }

            if (selectedDistroModel.WslVersion == 2)
            {
                _convertWslVersionViewModel.MessageOutput = "Selected Distro is already a valid WSL2 distro";
                return false;
            }

            if (selectedDistroModel.IsRunning)
            {
                _convertWslVersionViewModel.MessageOutput =
                    "Distro is currently running, please close the distro and try again";
                return false;
            }

            _convertWslVersionViewModel.MessageOutput =
                "Distro is valid for upgrade, press \"Convert Distro\" below to upgrade";
            return true;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            _convertWslVersionViewModel.MainWindowViewModel.CanGoBack = false;
            Thread convertDistroThread = new Thread(() =>
            {
                string selectedDistroName = _convertWslVersionViewModel.SelectedDistroModel.DistroName;
                Process upgrade = new Process();
                upgrade.StartInfo.FileName = "cmd.exe";
                upgrade.StartInfo.Arguments = $"/c wsl --set-version {selectedDistroName} 2";
                upgrade.StartInfo.CreateNoWindow = true;

                upgrade.Start();
                
                while (!upgrade.HasExited)
                {
                    _convertWslVersionViewModel.IsIndeterminate = true;
                    _convertWslVersionViewModel.MessageOutput = "Converting distro please wait";
                }
                _convertWslVersionViewModel.MainWindowViewModel.CanGoBack = true;
                _convertWslVersionViewModel.IsIndeterminate = !upgrade.HasExited;
                _convertWslVersionViewModel.MessageOutput = "Distro has been Converted to WSL2";
            });


            convertDistroThread.Name = "Convert Distro Thread";
            convertDistroThread.Start();
        }

        #endregion
    }
}