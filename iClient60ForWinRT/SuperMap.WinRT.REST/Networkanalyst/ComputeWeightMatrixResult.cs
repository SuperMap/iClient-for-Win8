using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_ComputeWeightMatrixResult_Title}</para>
    /// 	<para>${REST_ComputeWeightMatrixResult_Description}</para>
    /// </summary>
    public class ComputeWeightMatrixResult
    {
        internal ComputeWeightMatrixResult()
        { }
        /// <summary>${REST_ComputeWeightMatrixResult_attribute_WeightMatrix_D}</summary>
        public List<List<double>> WeightMatrix { get; private set; }

        /// <summary>${REST_ComputeWeightMatrixResult_method_fromJson_D}</summary>
        /// <returns>${REST_ComputeWeightMatrixResult_method_fromJson_return}</returns>
        /// <param name="json">${REST_ComputeWeightMatrixResult_method_fromJson_param_jsonObject}</param>
        internal static ComputeWeightMatrixResult FromJson(JsonArray json)
        {
            if (json == null)
                return null;

            ComputeWeightMatrixResult result = new ComputeWeightMatrixResult();
            if (json.Count > 0)
            {
                result.WeightMatrix = new List<List<double>>();
                for (int i = 0; i < json.Count; i++)
                {
                    List<double> list = new List<double>();
                    for (int j = 0; j < json[i].GetArray().Count; j++)
                    {
                        list.Add(json[i].GetArray()[j].GetNumberEx());
                    }
                    result.WeightMatrix.Add(list);
                }
            }

            return result;
        }
    }
}
