using System;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeResult_Title}</para>
    /// 	<para>${REST_ThemeResult_Description}</para>
    /// </summary>
    public class ThemeResult
    {
        internal ThemeResult()
        { 

        }

        /// <summary>${REST_ThemeResult_attribute_ResourceInfo_D}</summary>
        public ResourceInfo ResourceInfo
        {
            get;
            private set;
        }
        /// <summary>${REST_ThemeResult_method_FromJson_D}</summary>
        /// <returns>${REST_ThemeResult_method_FromJson_return}</returns>
        /// <param name="jsonResult">${REST_ThemeResult_method_FromJson_param_jsonObject}</param>
        internal static ThemeResult FromJson(string jsonResult)
        {
            JsonObject resultObject = JsonObject.Parse(jsonResult);
            ResourceInfo resourceInfo = new ResourceInfo
            {
                NewResourceID = resultObject["newResourceID"].GetStringEx(),
                NewResourceLocation = resultObject["newResourceLocation"].GetStringEx(),
                Succeed = resultObject["succeed"].GetBooleanEx()
            };

            ThemeResult themeResult = new ThemeResult
            {
                ResourceInfo = resourceInfo
            };
            return themeResult;
        }
    }
}
