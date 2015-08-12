using System;
using System.ComponentModel;
using System.Globalization;

namespace SuperMap.WindowsPhone.Core
{
    /// <summary>${WP_core_Rectangle2DConverter_Title}</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Rectangle2DConverter : TypeConverter
    {
        /// <summary>${WP_core_Rectangle2DConverter_method_CanConvertFrom_D}</summary>
        /// <param name="context">${WP_core_Rectangle2DConverter_method_CanConvertFrom_param_context}</param>
        /// <param name="sourceType">${WP_core_Rectangle2DConverter_method_CanConvertFrom_param_sourceType}</param>
        /// <returns>${WP_core_Rectangle2DConverter_method_CanConvertFrom_return}</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if ((sourceType != typeof(string)) && (sourceType != typeof(Rectangle2D)))
            {
                return base.CanConvertFrom(context, sourceType);
            }
            return true;
        }
        /// <summary>${WP_core_Rectangle2DConverter_method_ConvertFrom_D}</summary>
        /// <param name="value">${WP_core_Rectangle2DConverter_method_ConvertFrom_param_value}</param>
        /// <param name="context">${WP_core_Rectangle2DConverter_method_ConvertFrom_param_context}</param>
        /// <param name="culture">${WP_core_Rectangle2DConverter_method_ConvertFrom_param_culture}</param>
        /// <returns>${WP_core_GeoPointConverter_method_ConvertFrom_return}</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return converFromString(value as string);
        }

        private static object converFromString(string text)
        {
            if (text == null)
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
            double[] doubleArray = new double[strArray.Length];
            //TODO:
            if (doubleArray.Length != 4)
            {
                return null;
            }

            for (int i = 0; i < strArray.Length; i++)
            {
                doubleArray[i] = double.Parse(strArray[i], invariantCulture);
            }
            //if (doubleArray.Length != 4)
            //{
            //    throw new ArgumentException(ExceptionStrings.TextParseFailed, "Left,Bottom,Right,Top");
            //}

            return new Rectangle2D(doubleArray[0], doubleArray[1], doubleArray[2], doubleArray[3]);
        }

        /// <summary>${WP_core_Rectangle2DConverter_method_CanConvertTo_D}</summary>
        /// <param name="context">${WP_core_Rectangle2DConverter_method_CanConvertTo_param_context}</param>
        /// <param name="destinationType">${WP_core_Rectangle2DConverter_method_CanConvertTo_param_destinationType}</param>
        /// <returns>${WP_core_Rectangle2DConverter_method_CanConvertTo_return}</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType != typeof(string) && (destinationType != typeof(Rectangle2D)))
            {
                return base.CanConvertTo(context, destinationType);
            }
            return true;
        }
        /// <summary>${WP_core_Rectangle2DConverter_method_ConvertTo_D}</summary>
        /// <param name="context">${WP_core_Rectangle2DConverter_method_ConvertTo_param_context}</param>
        /// <param name="culture">${WP_core_Rectangle2DConverter_method_ConvertTo_param_culture}</param>
        /// <param name="value">${WP_core_Rectangle2DConverter_method_ConvertTo_param_value}</param>
        /// <param name="destinationType">${WP_core_Rectangle2DConverter_method_ConvertTo_param_destinationType}</param>
        /// <returns>${WP_core_Rectangle2DConverter_method_ConvertTo_return}</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if ((destinationType == typeof(string)) && (value is Rectangle2D))
            {
                return convertToString((Rectangle2D)value);
            }

            return base.ConvertTo(context, culture, value, destinationType);

        }

        private static string convertToString(Rectangle2D rect)
        {
            if (Rectangle2D.IsNullOrEmpty(rect))
            {
                return null;
            }
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;
            string listSeparator = invariantCulture.TextInfo.ListSeparator;
            return string.Format(invariantCulture, "{0}{4}{1}{4}{2}{4}{3}", new object[] { rect.Left, rect.Bottom, rect.Right, rect.Top, listSeparator });
        }
    }
}
