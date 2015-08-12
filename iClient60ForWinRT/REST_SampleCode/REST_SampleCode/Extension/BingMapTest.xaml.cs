using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Core;
using Windows.UI.Xaml.Controls;
using System.Text;
using Windows.UI.Xaml;
using Windows.Storage;
using System.Threading;

namespace REST_SampleCode
{
    public partial class BingMapTest : Page
    {
        TiledBingMapsLayer _layer;

        public BingMapTest()
        {
            InitializeComponent();

            _layer = myMap.Layers["bingMapLayer"] as TiledBingMapsLayer;
            this.Unloaded += BingMapTest_Unloaded;
        }

        void BingMapTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= BingMapTest_Unloaded;
            myMap.Dispose();
        }

    }

    public enum BMapsMode
    {
        Aerial,
        Road,
        AerialWithLabels
    }

    public class TiledBingMapsLayer : TiledCachedLayer
    {
        private const string RoadTileUri = "http://r{0}.tiles.ditu.live.com/tiles/r{1}.png?g=60";//汉语
        private const string AerialTileUri = "http://a{0}.ortho.tiles.virtualearth.net/tiles/a{1}.jpg?g=43";
        private const string AerialWithLabelsTileUri = "http://h{0}.ortho.tiles.virtualearth.net/tiles/h{1}.jpg?g=43";

        private string[] subDomains = { "0", "1", "2", "3" };
        private const double CornerCoordinate = 20037508.3427892;

        public TiledBingMapsLayer()
        { }

        public override MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken)
        {
            StringBuilder keyBuilder = new StringBuilder();
            for (int i = level; i >= 0; i--)
            {
                int num2 = ((int)1) << i;
                int num = ((indexX & num2) >> i) | (((indexY & num2) != 0) ? 2 : 0);
                keyBuilder.Append(num);
            }
            int num3 = ((indexY & 1) << 1) | (indexX & 1);
            MapImage mapImage = new MapImage();
            mapImage.MapImageType = MapImageType.Url;
            mapImage.Url = string.Format(this.Url, num3, keyBuilder);
            return mapImage;
        }

        public override void Initialize()
        {
            this.Bounds = new Rectangle2D(-CornerCoordinate, -CornerCoordinate, CornerCoordinate, CornerCoordinate);
            this.TileSize = 256;

            double res = CornerCoordinate * 2 / 512;
            double[] resolutions = new double[18];
            for (int i = 0; i < resolutions.Length; i++)
            {
                resolutions[i] = res;
                res *= 0.5;
            }
            this.Resolutions = resolutions;

            switch (this.Mode)
            {
                case BMapsMode.Road:
                    this.Url = RoadTileUri;
                    break;
                case BMapsMode.Aerial:
                    this.Url = AerialTileUri;
                    break;
                case BMapsMode.AerialWithLabels:
                    this.Url = AerialWithLabelsTileUri;
                    break;
            }

            base.Initialize();
        }

        public BMapsMode Mode
        {
            get { return (BMapsMode)GetValue(MapTypeProperty); }
            set { SetValue(MapTypeProperty, value); }
        }

        public static readonly DependencyProperty MapTypeProperty =
            DependencyProperty.Register("Mode", typeof(BMapsMode), typeof(TiledBingMapsLayer), new PropertyMetadata(BMapsMode.Aerial, new PropertyChangedCallback(new PropertyChangedCallback(OnMapTypePropertyChanged))));

        private static void OnMapTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TiledBingMapsLayer layer = d as TiledBingMapsLayer;
            if (layer.IsInitialized)
            {
                layer.ChangeTileSource();
            }
        }

        private void ChangeTileSource()
        {
            switch (this.Mode)
            {
                case BMapsMode.Road:
                    this.Url = RoadTileUri;
                    break;
                case BMapsMode.Aerial:
                    this.Url = AerialTileUri;
                    break;
                case BMapsMode.AerialWithLabels:
                    this.Url = AerialWithLabelsTileUri;
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
