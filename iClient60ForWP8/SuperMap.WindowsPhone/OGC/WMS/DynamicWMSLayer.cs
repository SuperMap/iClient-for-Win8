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
    /// ${mapping_DynamicWMSLayer_Title}<br/>
    /// ${mapping_DynamicWMSLayer_Description}
    /// </summary>
    public class DynamicWMSLayer : DynamicLayer
    {
        private string[] layersArray;
        private bool isInitializing;

        // Coordinate system WKIDs in WMS 1.3 where X,Y (Long,Lat) switched to Y,X (Lat,Long)
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

        /// <summary>
        ///     ${pubilc_Constructors_Initializes} <see cref="DynamicWMSLayer">DynamicWMSLayer</see> ${pubilc_Constructors_instance}
        /// </summary>
        public DynamicWMSLayer()
            : base()
        {
            this.AllLayerList = new ObservableCollection<WMSLayerInfo>();
            this.ImageFormat = "png";
            this.Transparent = false;
            this.BgColor = "0xFFFFFF";
        }

        /// <summary>${mapping_Layer_method_initialize_D}</summary>
        public override void Initialize()
        {
            if (isInitializing || IsInitialized)
            {
                return;
            }

            if (!Url.Contains("http://"))  //相对地址
            {
                var pageUrl = System.Windows.Browser.HtmlPage.Document.DocumentUri;
                var localUrl = pageUrl.AbsoluteUri.Substring(0, pageUrl.AbsoluteUri.IndexOf(pageUrl.AbsolutePath));
                Url = localUrl + Url;
            }

            isInitializing = true;
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
            if (this.Version == "1.3.0")
            {
                this.Bounds = new Rectangle2D(manager.Bounds.BottomLeft.Y, manager.Bounds.BottomLeft.X, manager.Bounds.TopRight.Y, manager.Bounds.TopRight.X);
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

        /// <summary>${mapping_DynamicWMSLayer_method_getImageUrl_D}</summary>
        /// <returns>${mapping_DynamicWMSLayer_method_getImageUrl_return}</returns>
        protected override string GetImageUrl()
        {
            Rectangle2D bounds = ViewBounds;
            int width = (int)Math.Round(ViewSize.Width);
            int height = (int)Math.Round(ViewSize.Height);

            if (CRS == null)
            {
                //TODO:Resource
                //throw new ArgumentNullException("CRS");
                throw new ArgumentNullException(ExceptionStrings.CRSIsNull);
            }

            int wkid = CRS.WKID;
            StringBuilder imgURL = new StringBuilder();

            //string stimgURLr = this.httpGetResource ?? this.Url;
            //imgURL.Append(stimgURLr);
            imgURL.Append(ProxyUrl + this.Url);
            if (!Url.EndsWith("?"))
            {
                imgURL.Append("?");
            }
            imgURL.Append("service=WMS&request=GetMap");
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&width={0}", width);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&height={0}", height);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&format=image/{0}", ImageFormat);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&layers={0}", new object[] { (this.Layers == null) ? "" : String.Join(",", Layers) });//数组元素间用逗号分隔
            imgURL.Append("&styles=");
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&bgcolor={0}", BgColor);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&transparent={0}", Transparent);
            imgURL.AppendFormat(CultureInfo.InvariantCulture, "&version={0}", this.Version);

            if (this.LowerThan13Version())
            {
                imgURL.AppendFormat(CultureInfo.InvariantCulture, "&SRS=EPSG:{0}", wkid);
                imgURL.AppendFormat(CultureInfo.InvariantCulture, "&bbox={0},{1},{2},{3}", bounds.Left, bounds.Bottom, bounds.Right, bounds.Top);
            }
            else
            {
                imgURL.AppendFormat(CultureInfo.InvariantCulture, "&CRS=EPSG:{0}", wkid);
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
                if (flag)
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
        private static Version highestSupportedVersion = new Version(1, 3);
        private bool LowerThan13Version()
        {
            Version version = new Version(this.Version);
            if (version < highestSupportedVersion)
            {
                return true;
            }
            return false;
        }
        /// <summary>${mapping_DynamicWMSLayer_attribute_layers_D}</summary>
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

        /// <summary>${mapping_DynamicWMSLayer_attribute_proxyUrl_D}</summary>
        public string ProxyUrl { get; set; }

        /// <summary>${mapping_DynamicWMSLayer_attribute_version_D}</summary>
        public string Version { get; set; }
        /// <summary>${mapping_DynamicWMSLayer_attribute_bgColor_D}</summary>
        public string BgColor { get; set; }//用string大家都方便

        ///// <summary>${mapping_DynamicWMSLayer_attribute_exceptions_D}</summary>
        //public string Exceptions { get; set; }
        ///// <summary>${mapping_DynamicWMSLayer_attribute_time_D}</summary>
        //public string Time { get; set; }
        ///// <summary>${mapping_DynamicWMSLayer_attribute_elevation_D}</summary>
        //public string Elevation { get; set; }
        ////style 还有1.1.1的 SLD 和 WFS 暂不支持，等研究研究再说。

        /// <summary>${mapping_DynamicWMSLayer_attribute_EnableGetCapabilities}</summary>
        public bool EnableGetCapabilities { get; set; }

        /// <summary>${mapping_DynamicWMSLayer_attribute_AllLayerList}</summary>
        public IList<WMSLayerInfo> AllLayerList { get; private set; }
    }
}
