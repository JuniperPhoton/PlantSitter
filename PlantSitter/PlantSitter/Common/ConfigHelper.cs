using JP.Utils.Data;

namespace PlantSitter.Common
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
