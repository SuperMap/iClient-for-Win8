using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Windows.Devices.Geolocation;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Mapping;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;
using WP8Sample.Common;
using System.Device.Location;
using Windows.Devices.Sensors;
using Microsoft.Devices;
using Windows.Storage;
using System.Windows.Threading;

namespace WP8Sample
{
    public partial class MainPage : PhoneApplicationPage
    {
        CloudLayer _layer;
        FeaturesLayer _fLayer;
        ElementsLayer _eLayer;
        ElementsLayer _pushpinLayer;
        bool _canPan = false;
        bool _mapInited = false;
        Point2D _location = Point2D.Empty;
        DispatcherTimer _addFeature;
        Queue<Point2D> _queue;
        Action _action;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            MyMap.ViewBounds = new Rectangle2D(11285095.6059184, 2296202.73281782, 13634177.6371684, 5702371.67813032);
            _layer = MyMap.Layers["CLayer"] as CloudLayer;
            _layer.LocalStorage = new OfflineStorage("CloudMap");
            _layer.Progress += _layer_Progress;
            _fLayer = MyMap.Layers["FLayer"] as FeaturesLayer;
            _eLayer = MyMap.Layers["ELayer"] as ElementsLayer;
            _pushpinLayer = MyMap.Layers["PushpinLayer"] as ElementsLayer;
            _queue = new Queue<Point2D>();

            Random randomX = new Random(1);
            Random ramdomY = new Random(2);
            double baseX = 101.95;
            double baseY = 29.3;
            for (int i = 0; i < 10; i++)
            {
                double x = baseX + randomX.NextDouble() * 2;
                double y = baseY + ramdomY.NextDouble() * 2;
                _queue.Enqueue(MercatorUtility.LatLonToMeters(new Point2D(x, y)));
            }

            MyMap.Tap += MyMap_Tap;
            MyMap.DoubleTap += MyMap_DoubleTap;
            MyMap.Hold += MyMap_Hold;
        }

        void MyMap_Hold(object sender, GestureEventArgs e)
        {
            if (_action != null)
            {
                _action.Hold(e);
            }
        }

        void MyMap_DoubleTap(object sender, GestureEventArgs e)
        {
            if (_action != null)
            {
                _action.DoubleTap(e);
            }
        }

        void MyMap_Tap(object sender, GestureEventArgs e)
        {
            if (_action != null)
            {
                _action.Tap(e);
            }
        }

        void _layer_Progress(object sender, ProgressEventArgs args)
        {
            if (args.Progress == 100 && !_mapInited)
            {
                _canPan = true;
                _mapInited = true;
            }
        }

        private void CompassButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (_canPan)
            {
                _canPan = false;
                Point2D pointLonLat = new Point2D(102.95, 30.3);
                Point2D pointM = MercatorUtility.LatLonToMeters(pointLonLat);
                MyMap.ZoomToLevel(8, pointM);
                Feature earthquake = new Feature();
                earthquake.Style = App.Current.Resources["MyMarkerStyle"] as MarkerStyle;
                earthquake.Geometry = new GeoPoint(pointM);
                _fLayer.AddFeature(earthquake);
                _addFeature = new DispatcherTimer();
                _addFeature.Interval = new TimeSpan(0, 0, 0, 0, 500);
                _addFeature.Tick += _addFeature_Tick;
                _addFeature.Start();
            }
        }

        void _addFeature_Tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    if (_queue.Count > 0)
                    {
                        Point2D point = _queue.Dequeue();
                        Pushpin pushpin = new Pushpin();
                        pushpin.Location = point;
                        _pushpinLayer.AddChild(pushpin);
                    }
                    else
                    {
                        _addFeature.Stop();
                        _addFeature.Tick -= _addFeature_Tick;
                    }
                });
        }

        private async Task<Point2D> LocationOffer(Point2D location)
        {
            string url = string.Format(CultureInfo.InvariantCulture, "http://42.120.50.220:7080/MapFactoryServices/GPSService?x={0}&y={1}", location.X.ToString(), location.Y.ToString());
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            TaskFactory factory = new TaskFactory();
            try
            {
                HttpWebResponse response = (HttpWebResponse)await factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = await reader.ReadToEndAsync();
                reader.Dispose();
                if (!string.IsNullOrEmpty(result))
                {
                    string[] array = result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (array.Length == 2)
                    {
                        double x = 0;
                        double y = 0;
                        if (double.TryParse(array[0], out x) && double.TryParse(array[1], out y))
                        {
                            return new Point2D(x, y);
                        }
                    }
                }
            }
            catch
            {

            }
            return Point2D.Empty;
        }

        private void DrawPoint_Click_1(object sender, EventArgs e)
        {
            if (_action != null)
            {
                _action.Deactivate();
            }
            _action = new DrawPoint(MyMap, _eLayer);
        }

        private void Measure_Click_1(object sender, EventArgs e)
        {
            if (_action != null)
            {
                _action.Deactivate();
            }
            _action = new MeasureDistance(MyMap, _eLayer, _fLayer);
        }

        private void Clear_Click_1(object sender, EventArgs e)
        {
            if (_action != null)
            {
                _action.Deactivate();
            }
            _action = null;
        }


    }
}