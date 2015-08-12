using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Resources;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// 	<para>${WP_mapping_HeatMapLayer_Title}</para>
    /// 	<para>${WP_mapping_HeatMapLayer_Description}</para>
    /// 	<para><img src="heatmap.jpg"/></para>
    /// </summary>
    public class HeatMapLayer : DynamicLayer
    {
        private HeatPointCollection heatPoints;
        private Rectangle2D fullBounds;
        private BackgroundWorker worker;
        private DispatcherTimer timer;

        /// <summary>${WP_mapping_HeatMapLayer_constructor_None_D}</summary>
        public HeatMapLayer()
        {
            heatPoints = new HeatPointCollection();
            heatPoints.CollectionChanged += new NotifyCollectionChangedEventHandler(heatPoints_CollectionChanged);
            fullBounds = Rectangle2D.Empty;

            //设置GradientStops的默认值
            GradientStopCollection stops = new GradientStopCollection();
            stops.Add(new GradientStop() { Color = Color.FromArgb(0, 0, 0, 0), Offset = 0 });
            stops.Add(new GradientStop() { Color = new Color() { A = 0xff, R = 0xff, G = 0x42, B = 0x00 }, Offset = 0.75 });
            stops.Add(new GradientStop() { Color = new Color() { A = 0xff, R = 0xff, G = 0xff, B = 0x00 }, Offset = .5 });
            stops.Add(new GradientStop() { Color = new Color() { A = 0xff, R = 0x1a, G = 0xa4, B = 0x03 }, Offset = .25 });
            GradientStops = stops;

            //初始化后台线程
            worker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = sender as BackgroundWorker;
            //hps, width, height, Radius, Intensity, stops, onCompleted
            object[] args = (object[])e.Argument;
            List<PixelPoint> points = (List<PixelPoint>)args[0];
            double res = (double)args[4];
            int width = (int)Math.Ceiling((int)args[1] * res);
            int height = (int)Math.Ceiling((int)args[2] * res);
            int radius = (int)Math.Ceiling((int)args[3] * res);
            List<ThreadSafeGradientStop> stops = (List<ThreadSafeGradientStop>)args[5];
            OnImageSourceCompleted onCompleted = (OnImageSourceCompleted)args[6];
            double mapReslution = (double)args[7];
            //地理半径
            int geoToScreenRadius;

            //if (radius % 2 == 0)
            //{
            //    radius++;
            //}
            //radius = radius * 2 + 1;
            ushort[] matrix = CreateDistanceMatrix(radius);
            int[] output = new int[width * height];
            foreach (PixelPoint p in points)
            {
                //如果当前点设置地理半径，就用这样的方法
                if (p.GeoRadius != 0 )
                {
                    geoToScreenRadius = (int)Math.Round(p.GeoRadius / mapReslution);
                    //如果显示的地理半径小于1像素则不进行显示
                    if (!(geoToScreenRadius < 1))
                    {
                        ushort[] matrixGeoRadius = CreateDistanceMatrix(geoToScreenRadius);
                        AddPoint(matrixGeoRadius, geoToScreenRadius, p.X, p.Y, p.Value, output, width);
                    }
                }
                else
                {
                    AddPoint(matrix, radius, p.X, p.Y, p.Value, output, width);
                }
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    e.Result = null;
                    return;
                }
            }
            matrix = null;
            int max = 0;
            foreach (int val in output)
            {
                if (max < val)
                {
                    max = val;
                }
            }
            if (max < 2)
            {
                max = 2;
            }

            int[] pixels = new int[width * height];
            for (int i = 0; i < height; i++)      // Height (y)
            {
                for (int j = 0; j < width; j++)     // Width (x)
                {
                    Color c = InterpolateColor(output[i * width + j] / (float)max, stops);

                    if (c.A == 0)
                    {
                        pixels[i * width + j] = 0;
                    }
                    else
                    {
                        //byte[] brg = new byte[4] { c.B, c.G, c.R, c.A };
                        //int pixel = BitConverter.ToInt32(brg, 0);//ok
                        int pixel = (c.A << 24) | (c.R << 16) | (c.G << 8) | c.B;
                        pixels[i * width + j] = pixel;
                    }
                }
                if (bgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    e.Result = null;
                    output = null;
                    pixels = null;
                    return;
                }
                //一行报告进度一次

                //调用 ReportProgress 方法时将引发ProgressChanged 事件。
                bgWorker.ReportProgress((i + 1) * 100 / height);
            }
            stops.Clear();
            output = null;

            e.Result = new object[] { pixels, width, height, onCompleted };
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled || e.Result == null)
            {
                return;
            }

            object[] result = (object[])e.Result;
            int[] pixels = (int[])result[0];
            int width = (int)result[1];
            int height = (int)result[2];
            OnImageSourceCompleted onCompleted = (OnImageSourceCompleted)result[3];
            WriteableBitmap wb = new WriteableBitmap(width, height);
            for (int i = 0; i < height; i++)      // Height (y)
            {
                for (int j = 0; j < width; j++)     // Width (x)
                {
                    int temp = pixels[i * width + j];
                    wb.Pixels[i * width + j] = temp;
                }
            }

            onCompleted(wb);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnProgress(e.ProgressPercentage);
        }

        /// <summary>${WP_mapping_HeatMapLayer_method_GetImageSource_D}</summary>
        protected override void GetImageSource(DynamicLayer.OnImageSourceCompleted onCompleted)
        {
            if (!IsInitialized)
            {
                onCompleted(null);
            }
            else
            {
                if (worker != null && worker.IsBusy)
                {
                    worker.CancelAsync();
                    while (worker.IsBusy)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            List<ThreadSafeGradientStop> stops = new List<ThreadSafeGradientStop>();
            foreach (GradientStop item in GradientStops)
            {
                stops.Add(new ThreadSafeGradientStop { Color = item.Color, Offset = item.Offset });
            }
            //stops.Sort((ThreadSafeGradientStop g1, ThreadSafeGradientStop g2) => { return g1.Offset.CompareTo(g2.Offset); });

            stops.Sort((Comparison<ThreadSafeGradientStop>)((g1, g2) => g1.Offset.CompareTo(g2.Offset)));

            List<PixelPoint> pps = new List<PixelPoint>(this.GradientStops.Count);

            foreach (HeatPoint point in this.heatPoints)
            {
                if (ViewBounds.Contains(new Point2D(point.X, point.Y)) && point.Value > 0)
                {

                    PixelPoint pp = new PixelPoint
                    {
                        X = (int)Math.Round((point.X - ViewBounds.Left) / Resolution * Intensity),
                        Y = (int)Math.Round((ViewBounds.Top - point.Y) / Resolution * Intensity),
                        Value = (int)Math.Round(point.Value),
                        //判断给定的地理半径是否小于0，小于0则设置成0
                        GeoRadius = point.GeoRadius < 0 ? 0 : point.GeoRadius,
                    };
                    pps.Add(pp);
                }
            }

            //在视图范围内的转换为像素空间坐标
            //for (int i = 0; i < heatPoints.Count; i++)
            //{
            //    if (ViewBounds.Contains(new Point2D(heatPoints[i].X, heatPoints[i].Y)) && heatPoints[i].Value > 0)
            //    {
            //        PixelPoint point = new PixelPoint
            //        {
            //            X = (int)Math.Round((heatPoints[i].X - ViewBounds.Left) / Resolution * Intensity),
            //            Y = (int)Math.Round((ViewBounds.Top - heatPoints[i].Y) / Resolution * Intensity),
            //            Value = (int)Math.Round(heatPoints[i].Value)
            //        };
            //        pps.Add(point);
            //        //按照权重多加value遍；
            //        ////int sum = (int)Math.Round(heatPoints[i].Value);
            //        ////for (int k = 0; k < sum; k++)
            //        ////{
            //        ////    hps.Add(point);
            //        ////}
            //    }
            //}

            int width = (int)Math.Round(ViewSize.Width);
            int height = (int)Math.Round(ViewSize.Height);
            //调用 RunWorkerAsync 方法时将引发DoWork事件。这里就是您启动耗时操作的地方
            worker.RunWorkerAsync(new object[] { pps, width, height, Radius, Intensity, stops, onCompleted, this.Map.Resolution });
        }

        /// <summary>${WP_mapping_HeatMapLayer_method_GetImageUrl_D}</summary>
        protected override string GetImageUrl()
        {
            return string.Empty;
        }

        /// <summary>${WP_mapping_HeatMapLayer_method_Cancel_D}</summary>
        protected override void Cancel()
        {
            if (worker != null && worker.IsBusy)
            {
                worker.CancelAsync();
            }
            base.Cancel();
        }

        private static void AddPoint(ushort[] distanceMatrix, int radius, int x, int y, int value, int[] output, int width)
        {
            int diameter = radius * 2 - 1;
            for (int i = 0; i < diameter; i++)
            {
                int start = (y - radius + 1 + i) * width + x - radius;
                for (int j = 0; j < diameter; j++)
                {
                    if (j + x - radius < 0 || j + x - radius >= width)
                    {
                        continue;
                    }
                    int idx = start + j;
                    if (idx < 0 || idx >= output.Length)
                    {
                        continue;
                    }
                    output[idx] += distanceMatrix[i * diameter + j] * value;
                }
            }
        }

        //举例radius=10,则21行21列,即左10右10中间1列,行类似。
        private static ushort[] CreateDistanceMatrix(int radius)
        {
            int diameter = radius * 2 - 1;
            ushort[] matrix = new ushort[(int)Math.Pow(diameter, 2)];
            for (int i = 0; i < diameter; i++)
            {
                for (int j = 0; j < diameter; j++)
                {
                    matrix[i * diameter + j] = (ushort)Math.Max((radius - (Math.Sqrt(Math.Pow(i - radius + 1, 2) + Math.Pow(j - radius + 1, 2)))), 0);
                }//距离中心点为(radius,radius)越近，值则越大。介于0~radius之间
            }
            return matrix;
        }
        private static Color InterpolateColor(float value, List<ThreadSafeGradientStop> stops)
        {
            if (value < 1 / 255f)
            {
                return Colors.Transparent;
            }
            if (stops == null || stops.Count == 0)
            {
                return Colors.Black;
            }
            if (stops.Count == 1)
            {
                return stops[0].Color;
            }

            if (stops[0].Offset >= value) //clip to bottom
            {
                return stops[0].Color;
            }
            else if (stops[stops.Count - 1].Offset <= value) //clip to top
            {
                return stops[stops.Count - 1].Color;
            }

            int i = 0;
            for (i = 1; i < stops.Count; i++)
            {
                if (stops[i].Offset > value)
                {
                    Color start = stops[i - 1].Color;
                    Color end = stops[i].Color;

                    double frac = (value - stops[i - 1].Offset) / (stops[i].Offset - stops[i - 1].Offset);
                    byte R = (byte)Math.Round((start.R * (1 - frac) + end.R * frac));
                    byte G = (byte)Math.Round((start.G * (1 - frac) + end.G * frac));
                    byte B = (byte)Math.Round((start.B * (1 - frac) + end.B * frac));
                    byte A = (byte)Math.Round((start.A * (1 - frac) + end.A * frac));
                    return Color.FromArgb(A, R, G, B);
                }
            }
            return stops[stops.Count - 1].Color;
        }

        /// <summary>${WP_mapping_HeatMapLayer_attribute_bounds_D}</summary>
        public new Rectangle2D Bounds
        {
            get
            {
                if (fullBounds.IsEmpty && heatPoints != null && heatPoints.Count > 0)
                {
                    fullBounds = Rectangle2D.Empty;
                    foreach (HeatPoint point in this.heatPoints)
                    {
                        fullBounds.Union(new Point2D(point.X, point.Y));
                    }
                }
                return fullBounds;
            }
        }

        /// <summary>${WP_mapping_HeatMapLayer_attribute_heatPoints_D}</summary>
        public HeatPointCollection HeatPoints
        {
            get
            {
                return heatPoints;
            }
            set
            {
                if (value != null)
                {
                    heatPoints = value;
                    OnLayerChanged();
                    heatPoints.CollectionChanged += new NotifyCollectionChangedEventHandler(heatPoints_CollectionChanged);
                    fullBounds = Rectangle2D.Empty;
                }
                else
                {
                    heatPoints.Clear();
                }

            }
        }

        private void heatPoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnLayerChanged();
        }

        /// <summary>${WP_mapping_HeatMapLayer_attribute_radius_D}</summary>
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>${WP_mapping_HeatMapLayer_field_bufferProperty_D}</summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(HeatMapLayer), new PropertyMetadata(40, OnIntensityPropertyChanged));
        private static void OnIntensityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < 1)
            {
                //TODO:Resource
                //throw new ArgumentOutOfRangeException("Radius");
                throw new ArgumentOutOfRangeException(ExceptionStrings.RadiusLessThanOne);
            }
            HeatMapLayer layer = d as HeatMapLayer;
            if (layer.IsInitialized)
            {
                if (layer.timer == null)
                {
                    layer.timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(50.0) };

                    layer.timer.Tick += (s1, e1) =>
                    {
                        layer.timer.Stop();
                        layer.OnLayerChanged();
                    };

                }
                layer.timer.Stop();
                layer.timer.Start();
            }
            layer.OnLayerChanged();
        }

        //0~1
        /// <summary>${WP_mapping_HeatMapLayer_attribute_intensity_D}</summary>
        public double Intensity
        {
            get { return (double)GetValue(IntensityProperty); }
            set { SetValue(IntensityProperty, value); }
        }
        /// <summary>${WP_mapping_HeatMapLayer_field_intensityProperty_D}</summary>
        public static readonly DependencyProperty IntensityProperty =
            DependencyProperty.Register("Intensity", typeof(double), typeof(HeatMapLayer), new PropertyMetadata(1.0, OnPrecisionPropertyChanged));
        private static void OnPrecisionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeatMapLayer layer = d as HeatMapLayer;
            double newValue = (double)e.NewValue;
            if (newValue <= 0 || newValue > 1)
            {
                //TODO:Resource
                //throw new ArgumentOutOfRangeException("Intensity");
                throw new ArgumentOutOfRangeException(ExceptionStrings.IvalidIntensity);
            }
            layer.OnLayerChanged();
        }

        /// <summary>${WP_mapping_HeatMapLayer_attribute_gradientStops_D}</summary>
        public GradientStopCollection GradientStops
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty); }
            set { SetValue(GradientStopsProperty, value); }
        }
        /// <summary>${WP_mapping_HeatMapLayer_field_gradientStopsProperty_D}</summary>
        public static readonly DependencyProperty GradientStopsProperty =
            DependencyProperty.Register("GradientStops", typeof(GradientStopCollection), typeof(HeatMapLayer), new PropertyMetadata(null, OnGradientStopsPropertyChanged));
        private static void OnGradientStopsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeatMapLayer layer = d as HeatMapLayer;
            layer.OnLayerChanged();
        }

        private struct ThreadSafeGradientStop
        {
            public double Offset;
            public Color Color;
        }

        private struct PixelPoint
        {
            public int X;
            public int Y;
            public int Value;
            public double GeoRadius;
        }//像素坐标 并且线程安全

    }

    /// <summary>
    /// 	<para>${WP_mapping_HeatMapLayer_HeatPoint_Title}</para>
    /// 	<para>${WP_mapping_HeatMapLayer_HeatPoint_Description}</para>
    /// </summary>
    public struct HeatPoint
    {
        /// <summary>${WP_mapping_HeatMapLayer_HeatPoint_constructor_None_D}</summary>
        public HeatPoint(double x, double y, double value, double geoRadius)
        {
            Value = value;
            X = x;
            Y = y;
            GeoRadius = geoRadius;
        }
        /// <summary>${WP_mapping_HeatMapLayer_HeatPoint_attribute_x_D}</summary>
        public double X;
        /// <summary>${WP_mapping_HeatMapLayer_HeatPoint_attribute_y_D}</summary>
        public double Y;
        /// <summary>${WP_mapping_HeatMapLayer_HeatPoint_attribute_value_D}</summary>
        public double Value;
        /// <summary>${WP_mapping_HeatMapLayer_HeatPoint_attribute_GeoRadius_D}</summary>
        public double GeoRadius;
        /// <summary>${WP_mapping_HeatMapLayer_operators_DoubleEquals_D}<br/></summary>
        /// <returns>${WP_mapping_HeatMapLayer_operators_DoubleEquals_return}</returns>
        /// <param name="p1">${WP_mapping_HeatMapLayer_operators_DoubleEquals_param_point}</param>
        /// <param name="p2">${WP_mapping_HeatMapLayer_operators_DoubleEquals_param_point}<br/></param>
        public static bool operator ==(HeatPoint p1, HeatPoint p2)
        {
            return (p1.X == p2.X) && (p1.Y == p2.Y) && (p1.Value == p2.Value);
        }
        /// <summary>${WP_mapping_HeatMapLayer_operators_NotEqual_D}<br/></summary>
        /// <returns>${WP_mapping_HeatMapLayer_operators_NotEqual_return}</returns>
        /// <param name="p1">${WP_mapping_HeatMapLayer_operators_DoubleEquals_param_point}</param>
        /// <param name="p2">${WP_mapping_HeatMapLayer_operators_DoubleEquals_param_point}<br/></param>
        public static bool operator !=(HeatPoint p1, HeatPoint p2)
        {
            return !(p1 == p2);
        }
        /// <overloads>${WP_mapping_HeatMapLayer_method_equals_overloads}</overloads>
        /// <summary>${WP_mapping_HeatMapLayer_method_equals_Object_D}</summary>
        /// <returns>${WP_mapping_HeatMapLayer_method_equals_Object_return}</returns>
        /// <param name="obj">${WP_mapping_HeatMapLayer_method_equals_Object_param_object}</param>
        public override bool Equals(object obj)
        {
            if (!(obj is HeatPoint))
            {
                return false;
            }
            return Equals((HeatPoint)obj);
        }
        /// <returns>${WP_mapping_HeatMapLayer_method_equals_HeatPoint_return}</returns>
        /// <summary>${WP_mapping_HeatMapLayer_method_equals_HeatPoint_D}<br/></summary>
        /// <overloads>${WP_mapping_HeatMapLayer_method_equals_overloads}</overloads>
        /// <param name="point">${WP_mapping_HeatMapLayer_method_equals_HeatPoint_param_point}<br/></param>
        public bool Equals(HeatPoint point)
        {
            return (this == point);
        }
        /// <summary>${WP_mapping_HeatMapLayer_method_GetHashCode_D}</summary>
        public override int GetHashCode()
        {
            return (this.X.GetHashCode() ^ this.Y.GetHashCode());
        }
    }
}
