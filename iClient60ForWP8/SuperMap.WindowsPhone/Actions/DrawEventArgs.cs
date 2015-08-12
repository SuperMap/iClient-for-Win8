using System;
using System.Windows;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Actions
{
    /// <summary>
    /// 	<para>${ui_action_DrawEventArgs_Title}。</para>
    /// 	<para>${ui_action_DrawEventArgs_Description}</para>
    /// </summary>
    public class DrawEventArgs : EventArgs
    {
        /// <summary>${ui_action_DrawEventArgs_constructor_None_D}</summary>
        public DrawEventArgs()
        { }

        /// <summary>${ui_action_DrawEventArgs_attribute_DrawName_D}</summary>
        public string DrawName { get; set; }
        /// <summary>${ui_action_DrawEventArgs_attribute_Element_D}</summary>
        /// <example>
        /// 	<code lang="CS">
        /// this.arbitraryLayer.Children.Add(e.Element as PolygonBase);
        /// </code>
        /// </example>
        public UIElement Element { get; set; }
        /// <example>
        /// 	<code lang="CS">
        /// Feature feature = new Feature() { Geometry = e.Geometry as GeoRegion, Style = this.GreenFillStyle };
        /// this.featuresLayer.Features.Add(feature);
        /// </code>
        /// </example>
        /// <summary>${ui_action_DrawEventArgs_attribute_Geometry_D}</summary>
        public Geometry Geometry { get; set; }

        //标识draw过程中是否取消，如果取消则为true，否则为false
        public bool Canceled { get; internal set; }
    }
}
