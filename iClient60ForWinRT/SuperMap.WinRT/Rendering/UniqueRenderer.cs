using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using SuperMap.WinRT.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace SuperMap.WinRT.Rendering
{
    /// <summary>
    /// 	<para>${mapping_UniqueRender_Title}</para>
    /// 	<para>${mapping_UniqueRender_Description}</para>
    /// </summary>
    [ContentProperty(Name = "Items")]
    public sealed class UniqueRenderer : DependencyObject, IRenderer, INotifyPropertyChanged
    {
        private string attribute;
        private SuperMap.WinRT.Core.Style defaultStyle;

        /// <summary>${mapping_UniqueRender_event_propertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>${mapping_UniqueRender_constructor_None_D}</summary>
        public UniqueRenderer()
        {
            Items = new ObservableCollection<UniqueItem>();
            Items.CollectionChanged += (s, e) =>
            {
                this.OnPropertyChanged("Items");
            };
        }

        /// <summary>${mapping_UniqueRender_method_getStyle_D}</summary>
        /// <returns>${mapping_UniqueRender_method_getStyle_return}</returns>
        /// <param name="feature">${mapping_UniqueRender_method_getStyle_param_feature}</param>
        public SuperMap.WinRT.Core.Style GetStyle(Feature feature)
        {
            if (feature != null && !string.IsNullOrEmpty(this.Attribute) && feature.Attributes.ContainsKey(this.Attribute))
            {
                object obj = feature.Attributes[this.Attribute];
                foreach (UniqueItem item in this.Items)
                {
                    if ((item.Value == obj) || (((obj != null) && (item.Value != null)) && (obj.GetHashCode() == item.Value.GetHashCode())))
                    {
                        if (item.Style != null)
                        {
                            return item.Style;
                        }//设Value了但没有设置Style,返回default
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
                return null;//Attribute属性没有设置,或者设错了。
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>${mapping_UniqueRender_attribute_defaultStyle_D}</summary>
        public SuperMap.WinRT.Core.Style DefaultStyle
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

        /// <summary>${mapping_UniqueRender_attribute_attribute_D}</summary>
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


        /// <summary>${mapping_UniqueRender_attribute_items_D}</summary>
        public ObservableCollection<UniqueItem> Items
        {
            get { return (ObservableCollection<UniqueItem>)GetValue(ItemsProperty); }
            private set { base.SetValue(ItemsProperty, value); }
        }
        private static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<UniqueItem>), typeof(UniqueRenderer), null);
    }
}
