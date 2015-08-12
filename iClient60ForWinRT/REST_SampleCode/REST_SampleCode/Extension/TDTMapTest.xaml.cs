using Windows.UI.Xaml;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.Core;
using Windows.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.Storage;
using System.Threading;

namespace REST_SampleCode
{
    public partial class TDTMapTest : Page
    {
        String m_layerType = "vec";
        String m_projection = "900913";

        public TDTMapTest()
        {
            InitializeComponent();
            TileTDTMapsLayer layer1 = myMap.Layers["layer1"] as TileTDTMapsLayer;
            TileTDTMapsLayer layer2 = myMap.Layers["layer2"] as TileTDTMapsLayer;
            SetCacheFolder(layer1);
            SetCacheFolder(layer2);
            this.Unloaded += TDTMapTest_Unloaded;
        }

        void TDTMapTest_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= TDTMapTest_Unloaded;
            myMap.Dispose();
        }

        private async void SetCacheFolder(TileTDTMapsLayer layer)
        {
            string mapName = string.Empty;
            if (layer.IsLabel)
            {
                mapName += "Label";
            }
            else
            {
                mapName += "Map";
            }
            mapName += "_" + m_layerType + "_" + m_projection;
            layer.LocalStorage = new OfflineStorage(mapName);
        }

        private void comboBoxProjType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxProjType != null)
            {
                if (comboBoxProjType.SelectedIndex == 0)
                {
                    m_layerType = "vec";
                }
                else if (comboBoxProjType.SelectedIndex == 1)
                {
                    m_layerType = "img";
                }
                else if (comboBoxProjType.SelectedIndex == 2)
                {
                    m_layerType = "ter";
                }
                RefreshMap();
            }

        }

        private void comboBoxMapType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxMapType != null)
            {
                if (comboBoxMapType.SelectedIndex == 0)
                {
                    m_projection = "900913";
                }
                else if (comboBoxMapType.SelectedIndex == 1)
                {
                    m_projection = "4326";
                }
                RefreshMap();
            }
        }
        //移除图层重新加载
        private void RefreshMap()
        {
            myMap.Layers.Clear();
            TileTDTMapsLayer layer1 = new TileTDTMapsLayer();
            layer1.LayerType = m_layerType;
            layer1.Projection = m_projection;
            TileTDTMapsLayer layer2 = new TileTDTMapsLayer();
            layer2.IsLabel = true;
            layer2.LayerType = m_layerType;
            layer2.Projection = m_projection;
            myMap.Layers.Add(layer1);
            myMap.Layers.Add(layer2);
            SetCacheFolder(layer1);
            SetCacheFolder(layer2);
        }
    }

    public class TileTDTMapsLayer : TiledCachedLayer
    {
        //是否是标签图层
        private Boolean isLabel = false;

        //投影坐标系，天地图支持4326和3857两种，分别对应EPSG:4326和EPSG:900913
        private string projection = "900913";


        //EPSG:4326对应的是c，而EPSG:900913对应的是w
        private string proj = "c";
        //vec:矢量图层，cva:矢量标签图层，img:影像图层,cia:影像标签图层，ter:地形,cta:地形标签图层
        private string layerType = "vec";
        private Int32 levelOffset = 1;
        private const string URL = "http://t{subdomain}.tianditu.com/DataServer?T={layerType}_{proj}&x={tileX}&y={tileY}&l={level}";


        //定义基本URL
        private string[] subDomains = { "0", "1", "2", "3", "4", "5" };


        public Boolean IsLabel
        {
            get { return this.isLabel; }
            set { this.isLabel = value; }
        }
        public string Projection
        {
            get { return this.projection; }
            set { this.projection = value; RefreshResolutions(); }
        }

        //定义地图类型的依赖属性，只允许设置为
        public string LayerType
        {
            get { return this.layerType; }
            set { this.layerType = value; }
        }


        private static void OnMapTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TileTDTMapsLayer layer = d as TileTDTMapsLayer;
            if (layer.IsInitialized)
            {
                layer.ChangeTileSource();
            }
        }

        //改变地图类型触发刷新
        private void ChangeTileSource()
        {
            if (!base.IsInitialized)
            {
                base.Initialize();
            }
            else
            {
                base.Refresh();
            }
        }

        public TileTDTMapsLayer()
        { }

        public override void Initialize()
        {
            this.Url = TileTDTMapsLayer.URL;
            RefreshResolutions();
            TileSize = 256;
            base.Initialize();
        }
        private void RefreshResolutions()
        {
            string lt = this.layerType;
            Int32 resStart = 0;
            Int32 resLength = 0;
            if (lt == "vec")
            {
                resStart = 0;
                resLength = 17;
                this.levelOffset = 1;
            }
            else if (lt == "img")
            {
                resStart = 1;
                resLength = 17;
                this.levelOffset = 2;
            }
            else if (lt == "ter")
            {
                resStart = 0;
                resLength = 13;
                this.levelOffset = 1;
            }
            this.Resolutions = new double[resLength + 1 - resStart];
            if (this.projection == "4326")
            {
                this.Bounds = new Rectangle2D(-179.99999999999997, -90.0, 180.00000000000023, 89.99999999999994);
                this.Origin = new Point2D(-180, 90);
                for (Int32 i = resStart; i <= resLength; i++)
                {
                    this.Resolutions[i - resStart] = (1.40625 / 2 / Math.Pow(2, i));
                }
                this.proj = "c";
            }
            //EPSG:900913
            else
            {
                this.Bounds = new Rectangle2D(-20037508.3392, -20037508.3392, 20037508.3392, 20037508.3392);
                this.Origin = new Point2D(-20037508.3392, 20037508.3392);

                for (Int32 i = resStart; i <= resLength; i++)
                {

                    this.Resolutions[i - resStart] = (156543.0339 / 2 / Math.Pow(2, i));
                }
                this.proj = "w";
            }
        }

        //重写返回Url函数
        public override MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken)
        {
            MapImage mapImage = new MapImage();
            mapImage.MapImageType = MapImageType.Url;
            level += this.levelOffset;
            string serverURL = TileTDTMapsLayer.URL;
            string lt = this.layerType;
            if (this.isLabel)
            {
                if (lt == "vec") lt = "cva";
                if (lt == "img") lt = "cia";
                if (lt == "ter") lt = "cta";
            }
            Random random = new Random();
            serverURL = serverURL.Replace("{subdomain}", Math.Round(random.NextDouble() * 7).ToString());
            serverURL = serverURL.Replace("{tileX}", indexX.ToString());
            serverURL = serverURL.Replace("{tileY}", indexY.ToString());
            serverURL = serverURL.Replace("{level}", level.ToString());
            serverURL = serverURL.Replace("{proj}", this.proj);
            serverURL = serverURL.Replace("{layerType}", lt);
            mapImage.Url = serverURL;
            return mapImage;
        }

    }
}
