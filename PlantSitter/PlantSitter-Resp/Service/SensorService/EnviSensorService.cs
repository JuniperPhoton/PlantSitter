using Sensor.Env;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace PlantSitterResp.Service.SensorService
{
    public class EnviSensorService : ISensorService, IGPIOSensorService
    {
        public uint? GPIOPIN { get; set; }

        public EnviSensorService(uint? gpioPin)
        {
            this.GPIOPIN = gpioPin;
        }

        public async Task Init()
        {
            var sensor = new DhtSensor((int)GPIOPIN.Value);
            sensor.Init();
            sensor.OnRead += async (value) =>
              {
                  await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                   {
                       App.MainVM.TempTimelineData[0] = value.Humidity;
                       App.MainVM.TempTimelineData[1] = value.Temperature;
                   });
              };
        }

        public async Task InitWithNewPin(uint? gpioPin)
        {
            this.GPIOPIN = gpioPin;
            await Init();
        }
    }
}
