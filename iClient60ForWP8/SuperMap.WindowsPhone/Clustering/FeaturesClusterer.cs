using System;
using System.Collections.Generic;
using System.ComponentModel;
using SuperMap.WindowsPhone.Core;
using SuperMap.WindowsPhone.Utilities;
using System.Collections.ObjectModel;
using System.Windows;

namespace SuperMap.WindowsPhone.Clustering
{
    /// <summary>${WP_mapping_FeaturesClusterer_Title}</summary>
    public abstract class FeaturesClusterer : Clusterer
    {
        private BackgroundWorker clusterThread;
        private ObservableCollection<GeoRegion> geosClone;
        FeatureCollection featuresClone;
        /// <summary>
        ///     ${WP_pubilc_Constructors_Initializes} <see cref="FeaturesClusterer">FeaturesClusterer</see> ${pubilc_Constructors_instance}
        /// </summary>
        protected FeaturesClusterer()
        {
            this.Radius = MagicNumber.FEATURESCLUSTERER_DEFAULT_RADIUS;
            newregionCol = new ObservableCollection<GeoRegion>();
        }
        /// <summary>${WP_mapping_FeaturesClusterer_method_OnCreateFeature_D}</summary>
        /// <param name="cluster">${WP_mapping_FeaturesClusterer_method_OnCreateFeature_param_cluster}</param>
        /// <param name="center">${WP_mapping_FeaturesClusterer_method_OnCreateFeature_param_center}</param>
        /// <param name="maxClusterCount">${WP_mapping_FeaturesClusterer_method_OnCreateFeature_param_maxClusterCount}</param>
        protected abstract Feature OnCreateFeature(FeatureCollection cluster, GeoPoint center, int maxClusterCount);

        /// <summary>${WP_mapping_Clusterer_method_ClusterFeaturesAsync_D}</summary>
        /// <param name="features">${WP_mapping_Clusterer_method_ClusterFeaturesAsync_param_features}</param>
        /// <param name="resolution">${WP_mapping_Clusterer_method_ClusterFeaturesAsync_param_resolution}</param>
        public override void ClusterFeaturesAsync(IEnumerable<Feature> features, double resolution)
        {
            if (features == null)
            {
                base.OnClusteringCompleted(new FeatureCollection());
            }
            else
            {
                if (this.clusterThread != null)
                {
                    this.clusterThread.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(clusterThread_RunWorkerCompleted);
                    if (this.clusterThread.IsBusy)
                    {
                        this.clusterThread.CancelAsync();
                    }
                    this.clusterThread = null;
                }
                this.clusterThread = new BackgroundWorker() { WorkerSupportsCancellation = true };
                this.clusterThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(clusterThread_RunWorkerCompleted);
                
                featuresClone = new FeatureCollection();
                foreach (Feature item in features)
                {
                    featuresClone.Add(item);
                }

                geosClone = new ObservableCollection<GeoRegion>();
                if (this.RegionCollection != null)
                {
                    foreach (var geo in this.RegionCollection)
                    {
                        geosClone.Add(geo);
                    }
                }

                this.clusterThread.DoWork += (s, e) =>
                {
                    e.Result = ClusterGeoPoints(featuresClone, this.Radius, resolution, this.clusterThread, geosClone);
                };
                this.clusterThread.RunWorkerAsync(new object[] { featuresClone, resolution, geosClone, this.Dispatcher });
            }
        }

        private void clusterThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((!e.Cancelled) && (e.Result != null))
            {
                Dictionary<int, Cluster> result = e.Result as Dictionary<int, Cluster>;
                if (result != null)
                {
                    FeatureCollection clusters = new FeatureCollection();
                    int num = 0;
                    foreach (int num2 in result.Keys)
                    {
                        if (result.ContainsKey(num2))
                            num = Math.Max(result[num2].Count, num);
                    }
                    foreach (int num3 in result.Keys)
                    {
                        if (result.ContainsKey(num3) && result[num3].Features.Count == 1)
                        {
                            clusters.Add(result[num3].Features[0]);
                        }
                        else if (result.ContainsKey(num3))
                        {
                            Feature item = this.OnCreateFeature(result[num3].Features, new GeoPoint(result[num3].X, result[num3].Y), num);
                            item.DisableToolTip = true;
                            item.SetValue(Clusterer.ClusterProperty, result[num3].Features);
                            clusters.Add(item);
                        }
                    }
                    base.OnClusteringCompleted(clusters);
                }
            }
        }

