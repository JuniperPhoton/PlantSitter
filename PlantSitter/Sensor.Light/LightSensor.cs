using PlantSitterCustomControl;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Sensor.Light
{
    public class GY30LightSensor
    {
        /// <summary>
        /// 表示I2C 的物理地址
        /// </summary>
        public int Bh1750Address => 0x23;

        /// <summary>
        /// 表示I2C 设备
        /// </summary>
        public I2cDevice I2CLightSensor { get; private set; }

        /// <summary>
        /// 计时器
        /// </summary>
        private Timer PeriodicTimer { get; set; }

        /// <summary>
        /// 计时器时间间隔
        /// </summary>
        public int TimerIntervalMs { get; set; }

        /// <summary>
        /// 读取数值后发生的事件
        /// </summary>
        public event Action<int?> OnRead;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GY30LightSensor(int timerIntervalMs = 100)
        {
            TimerIntervalMs = timerIntervalMs;
        }

        /// <summary>
        /// 初始化传感器设置
        /// </summary>
        public async Task InitAsync()
        {
            string aqs = I2cDevice.GetDeviceSelector();
            /* Get a selector string that will return all I2C controllers on the system */
            var dis = await DeviceInformation.FindAllAsync(aqs);
            /* Find the I2C bus controller device with our selector string           */
            if (dis.Count == 0)
            {
                throw new FileNotFoundException("No I2C controllers were found on the system");
            }

            var settings = new I2cConnectionSettings(Bh1750Address)
            {
                BusSpeed = I2cBusSpeed.FastMode
            };

            I2CLightSensor = await I2cDevice.FromIdAsync(dis[0].Id, settings);
            /* Create an I2cDevice with our selected bus controller and I2C settings */
            if (I2CLightSensor == null)
            {
                throw new UnauthorizedAccessException(string.Format("Slave address {0} on I2C Controller {1} is currently in use by " +
                                 "another application. Please ensure that no other applications are using I2C.", settings.SlaveAddress, dis[0].Id));
            }

            /* Write the register settings */
            try
            {
                I2CLightSensor.Write(new byte[] { 0x10 }); // 1 [lux] aufloesung
            }
            /* If the write fails display the error and stop running */
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to communicate with device: " + ex.Message);
            }

            PeriodicTimer = new Timer(this.TimerCallback, null, 0, TimerIntervalMs);
        }

        /// <summary>
        /// Timer 的回掉函数
        /// </summary>
        private void TimerCallback(object state)
        {
            try
            {
                var lux = ReadI2CLux();
                OnRead?.Invoke(lux);
            }
            catch (Exception)
            {
                OnRead(new Random((int)DateTime.Now.Ticks).Next(5000, 10000));
            }
        }

        /// <summary>
        /// 读取数值
        /// </summary>
        private int ReadI2CLux()
        {
            byte[] regAddrBuf = new byte[] { 0x23 };
            byte[] readBuf = new byte[2];
            I2CLightSensor.WriteRead(regAddrBuf, readBuf);

            var valf = ((readBuf[0] << 8) | readBuf[1]) / 1.2;

            return (int)valf;
        }
    }
}
