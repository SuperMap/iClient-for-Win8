﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;
using System.Text;
using System.Threading;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// 	<para>${WP_mapping_CloudLayer_Title}</para>
    /// 	<para>${WP_mapping_CloudLayer_Description}</para>
    /// </summary>
    public class CloudLayer : TiledCachedLayer
    {
        /// <summary>${WP_mapping_CloudLayer_attribute_MapName_D}</summary>
        public string MapName { get; set; }
        private string type { get; set; }
        private double[] scales = null;

        /// <summary>${WP_mapping_CloudLayer_constructor_None_D}</summary>
        public CloudLayer()
        {
            this.Url = "http://t0.supermapcloud.com/FileService/image";
            this.Bounds = new Rectangle2D(-2.00375083427892E7, -2.00375083427892E7, 2.00375083427892E7, 2.00375083427892E7);
            this.Resolutions = new double[] { 156605.46875, 78302.734375, 39151.3671875, 19575.68359375, 9787.841796875, 4893.9208984375, 2446.96044921875, 1223.48022460937, 611.740112304687, 305.870056152344, 152.935028076172, 76.4675140380859, 38.233757019043, 19.1168785095215, 9.55843925476074, 4.77921962738037, 2.38960981369019, 1.19480490684509, 0.597402453422546 };
            this.MapName = "quanguo";
            this.type = "web";
            this.TileSize = 256;
        }
        /// <summary>${WP_mapping_CloudLayer_method_initialize_D}</summary>
        public override void Initialize()
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(this.Url);
            urlBuilder.Append("?map={0}&type={1}&x={2}&y={3}&z={4}");
            this.Url = urlBuilder.ToString();
            base.Initialize();
        }
        /// <summary>${WP_mapping_CloudLayer_method_GetTileUrl_D}</summary>
        public override MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken)
        {
            MapImage image = new MapImage();
            image.MapImageType = MapImageType.Url;
            image.Url=String.Format(System.Globalization.CultureInfo.InvariantCulture,this.Url, this.MapName, this.type, indexX, indexY, level);
            return image;
        }
    }
}

