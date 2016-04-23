using GPIOSensor.Common;
using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;

namespace Sensor.Env
{
    public class DhtSensor : IGPIOSensor, ISensorDisposable
    {
        private DispatcherTimer _timer;
        private IDht _dht = null;
        private GpioPin _gpioPin;

        public int GpioPinNumber { get; set; }

        public event Action<DhtReadingResult> OnRead;

        public DhtSensor(int gpioPin)
        {
            this.GpioPinNumber = gpioPin;
        }

        public void Init()
        {
            _gpioPin = GpioController.GetDefault().OpenPin(GpioPinNumber, GpioSharingMode.Exclusive);
            _dht = new Dht11(_gpioPin, GpioPinDriveMode.Input);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += _timer_Tick; ;
            _timer.Start();
        }

        private async void _timer_Tick(object sender, object e)
        {
            var readingResult = new DhtReadingResult();

            readingResult = await _dht.GetReadingAsync().AsTask();

            if (readingResult.IsValid)
            {
                OnRead?.Invoke(readingResult);
            }
            else
            {

            }
        }

        public void Dispose()
        {
            _gpioPin.Dispose();
            _timer?.Stop();
            _gpioPin = null;
            _dht = null;
        }
    }
}
