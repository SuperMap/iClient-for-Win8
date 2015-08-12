using SuperMap.WindowsPhone.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SuperMap.WindowsPhone.Samples.Common
{
    public class CompassMarkerStyle : MarkerStyle
    {
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register
            ("Height", typeof(double), typeof(CompassMarkerStyle), new PropertyMetadata(40.0, new PropertyChangedCallback(OnHeightPropertyChanged)));
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register
            ("Width", typeof(double), typeof(CompassMarkerStyle), new PropertyMetadata(30.0, new PropertyChangedCallback(OnWidthPropertyChanged)));
        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register
            ("Rotation", typeof(double), typeof(CompassMarkerStyle), new PropertyMetadata(0.0, new PropertyChangedCallback(OnRotationPropertyChanged)));

        public CompassMarkerStyle()
        {
            ControlTemplate = App.Current.Resources["CompassMarkerStyle"] as ControlTemplate;
        }

        private static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CompassMarkerStyle style = d as CompassMarkerStyle;
            style.OnPropertyChanged("Width");
            style.OnPropertyChanged("OffsetX");
        }

        private static void OnHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CompassMarkerStyle style = d as CompassMarkerStyle;
            style.OnPropertyChanged("Height");
            style.OnPropertyChanged("OffsetY");
        }

        private static void OnRotationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CompassMarkerStyle style = d as CompassMarkerStyle;
            style.OnPropertyChanged("Rotation");
        }

        public override double OffsetX
        {
            get
            {
                return (this.Width * 0.5);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override double OffsetY
        {
            get
            {
                return (this.Height * 0.5);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public double Width
        {
            get
            {
                return (double)base.GetValue(WidthProperty);
            }
            set
            {
                value = value <= 0 ? 30 : value;
                base.SetValue(WidthProperty, value);
            }
        }

        public double Height
        {
            get
            {
                return (double)base.GetValue(HeightProperty);
            }
            set
            {
                value = value <= 0 ? 40 : value;
                base.SetValue(HeightProperty, value);
            }
        }

        public double Rotation
        {
            get
            {
                return (double)base.GetValue(RotationProperty);
            }
            set
            {
                base.SetValue(RotationProperty, value);
            }
        }
    }
}
