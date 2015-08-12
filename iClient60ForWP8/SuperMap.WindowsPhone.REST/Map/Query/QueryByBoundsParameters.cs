using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SuperMap.WindowsPhone.Core;
using Newtonsoft.Json;

namespace SuperMap.WindowsPhone.REST
{
    /// <summary>${WP_REST_Query_QueryByBoundsParameters_Title}</summary>
    public class QueryByBoundsParameters : QueryParameterBase
    {
        /// <summary>${WP_REST_Query_QueryByBoundsParameters_constructor_None_D}</summary>
        public QueryByBoundsParameters()
        { }
        /// <summary>${WP_REST_Query_QueryByBoundsParameters_attribute_Bounds_D}</summary>
        [JsonProperty("bounds")]
        [JsonConverter(typeof(Rectangle2DConverter))]
        public Rectangle2D Bounds { get; set; }
    }
}
