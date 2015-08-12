
using SuperMap.WindowsPhone.Core;
using System.Windows;
namespace SuperMap.WindowsPhone.Mapping
{
    
    internal class DrawParameter
    {
        internal DrawParameter()
        { }

        /// <summary>${WP_mapping_DrawParameter_attribute_useTransitions_D}</summary>
        public bool UseTransitions { get; set; }

        //这个参数是Map的Resolutions传进去，比如TiledDynamicLayer需要用这个参数，其他的图层基本不需要。
        /// <summary>${WP_mapping_Map_attribute_Resolutions_D}</summary>       
        public double[] Resoluitons { get; set; }

        /// <summary>
        /// 需要更改的分辨率。
        /// </summary>
        public double Resolution { get; set; }

        public Rectangle2D ViewBounds { get; set; }

        public Size ViewSize { get; set; }

        public Point2D LayerOrigin { get; set; }

        //TODO:set改为internal?
    }
}
