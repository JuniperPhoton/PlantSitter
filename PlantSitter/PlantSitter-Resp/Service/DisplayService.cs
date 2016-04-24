using PlantSitterResp;
using System;
using System.Linq;
using Windows.UI.Xaml;
using JP.Utils.Functions;

namespace PlantSitter_Resp.Service
{
    public class DisplayService
    {
        private DispatcherTimer _timer;

        public DisplayService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            if (App.MainVM.UserPlanVM.SelectedPlan != null && App.MainVM.TempTimelineData != null)
            {
                App.MainVM.TempTimelineData.RecordTime = DateTime.Now;
                App.MainVM.UserPlanVM.SelectedPlan.RecordData.Insert(0, App.MainVM.TempTimelineData);
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[0] = App.MainVM.TempTimelineData;

                var timeline30MinAgo = App.MainVM.UserPlanVM.SelectedPlan.RecordData.ToList().Find((timeline) =>
                  {
                      var lastTime = timeline.RecordTime.GetDateTimeIn24Format();
                      var currentTime = DateTime.Now.AddMinutes(-1).GetDateTimeIn24Format();
                      if ( lastTime== currentTime)
                      {
                          return true;
                      }
                      else return false;
                  });
                var timeline1HourAgo = App.MainVM.UserPlanVM.SelectedPlan.RecordData.ToList().Find((timeline) =>
                {
                    if (timeline.RecordTime.GetDateTimeIn24Format() == DateTime.Now.AddMinutes(-2).GetDateTimeIn24Format())
                    {
                        return true;
                    }
                    else return false;
                });
                var timeline1HalfHourAgo = App.MainVM.UserPlanVM.SelectedPlan.RecordData.ToList().Find((timeline) =>
                {
                    if (timeline.RecordTime.GetDateTimeIn24Format() == DateTime.Now.AddMinutes(-3).GetDateTimeIn24Format())
                    {
                        return true;
                    }
                    else return false;
                });
                var timeline2HalfHourAgo = App.MainVM.UserPlanVM.SelectedPlan.RecordData.ToList().Find((timeline) =>
                {
                    if (timeline.RecordTime.GetDateTimeIn24Format() == DateTime.Now.AddMinutes(-4).GetDateTimeIn24Format())
                    {
                        return true;
                    }
                    else return false;
                });

                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[1] = timeline30MinAgo;
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[2] = timeline1HourAgo;
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[3] = timeline1HalfHourAgo;
                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[4] = timeline2HalfHourAgo;

                App.MainVM.UserPlanVM.SelectedPlan.RaiseTimelineChanged();
            }
        }
    }
}
