using System;
using System.Windows;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using Windows.Data.Json;
using Windows.Foundation;

namespace SuperMap.WinRT.REST
{
    internal class SmMapServiceInfo
    {
        public double Scale { get; set; }
        public Rectangle2D ViewBounds { get; set; }
        public Rect Viewer { get; set; }
        public Rectangle2D Bounds { get; set; }
        public PrjCoordSys PrjCoordSys { get; set; }
        public Unit CoordUnit { get; set; }

        internal static SmMapServiceInfo FromJson(JsonObject json)
        {
            if (json == null) return null;

            if (!json.ContainsKey("name") || !json.ContainsKey("bounds")) return null;

            return new SmMapServiceInfo
            {
                ViewBounds = JsonHelper.ToRectangle2D(json["viewBounds"].GetObjectEx()),
                CoordUnit = (Unit)Enum.Parse(typeof(Unit), json["coordUnit"].GetStringEx(), true),
                Bounds = JsonHelper.ToRectangle2D(json["bounds"].GetObjectEx()),
                PrjCoordSys = PrjCoordSys.FromJson(json["prjCoordSys"].GetObjectEx()),
                Scale = json["scale"].GetNumberEx(),
                Viewer = JsonHelper.ToRect(json["viewer"].GetObjectEx())
            };
        }
    }
}
