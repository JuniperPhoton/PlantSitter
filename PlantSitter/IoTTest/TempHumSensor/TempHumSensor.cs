using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace IoTTest
{
    public class TempHumSensor
    {
        int PIN_NUM = 7;
        GpioPin pin;
        string outputText = "";

        public void ReadValue()
        {
            var ok=InitGPIO();
            Debug.WriteLine(outputText);
            if(ok)
            {
                pin.DebounceTimeout = new TimeSpan(0, 0, 0, 0, 50);
                pin.SetDriveMode(GpioPinDriveMode.Input);
                pin.ValueChanged += Pin_ValueChanged;
                var value= pin.Read();
            }
        }

        private void Pin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            Debug.WriteLine("New pin value: " + args.Edge.ToString());
        }

        private bool InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin = null;
                outputText += "There is no GPIO controller on this device.";
                return false;
            }

            pin = gpio.OpenPin(PIN_NUM);

            // Show an error if the pin wasn't initialized properly
            if (pin == null)
            {
                outputText += "There were problems initializing the GPIO pin.";
                return false;
            }

            pin.Write(GpioPinValue.High);
            pin.SetDriveMode(GpioPinDriveMode.Output);

            outputText += "GPIO pin initialized correctly.";

            return true;
        }
    }
}
