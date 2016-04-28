using GalaSoft.MvvmLight;
using JP.Utils.Data.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace PlantSitterShared.Model
{
    public class PlantTimeline : ViewModelBase
    {
        private int _tid;
        public int Tid
        {
            get
            {
                return _tid;
            }
            set
            {
                if (_tid != value)
                {
                    _tid = value;
                    RaisePropertyChanged(() => Tid);
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

        private PlantSitterUser _currentUser;
        public PlantSitterUser CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    RaisePropertyChanged(() => CurrentUser);
                }
            }
        }

        private int _gid;
        public int Gid
        {
            get
            {
                return _gid;
            }
            set
            {
                if (_gid != value)
                {
                    _gid = value;
                    RaisePropertyChanged(() => Gid);
                }
            }
        }

        private double _soilMoisture;
        public double SoilMoisture
        {
            get
            {
                return _soilMoisture;
            }
            set
            {
                if (_soilMoisture != value)
                {
                    _soilMoisture = value;
                    RaisePropertyChanged(() => SoilMoisture);
                }
            }
        }

        private double _enviTemp;
        public double EnviTemp
        {
            get
            {
                return _enviTemp;
            }
            set
            {
                if (_enviTemp != value)
                {
                    _enviTemp = value;
                    RaisePropertyChanged(() => EnviTemp);
                }
            }
        }

        private double _enviMoisture;
        public double EnviMoisture
        {
            get
            {
                return _enviMoisture;
            }
            set
            {
                if (_enviMoisture != value)
                {
                    _enviMoisture = value;
                    RaisePropertyChanged(() => EnviMoisture);
                }
            }
        }

        private double _light;
        public double Light
        {
            get
            {
                return _light;
            }
            set
            {
                if (_light != value)
                {
                    _light = value;
                    RaisePropertyChanged(() => Light);
                }
            }
        }

        private DateTime _recoredTime;
        public DateTime RecordTime
        {
            get
            {
                return _recoredTime;
            }
            set
            {
                if (_recoredTime != value)
                {
                    _recoredTime = value;
                    RaisePropertyChanged(() => RecordTime);
                }
            }
        }

        public PlantTimeline()
        {

        }

        /*
        {"isSuccessed":true,
        "error_code":0,"error_message":"",
        "Record":[{"tid":"120","pid":"6","uid":"3","gid":"19","soil_moisture":"1","envi_temp":"25","envi_moisture":"40",
        "light":"200","time":"2016-05-01 01:43:00"},{"tid":"119","pid":"6","uid":"3","gid":"19","soil_moisture":"1","envi_temp":"25","envi_moisture":"40","light":"200","time":"2016-05-01 00:43:00"},{"tid":"118","pid":"6","uid":"3","gid":"19","soil_moisture":"1","envi_temp":"25","envi_moisture":"40","light":"200","time":"2016-04-30 23:43:00"},{"tid":"117","pid":"6","uid":"3","gid":"19","soil_moisture":"1","envi_temp":"25","envi_moisture":"40","light":"200","time":"2016-04-30 22:43:00"},{"tid":"116","pid":"6","uid":"3","gid":"19","soil_moisture":"1","envi_temp":"25","envi_moisture":"40","light":"200","time":"2016-04-30 21:43:00"}],"executionTime":0.0011789999999999}
        */
        public static List<PlantTimeline> ParseToList(string json)
        {
            var list = new List<PlantTimeline>();

            var obj = JsonObject.Parse(json);
            var array = JsonParser.GetJsonArrayFromJsonObj(obj, "Record");
            foreach (var item in array)
            {
                var timeline = ParseToObject(item.ToString());
                if (timeline != null)
                {
                    list.Add(timeline);
                }
            }

            return list;
        }

        public static PlantTimeline ParseToObject(string json)
        {
            var item = JsonObject.Parse(json);
            var tid = JsonParser.GetStringFromJsonObj(item, "tid");
            var pid = JsonParser.GetStringFromJsonObj(item, "pid");
            var uid = JsonParser.GetStringFromJsonObj(item, "uid");
            var gid = JsonParser.GetStringFromJsonObj(item, "gid");
            var soil_moisture = JsonParser.GetStringFromJsonObj(item, "soil_moisture");
            var envi_temp = JsonParser.GetStringFromJsonObj(item, "envi_temp");
            var light = JsonParser.GetStringFromJsonObj(item, "light");
            var time = JsonParser.GetStringFromJsonObj(item, "time");

            try
            {
                var timeline = new PlantTimeline()
                {
                    Tid = int.Parse(tid),
                    CurrentPlant = new Plant() { Pid = int.Parse(pid) },
                    CurrentUser = new PlantSitterUser() { Uid = int.Parse(uid) },
                    Gid = int.Parse(gid),
                    SoilMoisture = double.Parse(soil_moisture),
                    EnviTemp = double.Parse(envi_temp),
                    Light = double.Parse(light),
                    RecordTime = DateTime.Parse(time)
                };
                return timeline;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
