using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SuperMap.WindowsPhone.Core;

namespace Map_Res_test
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            double[] resolutions=new double[]{0.28526148969889065,0.14263074484944532,0.071315372424722662,0.035657686212361331,0.017828843106180665,0.0089144215530903327,0.0044572107765451664,0.0022286053882725832,0.0011143026941362916,
            0.00055715134706814579,0.0002785756735340729,0.00013928783676703645,0.000069643918383518224,0.000034821959191759112};
            MyMap.Resolutions = resolutions;
        }

    }
}