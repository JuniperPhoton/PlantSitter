using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPIOSensor.Common
{
    public interface IGPIOSensor
    {
        int GpioPinNumber { get; set; }
    }
}
