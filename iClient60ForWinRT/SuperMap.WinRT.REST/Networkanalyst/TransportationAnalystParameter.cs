

using System.Collections.Generic;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
namespace SuperMap.WinRT.REST.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${REST_TransportationAnalystParameter_Title}</para>
    /// 	<para>${REST_TransportationAnalystParameter_Description}</para>
    /// </summary>
    /// <remarks>${REST_TransportationAnalystParameter_Remarks}</remarks>
    public class TransportationAnalystParameter
    {
        /// <summary>${REST_TransportationAnalystParameter_constructor_D}</summary>
        public TransportationAnalystParameter()
        { }

        /// <summary>${REST_TransportationAnalystParameter_attribute_BarrierEdgeIDs_D}</summary>
        public IList<int> BarrierEdgeIDs { get; set; }
        /// <summary>${REST_TransportationAnalystParameter_attribute_BarrierNodeIDs_D}</summary>
        public IList<int> BarrierNodeIDs { get; set; }
        /// <summary>${REST_TransportationAnalystParameter_attribute_BarrierPoints_D}</summary>
        public IList<Point2D> BarrierPoints { get; set; }
        ///// <summary>${REST_TransportationAnalystParameter_attribute_MapParameter_D}</summary>
        //public NAResultMapParameter MapParameter { get; set; }
        /// <summary>${REST_TransportationAnalystParameter_attribute_ResultSetting_D}</summary>
        public TransportationAnalystResultSetting ResultSetting { get; set; }
        /// <summary>${REST_TransportationAnalystParameter_attribute_TurnWeightField_D}</summary>
        /// <remarks>
        /// 	<para>
        ///     ${REST_TransportationAnalystParameter_attribute_TurnWeightField_Remarks_1}</para>
        ///     <para><img src="turn.png"/></para>
        /// 	<para>
        ///     ${REST_TransportationAnalystParameter_attribute_TurnWeightField_Remarks_2}</para>
        ///     <para><img src="turnTable.jpg"/></para>
        /// </remarks>
        public string TurnWeightField { get; set; }
        /// <summary>${REST_TransportationAnalystParameter_attribute_WeightFieldName_D}</summary>
        public string WeightFieldName { get; set; }

        internal static string ToJson(TransportationAnalystParameter param)
        {
            if (param != null)
            {
                string json = "{";
                List<string> list = new List<string>();

                if (param.BarrierPoints != null && param.BarrierPoints.Count > 0)
                {
                    List<string> temp = new List<string>();
                    for (int i = 0; i < param.BarrierPoints.Count; i++)
                    {
                        temp.Add(JsonHelper.FromPoint2D(param.BarrierPoints[i]));
                    }
                    list.Add(string.Format("\"barrierPoints\":[{0}]", string.Join(",", temp.ToArray())));
                }

                if (param.BarrierEdgeIDs != null && param.BarrierEdgeIDs.Count > 0)
                {
                    List<string> temp = new List<string>();
                    foreach (int id in param.BarrierEdgeIDs)
                    {
                        temp.Add(id.ToString());
                    }
                    list.Add(string.Format("\"barrierEdgeIDs\":[{0}]", string.Join(",", temp.ToArray())));
                }

                if (param.BarrierNodeIDs != null && param.BarrierNodeIDs.Count > 0)
                {
                    List<string> temp = new List<string>();
                    foreach (int id in param.BarrierNodeIDs)
                    {
                        temp.Add(id.ToString());
                    }
                    list.Add(string.Format("\"barrierNodeIDs\":[{0}]", string.Join(",", temp.ToArray())));
                }

                //if (param.MapParameter != null)
                //    list.Add(string.Format("\"mapParameter\":{0}", NAResultMapParameter.ToJson(param.MapParameter)));

                if (param.ResultSetting != null)
                    list.Add(string.Format("\"resultSetting\":{0}", TransportationAnalystResultSetting.ToJson(param.ResultSetting)));

                if (param.TurnWeightField != null)
                    list.Add(string.Format("\"turnWeightField\":\"{0}\"", param.TurnWeightField));

                if (param.WeightFieldName != null)
                    list.Add(string.Format("\"weightFieldName\":\"{0}\"", param.WeightFieldName));

                json += string.Join(",", list.ToArray());
                json += "}";
                return json;
            }

            return null;
        }
    }
}
