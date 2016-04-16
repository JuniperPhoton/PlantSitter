using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitterShared.API
{
    public class APIException:Exception
    {
        public string ErrorMessage { get; set; } = "";

        public APIException(string message)
        {
            ErrorMessage = message;
        }

        public APIException()
        {

        }
    }
}
