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
    /// 	<para>${REST_LabelSymbolCell_Title}</para>
    /// 	<para>${REST_LabelSymbolCell_Description}</para>
    /// 	<para><img src="MatrixLabel.png"/></para>
    /// </summary>
    public class LabelSymbolCell : LabelMatrixCell
    {
        /// <summary>${REST_LabelSymbolCell_constructor_D}</summary>
        public LabelSymbolCell() 
        {
            Type = LabelMatrixCellType.SYMBOL;
        }
        /// <summary>${REST_LabelSymbolCell_attribute_Style_D}</summary>
        public ServerStyle Style
        {
            get;
            set;
        }
        /// <summary>${REST_LabelSymbolCell_attribute_SymbolIDField_D}</summary>
        public string SymbolIDField
        {
            get;
            set;
        }

        internal static string ToJson(LabelSymbolCell symbolCell)
        {
            string json = "{";
            List<string> list = new List<string>();

            list.Add(string.Format("\"symbolIDField\":\"{0}\"", symbolCell.SymbolIDField));
            if (symbolCell.Style != null)
            {
                list.Add(string.Format("\"style\":{0}", ServerStyle.ToJson(symbolCell.Style)));
            }
            else
            {
                list.Add(string.Format("\"style\":{0}", new ServerStyle()));
            }

            list.Add(string.Format("\"type\":\"{0}\"", symbolCell.Type));

            json += string.Join(",", list.ToArray());
            json += "}";

            return json;
        }

        internal static LabelSymbolCell FromJson(JsonObject json)
        {
            if (json == null) return null;
            LabelSymbolCell symbolCell = new LabelSymbolCell();
            if (json["style"] != null)
            {
                symbolCell.Style = ServerStyle.FromJson(json["style"].GetObjectEx());
            }
            symbolCell.SymbolIDField = json["symbolIDField"].GetStringEx();

            if (json.ContainsKey("type") && json["type"] != null)
            {
                symbolCell.Type = (LabelMatrixCellType)Enum.Parse(typeof(LabelMatrixCellType), json["type"].GetStringEx(), true);
            }

            return symbolCell;
        }
    }
}
