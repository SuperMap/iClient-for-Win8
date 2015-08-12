using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Data.Json;

namespace SuperMap.WinRT.REST
{
    /// <summary>${REST_UGCVectorLayer_Title}</summary>
    public class UGCVectorLayer : UGCLayer
    {
        /// <summary>${REST_UGCVectorLayer_constructor_D}</summary>
        public UGCVectorLayer() { }
        /// <summary>${REST_UGCVectorLayer_attribute_Style_D}</summary>
        public ServerStyle Style { get; set; }
        /// <summary>${REST_UGCVectorLayer_method_FromJson_D}</summary>
        /// <returns>${REST_UGCVectorLayer_method_FromJson_return}</returns>
        /// <param name="json">${REST_UGCVectorLayer_method_FromJson_param_jsonObject}</param>
        internal static UGCVectorLayer FromJson(JsonObject json)
        {
            if (json == null) return null;
            UGCVectorLayer vectorLayer = new UGCVectorLayer();
            vectorLayer.Style = ServerStyle.FromJson(json);
            return vectorLayer;
        }
    }
}
