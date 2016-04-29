using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JP.API;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using PlantSitterCustomControl;
using PlantSitterShared.API;
using PlantSitterShared.Common;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;

namespace PlantSitterShared.Model
{
    public class Plant : ViewModelBase
    {
        private int _pid;
        public int Pid
        {
            get
            {
                return _pid;
            }
            set
            {
                if (_pid != value)
                {
                    _pid = value;
                    RaisePropertyChanged(() => Pid);
                }
            }
        }

        private string _desc;
        public string Desc
        {
            get
            {
                return _desc;
            }
            set
            {
                if (_desc != value)
                {
                    _desc = value;
                    RaisePropertyChanged(() => Desc);
                }
            }
        }

        private string _nameInChinese;
        public string NameInChinese
        {
            get
            {
                return _nameInChinese;
            }
            set
            {
                if (_nameInChinese != value)
                {
                    _nameInChinese = value;
                    RaisePropertyChanged(() => NameInChinese);
                }
            }
        }

        private string _nameInEnglish;
        public string NameInEnglish
        {
            get
            {
                return _nameInEnglish;
            }
            set
            {
                if (_nameInEnglish != value)
                {
                    _nameInEnglish = value;
                    RaisePropertyChanged(() => NameInEnglish);
                }
            }
        }

        private Vector2 _soilMoistureRange;
        public Vector2 SoilMoistureRange
        {
            get
            {
                return _soilMoistureRange;
            }
            set
            {
                if (_soilMoistureRange != value)
                {
                    _soilMoistureRange = value;
                    RaisePropertyChanged(() => SoilMoistureRange);
                }
            }
        }

        private Vector2 _enviTempRange;
        public Vector2 EnviTempRange
        {
            get
            {
                return _enviTempRange;
            }
            set
            {
                if (_enviTempRange != value)
                {
                    _enviTempRange = value;
                    RaisePropertyChanged(() => EnviTempRange);
                    TempRangeStr = $"{EnviTempRange.X} ℃ ~ {EnviTempRange.Y} ℃";
                }
            }
        }

        private Vector2 _enviMoistureRange;
        public Vector2 EnviMoistureRange
        {
            get
            {
                return _enviMoistureRange;
            }
            set
            {
                if (_enviMoistureRange != value)
                {
                    _enviMoistureRange = value;
                    RaisePropertyChanged(() => EnviMoistureRange);
                    MoistureRangeStr = $"{EnviMoistureRange.X} % ~ {EnviMoistureRange.Y} %";
                }
            }
        }

        private Vector2 _lightRange;
        public Vector2 LightRange
        {
            get
            {
                return _lightRange;
            }
            set
            {
                if (_lightRange != value)
                {
                    _lightRange = value;
                    RaisePropertyChanged(() => LightRange);
                }
            }
        }

        private BitmapImage _imgBitmap;
        public BitmapImage ImgBitmap
        {
            get
            {
                return _imgBitmap;
            }
            set
            {
                if (_imgBitmap != value)
                {
                    _imgBitmap = value;
                    RaisePropertyChanged(() => ImgBitmap);
                }
            }
        }

        public string ImageUrl { get; set; }

        public string CacheFilePath { get; set; }

        private RelayCommand _selectCommand;
        public RelayCommand SelectCommand
        {
            get
            {
                if (_selectCommand != null) return _selectCommand;
                return _selectCommand = new RelayCommand(() =>
                  {
                      Messenger.Default.Send(new GenericMessage<Plant>(this), MessengerToken.SelectPlantToGrow);
                  });
            }
        }

        private bool _likeSunshine;
        public bool LikeSunshine
        {
            get
            {
                return _likeSunshine;
            }
            set
            {
                if (_likeSunshine != value)
                {
                    _likeSunshine = value;
                    RaisePropertyChanged(() => LikeSunshine);
                    if (value) SunshineKindStr = "喜阳植物";
                    else SunshineKindStr = "喜阴植物";
                }
            }
        }

