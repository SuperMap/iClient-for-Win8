using System;
using System.Collections.Generic;
using System.Globalization;
using SuperMap.WinRT.Core;
using SuperMap.WinRT.Utilities;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;
using System.Net.Http;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${mapping_TiledLayer_Title}</para>
    /// 	<para>${mapping_TiledLayer_Description}</para>
    /// </summary>
    public abstract class TiledLayer : ImageLayer
    {
        private static int uniqueId;//static
        private string uniqueLayerId;
        List<Tile> needShowTiles;
        private double[] realResolutions;
        QueueSystem _queueSystem;
        private Dictionary<string, Tile> _inContainer;
        SynchronizationContext _sync;
        
        /// <summary>${mapping_TiledLayer_constructor_None_D}</summary>
        protected TiledLayer()
        {
            base.progressWeight = 0.0;
            uniqueLayerId = string.Format(CultureInfo.InvariantCulture, "TiledLayer{0}", uniqueId++);
            TileSize = MagicNumber.TILEDLAYER_DEFAULT_SIZE;
            needShowTiles = new List<Tile>();
            _inContainer = new Dictionary<string, Tile>();
            Load();
        }

        internal override void InitContainer()
        {
            Container = new TiledLayerContainer(this);
        }

        void _queueSystem_ImageCompleted(object sender, Tile e)
        {
            if (this.IsInitialized && e != null && this.uniqueLayerId == e.LayerID)
            {
                string key = e.TileKey;
                Action<object> action = async (obj) =>
                {
                    if (!_inContainer.ContainsKey(key) && needShowTiles.Contains(e))
                    {
                        Image img = await DataToImage(e);
                        Container.Children.Add(img);
                        _inContainer.Add(key, e);
                    }
                    int progress = GetProgress();
                    OnProgress(progress);
                    if (progress == 100)
                    {
                        Complated();
                    }
                };
                SendOrPostCallback callback = new SendOrPostCallback(action);
                _sync.Post(callback, null);
            }
        }

        /// <summary>${mapping_TiledLayer_method_getCachedResolutions_D}</summary>
        /// <returns>${mapping_TiledLayer_method_getCachedResolutions_returns}</returns>
        public abstract double[] GetCachedResolutions();
        /// <summary>
        /// ${mapping_TiledLayer_method_GetTile_D}
        /// </summary>
        /// <param name="indexX">${mapping_TiledLayer_method_GetTile_param_indexX_D}</param>
        /// <param name="indexY">${mapping_TiledLayer_method_GetTile_param_indexY_D}</param>
        /// <param name="level">${mapping_TiledLayer_method_GetTile_param_level_D}</param>
        /// <param name="resolution">${mapping_TiledLayer_method_GetTile_param_resolution_D}</param>
        /// <param name="cancellationToken">${mapping_TiledLayer_method_GetTile_param_cancellationToken_D}</param>
        /// <returns>${mapping_TiledLayer_method_GetTile_returns_D}</returns>
        protected virtual MapImage GetTile(int indexX, int indexY, int level, double resolution, CancellationToken cancellationToken)
        {
            return null;
        }

        internal override bool CheckBeforeDraw(UpdateParameter updateParameter)
        {
            if ((base.IsInitialized && base.IsVisible) && (base.Container != null))
            {
                if (Map.MapStatus == MapStatus.Zooming && (GetCachedResolutions() == null) &&
                    updateParameter.Resolutions == null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        internal override void Draw(UpdateParameter updateParameter)
        {
            base.Draw(updateParameter);
            if ((base.IsInitialized && base.IsVisible) && (!Rectangle2D.IsNullOrEmpty(ViewBounds)) && (base.Container != null))
            {
                if (GetCachedResolutions() != null)
                {
                    realResolutions = this.GetCachedResolutions();
                }
                else
                {
                    realResolutions = updateParameter.Resolutions;
                }
                double resolution = MathUtil.GetNearest(updateParameter.Resolution,realResolutions,MinVisibleResolution,MaxVisibleResolution);

                int[] span = this.GetTileSpanWithin(ViewBounds, resolution);

                int level = -1;
                needShowTiles = NeedShowTiles(resolution, span, updateParameter.UseTransitions, out level);
                TileComparer comparer = new TileComparer();
                List<Tile> needDownload = needShowTiles.Except<Tile>(_inContainer.Values, comparer).ToList();
                if (needDownload.Count > 0)
                {
                    _queueSystem.InputTiles(uniqueLayerId, needDownload);
                }
            }
        }

        /// <summary>
        /// 计算需要添加的Tile块。
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="span"></param>
        /// <param name="useTransitions"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private List<Tile> NeedShowTiles(double resolution, int[] span, bool useTransitions, out int level)
        {
            double num = ((span[3] - span[1]) * 0.5) + span[1];
            double num2 = ((span[2] - span[0]) * 0.5) + span[0];
            List<Tile> tiles = new List<Tile>();
            List<DistanceTile> list = new List<DistanceTile>();
            for (int i = span[1]; i <= span[3]; i++)
            {
                for (int j = span[0]; j <= span[2]; j++)
                {
                    list.Add(new DistanceTile { Row = i, Column = j, Distance = Math.Pow(i - num, 2.0) + Math.Pow(j - num2, 2.0) });

                }
            }
            list.Sort();
            level = MathUtil.GetIndex(resolution, realResolutions);

            if (realResolutions != null)
            {
                if (level == -1)
                {
                    return tiles; //存在比例尺数组，但是不存在所请求的分辨率，那么就不要发送请求了。
                }
            }
            else
            {
                level = -1;
            }

            foreach (DistanceTile disTile in list)
            {
                MapImage mapImage = null;

                string str = Tile.GetTileKey(uniqueLayerId, disTile.Row, disTile.Column, resolution, level);
                Tile tile = new Tile(disTile.Row, disTile.Column, resolution, mapImage, useTransitions);
                tile.Level = level;
                tile.TileKey = str;
                tile.LayerID = uniqueLayerId;
                tile.TileSize = TileSize;
                tiles.Add(tile);
            }
            return tiles;
        }

        /// <summary>${mapping_TiledLayer_method_refresh_D}</summary>
        public override void Refresh()
        {
            _queueSystem.Cancel(uniqueLayerId);
            _inContainer.Clear();
            Container.Children.Clear();
            _queueSystem.ClearMemoryCache(uniqueLayerId);
            OnLayerChanged();
            base.Refresh();
        }

        /// <summary>${mapping_TiledLayer_attribute_Cancel_D}</summary>
        protected override void Cancel()
        {
            _queueSystem.Cancel(uniqueLayerId);
            Complated();
            base.Cancel();
        }

        /// <summary>${mapping_TiledLayer_method_ClearLocalStorage_D}</summary>
        public async void ClearLocalStorage()
        {
            if (LocalStorage != null)
            {
                await LocalStorage.Clear();
            }
        }

        private async Task<Tile> GetImage(Tile tile)
        {
            if (!string.IsNullOrEmpty(tile.TileKey))
            {
                try
                {
                    tile.CancellationTokenSource = new CancellationTokenSource(5000);
                    tile.MapImage = await Task.Run<MapImage>(() => GetTile(tile.Column, tile.Row, tile.Level, tile.Resolution, tile.CancellationTokenSource.Token)).AsAsyncOperation<MapImage>();
                    if (tile.CancellationTokenSource.IsCancellationRequested)
                    {
                        tile.CancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }
                    if (tile.MapImage.MapImageType == MapImageType.Url)
                    {
                        HttpClient client = new HttpClient();
                        tile.CancellationTokenSource.Token.Register(() =>
                            {
                                try
                                {
                                    //可能出现的情况是，client已经Dispose了，但是CancellationTokenSource还没来得及Dispose
                                    //这时候Cancel了，此处client由于已经Disopose了，再Cancel会异常。
                                    //需要有方法在client调用Dispose时，先Dispose掉CancellationTokenSource。或者移除掉此函数。
                                    client.CancelPendingRequests();
                                }
                                catch
                                {

                                }
                            });
                        try
                        {
                            tile.MapImage.Data = await client.GetByteArrayAsync(tile.MapImage.Url);
                        }
                        finally
                        {
                            client.Dispose();
                        }
                        if (tile.CancellationTokenSource.IsCancellationRequested)
                        {
                            tile.CancellationTokenSource.Token.ThrowIfCancellationRequested();
                        }
                    }
                    if (tile.MapImage.Data != null && tile.MapImage.Data.Length > 0)
                    {
                        tile.IsSuccessd = true;
                    }
                    return tile;
                }
                catch (Exception ex)
                {
                    //出现异常。

                    return tile;
                }
                finally
                {
                    if (tile.CancellationTokenSource != null)
                    {
                        if (tile.CancellationTokenSource.IsCancellationRequested)
                        {
                            tile.IsCanceled = true;
                        }
                        tile.CancellationTokenSource.Dispose();
                        tile.CancellationTokenSource = null;
                    }
                }
            }
            return null;
        }

        private void RaiseTileLoad(Tile tile, Rectangle2D bounds)
        {
            if (TileLoaded != null)
            {
                TileLoadEventArgs loadTileArgs = new TileLoadEventArgs();
                loadTileArgs.Column = tile.Column;

                loadTileArgs.Level = tile.Level;
                loadTileArgs.Row = tile.Row;
                loadTileArgs.Bounds = bounds;

                TileLoaded(this, loadTileArgs);
            }
        }

        //由bounds获取tile的起始和结束行列号
        //[startColumn,startRow,endColumn,endRow] //Returns [0,0,-1,-1] if fails
        private int[] GetTileSpanWithin(Rectangle2D bounds, double resolution)
        {
            Rectangle2D temp = bounds.Intersect(this.Bounds);
            if (!Rectangle2D.IsNullOrEmpty(temp))
            {
                double x = this.Origin.X;
                double y = this.Origin.Y;

                int startColumn = (int)Math.Floor((double)(((temp.Left - x) + (resolution * 0.5)) / (resolution * this.TileSize)));
                int startRow = (int)Math.Floor((double)(((y - temp.Top) + (resolution * 0.5)) / (resolution * this.TileSize)));
                int endColumn = (int)Math.Floor((double)(((temp.Right - x) - (resolution * 0.5)) / (resolution * this.TileSize)));
                int endRow = (int)Math.Floor((double)(((y - temp.Bottom) - (resolution * 0.5)) / (resolution * this.TileSize)));
                return new int[] { startColumn, startRow, endColumn, endRow };

            }
            return new int[4] { 0, 0, -1, -1 };
        }

        private Rectangle2D GetTilesBounds(int cloumn, int row, double resolution)
        {
            double left = this.Origin.X + ((TileSize * cloumn) * resolution);
            double top = this.Origin.Y - ((TileSize * row) * resolution);
            double right = left + (this.TileSize * resolution);
            double bottom = top - (this.TileSize * resolution);
            return new Rectangle2D(left, bottom, right, top);
        }

        /// <summary>
        /// 获取当前进度。
        /// </summary>
        /// <returns></returns>
        private int GetProgress()
        {
            TileComparer comparer = new TileComparer();
            int progress = 0;
            if (needShowTiles.Count == 0)
            {
                //可能存在此处needShowTiles.Count=0的情况，比如迅速将地图移出Bounds的范围。
                return 100;
            }
            progress = _inContainer.Values.Intersect(needShowTiles).ToList().Count * 100 / needShowTiles.Count;

            return progress;
        }

        private void Complated()
        {
            TileComparer comparer = new TileComparer();
            List<string> needRemoveKeys = _inContainer.Values.Except(needShowTiles, comparer).Distinct(comparer).Select(c => c.TileKey).ToList();
            foreach (string key in needRemoveKeys)
            {
                bool suc = _inContainer.Remove(key);
                Container.Children.Remove(Container.FindName(key) as UIElement);
            }
        }

        private async Task<Image> DataToImage(Tile tile)
        {
            BitmapImage bitmapImage = new BitmapImage();
            if (tile.IsSuccessd)
            {
                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                DataWriter writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0));
                writer.WriteBytes(tile.MapImage.Data);
                await writer.StoreAsync();

                bitmapImage.SetSource(randomAccessStream);
                writer.Dispose();
                randomAccessStream.Dispose();
            }
            Image image = new Image()
            {
                Opacity = 1.0,
                Tag = tile,
                IsHitTestVisible = false,
                Name = tile.TileKey,
                Stretch = Stretch.Fill,
                Source = bitmapImage
            };

            double resolution = tile.Resolution;
            LayerContainer.SetBounds(image, GetTilesBounds(tile.Column, tile.Row, tile.Resolution));//计算该image的范围
            return image;
        }

        private void ShowImage(Image img, bool enableFading)
        {
            if (enableFading)
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = new double?(img.Opacity),
                    To = 1.0,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                animation.SetValue(Storyboard.TargetPropertyProperty, "Opacity");

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, img);
                storyboard.Completed += (s, e) =>
                {
                    DispatchedHandler handler = new DispatchedHandler(delegate { base.OnProgress(this.GetProgress()); });
                    base.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
                };
                storyboard.Begin();
            }
            else
            {
                img.Opacity = 1.0;
            }

            base.OnProgress(this.GetProgress()); //到处都有你的身影
        }

        internal sealed override bool ContinuousDraw
        {
            get
            {
                return true;
            }
        }

        /// <summary>${mapping_TiledLayer_attribute_tileSize_D}</summary>
        public int TileSize { get; set; }
        /// <summary>${mapping_TiledLayer_event_TileLoaded_D}</summary>
        public event EventHandler<TileLoadEventArgs> TileLoaded;
        /// <summary>${mapping_TiledLayer_event_TileLoading_D}</summary>
        public event EventHandler<TileLoadEventArgs> TileLoading;

        private Point2D mapOrigin = Point2D.Empty;
        /// <summary>${mapping_TiledLayer_attribute_origin_D}</summary>
        public Point2D Origin
        {
            set { this.mapOrigin = value; }
            get
            {
                return (Point2D.IsNullOrEmpty(mapOrigin)) ? new Point2D(this.Bounds.Left, this.Bounds.Top) : this.mapOrigin;
            }
        }

        /// <summary>
        /// ${mapping_TiledLayer_attribute_LocalStorage_D}
        /// </summary>
        public IOfflineStorage LocalStorage
        {
            get
            {
                return _queueSystem.GetStorage(uniqueLayerId);
            }
            set
            {
                _queueSystem.ResetStorage(uniqueLayerId, value);
            }
        }

        internal override void Load()
        {
            base.Load();
            _queueSystem = QueueSystem.Instance;
            _sync = SynchronizationContext.Current;
            _queueSystem.Register(uniqueLayerId, this.LocalStorage, GetImage);
            _queueSystem.ImageCompleted += _queueSystem_ImageCompleted;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.Cancel();
            _queueSystem.Unregiest(uniqueLayerId);
            _queueSystem.ImageCompleted -= _queueSystem_ImageCompleted;
        }

    }
    /// <summary>${mapping_TileLoadEventArgs_Title}</summary>
    public sealed class TileLoadEventArgs : EventArgs
    {
        internal TileLoadEventArgs()
        {
        }
        /// <summary>${mapping_TiledLayer_attribute_Column_D}</summary>
        public int Column
        {
            get;
            internal set;
        }
        /// <summary>${mapping_TiledLayer_attribute_ImageSource_D}</summary>
        public ImageSource ImageSource
        {
            get;
            internal set;
        }
        /// <summary>${mapping_TiledLayer_attribute_Level_D}</summary>
        public int Level
        {
            get;
            internal set;
        }
        /// <summary>${mapping_TiledLayer_attribute_Row_D}</summary>
        public int Row
        {
            get;
            internal set;
        }
        /// <summary>${mapping_TiledLayer_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds { get; internal set; }
    }
}
