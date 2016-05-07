using PlantSitterCustomControl;
using Sensor.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace PlantSitterResp.Service.SensorService
{
    public class LightSensorService : ISensorService
    {
        public LightSensorService()
        {

        }

        public async Task Init()
        {
            try
            {
                var sensor = new GY30LightSensor();
                await sensor.InitAsync();
                sensor.OnRead += async (value) =>
                {
                    await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                     {
                         App.MainVM.TempTimelineData[3] = value.Value;
                     });
                };
            }
            catch (Exception)
            {
                ToastService.SendToast("光线传感器加载失败，请检查 GPIO 设置。");
            }
        }
    }
}
