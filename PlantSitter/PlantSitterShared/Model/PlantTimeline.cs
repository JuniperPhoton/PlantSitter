using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitterShard.Model
{
    public class PlantTimeline:ViewModelBase
    {
        private int _tid;
        public int Tid
        {
            get
            {
                return _tid;
            }
            set
            {
                if (_tid != value)
                {
                    _tid = value;
                    RaisePropertyChanged(() => Tid);
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

        private double _soilMoisture;
        public double SoilMoisture
        {
            get
            {
                return _soilMoisture;
            }
            set
            {
                if (_soilMoisture != value)
                {
                    _soilMoisture = value;
                    RaisePropertyChanged(() => SoilMoisture);
                }
            }
        }

        private double _enviTemp;
        public double EnviTemp
        {
            get
            {
                return _enviTemp;
            }
            set
            {
                if (_enviTemp != value)
                {
                    _enviTemp = value;
                    RaisePropertyChanged(() => EnviTemp);
                }
            }
        }

        private double _enviMoisture;
        public double EnviMoisture
        {
            get
            {
                return _enviMoisture;
            }
            set
            {
                if (_enviMoisture != value)
                {
                    _enviMoisture = value;
                    RaisePropertyChanged(() => EnviMoisture);
                }
            }
        }

        private double _light;
        public double Light
        {
            get
            {
                return _light;
            }
            set
            {
                if (_light != value)
                {
                    _light = value;
                    RaisePropertyChanged(() => Light);
                }
            }
        }

        private DateTime _recoredTime;
        public DateTime RecordTime
        {
            get
            {
                return _recoredTime;
            }
            set
            {
                if (_recoredTime != value)
                {
                    _recoredTime = value;
                    RaisePropertyChanged(() => RecordTime);
                }
            }
        }

        public PlantTimeline()
        {

        }
    }
}
