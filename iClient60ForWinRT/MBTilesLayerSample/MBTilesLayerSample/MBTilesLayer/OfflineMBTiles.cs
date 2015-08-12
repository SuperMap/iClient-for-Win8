using SuperMap.WinRT.Utilities;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using SuperMap.WinRT.Core;
namespace SuperMap.WinRT.Mapping
{
    public class OfflineMBTiles : IOfflineStorage
    {
        private string _mbTilesPath;
        public OfflineMBTiles()
        {
     
        }

       public string MBTilesPath { get;  set; }
       async public Task<bool> IsImageExistAsync(int width, int height, double resolution, int level, int column, int row)
        {
            
            try
            {
                SQLiteAsyncConnection connection = new SQLiteAsyncConnection(MBTilesPath);
                AsyncTableQuery<MBTilesData> table = connection.Table<MBTilesData>();
                MBTilesData data = await table.Where(c => c.Column == column && c.Level == level && c.Row == row).FirstOrDefaultAsync();
                if (data != null && data.Data != null && data.Data.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }


        public async Task<byte[]> ImageReadAsync(int width, int height, double resolution, int level, int column, int row)
        {
            
            MapImage image = new MapImage();
            image.MapImageType = MapImageType.Data;
            try
            {
                SQLiteAsyncConnection connection = new SQLiteAsyncConnection(MBTilesPath);
                AsyncTableQuery<MBTilesData> table = connection.Table<MBTilesData>();
                MBTilesData data = await table.Where(c => c.Column == column && c.Resolution == resolution && c.Row == row).FirstOrDefaultAsync();
                if (data != null)
                {
                    image.Data = data.Data;
                }
                return image.Data;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
            }
        }

        public Task ImageWriteAsync(int width, int height, double resolution, int level, int column, int row, byte[] data)
        {
            return Task.Delay(0);
        }

        public async Task Clear()
        {
            try
            {
              StorageFile file = await StorageFile.GetFileFromPathAsync(MBTilesPath);
              await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch(Exception ex)
            {
                
            }
            finally
            {
            }
            throw new NotImplementedException();
        }
    }
    [SQLite.Table("metadata")]
    internal class MBTilesInfoTable
    {
        [SQLite.Column("name")]
        public string Name { get; set; }

        [SQLite.Column("value")]
        public string Value { get; set; }
    }

    [SQLite.Table("tiles")]
    internal class MBTilesData
    {
        [SQLite.Column("tile_column")]
        public int Column { get; set; }

        [SQLite.Column("tile_row")]
        public int Row { get; set; }

        [SQLite.Column("resolution")]
        public double Resolution { get; set; }

        [SQLite.Column("zoom_level")]
        public int Level { get; set; }

        [SQLite.Column("tile_data")]
        public byte[] Data { get; set; }
    }
    internal class MBTilesParameters
    {
        public string Name { get; set; }

        public MBTilesLayerType LayerType { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public FormatType Format { get; set; }

        public Rectangle2D Bounds { get; set; }

        public Point2D Origin { get; set; }

        public PositiveDirection PositiveDirection { get; set; }

        public int WKID { get; set; }

        public string WKT { get; set; }

        public int TileHeight { get; set; }

        public int TileWidth { get; set; }

        public double[] Resolutions { get; set; }

        public double[] Scales { get; set; }

        public string MapParameter { get; set; }

        public bool Compatible { get; set; }
    }

    internal enum MBTilesLayerType
    {
        Overlay,
        Baselayer
    }

    internal enum FormatType
    {
        PNG,
        JPG,
        JPG_PNG
    }

    internal enum PositiveDirection
    {
        RightDown,
        RightUp,
        LeftDown,
        LeftUp
    }
}
