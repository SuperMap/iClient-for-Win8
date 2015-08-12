using System.Collections.Generic;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_EditFeaturesResult_Title}</para>
    /// 	<para>${REST_EditFeaturesResult_Description}</para>
    /// </summary>
    public class EditFeaturesResult
    {
        internal EditFeaturesResult()
        {
        }

        /// <summary>${REST_EditFeaturesResult_attribute_Succeed_D}</summary>
        public bool Succeed { get; internal set; }
        /// <summary>${REST_EditFeaturesResult_attribute_NewResourceLocation_D}</summary>
        public string NewResourceLocation { get; private set; }
        //public HttpError Error { get; private set; }
        /// <summary>${REST_EditFeaturesResult_attribute_IDs_D}</summary>
        public List<int> IDs { get; internal set; }

        /// <summary>${REST_EditFeaturesResult_method_FromJson_D}</summary>
        /// <returns>${REST_EditFeaturesResult_method_FromJson_return}</returns>
        /// <param name="json">${REST_EditFeaturesResult_method_FromJson_param_jsonObject}</param>
        internal static EditFeaturesResult FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            EditFeaturesResult result = new EditFeaturesResult();

            result.Succeed = json["succeed"].GetBooleanEx();
            //TODO:
            if (json.ContainsKey("newResourceLocation"))
            {
                result.NewResourceLocation = json["newResourceLocation"].GetStringEx();
            }
            //if (json.ContainsKey("error"))
            //{
            //    result.Error = HttpError.FromJson((JsonObject)json["error"]);
            //}

            return result;
        }


    }
}
