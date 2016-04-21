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

        public event Action<double> OnReadingValue;

        private DispatcherTimer _dispatcherTimer { get; set; }
        private int? _pinNumber;

        public async Task InitAsync()
        {
            if (_pinNumber == null) throw new ArgumentNullException("Must specify a Pin Number.");

            var ctl = await GpioController.GetDefaultAsync();
            MoistureSensorOutputPin = ctl?.OpenPin(_pinNumber.Value);
            if (MoistureSensorOutputPin != null)
            {
                MoistureSensorOutputPin.SetDriveMode(GpioPinDriveMode.Input);
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
                _dispatcherTimer.Tick += (sender, e) =>
                {
                    var pinValue = MoistureSensorOutputPin.Read();
                    if (pinValue == GpioPinValue.High)
                    {
                        OnReadingValue?.Invoke(0);
                    }
                    else
                    {
                        OnReadingValue?.Invoke(1);
                    }
                };
                _dispatcherTimer.Start();
            }
        }

        public SoilSensor(int pinNumber)
        {
            _pinNumber = pinNumber;
        }
    }
}
