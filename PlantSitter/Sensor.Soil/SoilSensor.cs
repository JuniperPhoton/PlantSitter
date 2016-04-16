using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;

namespace Sensor.Soil
{
    public class SoilSensor
    {
        public GpioPin MoistureSensorOutputPin { get; set; }

        public DispatcherTimer DispatcherTimer { get; set; }

        public async Task InitGpioPin(int pinNumber)
        {
            var ctl = await GpioController.GetDefaultAsync();
            MoistureSensorOutputPin = ctl?.OpenPin(pinNumber);
            if (MoistureSensorOutputPin != null)
            {
                MoistureSensorOutputPin.SetDriveMode(GpioPinDriveMode.Input);
                DispatcherTimer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                DispatcherTimer.Tick += (sender, e) =>
                {
                    var pinValue = MoistureSensorOutputPin.Read();
                    if (pinValue == GpioPinValue.High)
                    {
                        Debug.WriteLine("Dry");
                    }
                    else
                    {
                        Debug.WriteLine("Water Detected!");
                    }
                };
                DispatcherTimer.Start();
            }
        }
    }
}
