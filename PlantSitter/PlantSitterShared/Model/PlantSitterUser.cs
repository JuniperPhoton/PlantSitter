using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitterShard.Model
{
    public class PlantSitterUser:ViewModelBase
    {
        private int _uid;
        public int Uid
        {
            get
            {
                return _uid;
            }
            set
            {
                if (_uid != value)
                {
                    _uid = value;
                    RaisePropertyChanged(() => Uid);
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

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged(() => Email);
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

    }
}
