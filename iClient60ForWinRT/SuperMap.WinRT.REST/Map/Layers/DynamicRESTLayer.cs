


using System;
using System.Security;
using System.Windows;
using SuperMap.WinRT.Resources;
using SuperMap.WinRT.Core;
using System.Text;
using SuperMap.WinRT.REST;
using System.Collections.Generic;
using SuperMap.WinRT.Utilities;
using Windows.Foundation;
using System.Threading;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${Mapping_DynamicRESTLayer_Title}</para>
    /// 	<para>${Mapping_DynamicRESTLayer_Description}</para>
    /// </summary>
    public class DynamicRESTLayer : DynamicLayer, IClearTheme
    {
        private bool isInitializing;
        private SmMapService mapService;
        /// <summary>${Mapping_DynamicRESTLayer_constructor_None_D}</summary>
        public DynamicRESTLayer()
            : base()
        {
            ImageFormat = "png";
            AdjustFactor = 1.0;
            Transparent = false;
            EnableServerCaching = true;
            ClipRegion = null;
            MaxVisibleVertex = int.MaxValue;
        }

        /// <summary>${Mapping_DynamicRESTLayer_method_Initialize_D}</summary>
        public async override void Initialize()
        {
            if (!this.isInitializing && !base.IsInitialized)
            {
                #region 必设参数判断
                if (string.IsNullOrEmpty(this.Url))
                {
                    base.Error = new ArgumentNullException(SuperMap.WinRT.REST.Resources.ExceptionStrings.InvalidUrl);
                    base.Initialize();
                    return;
                }

                #endregion

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
        //http://localhost:8090/iserver/services/map-world/rest/maps/World%20Map/
        //tileImage.bmp?tileImage.bmp?scale=0.00000002&x=4&y=1&width=500&height=500
        /// <summary>${Mapping_DynamicRESTLayer_method_GetImageUrl_D}</summary>
        protected override MapImage GetImage(CancellationToken cancel)
        {
            Rectangle2D bounds = ViewBounds;
            int width = (int)Math.Round(ViewSize.Width);
            int height = (int)Math.Round(ViewSize.Height);

            StringBuilder imgUrl = new StringBuilder();
            imgUrl.Append(Url);
            if (!Url.EndsWith("/"))
            {
                imgUrl.Append("/");
            }

            imgUrl.AppendFormat("image.{0}?", ImageFormat);
            imgUrl.AppendFormat("viewBounds={0}", JsonHelper.FromRectangle2D(bounds));
            imgUrl.AppendFormat("&height={0}", height);
            imgUrl.AppendFormat("&width={0}", width);

            if (!string.IsNullOrEmpty(LayersID))
            {
                imgUrl.AppendFormat("&layersID={0}", LayersID);
            }
            imgUrl.AppendFormat("&transparent={0}", Transparent);

            if (ClipRegion != null)
            {
                imgUrl.Append("&clipRegionEnabled=True&");
                imgUrl.AppendFormat("clipRegion={0}", ServerGeometry.ToJson(ClipRegion.ToServerGeometry()));
            }
            if (MaxVisibleVertex != int.MaxValue && MaxVisibleVertex >= 0)
            {
                imgUrl.AppendFormat("&maxVisibleVertex={0}", MaxVisibleVertex);
            }

            if (!EnableServerCaching)
            {
                imgUrl.AppendFormat("&cacheEnabled={0}", EnableServerCaching);
                imgUrl.AppendFormat("&t={0}", DateTime.Now.Ticks.ToString());
            }
            if (this.CRS != null && this.CRS.WKID > 0)
            {
                imgUrl.Append("&prjCoordSys={\"epsgCode\":" + this.CRS.WKID + "}");
            }
            imgUrl.AppendFormat("&customParams={0}", CustomParams);
            return new MapImage() {MapImageType=MapImageType.Url,Url=imgUrl.ToString() };
        }

        /// <summary>${Mapping_DynamicRESTLayer_method_ClearTheme_D}</summary>
        public void ClearTheme()
        {
            if (!string.IsNullOrEmpty(layersID))
            {
                LayersID = string.Empty;
            }
        }
        //此值必须是非负数。
        /// <summary>${Mapping_DynamicRESTLayer_attribute_maxVisibleVertex_D}</summary>
        public int MaxVisibleVertex { get; set; }

        /// <summary>${Mapping_TiledDynamicRESTLayer_attribute_AdjustFactor_D}</summary>
        public GeoRegion ClipRegion { get; set; }

        //internal SmMapServiceInfo MapServiceInfo { get; private set; }
        /// <summary>${Mapping_DynamicRESTLayer_attribute_AdjustFactor_D}</summary>
        public double AdjustFactor { get; set; }

        public string CustomParams { get; set; }
        /// <summary>${Mapping_DynamicRESTLayer_attribute_Dpi_D}</summary>
        protected override double Dpi { get; set; }

        /// <summary>${Mapping_DynamicRESTLayer_attribute_isScaleCentric_D}</summary>
        public override bool IsScaleCentric
        {
            get
            {
                return true;
            }
        }

        //当LayersID发生变化时，应该刷新图层；
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
                        base.IsInitialized = false;
                        isInitializing = false;
                        base.Error = null;
                        base.Refresh();
                        this.Initialize();
                    }
                }
            }
        }

        /// <summary>${Mapping_DynamicRESTLayer_attribute_EnableServerCaching_D}</summary>
        public bool EnableServerCaching { get; set; }


        //需要已知Bounds、DPI；
        //IsSkipGetSMMapServiceInfo默认是false，
        //如果设置为true，则Bounds、 ReferViewBounds、ReferViewer、ReferScale必须设置；
        //CRS此时为空，要用到与CRS相关时，需要设置CRS；
        /// <summary>${Mapping_DynamicRESTLayer_attribute_IsSkipGetSMMapServiceInfo_D}</summary>
        public bool IsSkipGetSMMapServiceInfo { get; set; }
        /// <summary>${Mapping_DynamicRESTLayer_attribute_ReferViewBounds_D}</summary>
        public Rectangle2D ReferViewBounds { get; set; }
        /// <summary>${Mapping_DynamicRESTLayer_attribute_ReferViewer_D}</summary>
        public Rect ReferViewer { get; set; }
        /// <summary>${Mapping_DynamicRESTLayer_attribute_ReferScale_D}</summary>
        public double ReferScale { get; set; }

    }
}
