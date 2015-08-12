using System;
using System.ComponentModel;
using System.Globalization;
using SuperMap.WindowsPhone.Resources;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>${WP_core_GeoPointConverter_Title}</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GeoPointConverter : TypeConverter
    {
        /// <summary>${WP_core_GeoPointConverter_method_CanConvertFrom_D}</summary>
        /// <param name="context">${WP_core_GeoPointConverter_method_CanConvertFrom_param_context}</param>
        /// <param name="sourceType">${WP_core_GeoPointConverter_method_CanConvertFrom_param_sourceType}</param>
        /// <returns>${WP_core_GeoPointConverter_method_CanConvertFrom_return}</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if ((sourceType != typeof(string)) && (sourceType != typeof(GeoPoint)))
            {
                return base.CanConvertFrom(context, sourceType);
            }
            return true;
        }
        /// <summary>${WP_core_GeoPointConverter_method_CanConvertTo_D}</summary>
        /// <param name="context">${WP_core_GeoPointConverter_method_CanConvertTo_param_context}</param>
        /// <param name="destinationType">${WP_core_GeoPointConverter_method_CanConvertTo_param_destinationType}</param>
        /// <returns>${WP_core_GeoPointConverter_method_CanConvertTo_return}</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if ((destinationType != typeof(string)) && (destinationType != typeof(GeoPoint)))
            {
                return base.CanConvertTo(context, destinationType);
            }
            return true;
        }

        /// <summary>${WP_core_GeoPointConverter_method_ConvertFrom_D}</summary>
        /// <param name="value">${WP_core_GeoPointConverter_method_ConvertFrom_param_value}</param>
        /// <param name="context">${WP_core_GeoPointConverter_method_ConvertFrom_param_context}</param>
        /// <param name="culture">${WP_core_GeoPointConverter_method_ConvertFrom_param_culture}</param>
        /// <returns>${WP_core_GeoPointConverter_method_ConvertFrom_return}</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return convertFromString(value as string);
        }

        private static object convertFromString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            string str = text.Trim();
            if (str.Length == 0)
            {
                return null;
            }
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;
            char ch = invariantCulture.TextInfo.ListSeparator[0];
            string[] strArray = str.Split(new char[] { ch });
            double[] numArray = new double[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                numArray[i] = double.Parse(strArray[i], invariantCulture);
            }
            if (numArray.Length != 2)
            {
                //TODO:资源
                //throw new ArgumentException("Text Parse Failed", "X,Y");
                throw new ArgumentException(ExceptionStrings.ParseFailed, "text");
            }
            return new GeoPoint { X = numArray[0], Y = numArray[1] };
        }
        /// <summary>${WP_core_GeoPointConverter_method_ConvertTo_D}</summary>
        /// <param name="context">${WP_core_GeoPointConverter_method_ConvertTo_param_context}</param>
        /// <param name="culture">${WP_core_GeoPointConverter_method_ConvertTo_param_culture}</param>
        /// <param name="value">${WP_core_GeoPointConverter_method_ConvertTo_param_value}</param>
        /// <param name="destinationType">${WP_core_GeoPointConverter_method_ConvertTo_param_destinationType}</param>
        /// <returns>${WP_core_GeoPointConverter_method_ConvertTo_return}</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                //TODO:资源
                //throw new ArgumentNullException("destinationType");
                throw new ArgumentNullException(ExceptionStrings.DestinationTypeIsNull);
            }
            if ((destinationType == typeof(string)) && (value is GeoPoint))
            {
                return convertToString((GeoPoint)value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private static string convertToString(GeoPoint value)
        {
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;
            string listSeparator = invariantCulture.TextInfo.ListSeparator;
            return string.Format(invariantCulture, "{0}{2}{1}", new object[] { value.X, value.Y, listSeparator });
        }
    }
}
