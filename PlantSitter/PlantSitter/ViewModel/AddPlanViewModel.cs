using GalaSoft.MvvmLight;
using JP.Utils.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter.ViewModel
{
    public class AddPlanViewModel : ViewModelBase,INavigable
    {
        public AddPlanViewModel()
        {

        }

        public bool IsFirstActived { get; set; }

        public bool IsInView { get; set; }

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
