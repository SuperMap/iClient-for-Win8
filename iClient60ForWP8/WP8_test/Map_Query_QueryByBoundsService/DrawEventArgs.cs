using SuperMap.WindowsPhone.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Map_Query_QueryByBoundsService
{
   public class DrawEventArgs : EventArgs
    {
    
        /// <summary>${ui_action_DrawEventArgs_constructor_None_D}</summary>
        public DrawEventArgs()
        { }

        /// <example>
        /// 	<code lang="CS">
        /// Feature feature = new Feature() { Geometry = e.Geometry as GeoRegion, Style = this.GreenFillStyle };
        /// this.featuresLayer.Features.Add(feature);
        /// </code>
        /// </example>
        /// <summary>${ui_action_DrawEventArgs_attribute_Geometry_D}</summary>
        public Geometry Geometry { get; set; }

    }
}
