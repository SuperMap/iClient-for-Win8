
namespace SuperMap.WinRT.REST
{
    /// <summary>
    /// 	<para>${REST_DatasetType_Title}</para>
    /// 	<para>${REST_DatasetType_Description}</para>
    /// </summary>
    public enum DatasetType
    {
        /// <summary>${REST_DatasetType_attribute_CAD_D}</summary>
        CAD,//           复合数据集。 
        /// <summary>${REST_DatasetType_attribute_GRID_D}</summary>
        GRID,//           栅格数据集。 
        /// <summary>
        ///     <para>${REST_DatasetType_attribute_IMAGE_D}</para>
        ///     <para><img src="IMAGE.png"/></para>
        /// </summary>
        IMAGE,//           影像数据集。 
        /// <summary>
        ///     <para>${REST_DatasetType_attribute_IMAGE_D}</para>
        ///     <para><img src="LINE.png"/></para>
        /// </summary>
        LINE,//           线数据集。
        /// <summary>${REST_DatasetType_attribute_LINE3D_D}</summary>
        LINE3D,//          三维线数据集。
        /// <summary>
        ///     <para>${REST_DatasetType_attribute_LINEM_D}</para>
        ///     <para><img src="LINEM.png"/></para>
        /// </summary>
        LINEM,//           路由数据集。 
        /// <summary>${REST_DatasetType_attribute_LINKTABLE_D}</summary>
        LINKTABLE,//           数据库表。
        /// <summary>
        ///     <para>${REST_DatasetType_attribute_network_D}</para>
        ///     <para><img src="NETWORK.png"/></para>
        /// </summary>
        NETWORK,//           网络数据集。 
        /// <summary>${REST_DatasetType_attribute_networkpoint_D}</summary>
        NETWORKPOINT,//           网络数据集的子数据集。 
        /// <summary>
        ///     <para>${REST_DatasetType_attribute_POINT_D}</para>
        ///     <para><img src="POINT.png"/></para>
        /// </summary>
        POINT,//           点数据集。
        /// <summary>${REST_DatasetType_attribute_POINT3D_D}</summary>
        POINT3D,//          三维点数据集。 
        /// <summary>
        ///     <para>${REST_DatasetType_attribute_REGION_D}</para>
        ///     <para><img src="REGION.png"/></para>
        /// </summary>
        REGION,//           多边形数据集。 
        /// <summary>${REST_DatasetType_attribute_REGION3D_D}</summary>
        REGION3D,//          三维面数据集。 
        /// <summary>
        ///     <para>${REST_DatasetType_attribute_TABULAR_D}</para>
        ///     <para><img src="TABULAR.png"/></para>
        /// </summary>
        TABULAR,//           纯属性数据集。
        /// <summary>
        ///     <para>${REST_DatasetType_attribute_TEXT_D}</para>
        ///     <para><img src="TEXT.png"/></para>
        /// </summary>
        TEXT,//           文本数据集。
        /// <summary>${REST_DatasetType_attribute_UNDEFINED_D}</summary>
        UNDEFINED,//           未定义。
        /// <summary>${REST_DatasetType_attribute_wcs_D}</summary>
        WCS,//           WCS 数据集，是影像数据集 的一种类型。 
        /// <summary>${REST_DatasetType_attribute_wms_D}</summary>
        WMS,//           WMS 数据集，是影像数据集的一种类型。 
    }
}
