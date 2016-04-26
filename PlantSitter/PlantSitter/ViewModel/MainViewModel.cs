﻿using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Data;
using JP.Utils.Framework;
using PlantSitter.View;
using PlantSitterCustomControl;
using PlantSitterShared.Model;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using PlantSitter.Common;
using JP.UWP.CustomControl;
using PlantSitter.UC;
using Windows.UI.ViewManagement;
using JP.Utils.Helper;

namespace PlantSitter.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        public PlantSitterUser CurrentUser { get; set; }

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand != null) return _logoutCommand;
                return _logoutCommand = new RelayCommand(async () =>
                  {
                      DialogService ds = new DialogService(DialogKind.PlainText, "注意", "确定要登出吗？");
                      ds.OnLeftBtnClick += (e) =>
                        {
                            ds.Hide();
                            LocalSettingHelper.CleanUpAll();
                            Common.NavigationService.NavigateViaRootFrame(typeof(StartPage), null);
                            Common.NavigationService.RootFrame.BackStack.Clear();
                        };
                      ds.OnRightBtnClick += () =>
                        {
                            ds.Hide();
                            IsDrawerOpen = false;
                        };
                      await ds.ShowAsync();
                  });
            }
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                if (_addCommand != null) return _addCommand;
                return _addCommand = new RelayCommand(async() =>
                  {
                      if(DeviceHelper.IsDesktop)
                      {
                          var control = new AddingPlanControl() { DataContext = new AddPlanViewModel(), Width = 500, Height = Window.Current.Bounds.Height * 0.9 };
                          ContentPopupEx cpex = new ContentPopupEx(control);
                          await cpex.ShowAsync();
                      }
                      else
                      {
                          NavigationService.RootFrame.Navigate(typeof(AddPlanPage), null);
                      }
                  });
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    RaisePropertyChanged(() => SelectedIndex);
                    if (value == 0)
                    {
                        Title = "首页";
                        NavigationService.ContentFrame.Navigate(typeof(AllPlansPage), null);
                        IsDrawerOpen = false;
                    }
                    else if (value == 1)
                    {
                        Title = "设置";
                        NavigationService.ContentFrame.Navigate(typeof(SettingPage), null);
                        IsDrawerOpen = false;
                    }
                    else if (value == 2)
                    {
                        Title = "关于";
                        NavigationService.ContentFrame.Navigate(typeof(AboutPage), null);
                        IsDrawerOpen = false;
                    }
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


        private bool _isDrawerOpen;
        public bool IsDrawerOpen
        {
            get
            {
                return _isDrawerOpen;
            }
            set
            {
                if (_isDrawerOpen != value)
                {
                    _isDrawerOpen = value;
                    RaisePropertyChanged(() => IsDrawerOpen);
                }
            }
        }

        public bool IsInView { get; set; }

        public bool IsFirstActived { get; set; } = true;

        public MainViewModel()
        {
            IsDrawerOpen = false;
            SelectedIndex = -1;
            CurrentUser = new PlantSitterUser()
            {
                Uid = int.Parse(LocalSettingHelper.GetValue("uid")),
                Email = LocalSettingHelper.GetValue("email"),
            };
            App.MainVM = this;
        }

        public void Activate(object param)
        {

        }

        public void Deactivate(object param)
        {

        }

        public void OnLoaded()
        {
            if (IsFirstActived)
            {
                IsFirstActived = false;
                SelectedIndex = 0;
            }
        }
    }
}
