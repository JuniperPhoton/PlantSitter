using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public static class Helper
    {
        public static void AddAuthInfo()
        {
            LocalSettingHelper.AddValue("uid", "3");
            LocalSettingHelper.AddValue("access_token", "d41d8cd98f00b204e9800998ecf8427e");
        }

        public static void DeleteAuthInfo()
        {
            LocalSettingHelper.DeleteValue("uid");
            LocalSettingHelper.DeleteValue("access_token");
        }
    }
}
