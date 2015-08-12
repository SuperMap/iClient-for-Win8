
using System.Threading;
namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// ${mapping_TiledDynamicLayer_Title}<br/>
    /// ${mapping_TiledDynamicLayer_Description}
    /// </summary>
    public abstract class TiledDynamicLayer : TiledLayer
    {
        /// <summary>
        ///     ${pubilc_Constructors_Initializes} <see cref="TiledDynamicLayer">TileDynamicLayer</see> ${pubilc_Constructors_instance}
        /// </summary>
        protected TiledDynamicLayer()
        {
        }

        /// <summary>${mapping_TiledDynamicLayer_method_GetTile_D}</summary>
        /// <returns>${mapping_TiledDynamicLayer_method_GetTile_param_return}</returns>
        /// <param name="indexX">${mapping_TiledDynamicLayer_method_GetTile_param_indexX}</param>
        /// <param name="indexY">${mapping_TiledDynamicLayer_method_GetTile_param_indexY}</param>
        /// <param name="resolution">${mapping_TiledDynamicLayer_method_GetTile_param_resolution}</param>
        /// <param name="cancellationToken">${mapping_TiledDynamicLayer_method_GetTile_param_cancellationToken_D}</param>
        public abstract MapImage GetTile(int indexX, int indexY, double resolution, CancellationToken cancellationToken);

        protected sealed override MapImage GetTile(int indexX, int indexY, int level, double resolution, CancellationToken cancellationToken)
        {
            return this.GetTile(indexX, indexY, resolution,cancellationToken);
        }

        /// <summary>${mapping_TiledDynamicLayer_method_getCachedResolutions_D}</summary>
        /// <returns>${mapping_TiledDynamicLayer_method_returns_D}</returns>
        public override double[] GetCachedResolutions()
        {
            return null;
        }
    }
}
