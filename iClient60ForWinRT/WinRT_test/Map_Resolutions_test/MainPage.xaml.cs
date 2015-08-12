using SuperMap.WinRT.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Map_Resolutions_test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TiledDynamicRESTLayer layer;
        public MainPage()
        {
            this.InitializeComponent();
            layer=MyMap.Layers["TDLayer"] as TiledDynamicRESTLayer;
            Double[] resolutions = new Double[] {0.28526148969889065,0.14263074484944532,0.071315372424722662,0.035657686212361331,0.017828843106180665,0.0089144215530903327,0.0044572107765451664,0.0022286053882725832,
 0.0011143026941362916,0.00055715134706814579,0.0002785756735340729,0.00013928783676703645,0.000069643918383518224,0.000034821959191759112};
            MyMap.Resolutions = resolutions;
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
        }

        void MyMap_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            this.resolution.Text = "Resolution: " + MyMap.Resolution.ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
