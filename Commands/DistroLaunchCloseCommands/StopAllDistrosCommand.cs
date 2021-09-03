using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WSLManager.Models;
using WSLManager.ViewModels;

namespace WSLManager.Commands.DistroLaunchCloseCommands
{
    public class StopAllDistrosCommand:ICommand
    {
        private DistroLaunchCloseViewModel _distroLaunchCloseViewModel;
        private MainWindowViewModel _mainWindowViewModel;
        
        public StopAllDistrosCommand(DistroLaunchCloseViewModel viewModel,MainWindowViewModel mainWindowViewModel)
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
            ObservableCollection<DistroModel> tempCollection = _mainWindowViewModel.DistroCollection;
            for (int i = 0; i < tempCollection.Count; i++)
            {
                if (tempCollection[i].IsRunning)
                {
                    return true;
                }
            }
            return false;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            ObservableCollection<DistroModel> tempCollection = _mainWindowViewModel.DistroCollection;
            for (int i = 0; i < tempCollection.Count; i++)
            {
                if (tempCollection[i].IsRunning)
                {
                    tempCollection[i].EndDistro();
                }
            }
        }

        #endregion
    }
}