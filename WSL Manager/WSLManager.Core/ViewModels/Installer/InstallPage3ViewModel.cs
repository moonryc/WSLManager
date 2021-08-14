using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using WSLManager.Core.Commands;
using WSLManager.Core.Models;

namespace WSLManager.Core.ViewModels.Installer
{
    public class InstallPage3ViewModel:MvxViewModel<InstallPage3NavigationArgs>
    {
        private Dictionary<string, DistroInstance> _bank;
        
        #region Properties
        
        private string _distro;
        private bool _isKali;
        private string _installerDebugOutput;
        private int _progressBar;
        private bool _page4IsEnabledButton;
        private bool _progressBarIndeterminate;
        private bool _errorProgressBar;
        private int _numberOfProgressSteps = 22;
        
        public string Distro { get=>_distro; private set=> SetProperty(ref _distro,value); }
        public bool IsKali { get=>_isKali; private set=>SetProperty(ref _isKali,value); }
        public string InstallerDebugOutput { get=>_installerDebugOutput; private set=>SetProperty(ref _installerDebugOutput,value); }
        public bool Page4IsEnabledButton { get=>_page4IsEnabledButton; set=>SetProperty(ref _page4IsEnabledButton,value); }
        public int NumberOfProgressSteps { get=>_numberOfProgressSteps;}
        public int ProgressBar
        {
            get=>_progressBar;
            set
            {
                SetProperty(ref _progressBar, value);
                if (_progressBar == _numberOfProgressSteps)
                {
                    Page4IsEnabledButton = true;
                }
            }
        }

        public bool ProgressBarIndeterminate { get=>_progressBarIndeterminate; set=>SetProperty(ref _progressBarIndeterminate,value); }
        public bool ErrorProgressBar
        {
            get=>_errorProgressBar;
            set
            {
                SetProperty(ref _errorProgressBar, value);
                RaisePropertyChanged(() => Foreground);
            }
        
        }
        
        //TODO GET THIS COLOR CHANGE WORKING
        public string Foreground
        {
            get
            {
                if (_errorProgressBar)
                {
                    return "#FFFF9700";
                }
                else
                {
                    
                    return "#FF06B025";
                }
            }
        }

        #endregion
        
        #region Prepare
        
        public override void Prepare(InstallPage3NavigationArgs parameter)
        {
            Distro = parameter.distro;
            IsKali = parameter.isKali;
            _bank = parameter.Bank.DistroDictionaryBank;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            await Task.Run(() => { StartInstaller(); });
        }
        
        #endregion

        #region Text output
        
        /// <summary>
        ///     For letting the user knowing whats happening
        /// </summary>
        /// <param name="textToAdd"></param>
        private void NewBlockProgress(string textToAdd)
        {
            _installerDebugOutput += "-----------------------------------------------------------------------------------------------------\n";
            _installerDebugOutput += textToAdd + "\n";
            RaisePropertyChanged(() => InstallerDebugOutput);
        }

        private void SameBlockProgress(string textToAdd)
        {
            _installerDebugOutput += textToAdd + "\n";
            RaisePropertyChanged(() => InstallerDebugOutput);
        }
        
        #endregion
        
        #region Installer Methods

         /// <summary>
         /// Handles the removal of WSLInstaller folders on windows they exist before running the rest of the program
         /// </summary>
         private void RemovePriorWslInstallerFilesWindows()
         {
             //First windows cleanup
             if (Directory.Exists(InstallerCommands.windowsFolderLocation))
             {
                 SameBlockProgress($"Removing outdated GUIInstaller folders at {InstallerCommands.windowsFolderLocation} \n");
                 InstallerCommands.CommandAction(InstallerCommands.removeFolderFromWindowsFull,true);
             }
         }
        
         /// <summary>
         /// Handles the removal of WSLInstaller folders in the distro if they exist before running the rest of the program
         /// </summary>
         private void RemovePriorWslInstallerFilesFromDistro()
         {
             string testToRemoveExistingMkDir = $"/c wsl -d {Distro} {InstallerCommands.doesDistroHaveFolder}";
             string removeExistingScriptInDistro = $"/c wsl -d {Distro} {InstallerCommands.removeFolderFromDistro}";
             
             //Handels the of the Existing Script in the Distro
             string outputTestRemove = InstallerCommands.CommandAction(testToRemoveExistingMkDir, true);
             if (outputTestRemove.Contains("CONTAINS OLD VERSION"))
             {
                 SameBlockProgress($"Removing outdated batch WSLInstaller Folder located in {Distro}");
                 InstallerCommands.CommandAction(removeExistingScriptInDistro, true);
             }
         }
         
         
        /// <summary>
        ///     creates folder in C:\programs
        ///     full path is C:\programs\WSLInstaller
        /// </summary>
        private void GenerateWslInstallerFolder()
        {
            ProgressBarIndeterminate = false;
            
            SameBlockProgress($"Creating new folder at {InstallerCommands.windowsFolderLocation}\n");
            InstallerCommands.CommandAction(InstallerCommands.makeNewFolderWindows, true);
            
            ProgressBar = ProgressBar + 1;
        }
        
