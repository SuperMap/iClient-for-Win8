using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SuperMap.WindowsPhone.Core
{    
    /// <summary>
    /// 	<para>${WP_core_FeatureGestureEventArgs_Title}</para>
    /// 	<para>${WP_core_FeatureGestureEventArgs_Description}</para>
    /// </summary>
    public class FeatureGestureEventArgs:EventArgs
    {
        private GestureEventArgs source;

        internal FeatureGestureEventArgs(Feature f, GestureEventArgs args)
        {
            Feature = f;
            source = args;
        }

        /// <summary>${WP_core_FeatureGestureEventArgs_method_getPosition_D}</summary>
        /// <returns>${WP_core_FeatureGestureEventArgs_method_getPosition_return}</returns>
        /// <param name="relativeTo">${WP_core_FeatureGestureEventArgs_method_getPosition_param_relativeTo}</param>
        public Point GetPosition(UIElement relativeTo)
        {
            return this.source.GetPosition(relativeTo);
        }

        /// <summary>${WP_core_FeatureGestureEventArgs_attribute_feature_D}</summary>
        public Feature Feature { get; private set; }

        /// <summary>${WP_core_FeatureGestureEventArgs_attribute_originalSource_D}</summary>
        public object OriginalSource
        {
            get
            {
                return this.source.OriginalSource;
            }
        }

        /// <summary>${WP_core_FeatureGestureEventArgs_attribute_handled_D}</summary>
        public bool Handled
        {
            get
            {
                return this.source.Handled;
            }
            set
            {
                this.source.Handled = value;
            }
        }
    }
}