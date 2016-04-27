using JP.API;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;

namespace PlantSitterShared.API
{
    public static class CloudService
    {
        /// <summary>
        /// 获得默认的参数，
        /// 带上 a 是为了让每次服务器返回来都是最新的，也就是没有缓存的
        /// </summary>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> GetDefaultParam()
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("a", new Random().Next().ToString()));
            return param;
        }

        /// <summary>
        /// 获得默认的参数
        /// 将会返回带 UID AccessToken 的
        /// </summary>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> GetDefaultParamWithAuthParam()
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("a", new Random().Next().ToString()));
            param.Add(new KeyValuePair<string, string>("uid", LocalSettingHelper.GetValue("uid")));
            param.Add(new KeyValuePair<string, string>("access_token", LocalSettingHelper.GetValue("access_token")));

            return param;
        }

        #region User interface
        /// <summary>
        /// 获得”盐”
        /// </summary>
        /// <param name="email">电子邮件</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> GetSalt(string email, CancellationToken token)
        {
            var param = GetDefaultParam();
            param.Add(new KeyValuePair<string, string>("email", email));
            return await APIHelper.SendGetRequestAsync(
                UrlHelper.MakeFullUrlForGetReq(UrlHelper.GetSalt, param, false), token);
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="email">邮件</param>
        /// <param name="token"></param>
        /// <returns>返回值将带有 IsExist 参数</returns>
        public static async Task<CommonRespMsg> CheckUserExist(string email, CancellationToken token)
        {
            var param = GetDefaultParam();
            param.Add(new KeyValuePair<string, string>("email", email));
            return await APIHelper.SendGetRequestAsync(
                UrlHelper.MakeFullUrlForGetReq(UrlHelper.CheckUserExist, param, false), token);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="email">电子邮件</param>
        /// <param name="passwordInMd5">MD5 后的密码</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> Register(string email, string passwordInMd5, CancellationToken token)
        {
            var param = GetDefaultParam();
            param.Add(new KeyValuePair<string, string>("email", email));
            param.Add(new KeyValuePair<string, string>("password", passwordInMd5));

            return await APIHelper.SendPostRequestAsync(UrlHelper.MakeFullUrlForPostReq(UrlHelper.Register, false), param, token);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="email">电子邮件</param>
        /// <param name="passwordSaltInMd5">MD5后的密码拼接“盐”再MD5的值</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> Login(string email, string passwordSaltInMd5, CancellationToken token)
        {
            var param = GetDefaultParam();
            param.Add(new KeyValuePair<string, string>("email", email));
            param.Add(new KeyValuePair<string, string>("password", passwordSaltInMd5));

            return await APIHelper.SendPostRequestAsync(UrlHelper.MakeFullUrlForPostReq(UrlHelper.Login, false), param, token);
        }

        #endregion

        #region Timeline interface
        /// <summary>
        /// 上传记录的数据
        /// </summary>
        /// <param name="pid">植物ID</param>
        /// <param name="gid">培养计划ID</param>
        /// <param name="soilMoisture">土壤湿度，0/1 表示偏向干燥/湿润</param>
        /// <param name="enviTemp">环境温度，单位为摄氏度 ℃</param>
        /// <param name="enviMoisture">环境湿度，单位为 RH</param>
        /// <param name="light">光照强度，单位为流明 Lux</param>
        /// <param name="time">记录的时间</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> UploadData(int pid, int gid,
                    double soilMoisture, double enviTemp, double enviMoisture, double light, string time, CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            param.Add(new KeyValuePair<string, string>("pid", pid.ToString()));
            param.Add(new KeyValuePair<string, string>("gid", gid.ToString()));
            param.Add(new KeyValuePair<string, string>("soil_moisture", soilMoisture.ToString()));
            param.Add(new KeyValuePair<string, string>("envi_temp", enviTemp.ToString()));
            param.Add(new KeyValuePair<string, string>("envi_moisture", enviMoisture.ToString()));
            param.Add(new KeyValuePair<string, string>("light", light.ToString()));
            param.Add(new KeyValuePair<string, string>("time", time));

            return await APIHelper.SendPostRequestAsync(UrlHelper.MakeFullUrlForPostReq(UrlHelper.UploadData, true), param, token);
        }

        /// <summary>
        /// 获得记录的数据
        /// </summary>
        /// <param name="gid">计划ID</param>
        /// <param name="filterKind">筛选的类型，有 byNumber,byYears,byMonths,byDays,betweenDate</param>
        /// <param name="filterValue">根据筛选的类型，给予不同的值，比如：byNumber:10, byMonths:2,byYears:3,byDays:10,betweenDate: 2016-01-12~2016-03-20</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> GetTimelineData(int gid, string filterKind, string filterValue, CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            param.Add(new KeyValuePair<string, string>("gid", gid.ToString()));
            param.Add(new KeyValuePair<string, string>("filter_kind", filterKind));
            param.Add(new KeyValuePair<string, string>("filter_value", filterValue));

            var url = UrlHelper.MakeFullUrlForGetReq(UrlHelper.GetTimelineData, param, true);
            return await APIHelper.SendGetRequestAsync(url, token);
        }
        #endregion

        #region Plan interface
        /// <summary>
        /// 获得用户添加的所有计划
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> GetAllPlans(CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            var url = UrlHelper.MakeFullUrlForGetReq(UrlHelper.GetAllPlans, param, true);

            return await APIHelper.SendGetRequestAsync(url, token);
        }

        /// <summary>
        /// 获得计划详情
        /// </summary>
        /// <param name="gid">计划ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> GetPlan(int gid, CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            param.Add(new KeyValuePair<string, string>("gid", gid.ToString()));
            var url = UrlHelper.MakeFullUrlForGetReq(UrlHelper.GetPlan, param, true);

            return await APIHelper.SendGetRequestAsync(url, token);
        }

        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="gid">计划ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> DeletePlan(int gid, CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            param.Add(new KeyValuePair<string, string>("gid", gid.ToString()));
            var url = UrlHelper.MakeFullUrlForGetReq(UrlHelper.DeletePlan, param, true);

            return await APIHelper.SendGetRequestAsync(url, token);
        }

        /// <summary>
        /// 添加计划
        /// </summary>
        /// <param name="pid">植物ID</param>
        /// <param name="name">计划名字</param>
        /// <param name="time">创建的时间</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> AddPlan(int pid, string name, string time, CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            param.Add(new KeyValuePair<string, string>("pid", pid.ToString()));
            param.Add(new KeyValuePair<string, string>("name", name));
            param.Add(new KeyValuePair<string, string>("time", time));

            return await APIHelper.SendPostRequestAsync(UrlHelper.MakeFullUrlForPostReq(UrlHelper.AddPlan, true), param, token);
        }
        #endregion

        #region Plant interface
        /// <summary>
        /// 获得植物的信息
        /// </summary>
        /// <param name="pid">植物ID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> GetPlantInfo(int pid, CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            param.Add(new KeyValuePair<string, string>("pid", pid.ToString()));
            var url = UrlHelper.MakeFullUrlForGetReq(UrlHelper.GetPlantInfo, param, true);

            return await APIHelper.SendGetRequestAsync(url, token);
        }

        /// <summary>
        /// 搜索植物，
        /// 关键字是有优先级的，
        /// 优先搜索 ID，
        /// 然后英文名字，
        /// 再就是中文名字
        /// </summary>
        /// <param name="pid">植物ID</param>
        /// <param name="nameC">中文名字</param>
        /// <param name="nameE">英文名字</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> SearchPlant(int? pid, string nameC, string nameE, CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            if (pid != null)
            {
                param.Add(new KeyValuePair<string, string>("pid", pid.ToString()));
            }
            else if (nameE.IsNotNullOrEmpty())
            {
                param.Add(new KeyValuePair<string, string>("name_e", nameE));
            }
            else if (nameC.IsNotNullOrEmpty())
            {
                param.Add(new KeyValuePair<string, string>("name_c", nameC));
            }
            var url = UrlHelper.MakeFullUrlForGetReq(UrlHelper.SearchPlant, param, true);

            return await APIHelper.SendGetRequestAsync(url, token);
        }

        /// <summary>
        /// 添加植物信息到数据库
        /// 注意不应该被树莓派调用
        /// </summary>
        /// <param name="nameC">中文名字</param>
        /// <param name="nameE">英文名字</param>
        /// <param name="soilMoisture">土壤湿度范围，默认应该是 0~1 无单位</param>
        /// <param name="enviMoisture">环境湿度范围，比如 13~32 单位</param>
        /// <param name="enviTemp">环境温度范围 比如 34~45 单位摄氏度</param>
        /// <param name="light">光照强度范围，比如12~34 单位流明</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> AddPlant(string nameC, string nameE,
            string soilMoisture, string enviMoisture, string enviTemp, string light, string desc, string imgUrl, CancellationToken token)
        {
            var param = GetDefaultParamWithAuthParam();
            param.Add(new KeyValuePair<string, string>("name_c", nameC));
            param.Add(new KeyValuePair<string, string>("name_e", nameE));
            param.Add(new KeyValuePair<string, string>("desc", desc));
            param.Add(new KeyValuePair<string, string>("soil_moisture", soilMoisture));
            param.Add(new KeyValuePair<string, string>("envi_moisture", enviMoisture));
            param.Add(new KeyValuePair<string, string>("envi_temp", enviTemp));
            param.Add(new KeyValuePair<string, string>("light", light));
            param.Add(new KeyValuePair<string, string>("img_url", imgUrl));

            return await APIHelper.SendPostRequestAsync(UrlHelper.MakeFullUrlForPostReq(UrlHelper.AddPlant, true), param, token);
        }
        #endregion

        /// <summary>
        /// 搜索图片
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<CommonRespMsg> SearchImage(string query)
        {
            var url = $"{UrlHelper.SearchImage}q={query}";
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            request.Headers.Add(new KeyValuePair<string, string>("Ocp-Apim-Subscription-Key", UrlHelper.AppKey));
            return await APIHelper.SendRequest(request, CTSFactory.MakeCTS(10000).Token);
        }

        public static void ParseAPIResult(this CommonRespMsg result)
        {
            var json = JsonObject.Parse(result.JsonSrc);
            var isSuccess = JsonParser.GetBooleanFromJsonObj(json, "isSuccessed");
            result.IsSuccessful = isSuccess;
            result.ErrorCode = (int)JsonParser.GetNumberFromJsonObj(json, "error_code");
            result.ErrorMsg = JsonParser.GetStringFromJsonObj(json, "error_message");
        }
    }
}
