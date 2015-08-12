

using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using System.Threading;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace REST_SampleCode
{
    public partial class OpenStreetMapTest : Page
    {
        TiledOpenStreetMapsLayer _layer;

        public OpenStreetMapTest()
        {
            InitializeComponent();
            _layer = myMap.Layers["openStreetLayer"] as TiledOpenStreetMapsLayer;
            this.Unloaded += OpenStreetMapTest_Unloaded;
        }

        void OpenStreetMapTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= OpenStreetMapTest_Unloaded;
            myMap.Dispose();
        }
    }

    public enum OMapsType
    {
        Mapnik,
        Osmarender,
        CycleMap,
        NoName
    }

    public class TiledOpenStreetMapsLayer : TiledCachedLayer
    {
        private string[] subDomains = { "a", "b", "c" };
        private const string MapnikUri = "http://{0}.tile.openstreetmap.org/{1}/{2}/{3}.png";
        private const string OsmarenderUri = "http://{0}.tah.openstreetmap.org/Tiles/tile/{1}/{2}/{3}.png";
        private const string CycleMapUri = "http://{0}.andy.sandbox.cloudmade.com/tiles/cycle/{1}/{2}/{3}.png";
        private const string NoNameUri = "http://{0}.tile.cloudmade.com/fd093e52f0965d46bb1c6c6281022199/3/256/{1}/{2}/{3}.png";

        private const double CornerCoordinate = 20037508.3427892;

        public TiledOpenStreetMapsLayer()
        {
        }

        public override MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken)
        {
            string subDomain = subDomains[(indexX + indexY + level) % subDomains.Length];
            MapImage mapImage = new MapImage();
            mapImage.MapImageType = MapImageType.Url;
            mapImage.Url=string.Format(this.Url, subDomain, level, indexX, indexY);
            return mapImage;
        }

        public override void Initialize()
        {
            this.Bounds = new Rectangle2D(-CornerCoordinate, -CornerCoordinate, CornerCoordinate, CornerCoordinate);
            this.TileSize = 256;

            double res = CornerCoordinate * 2 / 256;
            double[] resolutions = new double[8];
            for (int i = 0; i < resolutions.Length; i++)
            {
                resolutions[i] = res;
                res *= 0.5;
            }
            this.Resolutions = resolutions;

            switch (this.MapType)
            {
                case OMapsType.Mapnik:
                    this.Url = MapnikUri;
                    break;
                case OMapsType.Osmarender:
                    this.Url = OsmarenderUri;
                    break;
                case OMapsType.CycleMap:
                    this.Url = CycleMapUri;
                    break;
                case OMapsType.NoName:
                    this.Url = NoNameUri;
                    break;
            }
            base.Initialize();
        }

        public OMapsType MapType
        {
            get { return (OMapsType)GetValue(MapTypeProperty); }
            set { SetValue(MapTypeProperty, value); }
        }

        public static readonly DependencyProperty MapTypeProperty =
            DependencyProperty.Register("MapType", typeof(OMapsType), typeof(TiledOpenStreetMapsLayer), new PropertyMetadata(OMapsType.Mapnik
                ,(new PropertyChangedCallback(OnMapTypePropertyChanged))));

        private static void OnMapTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TiledOpenStreetMapsLayer layer = d as TiledOpenStreetMapsLayer;
            if (layer.IsInitialized)
            {
                layer.ChangeTileSource();
            }
        }

        private void ChangeTileSource()
        {
            switch (this.MapType)
            {
                case OMapsType.Mapnik:
                    this.Url = MapnikUri;
                    break;
                case OMapsType.Osmarender:
                    this.Url = OsmarenderUri;
                    break;
                case OMapsType.CycleMap:
                    this.Url = CycleMapUri;
                    break;
                case OMapsType.NoName:
                    this.Url = NoNameUri;
                    break;
            }
            if (!base.IsInitialized)
            {
                base.Initialize();
            }
            else
            {
                base.Refresh();
            }
        }
    }
}
