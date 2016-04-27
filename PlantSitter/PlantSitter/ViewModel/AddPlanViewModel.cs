using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JP.API;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using JP.Utils.Framework;
using JP.Utils.Functions;
using JP.UWP.CustomControl;
using PlantSitter.Common;
using PlantSitterCustomControl;
using PlantSitterShared.API;
using PlantSitterShared.Common;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Json;
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

        private bool _showSearchResultGrid;
        public bool ShowSearchResultGrid
        {
            get
            {
                return _showSearchResultGrid;
            }
            set
            {
                if (_showSearchResultGrid != value)
                {
                    _showSearchResultGrid = value;
                    RaisePropertyChanged(() => ShowSearchResultGrid);
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

        #region Search plant

        private bool _showSearch;
        public bool ShowSearch
        {
            get
            {
                return _showSearch;
            }
            set
            {
                if (_showSearch != value)
                {
                    _showSearch = value;
                    RaisePropertyChanged(() => ShowSearch);
                }
            }
        }


        private RelayCommand _searchImageCommand;
        public RelayCommand SearchImageCommand
        {
            get
            {
                if (_searchImageCommand != null) return _searchImageCommand;
                return _searchImageCommand = new RelayCommand(async () =>
                  {
                      await SearchPlantImageByNameAsync();
                  });
            }
        }

        private ObservableCollection<NetworkImage> _searchReusltForImages;
        public ObservableCollection<NetworkImage> SearchReusltForImages
        {
            get
            {
                return _searchReusltForImages;
            }
            set
            {
                if (_searchReusltForImages != value)
                {
                    _searchReusltForImages = value;
                    RaisePropertyChanged(() => SearchReusltForImages);
                }
            }
        }

        private RelayCommand<object> _selectImageCommand;
        public RelayCommand<object> SelectImageCommand
        {
            get
            {
                if (_selectImageCommand != null) return _selectImageCommand;
                return _selectImageCommand = new RelayCommand<object>((o) =>
                  {
                      var img = o as NetworkImage;
                      var url = img.Url;
                      CurrentPlant.ImageUrl = url;
                      CurrentPlant.ImgBitmap = img.ImgSource;
                      ShowSearchResultGrid = false;
                      SearchReusltForImages.Clear();
                      SearchReusltForImages = null;
                  });
            }
        }

        #endregion

        #region Add plant
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
                      await AddPlantAsync();
                  });
            }
        }

        #endregion

        public AddPlanViewModel()
        {
            ShowNoResult = Visibility.Collapsed;
            ShowLoading = false;
            ShowSearch = false;
            ResultPlants = new ObservableCollection<Plant>();
            CurrentPlant = new Plant();

            EnviTempMax = 35;
            EnviTempMin = 20;
            EnviMoistureMax = 80;
            EnviMoistureMin = 20;
            RegisterMessage();
        }

        private void RegisterMessage()
        {
            Messenger.Default.Register<GenericMessage<Plant>>(this, MessengerToken.SelectPlantToGrow, async act =>
              {
                  await SelectPlantToGrowAsync(act.Content);
              });
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

        private async Task AddPlantAsync()
        {
            if (!CurrentPlant.NameInChinese.IsNotNullOrEmpty())
            {
                ToastService.SendToast("请添加植物中文名");
                return;
            }
            if (!CurrentPlant.ImageUrl.IsNotNullOrEmpty())
            {
                ToastService.SendToast("请选择一张照片");
                return;
            }
            if (!CurrentPlant.Desc.IsNotNullOrEmpty())
            {
                var likeSunshine = CurrentPlant.LikeSunshine ? "喜阳" : "喜阴";
                CurrentPlant.Desc = $"{CurrentPlant.NameInChinese} 是一种{likeSunshine}植物。";
            }
            try
            {
                var result = await CloudService.AddPlant(
                   CurrentPlant.NameInChinese, CurrentPlant.NameInEnglish,
                   "0~1",
                   $"{EnviMoistureMin}~{EnviMoistureMax}",
                   $"{EnviTempMin}~{EnviTempMax}",
                   CurrentPlant.LightRange.ConvertToString(),
                   CurrentPlant.Desc, CurrentPlant.ImageUrl, CTSFactory.MakeCTS(10000).Token);

                result.ParseAPIResult();
                if (!result.IsSuccessful)
                {
                    ToastService.SendToast(ErrorTable.GetMessageFromErrorCode(result.ErrorCode));
                    return;
                }
                PopupService.TryHideCPEX();
                var obj = JsonObject.Parse(result.JsonSrc);
                var pid = JsonParser.GetStringFromJsonObj(obj, "PlantID",null);
                if (pid != null)
                {
                    CurrentPlant.Pid = int.Parse(pid);
                    await SelectPlantToGrowAsync(CurrentPlant);
                }
                else throw new ArgumentNullException();
            }
            catch (Exception)
            {
                ToastService.SendToast("添加失败");
                return;
            }
        }

        private async Task SelectPlantToGrowAsync(Plant plant)
        {
            ShowLoading = true;
            var result = await CloudService.AddPlan(plant.Pid, "Plant", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), CTSFactory.MakeCTS(10000).Token);
            result.ParseAPIResult();
            ShowLoading = false;
            if (!result.IsSuccessful)
            {
                ToastService.SendToast("添加计划失败");
                return;
            }
            PopupService.TryHideCPEX();
            App.VMLocator.UsersPlansVM.AddNewPlan(new UserPlan()
            {
                CurrentPlant = plant,
                CreateTime = DateTime.Now,
                Name = "Plant",
                CurrentUser = App.VMLocator.MainVM.CurrentUser,
            });
        }

        private async Task SearchPlantImageByNameAsync()
        {
            if (!CurrentPlant.NameInChinese.IsNotNullOrEmpty() && !CurrentPlant.NameInEnglish.IsNotNullOrEmpty())
            {
                ToastService.SendToast("请先输入植物的名字");
                return;
            }
            SearchReusltForImages = new ObservableCollection<NetworkImage>();
            var query = CurrentPlant.NameInChinese.IsNotNullOrEmpty() ? CurrentPlant.NameInChinese : CurrentPlant.NameInEnglish;
            ShowSearchResultGrid = true;
            ShowSearch = true;
            var result = await CloudService.SearchImage(query);
            if (!result.IsSuccessful)
            {
                ToastService.SendToast("搜索失败");
                return;
            }
            var list = NetworkImage.ParseToList(result.JsonSrc);
            list.ForEach(i => SearchReusltForImages.Add(i));

            var taskList = new List<Task>();
            foreach (var item in SearchReusltForImages)
            {
                taskList.Add(item.DownloadThumbImageAsync());
            }
            await Task.WhenAll(taskList);
            ShowSearch = false;
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
