using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Data;
using PlantSitter.View;
using PlantSitterCusomControl;
using PlantSitterShard.Model;

namespace PlantSitter.ViewModel
{
    public class MainViewModel:ViewModelBase
    {
        public UserPlansViewModel UserPlansVM { get; set; }

        public PlantSitterUser CurrentUser { get; set; }

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand != null) return _logoutCommand;
                return _logoutCommand = new RelayCommand(async() =>
                  {
                      DialogService ds = new DialogService(DialogKind.PlainText, "注意", "确定要登出吗？");
                      ds.OnLeftBtnClick += (e) =>
                        {
                            ds.Hide();
                            LocalSettingHelper.CleanUpAll();
                            Common.NavigationService.NavigateViaRootFrame(typeof(StartPage), null);
                            Common.NavigationService.RootFrame.BackStack.Clear();
                        };
                      ds.OnRightBtnClick += () =>
                        {
                            ds.Hide();
                            IsDrawerOpen = false;
                        };
                     await ds.ShowAsync();
                  });
            }
        }

        private bool _isDrawerOpen;
        public bool IsDrawerOpen
        {
            get
            {
                return _isDrawerOpen;
            }
            set
            {
                if (_isDrawerOpen != value)
                {
                    _isDrawerOpen = value;
                    RaisePropertyChanged(() => IsDrawerOpen);
                }
            }
        }

        public MainViewModel()
        {
            IsDrawerOpen = false;

            CurrentUser = new PlantSitterUser()
            {
                Uid = int.Parse(LocalSettingHelper.GetValue("uid")),
                Email = LocalSettingHelper.GetValue("email"),
            };
            UserPlansVM = new UserPlansViewModel(CurrentUser);
            App.MainVM = this;
        }
    }
}
