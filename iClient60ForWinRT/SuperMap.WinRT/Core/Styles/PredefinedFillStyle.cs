using Windows.UI.Xaml.Controls;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_PredefineFillStyle_Title}</para>
    /// 	<para>${core_PredefineFillStyle_Description}</para>
    /// </summary>
    public sealed class PredefinedFillStyle : FillStyle
    {
        /// <summary>${core_PredefinedFillStyle_constructor_None_D}</summary>
        public PredefinedFillStyle()
        {
            ControlTemplate = ResourceData.Dictionary["PredefinedFillStyle"] as ControlTemplate;
        }
    }
}
