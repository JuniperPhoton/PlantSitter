using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using JP.Utils.Debug;
using JP.Utils.Framework;
using JP.Utils.Functions;
using JP.Utils.Network;
using PlantSitter_Resp.Common;
using PlantSitterCusomControl;
using PlantSitterShardModel.Model;
using PlantSitterShared.API;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml;

namespace PlantSitter_Resp.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

        private const string TEST_EMAIL = "TEST@TEST.com";
        private const string TEST_PWD = "TESTPWD";

        #region Login

        public LoginViewModel LoginVM { get; set; }

        #endregion

        private ObservableCollection<Plant> _currentPlants;
        public ObservableCollection<Plant> CurrentPlants
        {
            get
            {
                return _currentPlants;
            }
            set
            {
                if (_currentPlants != value)
                {
                    _currentPlants = value;
                    RaisePropertyChanged(() => CurrentPlants);
                }
            }
        }

        private PlantSitterUser _currentUser;
        public PlantSitterUser CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    RaisePropertyChanged(() => CurrentUser);
                }
            }
        }

        private Visibility _noItemVisibility;
        public Visibility NoItemVisibility
        {
            get
            {
                return _noItemVisibility;
            }
            set
            {
                if (_noItemVisibility != value)
                {
                    _noItemVisibility = value;
                    RaisePropertyChanged(() => NoItemVisibility);
                }
            }
        }

        public MainViewModel()
        {
            NoItemVisibility = Visibility.Collapsed;
            LoginVM = new LoginViewModel() { MainVM = this };
            CurrentPlants = new ObservableCollection<Plant>();
            if (ConfigHelper.IsLogin)
            {
                CurrentUser = new PlantSitterUser()
                {
                    Email = LocalSettingHelper.GetValue("email"),
                };
            }
        }

        public void Activate(object param)
        {

        }

        public void Deactivate(object param)
        {

        }

        public void OnLoaded()
        {
            if (ConfigHelper.IsLogin)
            {
                ToastService.SendToast("欢迎回来:-)", TimeSpan.FromMilliseconds(2000));
            }
        }
    }
}
