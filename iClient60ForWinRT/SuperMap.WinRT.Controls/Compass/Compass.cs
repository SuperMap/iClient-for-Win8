using System;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Mapping;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_Compass_Title}</para>
    /// 	<para>${controls_Compass_Description}</para>
    /// 	<para><img src="compass.png"/></para>
    /// </summary>
    [TemplatePart(Name = "PanUpElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PanDownElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PanLeftElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PanRightElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "ViewEntireElement", Type = typeof(Button))]

    public class Compass : Control
    {
        private RepeatButton panDownElement;
        private RepeatButton panLeftElement;
        private RepeatButton panRightElement;
        private RepeatButton panUpElement;
        private Button viewEntireElement;

        /// <summary>${controls_Compass_field_MapProperty_D}</summary>
        public static readonly DependencyProperty MapProperty = DependencyProperty.Register("Map", typeof(Map), typeof(Compass), null);

        /// <summary>${controls_Compass_attribute_map_D}</summary>
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

        /// <summary>${controls_Compass_constructor_None_D}</summary>
        public Compass()
        {
            base.DefaultStyleKey = typeof(Compass);
        }

        /// <summary>${controls_Compass_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.panLeftElement = base.GetTemplateChild("PanLeftElement") as RepeatButton;
            this.panRightElement = base.GetTemplateChild("PanRightElement") as RepeatButton;
            this.panUpElement = base.GetTemplateChild("PanUpElement") as RepeatButton;
            this.panDownElement = base.GetTemplateChild("PanDownElement") as RepeatButton;
            this.viewEntireElement = base.GetTemplateChild("ViewEntireElement") as Button;

            #region 上下左右
            if (this.panLeftElement != null)
            {
                this.panLeftElement.ClickMode = ClickMode.Press;
                this.panLeftElement.Click += (sender, args) =>
                           {
                               if ((this.Map != null) && (sender != null))
                               {
                                   this.Map.Pan(-this.Map.ViewBounds.Width * this.Map.PanFactor, 0);
                               }
                           };
                this.panLeftElement.PointerExited += (sender, args) =>
                {
                    this.panLeftElement.ReleasePointerCaptures();
                };
            }

            if (this.panRightElement != null)
            {
                this.panRightElement.ClickMode = ClickMode.Press;
                this.panRightElement.Click += (sender, args) =>
                {
                    if ((this.Map != null) && (sender != null))
                    {
                        this.Map.Pan(this.Map.ViewBounds.Width * this.Map.PanFactor, 0);
                    }
                };
                this.panRightElement.PointerExited += (sender, args) =>
                    {
                        this.panRightElement.ReleasePointerCaptures();
                    };
            }
            if (this.panDownElement != null)
            {
                this.panDownElement.ClickMode = ClickMode.Press;
                this.panDownElement.Click += (sender, args) =>
                {
                    if ((this.Map != null) && (sender != null))
                    {
                        this.Map.Pan(0, -this.Map.ViewBounds.Height * this.Map.PanFactor);
                    }
                };
                this.panDownElement.PointerExited += (sender, args) =>
                {
                    this.panDownElement.ReleasePointerCaptures();
                };
            }

            if (this.panUpElement != null)
            {
                this.panUpElement.ClickMode = ClickMode.Press;
                this.panUpElement.Click += (sender, args) =>
                {
                    if ((this.Map != null) && (sender != null))
                    {
                        this.Map.Pan(0, this.Map.ViewBounds.Height * this.Map.PanFactor);
                    }
                };
                this.panUpElement.PointerExited += (sender, args) =>
                {
                    this.panUpElement.ReleasePointerCaptures();
                };
            }
            #endregion

            if (this.viewEntireElement != null)
            {
                this.viewEntireElement.Click += viewEntireElement_Click;
            }
        }

        void viewEntireElement_Click(object sender, RoutedEventArgs e)
        {
            if (this.Map != null)
            {
                this.Map.ViewEntire();
            }
        }

        private bool GoToState(bool useTransitions, string stateName)
        {
            return VisualStateManager.GoToState(this, stateName, useTransitions);
        }

    }
}
