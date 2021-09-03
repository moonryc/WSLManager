using System;
using System.Windows.Input;
using WSLManager.Models;
using WSLManager.ViewModels.Installer;

namespace WSLManager.Commands.InstallerCommands
{
    public class InstallGuiToolsCommand:ICommand
    {
        private InstallerAutomatedSetupViewModel _viewModel; 
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewModel"></param>
        public InstallGuiToolsCommand(InstallerAutomatedSetupViewModel viewModel)
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
            DistroModel selectedDistro = _viewModel.DistroModel;
            if (selectedDistro != null)
            {
                bool isRunning = selectedDistro.IsRunning;    
                bool isValidDistroName = !String.IsNullOrWhiteSpace(selectedDistro.DistroName);
                bool isWsl2 = selectedDistro.WslVersion ==2;
                if (!isRunning && isValidDistroName && isWsl2)
                {
                    return true;
                }
            }
            return false;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            _viewModel.SelectedViewModel = new InstallerAutomatedViewModel(_viewModel.DistroModel.DistroName,_viewModel.IsKali);
        }

        #endregion
    }
}