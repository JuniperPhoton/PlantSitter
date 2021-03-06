﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.Utils.Framework;
using JP.UWP.CustomControl;
using PlantSitter.Common;
using PlantSitter.UC;
using PlantSitter.View;
using PlantSitterCustomControl;
using PlantSitterShared.API;
using PlantSitterShared.Common;
using PlantSitterShared.Enum;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PlantSitter.ViewModel
{
    public class PlanDetailViewModel : ViewModelBase, INavigable
    {
        public bool IsFirstActived { get; set; } = true;

        public bool IsInView { get; set; }

        private UserPlanWrapped _currentPlanWrapped;
        public UserPlanWrapped CurrentPlanWrapped
        {
            get
            {
                return _currentPlanWrapped;
            }
            set
            {
                if (_currentPlanWrapped != value)
                {
                    _currentPlanWrapped = value;
                    RaisePropertyChanged(() => CurrentPlanWrapped);
                }
            }
        }

        private bool _showTableView;
        public bool ShowTableView
        {
            get
            {
                return _showTableView;
            }
            set
            {
                if (_showTableView != value)
                {
                    _showTableView = value;
                    RaisePropertyChanged(() => ShowTableView);
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

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand != null) return _deleteCommand;
                return _deleteCommand = new RelayCommand(async () =>
                  {
                      DialogService ds = new DialogService(DialogKind.PlainText, "注意", "确实要删除吗？删除后无法恢复");
                      ds.OnLeftBtnClick += async (s) =>
                      {
                          NavigationService.RootFrame.GoBack();
                          App.VMLocator.UsersPlansVM.DeletePlan(this.CurrentPlanWrapped);
                          var result = await CloudService.DeletePlan(CurrentPlanWrapped.CurrentPlan.Gid, CTSFactory.MakeCTS().Token);
                      };
                      await ds.ShowAsync();
                  });
            }
        }

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand != null) return _refreshCommand;
                return _refreshCommand = new RelayCommand(async () =>
                  {
                      await Refresh();
                  });
            }
        }

        private RelayCommand _tapPlantCommand;
        public RelayCommand TapPlantCommand
        {
            get
            {
                if (_tapPlantCommand != null) return _tapPlantCommand;
                return _tapPlantCommand = new RelayCommand(async () =>
                  {
                      if (Window.Current.Bounds.Width <= 700)
                      {
                          NavigationService.RootFrame.Navigate(typeof(PlantDetailPage), CurrentPlanWrapped.CurrentPlan.CurrentPlant);
                      }
                      else
                      {
                          ContentPopupEx cpex = new ContentPopupEx(new PlantDetailControl() { DataContext = CurrentPlanWrapped.CurrentPlan.CurrentPlant, Width = 500, Height = Window.Current.Bounds.Height * 0.9 });
                          await cpex.ShowAsync();
                      }
                  });
            }
        }

        private Visibility _showLoadingTable;
        public Visibility ShowLoadingTable
        {
            get
            {
                return _showLoadingTable;
            }
            set
            {
                if (_showLoadingTable != value)
                {
                    _showLoadingTable = value;
                    RaisePropertyChanged(() => ShowLoadingTable);
                }
            }
        }

        private RelayCommand _hideTableViewCommand;
        public RelayCommand HideTableViewCommand
        {
            get
            {
                if (_hideTableViewCommand != null) return _hideTableViewCommand;
                return _hideTableViewCommand = new RelayCommand(() =>
                  {
                      ShowTableView = false;
                  });
            }
        }

        private RelayCommand _setMainCommand;
        public RelayCommand SetMainCommand
        {
            get
            {
                if (_setMainCommand != null) return _setMainCommand;
                return _setMainCommand = new RelayCommand(async () =>
                  {
                      DialogService ds = new DialogService(DialogKind.PlainText, "注意", "确实要设置为照看中吗？");
                      ds.OnLeftBtnClick += async (s) =>
                      {
                          this.CurrentPlanWrapped.IsMain = true;
                          App.VMLocator.UsersPlansVM.SetMain(this.CurrentPlanWrapped);
                          var result = await CloudService.SetMainPlan(CurrentPlanWrapped.CurrentPlan.Gid, CTSFactory.MakeCTS().Token);
                          ToastService.SendToast("设置成功 :-P");
                      };
                      await ds.ShowAsync();
                  });
            }
        }

        private RelayCommand<object> _tapItemCommand;
        public RelayCommand<object> TapItemCommand
        {
            get
            {
                if (_tapItemCommand != null) return _tapItemCommand;
                return _tapItemCommand = new RelayCommand<object>((o) =>
                  {
                      var kind = GetRecordDataKind(int.Parse(((Grid)o).Tag.ToString()));
                      UpdateTableName(kind);
                      CurrentPlanWrapped.UpdateTableToBeShown(kind);
                      ShowTableView = true;
                  });
            }
        }

        private string _tableName;
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                if (_tableName != value)
                {
                    _tableName = value;
                    RaisePropertyChanged(() => TableName);
                }
            }
        }

        private DispatcherTimer _timer;

        public PlanDetailViewModel()
        {
            ShowLoading = Visibility.Collapsed;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(15);
            _timer.Tick += _timer_Tick;
            _timer.Start();
            ShowLoadingTable = Visibility.Collapsed;
        }

        private RecordDataKind GetRecordDataKind(int tag)
        {
            switch(tag)
            {
                case 0:
                    {
                        return RecordDataKind.SoilMoisture;
                    };
                case 1:
                    {
                        return RecordDataKind.EnviTemp;
                    }
                case 2:
                    {
                        return RecordDataKind.Light;
                    }
                case 3:
                    {
                        return RecordDataKind.EnviMoisture;
                    }
                default:
                    {
                        return RecordDataKind.EnviMoisture;
                    }
            }
        }

        private void UpdateTableName(RecordDataKind kind)
        {
            switch (kind)
            {
                case RecordDataKind.SoilMoisture:
                    {
                        TableName = "土壤湿度";
                    }; break;
                case RecordDataKind.EnviTemp:
                    {
                        TableName = "温度";
                    }; break;
                case RecordDataKind.Light:
                    {
                        TableName = "光线强度";
                    }; break;
                case RecordDataKind.EnviMoisture:
                    {
                        TableName = "湿度";
                    }; break;
            }
        }

        private async void _timer_Tick(object sender, object e)
        {
            await Refresh();
        }

        private async Task Refresh()
        {
            ShowLoading = Visibility.Visible;
            await CurrentPlanWrapped.FetchLatestRecordGetScoreAsync();
            if (CurrentPlanWrapped.IsMain)
            {
                LiveTileUpdater.UpdateTile(CurrentPlanWrapped.CurrentPlan);
            }
            await CurrentPlanWrapped.FetchTableDataAndUpdateAsync(TableXAxisKind.Newest10);
            await UpdateTable();
            ShowLoading = Visibility.Collapsed;
        }

        private async Task UpdateTable()
        {
            ShowLoadingTable = Visibility.Visible;
            await CurrentPlanWrapped.FetchTableDataAndUpdateAsync(TableXAxisKind.Newest10);
            ShowLoadingTable = Visibility.Collapsed;
        }

        public async void Activate(object param)
        {
            this.CurrentPlanWrapped = param as UserPlanWrapped;
            await UpdateTable();
        }

        public void Deactivate(object param)
        {
            _timer?.Stop();
        }

        public void OnLoaded()
        {

        }
    }
}
