using System;
using System.Windows;
using System.Windows.Controls;


namespace WSL_Manager
{
    /// <summary>
    /// Interaction logic for InstallPage1.xaml
    /// </summary>
    public partial class InstallPage1 : Page
    {
        
        /// <summary>
        /// Closes GUI installer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ClosePageOne(object sender, RoutedEventArgs e)
        {
            //TODO CLOSE PAGE ONE BUTTON
        }
        
        //TODO GUIDE ME WALKTHROUGH
        //TODO GUIDE ME PAGE ONE BUTTON
        /// <summary>
        /// Takes you to the Gui Installer walkthrough rather than automation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void GuideMePageOne(object sender, RoutedEventArgs e) {
            MainWindow returnToWSLManager = new MainWindow();
            returnToWSLManager.Show();
        }
        
        /// <summary>
        /// Takes you to Page 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinuePageOne(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new InstallPage2());
        }
        
        
        public InstallPage1()
        {
            InitializeComponent();
        }
        
    }
}
