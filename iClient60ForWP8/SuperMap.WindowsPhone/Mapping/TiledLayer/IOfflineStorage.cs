using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// <para>${WP_mapping_IOfflineStorage_Title}</para>
    /// <para>${WP_mapping_IOfflineStorage_Description}</para>
    /// </summary>
    public interface IOfflineStorage
    {
        /// <summary>
        /// ${WP_mapping_IOfflineStorage_mathod_IsImageExistAsync_D}
        /// </summary>
        /// <param name="width">${WP_mapping_IOfflineStorage_mathod_param_IsImageExistAsync_width_D}</param>
        /// <param name="height">${WP_mapping_IOfflineStorage_mathod_param_IsImageExistAsync_height_D}</param>
        /// <param name="resolution">${WP_mapping_IOfflineStorage_mathod_param_IsImageExistAsync_resolution_D}</param>
        /// <param name="level">${WP_mapping_IOfflineStorage_mathod_param_IsImageExistAsync_level_D}</param>
        /// <param name="column">${WP_mapping_IOfflineStorage_mathod_param_IsImageExistAsync_column_D}</param>
        /// <param name="row">${WP_mapping_IOfflineStorage_mathod_param_IsImageExistAsync_row_D}</param>
        /// <returns>${WP_mapping_IOfflineStorage_mathod_returns_IsImageExistAsync_returns_D}</returns>
        Task<bool> IsImageExistAsync(int width, int height, double resolution, int level, int column, int row);

        /// <summary>
        /// ${WP_mapping_IOfflineStorage_mathod_ImageReadAsync_D}
        /// </summary>
        /// <param name="width">${WP_mapping_IOfflineStorage_mathod_param_ImageReadAsync_width_D}</param>
        /// <param name="height">${WP_mapping_IOfflineStorage_mathod_param_ImageReadAsync_height_D}</param>
        /// <param name="resolution">${WP_mapping_IOfflineStorage_mathod_param_ImageReadAsync_resolution_D}</param>
        /// <param name="level">${WP_mapping_IOfflineStorage_mathod_param_ImageReadAsync_level_D}</param>
        /// <param name="column">${WP_mapping_IOfflineStorage_mathod_param_ImageReadAsync_column_D}</param>
        /// <param name="row">${WP_mapping_IOfflineStorage_mathod_param_ImageReadAsync_row_D}</param>
        /// <returns>${WP_mapping_IOfflineStorage_mathod_returns_ImageReadAsync_returns_D}</returns>
        Task<byte[]> ImageReadAsync(int width, int height, double resolution, int level, int column, int row);

        /// <summary>
        /// ${WP_mapping_IOfflineStorage_mathod_ImageWriteAsync_D}
        /// </summary>
        /// <param name="width">${WP_mapping_IOfflineStorage_mathod_param_ImageWriteAsync_width_D}</param>
        /// <param name="height">${WP_mapping_IOfflineStorage_mathod_param_ImageWriteAsync_height_D}</param>
        /// <param name="resolution">${WP_mapping_IOfflineStorage_mathod_param_ImageWriteAsync_resolution_D}</param>
        /// <param name="level">${WP_mapping_IOfflineStorage_mathod_param_ImageWriteAsync_level_D}</param>
        /// <param name="column">${WP_mapping_IOfflineStorage_mathod_param_ImageWriteAsync_column_D}</param>
        /// <param name="row">${WP_mapping_IOfflineStorage_mathod_param_ImageWriteAsync_row_D}</param>
        /// <param name="data">${WP_mapping_IOfflineStorage_mathod_param_ImageWriteAsync_data_D}</param>
        Task ImageWriteAsync(int width, int height, double resolution, int level, int column, int row,byte[] data);

        /// <summary>
        /// ${WP_mapping_IOfflineStorage_mathod_Clear_D}
        /// </summary>
        Task Clear();
    }
}
