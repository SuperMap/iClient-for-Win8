using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace SuperMap.WinRT.Controls
{

    /// <summary>
    /// 	<para>${controls_TrackMark_Title}</para>
    /// 	<para>${controls_TrackMark_Description}</para>
    /// 	<para><img src="zoomSlider.png"/></para>
    /// </summary>
    public sealed class TrackMark : Control
    {

        /// <summary>${controls_TrackMark_constructor_None_D}</summary>
        public TrackMark()
        {
            base.IsTabStop = false;
            base.DefaultStyleKey = typeof(TrackMark);
        }
    }
}
