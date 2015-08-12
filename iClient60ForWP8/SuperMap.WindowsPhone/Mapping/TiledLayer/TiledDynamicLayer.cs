
using System.Threading;
namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// ${WP_mapping_TiledDynamicLayer_Title}<br/>
    /// ${WP_mapping_TiledDynamicLayer_Description}
    /// </summary>
    public abstract class TiledDynamicLayer : TiledLayer
    {
        /// <summary>
        ///     ${WP_pubilc_Constructors_Initializes} <see cref="TiledDynamicLayer">TileDynamicLayer</see> ${WP_pubilc_Constructors_instance}
        /// </summary>
        protected TiledDynamicLayer()
        {
        }

        /// <summary>${WP_mapping_TiledDynamicLayer_method_getTileUrl_D}</summary>
        /// <returns>${WP_mapping_TiledDynamicLayer_method_getTileUrl_param_return}</returns>
        /// <param name="indexX">${WP_mapping_TiledDynamicLayer_method_getTileUrl_param_indexX}</param>
        /// <param name="indexY">${WP_mapping_TiledDynamicLayer_method_getTileUrl_param_indexY}</param>
        /// <param name="resolution">${WP_mapping_TiledDynamicLayer_method_getTileUrl_param_resolution}</param>
        /// <param name="cancellationToken">${WP_mapping_TiledDynamicLayer_method_getTileUrl_param_cancellationToken}</param>
        public abstract MapImage GetTile(int indexX, int indexY, double resolution,CancellationToken cancellationToken);

        /// <summary>${WP_mapping_TiledDynamicLayer_method_getTileUrl_D}</summary>
        protected sealed override MapImage GetTile(int indexX, int indexY, int level, double resolution, CancellationToken cancellationToken)
        {
            return this.GetTile(indexX, indexY, resolution,cancellationToken);
        }

        /// <summary>${WP_mapping_TiledDynamicLayer_method_getCachedResolutions_D}</summary>
        /// <returns>${WP_mapping_TiledDynamicLayer_method_returns_D}</returns>
        public override double[] GetCachedResolutions()
        {
            return null;
        }
    }
}
