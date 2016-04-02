using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlantSitter_Resp.ViewModel
{
    public class LoginViewModel:ViewModelBase,INavigable
    {
        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

        private Visibility _confirmPwdVisibility;
        public Visibility ConfirmPwdVisibility
        {
            get
            {
                return _confirmPwdVisibility;
            }
            set
            {
                if (_confirmPwdVisibility != value)
                {
                    _confirmPwdVisibility = value;
                    RaisePropertyChanged(() => ConfirmPwdVisibility);
                }
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        private string _actionText;
        public string ActionText
        {
            get
            {
                return _actionText;
            }
            set
            {
                if (_actionText != value)
                {
                    _actionText = value;
                    RaisePropertyChanged(() => ActionText);
                }
            }
        }

        private RelayCommand _confrimActionCommand;
        public RelayCommand ConfirmActionCommand
        {
            get
            {
                if (_confrimActionCommand != null) return _confrimActionCommand;
                return _confrimActionCommand = new RelayCommand(() =>
                  {
                      
                  });
            }
        }

        public LoginViewModel()
        {

        }

        public void Activate(object param)
        {
            if(param is int)
            {
                switch((int)param)
                {
                    //Register
                    case 0:
                        {
                            Title = "注册";
                            ConfirmPwdVisibility = Visibility.Visible;
                            ActionText = Title;
                        };break;
                    //Login
                    case 1:
                        {
                            Title = "登录";
                            ConfirmPwdVisibility = Visibility.Collapsed;
                            ActionText = Title;
                        };break;
                }
            }
        }

        public void Deactivate(object param)
        {

        }

        public void OnLoaded()
        {

        }
    }
}
