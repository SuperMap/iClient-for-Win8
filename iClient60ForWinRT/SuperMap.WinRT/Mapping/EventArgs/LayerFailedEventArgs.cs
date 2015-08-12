using System;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>${mapping_LayerFailedEventArgs_Title}</summary>
    /// <remarks>${mapping_LayerFailedEventArgs_Exception}</remarks>
    public class LayerFailedEventArgs : EventArgs
    {
        internal LayerFailedEventArgs(Exception error)
        {
            Error = error;
        }

        /// <summary>${mapping_LayerFailedEventArgs_attribute_Error_D}</summary>
        public Exception Error { get; set; }
    }
}
