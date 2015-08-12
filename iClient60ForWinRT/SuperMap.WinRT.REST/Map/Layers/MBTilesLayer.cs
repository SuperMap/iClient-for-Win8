using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// 	<para>${Mapping_MBTilesLayer_Tile}</para>
    /// 	<para>${Mapping_MBTilesLayer_Description}</para>
    /// </summary>
    public class MBTilesLayer:TiledCachedLayer
    {
        private string _mbTilesPath;
        bool _isInitializing = false;
        MBTilesParameters _parameter;

        /// <summary>${Mapping_MBTilesLayer_constructor_None_D}</summary>
        public MBTilesLayer()
        {

        }

        /// <summary>${Mapping_MBTilesLayer_method_Initialize_D}</summary>
        public override async void Initialize()
        {
            if (!_isInitializing&&!IsInitialized&&!string.IsNullOrEmpty(MBTilesPath))
            {
                _parameter = await MBTilesHelper.GetMBTilesParameter(MBTilesPath);
                this.Resolutions = _parameter.Resolutions;
                this.Bounds = _parameter.Bounds;
                this.Origin = _parameter.Origin;
                this.TileSize = _parameter.TileHeight;
                if (_parameter.WKID > 0)
                {
                    this.CRS = new Core.CoordinateReferenceSystem(_parameter.WKID);
                }
                base.Initialize();
            }
        }

        /// <summary>${Mapping_MBTilesLayer_method_GetTile_D}</summary>
        /// <returns>${Mapping_MBTilesLayer_method_GetTile_return}</returns>
        /// <param name="indexX">${Mapping_MBTilesLayer_method_GetTile_param_indexX}</param>
        /// <param name="indexY">${Mapping_MBTilesLayer_method_GetTile_param_indexY}</param>
        /// <param name="level">${Mapping_MBTilesLayer_method_GetTile_param_level}</param>
        public override MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken)
        {
            Task<MapImage> task;
            if (_parameter.Compatible && _parameter.WKID == 3857)
            {
                task= MBTilesHelper.GetTile(MBTilesPath, indexX, indexY, level);
            }
            else
            {
                task=MBTilesHelper.GetTile(MBTilesPath, indexX, indexY, Resolutions[level]);
            }
            task.Wait();
            return task.Result;
        }

        /// <summary>${Mapping_MBTilesLayer_attribute_MBTilesPath_D}</summary>
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
    }
}
