using PlantSitterResp;
using PlantSitterResp.Common;
using PlantSitterShared.Model;
using PlantSitterShared.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using JP.Utils.Functions;
using System.Diagnostics;

namespace PlantSitter.Service
{
    public class UploadService
    {
        private DispatcherTimer _timer;

        public UploadService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = GetTimeInMillisecondFromSettings();
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private async void _timer_Tick(object sender, object e)
        {
            if (App.MainVM.UserPlanVM.SelectedPlan != null)
            {
                var tempdata = App.MainVM.UserPlanVM.SelectedPlan.RecordData[0];
                if (tempdata != null)
                {
                    var result = await CloudService.UploadData(tempdata.CurrentPlant.Pid, tempdata.Gid, tempdata.SoilMoisture,
                            tempdata.EnviTemp, tempdata.EnviMoisture, tempdata.Light, tempdata.RecordTime.GetDateTimeIn24Format(), CTSFactory.MakeCTS().Token);
                    result.ParseAPIResult();

                    Debug.WriteLine(result.ToString());
                }
            }
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
