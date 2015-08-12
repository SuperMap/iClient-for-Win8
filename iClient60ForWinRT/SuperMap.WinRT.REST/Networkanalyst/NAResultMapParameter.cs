

using SuperMap.WinRT.Core;
using System.Windows;
using System.Collections.Generic;
using SuperMap.WinRT.Utilities;
using System;
using Windows.Data.Json;
using Windows.Foundation;
namespace SuperMap.WinRT.iServerJava6R.NetworkAnalyst
{
    /// <summary>
    /// 	<para>${iServerJava6R_NAResultMapParameter_Title}</para>
    /// 	<para>${iServerJava6R_NAResultMapParameter_Description}</para>
    /// </summary>
    public class NAResultMapParameter
    {
        /// <summary>${iServerJava6R_NAResultMapParameter_constructor_D}</summary>
        public NAResultMapParameter()
        {
            BackgroundTransparent = true;
        }

        /// <summary>${iServerJava6R_NAResultMapParameter_attribute_BackgroundTransparent_D}</summary>
        public bool BackgroundTransparent { get; set; }
        /// <summary>${iServerJava6R_NAResultMapParameter_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds { get; set; }
        /// <summary>${iServerJava6R_NAResultMapParameter_attribute_Center_D}</summary>
        public Point2D Center { get; set; }
        /// <summary>${iServerJava6R_NAResultMapParameter_attribute_Format_D}</summary>
        public OutputFormat Format { get; set; }
        /// <summary>${iServerJava6R_NAResultMapParameter_attribute_Scale_D}</summary>
        public double Scale { get; set; }
        /// <summary>${iServerJava6R_NAResultMapParameter_attribute_Style_D}</summary>
        public ServerStyle Style { get; set; }
        /// <summary>${iServerJava6R_NAResultMapParameter_attribute_UseDefaultParameter_D}</summary>
        public bool UseDefaultParameter { get; set; }
        /// <summary>${iServerJava6R_NAResultMapParameter_attribute_Viewer_D}</summary>
        public Rect Viewer { get; set; }

        internal static string ToJson(NAResultMapParameter param)
        {
            if (param != null)
            {
                string json = "{";
                List<string> list = new List<string>();

                list.Add(string.Format("\"backgroundTransparent\":{0}", param.BackgroundTransparent.ToString().ToLower()));
                list.Add(string.Format("\"useDefaultParameter\":{0}", param.UseDefaultParameter));

                if (param.Bounds != Rectangle2D.Empty)
                    list.Add(string.Format("\"bounds\":{0}", JsonHelper.FromRectangle2D(param.Bounds)));
                if (param.Center != Point2D.Empty)
                    list.Add(string.Format("\"center\":{0}", JsonHelper.FromPoint2D(param.Center)));
                list.Add(string.Format("\"format\":\"{0}\"", param.Format));
                if (param.Scale != 0)
                    list.Add(string.Format("\"scale\":\"{0}\"", param.Scale));
                if (param.Style != null)
                    list.Add(string.Format("\"style\":\"{0}\"", ServerStyle.ToJson(param.Style)));
                if (param.Viewer != null)
                    list.Add(string.Format("\"viewer\":{0}", string.Format("{{\"leftTop\":{{\"x\":0,\"y\":0}},\"rightBottom\":{{\"x\":{0},\"y\":{1}}}}}", param.Viewer.Width, param.Viewer.Height)));

                json += string.Join(",", list.ToArray());
                json += "}";
                return json;
            }
            return null;
        }

        /// <summary>${iServerJava6R_NAResultMapParameter_method_FromJson_D}</summary>
        /// <returns>${iServerJava6R_NAResultMapParameter_method_FromJson_return}</returns>
        /// <param name="json">${iServerJava6R_NAResultMapParameter_method_FromJson_param_jsonObject}</param>
        public static NAResultMapParameter FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            NAResultMapParameter result = new NAResultMapParameter();
            result.BackgroundTransparent = json["backgroundTransparent"].GetBooleanEx();
            result.Bounds = JsonHelper.ToRectangle2D(json["bounds"].GetObjectEx());
            result.Center = JsonHelper.ToPoint2D(json["center"].GetObjectEx());
            result.Format = (OutputFormat)Enum.Parse(typeof(OutputFormat), json["format"].GetStringEx(), true);
            result.Scale = json["scale"].GetNumberEx();
            result.Style = ServerStyle.FromJson(json["style"].GetObjectEx());
            result.UseDefaultParameter = json["useDefaultParameter"].GetBooleanEx();
            result.Viewer = JsonHelper.ToRect(json["viewer"].GetObjectEx());

            return result;
        }
    }
}
