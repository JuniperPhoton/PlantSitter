using PlantSitterResp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PlantSitter.Service
{
    public class DateTimeService
    {
        private DispatcherTimer _timer;

        public DateTimeService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            App.MainVM.CurrentDate = $"今天：{DateTime.Now.ToString()}";
        }
    }
}
