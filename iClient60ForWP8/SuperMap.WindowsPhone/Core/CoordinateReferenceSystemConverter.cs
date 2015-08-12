using System;
using System.ComponentModel;
using System.Globalization;
using SuperMap.WindowsPhone.Resources;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>${WP_core_CRSTypeConverter_Title}</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CoordinateReferenceSystemConverter : TypeConverter
    {
        /// <summary>${WP_core_CRSTypeConverter_method_CanConvertFrom_D}</summary>
        /// <returns>${WP_core_CRSTypeConverter_method_CanConvertFrom_return}</returns>
        /// <param name="context">${WP_core_CRSTypeConverter_method_CanConvertFrom_param_context}</param>
        /// <param name="sourceType">${WP_core_CRSTypeConverter_method_CanConvertFrom_param_sourceType}</param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        /// <summary>${WP_core_CRSTypeConverter_method_ConvertFrom_D}</summary>
        /// <returns>${WP_core_CRSTypeConverter_method_ConvertFrom_return}</returns>
        /// <param name="context">${WP_core_CRSTypeConverter_method_ConvertFrom_param_context}</param>
        /// <param name="culture">${WP_core_CRSTypeConverter_method_ConvertFrom_param_culture}</param>
        /// <param name="value">${WP_core_CRSTypeConverter_method_ConvertFrom_param_value}</param>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str != null)
            {

                int wkid = Convert.ToInt32(str,CultureInfo.InvariantCulture);
                return new CoordinateReferenceSystem(wkid);
            }
            throw new NotSupportedException(ExceptionStrings.InvalidToCRS);
            //TODO:资源 Cannot convert to CoordinateReferenceSystem
        }

    }
}
