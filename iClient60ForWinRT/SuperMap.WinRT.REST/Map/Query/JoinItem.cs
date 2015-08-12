using SuperMap.WinRT.Utilities;
using System.Collections.Generic;
using System;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_JoinItem_Tile}</para>
    /// 	<para>${REST_JoinItem_Description}</para>
    /// </summary>
    /// <remarks>${REST_LinkItem_remarks}</remarks>
    public class JoinItem
    {
        /// <summary>${REST_JoinItem_constructor_None_D}</summary>
        public JoinItem()
        { }

        /// <summary>${REST_JoinItem_attribute_ForeignTableName_D}</summary>
        public string ForeignTableName { get; set; }

        /// <summary>${REST_JoinItem_attribute_JoinFilter_D}</summary>
        public string JoinFilter { get; set; }

        /// <summary>${REST_JoinItem_attribute_JoinType_D}</summary>
        public JoinType JoinType { get; set; }


        internal static string ToJson(JoinItem param)
        {
            if (param == null)
            {
                return null;
            }

            string json = "{";
            List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(param.ForeignTableName))
            {
                list.Add(string.Format("\"foreignTableName\":\"{0}\"", param.ForeignTableName));
            }
            else
            {
                list.Add("\"foreignTableName\":null");
            }

            if (!string.IsNullOrEmpty(param.JoinFilter))
            {
                list.Add(string.Format("\"joinFilter\":\"{0}\"", param.JoinFilter));
            }
            else
            {
                list.Add("\"joinFilter\":null");
            }

            list.Add(string.Format("\"joinType\":\"{0}\"", param.JoinType.ToString()));

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static JoinItem FromJson(JsonObject json)
        {
            if (json == null) return null;
            JoinItem item = new JoinItem();
            if (json.ContainsKey("foreignTableName"))
            {
                item.ForeignTableName = json["foreignTableName"].GetStringEx();
            }
            if (json.ContainsKey("joinFilter"))
            {
                item.JoinFilter = json["joinFilter"].GetStringEx();
            }
            if (json.ContainsKey("joinType") && !string.IsNullOrEmpty(json["joinType"].GetStringEx()))
            {
                item.JoinType = (JoinType)Enum.Parse(typeof(JoinType), json["joinType"].GetStringEx(), true);
            }
            return item;
        }
    }
}
