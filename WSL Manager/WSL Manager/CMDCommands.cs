using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WSL_Manager
{
        public static class CMDCommands
        {
                private static string shutDownAll = "/c wsl.exe --shutdown";
                private static string shutDownSpecificDistro = "/c wsl -t ";
                private static string startDistro = "/k wsl -d ";
                private static string startLinuxRDP = "/k wsl sudo /etc/init.d/xrdp";
                private static string listInstalledDistros = "/c wsl -l -v";
                private static string runSpecificUser = " -u ";
                private static string upgradeToWSL2 = "/k wsl --set-version ";
                
                #region GUI commands

                private static string[] guiInstall = new[]
                {
                        "sudo apt update && sudo apt -y upgrade",
                        "sudo apt-get purge xrdp",
                        "sudo apt install -y xrdp",
                        "sudo apt install -y xfce4",
                        "sudo apt install -y xfce4-goodies",
                        "sudo cp /etc/xrdp/xrdp.ini /etc/xrdp/xrdp.ini.bak",
                        "sudo sed -i ‘s/3389/3390/g’ /etc/xrdp/xrdp.ini",
                        "sudo sed -i ‘s/max_bpp=32/#max_bpp=32\\nmax_bpp=128/g’ /etc/xrdp/xrdp.ini",
                        "sudo sed -i ‘s/xserverbpp=24/#xserverbpp=24\\nxserverbpp=128/g’ /etc/xrdp/xrdp.ini",
                        "echo xfce4-session > ~/.xsession"
                };
                
                //TODO:
                /// <summary>
                ///sudo nano /etc/xrdp/startwm.sh
                ///edit these lines to:
                ///# test -x /etc/X11/Xsession && exec /etc/X11/Xsession
                ///# exec /bin/sh /etc/X11/Xsession
                ///add these lines:
                ///# xfce
                ///startxfce4
                /// </summary>

                #endregion

                public static string UpgradeToWSL2
                {
                        get { return upgradeToWSL2; }
                }
                
                public static string ShutDownAll
                {
                        get { return shutDownAll; }

                }

                public static string StartDistro
                {
                        get { return startDistro; }

                }

                public static string StartLinuxRDP
                {
                        get { return startLinuxRDP; }

                }

                public static string ListInstalledDistros
                {
                        get { return listInstalledDistros; }

                }

                public static string[] GuiInstall
                {
                        get { return guiInstall; }
                }

                public static string RunSpecificUser
                {
                        get { return runSpecificUser; }
                }

                public static string ShutDownSpecificDistro
                {
                        get { return shutDownSpecificDistro; }
                }
                
                

        }
}