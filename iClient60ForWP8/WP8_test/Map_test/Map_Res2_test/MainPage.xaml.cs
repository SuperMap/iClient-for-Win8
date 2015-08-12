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

            double[] resolutions = new double[14];
            double resolution = 0.28526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions;
            double[] resolutions1 = new double[14];
            double resolution1 = 0.18526148969889065;
            for (int i = 0; i < 14; i++)
            {
                resolutions[i] = resolution;
                resolution /= 2;
            }
            MyMap.Resolutions = resolutions1;
        }

    }
}