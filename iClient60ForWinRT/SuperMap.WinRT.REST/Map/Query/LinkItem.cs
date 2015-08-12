

using System.Collections.Generic;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_LinkItem_Tile}</para>
    /// 	<para>${REST_LinkItem_Description}</para>
    /// </summary>
    /// <remarks><para>${REST_LinkItem_remarks}</para></remarks>
    public class LinkItem
    {
        /// <summary>${REST_LinkItem_constructor_None_D}</summary>
        public LinkItem()
        { }

        /// <summary>${REST_LinkItem_attribute_DatasourceConnectionInfo_D}</summary>
        public DatasourceConnectionInfo DatasourceConnectionInfo { get; set; }

        /// <summary>${REST_LinkItem_attribute_PrimaryKeys_D}</summary>
        public IList<string> PrimaryKeys { get; set; }
        /// <summary>${REST_LinkItem_attribute_ForeignKeys_D}</summary>
        public IList<string> ForeignKeys { get; set; }
        /// <summary>${REST_LinkItem_attribute_LinkFields_D}</summary>
        public IList<string> LinkFields { get; set; }

        /// <summary>${REST_LinkItem_attribute_ForeignTable_D}</summary>
        public string ForeignTable { get; set; }
        /// <summary>${REST_LinkItem_attribute_LinkFilter_D}</summary>
        public string LinkFilter { get; set; }
        /// <summary>${REST_LinkItem_attribute_Name_D}</summary>
        public string Name { get; set; }

        internal static string ToJson(LinkItem param)
        {
            if (param == null)
            {
                return null;
            }
            string json = "{";
            List<string> list = new List<string>();

            list.Add(string.Format("\"datasourceConnectionInfo\":{0}", DatasourceConnectionInfo.ToJson(param.DatasourceConnectionInfo)));

            if (param.ForeignKeys != null && param.ForeignKeys.Count > 0)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < param.ForeignKeys.Count; i++)
                {
                    temp.Add(string.Format("\"{0}\"", param.ForeignKeys[i]));
                }
                list.Add(string.Format("\"foreignKeys\":[{0}]", string.Join(",", temp.ToArray())));
            }
            if (param.LinkFields != null && param.LinkFields.Count > 0)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < param.LinkFields.Count; i++)
                {
                    temp.Add(string.Format("\"{0}\"", param.LinkFields[i]));
                }
                list.Add(string.Format("\"linkFields\":[{0}]", string.Join(",", temp.ToArray())));
            }
            if (param.PrimaryKeys != null && param.PrimaryKeys.Count > 0)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < param.PrimaryKeys.Count; i++)
                {
                    temp.Add(string.Format("\"{0}\"", param.PrimaryKeys[i]));
                }
                list.Add(string.Format("\"primaryKeys\":[{0}]", string.Join(",", temp.ToArray())));
            }


            if (!string.IsNullOrEmpty(param.Name))
            {
                list.Add(string.Format("\"name\":\"{0}\"", param.Name));
            }
            else
            {
                list.Add("\"name\":null");
            }

            if (!string.IsNullOrEmpty(param.ForeignTable))
            {
                list.Add(string.Format("\"foreignTable\":\"{0}\"", param.ForeignTable));
            }
            else
            {
                list.Add("\"foreignTable\":null");
            }

            if (!string.IsNullOrEmpty(param.LinkFilter))
            {
                list.Add(string.Format("\"linkFilter\":\"{0}\"", param.LinkFilter));
            }
            else
            {
                list.Add("\"linkFilter\":null");
            }


            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }
    }
}
