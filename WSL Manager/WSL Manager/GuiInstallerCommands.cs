using System;
using System.Diagnostics;
using System.IO;

namespace WSL_Manager
{
    /// <summary>
    ///     Interaction logic for PopUpWindow.xaml
    /// </summary>
    public static class GuiInstallerCommands
    {
        private static readonly string fileName = "GUIInstallerScript";
        private static readonly string tempFilePathLocation = Path.Combine(GetInstallLocation, fileName);

        private static readonly string[] bashCommandsToAppend =
        {
            "!# /bin/bash ", "sudo apt update ", "sudo apt -y upgrade", "sudo apt-get purge xrdp",
            "sudo apt install -y xrdp", "sudo apt install -y xfce4", "sudo apt install -y xfce4-goodies",
            "sudo cp /etc/xrdp/xrdp.ini /etc/xrdp/xrdp.ini.bak", "sudo sed -i \"s/3389/3390/g\" /etc/xrdp/xrdp.ini",
            "sudo sed -i \"s/max_bpp=32/#max_bpp=32\\nmax_bpp=128/g\" /etc/xrdp/xrdp.ini",
            "sudo sed -i \"s/xserverbpp=24/#xserverbpp=24\\nxserverbpp=128/g\" /etc/xrdp/xrdp.ini",
            "echo xfce4-session > ~/.xsession",
            "sudo su - && sudo echo -e \"# xfce\" >> /etc/xrdp/startwm.sh",
            "sudo su - && sudo echo -e \"startxfce\" >> /etc/xrdp/startwm.sh",
            "exit"
        };

        public static string GetInstallLocation { get; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WSLInstaller");


        private static string[] CommandAction(string command, bool hide)
        {
            //starts new process
            var process = new Process();
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
            for (var i = 0; i < returnText.Length; i++)
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
                if (!File.Exists(GetInstallLocation))
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
            var returnString = "";

            //DELETE OLD SCRIPT
            try
            {
                //remove existing file
                if (File.Exists(tempFilePathLocation))
                {
                    CommandAction($"/c del {tempFilePathLocation}", true);
                    returnString +=
                        $"Removing old script file located in {tempFilePathLocation} to ensure latest version\n";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //create file
            try
            {
                if (!File.Exists(tempFilePathLocation))
                {
                    using (var createFile = File.CreateText(tempFilePathLocation))
                    {
                        returnString += "Creating new script file\n";

                        //Add the script to the file
                        try
                        {
                            returnString +=
                                $"Adding the following commands to the file located at {tempFilePathLocation} before converting to batch script\n";
                            //writes commands to file
                            foreach (var command in bashCommandsToAppend)
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
            var returnString = "";

            var testToRemoveExistingScriptInDistro =
                $"/c wsl -d {selectedDistro} -e test -f /tmp/WSLInstaller/GUIInstallerScript && echo \"CONTAINS OLD VERSION\"";
            var testToRemoveExistingMkDir =
                $"/c wsl -d {selectedDistro} -e test -d /tmp/WSLInstaller && echo \"CONTAINS OLD VERSION\"";
            var removeExistingScriptInDistro = $"/c wsl -d {selectedDistro} -e rm /tmp/WSLInstaller/GUIInstallerScript";
            var removeExistingWSLInstallerDirectoryInDistro = $"/c wsl -d {selectedDistro} -e rm -r /tmp/WSLInstaller";

            try
            {
                //Handels the of the Existing Script in the Distro
                var outputTestRemove = CommandAction(testToRemoveExistingScriptInDistro, true);
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
                var outputTestMkDirExists = CommandAction(testToRemoveExistingMkDir, true);
                if (outputTestMkDirExists[0].Contains("CONTAINS OLD VERSION") && outputTestMkDirExists[1].Equals(""))
                {
                    //Remove Directory
                    var outputRemoveMkdir = CommandAction(removeExistingWSLInstallerDirectoryInDistro, true);
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
            var returnString = "";
            var scriptDestination = "/temp";
            var linuxFilePathLocation = tempFilePathLocation.Replace(@"\", "/").Replace("C:", "/mnt/c");
            var mkdirToMoveTo = $"/c wsl -d {selectedDistro} -e mkdir /tmp/WSLInstaller";
            var moveCommand = $"/c wsl -d {selectedDistro} -e cp {linuxFilePathLocation} {scriptDestination}";


            try
            {
                //Make directory to move to
                var outputMkdir = CommandAction(mkdirToMoveTo, true);
                if (!outputMkdir[1].Equals(""))
                    throw new Exception(outputMkdir[1]);
                returnString += $"Directory created in {selectedDistro} located under /temp/WSLInstaller";

                //Moves the file
                var outputMove = CommandAction(moveCommand, true);
                if (!outputMove[1].Equals(""))
                    throw new Exception(outputMove[1]);
                returnString += $"Moved script to {scriptDestination} successfully";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return returnString;
        }

        public static string ConvertToBatchShell(string selectedDistro)
        {
            var convertToBatchShell =
                CommandAction($"/c wsl -d {selectedDistro} -e mv {fileName} /{fileName}.sh", true);

            if (!convertToBatchShell[1].Equals(""))
                return convertToBatchShell[0];
            throw new Exception("ERROR");
        }

        public static string MakeRunnable(string selectedDistro)
        {
            var makeRunnable = CommandAction($"/k wsl -d {selectedDistro} -e chmod +x /{fileName}.sh", false);

            if (makeRunnable[1].Equals(""))
                //TODO
                return
                    "makeRunnable[0]SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS";
            throw new Exception(makeRunnable[1]);
        }

        /// <summary>
        ///     Runs the bash script in the distro
        /// </summary>
        public static void RunScript(string selectedDistro)
        {
            var runScript = new Process();
            runScript.StartInfo.FileName = "cmd.exe";
            runScript.StartInfo.Arguments = $"/k wsl -d {selectedDistro} -e ./{fileName}.sh";
            runScript.Start();
        }
    }
}