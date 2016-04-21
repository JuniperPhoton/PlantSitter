using PlantSitterResp;
using PlantSitterShard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlantSitter_Resp.Service
{
    public class DisplayService
    {
        private DispatcherTimer _timer;

        private PlantTimeline TempTimeline { get; set; }

        public DisplayService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            if (App.MainVM.UserPlanVM.SelectedPlan != null)
            {
                App.MainVM.UserPlanVM.SelectedPlan.RecordData.Insert(0, TempTimeline);
            }
        }
    }
}
