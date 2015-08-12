using REST_SampleCode.Controls;
using SuperMap.WinRT.Actions;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System;
using System.Windows;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace REST_SampleCode
{
    public partial class InRegionCheck : Page
    {
        private FeaturesLayer pointsLayer;
        private FeaturesLayer regionLayer;
        private GeoRegion region;
        private Random random;
        private int X;
        private int Y;
        private int count;
        TiledDynamicRESTLayer _layer;

        public InRegionCheck()
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
            pointsLayer = this.MyMap.Layers["MyPointsLayer"] as FeaturesLayer;
            regionLayer = this.MyMap.Layers["MyRegionLayer"] as FeaturesLayer;
            _layer = MyMap.Layers["tiledLayer"] as TiledDynamicRESTLayer;
            _layer.LocalStorage = new OfflineStorage("World");
            _layer.Failed += InRegionCheck_Failed;
            this.Unloaded += InRegionCheck_Unloaded;
            random = new Random();
            for (int i = 0; i < 100; i++)
            {
                X = random.Next(-160, 160);
                Y = random.Next(-60, 60);

                Feature feature = new Feature
                {
                    Geometry = new GeoPoint
                    {
                        X = X,
                        Y = Y
                    },
                    Style = new PredefinedMarkerStyle
                    {
                        Color = new SolidColorBrush
                        {
                            Color = Colors.Red
                        }
                    }
                };

                pointsLayer.AddFeature(feature);
            }
        }

        void InRegionCheck_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= InRegionCheck_Unloaded;
            _layer.Failed -= InRegionCheck_Failed;
            MyMap.Dispose();
        }

        async void InRegionCheck_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        //绘制面
        private void btn_FreeRegion_Click(object sender, RoutedEventArgs e)
        {
            DrawPolygon polygon = new DrawPolygon(this.MyMap)
            {
                Fill = regionColor,
                Stroke = new SolidColorBrush { Color=Colors.Blue}
            };
            this.MyMap.Action = polygon;
            polygon.DrawCompleted += new EventHandler<DrawEventArgs>(polygon_DrawCompleted);
        }

        //绘制面结束后将所绘制的面转化为矢量要素并加载到矢量要素图层中
        private void polygon_DrawCompleted(object sender, DrawEventArgs e)
        {
            //将绘制面转化为面要素并加载到矢量要素图层中
            Feature feature = new Feature()
            {
                Geometry = e.Geometry as GeoRegion,
                Style = new PredefinedFillStyle 
                {
                    Fill = regionColor1
                }
            };

            regionLayer.Features.Add(feature);
            region = e.Geometry as GeoRegion;

            Check();
        }

        //判断点是否在面内
        async private void Check()
        {
            count = 0;
            foreach (var item in pointsLayer.Features)
            {
                if (CheckInRegion.CheckRegion(region, ((GeoPoint)item.Geometry).Location))
                {
                    item.Style = new PredefinedMarkerStyle { Color = new SolidColorBrush { Color = Colors.Green } };
                    count++;
                }
            }

           await  MessageBox.Show(string.Format("{0} 个点在面区域内", count));
        }

        //清除矢量要素图层中全部元素
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            if (this.regionLayer != null)
            {
                this.regionLayer.Features.Clear();
            }

            foreach (var item in pointsLayer.Features)
            {
                item.Style = new PredefinedMarkerStyle { Color = new SolidColorBrush { Color = Colors.Red } };
            }
        }
    }

    public static class CheckInRegion
    {
        public static bool CheckRegion(GeoRegion region, Point2D point)
        {
            Point2DCollection points = region.Parts[0];

            double pValue = double.NaN;
            int i = 0;
            int j = 0;

            double yValue = double.NaN;
            int m = 0;
            int n = 0;

            double iPointX = double.NaN;
            double iPointY = double.NaN;
            double jPointX = double.NaN;
            double jPointY = double.NaN;

            int k = 0;
            int p = 0;

            yValue = points[0].Y - points[(points.Count - 1)].Y;
            if (yValue < 0)
            {
                p = 1;
            }
            else if (yValue > 0)
            {
                p = 0;
            }
            else
            {
                m = points.Count - 2;
                n = m + 1;
                while (points[m].Y == points[n].Y)
                {
                    m--;
                    n--;
                    if (m == 0)
                    {
                        return true;
                    }
                }
                yValue = points[n].Y - points[m].Y;
                if (yValue < 0)
                {
                    p = 1;
                }
                else if (yValue > 0)
                {
                    p = 0;
                }
            }


            //使多边形封闭
            int count = points.Count;
            i = 0;
            j = count - 1;
            while (i < count)
            {
                iPointX = points[j].X;
                iPointY = points[j].Y;
                jPointX = points[i].X;
                jPointY = points[i].Y;
                if (point.Y > iPointY)
                {
                    if (point.Y < jPointY)
                    {
                        pValue = (point.Y - iPointY) * (jPointX - iPointX) / (jPointY - iPointY) + iPointX;
                        if (point.X < pValue)
                        {
                            k++;
                        }
                        else if (point.X == pValue)
                        {
                            return true;
                        }
                    }
                    else if (point.X == jPointY)
                    {
                        p = 0;
                    }
                }
                else if (point.Y < iPointY)
                {
                    if (point.Y > jPointY)
                    {
                        pValue = (point.Y - iPointY) * (jPointX - iPointX) / (jPointY - iPointY) + iPointX;
                        if (point.X < pValue)
                        {
                            k++;
                        }
                        else if (point.X == pValue)
                        {
                            return true;
                        }
                    }
                    else if (point.Y == jPointY)
                    {
                        p = 1;
                    }
                }
                else
                {
                    if (point.X == iPointX)
                    {
                        return true;
                    }
                    if (point.Y < jPointY)
                    {
                        if (p != 1)
                        {
                            if (point.X < iPointX)
                            {
                                k++;
                            }
                        }
                    }
                    else if (point.Y > jPointY)
                    {
                        if (p > 0)
                        {
                            if (point.X < iPointX)
                            {
                                k++;
                            }
                        }
                    }
                    else
                    {
                        if (point.X > iPointX && point.X <= jPointX)
                        {
                            return true;
                        }
                        if (point.X < iPointX && point.X >= jPointX)
                        {
                            return true;
                        }
                    }
                }
                j = i;
                i++;
            }

            if (k % 2 != 0)
            {
                return true;
            }
            return false;
        }
    }

}
