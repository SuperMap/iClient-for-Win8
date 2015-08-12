
using SuperMap.WinRT.Core;
using Windows.Foundation;
namespace SuperMap.WinRT.Mapping
{
    internal class UpdateParameter
    {
        internal UpdateParameter()
        { }

        
        public bool UseTransitions { get; set; }

        //这个参数是Map的Resolutions传进去，比如TiledDynamicLayer需要用这个参数，其他的图层基本不需要。
            
        public double[] Resolutions { get; set; }

       
        // 需要更改的分辨率。
       
        public double Resolution { get; set; }

        public Rectangle2D ViewBounds { get; set; }

        public Size ViewSize { get; set; }

        public Point2D LayerOrigin { get; set; }

        //TODO:set改为internal?
    }
}
