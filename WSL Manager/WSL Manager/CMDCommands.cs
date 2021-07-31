using System.Windows.Controls;

namespace WSL_Manager
{
    public static class CMDCommands
    {
        private static readonly string upgradeToWSL2 = "/k wsl --set-version ";

        public static string ShutDownAll { get; } = "/c wsl.exe --shutdown";

        public static string StartDistro { get; } = "/k wsl -d";

        public static string StartLinuxRDP { get; } = "-e sudo /etc/init.d/xrdp start -d";

        public static string ListInstalledDistros { get; } = "/c wsl -l -v";

        public static string[] GuiInstallPartOne { get; } =
        {
            "sudo apt update",
            "sudo apt -y upgrade",
            "sudo apt-get purge xrdp",
            "sudo apt install -y xrdp",
            "sudo apt install -y xfce4",
            "sudo apt install -y xfce4-goodies",
        };
        
        public static string[] GuiInstallPartTwo { get; } =
        {
            "sudo cp /etc/xrdp/xrdp.ini /etc/xrdp/xrdp.ini.bak",
            "sudo sed -i \"s/3389/3390/g\" /etc/xrdp/xrdp.ini",
            "sudo sed -i \"s/max_bpp=32/#max_bpp=32\\nmax_bpp=128/g\" /etc/xrdp/xrdp.ini",
            "sudo sed -i \"s/xserverbpp=24/#xserverbpp=24\\nxserverbpp=128/g\" /etc/xrdp/xrdp.ini",
            "echo xfce4-session > ~/.xsession"
        };

        public static string GuiInstallPartThree { get; } =
            "sudo su - && sudo echo -e \"# xfce\" >> /etc/xrdp/startwm.sh && sudo echo -e \"startxfce\" >> /etc/xrdp/startwm.sh && exit";

        public static string RunSpecificUser { get; } = "-u";

        public static string ShutDownSpecificDistro { get; } = "/c wsl -t";

        #region GUI commands
        
        #endregion

        public static string UpgradeToWsl2
        {
            get { return upgradeToWSL2; }
        }
    }
}