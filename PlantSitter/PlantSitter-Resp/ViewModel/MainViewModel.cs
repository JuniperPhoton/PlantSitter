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
using Sensor.Soil;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Sensors;
using Windows.UI.Xaml;
using Sensor.Light;
using System.Diagnostics;
using System.Linq;

namespace PlantSitter_Resp.ViewModel
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

        private ObservableCollection<UserPlan> _currentUserPlans;
        public ObservableCollection<UserPlan> CurrentUserPlans
        {
            get
            {
                return _currentUserPlans;
            }
            set
            {
                if (_currentUserPlans != value)
                {
                    _currentUserPlans = value;
                    RaisePropertyChanged(() => CurrentUserPlans);
                }
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    RaisePropertyChanged(() => SelectedIndex);
                    if (value > 0 && CurrentUserPlans.Count > value)
                    {
                        SelectedPlan = CurrentUserPlans.ElementAt(value);
                    }
                }
            }
        }

        private UserPlan _selectedPlan;
        public UserPlan SelectedPlan
        {
            get
            {
                return _selectedPlan;
            }
            set
            {
                if (_selectedPlan != value)
                {
                    _selectedPlan = value;
                    RaisePropertyChanged(() => SelectedPlan);
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

        public MainViewModel()
        {
            SelectedIndex = -1;
            NoItemVisibility = Visibility.Collapsed;
            _dateTimer = new DispatcherTimer();
            _dateTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _dateTimer.Tick += (sender, e) =>
              {
                  CurrentDate = "今天：" + DateTime.Now.ToString();
              };
            _dateTimer.Start();

            LoginVM = new LoginViewModel() { MainVM = this };
            CurrentPlants = new ObservableCollection<Plant>();
            if (ConfigHelper.IsLogin)
            {
                CurrentUser = new PlantSitterUser()
                {
                    Email = LocalSettingHelper.GetValue("email"),
                };
                var task1 = GetUserPlan();
                var task2 = RefreshAllSensor();
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

        public async Task GetUserPlan()
        {
            CurrentUserPlans = new ObservableCollection<UserPlan>();

            var getResult = await CloudService.GetAllPlans(CTSFactory.MakeCTS().Token);
            getResult.ParseAPIResult();
            if(!getResult.IsSuccessful)
            {
                ToastService.SendToast("获得培养计划失败");
                return;
            }
            var json = getResult.JsonSrc;


            SelectedIndex = 0;

            if (CurrentUserPlans.Count == 0)
            {
                NoItemVisibility = Visibility.Visible;
            }
            else NoItemVisibility = Visibility.Collapsed;
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
