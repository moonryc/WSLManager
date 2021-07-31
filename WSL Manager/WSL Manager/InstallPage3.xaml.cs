using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WSL_Manager
{
    public partial class InstallPage3 : Page
    {
        private string distro = "";
        private bool isKali;
        private string progressText = "";


        public InstallPage3()
        {
            InitializeComponent();
        }


        /// <summary>
        ///     For letting the user knowing whats happening
        /// </summary>
        /// <param name="textToAdd"></param>
        private void UpdateInstallerProgressText(string textToAdd)
        {
            progressText += "----------------------------------------------------------------------------------\n";
            progressText += textToAdd + "\n";
            InstallerText.Text = progressText;
        }


        /// <summary>
        ///     CREATES FOLDER AND WORKS
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void GeneratingFolder()
        {
            try
            {
                var message = GuiInstallerCommands.GenerateWSLInstallerFolder();
                UpdateInstallerProgressText(message);
            }
            catch (Exception e)
            {
                UpdateInstallerProgressText("ERROR SEE BELOW");
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        ///     CREATES THE SCRIPT AND WORKS
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void BashScript()
        {
            try
            {
                var messgage = GuiInstallerCommands.GenerateBashScriptTextFile();
                UpdateInstallerProgressText(messgage);
            }
            catch (Exception e)
            {
                UpdateInstallerProgressText(
                    $"ERROR: \n ERROR CREATING THE SCRIPT TO INSTALL IN {GuiInstallerCommands.GetInstallLocation}");
                UpdateInstallerProgressText(e.Message);
                throw new Exception("INSTALLATION COULD NOT CONTINUE TO TO THE ERRORS ABOVE");
            }
        }

        private void RemoveOutdatedFromDistro()
        {
            try
            {
                var message = GuiInstallerCommands.RemoveOutDatedFolderAndFilesOnDistro(distro);
                if (!message.Equals("")) UpdateInstallerProgressText(message);
            }
            catch (Exception e)
            {
                UpdateInstallerProgressText("ERROR SEE BELOW");
                throw new Exception(e.Message);
            }
        }


        private void MoveScript()
        {
            try
            {
                var message = GuiInstallerCommands.MoveTxtToWsl(distro);
                UpdateInstallerProgressText(message);
            }
            catch (Exception e)
            {
                UpdateInstallerProgressText("ERROR SEE BELOW");
                throw new Exception(e.Message);
            }
        }

        private void ConvertScript()
        {
            try
            {
                var message = GuiInstallerCommands.ConvertToBatchShell(distro);
                UpdateInstallerProgressText(message);
            }
            catch (Exception e)
            {
                UpdateInstallerProgressText("FAILURE TO CONVERT SCRIPT INTO BATCH EXECUTABLE");
                UpdateInstallerProgressText(e.Message);
                throw new Exception("INSTALLATION COULD NOT CONTINUE TO TO THE ERRORS ABOVE");
            }
        }

        private void MakeScriptRunnable()
        {
            try
            {
                var message = GuiInstallerCommands.MakeRunnable(distro);
                UpdateInstallerProgressText(message);
            }
            catch (Exception e)
            {
                UpdateInstallerProgressText("ERROR SEE BELOW");
                throw new Exception(e.Message);
            }
        }

        private void RunTheScript()
        {
            try
            {
                GuiInstallerCommands.RunScript(distro);
                UpdateInstallerProgressText(
                    "INSTALLATION COMPLETE\n PRESS THE \"LAUNCH GUI\" BUTTON TO OPEN THE GUI VERSION OF LINUX");
            }
            catch (Exception e)
            {
                UpdateInstallerProgressText("ERROR SEE BELOW");
                throw new Exception(e.Message);
            }
        }


        private void StartInstaller()
        {
            Dispatcher.Invoke(() =>
            {
                var numberOfCommands = 6;
                double loadingBarIncrease = 100 % 6;
                var timeToWait = 1 * 1000;

                #region CONFIRMED

                //Generating Folder
                try
                {
                    GeneratingFolder();
                    LoadingBar.Value += loadingBarIncrease;
                }
                catch (Exception e)
                {
                    UpdateInstallerProgressText(e.Message);
                    UpdateInstallerProgressText("INSTALLATION HAS BEEN HALTED");
                }

                //BashScript
                try
                {
                    if (!InstallerText.Text.Contains("INSTALLATION HAS BEEN HALTED"))
                    {
                        Thread.Sleep(timeToWait);
                        BashScript();
                        LoadingBar.Value += loadingBarIncrease;
                    }
                }
                catch (Exception e)
                {
                    UpdateInstallerProgressText(e.Message);
                    UpdateInstallerProgressText("INSTALLATION HAS BEEN HALTED");
                }

                //RemoveOutdated From Distro
                try
                {
                    if (!InstallerText.Text.Contains("INSTALLATION HAS BEEN HALTED"))
                    {
                        Thread.Sleep(timeToWait);
                        RemoveOutdatedFromDistro();
                        LoadingBar.Value += loadingBarIncrease;
                    }
                }
                catch (Exception e)
                {
                    UpdateInstallerProgressText(e.Message);
                    UpdateInstallerProgressText("INSTALLATION HAS BEEN HALTED");
                }

                #endregion

                //MoveScript
                try
                {
                    if (!InstallerText.Text.Contains("INSTALLATION HAS BEEN HALTED"))
                    {
                        Thread.Sleep(timeToWait);
                        MoveScript();
                        LoadingBar.Value += loadingBarIncrease;
                    }
                }
                catch (Exception e)
                {
                    UpdateInstallerProgressText(e.Message);
                    UpdateInstallerProgressText("INSTALLATION HAS BEEN HALTED");
                }

                #region NOT CONFIRMED YET

                //ConvertScript
                // try
                // {
                //     if (!InstallerText.Text.Contains("INSTALLATION HAS BEEN HALTED"))
                //     {
                //         Thread.Sleep(timeToWait);
                //         ConvertScript();
                //         LoadingBar.Value += loadingBarIncrease;
                //     }
                // }
                // catch (Exception e)
                // {
                //     UpdateInstallerProgressText(e.Message);
                //     UpdateInstallerProgressText("INSTALLATION HAS BEEN HALTED");
                // }
                //
                // //MakeScriptRunnable
                // try
                // {
                //     if (!InstallerText.Text.Contains("INSTALLATION HAS BEEN HALTED"))
                //     {
                //         Thread.Sleep(timeToWait);
                //         MakeScriptRunnable();
                //         LoadingBar.Value += loadingBarIncrease;
                //     }
                // }
                // catch (Exception e)
                // {
                //     UpdateInstallerProgressText(e.Message);
                //     UpdateInstallerProgressText("INSTALLATION HAS BEEN HALTED");
                // }
                //
                // //RunTheScript
                // try
                // {
                //     if (!InstallerText.Text.Contains("INSTALLATION HAS BEEN HALTED"))
                //         Thread.Sleep(timeToWait);
                //     RunTheScript();
                //     LoadingBar.Value += loadingBarIncrease;
                // }
                // catch (Exception e)
                // {
                //     UpdateInstallerProgressText(e.Message);
                //     UpdateInstallerProgressText("INSTALLATION HAS BEEN HALTED");
                // }

                #endregion
            });
        }

        public void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var test = (object[]) e.ExtraData;

            isKali = (bool) test[0];
            distro = (string) test[1];
            NavigationService.LoadCompleted -= NavigationService_LoadCompleted;
            StartInstaller();
        }
    }
}