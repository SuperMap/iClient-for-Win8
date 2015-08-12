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
    /// 	<para>${REST_ThemeGraduatedSymbol_Title}</para>
    /// 	<para>${REST_ThemeGraduatedSymbol_Description}</para>
    /// 	<para>
    /// 		<img src="ThemeGraduatedSymboliServer6.bmp"/>
    /// 	</para>
    /// </summary>
    public class ThemeGraduatedSymbol:Theme
    {
        /// <summary>${REST_ThemeGraduatedSymbol_constructor_D}</summary>
        public ThemeGraduatedSymbol()
        {
        }

        /// <summary>${REST_ThemeGraduatedSymbol_attribute_BaseValue_D}</summary>
        public double BaseValue 
        { 
            get;
            set;
        }

        /// <summary>${REST_ThemeGraduatedSymbol_attribute_Expression_D}</summary>
        public string Expression 
        {
            get; 
            set;
        }

        /// <summary>${REST_ThemeGraduatedSymbol_attribute_GraduatedMode_D}</summary>
        public GraduatedMode GraduatedMode
        { 
            get; 
            set;
        }

        /// <summary>${REST_ThemeGraduatedSymbol_attribute_Offset_D}</summary>
        public ThemeOffset Offset 
        {            
            get; 
            set; 
        }

        /// <summary>${REST_ThemeGraduatedSymbol_attribute_style_D}</summary>
        public ThemeGraduatedSymbolStyle Style
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeGraduatedSymbol_attribute_Flow_D}</summary>
        public ThemeFlow Flow
        {
            get;
            set;
        }

        internal static string ToJson(ThemeGraduatedSymbol graduatedSymbolTheme)
        {
            string json = "{";
            List<string> list = new List<string>();

            list.Add(string.Format("\"baseValue\":{0}", graduatedSymbolTheme.BaseValue.ToString()));

            if (!string.IsNullOrEmpty(graduatedSymbolTheme.Expression))
            {
                list.Add(string.Format("\"expression\":\"{0}\"", graduatedSymbolTheme.Expression));
            }
            else
            {
                list.Add("\"expression\":\"\"");
            }

            list.Add(string.Format("\"graduatedMode\":\"{0}\"", graduatedSymbolTheme.GraduatedMode.ToString()));

            if (graduatedSymbolTheme.Offset != null)
            {
                list.Add(ThemeOffset.ToJson(graduatedSymbolTheme.Offset));
            }
            else 
            {
                list.Add("\"offsetX\":\"\"");
                list.Add("\"offsetY\":\"\"");
            }

            if (graduatedSymbolTheme.Style != null)
            {
                list.Add(ThemeGraduatedSymbolStyle.ToJson(graduatedSymbolTheme.Style));
            }
            else
            {
                list.Add(string.Format("\"positiveStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
                list.Add(string.Format("\"negativeStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
                list.Add(string.Format("\"zeroStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            if (graduatedSymbolTheme.Flow != null)
            {
                list.Add(ThemeFlow.ToJson(graduatedSymbolTheme.Flow));
            }
            else
            {
                list.Add(string.Format("\"leaderLineStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            if (graduatedSymbolTheme.MemoryData != null)
            {
                list.Add("\"memoryData\":" + graduatedSymbolTheme.ToJson(graduatedSymbolTheme.MemoryData));
            }
            else
            {
                list.Add("\"memoryData\":null");
            }
            list.Add("\"type\":\"GRADUATEDSYMBOL\"");

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static ThemeGraduatedSymbol FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeGraduatedSymbol graduatedSymbol = new ThemeGraduatedSymbol();
            graduatedSymbol.BaseValue = json["baseValue"].GetNumberEx();
            graduatedSymbol.Expression = json["expression"].GetStringEx();
            graduatedSymbol.Flow = ThemeFlow.FromJson(json);
            if (json["graduatedMode"].ValueType != JsonValueType.Null)
            {
                graduatedSymbol.GraduatedMode = (GraduatedMode)Enum.Parse(typeof(GraduatedMode), json["graduatedMode"].GetStringEx(), true);
            }
            else
            {
                //这里不处理为空时的情况
            }
            graduatedSymbol.Offset = ThemeOffset.FromJson(json);
            graduatedSymbol.Style = ThemeGraduatedSymbolStyle.FromJson(json.GetObject());
            return graduatedSymbol;
        }
    }
}
