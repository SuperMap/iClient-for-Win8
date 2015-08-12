using System;
using System.Security;
using System.Windows;
using SuperMap.WindowsPhone.Utilities;
using SuperMap.WindowsPhone.Core;
using System.Globalization;
using SuperMap.WindowsPhone.REST;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// 	<para>${WP_Mapping_TiledDynamicRESTLayer_Tile}</para>
    /// 	<para>${WP_Mapping_TiledDynamicRESTLayer_Description}</para>
    /// </summary>
    public class TiledDynamicRESTLayer : TiledDynamicLayer
    {
        private bool isInitializing;
        private GetMapStatusService mapService;
        private GetMapStatusService _mapServiceDefault;
        private string _randomKey;

        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_constructor_None_D}</summary>
        public TiledDynamicRESTLayer()
        {
            ImageFormat = "png";
            Transparent = false;
            TileSize = 512;
            AdjustFactor = 1.0;
            EnableServerCaching = true;
            ClipRegion = null;
            MaxVisibleVertex = int.MaxValue;
        }

        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_method_GetTileUrl_D}</summary>
        /// <returns>${WP_Mapping_TiledDynamicRESTLayer_method_GetTileUrl_return}</returns>
        /// <param name="indexX">${WP_Mapping_TiledDynamicRESTLayer_method_GetTileUrl_param_indexX}</param>
        /// <param name="indexY">${WP_Mapping_TiledDynamicRESTLayer_method_GetTileUrl_param_indexY}</param>
        /// <param name="resolution">${WP_Mapping_TiledDynamicRESTLayer_method_GetTileUrl_param_resolution}</param>
        /// <param name="cancellationToken">${WP_Mapping_TiledDynamicRESTLayer_method_GetTileUrl_param_cancellationToken}</param>
        public override MapImage GetTile(int indexX, int indexY, double resolution, CancellationToken cancellationToken)
        {
            double scale = 0;
            scale = ScaleHelper.ScaleConversion(resolution, this.Dpi, this.CRS);
            string str = string.Empty;

            str = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}/tileImage.{1}?scale={2}&x={3}&y={4}&width={5}&height={5}&transparent={6}&t={7}",
                       Url, ImageFormat.ToLower(), scale.ToString(CultureInfo.InvariantCulture), indexX, indexY, TileSize, Transparent, _randomKey);

            if (!EnableServerCaching)
            {
                str += string.Format("&cacheEnabled={0}", EnableServerCaching.ToString(System.Globalization.CultureInfo.InvariantCulture).ToLower());
            }

            if (!string.IsNullOrEmpty(LayersID))
            {
                str += string.Format("&layersID={0}", layersID);
            }
            if (ClipRegion != null)
            {
                str += "&clipRegionEnabled=True&";
                str += string.Format("clipRegion={0}", JsonConvert.SerializeObject(ClipRegion.ToServerGeometry()));
            }
            if (MaxVisibleVertex != int.MaxValue && MaxVisibleVertex >= 0)
            {
                str += string.Format(System.Globalization.CultureInfo.InvariantCulture, "&maxVisibleVertex={0}", MaxVisibleVertex);
            }
            if (!Point2D.IsNullOrEmpty(this.Origin))
            {
                // origin={"x":-200,"y":45}，
                str += "&origin={" + string.Format(CultureInfo.InvariantCulture, "\"x\":{0},\"y\":{1}", this.Origin.X, this.Origin.Y) + "}";
            }
            //iServer tileImage请求中只要存在prjCoordSys参数就会把图片生成到temp目录下，
            //所以如果CRS和服务端一致，就不传这个参数上去了。
            if (this.mapService != null && this.mapService.LastResult != null
                && this.mapService.LastResult.PrjCoordSys != null)
            {
                CoordinateReferenceSystem tempCRS = new CoordinateReferenceSystem();
                if (_mapServiceDefault.LastResult != null)
                {
                    tempCRS.WKID = this._mapServiceDefault.LastResult.PrjCoordSys.EpsgCode;
                    tempCRS.Unit = this._mapServiceDefault.LastResult.CoordUnit;
                }
                if (!CoordinateReferenceSystem.Equals(tempCRS, this.CRS, true))
                {
                    if (this.CRS != null && this.CRS.WKID > 0)
                    {
                        str += "&prjCoordSys={\"epsgCode\":" + this.CRS.WKID + "}";
                    }
                }
            }
            str += string.Format("&customParams={0}", CustomParams);
            MapImage image = new MapImage();
            image.MapImageType = MapImageType.Url;
            image.Url = str;
            return image;
        }


        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_method_Initialize_D}</summary>
        public async override void Initialize()
        {
            _randomKey = DateTime.Now.Ticks.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (!this.isInitializing && !base.IsInitialized)
            {
                if (string.IsNullOrEmpty(this.Url))
                {
                    Error = new ArgumentNullException("Url");
                    Initialize();
                    return;
                }
                if (IsSkipGetSMMapServiceInfo)
                {
                    if (Rectangle2D.IsNullOrEmpty(Bounds))
                    {
                        Error = new ArgumentNullException("Bounds");
                    }
                    if (this.CRS != null)
                    {

                        Dpi = ScaleHelper.GetSmDpi(ReferViewBounds, ReferViewer, ReferScale, this.CRS);
                    }
                    Dpi *= AdjustFactor;

                    this.isInitializing = true;

                    base.Initialize();
                    return;
                }

                this.isInitializing = true;
                try
                {
                    this._mapServiceDefault = new GetMapStatusService(this.Url.Trim());
                    await this._mapServiceDefault.ProcessAsync();

                    if (this.CRS != null && this.CRS.WKID > 0)
                    {
                        this.mapService = new GetMapStatusService(this.Url.Trim(), this.CRS.WKID);
                    }
                    else
                    {
                        this.mapService = new GetMapStatusService(this.Url.Trim());
                    }
                    await mapService.ProcessAsync();
                    mapService_Initialized();
                }
                catch (Exception ex)
                {
                    if (ex is SecurityException)
                    {
                        base.Error = new SecurityException(SuperMap.WindowsPhone.Resources.ExceptionStrings.InvalidURISchemeHost, ex);
                    }
                    else
                    {
                        base.Error = ex;
                    }
                    base.Initialize();
                }
            }
        }

        private void mapService_Initialized()
        {
            if (mapService.LastResult != null)
            {
                if (base.Error != null)
                {
                    base.Initialize();
                }
                else
                {
                    if (CRS == null)
                    {
                        CRS = new CoordinateReferenceSystem();
                        if (this.mapService.LastResult != null)
                        {
                            CRS.Unit = this.mapService.LastResult.CoordUnit;
                            if (mapService.LastResult.PrjCoordSys != null)
                            {
                                CRS.WKID = mapService.LastResult.PrjCoordSys.EpsgCode;
                                if (mapService.LastResult.PrjCoordSys.CoordSystem != null &&
                                    mapService.LastResult.PrjCoordSys.CoordSystem.Datum != null &&
                                    mapService.LastResult.PrjCoordSys.CoordSystem.Datum.Spheroid != null)
                                {
                                    CRS.DatumAxis = mapService.LastResult.PrjCoordSys.CoordSystem.Datum.Spheroid.Axis;
                                }
                            }
                        }
                    }
                    Bounds = this.mapService.LastResult.Bounds;
                    Dpi = ScaleHelper.GetSmDpi(this.mapService.LastResult.ViewBounds, this.mapService.LastResult.Viewer, this.mapService.LastResult.Scale, CRS);
                    Dpi *= AdjustFactor;
                    base.Initialize();
                }
            }
        }

        /// <summary>${WP_Mapping_DynamicRESTLayer_method_ClearTheme_D}</summary>
        public void ClearTheme()
        {
            if (!string.IsNullOrEmpty(layersID))
            {
                LayersID = string.Empty;
            }
        }

        public override void Refresh()
        {
            _randomKey = DateTime.Now.Ticks.ToString(System.Globalization.CultureInfo.InvariantCulture);
            base.Refresh();
        }

        /// <summary>${WP_Mapping_DynamicRESTLayer_attribute_maxVisibleVertex_D}</summary>
        public int MaxVisibleVertex { get; set; }
        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_attribute_ClipRegion_D}</summary>
        public GeoRegion ClipRegion { get; set; }
        //internal SmMapServiceInfo MapServiceInfo { get; private set; }
        /// <summary>${WP_Mapping_DynamicRESTLayer_attribute_Dpi_D}</summary>
        protected override double Dpi { get; set; }
        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_attribute_AdjustFactor_D}</summary>
        public double AdjustFactor { get; set; }

        public string CustomParams { get; set; }
        /// <summary>${WP_Mapping_DynamicRESTLayer_attribute_isScaleCentric_D}</summary>
        public override bool IsScaleCentric
        {
            get
            {
                return true;
            }
        }
        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_method_EnableServerCaching_D}</summary>
        public bool EnableServerCaching { get; set; }

        private string layersID = string.Empty;
        /// <summary>${WP_Mapping_DynamicRESTLayer_attribute_LayersID_D}</summary>    
        public string LayersID
        {
            get
            {
                return layersID;
            }
            set
            {
                if (layersID != value)
                {
                    layersID = value;
                    if (base.IsInitialized || isInitializing)
                    {
                        base.IsInitialized = false;
                        isInitializing = false;
                        base.Error = null;
                        base.Refresh();
                        this.Initialize();
                    }
                }
            }
        }

        //需要已知Bounds、DPI；
        //IsSkipGetSMMapServiceInfo默认是false，
        //如果设置为true，则ReferViewBounds、ReferViewer、ReferScale必须设置；
        //CRS此时为空，要用到与CRS相关时，需要设置CRS；
        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_attribute_IsSkipGetSMMapServiceInfo_D}</summary>
        public bool IsSkipGetSMMapServiceInfo { get; set; }
        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_attribute_ReferViewBounds_D}</summary>
        public Rectangle2D ReferViewBounds { get; set; }
        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_attribute_ReferViewer_D}</summary>
        public Rect ReferViewer { get; set; }
        /// <summary>${WP_Mapping_TiledDynamicRESTLayer_attribute_ReferScale_D}</summary>
        public double ReferScale { get; set; }

    }
}