using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using JP.Utils.Functions;
using PlantSitterCusomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlantSitter_Resp.ViewModel
{
    public enum LoginMode
    {
        Register,
        Login
    }

    public class LoginViewModel:ViewModelBase,INavigable
    {
        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

        private LoginMode LoginMode;

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

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged(() => Email);
                }
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged(() => Password);
                }
            }
        }

        private string _passwordToBeConfirmed;
        public string PasswordToBeConfirmed
        {
            get
            {
                return _passwordToBeConfirmed;
            }
            set
            {
                if (_passwordToBeConfirmed != value)
                {
                    _passwordToBeConfirmed = value;
                    RaisePropertyChanged(() => PasswordToBeConfirmed);
                }
            }
        }

        private RelayCommand _confrimActionCommand;
        public RelayCommand ConfirmActionCommand
        {
            get
            {
                if (_confrimActionCommand != null) return _confrimActionCommand;
                return _confrimActionCommand = new RelayCommand(async() =>
                  {
                      if (!IsInputDataValid()) return;
                      if(LoginMode==LoginMode.Register)
                      {
                          await Register();
                      }
                      if(LoginMode==LoginMode.Login)
                      {
                          await Login();
                      }
                  });
            }
        }

        public LoginViewModel()
        {

        }

        private bool IsInputDataValid()
        {
            if(string.IsNullOrEmpty(Email))
            {
                ToastService.SendToast("请输入邮箱");
                return false;
            }
            if(!Functions.IsValidEmail(Email))
            {
                ToastService.SendToast("邮箱格式不对");
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                ToastService.SendToast("请输入密码");
                return false;
            }
            if(LoginMode==LoginMode.Register && string.IsNullOrEmpty(PasswordToBeConfirmed))
            {
                ToastService.SendToast("请再次输入密码");
                return false;
            }
            if (LoginMode == LoginMode.Register && Password!=PasswordToBeConfirmed)
            {
                ToastService.SendToast("两次输入的密码不匹配");
                return false;
            }
            return true;
        }

        private async Task Register()
        {

        }

        private async Task Login()
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
                            LoginMode = LoginMode.Register;
                        };break;
                    //Login
                    case 1:
                        {
                            Title = "登录";
                            ConfirmPwdVisibility = Visibility.Collapsed;
                            ActionText = Title;
                            LoginMode = LoginMode.Login;
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
