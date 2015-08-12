using System;
using System.ComponentModel;
using System.Windows;
using SuperMap.WinRT.Core;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SuperMap.WinRT.Mapping
{
    internal class LayerContainer : Panel
    {
        internal LayerContainer(Layer layer)
        {
            this.Layer = layer;
            base.Opacity = layer.Opacity;
            layer.PropertyChanged += new PropertyChangedEventHandler(this.layer_PropertyChanged);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement element in base.Children)
            {
                if (element.Visibility == Visibility.Visible)
                {
                    element.Measure(availableSize);
                }
            }
            return base.MeasureOverride(availableSize);
        }

        internal void ResetGeometryTransforms()
        {
            if (this.Layer is FeaturesLayer)
            {
                foreach (UIElement element in base.Children)
                {
                    if (element is FeatureElement)
                    {
                        this.ResetGeometryTransform((FeatureElement)element);
                    }
                }
                base.InvalidateMeasure();
            }
            if (this.Layer is ElementsLayer)
            {
                foreach (UIElement element in base.Children)
                {
                    if (element is ShapeElement)
                    {
                        ShapeElement elm = (ShapeElement)element;
                        this.ResetShapeBaseTransform(elm);
                    }
                }
                base.InvalidateMeasure();
            }
        }

        internal void ResetGeometryTransform(FeatureElement elm)
        {
            if (((elm.OriginX != this.Origin.X) || (elm.OriginY != this.Origin.Y)) || (elm.Resolution != this.Resolution))
            {
                elm.InvalidatePath(this.Resolution, this.OriginX, this.OriginY);
            }
        }

        internal void ResetShapeBaseTransform(ShapeElement sb)
        {
            if (((sb.OriginX != this.Origin.X) || (sb.OriginY != this.Origin.Y)) || (sb.Resolution != this.Resolution))
            {
                sb.InvalidatePath(this.Resolution, this.OriginX, this.OriginY);
            }
        }

        private void layer_PropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            Layer layer = sender as Layer;
            if (e.PropertyName == "IsVisible")
            {
                if (!layer.IsVisible)
                {
                    layer.Container.Visibility = Visibility.Collapsed;
                }
                else if (((layer.Container.Resolution < layer.MinVisibleResolution) && !double.IsNaN(layer.MinVisibleResolution)) || ((layer.Container.Resolution > layer.MaxVisibleResolution) && !double.IsNaN(layer.MaxVisibleResolution)))
                {
                    layer.Container.Visibility = Visibility.Collapsed;
                }
                else
                {
                    layer.Container.Visibility = Visibility.Visible;
                }
            }
            else if (e.PropertyName == "Opacity")
            {
                base.Opacity = layer.Opacity;
            }
        }

        public Point2D Origin
        {
            get
            {
                return new Point2D(this.OriginX, this.OriginY);
            }
        }
        private static void OnOriginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LayerContainer container = d as LayerContainer;
            if (container != null)
            {
                container.InvalidateArrange();
            }
        }//X,Y共用

        public static readonly DependencyProperty OriginXProperty = DependencyProperty.Register("OriginX", typeof(double), typeof(LayerContainer), new PropertyMetadata(0.0, new PropertyChangedCallback(OnOriginPropertyChanged)));
        public double OriginX
        {
            get
            {
                return (double)base.GetValue(OriginXProperty);
            }
            set
            {
                base.SetValue(OriginXProperty, value);
            }
        }

        public static readonly DependencyProperty OriginYProperty = DependencyProperty.Register("OriginY", typeof(double), typeof(LayerContainer), new PropertyMetadata(0.0, new PropertyChangedCallback(OnOriginPropertyChanged)));
        public double OriginY
        {
            get
            {
                return (double)base.GetValue(OriginYProperty);
            }
            set
            {
                base.SetValue(OriginYProperty, value);
            }
        }

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(double), typeof(LayerContainer), new PropertyMetadata(0.0, new PropertyChangedCallback(OnResolutionPropertyChanged)));
        public double Resolution
        {
            get
            {
                return (double)base.GetValue(ResolutionProperty);
            }
            set
            {
                base.SetValue(ResolutionProperty, value);
            }
        }
        private static void OnResolutionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LayerContainer container = d as LayerContainer;

            if (container != null)
            {
                if ((container.Layer != null) && container.Layer.IsVisible)
                {
                    //在这里判断当分辨率发生变化，是否超出最大，最小可见比例尺的可见性；
                    //设置了最大、小可见分辨率！
                    bool isCollapsed = false;
                    if (container.Layer.MinVisibleResolution != double.Epsilon || !double.IsPositiveInfinity(container.Layer.MaxVisibleResolution))
                    {
                        isCollapsed = (container.Resolution < container.Layer.MinVisibleResolution) || (container.Resolution > container.Layer.MaxVisibleResolution);
                    }  //如果设置了分辨率；
                    else if (container.Layer.Map != null)
                    {
                        isCollapsed = container.Layer.Map.Scale < container.Layer.MinVisibleScale || container.Layer.Map.Scale > container.Layer.MaxVisibleScale;
                    }//如果设置了比例尺; 主要为了在Map初始化完成之前设置了layer的最大最小比例尺

                    if (isCollapsed)
                    {
                        container.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        container.Visibility = Visibility.Visible;
                    }

                }
                container.InvalidateArrange();
            }
        }

        //每个Element的bbox
        #region Bounds (Attached DependencyProperty)

        internal static readonly DependencyProperty BoundsProperty =
            DependencyProperty.RegisterAttached("Bounds", typeof(Rectangle2D), typeof(LayerContainer), new PropertyMetadata(Rectangle2D.Empty, new PropertyChangedCallback(OnBoundsChanged)));
        public static void SetBounds(DependencyObject o, Rectangle2D value)
        {
            o.SetValue(BoundsProperty, value);
        }
        public static Rectangle2D GetBounds(DependencyObject o)
        {
            return (Rectangle2D)o.GetValue(BoundsProperty);
        }
        private static void OnBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement)
            {
                FrameworkElement element = (FrameworkElement)d;
                LayerContainer parent = element.Parent as LayerContainer;
                if ((parent != null) && (parent.Layer is ElementsLayer))
                {
                    Rectangle2D oldValue = (Rectangle2D)e.OldValue;
                    Rectangle2D newValue = (Rectangle2D)e.NewValue;
                    if (Rectangle2D.IsNullOrEmpty(newValue))
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                    else if (!Rectangle2D.IsNullOrEmpty(oldValue) && oldValue.Width == newValue.Width && oldValue.Height == newValue.Height)
                    {
                        parent.InvalidateArrange();
                    }
                    else
                    {
                        parent.InvalidateMeasure();
                    }
                }
            }
        }

        #endregion

        internal Layer Layer { get; private set; }
        
    }
}
