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
        /// <summary>
        /// 计时器
        /// </summary>
        private DispatcherTimer _dispatcherTimer { get; set; }
        private GpioPin _pin { get; set; }

        /// <summary>
        /// Pin 序号
        /// </summary>
        public int GpioPinNumber { get; set; }

        /// <summary>
        /// 读取到传感器数目的时候的事件
        /// </summary>
        public event Action<double> OnRead;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gpioPin">传入GPIO Pin 的序号</param>
        public SoilSensor(int gpioPin)
        {
            GpioPinNumber = gpioPin;
        }

        /// <summary>
        /// 初始化传感器设置
        /// </summary>
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

        /// <summary>
        /// 释放GPIO资源
        /// </summary>
        public void Dispose()
        {
            _pin.Dispose();
            _pin = null;
        }
    }
}
