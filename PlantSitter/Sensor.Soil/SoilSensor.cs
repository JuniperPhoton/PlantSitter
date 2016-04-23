using GPIOSensor.Common;
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
    public class SoilSensor : IGPIOSensor
    {
        private DispatcherTimer _dispatcherTimer { get; set; }
        private GpioPin _pin { get; set; }

        public int GpioPinNumber { get; set; }

        public event Action<double> OnRead;

        public SoilSensor(int gpioPin)
        {
            GpioPinNumber = gpioPin;
        }

        public async Task InitAsync()
        {
            var controller = await GpioController.GetDefaultAsync();
            _pin = controller?.OpenPin(GpioPinNumber);
            if (_pin != null)
            {
                _pin.SetDriveMode(GpioPinDriveMode.Input);
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
                _dispatcherTimer.Tick += (sender, e) =>
                {
                    var pinValue = _pin.Read();
                    if (pinValue == GpioPinValue.High)
                    {
                        OnRead?.Invoke(0);
                    }
                    else
                    {
                        OnRead?.Invoke(1);
                    }
                };
                _dispatcherTimer.Start();
            }
        }

        public void Dispose()
        {
            _pin.Dispose();
            _pin = null;
        }
    }
}
