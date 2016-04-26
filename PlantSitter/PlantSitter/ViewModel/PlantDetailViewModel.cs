using GalaSoft.MvvmLight;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter.ViewModel
{
    public class PlantDetailViewModel : ViewModelBase
    {
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

        public PlantDetailViewModel()
        {

        }
    }
}
