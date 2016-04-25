using GalaSoft.MvvmLight;
using JP.Utils.Data.Json;
using PlantSitterCusomControl;
using PlantSitterShard.Model;
using PlantSitterShared.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml;

namespace PlantSitter.ViewModel
{
    public class UserPlansViewModel:ViewModelBase
    {
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


        public UserPlansViewModel(PlantSitterUser user)
        {
            this.CurrentUser = user;
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
                plan.CurrentUser = this.CurrentUser;
                var task = plan.UpdatePlantInfo();
                tasks.Add(task);
                CurrentUserPlans.Add(plan);
            }

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
