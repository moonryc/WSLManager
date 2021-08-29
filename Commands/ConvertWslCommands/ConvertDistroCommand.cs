using System;
using System.Diagnostics;
using System.Windows.Input;
using WSLManager.Models;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ConvertWslCommands
{
    public class ConvertDistroCommand:ICommand
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
            DistroModel selectedDistroModel = _convertWslVersionViewModel.SelectedDistroModel;
            if (selectedDistroModel != null && !selectedDistroModel.DistroName.Contains("Select") &&
                !selectedDistroModel.IsRunning && selectedDistroModel.WslVersion == 1)
            {
                return true;
            }
            return false;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            string selectedDistroName = _convertWslVersionViewModel.SelectedDistroModel.DistroName;
            Process upgrade = new Process();
            upgrade.StartInfo.FileName = "cmd.exe";
            upgrade.StartInfo.Arguments = $"/c wsl --set-version {selectedDistroName} 2";
            upgrade.StartInfo.CreateNoWindow = false;
            
            upgrade.Start();
            
        }

        #endregion
    }
}