using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitterResp.Service.SensorService
{
    public interface IGPIOSensorService
    {
        uint? GPIOPIN { get; set; }

        Task InitWithNewPin(uint? gpioPin);
    }
}
