using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;


namespace WSL_Manager
{

    public class Distro
    {
        private bool isRunning = false;
        private int version = 0;
        private string distroName = "";
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        
        
        public Distro(string distroName, bool isRunning,int version)
        {
            this.isRunning = isRunning;
            this.version = version;
            this.distroName = distroName;
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }
        
        public int WSLVersion
        {
            get { return version; }
            set { version = value; }
        }

        public void StartDistro(string user)
        {
            //picks the application to run
            process.StartInfo.FileName = "cmd.exe";
            
            if (user == "Default")
            {
                // takes an argument          
                process.StartInfo.Arguments = CMDCommands.StartDistro + distroName;    
            }
            else
            {
                // takes an argument          
                string command = CMDCommands.StartDistro + distroName + CMDCommands.RunSpecificUser + user;
                process.StartInfo.Arguments = command;
            }
            
    
            // runs the program
            process.Start();
        }
        
        public void EndDistro()
        {
            System.Diagnostics.Process quit = new System.Diagnostics.Process();
            quit.StartInfo.FileName = "cmd.exe";
            quit.StartInfo.CreateNoWindow = true;
            quit.StartInfo.Arguments = CMDCommands.ShutDownSpecificDistro + distroName;
            quit.Start();    
            process.Kill();
        }
        
        
        public string DistroName
        {
            get { return distroName; }
        }

        public int Version
        {
            get { return version; }
        }
        
    }
}