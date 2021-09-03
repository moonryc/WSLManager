using System;
using System.Threading;
using System.Windows.Input;
using WSLManager.Models;
using WSLManager.ViewModels;

namespace WSLManager.Commands.DistroLaunchCloseCommands
{
    public class StartDistroCommand:ICommand
    {
        private DistroLaunchCloseViewModel _distroLaunchCloseViewModel;
        private MainWindowViewModel _mainWindowViewModel;

        private bool distroName = false;
        bool isRunning = false;
        bool loginMethod = false;
        bool userName = false;
        
        public StartDistroCommand( DistroLaunchCloseViewModel viewModel, MainWindowViewModel mainWindowViewModel)
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
            if (_mainWindowViewModel.SelectedDistroModel != null)
            {
                isRunning = _mainWindowViewModel.SelectedDistroModel.IsRunning;
                
                if (!_mainWindowViewModel.SelectedDistroModel.DistroNameStatus.Contains("Select") && _mainWindowViewModel.SelectedDistroModel.DistroNameStatus != null)
                {
                    distroName = true;
                }
                else
                {
                    distroName = false;
                }

                if (_distroLaunchCloseViewModel.UserLoginType != 0)
                {
                    loginMethod = true;
                }
                else
                {
                    loginMethod = false;
                }

                if (_distroLaunchCloseViewModel.UserLoginType == (int)LoginOptions.SelectLoginMethod && String.IsNullOrEmpty(_distroLaunchCloseViewModel.UserName) )
                {
                    userName = false;
                }
                else
                {
                    userName = true;
                }
                
                if (distroName && loginMethod && userName && !isRunning)
                {
                    return true;
                }
            }
            return false;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            Thread startDistro = new Thread(() =>
            {
                LoginOptions selectedLogin = (LoginOptions)_distroLaunchCloseViewModel.UserLoginType;
                switch (selectedLogin)
                {
                    case LoginOptions.root:
                        _mainWindowViewModel.SelectedDistroModel.StartDistroUser("root");
                        break;
                    case LoginOptions.DefaultUser:
                        _mainWindowViewModel.SelectedDistroModel.StartDistro();
                        break;
                    case LoginOptions.SpecificUser:
                        _mainWindowViewModel.SelectedDistroModel.StartDistroUser(_distroLaunchCloseViewModel
                            .UserName);
                        break;
                    default:
                        throw new Exception("NO A VALID LOGIN TYPE");
                }
            });
            startDistro.Name = "Start Distro";
            startDistro.Start();
        }

        #endregion
    }
}