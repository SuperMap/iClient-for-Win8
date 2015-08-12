using System;
using System.Windows;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_ZoomSlider_Title}</para>
    /// 	<para>${controls_ZoomSlider_Description}</para>
    /// 	<para><img src="zoomSlider.png"/></para>
    /// </summary>
    [TemplatePart(Name = "ZoomInElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "ZoomOutElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "SliderElement", Type = typeof(SliderElement))]
    public class ZoomSlider : Control
    {
        /// <summary>${controls_ZoomSlider_field_MapProperty_D}</summary>
        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(Map), typeof(ZoomSlider), new PropertyMetadata(null,new PropertyChangedCallback(OnMapPropertyChanged)));
        /// <summary>${controls_ZoomSlider_attribute_Map_D}</summary>
        public Map Map
        {
            get
            {
                return (base.GetValue(MapProperty) as Map);
            }
            set
            {
                base.SetValue(MapProperty, value);
            }
        }
        private static void OnMapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomSlider zs = d as ZoomSlider;
            Map newValue = e.NewValue as Map;
            Map oldValue = e.OldValue as Map;

            if (oldValue != null)
            {
                oldValue.ViewBoundsChanged -= new EventHandler<ViewBoundsEventArgs>(zs.Map_ViewBoundsChanged);
                oldValue.ViewBoundsChanging -= new EventHandler<ViewBoundsEventArgs>(zs.Map_ViewBoundsChanging);
                if (oldValue.Layers != null)
                {
                    oldValue.Layers.LayersInitialized -= new EventHandler(zs.Layers_LayersInitialized);
                }
                newValue.ResolutionsChanged -= new EventHandler<ResolutionsEventArgs>(zs.newValue_ResolutionsChanged);

            }

            if (newValue != null)
            {
                newValue.ViewBoundsChanged += new EventHandler<ViewBoundsEventArgs>(zs.Map_ViewBoundsChanged);
                newValue.ViewBoundsChanging += new EventHandler<ViewBoundsEventArgs>(zs.Map_ViewBoundsChanging);
                if (newValue.Layers != null)
                {
                    newValue.Layers.LayersInitialized += new EventHandler(zs.Layers_LayersInitialized);
                }
                //后设置Map的Scales也要起作用。
                newValue.ResolutionsChanged += new EventHandler<ResolutionsEventArgs>(zs.newValue_ResolutionsChanged);
            }
        }

        private void newValue_ResolutionsChanged(object sender, ResolutionsEventArgs e)
        {
            this.SetupZoom();
        }

        private double[] mapResolutions;
        private RepeatButton zoomInElement;
        private RepeatButton zoomOutElement;
        private SliderElement sliderElement;

        /// <summary>${controls_ZoomSlider_constructor_None_D}</summary>
        public ZoomSlider()
        {
            base.DefaultStyleKey = typeof(ZoomSlider);
        }

        /// <summary>${controls_ZoomSlider_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.sliderElement = base.GetTemplateChild("SliderElement") as SliderElement;
            this.zoomInElement = base.GetTemplateChild("ZoomInElement") as RepeatButton;
            this.zoomOutElement = base.GetTemplateChild("ZoomOutElement") as RepeatButton;

            if (this.sliderElement != null)
            {
                if (this.Map != null)
                {
                    this.SetupZoom();
                }
                this.sliderElement.ValueChanged += this.ZoomSlider_ValueChanged;
            }
            if (this.zoomInElement != null)
            {
                this.zoomInElement.Click += new RoutedEventHandler(this.ZoomInButton_Click);
                this.zoomInElement.PointerExited += zoomInElement_PointerExited;
            }
            if (this.zoomOutElement != null)
            {
                this.zoomOutElement.Click += new RoutedEventHandler(this.ZoomOutButton_Click);
                this.zoomOutElement.PointerExited += zoomOutElement_PointerExited;
            }
        }

        void zoomOutElement_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.zoomOutElement.ReleasePointerCaptures();
        }

        void zoomInElement_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.zoomInElement.ReleasePointerCaptures();
        }

        private void SetupZoom()
        {
            if (Map == null || this.sliderElement == null)
            {
                return;
            }

            if (this.Map.Scales == null && this.Map.Resolutions == null && !hasTiledCachedLayer())
            {
                this.sliderElement.Visibility = Visibility.Collapsed;
            }
            else if (this.Map.Resolutions != null)
            {
                this.mapResolutions = this.Map.Resolutions;
                this.sliderElement.Visibility = Visibility.Visible;
                this.sliderElement.Minimum = 0.0;
                this.sliderElement.Maximum = this.mapResolutions.Length - 1;
                double value = resolutionToValue(Map.Resolution);
                if (value >= 0.0)
                {
                    this.sliderElement.Value = value;
                }
            }
        }

        private bool hasTiledCachedLayer()
        {
            foreach (Layer layer in this.Map.Layers)
            {
                if (layer is TiledCachedLayer)
                {
                    return true;
                }
            }
            return false;
        }
        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            if (Map != null)
            {
                this.Map.ZoomToResolution(this.Map.GetNextResolution(true));
            }
            if ((this.sliderElement != null) && (this.sliderElement.Visibility == Visibility.Visible))
            {
                sliderElement.Value++;
                isThumbDragging = true;//相当于拖动滑块一下
            }
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Map != null)
            {
                this.Map.ZoomToResolution(this.Map.GetNextResolution(false));
            }
            if ((this.sliderElement != null) && (this.sliderElement.Visibility == Visibility.Visible))
            {
                sliderElement.Value--;
                isThumbDragging = true;//相当于拖动滑块一下
            }
        }

        private bool isThumbDragging;
        private void ZoomSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            int index = Convert.ToInt32(Math.Round(e.NewValue));
            if (this.sliderElement.IsDraggingThumb)
            {
                this.Map.ZoomToResolution(this.mapResolutions[index]);
                this.isThumbDragging = true;
            }//拖动滑块
            //else
            //{

            //}//点击,暂时屏蔽
            //我们的level是整数，valueChange时没法zoom，即便想，那么会相互调用??
            //这导致了 点击zoomSlider 远距离 点击 缩放时，无效。所以我们的鼠标形状在xaml中体现了。
        }

        private void Layers_LayersInitialized(object sender, EventArgs args)
        {
            this.SetupZoom();
        }

        private void SuppressSlider()
        {
            if ((this.sliderElement != null) && !this.isThumbDragging)
            {
                double value = resolutionToValue(Map.Resolution);
                if ((value >= 0.0) && (this.sliderElement.Value != value))
                {
                    this.sliderElement.Value = value;
                }
            }
        }

        private void Map_ViewBoundsChanging(object sender, ViewBoundsEventArgs e)
        {
            SuppressSlider();
        }

        private void Map_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            SuppressSlider();
            this.isThumbDragging = false;
        }

        private double resolutionToValue(double resolution)
        {
            if (double.IsNaN(resolution) || mapResolutions == null)
            {
                return -1.0;
            }
            for (int i = 0; i < (this.mapResolutions.Length - 1); i++)
            {
                double big = this.mapResolutions[i];
                double small = this.mapResolutions[i + 1];
                if (resolution >= big)
                {
                    return i;
                }
                if ((resolution < big) && (resolution > small))
                {
                    return (i + ((big - resolution) / (big - small)));
                }//介于之间，算下占 零点几
            }
            return this.mapResolutions.Length - 1;
        }
    }
}