
using System.Threading;
namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// ${WP_mapping_TiledCachedLayer_Title}<br/>
    /// ${WP_mapping_TiledCachedLayer_Description}
    /// </summary>
    public abstract class TiledCachedLayer : TiledLayer
    {
        /// <summary>
        ///     ${WP_pubilc_Constructors_Initializes} <see cref="TiledCachedLayer">TiledCachedLayer</see> ${WP_pubilc_Constructors_instance}
        /// </summary>
        protected TiledCachedLayer()
        {
        }

        /// <summary>${WP_mapping_TiledCachedLayer_method_getTileUrl_D}</summary>
        /// <returns>${WP_mapping_TiledCachedLayer_method_getTileUrl_returns}</returns>
        /// <param name="indexX">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_indexX}</param>
        /// <param name="indexY">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_indexY}</param>
        /// <param name="level">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_level}</param>
		/// <param name="cancellationToken">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_cancellationToken}</param>
        public abstract MapImage GetTile(int indexX, int indexY, int level, CancellationToken cancellationToken);
        /// <summary>${WP_mapping_TiledCachedLayer_method_getTileUrl_D}</summary>
        /// <returns>${WP_mapping_TiledCachedLayer_method_getTileUrl_returns}</returns>
        /// <param name="indexX">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_indexX}</param>
        /// <param name="indexY">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_indexY}</param>
        /// <param name="resolution">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_resolution}</param>
        /// <param name="level">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_level}</param>
		/// <param name="cancellationToken">${WP_mapping_TiledCachedLayer_method_getTileUrl_param_cancellationToken}</param>
        protected sealed override MapImage GetTile(int indexX, int indexY, int level, double resolution, CancellationToken cancellationToken)
        {
            return this.GetTile(indexX, indexY, level,cancellationToken);
        }

        /// <summary>${WP_mapping_TiledCachedLayer_method_getCachedResolutions_D}</summary>
        /// <returns>${WP_mapping_TiledCachedLayer_method_getCachedResolutions_returns}</returns>
        public override double[] GetCachedResolutions()
        {
            return this.Resolutions;
        }
        /// <summary>${WP_mapping_TiledCachedLayer_attribute_resolutions_D}</summary>
        public virtual double[] Resolutions { get; protected set; }
    }
}


