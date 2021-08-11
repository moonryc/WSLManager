using System.Collections.Generic;
using System.Diagnostics;

namespace WSLManager.Core.Commands
{
    public static class DataCommands
    {
        public static List<string[]> GenerateListOfDistros()
        {
            List<string[]> _distroList = new List<string[]>();

            #region CMD create in command line

            //Create process
            Process process = new Process();
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
    }
}