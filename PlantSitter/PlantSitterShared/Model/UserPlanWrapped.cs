using GalaSoft.MvvmLight;
using JP.Utils.UI;
using PlantSitterShared.API;
using PlantSitterShared.Common;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace PlantSitterShared.Model
{
    public class UserPlanWrapped : ViewModelBase
    {
        public UserPlan CurrentPlan { get; set; }

        private bool _isMain;
        public bool IsMain
        {
            get
            {
                return _isMain;
            }
            set
            {
                if (_isMain != value)
                {
                    _isMain = value;
                    RaisePropertyChanged(() => IsMain);
                }
            }
        }

        public string CreateTimeString
        {
            get
            {
                var diff = DateTime.Now - CurrentPlan.CreateTime;
                var days = diff.TotalDays;
                var months = days / 30;
                var daysLeft = days % 30;
                if ((int)months >= 1)
                {
                    return $"照看了 {(int)months} 月 {(int)daysLeft} 天";
                }
                else
                {
                    return $"照看了 {(int)daysLeft} 天";
                }
            }
        }

        /// <summary>
        /// 首页显示的
        /// </summary>
        private string _scoreSumUp;
        public string ScoreSumUp
        {
            get
            {
                return _scoreSumUp;
            }
            set
            {
                if (_scoreSumUp != value)
                {
                    _scoreSumUp = value;
                    RaisePropertyChanged(() => ScoreSumUp);
                }
            }
        }

        private int _scoreValue;
        public int ScoreValue
        {
            get
            {
                return _scoreValue;
            }
            set
            {
                if (_scoreValue != value)
                {
                    _scoreValue = value;
                    RaisePropertyChanged(() => ScoreValue);
                    ScoreToDisplay = $"环境 {value} 分";
                    if (value >= 70)
                    {
                        ScoreSumUp = "适宜";
                        ScoreDesc = $"环境综合分{ScoreValue}分，说明此时的环境十分适合成长喔~";
                        UpdateColorByScoreLevel(0);
                    }
                    else if (value < 70 && value >= 40)
                    {
                        ScoreSumUp = "不太适宜";
                        ScoreDesc = $"环境综合分{ScoreValue}分，说明此时的环境比较一般~";
                        UpdateColorByScoreLevel(1);
                    }
                    else
                    {
                        ScoreSumUp = "非常糟糕";
                        ScoreDesc = $"环境综合分{ScoreValue}分，说明此时的环境非常糟糕 :-(";
                        UpdateColorByScoreLevel(2);
                    }
                }
            }
        }

        /// <summary>
        /// 首页显示的
        /// </summary>
        private string _scoreToDisplay;
        public string ScoreToDisplay
        {
            get
            {
                return _scoreToDisplay;
            }
            set
            {
                if (_scoreToDisplay != value)
                {
                    _scoreToDisplay = value;
                    RaisePropertyChanged(() => ScoreToDisplay);
                }
            }
        }

        /// <summary>
        /// 在详情页面的植物名字下面显示的
        /// </summary>
        private string _scoreDesc;
        public string ScoreDesc
        {
            get
            {
                return _scoreDesc;
            }
            set
            {
                if (_scoreDesc != value)
                {
                    _scoreDesc = value;
                    RaisePropertyChanged(() => ScoreDesc);
                }
            }
        }

        private SolidColorBrush _colorByScore;
        public SolidColorBrush ColorByScore
        {
            get
            {
                return _colorByScore;
            }
            set
            {
                if (_colorByScore != value)
                {
                    _colorByScore = value;
                    RaisePropertyChanged(() => ColorByScore);
                }
            }
        }

        private SolidColorBrush _secondColorByScore;
        public SolidColorBrush SecondColorByScore
        {
            get
            {
                return _secondColorByScore;
            }
            set
            {
                if (_secondColorByScore != value)
                {
                    _secondColorByScore = value;
                    RaisePropertyChanged(() => SecondColorByScore);
                }
            }
        }

        #region Soil card
        private string _smStatus;
        public string SMStauts
        {
            get
            {
                return _smStatus;
            }
            set
            {
                if (_smStatus != value)
                {
                    _smStatus = value;
                    RaisePropertyChanged(() => SMStauts);
                }
            }
        }

        private string _smStatusSumUp;
        public string SMStatusSumUp
        {
            get
            {
                return _smStatusSumUp;
            }
            set
            {
                if (_smStatusSumUp != value)
                {
                    _smStatusSumUp = value;
                    RaisePropertyChanged(() => SMStatusSumUp);
                }
            }
        }

        private string _smAddv;
        public string SMAdv
        {
            get
            {
                return _smAddv;
            }
            set
            {
                if (_smAddv != value)
                {
                    _smAddv = value;
                    RaisePropertyChanged(() => SMAdv);
                }
            }
        }

        #endregion

        #region Temp
        private string _tempValue;
        public string TempValue
        {
            get
            {
                return _tempValue;
            }
            set
            {
                if (_tempValue != value)
                {
                    _tempValue = value;
                    RaisePropertyChanged(() => TempValue);
                }
            }
        }

        private string _tempSumUp;
        public string TempSumUp
        {
            get
            {
                return _tempSumUp;
            }
            set
            {
                if (_tempSumUp != value)
                {
                    _tempSumUp = value;
                    RaisePropertyChanged(() => TempSumUp);
                }
            }
        }

        private string _tempAdv;
        public string TempAdv
        {
            get
            {
                return _tempAdv;
            }
            set
            {
                if (_tempAdv != value)
                {
                    _tempAdv = value;
                    RaisePropertyChanged(() => TempAdv);
                }
            }
        }

        #endregion

        #region Moisture
        private string _moistureValue;
        public string MoistureValue
        {
            get
            {
                return _moistureValue;
            }
            set
            {
                if (_moistureValue != value)
                {
                    _moistureValue = value;
                    RaisePropertyChanged(() => MoistureValue);
                }
            }
        }

        private string _moistureSumUp;
        public string MoistureSumUp
        {
            get
            {
                return _moistureSumUp;
            }
            set
            {
                if (_moistureSumUp != value)
                {
                    _moistureSumUp = value;
                    RaisePropertyChanged(() => MoistureSumUp);
                }
            }
        }

        private string _moistureAdv;
        public string MoistureAdv
        {
            get
            {
                return _moistureAdv;
            }
            set
            {
                if (_moistureAdv != value)
                {
                    _moistureAdv = value;
                    RaisePropertyChanged(() => MoistureAdv);
                }
            }
        }

        #endregion

        #region Light
        private string _lightValue;
        public string LightValue
        {
            get
            {
                return _lightValue;
            }
            set
            {
                if (_lightValue != value)
                {
                    _lightValue = value;
                    RaisePropertyChanged(() => LightValue);
                }
            }
        }

        private string _lightSumUp;
        public string LightSumUp
        {
            get
            {
                return _lightSumUp;
            }
            set
            {
                if (_lightSumUp != value)
                {
                    _lightSumUp = value;
                    RaisePropertyChanged(() => LightSumUp);
                }
            }
        }

        private string _lightAdv;
        public string LightAdv
        {
            get
            {
                return _lightAdv;
            }
            set
            {
                if (_lightAdv != value)
                {
                    _lightAdv = value;
                    RaisePropertyChanged(() => LightAdv);
                }
            }
        }

        #endregion

        private string _lastRecordTime;
        public string LastRecordTime
        {
            get
            {
                return _lastRecordTime;
            }
            set
            {
                if (_lastRecordTime != value)
                {
                    _lastRecordTime = value;
                    RaisePropertyChanged(() => LastRecordTime);
                }
            }
        }

        public UserPlanWrapped(UserPlan plan)
        {
            this.CurrentPlan = plan;
        }

        /// <summary>
        /// 从服务端获取24小时之内的数据
        /// </summary>
        /// <returns></returns>
        public async Task FetchRecordGetScoreAsync()
        {
            var records = await CloudService.GetTimelineData(this.CurrentPlan.Gid, "byDays", "1", CTSFactory.MakeCTS().Token);
            var timelineData = PlantTimeline.ParseToList(records.JsonSrc);

            //Clear before add new one
            CurrentPlan.RecordData.Clear();
            foreach(var item in timelineData)
            {
                CurrentPlan.RecordData.Insert(0,item);
            }
            LastRecordTime = CurrentPlan.RecordData.Last().RecordTime.ToString();

            CalculateScoreAndUpdate();
            UpdateCardStatus();
        }

        private void UpdateCardStatus()
        {
            SMAdv = CurrentPlan.CurrentPlant.SoilMoistureRange.X == 0 ? "请保持土壤干燥" : "请保持土壤湿润";
            LightAdv = CurrentPlan.CurrentPlant.LikeSunshine ? "喜阳植物光线应该强一点" : "喜阴植物光线应该弱一点";
            MoistureAdv = $"相对湿度适合在 {CurrentPlan.CurrentPlant.EnviMoistureRange.X} ~ {CurrentPlan.CurrentPlant.EnviMoistureRange.Y} %";
            TempAdv= $"环境温度适合在 {CurrentPlan.CurrentPlant.EnviTempRange.X} ~ {CurrentPlan.CurrentPlant.EnviTempRange.Y} ℃";

            var lastRecord = CurrentPlan.RecordData.Last();
            if (lastRecord.SoilMoisture == 0)
            {
                SMStatusSumUp = "不湿润";
                SMStauts = "无水";
            }
            else
            {
                SMStatusSumUp = "湿润";
                SMStauts = "有水";
            }

            TempValue = lastRecord.EnviTemp.ToString() + " ℃";
            if (lastRecord.EnviTemp >= CurrentPlan.CurrentPlant.EnviTempRange.X && lastRecord.EnviTemp <= CurrentPlan.CurrentPlant.EnviTempRange.Y)
            {
                TempSumUp = "适宜";
            }
            else TempSumUp = "不适宜";

            MoistureValue = lastRecord.EnviMoisture.ToString() +" %";
            if (lastRecord.EnviMoisture >= CurrentPlan.CurrentPlant.EnviMoistureRange.X && lastRecord.EnviTemp <= CurrentPlan.CurrentPlant.EnviMoistureRange.Y)
            {
                MoistureSumUp = "适宜";
            }
            else MoistureSumUp = "不适宜";

            LightValue = lastRecord.Light.ToString()+ " Lux";
            if (lastRecord.Light >= CurrentPlan.CurrentPlant.LightRange.X && lastRecord.EnviTemp <= CurrentPlan.CurrentPlant.LightRange.Y)
            {
                LightSumUp = "充足";
            }
            else if (lastRecord.Light < CurrentPlan.CurrentPlant.LightRange.X)
            {
                LightSumUp = "不充足";
            }
            else if (lastRecord.Light > CurrentPlan.CurrentPlant.LightRange.Y)
            {
                LightSumUp = "过曝";
            }
        }

        /// <summary>
        /// 更新动态磁铁
        /// </summary>
        public void UpdateTile()
        {
            var lastRecord = CurrentPlan.RecordData.First();
            var soilSumup= lastRecord.SoilMoisture.ToString() == "0" ? "干燥" : "湿润";
            var line1 = "土壤：" + soilSumup;
            var line2 = "环境温度：" + lastRecord.EnviTemp.ToString() + " ℃";
            var line3 = "环境湿度：" + lastRecord.EnviMoisture.ToString() + " %";
            var line4 = "光照：" + lastRecord.Light.ToString() + " Lux";

            LiveTileUpdater.UpdateTile(CurrentPlan.CurrentPlant.CacheFilePath, lastRecord.RecordTime.ToString(), line2, line3, line4);
        }

        /// <summary>
        /// 根据分数等级设置提醒颜色
        /// </summary>
        /// <param name="level"></param>
        private void UpdateColorByScoreLevel(int level)
        {
            switch (level)
            {
                case 0:
                    {
                        ColorByScore = new SolidColorBrush(ColorConverter.HexToColor("#71997b").Value);
                        SecondColorByScore = new SolidColorBrush(ColorConverter.HexToColor("#8db923").Value);

                    }; break;
                case 1:
                    {
                        ColorByScore = new SolidColorBrush(ColorConverter.HexToColor("#eea466").Value);
                        SecondColorByScore = new SolidColorBrush(ColorConverter.HexToColor("#f57949").Value);

                    }; break;
                case 2:
                    {
                        ColorByScore = new SolidColorBrush(ColorConverter.HexToColor("#e56060").Value);
                        SecondColorByScore = new SolidColorBrush(ColorConverter.HexToColor("#c00404").Value);

                    }; break;
            }
        }

        /* <summary>
                一个例子：

        植物A：
	        - 喜阳植物（100-20000lux）
	        - 环境温度（25~30摄氏度）
	        - 土壤湿度（1~1）
	        - 环境湿度（60~80%）

        当前记录的数据：
	        - 光照强度200lux
	        - 温度20摄氏度
	        - 土壤湿度为1
	        - 环境湿度70%
        
            一般来说，光照强度和温度是影响植物的最重要因素，因此这2个值占总分（100分）的比例一共为70%。
            土壤湿度和环境湿度各占15%。总分数从0开始，每满足一些条件就加分。

            注意的是，计算当前植物的得分不应该通过最后一个数据来计算，得通过数据所在的最后一天的所有数据计算，
            树莓派端的默认上传数据频率为1小时一次，因此一天下来最少有24条记录，但是用户可能会选择2小时从传一次数据，
            因此不能默认为24条数据来处理。

	            - 土壤湿度：一天24小时的数据之间，有1次测量到的土壤湿度为1则加15分，2次加10分，3次加6分，
                4次加3分，若超过5次，则1分（事实上太多水肯定会不好的）；
	            - 环境湿度：设满足60~80%的数据有x个，24小时之内的数据有y个，那么得分为 15*(x/y) ；
	            - 温度：日间（7~18），满足25~30摄氏度的有x个，日间总数y个，那么得分为20*(x/y)，
	            夜间（18~7），满足最低25摄氏度+2/-5的有x2个，夜间总数y2个，那么得分为20*(x2/y2)；
	            - 光照强度：设满足光照强度的数据有x个，总数y个，那么比例i=x/y，
                如果x/y落在70~100%之间，得分为30*i，如果x/y落在40~70%之间，得分为20*i，x/y落在40%一下，得分为10*i；
        */
        private void CalculateScoreAndUpdate()
        {
            var score = 0d;

            //SoilMoisture
            var wetCount = CurrentPlan.RecordData.Select(s => s.SoilMoisture == 1).Count();
            if (wetCount == 1) score += 15;
            else if (wetCount == 2) score += 10;
            else if (wetCount == 3) score += 6;
            else if (wetCount == 4) score += 3;
            else score += 1;

            //EnviMoisture
            var enviMoistureRange = CurrentPlan.CurrentPlant.EnviMoistureRange;
            var emcount = CurrentPlan.RecordData.Select(s =>
            {
               return s.EnviMoisture <= enviMoistureRange.Y && s.EnviMoisture >= enviMoistureRange.X;
            }).Count();
            var emi = emcount / (double)CurrentPlan.RecordData.Count;
            score += 15 * emi;

            //EnviTemp
            var dayData = from e in CurrentPlan.RecordData where e.RecordTime.Hour >= 7 && e.RecordTime.Hour <= 16 select e;
            
            var nightData = from e in CurrentPlan.RecordData where e.RecordTime.Hour >= 16 && e.RecordTime.Hour <= 7 select e;

            var tempRange = CurrentPlan.CurrentPlant.EnviTempRange;

            var dayGoodData = (from e in dayData where e.EnviTemp <= tempRange.Y && e.EnviTemp >= tempRange.X select e).ToList();
            var nightGoodData = (from e in nightData where e.EnviTemp <= tempRange.X+2 && e.EnviTemp >= tempRange.X-5 select e).ToList();

            if (dayData.Count() != 0)
            {
                score += 20 * (dayGoodData.Count() / (double)dayData.Count());
            }
            else score += 10;
            if (nightData.Count() != 0)
            {
                score += 20 * (nightGoodData.Count() / (double)nightData.Count());
            }
            else score += 10;

            //Light
            var lightRange = GetLightRange();
            var goodLightDataCount= CurrentPlan.RecordData.Select(s =>
            {
                return s.Light <= lightRange.Y && s.Light >= lightRange.X;
            }).Count();
            var ratio = goodLightDataCount / (double)CurrentPlan.RecordData.Count;
            if (ratio >= 0.7) score += 30 * ratio;
            else if (ratio >= 0.4 && ratio < 0.7) score += 20 * ratio;
            else score += 10 * ratio;

            this.ScoreValue = (int)score;
        }

        private Vector2 GetLightRange()
        {
            if(CurrentPlan.CurrentPlant.LikeSunshine)
            {
                return new Vector2(100, 20000);
            }
            else return new Vector2(1, 10000);
        }
    }
}
