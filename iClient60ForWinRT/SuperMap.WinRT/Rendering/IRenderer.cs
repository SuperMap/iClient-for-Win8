using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.Rendering
{
    /// <summary>
    /// 	<para>${core_IRender_Title}</para>
    /// 	<para>${core_IRender_Description}</para>
    /// </summary>
    public interface IRenderer
    {
        /// <summary>${core_IRender_method_getStyle_D}</summary>
        /// <returns>${core_IRender_method_getStyle_return}</returns>
        /// <param name="feature">${core_IRender_method_getStyle_param_feature}</param>
        Style GetStyle(Feature feature);
    }
}

