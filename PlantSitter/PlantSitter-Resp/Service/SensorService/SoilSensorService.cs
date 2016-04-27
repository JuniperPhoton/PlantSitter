using PlantSitterCustomControl;
using Sensor.Soil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace PlantSitterResp.Service.SensorService
{
    public class SoilSensorService : IGPIOSensorService, ISensorService
    {
        public uint? GPIOPIN { get; set; }

        public SoilSensorService(uint? pin)
        {
            GPIOPIN = pin;
        }

        public async Task InitWithNewPin(uint? gpioPin)
        {
            this.GPIOPIN = gpioPin;
            await Init();
        }

        public async Task Init()
        {
            if (GPIOPIN == null) throw new ArgumentNullException();

            try
            {
                SoilSensor soilSensor = new SoilSensor((int)GPIOPIN.Value);
                await soilSensor.InitAsync();
                soilSensor.OnRead += (async (value) =>
                {
                    await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        App.MainVM.TempTimelineData[2] = value;
                    });
                });
            }
            catch (Exception)
            {
                ToastService.SendToast("土壤传感器加载失败，请检查 GPIO 的设置。");
            }
        }
    }
}
