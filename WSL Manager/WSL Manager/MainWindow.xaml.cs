using System;
using System.Collections.Generic;
using System.Windows;



namespace WSL_Manager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string[]> _distroList = new List<string[]>();
        private Dictionary<string, Distro> distros = new Dictionary<string, Distro>();

        
        
        //TODO: INSTALL RDP
        /// <summary>
        /// Button installs RDP in the selected distro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallRdp(object sender, RoutedEventArgs e)
        {
            string distro = GetSelectedDistro();
            if (distro != "Select Distro")
            {
                RemoteDesktop.InstallRdp(distro);
            }
        }

        
        
        //TODO: THIS WILL BE THE LAST THING THAT WILL BE WORKED ON
        /// <summary>
        /// Button
        /// This lets you now connect to the distro using RDP
        /// </summary>
        private void StartRdp(object sender, RoutedEventArgs e)
        {
            //Starts the selected Linux distro
            string distro = GetSelectedDistro();
            if (distro != "Select Distro")
            {
                RemoteDesktop.StartRdpConnection();
                RemoteDesktop.LaunchRdpProgram();
            }
        }

        
        
        /// <summary>
        /// Button
        /// Converts Distro to WSL2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpgradeToWsl2(object sender, RoutedEventArgs e)
        {
            string distro = GetSelectedDistro();
            distros[distro].EndDistro();
            ManageDistros.ConvertWslToWslTwo(distro);
        }
        
        /// <summary>
        /// Button
        /// Launches the selected distro only if the distro is not already running
        /// Can Launch a specific distro with a specific user provided that the user exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSelectedDistro(object sender, RoutedEventArgs e)
        {
            string distro = GetSelectedDistro();
            string loginMethod = LogInMethod();
            distros[distro].StartDistro(UserNameBox.Text);
            
        }

        /// <summary>
        /// Button
        /// Shuts down all of WSL
        /// </summary>
        private void ShutdownAllDistros(object sender, RoutedEventArgs e)
        {
            ManageDistros.ShutDownAllDistros();
        }     
        
        /// <summary>
        /// Button
        /// Shuts down the distro listed in the drop down menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShutDownSpecificDistro(object sender, RoutedEventArgs e)
        {
            string distro = GetSelectedDistro();
            distros[distro].EndDistro();
        }
        
        
        /// <summary>
        /// DropDown Menu
        /// Dropdown Menu for Distros, runs on click
        /// Select distro wanted for starting stopping, etc. updates the list on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOfDistrosDropDown(object sender, EventArgs e)
        {
            string selectedDistro = GetSelectedDistro();
            
            ListOfDistros.Items.Clear();

            RefreshDistroList();

            ListOfDistros.Items.Add("Select Distro");
            //adds new items to the drop down menu
            foreach (string[] distro in _distroList)
            {
                //spelled it out for legibility
                string distroNameTemp = distro[0];
                bool distroIsRunningTemp = distro[1] == "Running";
                int distroVersionTemp = int.Parse(distro[2]);
                
                //if distro is a repeat update it in case it has been altered
                //this updates all distro objects
                if (distros.ContainsKey(distroNameTemp))
                {
                    distros[distroNameTemp].IsRunning = distroIsRunningTemp;
                    distros[distroNameTemp].WSLVersion = distroVersionTemp;
                }
                else
                {
                    distros.Add(distroNameTemp, new Distro(distroNameTemp,distroIsRunningTemp,distroVersionTemp));
                }
                
                //add the item to the list
                ListOfDistros.Items.Add(distroNameTemp + " " + distro[1]);

            }
            
            //Set Selection in combo box to whatever the previous selection was
            ListOfDistros.Items.MoveCurrentToFirst();
            //ListOfDistros.SelectedItem = selectedDistro;
            if(!ListOfDistros.Text.Contains("Select")){
                WhenToDisplayButtons(null,null);
            }
        }

        /// <summary>
        /// Dropdown menu
        /// Runs when the user dropdown is clicked, enables and disabbles textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogInAs_ContextMenuClose(object sender, EventArgs e)
        {
            WhenToDisplayButtons(null,null);
        }
        
        #region Helper Functions

        /// <summary>
        /// This disabled the GUI buttons should the Distro selected not be version 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhenToDisplayButtons(object sender, EventArgs e)
        {
            string distro = GetSelectedDistro();
            string logInStyle = LogInMethod();

            bool properDistroSelected = !distro.Contains("Select");
            bool properLoginSelected = !logInStyle.Contains("Select");
            bool properIsRunning = ListOfDistros.SelectedItem.ToString().Contains("Running");

            if (properDistroSelected)
            {
                LogInAs.IsEnabled = true;
                //If you can upgrade your WSL
                int wslVersion = GetSelectedDistroVersionNumber();

                if (2 == wslVersion)
                {
                    UpgradeDistroButton.IsEnabled = false;
                    InstallGuiTools.IsEnabled = true;
                    OpenDistroGui.IsEnabled = true;
                }
                else
                {
                    UpgradeDistroButton.IsEnabled = true;
                    InstallGuiTools.IsEnabled = false;
                    OpenDistroGui.IsEnabled = false;    
                }
            }
            else
            {
                LogInAs.IsEnabled = false;
                UpgradeDistroButton.IsEnabled = false;
                InstallGuiTools.IsEnabled = false;
                OpenDistroGui.IsEnabled = false;
                ShutDownSelectedDistro.IsEnabled = false;
            }

                //Launch Distro
                if (properDistroSelected && properLoginSelected && !properIsRunning)
                {
                    //if it does not display Select && the distro is Stopped
                    
                    StartSelectedDistroButton.IsEnabled = true;
                    ShutDownAllDistro.IsEnabled = true;
                }
                else
                {
                    StartSelectedDistroButton.IsEnabled = false;
                }

                if (properIsRunning)
                {
                    StartSelectedDistroButton.IsEnabled = false;
                    ShutDownSelectedDistro.IsEnabled = true;
                }
                else
                {
                    ShutDownSelectedDistro.IsEnabled = false;
                }

                //If you want to login as a specific user
                if ((string) LogInAs.SelectedItem == "User")
                {
                    UserNameBox.IsEnabled = true;
                }
                else
                {
                    UserNameBox.IsEnabled = false;
                }
            
        }

        /// <summary>
        /// Returns the selected method to login
        /// </summary>
        /// <returns></returns>
        private string LogInMethod()
        {
            return LogInAs.SelectedItem.ToString();
        }
        
        /// <summary>
        /// Returns the selected distro from the drop down and
        /// returns null if the placeholder is what is selected
        /// </summary>
        /// <returns></returns>
        private string GetSelectedDistro()
        {
            return ListOfDistros.SelectedItem.ToString().Split(" ")[0];
        }

        /// <summary>
        /// returns the selected distros version number
        /// </summary>
        /// <returns></returns>
        private int GetSelectedDistroVersionNumber()
        {
            string distro = GetSelectedDistro();
            if (!distro.Contains("Select"))
            {
                return distros[distro].WSLVersion;
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// Refreshes the distro list for new distros and changes to exiting distros
        /// </summary>
        private void RefreshDistroList()
        {
            _distroList = new List<string[]>();
            
            #region CMD create in command line
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
            #endregion
            
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            
            //Remove unneded characters from output
            output = output.Replace("*", "").Replace("\0", "").Replace("\r", "");
            while (output.Contains("  "))
            {
                output = output.Replace("  ", " ");
            }
            
            
            //Add distro info to distroList
            string[] outputArray = output.Split("\n");
            
            for (int line = 1; line < outputArray.Length - 1; line++)
            {
                string[] modifiedLine = outputArray[line].Split(" ");
                
                _distroList.Add( new[] {modifiedLine[1], modifiedLine[2], modifiedLine[3]} );
            }
        }
        
        #endregion
        
        
        public MainWindow()
        {
            InitializeComponent();
            ListOfDistros.Items.Add("Select Distro");
            LogInAs.Items.Add("Select LogIn");
            LogInAs.Items.Add("Default");
            LogInAs.Items.Add("User");
            StartSelectedDistroButton.IsEnabled = false;
            UpgradeDistroButton.IsEnabled = false;
            StartSelectedDistroButton.IsEnabled = false;
            ShutDownAllDistro.IsEnabled = false;
            ShutDownSelectedDistro.IsEnabled = false;
            LogInAs.IsEnabled = false;
            
            InstallGuiTools.IsEnabled = false;
            OpenDistroGui.IsEnabled = false;
            //LogInAs_ContextMenuClose(null,null);
        }


    }
}
