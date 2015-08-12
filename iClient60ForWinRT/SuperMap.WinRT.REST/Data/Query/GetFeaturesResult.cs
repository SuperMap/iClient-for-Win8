using SuperMap.WinRT.Utilities;
using SuperMap.WinRT.Core;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_GetFeaturesResult_Title}</para>
    /// 	<para>${REST_GetFeaturesResult_Description}</para>
    /// </summary>
    public class GetFeaturesResult
    {
        internal GetFeaturesResult()
        {
        }
        /// <summary>${REST_GetFeaturesResult_attribute_FeatureCount_D}</summary>
        public int FeatureCount { get; private set; }
        /// <summary>${REST_GetFeaturesResult_attribute_Features_D}</summary>
        public FeatureCollection Features { get; private set; }

        /// <summary>${REST_GetFeaturesResult_method_fromJson_D}</summary>
        /// <returns>${REST_GetFeaturesResult_method_fromJson_return}</returns>
        /// <param name="json">${REST_GetFeaturesResult_method_fromJson_param_jsonObject}</param>
        internal static GetFeaturesResult FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            GetFeaturesResult result = new GetFeaturesResult();
            result.FeatureCount = (int)json["featureCount"].GetNumberEx();
            if (result.FeatureCount < 1)
            {
                return null;
            }

            JsonArray features = json["features"].GetArray();
            if (features != null && features.Count > 0)
            {
                result.Features = new FeatureCollection();

                for (int i = 0; i < features.Count; i++)
                {
                    ServerFeature f = ServerFeature.FromJson(features[i].GetObjectEx());
                    result.Features.Add(f.ToFeature());
                }
            }

            return result;
        }
    }
}
