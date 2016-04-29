using GalaSoft.MvvmLight;
using JP.Utils.UI;
using PlantSitterShared.API;
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

        public UserPlanWrapped(UserPlan plan)
        {
            this.CurrentPlan = plan;

            var random = new Random((int)DateTime.Now.Ticks);
            this.ScoreValue = random.Next(1, 100);
        }

        public async Task FetchAndUpdateScore()
        {
            var records = await CloudService.GetTimelineData(this.CurrentPlan.Gid, "byNumber", "5", CTSFactory.MakeCTS().Token);
            var timelineData = PlantTimeline.ParseToList(records.JsonSrc);
            
            foreach(var item in timelineData)
            {
                CurrentPlan.RecordData.Add(item);
            }
        }

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

        */
        private void CalculateScoreAndUpdate()
        {
            //var lastRecord = CurrentPlan.RecordData.Last();
            //var score = 0;

            //var lightRange = GetLightRange();
            //var
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
