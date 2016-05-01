using GalaSoft.MvvmLight;
using JP.Utils.Data.Json;
using PlantSitterCustomControl;
using PlantSitterShared.Model;
using PlantSitterShared.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml;
using JP.Utils.Framework;
using System;
using GalaSoft.MvvmLight.Command;
using PlantSitter.Common;
using PlantSitter.View;
using System.Linq;
using PlantSitterShared.Common;

namespace PlantSitter.ViewModel
{
    public class UserPlansViewModel : ViewModelBase, INavigable
    {
        private ObservableCollection<UserPlanWrapped> _currentUserPlans;
        public ObservableCollection<UserPlanWrapped> CurrentUserPlans
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

        private UserPlanWrapped _selectedPlan;
        public UserPlanWrapped SelectedPlan
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

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand != null) return _refreshCommand;
                return _refreshCommand = new RelayCommand(async () =>
                {
                    await RefreshAsync();
                });
            }
        }

        private RelayCommand<UserPlanWrapped> _selectPlanCommand;
        public RelayCommand<UserPlanWrapped> SelectPlanCommand
        {
            get
            {
                if (_selectPlanCommand != null) return _selectPlanCommand;
                return _selectPlanCommand = new RelayCommand<UserPlanWrapped>((plan) =>
                  {
                      NavigationService.NavigateViaRootFrame(typeof(PlanDetailPage), plan);
                  });
            }
        }



        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged(() => IsLoading);
                }
            }
        }

        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

        private int _mainGid = -1;

        public UserPlansViewModel(PlantSitterUser user)
        {
            this.CurrentUser = user;
            App.VMLocator.UsersPlansVM = this;
        }

        private async Task GetAllUserPlansAsync()
        {
            var plans = new ObservableCollection<UserPlanWrapped>();

            var getResult = await CloudService.GetAllPlans(CTSFactory.MakeCTS(5000).Token);
            getResult.ParseAPIResult();
            if (!getResult.IsSuccessful)
            {
                ToastService.SendToast("获得培养计划失败");
                return;
            }
            var json = getResult.JsonSrc;
            var obj = JsonObject.Parse(json);
            var plansArray = JsonParser.GetJsonArrayFromJsonObj(obj, "Plans");

            var tasks = new List<Task>();
            foreach (var item in plansArray)
            {
                var plan = UserPlan.ParseFromJson(item.ToString());
                plan.CurrentUser = this.CurrentUser;
                var task = plan.UpdatePlantInfoAsync();
                tasks.Add(task);
                plans.Add(new UserPlanWrapped(plan));
            }

            var mainGidResult = await CloudService.GetMainPlan(CTSFactory.MakeCTS().Token);
            if (mainGidResult.IsSuccessful)
            {
                var jsonObj = JsonObject.Parse(mainGidResult.JsonSrc);
                var gidObj = jsonObj["Gid"];
                var gid = JsonParser.GetStringFromJsonObj(gidObj, "main_plan_id");
                int.TryParse(gid, out _mainGid);
            }

            this.CurrentUserPlans = plans;

            if (CurrentUserPlans.Count == 0)
            {
                NoItemVisibility = Visibility.Visible;
            }
            else NoItemVisibility = Visibility.Collapsed;

            await Task.WhenAll(tasks.ToArray());

            foreach (var plan in CurrentUserPlans)
            {
                var task = DownloadImageAndUpdateScore(plan);
            }
        }

        private async Task DownloadImageAndUpdateScore(UserPlanWrapped plan)
        {
            await plan.FetchLatestRecordGetScoreAsync();
            await DownloadImageHelper.DownloadImage(plan.CurrentPlan.CurrentPlant);
            if (_mainGid != -1 && plan.CurrentPlan.Gid == _mainGid)
            {
                SelectedPlan = plan;
                plan.IsMain = true;

                if (App.AppSettings.EnableLiveTile)
                {
                    LiveTileUpdater.UpdateTile(this.SelectedPlan.CurrentPlan);
                }
            }
        }

        private async Task RefreshAsync()
        {
            try
            {
                IsLoading = true;
                await GetAllUserPlansAsync();
            }
            catch (Exception)
            {
                ToastService.SendToast("获取培养计划失败.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void AddNewPlan(UserPlan plan)
        {
            var planToDisplay = new UserPlanWrapped(plan);
            this.CurrentUserPlans.Add(planToDisplay);
        }

        public void DeletePlan(UserPlanWrapped plan)
        {
            this.CurrentUserPlans.Remove(plan);
        }

        public void SetMain(UserPlanWrapped plan)
        {
            foreach (var item in CurrentUserPlans)
            {
                item.IsMain = false;
            }
            plan.IsMain = true;
        }


        public void Activate(object param)
        {

        }

        public void Deactivate(object param)
        {

        }

        public async void OnLoaded()
        {
            if (IsFirstActived)
            {
                IsFirstActived = false;
                await RefreshAsync();
            }
        }
    }
}
