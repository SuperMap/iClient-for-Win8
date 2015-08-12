using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SuperMap.WinRT.Core;

namespace SuperMap.WinRT.REST
{
    /// <summary>${REST_Query_QueryByBoundsParameters_Title}</summary>
    public class QueryByBoundsParameters : QueryParameters
    {
        /// <summary>${REST_Query_QueryByBoundsParameters_constructor_None_D}</summary>
        public QueryByBoundsParameters()
        { }
        /// <summary>${REST_Query_QueryByBoundsParameters_attribute_Bounds_D}</summary>
        public Rectangle2D Bounds { get; set; }
    }
}
