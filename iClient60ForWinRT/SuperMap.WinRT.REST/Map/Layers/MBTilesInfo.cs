using SQLite;
using SuperMap.WinRT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMap.WinRT.Mapping
{
    internal class MBTilesHelper
    {
        private static Rectangle2D StringToRectangle2D(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Rectangle2D.Empty;
            }
            string[] array = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length != 4)
            {
                return Rectangle2D.Empty;
            }
            bool success = true;
            double[] valuesArray = new double[4];
            for (int i = 0; i < 4; i++)
            {
                double convertValue = 0;
                if (!double.TryParse(array[i], out convertValue))
                {
                    success = false;
                    break;
                }
                else
                {
                    valuesArray[i] = convertValue;
                }
            }
            if (success)
            {
                Rectangle2D rectangle = new Rectangle2D(valuesArray[0], valuesArray[1], valuesArray[2], valuesArray[3]);
                return rectangle;
            }
            else
            {
                return Rectangle2D.Empty;
            }
        }

        private static Point2D StringToPoint2D(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Point2D.Empty;
            }
            string[] array = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length != 2)
            {
                return Point2D.Empty;
            }
            bool success = true;
            double[] valuesArray = new double[2];
            for (int i = 0; i < 2; i++)
            {
                double convertValue = 0;
                if (!double.TryParse(array[i], out convertValue))
                {
                    success = false;
                    break;
                }
                else
                {
                    valuesArray[i] = convertValue;
                }
            }
            if (success)
            {
                Point2D point = new Point2D(valuesArray[0], valuesArray[1]);
                return point;
            }
            else
            {
                return Point2D.Empty;
            }
        }

        private static double[] StringToDoubleArray(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            string[] array = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            bool success = true;
            double[] valuesArray = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                double convertValue = 0;
                if (!double.TryParse(array[i], out convertValue))
                {
                    success = false;
                    break;
                }
                else
                {
                    valuesArray[i] = convertValue;
                }
            }
            if (success)
            {
                return valuesArray;
            }
            else
            {
                return null;
            }
        }

        public static async Task<MBTilesParameters> GetMBTilesParameter(string path)
        {
            MBTilesParameters parameter = null;
            try
            {
                parameter = new MBTilesParameters();
                SQLiteAsyncConnection connection = new SQLiteAsyncConnection(path);
                AsyncTableQuery<MBTilesInfoTable> table = connection.Table<MBTilesInfoTable>();
                List<MBTilesInfoTable> list = await table.ToListAsync();
                Dictionary<string, MBTilesInfoTable> paramDic = list.ToDictionary<MBTilesInfoTable, string>(c => c.Name);

                parameter.Bounds = StringToRectangle2D(paramDic["bounds"].Value);

                bool compatible = false;
                bool.TryParse(paramDic["compatible"].Value, out compatible);
                parameter.Compatible = compatible;

                parameter.Description = paramDic["description"].Value;

                FormatType format = FormatType.PNG;
                Enum.TryParse<FormatType>(paramDic["compatible"].Value, out format);
                parameter.Format = format;

                MBTilesLayerType layertype = MBTilesLayerType.Overlay;
                Enum.TryParse<MBTilesLayerType>(paramDic["type"].Value, out layertype);
                parameter.LayerType = layertype;

                parameter.MapParameter = paramDic["map_parameter"].Value;

                parameter.Name = paramDic["name"].Value;

                parameter.Origin = StringToPoint2D(paramDic["axis_origin"].Value);

                PositiveDirection positiveDirection = PositiveDirection.RightDown;
                Enum.TryParse<PositiveDirection>(paramDic["axis_positive_direction"].Value, out positiveDirection);
                parameter.PositiveDirection = positiveDirection;

                parameter.Resolutions = StringToDoubleArray(paramDic["resolutions"].Value).OrderByDescending(c=>c).ToArray();

                parameter.Scales = StringToDoubleArray(paramDic["scales"].Value).OrderBy(c=>c).ToArray();

                int height = 0;
                int.TryParse(paramDic["tile_height"].Value, out height);
                parameter.TileHeight = height;

                int width = 0;
                int.TryParse(paramDic["tile_width"].Value, out width);
                parameter.TileHeight = width;

                parameter.Version = paramDic["version"].Value;

                int wkid = 0;
                int.TryParse(paramDic["crs_wkid"].Value, out wkid);
                parameter.WKID = wkid;

                parameter.WKT = paramDic["crs_wkt"].Value;
            }
            catch (Exception ex)
            {

            }
            return parameter;
        }

        public static async Task<MapImage> GetTile(string path, int column, int row, int level)
        {
            MapImage image = new MapImage();
            image.MapImageType = MapImageType.Data;
            try
            {
                SQLiteAsyncConnection connection = new SQLiteAsyncConnection(path);
                AsyncTableQuery<MBTilesData> table = connection.Table<MBTilesData>();
                MBTilesData data = await table.Where(c => c.Column == column && c.Level == level && c.Row == row).FirstOrDefaultAsync();
                if (data != null)
                {
                    image.Data = data.Data;
                }
            }
            catch
            {

            }
            return image;
        }

        public static async Task<MapImage> GetTile(string path, int column, int row, double resolution)
        {
            MapImage image = new MapImage();
            image.MapImageType = MapImageType.Data;
            try
            {
                SQLiteAsyncConnection connection = new SQLiteAsyncConnection(path);
                AsyncTableQuery<MBTilesData> table = connection.Table<MBTilesData>();
                MBTilesData data = await table.Where(c => c.Column == column && c.Resolution == resolution && c.Row == row).FirstOrDefaultAsync();
                if (data != null)
                {
                    image.Data = data.Data;
                }
            }
            catch
            {

            }
            return image;
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
