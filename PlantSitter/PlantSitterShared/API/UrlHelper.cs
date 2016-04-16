using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitterShared.API
{
    public static class UrlHelper
    {
        public const string HOST = "juniperphoton.net";

        public static string CheckUserExist  => $"http://{HOST}/plantsitter/User/CheckUserExist/v1?";
        public static string GetSalt => $"http://{HOST}/plantsitter/User/GetSalt/v1?";
        public static string Register => $"http://{HOST}/plantsitter/User/Register/v1?";
        public static string Login => $"http://{HOST}/plantsitter/User/Login/v1?";
        public static string UploadData => $"http://{HOST}/plantsitter/Timeline/UploadData/v1?";
        public static string GetTimelineData => $"http://{HOST}/plantsitter/Timeline/GetTimelineData/v1?";
        public static string GetAllPlans => $"http://{HOST}/plantsitter/Plan/GetAllPlans/v1?";
        public static string GetPlan => $"http://{HOST}/plantsitter/Plan/GetPlan/v1?";
        public static string DeletePlan => $"http://{HOST}/plantsitter/Plan/DeletePlan/v1?";
        public static string AddPlan => $"http://{HOST}/plantsitter/Plan/AddPlan/v1?";
        public static string GetPlantInfo => $"http://{HOST}/plantsitter/Plant/GetPlantInfo/v1?";
        public static string AddPlant => $"http://{HOST}/plantsitter/Plant/AddPlant/v1?";
        public static string SearchPlant => $"http://{HOST}/plantsitter/Plant/SearchPlant/v1?";

        public static string MakeFullUrlForGetReq(string baseUrl, List<KeyValuePair<string, string>> paramList,bool withAuth)
        {
            StringBuilder sb = new StringBuilder(baseUrl);
            foreach (var item in paramList)
            {
                sb.Append(item.Key + "=" + item.Value + "&");
            }
            if(withAuth)
            {
                sb.Append("&uid=" + LocalSettingHelper.GetValue("uid"));
                sb.Append("&access_token=" + LocalSettingHelper.GetValue("access_token"));
            }
            return sb.ToString();
        }

        public static string MakeFullUrlForPostReq(string baseUrl,bool withAuth)
        {
            StringBuilder sb = new StringBuilder(baseUrl);
            if (withAuth)
            {
                sb.Append("&uid=" + LocalSettingHelper.GetValue("uid"));
                sb.Append("&access_token=" + LocalSettingHelper.GetValue("access_token"));
            }
            return sb.ToString();
        }
    }
}
