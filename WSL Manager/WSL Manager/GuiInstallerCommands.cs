using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace WSL_Manager
{
    /// <summary>
    ///     Interaction logic for PopUpWindow.xaml
    /// </summary>
    public static class GuiInstallerCommands
    {
        //File information
        public static string fileName { get; } = "GUIInstallerScript";
        public static string folderName { get; } = "WSLInstaller";
        public static string windowsFolderLocation { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folderName); // /wslinstaller
        public static string windowsFileLocation { get; } = Path.Combine(windowsFolderLocation, fileName); // /wslinstaller/guiinstallerscript
        public static string linuxFolderToMoveFolderTo { get; } = "/tmp";
        public static string LinuxScriptFolderLocation { get; } = $"{linuxFolderToMoveFolderTo}/{folderName}";
        public static string LinuxScriptFileLocation { get; } = $"{LinuxScriptFolderLocation}/{fileName}";
        public static string linuxFilePathToWindowsLocationForCopying { get; } = windowsFolderLocation.Replace(@"\", "/").Replace("C:", "/mnt/c");


        //commands
        //TODO FIX THIS COMMAND
        public static string removeFolderFromDistro {get;}= $"-u root -e rm -rf {LinuxScriptFolderLocation}";
        public static string doesDistroHaveFolder {get;}= $"-e test -d \"{LinuxScriptFolderLocation}\" && echo \"CONTAINS OLD VERSION\"";
        public static string removeFolderFromWindowsFull {get;}= $"Remove-Item {windowsFolderLocation} -Force  -Recurse -ErrorAction SilentlyContinue";
        public static string makeNewFolderWindows {get;}= $"mkdir {windowsFolderLocation}";

        public static string moveCommand { get; } = $"-u root -e cp -R {linuxFilePathToWindowsLocationForCopying} {linuxFolderToMoveFolderTo}";

        public static string ConvertToBatchShell { get; } = $"-u root -e mv {LinuxScriptFileLocation} {LinuxScriptFileLocation}.sh";

        public static string carrigeReturn { get; } = $"-u root -e sed -i -e {Regex.Escape("\"s/\r") + "$" + Regex.Escape("//\"").Replace(" ", "")} {LinuxScriptFileLocation}";

        public static string makeRunnable { get; } = $"-u root -e chmod +x {LinuxScriptFileLocation}.sh";
        //TODO FIX runSCript command
        public static string runScript { get; } = $"-e sudo sh {LinuxScriptFileLocation}.sh";
        
        public static string[] bashCommandsToAppend { get; }=
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
        
        /// <summary>
        /// Runs the user entered CMD
        /// </summary>
        /// <param name="command">Enter CMD command you wish to run</param>
        /// <param name="hide">True if you want to hid the CMD window</param>
        /// <returns></returns>
        public static string[] CommandAction(string command, bool hide)
        {
            //starts new process
            Process process = new Process();
            if (hide) process.StartInfo.CreateNoWindow = true;
            //picks the application to run
            process.StartInfo.FileName = "cmd.exe";

            // takes an argument          
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            // runs the program
            process.Start();

            //Wait for process to finish
            string[] returnText =
            {
                process.StandardOutput.ReadToEnd().Replace("*", "").Replace("\0", "").Replace("\r", ""),
                process.StandardError.ReadToEnd().Replace("*", "").Replace("\0", "").Replace("\r", "")
            };
            process.WaitForExit();
            for (int i = 0; i < returnText.Length; i++)
                while (returnText[i].Contains("  "))
                    returnText[i] = returnText[i].Replace("  ", " ");
            return returnText;
        }
    }
}