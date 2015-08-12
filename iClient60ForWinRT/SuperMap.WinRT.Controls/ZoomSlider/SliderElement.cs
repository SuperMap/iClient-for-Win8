using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace SuperMap.WinRT.Controls
{

    /// <summary>
    /// 	<para>${controls_SliderElement_Title}</para>
    /// 	<para>${controls_SliderElement_Description}</para>
    /// 	<para><img src="zoomSlider.png"/></para>
    /// </summary>
    [TemplatePart(Name="Buffer", Type=typeof(FrameworkElement))]
    [TemplatePart(Name = "TrackPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "ThumbElement", Type = typeof(Thumb))]
    public class SliderElement : RangeBase
    {
        internal const string BufferElementName = "Buffer";
        internal const string ThumbName = "ThumbElement";
        internal const string TrackPanelName = "TrackPanel";

        private FrameworkElement bufferElement;
        internal Panel trackPanel;
        private Thumb thumb;
        
        private Style currentTrackStyle;
        private double dragValue;
        private bool suppressValueChange;
        private Queue<TrackMark> trackMarkQueue;

        /// <summary>${controls_SliderElement_field_trackMarkStyleProperty_D}</summary>
        public static readonly DependencyProperty TrackMarkStyleProperty = DependencyProperty.Register("TrackMarkStyle", typeof(Style), typeof(SliderElement), new PropertyMetadata(null,new PropertyChangedCallback(OnTrackMarkStylePropertyChanged)));
        /// <summary>${controls_SliderElement_attribute_trackMarkStyle_D}</summary>
        public Style TrackMarkStyle
        {
            get
            {
                return (Style)base.GetValue(TrackMarkStyleProperty);
            }
            set
            {
                base.SetValue(TrackMarkStyleProperty, value);
            }
        }
        private static void OnTrackMarkStylePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SliderElement slider = (SliderElement)sender;
            slider.trackMarkQueue.Clear();
            slider.UpdateTrackMarks();
        }

        /// <summary>${controls_SliderElement_event_dragCompleted_D}</summary>
        public event EventHandler DragCompleted;

        /// <summary>${controls_SliderElement_constructor_None_D}</summary>
        public SliderElement()
        {
            base.Loaded += new RoutedEventHandler(this.OnLoaded);
            this.trackMarkQueue = new Queue<TrackMark>();
            base.DefaultStyleKey = typeof(SliderElement);
        }

        /// <summary>${controls_SliderElement_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.trackPanel = base.GetTemplateChild("TrackPanel") as Panel;
            this.thumb = base.GetTemplateChild("ThumbElement") as Thumb;
            if (this.thumb != null)
            {
                this.thumb.DragStarted += new DragStartedEventHandler(this.OnThumbDragStarted);
                this.thumb.DragDelta += new DragDeltaEventHandler(this.OnThumbDragDelta);
                this.thumb.DragCompleted += new DragCompletedEventHandler(this.OnThumbDragCompleted);
            }
            this.bufferElement = base.GetTemplateChild("Buffer") as Rectangle;
            this.UpdateTrackMarks();
        }

        /// <summary>${controls_SliderElement_method_NotifyValueChange_D}</summary>
        /// <param name="newValue">${controls_SliderElement_method_NotifyValueChange_pram_newValue}</param>
        protected void NotifyValueChange(double newValue)
        {
            this.suppressValueChange = true;
            this.OnValueChanged(base.Value, newValue);
        }

        //虚函数，可继承覆盖
        /// <summary>${controls_SliderElement_method_UpdateTrackMarks_D}</summary>
        protected virtual void UpdateTrackMarks()
        {
            if (this.trackPanel != null)
            {
                int num = (int)Math.Ceiling(base.Minimum);
                int num2 = (int)Math.Floor(base.Maximum);
                if (this.currentTrackStyle != this.TrackMarkStyle)
                {
                    this.currentTrackStyle = this.TrackMarkStyle;
                    this.trackPanel.Children.Clear();
                    for (int i = num; i <= num2; i++)
                    {
                        TrackMark mark = this.SignOutTrackMark();
                        mark.Tag = i;
                        this.trackPanel.Children.Insert(0, mark);
                    }
                    DispatchedHandler handler = new DispatchedHandler(this.UpdateThumbPosition);
                    base.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
                }
                else
                {
                    int index = (num2 - num) + 1;
                    while (this.trackPanel.Children.Count > index)
                    {
                        this.SignInTrackMark((TrackMark)this.trackPanel.Children[index]);
                        this.trackPanel.Children.RemoveAt(index);
                    }
                    while (this.trackPanel.Children.Count < index)
                    {
                        this.trackPanel.Children.Add(this.SignOutTrackMark());
                    }
                    int num5 = 0;
                    for (int j = num2; num5 < this.trackPanel.Children.Count; j--)
                    {
                        ((FrameworkElement)this.trackPanel.Children[num5]).Tag = j;
                        num5++;
                    }
                    this.UpdateThumbPosition();
                }
            }
        }

        /// <summary>${controls_SliderElement_method_UpdateThumbPosition_D}</summary>
        protected virtual void UpdateThumbPosition()
        {
            if (this.suppressValueChange)
            {
                this.suppressValueChange = false;
            }
            else if (((this.bufferElement != null) && (this.trackPanel != null)) && (this.trackPanel.Children.Count > 0))
            {
                int num = (int)Math.Ceiling(base.Minimum);
                double height = this.trackPanel.Children[0].RenderSize.Height;
                this.bufferElement.Height = height * (base.Value - num);
            }
        }

        //覆盖父类的方法，增添新动作
        /// <summary>${controls_SliderElement_method_OnValueChanged_D}</summary>
        /// <param name="oldValue">${controls_SliderElement_method_OnValueChanged_pram_oldValue}</param>
        /// <param name="newValue">${controls_SliderElement_method_OnValueChanged_pram_newValue}</param>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            this.UpdateThumbPosition();
            base.OnValueChanged(oldValue, newValue);
        }

        /// <summary>${controls_SliderElement_method_OnMaximumChanged_D}</summary>
        /// <param name="oldMaximum">${controls_SliderElement_method_OnMaximumChanged_pram_oldMaximum}</param>
        /// <param name="newMaximum">${controls_SliderElement_method_OnMaximumChanged_pram_newMaximum}</param>
        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            this.UpdateTrackMarks();
        }

        /// <summary>${controls_SliderElement_method_OnMinimumChanged_D}</summary>
        /// <param name="oldMinimum">${controls_SliderElement_method_OnMinimumChanged_pram_oldMinimum}</param>
        /// <param name="newMinimum">${controls_SliderElement_method_OnMinimumChanged_pram_newMinimum}</param>
        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            this.UpdateTrackMarks();
        }


        //私有方法
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.UpdateThumbPosition();
        }

        private void SignInTrackMark(TrackMark item)
        {
            this.trackMarkQueue.Enqueue(item);
        }

        private TrackMark SignOutTrackMark()
        {
            if (this.trackMarkQueue.Count > 0)
            {
                return this.trackMarkQueue.Dequeue();
            }
            TrackMark mark = new TrackMark();
            if (this.TrackMarkStyle != null)
            {
                mark.Style = this.TrackMarkStyle;
            }
            mark.PointerPressed += mark_PointerPressed;
            return mark;
        }

        void mark_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TrackMark mark = (TrackMark)sender;
            if (mark.Tag != null)
            {
                this.NotifyValueChange((double)((int)mark.Tag));
            }
        }
        
        private void OnThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            this.dragValue = base.ActualHeight - (e.VerticalOffset + (this.thumb.ActualHeight / 2.0));
        }

        private void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (((this.thumb != null) && (this.trackPanel != null)) && (this.trackPanel.Children.Count > 0))
            {
                this.dragValue -= e.VerticalChange;
                double d = base.Minimum + (this.dragValue / this.trackPanel.Children[0].RenderSize.Height);
                if (!double.IsNaN(d) && !double.IsInfinity(d))
                {
                    d = Math.Min(base.Maximum, Math.Max(base.Minimum, d));
                    if (d != base.Value)
                    {
                        base.Value = d;
                    }
                }
            }
        }

        private void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            double num = Math.Round(base.Value);
            if (num != base.Value)
            {
                base.Value = num;
            }
            EventHandler dragCompleted = this.DragCompleted;
            if (dragCompleted != null)
            {
                dragCompleted(this, EventArgs.Empty);
            }
        }

        //属性

        /// <summary>${controls_SliderElement_attribute_isDraggingThumb_D}</summary>
        public bool IsDraggingThumb
        {
            get
            {
                return ((this.thumb != null) && this.thumb.IsDragging);
            }
        }

        /// <summary>${controls_SliderElement_attribute_TrackPanel_D}</summary>
        protected Panel TrackPanel
        {
            get
            {
                return this.trackPanel;
            }
        }

    }
}
