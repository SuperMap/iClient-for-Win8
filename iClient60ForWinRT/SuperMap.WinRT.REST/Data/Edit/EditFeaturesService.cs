

using System;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Service;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
using System.Threading.Tasks;

namespace SuperMap.WinRT.REST.Data
{
    /// <summary>
    /// 	<para>${REST_EditFeaturesService_Title}</para>
    /// 	<para>${REST_EditFeaturesService_Description}</para>
    /// </summary>
    public class EditFeaturesService : ServiceBase
    {
        /// <summary>${REST_EditFeaturesService_constructor_D}</summary>
        /// <overloads>${REST_ComputeWeightMatrixService_constructor_overloads_D}</overloads>
        public EditFeaturesService()
        { }

        /// <summary>${REST_ComputeWeightMatrixService_constructor_String_D}</summary>
        /// <param name="url">${REST_EditFeaturesService_constructor_param_url}</param>
        public EditFeaturesService(string url)
            : base(url)
        {
        }

        /// <summary>${REST_EditFeaturesService_method_ProcessAsync_D}</summary>
        /// <param name="parameters">${REST_EditFeaturesService_method_ProcessAsync_param_Parameters}</param>
        /// <param name="state">${REST_EditFeaturesService_method_ProcessAsync_param_state}</param>
        public async Task<EditFeaturesResult> ProcessAsync(EditFeaturesParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
            }
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new InvalidOperationException(ExceptionStrings.InvalidUrl);
            }

            if (parameters.EditType == EditType.ADD)
            {
                base.Url += ".json?returnContent=true&debug=true";//直接获取添加地物SmID
            }
            else if (parameters.EditType == EditType.DELETE)
            {
                base.Url += ".json?_method=DELETE&debug=true";
            }
            else
            {
                base.Url += ".json?_method=PUT&debug=true";
            }

            var jsonResult = await base.SubmitRequest(base.Url, GetParameters(parameters), true, true);
            EditFeaturesResult result = null;
            if (JsonObject.Parse(jsonResult) is JsonObject)
            {
                JsonObject jsonObject = JsonObject.Parse(jsonResult);
                result = EditFeaturesResult.FromJson(jsonObject);

            }
            else
            {
                JsonArray json = JsonArray.Parse(jsonResult);
                result = new EditFeaturesResult();
                result.Succeed = true;
                result.IDs = new List<int>();
                for (int i = 0; i < json.Count; i++)
                {
                    result.IDs.Add((int)json[i].GetNumberEx());
                }
            }
            return result;
        }

        private System.Collections.Generic.Dictionary<string, string> GetParameters(EditFeaturesParameters parameters)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (parameters.EditType == EditType.DELETE)
            {
                if (parameters.IDs != null)
                {
                    string ids = string.Empty;
                    List<string> idslist = new List<string>();
                    for (int k = 0; k < parameters.IDs.Count; k++)
                    {
                        //idslist.Add(parameters.IDs[k].ToString());
                        dictionary.Add(k.ToString(), parameters.IDs[k].ToString());
                    }
                    //ids += string.Join(",", idslist.ToArray());
                    //dictionary.Add("Edit", ids);
                }
                else
                {
                    throw new ArgumentNullException(ExceptionStrings.ArgumentIsNull);
                }
            }
            else
            {
                if (parameters.Features != null && parameters.Features.Count > 0)
                {
                    string json = "";
                    List<string> list = new List<string>();

                    for (int i = 0; i < parameters.Features.Count; i++)
                    {
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        ServerFeature sf = parameters.Features[i].ToServerFeature();

                        //FiledNames
                        dict.Add("fieldNames", JsonHelper.FromIList(sf.FieldNames));

                        //FiledValues
                        dict.Add("fieldValues", JsonHelper.FromIList(sf.FieldValues));

                        //Geometry
                        if (parameters.EditType == EditType.UPDATA)
                        {
                            //更新时，ID的个数和Feature的个数不一致更新就不起作用！
                            if (parameters.IDs.Count == parameters.Features.Count)
                            {
                                if (sf.Geometry != null)
                                {
                                    sf.Geometry.ID = parameters.IDs[i];
                                }
                                else
                                {
                                    sf.ID = parameters.IDs[i];
                                }

                            }
                        }

                        dict.Add("ID", sf.ID.ToString());
                        dict.Add("geometry", sf.Geometry == null ? "null" : ServerGeometry.ToJson(sf.Geometry));

                        list.Add(EditGetStringFromDict(dict));
                    }


                    json += string.Join(",", list.ToArray());
                    dictionary.Add("Edit", json);
                }
            }

            return dictionary;
        }
        private static string EditGetStringFromDict(Dictionary<string, string> parameters)
        {
            string json = "{";
            List<string> list = new List<string>();
            foreach (string key in parameters.Keys)
            {
                list.Add(string.Format("\"{0}\":{1}", new object[] { key, parameters[key] }));
            }
            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        //private void request_Completed(object sender, RequestEventArgs e)
        //{
        //    EditFeaturesResult result = null;
        //    if (JsonObject.Parse(e.Result) is JsonObject)
        //    {
        //        JsonObject jsonObject = JsonObject.Parse(e.Result);
        //        result = EditFeaturesResult.FromJson(jsonObject);

        //    }
        //    else
        //    {
        //        JsonArray json = JsonArray.Parse(e.Result);
        //        result = new EditFeaturesResult();
        //        result.Succeed = true;
        //        result.IDs = new List<int>();
        //        for (int i = 0; i < json.Count; i++)
        //        {
        //            result.IDs.Add((int)json[i].GetNumberEx());
        //        }
        //    }

        //    LastResult = result;
        //    EditFeaturesEventArgs args = new EditFeaturesEventArgs(result, e.Result, e.UserState);
        //    OnProcessCompleted(args);

        //}

        //private void OnProcessCompleted(EditFeaturesEventArgs args)
        //{
        //    if (ProcessCompleted != null)
        //    {
        //        this.ProcessCompleted(this, args);
        //    }
        //}

        ///// <summary>${REST_EditFeaturesService_event_processCompleted_D}</summary>

        //public event EventHandler<EditFeaturesEventArgs> ProcessCompleted;

        //private EditFeaturesResult lastResult;
        ///// <summary>${REST_EditFeaturesService_attribute_lastResult_D}</summary>
        //public EditFeaturesResult LastResult
        //{
        //    get
        //    {
        //        return lastResult;
        //    }
        //    private set
        //    {
        //        lastResult = value;
        //        base.OnPropertyChanged("LastResult");
        //    }
        //}
    }
}
