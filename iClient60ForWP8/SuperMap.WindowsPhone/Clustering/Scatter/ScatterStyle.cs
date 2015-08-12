using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;

namespace SuperMap.WindowsPhone.Clustering
{
    /// <summary>
    /// 	<para>${WP_mapping_ScatterStyle_Title}</para>
    /// 	<para>${WP_mapping_ScatterStyle_Description}</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ScatterStyle : MarkerStyle
    {
        /// <summary>${WP_mapping_ScatterStyle_filed_FillColorProperty_D}</summary>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Brush), typeof(ScatterStyle), null);
        /// <summary>${WP_mapping_ScatterStyle_attribute_FillColor_D}</summary>
        public Brush FillColor
        {
            get
            {
                return (Brush)base.GetValue(FillColorProperty);
            }
            internal set
            {
                base.SetValue(FillColorProperty, value);
            }
        }

        /// <summary>${WP_mapping_ScatterStyle_filed_ForeColorProperty_D}</summary>
        public static readonly DependencyProperty ForeColorProperty = DependencyProperty.Register("ForeColor", typeof(Brush), typeof(ScatterStyle), null);
        /// <summary>${WP_mapping_ScatterStyle_attribute_ForeColor_D}</summary>
        public Brush ForeColor
        {
            get
            {
                return (Brush)base.GetValue(ForeColorProperty);
            }
            internal set
            {
                base.SetValue(ForeColorProperty, value);
            }
        }

        internal ScatterStyle(int count, bool enableRotaion)
        {
            this.FillColor = new SolidColorBrush(Colors.Black);
            this.ForeColor = new SolidColorBrush(Colors.White);

            this.LoadTemplateFromResource(count, enableRotaion);
        }

        private static string entireStyle;
        private static string outGridStyle;
        private void LoadTemplateFromResource(int count, bool enableRotation)
        {
            if (string.IsNullOrEmpty(entireStyle))
            {
                outGridStyle = StyleUtility.XamlFileToString("/SuperMap.WindowsPhone;component/Clustering/OutGridStyle.xaml");
                if (enableRotation)
                {
                    entireStyle = StyleUtility.XamlFileToString("/SuperMap.WindowsPhone;component/Clustering/Scatter/ScatterStyle.xaml");
                }
                else
                {
                    entireStyle = StyleUtility.XamlFileToString("/SuperMap.WindowsPhone;component/Clustering/Scatter/ScatterStyleWithoutRotation.xaml");
                }
            }

            StringBuilder outGrids = new StringBuilder();
            StringBuilder clusterIDs = new StringBuilder();
            double angle = 0.0;
            double deltaAngle = 360.0 / count;
            for (int i = 0; i < count; i++)
            {
                string id = ClusterUtil.BuildClusterIDs(clusterIDs, i);
                ClusterUtil.BuildOutGrids(outGridStyle, "0", angle, id, outGrids);
                angle += deltaAngle;
            }

            //连续替换两次，构造出复杂的xaml
            string xaml = entireStyle.Replace("{OutGrids}", outGrids.ToString()).Replace("{IDs}", clusterIDs.ToString());
            base.ControlTemplate = XamlReader.Load(xaml) as ControlTemplate;
        }
    }
}
