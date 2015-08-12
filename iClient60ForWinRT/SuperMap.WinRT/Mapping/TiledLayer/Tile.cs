using System.Globalization;
using SuperMap.WinRT.Utilities;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using SuperMap.WinRT.Core;
using System;
using System.Threading;

namespace SuperMap.WinRT.Mapping
{
    internal class Tile : IEquatable<Tile>
    {
        public Tile(int row, int col, double resolution, MapImage mapImage, bool useTransitions)
        {
            Row = row;
            Column = col;
            Resolution = resolution;

            MapImage = mapImage;
            UseTransitions = useTransitions;
            RetryCount = 0;
        }

        public int Column { get; private set; }
        public int Row { get; private set; }
        public double Resolution { get; private set; }

        public MapImage MapImage { get; set; }
        public bool UseTransitions { get; private set; }
        public bool IsSuccessd { get; set; }
        public bool IsCanceled { get; set; }
        public string LayerID { get; set; }
        public int TileSize { get; set; }
        public int RetryCount { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public int Level
        {
            get;
            set;
        }

        public static string GetTileKey(string layerID, int row, int column, double resolution, int level)
        {
            if (level > -1)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}_{3}", layerID, row, column, level);
            }//只要有Resolutions则按照level返回。
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}_{3}", layerID, row, column, resolution);
            }
        }
        public string TileKey
        {
            get;
            set;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(Tile))
            {
                return false;
            }
            Tile temp = obj as Tile;
            return this.TileKey.Equals(temp.TileKey);
        }

        public bool Equals(Tile other)
        {
            if (other == null)
            {
                return false;
            }
            return this.TileKey.Equals(other.TileKey);
        }

        public override int GetHashCode()
        {
            return this.TileKey.GetHashCode();
        }
    }

    internal class TileComparer : IEqualityComparer<Tile>
    {
        public bool Equals(Tile x, Tile y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.TileKey.Equals(y.TileKey);
        }

        public int GetHashCode(Tile obj)
        {
            return obj.TileKey.GetHashCode();
        }
    }
}
