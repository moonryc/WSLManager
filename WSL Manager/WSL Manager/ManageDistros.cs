using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management.Automation;
using System.Text.RegularExpressions;


namespace WSL_Manager
{

    public static class ManageDistros
    {
        private static void test()
        {
            PowerShell s = PowerShell.Create();    
        }
        
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
                process.StartInfo.CreateNoWindow = false;
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
        
        /// <summary>
        /// Terminates the Selected Distro
        /// </summary>
        /// <param name="distro"></param>
        public static void ShutDownSelectedDistro(string distro)
        {
            string command = (CMDCommands.ShutDownSpecificDistro + distro);
            User_Issued_Command(command,true);
        }
        
        /// <summary>
        /// Launches the selected Distro
        /// </summary>
        /// <param name="distro"></param>
        public static void LaunchSelectedDistro(string distro)
        {
            //Starts the selected Linux distro
       
                string command = (CMDCommands.StartDistro + distro);
                User_Issued_Command(command,false);
        }
        
        public static void ConvertWSLToWSLTWO()
        {
            //converts the selected WSL to WSL2
            throw new Exception("THIS FEATURE HAS NOT BEEN IMPLIMENTED YET");
        }

        /// <summary>
        /// Gets List of installed distros on the pc
        /// </summary>
        /// <returns></returns>
        public static List<string[]> ListOfDistros()
        {
            List<string[]> distroList = new List<string[]>();
            
            //Create process
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.CreateNoWindow = true;
            //strCommand is path and file name of command to run
            process.StartInfo.FileName = "cmd.exe";
            //strCommandParameters are parameters to pass to program
            process.StartInfo.Arguments = CMDCommands.ListInstalledDistros;
            process.StartInfo.UseShellExecute = false;
            //Set output of program to be written to process output stream
            process.StartInfo.RedirectStandardOutput = true;   
            //Start the process
            process.Start();

            //Get program output
            string output = process.StandardOutput.ReadToEnd();

            //Wait for process to finish
            process.WaitForExit();

            string[] outputArray = output.Replace("*", "").Replace("\0","").Split("\n");
            
            for (int line = 1; line < outputArray.Length - 1; line++)
            {
                string[] modifiedLine = outputArray[line].Split(" ");
                string[] temp = modifiedLine;
                
                if (line == 1)
                {
                    distroList.Add( new[] {modifiedLine[1], modifiedLine[7], modifiedLine[16]} );
                }
                else
                {
                    distroList.Add( new[] {modifiedLine[2], modifiedLine[6], modifiedLine[15]} );
                }
                //1,7,17
            }
            return distroList;
        }

    }
}