using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using SuperMap.Web.Core;
using SuperMap.Web.OGC;
using SuperMap.Web.Resources;
using SuperMap.Web.Utilities;

namespace SuperMap.Web.Mapping
{
    /// <summary>
    /// 	<para>${mapping_TiledWMSLayer_Title}</para>
    /// 	<para>${mapping_TiledWMSLayer_Description}</para>
    /// </summary>
    public class TiledWMSLayer : TiledDynamicLayer
    {
        //private string httpGetResource;
        private string[] layersArray;
        private static Version highestSupportedVersion = new Version(1, 3);
        #region LatLongCRSRanges
        private int[,] LatLongCRSRanges = new int[,] { { 4001, 4999 },
						{2044, 2045},   {2081, 2083},   {2085, 2086},   {2093, 2093},
						{2096, 2098},   {2105, 2132},   {2169, 2170},   {2176, 2180},
						{2193, 2193},   {2200, 2200},   {2206, 2212},   {2319, 2319},
						{2320, 2462},   {2523, 2549},   {2551, 2735},   {2738, 2758},
						{2935, 2941},   {2953, 2953},   {3006, 3030},   {3034, 3035},
						{3058, 3059},   {3068, 3068},   {3114, 3118},   {3126, 3138},
						{3300, 3301},   {3328, 3335},   {3346, 3346},   {3350, 3352},
						{3366, 3366},   {3416, 3416},   {20004, 20032}, {20064, 20092},
						{21413, 21423}, {21473, 21483}, {21896, 21899}, {22171, 22177},
						{22181, 22187}, {22191, 22197}, {25884, 25884}, {27205, 27232},
						{27391, 27398}, {27492, 27492}, {28402, 28432}, {28462, 28492},
						{30161, 30179}, {30800, 30800}, {31251, 31259}, {31275, 31279},
						{31281, 31290}, {31466, 31700} };
        #endregion

        /// <summary>${mapping_TiledWMSLayer_constructor_None_D}</summary>
        public TiledWMSLayer()
        {
            this.AllLayerList = new ObservableCollection<WMSLayerInfo>();
            this.ImageFormat = "png";
            this.Transparent = false;
            this.BgColor = "0xFFFFFF";
        }

        /// <summary>${mapping_TiledCachedIServerLayer_method_initialize_D}</summary>
        public override void Initialize()
        {
            if (base.IsInitialized)
            {
                return;
            }
            if (string.IsNullOrEmpty(this.Url))
            {
                base.Error = new ArgumentNullException(ExceptionStrings.InvalidURISchemeHost);
                base.Initialize();
                return;
            }

            if (!Url.Contains("http://"))  //相对地址
            {
                var pageUrl = System.Windows.Browser.HtmlPage.Document.DocumentUri;
                var localUrl = pageUrl.AbsoluteUri.Substring(0, pageUrl.AbsoluteUri.IndexOf(pageUrl.AbsolutePath));
                Url = localUrl + Url;
            }

            if (!this.EnableGetCapabilities)
            {
                base.Initialize();
            }
            else
            {
                WMSManager manager = new WMSManager(this.Url, this.Version, this.ProxyUrl);
                manager.GetCapabilityCompleted += new EventHandler<EventArgs>(manager_GetCapabilityCompleted);
            }
        }
        private void manager_GetCapabilityCompleted(object sender, EventArgs e)
        {
            WMSManager manager = sender as WMSManager;
            if (manager.Error != null)
            {
                base.Error = manager.Error;
                base.Initialize();
                return;
            }
            //当版本号是1.3.0时，x，y是反的；
            if (this.Version == "1.3.0" && isInRange(manager.CRS.WKID))
            {
                this.Bounds = new Rectangle2D(manager.Bounds.Bottom, manager.Bounds.Left, manager.Bounds.Top, manager.Bounds.Right);
            }
            else
            {
                this.Bounds = manager.Bounds;
            }
            this.CRS = manager.CRS;
            this.AllLayerList = manager.AllLayerList;
            this.Metadata = manager.Metadata;
            if (string.IsNullOrEmpty(this.Version))
            {
                this.Version = manager.Version;
            }
            base.Initialize();
        }

