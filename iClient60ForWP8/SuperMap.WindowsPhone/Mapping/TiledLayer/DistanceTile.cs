using System;

namespace SuperMap.WindowsPhone.Mapping
{
    internal class DistanceTile : IComparable<DistanceTile>
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public double Distance { get; set; }

        public int CompareTo(DistanceTile other)
        {
            return this.Distance.CompareTo(other.Distance);
        }
    }
}
