using JP.API;
using JP.Utils.Data.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace PlantSitterShared.API
{
    public static class CloudService
    {
        private static List<KeyValuePair<string, string>> GetDefaultParam()
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("a", new Random().Next().ToString()));
            return param;
        }

        public static async Task<CommonRespMsg> GetSalt(string email,CancellationToken token)
        {
            var param = GetDefaultParam();
            param.Add(new KeyValuePair<string, string>("email", email));
            return await APIHelper.SendGetRequestAsync(
                UrlHelper.MakeFullUrlForGetReq(UrlHelper.GetSalt, param),token);
        }

        public static async Task<CommonRespMsg> CheckUserExist(string email, CancellationToken token)
        {
            var param = GetDefaultParam();
            param.Add(new KeyValuePair<string, string>("email", email));
            return await APIHelper.SendGetRequestAsync(
                UrlHelper.MakeFullUrlForGetReq(UrlHelper.CheckUserExist, param), token);
        }

        public static async Task<CommonRespMsg> Register(string email,string passwordInMd5, CancellationToken token)
        {
            var param = GetDefaultParam();
            param.Add(new KeyValuePair<string, string>("email", email));
            param.Add(new KeyValuePair<string, string>("password", passwordInMd5));

            return await APIHelper.SendPostRequestAsync(UrlHelper.Register, param, token);
        }

        public static async Task<CommonRespMsg> Login(string email, string passwordSaltInMd5, CancellationToken token)
        {
            var param = GetDefaultParam();
            param.Add(new KeyValuePair<string, string>("email", email));
            param.Add(new KeyValuePair<string, string>("password", passwordSaltInMd5));

            return await APIHelper.SendPostRequestAsync(UrlHelper.Register, param, token);
        }

        public static void PhaseAPIResult(this CommonRespMsg result)
        {
            var json = JsonObject.Parse(result.JsonSrc);
            var isSuccess = JsonParser.GetBooleanFromJsonObj(json, "isSuccessed");
            result.IsSuccessful = isSuccess;
            result.ErrorCode = (int)JsonParser.GetNumberFromJsonObj(json, "error_code");
            result.ErrorMsg = JsonParser.GetStringFromJsonObj(json, "error_message");
        }
    }
}
