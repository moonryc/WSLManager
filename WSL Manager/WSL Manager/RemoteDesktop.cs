using System.Diagnostics;
using System.Windows;

namespace WSL_Manager
{
    public static class RemoteDesktop
    {
        /// <summary>
        ///     Issues a CMD command, for hide use false to show the
        ///     terminal and use true to hide the terminal
        /// </summary>
        /// <param name="command"></param>
        /// ///
        /// <param name="hide"></param>
        private static void User_Issued_Command(string command, bool hide)
        {
            //starts new process
            Process process = new Process();
            if (hide) process.StartInfo.CreateNoWindow = false;
            //picks the application to run
            process.StartInfo.FileName = "cmd.exe";

            // takes an argument          
            process.StartInfo.Arguments = command;

            // runs the program
            process.Start();

            //Wait for process to finish
            process.WaitForExit();
        }

        //THIS SHOULD PARTIALLY WORK
        public static void InstallRdp(string distro)
        {
            //quickly generates the list of packages that will be installed// commands that will be run
            string commandList = "";
            foreach (var command in CMDCommands.GuiInstallPartOne) commandList += command + "\n";

            string installPackagesConfirmation = "WARNING:\n" + "\n" +
                                              "Are you sure you wish to run the following commands and install the the following packages on the selected WSL Distro?\n \n" +
                                              $"{commandList} \n \n You are responsible for fixing any error during installation ";

            string whereErrorsFound = "Did you recive any errors during installation?";
            string errorsFound =
                "Please address any errors that you recived.\n" +
                "Once you have addressed these errors press attempt to install again to complete the setup";
            
            if (MessageBox.Show(installPackagesConfirmation, "Confirm Package Install", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                //runs the installation commands
                foreach (string command in CMDCommands.GuiInstallPartOne)
                {
                    string input = $"/k wsl -d {distro} -e {command}";
                    User_Issued_Command(input, false);
                }

                if (MessageBox.Show(whereErrorsFound, "WSL Manager", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    //TODO add in the parts with nano
                    MessageBox.Show("");
                }
                else
                {

                    MessageBox.Show(errorsFound);
                }
                

            }
        }

        //THIS SHOULD WORK
        public static void StartRdpConnection(string distro)
        {
            User_Issued_Command($"/k wsl {CMDCommands.StartLinuxRDP} {distro}", false);
        }

        public static void LaunchRdpProgram()
        {
            MessageBox.Show("The default Login is: \"localhost:3390\"\n If you would " +
                            "like to change the port run: \"sudo sed -i ‘s/3389/PLACEDESIREDPORTHERE/g’ /etc/xrdp/xrdp.ini\"  ",
                "Remote Desktop Connection Login");
        }
    }
}