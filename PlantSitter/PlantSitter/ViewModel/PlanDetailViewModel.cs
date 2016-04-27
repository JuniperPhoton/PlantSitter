using GalaSoft.MvvmLight;
using JP.Utils.Framework;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter.ViewModel
{
    public class PlanDetailViewModel:ViewModelBase,INavigable
    {
        public bool IsFirstActived { get; set; } = true;

        public bool IsInView { get; set; }

        private UserPlanWrapped _currentPlan;
        public UserPlanWrapped CurrentPlan
        {
            get
            {
                return _currentPlan;
            }
            set
            {
                if (_currentPlan != value)
                {
                    _currentPlan = value;
                    RaisePropertyChanged(() => CurrentPlan);
                }
            }
        }

        public PlanDetailViewModel()
        {

        }

        public void Activate(object param)
        {
            
        }

        public void Deactivate(object param)
        {
            
        }

        public void OnLoaded()
        {
            
        }
    }
}
