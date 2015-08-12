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
    /// 	<para>${REST_ThemeLabelAlongLine_Title}</para>
    /// 	<para>${REST_ThemeLabelAlongLine_Description}</para>
    /// </summary>
    public class ThemeLabelAlongLine
    {
        /// <summary>${REST_ThemeLabelAlongLine_constructor_D}</summary>
        public ThemeLabelAlongLine()
        {
        }

        /// <summary>${REST_ThemeLabelAlongLine_attribute_IsAlongLine_D}</summary>
        public bool IsAlongLine
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelAlongLine_attribute_alongLineDirection_D}</summary>
        public AlongLineDirection AlongLineDirection
        {
            get;
            set;
        }

        /// <summary>
        /// <para>${REST_ThemeLabelAlongLine_attribute_angleFixed_D}</para> 
        ///  	<para>
        /// 		<list type="table">
        /// 			<item>
        /// 				<term><img src="angleFixed_iServer6.bmp"/></term>
        /// 				<term><img src="angleunFixed_iServer6.bmp"/></term>
        /// 			</item>
        /// 		</list>
        /// 	</para>
        /// </summary>
        public bool AngleFixed
        {
            get;
            set;
        }

        /// <summary>
        /// <para>${REST_ThemeLabelAlongLine_attribute_RepeatedLabelAvoided_D}</para>
        /// <para><img src="isReapeatedLabelAvoid_iServer6.bmp"/></para> 
        /// </summary>
        /// 

        public bool RepeatedLabelAvoided
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelAlongLine_attribute_repeatIntervalFixed_D}</summary>
        public bool RepeatIntervalFixed
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabelAlongLine_attribute_labelRepeatInterval_D}</summary>
        public double LabelRepeatInterval
        {
            get;
            set;
        }

        //此属性重复，需要与刘宏商量。
        //public bool IsLabelRepeated
        //{
        //    get;
        //    set;
        //}

        internal static string ToJson(ThemeLabelAlongLine themeLabelAlongLine)
        {
            string json = "";
            List<string> list = new List<string>();

            list.Add(string.Format("\"alongLine\":{0}", themeLabelAlongLine.IsAlongLine.ToString().ToLower()));

            list.Add(string.Format("\"alongLineDirection\":\"{0}\"", themeLabelAlongLine.AlongLineDirection));

            list.Add(string.Format("\"angleFixed\":{0}", themeLabelAlongLine.AngleFixed.ToString().ToLower()));

            list.Add(string.Format("\"repeatedLabelAvoided\":{0}", themeLabelAlongLine.RepeatedLabelAvoided.ToString().ToLower()));

            list.Add(string.Format("\"labelRepeatInterval\":{0}", themeLabelAlongLine.LabelRepeatInterval.ToString()));

            list.Add(string.Format("\"repeatIntervalFixed\":{0}", themeLabelAlongLine.RepeatIntervalFixed.ToString().ToLower()));

            json = string.Join(",", list.ToArray());
            return json;
        }

        internal static ThemeLabelAlongLine FromJson(JsonObject json)
        {
            if (json != null) return null;
            ThemeLabelAlongLine alongLine = new ThemeLabelAlongLine();

            if (json["alongLineDirection"] != null)
            {
                alongLine.AlongLineDirection = (AlongLineDirection)Enum.Parse(typeof(AlongLineDirection), json["alongLineDirection"].GetStringEx(), true);
            }
            else
            {
                //不处理null的情况
            }
            alongLine.AngleFixed = json["angleFixed"].GetBooleanEx();
            alongLine.IsAlongLine = json["alongLine"].GetBooleanEx();
            alongLine.LabelRepeatInterval = json["labelRepeatInterval"].GetNumberEx();
            alongLine.RepeatedLabelAvoided = json["repeatedLabelAvoided"].GetBooleanEx();
            alongLine.RepeatIntervalFixed = json["repeatIntervalFixed"].GetBooleanEx();
            return alongLine;
        }
    }
}
