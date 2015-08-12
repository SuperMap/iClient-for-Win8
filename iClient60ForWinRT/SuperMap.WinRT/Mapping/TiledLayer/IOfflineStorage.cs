using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// <para>${mapping_IOfflineStorage_Title}</para>
    /// <para>${mapping_IOfflineStorage_Description}</para>
    /// </summary>
    public interface IOfflineStorage
    {
        /// <summary>
        /// ${mapping_IOfflineStorage_mathod_IsImageExistAsync_D}
        /// </summary>
        /// <param name="width">${mapping_IOfflineStorage_mathod_param_IsImageExistAsync_width_D}</param>
        /// <param name="height">${mapping_IOfflineStorage_mathod_param_IsImageExistAsync_height_D}</param>
        /// <param name="resolution">${mapping_IOfflineStorage_mathod_param_IsImageExistAsync_resolution_D}</param>
        /// <param name="level">${mapping_IOfflineStorage_mathod_param_IsImageExistAsync_level_D}</param>
        /// <param name="column">${mapping_IOfflineStorage_mathod_param_IsImageExistAsync_column_D}</param>
        /// <param name="row">${mapping_IOfflineStorage_mathod_param_IsImageExistAsync_row_D}</param>
        /// <returns>${mapping_IOfflineStorage_mathod_returns_IsImageExistAsync_returns_D}</returns>
        Task<bool> IsImageExistAsync(int width, int height, double resolution, int level, int column, int row);

        /// <summary>
        /// ${mapping_IOfflineStorage_mathod_ImageReadAsync_D}
        /// </summary>
        /// <param name="width">${mapping_IOfflineStorage_mathod_param_ImageReadAsync_width_D}</param>
        /// <param name="height">${mapping_IOfflineStorage_mathod_param_ImageReadAsync_height_D}</param>
        /// <param name="resolution">${mapping_IOfflineStorage_mathod_param_ImageReadAsync_resolution_D}</param>
        /// <param name="level">${mapping_IOfflineStorage_mathod_param_ImageReadAsync_level_D}</param>
        /// <param name="column">${mapping_IOfflineStorage_mathod_param_ImageReadAsync_column_D}</param>
        /// <param name="row">${mapping_IOfflineStorage_mathod_param_ImageReadAsync_row_D}</param>
        /// <returns>${mapping_IOfflineStorage_mathod_returns_ImageReadAsync_returns_D}</returns>
        Task<byte[]> ImageReadAsync(int width, int height, double resolution, int level, int column, int row);

        /// <summary>
        /// ${mapping_IOfflineStorage_mathod_ImageWriteAsync_D}
        /// </summary>
        /// <param name="width">${mapping_IOfflineStorage_mathod_param_ImageWriteAsync_width_D}</param>
        /// <param name="height">${mapping_IOfflineStorage_mathod_param_ImageWriteAsync_height_D}</param>
        /// <param name="resolution">${mapping_IOfflineStorage_mathod_param_ImageWriteAsync_resolution_D}</param>
        /// <param name="level">${mapping_IOfflineStorage_mathod_param_ImageWriteAsync_level_D}</param>
        /// <param name="column">${mapping_IOfflineStorage_mathod_param_ImageWriteAsync_column_D}</param>
        /// <param name="row">${mapping_IOfflineStorage_mathod_param_ImageWriteAsync_row_D}</param>
        /// <param name="data">${mapping_IOfflineStorage_mathod_param_ImageWriteAsync_data_D}</param>
        Task ImageWriteAsync(int width, int height, double resolution, int level, int column, int row,byte[] data);

        /// <summary>
        /// ${mapping_IOfflineStorage_mathod_Clear_D}
        /// </summary>
        Task Clear();
    }
}
