using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using System.Windows.Input;
using SuperMap.WindowsPhone.Mapping;
using SuperMap.WindowsPhone.Core;
using System.Windows.Media;
using System.Diagnostics;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ElementsLayer elementLayer;
        private FeaturesLayer featuresLayer;
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            elementLayer = MyMap.Layers["ElementLayer"] as ElementsLayer;
            featuresLayer = MyMap.Layers["FeaturesLayer"] as FeaturesLayer;
           featuresLayer.Tap += featuresLayer_Tap;
          //featuresLayer.MouseEnter += featuresLayer_MouseEnter;
          // featuresLayer.MouseLeave += featuresLayer_MouseLeave;
          // featuresLayer.MouseLeftButtonDown += featuresLayer_MouseLeftButtonDown;
          //  featuresLayer.MouseLeftButtonUp += featuresLayer_MouseLeftButtonUp; 
          //  featuresLayer.MouseMove += featuresLayer_MouseMove;
            //MyMap.Tap += MyMap_Tap;
            //MyMap.MouseEnter += MyMap_MouseEnter;
           //MyMap.MouseLeave += MyMap_MouseLeave;
           //MyMap.MouseLeftButtonDown += MyMap_MouseLeftButtonDown;

        }

        void MyMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        void MyMap_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        void MyMap_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        void MyMap_Tap(object sender, GestureEventArgs e)
        {
            
        }

        void featuresLayer_MouseMove(object sender, FeatureMouseEventArgs e)
        {
            
        }

        void featuresLayer_MouseLeftButtonUp(object sender, FeatureMouseButtonEventArgs e)
        {
           
        }

        void featuresLayer_MouseLeftButtonDown(object sender, FeatureMouseButtonEventArgs e)
        {
            
        }

        void featuresLayer_MouseLeave(object sender, FeatureMouseEventArgs e)
        {
            
        }

        void featuresLayer_MouseEnter(object sender, FeatureMouseEventArgs e)
        {
            
        }

        void featuresLayer_Tap(object sender, FeatureGestureEventArgs e)
        {
            
        }

    

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             this.MyMap.ZoomIn(); 
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.MyMap.ZoomOut();
        }

        //private void Button_Click_3(object sender, RoutedEventArgs e)
        //{
        //    DrawLine line = new DrawLine(this.MyMap);
        //    this.MyMap.Action = line;
        //   // this.featuresLayer.ClearFeatures();
        //    line.DrawCompleted += new EventHandler<DrawEventArgs>(line_DrawCompleted);
        //}

        //private void line_DrawCompleted(object sender, DrawEventArgs e)
        //{
        //    this.featuresLayer.ClearFeatures();
        //    Feature feature = new Feature { Geometry=e.Geometry};
        //    //this.elementLayer.Children.Add(e.Element);
        //    this.featuresLayer.AddFeature(feature);
        //}

        //private void Button_Click_4(object sender, RoutedEventArgs e)
        //{
        //    DrawPolygon polygon = new DrawPolygon(this.MyMap);
        //    this.MyMap.Action = polygon;
        //    polygon.DrawCompleted += new EventHandler<DrawEventArgs>(polygon_DrawCompleted);

        //}

        //private void polygon_DrawCompleted(object sender, DrawEventArgs e)
        //{
        //    this.featuresLayer.ClearFeatures();
        //    Feature feature = new Feature { Geometry=e.Geometry};
        //    this.featuresLayer.AddFeature(feature);
        //}

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Feature f = new Feature();
            //f.Tap += f_Tap;
            //f.MouseEnter += f_MouseEnter;
            //f.MouseLeave += f_MouseLeave;
            //f.MouseLeftButtonDown += f_MouseLeftButtonDown;
            //f.MouseLeftButtonUp += f_MouseLeftButtonUp;
            //f.MouseMove+=f_MouseMove;   
            GeoPoint point = new GeoPoint(0, 0);
            PredefinedMarkerStyle style = new PredefinedMarkerStyle();
            style.Color = new SolidColorBrush(Colors.Red);
            style.Size = 80;
            style.Symbol = PredefinedMarkerStyle.MarkerSymbol.Circle;
            f.Geometry = point;
            f.Style = style;
            this.featuresLayer.AddFeature(f);
        }

        private void f_MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("f.MouseMove");
        }

        void f_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("f.MouseLeftButtonUp"); 
        }

        void f_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("f.MouseLeftButtonDown"); 
        }

        void f_MouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("f.MouseLeave"); 
        }

        void f_MouseEnter(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("f.MouseEnter");
        }

        void f_Tap(object sender, GestureEventArgs e)
        {
            Debug.WriteLine("f.Tap");
        }
    }
}