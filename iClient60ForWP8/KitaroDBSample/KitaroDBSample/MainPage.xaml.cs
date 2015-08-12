using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using KitaroDBSample.Resources;
using SuperMap.WindowsPhone.Mapping;

namespace KitaroDBSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        CloudLayer _layer;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            _layer = MyMap.Layers["Layer"] as CloudLayer;
            _layer.LocalStorage = new OfflineKitaroDB("CloudMap");
        }

    }
}