using GalaSoft.MvvmLight;
using JP.Utils.Data;
using JP.Utils.Framework;
using PlantSitterCusomControl;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using PlantSitterShared.Model;
using PlantSitterResp.Common;
using GalaSoft.MvvmLight.Command;
using PlantSitter_Resp.Service;
using PlantSitterResp.Service.SensorService;

namespace PlantSitterResp.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        private DisplayService _displayService;
        private UploadService _uploadService;
        private DateTimeService _dateTimeService;

        private EnviSensorService _enviSensorService;
        private LightSensorService _lightSensorService;
        private SoilSensorService _soilMoistureSensorService;

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
                return _refreshCommand = new RelayCommand(async () =>
                  {
                      LoadingVisibility = Visibility.Visible;
                      await UserPlanVM.GetAllUserPlansAsync();
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
                return _repairSensorCommand = new RelayCommand(async () =>
                  {
                      LoadingVisibility = Visibility.Visible;
                      await ResetSensorSettingsAsync();
                      LoadingVisibility = Visibility.Collapsed;
                  });
            }
        }

        private int _enviGPIOPIN;
        public int EnviGPIOPIN
        {
            get
            {
                return _enviGPIOPIN;
            }
            set
            {
                if (_enviGPIOPIN != value)
                {
                    _enviGPIOPIN = value;
                    RaisePropertyChanged(() => EnviGPIOPIN);
                }
            }
        }

        private int _soilGPIOPIN;
        public int SoilGPIOPIN
        {
            get
            {
                return _soilGPIOPIN;
            }
            set
            {
                if (_soilGPIOPIN != value)
                {
                    _soilGPIOPIN = value;
                    RaisePropertyChanged(() => SoilGPIOPIN);
                }
            }
        }

        public double[] TempTimelineData { get; set; } = new double[4];

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
                await UserPlanVM.GetAllUserPlansAsync();
                await RefreshAllSensorsAsync();
                LoadingVisibility = Visibility.Collapsed;

                EnviGPIOPIN = 0;
                SoilGPIOPIN = 0;
            }
        }

        private async Task RefreshAllSensorsAsync()
        {
            _lightSensorService = new LightSensorService();
            _soilMoistureSensorService = new SoilSensorService(26);
            _enviSensorService = new EnviSensorService(17);

            await _lightSensorService.Init();
            await _soilMoistureSensorService.Init();
            await _enviSensorService.Init();
        }

        private async Task ResetSensorSettingsAsync()
        {
            await _lightSensorService.Init();
            await _soilMoistureSensorService.InitWithNewPin((uint)SoilGPIOPIN);
            await _enviSensorService.InitWithNewPin((uint)EnviGPIOPIN);
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
