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
    /// 	<para>${REST_LabelImageCell_Title}</para>
    /// 	<para>${REST_LabelImageCell_Description}</para>
    /// </summary>
    public class LabelImageCell : LabelMatrixCell
    {
        /// <summary>${REST_LabelImageCell_constructor_D}</summary>
        public LabelImageCell()
        {
            height = 10;
            width = 10;
            Type = LabelMatrixCellType.IMAGE;
        }

        private double height;
        /// <summary>${REST_LabelImageCell_attribute_Height_D}</summary>
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value < 0)
                {
                    value = 10;
                }

                height = value;
            }
        }
        /// <summary>${REST_LabelImageCell_attribute_PathField_D}</summary>
        public string PathField
        {
            get;
            set;
        }
        /// <summary>${REST_LabelImageCell_attribute_Rotation_D}</summary>
        public double Rotation
        {
            get;
            set;
        }
        /// <summary>${REST_LabelImageCell_attribute_SizeFixed_D}</summary>
        public bool SizeFixed
        {
            get;
            set;
        }

        private double width;
        /// <summary>${REST_LabelImageCell_attribute_Width_D}</summary>
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                if (width < 0)
                {
                    value = 10;
                }
                width = value;
            }
        }

        internal static string ToJson(LabelImageCell imageCell)
        {
            string json = "{";
            List<string> list = new List<string>();
            list.Add(string.Format("\"pathField\":\"{0}\"", imageCell.PathField));
            list.Add(string.Format("\"sizeFixed\":{0}", imageCell.SizeFixed.ToString().ToLower()));
            list.Add(string.Format("\"height\":{0}", imageCell.Height));
            list.Add(string.Format("\"width\":{0}", imageCell.Width));
            list.Add(string.Format("\"rotation\":{0}", imageCell.Rotation));
            list.Add(string.Format("\"type\":\"{0}\"", imageCell.Type));
            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static LabelImageCell FromJson(JsonObject json)
        {
            if (json == null) return null;

            LabelImageCell imageCell = new LabelImageCell();
            imageCell.Height = json["height"].GetNumberEx();
            imageCell.PathField = json["pathField"].GetStringEx();
            imageCell.Rotation = json["rotation"].GetNumberEx();
            imageCell.SizeFixed = json["sizeFixed"].GetBooleanEx();
            imageCell.Width = json["width"].GetNumberEx();
            if (json.ContainsKey("type") && json["type"] != null)
            {
                imageCell.Type = (LabelMatrixCellType)Enum.Parse(typeof(LabelMatrixCellType), json["type"].GetStringEx(), true);
            }
            return imageCell;
        }
    }
}
