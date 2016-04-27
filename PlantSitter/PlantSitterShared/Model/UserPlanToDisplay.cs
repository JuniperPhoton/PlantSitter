using GalaSoft.MvvmLight;
using JP.Utils.UI;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace PlantSitterShared.Model
{
    public class UserPlanToDisplay:ViewModelBase
    {
        public UserPlan CurrentPlan { get; set; }

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
                        UpdateColorByScoreLevel(0);
                    }
                    else if(value<70 && value>=40)
                    {
                        ScoreSumUp = "不太适宜";
                        UpdateColorByScoreLevel(1);
                    }
                    else 
                    {
                        ScoreSumUp = "非常糟糕";
                        UpdateColorByScoreLevel(2);
                    }
                }
            }
        }

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

        public UserPlanToDisplay(UserPlan plan)
        {
            this.CurrentPlan = plan;
            ColorByScore = new SolidColorBrush(ColorConverter.HexToColor("#71997b").Value);
            SecondColorByScore = new SolidColorBrush(ColorConverter.HexToColor("#8db923").Value);
            var random = new Random((int)DateTime.Now.Ticks);
            this.ScoreValue = random.Next(1, 100);
        }

        public async Task FetchAndUpdateScore()
        {

        }

        private void UpdateColorByScoreLevel(int level)
        {
            switch(level)
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
    }
}
