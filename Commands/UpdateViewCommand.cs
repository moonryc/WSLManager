using System;
using System.Windows.Input;
using WSLManager.ViewModels;
using WSLManager.ViewModels.Installer;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands
{
    public class UpdateViewCommand:ICommand
    {
        private MainWindowViewModel _mainWindowViewModel;
        public UpdateViewCommand(MainWindowViewModel viewModel)
        {
            _mainWindowViewModel = viewModel;
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
            if (_mainWindowViewModel.CanGoBack)
            {
                return true;    
            }

            return false;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "DistroLaunchClose":
                    _mainWindowViewModel.SelectedViewModel = new DistroLaunchCloseViewModel(_mainWindowViewModel);
                    break;
                case "ModifyDistroTools":
                    _mainWindowViewModel.SelectedViewModel = new ModifyDistroToolsViewModel(_mainWindowViewModel);
                    break;
                case "ConvertWsl":
                    _mainWindowViewModel.SelectedViewModel = new ConvertWslVersionViewModel(_mainWindowViewModel);
                    break;
                case "GUI":
                    _mainWindowViewModel.SelectedViewModel = new GuiDistroLauncherViewModel(_mainWindowViewModel);
                    break;
                case "Export":
                    _mainWindowViewModel.SelectedViewModel = new ExportDistroViewModel(_mainWindowViewModel);
                    break;
                case "Import":
                    _mainWindowViewModel.SelectedViewModel = new ImportDistroViewModel(_mainWindowViewModel);
                    break;
                case "Remove":
                    _mainWindowViewModel.SelectedViewModel = new RemoveDistroViewModel(_mainWindowViewModel);
                    break;
                case "InstallerIntro":
                    _mainWindowViewModel.SelectedViewModel = new InstallerIntroViewModel(_mainWindowViewModel);
                    break;
                case "Walkthrough":
                    _mainWindowViewModel.SelectedViewModel = new InstallerWalkthroughViewModel(_mainWindowViewModel);
                    break;
                case "Automated":
                    _mainWindowViewModel.SelectedViewModel = new InstallerAutomatedSetupViewModel(_mainWindowViewModel);
                    break;
                default:
                    throw new Exception($"{parameter}");
            }
        }
        
        #endregion
    }
}