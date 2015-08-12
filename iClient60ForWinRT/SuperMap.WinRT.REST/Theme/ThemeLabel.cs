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
    /// 	<para>${REST_ThemeLabel_Title}</para>
    /// 	<para>${REST_ThemeLabel_Description}</para>
    /// 	<para><img src="ThemeLableiServer6.bmp"/></para>
    /// 	</summary>
    public class ThemeLabel : Theme
    {
        /// <summary>${REST_ThemeLabel_constructor_D}</summary>
        public ThemeLabel()
        {
            LabelOverLengthMode = LabelOverLengthMode.NONE;
        }

        /// <summary>${REST_ThemeLabel_attribute_AlongLine_D}</summary>
        public ThemeLabelAlongLine AlongLine
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_offset_D}</summary>
        public ThemeOffset Offset
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_Flow_D}</summary>
        public ThemeFlow Flow
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_text_D}</summary>
        public ThemeLabelText Text
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_Background_D}</summary>
        public ThemeLabelBackSetting BackSetting
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_labelOverLengthMode_D}</summary>
        public LabelOverLengthMode LabelOverLengthMode
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_maxLabelLength_D}</summary>
        public int MaxLabelLength
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_smallGeometryLabeled_D}</summary>
        public bool SmallGeometryLabeled
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_rangeExpression_D}</summary>
        public string RangeExpression
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_numericPrecision_D}</summary>
        public int NumericPrecision
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_Items_D}</summary>
        public IList<ThemeLabelItem> Items
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_labelExpression_D}</summary>
        public string LabelExpression
        {
            get;
            set;
        }

        /// <summary>${REST_ThemeLabel_attribute_overlapAvoided_D}</summary>
        public bool OverlapAvoided
        {
            get;
            set;
        }
        /// <summary>${REST_ThemeLabel_attribute_MatrixCells_D}</summary>
        public LabelMatrixCell[,] MatrixCells
        {
            get;
            set;
        }

        internal static string ToJson(ThemeLabel themeLabel)
        {
            string json = "{";

            List<string> list = new List<string>();
            if (themeLabel.AlongLine != null)
            {
                list.Add(ThemeLabelAlongLine.ToJson(themeLabel.AlongLine));
            }
            else
            {
                list.Add(string.Format("\"alongLineDirection\":\"{0}\"", AlongLineDirection.ALONG_LINE_NORMAL));
                list.Add("\"labelRepeatInterval\":0.0");
            }

            if (themeLabel.BackSetting != null)
            {
                list.Add(ThemeLabelBackSetting.ToJson(themeLabel.BackSetting));
            }
            else
            {
                list.Add(string.Format("\"backStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
                list.Add("\"labelBackShape\":\"ROUNDRECT\"");
            }

            if (themeLabel.Flow != null)
            {
                list.Add(ThemeFlow.ToJson(themeLabel.Flow));
            }
            else
            {
                list.Add(string.Format("\"leaderLineStyle\":{0}", ServerStyle.ToJson(new ServerStyle())));
            }

            if (themeLabel.Items != null && themeLabel.Items.Count > 1)
            {
                List<string> itemList = new List<string>();
                foreach (var item in themeLabel.Items)
                {
                    itemList.Add(ThemeLabelItem.ToJson(item));
                }

                list.Add(string.Format("\"items\":[{0}]", string.Join(",", itemList.ToArray())));
            }
            else
            {
                list.Add("\"items\":[]");
            }

            if (!string.IsNullOrEmpty(themeLabel.LabelExpression))
            {
                list.Add(string.Format("\"labelExpression\":\"{0}\"", themeLabel.LabelExpression));
            }
            else
            {
                list.Add("\"labelExpression\":\"\"");
            }

            list.Add(string.Format("\"labelOverLengthMode\":\"{0}\"", themeLabel.LabelOverLengthMode));

            list.Add(string.Format("\"maxLabelLength\":{0}", themeLabel.MaxLabelLength.ToString()));

            list.Add(string.Format("\"numericPrecision\":{0}", themeLabel.NumericPrecision.ToString()));

            if (themeLabel.Offset != null)
            {
                list.Add(ThemeOffset.ToJson(themeLabel.Offset));
            }
            else
            {
                list.Add("\"offsetX\":\"\"");
                list.Add("\"offsetY\":\"\"");
            }

            list.Add(string.Format("\"overlapAvoided\":{0}", themeLabel.OverlapAvoided.ToString().ToLower()));

            if (!string.IsNullOrEmpty(themeLabel.RangeExpression))
            {
                list.Add(string.Format("\"rangeExpression\":\"{0}\"", themeLabel.RangeExpression));
            }
            else
            {
                list.Add("\"rangeExpression\":\"\"");
            }

            list.Add(string.Format("\"smallGeometryLabeled\":{0}", themeLabel.SmallGeometryLabeled.ToString().ToLower()));

            if (themeLabel.Text != null)
            {
                list.Add(ThemeLabelText.ToJson(themeLabel.Text));
            }
            else
            {
                list.Add("\"minTextHeight\":0");
                list.Add("\"maxTextWidth\":0");
                list.Add("\"minTextWidth\":0");
                list.Add("\"maxTextHeight\":0");
                list.Add(string.Format("\"uniformStyle\":{0}", ServerTextStyle.ToJson(new ServerTextStyle())));
                list.Add("\"uniformMixedStyle\":null");
            }

            if (themeLabel.MatrixCells != null)
            {
                List<string> cellList = new List<string>();
                for (int column = 0; column < themeLabel.MatrixCells.GetLength(1); column++)          //列
                {
                    List<string> columnList = new List<string>();

                    for (int row = 0; row < themeLabel.MatrixCells.GetLength(0); row++)                  //行
                    {
                        if (themeLabel.MatrixCells[row, column] is LabelImageCell)
                        {
                            columnList.Add(LabelImageCell.ToJson((LabelImageCell)themeLabel.MatrixCells[row, column]));
                        }
                        else if (themeLabel.MatrixCells[row, column] is LabelSymbolCell)
                        {
                            columnList.Add(LabelSymbolCell.ToJson((LabelSymbolCell)themeLabel.MatrixCells[row, column]));
                        }
                        else if (themeLabel.MatrixCells[row, column] is LabelThemeCell)
                        {
                            columnList.Add(LabelThemeCell.ToJson((LabelThemeCell)themeLabel.MatrixCells[row, column]));
                        }
                    }

                    cellList.Add(string.Format("[{0}]", string.Join(",", columnList.ToArray())));
                }

                list.Add(string.Format("\"matrixCells\":[{0}]", string.Join(",", cellList.ToArray())));
            }
            else
            {
                list.Add("\"matrixCells\":null");
            }

            if (themeLabel.MemoryData != null)
            {
                list.Add("\"memoryData\":" + themeLabel.ToJson(themeLabel.MemoryData));
            }
            else
            {
                list.Add("\"memoryData\":null");
            }
            list.Add("\"type\":\"LABEL\"");

            json += string.Join(",", list.ToArray());
            json += "}";
            return json;
        }

        internal static ThemeLabel FromJson(JsonObject json)
        {
            if (json == null) return null;
            ThemeLabel themeLabel = new ThemeLabel();

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
            themeLabel.AlongLine = alongLine;

            ThemeLabelBackSetting backSetting = new ThemeLabelBackSetting();
            if (json["backStyle"] != null)
            {
                backSetting.BackStyle = ServerStyle.FromJson(json["backStyle"].GetObjectEx());
            }
            if (json["labelBackShape"] != null)
            {
                backSetting.LabelBackShape = (LabelBackShape)Enum.Parse(typeof(LabelBackShape), json["labelBackShape"].GetStringEx(), true);
            }
            else
            {
                //不处理null的情况
            }

            themeLabel.BackSetting = backSetting;

            ThemeFlow flow = new ThemeFlow();
            flow.FlowEnabled = json["flowEnabled"].GetBooleanEx();
            flow.LeaderLineDisplayed = json["leaderLineDisplayed"].GetBooleanEx();
            if (json["leaderLineStyle"] != null)
            {
                flow.LeaderLineStyle = ServerStyle.FromJson(json["leaderLineStyle"].GetObjectEx());
            }
            themeLabel.Flow = flow;

            List<ThemeLabelItem> items = new List<ThemeLabelItem>();
            if (json["items"].ValueType != JsonValueType.Null && json["items"].GetArray().Count > 0)
            {
                for (int i = 0; i < json["items"].GetArray().Count; i++)
                {
                    items.Add(ThemeLabelItem.FromJson(json["items"].GetArray()[i].GetObjectEx()));
                }
            }
            themeLabel.Items = items;

            themeLabel.LabelExpression = json["labelExpression"].GetStringEx();
            if (json["labelOverLengthMode"].ValueType != JsonValueType.Null)
            {
                themeLabel.LabelOverLengthMode = (LabelOverLengthMode)Enum.Parse(typeof(LabelOverLengthMode), json["labelOverLengthMode"].GetStringEx(), true);
            }
            else
            {
                //不处理null的情况
            }

            //themeLabel.MatrixCells
            if (json["matrixCells"].ValueType != JsonValueType.Null)
            {
                int rowCount = (json["matrixCells"].GetArray()).Count;
                int columnCount = ((json["matrixCells"].GetArray())[0].GetArray()).Count;
                LabelMatrixCell[,] matrixCells = new LabelMatrixCell[columnCount, rowCount];

                for (int column = 0; column < (json["matrixCells"].GetArray()).Count; column++)
                {
                    JsonArray cells = (json["matrixCells"].GetArray())[column].GetArray();
                    for (int row = 0; row < cells.Count; row++)
                    {
                        if (cells[row].GetObjectEx().ContainsKey("height") && cells[row].GetObjectEx().ContainsKey("pathField") && cells[row].GetObjectEx().ContainsKey("rotation") && cells[row].GetObjectEx().ContainsKey("sizeFixed") && cells[row].GetObjectEx().ContainsKey("width"))
                        {
                            matrixCells[row, column] = (LabelImageCell.FromJson(cells[row].GetObjectEx()));
                        }
                        else if (cells[row].GetObjectEx().ContainsKey("style") && cells[row].GetObjectEx().ContainsKey("symbolIDField"))
                        {
                            matrixCells[row, column] = (LabelSymbolCell.FromJson(cells[row].GetObjectEx()));
                        }
                        else if (cells[row].GetObjectEx().ContainsKey("themeLabel"))
                        {
                            matrixCells[row, column] = (LabelThemeCell.FromJson(cells[row].GetObjectEx()["themeLabel"].GetObjectEx()));
                        }
                    }
                }

                themeLabel.MatrixCells = matrixCells;
            }
            themeLabel.MaxLabelLength = (int)json["maxLabelLength"].GetNumberEx();
            themeLabel.NumericPrecision = (int)json["numericPrecision"].GetNumberEx();
            themeLabel.Offset = ThemeOffset.FromJson(json);
            themeLabel.OverlapAvoided = json["overlapAvoided"].GetBooleanEx();
            themeLabel.RangeExpression = json["rangeExpression"].GetStringEx();
            themeLabel.SmallGeometryLabeled = json["smallGeometryLabeled"].GetBooleanEx();
            themeLabel.Text = ThemeLabelText.FromJson(json);
            return themeLabel;
        }
    }
}
