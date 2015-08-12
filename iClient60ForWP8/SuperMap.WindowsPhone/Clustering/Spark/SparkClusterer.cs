using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Clustering
{
    /// <summary>
    /// 	<para>${WP_mapping_SparkClusterer_Title}</para>
    /// 	<para>${WP_mapping_SparkClusterer_Description}</para>
    /// </summary>
    public class SparkClusterer : FeaturesClusterer
    {
        private const int MAXIMUMCOUNT = 24;
        private const int RADIUS = 40;
        
        private Dictionary<int, SparkStyle> cache;

        /// <summary>${WP_mapping_SparkClusterer_constructor_D}</summary>
        public SparkClusterer()
        {
            Radius = RADIUS;
            
            cache = new Dictionary<int, SparkStyle>();
            Gradient = new LinearGradientBrush();
            Gradient.MappingMode = BrushMappingMode.RelativeToBoundingBox;
            Gradient.GradientStops.Add(new GradientStop { Offset = 0.0, Color = Color.FromArgb(0x7f, 0xff, 0xff, 0) });
            Gradient.GradientStops.Add(new GradientStop { Offset = 1.0, Color = Color.FromArgb(0x7f, 0xff, 0, 0) });

            Background = new SolidColorBrush(Colors.Red);
            Foreground = new SolidColorBrush(Colors.White);
        }
        /// <summary>${WP_mapping_Clusterer_method_ClusterFeaturesAsync_D}</summary>
        /// <param name="features">${WP_mapping_Clusterer_method_ClusterFeaturesAsync_param_features}</param>
        /// <param name="resolution">${WP_mapping_Clusterer_method_ClusterFeaturesAsync_param_resolution}</param>
        public override void ClusterFeaturesAsync(IEnumerable<Feature> features, double resolution)
        {
            base.ClusterFeaturesAsync(features, resolution);
        }
        /// <summary>${WP_mapping_FeaturesClusterer_method_OnCreateFeature_D}</summary>
        /// <param name="cluster">${WP_mapping_FeaturesClusterer_method_OnCreateFeature_param_cluster}</param>
        /// <param name="center">${WP_mapping_FeaturesClusterer_method_OnCreateFeature_param_center}</param>
        /// <param name="maxClusterCount">${WP_mapping_SparkClusterer_method_OnCreateFeature_param_maxClusterCount}</param>
        protected override Feature OnCreateFeature(FeatureCollection cluster, GeoPoint center, int maxClusterCount)
        {
            if (cluster.Count == 1)
            {
                return cluster[0];
            }
            Feature feature = null;
            double size = (Math.Log((double)(cluster.Count / 10)) * 10.0) + 20.0;//小于10个,size都是负无穷
            if (size < 12.0)
            {
                size = 12.0;
            }
            if (cluster.Count <= MAXIMUMCOUNT)
            {
                if (!this.cache.ContainsKey(cluster.Count))
                {
                    SparkStyle style = new SparkStyle(cluster.Count)
                    {
                        ForeColor = this.Foreground,
                        FillColor = this.Background
                    };
                    this.cache.Add(cluster.Count, style);
                }//如果没有这种 样式，就创建新的样式。 根据Count决定。
                SparkStyle style3 = this.cache[cluster.Count];
                feature = new Feature
                {
                    Geometry = center,
                    Style = style3
                };
            }
            else
            {
                feature = new ClusterFeature(size)
                {
                    Geometry = center
                };
            }//如果不达到聚合数(太多，比如大于10)，则去生成新的样式
            feature.Attributes.Add("Count", cluster.Count);
            feature.Attributes.Add("Size", size);
            feature.Attributes.Add("Color", ClusterUtil.InterpolateColor((double)cluster.Count, maxClusterCount, this.Gradient));
            return feature;
        }
        /// <summary>${WP_mapping_ScatterClusterer_attribute_Gradient_D}</summary>
        public LinearGradientBrush Gradient { get; set; }
        /// <summary>${WP_mapping_ScatterClusterer_attribute_Background_D}</summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        /// <summary>${WP_mapping_ScatterClusterer_field_BackgroundProperty_D}</summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(SparkClusterer),
            new PropertyMetadata(new PropertyChangedCallback(OnBackgroundChanged)));

        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkClusterer clusterer = d as SparkClusterer;
            if (clusterer.cache != null)
            {
                foreach (SparkStyle style in clusterer.cache.Values)
                {
                    style.FillColor = clusterer.Background;
                }
            }
            clusterer.OnPropertyChanged("Background");
        }
        /// <summary>${WP_mapping_ScatterClusterer_attribute_Foreground_D}</summary>
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        /// <summary>${WP_mapping_ScatterClusterer_field_FlareForegroundProperty_D}</summary>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(SparkClusterer),
            new PropertyMetadata(new PropertyChangedCallback(OnForegroundChanged)));

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SparkClusterer clusterer = d as SparkClusterer;
            if (clusterer.cache != null)
            {
                foreach (SparkStyle style in clusterer.cache.Values)
                {
                    style.FillColor = clusterer.Foreground;
                }
            }
            clusterer.OnPropertyChanged("Foreground");

        }
    }
}
