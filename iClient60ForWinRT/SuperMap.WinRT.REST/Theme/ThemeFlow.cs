using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Data.Json;
using SuperMap.WinRT.Utilities;

namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_ThemeFlow_Title}</para>
    /// 	<para>${REST_ThemeFlow_Description}</para>
    /// </summary>
    public class ThemeFlow
    {
        /// <summary>${REST_ThemeFlow_constructor_D}</summary>
        public ThemeFlow()
        {
            FlowEnabled = true;
            LeaderLineDisplayed = false;
        }

        /// <summary>
        /// <para>${REST_ThemeFlow_attribute_FlowEnabled_D}</para>
        /// <para>
        /// 		<list type="table">
        /// 			<item>
        /// 				<term><img src="unflowiServer6.bmp"/></term>
        /// 				<description><img src="flowiServer6.bmp"/></description>
        /// 			</item>
        /// 		</list>
        /// 	</para>
        /// </summary>
        public bool FlowEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// <para>${REST_ThemeFlow_attribute_LeaderLineStyle_D}</para>
        /// <para><img src="leaderLineiServer6.bmp"/></para>
        /// </summary>
        public ServerStyle LeaderLineStyle
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeFlow_attribute_LeaderLineDisplayed_D}</summary>
        public bool LeaderLineDisplayed
        {
            get;
            set;
        }

        internal static string ToJson(ThemeFlow flowTheme)
        {
            string json = "";
            List<string> list = new List<string>();

            if (flowTheme.LeaderLineStyle != null)
            {
                list.Add(string.Format("\"leaderLineStyle\":{0}", ServerStyle.ToJson(flowTheme.LeaderLineStyle)));
            }
            else
            {
                list.Add(string.Format("\"leaderLineStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            list.Add(string.Format("\"flowEnabled\":{0}", flowTheme.FlowEnabled.ToString().ToLower()));
            list.Add(string.Format("\"leaderLineDisplayed\":{0}", flowTheme.LeaderLineDisplayed.ToString().ToLower()));

            json = string.Join(",", list.ToArray());
            return json;
        }

        internal static ThemeFlow FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeFlow flow = new ThemeFlow();

            flow.FlowEnabled = json["flowEnabled"].GetBooleanEx();
            flow.LeaderLineDisplayed = json["leaderLineDisplayed"].GetBooleanEx();
            if (json["leaderLineStyle"] != null)
            {
                flow.LeaderLineStyle = ServerStyle.FromJson(json["leaderLineStyle"].GetObjectEx());
            }
            return flow;
        }
    }
}
