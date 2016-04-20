using GalaSoft.MvvmLight;
using JP.API;
using JP.Utils.Data.Json;
using PlantSitterShared.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;

namespace PlantSitterShard.Model
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

        public Plant()
        {
            ImgBitmap = new BitmapImage();
        }

        public async Task DownloadImage()
        {
            if(string.IsNullOrEmpty(ImageUrl))
            {
                return;
            }
            var stream =await APIHelper.GetIRandomAccessStreamFromUrlAsync(ImageUrl);
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
    }
}
