using GalaSoft.MvvmLight;
using JP.Utils.Data.Json;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Media;

namespace PlantSitterShared.Model
{
    public class UserPlan:ViewModelBase
    {
        private int _gid;
        public int Gid
        {
            get
            {
                return _gid;
            }
            set
            {
                if (_gid != value)
                {
                    _gid = value;
                    RaisePropertyChanged(() => Gid);
                }
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
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

        private Plant _currentPlant;
        public Plant CurrentPlant
        {
            get
            {
                return _currentPlant;
            }
            set
            {
                if (_currentPlant != value)
                {
                    _currentPlant = value;
                    RaisePropertyChanged(() => CurrentPlant);
                }
            }
        }

        private ObservableCollection<PlantTimeline> _recordData;
        public ObservableCollection<PlantTimeline> RecordData
        {
            get
            {
                return _recordData;
            }
            set
            {
                if (_recordData != value)
                {
                    _recordData = value;
                    RaisePropertyChanged(() => RecordData);
                }
            }
        }

        private PlantTimeline[] _timelineDataToDisplay;
        public PlantTimeline[] TimelineDataToDisplay
        {
            get
            {
                return _timelineDataToDisplay;
            }
            set
            {
                if (_timelineDataToDisplay != value)
                {
                    _timelineDataToDisplay = value;
                    RaisePropertyChanged(() => TimelineDataToDisplay);
                }
            }
        }

        private DateTime _createTime;
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
            set
            {
                if (_createTime != value)
                {
                    _createTime = value;
                    RaisePropertyChanged(() => CreateTime);
                }
            }
        }

        public UserPlan()
        {
            RecordData = new ObservableCollection<PlantTimeline>();
            TimelineDataToDisplay = new PlantTimeline[5];
        }

        public void RaiseTimelineChanged()
        {
            RaisePropertyChanged(() => TimelineDataToDisplay);
        }

        public static UserPlan ParseFromJson(string json)
        {
            var plan = new UserPlan();
            var obj = JsonObject.Parse(json);
            var id = JsonParser.GetStringFromJsonObj(obj, "gid");
            var pid = JsonParser.GetStringFromJsonObj(obj, "pid");
            var name = JsonParser.GetStringFromJsonObj(obj, "name");
            var time = JsonParser.GetStringFromJsonObj(obj, "time");
            plan.Gid = int.Parse(id);
            plan.CurrentPlant = new Plant() { Pid =int.Parse(pid) };
            plan.Name = name;
            plan.CreateTime = DateTime.Parse(time);
            return plan;
        }

        public async Task UpdatePlantInfo()
        {
            var plant = await Plant.GetPlantByIdAsync(CurrentPlant.Pid);
            if(plant!= null)
            {
                this.CurrentPlant = plant;
            }
        }
    }
}
