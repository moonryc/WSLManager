using System.Diagnostics;

namespace WSL_Manager
{
    public class Distro
    {
        private bool isRunning = false;
        private int version = 0;
        private string distroName = "";
        Process process = new Process();
        
        
        public Distro(string distroName, bool isRunning,int version)
        {
            this.isRunning = isRunning;
            this.version = version;
            this.distroName = distroName;
        }

        public bool IsRunning { get; set; }

        public int WSLVersion { get; set; }


        public string DistroName { get; } = "";

        public int Version => WSLVersion;

        public void StartDistro(string user)
        {
            //picks the application to run
            process.StartInfo.FileName = "cmd.exe";

            if (user == "Default")
            {
                // takes an argument          
                process.StartInfo.Arguments = $"{CMDCommands.StartDistro} {DistroName}";
            }
            else
            {
                // takes an argument          
                string command = $"{CMDCommands.StartDistro} {DistroName} {CMDCommands.RunSpecificUser} {user}";
                process.StartInfo.Arguments = command;
            }


            // runs the program
            process.Start();
        }

        public void EndDistro()
        {
            Process quit = new Process();
            quit.StartInfo.FileName = "cmd.exe";
            quit.StartInfo.CreateNoWindow = true;
            quit.StartInfo.Arguments = $"{CMDCommands.ShutDownSpecificDistro} {DistroName}";
            quit.Start();
            process.Kill();
        }
    }
}