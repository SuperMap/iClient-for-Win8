using System.Collections.Generic;
using System.ComponentModel;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>
    /// 	<para>${WP_core_BindingInfo_Title}。</para>
    /// 	<para>${WP_core_BindingInfo_Description}</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class BindingInfo
    {
        private IDictionary<string, object> attributes;

        internal BindingInfo()
        {
        }

        /// <summary>${WP_core_BindingInfo_attribute_Attributes_D}</summary>
        public IDictionary<string, object> Attributes
        {
            get
            {
                return this.attributes;
            }
            internal set
            {
                this.attributes = value;
            }
        }
        /// <summary>${WP_core_BindingInfo_attribute_Style_D}</summary>
        public Style Style { get; set; }
    }
}

