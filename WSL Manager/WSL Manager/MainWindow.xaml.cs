using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management.Automation;
using System.Text.RegularExpressions;


namespace WSL_Manager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        #region DONE
        

        
        /// <summary>
        /// Shuts down all of WSL
        /// </summary>
        private void ShutdownAllDistros(object sender, RoutedEventArgs e)
        {
            ManageDistros.ShutDownAllDistros();
        }

        /// <summary>
        /// Returns the selected distro from the drop down and
        /// returns null if the placeholder is what is selected
        /// </summary>
        /// <returns></returns>
        private string GetSelectedDistro()
        {
            return ListOfDistros.SelectedItem.ToString();
        }

        private string LogInMethod()
        {
            return LogInAs.SelectedItem.ToString();
        }
        
        private void InstallRDP(object sender, RoutedEventArgs e)
        {
            string distro = GetSelectedDistro();
            if (distro != "Select Distro")
            {
                RemoteDesktop.InstallRDP(distro);
            }
        }

   
        
        /// <summary>
        /// This lets you now connect to the distro using RDP
        /// </summary>
        private void StartRDP(object sender, RoutedEventArgs e)
        {
            //Starts the selected Linux distro
            string distro = GetSelectedDistro();
            if (distro != "Select Distro")
            {
                RemoteDesktop.StartRDPConnection();
                RemoteDesktop.LaunchRDPProgram();
            }
        }
        
        /// <summary>
        /// Starts the selected Linux Distro in cmd the terminal
        /// MUST be visible unlike others commands
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSelectedDistro(object sender, RoutedEventArgs e)
        {
            string distro = GetSelectedDistro();
            string loginMethod = LogInMethod();
            if (distro != "Select Distro" &&  loginMethod != "Select LogIn")
            {
                if (loginMethod == "Default")
                {
                    ManageDistros.LaunchSelectedDistro(distro);    
                }else if (loginMethod == "User")
                {
                    ManageDistros.LaunchSelectedDistro(distro + CMDCommands.RunSpecificUser + UserNameBox.Text);
                }
                
            }
        }       
        
        private void ShutDownSpecificDistro(object sender, RoutedEventArgs e)
        {
            string distro = GetSelectedDistro();
            if (distro != "Select Distro")
            {
                ManageDistros.ShutDownSelectedDistro(distro);
            }
        }
        
        private void ListOfDistros_ContextMenuOpening(object sender, EventArgs e)
        {
            string selectedDistro = GetSelectedDistro();
            
            ListOfDistros.Items.Clear();
            
            //LoadDistroList
            List<string[]> installedDistros = ManageDistros.ListOfDistros();
            ListOfDistros.Items.Add("Select Distro");
            //adds items to the drop down menu
            foreach (string[] distro in installedDistros)
            {
                ListOfDistros.Items.Add(distro[0]);
            }
            
            
            //Set Selection in combo box to whatever the previous selection was
            ListOfDistros.SelectedItem = selectedDistro;
        }

        
        #endregion
        
        private void LogInAs_ContextMenuClose(object sender, EventArgs e)
        {
            if (LogInAs.SelectedItem == "User")
            {
                UserNameBox.IsEnabled = true;
                PassWordBox.IsEnabled = true;
            }
            else if (LogInAs.SelectedItem == "Default")
            {
                UserNameBox.IsEnabled = false;
                PassWordBox.IsEnabled = false;
            }
            
        }
        
        
        
        public MainWindow()
        {
            InitializeComponent();
            ListOfDistros.Items.Add("Select Distro");
            LogInAs.Items.Add("Select LogIn");
            LogInAs.Items.Add("Default");
            LogInAs.Items.Add("User");
        }


    }
}
