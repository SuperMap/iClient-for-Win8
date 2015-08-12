using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace SuperMap.WinRT.Utilities
{
    internal static class StorageHelper
    {
        public async static Task<bool> FolderExist(this StorageFolder folder, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            try
            {
                StorageFolder childFolder = await folder.GetFolderAsync(path);
                if (childFolder != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            //QueryOptions options = new QueryOptions();
            //options.FolderDepth = FolderDepth.Deep;
            //options.UserSearchFilter = "path:" + path;
            //StorageFolderQueryResult result = folder.CreateFolderQueryWithOptions(options);
            //uint count = await result.GetItemCountAsync();
            //return count > 0;
        }

        public async static Task<bool> FileExist(this StorageFolder folder, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            try
            {
                StorageFile file = await folder.GetFileAsync(path);
                if (file != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async static Task FolderDelete(this StorageFolder storageFolder)
        {
            IReadOnlyList<StorageFile> files = await storageFolder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }

            IReadOnlyList<StorageFolder> folders = await storageFolder.GetFoldersAsync();
            foreach (StorageFolder folder in folders)
            {
                await folder.FolderDelete();
            }
            await storageFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
    }
}
