using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace WSL_Manager
{
    /// <summary>
    ///     Interaction logic for PopUpWindow.xaml
    /// </summary>
    public static class GuiInstallerCommands
    {
        private static readonly string fileName = "GUIInstallerScript";
        private static readonly string folderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WSLInstaller");
        private static readonly string fileLocation = Path.Combine(folderLocation, fileName);
        

        private static readonly string[] bashCommandsToAppend =
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

        public static string GetInstallLocation { get; } =
            folderLocation;


        private static string[] CommandAction(string command, bool hide)
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
        
        /// <summary>
        ///     creates folder in C:\programs
        ///     full path is C:\programs\WSLInstaller
        /// </summary>
        public static string GenerateWSLInstallerFolder()
        {
            try
            {
                if (!File.Exists(folderLocation))
                {
                    Directory.CreateDirectory(GetInstallLocation);
                    return $"Folder created at {GetInstallLocation}";
                }

                return $"Folder already exists at {GetInstallLocation}";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///     Creates the text file which will store the script
        /// </summary>
        public static string GenerateBashScriptTextFile()
        {
            string returnString = "";

            //DELETE OLD SCRIPT
            try
            {
                //remove existing file
                if (File.Exists(fileLocation))
                {
                    CommandAction($"/c del {fileLocation}", true);
                    returnString +=
                        $"Removing old script file located in {fileLocation} to ensure latest version\n";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //create file
            try
            {
                if (!File.Exists(fileLocation))
                {
                    using (StreamWriter createFile = File.CreateText(fileLocation))
                    {
                        returnString += "Creating new script file\n";

                        //Add the script to the file
                        try
                        {
                            returnString +=
                                $"Adding the following commands to the file located at {fileLocation} before converting to batch script\n";
                            //writes commands to file
                            foreach (string command in bashCommandsToAppend)
                            {
                                createFile.WriteLine(command);
                                returnString += command + '\n';
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message);
                        }
                    }

                    return returnString;
                }

                throw new Exception("ERROR:\n FILE WAS NOT REMOVED PROPERLY");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///     Removes outdated Directoies and files that the WSL Installer places in the distro
        /// </summary>
        /// <param name="selectedDistro"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string RemoveOutDatedFolderAndFilesOnDistro(string selectedDistro)
        {
            string returnString = "";

            string testToRemoveExistingScriptInDistro =
                $"/c wsl -d {selectedDistro} -e test -f /tmp/WSLInstaller/GUIInstallerScript && echo \"CONTAINS OLD VERSION\"";
            string testToRemoveExistingMkDir =
                $"/c wsl -d {selectedDistro} -e test -d /tmp/WSLInstaller && echo \"CONTAINS OLD VERSION\"";
            string removeExistingScriptInDistro = $"/c wsl -d {selectedDistro} -e -u - root rm /tmp/WSLInstaller/GUIInstallerScript";
            string removeExistingWSLInstallerDirectoryInDistro = $"/c wsl -d -u root {selectedDistro} -e rm -r /tmp/WSLInstaller";

            try
            {
                //Handels the of the Existing Script in the Distro
                string[] outputTestRemove = CommandAction(testToRemoveExistingScriptInDistro, true);
                if (outputTestRemove[0].Contains("CONTAINS OLD VERSION") && outputTestRemove[1].Equals(""))
                {
                    //Remove the existing file
                    var outputRemove = CommandAction(removeExistingScriptInDistro, true);
                    if (!outputRemove[1].Equals(""))
                        throw new Exception(outputRemove[1]);
                    returnString +=
                        $"Removing outdated batch WSLInstaller script file located in {selectedDistro}\n";
                }


                //Handels the Removal of the Existing WSLInstallerFolder in the Distro
                string[] outputTestMkDirExists = CommandAction(testToRemoveExistingMkDir, true);
                if (outputTestMkDirExists[0].Contains("CONTAINS OLD VERSION") && outputTestMkDirExists[1].Equals(""))
                {
                    //Remove Directory
                    string[] outputRemoveMkdir = CommandAction(removeExistingWSLInstallerDirectoryInDistro, true);
                    if (!outputRemoveMkdir[1].Equals(""))
                        throw new Exception(outputRemoveMkdir[1]);
                    returnString += $"Removing outdated WSLInstaller directory in {selectedDistro}";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return returnString;
        }

        /// <summary>
        ///     takes the script from the c drive and moves it to the distro installation where it can be run
        ///     more easily and converts it to a useable bash script
        /// </summary>
        public static string MoveTxtToWsl(string selectedDistro)
        {
            string returnString = "";
            string scriptDestination = "/tmp";
            string linuxFilePathLocation = folderLocation.Replace(@"\", "/").Replace("C:", "/mnt/c");
            string mkdirToMoveTo = $"/c wsl -d {selectedDistro} -e mkdir /tmp/WSLInstaller";
            string moveCommand = $"/c wsl -d {selectedDistro} -u root -e cp -R {linuxFilePathLocation} {scriptDestination}";


            try
            {
                // //Make directory to move to
                // string[] outputMkdir = CommandAction(mkdirToMoveTo, true);
                // if (!outputMkdir[1].Equals(""))
                //     throw new Exception(outputMkdir[1]);
                // returnString += $"Directory created in {selectedDistro} located under /temp/WSLInstaller";

                //Moves the file
                string[] outputMove = CommandAction(moveCommand, true);
                if (!outputMove[1].Equals(""))
                    throw new Exception(outputMove[1]);
                returnString += $"Moved script to {folderLocation} successfully";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return returnString;
        }

        public static string ConvertToBatchShell(string selectedDistro)
        {
            string[] convertToBatchShell =
                CommandAction($"/c wsl -d {selectedDistro} -u root -e mv /tmp/WSLInstaller/{fileName} /tmp/WSLInstaller/{fileName}.sh", true);

            if (!convertToBatchShell[1].Equals(""))
                throw new Exception("ERROR CONVERTING FILE TO BATCH SCRIPT");
            return "Converted file to batch script successfully";
        }

        public static string CorrectingCarriageReturns(string selectedDistro)
        {
            string command = $"wsl -d {selectedDistro} -u root -e sed -i -e {Regex.Escape("\"s/\r") +"$" +Regex.Escape("//\"").Replace(" ", "")} /tmp/WSLInstaller/{fileName}.sh";
            //string[] output = CommandAction(command, true);
            
            PowerShell.Create().AddScript(command).Invoke();
            return "Correcting carriage returns to make file runnable";
        }
        
        public static string MakeRunnable(string selectedDistro)
        {
            string[] makeRunnable = CommandAction($"/c wsl -d {selectedDistro} -u root -e chmod +x /tmp/WSLInstaller/{fileName}.sh", true);
            
            if (!makeRunnable[1].Equals(""))
                throw new Exception(makeRunnable[1]);
            return $"The Script is now runnable by the distro using chmod +x /{fileName}.sh";
            
        }

        /// <summary>
        ///     Runs the bash script in the distro
        /// </summary>
        public static void RunScript(string selectedDistro)
        {
            Process runScript = new Process();
            runScript.StartInfo.FileName = "cmd.exe";
            runScript.StartInfo.Arguments = $"/k wsl -d {selectedDistro} -e sh /tmp/WSLInstaller/{fileName}.sh";
            runScript.Start();
        }
    }
}