        /// <summary>${mapping_TiledDynamicLayer_method_getTileUrl_D}</summary>
        /// <returns>${mapping_TiledDynamicLayer_method_getTileUrl_param_return}</returns>
        /// <param name="indexX">${mapping_TiledDynamicLayer_method_getTileUrl_param_indexX}</param>
        /// <param name="indexY">${mapping_TiledDynamicLayer_method_getTileUrl_param_indexY}</param>
        /// <param name="resolution">${mapping_TiledDynamicLayer_method_getTileUrl_param_resolution}</param>
        public override string GetTileUrl(int indexX, int indexY, double resolution)
        {
            Rectangle2D bounds = this.GetBBox(resolution, indexY, indexX);

            if (CRS == null)
            {
                throw new ArgumentNullException(ExceptionStrings.CRSIsNull);
            }

            int wkid = CRS.WKID;
            StringBuilder imgURL = new StringBuilder();

            imgURL.Append(ProxyUrl + Url);
            imgURL.Append("?service=WMS&request=GetMap");
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&width={0}", TileSize);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&height={0}", TileSize);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&format=image/{0}", ImageFormat);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&layers={0}", new object[] { (this.Layers == null) ? "" : String.Join(",", Layers) });
            imgURL.Append("&styles=");
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&bgcolor={0}", BgColor);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&transparent={0}", Transparent);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&version={0}", Version);

            if (this.LowerThan13Version())
            {
                imgURL.AppendFormat(CultureInfo.InvariantCulture, "&SRS=EPSG:{0}", wkid);
                imgURL.AppendFormat(CultureInfo.InvariantCulture, "&bbox={0},{1},{2},{3}", bounds.Left, bounds.Bottom, bounds.Right, bounds.Top);
            }
            else
            {
                imgURL.AppendFormat(CultureInfo.InvariantCulture, "&CRS=EPSG:{0}", wkid);
                if (isInRange(wkid))
                {
                    imgURL.AppendFormat(CultureInfo.InvariantCulture, "&bbox={0},{1},{2},{3}", bounds.Bottom, bounds.Left, bounds.Top, bounds.Right);
                }
                else
                {
                    imgURL.AppendFormat(CultureInfo.InvariantCulture, "&bbox={0},{1},{2},{3}", bounds.Left, bounds.Bottom, bounds.Right, bounds.Top);
                }
            }
            return imgURL.ToString();
        }

        private bool isInRange(int wkid)
        {
            bool flag = false;
            int num = LatLongCRSRanges.Length / 2;
            for (int i = 0; i < num; i++)
            {
                if ((wkid >= LatLongCRSRanges[i, 0]) && (wkid <= LatLongCRSRanges[i, 1]))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        private Rectangle2D GetBBox(double resolution, int indexX, int indexY)
        {
            double left = this.Bounds.Left + TileSize * indexY * resolution;
            double right = this.Bounds.Left + TileSize * (indexY + 1) * resolution;
            double top = this.Bounds.Top - TileSize * (indexX) * resolution;
            double bottom = this.Bounds.Top - TileSize * (indexX + 1) * resolution;
            return new Rectangle2D(left, bottom, right, top);
        }

        private bool LowerThan13Version()
        {
            Version version = new Version(this.Version);
            if (version < highestSupportedVersion)
            {
                return true;
            }
            return false;
        }
        /// <summary>${mapping_DynamicWMSLayer_attribute_proxyUrl_D}</summary>
        public string ProxyUrl { get; set; }

        /// <summary>${mapping_TiledWMSLayer_attribute_layers_D}</summary>
        [TypeConverter(typeof(StringArrayConverter))]
        public string[] Layers
        {
            get { return layersArray; }
            set
            {
                layersArray = value;
                OnLayerChanged();
            }
        }
        /// <summary>${mapping_TiledWMSLayer_attribute_version_D}</summary>
        public string Version { get; set; }

        /// <summary>${mapping_TiledWMSLayer_attribute_bgColor_D}</summary>
        public string BgColor { get; set; }

        ///// <summary>${mapping_TiledWMSLayer_attribute_exceptions_D}</summary>
        //public string Exceptions { get; set; }
        ///// <summary>${mapping_TiledWMSLayer_attribute_time}</summary>
        //public string Time { get; set; }
        ///// <summary>${mapping_TiledWMSLayer_attribute_elevation_D}</summary>
        //public string Elevation { get; set; }

        /// <summary>${mapping_TiledWMSLayer_attribute_EnableGetCapabilities_D}</summary>
        public bool EnableGetCapabilities { get; set; }

        /// <summary>${mapping_TiledWMSLayer_attribute_AllLayerList_D}</summary>
        public IList<WMSLayerInfo> AllLayerList { get; private set; }
    }
}
