using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Core;
using System.Collections.Generic;
using Windows.Data.Json;
using  SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_GetLayersInfoResult_Title}</para>
    /// 	<para>${REST_GetLayersInfoResult_Description}</para>
    /// </summary>
    public class GetLayersInfoResult
    {
        internal GetLayersInfoResult()
        { }
        /// <summary>${REST_GetLayersInfoResult_attribute_LayersInfo_D}</summary>
        public IList<ServerLayer> LayersInfo { get; internal set; }

        /// <summary>${REST_GetLayersInfoResult_method_FromJson_D}</summary>
        /// <returns>${REST_GetLayersInfoResult_method_FromJson_return}</returns>
        /// <param name="strResult">${REST_GetLayersInfoResult_method_FromJson_param_jsonObject}</param>
        internal static GetLayersInfoResult FromJson(string strResult)
        {
            GetLayersInfoResult result = new GetLayersInfoResult();

            var json = JsonValue.Parse(strResult);
            if (json == null)
            {
                return null;
            }             

            List<ServerLayer> layers = new List<ServerLayer>();
            foreach (var layerJson in json.GetArray())
            {
                if (layerJson.GetObjectEx().ContainsKey("subLayers"))
                {
                    foreach (JsonValue item in layerJson.GetObjectEx()["subLayers"].GetObjectEx()["layers"].GetArray())
                    {
                        layers.Add(ServerLayer.FromJson(item.GetObjectEx()));
                    }
                }
            }
            result.LayersInfo = layers;
            return result;
        }
    }
}
