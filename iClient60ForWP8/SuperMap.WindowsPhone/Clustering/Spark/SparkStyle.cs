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
    /// 	<para>${WP_mapping_SparkStyle_Title}</para>
    /// 	<para>${WP_mapping_SparkStyle_Description}</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SparkStyle : MarkerStyle
    {
        /// <summary>${WP_mapping_SparkStyle_filed_FillColorProperty_D}</summary>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Brush), typeof(SparkStyle), null);
        /// <summary>${WP_mapping_SparkStyle_attribute_FillColor_D}</summary>
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

        /// <summary>${WP_mapping_SparkStyle_filed_ForeColorProperty_D}</summary>
        public static readonly DependencyProperty ForeColorProperty = DependencyProperty.Register("ForeColor", typeof(Brush), typeof(SparkStyle), null);
        /// <summary>${WP_mapping_SparkStyle_attribute_ForeColor_D}</summary>
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

        internal SparkStyle(int count)
        {
            this.FillColor = new SolidColorBrush(Colors.Black);
            this.ForeColor = new SolidColorBrush(Colors.White);

            this.LoadTemplateFromResource(count);
        }

        private static string entireStyle;
        private static string outGridStyle;
        private void LoadTemplateFromResource(int count)
        {
            if (string.IsNullOrEmpty(entireStyle))
            {
                entireStyle = StyleUtility.XamlFileToString("/SuperMap.WindowsPhone;component/Clustering/Spark/SparkStyle.xaml");
                outGridStyle = StyleUtility.XamlFileToString("/SuperMap.WindowsPhone;component/Clustering/OutGridStyle.xaml");
            }

            StringBuilder outGrids;
            StringBuilder clusterIDs;

            if (count <= 6)
            {
                buildeIDsAndOutsOneRing(count, out outGrids, out clusterIDs);
            }//1-6
            else if (count <= 12)
            {
                buildeIDsAndOutsTwoRings(count, out outGrids, out clusterIDs);
            }//7-12
            else if (count <= 18)
            {
                buildeIDsAndOutsThreeRings(count, out outGrids, out clusterIDs);
            }//13-18
            else
            {
                buildeIDsAndOutsFourRings(count, out outGrids, out clusterIDs);
            }//19-24


            //连续替换两次，构造出复杂的xaml
            string xaml = entireStyle.Replace("{OutGrids}", outGrids.ToString()).Replace("{IDs}", clusterIDs.ToString());
            base.ControlTemplate = XamlReader.Load(xaml) as ControlTemplate;
        }

        private static void buildeIDsAndOutsOneRing(int count, out StringBuilder outGrids, out StringBuilder clusterIDs)
        {
            outGrids = new StringBuilder();
            clusterIDs = new StringBuilder();
            double angle = 0.0;
            double deltaAngle = 360.0 / count;
            for (int i = 0; i < count; i++)
            {
                string id = ClusterUtil.BuildClusterIDs(clusterIDs, i);
                ClusterUtil.BuildOutGrids(outGridStyle, "0", angle, id, outGrids);
                angle += deltaAngle;
            }
        }


        private static void buildeIDsAndOutsTwoRings(int count, out StringBuilder outGrids, out StringBuilder clusterIDs)
        {
            outGrids = new StringBuilder();
            clusterIDs = new StringBuilder();

            double angle1 = 0.0;
            double angle2 = 30.0;
            for (int i = 0; i < count; i++)
            {
                string id = ClusterUtil.BuildClusterIDs(clusterIDs, i);

                if (i < 6)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0", angle1, id, outGrids);
                    angle1 += 60.0;
                }
                else if (i < count)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0,0,-20,0", angle2, id, outGrids);
                    angle2 += 60.0;
                }
            }
        }

        private static void buildeIDsAndOutsThreeRings(int count, out StringBuilder outGrids, out StringBuilder clusterIDs)
        {
            outGrids = new StringBuilder();
            clusterIDs = new StringBuilder();

            double angle1 = 0.0;
            double angle2 = 20.0;
            double angle3 = 40.0;
            for (int i = 0; i < count; i++)
            {
                string id = ClusterUtil.BuildClusterIDs(clusterIDs, i);

                if (i < 6)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0", angle1, id, outGrids);
                    angle1 += 60.0;
                }
                else if (i < 12)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0,0,-20,0", angle2, id, outGrids);
                    angle2 += 60.0;
                }
                else if (i < count)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0,0,-40,0", angle3, id, outGrids);
                    angle3 += 60.0;
                }
            }
        }

        private static void buildeIDsAndOutsFourRings(int count, out StringBuilder outGrids, out StringBuilder clusterIDs)
        {
            outGrids = new StringBuilder();
            clusterIDs = new StringBuilder();

            double angle1 = 0.0;
            double angle2 = 15.0;
            double angle3 = 30.0;
            double angle4= 45.0;
            for (int i = 0; i < count; i++)
            {
                string id = ClusterUtil.BuildClusterIDs(clusterIDs, i);


                if (i < 6)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0", angle1, id, outGrids);
                    angle1 += 60.0;
                }
                else if (i < 12)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0,0,-20,0", angle2, id, outGrids);
                    angle2 += 60.0;
                }
                else if (i < 18)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0,0,-40,0", angle3, id, outGrids);
                    angle3 += 60.0;
                }
                else if (i < count)
                {
                    ClusterUtil.BuildOutGrids(outGridStyle, "0,0,-60,0", angle4, id, outGrids);
                    angle4 += 60.0;
                }
            }
        }
    }
}
