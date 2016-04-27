using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using JP.Utils.Debug;
using JP.Utils.Framework;
using JP.Utils.Functions;
using JP.Utils.Network;
using PlantSitter.Common;
using PlantSitter.View;
using PlantSitterCustomControl;
using PlantSitterShared.API;
using System;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml;

namespace PlantSitter.ViewModel
{
    public enum LoginMode
    {
        Register,
        Login
    }

    public class LoginViewModel : ViewModelBase, INavigable
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
                return _confrimActionCommand = new RelayCommand(async () =>
                  {
                      if (!IsInputDataValid())
                      {
                          return;
                      }

                      ShowLoading = Visibility.Visible;

                      if (LoginMode == LoginMode.Register)
                      {
                          await Register();
                      }
                      if (LoginMode == LoginMode.Login)
                      {
                          await Login();
                      }
                  });
            }
        }

        private Visibility _showLoading;
        public Visibility ShowLoading
        {
            get
            {
                return _showLoading;
            }
            set
            {
                if (_showLoading != value)
                {
                    _showLoading = value;
                    RaisePropertyChanged(() => ShowLoading);
                }
            }
        }


        public LoginViewModel()
        {
            ShowLoading = Visibility.Collapsed;
        }

        private bool IsInputDataValid()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ToastService.SendToast("请输入邮箱");
                return false;
            }
            if (!Functions.IsValidEmail(Email))
            {
                ToastService.SendToast("邮箱格式不对");
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                ToastService.SendToast("请输入密码");
                return false;
            }
            if (LoginMode == LoginMode.Register && string.IsNullOrEmpty(PasswordToBeConfirmed))
            {
                ToastService.SendToast("请再次输入密码");
                return false;
            }
            if (LoginMode == LoginMode.Register && Password != PasswordToBeConfirmed)
            {
                ToastService.SendToast("两次输入的密码不匹配");
                return false;
            }
            return true;
        }

        private async Task Login()
        {
            try
            {
                var saltResult = await CloudService.GetSalt(Email, CTSFactory.MakeCTS(100000).Token);
                saltResult.ParseAPIResult();
                if (!saltResult.IsSuccessful)
                {
                    throw new APIException(ErrorTable.GetMessageFromErrorCode(saltResult.ErrorCode));
                }
                var saltObj = JsonObject.Parse(saltResult.JsonSrc);
                var salt = JsonParser.GetStringFromJsonObj(saltObj, "Salt");
                if (string.IsNullOrEmpty(salt))
                {
                    throw new APIException("用户不存在.");
                }

                var newPwd = MD5.GetMd5String(Password);
                var newPwdInSalt = MD5.GetMd5String(newPwd + salt);
                var loginResult = await CloudService.Login(Email, newPwdInSalt, CTSFactory.MakeCTS(100000).Token);
                loginResult.ParseAPIResult();
                if (!loginResult.IsSuccessful)
                {
                    throw new APIException(ErrorTable.GetMessageFromErrorCode(loginResult.ErrorCode));
                }
                var loginObj = JsonObject.Parse(loginResult.JsonSrc);
                var userObj = loginObj["UserInfo"];
                var uid = JsonParser.GetStringFromJsonObj(userObj, "uid");
                var accessToken = JsonParser.GetStringFromJsonObj(userObj, "access_token");
                if (uid.IsNotNullOrEmpty() && accessToken.IsNotNullOrEmpty())
                {
                    LocalSettingHelper.AddValue("uid", uid);
                    LocalSettingHelper.AddValue("access_token", accessToken);
                    LocalSettingHelper.AddValue("email", Email);
                    NavigationService.NavigateViaRootFrame(typeof(ShellPage));
                }
            }
            catch (TaskCanceledException)
            {
                ToastService.SendToast("Connection time out");
            }
            catch (APIException e)
            {
                ToastService.SendToast(e.ErrorMessage.IsNotNullOrEmpty() ? e.Message : "Fail to login");
            }
            catch (Exception e)
            {
                ToastService.SendToast("Fail to login");
                var task = ExceptionHelper.WriteRecordAsync(e, nameof(LoginViewModel), nameof(Login));
            }
            finally
            {
                ShowLoading = Visibility.Collapsed;
            }
        }

        private async Task Register()
        {
            try
            {
                var checkResult = await CloudService.CheckUserExist(Email, CTSFactory.MakeCTS(10000).Token);
                checkResult.ParseAPIResult();
                if (!checkResult.IsSuccessful)
                {
                    throw new APIException(ErrorTable.GetMessageFromErrorCode(checkResult.ErrorCode));
                }
                var json = JsonObject.Parse(checkResult.JsonSrc);
                var isExist = JsonParser.GetBooleanFromJsonObj(json, "isExist", false);
                if (isExist)
                {
                    throw new APIException(ErrorTable.GetMessageFromErrorCode(checkResult.ErrorCode));
                }

                var registerResult = await CloudService.Register(Email, MD5.GetMd5String(Password), CTSFactory.MakeCTS(100000).Token);
                registerResult.ParseAPIResult();
                if (!registerResult.IsSuccessful)
                {
                    throw new APIException(ErrorTable.GetMessageFromErrorCode(registerResult.ErrorCode));
                }
                await Login();
            }
            catch (ArgumentException e)
            {
                ToastService.SendToast(e.Message.IsNotNullOrEmpty() ? e.Message : "Fail to register");
            }
            catch (TaskCanceledException)
            {
                ToastService.SendToast("请求超时");
            }
            finally
            {
                ShowLoading = Visibility.Collapsed;
            }
        }

        public void Activate(object param)
        {
            if (param is int)
            {
                switch ((int)param)
                {
                    //Register
                    case 0:
                        {
                            Title = "注册";
                            ConfirmPwdVisibility = Visibility.Visible;
                            ActionText = Title;
                            LoginMode = LoginMode.Register;
                        }; break;
                    //Login
                    case 1:
                        {
                            Title = "登录";
                            ConfirmPwdVisibility = Visibility.Collapsed;
                            ActionText = Title;
                            LoginMode = LoginMode.Login;
                        }; break;
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
