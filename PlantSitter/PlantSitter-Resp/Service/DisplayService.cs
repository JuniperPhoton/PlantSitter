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
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            if (App.MainVM.UserPlanVM.SelectedPlan != null)
            {
                App.MainVM.UserPlanVM.SelectedPlan.RecordData.Insert(0, TempTimeline);
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[0] = TempTimeline;
                var timeline30MinAgo = App.MainVM.UserPlanVM.SelectedPlan.RecordData.ToList().Find((timeline) =>
                  {
                      if (timeline.RecordTime.ToString("YYYY/MM/DD HH::MM") == DateTime.Now.AddMinutes(-30).ToString("YYYY/MM/DD HH::MM"))
                      {
                          return true;
                      }
                      else return false;
                  });
                var timeline1HourAgo = App.MainVM.UserPlanVM.SelectedPlan.RecordData.ToList().Find((timeline) =>
                {
                    if (timeline.RecordTime.ToString("YYYY/MM/DD HH::MM") == DateTime.Now.AddHours(-1).ToString("YYYY/MM/DD HH::MM"))
                    {
                        return true;
                    }
                    else return false;
                });
                var timeline1HalfHourAgo = App.MainVM.UserPlanVM.SelectedPlan.RecordData.ToList().Find((timeline) =>
                {
                    if (timeline.RecordTime.ToString("YYYY/MM/DD HH::MM") == DateTime.Now.AddHours(-1.5).ToString("YYYY/MM/DD HH::MM"))
                    {
                        return true;
                    }
                    else return false;
                });
                var timeline2HalfHourAgo = App.MainVM.UserPlanVM.SelectedPlan.RecordData.ToList().Find((timeline) =>
                {
                    if (timeline.RecordTime.ToString("YYYY/MM/DD HH::MM") == DateTime.Now.AddHours(-2).ToString("YYYY/MM/DD HH::MM"))
                    {
                        return true;
                    }
                    else return false;
                });
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[0] = TempTimeline;
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[1] = timeline30MinAgo;
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[2] = timeline1HourAgo;
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[3] = timeline1HalfHourAgo;
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[4] = timeline2HalfHourAgo;
            }
        }
    }
}
