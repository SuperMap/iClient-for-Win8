using System;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>${WP_mapping_LayerFailedEventArgs_Title}</summary>
    /// <remarks>${WP_mapping_LayerFailedEventArgs_Exception}</remarks>
    public class LayerFailedEventArgs : EventArgs
    {
        internal LayerFailedEventArgs(Exception error)
        {
            Error = error;
        }

        /// <summary>${WP_mapping_LayerFailedEventArgs_attribute_Error_D}</summary>
        public Exception Error { get; set; }
    }
}
