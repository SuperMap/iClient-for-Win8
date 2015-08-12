using System;

namespace SuperMap.Web.OGC
{
    /// <summary> ${mapping_FailedEventArgs_Tile} </summary>
    public class FailedEventArgs : EventArgs
    {
        /// <summary>
        /// ${mapping_FailedEventArgs_constructor_D}
        /// </summary>
        /// <param name="error">${mapping_FailedEventArgs_attribute_Error_D}</param>
        public FailedEventArgs(RequestException error)
        {
            this.Error = error;
        }
        /// <summary> ${mapping_FailedEventArgs_attribute_Error_D} </summary>
        public RequestException Error { get; private set; }
    }
}
