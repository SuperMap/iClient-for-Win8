using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>${WP_mapping_LayerCollection_Title}</summary>
    public sealed class LayerCollection : ObservableCollection<Layer>
    {
        private int currentProgress = -1;
        ///// <summary>
        ///// 	<see cref="LayersInitialized">LayersInitialized</see>
        /////     ${WP_pubilc_delegates_description_sl}
        ///// </summary>
        //public delegate void LayersInitializedHandler(object sender, EventArgs e);
        /// <summary>${WP_mapping_LayerCollection_event_LayersInitialized_D}</summary>
        public event EventHandler LayersInitialized;

        internal event EventHandler<ProgressEventArgs> Progress;

        /// <summary>
        /// 	<para align="left">
        ///         ${WP_pubilc_Constructors_Initializes} <see cref="LayerCollection">LayerCollection</see> ${WP_pubilc_Constructors_instance}
        ///     </para>
        /// </summary>
        public LayerCollection()
        {
            base.CollectionChanged += new NotifyCollectionChangedEventHandler(this.LayerCollection_CollectionChanged);
        }

        internal double GetDpi()
        {
            foreach (Layer layer in this)
            {
                if (layer.IsScaleCentric)
                {
                    return layer.GetDpi();
                }
            }
            return 0;
        }
        /// <summary>${WP_mapping_LayerCollection_method_GetCRS_D}</summary>
        public CoordinateReferenceSystem GetCRS()
        {
            if (this != null)
            {
                foreach (Layer layer in this)
                {
                    if (!layer.IsInitialized)
                    {
                        return null;
                    }//TODO:比如SuperMap还没获取值...还有点问题
                    if (layer.CRS != null)
                    {
                        return layer.CRS.Clone();
                    }//小问题一个，不显示设置layer的CRS，那么还有网络取的时差
                }
            }
            return null;
        }

        /// <summary>${WP_mapping_LayerCollection_method_GetBounds_D}</summary>
        public Rectangle2D GetBounds(CoordinateReferenceSystem crs)
        {
            if (base.Count == 0)
            {
                return Rectangle2D.Empty;
            }
            Rectangle2D bounds = Rectangle2D.Empty;
            foreach (Layer item in this)
            {
                if (item.IsScaleCentric && Rectangle2D.IsNullOrEmpty(item.Bounds))//表明取json值未返回
                {
                    return Rectangle2D.Empty;//返回空,继续等待
                }
                if (!Rectangle2D.IsNullOrEmpty(item.Bounds) && CoordinateReferenceSystem.Equals(crs, item.CRS, true))
                {
                    bounds=bounds.Union(item.Bounds);
                }
            }
            return bounds;
        }

        /// <summary>${WP_mapping_LayerCollection_method_GetResolutions_D}</summary>
        public double[] GetResolutions(CoordinateReferenceSystem crs)
        {
            double[] levels = null;
            if ((base.Count > 0) && !this.HasPendingLayers)
            {
                if (this.HasTiledResources)
                {
                    LayerCollection layers = this;
                    bool flag = false;
                    try
                    {
                        flag = Monitor.TryEnter(layers);
                        //各个TiledCached图层 尺度 排序综合
                        #region Lambda表达式方式 LINQ
                        List<double> list = new List<double>();
                        foreach (TiledCachedLayer layer in this.GetTileLayerEnumerator())
                        {
                            if (layer.IsInitialized && (layer.Error == null) && CoordinateReferenceSystem.Equals(crs, layer.CRS, true) && layer.Resolutions != null)
                            {
                                IEnumerable<double> collection =
                                    from resolution in layer.Resolutions
                                    where (resolution > 0)
                                    select resolution;
                                list.AddRange(collection);
                            }
                        }
                        list.Sort();
                        if (list.Count > 0)
                        {
                            levels = list.Distinct<double>().Reverse<double>().ToArray<double>();
                        }
                        else
                        {
                            levels = null;
                        }
                        #endregion

                        #region 返回第一个的尺度分级，排序吧
                        //foreach (TiledCachedLayer layer in this.GetTileLayerEnumerator())
                        //{
                        //    if (layer.IsInitialized && (layer.Error == null) && CoordinateReferenceSystem.Equals(crs, layer.CRS, true) && layer.Resolutions != null)
                        //    {
                        //        List<double> list = new List<double>();
                        //        foreach (double item in layer.Resolutions)
                        //        {
                        //            if (!list.Contains(item))
                        //            {
                        //                list.Add(item);
                        //            }
                        //        }
                        //        if (list.Count > 0)
                        //        {
                        //            list.Sort();
                        //            list.Reverse();
                        //            levels = list.ToArray();
                        //        }
                        //        break;
                        //    }
                        //}
                        #endregion
                    }
                    finally
                    {
                        if (flag)
                        {
                            Monitor.Exit(layers);
                        }
                    }
                }
            }
            return levels;
        }
        /// <summary>${WP_mapping_LayerCollection_method_ClearItems_D}</summary>
        protected override void ClearItems()
        {
            foreach (Layer layer in this)
            {
                if (layer != null)
                {
                    layer.CancelLoad();
                    if (layer.Container != null)
                    {
                        layer.Container.Children.Clear();
                    }
                    layer.Initialized -= new EventHandler<EventArgs>(this.layer_OnInitialized);
                    layer.Progress -= new EventHandler<ProgressEventArgs>(this.layer_OnProgress);
                }
            }
            base.ClearItems();
            this.CalculateLevelScheme();
        }

        private void LayerCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IList oldItems = e.OldItems;
            if (oldItems != null)
            {
                foreach (object obj2 in oldItems)
                {
                    Layer layer = obj2 as Layer;
                    if (layer != null)
                    {
                        layer.CancelLoad();
                        if (layer.Container != null)
                        {
                            layer.Container.Children.Clear();
                        }
                        layer.Initialized -= new EventHandler<EventArgs>(this.layer_OnInitialized);
                        layer.Progress -= new EventHandler<ProgressEventArgs>(this.layer_OnProgress);
                    }
                }
                this.CalculateLevelScheme();
            }
            if (e.NewItems != null)
            {
                bool flag = true;
                foreach (Layer layer2 in e.NewItems)
                {
                    if (layer2 != null)
                    {
                        layer2.Initialized += new EventHandler<EventArgs>(this.layer_OnInitialized);
                        layer2.Progress += new EventHandler<ProgressEventArgs>(this.layer_OnProgress);
                        if (!layer2.IsInitialized)
                        {
                            flag = false;
                        }
                    }
                }
                if (flag)
                {
                    this.CalculateLevelScheme();
                }
            }


        }
        private void layer_OnInitialized(object sender, EventArgs args)
        {
            this.CalculateLevelScheme();
        }
        private void layer_OnProgress(object sender, ProgressEventArgs args)
        {
            double a = 0.0;
            double num2 = 0.0;
            foreach (Layer layer in this)
            {
                if (layer != null && layer.IsVisible)
                {
                    num2 += layer.progressWeight;
                }
            }
            if (num2 == 0.0)
            {
                a = 100.0;
            }
            else
            {
                foreach (Layer layer2 in this)
                {
                    if (layer2 != null && layer2.IsVisible && (layer2.progressWeight > 0.0))
                    {
                        a += (((double)layer2.progress) / num2) * layer2.progressWeight;
                    }
                }
            }
            int progress = (int)Math.Round(a);
            if (this.currentProgress != progress)
            {
                this.currentProgress = progress;
                if (this.Progress != null)
                {
                    this.Progress(this, new ProgressEventArgs(progress));
                }
            }
        }

        private void CalculateLevelScheme()
        {
            //ResetLevels();

            EventHandler temp = LayersInitialized;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }

        private bool ResolutionsAreSame(double r1, double r2)
        {
            return ((r1 == r2) || (Math.Abs((double)(((256.0 / r1) * r2) - 256.0)) < 0.5));
        }
        //获取所有的TiledCachedLayer
        private IEnumerable<TiledCachedLayer> GetTileLayerEnumerator()
        {
            foreach (Layer item in this)
            {
                if (item is TiledCachedLayer)
                {
                    yield return (TiledCachedLayer)item;
                }
            }
        }

        //属性
        //判断是否有 没有初始化完毕 的Layer
        internal bool HasPendingLayers
        {
            get
            {
                foreach (Layer layer in this)
                {
                    if (layer != null && !layer.IsInitialized)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        //判断是否有TileLayer
        internal bool HasTiledResources
        {
            get
            {
                using (IEnumerator<TiledCachedLayer> enumerator = GetTileLayerEnumerator().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        TiledCachedLayer current = enumerator.Current;//这一句干什么呢？
                        return true;
                    }
                }
                return false;
            }
        }
        //除数字外，id也可作为索引器
        /// <summary>
        /// ${WP_mapping_LayerCollection_method_This_D}
        /// </summary>
        /// <param name="id">${WP_mapping_LayerCollection_method_This_param_id}</param>
        /// <returns></returns>
        public Layer this[string id]
        {
            get
            {
                foreach (Layer layer in this)
                {
                    if ((layer != null) && (((string)layer.GetValue(Layer.IDProperty)) == id))
                    {
                        return layer;
                    }
                }
                return null;
            }
        }
    }
}
