using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.Mapping
{

    /// <summary>
    /// 最终请求图片的地方，通过SemaphoreSlim限制同时请求的数量，当打到上限后，阻塞线程。
    /// 设置缓冲区，可以缓村需要请求图片的Tile。
    /// </summary>
    internal class ExecutantPool
    {
        SemaphoreSlim _ss;
        public event EventHandler<Tile> DownloadImageCompleted;
        private QueueBuffer _requestBuffer;
        private object _thisLock = new object();
        private Dictionary<string, Tile> _downloading;

        public ExecutantPool(int count)
        {
            _ss = new SemaphoreSlim(count);
            _requestBuffer = new QueueBuffer();
            _downloading = new Dictionary<string, Tile>();
        }

        public void Register(string id)
        {
            _requestBuffer.Register(id);
        }

        public void Unregiest(string id)
        {
            _requestBuffer.Unregiest(id);
        }

        public bool IsDownloading(string key)
        {
            return _downloading.ContainsKey(key);
        }

        /// <summary>
        /// 向线程池中加入待请求的tile。此tile会先进入队列中进行等待，直到有空闲的线程，才会开始请求。
        /// </summary>
        /// <param name="tile"></param>
        public void AddTiles(IList<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                if (_requestBuffer.Contains(tile.LayerID, tile) || _downloading.ContainsKey(tile.TileKey))
                {
                    continue;
                }
                _requestBuffer.AddLast(tile.LayerID, tile);
            }
            if (_ss.CurrentCount > 0)
            {
                Start();
            }
        }

        /// <summary>
        /// 从队列中移除掉指定图层的Tile。
        /// 逻辑还不太清晰，与CancelByLayerId的关系。
        /// </summary>
        /// <param name="id"></param>
        public void Clear(string id)
        {
            _requestBuffer.Clear(id);
        }

        /// <summary>
        /// 从队列中获取Tile，并请求图片。
        /// </summary>
        private void Start()
        {
            Task.Run(() =>
            {
                while (_requestBuffer.Count > 0)
                {
                    //占据一个位置，位置满了后会在此处阻塞掉。
                    _ss.Wait(-1);
                    //必须再次检查一遍，可能出现在获取到位置后，却没有需要请求的Tile的情况，例如两个线程同时争抢，_requestBuffer中却只有一个Tile。
                    if (_requestBuffer.Count > 0)
                    {
                        Tile tile = _requestBuffer.Pop();
                        //必须做为null的判断，理由同上。
                        if (tile != null)
                        {
                            DownloadImage(tile);
                        }
                        else
                        {
                            //为null表明其实没有tile需要请求，那就让出位置来吧
                            _ss.Release();
                        }
                    }
                    else
                    {
                        //Count=0表明没有tile需要请求，那就让出位置来吧
                        _ss.Release();
                    }
                }
            });
        }

        /// <summary>
        /// 开始请求图片
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        private Task DownloadImage(Tile tile)
        {
            return Task.Run(async () =>
            {
                Tile result = tile;
                bool needStart = false;
                try
                {
                    lock (_thisLock)
                    {
                        //确定Tile没有被请求，尽量减少重复请求。
                        if (!_downloading.ContainsKey(tile.TileKey))
                        {
                            _downloading.Add(tile.TileKey, tile);
                            needStart = true;
                        }
                    }
                    if (needStart)
                    {
                        Func<Tile, Task<Tile>> howtodownload = QueueSystem.Instance.GetHowToDownloadById(tile.LayerID);
                        result = await howtodownload(tile);
                    }
                }
                finally
                {
                    //在执行完后，不管结果如何，都要从_downloading中移除掉，并释放一个位置。
                    if (needStart)
                    {
                        lock (_thisLock)
                        {
                            _downloading.Remove(tile.TileKey);
                        }
                    }
                    _ss.Release();
                }
                if (needStart)
                {
                    OnDownloadImageCompleted(result);
                }
            });

        }

        private void OnDownloadImageCompleted(Tile tile)
        {
            if (DownloadImageCompleted != null)
            {
                DownloadImageCompleted(this, tile);
            }
        }

        /// <summary>
        /// 通过图层id清除正在请求的Tile，此处设计不太合理，下回修改吧……
        /// </summary>
        /// <param name="layerId"></param>
        public void CancelByLayerId(string layerId)
        {
            _requestBuffer.Clear(layerId);
            List<Tile> list = new List<Tile>();
            lock (_thisLock)
            {
                list = _downloading.Values.Where(c => c.LayerID == layerId).ToList();
            }
            foreach (Tile tile in list)
            {
                //不知道为什么，在某些极限情况下，此处的tile会为null……
                //正常情况下不会出现
                if (tile != null && !string.IsNullOrEmpty(tile.LayerID) && tile.LayerID == layerId)
                {
                    if (tile.CancellationTokenSource != null)
                    {
                        tile.CancellationTokenSource.Cancel();
                    }
                }
            }
        }
    }
}
