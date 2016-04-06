using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
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

        public UserPlan()
        {

        }
    }
}
