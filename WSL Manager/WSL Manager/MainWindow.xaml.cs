using System;
using System.Collections.Generic;
using System.Timers;
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
        
        private bool displayGuiTools = false;
        
        #region TIMER/UPDATE
        
        private System.Timers.Timer timer1;
        private void UpdateFormsTimer()
        {
            double seconds = .1 *1000;
            timer1 = new System.Timers.Timer(seconds);
            timer1.Elapsed += UpdateFormsFunction;
            timer1.AutoReset = true;
            timer1.Enabled = true;
        }

        /// <summary>
        /// Allows multithreading so that the timer can continuosly
        /// check to make sure that the buttons and list of distros
        /// are handled properly
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void UpdateFormsFunction(Object source, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                EnableDisableButtons();
                UpdateSelectedDistroField();
            });
        }
        
        #endregion
        
        #region BUTTONS
        
        //TODO: INSTALL RDP
        /// <summary>
        /// Button installs RDP in the selected distro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstallRdp(object sender, RoutedEventArgs e)
        {
            string distro = GetSelectedDistro();
            if (!distro.Contains("Select"))
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
        
        #endregion
        
        #region DROPDOWN
        
        /// <summary>
        /// DropDown Menu
        /// Dropdown Menu for Distros, runs on click
        /// Select distro wanted for starting stopping, etc. updates the list on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOfDistrosSelectOnClick(object sender, EventArgs e)
        {
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
        }
        
        #endregion

        #region HELPER FUNCTIONS

        /// <summary>
        /// This disabled the GUI buttons should the Distro selected not be version 2
        /// </summary>
        private void EnableDisableButtons()
        {
            
            //Can Select Login Method
            LogInAs.IsEnabled = canSelectLogin();
         
            //Can Type Username
            UserNameBox.IsEnabled = canTypeUsername();
            
            //Can Launch Selected Distro
            StartSelectedDistroButton.IsEnabled = canLaunchDistro();
            
            //Can Upgrade to WSL2
            UpgradeDistroButton.IsEnabled = canUpgradeToWsl2();
                
            //Can Install GUI TOOLS
            InstallGuiTools.IsEnabled = displayGuiTools;//canInstallGuiTools();
            
            //Can Open Distro Gui
            OpenDistroGui.IsEnabled = displayGuiTools;//canOpenDistroGUI();
            
            //Can Shut Down Selected Gui
            ShutDownSelectedDistro.IsEnabled = canShutDownSelectedDistro();
            
            //Shut down all distros
            ShutDownAllDistro.IsEnabled = isADistroRunning();
        }

        /// <summary>
        /// Get Login Choice
        /// Returns the selected method to login
        /// </summary>
        /// <returns></returns>
        private string GetSelectedLogInMethod()
        {
            return LogInAs.SelectedItem.ToString();
        }

        /// <summary>
        /// Get Distro
        /// Returns the selected distro from the drop down and
        /// returns null if the placeholder is what is selected
        /// </summary>
        /// <returns></returns>
        private string GetSelectedDistro()
        {
            return ListOfDistros.SelectedItem.ToString().Split(" ")[0];
        }

        /// <summary>
        /// Update List of installed Distros and distro statuses
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
        
        /// <summary>
        /// Updates the selected Distro for when
        /// its changing from running to not running or visa versa 
        /// </summary>
        private void UpdateSelectedDistroField()
        {
            string distro = GetSelectedDistro();
            bool oldProperIsRunning = ListOfDistros.SelectedItem.ToString().Contains("Running");
            
            bool properDistroSelected = !distro.ToString().Contains("Select");
            if (properDistroSelected)
            {
                RefreshDistroList();
                foreach (string[] distroProperties in _distroList)
                {
                    bool newProperRunning = distroProperties[1].Contains("Running");
                    if (distro == distroProperties[0] && oldProperIsRunning != newProperRunning)
                    {
                        ListOfDistros.Items.Add(distro + " " + distroProperties[1]);
                        ListOfDistros.SelectedItem = distro + " " + distroProperties[1];
                    }
                }
            }
        }
        /// <summary>
        /// THis runs at startup to ensure that distros launched prior to startup are closed
        /// </summary>
        private void AreDistrosRunningOnLaunch()
        {
            ListOfDistrosSelectOnClick(null,null);
            string message = "ERROR:\n" +
                             "\n" + 
                             "A Linux Distro was running prior to launch.\n" +
                             "\n" +
                             "OPTIONS: \n" +
                             "\n" +
                             "1) You can select \"Yes\" to close all active WSL distros and proceed.\n" +
                             "2) You can select \"No\" to close the program and and manually close out of all WSL distros yourself.\n" +
                             "\n" +
                             "Would You like to Force Close all distros and continue?";
            
            if (isADistroRunning())
            {
                if (MessageBox.Show(this, message, "ERROR", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ManageDistros.ShutDownAllDistros();
                }
                else
                {
                    System.Environment.Exit(0);  
                }
                
            }
        }
        
        #endregion
        
        #region WHEN TO DISPLAY BUTTONS
        
        /// <summary>
        /// If a distro is running return true
        /// This is used to control wether or not you should enable the shut all distros down button
        /// </summary>
        /// <returns></returns>
        private bool isADistroRunning()
        {
            foreach (string[] distroProperties in _distroList)
            {
                bool isRunning = distroProperties[1].Contains("Running");
                if (isRunning)
                {
                    return true;
                }
            }

            return false;

        }
        /// <summary>
        /// If a proper distro is selected return true
        /// </summary>
        /// <returns></returns>
        private bool canSelectLogin()
        {
            if (ListOfDistros.SelectedItem.ToString().Contains("Select"))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// If a proper login method and a proper distro are selected return true
        /// </summary>
        /// <returns></returns>
        private bool canLaunchDistro()
        {
            string distro = GetSelectedDistro();
            string logInStyle = GetSelectedLogInMethod();

            bool properDistroSelected = !distro.Contains("Select");
            bool properLoginSelected = !logInStyle.Contains("Select");
            bool properIsRunning = ListOfDistros.SelectedItem.ToString().Contains("Running");

            if (properDistroSelected && properLoginSelected && !properIsRunning)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        /// <summary>
        /// Can use textbox to type name if User is selcted
        /// </summary>
        /// <returns></returns>
        private bool canTypeUsername()
        {
            //If you want to login as a specific user
            if (LogInAs.SelectedItem.ToString().Contains("User") && canSelectLogin())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if the selected distro is WSL2
        /// if it is not version 2 it returns true
        /// </summary>
        /// <returns></returns>
        private bool canUpgradeToWsl2()
        {
            if (canSelectLogin() && !canShutDownSelectedDistro())
            {
                string distro = GetSelectedDistro();
                foreach (string[] distroProperties in _distroList)
                {
                    bool isWSL2 = distroProperties[2].Contains("2");
                    if (distro == distroProperties[0] && !isWSL2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// If distro is WSL2 return true
        /// </summary>
        /// <returns></returns>
        private bool canInstallGuiTools()
        {
            if (canSelectLogin() && canShutDownSelectedDistro())
            {
                string distro = GetSelectedDistro();
                foreach (string[] distroProperties in _distroList)
                {
                    bool isWSL2 = distroProperties[2].Contains("2");
                    if (distro == distroProperties[0] && isWSL2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// If distro is WSL2 return true
        /// </summary>
        /// <returns></returns>
        private bool canOpenDistroGUI()
        {
            if (canSelectLogin() && canShutDownSelectedDistro())
            {
                string distro = GetSelectedDistro();
                foreach (string[] distroProperties in _distroList)
                {
                    bool isWSL2 = distroProperties[2].Contains("2");
                    if (distro == distroProperties[0] && isWSL2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Returns true is selected distro displays running
        /// </summary>
        /// <returns></returns>
        private bool canShutDownSelectedDistro()
        {
            if (ListOfDistros.SelectedItem.ToString().Contains("Running"))
            {
                return true;
            }

            return false;
        }
        
        #endregion
        
        
        public MainWindow()
        {
            InitializeComponent();           
            UpdateFormsTimer();
            AreDistrosRunningOnLaunch();
        }


    }
}
