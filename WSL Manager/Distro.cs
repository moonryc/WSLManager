using System.Diagnostics;

namespace WSL_Manager
{
    public class Distro
    {
        private readonly Process process = new Process();


        public Distro(string distroName, bool isRunning, int version)
        {
            IsRunning = isRunning;
            WSLVersion = version;
            DistroName = distroName;
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
                var command = $"{CMDCommands.StartDistro} {DistroName} {CMDCommands.RunSpecificUser} {user}";
                process.StartInfo.Arguments = command;
            }


            // runs the program
            process.Start();
        }

        public void EndDistro()
        {
            var quit = new Process();
            quit.StartInfo.FileName = "cmd.exe";
            quit.StartInfo.CreateNoWindow = true;
            quit.StartInfo.Arguments = $"{CMDCommands.ShutDownSpecificDistro} {DistroName}";
            quit.Start();
            process.Kill();
        }
    }
}