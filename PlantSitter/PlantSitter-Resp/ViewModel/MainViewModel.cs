using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using JP.Utils.Functions;
using PlantSitter_Resp.Common;
using PlantSitter_Resp.View;
using PlantSitterCusomControl;
using PlantSitterShardModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(Email==TEST_EMAIL && Password==TEST_PWD)
            {
                ShowLoginControl = false;
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
