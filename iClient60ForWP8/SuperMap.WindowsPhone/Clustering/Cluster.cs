using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Clustering
{
    //一个聚类点
    //属性:它所聚起来的所有要素Features，Count为个数，位置是所有点的平均数。
    //Cx,Cy为其所在的格网索引
    internal class Cluster : GeoPoint
    {
        public Cluster(double x, double y, int cx, int cy)
            : this(x, y)
        {
            this.Cx = cx;
            this.Cy = cy;
        }

        public Cluster(double x, double y)
            : base(x, y)
        {
            this.Features = new FeatureCollection();
        }

        public int Cx { get; set; }
        public int Cy { get; set; }
        public FeatureCollection Features { get; set; }
        public int Count { get { return Features.Count; } }
    }
}
