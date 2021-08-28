using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WSLManager.ViewModels
{
    public class BaseViewModel:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        //CallerMemberName = name of the variable and in this case it is directly substituted into propertyName
        //use OnPropertyChanged(); in each set for each prop to update it.
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}