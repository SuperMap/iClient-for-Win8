using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_Feature_Title}</para>
    /// 	<para>${WP_core_Feature_Description}</para>
    /// </summary>
    /// <remarks>${WP_core_Feature_Description_Remarks}</remarks>
    [ContentProperty("Geometry")]
    public class Feature : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>${WP_core_Feature_field_selectedProperty_D}</summary>
        /// <remarks>${WP_core_dependencyProperty_Remarks}</remarks>
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(Feature), new PropertyMetadata(false, new PropertyChangedCallback(Feature.OnSelectedPropertyChanged)));
        /// <summary>${WP_core_Feature_attribute_Selected_D}</summary>
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
        /// <summary>${WP_core_Feature_method_UnSelect_D}</summary>
        public void UnSelect()
        {
            base.SetValue(SelectedProperty, false);
        }

        internal BindingInfo dataContext = new BindingInfo();
        private WeakReference elementReference;
        private SuperMap.WindowsPhone.Core.Geometry geometry;
        private SuperMap.WindowsPhone.Core.Style style;
        private int zIndex;

        internal System.Windows.Visibility Visibility
        {
            get
            {
                return (System.Windows.Visibility)base.GetValue(VisibilityProperty);
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

        /// <summary>${WP_core_Feature_event_MouseEnter_D}</summary>
        public event MouseEventHandler MouseEnter;
        /// <summary>${WP_core_Feature_event_MouseLeave_D}</summary>
        public event MouseEventHandler MouseLeave;
        /// <summary>${WP_core_Feature_event_MouseMove_D}</summary>
        public event MouseEventHandler MouseMove;
        /// <summary>${WP_core_Feature_event_MouseLeftButtonDown_D}</summary>
        public event MouseButtonEventHandler MouseLeftButtonDown;
        /// <summary>${WP_core_Feature_event_MouseLeftButtonUp_D}</summary>
        public event MouseButtonEventHandler MouseLeftButtonUp;
        /// <summary>${WP_core_Feature_event_PropertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>${WP_core_Feature_event_AttributeValueChanged_D}</summary>
        public event EventHandler<DictionaryChangedEventArgs> AttributeValueChanged;

        /// <summary>
        /// ${WP_mapping_Feature_event_Tap_D}
        /// </summary>
        public event EventHandler<GestureEventArgs> Tap;
        /// <summary>
        /// ${WP_mapping_Feature_event_DoubleTap_D}
        /// </summary>
        public event EventHandler<GestureEventArgs> DoubleTap;
        /// <summary>
        /// ${WP_mapping_Feature_event_Hold_D}
        /// </summary>
        public event EventHandler<GestureEventArgs> Hold;

        /// <summary>${WP_core_Feature_constructor_None_D}</summary>
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
                    SuperMap.WindowsPhone.Mapping.LayerContainer.SetBounds(element, this.geometry.Bounds);
                }
            }
        }

        /// <summary>${WP_core_Feature_method_GetZIndex_D}</summary>
        /// <returns>${WP_core_Feature_method_GetZIndex_return}</returns>
        public int GetZIndex()
        {
            return this.zIndex;
        }
        /// <summary>${WP_core_Feature_method_SetZIndex_D}</summary>
        /// <param name="value">${WP_core_Feature_method_SetZIndex_param_value}</param>
        public void SetZIndex(int value)
        {
            this.zIndex = value;
            FeatureElement elementReference = this.GetElementReference();
            if (elementReference != null)
            {
                Canvas.SetZIndex(elementReference, value);
            }
        }

        internal void RaiseMouseEnter(MouseEventArgs args)
        {
            if (this.MouseEnter != null)
            {
                this.MouseEnter(this, args);
            }
        }
        internal void RaiseMouseLeave(MouseEventArgs args)
        {
            if (this.MouseLeave != null)
            {
                this.MouseLeave(this, args);
            }
        }
        internal void RaiseMouseMove(MouseEventArgs args)
        {
            if (this.MouseMove != null)
            {
                this.MouseMove(this, args);
            }
        }
        internal void RaiseMouseLeftButtonDown(MouseButtonEventArgs args)
        {
            if (this.MouseLeftButtonDown != null)
            {
                this.MouseLeftButtonDown(this, args);
            }
        }
        internal void RaiseMouseLeftButtonUp(MouseButtonEventArgs args)
        {
            if (this.MouseLeftButtonUp != null)
            {
                this.MouseLeftButtonUp(this, args);
            }
        }

        internal void RaiseTap(GestureEventArgs args)
        {
            if(this.Tap!=null)
            {
                this.Tap(this, args);
            }
        }

        internal void RaiseDoubleTap(GestureEventArgs args)
        {
            if (this.DoubleTap != null)
            {
                this.DoubleTap(this, args);
            }
        }

        internal void RaiseHold(GestureEventArgs args)
        {
            if (this.Hold != null)
            {
                this.Hold(this, args);
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void SetBoundedStyle(SuperMap.WindowsPhone.Core.Style style)
        {
            if (this.dataContext.Style != style)
            {
                this.dataContext.Style = style;
                this.RaisePropertyChanged("Style");
                this.SetElementReference(null);
            }
        }

        /// <summary>${WP_core_Feature_attribute_ToolTip_D}</summary>
        /// <remarks>${WP_core_Feature_attribute_ToolTip_Remarks}</remarks>
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
                        //TODO:...
                        //try
                        //{
                        //    target.Template = null;
                        //}
                        //catch
                        //{
                        //}
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

        /// <summary>${WP_core_Feature_attribute_Attributes_D}</summary>
        /// <remarks>${WP_core_Feature_attribute_Attributes_Remarks}</remarks>
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
        /// <summary>${WP_core_Feature_attribute_geometry_D}</summary>
        public SuperMap.WindowsPhone.Core.Geometry Geometry
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
        /// <summary>${WP_core_Feature_attribute_style_D}</summary>
        public SuperMap.WindowsPhone.Core.Style Style
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

        internal SuperMap.WindowsPhone.Mapping.FeaturesLayer Layer
        {
            get
            {
                return (base.GetValue(SuperMap.WindowsPhone.Mapping.FeaturesLayer.FeaturesLayerProperty) as SuperMap.WindowsPhone.Mapping.FeaturesLayer);
            }
        }
        /// <summary>${WP_core_Feature_method_Clone_D}</summary>
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

