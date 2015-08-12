using System.Windows.Input;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_FeatureMouseButtonEventArgs_Title}</para>
    /// 	<para>${WP_core_FeatureMouseButtonEventArgs_Description}</para>
    /// </summary>
    public class FeatureMouseButtonEventArgs : FeatureMouseEventArgs
    {
        private MouseButtonEventArgs source;

        internal FeatureMouseButtonEventArgs(Feature f, MouseButtonEventArgs e)
            : base(f, e)
        {
            this.source = e;
        }

        /// <summary>${WP_core_FeatureMouseButtonEventArgs_attribute_handled_D}</summary>
        public bool Handled
        {
            get
            {
                return this.source.Handled;
            }
            set
            {
                this.source.Handled = value;
            }
        }
    }
}
