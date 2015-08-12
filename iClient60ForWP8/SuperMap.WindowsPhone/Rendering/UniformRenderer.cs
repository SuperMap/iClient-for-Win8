using System.ComponentModel;
using System.Windows;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Rendering
{
    /// <summary>
    /// 	<para>${WP_mapping_UniformRenderer_Title}</para>
    /// 	<para>${WP_mapping_UniformRenderer_Description}</para>
    /// </summary>
    public sealed class UniformRenderer : DependencyObject, IRenderer, INotifyPropertyChanged
    {
        private MarkerStyle markerStyle;
        private LineStyle lineStyle;
        private FillStyle fillStyle;

        /// <summary>${WP_mapping_UniformRenderer_event_propertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>${WP_mapping_UniformRenderer_constructor_None_D}</summary>
        public UniformRenderer()
        { }

        /// <summary>${WP_mapping_UniformRenderer_method_GetStyle_D}</summary>
        /// <returns>${WP_mapping_UniformRenderer_method_GetStyle_return}</returns>
        /// <param name="feature">${WP_mapping_UniformRenderer_method_GetStyle_param_feature}</param>
        public SuperMap.WindowsPhone.Core.Style GetStyle(Feature feature)
        {
            if (feature == null)
            {
                return null;
            }
            if (feature.Geometry is GeoPoint)
            {
                return MarkerStyle;
            }
            else if (feature.Geometry is GeoLine)
            {
                return LineStyle;
            }
            return this.FillStyle;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>${WP_mapping_UniformRenderer_attribute_MarkerStyle_D}</summary>
        public MarkerStyle MarkerStyle
        {
            get
            {
                return this.markerStyle;
            }
            set
            {
                if (this.markerStyle != value)
                {
                    this.markerStyle = value;
                    this.OnPropertyChanged("MarkerStyle");
                }
            }
        }

        /// <summary>${WP_mapping_UniformRenderer_attribute_LineStyle_D}</summary>
        public LineStyle LineStyle
        {
            get
            {
                return this.lineStyle;
            }
            set
            {
                if (this.lineStyle != value)
                {
                    this.lineStyle = value;
                    this.OnPropertyChanged("LineStyle");
                }
            }
        }

        /// <summary>${WP_mapping_UniformRenderer_attribute_FillStyle_D}</summary>
        public FillStyle FillStyle
        {
            get
            {
                return this.fillStyle;
            }
            set
            {
                if (this.fillStyle != value)
                {
                    this.fillStyle = value;
                    this.OnPropertyChanged("FillStyle");
                }
            }
        }
    }
}
