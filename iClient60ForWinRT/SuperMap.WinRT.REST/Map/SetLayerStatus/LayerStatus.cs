namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${iServer6_LayerStatus_Title}</para>
    /// 	<para>${iServer6_LayerStatus_Description}</para>
    /// </summary>
    public class LayerStatus
    {
        /// <summary>${iServer6_LayerStatus_constructor_None_D}</summary>
        public LayerStatus()
        {
            IsVisible = true;
        }
        /// <summary>${iServer6_LayerStatus_attribute_layerName_D}</summary>
        public string LayerName { get; set; }

        /// <summary>${iServer6_LayerStatus_attribute_isVisible_D}</summary>
        public bool IsVisible { get; set; }

        public string DisplayFilter { get; set; }

        internal static string ToJson(LayerStatus layerstatus)
        {
            string json = "{";
            json += string.Format("\"type\":\"UGC\",");
            json += string.Format("\"name\":\"{0}\",", layerstatus.LayerName);
            json += string.Format("\"visible\":{0},", layerstatus.IsVisible.ToString().ToLower());
            if (!string.IsNullOrEmpty(layerstatus.DisplayFilter))
            {
                json += string.Format("\"displayFilter\":\"{0}\"", layerstatus.DisplayFilter.ToString());
            }
            json += "}";
            return json;
        }
    }
}
