using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using PlantSitterCustomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter.ViewModel
{
    public class SettingViewModel : ViewModelBase, INavigable
    {
        private RelayCommand _clearCacheCommand;
        public RelayCommand ClearCacheCommand
        {
            get
            {
                if (_clearCacheCommand != null) return _clearCacheCommand;
                return _clearCacheCommand = new RelayCommand(async () =>
                  {
                      await App.CacheUtilInstance.CleanUpAsync();
                      ToastService.SendToast("清理成功");
                  });
            }
        }

        public bool IsInView
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsFirstActived
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SettingViewModel()
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
