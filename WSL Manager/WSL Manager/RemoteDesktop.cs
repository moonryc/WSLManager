using System;


namespace WSL_Manager
{
    public static class RemoteDesktop
    {
        /// <summary>
        /// Issues a CMD command, for hide use false to show the
        /// terminal and use true to hide the terminal
        /// </summary>
        /// <param name="command"></param>
        /// /// <param name="hide"></param>
        private static void User_Issued_Command(string command,bool hide)
        {
            //starts new process
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            if (hide)
            {
                process.StartInfo.CreateNoWindow = false;
            }
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
            User_Issued_Command(CMDCommands.ShutDownAll, false);
            
            //this is only the first half, the steps with nano have yet to be done.
            foreach (string command in CMDCommands.GuiInstall)
            {
                //User_Issued_Command("/k wsl " + command,false);    
            }
            
            //TODO add in the parts with nano
            
        }

        //THIS SHOULD WORK
        public static void StartRdpConnection(string distro)
        {
            User_Issued_Command("/k wsl " + CMDCommands.StartLinuxRDP + distro,false);
        }

        public static void LaunchRdpProgram()
        {
            throw new Exception("THIS HAS NOT BEEN IMPLEMENTED YET");
        }
        
    }
}