using System;
using System.Windows;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Controls.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace SuperMap.WinRT.Controls
{
    /// <summary>
    /// 	<para>${controls_ScaleBar_Title}</para>
    /// 	<para>${controls_ScaleBar_Description}</para>
    /// </summary>
    [TemplatePart(Name = "ScaleBarBlock", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PaddingLeftForScaleBarTextMeters", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PaddingLeftTopNotch", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PaddingLeftForScaleBarTextMiles", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PaddingLeftBottomNotch", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "ScaleBarTextForMeters", Type = typeof(TextBlock))]
    [TemplatePart(Name = "ScaleBarTextForMiles", Type = typeof(TextBlock))]
    public class ScaleBar : Control
    {
        private FrameworkElement scaleBarBlock;
        private FrameworkElement paddingLeftForScaleBarTextMeters;
        private TextBlock scaleBarTextForMeters;
        private FrameworkElement paddingLeftTopNotch;
        private FrameworkElement paddingLeftForScaleBarTextMiles;
        private FrameworkElement paddingLeftBottomNotch;
        private TextBlock scaleBarTextForMiles;

        /// <summary>${controls_ScaleBar_constructor_None_D}</summary>
        public ScaleBar()
        {
            DefaultStyleKey = typeof(ScaleBar);
        }
        /// <summary>${controls_ScaleBar_method_onApplyTemplate_D}</summary>
        protected override void OnApplyTemplate()
        {
            scaleBarBlock = this.GetTemplateChild("ScaleBarBlock") as FrameworkElement;
            paddingLeftForScaleBarTextMeters = this.GetTemplateChild("PaddingLeftForScaleBarTextMeters") as FrameworkElement;
            paddingLeftTopNotch = this.GetTemplateChild("PaddingLeftTopNotch") as FrameworkElement;
            paddingLeftForScaleBarTextMiles = this.GetTemplateChild("PaddingLeftForScaleBarTextMiles") as FrameworkElement;
            paddingLeftBottomNotch = this.GetTemplateChild("PaddingLeftBottomNotch") as FrameworkElement;
            scaleBarTextForMeters = this.GetTemplateChild("ScaleBarTextForMeters") as TextBlock;
            scaleBarTextForMiles = this.GetTemplateChild("ScaleBarTextForMiles") as TextBlock;

            RefreshScaleBar();
            base.OnApplyTemplate();
        }

        private void newMap_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            RefreshScaleBar();
        }

        //更新控件
        private void RefreshScaleBar()
        {
            if (Map == null || double.IsNaN(Map.Resolution))
            {
                return;
            }

            Unit outUnit = Unit.Undefined;
            double outResolution;

            #region 对m和km处理；
            double roundedKiloMeters = GetBestEstimateOfValue(Map.Resolution, Unit.Kilometer, out outUnit, out outResolution);
            double widthMeters = roundedKiloMeters / outResolution;
            bool inMeters = outUnit == Unit.Meter;
            if (paddingLeftForScaleBarTextMeters != null)
            {
                paddingLeftForScaleBarTextMeters.Width = widthMeters;
            }
            if (paddingLeftTopNotch != null)
            {
                paddingLeftTopNotch.Width = widthMeters;
            }
            if (scaleBarTextForMeters != null)
            {
                scaleBarTextForMeters.Text = string.Format("{0}{1}", roundedKiloMeters, (inMeters ? Resource.ScaleBar_Meter : Resource.ScaleBar_Kilometer));
                scaleBarTextForMeters.Width = widthMeters;
            }
            #endregion

            #region 对Mile处理；
            double roundedMiles = GetBestEstimateOfValue(Map.Resolution, Unit.Mile, out outUnit, out outResolution);
            double widthMiles = roundedMiles / outResolution;
            bool inFeet = outUnit == Unit.Foot;
            if (paddingLeftForScaleBarTextMiles != null)
            {
                paddingLeftForScaleBarTextMiles.Width = widthMiles;
            }
            if (paddingLeftBottomNotch != null)
            {
                paddingLeftBottomNotch.Width = widthMiles;
            }
            if (scaleBarTextForMiles != null)
            {
                scaleBarTextForMiles.Text = string.Format("{0}{1}", roundedMiles, inFeet ? Resource.ScaleBar_Foot : Resource.ScaleBar_Mile);
                scaleBarTextForMiles.Width = widthMiles;
            }
            #endregion

            double widthOfNotches = 4;
            double scaleBarBlockWidth = (widthMiles > widthMeters) ? widthMiles : widthMeters;
            scaleBarBlockWidth += widthOfNotches;
            if (!double.IsNaN(scaleBarBlockWidth) && scaleBarBlock != null)
            {
                scaleBarBlock.Width = scaleBarBlockWidth;
            }
        }

        private double GetBestEstimateOfValue(double resolution, Unit displayUnit, out Unit unit, out double outResolution)
        {
            unit = displayUnit;
            //要显示的数；
            double rounded = 0;
            double originalRes = resolution;
            while (rounded < 0.5)
            {
                resolution = originalRes;
                //IS6需要设置CRS是meter还是Degree
                if (Map.CRS == null)//此使用于BingMap、GoogleMap等
                {
                    double lat = (this.Map.ViewBounds.Center.Y / this.Map.Bounds.Height) * LatitudeSpan;
                    resolution = resolution * (int)Unit.Meter / (int)unit * Math.Cos((Math.PI / 180) * lat);
                }
                else
                {
                    //if (Map.CRS.Unit == Unit.DecimalDegree)
                    //{
                    //   // resolution = GetResolution(Map.ViewBounds, resolution);
                    //    resolution = resolution * (int)Unit.Meter / (int)unit;
                    //}
                    //else
                    if (Map.CRS.Unit != Unit.Undefined)
                    {
                        resolution = resolution * (int)Map.CRS.Unit / (int)unit;
                    }
                }

                //
                double val = TargetWidth * resolution;
                //保留5为有效数字；
                val = RoundToSignificant(val, resolution);
                //去到最接近的整数；
                double noFrac = Math.Round(val);

                if (val < 0.5)
                {
                    Unit newUnit = Unit.Undefined;
                    if (unit == Unit.Kilometer)
                    {
                        newUnit = Unit.Meter;
                    }
                    else if (unit == Unit.Mile)
                    {
                        newUnit = Unit.Foot;
                    }
                    if (newUnit == Unit.Undefined)
                    {
                        break;
                    }
                    unit = newUnit;
                }
                else if (noFrac > 1)
                {
                    rounded = noFrac;
                    var len = noFrac.ToString().Length;
                    if (len <= 2)
                    {
                        if (noFrac > 5)
                        {
                            rounded -= noFrac % 5;
                        }
                        while (rounded > 1 && (rounded / resolution) > TargetWidth)
                        {
                            double decr = noFrac > 5 ? 5 : 1;
                            rounded = rounded - decr;
                        }
                    }
                    else if (len > 2)
                    {
                        rounded = Math.Round(noFrac / Math.Pow(10, len - 1)) * Math.Pow(10, len - 1);
                        if ((rounded / resolution) > TargetWidth)
                        {
                            rounded = Math.Floor(noFrac / Math.Pow(10, len - 1)) * Math.Pow(10, len - 1);
                        }
                    }
                }
                else
                {
                    rounded = Math.Floor(val);
                    if (rounded == 0)
                    {
                        rounded = (val == 0.5) ? 0.5 : 1;
                        if ((rounded / resolution) > TargetWidth)
                        {
                            rounded = 0;
                            Unit newUnit = Unit.Undefined;
                            if (unit == Unit.Kilometer)
                            {
                                newUnit = Unit.Meter;
                            }
                            else if (unit == Unit.Mile)
                            {
                                newUnit = Unit.Foot;
                            }
                            if (newUnit == Unit.Undefined)
                            {
                                break;
                            }
                            unit = newUnit;
                        }
                    }
                }
            }
            outResolution = resolution;
            return rounded;
        }
        private double RoundToSignificant(double value, double resolution)
        {
            var round = Math.Floor(-Math.Log(resolution));
            if (round > 0)
            {
                round = Math.Pow(10, round);
                return Math.Round(value * round) / round;
            }
            else
            {
                return Math.Round(value);
            }
        }
        //private double toRadians = 0.017453292519943295769236907684886;
        //private double earthRadius = 6378137;
        //private double degreeDist;
        //private double GetResolution(Rectangle2D bounds, double resolution)
        //{
        //    degreeDist = earthRadius * toRadians;
        //    Point2D center = bounds.Center;
        //    double y = center.Y;

        //    return Math.Cos(y * toRadians) * resolution * degreeDist;
        //}


        //设置控件的宽度；
        /// <summary>${controls_ScaleBar_attribute_targetWidth_D}</summary>
        public double TargetWidth
        {
            get { return (double)GetValue(TargetWidthProperty); }
            set { SetValue(TargetWidthProperty, value); }
        }

        /// <summary>${controls_ScaleBar_field_targetWidthProperty_D}</summary>
        public static readonly DependencyProperty TargetWidthProperty =
            DependencyProperty.Register("TargetWidth", typeof(double), typeof(ScaleBar), new PropertyMetadata(150.0));

        /// <summary>${controls_ScaleBar_attribute_targetHeight_D}</summary>
        public double TargetHeight
        {
            get { return (double)GetValue(TargetHeightProperty); }
            set { SetValue(TargetHeightProperty, value); }
        }

        /// <summary>${controls_ScaleBar_field_targetHeightProperty_D}</summary>
        public static readonly DependencyProperty TargetHeightProperty =
            DependencyProperty.Register("TargetHeight", typeof(double), typeof(ScaleBar), new PropertyMetadata(10.0));

        /// <summary>${controls_ScaleBar_attribute_map_D}</summary>
        public Map Map
        {
            get { return (Map)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        /// <summary>${controls_ScaleBar_field_mapProperty_D}</summary>
        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(Map), typeof(ScaleBar), new PropertyMetadata(null,OnMapPropertyChanged));

        private static void OnMapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Map oldMap = e.OldValue as Map;
            Map newMap = e.NewValue as Map;
            ScaleBar bar = d as ScaleBar;
            if (bar != null)
            {
                if (oldMap != null)
                {
                    oldMap.ViewBoundsChanged -= bar.newMap_ViewBoundsChanged;
                    oldMap.ViewBoundsChanging -= bar.newMap_ViewBoundsChanged;
                }
                if (newMap != null)
                {
                    newMap.ViewBoundsChanged += bar.newMap_ViewBoundsChanged;
                    newMap.ViewBoundsChanging += bar.newMap_ViewBoundsChanged;
                }
                bar.RefreshScaleBar();
            }
        }
        /// <summary>${controls_ScaleBar_attribute_color_D}</summary>
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>${controls_ScaleBar_field_colorProperty_D}</summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(ScaleBar), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        private double latSpan = 180.0;
        /// <summary>${controls_ScaleBar_attribute_latitudeSpan_D}</summary>
        public double LatitudeSpan
        {
            get { return latSpan; }
            set
            {
                if (latSpan < 0)
                {
                    latSpan = 0;
                }
                else if (latSpan > 180)
                {
                    latSpan = 180;
                }
                latSpan = value;
            }
        }

    }
}
