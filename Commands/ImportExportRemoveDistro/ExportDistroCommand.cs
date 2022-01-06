using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Input;
using WSLManager.Logger.Core;
using WSLManager.Models;
using WSLManager.ViewModels.ModifyDistroTools;

namespace WSLManager.Commands.ImportExportRemoveDistro
{
    public class ExportDistroCommand:ICommand
    {

        private ExportDistroViewModel _viewModel;

        public ExportDistroCommand(ExportDistroViewModel viewModel)
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
            string filePath = _viewModel.FullFilePath;
            DistroModel selectedDistro = _viewModel.SelectedDistro;
            if (String.IsNullOrEmpty(filePath) || String.IsNullOrWhiteSpace(filePath) || selectedDistro == null || !Directory.Exists(filePath))
            {
                _viewModel.MessageOutput = "Please select a valid filepath";
                return false;
            }
            
            if (_viewModel.SelectedDistro.DistroNameStatus.Contains("Select Distro"))
            {
                _viewModel.MessageOutput = "Please Select a valid distro";
                return false;
            }

            if (_viewModel.SelectedDistro.IsRunning)
            {
                _viewModel.MessageOutput = "Please close the distro before exporting";
                return false;
            }
            _viewModel.MessageOutput = "Directory and Distro are valid press Export to continue";
            return true;
        }

        //Defines the method to be called when the command is invoked
        public void Execute(object parameter)
        {
            
            _viewModel.MainWindowViewModel.CanGoBack = false;
            Thread exportDistroThread = new Thread(() =>
            {
                Process exportProcess = new Process();
                exportProcess.StartInfo.FileName = "cmd.exe";
                exportProcess.StartInfo.CreateNoWindow = true;
                exportProcess.StartInfo.Arguments =
                    $"/c wsl --export {_viewModel.SelectedDistro.DistroName} {_viewModel.FullFilePath}\\{_viewModel.SelectedDistro.DistroName}";
                exportProcess.Start();
                IoC.Base.IoC.baseFactory.Log("exporting distro", LogLevel.Critical);

                while (!exportProcess.HasExited)
                {
                    if (_viewModel.IsIndetermiante != true)
                    {
                        _viewModel.IsIndetermiante = true;
                        _viewModel.MessageOutput = "Exporting Distro please wait";
                    }
                }
                _viewModel.MainWindowViewModel.CanGoBack = true;
                _viewModel.IsIndetermiante = !exportProcess.HasExited;
                _viewModel.MessageOutput = "Export complete";

            });
            exportDistroThread.Name = "Export Distro Thread";
            exportDistroThread.Start();
            
            
        }

        #endregion
        
    }
}