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
    /// 	<para>${REST_ThemeGraduatedSymbolStyle_Title}</para>
    /// 	<para>${REST_ThemeGraduatedSymbolStyle_Description}</para>
    /// </summary>
    public class ThemeGraduatedSymbolStyle
    {
        /// <summary>${REST_ThemeGraduatedSymbolStyle_constructor_D}</summary>
        public ThemeGraduatedSymbolStyle()
        {

        }

        /// <summary>${REST_ThemeGraduatedSymbolStyle_attribute_positiveStyle_D}</summary>
        public ServerStyle PositiveStyle
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeGraduatedSymbolStyle_attribute_negativeStyle_D}</summary>
        public ServerStyle NegativeStyle
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeGraduatedSymbolStyle_attribute_negativeDisplayed_D}</summary>
        public bool NegativeDisplayed
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeGraduatedSymbolStyle_attribute_zeroStyle_D}</summary>
        public ServerStyle ZeroStyle
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeGraduatedSymbolStyle_attribute_zeroDisplayed_D}</summary>
        public bool ZeroDisplayed
        {
            get;
            set;
        }

        internal static string ToJson(ThemeGraduatedSymbolStyle valueSection)
        {
            string json = "";
            List<string> list = new List<string>();

            if (valueSection.PositiveStyle != null)
            {
                list.Add(string.Format("\"positiveStyle\":{0}", ServerStyle.ToJson(valueSection.PositiveStyle)));
            }
            else
            {
                list.Add(string.Format("\"positiveStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            if (valueSection.NegativeStyle != null)
            {
                list.Add(string.Format("\"negativeStyle\":{0}", ServerStyle.ToJson(valueSection.NegativeStyle)));
            }
            else
            {
                list.Add(string.Format("\"negativeStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            if (valueSection.ZeroStyle != null)
            {
                list.Add(string.Format("\"zeroStyle\":{0}", ServerStyle.ToJson(valueSection.ZeroStyle)));
            }
            else
            {
                list.Add(string.Format("\"zeroStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            if (valueSection.ZeroDisplayed)
            {
                list.Add(string.Format("\"zeroDisplayed\":{0}", valueSection.ZeroDisplayed.ToString().ToLower()));
            }

            if (valueSection.NegativeDisplayed)
            {
                list.Add(string.Format("\"negativeDisplayed\":{0}", valueSection.NegativeDisplayed.ToString().ToLower()));
            }
            json = string.Join(",", list.ToArray());
            return json;
        }


        internal static ThemeGraduatedSymbolStyle FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeGraduatedSymbolStyle symbolStyle = new ThemeGraduatedSymbolStyle();
            symbolStyle.NegativeDisplayed = json["negativeDisplayed"].GetBooleanEx();
            symbolStyle.NegativeStyle = ServerStyle.FromJson(json["negativeStyle"].GetObjectEx());
            symbolStyle.PositiveStyle = ServerStyle.FromJson(json["positiveStyle"].GetObjectEx());
            symbolStyle.ZeroDisplayed = json["zeroDisplayed"].GetBooleanEx();
            symbolStyle.ZeroStyle = ServerStyle.FromJson(json["zeroStyle"].GetObjectEx());
            return symbolStyle;
        }
    }
}
