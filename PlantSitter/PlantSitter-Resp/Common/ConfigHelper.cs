using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter_Resp.Common
{
    public static class ConfigHelper
    {
        public static bool IsLogin
        {
            get
            {
                if (LocalSettingHelper.HasValue("access_token") && LocalSettingHelper.HasValue("uid"))
                {
                    return true;
                }
                else return false;
            }
        }
    }
}
