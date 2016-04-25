using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using JP.Utils.Debug;
using JP.Utils.Functions;
using JP.Utils.Network;
using PlantSitterCusomControl;
using PlantSitterResp.Common;
using PlantSitterShared.Model;
using PlantSitterShared.API;
using System;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml;

namespace PlantSitterResp.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public MainViewModel MainVM
        {
            get
            {
                return App.MainVM;
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

        private RelayCommand _confrimActionCommand;
        public RelayCommand ConfirmActionCommand
        {
            get
            {
                if (_confrimActionCommand != null) return _confrimActionCommand;
                return _confrimActionCommand = new RelayCommand(async () =>
                {
                    if (!ShowLoginControl) return;
                    if (!IsInputDataValid()) return;
                    ShowLoading = Visibility.Visible;
                    await Login();
                });
            }
        }

        private bool _showLoginControl;
        public bool ShowLoginControl
        {
            get
            {
                return _showLoginControl;
            }
            set
            {
                if (_showLoginControl != value)
                {
                    _showLoginControl = value;
                    RaisePropertyChanged(() => ShowLoginControl);
                }
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

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand != null) return _logoutCommand;
                return _logoutCommand = new RelayCommand(() =>
                {
                    LocalSettingHelper.CleanUpAll();
                    ShowLoginControl = true;
                    MainVM.CurrentUser = null;
                    MainVM.OperationAfterLoginVisibility = Visibility.Collapsed;
                });
            }
        }

        public LoginViewModel()
        {
            ShowLoginControl = false;
            if (!ConfigHelper.IsLogin)
            {
                ShowLoginControl = true;
            }
            ShowLoading = Visibility.Collapsed;
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
                    throw new APIException();
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
                    ShowLoginControl = false;
                    MainVM.CurrentUser = new PlantSitterUser() { Email = Email };

                    await MainVM.Init();
                }
            }
            catch (TaskCanceledException)
            {
                ToastService.SendToast("Connection time out");
            }
            catch (APIException e)
            {
                ToastService.SendToast(e.ErrorMessage.IsNotNullOrEmpty() ? e.ErrorMessage : "Fail to login");
            }
            catch (Exception e)
            {
                ToastService.SendToast("Fail to login");
                var task = ExceptionHelper.WriteRecordAsync(e, nameof(MainViewModel), nameof(Login));
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
                var isUserExist = await CloudService.CheckUserExist(Email, CTSFactory.MakeCTS(10000).Token);
                isUserExist.ParseAPIResult();
                if (!isUserExist.IsSuccessful)
                {
                    throw new APIException(isUserExist.ErrorMsg);
                }
                var json = JsonObject.Parse(isUserExist.JsonSrc);
                var isExist = JsonParser.GetBooleanFromJsonObj(json, "isExist", false);
                if (isExist)
                {
                    throw new ArgumentException("邮件已经被注册.");
                }

                var registerResult = await CloudService.Register(Email, MD5.GetMd5String(Password), CTSFactory.MakeCTS(100000).Token);
                registerResult.ParseAPIResult();
                if (!registerResult.IsSuccessful)
                {
                    throw new APIException(ErrorTable.GetMessageFromErrorCode(registerResult.ErrorCode));
                }
                await Login();
            }
            catch (APIException e)
            {
                ToastService.SendToast(e.ErrorMessage.IsNotNullOrEmpty() ? e.ErrorMessage : "Fail to register");
            }
            catch (TaskCanceledException e)
            {

            }
            catch (Exception e)
            {

            }
            finally
            {
                ShowLoading = Visibility.Collapsed;
            }
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
            return true;
        }
    }
}
