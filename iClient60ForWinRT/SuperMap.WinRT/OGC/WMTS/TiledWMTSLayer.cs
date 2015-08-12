using System;
using System.Text;
using SuperMap.WinRT.OGC;
using SuperMap.WinRT.Utilities;
using System.ComponentModel;
using SuperMap.WinRT.Core;
using System.Net;
using System.Threading;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${mapping_TiledWMTSLayer_Tile}</para>
    /// 	<para>${mapping_TiledWMTSLayer_Description}</para>
    /// </summary>
    public class TiledWMTSLayer : TiledCachedLayer
    {
        //WMTS预定义的分辨率级别
        double[] preDefResScale ={1.25764139776733,0.628820698883665,0.251528279553466,
                                     0.125764139776733,0.0628820698883665,0.0251528279553466,
                                     0.0125764139776733,0.00628820698883665,0.00251528279553466,
                                     0.00125764139776733,0.000628820698883665,0.000251528279553466,
                                     0.000125764139776733,0.0000628820698883665,0.0000251528279553466,
                                     0.0000125764139776733,0.00000628820698883665,0.00000251528279553466,
                                     0.00000125764139776733,0.000000628820698883665,0.000000251528279553466};

        double[] preDefResPixel = { 240000, 120000, 60000, 40000, 20000, 10000, 4000, 2000, 1000, 500, 166, 100, 33, 16, 10, 3, 1, 0.33 };

        double[] preDefGoogleCRS84Quad = {1.40625000000000,0.703125000000000,0.351562500000000,0.175781250000000,
                                             0.0878906250000000,0.0439453125000000,0.0219726562500000,
                                             0.0109863281250000,0.00549316406250000,0.00274658203125000,
                                             0.00137329101562500,0.000686645507812500,0.000343322753906250,
                                             0.000171661376953125,0.0000858306884765625,0.0000429153442382812,
                                             0.0000214576721191406,0.0000107288360595703,0.00000536441802978516};

        double[] preDefGoogleMapsCompatible = {156543.0339280410,78271.51696402048,39135.75848201023,19567.87924100512,
                                                  9783.939620502561,4891.969810251280,2445.984905125640,1222.992452562820,
                                                  611.4962262814100,305.7481131407048,152.8740565703525,76.43702828517624,
                                                  38.21851414258813,19.10925707129406,9.554628535647032,4.777314267823516,
                                                  2.388657133911758,1.194328566955879,0.5971642834779395 };
        double[] preDefResChina = { 0.7031249999891485,0.35156249999999994,0.17578124999999997,
								        0.08789062500000014,0.04394531250000007,0.021972656250000007,0.01098632812500002,
								        0.00549316406250001,0.0027465820312500017,0.0013732910156250009,0.000686645507812499,
								        0.0003433227539062495,0.00017166137695312503,0.00008583068847656251,0.000042915344238281406,
								        0.000021457672119140645,0.000010728836059570307,0.000005364418029785169,0.000002682210361715995,0.0000013411051808579975};

        /// <summary>${mapping_TiledWMTSLayer_constructor_D}</summary>
        public TiledWMTSLayer()
        { }

        private void wmtsManager_ProcessComplated(GetWMTSCapabilitiesResult e)
        {
            Rectangle2D bbox;
            double[] resolutions;
            WMTSManagerResult result = e as WMTSManagerResult;
            if (result != null)
            {
                if (result != null && result.MatrixSetInfo != null && result.MatrixSetInfo.TileMatrixs != null && result.MatrixSetInfo.TileMatrixs.Count > 0)
                {

                    tileMatrixs = result.MatrixSetInfo.TileMatrixs.ToArray();

                    //已经定义了WellKnownScaleSet值:GlobalCRS84Scale
                    if ((!string.IsNullOrEmpty(result.MatrixSetInfo.WellKnownScaleSet) && (result.MatrixSetInfo.WellKnownScaleSet == "GlobalCRS84Scale" || result.MatrixSetInfo.WellKnownScaleSet.Contains("GlobalCRS84Scale"))) || (!string.IsNullOrEmpty(this.WellKnownScaleSet) && (this.WellKnownScaleSet == "GlobalCRS84Scale" || this.WellKnownScaleSet.Contains("GlobalCRS84Scale"))))
                    {
                        bbox = new Core.Rectangle2D(-180, -90, 180, 90);
                        resolutions = preDefResScale;
                    }
                    //已经定义了WellKnownScaleSet值:GlobalCRS84Pixel
                    else if ((!string.IsNullOrEmpty(result.MatrixSetInfo.WellKnownScaleSet) && (result.MatrixSetInfo.WellKnownScaleSet == "GlobalCRS84Pixel" || result.MatrixSetInfo.WellKnownScaleSet.Contains("GlobalCRS84Pixel"))) || (!string.IsNullOrEmpty(this.WellKnownScaleSet) && (this.WellKnownScaleSet == "GlobalCRS84Pixel" || this.WellKnownScaleSet.Contains("GlobalCRS84Pixel"))))
                    {
                        bbox = new Core.Rectangle2D(-180, -90, 180, 90);
                        resolutions = preDefResPixel;
                    }
                    //已经定义了WellKnownScaleSet值:GoogleCRS84Quad
                    else if ((!string.IsNullOrEmpty(result.MatrixSetInfo.WellKnownScaleSet) && (result.MatrixSetInfo.WellKnownScaleSet == "GoogleCRS84Quad" || result.MatrixSetInfo.WellKnownScaleSet.Contains("GoogleCRS84Quad"))) || (!string.IsNullOrEmpty(this.WellKnownScaleSet) && (this.WellKnownScaleSet == "GoogleCRS84Quad" || this.WellKnownScaleSet.Contains("GoogleCRS84Quad"))))
                    {
                        bbox = new Core.Rectangle2D(-180, -90, 180, 90);
                        resolutions = preDefGoogleCRS84Quad;
                    }
                    //已经定义了WellKnownScaleSet值:GoogleMapsCompatible
                    else if ((!string.IsNullOrEmpty(result.MatrixSetInfo.WellKnownScaleSet) && (result.MatrixSetInfo.WellKnownScaleSet == "GoogleMapsCompatible" || result.MatrixSetInfo.WellKnownScaleSet.Contains("GoogleMapsCompatible"))) || (!string.IsNullOrEmpty(this.WellKnownScaleSet) && (this.WellKnownScaleSet == "GoogleMapsCompatible" || this.WellKnownScaleSet.Contains("GoogleMapsCompatible"))))
                    {
                        bbox = new Core.Rectangle2D(-20037508.3427892, -20037508.3427892, 20037508.3427892, 20037508.3427892);
                        resolutions = preDefGoogleMapsCompatible;
                    }
                    //已经定义了WellKnownScaleSet值:custom
                    else if (((!string.IsNullOrEmpty(result.MatrixSetInfo.WellKnownScaleSet) && result.MatrixSetInfo.WellKnownScaleSet == "custom") || (!string.IsNullOrEmpty(this.WellKnownScaleSet) && this.WellKnownScaleSet == "custom")))
                    {
                        bbox = new Core.Rectangle2D(-180, -90, 180, 90);
                        resolutions = preDefResChina;
                    }
                    //若WellKnownScaleSet为空或者其他非法值，则默认使用GlobalCRS84Scale
                    else
                    {
                        bbox = new Core.Rectangle2D(-180, -90, 180, 90);
                        resolutions = preDefResScale;
                    }
                    GetTileUrl();
                    //若用户未定义Bounds值，则使用默认Bounds
                    if (Rectangle2D.IsNullOrEmpty(this.Bounds))
                    {
                        this.Bounds = bbox;
                    }
                    //若用户未定义Resolutions值，则使用默认resolutions
                    if (this.Resolutions == null)
                    {
                        //如果tileMatrixs的长度与预定义的resolutions的长度不同，则返回
                        if (tileMatrixs.Length != resolutions.Length)
                            return;
                        this.Resolutions = resolutions;
                    }
                    this.CRS = result.MatrixSetInfo.SupportedCRS;
                }
            }

            base.Initialize();
        }

        /// <summary>${mapping_TiledWMTSLayer_method_Initialize_D}</summary>
        public async override void Initialize()
        {
            if (!string.IsNullOrEmpty(this.Url) && this.Url.EndsWith("/"))
            {
                //去除结尾可能的'/'符号
                this.Url = this.Url.TrimEnd('/');
            }

            //this.Resolutions = preDefResScale;
            this.TileSize = 256;

            if (enableGetCapabilities)
            {
                try
                {
                    WMTSManager wmtsManager = new WMTSManager(this);
                    GetWMTSCapabilitiesResult result = await wmtsManager.ProcessAsync();
                    wmtsManager_ProcessComplated(result);
                }
                catch (Exception ex)
                {
                    base.Error = ex;
                    base.Initialize();
                }
            }
            else if (this.Resolutions == null)
            {
                this.Resolutions = this.Map.Resolutions;
                GetTileUrl();
                if (Rectangle2D.IsNullOrEmpty(this.Bounds))
                {
                    this.Bounds = new Core.Rectangle2D(-180, -90, 180, 90);
                }
                base.Initialize();
            }
            else
            {
                GetTileUrl();
                if (Rectangle2D.IsNullOrEmpty(this.Bounds))
                {
                    this.Bounds = new Core.Rectangle2D(-180, -90, 180, 90);
                }
                base.Initialize();
            }
        }

        private void GetTileUrl()
        {
            if (RequestEncoding == RequestEncoding.KVP)
            {
                StringBuilder sb = new StringBuilder(this.Url);
                sb.Append("?SERVICE=WMTS");
                sb.Append("&REQUEST=GetTile");
                sb.Append("&VERSION=").Append(this.Version);
                sb.Append("&LAYER=").Append(this.LayerName);
                sb.Append("&STYLE=").Append(this.Style);
                sb.Append("&TILEMATRIXSET=").Append(this.TileMatrixSet);

                sb.Append("&TILEMATRIX=" + "{0}");

                sb.Append("&TILEROW={1}");
                sb.Append("&TILECOL={2}");
                sb.Append("&FORMAT=image/").Append(this.ImageFormat);
                this.Url = sb.ToString();
            }
            else if (RequestEncoding == RequestEncoding.REST)
            {
                StringBuilder sb = new StringBuilder(this.Url);
                sb.Append("/").Append(this.LayerName);
                sb.Append("/").Append(this.Style);
                sb.Append("/").Append(this.TileMatrixSet);
                sb.Append("/" + "{0}");
                sb.Append("/{1}");
                sb.Append("/{2}");
                sb.Append(".").Append(this.ImageFormat);

                this.Url = sb.ToString();
            }
        }
        /// <summary>${mapping_TiledWMTSLayer_method_GetTileUrl_D}</summary>
        public override MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken)
        {
            MapImage mapImage = new MapImage();
            mapImage.MapImageType = MapImageType.Url;
            if (tileMatrixIdentifiers == null)
            {
                mapImage.Url = string.Format(this.Url, tileMatrixs[level].Name, indexY, indexX);
                return mapImage;
            }
            else
            {
                int index = findMatrixLevel(this.Resolution);
                if (index == -1)
                {
                    return null;
                }
                string name = string.Empty;
                if (tileMatrixs != null)
                {
                    name = tileMatrixs[index].Name;
                    if (!CheckMatrix(name))
                    {
                        name = string.Empty;
                    }
                }
                else
                {
                    name = tileMatrixIdentifiers[index];
                }
                //string index = tileMatrixIdentifiers[];
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }
                else
                {
                    mapImage.Url = string.Format(this.Url, name, indexY, indexX);
                    return mapImage;
                }
            }

        }

        private bool CheckMatrix(string s)
        {
            if (tileMatrixIdentifiers == null)
            {
                return true;
            }
            bool isIn = false;
            for (int i = 0; i < tileMatrixIdentifiers.Length; i++)
            {
                if (tileMatrixIdentifiers[i] == s)
                {
                    isIn = true;
                    break;
                }
            }
            return isIn;

        }

        private int findMatrixLevel(double resolution)
        {
            if (tileMatrixIdentifiers != null && Resolutions != null)
            {
                return MathUtil.GetNearestIndex(resolution, Resolutions);
            }
            return -1;
        }

        //OGC的<Layer><ows:Identifier>
        /// <summary>${mapping_TiledWMTSLayer_attribute_LayerName_D}</summary>
        public string LayerName { get; set; }

        //OGC的Style
        /// <summary>${mapping_TiledWMTSLayer_attribute_Style_D}</summary>
        public string Style { get; set; }

        //OGC的TileMatrixSet
        /// <summary>${mapping_TiledWMTSLayer_attribute_TileMatrixSet_D}</summary>
        public string TileMatrixSet { get; set; }

        //不让用户自定义显示级别了，否则不好控制level
        //public List<string> MatrixIds { get; set; }

        /// <summary>${mapping_TiledWMTSLayer_attribute_RequestEncoding_D}</summary>
        public RequestEncoding RequestEncoding { get; set; }

        private bool enableGetCapabilities = true;
        /// <summary>${mapping_TiledWMTSLayer_attribute_EnableGetCapabilities_D}</summary>
        public bool EnableGetCapabilities
        {
            get { return enableGetCapabilities; }
            set { enableGetCapabilities = value; }
        }

        private string version = "1.0.0";
        /// <summary>${mapping_TiledWMTSLayer_attribute_Version_D}</summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        //private double[] resolution3;
        /// <summary>${mapping_TiledWMTSLayer_attribute_Resolutions_D}</summary>
        public new double[] Resolutions
        {
            get { return base.Resolutions; }
            set { base.Resolutions = value; }
        }

        private string[] tileMatrixIdentifiers;

        /// <summary>${mapping_TiledWMTSLayer_attribute_TileMatrixIdentifiers_D}</summary>
        public string[] TileMatrixIdentifiers
        {
            get { return this.tileMatrixIdentifiers; }
            set { this.tileMatrixIdentifiers = value; }
        }
        //自定义WellKnownScaleSet，值为OGC和国家标准值（custom）时有效
        /// <summary>${mapping_TiledWMTSLayer_attribute_WellKnownScaleSet_D}</summary>
        public string WellKnownScaleSet
        {
            get;
            set;
        }
        private TileMatrix[] tileMatrixs;
    }
}
