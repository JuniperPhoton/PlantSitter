using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using JP.Utils.Debug;
using JP.Utils.Framework;
using JP.Utils.Functions;
using JP.Utils.Network;
using PlantSitter_Resp.Common;
using PlantSitter_Resp.View;
using PlantSitterCusomControl;
using PlantSitterShardModel.Model;
using PlantSitterShared.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace PlantSitter_Resp.ViewModel
{
    public class MainViewModel:ViewModelBase,INavigable
    {
        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

        private const string TEST_EMAIL = "TEST@TEST.com";
        private const string TEST_PWD = "TESTPWD";

        #region Login
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
                    if (!IsInputDataValid()) return;
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
        #endregion

        private ObservableCollection<Plant> _currentPlants;
        public ObservableCollection<Plant> CurrentPlants
        {
            get
            {
                return _currentPlants;
            }
            set
            {
                if (_currentPlants != value)
                {
                    _currentPlants = value;
                    RaisePropertyChanged(() => CurrentPlants);
                }
            }
        }

        public MainViewModel()
        {
            ShowLoginControl = false;
            if(!ConfigHelper.IsLogin)
            {
                ShowLoginControl = true;
            }
            CurrentPlants = new ObservableCollection<Plant>();
        }

        private async Task Login()
        {
            try
            {
                var saltResult = await CloudService.GetSalt(Email, CTSFactory.MakeCTS(100000).Token);
                saltResult.PhaseAPIResult();
                if (!saltResult.IsSuccessful)
                {
                    throw new ArgumentNullException("User does not exist.");
                }
                var saltObj = JsonObject.Parse(saltResult.JsonSrc);
                var salt = JsonParser.GetStringFromJsonObj(saltObj, "Salt");
                if(string.IsNullOrEmpty(salt))
                {
                    throw new ArgumentNullException("User does not exist.");
                }
                var newPwd = MD5.GetMd5String(Password);
                var newPwdInSalt = MD5.GetMd5String(newPwd + salt);
                var loginResult = await CloudService.Login(Email, newPwdInSalt, CTSFactory.MakeCTS(100000).Token);
                loginResult.PhaseAPIResult();
                if(!loginResult.IsSuccessful)
                {
                    throw new ArgumentNullException();
                }
                var loginObj = JsonObject.Parse(loginResult.JsonSrc);
                var userObj = loginObj["UserInfo"];
                var uid = JsonParser.GetStringFromJsonObj(userObj, "uid");
                var accessToken = JsonParser.GetStringFromJsonObj(userObj, "access_token");
                if(uid.IsNotNullOrEmpty() && accessToken.IsNotNullOrEmpty())
                {
                    LocalSettingHelper.AddValue("uid", uid);
                    LocalSettingHelper.AddValue("access_token", accessToken);
                    ShowLoginControl = false;
                }
            }
            catch (TaskCanceledException)
            {
                ToastService.SendToast("Connection time out");
            }
            catch(ArgumentNullException e)
            {
                ToastService.SendToast(e.ParamName.IsNotNullOrEmpty()?e.Message:"Fail to login");
            }
            catch(Exception e)
            {
                ToastService.SendToast("Fail to login");
                var task = ExceptionHelper.WriteRecordAsync(e, nameof(MainViewModel), nameof(Login));
            }
        }

        private async Task Register()
        {
            try
            {
                var isUserExist = await CloudService.CheckUserExist(Email, CTSFactory.MakeCTS(10000).Token);
                isUserExist.PhaseAPIResult();
                if (!isUserExist.IsSuccessful)
                {

                }
                var json = JsonObject.Parse(isUserExist.JsonSrc);
                var isExist = JsonParser.GetBooleanFromJsonObj(json, "isExist", false);
                if (!isExist)
                {

                }
            }
            catch(TaskCanceledException e)
            {

            }
            catch(Exception e)
            {

            }
        }

        private bool IsInputDataValid()
        {
            if (string.IsNullOrEmpty(Email))
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
            return true;
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
