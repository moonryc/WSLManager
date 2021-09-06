using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WSLManager.Logger.Core;

namespace WSLManager.Models
{
    public class DistroModel:INotifyPropertyChanged
    {

        private string _distroName;
        private bool _isRunning;
        private int _wslVersion;
        private Process _distroProcess = new Process();
        
        public string DistroName
        {
            get => _distroName;
            set
            {
                _distroName = value;
                OnPropertyChanged();
            }
        }
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }
        public int WslVersion
        {
            get => _wslVersion;
            set
            {
                _wslVersion = value;
                OnPropertyChanged();
            }
        }

        public string DistroNameStatus
        {
            get
            {

                if (DistroName.Equals("Select Distro"))
                {
                    return DistroName;
                }
                else
                {
                    return $"{ DistroName} { (IsRunning ? "Running":"Stopped")}";    
                }
            }
        }

        public DistroModel(string distroName, bool isRunning, int wslVersion)
        {
            
            _distroName = distroName;
            _isRunning = isRunning;
            _wslVersion = wslVersion;
            
        }

        public void StartDistro()
        {
            _distroProcess.StartInfo.FileName = "cmd.exe";
            _distroProcess.StartInfo.Arguments = $"/c wsl -d {DistroName}";
            _distroProcess.Start();
        }

        public void StartDistroUser(string user)
        {
            _distroProcess.StartInfo.FileName = "cmd.exe";
            _distroProcess.StartInfo.Arguments = $"/c wsl -d {DistroName} -u {user}";
            _distroProcess.Start();
        }

        public void EndDistro()
        {
            Process terminater = new Process();
            terminater.StartInfo.FileName = "cmd.exe";
            terminater.StartInfo.Arguments = $"/k wsl -t {DistroName}";
            terminater.StartInfo.CreateNoWindow = true;
            terminater.Start();
            try
            {
                _distroProcess.Kill();
            }
            catch(Exception e)
            {
                IoC.Base.IoC.baseFactory.Log(e.Message,LogLevel.Critical);
            }
            
        }
        
        
        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        //CallerMemberName = name of the variable and in this case it is directly substituted into propertyName
        //use OnPropertyChanged(); in each set for each prop to update it.
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        
    }
}