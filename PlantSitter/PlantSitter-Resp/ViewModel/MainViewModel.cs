using GalaSoft.MvvmLight;
using JP.Utils.Data;
using JP.Utils.Framework;
using PlantSitterCusomControl;
using Sensor.Soil;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Sensor.Light;
using PlantSitterShard.Model;
using PlantSitterResp.Common;
using GalaSoft.MvvmLight.Command;
using PlantSitter_Resp.Service;

namespace PlantSitterResp.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        private const int SOIL_SENSOR_GPIO_PIN = 18;
        private const int TEMP_MOISTURE_SENSOR_GPIO_PIN = 18;

        private DisplayService _displayService;
        private UploadService _uploadService;
        private DateTimeService _dateTimeService;

        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

        #region Login

        public LoginViewModel LoginVM { get; set; }

        #endregion

        public UserPlanViewModel UserPlanVM { get; set; }

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

        private Visibility _OperationAfterLoginVisibility;
        public Visibility OperationAfterLoginVisibility
        {
            get
            {
                return _OperationAfterLoginVisibility;
            }
            set
            {
                if (_OperationAfterLoginVisibility != value)
                {
                    _OperationAfterLoginVisibility = value;
                    RaisePropertyChanged(() => OperationAfterLoginVisibility);
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

        private RelayCommand _repairSensorCommand;
        public RelayCommand RepairSensorCommand
        {
            get
            {
                if (_repairSensorCommand != null) return _repairSensorCommand;
                return _repairSensorCommand = new RelayCommand(async() =>
                  {
                      LoadingVisibility = Visibility.Visible;
                      await RefreshAllSensor();
                      LoadingVisibility = Visibility.Collapsed;
                  });
            }
        }

        private double[] TempData { get; set; } = new double[4];

        public MainViewModel()
        {
            UserPlanVM = new UserPlanViewModel();

            LoadingVisibility = Visibility.Collapsed;
            OperationAfterLoginVisibility = Visibility.Collapsed;

            _dateTimeService = new DateTimeService();
            _displayService = new DisplayService();

            LoginVM = new LoginViewModel();
        }

        public async Task Init()
        {
            if (ConfigHelper.IsLogin)
            {
                OperationAfterLoginVisibility = Visibility.Visible;

                _uploadService = new UploadService();

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

        private async Task RefreshAllSensor()
        {            
            var task1 = InitialLightSensor();
            var task2 = InitialDhtSensor();
            var task3 = InitialSoilSensor();

            await task1;
            await task2;
            await task3;
        }

        private async Task InitialLightSensor()
        {
            try
            {
                var sensor = new GY30LightSensor();
                await sensor.InitLightSensorAsync();
                sensor.Reading += (sender, e) =>
                {
                    TempData[3] = e.Lux.Value;
                };
            }
            catch(Exception)
            {
                ToastService.SendToast("初始化光线传感器失败 :-(");
            }
        }

        private async Task InitialDhtSensor()
        {
            try
            {

            }
            catch(Exception)
            {
                ToastService.SendToast("初始化温湿度传感器失败 :-(");
            }
        }

        private async Task InitialSoilSensor()
        {
            try
            {
                SoilSensor soilSensor = new SoilSensor(SOIL_SENSOR_GPIO_PIN);
                await soilSensor.InitAsync();
                soilSensor.OnReadingValue += ((value) =>
                {
                    TempData[1] = value;
                });
            }
            catch(Exception)
            {
                ToastService.SendToast("初始化土壤湿度传感器失败 :-(");
            }
        }

        public async void Activate(object param)
        {
            await Init();
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
