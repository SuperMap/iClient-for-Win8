using System;
using System.Windows;
using System.Windows.Input;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_FeatureMouseEventArgs_Title}</para>
    /// 	<para>${WP_core_FeatureMouseEventArgs_Description}</para>
    /// </summary>
    public class FeatureMouseEventArgs : EventArgs
    {
        private MouseEventArgs source;

        internal FeatureMouseEventArgs(Feature f, MouseEventArgs args)
        {
            Feature = f;
            source = args;
        }

        /// <summary>${WP_core_FeatureMouseEventArgs_method_getPosition_D}</summary>
        /// <returns>${WP_core_FeatureMouseEventArgs_method_getPosition_return}</returns>
        /// <param name="relativeTo">${WP_core_FeatureMouseEventArgs_method_getPosition_param_relativeTo}</param>
        public Point GetPosition(UIElement relativeTo)
        {
            return this.source.GetPosition(relativeTo);
        }

        /// <summary>${WP_core_FeatureMouseEventArgs_attribute_feature_D}</summary>
        public Feature Feature { get; private set; }

        /// <summary>${WP_core_FeatureMouseEventArgs_attribute_originalSource_D}</summary>
        public object OriginalSource
        {
            get
            {
                return this.source.OriginalSource;
            }
        }

        /// <summary>${WP_core_FeatureMouseEventArgs_attribute_stylusDevice_D}</summary>
        public System.Windows.Input.StylusDevice StylusDevice
        {
            get
            {
                return this.source.StylusDevice;
            }
        }
    }
}
