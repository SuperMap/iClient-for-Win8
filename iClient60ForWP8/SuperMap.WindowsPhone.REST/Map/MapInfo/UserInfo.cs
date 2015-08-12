
using Newtonsoft.Json;
namespace SuperMap.WindowsPhone.REST
{
    /// <summary>
    /// 	<para>${WP_REST_UserInfo_Title}</para>
    /// 	<para>${WP_REST_UserInfo_Description}</para>
    /// </summary>
    public class UserInfo
    {
        /// <summary>${WP_REST_UserInfo_constructor_D}</summary>
        public UserInfo()
        { }
        /// <summary>${WP_REST_UserInfo_attribute_UserID_D}</summary>
        [JsonProperty("userID")]
        public string UserID { get; set; }

    }
}
