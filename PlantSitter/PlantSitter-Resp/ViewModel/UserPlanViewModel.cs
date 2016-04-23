using GalaSoft.MvvmLight;
using JP.Utils.Data.Json;
using PlantSitterCusomControl;
using PlantSitterShard.Model;
using PlantSitterShared.API;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml;

namespace PlantSitterResp.ViewModel
{
    public class UserPlanViewModel : ViewModelBase
    {
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
                    if (value >= 0 && CurrentUserPlans.Count > value)
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

        public UserPlanViewModel()
        {
            NoItemVisibility = Visibility.Collapsed;
            CurrentUserPlans = new ObservableCollection<UserPlan>();
            SelectedIndex = -1;
        }

        public async Task GetAllUserPlansAsync()
        {
            CurrentUserPlans = new ObservableCollection<UserPlan>();

            var getResult = await CloudService.GetAllPlans(CTSFactory.MakeCTS().Token);
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
                plan.CurrentUser = App.MainVM.CurrentUser;
                var task = plan.UpdatePlantInfo();
                tasks.Add(task);
                CurrentUserPlans.Add(plan);
            }

            SelectedIndex = 0;

            if (CurrentUserPlans.Count == 0)
            {
                NoItemVisibility = Visibility.Visible;
            }
            else NoItemVisibility = Visibility.Collapsed;

            await Task.WhenAll(tasks.ToArray());

            foreach (var plan in CurrentUserPlans)
            {
                var task = plan.CurrentPlant.DownloadImage();
            }
        }
    }
}
