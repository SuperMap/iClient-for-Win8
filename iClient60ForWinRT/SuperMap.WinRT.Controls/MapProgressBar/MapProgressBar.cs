using System;
using System.ComponentModel;
using System.Windows;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel;

namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_MapProgressBar_Title}</para>
    /// 	<para>${controls_MapProgressBar_Description}</para>
    /// </summary>
    [TemplatePart(Name = "Progress", Type = typeof(ProgressBar))]
    [TemplatePart(Name = "ValueText", Type = typeof(TextBlock))]
    [TemplateVisualState(Name = "Show", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Hide", GroupName = "CommonStates")]
    public class MapProgressBar : Control
    {
        private ProgressBar bar;
        private bool isVisible;
        /// <summary>${controls_MapProgressBar_field_MapProperty_D}</summary>
        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(Map), typeof(MapProgressBar), new PropertyMetadata(null, new PropertyChangedCallback(OnMapPropertyChanged)));
        private TextBlock text;
        /// <summary>${controls_MapProgressBar_field_textBrushProperty_D}</summary>
        public static readonly DependencyProperty TextBrushProperty = DependencyProperty.Register("TextBrush", typeof(Brush), typeof(MapProgressBar), null);

        /// <summary>${controls_MapProgressBar_constructor_None_D}</summary>
        public MapProgressBar()
        {
            DefaultStyleKey = typeof(MapProgressBar);
        }

        private void ChangeVisualState(bool useTransitions)
        {
            if (this.isVisible || this.isVisible)
            {
                this.GoToState(useTransitions, "Show");
            }
            else
            {
                this.GoToState(useTransitions, "Hide");
            }
        }

        private bool GoToState(bool useTransitions, string stateName)
        {
            return VisualStateManager.GoToState(this, stateName, useTransitions);
        }

        private void map_Progress(object sender, ProgressEventArgs e)
        {
            if (this.bar != null)
            {
                this.bar.Value = (double)e.Progress;

            }
            if (this.text != null)
            {
                this.text.Text = string.Format("{0}%", e.Progress);
            }
            this.isVisible = e.Progress < 99;
            this.ChangeVisualState(true);
        }

        /// <summary>${controls_MapProgressBar_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.text = base.GetTemplateChild("ValueText") as TextBlock;
            this.bar = base.GetTemplateChild("Progress") as ProgressBar;
            if (DesignMode.DesignModeEnabled)
            {
                this.isVisible = true;
                if (this.bar != null)
                {
                    this.bar.Value = 50.0;
                }
                if (this.text != null)
                {
                    this.text.Text = string.Format("{0}%", 50);
                }
            }
            this.ChangeVisualState(false);
        }

        private static void OnMapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MapProgressBar bar = d as MapProgressBar;
            Map map = e.OldValue as Map;
            Map map2 = e.NewValue as Map;
            if (map != null)
            {
                map.Progress -= new EventHandler<ProgressEventArgs>(bar.map_Progress);
            }
            if (map2 != null)
            {
                map2.Progress += new EventHandler<ProgressEventArgs>(bar.map_Progress);
            }
        }

        /// <summary>${controls_MapProgressBar_attribute_Map_D}</summary>
        public Map Map
        {
            get
            {
                return (Map)base.GetValue(MapProperty);
            }
            set
            {
                base.SetValue(MapProperty, value);
            }
        }

        /// <summary>${controls_MapProgressBar_attribute_textBrush_D}</summary>
        public Brush TextBrush
        {
            get
            {
                return (Brush)base.GetValue(TextBrushProperty);
            }
            set
            {
                base.SetValue(TextBrushProperty, value);
            }
        }
    }
}

