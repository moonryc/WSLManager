using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using WSLManager.Logger.Core;
using WSLManager.Models;
using Timer = System.Timers.Timer;

namespace WSLManager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private DistroBankModel _distroBank = new DistroBankModel();
        private ObservableCollection<DistroModel> _distroCollection = new ObservableCollection<DistroModel>();
        private static Timer _updateTimer;
        private BaseViewModel _baseViewModel;
        private ObservableCollection<DistroModel> _oldDistroCollection;
        private DistroModel _selectedDistroModel;
        private int _selectedDistroIndex;
        private bool _canGoBack = true;
        object _mylock = new object();

        public bool CanGoBack
        {
            get => _canGoBack;
            set
            {
                _canGoBack = value;
                OnPropertyChanged();
            }
        }

        public DistroBankModel DistroBank
        {
            get => _distroBank;
        }

        /// <summary>
        /// Gets/Sets The selected viewmodel
        /// </summary>
        public BaseViewModel SelectedViewModel
        {
            get => _baseViewModel;
            set
            {
                _baseViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The observable collection that is binded to the list of distros that can be selected from
        /// </summary>
        public ObservableCollection<DistroModel> DistroCollection
        {
            get => _distroCollection;
            set
            {
                _distroCollection = value;
                SelectedDistroModel = _distroCollection[SelectedDistroIndex];
                SelectedDistroIndex = _selectedDistroIndex;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///  Gets/Sets the Selected distro model
        /// </summary>
        public DistroModel SelectedDistroModel
        {
            get { return _selectedDistroModel; }
            set
            {
                _selectedDistroModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets/Sets the selected distro index
        /// </summary>
        public int SelectedDistroIndex
        {
            get { return _selectedDistroIndex; }
            set
            {
                _selectedDistroIndex = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _baseViewModel = new DistroLaunchCloseViewModel(this);
            Application.Current.Dispatcher.Invoke(() =>
                BindingOperations.EnableCollectionSynchronization(DistroCollection, _mylock));
            UpdateObservableCollection();
            TimerBackgroundUpdate(2);

        }

        #region Update DistroBank and ObservableCollection

        /// <summary>
        /// Timer that Runs the BackgroundUpdate every X seconds
        /// </summary>
        /// <param name="seconds"></param>
        private void TimerBackgroundUpdate(int seconds)
        {
            _updateTimer = new Timer(1000 * seconds);
            _updateTimer.Elapsed += BackgroundUpdate;
            _updateTimer.AutoReset = true;
            _updateTimer.Enabled = true;
        }

        /// <summary>
        /// The method that runs the update for both the DistroBank and the Distro Observable collection
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void BackgroundUpdate(Object source, ElapsedEventArgs e)
        {
            Thread backgroundUpdateThread = new Thread(() =>
            {
                _distroBank.UpdateDictionary();
                UpdateObservableCollection();
            });
            backgroundUpdateThread.Name = $"Background update";
            backgroundUpdateThread.Start();
        }

        private bool test = true;

        /// <summary>
        /// Updates the Observable Collection
        /// </summary>
        private void UpdateObservableCollection()
        {
            CompareOldAndCurrentCollection();
            OldCollectionUpdate();
        }

        /// <summary>
        /// Updates the Observable collection by checking 2 different things
        /// (1) are the number of distros in the bankModel different from that of the Collection
        /// (2) Does the content of a brand new collection differ from that of an old copy of the collection
        /// if either of these are true it updates the DistroCollection and returns 0 (simply to safe memory/time)  
        /// </summary>
        /// <returns>Nothing</returns>
        private int CompareOldAndCurrentCollection()
        {
            lock (_mylock)
            {
                List<DistroModel> distroModelList = _distroBank.DistroDictionary.Values.ToList();
                ObservableCollection<DistroModel> tempCollection = new ObservableCollection<DistroModel>();

                foreach (var keyValuePair in _distroBank.DistroDictionary)
                {
                    tempCollection.Add(keyValuePair.Value);
                }

                //if a distro has been added or removed
                if (DistroCollection.Count != distroModelList.Count)
                {
                    DistroCollection = tempCollection;
                    IoC.Base.IoC.baseFactory.Log("Collection updated Due to Size", LogLevel.Debug);
                    return 0;
                }

                //if the content between old and new differs 
                for (int i = 0; i < distroModelList.Count; i++)
                {
                    bool test1 = DistroCollection[i].DistroName == _oldDistroCollection[i].DistroName;
                    bool test2 = DistroCollection[i].IsRunning == _oldDistroCollection[i].IsRunning;
                    bool test3 = DistroCollection[i].WslVersion == _oldDistroCollection[i].WslVersion;

                    if (!test1 || !test2 || !test3)
                    {
                        DistroCollection = tempCollection;
                        IoC.Base.IoC.baseFactory.Log("Collection updated due to Content", LogLevel.Debug);
                        return 0;
                    }
                }

                IoC.Base.IoC.baseFactory.Log("No Change", LogLevel.Informative);
                return 0;
            }
        }

        /// <summary>
        /// Creates a copy of the DistroCollection where each distro model is new and not tied as a ref to the original
        /// This is done to update 
        /// </summary>
        private void OldCollectionUpdate()
        {
            _oldDistroCollection = new ObservableCollection<DistroModel>();
            foreach (DistroModel distro in DistroCollection)
            {
                _oldDistroCollection.Add(new DistroModel(distro.DistroName, distro.IsRunning, distro.WslVersion));
            }
        }

        #endregion
    }
}