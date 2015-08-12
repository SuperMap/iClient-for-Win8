using System.Collections.Generic;
using System.Windows;
using SuperMap.WinRT.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using SuperMap.WinRT.iServerJava6R;

namespace iServerJava6R_SampleCode
{
    public partial class DynamicWMS : Page
    {
        public DynamicWMS( )
        {
            InitializeComponent();
        }

        private void MyCheckBox1_Click(object sender , RoutedEventArgs e)
        {
            List<string> layers = new List<string> {  };
            foreach (CheckBox item in this.MyStackPanel.Children)
            {
                if ((bool)item.IsChecked)
                {
                    layers.Add(item.Tag.ToString());
                }
            }
            DynamicWMSLayer wmsLayer = this.MyMap.Layers["MyWMS"] as DynamicWMSLayer;
            wmsLayer.Layers = layers.ToArray();
        }
    }
}
