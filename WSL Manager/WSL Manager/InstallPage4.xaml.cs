using System.Windows.Controls;
using System.Windows;

namespace WSL_Manager
{
    public partial class InstallPage4 : Page
    {
        private string _installCompleteMessage = "";
        
        private void OnPageLoad()
        {
            InfoTextBlock.Text = _installCompleteMessage;
        }

        
        /// <summary>
        /// Closes Installer Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            
        }
        
        /// <summary>
        /// Goes to Page 3 which is the configuration commands page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new InstallPage3());
        }
        
        public InstallPage4()
        {
            InitializeComponent();
            OnPageLoad();
        }
    }
}