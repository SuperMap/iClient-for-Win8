using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>
    /// ${WP_mapping_OfflineStorage_Title}
    /// </summary>
    public class OfflineStorage : IOfflineStorage
    {
        /// <summary>
        /// ${WP_mapping_OfflineStorage_constructor_D}
        /// </summary>
        /// <param name="rootName">${WP_mapping_OfflineStorage_constructor_param_rootName}</param>
        public OfflineStorage(string rootName)
        {
            RootName = rootName;
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {
                if (!storage.DirectoryExists(rootName))
                {
                    storage.CreateDirectory(rootName);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                storage.Dispose();
            }
        }

        /// <summary>
        /// ${WP_mapping_OfflineStorage_attribute_RootName_D}
        /// </summary>
        public string RootName
        {
            get;
            private set;
        }

        /// <summary>
        /// ${WP_mapping_OfflineStorage_method_BuildPath_D}
        /// </summary>
        private string BuildPath(int width, int height, double resolution, int level, int column, int row)
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

        private void DeleteFolders(string path)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.DirectoryExists(path))
            {
                string[] files = storage.GetFileNames(path + "/");
                if (files.Length > 0)
                {
                    foreach (string s in files)
                    {
                        storage.DeleteFile(path + "/" + s);
                    }
                }
                string[] dics = storage.GetDirectoryNames(path + "/");
                if (dics.Length > 0)
                {
                    foreach (string s in dics)
                    {
                        DeleteFolders(path + "/" + s);
                    }
                }
                storage.DeleteDirectory(path);
            }
            storage.Dispose();
        }
		/// <summary>
        /// ${WP_mapping_OfflineStorage_mathod_IsImageExistAsync_D}
        /// </summary>
        /// <param name="width">${WP_mapping_OfflineStorage_mathod_param_IsImageExistAsync_width_D}</param>
        /// <param name="height">${WP_mapping_OfflineStorage_mathod_param_IsImageExistAsync_height_D}</param>
        /// <param name="resolution">${WP_mapping_OfflineStorage_mathod_param_IsImageExistAsync_resolution_D}</param>
        /// <param name="level">${WP_mapping_OfflineStorage_mathod_param_IsImageExistAsync_level_D}</param>
        /// <param name="column">${WP_mapping_OfflineStorage_mathod_param_IsImageExistAsync_column_D}</param>
        /// <param name="row">${WP_mapping_OfflineStorage_mathod_param_IsImageExistAsync_row_D}</param>
        /// <returns>${WP_mapping_OfflineStorage_mathod_returns_IsImageExistAsync_returns_D}</returns>
        public async Task<bool> IsImageExistAsync(int width, int height, double resolution, int level, int column, int row)
        {
            bool exists = false;
            string path = BuildPath(width, height, resolution, level, column, row);
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {
                exists = storage.FileExists(path);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                storage.Dispose();
            }
            return exists;
        }
		 /// <summary>
        /// ${WP_mapping_OfflineStorage_mathod_ImageReadAsync_D}
        /// </summary>
        /// <param name="width">${WP_mapping_OfflineStorage_mathod_param_ImageReadAsync_width_D}</param>
        /// <param name="height">${WP_mapping_OfflineStorage_mathod_param_ImageReadAsync_height_D}</param>
        /// <param name="resolution">${WP_mapping_OfflineStorage_mathod_param_ImageReadAsync_resolution_D}</param>
        /// <param name="level">${WP_mapping_OfflineStorage_mathod_param_ImageReadAsync_level_D}</param>
        /// <param name="column">${WP_mapping_OfflineStorage_mathod_param_ImageReadAsync_column_D}</param>
        /// <param name="row">${WP_mapping_OfflineStorage_mathod_param_ImageReadAsync_row_D}</param>
        /// <returns>${WP_mapping_OfflineStorage_mathod_returns_ImageReadAsync_returns_D}</returns>
        public async Task<byte[]> ImageReadAsync(int width, int height, double resolution, int level, int column, int row)
        {
            byte[] buffer = null;
            string path = BuildPath(width, height, resolution, level, column, row);
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            bool exists = false;
            try
            {
                exists = storage.FileExists(path);
                if (exists)
                {
                    using (IsolatedStorageFileStream file = storage.OpenFile(path, FileMode.Open))
                    {
                        buffer = new byte[file.Length];
                        await file.ReadAsync(buffer, 0, buffer.Length);
                        await file.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                storage.Dispose();
            }
            return buffer;
        }
		
        /// <summary>
        /// ${WP_mapping_OfflineStorage_mathod_ImageWriteAsync_D}
        /// </summary>
        /// <param name="width">${WP_mapping_OfflineStorage_mathod_param_ImageWriteAsync_width_D}</param>
        /// <param name="height">${WP_mapping_OfflineStorage_mathod_param_ImageWriteAsync_height_D}</param>
        /// <param name="resolution">${WP_mapping_OfflineStorage_mathod_param_ImageWriteAsync_resolution_D}</param>
        /// <param name="level">${WP_mapping_OfflineStorage_mathod_param_ImageWriteAsync_level_D}</param>
        /// <param name="column">${WP_mapping_OfflineStorage_mathod_param_ImageWriteAsync_column_D}</param>
        /// <param name="row">${WP_mapping_OfflineStorage_mathod_param_ImageWriteAsync_row_D}</param>
        /// <param name="data">${WP_mapping_OfflineStorage_mathod_param_ImageWriteAsync_data_D}</param>
        public async Task ImageWriteAsync(int width, int height, double resolution, int level, int column, int row, byte[] data)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            string path = BuildPath(width, height, resolution, level, column, row);
            try
            {
                IsolatedStorageFileStream file = null;
                if (!storage.DirectoryExists(Path.GetDirectoryName(path)))
                {
                    storage.CreateDirectory(Path.GetDirectoryName(path));
                    file = storage.CreateFile(path);
                }
                else
                {
                    file = storage.OpenFile(path, FileMode.OpenOrCreate);
                }
                await file.WriteAsync(data, 0, data.Length);
                await file.FlushAsync();
                file.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                storage.Dispose();
            }
        }

        public async Task Clear()
        {
            DeleteFolders(RootName);
        }
    }
}
