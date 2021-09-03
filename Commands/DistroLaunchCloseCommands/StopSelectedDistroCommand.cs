using System;
using System.Threading;
using System.Windows.Input;
using WSLManager.ViewModels;

namespace WSLManager.Commands.DistroLaunchCloseCommands
{
    public class StopSelectedDistroCommand:ICommand
    {
        private DistroLaunchCloseViewModel _distroLaunchCloseViewModel; 
        private MainWindowViewModel _mainWindowViewModel; 
            
        public StopSelectedDistroCommand(DistroLaunchCloseViewModel viewModel, MainWindowViewModel mainWindowViewModel)
        {
            _distroLaunchCloseViewModel = viewModel;
            _mainWindowViewModel = mainWindowViewModel;
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
            if (_mainWindowViewModel.SelectedDistroModel != null && _mainWindowViewModel.SelectedDistroModel.IsRunning)
            {
                return true;    
            }
            return false;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            Thread closeDistro = new Thread(() =>
            {
                _mainWindowViewModel.SelectedDistroModel.EndDistro();
            });
            closeDistro.Name = "Close Distro";
            closeDistro.Start();
        }

        #endregion
    }
}