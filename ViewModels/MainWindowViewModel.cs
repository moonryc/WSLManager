using System.Collections.ObjectModel;
using WSLManager.Models;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Timers;

namespace WSLManager.ViewModels
{
    
    
    public class MainWindowViewModel: BaseViewModel
    {
        
        
        private DistroBankModel _distroBank = new DistroBankModel();
        private ObservableCollection<DistroModel> _distroCollection = new ObservableCollection<DistroModel>();
        private static System.Timers.Timer updateTimer;
        private BaseViewModel _baseViewModel;
        
        
        
        public BaseViewModel SelectedViewModel
        {
            
            get => _baseViewModel;
            set
            {
                _baseViewModel = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<DistroModel> DistroCollection
        {
            get => _distroCollection;
            set
            {
                _distroCollection = value;
                OnPropertyChanged();
            }
        }
        
        
        public MainWindowViewModel()
        {
            _baseViewModel = new DistroLaunchCloseViewModel(this);
            
            
            TimerBackgroundUpdate(2);
        }

        #region Update DistroBank and ObservableCollection

        /// <summary>
        /// Timer that Runs the BackgroundUpdate every X seconds
        /// </summary>
        /// <param name="seconds"></param>
        private void TimerBackgroundUpdate(int seconds)
        {
            updateTimer = new System.Timers.Timer(1000* seconds);
            updateTimer.Elapsed += BackgroundUpdate;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;
        }
        
        /// <summary>
        /// The method that runs the update for both the DistroBank and the Distro Observable collection
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void BackgroundUpdate(Object source, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(()=>
            {
                BankUpdate();
                UpdateObservableCollection();
            });
        }
        
        /// <summary>
        /// Updates the DistroBank Disctionary
        /// </summary>
        private void BankUpdate()
        {
            _distroBank.UpdateDictionary();    
        }
        
        /// <summary>
        /// Updates the Observable Collection
        /// </summary>
        private void UpdateObservableCollection()
        {
            ObservableCollection<DistroModel> tempCollection = new ObservableCollection<DistroModel>();
            
            tempCollection.Add(new DistroModel("Select Distro",false,0));
            foreach (KeyValuePair<string,DistroModel> keyValuePair in _distroBank.DistroDictionary)
            {
                tempCollection.Add(keyValuePair.Value);
            }

            DistroCollection = tempCollection;
        }
        
        #endregion
        
    }
    
    
}
