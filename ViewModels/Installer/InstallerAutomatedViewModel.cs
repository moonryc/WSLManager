using System;
using System.IO;
using System.Threading;
using WSLManager.Logger.Core;
using WSLManager.Models;

namespace WSLManager.ViewModels.Installer
{
    public class InstallerAutomatedViewModel:BaseViewModel
    {

        private InstallerModel _installerModel;
        private MainWindowViewModel _parent;
        private string _distro;
        private bool _isKali;
        
        
        
        public InstallerAutomatedViewModel(string distro, bool isKali)
        {
            _distro = distro;
            _isKali = isKali;
            _installerModel = new InstallerModel(_distro, _isKali);
            StartInstaller();
        }

        private void SameBlockProgress(string temp)
        {
            IoC.Base.IoC.baseFactory.Log(temp,LogLevel.Debug);
        }
        private void NewBlockProgress(string temp)
        {
            IoC.Base.IoC.baseFactory.Log(temp);
        }        
        
        /// <summary>
        ///     Creates the text file which will store the script
        /// </summary>
        private void GenerateBashScriptTextFile()
        {
            //create file
            if (!File.Exists(_installerModel.WindowsFileLocation))
            {
                using (StreamWriter createFile = File.CreateText(_installerModel.WindowsFileLocation))
                {
                    SameBlockProgress("Loading Commands into Script file");
                    SameBlockProgress($"Adding the following commands to the file located at {_installerModel.WindowsFileLocation} before converting to batch script");
                    
                    //writes commands to file
                    foreach (string command in _installerModel.BashCommandsToAppend)
                    {
                        Thread.Sleep(300);
                        createFile.WriteLine(command);
                        SameBlockProgress($"Successfully added command: {command} to the script.");
                    }
                }
            }
            SameBlockProgress($"Successfully added commands to the script.");
        }
        
        private void StartInstaller()
        {
            try
            {
                //Initial Cleanup
                //does not print text if there is nothing to show
                string removePriorWslInstallerFilesWindowsReturnText = _installerModel.RemovePriorWslInstallerFilesWindows();
                string removePriorWslInstallerFilesFromDistroReturnText = _installerModel.RemovePriorWslInstallerFilesFromDistro();
                if (removePriorWslInstallerFilesWindowsReturnText.Equals(""))
                {
                    SameBlockProgress(removePriorWslInstallerFilesWindowsReturnText);
                }
                if (removePriorWslInstallerFilesFromDistroReturnText.Equals(""))
                {
                    SameBlockProgress(removePriorWslInstallerFilesFromDistroReturnText);
                }
                
                //Beginning Installation
                SameBlockProgress("Beginning Installation");
                
                //Generate Files and Folders
                NewBlockProgress("Creating InstallationFolder");
                SameBlockProgress(_installerModel.GenerateWslInstallerFolder());
                Thread.Sleep(1000);
                
                NewBlockProgress("Creating Bash file");
                GenerateBashScriptTextFile();
                Thread.Sleep(1000);
                
                // Move Folder containing script
                NewBlockProgress($"Moving file to {_distro}");
                SameBlockProgress(_installerModel.MoveScriptToWsl());
                Thread.Sleep(1000);
                
                //Modify script
                NewBlockProgress("Configuring Batch file");
                SameBlockProgress("Correcting Carriage returns");
                SameBlockProgress(_installerModel.CorrectingCarriageReturns());
                Thread.Sleep(1000);
                
                SameBlockProgress("Converting to Shell Script");
                SameBlockProgress(_installerModel.ConvertToBatchShell());
                Thread.Sleep(1000);
                
                SameBlockProgress("Making Batch Script executable");
                SameBlockProgress(_installerModel.MakeRunnable());
                Thread.Sleep(1000);
                
                //Run Script
                NewBlockProgress("The Batch Script has been launched please monitor it and type in your password when applicable. Once the script has finished the installation will be complete");
                _installerModel.RunScript();
                NewBlockProgress("INSTALLATION COMPLETE");
                //SameBlockProgress("PRESS THE \"LAUNCH GUI\" BUTTON TO OPEN THE GUI VERSION OF LINUX");
            }
            catch (Exception e)
            {
                NewBlockProgress("ERROR SEE BELOW:\n");
                IoC.Base.IoC.baseFactory.Log(e.Message,LogLevel.Critical);
                SameBlockProgress("INSTALLATION HAS BEEN HALTED");
            }
        }
        
    }
}