        /// <summary>
        ///     Creates the text file which will store the script
        /// </summary>
        private void GenerateBashScriptTextFile()
        {
            //create file
            if (!File.Exists(InstallerCommands.windowsFileLocation))
            {
                using (StreamWriter createFile = File.CreateText(InstallerCommands.windowsFileLocation))
                {
                    SameBlockProgress("Loading Commands into Script file");
                    SameBlockProgress($"Adding the following commands to the file located at {InstallerCommands.windowsFileLocation} before converting to batch script");
                    
                    //writes commands to file
                    foreach (string command in InstallerCommands.bashCommandsToAppend)
                    {
                        Thread.Sleep(300);
                        createFile.WriteLine(command); 
                        SameBlockProgress($"Successfully added command: {command} to the script.");
                        ProgressBar = ProgressBar + 1;
                    }
                }
            }
            SameBlockProgress($"Successfully added commands to the script.");
            ProgressBar = ProgressBar + 1;
        }
        
        /// <summary>
        /// Moves the script to the /tmp/WSLInstaller/GUIInstaller
        /// </summary>
        private void MoveScriptToWsl()
        {
            string command = $"/c wsl -d {Distro} {InstallerCommands.moveCommand}";
            InstallerCommands.CommandAction(command,true);
            SameBlockProgress("Moved script to {Distro} to folder {InstallerCommands.LinuxScriptFolderLocation} successfully");
            ProgressBar = ProgressBar + 1;
        }
        
        /// <summary>
        /// Corrects the Carriage returns in the script to make it runnable
        /// </summary>
        private void CorrectingCarriageReturns()
        {
            string command = $"/c wsl -d {Distro} {InstallerCommands.carrigeReturn}";
            InstallerCommands.CommandAction(command,true);
            SameBlockProgress("Correcting carriage returns to make file runnable");
            ProgressBar = ProgressBar + 1;
        }
        
        /// <summary>
        /// Converts the file to .sh
        /// </summary>
        private void ConvertToBatchShell()
        {
            string command = $"/c wsl -d {Distro} {InstallerCommands.ConvertToBatchShell}";
            InstallerCommands.CommandAction(command, true);
            ProgressBar = ProgressBar + 1;
            SameBlockProgress("Converted file to batch script successfully");
        }
        
        /// <summary>
        ///     Makes it so that the script can be excecuted
        /// </summary>
        /// <returns></returns>
        private void MakeRunnable()
        {
            string command = $"/c wsl -d {Distro} {InstallerCommands.makeRunnable}";
            InstallerCommands.CommandAction(command, true);
            SameBlockProgress($"The Script is now runnable by the distro using {command}");
            ProgressBar = ProgressBar + 1;
        }
        
        /// <summary>
        ///     Runs the bash script in the distro
        /// </summary>
        private void RunScript()
        {
            string command = $"wsl -d {Distro} {InstallerCommands.runScript}";

            //PowerShell.Create().AddScript(command).Invoke();
            //GuiInstallerCommands.CommandAction(command, false);
            Process runScript = new Process();
            runScript.StartInfo.FileName = "cmd.exe";
            runScript.StartInfo.Arguments = $"/k {command}";
            //runScript.StartInfo.UseShellExecute = false;
            runScript.StartInfo.CreateNoWindow = false;
            runScript.StartInfo.RedirectStandardOutput = false;
            runScript.Start();
            runScript.WaitForExit();
            ProgressBar = ProgressBar + 1;
        }
        
        /// <summary>
        ///     Runs the Installer
        /// </summary>
        public void StartInstaller()
        {
            try 
            { 
                //Initial cleanup
                RemovePriorWslInstallerFilesWindows();
                RemovePriorWslInstallerFilesFromDistro();
                
                //Beginning Installation
                SameBlockProgress("Beginning Installation");
                
                //Initial Folder creation
                GenerateWslInstallerFolder();
                Thread.Sleep(1000);
                
                //Creating bash file
                NewBlockProgress("Creating Bash file");
                GenerateBashScriptTextFile();
                Thread.Sleep(1000);
                
                //Moving Batch script to WSL 
                NewBlockProgress($"Moving file to {Distro}");
                MoveScriptToWsl();
                Thread.Sleep(1000);
                
                //Configuring batch file
                NewBlockProgress("Configuring Batch file");
                SameBlockProgress("Correcting Carriage returns");
                CorrectingCarriageReturns();
                Thread.Sleep(1000);
                SameBlockProgress("Converting to Shell Script");
                ConvertToBatchShell();
                Thread.Sleep(1000);
                SameBlockProgress("Making Batch Script executable");
                MakeRunnable();
                Thread.Sleep(1000);
                
                //Running file
                NewBlockProgress("The Batch Script has been launched please monitor it and type in your password when applicable. Once the script has finished the installation will be complete");
                RunScript();
                NewBlockProgress("INSTALLATION COMPLETE\n PRESS THE \"LAUNCH GUI\" BUTTON TO OPEN THE GUI VERSION OF LINUX");
            }
            catch (Exception e)
            {
                NewBlockProgress("ERROR SEE BELOW:\n");
                SameBlockProgress(e.Message);
                SameBlockProgress("INSTALLATION HAS BEEN HALTED");
                ErrorProgressBar = true;
            }
        }
        
        #endregion
        
        #region Navigation
        
        private readonly IMvxNavigationService _navigationService;
        public IMvxAsyncCommand NavPage4Command => new MvxAsyncCommand(NavPage4);
        
        public InstallPage3ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        private async Task NavPage4(){
            await _navigationService.Navigate<InstallPage4ViewModel, DistroInstanceBankNavigationArgs>(new DistroInstanceBankNavigationArgs(_bank));
        }
        
        #endregion
        
    }
}