using System;
using System.Security;
using System.Windows;
using SuperMap.WinRT.REST;
using SuperMap.WinRT.Utilities;
using SuperMap.WinRT.Core;
using System.Globalization;
using Windows.Foundation;
using System.Threading;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${Mapping_TiledDynamicRESTLayer_Tile}</para>
    /// 	<para>${Mapping_TiledDynamicRESTLayer_Description}</para>
    /// </summary>
    public class TiledDynamicRESTLayer : TiledDynamicLayer
    {
        private bool isInitializing;
        private SmMapService mapService;
        private SmMapService _mapServiceDefault;

        /// <summary>${Mapping_TiledDynamicRESTLayer_constructor_None_D}</summary>
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

        /// <summary>${Mapping_TiledDynamicRESTLayer_method_GetTile_D}</summary>
        /// <returns>${Mapping_TiledDynamicRESTLayer_method_GetTile_return}</returns>
        /// <param name="indexX">${Mapping_TiledDynamicRESTLayer_method_GetTile_param_indexX}</param>
        /// <param name="indexY">${Mapping_TiledDynamicRESTLayer_method_GetTile_param_indexY}</param>
        /// <param name="resolution">${Mapping_TiledDynamicRESTLayer_method_GetTile_param_resolution}</param>
        /// <param name="cancellationToken">${Mapping_TiledDynamicRESTLayer_method_GetTile_param_cancellationToken}</param>
        public override MapImage GetTile(int indexX, int indexY, double resolution, CancellationToken cancellationToken)
        {
            double scale = ScaleHelper.ScaleConversion(resolution, this.Dpi, this.CRS);
            string str = string.Empty;

            str = string.Format("{0}/tileImage.{1}?scale={2}&x={3}&y={4}&width={5}&height={5}&transparent={6}",
                       Url, ImageFormat.ToLower(), scale.ToString(CultureInfo.InvariantCulture), indexX, indexY, TileSize, Transparent);

            if (!EnableServerCaching)
            {
                str += string.Format("&cacheEnabled={0}&t={1}", EnableServerCaching.ToString().ToLower(), DateTime.Now.Ticks.ToString());
            }

            if (!string.IsNullOrEmpty(LayersID))
            {
                str += string.Format("&layersID={0}", layersID);
            }
            if (ClipRegion != null)
            {
                str += "&clipRegionEnabled=True&";
                str += string.Format("clipRegion={0}", ServerGeometry.ToJson(ClipRegion.ToServerGeometry()));
            }
            if (MaxVisibleVertex != int.MaxValue && MaxVisibleVertex >= 0)
            {
                str += string.Format("&maxVisibleVertex={0}", MaxVisibleVertex);
            }
            if (!Point2D.IsNullOrEmpty(this.Origin))
            {
                // origin={"x":-200,"y":45}，
                str += "&origin={" + string.Format("\"x\":{0},\"y\":{1}", this.Origin.X, this.Origin.Y) + "}";
            }
            //iServer tileImage请求中只要存在prjCoordSys参数就会把图片生成到temp目录下，
            //所以如果CRS和服务端一致，就不传这个参数上去了。
            if (this.mapService != null && this.mapService.MapServiceInfo != null
                && this.mapService.MapServiceInfo.PrjCoordSys != null)
            {
                CoordinateReferenceSystem tempCRS = new CoordinateReferenceSystem();
                if (_mapServiceDefault.MapServiceInfo != null)
                {
                    tempCRS.WKID = this._mapServiceDefault.MapServiceInfo.PrjCoordSys.EpsgCode;
                    tempCRS.Unit = this._mapServiceDefault.MapServiceInfo.CoordUnit;
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
            MapImage mapImage = new MapImage();
            mapImage.MapImageType = MapImageType.Url;
            mapImage.Url = str;
            return mapImage;
        }


        /// <summary>${Mapping_TiledDynamicRESTLayer_method_Initialize_D}</summary>
        public async override void Initialize()
        {
            if (!this.isInitializing && !base.IsInitialized)
            {
                if (string.IsNullOrEmpty(this.Url))
                {
                    Error = new ArgumentNullException(SuperMap.WinRT.REST.Resources.ExceptionStrings.InvalidUrl);
                    Initialize();
                    return;
                }
                if (IsSkipGetSMMapServiceInfo)
                {
                    if (Rectangle2D.IsNullOrEmpty(Bounds))
                    {
                        Error = new ArgumentNullException("Bounds");
                    }
                    Dpi = ScaleHelper.GetSmDpi(ReferViewBounds, ReferViewer, ReferScale, this.CRS);
                    Dpi *= AdjustFactor;

                    this.isInitializing = true;

                    base.Initialize();
                    return;
                }

                this.isInitializing = true;
                try
                {
                    this._mapServiceDefault = new SmMapService(this.Url.Trim());
                    await this._mapServiceDefault.Initialize();
                    this.mapService = new SmMapService(this.Url.Trim());
                    if (this.CRS != null && this.CRS.WKID > 0)
                    {
                        await this.mapService.Initialize(this.CRS.WKID);
                    }
                    else
                    {
                        await this.mapService.Initialize();
                    }
                    mapService_Initialized();
                }
                catch (Exception ex)
                {
                    if (ex is SecurityException)
                    {
                        base.Error = new SecurityException(SuperMap.WinRT.Resources.ExceptionStrings.InvalidURISchemeHost, ex);
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
            if (mapService.MapServiceUrl == this.Url.Trim())
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
                        if (this.mapService.MapServiceInfo != null)
                        {
                            CRS.Unit = this.mapService.MapServiceInfo.CoordUnit;
                            if (mapService.MapServiceInfo.PrjCoordSys != null)
                            {
                                CRS.WKID = mapService.MapServiceInfo.PrjCoordSys.EpsgCode;
                                if (mapService.MapServiceInfo.PrjCoordSys.CoordSystem != null &&
                                    mapService.MapServiceInfo.PrjCoordSys.CoordSystem.Datum != null &&
                                    mapService.MapServiceInfo.PrjCoordSys.CoordSystem.Datum.Spheroid != null)
                                {
                                    CRS.DatumAxis = mapService.MapServiceInfo.PrjCoordSys.CoordSystem.Datum.Spheroid.Axis;
                                }
                            }
                        }
                    }
                    Bounds = this.mapService.MapServiceInfo.Bounds;
                    Dpi = ScaleHelper.GetSmDpi(this.mapService.MapServiceInfo.ViewBounds, this.mapService.MapServiceInfo.Viewer, this.mapService.MapServiceInfo.Scale, this.CRS);
                    Dpi *= AdjustFactor;
                    base.Initialize();
                }
            }
        }

        /// <summary>${Mapping_DynamicRESTLayer_attribute_maxVisibleVertex_D}</summary>
        public int MaxVisibleVertex { get; set; }
        /// <summary>${Mapping_TiledDynamicRESTLayer_attribute_ClipRegion_D}</summary>
        public GeoRegion ClipRegion { get; set; }
        //internal SmMapServiceInfo MapServiceInfo { get; private set; }
        /// <summary>${Mapping_DynamicRESTLayer_attribute_Dpi_D}</summary>
        protected override double Dpi { get; set; }
        /// <summary>${Mapping_TiledDynamicRESTLayer_attribute_AdjustFactor_D}</summary>
        public double AdjustFactor { get; set; }

        public string CustomParams { get; set; }
        /// <summary>${Mapping_DynamicRESTLayer_attribute_isScaleCentric_D}</summary>
        public override bool IsScaleCentric
        {
            get
            {
                return true;
            }
        }
        /// <summary>${Mapping_TiledDynamicRESTLayer_method_EnableServerCaching_D}</summary>
        public bool EnableServerCaching { get; set; }

        private string layersID = string.Empty;
        /// <summary>${Mapping_DynamicRESTLayer_attribute_LayersID_D}</summary>    
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
                        base.Refresh();
                    }
                }
            }
        }

        //需要已知Bounds、DPI；
        //IsSkipGetSMMapServiceInfo默认是false，
        //如果设置为true，则ReferViewBounds、ReferViewer、ReferScale必须设置；
        //CRS此时为空，要用到与CRS相关时，需要设置CRS；
        /// <summary>${Mapping_TiledDynamicRESTLayer_attribute_IsSkipGetSMMapServiceInfo_D}</summary>
        public bool IsSkipGetSMMapServiceInfo { get; set; }
        /// <summary>${Mapping_TiledDynamicRESTLayer_attribute_ReferViewBounds_D}</summary>
        public Rectangle2D ReferViewBounds { get; set; }
        /// <summary>${Mapping_TiledDynamicRESTLayer_attribute_ReferViewer_D}</summary>
        public Rect ReferViewer { get; set; }
        /// <summary>${Mapping_TiledDynamicRESTLayer_attribute_ReferScale_D}</summary>
        public double ReferScale { get; set; }
    }

}