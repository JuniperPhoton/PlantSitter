using PlantSitterResp;
using PlantSitterResp.Common;
using PlantSitterShard.Model;
using PlantSitterShared.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlantSitter_Resp.Service
{
    public class UploadService
    {
        private DispatcherTimer _timer;

        private PlantTimeline TempTimeline { get; set; }

        public UploadService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = GetTimeInMillisecondFromSettings();
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private async void _timer_Tick(object sender, object e)
        {
            var result = await CloudService.UploadData(TempTimeline.CurrentPlant.Pid, TempTimeline.Gid, TempTimeline.SoilMoisture,
                TempTimeline.EnviTemp, TempTimeline.EnviMoisture, TempTimeline.Light, DateTime.Now.ToString("YYYY/MM/DD hh:mm:ss"), CTSFactory.MakeCTS().Token);
        }

        private TimeSpan GetTimeInMillisecondFromSettings()
        {
            switch (App.AppSettings.UploadFequency)
            {
                case 0:
                    {
                        return TimeSpan.FromMinutes(5);
                    }
                case 1:
                    {
                        return TimeSpan.FromMinutes(15);
                    }
                case 2:
                    {
                        return TimeSpan.FromMinutes(30);
                    }
                case 3:
                    {
                        return TimeSpan.FromHours(1);
                    }
                case 4:
                    {
                        return TimeSpan.FromHours(1.5);
                    }
                case 5:
                    {
                        return TimeSpan.FromHours(2);
                    }
                case 6:
                    {
                        return TimeSpan.FromHours(3);
                    }
                default:
                    {
                        return TimeSpan.FromMinutes(30);
                    }
            }
        }
    }
}
