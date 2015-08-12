using System;
using System.Windows;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// 	<para>${WP_mapping_MouseClickEventArgs_Title}</para>
    /// 	<para>${WP_mapping_MouseClickEventArgs_Description}</para>
    /// </summary>
    public class MouseClickEventArgs : EventArgs
    {
        /// <summary>${WP_mapping_MouseClickEventArgs_attribute_Handled_D}</summary>
        public bool Handled { get; set; }
        /// <summary>${WP_mapping_MouseClickEventArgs_attribute_MapPoint_D}</summary>
        public Point2D MapPoint { get; set; }
        /// <summary>${WP_mapping_MouseClickEventArgs_attribute_ScreenPoint_D}</summary>
        public Point ScreenPoint { get; set; }
    }
}
