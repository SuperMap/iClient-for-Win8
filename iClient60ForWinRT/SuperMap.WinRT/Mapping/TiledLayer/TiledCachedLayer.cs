
using System.Threading;
namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// ${mapping_TiledCachedLayer_Title}<br/>
    /// ${mapping_TiledCachedLayer_Description}
    /// </summary>
    public abstract class TiledCachedLayer : TiledLayer
    {
        /// <summary>
        ///     ${pubilc_Constructors_Initializes} <see cref="TiledCachedLayer">TiledCachedLayer</see> ${pubilc_Constructors_instance}
        /// </summary>
        protected TiledCachedLayer()
        {
        }

        /// <summary>${mapping_TiledCachedLayer_method_GetTile_D}</summary>
        /// <returns>${mapping_TiledCachedLayer_method_GetTile_returns}</returns>
        /// <param name="indexX">${mapping_TiledCachedLayer_method_GetTile_param_indexX}</param>
        /// <param name="indexY">${mapping_TiledCachedLayer_method_GetTile_param_indexY}</param>
        /// <param name="level">${mapping_TiledCachedLayer_method_GetTile_param_level}</param>
        /// <param name="cancellationToken">${mapping_TiledCachedLayer_method_GetTile_param_cancellationToken_D}</param>
        public abstract MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken);
        
        protected sealed override MapImage GetTile(int indexX, int indexY, int level, double resolution, CancellationToken cancellationToken)
        {
            return this.GetTile(indexX, indexY, level,cancellationToken);
        }

        /// <summary>${mapping_TiledCachedLayer_method_getCachedResolutions_D}</summary>
        /// <returns>${mapping_TiledCachedLayer_method_getCachedResolutions_returns}</returns>
        public override double[] GetCachedResolutions()
        {
            return this.Resolutions;
        }
        /// <summary>${mapping_TiledCachedLayer_attribute_resolutions_D}</summary>
        public virtual double[] Resolutions { get; protected set; }
    }
}


