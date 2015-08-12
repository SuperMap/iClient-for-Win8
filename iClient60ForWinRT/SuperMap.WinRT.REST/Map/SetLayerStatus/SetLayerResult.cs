using Windows.Data.Json;
using SuperMap.WinRT.Utilities;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${iServer6_SetLayerStatusResult_Title}</para>
    /// 	<para>${iServer6_SetLayerStatusResult_Description}</para>
    /// </summary>
    public class SetLayerResult
    {
        /// <summary>${iServer6_SetLayerStatusResult_attribute_IsSucceed_D}</summary>
        public bool IsSucceed
        {
            get;
            internal set;
        }
        /// <summary>${iServer6_SetLayerStatusResult_attribute_NewResourceID_D}</summary>
        public string NewResourceID
        {
            get;
            internal set;
        }
        /// <summary>${iServer6_SetLayerStatusResult_method_fromJson_D}</summary>
        /// <returns>${iServer6_SetLayerStatusResult_method_fromJson_return}</returns>
        /// <param name="jsonResult">${iServer6_SetLayerStatusResult_method_fromJson_param_jsonObject}</param>
        internal static SetLayerResult FromJson(string jsonResult)
        {
            JsonObject resultObject = JsonObject.Parse(jsonResult);

            if (resultObject.ContainsKey("succeed"))
            {
                SetLayerResult setLayerStatusResult = new SetLayerResult
                {
                    IsSucceed = resultObject["succeed"].GetBooleanEx(),
                };
                return setLayerStatusResult;
            }
            return null;
        }
    }
}
