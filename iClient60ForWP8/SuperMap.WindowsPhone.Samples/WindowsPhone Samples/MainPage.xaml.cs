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
using SuperMap.WindowsPhone.Samples.Common;
using System.Device.Location;
using Windows.Devices.Sensors;
using Microsoft.Devices;

namespace SuperMap.WindowsPhone.Samples
{
    public partial class MainPage : PhoneApplicationPage
    {
        CloudLayer _layer;
        Feature _compassFeature;
        FeaturesLayer _fLayer;
        ElementsLayer _eLayer;
        GeoCoordinateWatcher _watcher;
        Compass _compass;
        bool _mapInited = false;
        bool _needPan = false;
        Point2D _location = Point2D.Empty;
        Action _action;
        bool _isMeasure;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _fLayer = MyMap.Layers["FLayer"] as FeaturesLayer;
            _eLayer = MyMap.Layers["ELayer"] as ElementsLayer;
            _layer = MyMap.Layers["CLayer"] as CloudLayer;
            _layer.LocalStorage = new OfflineStorage("CloudMap");
            MyMap.ViewBoundsChanged += MyMap_ViewBoundsChanged;
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            _watcher.MovementThreshold = 20;
            _watcher.PositionChanged += _watcher_PositionChanged;
            _watcher.StatusChanged += _watcher_StatusChanged;
            _compass = Compass.GetDefault();
            if (_compass != null)
            {
                _compass.ReadingChanged += _compass_ReadingChanged;
                _compass.ReportInterval = 100;
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

        void _compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    if (_compassFeature != null)
                    {
                        CompassMarkerStyle style = _compassFeature.Style as CompassMarkerStyle;
                        style.Rotation = (args.Reading.HeadingTrueNorth.HasValue ? args.Reading.HeadingTrueNorth.Value : 0);
                    }
                });
        }

        void _watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                case GeoPositionStatus.NoData:
                    _watcher.Stop();
                    MessageBox.Show("无法获取位置信息");
                    break;
                case GeoPositionStatus.Ready:
                    _watcher.Start();
                    break;
            }
        }

        void _watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            LocationChanged(e.Position.Location);
        }

        void MyMap_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            if (!_mapInited && !Rectangle2D.IsNullOrEmpty(e.NewViewBounds))
            {
                _mapInited = true;
                MyMap.ZoomToLevel(8, new Point2D(12969134.4929308, 4863396.19924929));
                _compassFeature = new Feature();
                _compassFeature.Geometry = new GeoPoint(Point2D.Empty);
                CompassMarkerStyle style = new CompassMarkerStyle();
                style.Height = 40;
                style.Width = 30;
                style.Rotation = 0;
                _compassFeature.Style = style;
                _fLayer.AddFeature(_compassFeature);
                _needPan = true;
                _watcher.Start();
            }
        }

        private void CompassButton_Click_1(object sender, EventArgs e)
        {
            _needPan = true;
            if (_compassFeature != null)
            {
                GeoPoint location = _compassFeature.Geometry as GeoPoint;
                if (!Point2D.IsNullOrEmpty(location.Location))
                {
                    MyMap.PanTo(location.Location);
                }
            }
            _watcher.Stop();
            _watcher.Start();
        }

        private void LocationChanged(GeoCoordinate position)
        {
            Dispatcher.BeginInvoke(async() =>
                {
                    CompassMarkerStyle style = _compassFeature.Style as CompassMarkerStyle;
                    GeoPoint location = _compassFeature.Geometry as GeoPoint;
                    Point2D locationOffer = await LocationOffer(new Point2D(position.Longitude, position.Latitude));
                    Point2D met = MercatorUtility.LatLonToMeters(new Point2D(locationOffer.X, locationOffer.Y));
                    location.Location = new Point2D(met.X, met.Y);
                    if (_needPan)
                    {
                        if (Math.Abs(position.Longitude - _location.X) > 0.005 || Math.Abs(position.Latitude - _location.Y) > 0.005)
                        {
                            MyMap.PanTo(location.Location);
                        }
                        _needPan = false;
                    }
                    _location = new Point2D(position.Longitude, position.Latitude);
                });
        }

        private async Task<Point2D> LocationOffer(Point2D location)
        {
            string url = string.Format(CultureInfo.InvariantCulture,"http://42.120.50.220:7080/MapFactoryServices/GPSService?x={0}&y={1}", location.X.ToString(), location.Y.ToString());
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            TaskFactory factory = new TaskFactory();
            try
            {
                HttpWebResponse response = (HttpWebResponse)await factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string result = await reader.ReadToEndAsync();
                reader.Dispose();
                response.Dispose();
                response.Close();
                request.Abort();
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
            MyMap.IsDoubleTap = false;
            
        }

        private void Measure_Click_1(object sender, EventArgs e)
        {
            if (_action != null)
            {
                _action.Deactivate();
            }
            _isMeasure = true;
            _action = new MeasureDistance(MyMap, _eLayer, _fLayer,_isMeasure);
            MyMap.IsDoubleTap = true;
        }

        private void Clear_Click_1(object sender, EventArgs e)
        {
            if (_action != null)
            {
                _action.Deactivate();
            }
            _action = null;
            MyMap.IsDoubleTap = true;
        }

        private void ZoomIn_Click_1(object sender, EventArgs e)
        {
            this.MyMap.ZoomIn();
        }

        private void ZoomOut_Click_1(object sender, EventArgs e)
        {
            this.MyMap.ZoomOut();
        }

    }
}