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

namespace Map_ScalesChanged_test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer _timer;
        DispatcherTimer _timer1;

        public MainPage()
        {
            this.InitializeComponent();
            double[] scales = new double[14];
            double scale = 0.00000128;
            for (int i = 0; i < 14; i++)
            {
                scales[i] = scale;
                scale /= 2;
            }
            MyMap.Scales = scales;
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
            MyMap.ScalesChanged += MyMap_ScalesChanged;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += _timer_Tick;
            _timer1 = new DispatcherTimer();
            _timer1.Interval = TimeSpan.FromMilliseconds(1000);
            _timer1.Tick += _timer1_Tick;

        }

        void _timer1_Tick(object sender, object e)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            double num = random.NextDouble() * 6 - 3;
        
            MyMap.Pan(20*num,32*num);
        }

        void _timer_Tick(object sender, object e)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            double num = random.NextDouble() * 4 - 2;
            if (num < 0)
            {
                num = -1 / (num - 1);
            }
            else if (num > 0)
            {
                num = num + 1;
            }
            double newResolution = MyMap.Resolution * num;
            MyMap.Zoom(newResolution);

        }

        void MyMap_ScalesChanged(object sender, SuperMap.WinRT.Mapping.ScalesEventArgs e)
        {
            this.scale_changed.Text = "scalechanged: " + MyMap.Scale.ToString();
        }

        void MyMap_ViewBoundsChanged(object sender, SuperMap.WinRT.Mapping.ViewBoundsEventArgs e)
        {
            this.scale.Text = "scale: " + MyMap.Scale.ToString();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //double[] scales = new double[14];
            //double scale = 0.00006;
            //for (int i = 0; i < 14; i++)
            //{
            //    scales[i] = scale;
            //    scale /= 2;
            //}
            //MyMap.Scales = scales;
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //double[] scales = new double[14];
            //double scale = 0.0000089;
            //for (int i = 0; i < 14; i++)
            //{
            //    scales[i] = scale;
            //    scale /= 2;
            //}
            //MyMap.Scales = scales;
            if (_timer1.IsEnabled)
            {
                _timer1.Stop();
            }
            else
            {
                _timer1.Start();
            }
        }
    }
}
