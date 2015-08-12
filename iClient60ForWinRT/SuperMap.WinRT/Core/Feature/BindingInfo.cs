using System.Collections.Generic;
using System.ComponentModel;

namespace SuperMap.WinRT.Core
{
    /// <summary>
    /// 	<para>${core_BindingInfo_Title}。</para>
    /// 	<para>${core_BindingInfo_Description}</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class BindingInfo
    {
        private IDictionary<string, object> attributes;

        internal BindingInfo()
        {
        }

        /// <summary>${core_BindingInfo_attribute_Attributes_D}</summary>
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
        /// <summary>${core_BindingInfo_attribute_Style_D}</summary>
        public Style Style { get; set; }
    }
}

