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
    public class StartViewModel:ViewModelBase,INavigable
    {
        private RelayCommand _goToLoginPageCommand;
        public RelayCommand GoToLoginPageCommand
        {
            get
            {
                if (_goToLoginPageCommand != null) return _goToLoginPageCommand;
                return _goToLoginPageCommand = new RelayCommand(() =>
                {
                    App.RootFrame.Navigate(typeof(RegisterLoginPage), 1);
                });
            }
        }

        private RelayCommand _goToRegisterPageCommanad;
        public RelayCommand GoToRegisterPageCommand
        {
            get
            {
                if (_goToRegisterPageCommanad != null) return _goToRegisterPageCommanad;
                return _goToRegisterPageCommanad = new RelayCommand(() =>
                {
                    App.RootFrame.Navigate(typeof(RegisterLoginPage), 0);
                });
            }
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
