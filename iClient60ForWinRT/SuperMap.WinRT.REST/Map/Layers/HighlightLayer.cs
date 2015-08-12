using System;
using System.Text;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.REST;
using System.Security;
using System.Windows;
using SuperMap.WinRT.REST.Resources;
using SuperMap.WinRT.Utilities;
using System.Threading;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${mapping_HighlightLayer_Title}</para>
    /// 	<para>${mapping_HighlightLayer_Description}</para>
    /// </summary>
    public class HighlightLayer : DynamicLayer
    {
        private bool isInitializing;

        /// <summary>${mapping_HighlightLayer_constructor_None_D}</summary>
        /// <overloads>${mapping_HighlightLayer_constructor_None_overloads}</overloads>
        public HighlightLayer()
            : base()
        {
            ImageFormat = "png";
            AdjustFactor = 1.0;
            Transparent = false;
            queryResultID = null;
            highlightTargetSetID = null;
            Redirect = true;
            Style = null;
            ClipRegion = null;
            MaxVisibleVertex = int.MaxValue;
        }
        /// <summary>${mapping_HighlightLayer_string_constructor}</summary>
        /// <param name="url">${mapping_HighlightLayer_string_constructor_param_url}</param>
        public HighlightLayer(string url)
            : this()
        {
            this.Url = url;
        }

        /// <summary>${mapping_HighlightLayer_method_initialize_D}</summary>
        public override void Initialize()
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
                //如果这两个属性都不设置，就要抛出异常！
                if (QueryResultID == null && HighlightTargetSetID == null)
                {
                    base.Error = new ArgumentNullException(SuperMap.WinRT.REST.Resources.ExceptionStrings.InvalidArgument);
                    base.Initialize();
                    return;
                }
                #endregion

                base.Initialize();
            }
        }
        /// <summary>${mapping_DynamicImageLayer_method_getImageUrl_D}</summary>
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

            imgUrl.AppendFormat("highlightImage.{0}?", ImageFormat);
            imgUrl.AppendFormat("viewBounds={0}", JsonHelper.FromRectangle2D(bounds));

            imgUrl.AppendFormat("&width={0}", width);
            imgUrl.AppendFormat("&height={0}", height);

            if (Transparent)
            {
                imgUrl.AppendFormat("&transparent={0}", Transparent);
            }
            if (ClipRegion != null)
            {
                imgUrl.Append("&clipRegionEnabled=True&");
                imgUrl.AppendFormat("clipRegion={0}", ServerGeometry.ToJson(ClipRegion.ToServerGeometry()));
            }
            //这两个属性是互斥的!!!这个控制以QueryResultID为标准；
            if (QueryResultID != null)
            {
                imgUrl.AppendFormat("&queryResultID={0}", QueryResultID);
            }
            else //if (HighlightTargetSetID != null)
            {
                imgUrl.AppendFormat("&highlightTargetSetID={0}", HighlightTargetSetID);
            }

            if (MaxVisibleVertex != int.MaxValue && MaxVisibleVertex >= 0)
            {
                imgUrl.AppendFormat("&maxVisibleVertex={0}", MaxVisibleVertex);
            }

            if (Style != null)
            {
                imgUrl.AppendFormat("&style={0}", ServerStyle.ToJson(Style));
            }
            if (!Redirect)
            {
                imgUrl.AppendFormat("&redirect={0}", Redirect.ToString().ToLower());
            }

            return new MapImage() { MapImageType = MapImageType.Url, Url = imgUrl.ToString() };
        }
        //此值必须是非负数。
        /// <summary>${Mapping_DynamicRESTLayer_attribute_maxVisibleVertex_D}</summary>
        public int MaxVisibleVertex { get; set; }

        /// <summary>${mapping_HighlightLayer_ClipRegion_D}</summary>
        public GeoRegion ClipRegion { get; set; }

        private string queryResultID { get; set; }
        /// <summary>${mapping_HighlightLayer_QueryResultID_D}</summary>
        public string QueryResultID
        {
            get
            {
                return queryResultID;
            }
            set
            {
                if (queryResultID != value)
                {
                    queryResultID = value;
                    if (IsInitialized || isInitializing)
                    {
                        IsInitialized = false;
                        isInitializing = false;
                        Error = null;
                        Refresh();
                        Initialize();
                    }
                }
            }
        }

        private string highlightTargetSetID { get; set; }
        /// <summary>${mapping_HighlightLayer_HighlightTargetSetID_D}</summary>
        public string HighlightTargetSetID
        {
            get
            {
                return highlightTargetSetID;
            }
            set
            {
                if (highlightTargetSetID != value)
                {
                    highlightTargetSetID = value;
                    if (IsInitialized || isInitializing)
                    {
                        IsInitialized = false;
                        isInitializing = false;
                        Error = null;
                        Refresh();
                        Initialize();
                    }
                }
            }
        }

        //设置高亮目标风格
        /// <summary>${mapping_HighlightLayer_Style_D}</summary>
        public ServerStyle Style { get; set; }

        //
        /// <summary>${mapping_HighlightLayer_Redirect_D}</summary>
        public bool Redirect { get; set; }

        /// <summary>${mapping_HighlightLayer_AdjustFactor_D}</summary>
        public double AdjustFactor { get; set; }
        /// <summary>${mapping_DynamicISLayer_attribute_Dpi_D}</summary>
        protected override double Dpi { get; set; }
    }
}
