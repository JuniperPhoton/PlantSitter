using PlantSitterResp;
using System;
using System.Linq;
using Windows.UI.Xaml;
using JP.Utils.Functions;
using PlantSitterShared.Model;
using System.Diagnostics;

namespace PlantSitter_Resp.Service
{
    public class DisplayService
    {
        private DispatcherTimer _timer;
        private int _internal => 2;

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
                var recordData = App.MainVM.UserPlanVM.SelectedPlan.RecordData;
                var tempData = App.MainVM.TempTimelineData;

                var newTimeline = new PlantTimeline()
                {
                    RecordTime = DateTime.Now,
                    EnviMoisture = tempData[0],
                    EnviTemp = tempData[1],
                    SoilMoisture = tempData[2],
                    Light =tempData[3],
                    CurrentUser=App.MainVM.CurrentUser,
                    CurrentPlant=App.MainVM.UserPlanVM.SelectedPlan.CurrentPlant,
                    Gid=App.MainVM.UserPlanVM.SelectedPlan.Gid
                };

                recordData.Insert(0, newTimeline);

                App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[0] = newTimeline;

                PlantTimeline timeline30MinAgo = null;
                PlantTimeline timeline1HourAgo = null;
                PlantTimeline timeline1HalfHourAgo = null;
                PlantTimeline timeline2HourAgo = null;

                if (recordData.Count>=60/_internal)
                {
                    timeline30MinAgo = recordData.ElementAt((60 / _internal)-1);
                }
                if(recordData.Count>=3600/_internal)
                {
                    timeline1HourAgo = recordData.ElementAt((3600 / _internal)-1);
                }
                if(recordData.Count>=5400/_internal)
                {
                    timeline1HalfHourAgo = recordData.ElementAt((5400 / _internal) - 1);
                }
                if(recordData.Count>=7200/_internal)
                {
                    timeline1HalfHourAgo = recordData.ElementAt((7200 / _internal) - 1);
                }

                if (timeline30MinAgo!=null)
                    App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[1] = timeline30MinAgo;
                if(timeline1HourAgo!=null)
                    App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[2] = timeline1HourAgo;
                if(timeline1HalfHourAgo != null)
                    App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[3] = timeline1HalfHourAgo;
                if(timeline2HourAgo != null)
                    App.MainVM.UserPlanVM.SelectedPlan.TimelineDataToDisplay[4] = timeline2HourAgo;

                App.MainVM.UserPlanVM.SelectedPlan.RaiseTimelineChanged();

                //var debugStr = "";
                //foreach(var item in recordData)
                //{
                //    debugStr += item.Light + ",";
                //}
                //Debug.WriteLine(debugStr);
            }
        }
    }
}
