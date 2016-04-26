using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JP.API;
using JP.Utils.Framework;
using JP.Utils.Functions;
using PlantSitter.Common;
using PlantSitterCustomControl;
using PlantSitterShared.API;
using PlantSitterShared.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlantSitter.ViewModel
{
    public class AddPlanViewModel : ViewModelBase, INavigable
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
                return _searchCommand = new RelayCommand(async () =>
                  {
                      try
                      {
                          await SearchPlantAsync();
                      }
                      catch (Exception)
                      {
                          ShowNoResult = Visibility.Visible;
                          ShowLoading = false;
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

        private RelayCommand<object> _selectCommand;
        public RelayCommand<object> SelectCommand
        {
            get
            {
                if (_selectCommand != null) return _selectCommand;
                return _selectCommand = new RelayCommand<object>(async (o) =>
                {
                    var pid = (int)o;
                    ShowLoading = true;
                    var result = await CloudService.AddPlan(pid, "Plant", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), CTSFactory.MakeCTS(10000).Token);
                    if (!result.IsSuccessful)
                    {
                        ShowLoading = false;
                        ToastService.SendToast("添加计划失败");
                        return;
                    }

                });
            }
        }

        private bool _showLoading;
        public bool ShowLoading
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

        private Plant _currentPlant;
        public Plant CurrentPlant
        {
            get
            {
                return _currentPlant;
            }
            set
            {
                if (_currentPlant != value)
                {
                    _currentPlant = value;
                    RaisePropertyChanged(() => CurrentPlant);
                }
            }
        }

        private double _enviMoistureMin;
        public double EnviMoistureMin
        {
            get
            {
                return _enviMoistureMin;
            }
            set
            {
                if (_enviMoistureMin != value)
                {
                    _enviMoistureMin = value;
                    RaisePropertyChanged(() => EnviMoistureMin);
                }
            }
        }

        private double _enviMoistureMax;
        public double EnviMoistureMax
        {
            get
            {
                return _enviMoistureMax;
            }
            set
            {
                if (_enviMoistureMax != value)
                {
                    _enviMoistureMax = value;
                    RaisePropertyChanged(() => EnviMoistureMax);
                }
            }
        }

        private double _enviTempMin;
        public double EnviTempMin
        {
            get
            {
                return _enviTempMin;
            }
            set
            {
                if (_enviTempMin != value)
                {
                    _enviTempMin = value;
                    RaisePropertyChanged(() => EnviTempMin);
                }
            }
        }

        private double _enviTempMax;
        public double EnviTempMax
        {
            get
            {
                return _enviTempMax;
            }
            set
            {
                if (_enviTempMax != value)
                {
                    _enviTempMax = value;
                    RaisePropertyChanged(() => EnviTempMax);
                }
            }
        }

        private RelayCommand _addPlantCommand;
        public RelayCommand AddPlantCommand
        {
            get
            {
                if (_addPlantCommand != null) return _addPlantCommand;
                return _addPlantCommand = new RelayCommand(async () =>
                  {
                      await AddPlant();
                  });
            }
        }

        public AddPlanViewModel()
        {
            ShowNoResult = Visibility.Collapsed;
            ShowLoading = false;
            ResultPlants = new ObservableCollection<Plant>();
            CurrentPlant = new Plant();

            EnviMoistureMax = EnviTempMax = 35;
            EnviMoistureMin = EnviTempMin = 20;
        }

        private async Task SearchPlantAsync()
        {
            ShowLoading = true;
            CommonRespMsg result;
            if (Functions.IsHasCHZN(SearchInfo))
            {
                result = await CloudService.SearchPlant(null, SearchInfo, "", CTSFactory.MakeCTS(10000).Token);
            }
            else
            {
                int id;
                var isID = int.TryParse(SearchInfo, out id);
                if (isID)
                {
                    result = await CloudService.SearchPlant(id, "", "", CTSFactory.MakeCTS(10000).Token);
                }
                else
                {
                    result = await CloudService.SearchPlant(null, "", SearchInfo, CTSFactory.MakeCTS(10000).Token);
                }
            }
            result.ParseAPIResult();
            if (!result.IsSuccessful)
            {
                ShowNoResult = Visibility.Visible;
                return;
            }
            ShowNoResult = Visibility.Collapsed;
            var plants = Plant.ParsePlantsToArray(result.JsonSrc);
            ResultPlants = new ObservableCollection<Plant>();
            plants.ForEach(p => ResultPlants.Add(p));

            foreach (var plant in ResultPlants)
            {
                var task = plant.DownloadImage();
            }
            ShowLoading = false;
        }

        private async Task AddPlant()
        {
            var result = await CloudService.AddPlant(
                CurrentPlant.NameInChinese, "",
                "0~0",
                $"{EnviMoistureMin}~{EnviMoistureMax}",
                $"{EnviTempMin}~{EnviTempMax}",
                CurrentPlant.LightRange.ConvertToString(),
                CurrentPlant.Desc, "", CTSFactory.MakeCTS(10000).Token);
            if (!result.IsSuccessful)
            {

            }
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
