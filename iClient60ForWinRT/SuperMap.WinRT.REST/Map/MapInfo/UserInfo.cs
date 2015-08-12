
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_UserInfo_Title}</para>
    /// 	<para>${REST_UserInfo_Description}</para>
    /// </summary>
    public class UserInfo
    {
        /// <summary>${REST_UserInfo_constructor_D}</summary>
        public UserInfo()
        { }
        /// <summary>${REST_UserInfo_attribute_UserID_D}</summary>
        public string UserID { get; set; }


        internal static UserInfo FromJson(JsonObject json)
        {
            if (json == null)
            {
                return null;
            }
            return new UserInfo { UserID = json["userID"].GetStringEx() };
        }
    }
}
