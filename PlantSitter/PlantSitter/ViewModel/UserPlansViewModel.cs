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

namespace PlantSitter.ViewModel
{
    public class UserPlansViewModel : ViewModelBase, INavigable
    {
        private ObservableCollection<UserPlanToDisplay> _currentUserPlans;
        public ObservableCollection<UserPlanToDisplay> CurrentUserPlans
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
                    await Refresh();
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

        public UserPlansViewModel(PlantSitterUser user)
        {
            this.CurrentUser = user;
            App.VMLocator.UsersPlansVM = this;
        }

        private async Task GetAllUserPlansAsync()
        {
            var plans = new ObservableCollection<UserPlanToDisplay>();

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
                var task = plan.UpdatePlantInfo();
                tasks.Add(task);
                plans.Add(new UserPlanToDisplay(plan));
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
                var task = plan.CurrentPlan.CurrentPlant.DownloadImage();
                var task2 = plan.FetchAndUpdateScore();
            }
        }

        private async Task Refresh()
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
            var planToDisplay = new UserPlanToDisplay(plan);
            this.CurrentUserPlans.Add(planToDisplay);
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
                await Refresh();
            }
        }
    }
}
