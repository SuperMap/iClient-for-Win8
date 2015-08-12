using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_Feature_Title}</para>
    /// 	<para>${core_Feature_Description}</para>
    /// </summary>
    /// <remarks>${core_Feature_Description_Remarks}</remarks>
    [ContentProperty(Name = "Geometry")]
    public class Feature : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>${core_Feature_field_selectedProperty_D}</summary>
        /// <remarks>${core_dependencyProperty_Remarks}</remarks>
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(Feature), new PropertyMetadata(false, new PropertyChangedCallback(Feature.OnSelectedPropertyChanged)));
        /// <summary>${core_Feature_attribute_Selected_D}</summary>
        public bool Selected
        {
            get
            {
                return (bool)base.GetValue(SelectedProperty);
            }
            set
            {
                base.SetValue(SelectedProperty, value);
            }
        }
        private static void OnSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Feature feature = d as Feature;
            if (feature != null)
            {
                feature.ChangeVisualState(true);
                feature.RaisePropertyChanged("Selected");
            }
        }
        /// <summary>${core_Feature_method_Select_D}</summary>
        public void Select()
        {
            base.SetValue(SelectedProperty, true);
        }
        /// <summary>${core_Feature_method_UnSelect_D}</summary>
        public void UnSelect()
        {
            base.SetValue(SelectedProperty, false);
        }

        internal BindingInfo dataContext = new BindingInfo();
        private WeakReference elementReference;
        private SuperMap.WinRT.Core.Geometry geometry;
        private SuperMap.WinRT.Core.Style style;
        private int zIndex;

        internal Visibility Visibility
        {
            get
            {
                return (Visibility)base.GetValue(VisibilityProperty);
            }
            set
            {
                base.SetValue(VisibilityProperty, value);
            }
        }
        internal static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(Feature), new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(Feature.OnVisibilityPropertyChanged)));
        private static void OnVisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FeatureElement elementReference = (d as Feature).GetElementReference();
            if (elementReference != null)
            {
                elementReference.Visibility = (Visibility)e.NewValue;
                if (elementReference.Visibility == Visibility.Visible)
                {
                    elementReference.SetPath();
                }
            }
        }

        /// <summary>${core_Feature_event_PointerEntered_D}</summary>
        public event PointerEventHandler PointerEntered;

        /// <summary>${core_Feature_event_PointerExited_D}</summary>
        public event PointerEventHandler PointerExited;

        /// <summary>${core_Feature_event_PointerMoved_D}</summary>
        public event PointerEventHandler PointerMoved;

        /// <summary>${core_Feature_event_PointerPressed_D}</summary>
        public event PointerEventHandler PointerPressed;

        /// <summary>${core_Feature_event_PointerReleased_D}</summary>
        public event PointerEventHandler PointerReleased;

        /// <summary>${core_Feature_event_PropertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>${core_Feature_event_AttributeValueChanged_D}</summary>
        public event EventHandler<DictionaryChangedEventArgs> AttributeValueChanged;

        /// <summary>${core_Feature_constructor_None_D}</summary>
        public Feature()
        {
            this.Attributes = new ObservableDictionary();
        }

        private void ChangeVisualState(bool useTransition)
        {
            FeatureElement elementReference = this.GetElementReference();
            if (elementReference != null)
            {
                elementReference.ChangeVisualState(useTransition);
            }
        }

        private void geometry_GeometryChanged(object sender, EventArgs e)
        {
            this.RaisePropertyChanged("Geometry");
        }

        internal FeatureElement GetElementReference()
        {
            if ((this.elementReference != null) && this.elementReference.IsAlive)
            {
                return (this.elementReference.Target as FeatureElement);
            }
            return null;
        }
        internal void SetElementReference(FeatureElement element)
        {
            if (element == null)
            {
                this.ElementReference = null;
            }
            else
            {
                this.ElementReference = new WeakReference(element);
                Canvas.SetZIndex(element, this.zIndex);
                if (this.geometry != null)
                {
                    SuperMap.WinRT.Mapping.LayerContainer.SetBounds(element, this.geometry.Bounds);
                }
            }
        }

        /// <summary>${core_Feature_method_GetZIndex_D}</summary>
        /// <returns>${core_Feature_method_GetZIndex_return}</returns>
        public int GetZIndex()
        {
            return this.zIndex;
        }
        /// <summary>${core_Feature_method_SetZIndex_D}</summary>
        /// <param name="value">${core_Feature_method_SetZIndex_param_value}</param>
        public void SetZIndex(int value)
        {
            this.zIndex = value;
            FeatureElement elementReference = this.GetElementReference();
            if (elementReference != null)
            {
                Canvas.SetZIndex(elementReference, value);
            }
        }

        internal void RaisePointerEntered(PointerRoutedEventArgs args)
        {
            if (this.PointerEntered != null)
            {
                this.PointerEntered(this, args);
            }
        }
        internal void RaisePointerExited(PointerRoutedEventArgs args)
        {
            if (this.PointerExited != null)
            {
                this.PointerExited(this, args);
            }
        }
        internal void RaisePointerMoved(PointerRoutedEventArgs args)
        {
            if (this.PointerMoved != null)
            {
                this.PointerMoved(this, args);
            }
        }
        internal void RaisePointerPressed(PointerRoutedEventArgs args)
        {
            if (this.PointerPressed != null)
            {
                this.PointerPressed(this, args);
            }
        }
        internal void RaisePointerReleased(PointerRoutedEventArgs args)
        {
            if (this.PointerReleased != null)
            {
                this.PointerReleased(this, args);
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void SetBoundedStyle(SuperMap.WinRT.Core.Style style)
        {
            if (this.dataContext.Style != style)
            {
                this.dataContext.Style = style;
                this.RaisePropertyChanged("Style");
                this.SetElementReference(null);
            }
        }

        /// <summary>${core_Feature_attribute_ToolTip_D}</summary>
        /// <remarks>${core_Feature_attribute_ToolTip_Remarks}</remarks>
        public FrameworkElement ToolTip { get; set; }
        internal bool DisableToolTip { get; set; }

        internal event EventHandler ElementReferenceChanging;
        private WeakReference ElementReference
        {
            get
            {
                return this.elementReference;
            }
            set
            {
                if (ElementReferenceChanging != null)
                {
                    ElementReferenceChanging(this, EventArgs.Empty);
                }
                if (((this.elementReference != null) && this.elementReference.IsAlive) && (this.elementReference.Target != null))
                {
                    FeatureElement target = this.elementReference.Target as FeatureElement;
                    if (target.GeoStyle != null)
                    {
                        target.GeoStyle.PropertyChanged -= new PropertyChangedEventHandler(style_PropertyChanged);
                    }
                    if (target.Parent != null)
                    {
                        (target.Parent as Panel).Children.Remove(target);
                    }
                    if (target.Template != null)
                    {
                        target.Template = null;
                    }
                    target.DataContext = null;
                    (this.elementReference.Target as FrameworkElement).DataContext = null;
                }
                this.elementReference = value;
                if (((this.elementReference != null) && this.elementReference.IsAlive) && (this.elementReference.Target != null))
                {
                    (this.elementReference.Target as FrameworkElement).DataContext = this.dataContext;
                    FeatureElement fe = this.elementReference.Target as FeatureElement;
                    if (fe.GeoStyle != null)
                    {
                        fe.GeoStyle.PropertyChanged += new PropertyChangedEventHandler(style_PropertyChanged);
                    }
                }
            }
        }

        /// <summary>${core_Feature_attribute_Attributes_D}</summary>
        /// <remarks>${core_Feature_attribute_Attributes_Remarks}</remarks>
        /// <example>
        /// 	<code lang="CS">
        /// 		<![CDATA[
        /// //${core_Feature_Example_comment_1}
        /// feature.Attributes.Add("Name", Tom);
        ///  
        /// //${core_Feature_Example_comment_2}
        /// String name;
        /// name = feature.Attributes["Name"].ToString();]]>
        /// 	</code>
        /// </example>
        public IDictionary<string, object> Attributes
        {
            get
            {
                return this.dataContext.Attributes;
            }
            internal set
            {
                if (!(value is ObservableDictionary))
                {
                    throw new ArgumentException("Attributes");
                }
                if (this.dataContext.Attributes != value)
                {
                    if (this.dataContext.Attributes != null)
                    {
                        ((ObservableDictionary)this.dataContext.Attributes).ValueChanged -= Feature_ValueChanged;
                    }
                    this.dataContext.Attributes = value;
                    if (this.dataContext.Attributes != null)
                    {
                        ((ObservableDictionary)this.dataContext.Attributes).ValueChanged += Feature_ValueChanged;
                    }
                    this.RaisePropertyChanged("Attributes");
                }
            }
        }

        private void Feature_ValueChanged(object sender, DictionaryChangedEventArgs e)
        {
            EventHandler<DictionaryChangedEventArgs> attributeValueChanged = this.AttributeValueChanged;
            if (attributeValueChanged != null)
            {
                attributeValueChanged(this, e);
            }
        }
        /// <summary>${core_Feature_attribute_geometry_D}</summary>
        public SuperMap.WinRT.Core.Geometry Geometry
        {
            get
            {
                return this.geometry;
            }
            set
            {
                if (this.geometry != value)
                {
                    if (this.geometry != null)
                    {
                        this.geometry.GeometryChanged -= new EventHandler(this.geometry_GeometryChanged);
                    }
                    this.geometry = value;
                    if (this.geometry != null)
                    {
                        this.geometry.GeometryChanged += new EventHandler(this.geometry_GeometryChanged);
                    }
                    this.RaisePropertyChanged("Geometry");
                }
            }
        }
        /// <summary>${core_Feature_attribute_style_D}</summary>
        public SuperMap.WinRT.Core.Style Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    if (this.style != null && this.elementReference != null)
                    {
                        style.PropertyChanged -= new PropertyChangedEventHandler(style_PropertyChanged);
                    }

                    if (this.dataContext.Style != this.style)
                    {
                        this.style = value;
                    }
                    else
                    {
                        this.style = value;
                        this.SetBoundedStyle(value);
                    }
                }
            }
        }
        private void style_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((sender is MarkerStyle) && ((e.PropertyName == "OffsetX") || (e.PropertyName == "OffsetY")))
            {
                FeatureElement elementReference = this.GetElementReference();
                if (elementReference != null)
                {
                    MarkerStyle style = (MarkerStyle)sender;
                    TranslateTransform renderTransform = elementReference.RenderTransform as TranslateTransform;
                    renderTransform.X = -style.OffsetX;
                    renderTransform.Y = -style.OffsetY;
                }
            }
            else if (e.PropertyName == "ControlTemplate")
            {
                FeatureElement element2 = this.GetElementReference();
                if (element2 != null)
                {
                    element2.Template = this.Style.ControlTemplate;
                    //TODO:
                    //this.RaisePropertyChanged("Style");
                }
            }
        }

        internal SuperMap.WinRT.Mapping.FeaturesLayer Layer
        {
            get
            {
                return (base.GetValue(SuperMap.WinRT.Mapping.FeaturesLayer.FeaturesLayerProperty) as SuperMap.WinRT.Mapping.FeaturesLayer);
            }
        }
        /// <summary>${core_Feature_method_Clone_D}</summary>
        public Feature Clone()
        {
            Feature fClone = new Feature();
            fClone.Geometry = this.Geometry.Clone();
            fClone.Style = this.Style;
            fClone.ToolTip = this.ToolTip;
            fClone.DisableToolTip = this.DisableToolTip;
            fClone.Attributes = this.Attributes;

            return fClone;
        }
    }
}

