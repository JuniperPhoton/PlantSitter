using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitterShardModel.Model
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
        }
    }
}
