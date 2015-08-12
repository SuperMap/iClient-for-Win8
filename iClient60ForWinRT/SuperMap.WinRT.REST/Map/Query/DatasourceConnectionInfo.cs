

using System.Collections.Generic;
using System;
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_DatasourceConnectionInfo_Title}</para>
    /// 	<para>${REST_DatasourceConnectionInfo_Description}</para>
    /// </summary>
    public class DatasourceConnectionInfo
    {
        /// <summary>${REST_DatasourceConnectionInfo_constructor_None_D}</summary>
        public DatasourceConnectionInfo()
        { }

        /// <summary>${REST_DatasourceConnectionInfo_attribute_Alias_D}</summary>
        public string Alias { get; set; }
        /// <summary>${REST_DatasourceConnectionInfo_attribute_DataBase_D}</summary>
        public string DataBase { get; set; }
        /// <summary>${REST_DatasourceConnectionInfo_attribute_Driver_D}</summary>
        public string Driver { get; set; }
        /// <summary>${REST_DatasourceConnectionInfo_attribute_Password_D}</summary>
        public string Password { get; set; }
        /// <summary>${REST_DatasourceConnectionInfo_attribute_Server_D}</summary>
        public string Server { get; set; }
        /// <summary>${REST_DatasourceConnectionInfo_attribute_User_D}</summary>
        public string User { get; set; }

        /// <summary>${REST_DatasourceConnectionInfo_attribute_EngineType_D}</summary>
        public EngineType EngineType { get; set; }

        /// <summary>${REST_DatasourceConnectionInfo_attribute_Connect_D}</summary>
        public bool Connect { get; set; }
        /// <summary>${REST_DatasourceConnectionInfo_attribute_Exclusive_D}</summary>
        public bool Exclusive { get; set; }
        /// <summary>${REST_DatasourceConnectionInfo_attribute_OpenLinkTable_D}</summary>
        public bool OpenLinkTable { get; set; }
        /// <summary>${REST_DatasourceConnectionInfo_attribute_ReadOnly_D}</summary>
        public bool ReadOnly { get; set; }



        internal static string ToJson(DatasourceConnectionInfo param)
        {
            if (param == null)
            {
                return null;
            }

            string json = "{";
            List<string> list = new List<string>();

            if (!string.IsNullOrEmpty(param.Alias))
            {
                list.Add(string.Format("\"alias\":\"{0}\"", param.Alias));
            }
            else
            {
                list.Add("\"alias\":null");
            }

            if (!string.IsNullOrEmpty(param.DataBase))
            {
                list.Add(string.Format("\"dataBase\":\"{0}\"", param.DataBase));
            }
            else
            {
                list.Add("\"dataBase\":null");
            }

            if (!string.IsNullOrEmpty(param.Driver))
            {
                list.Add(string.Format("\"driver\":\"{0}\"", param.Driver));
            }
            else
            {
                list.Add("\"driver\":null");
            }

            if (!string.IsNullOrEmpty(param.Password))
            {
                list.Add(string.Format("\"password\":\"{0}\"", param.Password));
            }
            else
            {
                list.Add("\"password\":null");
            }

            if (!string.IsNullOrEmpty(param.Server))
            {
                list.Add(string.Format("\"server\":\"{0}\"", param.Server));
            }
            else
            {
                list.Add("\"server\":null");
            }

            if (!string.IsNullOrEmpty(param.User))
            {
                list.Add(string.Format("\"user\":\"{0}\"", param.User));
            }
            else
            {
                list.Add("\"user\":null");
            }

            list.Add(string.Format("\"engineType\":\"{0}\"", param.EngineType.ToString()));

            list.Add(string.Format("\"connect\":{0}", param.Connect.ToString().ToLower()));
            list.Add(string.Format("\"exclusive\":{0}", param.Exclusive.ToString().ToLower()));
            list.Add(string.Format("\"openLinkTable\":{0}", param.OpenLinkTable.ToString().ToLower()));
            list.Add(string.Format("\"readOnly\":{0}", param.ReadOnly.ToString().ToLower()));

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }
    }
}
