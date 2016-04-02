using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using PlantSitter_Resp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter_Resp.ViewModel
{
    public class MainViewModel:ViewModelBase,INavigable
    {
        public MainViewModel()
        {

        }

        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

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
