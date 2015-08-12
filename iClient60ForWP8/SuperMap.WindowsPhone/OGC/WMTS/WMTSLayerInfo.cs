using System.Collections.Generic;
using SuperMap.Web.Core;

namespace SuperMap.Web.OGC
{
    /// <summary>${mapping_WMTSLayerInfo_Tile}</summary>
    public class WMTSLayerInfo
    {
        /// <summary>${mapping_WMTSLayerInfo_Tile}</summary>
        public WMTSLayerInfo()
        {

        }

        //<ows:Title>world</ows:Title>
        //public string Title { get; internal set; }

        //<ows:Abstract/>
        //public string Abstract { get; internal set; }
        //<ows:Identifier>world</ows:Identifier>
        /// <summary>${mapping_WMTSLayerInfo_attribute_Name_D}</summary>
        public string Name { get; internal set; }

        //Layer的Bounds值，Rectangle2D
        /// <summary>${mapping_WMTSLayerInfo_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds { get; internal set; }

        //如果用户不设置则默认取第一个值
        /// <summary>${mapping_WMTSLayerInfo_attribute_Style_D}</summary>
        public string Style { get; internal set; }

        //<Format>image/jpeg</Format> 用户不可设置 默认为image/png格式
        /// <summary>${mapping_WMTSLayerInfo_attribute_ImageFormat_D}</summary>
        public List<string> ImageFormat { get; internal set; }

        /// <summary>${mapping_WMTSLayerInfo_attribute_TileMatrixSetLinks_D}</summary>
        public List<TileMatrixSetLink> TileMatrixSetLinks { get; internal set; }
    }

    /// <summary>${mapping_TileMatrixSetLink_Tile}</summary>
    public class TileMatrixSetLink
    {
        /// <summary>${mapping_TileMatrixSetLink_constructor_D}</summary>
        public TileMatrixSetLink() { }

        /// <summary>${mapping_TileMatrixSetLink_attribute_TileMatrixSet_D}</summary>
        public string TileMatrixSet { get; internal set; }

        /// <summary>${mapping_TileMatrixSetLink_attribute_TileMatrixSetLimits_D}</summary>
        public List<TileMatrixLimits> TileMatrixSetLimits { get; internal set; }
    }

    /// <summary>${mapping_TileMatrixLimits_Tile}</summary>
    public class TileMatrixLimits
    {
        /// <summary>${mapping_TileMatrixLimits_constructor_D}</summary>
        public TileMatrixLimits() { }

        /// <summary>${mapping_TileMatrixLimits_attribute_TileMatrix_D}</summary>
        public string TileMatrix { get; internal set; }

        /// <summary>${mapping_TileMatrixLimits_attribute_MinTileRow_D}</summary>
        public int MinTileRow { get; internal set; }

        /// <summary>${mapping_TileMatrixLimits_attribute_MaxTileRow_D}</summary>
        public int MaxTileRow { get; internal set; }

        /// <summary>${mapping_TileMatrixLimits_attribute_MinTileCol_D}</summary>
        public int MinTileCol { get; internal set; }

        /// <summary>${mapping_TileMatrixLimits_attribute_MaxTileCol_D}</summary>
        public int MaxTileCol { get; internal set; }
    }

    /// <summary>${mapping_WMTSTileMatrixSetInfo_Tile}</summary>
    public class WMTSTileMatrixSetInfo
    {
        /// <summary>${mapping_WMTSTileMatrixSetInfo_constructor_D}</summary>
        public WMTSTileMatrixSetInfo()
        { }

        /// <summary>${mapping_WMTSTileMatrixSetInfo_attribute_Name_D}</summary>
        public string Name { get; internal set; }

        /// <summary>${mapping_WMTSTileMatrixSetInfo_attribute_SupportedCRS_D}</summary>
        public CoordinateReferenceSystem SupportedCRS { get; internal set; }

        /// <summary>${mapping_WMTSTileMatrixSetInfo_attribute_WellKnownScaleSet_D}</summary>
        public string WellKnownScaleSet { get; internal set; }

        /// <summary>${mapping_WMTSTileMatrixSetInfo_attribute_TileMatrixs_D}</summary>
        public List<TileMatrix> TileMatrixs { get; internal set; }
    }

    /// <summary>${mapping_TileMatrix_Tile}</summary>
    public class TileMatrix
    {
        /// <summary>${mapping_TileMatrix_constructor_D}</summary>
        public TileMatrix() { }

        /// <summary>${mapping_TileMatrix_attribute_Name_D}</summary>
        public string Name { get; internal set; }

        /// <summary>${mapping_TileMatrix_attribute_ScaleDenominator_D}</summary>
        public double ScaleDenominator { get; internal set; }

        /// <summary>${mapping_TileMatrix_attribute_TopLeftCorner_D}</summary>
        public Point2D TopLeftCorner { get; internal set; }

        /// <summary>${mapping_TileMatrix_attribute_TileWidth_D}</summary>
        public int TileWidth { get; internal set; }

        /// <summary>${mapping_TileMatrix_attribute_TileHeight_D}</summary>
        public int TileHeight { get; internal set; }

        /// <summary>${mapping_TileMatrix_attribute_MatrixWidth_D}</summary>
        public int MatrixWidth { get; internal set; }

        /// <summary>${mapping_TileMatrix_attribute_MatrixHeight_D}</summary>
        public int MatrixHeight { get; internal set; }
    }
}
