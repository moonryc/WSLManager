using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace WSLManager.Models
{
    public class DistroBankModel
    {
        private Dictionary<string, DistroModel> _distroDictionary;
        
        public Dictionary<string,DistroModel> DistroDictionary { get=>_distroDictionary; }
        
        public DistroBankModel()
        {
            _distroDictionary = new Dictionary<string, DistroModel>();
            UpdateDictionary();
        }
        
        /// <summary>
        /// creates a list of distros installed, their statuses and their WSL versions
        /// </summary>
        /// <returns></returns>
        private List<string[]> UpdateList()
        {
            List<string[]> _distroList = new List<string[]>();

            #region CMD Process
            
            Process process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c wsl -l -v";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            #endregion

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();


            //Remove unneed characters from output
            output = output.Replace("*", "").Replace("\0", "").Replace("\r", "");
            while (output.Contains("  ")) output = output.Replace("  ", " ");


            //Add distro info to distroList
            var outputArray = output.Split("\n");

            for (var line = 1; line < outputArray.Length - 1; line++)
            {
                var modifiedLine = outputArray[line].Split(" ");

                _distroList.Add(new[] {modifiedLine[1], modifiedLine[2], modifiedLine[3]});
            }

            return _distroList;
        }

        /// <summary>
        /// On call updates the distro dictionary
        /// </summary>
        public void UpdateDictionary()
        {
            List<string[]> distroList = UpdateList();

            foreach (string[] distro in distroList)
            {
                string nameDistro = distro[0];
                bool isRunning = distro[1].Equals("Running");
                int wslVersion = Int32.Parse(distro[2]);

                if (_distroDictionary.ContainsKey(nameDistro))
                {
                    _distroDictionary[nameDistro].IsRunning = isRunning;
                    _distroDictionary[nameDistro].WslVersion = wslVersion;
                }
                else
                {
                    _distroDictionary.Add(nameDistro,new DistroModel(nameDistro,isRunning,wslVersion));
                }
            }
        }

        /// <summary>
        /// Generates a list of strings for the observable collection
        /// </summary>
        /// <returns>List of strings</returns>
        public List<string> GetStatus()
        {
            List<string> statusList = new List<string>();

            foreach (KeyValuePair<string,DistroModel> keyValuePair in _distroDictionary)
            {
                statusList.Add(keyValuePair.Value.DistroNameStatus);
            }
            
            return statusList;
        }


        
    }
}