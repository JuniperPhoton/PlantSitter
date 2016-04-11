using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using PlantSitter.Common;
using PlantSitter.View;

namespace PlantSitter.ViewModel
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
                    NavigationService.NavigateViaRootFrame(typeof(RegisterLoginPage), 1);
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
                    NavigationService.NavigateViaRootFrame(typeof(RegisterLoginPage), 0);
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
