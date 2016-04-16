using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitterShard.Model
{
    public class Plant:ViewModelBase
    {
        private int _pid;
        public int Pid
        {
            get
            {
                return _pid;
            }
            set
            {
                if (_pid != value)
                {
                    _pid = value;
                    RaisePropertyChanged(() => Pid);
                }
            }
        }

        private string _nameInChinese;
        public string NameInChinese
        {
            get
            {
                return _nameInChinese;
            }
            set
            {
                if (_nameInChinese != value)
                {
                    _nameInChinese = value;
                    RaisePropertyChanged(() => NameInChinese);
                }
            }
        }

        private string _nameInEnglish;
        public string NameInEnglish
        {
            get
            {
                return _nameInEnglish;
            }
            set
            {
                if (_nameInEnglish != value)
                {
                    _nameInEnglish = value;
                    RaisePropertyChanged(() => NameInEnglish);
                }
            }
        }

        private Vector2 _soilMoistureRange;
        public Vector2 SoilMoistureRange
        {
            get
            {
                return _soilMoistureRange;
            }
            set
            {
                if (_soilMoistureRange != value)
                {
                    _soilMoistureRange = value;
                    RaisePropertyChanged(() => SoilMoistureRange);
                }
            }
        }

        private Vector2 _enviTempRange;
        public Vector2 EnviTempRange
        {
            get
            {
                return _enviTempRange;
            }
            set
            {
                if (_enviTempRange != value)
                {
                    _enviTempRange = value;
                    RaisePropertyChanged(() => EnviTempRange);
                }
            }
        }

        private Vector2 _enviMoistureRange;
        public Vector2 EnviMoistureRange
        {
            get
            {
                return _enviMoistureRange;
            }
            set
            {
                if (_enviMoistureRange != value)
                {
                    _enviMoistureRange = value;
                    RaisePropertyChanged(() => EnviMoistureRange);
                }
            }
        }

        private Vector2 _lightRange;
        public Vector2 LightRange
        {
            get
            {
                return _lightRange;
            }
            set
            {
                if (_lightRange != value)
                {
                    _lightRange = value;
                    RaisePropertyChanged(() => LightRange);
                }
            }
        }

        public Plant()
        {

        }
    }
}
