using SuperMap.WinRT.Core;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace SuperMap.WinRT.Theme
{
    /// <summary>
    /// 	<para>${core_RawTheme_Title}</para>
    /// 	<para>${core_RawTheme_Description}</para>
    /// </summary>
    public class RawTheme : Theme
    {
        /// <summary>${core_RawTheme_constructor_None_D}</summary>
        public RawTheme( )
        {
            this.Fill = new SolidColorBrush(new Color() { A = 0x40 , R = 0x42 , G = 0x72 , B = 0xd7 });
            this.Stroke = new SolidColorBrush(new Color() { A = 0xff , R = 0x50 , G = 0x82 , B = 0xe5 });
            this.StrokeThickness = 2;
            this.Opacity = 1;
            this.Size = 8;
            this.Color = new SolidColorBrush(new Color() { A = 0xff , R = 0x42 , G = 0x72 , B = 0xd7 });
            this.HoverVertexStyle = new PredefinedMarkerStyle()
            {
                Size = 12 ,
                Symbol = PredefinedMarkerStyle.MarkerSymbol.Circle ,
                //暂时找不到RadialGradientBrush的API，暂时设置为单一颜色
                Color=new SolidColorBrush(Colors.Red)
                //Color = new RadialGradientBrush()
                //{
                //    GradientStops = new GradientStopCollection() 
                //    {
                //        new GradientStop(){ Color=new Color(){ A=0xff,R=0xc2,G=0xc2,B=0xc2}, Offset=0.45},
                //        new GradientStop(){ Color=new Color(){ A=0xff,R=0x42,G=0x42,B=0x42}, Offset=0.703},
                //        new GradientStop(){ Color=new Color(){ A=0xff,R=0x8f,G=0x8e,B=0x8e}, Offset=0.687},
                //        new GradientStop(){ Color=new Color(){ A=0x00,R=0xff,G=0xff,B=0xff}, Offset=1},
                //    }
                //}
            };
            this.SnapStyle = new PredefinedMarkerStyle()
            {
                Size = 12 ,
                Symbol = PredefinedMarkerStyle.MarkerSymbol.Circle ,
                //暂时找不到RadialGradientBrush的API，暂时设置为单一颜色
                Color = new SolidColorBrush(Colors.Red)
                //Color = new RadialGradientBrush()
                // {
                //     GradientStops = new GradientStopCollection() 
                //    {
                //        new GradientStop(){ Color=new Color(){ A=0xff,R=0x2f,G=0x61,B=0xb2}, Offset=1},
                //        new GradientStop(){ Color=new Color(){ A=0xff,R=0x70,G=0xce,B=0x71}, Offset=0.631},
                //        new GradientStop(){ Color=new Color(){ A=0xff,R=0xe5,G=0xe4,B=0xe4}, Offset=0.558},
                //        new GradientStop(){ Color=new Color(){ A=0xff,R=0x9c,G=0xb1,B=0xd3}, Offset=0.944},
                //    }
                // }
            };
            this.HoverCenterStyle = new PredefinedMarkerStyle
            {
                Size = 12 ,
                Symbol = PredefinedMarkerStyle.MarkerSymbol.Circle ,
                //暂时找不到RadialGradientBrush的API，暂时设置为单一颜色
                Color = new SolidColorBrush(Colors.Red)
                //Color = new RadialGradientBrush()
                //{
                //    GradientStops = new GradientStopCollection() 
                //        {
                //            new GradientStop(){ Color=new Color(){ A=0xff,R=0xea,G=0xee,B=0xf0}, Offset=0},
                //            new GradientStop(){ Color=new Color(){ A=0xff,R=0xf4,G=0xf6,B=0xf0}, Offset=0.66},
                //            new GradientStop(){ Color=new Color(){ A=0xff,R=0x4c,G=0xd5,B=0xef}, Offset=0.45},
                //            new GradientStop(){ Color=new Color(){ A=0xff,R=0x64,G=0xc8,B=0xf0}, Offset=0.65},             
                //            new GradientStop(){ Color=new Color(){ A=0xff,R=0xef,G=0xf5,B=0xea}, Offset=0.44},  
                //            new GradientStop(){ Color=new Color(){ A=0xff,R=0x64,G=0xc8,B=0xf0}, Offset=1},
                //            new GradientStop(){ Color=new Color(){ A=0xff,R=0xf3,G=0xf3,B=0xf3}, Offset=0.89},
                //            new GradientStop(){ Color=new Color(){ A=0xff,R=0x64,G=0xc8,B=0xf0}, Offset=0.9},
                //        }
                //}
            };
            FCGreen = new Color { A = 0xff , R = 0x99 , G = 0xcc , B = 0x66 };
            FCOrange = new Color { A = 0xff , R = 0xd9 , G = 0x82 , B = 0x57 };
            FCBlue = new Color { A = 0xff , R = 0x95 , G = 0xd1 , B = 0xe5 };
            FCYollew = new Color { A = 0xff , R = 0xe5 , G = 0xe5 , B = 0x45 };
        }
        /// <summary>${core_Theme_attribute_FCGreen_D}</summary>
        public static Color FCGreen { get; set; }
        /// <summary>${core_Theme_attribute_FCOrange_D}</summary>
        public static Color FCOrange { get; set; }
        /// <summary>${core_Theme_attribute_FCBlue_D}</summary>
        public static Color FCBlue { get; set; }
        /// <summary>${core_Theme_attribute_FCYollew_D}</summary>
        public static Color FCYollew { get; set; }

    }
}
