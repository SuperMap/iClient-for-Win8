using SuperMap.WindowsPhone.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SuperMap.WindowsPhone.Mapping
{
    internal class QueueSystem
    {
        public static readonly QueueSystem Instance = new QueueSystem();
        public event EventHandler<Tile> ImageCompleted;

        private Dictionary<string, Func<Tile, Task<Tile>>> _howToDownload;
        private QueueBuffer _tilesBuffer;
        private LruCache<string, byte[]> _memoryCache;
        private Dictionary<string, IOfflineStorage> _storageCache;
        private ExecutantPool _executantPool;
        private bool _isStarted;

        public int RetryMaxCount
        {
            get;
            set;
        }

        private QueueSystem()
        {
            _howToDownload = new Dictionary<string, Func<Tile, Task<Tile>>>();
            _tilesBuffer = new QueueBuffer();
            _memoryCache = new LruCache<string, byte[]>(1000);
            _storageCache = new Dictionary<string, IOfflineStorage>();
            _executantPool = new ExecutantPool(6);
            RetryMaxCount = 5;
            _executantPool.DownloadImageCompleted += _executantPool_DownloadImageCompleted;
        }

        async void _executantPool_DownloadImageCompleted(object sender, Tile e)
        {
            if (e.IsSuccessd && !e.IsCanceled)
            {
                _memoryCache.AddObject(e.TileKey, e.MapImage.Data);
                if (_storageCache.ContainsKey(e.LayerID) && _storageCache[e.LayerID] != null)
                {
                    await _storageCache[e.LayerID].ImageWriteAsync(e.TileSize, e.TileSize, e.Resolution, e.Level, e.Column, e.Row, e.MapImage.Data);
                }
            }
            else if (!e.IsSuccessd && !e.IsCanceled)
            {
                e.RetryCount++;
                if (e.RetryCount <= RetryMaxCount)
                {
                    _tilesBuffer.InsertItemsById(e.LayerID, new List<Tile>() { e });
                    GetImageStart();
                    return;
                }
            }
            OnDownloadImageCompleted(e);
        }

        private void OnDownloadImageCompleted(Tile tile)
        {
            if (ImageCompleted != null)
            {
                ImageCompleted(this, tile);
            }
        }

        public Func<Tile, Task<Tile>> GetHowToDownloadById(string id)
        {
            if (_howToDownload.ContainsKey(id))
            {
                return _howToDownload[id];
            }
            return null;
        }

        public void InputTiles(string id, IList<Tile> tiles)
        {
            _tilesBuffer.Clear(id);
            _executantPool.Clear(id);
            _tilesBuffer.InsertItemsById(id, tiles);
            GetImageStart();
        }

        private async void GetImageStart()
        {
            if (_isStarted)
            {
                return;
            }
            List<Tile> tiles = new List<Tile>();
            while (_tilesBuffer.Count > 0)
            {
                _isStarted = true;
                Tile tile = _tilesBuffer.Pop();
                if (tile == null)
                {
                    continue;
                }
                if (_memoryCache.Contains(tile.TileKey))
                {
                    tile.MapImage = new MapImage();
                    tile.MapImage.MapImageType = MapImageType.Data;
                    tile.MapImage.Data = _memoryCache.GetObject(tile.TileKey);
                    tile.IsSuccessd = true;
                    OnDownloadImageCompleted(tile);
                    continue;
                }

                if (_storageCache.ContainsKey(tile.LayerID) && _storageCache[tile.LayerID] != null)
                {
                    IOfflineStorage storage = _storageCache[tile.LayerID];
                    byte[] data = await storage.ImageReadAsync(tile.TileSize, tile.TileSize, tile.Resolution, tile.Level, tile.Column, tile.Row);
                    if (data != null)
                    {
                        tile.MapImage = new MapImage();
                        tile.MapImage.MapImageType = MapImageType.Data;
                        tile.MapImage.Data = data;
                        _memoryCache.AddObject(tile.TileKey, data);
                        tile.IsSuccessd = true;
                        OnDownloadImageCompleted(tile);
                        continue;
                    }
                }
                if (_executantPool.IsDownloading(tile.TileKey))
                {
                    continue;
                }
                tiles.Add(tile);
            }
            _isStarted = false;
            _executantPool.AddTiles(tiles);

        }

        /// <summary>
        /// 注册一个新的图层。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storage"></param>
        public void Register(string id, IOfflineStorage storage, Func<Tile, Task<Tile>> howToDownload)
        {
            _tilesBuffer.Register(id);
            _executantPool.Register(id);
            if (!_storageCache.ContainsKey(id) && storage != null)
            {
                _storageCache.Add(id, storage);
            }
            if (!_howToDownload.ContainsKey(id))
            {
                _howToDownload.Add(id, howToDownload);
            }
        }

        public void ResetStorage(string id, IOfflineStorage storage)
        {
            if (!_storageCache.ContainsKey(id))
            {
                _storageCache.Add(id, storage);
            }
            else
            {
                _storageCache[id] = storage;
            }
        }

        public IOfflineStorage GetStorage(string id)
        {
            if (!_storageCache.ContainsKey(id))
            {
                return null;
            }
            else
            {
                return _storageCache[id];
            }
        }

        /// <summary>
        /// 注销一个图层。
        /// </summary>
        /// <param name="id"></param>
        public void Unregiest(string id)
        {
            _tilesBuffer.Unregiest(id);
            _executantPool.Unregiest(id);
            if (_storageCache.ContainsKey(id))
            {
                _storageCache.Remove(id);
            }
            if (_howToDownload.ContainsKey(id))
            {
                _howToDownload.Remove(id);
            }
        }

        public void Cancel(string id)
        {
            _tilesBuffer.Clear(id);
            _executantPool.CancelByLayerId(id);
        }

        public void ClearMemoryCache(string id)
        {
            _memoryCache.Clear((key, tile) =>
            {
                string layerId = key.Substring(0, key.IndexOf('_'));
                if (layerId == id)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

    }


}
