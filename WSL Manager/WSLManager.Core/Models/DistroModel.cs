using System.Collections.ObjectModel;
using MvvmCross.ViewModels;


namespace WSLManager.Core.Models
{
    public class DistroModel: MvxViewModel
    {
        private string _distroName;
        private string _distroStatus;
        private string _distroVersion;
        private ObservableCollection<DistroModel> _distro = new ObservableCollection<DistroModel>();

        public ObservableCollection<DistroModel> Distro
        {
            get => _distro;
            set => SetProperty(ref _distro, value);
        }
        
        public string DistroName { get=> _distroName; set=>SetProperty(ref _distroName,value); }
        public string DistroStatus { get=> _distroStatus; set=>SetProperty(ref _distroStatus,value); }
        public string DistroVersion { get=> _distroVersion; set=>SetProperty(ref _distroVersion,value); }

        
    }
    
}