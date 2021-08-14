using System.Diagnostics;
using MvvmCross.ViewModels;
using WSLManager.Core.Commands;

namespace WSLManager.Core.Models
{
    public class DistroInstance: MvxViewModel
    {

        private string _distroName;
        
        Process distroProcess = new Process();
        
        public DistroInstance(string distroName)
        {
            _distroName = distroName;
        }
        
        public void StartDistroInstance()
        {
            distroProcess = new Process();
            distroProcess.StartInfo.FileName = "cmd.exe";
            distroProcess.StartInfo.Arguments = $"{CMDCommands.StartDistro} {_distroName}";
            distroProcess.Start();
        }

        public void StartDistroInstanceUser(string user)
        {
            distroProcess = new Process();
            distroProcess.StartInfo.FileName = "cmd.exe";
            distroProcess.StartInfo.Arguments = $"{CMDCommands.StartDistro} {_distroName} -u {user}";
            distroProcess.Start();
        }

        public void StopDistroInstance()
        {
            Process closeDistroProcess = new Process();
            closeDistroProcess.StartInfo.FileName = "cmd.exe";
            closeDistroProcess.StartInfo.CreateNoWindow = true;
            closeDistroProcess.StartInfo.Arguments = $"{CMDCommands.ShutDownSpecificDistro} {_distroName}";
            closeDistroProcess.Start();
            distroProcess.Kill();
            

        }
        
        
    }
}