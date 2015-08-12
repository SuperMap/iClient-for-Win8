using SQLite;
using SuperMap.WindowsPhone.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MBTilesLayerSample
{
    public class MBTilesLayer:TiledCachedLayer
    {
        private string _mbTilesPath;
        bool _isInitializing = false;
        MBTilesParameters _parameter;

        public MBTilesLayer()
        {

        }

        public override async void Initialize()
        {
            if (!_isInitializing&&!IsInitialized)
            {
                if (LocalStorage != null)
                {
                    if (LocalStorage is OfflineMBTiles)
                    {
                        OfflineMBTiles a = LocalStorage as OfflineMBTiles;
                        _parameter = await MBTilesHelper.GetMBTilesParameter(a.MBTilesPath);
                        this.Resolutions = _parameter.Resolutions;
                        this.Bounds = _parameter.Bounds;
                        this.Origin = _parameter.Origin;
                        this.TileSize = _parameter.TileHeight;
                        if (_parameter.WKID > 0)
                        {
                            this.CRS = new SuperMap.WindowsPhone.Core.CoordinateReferenceSystem(_parameter.WKID);
                        }
                        base.Initialize();
                    }
                }
                
            }
        }

        public string MBTilesPath
        {
            get { return _mbTilesPath; }
            set
            {
                _mbTilesPath = value;
                this.IsInitialized = false;
                this._isInitializing = false;
                this.Initialize();
            }
        }

        public override MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
