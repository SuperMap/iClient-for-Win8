using SuperMap.WinRT.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>
    /// <para>${mapping_OfflineStorage_Title}</para>
    /// <para>${mapping_OfflineStorage_Description}</para>
    /// </summary>
    public class OfflineStorage : IOfflineStorage
    {
        /// <summary>
        /// ${mapping_OfflineStorage_constructor_string_D}
        /// </summary>
        /// <param name="rootName">${mapping_OfflineStorage_constructor_param_string_D}</param>
        public OfflineStorage(string rootName)
        {
            RootName = rootName;
            Init(rootName);
        }

        private async void Init(string rootName)
        {
            StorageFolder storage = ApplicationData.Current.LocalFolder;
            try
            {
                if (!await storage.FolderExist(rootName))
                {
                    await storage.CreateFolderAsync(rootName);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// ${mapping_OfflineStorage_attribute_RootName_D}
        /// </summary>
        public string RootName
        {
            get;
            private set;
        }

        /// <summary>
        /// ${mapping_OfflineStorage_mathod_IsImageExistAsync_D}
        /// </summary>
        /// <param name="width">${mapping_OfflineStorage_mathod_param_IsImageExistAsync_width_D}</param>
        /// <param name="height">${mapping_OfflineStorage_mathod_param_IsImageExistAsync_height_D}</param>
        /// <param name="resolution">${mapping_OfflineStorage_mathod_param_IsImageExistAsync_resolution_D}</param>
        /// <param name="level">${mapping_OfflineStorage_mathod_param_IsImageExistAsync_level_D}</param>
        /// <param name="column">${mapping_OfflineStorage_mathod_param_IsImageExistAsync_column_D}</param>
        /// <param name="row">${mapping_OfflineStorage_mathod_param_IsImageExistAsync_row_D}</param>
        /// <returns>${mapping_OfflineStorage_mathod_returns_IsImageExistAsync_returns_D}</returns>
        public async Task<bool> IsImageExistAsync(int width, int height, double resolution, int level, int column, int row)
        {
            try
            {
                string path = BuildFilePath(width, height, resolution, level, column, row);
                StorageFolder storage = ApplicationData.Current.LocalFolder;
                return await storage.FileExist(path);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// ${mapping_OfflineStorage_mathod_ImageReadAsync_D}
        /// </summary>
        /// <param name="width">${mapping_OfflineStorage_mathod_param_ImageReadAsync_width_D}</param>
        /// <param name="height">${mapping_OfflineStorage_mathod_param_ImageReadAsync_height_D}</param>
        /// <param name="resolution">${mapping_OfflineStorage_mathod_param_ImageReadAsync_resolution_D}</param>
        /// <param name="level">${mapping_OfflineStorage_mathod_param_ImageReadAsync_level_D}</param>
        /// <param name="column">${mapping_OfflineStorage_mathod_param_ImageReadAsync_column_D}</param>
        /// <param name="row">${mapping_OfflineStorage_mathod_param_ImageReadAsync_row_D}</param>
        /// <returns>${mapping_OfflineStorage_mathod_returns_ImageReadAsync_returns_D}</returns>
        public async Task<byte[]> ImageReadAsync(int width, int height, double resolution, int level, int column, int row)
        {
            string path = BuildFilePath(width, height, resolution, level, column, row);
            StorageFolder storage = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile file = await storage.GetFileAsync(path);
                var stream = await file.OpenReadAsync();
                var streamReader = stream.AsStreamForRead();
                byte[] buffer = new byte[streamReader.Length];
                await streamReader.ReadAsync(buffer, 0, buffer.Length);
                streamReader.Dispose();
                stream.Dispose();
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// ${mapping_OfflineStorage_mathod_ImageWriteAsync_D}
        /// </summary>
        /// <param name="width">${mapping_OfflineStorage_mathod_param_ImageWriteAsync_width_D}</param>
        /// <param name="height">${mapping_OfflineStorage_mathod_param_ImageWriteAsync_height_D}</param>
        /// <param name="resolution">${mapping_OfflineStorage_mathod_param_ImageWriteAsync_resolution_D}</param>
        /// <param name="level">${mapping_OfflineStorage_mathod_param_ImageWriteAsync_level_D}</param>
        /// <param name="column">${mapping_OfflineStorage_mathod_param_ImageWriteAsync_column_D}</param>
        /// <param name="row">${mapping_OfflineStorage_mathod_param_ImageWriteAsync_row_D}</param>
        /// <param name="data">${mapping_OfflineStorage_mathod_param_ImageWriteAsync_data_D}</param>
        public async Task ImageWriteAsync(int width, int height, double resolution, int level, int column, int row, byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return;
            }
            try
            {
                string path = BuildFilePath(width, height, resolution, level, column, row);
                StorageFolder storage = ApplicationData.Current.LocalFolder;
                if (await storage.FileExist(path))
                {
                    StorageFile file = await storage.GetFileAsync(path);
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
                StorageFile newFile = await storage.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
                using (IRandomAccessStream writeStream = await newFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (DataWriter dataWriter = new DataWriter(writeStream))
                    {
                        dataWriter.WriteBytes(data);
                        await dataWriter.StoreAsync();
                        await dataWriter.FlushAsync();
                        dataWriter.Dispose();
                    }
                    writeStream.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        /// <summary>
        /// ${mapping_OfflineStorage_mathod_Clear_D}
        /// </summary>
        public async Task Clear()
        {
            try
            {
                StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(RootName);
                await folder.FolderDelete();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private string BuildFilePath(int width, int height, double resolution, int level, int column, int row)
        {
            string path = string.Empty;
            if (level > -1)
            {
                path = RootName + "\\" + width + "ｘ" + height + "\\" + level + "\\" + column + "\\" + row;
            }
            else
            {
                path = RootName + "\\" + width + "ｘ" + height + "\\" + resolution + "\\" + column + "\\" + row;
            }
            return path;
        }

    }
}
