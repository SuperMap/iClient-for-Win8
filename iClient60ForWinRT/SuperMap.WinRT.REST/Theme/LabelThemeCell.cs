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
    /// 	<para>${REST_LabelThemeCell_Title}</para>
    /// 	<para>${REST_LabelThemeCell_Description}</para>
    /// </summary>
    public class LabelThemeCell : LabelMatrixCell
    {
        /// <summary>${REST_LabelThemeCell_constructor_D}</summary>
        public LabelThemeCell() 
        {
            Type = LabelMatrixCellType.THEME;
        }
        /// <summary>${REST_LabelThemeCell_attribute_ThemeLabel_D}</summary>
        public ThemeLabel ThemeLabel
        {
            get;
            set;
        }

        internal static string ToJson(LabelThemeCell themeCell)
        {
            string json = "{";
            List<string> list = new List<string>();

            if (themeCell.ThemeLabel != null)
            {
                list.Add(string.Format("\"themeLabel\":{0}", ThemeLabel.ToJson(themeCell.ThemeLabel)));
            }
            else
            {
                list.Add("\"themeLabel\":null");
            }

            list.Add(string.Format("\"type\":\"{0}\"", themeCell.Type));

            json += string.Join(",", list.ToArray());

            json += "}";
            return json;
        }

        internal static LabelThemeCell FromJson(JsonObject json)
        {
            if (json == null) return null;

            LabelThemeCell themeCell = new LabelThemeCell();
            themeCell.ThemeLabel = ThemeLabel.FromJson(json);
            if (json.ContainsKey("type") && json["type"] != null)
            {
                themeCell.Type = (LabelMatrixCellType)Enum.Parse(typeof(LabelMatrixCellType), json["type"].GetStringEx(), true);
            }
            return themeCell;
        }
    }
}
