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

        public static string CheckUserExist = $"http://{HOST}/plantsitter/User/CheckUserExist/v1?";
        public static string GetSalt = $"http://{HOST}/plantsitter/User/GetSalt/v1?";
        public static string Register = $"http://{HOST}/plantsitter/User/Register/v1?";
        public static string Login = $"http://{HOST}/plantsitter/User/Login/v1?";

        public static string MakeFullUrlForGetReq(string baseUrl, List<KeyValuePair<string, string>> paramList)
        {
            StringBuilder sb = new StringBuilder(baseUrl);
            foreach (var item in paramList)
            {
                sb.Append(item.Key + "=" + item.Value + "&");
            }
            sb.Append("a=" + new Random().Next().ToString());
            return sb.ToString();
        }
    }
}
