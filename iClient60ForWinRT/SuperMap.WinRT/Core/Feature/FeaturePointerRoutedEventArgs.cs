using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// ${core_FeaturePointerRoutedEventArgs_title}
    /// </summary>
    public class FeaturePointerRoutedEventArgs : EventArgs
    {
        private PointerRoutedEventArgs _source;
        /// <summary>
        /// ${core_FeaturePointerRoutedEventArgs_constructor_D}
        /// </summary>
        public FeaturePointerRoutedEventArgs(Feature feature, PointerRoutedEventArgs args)
        {
            _source = args;
            Feature = feature;
        }
        /// <summary>
        /// ${core_FeaturePointerRoutedEventArgs_attribute_Feature_D}
        /// </summary>
        public Feature Feature
        {
            get;
            private set;
        }
        /// <summary>
        /// ${core_FeaturePointerRoutedEventArgs_attribute_Handler_D}
        /// </summary>
        public bool Handler
        {
            get { return _source.Handled; }
            set { _source.Handled = value; }
        }
        /// <summary>
        /// ${core_FeaturePointerRoutedEventArgs_method_GetCurrentPoint_D}
        /// </summary>
        public PointerPoint GetCurrentPoint(UIElement element)
        {
            return _source.GetCurrentPoint(element);
        }
        /// <summary>
        /// ${core_FeaturePointerRoutedEventArgs_attribute_KeyModifiers_D}
        /// </summary>
        public VirtualKeyModifiers KeyModifiers
        {
            get { return _source.KeyModifiers; }
        }
        /// <summary>
        /// ${core_FeaturePointerRoutedEventArgs_attribute_Pointer_D}
        /// </summary>
        public Pointer Pointer 
        {
            get { return _source.Pointer; }
        }
        /// <summary>
        /// ${core_FeaturePointerRoutedEventArgs_method_GetIntermediatePoints_D}
        /// </summary>
        public IList<PointerPoint> GetIntermediatePoints(UIElement relativeTo)
        {
            return _source.GetIntermediatePoints(relativeTo);
        }
        /// <summary>
        /// ${core_FeaturePointerRoutedEventArgs_attribute_OriginalSource_D}
        /// </summary>
        public object OriginalSource
        {
            get { return _source.OriginalSource; }
        }
    }
}
