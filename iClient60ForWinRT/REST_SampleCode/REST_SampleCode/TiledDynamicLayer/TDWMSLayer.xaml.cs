using REST_SampleCode.Controls;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Mapping;
using SuperMap.WinRT.OGC;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace REST_SampleCode
{
    public partial class TDWMSLayer : Page
    {
        private FeaturesLayer featuresLayer;
        string url = "http://support.supermap.com.cn:8090/iserver/services/data-world/wfs100";
        public TDWMSLayer()
        {
            InitializeComponent();
            TiledWMSLayer tiledWMSLayer = this.MyMap.Layers["tiledWMSLayer"] as TiledWMSLayer;
            tiledWMSLayer.Layers = new string[] {"0"};
            featuresLayer = this.MyMap.Layers["featuresLayer"] as FeaturesLayer;
            MyMap.Layers["tiledWMSLayer"].Failed += TDWMSLayer_Failed;
            this.Unloaded += TDWMSLayer_Unloaded;
        }

        void TDWMSLayer_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= TDWMSLayer_Unloaded;
            MyMap.Layers["tiledWMSLayer"].Failed -= TDWMSLayer_Failed;
            MyMap.Dispose();
        }

        private async void TDWMSLayer_Failed(object sender, LayerFailedEventArgs e)
        {
            await MessageBox.Show(e.Error.Message);
        }

        private async void getWFS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                featuresLayer.ClearFeatures();
                GetWFSCapabilities getCapabilities = new GetWFSCapabilities(url);
                List<WFSFeatureType> featureTypes = await getCapabilities.ProcessAsync();

                if (featureTypes.Count > 9)
                {
                    GetWFSDescribeFeatureType featureType = new GetWFSDescribeFeatureType(url);
                    var featureInfo=featureTypes.Where(c=>c.Title=="Capitals").FirstOrDefault();
                    featureType.TypeNames.Add(featureInfo.TypeName);
                    Dictionary<string, List<WFSFeatureDescription>> dicFeatureDesp = await featureType.ProcessAsync();

                    SuperMap.WinRT.Core.PredefinedMarkerStyle markerStyle = new PredefinedMarkerStyle() { Symbol = PredefinedMarkerStyle.MarkerSymbol.Star,Size=15};
                   
                    foreach (var item in dicFeatureDesp)
                    {
                        if (item.Key == "http://www.supermap.com/World")
                        {
                            GetWFSFeature getWFSFeature = new GetWFSFeature(url)
                            {
                                MaxFeatures = 30,
                                FeatureNS = item.Key
                            };
                            WFSFeatureDescription typeCountries = new WFSFeatureDescription
                            {
                                TypeName = item.Value[0].TypeName,
                                SpatialProperty = item.Value[0].SpatialProperty,
                            };
                            typeCountries.Properties.Add(item.Value[0].Properties[0]);
                            typeCountries.Properties.Add(item.Value[0].Properties[1]);
                            getWFSFeature.FeatureDescriptions.Add(typeCountries);
                            GetWFSFeatureResult featureRes = await getWFSFeature.ProcessAsync();

                            foreach (var key in featureRes.FeaturePair)
                            {
                                featuresLayer.AddFeatureSet(key.Value, markerStyle);
                                foreach (Feature feature in key.Value)
                                {
                                    feature.ToolTip = new TextBlock
                                    {
                                        Text = "坐标："+ "\n" + "X: " + feature.Attributes["SMX"] + "\n" + "Y: " + feature.Attributes["SMY"],
                                        Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Black),
                                        FontSize = 16
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
