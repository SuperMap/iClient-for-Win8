using KitaroDB;
using SuperMap.WindowsPhone.Mapping;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitaroDBSample
{
    class OfflineKitaroDB : IOfflineStorage
    {
        // private string _dbFileName;
        private DB _imagesDB;
        private readonly AsyncLock m_lock = new AsyncLock();

        public OfflineKitaroDB(string dbName)
        {
            DBName = dbName;
        }

        public string DBName
        {
            get;
            private set;
        }
        //判断图片是否存在
        async public Task<bool> IsImageExistAsync(int width, int height, double resolution, int level, int column, int row)
        {
            try
            {
                string key = BuildKey(width, height, resolution, level, column, row);
                string value = await (await GetDB()).GetAsync(key);
                if (!string.IsNullOrEmpty(value))
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //读取图片
        async public Task<byte[]> ImageReadAsync(int width, int height, double resolution, int level, int column, int row)
        {
            try
            {
                string key = BuildKey(width, height, resolution, level, column, row);

                string value = await (await GetDB()).GetAsync(key);
                if (!string.IsNullOrEmpty(value))
                {
                    return Base64ToByte(value);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //保存图片
        async public Task ImageWriteAsync(int width, int height, double resolution, int level, int column, int row, byte[] data)
        {
            if (data == null)
            {
                return;
            }
            string key = BuildKey(width, height, resolution, level, column, row);
            try
            {
                string value = await (await GetDB()).GetAsync(key);
                if (string.IsNullOrEmpty(value))
                {
                    using (await m_lock.LockAsync())
                    {
                        string value1 = await (await GetDB()).GetAsync(key);
                        if (string.IsNullOrEmpty(value1))
                        {
                            await (await GetDB()).InsertAsync(key, ByteToBase64(data));
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        //清除
        async public Task Clear()
        {
            await DB.PurgeAsync(Windows.Storage.ApplicationData.Current.LocalFolder.Path);

        }
        private string BuildKey(int width, int height, double resolution, int level, int column, int row)
        {
            string key = string.Empty;
            if (level > -1)
            {
                key = width + "_" + height + "_" + level + "_" + column + "_" + row;
            }
            else
            {
                key = width + "_" + height + "_" + resolution + "_" + column + "_" + row;
            }
            return key;
        }
        
        private byte[] Base64ToByte(string s)
        {
            byte[] b = Convert.FromBase64String(s);
            return b;
        }

        private string ByteToBase64(byte[] data)
        {
            string s = Convert.ToBase64String(data);
            return s;
        }

        private async Task<DB> GetDB()
        {
            if (_imagesDB == null)
            {
                using (await m_lock.LockAsync())
                {
                    if (_imagesDB == null)
                    {
                        _imagesDB = await DB.CreateAsync(DBName);
                    }
                }
            }
            return _imagesDB;
        }

    }
}

