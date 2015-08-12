

using Newtonsoft.Json;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${REST_Query_ResourceInfo_Title}</para>
    /// 	<para>${REST_Query_ResourceInfo_Description}</para>
    /// </summary>
    public class ResourceInfo
    {
        internal ResourceInfo()
        { }

        /// <summary>${REST_Query_ResourceInfo_attribute_Succeed_D}</summary>
        [JsonProperty("succeed")]
        public bool Succeed { get; internal set; }
        /// <summary>${REST_Query_ResourceInfo_attribute_NewResourceLocation_D}</summary>
        [JsonProperty("newResourceLocation")]
        public string NewResourceLocation { get; internal set; }
        /// <summary>${REST_Query_ResourceInfo_attribute_NewResourceID_D}</summary>
        [JsonProperty("newResourceID")]
        public string NewResourceID { get; internal set; }
    }
}
