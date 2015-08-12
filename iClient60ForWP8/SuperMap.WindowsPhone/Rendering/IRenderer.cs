using SuperMap.WindowsPhone.Core;
namespace SuperMap.WindowsPhone.Rendering
{
    /// <summary>
    /// 	<para>${WP_core_IRender_Title}</para>
    /// 	<para>${WP_core_IRender_Description}</para>
    /// </summary>
    public interface IRenderer
    {
        /// <summary>${WP_core_IRender_method_getStyle_D}</summary>
        /// <returns>${WP_core_IRender_method_getStyle_return}</returns>
        /// <param name="feature">${WP_core_IRender_method_getStyle_param_feature}</param>
        Style GetStyle(Feature feature);
    }
}

