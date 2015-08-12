using REST_SampleCode.Controls;
using SuperMap.WinRT.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace REST_SampleCode
{
    public partial class GeoRSS : Page
    {
        private ElementsLayer elementsLayer;
        private Pushpin slectedPP;
        private const string url = "http://earthquake.usgs.gov/eqcenter/catalogs/eqs7day-M2.5.xml";
        private InfoWindow window;

        public GeoRSS()
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
            elementsLayer = MyMap.Layers["MyElementsLayer"] as ElementsLayer;
            window = new InfoWindow(this.MyMap);
            MyMap.Layers["dynamicRESTLayer"].Failed += GeoRSS_Failed;
            this.MyMap.ViewBoundsChanged += new System.EventHandler<ViewBoundsEventArgs>(MyMap_ViewBoundsChanged);
            LoadRSS(url);
            DispatcherTimer updateTimer = new DispatcherTimer();
            updateTimer.Interval = new TimeSpan(0, 0, 0, 60000);
            updateTimer.Tick += (s, args) =>
            {
                LoadRSS(url);
            };
            updateTimer.Start();



        }

        async void GeoRSS_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private void MyMap_ViewBoundsChanged(object sender, ViewBoundsEventArgs e)
        {
            if (Rectangle2D.IsNullOrEmpty(e.OldViewBounds))
            {
                this.MyMap.ScreenContainer.Children.Add(window);
            }
        }

        private async void LoadRSS(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                string content = await client.GetStringAsync(uri);
                wc_OpenReadCompleted(content);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void wc_OpenReadCompleted(string content)
        {
            elementsLayer.Children.Clear();
            XDocument doc = XDocument.Parse(content);
            XNamespace geo = "http://www.w3.org/2003/01/geo/wgs84_pos#";

            var temp = (from tt in doc.Descendants("item")
                        where tt.Element(geo + "long") != null && tt.Element(geo + "lat") != null
                        select new RssClass
                        {
                            Lon = Convert.ToDouble(tt.Element(geo + "long").Value),
                            Lat = Convert.ToDouble(tt.Element(geo + "lat").Value),
                            Title = tt.Element("title").Value,
                            Desc = tt.Element("description").Value
                        }).ToList();

            foreach (var item in temp)
            {
                Point2D lonlat = new Point2D(item.Lon, item.Lat);
                Pushpin pushpin = new Pushpin() { Location = lonlat, Tag = item };
                pushpin.PointerPressed += pushpin_PointerPressed;
                pushpin.PointerReleased += pushpin_PointerReleased;
                elementsLayer.AddChild(pushpin);
            }

        }

        void pushpin_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }

        void pushpin_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            slectedPP = sender as Pushpin;
            RssClass info = (RssClass)slectedPP.Tag;

            window.Content = info.Desc;
            window.Title = info.Title;
            window.Location = new Point2D(info.Lon, info.Lat);
            window.ShowInfoWindow();
        }

        private class RssClass
        {
            public double Lon { get; set; }
            public double Lat { get; set; }
            public string Title { get; set; }
            public string Desc { get; set; }
        }

    }
}
