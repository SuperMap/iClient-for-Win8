

namespace SuperMap.WinRT.REST
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
        public bool Succeed { get; internal set; }
        /// <summary>${REST_Query_ResourceInfo_attribute_NewResourceLocation_D}</summary>
        public string NewResourceLocation { get; internal set; }
        //
        /// <summary>${REST_Query_ResourceInfo_attribute_NewResourceID_D}</summary>
        public string NewResourceID { get; internal set; }
    }
}
