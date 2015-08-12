using System;

namespace SuperMap.WinRT.Mapping
{
    /// <summary>${mapping_ProgressEventArgs_Title}</summary>
    /// <remarks>${mapping_ProgressEventArgs_Exception}</remarks>
    public sealed class ProgressEventArgs : EventArgs
    {
        internal ProgressEventArgs(int progress)
        {
            this.Progress = progress;
        }

        /// <summary>${mapping_ProgressEventArgs_attribute_Progress_D}</summary>
        public int Progress { get; set; }
    }
}
