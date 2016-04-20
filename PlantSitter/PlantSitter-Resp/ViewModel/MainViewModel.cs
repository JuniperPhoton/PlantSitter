using GalaSoft.MvvmLight;
using JP.Utils.Data;
using JP.Utils.Framework;
using PlantSitterCusomControl;
using PlantSitterShared.API;
using Sensor.Soil;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Sensor.Light;
using System.Diagnostics;
using System.Linq;
using PlantSitterShard.Model;
using PlantSitterResp.Common;
using JP.Utils.Data.Json;
using Windows.Data.Json;
using GalaSoft.MvvmLight.Command;

namespace PlantSitterResp.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        private const int SOIL_SENSOR_GPIO_PIN = 17;
        private const int TEMP_MOISTURE_SENSOR_GPIO_PIN = 18;

        private DispatcherTimer _dateTimer;
        private DispatcherTimer _uploadTimer;

        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

        #region Login

        public LoginViewModel LoginVM { get; set; }

        #endregion

        public UserPlanViewModel UserPlanVM { get; set; }

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

        private string _currentDate;
        public string CurrentDate
        {
            get
            {
                return _currentDate;
            }
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    RaisePropertyChanged(() => CurrentDate);
                }
            }
        }

        private Visibility _loadingVisibility;
        public Visibility LoadingVisibility
        {
            get
            {
                return _loadingVisibility;
            }
            set
            {
                if (_loadingVisibility != value)
                {
                    _loadingVisibility = value;
                    RaisePropertyChanged(() => LoadingVisibility);
                }
            }
        }

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand != null) return _refreshCommand;
                return _refreshCommand = new RelayCommand(async() =>
                  {
                      LoadingVisibility = Visibility.Visible;
                      await UserPlanVM.GetAllUserPlans();
                      LoadingVisibility = Visibility.Collapsed;
                  });
            }
        }

        public MainViewModel()
        {
            UserPlanVM = new UserPlanViewModel();

            LoadingVisibility = Visibility.Collapsed;

            _dateTimer = new DispatcherTimer();
            _dateTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _dateTimer.Tick += (sender, e) =>
              {
                  CurrentDate = "今天：" + DateTime.Now.ToString();
              };
            _dateTimer.Start();

            LoginVM = new LoginViewModel() { MainVM = this };

            var task = Init();
        }

        private async Task Init()
        {
            if (ConfigHelper.IsLogin)
            {
                CurrentUser = new PlantSitterUser()
                {
                    Email = LocalSettingHelper.GetValue("email"),
                };

                LoadingVisibility = Visibility.Visible;
                await UserPlanVM.GetAllUserPlans();
                await RefreshAllSensor();
                LoadingVisibility = Visibility.Collapsed;
            }
        }

        public async Task RefreshAllSensor()
        {
            _uploadTimer = new DispatcherTimer();
            
            var task1 = InitialLightSensor();
            var task2 = InitialDhtSensor();
            var task3 = InitialSoilSensor();

            await task1;
            await task2;
            await task3;
        }

        public async Task InitialLightSensor()
        {
            var sensor = new GY30LightSensor();
            await sensor.InitLightSensorAsync();
            sensor.Reading += (sender, e) =>
            {
                Debug.WriteLine($"Light={e.Lux}");
            };
        }

        public async Task InitialDhtSensor()
        {

        }

        public async Task InitialSoilSensor()
        {
            SoilSensor soilSensor = new SoilSensor();
            await soilSensor.InitGpioPin(SOIL_SENSOR_GPIO_PIN);
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
