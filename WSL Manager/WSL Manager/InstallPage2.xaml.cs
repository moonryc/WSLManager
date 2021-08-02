using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;



namespace WSL_Manager
{
    public partial class InstallPage2 : Page
    {
        List<string[]> _distroList = new List<string[]>();
        
        #region GET-ers
        
        /// <summary>
        /// Returns the selected distro in the drop down menu
        /// </summary>
        /// <returns></returns>
        private string GetSelectedDistro()
        {
            return ListOfDistros.SelectionBoxItem.ToString();
        }
        /// <summary>
        /// Return true or false regarding if the kali checkbox has been checked
        /// </summary>
        /// <returns></returns>
        private bool GetIsKali()
        {
            if (IsKali.IsChecked == false)
            {
                return false;
            }

            return true;
        }

        #endregion
        
        #region Dropdown distro select
        
        /// <summary>
        ///     Updates the List of Distros that are WSL2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistroListDropDown(object sender, EventArgs eventArgs)
        {
            ListOfDistros.Items.Clear();

            RefreshDistroList();

            ListOfDistros.Items.Add("Select Distro");
            //adds new items to the drop down menu
            foreach (string[] distro in _distroList)
            {
                //spelled it out for legibility
                string distroNameTemp = distro[0];
                int distroVersionTemp = int.Parse(distro[2]);

                if (distroVersionTemp == 2)
                {
                    //add the item to the list
                    ListOfDistros.Items.Add(distroNameTemp);    
                }
            }

            //Set Selection in combo box to whatever the previous selection was
            ListOfDistros.Items.MoveCurrentToFirst();

            
        }

        private void DistroListDropDownClose(object sender, EventArgs eventArgs)
        {
            if (ListOfDistros.SelectionBoxItem.ToString().Contains("Select"))
            {
                Install.IsEnabled = false;
            }
            else
            {
                Install.IsEnabled = true;
            }
        }
        
        /// <summary>
        ///     Update List of installed Distros and distro statuses
        ///     Refreshes the distro list for new distros and changes to exiting distros
        /// </summary>
        private void RefreshDistroList()
        {
            _distroList = new List<string[]>();

            #region CMD create in command line

            //Create process
            var process = new Process();
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
        }
        
        #endregion
        
        #region BUTTONS
        
        /// <summary>
        /// Go back to page 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackPageTwo(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InstallPage1());
        }

        /// <summary>
        /// Go to page 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginInstall(object sender, RoutedEventArgs e)
        {
            
            object[] isKaliAndDistroSelected = new object[] {GetIsKali(), GetSelectedDistro()};
            InstallPage3 installer = new InstallPage3();
            this.NavigationService.LoadCompleted += installer.NavigationService_LoadCompleted;
            this.NavigationService.Navigate(installer, isKaliAndDistroSelected);
            
        }
        
        #endregion
        
        
        public InstallPage2()
        {
            InitializeComponent();
            
        }

     
    }
}