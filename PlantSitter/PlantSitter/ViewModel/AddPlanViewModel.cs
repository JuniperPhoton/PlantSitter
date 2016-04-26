using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.API;
using JP.Utils.Framework;
using JP.Utils.Functions;
using PlantSitterCustomControl;
using PlantSitterShared.API;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlantSitter.ViewModel
{
    public class AddPlanViewModel : ViewModelBase,INavigable
    {
        public bool IsFirstActived { get; set; } = true;

        public bool IsInView { get; set; }

        private bool _showAddGrid;
        public bool ShowAddGrid
        {
            get
            {
                return _showAddGrid;
            }
            set
            {
                if (_showAddGrid != value)
                {
                    _showAddGrid = value;
                    RaisePropertyChanged(() => ShowAddGrid);
                }
            }
        }

        private string _searchInfo;
        public string SearchInfo
        {
            get
            {
                return _searchInfo;
            }
            set
            {
                if (_searchInfo != value)
                {
                    _searchInfo = value;
                    RaisePropertyChanged(() => SearchInfo);
                }
            }
        }

        private ObservableCollection<Plant> _resultPlants;
        public ObservableCollection<Plant> ResultPlants
        {
            get
            {
                return _resultPlants;
            }
            set
            {
                if (_resultPlants != value)
                {
                    _resultPlants = value;
                    RaisePropertyChanged(() => ResultPlants);
                }
            }
        }

        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                if (_searchCommand != null) return _searchCommand;
                return _searchCommand = new RelayCommand(async() =>
                  {
                      try
                      {
                         await SearchPlantAsync();
                      }
                      catch (Exception)
                      {
                          ShowNoResult = Visibility.Visible;
                          ToastService.SendToast("搜索失败");
                      }
                  });
            }
        }

        private RelayCommand _showAddPlantGridCommand;
        public RelayCommand ShowAddPlantGridCommand
        {
            get
            {
                if (_showAddPlantGridCommand != null) return _showAddPlantGridCommand;
                return _showAddPlantGridCommand = new RelayCommand(() =>
                  {
                      ShowAddGrid = true;
                  });
            }
        }

        private RelayCommand _hideAddGridCommand;
        public RelayCommand HideAddGridCommand
        {
            get
            {
                if (_hideAddGridCommand != null) return _hideAddGridCommand;
                return _hideAddGridCommand = new RelayCommand(() =>
                  {
                      ShowAddGrid = false;
                  });
            }
        }

        private Visibility _showNoResult;
        public Visibility ShowNoResult
        {
            get
            {
                return _showNoResult;
            }
            set
            {
                if (_showNoResult != value)
                {
                    _showNoResult = value;
                    RaisePropertyChanged(() => ShowNoResult);
                }
            }
        }

        public AddPlanViewModel()
        {
            ShowNoResult = Visibility.Collapsed;
            ResultPlants = new ObservableCollection<Plant>();
        }

        private async Task SearchPlantAsync()
        {
            CommonRespMsg result;
            if(Functions.IsHasCHZN(SearchInfo))
            {
                result = await CloudService.SearchPlant(null, SearchInfo, "", CTSFactory.MakeCTS(10000).Token);
            }
            else
            {
                int id;
                var isID = int.TryParse(SearchInfo, out id);
                if(isID)
                {
                    result = await CloudService.SearchPlant(id, "", "", CTSFactory.MakeCTS(10000).Token);
                }
                else
                {
                    result = await CloudService.SearchPlant(null, "", SearchInfo, CTSFactory.MakeCTS(10000).Token);
                }
            }
            result.ParseAPIResult();
            if(!result.IsSuccessful)
            {
                ShowNoResult = Visibility.Visible;
                return;
            }
            ShowNoResult = Visibility.Collapsed;

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
