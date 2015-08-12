using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LayerOverlayTest.Resources;
using SuperMap.WindowsPhone.Mapping;

namespace LayerOverlayTest
{
    public partial class MainPage : PhoneApplicationPage
    {
        private TiledDynamicRESTLayer _layer1, _layer2;
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            double[] resolutions = new double[14];
            double resolution = 0.55641857128792931;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            _layer1 = MyMap.Layers["TDLayer1"] as TiledDynamicRESTLayer;

            _layer1.CRS = new SuperMap.WindowsPhone.Core.CoordinateReferenceSystem { WKID = 4326 };
            _layer1.Opacity = 0.5;
            //_layer1.AdjustFactor = 1.0;
            _layer2 = MyMap.Layers["TDLayer2"] as TiledDynamicRESTLayer;
            
            _layer2.CRS = new SuperMap.WindowsPhone.Core.CoordinateReferenceSystem { WKID = 4326 };
            _layer2.Transparent = true;
            _layer2.Opacity = 0.8;
            _layer2.IsVisible = true;
           // _layer2.AdjustFactor = 0.3;
            
            
            MyMap.Layers.LayersInitialized += Layers_LayersInitialized;
            MyMap.Layers.Add(_layer1);
            MyMap.Layers.Add(_layer2);
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
        }

        void MyMap_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            
        }

        void Layers_LayersInitialized(object sender, EventArgs e)
        {
            
        }

       
    }
}