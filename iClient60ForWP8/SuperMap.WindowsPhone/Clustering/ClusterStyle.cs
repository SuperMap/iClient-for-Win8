using System;
using System.Windows.Controls;
using System.Windows.Markup;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Clustering
{
    internal class ClusterStyle : MarkerStyle
    {
        public ClusterStyle()
        {
            String xaml = StyleUtility.XamlFileToString("/SuperMap.WindowsPhone;component/Clustering/ClusterStyle.xaml");
            base.ControlTemplate = XamlReader.Load(xaml) as ControlTemplate;
            //base.ControlTemplate = ResourceData.Dictionary["ClusterStyle"] as ControlTemplate;
        }

        public override double OffsetX
        {
            get
            {
                return (this.Size / 2.0);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override double OffsetY
        {
            get
            {
                return (this.Size / 2.0);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public double Size { get; set; }
    }
}
