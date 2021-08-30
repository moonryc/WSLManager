using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace WSLManager.Models
{
    public class InstallerModel
    {
        private bool _isKali;
        private string _distro = "";
        
        public InstallerModel(string distro, bool isKali)
        {
            _isKali = isKali;
            _distro = distro;
        }

        #region Constants

        private static string fileName { get; } = "GUIInstallerScript";
        private static string folderName { get; } = "WSLInstaller";
        private static string windowsFolderLocation { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folderName); // /wslinstaller
        private static string windowsFileLocation { get; } = Path.Combine(windowsFolderLocation, fileName); // /wslinstaller/guiinstallerscript
        private static string linuxFolderToMoveFolderTo { get; } = "/tmp";
        private static string LinuxScriptFolderLocation { get; } = $"{linuxFolderToMoveFolderTo}/{folderName}";
        private static string LinuxScriptFileLocation { get; } = $"{LinuxScriptFolderLocation}/{fileName}";
        private static string linuxFilePathToWindowsLocationForCopying { get; } = windowsFolderLocation.Replace(@"\", "/").Replace("C:", "/mnt/c");
        private static string removeFolderFromDistro {get;}= $"-u root -e rm -rf {LinuxScriptFolderLocation}";
        private static string doesDistroHaveFolder {get;}= $"-e test -d \"{LinuxScriptFolderLocation}\" && echo \"CONTAINS OLD VERSION\"";
        private static string removeFolderFromWindowsFull {get;}= $"/c rmdir /s /q {windowsFolderLocation} ";
        //$"/c Remove-Item {windowsFolderLocation} -Force  -Recurse -ErrorAction SilentlyContinue";
        private static string makeNewFolderWindows {get;}= $"/c mkdir {windowsFolderLocation}";
        private static string moveCommand { get; } = $"-u root -e cp -R {linuxFilePathToWindowsLocationForCopying} {linuxFolderToMoveFolderTo}";
        private static string convertToBatchShell { get; } = $"-u root -e mv {LinuxScriptFileLocation} {LinuxScriptFileLocation}.sh";
        private static string carrigeReturn { get; } = $"-u root -e sed -i -e {Regex.Escape("\"s/\r") + "$" + Regex.Escape("//\"").Replace(" ", "")} {LinuxScriptFileLocation}";
        private static string makeRunnable { get; } = $"-u root -e chmod +x {LinuxScriptFileLocation}.sh";
        private static string runScriptCommand { get; } = $"-e sudo sh {LinuxScriptFileLocation}.sh";
        private static string[] bashCommandsToAppend { get; }=
        {
            "#!/bin/bash",
            "sudo apt-get update",
            "sudo apt -y upgrade",
            "sudo apt-get purge xrdp",
            "sudo apt install -y xrdp",
            "sudo apt install -y xfce4",
            "sudo apt install -y xfce4-goodies",
            "sudo cp /etc/xrdp/xrdp.ini /etc/xrdp/xrdp.ini.bak",
            "sudo sed -i \"s/3389/3390/g\" /etc/xrdp/xrdp.ini",
            "sudo sed -i \"s/max_bpp=32/#max_bpp=32\\nmax_bpp=128/g\" /etc/xrdp/xrdp.ini",
            "sudo sed -i \"s/xserverbpp=24/#xserverbpp=24\\nxserverbpp=128/g\" /etc/xrdp/xrdp.ini",
            "echo xfce4-session > ~/.xsession",
            "sudo su - && sudo echo -e \"# xfce\" >> /etc/xrdp/startwm.sh",
            "sudo su - && sudo echo -e \"startxfce\" >> /etc/xrdp/startwm.sh",
            "exit",
        };

        public string WindowsFileLocation { get=>windowsFileLocation;}
        public string[] BashCommandsToAppend { get=>bashCommandsToAppend; }
        
        #endregion
        
        /// <summary>
        /// Runs the user entered CMD
        /// </summary>
        /// <param name="command">Enter CMD command you wish to run</param>
        /// <param name="hide">True if you want to hid the CMD window</param>
        /// <returns></returns>
        public static string CommandAction(string command, bool hide)
        {
            //starts new process
            Process process = new Process();
            if (hide) process.StartInfo.CreateNoWindow = true;
            //picks the application to run
            process.StartInfo.FileName = "cmd.exe";

            // takes an argument          
            process.StartInfo.Arguments = command;
            //process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            // runs the program
            process.Start();

            //Wait for process to finish
            string returnText = process.StandardOutput.ReadToEnd();                          
            string error = process.StandardError.ReadToEnd();
            if (!error.Equals(""))
            {
                throw new Exception(error);
            }           
            process.WaitForExit();
            returnText = returnText.Replace("*", "").Replace("\0", "").Replace("\r", "");
            for (int i = 0; i < returnText.Length; i++)
                while (returnText.Contains("  "))
                    returnText = returnText.Replace("  ", " ");
            return returnText;
        }
        
        #region Installer Methods

         /// <summary>
         /// Handles the removal of WSLInstaller folders on windows they exist before running the rest of the program
         /// </summary>
         public string RemovePriorWslInstallerFilesWindows()
         {
             //First windows cleanup
             if (Directory.Exists(windowsFolderLocation))
             {
                 CommandAction(removeFolderFromWindowsFull,true);
                 return $"Removing outdated GUIInstaller folders at {windowsFolderLocation} \n";
             }
             return "";
         }
        
         /// <summary>
         /// Handles the removal of WSLInstaller folders in the distro if they exist before running the rest of the program
         /// </summary>
         public string RemovePriorWslInstallerFilesFromDistro()
         {
             string testToRemoveExistingMkDir = $"/c wsl -d {_distro} {doesDistroHaveFolder}";
             string removeExistingScriptInDistro = $"/c wsl -d {_distro} {removeFolderFromDistro}";
             
             //Handels the of the Existing Script in the Distro
             string outputTestRemove = CommandAction(testToRemoveExistingMkDir, true);
             if (outputTestRemove.Contains("CONTAINS OLD VERSION"))
             {
                 CommandAction(removeExistingScriptInDistro, true);
                 return $"Removing outdated batch WSLInstaller Folder located in {_distro}";
             }
             return "";
         }
         
        /// <summary>
        ///     creates folder in C:\programs
        ///     full path is C:\programs\WSLInstaller
        /// </summary>
        public string GenerateWslInstallerFolder()
        {
            CommandAction(makeNewFolderWindows, true);
            return $"Creating new folder at {windowsFolderLocation}\n";
        }
        
        /// <summary>
        ///     Creates the text file which will store the script
        /// </summary>
        public void GenerateBashScriptTextFile()
        {
            //create file
            if (!File.Exists(windowsFileLocation))
            {
                using (StreamWriter createFile = File.CreateText(windowsFileLocation))
                {
                    //writes commands to file
                    foreach (string command in bashCommandsToAppend)
                    {
                        Thread.Sleep(300);
                        createFile.WriteLine(command);
                    }
                }
            }
            
        }
        
        /// <summary>
        /// Moves the script to the /tmp/WSLInstaller/GUIInstaller
        /// </summary>
        public string MoveScriptToWsl()
        {
            string command = $"/c wsl -d {_distro} {moveCommand}";
            CommandAction(command,true);
            return "Moved script to {Distro} to folder {InstallerCommands.LinuxScriptFolderLocation} successfully";
        }
        
        /// <summary>
        /// Corrects the Carriage returns in the script to make it runnable
        /// </summary>
        public string CorrectingCarriageReturns()
        {
            string command = $"/c wsl -d {_distro} {carrigeReturn}";
            CommandAction(command,true);
            return "Correcting carriage returns to make file runnable";
        }
        
        /// <summary>
        /// Converts the file to .sh
        /// </summary>
        public string ConvertToBatchShell()
        {
            string command = $"/c wsl -d {_distro} {convertToBatchShell}";
            CommandAction(command, true);
            return "Converted file to batch script successfully";
        }
        
        /// <summary>
        ///     Makes it so that the script can be excecuted
        /// </summary>
        /// <returns></returns>
        public string MakeRunnable()
        {
            string command = $"/c wsl -d {_distro} {makeRunnable}";
            CommandAction(command, true);
            return $"The Script is now runnable by the distro using {command}";
        }
        
        /// <summary>
        ///     Runs the bash script in the distro
        /// </summary>
        public void RunScript()
        {
            string command = $"wsl -d {_distro} {runScriptCommand}";
            
            Process runScript = new Process();
            runScript.StartInfo.FileName = "cmd.exe";
            runScript.StartInfo.Arguments = $"/k {command}";
            runScript.StartInfo.CreateNoWindow = false;
            runScript.StartInfo.RedirectStandardOutput = false;
            runScript.Start();
            runScript.WaitForExit();
        }
        
        #endregion
    }
}