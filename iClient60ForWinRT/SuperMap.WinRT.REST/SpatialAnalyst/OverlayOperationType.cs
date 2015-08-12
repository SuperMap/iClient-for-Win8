
namespace SuperMap.WinRT.REST.SpatialAnalyst
{
    /// <summary>
    /// 	<para>${REST_OverlayOperationType_Title}</para>
    /// 	<para>${REST_OverlayOperationType_Description}</para>
    /// </summary>
    public enum OverlayOperationType
    {  /// <summary>
        ///     <para>${REST_OverlayOperationType_attribute_CLIP_D}</para>
        ///     <para><img src="Clip.bmp"/></para>
        /// </summary>
        CLIP,                //裁剪。
        /// <summary>
        ///     <para>${REST_OverlayOperationType_attribute_ERASE_D}</para>
        ///     <para><img src="esrase.bmp"/></para>
        /// </summary>
        ERASE,               //擦除。
        /// <summary>
        ///     <para>${REST_OverlayOperationType_attribute_IDENTITY_D}</para>
        ///     <para><img src="identity.bmp"/></para>
        /// </summary>
        IDENTITY,            //同一。
        /// <summary>
        ///     <para>${REST_OverlayOperationType_attribute_INTERSECT_D}</para>
        ///     <para><img src="intersect.bmp"/></para>
        /// </summary>
        INTERSECT,           //相交
        /// <summary>
        ///     <para>${REST_OverlayOperationType_attribute_UNION_D}</para>
        ///     <para><img src="union.bmp"/></para>
        /// </summary>
        UNION,               //合并
        /// <summary>
        ///     <para>${REST_OverlayOperationType_attribute_UPDATE_D}</para>
        ///     <para><img src="update.png"/></para>
        /// </summary>
        UPDATE,              //更新
        /// <summary>
        ///     <para>${REST_OverlayOperationType_attribute_XOR_D}</para>
        ///     <para><img src="XOR.bmp"/></para>
        /// </summary>
        XOR,                 //对称差
    }
}
