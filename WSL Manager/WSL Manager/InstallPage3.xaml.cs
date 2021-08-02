using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WSL_Manager
{
    public partial class InstallPage3 : Page
    {
        private string distro = "";
        private bool isKali;
        private string progressText = "";


        public InstallPage3()
        {
            InitializeComponent();
            StartInstaller();
        }


        /// <summary>
        ///     For letting the user knowing whats happening
        /// </summary>
        /// <param name="textToAdd"></param>
        private void NewBlockProgress(string textToAdd)
        {
            progressText += "-----------------------------------------------------------------------------------------------------\n";
            progressText += textToAdd + "\n";
            InstallerText.Text = progressText;
        }

        private void SameBlockProgress(string textToAdd)
        {
            progressText += textToAdd + "\n";
            InstallerText.Text = progressText;
        }
        
        //TODO HAS ISSUES IN THIS METHOD REGARDING CLEANUP IN THE WSL FOLDER
        /// <summary>
        ///     creates folder in C:\programs
        ///     full path is C:\programs\WSLInstaller
        /// </summary>
        public string GenerateWSLInstallerFolder()
        {
            //First windows cleanup
            string returnString = "";
            if (Directory.Exists(GuiInstallerCommands.windowsFolderLocation))
            {
                returnString+= $"Removing outdated GUIInstaller folders at {GuiInstallerCommands.windowsFolderLocation} \n";
                PowerShell.Create().AddScript(GuiInstallerCommands.removeFolderFromWindowsFull).Invoke();
            }
            returnString += $"Creating new folder at {GuiInstallerCommands.windowsFolderLocation}\n";
            PowerShell.Create().AddScript(GuiInstallerCommands.makeNewFolderWindows).Invoke();
            
            
            string testToRemoveExistingMkDir = $"/c wsl -d {distro} {GuiInstallerCommands.doesDistroHaveFolder}";
            string removeExistingScriptInDistro = $"wsl -d {distro} {GuiInstallerCommands.removeFolderFromDistro}";
            
            //Handels the of the Existing Script in the Distro
            string[] outputTestRemove = GuiInstallerCommands.CommandAction(testToRemoveExistingMkDir, true);
            if (outputTestRemove[0].Contains("CONTAINS OLD VERSION") && outputTestRemove[1].Equals(""))
            {
                //TODO THIS POWERSHELL COMMAND DOES NOT WORK PROPERLY
                PowerShell.Create().AddCommand(removeExistingScriptInDistro);
                returnString += $"Removing outdated batch WSLInstaller Folder located in {distro}";
            }
            return returnString;
        }

        
        /// <summary>
        ///     Creates the text file which will store the script
        /// </summary>
        public string GenerateBashScriptTextFile()
        {
            string returnString = "";
            //create file
            if (!File.Exists(GuiInstallerCommands.windowsFileLocation))
            {
                using (StreamWriter createFile = File.CreateText(GuiInstallerCommands.windowsFileLocation))
                {
                    returnString += "Creating new script file\n";
                    returnString += $"Adding the following commands to the file located at {GuiInstallerCommands.windowsFileLocation} before converting to batch script\n";
                    //writes commands to file
                    foreach (string command in GuiInstallerCommands.bashCommandsToAppend) 
                    {
                        createFile.WriteLine(command); 
                        returnString += $"Successfully added command:\n{command} to the script.\n";
                    }
                }
            }
            return returnString;
        }
        
        
        /// <summary>
        /// Moves the script to the /tmp/WSLInstaller/GUIInstaller
        /// </summary>
        /// <param name="selectedDistro"></param>
        /// <returns></returns>
        public string MoveScriptToWsl()
        {
            string command = $"wsl -d {distro} {GuiInstallerCommands.moveCommand}";
            PowerShell.Create().AddScript(command).Invoke();
            return $"Moved script to {distro} to folder {GuiInstallerCommands.LinuxScriptFolderLocation} successfully"; 
        }
        
        /// <summary>
        /// Corrects the carrige returns in the script to make it runabble
        /// </summary>
        /// <param name="selectedDistro">enter the distro in which the script file exists</param>
        /// <returns></returns>
        public string CorrectingCarriageReturns()
        {
            string command = $"wsl -d {distro} {GuiInstallerCommands.carrigeReturn}";
            PowerShell.Create().AddScript(command).Invoke();
            return "Correcting carriage returns to make file runnable";
        }
        
        /// <summary>
        /// Converts the file to .sh
        /// </summary>
        /// <param name="selectedDistro"></param>
        /// <returns></returns>
        public string ConvertToBatchShell()
        {
            string command = $"wsl -d {distro} {GuiInstallerCommands.ConvertToBatchShell}";
            PowerShell.Create().AddScript(command).Invoke();
            return "Converted file to batch script successfully";
        }
        
        #region NOTWORKING
        
        
        //TODO MAKE THIS WORK
        /// <summary>
        ///     Makes it so that the script can be excecuted
        /// </summary>
        /// <param name="selectedDistro"></param>
        /// <returns></returns>
        public string MakeRunnable()
        {
            string command = $"wsl -d {distro} {GuiInstallerCommands.makeRunnable}";

            PowerShell.Create().AddScript(command).Invoke();
            return $"The Script is now runnable by the distro using {command}";
        }
        
        //TODO FIX IT SO THAT THE SCRIPT AUTO RUNS
        /// <summary>
        ///     Runs the bash script in the distro
        /// </summary>
        public void RunScript()
        {
            string command = $"/k wsl -d {distro} {GuiInstallerCommands.runScript}";

            //PowerShell.Create().AddScript(command).Invoke();
            //GuiInstallerCommands.CommandAction(command, false);
            Process runScript = new Process();
            runScript.StartInfo.FileName = "cmd.exe";
            runScript.StartInfo.Arguments = $"/k {command}";
            runScript.StartInfo.UseShellExecute = false;
            runScript.StartInfo.CreateNoWindow = false;
            runScript.StartInfo.RedirectStandardOutput = false;
            runScript.Start();
            runScript.WaitForExit();
        }
        
        #endregion
        
        /// <summary>
        ///     Installs all methods
        /// </summary>
        private void StartInstaller()
        {
            Dispatcher.Invoke(() =>
            {
                int numberOfCommands = 8;
                LoadingBar.Maximum = numberOfCommands;
                int loadingBarIncrease = numberOfCommands / numberOfCommands;
                //var timeToWait = 0 * 1000;
                
                 
                 try 
                 { 
                     //Begining Installation
                     SameBlockProgress("Begining Installation");
                
                     //Clearing ouf folder and making new ones
                     NewBlockProgress(GenerateWSLInstallerFolder());
                     LoadingBar.Value += loadingBarIncrease;
                     
                     //Creating bash file
                     NewBlockProgress(GenerateBashScriptTextFile());
                     LoadingBar.Value += loadingBarIncrease;
                     
                     //Moving Batch script to WSL 
                     NewBlockProgress(MoveScriptToWsl());
                     LoadingBar.Value += loadingBarIncrease;
                     
                     //Configuing batch file
                     NewBlockProgress(CorrectingCarriageReturns());
                     LoadingBar.Value += loadingBarIncrease;
                     SameBlockProgress(ConvertToBatchShell());
                     LoadingBar.Value += loadingBarIncrease;
                     
                     SameBlockProgress(MakeRunnable());
                     LoadingBar.Value += loadingBarIncrease;
                     
                     
                     
                     //Running file
                     NewBlockProgress("The script has been launched please monitor it and type in your password when applicable. Once the script has finished the installation will be complete");
                     RunScript();
                     LoadingBar.Value += loadingBarIncrease;
                     NewBlockProgress("INSTALLATION COMPLETE\n PRESS THE \"LAUNCH GUI\" BUTTON TO OPEN THE GUI VERSION OF LINUX");
                     LoadingBar.Value += loadingBarIncrease;
                 }
                 catch (Exception e)
                 {
                     NewBlockProgress("ERROR SEE BELOW");
                     SameBlockProgress(e.Message);
                     SameBlockProgress("INSTALLATION HAS BEEN HALTED");
                 }
            });
        }

        public void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var test = (object[]) e.ExtraData;

            isKali = (bool) test[0];
            distro = (string) test[1];
            NavigationService.LoadCompleted -= NavigationService_LoadCompleted;
            
        }
    }
}