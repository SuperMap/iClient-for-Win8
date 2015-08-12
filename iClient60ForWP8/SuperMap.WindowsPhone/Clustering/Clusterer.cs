using System;
using System.Collections.Generic;
using System.Windows;
using SuperMap.WindowsPhone.Core;
using System.ComponentModel;

namespace SuperMap.WindowsPhone.Clustering
{
    /// <summary>
    /// 	<para>${WP_mapping_Clusterer_Title}</para>
    /// 	<para>${WP_mapping_Clusterer_Description}</para>
    /// </summary>
    public abstract class Clusterer : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>${WP_mapping_Clusterer_constructor_D}</summary>
        protected Clusterer()
        {
        }

        /// <summary>${WP_mapping_Clusterer_method_OnClusteringCompleted}</summary>
        /// <param name="clusters">${WP_mapping_Clusterer_method_OnClusteringCompleted_param_clusters}</param>
        protected void OnClusteringCompleted(IEnumerable<Feature> clusters)
        {
            if (this.ClusteringCompleted != null)
            {
                ClusteringCompleted(this, new ClusterEventArgs(clusters));
            }
        }

        /// <summary>${WP_mapping_Clusterer_method_ClusterFeaturesAsync_D}</summary>
        /// <param name="features">${WP_mapping_Clusterer_method_ClusterFeaturesAsync_param_features}</param>
        /// <param name="resolution">${WP_mapping_Clusterer_method_ClusterFeaturesAsync_param_resolution}</param>
        public abstract void ClusterFeaturesAsync(IEnumerable<Feature> features, double resolution);
        /// <summary>${WP_mapping_Clusterer_method_CancelAsync_D}</summary>
        public abstract void CancelAsync();

        #region Cluster (Attached DependencyProperty)

        internal static readonly DependencyProperty ClusterProperty =
            DependencyProperty.RegisterAttached("Cluster", typeof(IList<Feature>), typeof(Clusterer), null);

        internal static void SetCluster(DependencyObject o, IList<Feature> value)
        {
            o.SetValue(ClusterProperty, value);
        }

        internal static IList<Feature> GetCluster(DependencyObject o)
        {
            return (IList<Feature>)o.GetValue(ClusterProperty);
        }

        #endregion

        #region ClusterChildElements (Attached DependencyProperty)

        /// <summary>${WP_mapping_Clusterer_ClusterChildElementsProperty_D}</summary>
        public static readonly DependencyProperty ClusterChildElementsProperty =
            DependencyProperty.RegisterAttached("ClusterChildElements", typeof(string), typeof(Clusterer), null);

        /// <summary>${WP_mapping_Clusterer_method_SetClusterChildElements_D}</summary>
        /// <param name="o">${WP_mapping_Clusterer_method_SetClusterChildElements_param_o}</param>
        /// <param name="value">${WP_mapping_Clusterer_method_SetClusterChildElements_param_value}</param>
        public static void SetClusterChildElements(DependencyObject o, string value)
        {
            o.SetValue(ClusterChildElementsProperty, value);
        }

        /// <summary>${WP_mapping_Clusterer_method_GetClusterChildElements_D}</summary>
        /// <param name="o">
        /// 	<para>${WP_mapping_Clusterer_method_GetClusterChildElements_param_o}</para>
        /// </param>
        public static string GetClusterChildElements(DependencyObject o)
        {
            return (string)o.GetValue(ClusterChildElementsProperty);
        }

        #endregion

        internal class ClusterEventArgs : EventArgs
        {
            public ClusterEventArgs(IEnumerable<Feature> clusters)
            {
                this.Clusters = clusters;
            }

            public IEnumerable<Feature> Clusters { get; private set; }
        }

        internal EventHandler<ClusterEventArgs> ClusteringCompleted;

        #region INotifyPropertyChanged 成员

        /// <summary>${WP_mapping_Layer_event_PropertyChanged_D}</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>${WP_core_Style_method_OnPropertyChanged_D}</summary>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
