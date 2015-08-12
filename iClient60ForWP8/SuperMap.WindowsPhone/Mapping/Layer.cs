using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// <para>${WP_mapping_Layer_Title}</para>
    /// <para>${WP_mapping_Layer_Description}</para>
    /// </summary>
    public abstract class Layer : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>
        ///     ${WP_pubilc_fields_identifies_sl} <see cref="ID">ID</see>
        ///     ${WP_pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty IDProperty = DependencyProperty.Register("ID", typeof(string), typeof(Layer), null);
        /// <summary>${WP_mapping_Layer_attribute_id_D}</summary>
        public string ID
        {
            get
            {
                return (string)base.GetValue(IDProperty);
            }
            set
            {
                base.SetValue(IDProperty, value);
            }
        }

        /// <summary>
        ///     ${WP_pubilc_fields_identifies_sl} <see cref="MaxVisibleResolution">MaxVisibleResolution</see>
        ///     ${WP_pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty MaxVisibleResolutionProperty = DependencyProperty.Register("MaxVisibleResolution", typeof(double), typeof(Layer), new PropertyMetadata(double.PositiveInfinity));
        /// <summary>${WP_mapping_Layer_attribute_maxVisibleResolution_D}</summary>
        public double MaxVisibleResolution
        {
            get
            {
                return (double)base.GetValue(MaxVisibleResolutionProperty);
            }
            set
            {
                base.SetValue(MaxVisibleResolutionProperty, value);
                this.OnPropertyChanged("MaxVisibleResolution");
            }
        }

        /// <summary>
        ///     ${WP_pubilc_fields_identifies_sl} <see cref="MinVisibleResolution">MinVisibleResolution</see>
        ///     ${WP_pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty MinVisibleResolutionProperty = DependencyProperty.Register("MinVisibleResolution", typeof(double), typeof(Layer), new PropertyMetadata(double.Epsilon));
        /// <summary>${WP_mapping_Layer_attribute_minVisibleResolution_D}</summary>
        public double MinVisibleResolution
        {
            get
            {
                return (double)base.GetValue(MinVisibleResolutionProperty);
            }
            set
            {
                base.SetValue(MinVisibleResolutionProperty, value);
                this.OnPropertyChanged("MinVisibleResolution");
            }
        }

        /// <summary>
        ///     ${WP_pubilc_fields_identifies_sl} <see cref="MinVisibleScale">MinVisibleScale</see>
        ///     ${WP_pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty MinVisibleScaleProperty = DependencyProperty.Register("MinVisibleScale", typeof(double), typeof(Layer), new PropertyMetadata(double.Epsilon));
        /// <summary>${WP_mapping_Layer_attribute_minScale_D}</summary>
        public double MinVisibleScale
        {
            get
            {
                return (double)base.GetValue(MinVisibleScaleProperty);
            }
            set
            {
                base.SetValue(MinVisibleScaleProperty, value);
                this.OnPropertyChanged("MinVisibleScale");
                if (Dpi != 0.0)
                {
                    MaxVisibleResolution = ScaleHelper.ScaleConversion(value, this.Dpi, this.CRS);
                }//set的时候管，get就不管了，就是说没有set就get默认
            }
        }

        /// <summary>
        ///     ${WP_pubilc_fields_identifies_sl} <see cref="MaxVisibleScale">MaxVisibleScale</see>
        ///     ${WP_pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty MaxVisibleScaleProperty = DependencyProperty.Register("MaxVisibleScale", typeof(double), typeof(Layer), new PropertyMetadata(double.PositiveInfinity));
        /// <summary>${WP_mapping_Layer_attribute_maxScale_D}</summary>
        public double MaxVisibleScale
        {
            get
            {
                return (double)base.GetValue(MaxVisibleScaleProperty);
            }
            set
            {
                base.SetValue(MaxVisibleScaleProperty, value);
                this.OnPropertyChanged("MaxVisibleScale");
                if (Dpi != 0.0)
                {
                    MinVisibleResolution = ScaleHelper.ScaleConversion(value, this.Dpi, this.CRS);
                }
            }
        }

        /// <summary>
        ///     ${WP_pubilc_fields_identifies_sl} <see cref="Opacity">Opacity</see>
        ///     ${WP_pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(double), typeof(Layer), new PropertyMetadata(1.0, new PropertyChangedCallback(OnOpacityPropertyChanged)));
        /// <summary>${WP_mapping_Layer_attribute_Opacity_D} </summary>
        public double Opacity
        {
            get
            {
                return (double)base.GetValue(OpacityProperty);
            }
            set
            {
                base.SetValue(OpacityProperty, value);
            }
        }
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Layer layer = d as Layer;
            if ((layer != null) && (e.NewValue != e.OldValue))
            {
                layer.OnPropertyChanged("Opacity");
            }
        }

        /// <summary>
        ///     ${WP_pubilc_fields_identifies_sl} <see cref="IsVisible">IsVisible</see>
        ///     ${WP_pubilc_fields_attachedproperty_sl}
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(bool), typeof(Layer), new PropertyMetadata(true, new PropertyChangedCallback(OnIsVisiblePropertyChanged)));
        /// <summary>${WP_mapping_Layer_attribute_isVisible_D}</summary>
        public bool IsVisible
        {
            get
            {
                return (bool)base.GetValue(IsVisibleProperty);
            }
            set
            {
                base.SetValue(IsVisibleProperty, value);
            }
        }
        private static void OnIsVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Layer layer = d as Layer;
            if ((layer != null) && (e.NewValue != e.OldValue))
            {
                layer.OnPropertyChanged("IsVisible");
                if (!((bool)e.NewValue))
                {
                    if (layer.IsInitialized)
                    {
                        layer.Cancel();
                    }
                }
                else
                {
                    layer.OnLayerChanged();
                }
            }
        }

        internal int progress;
        internal double progressWeight;

        //后续：事件的On函数可以健壮一下
        /// <summary>${WP_mapping_Layer_event_PropertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>${WP_mapping_Layer_event_failed_D}</summary>
        public event EventHandler<LayerFailedEventArgs> Failed;

        /// <summary>${WP_mapping_Layer_event_initialized_D}</summary>
        public event EventHandler<EventArgs> Initialized;

        internal delegate void LayerChangedHandler(object sender, EventArgs args);
        internal event LayerChangedHandler LayerChanged;
        /// <summary>${WP_mapping_Layer_event_Progress_D}</summary>
        public event EventHandler<ProgressEventArgs> Progress;

        /// <summary>${WP_mapping_Layer_constructor_None_D}</summary>
        protected Layer()
        {
            progress = 100;
            progressWeight = 1.0;
            Container = new LayerContainer(this);
            Metadata = new Dictionary<string, string>();

            ViewBounds = Rectangle2D.Empty;
            LayerOrigin = Point2D.Empty;
        }

        internal virtual void Draw(DrawParameter drawParameter)
        {
            Resolution = drawParameter.Resolution;
            ViewBounds = drawParameter.ViewBounds;
            ViewSize = drawParameter.ViewSize;
            LayerOrigin = drawParameter.LayerOrigin;
        }

        /// <summary>${WP_mapping_Layer_method_initialize_D} </summary>
        public virtual void Initialize()
        {
            if (!this.IsInitialized)
            {
                this.IsInitialized = true;
                this.OnInitialized(new EventArgs());
                if ((this.Error != null))
                {
                    if ((this.Failed != null))
                    {
                        OnFailed(this.Error);
                    }
                    else if (!DesignerProperties.GetIsInDesignMode(this))
                    {
                        throw Error;
                    }
                }
            }
        }
        /// <summary>${WP_mapping_Layer_method_OnFailed_D} </summary>
        protected void OnFailed(Exception e)
        {
            if ((this.Failed != null))
            {
                this.Failed(this, new LayerFailedEventArgs(e));
            }
        }

        internal void OnInitialized(EventArgs e)
        {
            EventHandler<EventArgs> temp = this.Initialized;
            if (temp != null)
            {
                this.Initialized(this, e);
            }
        }

        /// <summary>${WP_mapping_Layer_method_toBitMap_D}</summary>
        public WriteableBitmap ToBitmap()
        {
            if (((this.Map == null) || double.IsNaN(this.Map.ActualWidth)) || ((double.IsNaN(this.Map.ActualHeight) || (this.Map.ActualWidth < 1.0)) || (this.Map.ActualWidth < 1.0)))
            {
                return null;
            }
            WriteableBitmap bitmap = new WriteableBitmap((int)this.Map.ActualWidth, (int)this.Map.ActualHeight);
            Point point = this.Container.TransformToVisual(this.Map).Transform(new Point(0.0, 0.0));
            TranslateTransform transform = new TranslateTransform { X = point.X, Y = point.Y };
            bitmap.Render(this.Container, transform);
            bitmap.Invalidate();
            return bitmap;
        }

        /// <summary>${WP_mapping_Layer_method_Cancel_D}</summary>
        protected virtual void Cancel()
        {
        }
        //Cancel用于子类覆盖，CancelLoad()用于其他类调用
        internal void CancelLoad()
        {
            this.Cancel();
        }

        /// <summary>${WP_mapping_Layer_method_refresh_D}</summary>
        public virtual void Refresh()
        { }
        /// <summary>${WP_mapping_Layer_method_param_OnLayerChanged}</summary>
        protected void OnLayerChanged()
        {
            if (this.LayerChanged != null)
            {
                this.LayerChanged(this, new EventArgs());
            }
        }
        /// <summary>${WP_mapping_Layer_method_param_OnProgress}</summary>
        protected void OnProgress(int progress)
        {
            this.progress = progress;
            if (this.Progress != null)
            {
                this.Progress(this, new ProgressEventArgs(progress));
            }
        }

        /// <summary>${WP_mapping_Layer_method_param_OnPropertyChanged}</summary>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>${WP_mapping_Layer_attribute_error_D}</summary>
        public Exception Error { get; protected set; }
        /// <summary>${WP_mapping_Layer_attribute_isInitialized_D}</summary>
        public bool IsInitialized { get; protected set; }
        /// <summary>${WP_mapping_Layer_attribute_CRS_D}</summary>
        public CoordinateReferenceSystem CRS { get; set; }
        ///// <summary>${WP_mapping_Layer_attribute_description_D}</summary>
        //public string Description { get; set; }
        ///// <summary>${WP_mapping_Layer_attribute_caption_D}</summary>
        //public string Caption { get; set; }
        /// <summary>${WP_mapping_Layer_attribute_metaData_D}</summary>
        public Dictionary<string, string> Metadata { get; set; }
        /// <summary>${WP_mapping_Layer_attribute_url_D}</summary>
        public string Url { get; set; }
        private Rectangle2D bounds = Rectangle2D.Empty;
        /// <summary>${WP_mapping_Layer_attribute_bounds_D}</summary>
        public virtual Rectangle2D Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;
            }
        }
        /// <summary>${WP_mapping_Layer_attribute_param_Dpi}</summary>
        protected virtual double Dpi { get; set; }
        internal double GetDpi() { return Dpi; }
        /// <summary>${WP_mapping_Layer_attribute_viewBounds_D}</summary>
        public Rectangle2D ViewBounds { get; internal set; }
        /// <summary>${WP_mapping_Layer_attribute_viewSize_D}</summary>
        public Size ViewSize { get; internal set; }

        internal Point2D LayerOrigin { get; set; }
        /// <summary>${WP_mapping_Layer_attribute_resolution_D}</summary>
        public double Resolution { get; internal set; }

        internal virtual bool ContinuousDraw
        {
            get
            {
                return false;
            }
        }

        internal LayerContainer Container { get; private set; }
        //可能需要protected而非internal，如果它从Layer直接继承的话

        //只用来提高性能，没利用它的属性
        internal Map Map
        {
            get
            {
                return (base.GetValue(Map.MapProperty) as Map);
            }
        }

        //用来判断是否是以比例尺为中心来算dpi；
        /// <summary>${WP_mapping_Layer_attribute_IsScaleCentric_D}</summary>
        public virtual bool IsScaleCentric
        {
            get
            {
                return false;
            }
        }
    }
}
