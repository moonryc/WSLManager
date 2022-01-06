using System;
using System.Windows.Input;
using WSLManager.ViewModels.Installer;

namespace WSLManager.Commands
{
    public class CloseWindowCommand:ICommand
    {
        private InstallerWindowViewModel _viewModel;
        
        public CloseWindowCommand(InstallerWindowViewModel viewModel)
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
            if (_viewModel.CanClose)
            {
                return true;
            }
            return false;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            // _viewModel.CloseWindow(_viewModel.);
        }

        #endregion
        
        
    }
}