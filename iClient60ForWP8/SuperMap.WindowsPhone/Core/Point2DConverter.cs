using System;
using System.ComponentModel;
using System.Globalization;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>${WP_core_Point2DConverter_Title}</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Point2DConverter : TypeConverter
    {
        /// <summary>${WP_core_Point2DConverter_method_CanConvertFrom_D}</summary>
        /// <param name="context">${WP_core_Point2DConverter_method_CanConvertFrom_param_context}</param>
        /// <param name="sourceType">${WP_core_Point2DConverter_method_CanConvertFrom_param_sourceType}</param>
        /// <returns>${WP_core_Point2DConverter_method_CanConvertFrom_return}</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        /// <summary>${WP_core_Point2DConverter_method_ConvertFrom_D}</summary>
        /// <param name="value">${WP_core_Point2DConverter_method_ConvertFrom_param_value}</param>
        /// <param name="context">${WP_core_Point2DConverter_method_ConvertFrom_param_context}</param>
        /// <param name="culture">${WP_core_Point2DConverter_method_ConvertFrom_param_culture}</param>
        /// <returns>${WP_core_Point2DConverter_method_ConvertFrom_return}</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str != null)
            {
                string[] strArray = str.Split(new char[] { ',' });
                if (2 == strArray.Length)
                {
                    return new Point2D(double.Parse(strArray[0], CultureInfo.InvariantCulture), double.Parse(strArray[1], CultureInfo.InvariantCulture));
                }
            }
            throw new ArgumentException(SuperMap.WindowsPhone.Resources.ExceptionStrings.TextParseFailed, "value");
            //TODO:资源 Text Parse Failed
        }
    }
}
