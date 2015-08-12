using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Rendering
{
    /// <summary>
    /// 	<para>${WP_mapping_RangeRender_Title}</para>
    /// 	<para>${WP_mapping_RangeRender_Description}</para>
    /// </summary>
    [ContentProperty("Items")]
    public sealed class RangeRenderer : DependencyObject, IRenderer, INotifyPropertyChanged
    {

        private string attribute;
        private SuperMap.WindowsPhone.Core.Style defaultStyle;

        /// <summary>${WP_mapping_RangeRender_event_propertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>${WP_mapping_RangeRender_constructor_None_D}</summary>
        public RangeRenderer()
        {
            Items = new ObservableCollection<RangeItem>();
            Items.CollectionChanged += (s, e) =>
            {
                this.OnPropertyChanged("Items");
            };
        }

        /// <summary>${WP_mapping_RangeRender_attribute_items_D}</summary>
        public ObservableCollection<RangeItem> Items
        {
            get { return (ObservableCollection<RangeItem>)GetValue(ItemsProperty); }
            private set { base.SetValue(ItemsProperty, value); }
        }
        private static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<RangeItem>), typeof(RangeRenderer), null);

        /// <summary>${WP_mapping_RangeRender_method_getStyle_D}</summary>
        /// <returns>${WP_mapping_RangeRender_method_getStyle_return}</returns>
        /// <param name="feature">${WP_mapping_RangeRender_method_getStyle_param_feature}</param>
        public SuperMap.WindowsPhone.Core.Style GetStyle(Feature feature)
        {
            if (feature != null && !string.IsNullOrEmpty(this.Attribute) && feature.Attributes.ContainsKey(this.Attribute))
            {
                double num;
                object obj = feature.Attributes[this.Attribute];
                if (double.TryParse(string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { obj }), NumberStyles.Any, CultureInfo.InvariantCulture, out num))
                {
                    foreach (RangeItem item in this.Items)
                    {
                        if ((num >= item.MinimumValue) && (num < item.MaximumValue))
                        {
                            if (item.Style != null)
                            {
                                return item.Style;
                            }
                            else
                            {
                                return DefaultStyle;
                            }
                        }
                    }
                    return DefaultStyle;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>${WP_mapping_RangeRender_attribute_defaultStyle_D}</summary>
        public SuperMap.WindowsPhone.Core.Style DefaultStyle
        {
            get
            {
                return this.defaultStyle;
            }
            set
            {
                if (this.defaultStyle != value)
                {
                    this.defaultStyle = value;
                    this.OnPropertyChanged("DefaultStyle");
                }
            }
        }

        /// <summary>${WP_mapping_RangeRender_attribute_attribute_D}</summary>
        public string Attribute
        {
            get
            {
                return this.attribute;
            }
            set
            {
                if (this.attribute != value)
                {
                    this.attribute = value;
                    this.OnPropertyChanged("Attribute");
                }
            }
        }
    }
}
