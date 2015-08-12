using SuperMap.WindowsPhone.Core;

namespace SuperMap.WindowsPhone.Clustering
{
    internal class ClusterFeature : Feature
    {
        public ClusterFeature(double size)
        {
            base.Style = new ClusterStyle { Size = size };
        }
    }
}
