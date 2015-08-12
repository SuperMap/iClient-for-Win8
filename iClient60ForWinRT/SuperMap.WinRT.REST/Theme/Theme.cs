using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_Theme_Title}</para>
    /// 	<para>${REST_Theme_Description}</para>
    /// </summary>
    public class Theme
    {
        /// <summary>${REST_Theme_constructor_None_D}</summary>
        public Theme()
        {

        }
        /// <summary>${REST_Theme_MemoryData__D}</summary>
        public List<ThemeMemoryData> MemoryData { get; set; }

        internal string ToJson(List<ThemeMemoryData> memorydata)
        {
            if (memorydata != null)
            {
                List<string> memoryStr = new List<string>();
                foreach (var item in memorydata)
                {
                    memoryStr.Add("\"" + item.ScrData + "\":\"" + item.TargetData + "\"");
                }
                return "{" + string.Join(",", memoryStr.ToArray()) + "}";
            }
            else
            {
                return "null";
            }
        }
    }


    /// <summary>
    /// 	<para>${REST_ThemeMemoryData_Title}</para>
    /// 	<para>${REST_ThemeMemoryData_Description}</para>
    /// </summary>
    public class ThemeMemoryData
    {
        /// <summary>${REST_Theme_ThemeMemoryData_None_D}</summary>
        public ThemeMemoryData() { }
        /// <summary>${REST_Theme_ScrData_D}</summary>
        public string ScrData { get; set; }
        /// <summary>${REST_Theme_TargetData_D}</summary>
        public string TargetData { get; set; }
    }
}
