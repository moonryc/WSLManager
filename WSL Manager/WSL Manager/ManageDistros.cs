using System;


namespace WSL_Manager
{

    public static class ManageDistros
    {
        
        /// <summary>
        /// Issues a CMD command, for hide use false to show the
        /// terminal and use true to hide the terminal
        /// </summary>
        /// <param name="command"></param>
        /// <param name="hide"></param>
        private static void User_Issued_Command(string command, bool hide)
        {
            //starts new process
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            if (hide)
            {
                process.StartInfo.CreateNoWindow = true;
            }
            
            //picks the application to run
            process.StartInfo.FileName = "cmd.exe";
            
            // takes an argument          
            process.StartInfo.Arguments = command;
            
            // runs the program
            process.Start();

            
        }
        
       /// <summary>
       /// Shuts down All distros that are active
       /// </summary>
        public static void ShutDownAllDistros()
        {
            User_Issued_Command(CMDCommands.ShutDownAll,true);
        }

       public static void ConvertWslToWslTwo(string distro)
        {
            User_Issued_Command(CMDCommands.UpgradeToWSL2 + distro + " 2",false);
        }



    }
}