        private string _sunshineKindStr;
        public string SunshineKindStr
        {
            get
            {
                return _sunshineKindStr;
            }
            set
            {
                if (_sunshineKindStr != value)
                {
                    _sunshineKindStr = value;
                    RaisePropertyChanged(() => SunshineKindStr);
                }
            }
        }

        private string _tempRangeStr;
        public string TempRangeStr
        {
            get
            {
                return _tempRangeStr;
            }
            set
            {
                if (_tempRangeStr != value)
                {
                    _tempRangeStr = value;
                    RaisePropertyChanged(() => TempRangeStr);
                }
            }
        }

        private string _moistureRangeStr;
        public string MoistureRangeStr
        {
            get
            {
                return _moistureRangeStr;
            }
            set
            {
                if (_moistureRangeStr != value)
                {
                    _moistureRangeStr = value;
                    RaisePropertyChanged(() => MoistureRangeStr);
                }
            }
        }

        public Plant()
        {
            ImgBitmap = new BitmapImage();
            Desc = "暂无简介";
            ImageUrl = "";
            NameInChinese = "";
            NameInEnglish = "";
            LikeSunshine = true;
        }

        public async Task UpdateInfoAsync()
        {
            var result = await CloudService.GetPlantInfo(this.Pid, CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            if(!result.IsSuccessful)
            {
                ToastService.SendToast("获取植物信息失败");
                return;
            }
            var jsonObj = JsonObject.Parse(result.JsonSrc);
            var plantObj = JsonParser.GetJsonObjFromJsonObj(jsonObj, "Plant");
            var plant = Plant.ParseJsonToObj(plantObj.ToString());
            this.NameInChinese = plant.NameInChinese;
            this.NameInEnglish = plant.NameInEnglish;
            this.Desc = plant.Desc;
            this.SoilMoistureRange = plant.SoilMoistureRange;
            this.EnviMoistureRange = plant.EnviMoistureRange;
            this.EnviTempRange = plant.EnviTempRange;
            this.LightRange = plant.LightRange;
            this.ImageUrl = plant.ImageUrl;
        }

        public async Task DownloadImage()
        {
            if (string.IsNullOrEmpty(ImageUrl))
            {
                return;
            }
            var stream = await APIHelper.GetIRandomAccessStreamFromUrlAsync(ImageUrl);
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            ImgBitmap = bitmap;
        }

        public static async Task<Plant> GetPlantByIdAsync(int pid)
        {
            try
            {
                var result = await CloudService.GetPlantInfo(pid, CTSFactory.MakeCTS().Token);
                result.ParseAPIResult();
                if (!result.IsSuccessful)
                {
                    return null;
                }
                var obj = JsonObject.Parse(result.JsonSrc);
                var plantObj = JsonParser.GetJsonObjFromJsonObj(obj, "Plant");
                var id = JsonParser.GetStringFromJsonObj(plantObj, "pid");
                var nameInChinese = JsonParser.GetStringFromJsonObj(plantObj, "name_c");
                var nameInEnglish = JsonParser.GetStringFromJsonObj(plantObj, "name_e");
                var soilMoisture = JsonParser.GetStringFromJsonObj(plantObj, "soil_moisture");
                var enviMoisture = JsonParser.GetStringFromJsonObj(plantObj, "envi_moisture");
                var enviTemp = JsonParser.GetStringFromJsonObj(plantObj, "envi_temp");
                var light = JsonParser.GetStringFromJsonObj(plantObj, "light");
                var url = JsonParser.GetStringFromJsonObj(plantObj, "img_url");

                var newPlant = new Plant();
                newPlant.Pid = int.Parse(id);
                newPlant.NameInChinese = nameInChinese;
                newPlant.NameInEnglish = nameInEnglish;
                if (soilMoisture != null) newPlant.SoilMoistureRange = new Vector2(float.Parse(soilMoisture.Split('~')[0]), float.Parse(soilMoisture.Split('~')[1]));
                if (enviMoisture != null) newPlant.EnviMoistureRange = new Vector2(float.Parse(enviMoisture.Split('~')[0]), float.Parse(enviMoisture.Split('~')[1]));
                if (enviTemp != null) newPlant.EnviTempRange = new Vector2(float.Parse(enviTemp.Split('~')[0]), float.Parse(enviTemp.Split('~')[1]));
                if (light != null) newPlant.LightRange = new Vector2(float.Parse(light.Split('~')[0]), float.Parse(light.Split('~')[1]));
                if (url != null) newPlant.ImageUrl = url;

                return newPlant;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Plant> ParsePlantsToArray(string json)
        {
            var plants = new List<Plant>();
            var jsonObj = JsonObject.Parse(json);
            var plantsArray = JsonParser.GetJsonArrayFromJsonObj(jsonObj, "Plants");
            if(plantsArray!=null)
            {
                foreach(var item in plantsArray)
                {
                    var plant = ParseJsonToObj(item.ToString());
                    if(plant!= null) plants.Add((plant));
                }
            }
            return plants;
        }

        /// <summary>
        /// {
        //    "pid":"6",
        //    "name_c":"向日葵",
        //    "name_e":"Sunflower",
        //    "soil_moisture":"1~1",
        //    "envi_temp":"20~30",
        //    "envi_moisture":"48~84",
        //    "light":"100~20000",
        //    "img_url":"http://pic.baike.soso.com/p/20140210/20140210150929-2045729024.jpg",
        //    "desc":null
        //}
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Plant ParseJsonToObj(string json)
        {
            JsonObject jsonObj;
            if (!JsonObject.TryParse(json, out jsonObj))
            {
                return null;
            }

            Plant plant = new Plant();
            var pid = JsonParser.GetStringFromJsonObj(jsonObj, "pid");
            var name_c = JsonParser.GetStringFromJsonObj(jsonObj, "name_c");
            var name_e = JsonParser.GetStringFromJsonObj(jsonObj, "name_e");
            var soil_moisture = JsonParser.GetStringFromJsonObj(jsonObj, "soil_moisture");
            var envi_temp = JsonParser.GetStringFromJsonObj(jsonObj, "envi_temp");
            var envi_moisture = JsonParser.GetStringFromJsonObj(jsonObj, "envi_moisture");
            var light = JsonParser.GetStringFromJsonObj(jsonObj, "light");
            var img_url = JsonParser.GetStringFromJsonObj(jsonObj, "img_url");
            var desc = JsonParser.GetStringFromJsonObj(jsonObj, "description");

            try
            {
                if (pid != null) plant.Pid = int.Parse(pid);
                if (name_c.IsNotNullOrEmpty()) plant.NameInChinese = name_c;
                if (name_e.IsNotNullOrEmpty()) plant.NameInEnglish = name_e;
                if (soil_moisture.IsNotNullOrEmpty())
                {
                    var sms = soil_moisture.Split('~');
                    plant.SoilMoistureRange = new Vector2(float.Parse(sms[0]), float.Parse(sms[1]));
                }
                if (envi_temp.IsNotNullOrEmpty())
                {
                    var ets = envi_temp.Split('~');
                    plant.EnviTempRange = new Vector2(float.Parse(ets[0]), float.Parse(ets[1]));
                }
                if (envi_moisture.IsNotNullOrEmpty())
                {
                    var ems = envi_moisture.Split('~');
                    plant.EnviMoistureRange = new Vector2(float.Parse(ems[0]), float.Parse(ems[1]));
                }
                if (light.IsNotNullOrEmpty())
                {
                    var ls = light.Split('~');
                    plant.LightRange = new Vector2(float.Parse(ls[0]), float.Parse(ls[1]));
                }
                if (img_url.IsNotNullOrEmpty()) plant.ImageUrl = img_url;
                if (desc.IsNotNullOrEmpty()) plant.Desc = desc;

                if (plant.LightRange.Y >= 20000) plant.LikeSunshine = true;

                return plant;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
