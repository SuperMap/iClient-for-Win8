

using System.Collections.Generic;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_FilterParameter_Tile}</para>
    /// 	<para>${REST_FilterParameter_Description}</para>
    /// </summary>
    public class FilterParameter
    {
        /// <summary>${REST_FilterParameter_constructor_None_D}</summary>
        public FilterParameter()
        { }

        /// <summary>${REST_FilterParameter_attribute_Name_D}</summary>
        public string Name { get; set; }

        /// <summary>${REST_FilterParameter_attribute_JoinItems_D}</summary>
        public IList<JoinItem> JoinItems { get; set; }

        /// <summary>${REST_FilterParameter_attribute_LinkItems_D}</summary>
        public IList<LinkItem> LinkItems { get; set; }

        /// <summary>${REST_FilterParameter_attribute_IDs_D}</summary>
        public IList<int> IDs { get; set; }

        /// <summary>${REST_FilterParameter_attribute_AttributeFilter_D}</summary>
        public string AttributeFilter { get; set; }

        /// <summary>${REST_FilterParameter_attribute_OrderBy_D}</summary>
        public string OrderBy { get; set; }

        /// <summary>${REST_FilterParameter_attribute_GroupBy_D}</summary>
        public string GroupBy { get; set; }

        /// <summary>${REST_FilterParameter_attribute_Fields_D}</summary>
        public IList<string> Fields { get; set; }

        internal static string ToJson(FilterParameter param)
        {
            if (param == null)
            {
                return null;
            }

            string json = "{";
            List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(param.Name))
            {
                list.Add(string.Format("\"name\":\"{0}\"", param.Name));
            }
            else
            {
                list.Add("\"name\":null");
            }

            if (!string.IsNullOrEmpty(param.AttributeFilter))
            {
                list.Add(string.Format("\"attributeFilter\":\"{0}\"", param.AttributeFilter));
            }
            else
            {
                list.Add("\"attributeFilter\":null");
            }
            if (!string.IsNullOrEmpty(param.OrderBy))
            {
                list.Add(string.Format("\"orderBy\":\"{0}\"", param.OrderBy));
            }
            else
            {
                list.Add("\"orderBy\":null");
            }
            if (!string.IsNullOrEmpty(param.GroupBy))
            {
                list.Add(string.Format("\"groupBy\":\"{0}\"", param.GroupBy));
            }
            else
            {
                list.Add("\"groupBy\":null");
            }


            if (param.JoinItems != null && param.JoinItems.Count > 0)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < param.JoinItems.Count; i++)
                {
                    temp.Add(string.Format("{0}", JoinItem.ToJson(param.JoinItems[i])));
                }
                list.Add(string.Format("\"joinItems\":[{0}]", string.Join(",", temp.ToArray())));
            }
            else
            {
                list.Add("\"joinItems\":null");
            }

            if (param.LinkItems != null && param.LinkItems.Count > 0)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < param.LinkItems.Count; i++)
                {
                    temp.Add(string.Format("{0}", LinkItem.ToJson(param.LinkItems[i])));
                }
                list.Add(string.Format("\"linkItems\":[{0}]", string.Join(",", temp.ToArray())));
            }
            else
            {
                list.Add("\"linkItems\":null");
            }


            if (param.IDs != null && param.IDs.Count > 0)
            {
                List<string> temp = new List<string>();
                foreach (int id in param.IDs)
                {
                    temp.Add(id.ToString());
                }
                list.Add(string.Format("\"ids\":[{0}]", string.Join(",", temp.ToArray())));
            }

            if (param.Fields != null && param.Fields.Count > 0)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < param.Fields.Count; i++)
                {
                    temp.Add(string.Format("\"{0}\"", param.Fields[i]));
                }
                list.Add(string.Format("\"fields\":[{0}]", string.Join(",", temp.ToArray())));
            }

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }
    }
}