        /// <summary>${WP_mapping_Clusterer_method_CancelAsync_D}</summary>
        public override void CancelAsync()
        {
            if ((this.clusterThread != null) && this.clusterThread.IsBusy)
            {
                this.clusterThread.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.clusterThread_RunWorkerCompleted);
                this.clusterThread.CancelAsync();
            }
            this.clusterThread = null;
        }

        private static Dictionary<int, Cluster> ClusterGeoPoints(IEnumerable<Feature> features, int radius, double resolution, BackgroundWorker worker, ObservableCollection<GeoRegion> geos)
        {
            Dictionary<int, Cluster> dictionary;

            if (geos.Count > 0)
            {
                dictionary = AssignGeoPointsToClusters(features, geos);
            }
            else
            {
                bool flag = false;
                GeoPoint bottomLeft = GetBottomLeft(features);
                double diameter = (radius * 2) * resolution;
                dictionary = AssignGeoPointsToClusters(features, bottomLeft, diameter);

                do
                {
                    if (worker.CancellationPending)
                    {
                        return null;
                    }
                    dictionary = MergeOverlappingClusters(diameter, dictionary, bottomLeft, out flag, worker);
                } while (flag);
            }

            if (worker.CancellationPending)
            {
                return null;
            }
            return dictionary;
        }

        private static GeoPoint GetBottomLeft(IEnumerable<Feature> features)
        {
            GeoPoint point = new GeoPoint(double.MaxValue, double.MaxValue);
            foreach (Feature f in features)
            {
                if ((f != null) && (f.Geometry != null))
                {
                    GeoPoint geometry = f.Geometry as GeoPoint;
                    if (geometry == null)
                    {
                        geometry = new GeoPoint(f.Geometry.Bounds.Center.X, f.Geometry.Bounds.Center.Y); ;
                    }
                    if (!double.IsNaN(geometry.X) && !double.IsNaN(geometry.Y))
                    {
                        point.X = Math.Min(point.X, geometry.X);
                        point.Y = Math.Min(point.Y, geometry.Y);
                    }
                }
            }
            return point;
        }

        private static Dictionary<int, Cluster> AssignGeoPointsToClusters(IEnumerable<Feature> features, ObservableCollection<GeoRegion> geos)
        {
            Dictionary<int, Cluster> dictionary = new Dictionary<int, Cluster>();
            GeoPoint gp = null;
            foreach (Feature feature in features)
            {
                if (feature.Geometry == null)
                {
                    continue;
                }
                gp = feature.Geometry as GeoPoint;

                if (gp == null)
                {
                    Rectangle2D bounds = feature.Geometry.Bounds;
                    if (bounds.IsEmpty)
                    {
                        continue;
                    }
                    gp = new GeoPoint(bounds.Center.X, bounds.Center.Y);
                }

                bool isContained = false;
                double x = gp.X;
                double y = gp.Y;

                foreach (GeoRegion clusterRegion in geos)
                {
                    if (clusterRegion.Bounds.Contains(x, y) && clusterRegion.Contains(x, y))
                    {
                        int key = -geos.IndexOf(clusterRegion);
                        if (dictionary.ContainsKey(key))
                        {
                            Cluster cluster = dictionary[key];
                            cluster.X += x;
                            cluster.Y += y;
                            cluster.Features.Add(feature);
                        }//有就合并
                        else
                        {
                            dictionary[key] = new Cluster(x, y);
                            dictionary[key].Features.Add(feature);
                        }//没有就new一个
                        isContained = true;
                        break;
                    }
                }
                int key1 = feature.GetHashCode();
                if (!isContained)
                {
                    if (!dictionary.ContainsKey(key1))
                    {
                        dictionary[key1] = new Cluster(gp.X, gp.Y);
                        dictionary[key1].Features.Add(feature);
                    }
                }
            }

            Cluster cltValue;
            int featuresCount = 0;
            foreach (var clt in dictionary)
            {
                cltValue = clt.Value;
                featuresCount = cltValue.Features.Count;
                clt.Value.X = cltValue.X / featuresCount;
                clt.Value.Y = cltValue.Y / featuresCount;
            }

            return dictionary;
        }

        //遍历每一个点，把其归类，得到最终的聚类点Cluster,其所在格网index的X/Y组成Key
        private static Dictionary<int, Cluster> AssignGeoPointsToClusters(IEnumerable<Feature> features, GeoPoint bottomLeft, double diameter)
        {
            Dictionary<int, Cluster> dictionary = new Dictionary<int, Cluster>();
            GeoPoint gp = null;
            foreach (Feature feature in features)
            {
                if (feature.Geometry == null)
                {
                    continue;
                }
                if (feature.Geometry is GeoPoint)
                {
                    gp = feature.Geometry as GeoPoint;
                }
                else
                {
                    Rectangle2D bounds = feature.Geometry.Bounds;
                    if (bounds.IsEmpty)
                    {
                        continue;
                    }
                    gp = new GeoPoint(bounds.Center.X, bounds.Center.Y);
                }

                double x = gp.X;
                double y = gp.Y;
                int cx = (int)Math.Round((x - bottomLeft.X) / diameter);
                int cy = (int)Math.Round((y - bottomLeft.Y) / diameter);
                int key = (cx << 16) | cy;//int是32位的  左16位是x，右16位是y
                if (dictionary.ContainsKey(key))
                {
                    Cluster cluster = dictionary[key];
                    cluster.X = (cluster.X + x) / 2.0;
                    cluster.Y = (cluster.Y + y) / 2.0;
                    cluster.Features.Add(feature);
                }//有就合并
                else
                {
                    dictionary[key] = new Cluster(x, y, cx, cy);
                    dictionary[key].Features.Add(feature);
                }//没有就new一个
            }
            return dictionary;
        }

        private static Dictionary<int, Cluster> MergeOverlappingClusters(double diameter, Dictionary<int, Cluster> orig, GeoPoint lowerLeft, out bool overlapExists, BackgroundWorker worker)
        {
            overlapExists = false;
            Dictionary<int, Cluster> dictionary = new Dictionary<int, Cluster>();
            foreach (int key in orig.Keys)
            {
                Cluster cluster = orig[key];
                if (cluster.Count != 0)
                {
                    overlapExists = SearchAndMerge(cluster, -1, -1, diameter, orig, overlapExists);
                    overlapExists = SearchAndMerge(cluster, -1, 0, diameter, orig, overlapExists);
                    overlapExists = SearchAndMerge(cluster, -1, 1, diameter, orig, overlapExists);

                    overlapExists = SearchAndMerge(cluster, 0, -1, diameter, orig, overlapExists);
                    overlapExists = SearchAndMerge(cluster, 0, 1, diameter, orig, overlapExists);

                    overlapExists = SearchAndMerge(cluster, 1, -1, diameter, orig, overlapExists);
                    overlapExists = SearchAndMerge(cluster, 1, 0, diameter, orig, overlapExists);
                    overlapExists = SearchAndMerge(cluster, 1, 1, diameter, orig, overlapExists);

                    int x = (int)Math.Round((double)((cluster.X - lowerLeft.X) / diameter));
                    int y = (int)Math.Round((double)((cluster.Y - lowerLeft.Y) / diameter));
                    cluster.Cx = x;
                    cluster.Cy = y;
                    int num4 = (x << 16) | y;
                    dictionary[num4] = cluster;
                    if (worker.CancellationPending)
                    {
                        return null;
                    }
                }
            }
            return dictionary;
        }

        private static bool SearchAndMerge(Cluster cluster, int ox, int oy, double diameter, Dictionary<int, Cluster> orig, bool overlapExists)
        {
            int x = cluster.Cx + ox;
            int y = cluster.Cy + oy;
            int key = (x << 16) | y;
            if (orig.ContainsKey(key) && (orig[key].Count > 0))
            {
                Cluster neighbour = orig[key];
                double distanceX = neighbour.X - cluster.X;
                double distanceY = neighbour.Y - cluster.Y;
                if ((Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY)) < diameter) && (neighbour != cluster))
                {
                    overlapExists = true;
                    Merge(cluster, neighbour);
                }
            }
            return overlapExists;
        }

        private static void Merge(Cluster source, Cluster neighbour)
        {
            double num = source.Count + neighbour.Count;
            source.X = ((source.Count * source.X) + (neighbour.Count * neighbour.X)) / num;
            source.Y = ((source.Count * source.Y) + (neighbour.Count * neighbour.Y)) / num;
            foreach (Feature feature in neighbour.Features)
            {
                source.Features.Add(feature);
            }
            neighbour.Features.Clear();
        }

        /// <summary>${mapping_FeaturesClusterer_attribute_radius}</summary>
        public int Radius { get; set; }

        private ObservableCollection<GeoRegion> newregionCol;
        private ObservableCollection<GeoRegion> oldregionCol;
        /// <summary>${mapping_FeaturesClusterer_attribute_RegionCollection}</summary>
        public ObservableCollection<GeoRegion> RegionCollection
        {
            get { return newregionCol; }
            set
            {
                if (oldregionCol != value && oldregionCol != null)
                {
                    oldregionCol.CollectionChanged -= (s, e) => { base.OnPropertyChanged("RegionCollection"); };
                    oldregionCol = newregionCol;
                }
                newregionCol = value;
                if (newregionCol != null)
                {
                    newregionCol.CollectionChanged += (s, e) =>
                    {
                        base.OnPropertyChanged("RegionCollection");
                    };
                }
                base.OnPropertyChanged("RegionCollection");
            }
        }
    }
}
