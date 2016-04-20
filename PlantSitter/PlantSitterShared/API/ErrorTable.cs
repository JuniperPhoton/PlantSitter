using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitterShared.API
{
    public static class ErrorTable
    {
        /*
        //common error define
        define('API_ERROR_DATABASE_ERROR', 100);
        define('API_ERROR_ACTION_NOTEXIST', 101);
        define('API_ERROR_ACCESS_TOKEN_INVAID', 102);

        //user
        define('USER_NOT_EXIST', 200);
        define('USER_ALEADY_EXIST', 201);
        define('EMAIL_OR_PWD_WRONG',202);

        //Plan
        define('PLAN_NOT_EXIST', 300);
        define('LACK_SOME_PARAMS', 301);

        //Timeline
        define('LACK_PLANT_PARAM',400);
        define('LACK_FILTER_VALUE',401);
        define('LACK_FILTER_KIND',402);
        define('TIME_FORMAT_WRONG',403);
        define('USER_NOT_EXIST',404);
        define('PLAN_NOT_EXIST',405);
        define('PLANT_NOT_EXIST',406);

        //Plant
        define('PLANT_NOT_EXIST',500);
    */
        private static Dictionary<int, string> ErrorDictionary = new Dictionary<int, string>()
        {
            {100,"数据库错误，请检查查询语句" },
            {101,"请求接口错误" },
            {102,"AccessToken 无效" },
            {200,"用户不存在" },
            {201,"邮件已经被注册了" },
            {202,"电子邮件或者密码错误" },
            {300,"培养计划不存在" },
            {301,"缺少一些参数" },
            {400,"缺少一些植物参数" },
            {401,"缺少筛选值" },
            {402,"缺少筛选类型" },
            {403,"时间格式不对" },
            {404,"用户不存在" },
            {405,"培养计划不存在" },
            {406,"植物不存在" },
            {500,"植物不存在" }
        };

        private static string KNOWN_ERROR_MSG => "未知错误";

        public static string GetMessageFromErrorCode(int errorCode)
        {
            if (ErrorDictionary.ContainsKey(errorCode))
            {
                return ErrorDictionary[errorCode];
            }
            else return KNOWN_ERROR_MSG;
        }
    }
}
