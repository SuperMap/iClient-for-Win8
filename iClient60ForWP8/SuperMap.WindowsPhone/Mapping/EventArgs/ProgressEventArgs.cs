using System;

namespace SuperMap.WindowsPhone.Mapping
{
    /// <summary>${WP_mapping_ProgressEventArgs_Title}</summary>
    /// <remarks>${WP_mapping_ProgressEventArgs_Exception}</remarks>
    public sealed class ProgressEventArgs : EventArgs
    {
        internal ProgressEventArgs(int progress)
        {
            this.Progress = progress;
        }

        /// <summary>${WP_mapping_ProgressEventArgs_attribute_Progress_D}</summary>
        public int Progress { get; private set; }
    }
}
